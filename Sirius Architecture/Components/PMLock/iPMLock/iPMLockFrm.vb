Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 02/07/1997
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    'Developer Guide No: 7
    Private Const vbFormCode As Integer = 0


    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}

    ' Stores the PMLock query resultsets
    Private m_vLockArray(,) As Object

    ' Stores the currently highlighted list view items
    Private m_sLockListItemName As String = ""
    Private m_lLockListItemValue As Integer

    Private m_sUserListItem As String = ""

    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMLock.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer


    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)


    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Standard Property.

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property


    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)

    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub Status(ByVal Value As Integer)
    '
    ' Standard Property.
    '
    ' Set the interface exit status.
    'm_lStatus = Value
    '
    'End Sub
    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    Public Property Task() As Integer
        Get

            ' Return the objects task.
            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the objects task.
            m_iTask = Value

        End Set
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the navigate flag.
            m_lNavigate = Value

        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the process mode.
            m_lProcessMode = Value

        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the type of business.
            m_sTransactionType = Value

        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            ' Standard Property.

            ' Set the effective date.
            m_dtEffectiveDate = Value

        End Set
    End Property
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.

            ' {* USER DEFINED CODE (Begin) *}


            m_lReturn = m_oBusiness.GetDetails()

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return result
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BusinessToInterface
    '
    ' Description: Updates all interface details from the business
    '              object.
    '
    ' ***************************************************************** '
    Public Function BusinessToInterface() As Integer

        Dim result As Integer = 0

        Dim iUserItems As Integer
        Dim sUsername As String = ""
        Dim bIsOnList As Boolean
        Dim iLocksHeld As Integer

        Dim oListItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Assign the details from the business object
            ' to the data storage.
            m_lReturn = BusinessToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Assign the details to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            ' Clear list views
            lvwLocks.Items.Clear()
            lvwUsers.Items.Clear()

            ' Resultset only present if m_vLockArray is an array
            If Information.IsArray(m_vLockArray) Then
                ' Populate main list view from variant array
                For iLockItems As Integer = m_vLockArray.GetLowerBound(1) To m_vLockArray.GetUpperBound(1)
                    oListItem = lvwLocks.Items.Add(CStr(m_vLockArray(gPMConstants.PMEPMLockColumnPosition.PMLockFormAllName, iLockItems)).Trim())
                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vLockArray(gPMConstants.PMEPMLockColumnPosition.PMLockFormAllValue, iLockItems)).Trim()
                    sUsername = CStr(m_vLockArray(gPMConstants.PMEPMLockColumnPosition.PMLockFormAllUser, iLockItems)).Trim()
                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = sUsername
                    'Devlopment Work For Insurer Payment Locking
                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vLockArray(gPMConstants.PMEPMLockColumnPosition.PMLockFormAllValue2, iLockItems)).Trim()

                    ListViewHelper.GetListViewSubItem(oListItem, 3).Text = sUsername

                    ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vLockArray(gPMConstants.PMEPMLockColumnPosition.PMLockFormAllTime, iLockItems)).Trim()

                    'oListItem.SubItems(3) = _
                    'Trim$(m_vLockArray(PMLockFormAllTime, iLockItems%))

                    ' Set ListItem tag to UserID
                    oListItem.Tag = CStr(m_vLockArray(gPMConstants.PMEPMLockColumnPosition.PMLockFormAllUserID, iLockItems))

                    ' Highlight if previously selected
                    If oListItem.Text = m_sLockListItemName Then
                        If StringsHelper.ToDoubleSafe(ListViewHelper.GetListViewSubItem(oListItem, gPMConstants.PMEPMLockColumnPosition.PMLockFormAllValue).Text) = m_lLockListItemValue Then

                            'Developer Guide No: 283
                            lvwLocks.FocusedItem = oListItem

                        End If
                    End If

                    ' Add to users list box
                    For iUserItems = 1 To lvwUsers.Items.Count
                        bIsOnList = False
                        If lvwUsers.Items.Item(iUserItems - 1).Text = sUsername Then
                            bIsOnList = True

                            ' Increment no of locks held
                            iLocksHeld = Conversion.Val(ListViewHelper.GetListViewSubItem(lvwUsers.Items.Item(iUserItems - 1), 1).Text)
                            ListViewHelper.GetListViewSubItem(lvwUsers.Items.Item(iUserItems - 1), 1).Text = CStr(iLocksHeld + 1)
                            Exit For
                        End If
                    Next
                    If Not bIsOnList Then
                        oListItem = lvwUsers.Items.Add(sUsername)
                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = "1"

                        ' Highlight if previously selected
                        If oListItem.Text = m_sUserListItem Then

                            'Developer Guide No: 283
                            lvwLocks.FocusedItem = oListItem
                        End If

                        ' Set ListItem tag to UserID
                        oListItem.Tag = CStr(m_vLockArray(gPMConstants.PMEPMLockColumnPosition.PMLockFormAllUserID, iUserItems - 1))
                    End If
                Next

                ' Enable clear button
                cmdClear.Enabled = True
            Else
                ' Disable clear button
                cmdClear.Enabled = False

                'clear selected item variables
                m_sLockListItemName = ""
                m_lLockListItemValue = 0
                m_sUserListItem = ""

            End If

            ' Set delete button status
            cmdDeleteLock.Enabled = False
            If Not (lvwLocks.FocusedItem Is Nothing) Then
                oListItem = lvwLocks.FocusedItem
                If oListItem.Text = m_sLockListItemName Then
                    If StringsHelper.ToDoubleSafe(ListViewHelper.GetListViewSubItem(oListItem, gPMConstants.PMEPMLockColumnPosition.PMLockFormAllValue).Text) = m_lLockListItemValue Then
                        cmdDeleteLock.Enabled = True
                    End If
                Else
                    ' Selected item has disappeared
                    m_sLockListItemName = ""
                    m_lLockListItemValue = 0
                End If
            End If
            cmdDeleteUser.Enabled = False
            If Not (lvwUsers.FocusedItem Is Nothing) Then
                oListItem = lvwUsers.FocusedItem
                If oListItem.Text = m_sUserListItem Then
                    cmdDeleteUser.Enabled = True
                Else
                    ' Selected item has disappeared
                    m_sUserListItem = ""
                End If
            End If

            ' Force redraw
            Me.Refresh()

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: InterfaceToBusiness
    '
    ' Description: Updates all business members from the interface
    '              details.
    '
    ' ***************************************************************** '
    Public Function InterfaceToBusiness() As Integer

        Dim result As Integer = 0
        Dim lBusinessDataID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the business object.

            ' Assign the details from the interface to the data storage.
            m_lReturn = InterfaceToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Set the business data ID to one because we are only
            ' dealing with one record item only.
            lBusinessDataID = 1

            ' Check the task.
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Inform the business object with a new data item.

                    ' {* USER DEFINED CODE (Begin) *}
                    'm_lReturn& = m_oBusiness.EditAdd(lRow:=lBusinessDataID&, )
                    ' {* USER DEFINED CODE (End) *}

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Inform the business object with an updated data item.

                    ' {* USER DEFINED CODE (Begin) *}
                    'm_lReturn& = m_oBusiness.EditUpdate(lRow:=lBusinessDataID&, )
                    ' {* USER DEFINED CODE (End) *}
            End Select

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: BusinessToData
    '
    ' Description: Updates the data storage from the business object.
    '
    ' ***************************************************************** '
    Private Function BusinessToData() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details to the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            ' Call Business method to retrieve data

            m_lReturn = m_oBusiness.GetAllLocks(m_vLockArray)

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors.
            If m_lReturn = gPMConstants.PMEReturnCode.PMError Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: InterfaceToData
    '
    ' Description: Updates the data storage from the interface details.
    '
    ' ***************************************************************** '
    Private Function InterfaceToData() As Integer

        Dim result As Integer = 0
        Try


            ' Update the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign all of the details from the
            ' interface to the data storage.
            '
            ' Example:-
            '
            '    m_DName$ = trim$(txtName.Text)
            '    m_DDate = CDate(txtDate.Text)
            '    m_iDCodeID% = cmbCode.ItemData(cmbCode.ListIndex)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'm_lReturn& = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            ' Set Auto refresh rate to 5 seconds
            chkAutoRefresh.CheckState = CheckState.Checked

            'Developer Guide No:264
            sldRefreshRate.Value = 5
            txtSeconds.Text = CStr(5)

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

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


            'Developer Guide No: 243
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



            'Developer Guide No: 243
            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            'Developer Guide No: 243
            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            'Developer Guide No: 243
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

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteAllLocks (Private Method)
    '
    ' Description: Clear contents of PMLock table.
    '
    ' ***************************************************************** '

    Private Function DeleteAllLocks() As Integer

        Dim result As Integer = 0
        Try

            ' Call function on business object

            m_lReturn = m_oBusiness.ReleaseAllLocks()

            ' Check for errors.
            If m_lReturn = gPMConstants.PMEReturnCode.PMError Then
                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to delete all locks", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAllLocks")

                Return result
            End If

            ' Clear form variables
            'm_sLockListItemName$ = ""
            'm_lLockListItemValue& = 0
            'm_sUserListItem$ = ""

            ' Refresh display
            m_lReturn = BusinessToInterface()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to clear all the locks", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAllLocks", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteUser (Private Method)
    '
    ' Description: Clear selected item from PMLock for selected user.
    '
    ' ***************************************************************** '

    Private Function DeleteUser() As Integer

        Dim result As Integer = 0
        Try

            ' Set parameters

            ' Call function on business object
            ' Send user ID

            m_lReturn = m_oBusiness.UnLockAllForUser(Convert.ToString(lvwUsers.FocusedItem.Tag))

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to delete lock", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteUser")

                Return result
            End If

            ' Clear form variables
            'm_sUserListItem$ = ""

            ' Refresh display
            m_lReturn = BusinessToInterface()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete the lock", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteUser", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteLock (Private Method)
    '
    ' Description: Clear selected item from PMLock table.
    '
    ' ***************************************************************** '

    Private Function DeleteLock() As Integer

        Dim nResult As Integer = 0
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            Dim bUnlock As Boolean = True
            If Information.IsArray(m_vLockArray) Then
                For nLockItems As Integer = m_vLockArray.GetLowerBound(1) To m_vLockArray.GetUpperBound(1)
                    If lvwLocks.FocusedItem.Text = m_vLockArray(0, nLockItems) AndAlso
                    lvwLocks.FocusedItem.SubItems(1).Text = m_vLockArray(1, nLockItems) _
                    AndAlso ToSafeInteger(m_vLockArray(6, nLockItems)) = 1 Then
                        MessageBox.Show("This lock is generated by system and can not be deleted/unlocked manually.",
                                        "Delete Lock",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information,
                                        MessageBoxDefaultButton.Button1, 0)
                        bUnlock = False 'Dont Unlock
                        Exit For
                    End If
                Next
            End If

            If bUnlock Then
                m_lReturn = m_oBusiness.UnLockKey(lvwLocks.FocusedItem.Text,
                                                  lvwLocks.FocusedItem.SubItems(1).Text)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to delete lock", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteLock")
                    Return nResult
                End If
            End If


            ' Refresh display
            m_lReturn = BusinessToInterface()

            Return nResult

        Catch ex As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete the lock", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteLock", vErrNo:=Information.Err().Number, vErrDesc:=ex.Message, excep:=ex)

            Return nResult

        End Try
    End Function

    ' PRIVATE Methods (End)

    ' PRIVATE Events (Begin)

    Private Sub chkAutoRefresh_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkAutoRefresh.CheckStateChanged

        ' Toggle the Auto refresh function

        Try

            If chkAutoRefresh.CheckState = CheckState.Checked Then
                ' Disable Refresh button
                cmdRefresh.Enabled = False

                ' Enable Slider and Timer controls
                sldRefreshRate.Enabled = True
                tmrPMLock.Enabled = True
            Else
                ' Enable Refresh button
                cmdRefresh.Enabled = True

                ' Enable Slider and Timer controls
                sldRefreshRate.Enabled = False
                tmrPMLock.Enabled = False
            End If

        Catch excep As System.Exception




            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Click event", vApp:=ACApp, vClass:=ACClass, vMethod:="chkAutoRefresh_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdRefresh_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRefresh.Click

        ' manually refresh data

        Try

            m_lReturn = BusinessToInterface()

        Catch excep As System.Exception




            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Click event", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdRefresh_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            iPMFunc.ShowFormInTaskBar_Attach()

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bPMLock.Form", vInstanceManager:="ClientManager")
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.


                'Developer Guide No: 243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                'Developer Guide No: 243
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMLock.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Set the cancelled property to true. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            'Cancelled = True

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

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

            iPMFunc.ShowFormInTaskBar_Detach()

            ' Set the process modes for the busines object.

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)




            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    'Developer Guide No: 7
                    'Cancel = 1
                    eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()
            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Terminate the business object

            m_oBusiness.Dispose()
            ' Destroy the instance of the business object
            ' from memory.
            m_oBusiness = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Dim iCtrlDown As Integer

        Const ACCtrlMask As Integer = 2

        Try

            ' Set the control key value.
            iCtrlDown = (Shift And ACCtrlMask) > 0

            With tabMainTab
                ' Check the key pressed.
                Select Case KeyCode
                    Case Keys.PageUp
                        ' Page Up key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the first tab.
                            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                        Else
                            ' Check we are not on the
                            ' first tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) > 0 Then
                                ' Display the previous tab.
                                SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) - 1)
                            End If
                        End If

                    Case Keys.PageDown
                        ' Page Down key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the last tab.
                            SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetTabCount(tabMainTab) - 1)
                        Else
                            ' Check we are not on the
                            ' last tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) < (SSTabHelper.GetTabCount(tabMainTab) - 1) Then
                                ' Display the next tab.
                                SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) + 1)
                            End If
                        End If

                    Case Keys.Home
                        ' Home key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            '    If (.Tab <= UBound(m_ctlTabFirstLast, 2)) Then
                            '         m_ctlTabFirstLast(ACControlStart, .Tab).SetFocus
                            '    End If
                        End If

                    Case Keys.End
                        ' End key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            '    If (.Tab <= UBound(m_ctlTabFirstLast, 2)) Then
                            '         m_ctlTabFirstLast(ACControlEnd, .Tab).SetFocus
                            '    End If
                        End If
                End Select
            End With
            'Developer Guide No 293
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D2 Then
                tabMainTab.SelectedIndex = 1
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D3 Then
                tabMainTab.SelectedIndex = 2
            End If
        Catch



            ' Error Section.

            Exit Sub
        End Try


    End Sub

    Private Sub lvwLocks_ItemClick(ByVal Item As ListViewItem)

        ' Activate Delete Lock button when item is selected

        Try

            ' Enable Delete Locks button
            cmdDeleteLock.Enabled = True

            ' Store selected item
            m_sLockListItemName = Item.Text
            m_lLockListItemValue = CInt(ListViewHelper.GetListViewSubItem(Item, gPMConstants.PMEPMLockColumnPosition.PMLockFormAllValue).Text)

            ' Disable timer
            'tmrPMLock.Enabled = False

        Catch excep As System.Exception




            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the ItemClick event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwLocks_ItemClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwUsers_ItemClick(ByVal Item As ListViewItem)

        ' Activate Delete User button when item is selected

        Try

            ' Enable Delete button
            cmdDeleteUser.Enabled = True

            ' Store selected item
            m_sUserListItem = Item.Text

            ' Disable timer
            'tmrPMLock.Enabled = False

        Catch excep As System.Exception




            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the ItemClick event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwUsers_ItemClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub sldRefreshRate_Scroll(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles sldRefreshRate.Scroll

        Try

            ' Set text box to reflect slider value
            txtSeconds.Text = CStr(sldRefreshRate.Value)

            ' Apply interval to timer control
            If sldRefreshRate.Value * 1000 = 0 Then
                tmrPMLock.Enabled = False
            Else
                tmrPMLock.Interval = sldRefreshRate.Value * 1000
                tmrPMLock.Enabled = True
            End If

        Catch excep As System.Exception




            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Scroll event", vApp:=ACApp, vClass:=ACClass, vMethod:="sldRefreshRate_Scroll", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        Try

            With tabMainTab
                ' Set the default button.
                'If (.Tab < cmdNext.Count) Then
                '    cmdNext(.Tab).Default = True
                'Else
                VB6.SetDefault(cmdOK, True)
                'End If

                ' Now I know this is crap, this goes against
                ' all my principles, but for some reason when
                ' using the mouse to select a tab the setfocus
                ' code below doesn't work. The cursor sticks,
                ' and you can't tab off. Therefore I've used
                ' this to get around the problem.
                Application.DoEvents()

            End With

        Catch



            ' Error Section.


            tabMainTabPreviousTab = tabMainTab.SelectedIndex
        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdClear_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdClear.Click

        ' Clear all records from the PMLock table

        Try
            MessageBox.Show("All locks except those generated by system will be released.", _
                "Clear Lock", _
                MessageBoxButtons.OK, _
                MessageBoxIcon.Information, _
                MessageBoxDefaultButton.Button1, 0)
            ' Call function to delete all records from PMLock table
            m_lReturn = DeleteAllLocks()

        Catch excep As System.Exception




            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Clear command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdClear_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdDeleteLock_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteLock.Click


        Try


            m_lReturn = DeleteLock()
            'Show Msg Box

        Catch excep As System.Exception




            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Delete Lock command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteLock_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try

    End Sub

    Private Sub cmdDeleteUser_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteUser.Click

        ' Clear selected record from the PMLock table

        Try

            ' Call function to delete all records for selected user
            m_lReturn = DeleteUser()

        Catch excep As System.Exception




            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Delete command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteUser_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub tmrPMLock_Tick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tmrPMLock.Tick

        ' Refresh list views

        Try

            m_lReturn = BusinessToInterface()

        Catch excep As System.Exception




            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Timer method", vApp:=ACApp, vClass:=ACClass, vMethod:="tmrPMLock_Timer", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' PRIVATE Events (End)

    Private Sub lvwLocks_ColumnClick(ByVal sender As Object, ByVal eventArgs As System.Windows.Forms.ColumnClickEventArgs) Handles lvwLocks.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwLocks.Columns(eventArgs.Column)

        If lvwLocks.Sorting = SortOrder.Ascending Then
            lvwLocks.Sorting = SortOrder.Descending
            ListViewHelper.SetSortOrderProperty(lvwLocks, SortOrder.Descending)
            ListViewHelper.SetSortedProperty(lvwLocks, False)

        Else
            ListViewHelper.SetSortOrderProperty(lvwLocks, SortOrder.Ascending)
            lvwLocks.Sorting = SortOrder.Ascending
            ListViewHelper.SetSortedProperty(lvwLocks, True)
        End If
        ListViewHelper.SetSortKeyProperty(lvwLocks, ColumnHeader.Index + 1 - 1)

    End Sub




    Private Sub lvwLocks_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwLocks.GotFocus
        tabMainTab.TabStop = True
    End Sub


    Private Sub cmdHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHelp.Click
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID)
    End Sub

    Private Sub lvwUsers_ColumnClick(ByVal sender As Object, ByVal eventArgs As System.Windows.Forms.ColumnClickEventArgs) Handles lvwUsers.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwUsers.Columns(EventArgs.Column)

        If lvwUsers.Sorting = SortOrder.Ascending Then
            lvwUsers.Sorting = SortOrder.Descending
            ListViewHelper.SetSortOrderProperty(lvwUsers, SortOrder.Descending)
            ListViewHelper.SetSortedProperty(lvwUsers, False)

        Else
            ListViewHelper.SetSortOrderProperty(lvwUsers, SortOrder.Ascending)
            lvwUsers.Sorting = SortOrder.Ascending
            ListViewHelper.SetSortedProperty(lvwUsers, True)
        End If
        ListViewHelper.SetSortKeyProperty(lvwUsers, ColumnHeader.Index + 1 - 1)
    End Sub
End Class