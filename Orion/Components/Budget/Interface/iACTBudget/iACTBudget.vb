Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
Imports SharedFiles

Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 20/10/1998
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"


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

    Private m_lBudgetID As Integer
    Private m_lPeriodID As Integer
    Private m_sBudgetRef As String = ""
    Private m_sDescription As String = ""
    Private m_sPeriodYearName As String = ""
    Private m_lRevisesBudgetID As Integer
    Private m_lBasedOnBudgetID As Integer
    Private m_lBudgetStatusID As Integer
    'PN10254
    Private m_sBudgetStatus As String = ""

    Private m_sRevisesRef As String = ""

    Private m_sNavigatorTitle As String = ""


    'Developer Guide NO. 108
    Private m_oFindBudget As iACTFindBudget.Interface_Renamed

    ' {* USER DEFINED CODE (End) *}

    Private m_sStepStatus As New FixedLengthString(2)


    ' Declare an instance of the general interface object.
    Private m_oGeneral As iACTBudget.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)
    'Developer Guide No. 7
    Private Const vbFormCode As Integer = 0


    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property NavigatorTitle() As String
        Get

            ' Return the objects parameter value.
            Return m_sNavigatorTitle

        End Get
    End Property

    Public ReadOnly Property StepStatus() As String
        Get

            ' Standard Property.

            ' Return the Steps Status
            Return m_sStepStatus.Value

        End Get
    End Property

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

    Public Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the interface exit status.
            m_lStatus = Value

        End Set
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

    Public Property BudgetID() As Integer
        Get
            Return m_lBudgetID
        End Get
        Set(ByVal Value As Integer)
            m_lBudgetID = Value
        End Set
    End Property

    Public Property PeriodID() As Integer
        Get
            Return m_lPeriodID
        End Get
        Set(ByVal Value As Integer)
            m_lPeriodID = Value
        End Set
    End Property

    Public Property BudgetRef() As String
        Get
            Return m_sBudgetRef
        End Get
        Set(ByVal Value As String)
            m_sBudgetRef = Value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return m_sDescription
        End Get
        Set(ByVal Value As String)
            m_sDescription = Value
        End Set
    End Property

    Public Property PeriodYearName() As String
        Get
            Return m_sPeriodYearName
        End Get
        Set(ByVal Value As String)
            m_sPeriodYearName = Value
        End Set
    End Property

    Public Property BasedOnBudgetID() As Integer
        Get
            Return m_lBasedOnBudgetID
        End Get
        Set(ByVal Value As Integer)
            m_lBasedOnBudgetID = Value
        End Set
    End Property

    Public Property BudgetStatusID() As Integer
        Get
            Return m_lBudgetStatusID
        End Get
        Set(ByVal Value As Integer)
            m_lBudgetStatusID = Value
        End Set
    End Property

    Public Property RevisesBudgetID() As Integer
        Get
            Return m_lRevisesBudgetID
        End Get
        Set(ByVal Value As Integer)
            m_lRevisesBudgetID = Value
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


            m_lReturn = m_oBusiness.GetDetails(vBudgetID:=m_lBudgetID)

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
        Dim sStatus As String = ""
        Dim vStatusList(,) As Object

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

            ' ************************************************************
            ' Enter your code here to assign the all of the interface
            ' details from the business object, using the FormatField
            ' function for any type conversion.
            '
            ' Example:-
            '
            '    txtDesc.Text = FormatField( _
            ''        iFormatType:=PMFormatString, _
            ''        vFieldValue:=m_sDDesc$)
            '
            '    optChoice.Value = CBool(FormatField( _
            ''        iFormatType:=PMFormatBoolean, _
            ''        vFieldValue:=m_iDChoice%))
            '
            '    txtDate.Text = FormatField( _
            ''        iFormatType:=PMFormatDateLong, _
            ''        vFieldValue:=m_dtDDate)
            '
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            txtDescription.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sDescription)

            txtBudgetRef.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sBudgetRef)

            ' Set the year on the drop down list
            For iLoop1 As Integer = 0 To cboPeriodYear.Items.Count
                If VB6.GetItemString(cboPeriodYear, iLoop1).Trim() = m_sPeriodYearName.Trim() Then
                    cboPeriodYear.SelectedIndex = iLoop1
                    Exit For
                End If
            Next iLoop1

            ' Get the budget's status and a list of all the possible statuses

            m_lReturn = m_oBusiness.GetBudgetStatus(v_lBudgetStatus:=m_lBudgetStatusID, r_sDescription:=sStatus, r_vStatusList:=vStatusList)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            cboStatus.Items.Clear()

            ' add the status to the list

            For iLoop1 As Integer = vStatusList.GetLowerBound(1) To vStatusList.GetUpperBound(1)
                Dim cboStatus_NewIndex As Integer = -1

                cboStatus_NewIndex = cboStatus.Items.Add(CStr(vStatusList(4, iLoop1)))
                'PN10253 eck 05022004

                VB6.SetItemData(cboStatus, cboStatus_NewIndex, CInt(vStatusList(0, iLoop1)))

                If CStr(vStatusList(4, iLoop1)) = sStatus Then
                    cboStatus.SelectedIndex = cboStatus_NewIndex
                End If
                'PN10253End

            Next iLoop1

            ' set the right one
            'PN10253 eck 05022004
            '    cboStatus.ListIndex = m_lBudgetStatusID& - 1
            m_sBudgetStatus = sStatus
            'PN10253End
            ' if in add mode, then only registered is allowed
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                cboStatus.Enabled = False
                '       cmdRevisesBudget.Enabled = False

            Else
                ' likewise in edit mode, if its posted, then only allow posted status
                'PN10254 use description not hard coded id
                '       If (m_iTask% = PMEdit) And (m_lBudgetStatusID& = 3) Then
                If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                    cmdRevisesBudget.Enabled = False
                    cmdBasedOnBudget.Enabled = False
                    txtRevisesBudget.Enabled = False
                    txtBasedOnBudget.Enabled = False
                    If m_sBudgetStatus = "Posted" Then
                        m_lReturn = m_oGeneral.DisableForm(lDisabled:=True)
                    End If
                End If
            End If

            txtRevisesBudget.Text = m_sRevisesRef

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

                    m_lReturn = m_oBusiness.DirectAdd(vBudgetID:=m_lBudgetID, vBudgetRef:=m_sBudgetRef, vPeriodID:=m_lPeriodID, vBudgetDescription:=m_sDescription, vPeriodYearName:=m_sPeriodYearName, vRevisesBudgetID:=m_lRevisesBudgetID, vBudgetStatusID:=m_lBudgetStatusID)
                    ' {* USER DEFINED CODE (End) *}

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Inform the business object with an updated data item.

                    ' {* USER DEFINED CODE (Begin) *}

                    m_lReturn = m_oBusiness.EditUpdate(lRow:=lBusinessDataID, vBudgetID:=m_lBudgetID, vBudgetRef:=m_sBudgetRef, vPeriodID:=m_lPeriodID, vBudgetDescription:=m_sDescription, vPeriodYearName:=m_sPeriodYearName, vRevisesBudgetID:=m_lRevisesBudgetID, vBudgetStatusID:=m_lBudgetStatusID)
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

    ' ***************************************************************** '
    ' Name: DisplayLookupDetails
    '
    ' Description: Displays all of the lookup details using the lookup
    '              values/details.
    '
    ' ***************************************************************** '
    Public Function DisplayLookupDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            'm_lReturn& = GetLookupValues()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get all of the lookup details.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to retreive all of the lookup
            ' descriptions for a given lookup type.
            ' The GetLookupDetails function will allow you to do this.
            '
            ' Example:-
            '
            '    m_lReturn& = GetLookupDetails( _
            ''        sLookupTable:=PMLookupCodeName, _
            ''        ctlLookup:=cmbCodeName)
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        DisplayLookupDetails = PMFalse
            '        Exit Function
            '    End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            'm_lReturn& = GetLookupDetails( _
            ''    sLookupTable:=ACTLookupPostingStatus, _
            ''    ctlLookup:=cboStatus)

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim lPeriodID As Integer
        Dim sDescription, sPeriodYearName As String
        Dim lRevisesBudgetID, lBudgetStatusID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details to the data storage.

            ' {* USER DEFINED CODE (Begin) *}


            m_lReturn = m_oBusiness.GetNext(vBudgetID:=m_lBudgetID, vBudgetRef:=m_sBudgetRef, vPeriodID:=m_lPeriodID, vBudgetDescription:=m_sDescription, vPeriodYearName:=m_sPeriodYearName, vRevisesBudgetID:=m_lRevisesBudgetID, vBudgetStatusID:=m_lBudgetStatusID)

            ' Get the revises budget reference
            If m_lRevisesBudgetID <> 0 Then

                m_lReturn = m_oBusiness.GetDetails(vBudgetID:=m_lRevisesBudgetID)


                m_lReturn = m_oBusiness.GetNext(vBudgetID:=m_lRevisesBudgetID, vBudgetRef:=m_sRevisesRef, vPeriodID:=lPeriodID, vBudgetDescription:=sDescription, vPeriodYearName:=sPeriodYearName, vRevisesBudgetID:=lRevisesBudgetID, vBudgetStatusID:=lBudgetStatusID)
            End If

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
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

            result = gPMConstants.PMEReturnCode.PMTrue

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

            ' TODO CF

            'm_lBudgetID& = 1

            m_sBudgetRef = txtBudgetRef.Text
            m_sDescription = txtDescription.Text
            m_sPeriodYearName = cboPeriodYear.Text
            'PN10253 eck 05022004
            'm_lBudgetStatusID& = cboStatus.ListIndex + 1
            m_lBudgetStatusID = VB6.GetItemData(cboStatus, cboStatus.SelectedIndex)
            ' {* USER DEFINED CODE (End) *}

            Return result

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
        Dim vYearArray(,) As Object
        Dim sStatus As String = ""
        Dim vStatusList(,) As Object

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

            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            ' Add the unique period years

            m_lReturn = m_oBusiness.GetUniquePeriodYears(r_vYearArray:=vYearArray)



            For iLoop1 As Integer = vYearArray.GetLowerBound(1) To vYearArray.GetUpperBound(1)

                cboPeriodYear.Items.Add(CStr(vYearArray(0, iLoop1)))
            Next iLoop1

            If (cboPeriodYear.Items.Count > 0) And (m_iTask = gPMConstants.PMEComponentAction.PMAdd) Then
                cboPeriodYear.SelectedIndex = 0
            End If

            ' Get the budget's status and a list of all the possible statuses

            m_lReturn = m_oBusiness.GetBudgetStatus(v_lBudgetStatus:=m_lBudgetStatusID, r_sDescription:=sStatus, r_vStatusList:=vStatusList)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            cboStatus.Items.Clear()

            ' add the status to the list

            For iLoop1 As Integer = vStatusList.GetLowerBound(1) To vStatusList.GetUpperBound(1)
                Dim cboStatus_NewIndex As Integer = -1

                cboStatus_NewIndex = cboStatus.Items.Add(CStr(vStatusList(4, iLoop1)))
                'PN10253 eck 05022004

                VB6.SetItemData(cboStatus, cboStatus_NewIndex, CInt(vStatusList(0, iLoop1)))

                If CStr(vStatusList(4, iLoop1)) = sStatus Then
                    cboStatus.SelectedIndex = cboStatus_NewIndex
                End If
            Next iLoop1

            ' set the right one
            'PN10253 eck 05022004
            '    cboStatus.ListIndex = m_lBudgetStatusID& - 1
            m_sBudgetStatus = sStatus
            'PN10253End

            ' if in add mode, then only registered is allowed
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                cboStatus.Enabled = False
            Else
                ' likewise in edit mode, if its posted, then only allow posted status
                'PN10254 use description not hard coded id
                '       If (m_iTask% = PMEdit) And (m_lBudgetStatusID& = 3) Then
                If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                    cmdBasedOnBudget.Enabled = False
                    cmdRevisesBudget.Enabled = False
                    If m_sBudgetStatus = "Posted" Then
                        m_lReturn = m_oGeneral.DisableForm(lDisabled:=True)
                    End If
                End If

            End If

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
            ReDim m_ctlTabFirstLast(1, 0)

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

            m_ctlTabFirstLast(ACControlStart, 0) = txtBudgetRef
            m_ctlTabFirstLast(ACControlEnd, 0) = cmdBasedOnBudget

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


            lblBudgetRef.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBudgetRef, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblDescription.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDescription, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblPeriodYear.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPeriodYear, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblRevisesBudget.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRevisesBudget, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblBasedOnBudget.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBasedOnBudget, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblStatus.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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
    'UPGRADE_NOTE: (7001) The following declaration (GetLookupValues) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetLookupValues() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Gets all of the lookup values.
    '
    ' Check the task.
    'Select Case (m_iTask)
    'Case gPMConstants.PMEComponentAction.PMAdd
    ' Get all of the lookup values.

    'm_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
    '
    'Case gPMConstants.PMEComponentAction.PMEdit
    ' Get all of the lookup values with the correct
    ' effective date.

    'm_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
    '
    'Case gPMConstants.PMEComponentAction.PMView
    ' Get lookup values for viewing only.

    'm_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
    'End Select
    '
    ' Check for errors.
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error.
    'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")
    '
    'Return result
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

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


    'ctlLookup.ItemData(ctlLookup.NewIndex) = CInt(m_vLookupDetails(ACDetailKey, lCntr))
    '
    ' Check if this is the selected index.
    'If m_vLookupValues(ACValueID, lRow).Equals(m_vLookupDetails(ACDetailKey, lCntr)) Then


    'ctlLookup.ListIndex = ctlLookup.NewIndex
    'End If
    'Next lCntr
    '
    ' Check if the selected index is blank. If so,
    ' we set the controls index to zero.
    'If CStr(m_vLookupValues(ACValueID, lRow)) = "" Then

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
    ' Name: CallFindForm
    '
    ' Description: Calls the find form, and returns the budget_id
    '
    ' ***************************************************************** '
    Private Function CallFindForm(ByRef r_lBudgetID As Integer, ByRef r_sBudgetRef As Object, ByRef r_lStatus As Integer) As Integer
        Dim result As Integer = 0
      
        Dim oFindBudget As iACTFindBudget.Interface_Renamed

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the find object
            Dim temp_oFindBudget As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindBudget, sClassName:="iACTFindBudget.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindBudget = temp_oFindBudget
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'PN10274
            With oFindBudget

                .Revising = True
            End With

            ' Start it up

            m_lReturn = oFindBudget.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the return values
            With oFindBudget

                r_lBudgetID = .BudgetID


                r_sBudgetRef = .Reference

                r_lStatus = .Status
            End With

            ' Terminate the object
            ' CF - DONT terminate it as this will remove iACTBudgetFind's instance too!
            '    m_lReturn& = oFindBudget.Terminate()
            '    If (m_lReturn& <> PMTrue) Then
            '        CallFindForm = PMFalse
            '        Exit Function
            '    End If
            '
            '    Set oFindBudget = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CallFindForm Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CallFindForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PRIVATE Methods (End)

    Private Sub cmdBasedOnBudget_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdBasedOnBudget.Click

        Dim lBasedOnBudgetID As Integer
        Dim sBudgetRef As String = ""
        Dim lStatus As Integer
        Dim sMsg As String = ""

        Try

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Fire up find budget
            m_lReturn = CallFindForm(r_lBudgetID:=lBasedOnBudgetID, r_sBudgetRef:=sBudgetRef, r_lStatus:=lStatus)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Was OK pressed?
            If lStatus = gPMConstants.PMEReturnCode.PMOK Then

                If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then

                    sMsg = "Are you sure you want to base budgets on this one?" & Environment.NewLine & _
                           "Note that this will overwrite existing details already entered."

                    m_lReturn = MessageBox.Show(sMsg, "Based on Budget", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                    If m_lReturn = System.Windows.Forms.DialogResult.Yes Then

                        ' assign the reference
                        txtBasedOnBudget.Text = sBudgetRef

                        ' Store the ID number
                        m_lBasedOnBudgetID = lBasedOnBudgetID
                        ' and store it in the tag too...
                        txtBasedOnBudget.Tag = CStr(lBasedOnBudgetID)

                    End If

                Else

                    ' assign the reference
                    txtBasedOnBudget.Text = sBudgetRef

                    ' Store the ID number
                    m_lBasedOnBudgetID = lBasedOnBudgetID
                    ' and store it in the tag too...
                    txtBasedOnBudget.Tag = CStr(lBasedOnBudgetID)

                End If

            End If

        Catch excep As System.Exception



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdBasedOnBudget Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdBasedOnBudget", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        ' Fire up the help screen
        'Developer Guide No. 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, lContextID:=ScreenHelpID)

    End Sub

    Private Sub cmdRevisesBudget_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRevisesBudget.Click

        Dim lBudgetID As Integer
        Dim sBudgetRef As String = ""
        Dim lStatus As gPMConstants.PMEReturnCode

        Try

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Fire up find budget
            m_lReturn = CallFindForm(r_lBudgetID:=lBudgetID, r_sBudgetRef:=sBudgetRef, r_lStatus:=lStatus)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Was OK pressed?
            If lStatus = gPMConstants.PMEReturnCode.PMOK Then

                ' assign the revises budget id
                m_lRevisesBudgetID = lBudgetID
                ' set the tag of the box to the id too
                txtRevisesBudget.Tag = CStr(lBudgetID)

                ' assign the reference
                txtRevisesBudget.Text = sBudgetRef

            End If

        Catch excep As System.Exception



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdRevisesBudget Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdRevisesBudget", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bACTBudget.Form", vInstanceManager:="ClientManager")
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iACTBudget.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Get an instance of Find Budget
            Dim temp_m_oFindBudget As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oFindBudget, sClassName:="iACTFindBudget.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oFindBudget = temp_m_oFindBudget
            '    Set m_oFindBudget = CreateObject("iACTFindBudget.Interface")
            '    m_lReturn& = m_oFindBudget.Initialise()

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


    Public Sub frmInterfaceLoad()

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
                    Cancel = 1
                    eventArgs.cancel = True
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

            '    ' Terminate the find object
            '    m_lReturn& = m_oFindBudget.Terminate()
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        m_lErrorNumber& = PMFalse
            '
            '        ' Log Error.
            '        LogMessage _
            ''            iType:=PMLogError, _
            ''            sMsg:="Failed to terminate the find object", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="Form_QueryUnload"
            '    End If
            '
            '    ' Destroy the instance of the find object
            '    Set m_oFindBudget = Nothing

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
                            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                            End If
                        End If

                    Case Keys.End
                        ' End key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlEnd, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                            End If
                        End If
                End Select
            End With
            'Developer Guide No 293
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
        Catch



            ' Error Section.

            Exit Sub
        End Try


    End Sub

    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        '    With tabMainTab
        '        ' Set the default button.
        '        If (.Tab < cmdNext.Count) Then
        '            cmdNext(.Tab).Default = True
        '        Else
        '            cmdOK.Default = True
        '        End If
        ''
        '        ' Now I know this is crap, this goes against
        '        ' all my principles, but for some reason when
        '        ' using the mouse to select a tab the setfocus
        '        ' code below doesn't work. The cursor sticks,
        '        ' and you can't tab off. Therefore I've used
        '        ' this to get around the problem.
        '        DoEvents
        ''
        '        ' Set focus to the first control on the tab.
        '        If (.Tab <= UBound(m_ctlTabFirstLast, 2)) Then
        '            m_ctlTabFirstLast(ACControlStart, .Tab).SetFocus
        '        End If
        '    End With
        '
        'Catch 
        '
        '
        '
        ' Error Section.
        '
        '
        'tabMainTabPreviousTab = tabMainTab.SelectedIndex
        'End Try

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

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

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



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    'UPGRADE_NOTE: (7001) The following declaration (cmdNext_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub cmdNext_Click(ByRef Index As Integer)
    '
    'Try 
    '
    ' Change to the next tab.
    'If SSTabHelper.GetSelectedIndex(tabMainTab) < SSTabHelper.GetTabCount(tabMainTab) - 1 Then
    'SSTabHelper.SetSelectedIndex(tabMainTab, Index + 1)
    'End If
    '
    ' Set focus to the first control on the tab.
    'If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
    'm_ctlTabFirstLast(ACControlStart, Index + 1).Focus()
    'End If
    '
    'Catch 
    '
    '
    '
    ' Error Section
    '
    'Exit Sub
    'End Try
    '
    '
    'End Sub
    ' PRIVATE Events (End)
    Private isInitializingComponent As Boolean
    Private Sub txtBudgetRef_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBudgetRef.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

    End Sub
End Class
