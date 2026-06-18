Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 09/06/1999
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"
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
    Private m_lPrimaryCauseId As Integer
    Private m_sCode As String = ""
    Private m_sDescription As String = ""

    'JMK 19/10/2001 hidden options variables determine display of Insurer/Reinsurer
    Private m_sUnderwritingOrAgency As String = ""
    Private m_sUnderwritingType As String = ""

    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMUPrimCauseRiskType.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object
    'Private m_oBusiness As bSIRCoinsurance.Business

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Control array to store the first and last
    ' text box controls for each tab.
    ' Stores the search data from the business object.
    Private m_vPrimCauseRiskTypeGrp As Object

    Private m_vRiskTypeGroup(,) As Object 'All risk type groups
    Private m_vLinkedRiskTypeGroup(,) As Object 'risk type groups which are linked to current risk type

    Private m_sUniqueId As String
    Private m_sScreenHierarchy As String
    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)


    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

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
    ' Set the interface exit status.
    'm_lStatus = Value
    '
    'End Sub
    Public ReadOnly Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    Public Property Task() As Integer
        Get
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            m_iTask = Value
        End Set
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)
            m_lNavigate = Value
        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)
            m_lProcessMode = Value
        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)
            m_dtEffectiveDate = Value
        End Set
    End Property
    ' {* USER DEFINED CODE (Begin) *}
    Public WriteOnly Property PrimaryCauseId() As Integer
        Set(ByVal Value As Integer)
            m_lPrimaryCauseId = Value
        End Set
    End Property

    Public WriteOnly Property Description() As String
        Set(ByVal Value As String)
            m_sDescription = Value
        End Set
    End Property

    Public WriteOnly Property UniqueId() As String
        Set(ByVal Value As String)
            m_sUniqueId = Value
        End Set
    End Property

    Public WriteOnly Property ScreenHierarchy() As String
        Set(ByVal Value As String)
            m_sScreenHierarchy = Value
        End Set
    End Property

    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)
    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Private Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign all of the controls to
            ' PMFormControl
            '
            ' Example:-
            '
            '        ' Pass control and required settings to FormControl
            '        m_lReturn = m_oFormFields.AddNewFormField( _
            ''                       ctlControl:=<Control Name>, _
            ''                       lFieldType:=<PM field type>, _
            ''                       lFormat:=<PM format string>, _
            ''                       lMandatory:=<PMNonMandatory or PMNonMandatory)
            '
            '        'Error checking
            '        If m_lReturn <> PMTrue Then
            '          SetFieldValidation = PMFalse
            '          Exit Function
            '        End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtFormatDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DataToInterface
    '
    ' Description: Updates all interface details from the search data.
    '              storage.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (DataToInterface) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function DataToInterface() As Integer
    '
    'Dim result As Integer = 0
    'Dim oListItem As ListViewItem
    'Dim lRow As Integer
    'Dim bMatch As Boolean
    'Dim cThisPerc As Decimal
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'txtPrimCause.Text = m_sDescription
    '
    ' Update the interface details.
    '
    ' Enable the interface now that the search
    ' has completed.
    'm_lReturn = CType(DisableInterface(bDisable:=False), gPMConstants.PMEReturnCode)
    '
    ' Check for errors
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    ' Failed to get details.
    'result = gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'cmdOK.Enabled = True
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result


    '
    'Return result
    'End Try
    'End Function


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

            'Tell it what we're getting

            m_oBusiness.PrimaryCauseId = m_lPrimaryCauseId


            m_lReturn = m_oBusiness.GetPrimCauseRiskTypeGrp(r_vPrimCauseRiskTypeGrp:=m_vPrimCauseRiskTypeGrp)

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

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign the all of the interface
            ' details from the business object, using the FormatField
            ' function for any type conversion.
            '
            ' Example:-
            '
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtName, vControlValue:=m_sName$)
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=optChoice, vControlValue:=m_iDChoice%)
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtDate, vControlValue:=m_dtDDate)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************


            If Information.IsArray(m_vPrimCauseRiskTypeGrp) Then

                For lTemp As Integer = m_vPrimCauseRiskTypeGrp.GetLowerBound(1) To m_vPrimCauseRiskTypeGrp.GetUpperBound(1)
                    'For lTemp As Integer = m_vPrimCauseRiskTypeGrp.GetLowerBound(0) To m_vPrimCauseRiskTypeGrp.GetUpperBound(0)

                    ' find the item in the list view and check it
                    For Each oListItem As ListViewItem In lvwPrimCauseRiskType.Items

                        If CInt(m_vPrimCauseRiskTypeGrp(ACTPRiskTypeGroupId, lTemp)) = Convert.ToString(oListItem.Tag) Then
                            oListItem.Checked = True
                            Exit For
                        End If
                    Next oListItem

                Next lTemp

            End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




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
        Dim bDifferent As Boolean
        Dim sDescription As String = ""
        Dim vDescription As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the business object.
            m_lReturn = CType(InterfaceToData(), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oBusiness.PrimaryCauseId = m_lPrimaryCauseId


            m_lReturn = m_oBusiness.Update(v_vPrimCauseRiskTypeGrp:=m_vPrimCauseRiskTypeGrp, v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=m_sScreenHierarchy)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
            End If

            Return result

        Catch excep As System.Exception




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
    'UPGRADE_NOTE: (7001) The following declaration (BusinessToData) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function BusinessToData() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    '
    ' Assign the details to the data storage.
    '
    ' {* USER DEFINED CODE (Begin) *}
    '
    ' {* USER DEFINED CODE (End) *}
    '
    'Return gPMConstants.PMEReturnCode.PMTrue
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: InterfaceToData
    '
    ' Description: Updates the data storage from the interface details.
    '
    ' ***************************************************************** '
    Private Function InterfaceToData() As Integer

        Dim result As Integer = 0
        Dim lTemp As Integer
        Dim lLoopy As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}


            m_vPrimCauseRiskTypeGrp = ""

            ReDim m_vPrimCauseRiskTypeGrp(ACTPRiskTypeGroupId, 0)

            ' find the checked items and stick them in the array
            For Each oListItem As ListViewItem In lvwPrimCauseRiskType.Items
                If oListItem.Checked Then
                    ReDim Preserve m_vPrimCauseRiskTypeGrp(ACTPRiskTypeGroupId, lLoopy)

                    m_vPrimCauseRiskTypeGrp(ACTPPrimaryCauseId, lLoopy) = m_lPrimaryCauseId


                    m_vPrimCauseRiskTypeGrp(ACTPRiskTypeGroupId, lLoopy) = Convert.ToString(oListItem.Tag)
                    lLoopy += 1
                End If
            Next oListItem

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



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

            tabMainTab.Top = VB6.TwipsToPixelsY(120)

            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' get all risk type group

            m_lReturn = m_oBusiness.GetAllRiskTypeGroup(m_vRiskTypeGroup)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If LoadRiskTypeGroup() <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            If (lvwPrimCauseRiskType.Items.Count > 0) And m_iTask <> gPMConstants.PMEComponentAction.pmview Then
                cmdSelect.Enabled = True
            Else
                cmdSelect.Enabled = False
                lvwPrimCauseRiskType.HideSelection = True
                lvwPrimCauseRiskType.Enabled = False
            End If

            ' set the primary cause description
            txtPrimCause.Text = m_sDescription

            ' Set the column widths for the search list.
            lvwPrimCauseRiskType.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(1900))
            lvwPrimCauseRiskType.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(3900))
            lvwPrimCauseRiskType.Columns.Item(2).Width = CInt(VB6.TwipsToPixelsX(2500))

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
    '
    ' Name: LoadRiskTypeGroup
    '
    ' Description: load listview with data from data storage
    '
    ' ***************************************************************** '
    Private Function LoadRiskTypeGroup() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem

        Const ACIRiskTypeGroupId As Integer = 0
        Const ACICode As Integer = 1
        Const ACIDescription As Integer = 2
        Const ACIEffectiveDate As Integer = 3

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lvwPrimCauseRiskType.Items.Clear()

            If Information.IsArray(m_vRiskTypeGroup) Then
                For lRow As Integer = m_vRiskTypeGroup.GetLowerBound(1) To m_vRiskTypeGroup.GetUpperBound(1)

                    'column 1 Code
                    oListItem = lvwPrimCauseRiskType.Items.Add(CStr(m_vRiskTypeGroup(ACICode, lRow)))

                    'column 2 Description
                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vRiskTypeGroup(ACIDescription, lRow))

                    'column 3 effective date
                    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtFormatDate, vControlValue:=m_vRiskTypeGroup(ACIEffectiveDate, lRow))

                    ' Check for errors
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Failed to assign the data.
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = txtFormatDate.Text

                    oListItem.Tag = CStr(m_vRiskTypeGroup(ACIRiskTypeGroupId, lRow))

                    ' Refresh the first X amount of rows, to
                    ' allow the user to see the results instantly.
                    If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                        ' Select the first item.
                        lvwPrimCauseRiskType.Items.Item(0).Selected = True

                        ' Refresh the initial results.
                        lvwPrimCauseRiskType.Refresh()
                    End If

                Next
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadRiskTypeGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadRiskTypeGroup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UnloadRiskTypeGroup
    '
    ' Description: loop through listview and store selected item to array
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (UnloadRiskTypeGroup) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function UnloadRiskTypeGroup() As Integer
    '
    'Dim result As Integer = 0
    'Dim lPos As Integer
    'Dim bFirstTime As Boolean
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'bFirstTime = True
    'lPos = 0
    'm_vLinkedRiskTypeGroup = VB6.CopyArray(Nothing)
    '
    'For 'lCount As Integer = 1 To lvwPrimCauseRiskType.Items.Count
    '
    'lvwPrimCauseRiskType.Items.Item(lCount - 1).Selected = True
    '
    'check to see if user has selected this record as allowed GIS Screen
    'lPos = (CStr(Convert.ToString(lvwPrimCauseRiskType.Items.Item(lCount - 1).Tag)).IndexOf("checked") + 1)
    '
    'If lPos <> 0 Then
    '
    'If bFirstTime Then
    'bFirstTime = False
    ''ReDim m_vLinkedRiskTypeGroup(0, 0)
    'Else
    ''ReDim Preserve m_vLinkedRiskTypeGroup(0, m_vLinkedRiskTypeGroup.GetUpperBound(1) + 1)
    'End If
    '
    'm_vLinkedRiskTypeGroup(0, m_vLinkedRiskTypeGroup.GetUpperBound(1)) = m_vRiskTypeGroup(0, CInt(Convert.ToString(lvwPrimCauseRiskType.Items.Item(lvwPrimCauseRiskType.FocusedItem.Index).Tag).Substring(0, lPos - 1)))
    '
    '
    'End If
    'Next 
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnloadRiskTypeGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnloadRiskTypeGroup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function




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


            'Developer Guide No. 243
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


            'Developer Guide No. 243
            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 243
            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 243
            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 243
            cmdSelect.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSelectButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 243
            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 243
            lblPrimCause.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCPrimaryCause, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            'Developer Guide No. 243
            lvwPrimCauseRiskType.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            'Developer Guide No. 243
            lvwPrimCauseRiskType.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHDescription, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            'Developer Guide No. 243
            lvwPrimCauseRiskType.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHEffectiveDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        ' Fire up the help screen
        'Developer Guide No. 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = CType(PMHelpFunc.ShowHelp(cmdHelp, MainModule.ScreenHelpID), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub cmdSelect_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSelect.Click


        ' loop through the listview items, checking them if they're selected
        For Each itmCurrItem As ListViewItem In lvwPrimCauseRiskType.Items



            If itmCurrItem.Selected Then

                itmCurrItem.Checked = Not itmCurrItem.Checked

            End If

            'lvwPrimCauseRiskType_ItemCheck(lvwPrimCauseRiskType, New ItemCheckEventArgs(itmCurrItem.Index, CheckState.Indeterminate, CheckState.Indeterminate))
        Next itmCurrItem

    End Sub


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
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPrimCauseRiskType.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                'Developer Guide No. 243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                'Developer Guide No. 243
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            'JMK 19/10/2001

            m_sUnderwritingType = m_oBusiness.UnderwritingType

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMUPrimCauseRiskType.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




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

            m_oBusiness.PrimaryCauseId = m_lPrimaryCauseId
            ' {* USER DEFINED CODE (End) *}

            ' Validate fields using Forms Control
            m_lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

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

            ' Gets the interface details to be displayed.
            m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)

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
                'Process the next set of actions depending
                'upon the interface task etc.
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                'Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.cancel = True
                    'Set the mouse pointer to normal.
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

            ' Terminate the form control object.
            m_oFormFields.Dispose()
            ' Destroy the instance of the form control object
            ' from memory.
            m_oFormFields = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: DisableInterface
    '
    ' Description: Disables parts of the interface while a search is
    '              in progress.
    '
    ' ***************************************************************** '
    Private Function DisableInterface(ByRef bDisable As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cmdOK.Enabled = Not bDisable
            'cmdEdit.Enabled = Not bDisable

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim Msg As String = ""
        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




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
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwPrimCauseRiskType_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwPrimCauseRiskType.Click

        If Not (lvwPrimCauseRiskType.FocusedItem Is Nothing) Then
            'Developer Guide No. 185
            lvwPrimCauseRiskType_ItemClick(lvwPrimCauseRiskType.SelectedItems.Item(0))
        End If

    End Sub

    Private Sub lvwPrimCauseRiskType_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwPrimCauseRiskType.ColumnClick
        Dim colColumnHeader As ColumnHeader = lvwPrimCauseRiskType.Columns(eventArgs.Column)
        ' Column click event for the search details

        Try

            With lvwPrimCauseRiskType
                ' If current sort column header is
                ' pressed.
                If colColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwPrimCauseRiskType) Then
                    ' Set sort order opposite of
                    ' current direction.
                    ListViewHelper.SetSortOrderProperty(lvwPrimCauseRiskType, (ListViewHelper.GetSortOrderProperty(lvwPrimCauseRiskType) + 1) Mod 2)
                Else
                    ' Sort by this column (ascending).
                    ListViewHelper.SetSortedProperty(lvwPrimCauseRiskType, False)

                    ' Turn off sorting so that the list
                    ' is not sorted twice
                    ListViewHelper.SetSortOrderProperty(lvwPrimCauseRiskType, SortOrder.Ascending)
                    ListViewHelper.SetSortKeyProperty(lvwPrimCauseRiskType, colColumnHeader.Index + 1 - 1)
                    ListViewHelper.SetSortedProperty(lvwPrimCauseRiskType, True)
                End If
            End With

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwPrimCauseRiskType_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub



    Private Sub lvwPrimCauseRiskType_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwPrimCauseRiskType.DoubleClick

        If Not (lvwPrimCauseRiskType.FocusedItem Is Nothing) Then
            lvwPrimCauseRiskType.FocusedItem.Checked = Not (lvwPrimCauseRiskType.FocusedItem.Checked)
            'Developer Guide No. 185
            lvwPrimCauseRiskType_ItemClick(lvwPrimCauseRiskType.SelectedItems.Item(0))
        End If

    End Sub

    Private Sub lvwPrimCauseRiskType_ItemCheck(ByVal eventSender As Object, ByVal eventArgs As ItemCheckEventArgs) Handles lvwPrimCauseRiskType.ItemCheck
        Dim Item As ListViewItem = lvwPrimCauseRiskType.Items(eventArgs.Index)
        'Developer Guide No. 185
        If Item.Checked Then
            cmdSelect.Text = "&Select"
        Else
            cmdSelect.Text = "&Unselect"
            
        End If
       
    End Sub

    Private Sub lvwPrimCauseRiskType_ItemClick(ByVal Item As ListViewItem)

        ' check the items to see what has been selected, and which are checked
        ' if all are checked, set button caption to 'Unselect'
        ' if none are checked, set button caption to 'Select'
        ' if it's a mix, disable the button
        '
        ' AMB 07/01/03
        '

        Dim lCheckNoCount, lCheckYesCount, lSelectCount As Integer

        Try

            lCheckYesCount = 0
            lCheckNoCount = 0
            lSelectCount = 0

            For Each itmCurrItem As ListViewItem In lvwPrimCauseRiskType.Items

                If itmCurrItem.Selected Then
                    lSelectCount += 1
                    If itmCurrItem.Checked Then
                        lCheckYesCount += 1
                    Else
                        lCheckNoCount += 1
                    End If
                End If

            Next itmCurrItem

            If (lSelectCount = lCheckYesCount) Or (lSelectCount = lCheckNoCount) Then
                ' all selected ones are the same; checked or unchecked

                cmdSelect.Enabled = True

                If lSelectCount = lCheckYesCount Then
                    ' set caption to 'Unselect'

                    'Developer Guide No. 243
                    cmdSelect.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACUnselectButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ElseIf lSelectCount = lCheckNoCount Then
                    ' set caption to 'Select'

                    'Developer Guide No. 243
                    cmdSelect.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSelectButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                End If

            Else
                cmdSelect.Enabled = False
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to select the item", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwPrimCauseRiskType_ItemClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If
    End Sub
End Class
