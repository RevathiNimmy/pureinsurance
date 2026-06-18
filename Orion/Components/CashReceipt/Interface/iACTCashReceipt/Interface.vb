Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface

    ' Name of this class
    Private Const ACClass As String = "Interface"

    ' Return value
    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_oObjectManager As bObjectManager.ObjectManager

    Private m_lAccountID As Integer
    Private m_lCashListID As Integer
    Private m_lCashListItemID As Integer
    Private m_lBatchID As Integer
    Private m_lAllocationID As Integer
    Private m_lCashListTypeID As Integer 'DN 10/01/03
    'SJ 14/05/2003 - start
    Private m_oAccount As Object
    Private m_vTransdetailIDs As Object
    'SJ 14/05/2003 - end

    Public Property AccountID() As Integer
        Get
            Return m_lAccountID
        End Get
        Set(ByVal Value As Integer)
            m_lAccountID = Value
        End Set
    End Property

    Public Property CashListTypeID() As Integer
        Get
            Return m_lCashListTypeID
        End Get
        Set(ByVal Value As Integer)
            m_lCashListTypeID = Value
        End Set
    End Property

    Public Property CashListID() As Integer
        Get
            Return m_lCashListID
        End Get
        Set(ByVal Value As Integer)
            m_lCashListID = Value
        End Set
    End Property

    Public Property CashListItemID() As Integer
        Get
            Return m_lCashListItemID
        End Get
        Set(ByVal Value As Integer)
            m_lCashListItemID = Value
        End Set
    End Property

    Public Property BatchID() As Integer
        Get
            Return m_lBatchID
        End Get
        Set(ByVal Value As Integer)
            m_lBatchID = Value
        End Set
    End Property

    Public Property AllocationID() As Integer
        Get
            Return m_lAllocationID
        End Get
        Set(ByVal Value As Integer)
            m_lAllocationID = Value
        End Set
    End Property

    ' ***************************************************************** '
    '
    ' Name: Terminate
    '
    ' Description:
    '
    ' History: 29/02/2000 CTAF - Created.
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
                If m_oObjectManager IsNot Nothing Then
                    m_oObjectManager.Dispose()
                    m_oObjectManager = Nothing
                End If
                If m_oAccount IsNot Nothing Then
                    m_oAccount.Dispose()
                    m_oAccount = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    '
    ' Name: Initialise
    '
    ' Description:
    '
    ' History: 29/02/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise




        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of object manager
            m_oObjectManager = New bObjectManager.ObjectManager()

            m_lReturn = m_oObjectManager.Initialise(sCallingAppName:=ACApp)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oObjectManager.Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'SJ 14/05/2003 - start
            Dim temp_m_oAccount As Object
            m_lReturn = m_oObjectManager.GetInstance(temp_m_oAccount, "bACTAccount.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oAccount = temp_m_oAccount
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Failed to create instance of bACTAccount.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return result
            End If
            'SJ 14/05/2003 - end

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetValue
    '
    ' Description: Sets a key value in the array
    '
    ' History: 01/03/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function SetValue(ByRef r_vKeyArray(,) As Object, ByVal v_sKeyName As String, ByVal v_vKeyValue As String) As Integer

        Dim result As Integer = 0
        Dim iIndex As Integer
        Dim vTemp As Object



        result = gPMConstants.PMEReturnCode.PMTrue


        m_lReturn = CType(GetValue(v_vKeyArray:=r_vKeyArray, v_sKeyName:=v_sKeyName, r_vKeyValue:=CStr(vTemp), v_iIndex:=iIndex), gPMConstants.PMEReturnCode)

        ' Not found, so resize array
        If iIndex = -1 Then
            iIndex = r_vKeyArray.GetUpperBound(1) + 1
            ReDim Preserve r_vKeyArray(1, iIndex)

            r_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iIndex) = v_sKeyName
        End If


        r_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iIndex) = v_vKeyValue

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetValue
    '
    ' Description: Loops through a key array and gets the value
    '
    ' History: 01/03/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function GetValue(ByVal v_vKeyArray(,) As Object, ByVal v_sKeyName As String, ByRef r_vKeyValue As String, Optional ByRef v_iIndex As Integer = 0) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Default to not found
        If Not False Then
            v_iIndex = -1
        End If

        ' Loop the key array
        For iLoop1 As Integer = v_vKeyArray.GetLowerBound(1) To v_vKeyArray.GetUpperBound(1)

            If CStr(v_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop1)) = v_sKeyName Then
                ' A match, so store the value

                r_vKeyValue = CStr(v_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))
                If Not False Then
                    v_iIndex = iLoop1
                End If
                Exit For
            End If
        Next iLoop1

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessMap
    '
    ' Description:
    '
    ' History: 29/02/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessMap() As Integer

        Dim result As Integer = 0
        Dim vKeyArray(,) As Object

        ' Values for keys
        Dim vBatchSetID As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        ReDim vKeyArray(1, 0)

        ' set keys:
        ' get keys: batch set id
        ' ***************************************************************** '
        ' Batch Wrapper
        ' ***************************************************************** '
        m_lReturn = CType(StartStep(v_sComponent:=ACCreateBatch, v_iTask:=gPMConstants.PMEComponentAction.PMEdit, r_vKeyArray:=vKeyArray, v_bSetKeys:=False), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If



        m_lReturn = CType(GetValue(v_vKeyArray:=vKeyArray, v_sKeyName:=PMNavKeyConst.PMKeyNameBatchID, r_vKeyValue:=CStr(vBatchSetID)), gPMConstants.PMEReturnCode)

        ' ***************************************************************** '
        ' Find Transaction
        ' ***************************************************************** '

        m_lReturn = CType(SetValue(r_vKeyArray:=vKeyArray, v_sKeyName:=PMNavKeyConst.ACTKeyNameAccountID, v_vKeyValue:=CStr(m_lAccountID)), gPMConstants.PMEReturnCode)
        'eck180700
        'DN 10/01/03 ISS 1731 Use passed in Cash Lish Type

        m_lReturn = CType(SetValue(r_vKeyArray:=vKeyArray, v_sKeyName:=PMNavKeyConst.ACTKeyNameCashListTypeId, v_vKeyValue:=CStr(m_lCashListTypeID)), gPMConstants.PMEReturnCode)

        'MKW030703 PN5191 START - Pass CashListId to the called object.

        m_lReturn = CType(SetValue(r_vKeyArray:=vKeyArray, v_sKeyName:=PMNavKeyConst.ACTKeyNameCashListId, v_vKeyValue:=CStr(m_lCashListID)), gPMConstants.PMEReturnCode)
        'MKW030703 PN5191 END

        ' set keys: batch set id, account id
        ' get keys:
        m_lReturn = CType(StartStep(v_sComponent:=ACFindTransaction, v_iTask:=gPMConstants.PMEComponentAction.PMView, r_vKeyArray:=vKeyArray, v_bGetKeys:=True), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = CType(GetValue(v_vKeyArray:=vKeyArray, v_sKeyName:=PMNavKeyConst.ACTKeyNameTransDetailIDs, r_vKeyValue:=CStr(m_vTransdetailIDs)), gPMConstants.PMEReturnCode)

        ' ***************************************************************** '
        ' Create Allocation
        ' ***************************************************************** '


        m_lReturn = CType(SetValue(r_vKeyArray:=vKeyArray, v_sKeyName:=PMNavKeyConst.PMKeyNameBatchID, v_vKeyValue:=CStr(vBatchSetID)), gPMConstants.PMEReturnCode)


        m_lReturn = CType(SetValue(r_vKeyArray:=vKeyArray, v_sKeyName:=PMNavKeyConst.ACTKeyNameAllocationId, v_vKeyValue:=CStr(m_lAllocationID)), gPMConstants.PMEReturnCode)


        m_lReturn = CType(SetValue(r_vKeyArray:=vKeyArray, v_sKeyName:=PMNavKeyConst.ACTKeyNameAccountID, v_vKeyValue:=CStr(m_lAccountID)), gPMConstants.PMEReturnCode)


        m_lReturn = CType(SetValue(r_vKeyArray:=vKeyArray, v_sKeyName:=PMNavKeyConst.ACTKeyNameCashListId, v_vKeyValue:=CStr(m_lCashListID)), gPMConstants.PMEReturnCode)


        m_lReturn = CType(SetValue(r_vKeyArray:=vKeyArray, v_sKeyName:=PMNavKeyConst.ACTKeyNameCashListItemId, v_vKeyValue:=CStr(m_lCashListItemID)), gPMConstants.PMEReturnCode)

        ' set keys: batch set id, allocation id, account id, cash list id, cash list item id
        ' get keys: allocation id
        m_lReturn = CType(StartStep(v_sComponent:=ACCreateAllocation, v_iTask:=gPMConstants.PMEComponentAction.PMAdd, r_vKeyArray:=vKeyArray), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = CType(GetValue(v_vKeyArray:=vKeyArray, v_sKeyName:=PMNavKeyConst.ACTKeyNameAllocationId, r_vKeyValue:=CStr(m_lAllocationID)), gPMConstants.PMEReturnCode)

        ' ***************************************************************** '
        ' Allocation
        ' ***************************************************************** '

        m_lReturn = CType(SetValue(r_vKeyArray:=vKeyArray, v_sKeyName:=PMNavKeyConst.ACTKeyNameAllocationId, v_vKeyValue:=CStr(m_lAllocationID)), gPMConstants.PMEReturnCode)


        m_lReturn = CType(SetValue(r_vKeyArray:=vKeyArray, v_sKeyName:=PMNavKeyConst.ACTKeyNameCashListId, v_vKeyValue:=CStr(m_lCashListID)), gPMConstants.PMEReturnCode)


        m_lReturn = CType(SetValue(r_vKeyArray:=vKeyArray, v_sKeyName:=PMNavKeyConst.ACTKeyNameCashListItemId, v_vKeyValue:=CStr(m_lCashListItemID)), gPMConstants.PMEReturnCode)


        m_lReturn = CType(SetValue(r_vKeyArray:=vKeyArray, v_sKeyName:=PMNavKeyConst.ACTKeyAllowCashListButton, v_vKeyValue:="0"), gPMConstants.PMEReturnCode)


        m_lReturn = CType(SetValue(r_vKeyArray:=vKeyArray, v_sKeyName:=PMNavKeyConst.ACTKeyNameAutoAllocate, v_vKeyValue:="1"), gPMConstants.PMEReturnCode)

        ' set keys: allocation id, cash list id, cash list item id, allocation cash list button(0), auto allocate (1)
        ' get keys:
        m_lReturn = CType(StartStep(v_sComponent:=ACAllocation, v_iTask:=gPMConstants.PMEComponentAction.PMAdd, r_vKeyArray:=vKeyArray), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'eck060900

        ' ***************************************************************** '
        ' Post Allocation
        ' ***************************************************************** '

        m_lReturn = CType(SetValue(r_vKeyArray:=vKeyArray, v_sKeyName:=PMNavKeyConst.ACTKeyNameAllocationId, v_vKeyValue:=CStr(m_lAllocationID)), gPMConstants.PMEReturnCode)

        m_lReturn = CType(StartStep(v_sComponent:=ACPostAllocation, v_iTask:=gPMConstants.PMEComponentAction.PMAdd, r_vKeyArray:=vKeyArray), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' ***************************************************************** '
        ' Cash List Post
        ' ***************************************************************** '

        m_lReturn = CType(SetValue(r_vKeyArray:=vKeyArray, v_sKeyName:=PMNavKeyConst.ACTKeyNameCashListId, v_vKeyValue:=CStr(m_lCashListID)), gPMConstants.PMEReturnCode)


        m_lReturn = CType(SetValue(r_vKeyArray:=vKeyArray, v_sKeyName:=PMNavKeyConst.ACTKeyNameCashListItemId, v_vKeyValue:=CStr(m_lCashListItemID)), gPMConstants.PMEReturnCode)

        m_lReturn = CType(StartStep(v_sComponent:=ACPostCashList, v_iTask:=gPMConstants.PMEComponentAction.PMAdd, r_vKeyArray:=vKeyArray), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: Start
    '
    ' Description:
    '
    ' History: 29/02/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(ProcessMap(), gPMConstants.PMEReturnCode)

            'SJ 14/05/2003 - start
            'Delete all the allocation locks
            If Information.IsArray(m_vTransdetailIDs) Then

                m_lReturn = m_oAccount.DeleteAllocationLocks(v_vOSTransactions:=m_vTransdetailIDs)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="bACTAccount.Form.DeleteAllocationLocks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                    Return result
                End If
            End If
            'SJ 14/05/2003 - end

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: StartStep
    '
    ' Description:
    '
    ' History: 29/02/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function StartStep(ByVal v_sComponent As String, ByVal v_iTask As Integer, ByRef r_vKeyArray(,) As Object, Optional ByVal v_bSetKeys As Boolean = True, Optional ByVal v_bGetKeys As Boolean = True) As Integer

        Dim result As Integer = 0
        Dim oObject As Object
        Dim bNavV3, bNavV2 As Boolean


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Cursor to wobbly
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        ' Create an instance of the object
        Select Case v_sComponent.Substring(0, 1).ToLower()
            Case "b"
                m_lReturn = m_oObjectManager.GetInstance(oObject:=oObject, sClassName:=v_sComponent, vInstanceManager:=gPMConstants.PMGetViaClientManager)
            Case "i"
                m_lReturn = m_oObjectManager.GetInstance(oObject:=oObject, sClassName:=v_sComponent, vInstanceManager:=gPMConstants.PMGetLocalInterface)
        End Select
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of " & v_sComponent, vApp:=ACApp, vClass:=ACClass, vMethod:="StartStep", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' We need to invoke NavigatorV3 functions?
        bNavV3 = (v_sComponent.Substring(v_sComponent.Length - ("NavigatorV3").Length) = "NavigatorV3")

        ' We need to invoke NavigatorV2 functions?
        bNavV2 = (v_sComponent.Substring(v_sComponent.Length - ("AutoForCL").Length) = "AutoForCL") Or (v_sComponent.Substring(v_sComponent.Length - ("NavigatorV2").Length) = "NavigatorV2")

        ' Set it's task
        If bNavV3 Then

            m_lReturn = oObject.NavigatorV3_SetProcessModes(vTask:=v_iTask, vEffectiveDate:=DateTime.Now)
        ElseIf (bNavV2) Then

            m_lReturn = oObject.NavigatorV2_SetProcessModes(vTask:=v_iTask, vEffectiveDate:=DateTime.Now)
        Else

            m_lReturn = oObject.SetProcessModes(vTask:=v_iTask, vEffectiveDate:=DateTime.Now)
        End If

        If v_bSetKeys Then
            ' Set it's keys
            If bNavV3 Then

                m_lReturn = oObject.NavigatorV3_SetKeys(vKeyArray:=r_vKeyArray)
            ElseIf (bNavV2) Then

                m_lReturn = oObject.NavigatorV2_SetKeys(vKeyArray:=r_vKeyArray)
            Else

                m_lReturn = oObject.SetKeys(vKeyArray:=r_vKeyArray)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set keys for " & v_sComponent, vApp:=ACApp, vClass:=ACClass, vMethod:="StartStep", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                oObject = Nothing
                Return result
            End If
        End If

        ' Cursor to normal
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        ' Start
        If bNavV3 Then

            m_lReturn = oObject.NavigatorV3_Start()
        ElseIf (bNavV2) Then

            m_lReturn = oObject.NavigatorV2_Start()
        Else

            m_lReturn = oObject.Start()
        End If
        'eck030701
        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
            MessageBox.Show("Warning -Allocation data missing", Application.ProductName)
            result = gPMConstants.PMEReturnCode.PMNotFound
            GoTo StartStep_CleanUp
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start " & v_sComponent, vApp:=ACApp, vClass:=ACClass, vMethod:="StartStep", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            GoTo StartStep_CleanUp
        End If

        If bNavV3 Then

            If oObject.NavigatorV3_Status <> gPMConstants.PMEReturnCode.PMOK Then

                If oObject.NavigatorV3_Status = gPMConstants.PMEReturnCode.PMCancel Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    GoTo StartStep_CleanUp
                End If
            End If
        ElseIf (bNavV2) Then

            If oObject.NavigatorV2_Status <> gPMConstants.PMEReturnCode.PMOK Then

                If oObject.NavigatorV2_Status = gPMConstants.PMEReturnCode.PMCancel Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    GoTo StartStep_CleanUp
                End If
            End If
        Else

            If oObject.Status <> gPMConstants.PMEReturnCode.PMOK Then

                If oObject.Status = gPMConstants.PMEReturnCode.PMCancel Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    GoTo StartStep_CleanUp
                End If
            End If
        End If

        ' Get it's keys
        If v_bGetKeys Then
            If bNavV3 Then

                m_lReturn = oObject.NavigatorV3_GetKeys(vKeyArray:=r_vKeyArray)
            ElseIf (bNavV2) Then

                m_lReturn = oObject.NavigatorV2_GetKeys(vKeyArray:=r_vKeyArray)
            Else

                m_lReturn = oObject.GetKeys(vKeyArray:=r_vKeyArray)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get keys for " & v_sComponent, vApp:=ACApp, vClass:=ACClass, vMethod:="StartStep", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                GoTo StartStep_CleanUp
            End If
        End If

        GoTo StartStep_CleanUp

StartStep_CleanUp:

        ' Terminate and clean-up

        oObject.Dispose()


        oObject = Nothing

        ' Cursor to normal
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Return result

    End Function
    Private Shared _DefaultInstance As Interface_Renamed = Nothing
    Public Shared ReadOnly Property DefaultInstance() As Interface_Renamed
        Get
            If _DefaultInstance Is Nothing Then
                _DefaultInstance = New Interface_Renamed
            End If
            Return _DefaultInstance
        End Get
    End Property
End Class
