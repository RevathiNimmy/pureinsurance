Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmDetail
    Inherits System.Windows.Forms.Form

    'Developer Guide No. 69
    Private frmAuthority As frmAuthority
    Private Const ACClass As String = "frmDetail"

    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As gPMConstants.PMEReturnCode
    Private m_bDataChanged As Boolean

    Private m_vProducts(,) As Object
    Private m_vAuthorityLevels(,) As Object
    Private m_vUserAuthorityLevels(,) As Object
    Private m_vActionArray() As Object

    Private m_iLastUserListIndex As Integer

    ' Object parameter members.
    Private m_iTask As Integer
    Private m_lStatus As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_iSourceId As Integer
    Private m_sCallingAppName As String = ""
    Private m_oGeneral As Object
    Private m_sUniqueId As String = ""
    Private m_sScreenHierarchy As String = ""
    Private m_sUserDesc As String = ""

    Private m_lGISDataModelId As Integer
    ' UserId
    Private m_lUserId As Integer

    Public Property UserId() As Integer
        Get
            Return m_lUserId
        End Get
        Set(ByVal Value As Integer)
            m_lUserId = Value
        End Set
    End Property


    Public Property GISDataModelId() As Integer
        Get
            Return m_lGISDataModelId
        End Get
        Set(ByVal Value As Integer)
            m_lGISDataModelId = Value
        End Set
    End Property


    Public Property CallingAppName() As String
        Get
            Return m_sCallingAppName
        End Get
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property


    Public Property SourceId() As Integer
        Get
            Return m_iSourceId
        End Get
        Set(ByVal Value As Integer)
            m_iSourceId = Value
        End Set
    End Property


    Public Property EffectiveDate() As Date
        Get
            Return m_dtEffectiveDate
        End Get
        Set(ByVal Value As Date)
            m_dtEffectiveDate = Value
        End Set
    End Property


    Public Property TransactionType() As String
        Get
            Return m_sTransactionType
        End Get
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property


    Public Property ProcessMode() As Integer
        Get
            Return m_lProcessMode
        End Get
        Set(ByVal Value As Integer)
            m_lProcessMode = Value
        End Set
    End Property


    Public Property Navigate() As Integer
        Get
            Return m_lNavigate
        End Get
        Set(ByVal Value As Integer)
            m_lNavigate = Value
        End Set
    End Property


    Public Property Status() As Integer
        Get
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property


    Public Property ErrorNumber() As Integer
        Get
            Return m_lErrorNumber
        End Get
        Set(ByVal Value As Integer)
            m_lErrorNumber = Value
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

    Public Property UserDesc() As String
        Get
            Return m_sUserDesc
        End Get
        Set(ByVal Value As String)
            m_sUserDesc = Value
        End Set
    End Property



    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Dim sTitle, sMessage As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'Retrieve all system Products.

            m_lReturn = g_oBusiness.GetProducts(r_vProducts:=m_vProducts)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    result = gPMConstants.PMEReturnCode.PMFalse


                    sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACGetProductsFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                    Return result
                End If
            End If

            'Retrieve all system Authority Level Types.

            m_lReturn = g_oBusiness.GetAuthorityLevelTypes(r_vAuthorityLevels:=m_vAuthorityLevels)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Failed to get details.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    result = gPMConstants.PMEReturnCode.PMFalse


                    sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACGetAuthorityLevelsFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                    Return result
                Else

                    sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                    sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACGetAuthorityLevelsNotSetUp, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                    Return gPMConstants.PMEReturnCode.PMNotFound

                End If
            End If

            'Retrieve all system Authorities for user.
            m_lReturn = GetUserDetails()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    result = gPMConstants.PMEReturnCode.PMFalse


                    sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACGetAuthorityLevelsFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                    Return result
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: BusinessToInterface
    '
    ' Description:
    '
    ' History: 02/11/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function BusinessToInterface() As Integer
        Dim result As Integer = 0
        Dim oListitem As ListViewItem
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lvwAuthorityLevels.Items.Clear()

            'Populate ListView with user's authority details.
            If Information.IsArray(m_vUserAuthorityLevels) Then
                For iCount As Integer = 0 To m_vUserAuthorityLevels.GetUpperBound(1)
                    If m_vActionArray(iCount) <> ACActionDelete Then
                        oListitem = lvwAuthorityLevels.Items.Add(CStr(m_vUserAuthorityLevels(ACUserAuthProductDesc, iCount)))
                        ListViewHelper.GetListViewSubItem(oListitem, 1).Text = CStr(m_vUserAuthorityLevels(ACUserAuthLevelTypeDesc, iCount))
                        oListitem.Tag = CStr(iCount)
                    End If
                Next iCount

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BusinessToInterface Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        Dim sTitle, sMessage As String
        Dim iMsgResult As DialogResult

        ' Click event of the Cancel button.

        Try


            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

            ' Check message result.
            If iMsgResult = System.Windows.Forms.DialogResult.Yes Then
                ' Set the interface status.
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel

                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click
        Dim sTitle, sMessage As String

        ' Exit if nothing selected
        If lvwAuthorityLevels.FocusedItem Is Nothing Then

            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACGetSelectFromList, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

            Exit Sub
        End If


        If MessageBox.Show("Are you sure you wish to delete the selected User Authority ?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = System.Windows.Forms.DialogResult.Yes Then
            m_vActionArray(Convert.ToString(lvwAuthorityLevels.FocusedItem.Tag)) = ACActionDelete
            m_lReturn = BusinessToInterface()

        End If

    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click
        Dim sTitle, sMessage As String

        ' Exit if nothing selected
        If lvwAuthorityLevels.FocusedItem Is Nothing Then

            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACGetSelectFromList, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

            Exit Sub
        End If

        m_lReturn = EditAuthority()

    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        ' Fire up the help screen
        'Developer Guide No. 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = CType(PMHelpFunc.ShowHelp(cmdHelp, lContextID:=MainModule.ScreenHelpID), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click

        ' Click event of the Navigate button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMNavigate

            ' Process the next set of actions depending
            ' upon the interface task etc.

            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub

    Private Sub cmdNew_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNew.Click

        m_lReturn = AddAuthority()

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        ' Click event of the OK button.

        Try

            'At present all updates will already have been done by this time, so no
            'further processing will be done.

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            m_lReturn = gPMConstants.PMEReturnCode.PMTrue

            If Information.IsArray(m_vUserAuthorityLevels) Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_sUniqueId = GetUniqueID()
                m_sScreenHierarchy = $"User({UserDesc})"
                m_lReturn = Update_Renamed(m_sUniqueId, m_sScreenHierarchy)
            End If

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="Err_cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub



        End Try

    End Sub


    Private Sub frmDetail_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                Me.Close()
            End If

        End If
    End Sub

    Private Sub Form_Initialize_Renamed()

        ' Initialise the error number value.
        m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

    End Sub


    Private Sub frmDetail_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.

        Try

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_bDataChanged = False

            m_lReturn = GetBusiness()

            ' Check for errors.
            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                cmdNew.Enabled = False
                cmdEdit.Enabled = False
                cmdDelete.Enabled = False
                Exit Sub

            ElseIf (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the User details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If

            m_lReturn = BusinessToInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set get the User details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If

            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If


            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

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

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the status of the Navigate button.
            Select Case (m_lNavigate)
                Case gPMConstants.PMENavigateButtonStatus.PMNavigateEnabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = True

                Case gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = False

                Case Else
                    cmdNavigate.Visible = False
            End Select

            m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}#

            m_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwAuthorityLevels.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.


            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to display all language specific
            ' captions.
            ' The GetResData function will allow you to do this.
            '
            ' Example:-
            '
            '    lblDesc.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACDesc, _
            ''        iDataType:=PMResString)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            '    lblScreen.Caption = iPMFunc.GetResData( _
            'iLangID:=g_iLanguageID%, _
            'lId:=ACLblScreen, _
            'iDataType:=PMResString)

            '    fraDataDictionary.Caption = iPMFunc.GetResData( _
            'iLangID:=g_iLanguageID%, _
            'lId:=ACFraStucture, _
            'iDataType:=PMResString)

            '    fraScreen.Caption = iPMFunc.GetResData( _
            'iLangID:=g_iLanguageID%, _
            'lId:=ACFraCommon, _
            'iDataType:=PMResString)

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: SetFirstLastControls
    '
    ' Description: Sets the first and last data entry controls for
    '              each tab to the control array, for use with the
    '              keyboard navigation.
    '
    ' ***************************************************************** '
    Private Function SetFirstLastControls() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise the control array with the number of
            ' tabs which contain data entry fields on (Remember
            ' that arrays start from zero, therefore you must
            ' subtract one from the number of tabs).
            Dim m_ctlTabFirstLast(1, 0) As Object

            ' Set the first and last data entry controls for
            ' all of the tabs.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to set the first and last data entry
            ' controls for all of the tabs.
            '
            ' Example:-
            '
            '    Set m_ctlTabFirstLast(ACControlStart, 0) = txtName
            '    Set m_ctlTabFirstLast(ACControlEnd, 0) = txtAge
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            '    Set m_ctlTabFirstLast(ACControlStart, 0) = tvwDataDictionary
            '    Set m_ctlTabFirstLast(ACControlEnd, 0) = tvwDataDictionary

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetUserDetails
    '
    ' Description:
    '
    ' History: 20/12/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GetUserDetails() As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Reset array.
            m_vUserAuthorityLevels = Nothing
            m_vActionArray = Nothing

            'Retrieve all authority levels for this user from the database.

            m_lReturn = g_oBusiness.GetAuthorityLevelsForUser(v_lUserId:=m_lUserId, r_vUserAuthorityLevels:=m_vUserAuthorityLevels)


            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Dimension action array.
            If Information.IsArray(m_vUserAuthorityLevels) Then
                ReDim m_vActionArray(m_vUserAuthorityLevels.GetUpperBound(1))
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: AddAuthority
    '
    ' Description:
    '
    ' History: 21/12/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function AddAuthority() As Integer
             Dim nResult As Integer = 0
        Dim nRemainingProductsIndex As Integer
        Dim oRemainingProducts(,) As Object
        Dim bFound As Boolean

        Try
            'Developer Guide No. 69
            frmAuthority = New frmAuthority
            nResult = gPMConstants.PMEReturnCode.PMTrue

            nRemainingProductsIndex = 0

            If Information.IsArray(m_vProducts) Then
                For iProductCount As Integer = 0 To m_vProducts.GetUpperBound(1)
                    bFound = False

                    If Information.IsArray(m_vUserAuthorityLevels) Then
                        'Find whether this product already has an authority set up against it for
                        'this user.
                        For iListCount As Integer = 0 To m_vUserAuthorityLevels.GetUpperBound(1)
                            If m_vActionArray(iListCount) <> gPMConstants.PMEComponentAction.PMDelete Then
                                If Conversion.Val(CStr(m_vUserAuthorityLevels(ACUserAuthProductId, iListCount))) = Conversion.Val(CStr(m_vProducts(ACProductId, iProductCount))) Then
                                    bFound = True
                                    Exit For
                                End If
                            End If
                        Next iListCount
                    End If

                    If Not bFound Then
                        'Copy product into array of products without an authority set up
                        'against them for this user.
                        If Information.IsArray(oRemainingProducts) Then
                            ReDim Preserve oRemainingProducts(2, nRemainingProductsIndex)
                        Else
                            ReDim oRemainingProducts(2, nRemainingProductsIndex)
                        End If

                        oRemainingProducts(0, nRemainingProductsIndex) = m_vProducts(ACProductId, iProductCount)

                        oRemainingProducts(1, nRemainingProductsIndex) = m_vProducts(ACProductDesc, iProductCount)
                        nRemainingProductsIndex += 1

                    End If
                Next iProductCount
            End If

            With frmAuthority

                .Task = gPMConstants.PMEComponentAction.PMAdd
                .AuthorityLevelTypes = m_vAuthorityLevels
                .UnallocatedProducts = oRemainingProducts
                .ShowDialog()

                If .Status = gPMConstants.PMEReturnCode.PMOK Then
                    m_bDataChanged = True

                    If Information.IsArray(m_vUserAuthorityLevels) Then
                        ReDim Preserve m_vUserAuthorityLevels(ACUserAuthMaxIndex, m_vUserAuthorityLevels.GetUpperBound(1) + 1)
                    Else
                        ReDim m_vUserAuthorityLevels(ACUserAuthMaxIndex, 0)
                    End If
                    m_vUserAuthorityLevels(ACUserAuthProductId, m_vUserAuthorityLevels.GetUpperBound(1)) = frmAuthority.ProductId
                    m_vUserAuthorityLevels(ACUserAuthProductDesc, m_vUserAuthorityLevels.GetUpperBound(1)) = frmAuthority.ProductDescription
                    m_vUserAuthorityLevels(ACUserAuthLevelTypeId, m_vUserAuthorityLevels.GetUpperBound(1)) = frmAuthority.AuthorityTypeId
                    m_vUserAuthorityLevels(ACUserAuthLevelTypeDesc, m_vUserAuthorityLevels.GetUpperBound(1)) = frmAuthority.AuthorityTypeDescription

                    'Update action array.
                    If Information.IsArray(m_vActionArray) Then
                        ReDim Preserve m_vActionArray(m_vUserAuthorityLevels.GetUpperBound(1))
                    Else
                        ReDim m_vActionArray(0)
                    End If

                    m_vActionArray(m_vActionArray.GetUpperBound(0)) = ACActionAdd

                    m_lReturn = BusinessToInterface()

                End If

            End With
            frmAuthority.Close()
            frmAuthority = Nothing
            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddAuthority Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddAuthority", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try

    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateUserDetails
    '
    ' Description:
    '
    ' History: 21/12/2000 RWH - Created.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (UpdateUserDetails) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function UpdateUserDetails() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    '
    'Return gPMConstants.PMEReturnCode.PMTrue
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateUserDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateUserDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    '
    ' Name: Update
    '
    ' Description:
    '
    ' History: 21/12/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function Update_Renamed(Optional ByVal vUniqueId As String = "", Optional ByVal vScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = g_oBusiness.Update(v_lUserId:=m_lUserId, v_vUserDetails:=m_vUserAuthorityLevels, v_vActionArray:=m_vActionArray, v_sUniqueId:=vUniqueId, v_sScreenHierarchy:=vScreenHierarchy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: EditAuthority
    '
    ' Description:
    '
    ' History: 21/12/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function EditAuthority() As Integer
        Dim result As Integer = 0
        Dim vRequiredProduct As Object
        Dim iSelectedIndex As Integer

        Try
            'Developer Guide No. 69
            frmAuthority = New frmAuthority
            result = gPMConstants.PMEReturnCode.PMTrue


            iSelectedIndex = Convert.ToString(lvwAuthorityLevels.FocusedItem.Tag)

            ReDim vRequiredProduct(2, 0)

            vRequiredProduct(0, 0) = m_vUserAuthorityLevels(ACUserAuthProductId, iSelectedIndex)

            vRequiredProduct(1, 0) = m_vUserAuthorityLevels(ACUserAuthProductDesc, iSelectedIndex)

            With frmAuthority

                .Task = gPMConstants.PMEComponentAction.PMEdit
                'Developer Guide No. 24
                .AuthorityLevelTypes = m_vAuthorityLevels

                'Developer Guide No. 24
                .UnallocatedProducts = vRequiredProduct
                .AuthorityTypeId = CInt(m_vUserAuthorityLevels(ACUserAuthLevelTypeId, iSelectedIndex))

                .ShowDialog()

                If .Status = gPMConstants.PMEReturnCode.PMOK Then
                    m_bDataChanged = True
                    m_vUserAuthorityLevels(ACUserAuthLevelTypeId, iSelectedIndex) = frmAuthority.AuthorityTypeId
                    m_vUserAuthorityLevels(ACUserAuthLevelTypeDesc, iSelectedIndex) = frmAuthority.AuthorityTypeDescription

                    m_vActionArray(iSelectedIndex) = ACActionUpdate

                    m_lReturn = BusinessToInterface()

                End If

            End With
            frmAuthority.Close()
            frmAuthority = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAuthority Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAuthority", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub frmDetail_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If
    End Sub
End Class
