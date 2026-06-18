Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'Developer Guide no.129
Imports SharedFiles
'Modified as made the Friend class to public
'Partial Friend Class frmList
Partial Public Class frmList
    Inherits System.Windows.Forms.Form
    'Developer Guide no. 129
    Dim frmDetails As frmDetails
    Public frmAdd As frmAdd
    ' ***************************************************************** '
    ' Form Name: frmList
    '
    ' Date: 11th July 1997
    '
    ' Description: Main interface. Shows List of available details.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmList"


    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_vCurrentPeriod(,) As Object

    ' {* USER DEFINED CODE (Begin) *}
    Private m_sNavigatorTitle As String = ""
    ' Stores the last business data ID used
    Private m_lLastDataID As Integer

    ' Stores the result of GetPeriodYears
    Private m_vResultArray(,) As Object
    Private Const vbFormCode As Integer = 0

    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the Business object.

    ' 'Modified as per Vb code 
    'Private m_oBusiness As bACTPeriod.Form
    Private m_oBusiness As Object

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast() As Control

    ' Stores the List data from the business object.
    Public m_vListData(,) As Object

    'DD 27/08/2002: Added for Multi-Tree product option
    Private m_bMultiTree As Boolean
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

    ' {* USER DEFINED CODE (Begin) *}
    Public ReadOnly Property NavigatorTitle() As String
        Get

            ' Return the objects parameter value.
            Return m_sNavigatorTitle

        End Get
    End Property
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)
    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Initialise the form
    '
    ' ***************************************************************** '
    Public Function Initialise() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sMessage, sTitle As String

        Try
            'modified as add the instance of the frmDetails
            'frmDetails = New frmDetails
            'Add the new instance of frmadd 
            'frmAdd = New frmAdd
         
            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bACTPeriod.Form", vInstanceManager:="ClientManager")
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.
                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception



            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Load (Standard Method)
    '
    ' Description: Load the form details
    '
    ' ***************************************************************** '
    Public Function Load_Renamed() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the process modes for the busines object.

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Load")
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}

            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = CType(GetInterfaceDetails(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: ShowForm (Standard Method)
    '
    ' Description: Show the form
    '
    ' ***************************************************************** '
    Public Function ShowForm(ByRef lDisplayState As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Show the the form, allow user input etc.
            VB6.ShowForm(Me, lDisplayState)

            Return result

        Catch excep As System.Exception



            ' Error Section
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to show the form", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Private Function GetBusiness() As Integer
        Dim result As Integer = 0
        Dim lSubBranchID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.

            ' {* USER DEFINED CODE (Begin) *}

            ' Only retrieve data when the Sub-Branch is chosen
            If cboSubBranch.SelectedIndex >= 0 Or Not m_bMultiTree Then
                lvwListDetails.Items.Clear()
                If m_bMultiTree Then
                    lSubBranchID = VB6.GetItemData(cboSubBranch, cboSubBranch.SelectedIndex)
                Else
                    'Hardcoded for system speed
                    lSubBranchID = 1
                End If


                m_lReturn = m_oBusiness.GetCurrentPeriodDetails(r_vDetails:=m_vCurrentPeriod, vSubBranchID:=lSubBranchID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                        result = gPMConstants.PMEReturnCode.PMNotFound
                    Else
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get current period details.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")
                    End If
                    Return result
                End If


                m_lReturn = m_oBusiness.GetPeriodYears(vResultArray:=m_vResultArray, v_vSubBranchID:=lSubBranchID)

                ' {* USER DEFINED CODE (End) *}

                ' If no records found return NotFound
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If

                ' Check for other errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get details.
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                    Return result
                End If
            Else
                'nothing to show
                result = gPMConstants.PMEReturnCode.PMNotFound
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
    ' Name: BusinessToData
    '
    ' Description: Updates the data storage from the business object.
    '
    ' ***************************************************************** '
    Private Function BusinessToData() As Integer

        ' {* USER DEFINED CODE (Begin) *}
        Dim result As Integer = 0
        ' {* USER DEFINED CODE (End) *}

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details to the data storage to be
            ' used with the List.

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the data array (Allow one extra field
            ' for the unique key to been assigned).
            ' *** This also needs to be edited in FormReturn ***
            ReDim m_vListData(2, 0)

            ' Retrieve all of the details from the business object.
            For lRow As Integer = m_vResultArray.GetLowerBound(1) To m_vResultArray.GetUpperBound(1)
                ' Store all of the data.

                ' ************************************************************
                ' Enter your code here to assign all of the details from the
                ' business object to the List data storage
                '
                ' Example:-
                '
                '    m_vListData(0, UBound(m_vListData, 2)) = m_DName$
                '
                ' NOTE: Replace this section with your new code.
                ' ************************************************************
                ' Use dummy ID to indicate Year is already in database
                m_vListData(ACSubYearID, m_vListData.GetUpperBound(1)) = 1

                m_vListData(ACSubYearName, m_vListData.GetUpperBound(1)) = m_vResultArray(0, lRow)
                ' {* USER DEFINED CODE (End) *}

                ' Store unique key for this row.
                m_vListData(m_vListData.GetUpperBound(0), m_vListData.GetUpperBound(1)) = m_vListData.GetUpperBound(1) + 1

                ' Increment the data array.
                ReDim Preserve m_vListData(m_vListData.GetUpperBound(0), m_vListData.GetUpperBound(1) + 1)
            Next

            ' Check if we have data in the List array.
            If Information.IsArray(m_vListData) Then
                If m_vListData.GetUpperBound(1) > 0 Then
                    ' Decrement the data array.
                    ReDim Preserve m_vListData(m_vListData.GetUpperBound(0), m_vListData.GetUpperBound(1) - 1)
                End If
            End If

            ' Store the last Data ID used by the business
            m_lLastDataID = m_vListData.GetUpperBound(1) + 1

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
    ' Name: DataToBusiness
    '
    ' Description: Updates all business members from the data storage.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (DataToBusiness) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    Private Function DataToBusiness(ByRef lMode As Integer, ByRef lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim lBusinessDataID As Integer

        '{* USER DEFINED CODE (Begin) *}
        '{* USER DEFINED CODE (End) *}

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Assign the details from the data storage to
            'the business object.

            ' {* USER DEFINED CODE (Begin) *}

            'Store all of the displayable data.

            '************************************************************
            'Enter your code here to assign all of the details
            'from the List data array to the data storage.
            '
            'Example:-
            '
            '  m_DName$ = m_vListData(0, lRow&)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            '{* USER DEFINED CODE (End) *}

            ' Store unique key for this row.
            lBusinessDataID = CInt(m_vListData(m_vListData.GetUpperBound(0), lRow))

            'Check the task.
            Select Case (lMode)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Inform the business object with a new data item.

                    '{* USER DEFINED CODE (Begin) *}

                    m_lReturn = m_oBusiness.EditAdd(lRow:=lBusinessDataID)
                    '{* USER DEFINED CODE (End) *}

                Case gPMConstants.PMEComponentAction.PMEdit
                    'Inform the business object with an updated data item.

                    '{* USER DEFINED CODE (Begin) *}

                    m_lReturn = m_oBusiness.EditUpdate(lRow:=lBusinessDataID)
                    '{* USER DEFINED CODE (End) *}

                Case gPMConstants.PMEComponentAction.PMDelete
                    'Inform the business object with a deleted data item.

                    m_lReturn = m_oBusiness.EditDelete(lRow:=lBusinessDataID)
            End Select

            'Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                'Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the data details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToBusiness")
            End If

            Return result

        Catch excep As System.Exception



            'Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            'Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DataToInterface
    '
    ' Description: Updates all interface details from the List data.
    '              storage.
    '
    ' ***************************************************************** '
    Private Function DataToInterface() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        Dim lSubBranchID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Clear the List details.
            lvwListDetails.Items.Clear()

            ' Check that List details are present before
            ' continuing.
            If Not Information.IsArray(m_vListData) Then
                ' No details so disable Edit
                cmdEdit.Enabled = False
                Return result
            End If

            ' We have some details so we can enable Edit
            cmdEdit.Enabled = True

            ' Assign the details to the interface.

            For lRow As Integer = m_vListData.GetLowerBound(1) To m_vListData.GetUpperBound(1)

                ' {* USER DEFINED CODE (Begin) *}

                ' ************************************************************
                ' Enter your code here to assign the all of the interface
                ' details from the List data storage, using the FormatField
                ' function for any type conversion.
                '
                ' Example:-
                '
                '    ' Assign the details to the first column.
                '    Set oListItem = lvwListDetails.ListItems.Add(, , _
                ''        Trim$(m_vListData(ACName, lRow&)), , ACListImage)
                '
                '    ' Assign details to other the columns
                '    oListItem.SubItems(1) = Trim$(m_vListData(ACCode, lRow&))
                '
                '
                ' NOTE: Replace this section with your new code.
                ' ************************************************************

                'Use the key string value to display the image in the list view.
                'oListItem = lvwListDetails.Items.Add(CStr(m_vListData(ACSubYearName, lRow)).Trim(), "")
                oListItem = lvwListDetails.Items.Add(CStr(m_vListData(ACSubYearName, lRow)).Trim(), "PeriodImage")

                'oListItem.SubItems(1) = Trim$(m_vListData(ACSubCode, lRow&))

                ' {* USER DEFINED CODE (End) *}

                ' Set the tag property with the index of
                ' the List data storage.
                oListItem.Tag = CStr(lRow)
            Next lRow

            ' Select the first item.
            lvwListDetails.FocusedItem = lvwListDetails.Items.Item(0)

            ' Display the current caption

            If m_bMultiTree Then
                lSubBranchID = VB6.GetItemData(cboSubBranch, cboSubBranch.SelectedIndex)
            Else
                lSubBranchID = 1
            End If



            m_lReturn = m_oBusiness.GetCurrentPeriodDetails(r_vDetails:=m_vCurrentPeriod, vSubBranchID:=lSubBranchID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    result = gPMConstants.PMEReturnCode.PMNotFound
                Else
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get current period details.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")
                End If
                Return result
            End If

            pnlCurrentPeriod.Text = CStr(m_vCurrentPeriod(4, 0)) & " of " & CStr(m_vCurrentPeriod(3, 0))

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the List data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayLookupDetails
    '
    ' Description: Displays all of the lookup details using the lookup
    '              values/details.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (DisplayLookupDetails) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function DisplayLookupDetails() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Get the lookup values.
    '
    'm_lReturn = CType(GetLookupValues(), gPMConstants.PMEReturnCode)
    '
    ' Check for errors.
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    ' Get all of the lookup details.
    '
    ' {* USER DEFINED CODE (Begin) *}
    '
    ' ************************************************************
    ' Enter your code here to retreive all of the lookup
    ' descriptions for a given lookup type.
    ' The GetLookupDetails function will allow you to do this.
    ''
    ' Example:-
    ''
    '    m_lReturn& = GetLookupDetails( _
    ''        sLookupTable:=PMLookupCodeName, _
    ''        ctlLookup:=cmbCodeName)
    ''
    '    ' Check for errors.
    '    If (m_lReturn& <> PMTrue) Then
    '        DisplayLookupDetails = PMFalse
    '        Exit Function
    '    End If
    ''
    ' NOTE: Replace this section with your new code.
    ' ************************************************************
    '
    ' {* USER DEFINED CODE (End) *}
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: GetInterfaceDetails
    '
    ' Description: Gets the interface details.
    '
    ' ***************************************************************** '
    Private Function GetInterfaceDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the interface details from the
            ' business object.
            m_lReturn = CType(GetBusiness(), gPMConstants.PMEReturnCode)

            ' If we have some records populate array and list view
            If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get the details.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Assign the details from the business object
                ' to the List data storage.
                m_lReturn = CType(BusinessToData(), gPMConstants.PMEReturnCode)

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the details.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Assign the details from the List data storage
                ' to the interface.
                m_lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            '    ' Probably not needed by the List Form
            '    ' Display all of the lookup details.
            '    m_lReturn& = DisplayLookupDetails()
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        GetInterfaceDetails = PMFalse
            '        Exit Function
            '    End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInterfaceDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim vResultArray(,) As Object
        Dim vValue As String = ""

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


            ' Disable the Edit button by default.
            ' Enabled in DataToInterface when know we have something to edit
            cmdEdit.Enabled = False

            ' {* USER DEFINED CODE (Begin) *}
            ' Set the column widths for the List headers.

            'lvwListDetails.ColumnHeaders(1).Width = 4350

            ' CTAF 200100 - Set width to size of control
            lvwListDetails.Columns.Item(0).Width = CInt(lvwListDetails.Width - VB6.TwipsToPixelsX(380))
            ' CTAF 200100 - ...and enable full row select
            m_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwListDetails.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)

            'lvwListDetails.ColumnHeaders(2).Width = 2030

            'DD 27/08/2002: Get product option setting
            iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, g_iSourceID, vValue)
            m_bMultiTree = (gPMFunctions.NullToString(vValue) = "1")

            If m_bMultiTree Then
                ' DD 02/08/2002: Added population of Sub-branch Combo

                m_lReturn = m_oBusiness.GetSubBranches(vResultArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Display message.
                    MessageBox.Show("Failed to get Sub-Branches", "Sub-Branches failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    'initialise = gPMConstants.PMEReturnCode.PMFalse
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If

                If Information.IsArray(vResultArray) Then

                    For lRow As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)
                        Dim cboSubBranch_NewIndex As Integer = -1

                        cboSubBranch_NewIndex = cboSubBranch.Items.Add(CStr(vResultArray(3, lRow)))

                        VB6.SetItemData(cboSubBranch, cboSubBranch_NewIndex, CInt(vResultArray(0, lRow)))
                    Next lRow

                    'If there is only one sub-branch then choose it and disable the control

                    If vResultArray.GetUpperBound(1) = 0 Then
                        cboSubBranch.SelectedIndex = 0
                        cboSubBranch.Enabled = False
                    End If
                End If
            Else
                'Hide the sub-branch combo neatly.
                lblSubBranch.Visible = False
                cboSubBranch.Visible = False

                'Shift everything up
                lvwListDetails.Top = lblCurrentPeriod.Top
                lvwListDetails.Height += lblCurrentPeriod.Top - lblSubBranch.Top
                lblCurrentPeriod.Top = lblSubBranch.Top
                'Modified panel's top position
                ''pnlCurrentPeriod.Top = cboSubBranch.Top
                pnlCurrentPeriod.Top = cboSubBranch.Top + 2
            End If
            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


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


            cmdAdd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblSubBranch.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSubBranchLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            SSTabHelper.SetSelectedIndex(tabMainTab, 0)



            lvwListDetails.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'lvwListDetails.ColumnHeaders(2).Text = iPMFunc.GetResData( _
            'iLangID:=g_iLanguageID%, _
            'lID:=ACListTitle2, _
            'iDataType:=PMResString)

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
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '
    Private Function GetLookupValues() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Gets all of the lookup values.

            ' Check the task.
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Get all of the lookup values.

                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Get all of the lookup values with the correct
                    ' effective date.

                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)

                Case gPMConstants.PMEComponentAction.PMView
                    ' Get lookup values for viewing only.

                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
            End Select

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")

                Return result
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetLookupDetails) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As Control) As Integer
    '
    'Dim result As Integer = 0
    'Dim lRow As Integer
    'Dim bFoundMatch As Boolean
    '
    ' Lookup value contants.
    'Const ACValueTableName As Integer = 0
    'Const ACValueID As Integer = 1
    'Const ACValueStartPos As Integer = 2
    'Const ACValueNumber As Integer = 3
    '
    ' Lookup detail contants.
    'Const ACDetailKey As Integer = 0
    'Const ACDetailDesc As Integer = 1
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Get the lookup values.
    '
    'bFoundMatch = False
    '
    'For 'lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
    ' Check for a match of the table name.
    'If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
    ' Found a match
    'bFoundMatch = True
    'Exit For
    'End If
    'Next lRow
    '
    ' Check if there has been a table match.
    'If Not bFoundMatch Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")
    '
    'Return result
    'End If
    '
    ' Using the lookup values, populate the control with
    ' the details from the lookup details array.
    '
    'For 'lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
    ' Add the details to the control.

    'ctlLookup.AddItem(m_vLookupDetails(ACDetailDesc, lCntr))


    'ctlLookup.ItemData(ctlLookup.NewIndex) = m_vLookupDetails(ACDetailKey, lCntr)
    '
    ' Check if this is the selected index.
    'If CBool(CStr((CStr(m_vLookupValues(ACValueID, lRow))) = CStr(m_vLookupDetails(ACDetailKey, lCntr)))) Then


    'ctlLookup.ListIndex = ctlLookup.NewIndex
    'End If
    'Next lCntr
    '
    ' If nothing yet selected (index = -1)
    ' select first item (index = 0)

    'If ctlLookup.ListIndex < 0 Then

    'ctlLookup.ListIndex = 0
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    ' ***************************************************************** '
    ' Name: AddFormProcess (Standard Method)
    '
    ' Description: Display details form and Add, Edit or View
    '
    ' ***************************************************************** '
    Private Function AddFormProcess() As Integer

        Dim result As Integer = 0
        Dim lRow As Integer

        Try
            'Add the new instance of frmadd 
            frmAdd = New frmAdd
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Call the initialise method passing a business ref
            m_lReturn = CType(frmAdd.Initialise(oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                frmAdd = Nothing
                Return result
            End If

            ' Pass standard details to form properties
            With frmAdd
                .CallingAppName = m_sCallingAppName
                .Task = m_iTask
                .Navigate = m_lNavigate
                .ProcessMode = m_lProcessMode
                .TransactionType = m_sTransactionType
                .EffectiveDate = m_dtEffectiveDate
                If m_bMultiTree Then
                    .SubBranchID = VB6.GetItemData(cboSubBranch, cboSubBranch.SelectedIndex)
                Else
                    'DD 27/08/2002: Hardcoded for speed
                    .SubBranchID = 1
                End If

                ' {* USER DEFINED CODE (Begin) *}
                ' {* USER DEFINED CODE (End) *}
            End With

            ' Set the row to zero
            lRow = 0

            ' Call the Load method to setup the interface details
            m_lReturn = CType(frmAdd.Load_Renamed(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                frmAdd = Nothing
                Return result
            End If

            ' Call the ShowForm method to show the form, allow user input etc.
            m_lReturn = CType(frmAdd.ShowForm(lDisplayState:=FormShowConstants.Modal), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                frmAdd = Nothing
                Return result
            End If

            ' If OK was pressed return List data
            ' and update Business from Add Form properties
            If frmAdd.Status = gPMConstants.PMEReturnCode.PMOK Then
                m_lReturn = CType(FormReturn(lRow:=lRow, iTaskType:=gPMConstants.PMEComponentAction.PMAdd), gPMConstants.PMEReturnCode)
                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    frmAdd.Close()
                    frmAdd = Nothing
                    Return result
                End If
            End If

            frmAdd.Close()
            frmAdd = Nothing

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process add form", vApp:=ACApp, vClass:=ACClass, vMethod:="AddFormProcess", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: AddFormToData (Standard Method)
    '
    ' Description: Use add form properties to populate data array
    '
    ' ***************************************************************** '
    Private Function AddFormToData(ByRef lRow As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With frmAdd
                ' {* USER DEFINED CODE (Begin) *}
                ' Use zero ID to indicate Year just added to database
                m_vListData(ACSubYearID, lRow) = 0
                m_vListData(ACSubYearName, lRow) = .YearName
                ' {* USER DEFINED CODE (End) *}
            End With

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update data from add form", vApp:=ACApp, vClass:=ACClass, vMethod:="AddFormToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DetailsFormProcess (Standard Method)
    '
    ' Description: Display details form and Add, Edit or View
    '
    ' ***************************************************************** '
    Private Function DetailsFormProcess(ByRef iTaskType As Integer) As Integer

        Dim result As Integer = 0
        Dim lRow As Integer

        Try
            'modified as add the instance of the frmDetails
            frmDetails = New frmDetails
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Call the initialise method passing a business ref
            m_lReturn = CType(frmDetails.Initialise(oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                frmDetails = Nothing
                Return result
            End If

            ' Pass standard details to form properties
            With frmDetails
                .CallingAppName = m_sCallingAppName
                .Task = iTaskType
                .Navigate = m_lNavigate
                .ProcessMode = m_lProcessMode
                .TransactionType = m_sTransactionType
                .EffectiveDate = m_dtEffectiveDate
                ' {* USER DEFINED CODE (Begin) *}
                .YearChanged = False
                'DD 27/08/2002: Added for new product option
                If m_bMultiTree Then
                    .SubBranchID = VB6.GetItemData(cboSubBranch, cboSubBranch.SelectedIndex)
                Else
                    .SubBranchID = 1
                End If
                ' {* USER DEFINED CODE (End) *}
            End With

            ' Will be edit but standard code still ok
            ' If Edit or View populate Details Form properties
            ' from List data array
            Select Case (iTaskType)
                Case gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMView
                    ' Set the row to the selected item
                    lRow = Convert.ToString(lvwListDetails.FocusedItem.Tag)

                    ' Populate form properties
                    m_lReturn = CType(DataToDetailsForm(lRow:=lRow), gPMConstants.PMEReturnCode)
                    ' Check for errors.
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' No lookups at the moment so not needed
                    '            ' Inform the business of the current record
                    '            ' so that the lookup ID match works
                    '            m_oBusiness.CurrentRecord = m_vListData(UBound(m_vListData, 1), lRow&)

                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Set the row to zero
                    lRow = 0

                Case Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unknown Case condition", vApp:=ACApp, vClass:=ACClass, vMethod:="DetailsFormProcess")

            End Select

            ' Call the Load method to setup the interface details
            m_lReturn = CType(frmDetails.Load_Renamed(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                frmDetails = Nothing
                Return result
            End If

            ' Call the ShowForm method to show the form, allow user input etc.
            m_lReturn = CType(frmDetails.ShowForm(lDisplayState:=FormShowConstants.Modal), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                frmDetails = Nothing
                Return result
            End If

            ' If OK was pressed and Edit or Add return List data
            ' and update Business from Details Form properties
            If frmDetails.Status = gPMConstants.PMEReturnCode.PMOK Then
                Select Case (iTaskType)
                    Case gPMConstants.PMEComponentAction.PMEdit
                        m_lReturn = CType(FormReturn(lRow:=lRow, iTaskType:=gPMConstants.PMEComponentAction.PMEdit), gPMConstants.PMEReturnCode)
                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            frmDetails.Close()
                            frmDetails = Nothing
                            Return result
                        End If

                        ' CTAF 200100 - Check for change of year name
                        If frmDetails.YearChanged Then
                            ' Refresh the list
                            m_lReturn = CType(GetInterfaceDetails(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                ' Log Error.
                                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to refresh list of accounting years.", vApp:=ACApp, vClass:=ACClass, vMethod:="DetailsFormProcess", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                                Return result
                            End If
                        End If

                End Select
            End If

            frmDetails.Close()
            frmDetails = Nothing

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process details form", vApp:=ACApp, vClass:=ACClass, vMethod:="DetailsFormProcess", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FormReturn (Standard Method)
    '
    ' Description: Return List data
    '              from Add or Details Form properties
    '
    ' ***************************************************************** '
    Private Function FormReturn(ByRef lRow As Integer, ByRef iTaskType As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If Add expand List array to take new entry
            If iTaskType = gPMConstants.PMEComponentAction.PMAdd Then

                ' If the array exists increment it
                ' otherwise must be the first entry so set it up
                If Information.IsArray(m_vListData) Then
                    ReDim Preserve m_vListData(m_vListData.GetUpperBound(0), m_vListData.GetUpperBound(1) + 1)
                Else
                    ReDim m_vListData(2, 0)
                End If

                ' Store unique key for this row used to point to business collection.
                ' This must always be last key added + 1, as deletion does not actually
                ' remove the record from business collection it just flags for deletion
                m_lLastDataID += 1
                m_vListData(m_vListData.GetUpperBound(0), m_vListData.GetUpperBound(1)) = m_lLastDataID

                ' Set row to point to newly added List data array row
                lRow = m_vListData.GetUpperBound(1)

                ' Populate List array from add Form properties
                m_lReturn = CType(AddFormToData(lRow:=lRow), gPMConstants.PMEReturnCode)

                ' Must be Edit
            Else
                ' Populate List array from Details Form properties
                m_lReturn = CType(DetailsFormToData(lRow:=lRow), gPMConstants.PMEReturnCode)

            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Update currently done in Add or Details form
            '    ' Update Business from List array
            '    m_lReturn& = DataToBusiness( _
            ''                    lMode:=CLng(iTaskType), _
            ''                    lRow:=lRow)
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        FormReturn = PMFalse
            '        Exit Function
            '    End If

            ' Refresh contents of List box
            m_lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Reselect the current item.
            lvwListDetails.FocusedItem = lvwListDetails.Items.Item(lRow)

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update data from form", vApp:=ACApp, vClass:=ACClass, vMethod:="FormReturn", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DataToDetailsForm (Standard Method)
    '
    ' Description: Use data array to populate details form properties
    '
    ' ***************************************************************** '
    Private Function DataToDetailsForm(ByRef lRow As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Pass details to form properties
            With frmDetails
                ' {* USER DEFINED CODE (Begin) *}

                .YearName = CStr(m_vListData(ACSubYearName, lRow))

                ' {* USER DEFINED CODE (End) *}
            End With

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update details form from data", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToDetailsForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: DetailsFormToData (Standard Method)
    '
    ' Description: Use details form properties to populate data array
    '
    ' ***************************************************************** '
    Private Function DetailsFormToData(ByRef lRow As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With frmDetails
                ' {* USER DEFINED CODE (Begin) *}

                m_vListData(ACSubYearName, lRow) = .YearName

                ' {* USER DEFINED CODE (End) *}
            End With

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update data from details form", vApp:=ACApp, vClass:=ACClass, vMethod:="DetailsFormToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessCommand
    '
    ' Description: Determines which action to take on the details
    '              depending upon the task and interface state.
    '
    ' ***************************************************************** '
    Private Function ProcessCommand() As Integer

        Dim result As Integer = 0
        
        Try


            'BB Update already done in Add and Details Grid Forms so no need here
            '    ' Check the task.
            '    Select Case m_iTask
            '        Case PMEdit
            '            ' Check if form has been cancelled, if so,
            '            ' check if the details have changed and if
            '            ' so, prompt if they wish to cancel.
            '            If m_lStatus = PMCancel Then
            '                ' Check the details havn't changed.
            '                m_lReturn& = m_oBusiness.Cancel()
            '
            '                If (m_lReturn& = PMDataChanged) Then
            '                    ' Get string messages
            '
            '                    sTitle$ = iPMFunc.GetResData( _
            ''                        iLangID:=g_iLanguageID%, _
            ''                        lID:=ACCancelDetailsTitle, _
            ''                        iDataType:=PMResString)
            '
            '                    sMessage$ = iPMFunc.GetResData( _
            ''                        iLangID:=g_iLanguageID%, _
            ''                        lID:=ACCancelDetails, _
            ''                        iDataType:=PMResString)
            '
            '                    iMsgResult = MsgBox(sMessage$, _
            ''                    vbYesNo + vbDefaultButton2 + vbQuestion, sTitle$)
            '
            '                    ' Check message result.
            '                    If (iMsgResult = vbNo) Then
            '                        ' Set return to false, meaning
            '                        ' don't cancel.
            '                        ProcessCommand = PMFalse
            '                    End If
            '                End If
            '            Else
            '                ' Update the details using the business object.
            '                m_lReturn& = m_oBusiness.Update()
            '
            '                ' Check for errors.
            '                If (m_lReturn& <> PMTrue) Then
            '                   ' Failed to update the details
            '                   ProcessCommand = PMFalse
            '
            '                   ' Log Error.
            '                   LogMessage _
            ''                       iType:=PMLogError, _
            ''                       sMsg:="Failed to update the details", _
            ''                       vApp:=ACApp, _
            ''                       vClass:=ACClass, _
            ''                       vMethod:="ProcessCommand"
            '                End If
            '            End If
            '    End Select

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process command", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'PRIVATE Methods (Begin)
    'PRIVATE Methods (End)


    Private isInitializingComponent As Boolean
    Private Sub cboSubBranch_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboSubBranch.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        'Refresh the form
        GetInterfaceDetails()
    End Sub

    Private Sub cboSubBranch_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboSubBranch.SelectionChangeCommitted
        'Refresh the form
        GetInterfaceDetails()
    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        ' Fire up the help screen
        'Developer Guide no 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = CType(PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID2), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub Form_Initialize_Renamed()
        Try

            iPMFunc.ShowFormInTaskBar_Attach()

        Catch
        End Try



    End Sub


    Private Sub frmList_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        Try
            'added as load function of the form has to be called
            Load_Renamed()
            iPMFunc.ShowFormInTaskBar_Detach()

        Catch
        End Try



    End Sub

    ' PRIVATE Methods (End)

    ' PRIVATE Events (Begin)


    Private Sub frmList_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            'Developer Guide no.19
            If UnloadMode <> vbFormCode Then

                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = CType(ProcessCommand(), gPMConstants.PMEReturnCode)

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Terminate the business object.

            m_oBusiness.Dispose()
            ' Remove this instance of the business object from memory.
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

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions.
            m_lReturn = CType(ProcessCommand(), gPMConstants.PMEReturnCode)

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

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions.
            m_lReturn = CType(ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click

        ' Click event of the Add Button.

        Try

            ' {* USER DEFINED CODE (Begin) *}

            ' Add Details form
            ' DD 24/08/2002: Only open if user has selected a sub-branch
            ' and the MultiTree product option is active
            If cboSubBranch.SelectedIndex >= 0 Or Not m_bMultiTree Then
                m_lReturn = CType(AddFormProcess(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                    Exit Sub
                End If
            End If

            ' {* USER DEFINED CODE (End) *}

        Catch excep As System.Exception



            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Add button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAdd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        ' Click event of the Edit Button.
       
        Try

            ' {* USER DEFINED CODE (Begin) *}

            ' Edit Details form
            ' DD 02/08/2002: Only open if user has selected a sub-branch
            If cboSubBranch.SelectedIndex >= 0 Or Not m_bMultiTree Then
                m_lReturn = CType(DetailsFormProcess(gPMConstants.PMEComponentAction.PMEdit), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                    Exit Sub
                End If
            End If

            ' {* USER DEFINED CODE (End) *}

        Catch excep As System.Exception



            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Edit button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEdit_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click

        ' Click event of the Navigate button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMNavigate

            ' Process the next set of actions.
            m_lReturn = CType(ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwListDetails_ItemClick(ByVal Item As ListViewItem)

        ' Single ItemClick event for the List details.

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error Section.
        '
        ' Log Error.
        'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the ItemClick event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwListDetails_ItemClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Private Sub lvwListDetails_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwListDetails.DoubleClick

        ' Double click event for the List details.

        Try

            ' Check if there are any items available.
            If lvwListDetails.Items.Count = 0 Then
                Exit Sub
            End If

            ' Bring up Edit Details form
            m_lReturn = CType(DetailsFormProcess(gPMConstants.PMEComponentAction.PMEdit), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwListDetails_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwListDetails_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwListDetails.Enter

        ' GotFocus Event for the List details

        Try

            ' Check if there are any items available.
            If lvwListDetails.Items.Count = 0 Then
                VB6.SetDefault(cmdAdd, True)
            Else
                VB6.SetDefault(cmdEdit, True)
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwListDetails_GotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwListDetails_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwListDetails.Leave

        ' LostFocus Event for the List details

        Try

            ' Set the default button.
            VB6.SetDefault(cmdOK, True)

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwListDetails_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwListDetails_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwListDetails.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwListDetails.Columns(eventArgs.Column)

        ' Column click event for the List details

        Try

            With lvwListDetails
                ' If current sort column header is
                ' pressed.
                If ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwListDetails) Then
                    ' Set sort order opposite of
                    ' current direction.
                    ListViewHelper.SetSortOrderProperty(lvwListDetails, (ListViewHelper.GetSortOrderProperty(lvwListDetails) + 1) Mod 2)
                Else
                    ' Sort by this column (ascending).
                    ListViewHelper.SetSortedProperty(lvwListDetails, False)

                    ' Turn off sorting so that the list
                    ' is not sorted twice
                    ListViewHelper.SetSortOrderProperty(lvwListDetails, SortOrder.Ascending)
                    ListViewHelper.SetSortKeyProperty(lvwListDetails, ColumnHeader.Index + 1 - 1)
                    ListViewHelper.SetSortedProperty(lvwListDetails, True)
                End If
            End With

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwListDetails_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    'PRIVATE Events (End)

    Private Sub frmList_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown

        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If
    End Sub
End Class
