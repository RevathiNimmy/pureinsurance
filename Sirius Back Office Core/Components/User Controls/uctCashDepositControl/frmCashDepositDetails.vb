Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmCashDepositDetails
    Inherits System.Windows.Forms.Form

    Private Const ACClass As String = "frmCashDepositDetails"

    'Objects
    Private m_oBusiness As Object
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_lStatus As gPMConstants.PMEReturnCode
    '' generic interface details
    'Private m_iLanguageID                       As Integer
    'Private m_iSourceID                         As Integer
    'Private m_iUserId                           As Integer

    'Variables
    Private m_sPartyCode As String = ""
    Private m_sPartyName As String = ""
    Private m_sPreviousUserName As String = ""
    Private m_sCashDepositRef As String = ""
    Private m_lPartyCnt As Integer
    Private m_lAccountId As Integer
    Private m_sCashDepositNumber As Integer
    Private m_lCashDepositID As Integer
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_vAllBranches As Object
    Private m_vAllProducts As Object
    Private m_vSelectedBranches As Object
    Private m_vSelectedProducts As Object
    Private m_bIsClient As Boolean
    Private m_iIsSinglePolicy As Integer
    Private m_bLocked As Boolean
    Private Const ACLockName As String = "CashDeposit"

    Public ReadOnly Property Status() As Integer
        Get
            ' Standard Property.
            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    Public Property PartyCode() As String
        Get
            Return m_sPartyCode
        End Get
        Set(ByVal Value As String)
            m_sPartyCode = Value
        End Set
    End Property

    Public Property PartyName() As String
        Get
            Return m_sPartyName
        End Get
        Set(ByVal Value As String)
            m_sPartyName = Value
        End Set
    End Property

    Public Property PreviousUserName() As String
        Get
            Return m_sPreviousUserName
        End Get
        Set(ByVal Value As String)
            m_sPreviousUserName = Value
        End Set
    End Property

    Public Property CashDepositRef() As String
        Get
            Return m_sCashDepositRef
        End Get
        Set(ByVal Value As String)
            m_sCashDepositRef = Value
        End Set
    End Property


    Public Property Task() As Integer
        Get
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            m_iTask = Value
        End Set
    End Property
    'Public Property Let UserID(ByVal value As Integer)
    '    m_iUserId = value
    'End Property
    '
    'Public Property Get UserID() As Integer
    '    UserID = m_iUserId
    'End Property
    'Public Property Let SourceID(ByVal value As Integer)
    '    m_iSourceID = value
    'End Property
    '
    'Public Property Get SourceID() As Integer
    '    SourceID = m_iSourceID
    'End Property
    'Public Property Let LanguageID(ByVal value As Integer)
    '    m_iLanguageID = value
    'End Property
    '
    'Public Property Get LanguageID() As Integer
    '    LanguageID = m_iLanguageID
    'End Property


    Public Property CashDepositID() As Integer
        Get
            Return m_lCashDepositID
        End Get
        Set(ByVal Value As Integer)
            m_lCashDepositID = Value
        End Set
    End Property

    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)

            m_lPartyCnt = CInt(Value)
        End Set
    End Property


    Public Property IsClient() As Boolean
        Get
            Return m_bIsClient
        End Get
        Set(ByVal Value As Boolean)
            m_bIsClient = Value
        End Set
    End Property

    Public Property IsSinglePolicy() As Integer
        Get
            Return m_iIsSinglePolicy
        End Get
        Set(ByVal Value As Integer)
            m_iIsSinglePolicy = Value
        End Set
    End Property

    Public Property bLocked() As Boolean
        Get
            Return m_bLocked
        End Get
        Set(ByVal Value As Boolean)
            m_bLocked = Value
        End Set
    End Property


    Private Sub chkSinglePolicyLock_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkSinglePolicyLock.CheckStateChanged
        Dim bIsRepeated As Boolean
        If chkSinglePolicyLock.CheckState Then

            m_lReturn = m_oBusiness.CheckCDUsedForMultiPolicies(v_lCashDepositID:=m_lCashDepositID, r_bIsRepeated:=bIsRepeated)
            If bIsRepeated Then
                MessageBox.Show("The Cash Deposit is already used by more than one Policy", "Locking Failed", MessageBoxButtons.OK)
                chkSinglePolicyLock.Enabled = False
                chkSinglePolicyLock.CheckState = False
            End If
        End If
    End Sub

    Private Sub cmdAddTask_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddTask.Click
        m_lReturn = CreateWorkManagerTask()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Unable to create work task manager", "Task Window", MessageBoxButtons.OK, MessageBoxIcon.Error)
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Exit Sub
        End If
    End Sub

    Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click
        If PickListBranches.SelectedItems > 0 And PickListProducts.SelectedItems > 0 Then
            m_lReturn = AddEditCashDeposit()
        Else
            MessageBox.Show("Please select atleast one product or branch", Application.ProductName)
        End If

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        If m_bLocked Then

            If m_oBusiness.UnLockKey(v_sKeyName:=ACLockName, v_lKeyValue:=m_lPartyCnt, v_lUserID:=g_oObjectManager.UserID) <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to unlock KeyName: " & ACLockName & "for" & m_sPartyCode, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        Me.Close()

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        m_lStatus = gPMConstants.PMEReturnCode.PMOK
        Me.Close()
    End Sub


    Private Sub frmCashDepositDetails_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        Const kMethodName As String = "Form_Load"

        m_lReturn = SetInterfaceDefaults()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "SetInterfaceDefaults Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        m_lReturn = CType(GetBusiness(), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetBusiness Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

    End Sub

    Private Sub frmCashDepositDetails_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
        If m_bLocked Then

            If m_oBusiness.UnLockKey(v_sKeyName:=ACLockName, v_lKeyValue:=m_lPartyCnt, v_lUserID:=g_oObjectManager.UserID) <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to unlock KeyName: " & ACLockName & "for" & m_sPartyCode, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            m_bLocked = False
        End If
    End Sub

    Private Sub PickListBranches_Find(ByVal Sender As Object, ByVal e As EventArgs) Handles PickListBranches.Find

        For lCount As Integer = PickListBranches.ForeignKeys.Count To 1 Step -1
            PickListBranches.ForeignKeys.Remove(1)
        Next


        Dim Key As New uctPickList.PickListKey
        Key.KeyName = "WhereClause"
        Key.ValueType = gPMConstants.PMEDataType.PMString
        PickListBranches.ForeignKeys.Add(Key, Key:="WhereClause")


        PickListBranches.ForeignKeys.Item("WhereClause").value = PickListBranches.SearchString

        PickListBranches.PickListType = "AllBranches"
        m_lReturn = PickListBranches.LoadSearched()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Failed to load list of Branches", "Scheme Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        End If
    End Sub

    Private Sub PickListProducts_Find(ByVal Sender As Object, ByVal e As EventArgs) Handles PickListProducts.Find

        For lCount As Integer = PickListProducts.ForeignKeys.Count To 1 Step -1
            PickListProducts.ForeignKeys.Remove(1)
        Next

        Dim Key As New uctPickList.PickListKey
        Key.KeyName = "WhereClause"
        Key.ValueType = gPMConstants.PMEDataType.PMString
        PickListProducts.ForeignKeys.Add(Key, Key:="WhereClause")


        PickListProducts.ForeignKeys.Item("WhereClause").value = PickListProducts.SearchString

        PickListProducts.PickListType = "AllProducts"
        m_lReturn = PickListProducts.LoadSearched()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Failed to load list of Products", "Scheme Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        End If

    End Sub

    Private Function GetBusiness() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetBusiness"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue
            m_lReturn = CType(GetDefaultData(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetDefaultData Failed", gPMConstants.PMELogLevel.PMLogError)
            End If




        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally






        End Try
        Return result
    End Function

    Private Function GetDefaultData() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetDefaultData"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            Dim Key As uctPickList.PickListKey



            ' Get an instance of the business object via the public object manager.
            Dim temp_m_oBusiness As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRCashDeposit.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetInstance of bSIRCashDeposit.Business Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then



                m_lReturn = m_oBusiness.GetNextCashDepositNumber(v_lPartyId:=m_lPartyCnt, r_lCashDepositNumber:=m_sCashDepositNumber)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "m_oBusiness.GetNextCashDepositNumber Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                txtCDNumber.Text = m_sPartyCode & "-CD" & StringsHelper.Format(m_sCashDepositNumber, "000")
                m_sCashDepositRef = txtCDNumber.Text

                '        m_lReturn = m_oBusiness.GetAllBranches(r_vGetAllBranches:=m_vAllBranches)
                '
                '        If m_lReturn <> PMTrue Then
                '                RaiseError kMethodName, "m_oBusiness.GetAllBranches Failed", PMLogError
                '        Else
                '            PickListBranches.LoadFromArray (m_vAllBranches)
                '        End If
                '
                '        m_lReturn = m_oBusiness.GetAllProducts(r_vGetAllProducts:=m_vAllProducts)
                '
                '        If m_lReturn <> PMTrue Then
                '            RaiseError kMethodName, "m_oBusiness.GetAllProducts Failed", PMLogError
                '        Else
                '            PickListProducts.LoadFromArray (m_vAllProducts)
                '        End If
            ElseIf m_iTask = gPMConstants.PMEComponentAction.PMEdit Or m_iTask = gPMConstants.PMEComponentAction.PMView Then

                txtCDNumber.Text = m_sCashDepositRef
                SetPickListForEdit()
                If m_iIsSinglePolicy = 1 Then
                    chkSinglePolicyLock.CheckState = CheckState.Checked
                ElseIf (m_iIsSinglePolicy = 0) Then
                    chkSinglePolicyLock.CheckState = CheckState.Unchecked
                End If

                '         m_lReturn = m_oBusiness.GetAllBranches(v_lCashDepositID:=m_lCashDepositID, r_vGetCashDepositBranches:=m_vSelectedBranches)

                '        If m_lReturn <> PMTrue Then
                '             RaiseError kMethodName, "m_oBusiness.GetAllBranches Failed", PMLogError
                '        End If

                'm_lReturn = m_oBusiness.GetAllProducts(r_vGetCashDepositProducts:=m_vSelectedProducts, v_lCashDepositId:=m_lCashDepositID)

                '        If m_lReturn <> PMTrue Then
                '            RaiseError kMethodName, "m_oBusiness.GetAllProducts Failed", PMLogError
                '        End If

            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally





        End Try
        Return result
    End Function


    Public Function SetInterfaceDefaults() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "SetInterfaceDefaults"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

        Catch
        End Try



        m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "DisplayCaptions Failed", gPMConstants.PMELogLevel.PMLogError)
        End If
        If m_iTask = gPMConstants.PMEComponentAction.PMView Then
            cmdAddTask.Enabled = False
            cmdApply.Enabled = False
            cmdCancel.Enabled = False
            chkSinglePolicyLock.Enabled = False
            txtCDNumber.Enabled = False
            cmdOK.Enabled = True
        Else
            cmdAddTask.Enabled = True
            cmdApply.Enabled = True
            cmdCancel.Enabled = True
            chkSinglePolicyLock.Enabled = True
            cmdOK.Enabled = False
            txtCDNumber.Enabled = False
            PickListBranches.Enabled = True
            PickListProducts.Enabled = True
        End If

        Return result

    End Function

    Private Function DisplayCaptions() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "DisplayCaptions"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            cmdAddTask.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kCashDepositAddTask, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdApply.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kCashDepositApplyButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kCashDepositOkButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kCashDepositCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblCDNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kCashDepositCDNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            chkSinglePolicyLock.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kCashDepositSinglePolicyLock, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function


    Private Function SetPickListForEdit() As Integer

        Dim Key As New uctPickList.PickListKey
        Key.KeyName = "CashDeposit_ID"
        Key.ValueType = gPMConstants.PMEDataType.PMLong
        PickListBranches.ForeignKeys.Add(Key, Key:="CashDeposit_ID")


        PickListBranches.ForeignKeys.Item("CashDeposit_ID").value = m_lCashDepositID
        PickListBranches.PickListType = "Branch"
        m_lReturn = PickListBranches.Load_Renamed()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Failed to load list of Branchs", "Scheme Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        End If

        Key = New uctPickList.PickListKey()
        Key.KeyName = "CashDeposit_ID"
        Key.ValueType = gPMConstants.PMEDataType.PMLong
        PickListProducts.ForeignKeys.Add(Key, Key:="CashDeposit_ID")


        PickListProducts.ForeignKeys.Item("CashDeposit_ID").value = m_lCashDepositID
        PickListProducts.PickListType = "Product"

        m_lReturn = PickListProducts.Load_Renamed()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Failed to load list of Products", "Scheme Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        End If


    End Function

    Private Function AddEditCashDeposit() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddEditCashDeposit"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lCashDepositId, lCount As Integer
            Dim vBranches, vProducts As Object
            Dim sPartyType As String = ""



            vBranches = PickListBranches.GetItemDetails


            vProducts = PickListProducts.GetItemDetails
            If chkSinglePolicyLock.CheckState = CheckState.Checked Then
                m_iIsSinglePolicy = 1
            ElseIf chkSinglePolicyLock.CheckState = CheckState.Unchecked Then
                m_iIsSinglePolicy = 0
            End If

            If m_bIsClient Then
                sPartyType = "C"
            ElseIf Not m_bIsClient Then
                sPartyType = "A"
            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then


                m_lReturn = m_oBusiness.UpdateCashDeposit(v_lCashDepositID:=m_lCashDepositID, v_iIsSinglePolicy:=m_iIsSinglePolicy, v_vBranches:=vBranches, v_vProducts:=vProducts, v_lParty_ID:=m_lPartyCnt, v_sCashDepositRef:=m_sCashDepositRef, v_sPartyName:=m_sPartyName, v_sPreviousUserName:=m_sPreviousUserName, v_sPartyType:=sPartyType)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "UpdateCashDeposit Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            ElseIf m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                If m_bIsClient Then
                    sPartyType = "C"
                ElseIf Not m_bIsClient Then
                    sPartyType = "A"
                End If

                '       m_lReturn = m_oBusiness.CreateCDAccount(r_lAccountId:=m_lAccountId, _
                ''                                 v_sShortCode:=m_sCashDepositRef, _
                ''                                 v_sShortName:=m_sPartyName, _
                ''                                 v_sPartyType:=sPartyType, _
                ''                                 v_lPartyId:=m_lPartyCnt)
                '      If m_lReturn <> PMTrue Then
                '             RaiseError kMethodName, "CreateCDAccount Failed", PMLogError
                '      End If


                m_lReturn = m_oBusiness.AddCashDeposit(r_lCashDepositId:=m_lCashDepositID, v_sCashDeposit_Ref:=m_sCashDepositRef, v_lAccount_ID:=m_lAccountId, v_lParty_ID:=m_lPartyCnt, v_sPartyName:=m_sPartyName, v_sPartyType:=sPartyType, v_iIs_SinglePolicy:=m_iIsSinglePolicy, v_lUser_ID:=g_oObjectManager.UserID, v_vBranches:=vBranches, v_vProducts:=vProducts)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "AddCashDeposit Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If

            cmdApply.Enabled = False
            cmdOK.Enabled = True




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            If m_bLocked Then

                If m_oBusiness.UnLockKey(v_sKeyName:=ACLockName, v_lKeyValue:=m_lPartyCnt, v_lUserID:=g_oObjectManager.UserID) <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to unlock KeyName: " & ACLockName & "for" & m_sPartyCode, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
                m_bLocked = False
            End If





        End Try
        Return result
    End Function

    Private Function CreateWorkManagerTask() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CreateWorkManagerTask"


        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            'developer guide no. 88
            'Dim oTaskInstance As ClassInterface
            Dim oTaskInstance As iPMWrkTaskInstance.Interface_Renamed

            Dim lReturn As Integer
            Dim dtDueDate As Date
            Dim v_lAction As Integer
            If g_oObjectManager Is Nothing Then
                g_oObjectManager = New bObjectManager.ObjectManager
            End If



            v_lAction = 1

            ' Create the Component
            Dim temp_oTaskInstance As Object = Nothing
            lReturn = g_oObjectManager.GetInstance(temp_oTaskInstance, sClassName:="iPMWrkTaskInstance.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oTaskInstance = temp_oTaskInstance
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Process Modes

            lReturn = oTaskInstance.SetProcessModes(vTask:=v_lAction, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired, vEffectiveDate:=DateTime.Now)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_lPartyCnt > 0 Then

                oTaskInstance.Customer = m_sPartyName
            End If



            oTaskInstance.Description = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kCashDepositAddTaskDescription, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)

            oTaskInstance.DueDate = DateTime.Now
            'oTaskInstance.DisableCustomer = PMTrue

            ' Set Task Group Id and Task Id

            oTaskInstance.PMWrkTaskGroupId = 7

            oTaskInstance.PMWrkTaskId = 592
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Start the Form

            lReturn = oTaskInstance.Start
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "oTaskInstance.Start Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' If the User Cancelled then exit as we do not need
            ' to Refresh the Form details.

            If oTaskInstance.Status = gPMConstants.PMEReturnCode.PMCancel Then
                result = gPMConstants.PMEReturnCode.PMCancel
                'r_vPMWrkTaskInstanceCntArray = ""

                oTaskInstance.Dispose()
                oTaskInstance = Nothing
                Return result
            End If

            oTaskInstance.Dispose()
            oTaskInstance = Nothing

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally






        End Try
        Return result
    End Function

    Private Sub PickListProducts_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PickListProducts.Load

    End Sub
End Class