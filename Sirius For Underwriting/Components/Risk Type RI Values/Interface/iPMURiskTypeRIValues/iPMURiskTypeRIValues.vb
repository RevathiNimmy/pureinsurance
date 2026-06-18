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
    'Developer Guide No.243(Replaced iPMFunc.GetResData with GetResData in the whole document)

    'Developer Guide No.7
    Public Const vbFormCode As Integer = 0
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

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lRiskTypeId As Integer
    Private m_sCode As String = ""
    Private m_sDescription As String = ""
    Private m_lItemsFound As Integer

    'Variables to store data taken from the List View
    Private m_iAction As Integer
    Private m_lRiskTypeRIValuesId As Integer
    Private m_lGISObjectId As Integer
    Private m_lGISPropertyId As Integer
    Private m_sGISObjectName As String = ""
    Private m_sGISPropertyName As String = ""

    Private m_sGISHeaderInd1 As String = ""
    Private m_lGISHeaderIndId1 As Integer
    Private m_sGISHeaderInd2 As String = ""
    Private m_lGISHeaderIndId2 As Integer
    Private m_sGISHeaderInd3 As String = ""
    Private m_lGISHeaderIndId3 As Integer

    Private m_lMaxLevel As Integer
    Private m_lThisLevel As Integer

    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMURiskTypeRIValues.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object
    'Private m_oBusiness As bSIRRiskTypeRIValues.Business

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_bChanged As Boolean

    Private m_lLevel As Integer

    'Developer Guide No : 88
    Private m_oRIValues As Interface_Renamed

    ' Control array to store the first and last
    ' text box controls for each tab.
    ' Stores the search data from the business object.
    Private m_vRiskTypeRIValues(,) As Object

    'JMK 23/10/2001 display Insurer/Reinsurer
    Private m_sUnderwritingType As String = ""
    Private m_nRiskTypeLimitVersionId As Integer
    Private m_nRIModelUpperLimit As Long
    Private m_sUniqueId As String = ""
    Private m_sScreenHierarchy As String = ""
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
    Public WriteOnly Property RiskTypeId() As Integer
        Set(ByVal Value As Integer)
            m_lRiskTypeId = Value
        End Set
    End Property

    Public WriteOnly Property Description() As String
        Set(ByVal Value As String)
            m_sDescription = Value
        End Set
    End Property

    Public Property GISHeaderInd1() As String
        Get
            Return m_sGISHeaderInd1
        End Get
        Set(ByVal Value As String)
            m_sGISHeaderInd1 = Value
        End Set
    End Property

    Public Property GISHeaderIndId1() As Integer
        Get
            Return m_lGISHeaderIndId1
        End Get
        Set(ByVal Value As Integer)
            m_lGISHeaderIndId1 = Value
        End Set
    End Property

    Public Property GISHeaderInd2() As String
        Get
            Return m_sGISHeaderInd2
        End Get
        Set(ByVal Value As String)
            m_sGISHeaderInd2 = Value
        End Set
    End Property

    Public Property GISHeaderIndId2() As Integer
        Get
            Return m_lGISHeaderIndId2
        End Get
        Set(ByVal Value As Integer)
            m_lGISHeaderIndId2 = Value
        End Set
    End Property

    Public Property GISHeaderInd3() As String
        Get
            Return m_sGISHeaderInd3
        End Get
        Set(ByVal Value As String)
            m_sGISHeaderInd3 = Value
        End Set
    End Property

    Public Property GISHeaderIndId3() As Integer
        Get
            Return m_lGISHeaderIndId3
        End Get
        Set(ByVal Value As Integer)
            m_lGISHeaderIndId3 = Value
        End Set
    End Property

    Public WriteOnly Property MaxLevel() As Integer
        Set(ByVal Value As Integer)
            m_lMaxLevel = Value
        End Set
    End Property

    Public WriteOnly Property ThisLevel() As Integer
        Set(ByVal Value As Integer)
            m_lThisLevel = Value
        End Set
    End Property

    Public Property RiskTypeLimitVersionId() As Integer
        Get
            Return m_nRiskTypeLimitVersionId
        End Get
        Set(value As Integer)
            m_nRiskTypeLimitVersionId = value
        End Set
    End Property

    Public Property UniqueId() As String
        Get
            Return m_sUniqueId
        End Get
        Set(ByVal Value As String)
            m_sUniqueId = Value
        End Set
    End Property

    Public Property ScreenHierarchy() As String
        Get
            Return m_sScreenHierarchy
        End Get
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

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtValue, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (End) *}

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
    'Dim sTemp As String = ""
    '
    'Const ACFindImage As String = "FindImage"
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Update the interface details.
    '
    ' Clear the search details.
    'lvwRiskTypeRIValues.Items.Clear()
    '
    'm_lItemsFound = 0
    '
    'If Not Information.IsArray(m_vRiskTypeRIValues) Then
    'Return result
    'End If
    '
    ' Assign the details to the interface.
    '
    'For 'lRow As Integer = m_vRiskTypeRIValues.GetLowerBound(1) To m_vRiskTypeRIValues.GetUpperBound(1)
    ' {* USER DEFINED CODE (Begin) *}
    'm_lItemsFound += 1
    ' Assign the details to the first column.
    ' Column 1 indicator combination
    '
    '        sTemp = m_sGISHeaderInd1 & _
    'm_sGISHeaderInd2 & _
    'm_sGISHeaderInd3 & _
    'm_vRiskTypeRIValues(ACVGISHeaderInd1, lRow) & _
    'm_vRiskTypeRIValues(ACVGISHeaderInd2, lRow) & _
    'm_vRiskTypeRIValues(ACVGISHeaderInd3, lRow)
    '

    'oListItem = lvwRiskTypeRIValues.Items.Add(sTemp, "")
    '
    ' Assign details to the other columns
    '
    'Column 2 value
    '
    'm_lReturn = m_oFormFields.FormatControl(ctlControl:=txtValue, vControlValue:=m_vRiskTypeRIValues(ACVValue, lRow))
    '
    'ListViewHelper.GetListViewSubItem(oListItem, 1).Text = txtValue.Text
    '
    ' {* USER DEFINED CODE (End) *}
    '
    ' Set the tag property with the index of
    ' the search data storage.
    'oListItem.Tag = CStr(lRow)
    '
    ' Refresh the first X amount of rows, to
    ' allow the user to see the results instantly.
    'If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
    ' Select the first item.
    'lvwRiskTypeRIValues.Items.Item(0).Selected = True
    '
    ' Refresh the initial results.
    'lvwRiskTypeRIValues.Refresh()
    'End If
    '
    'Next lRow
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
    'cmdValues.Enabled = False
    'cmdEdit.Enabled = False
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
    ' Name: DataRefresh
    '
    ' Description: Populate Risk Type RI Values - Refresh
    '              storage.
    '
    ' ***************************************************************** '
    Private Function DataRefresh() As Integer

        Dim result As Integer = 0
        Dim lSelectedItem As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the property members.

            ' {* USER DEFINED CODE (Begin) *}


            lSelectedItem = Convert.ToString(lvwRiskTypeRIValues.Items.Item(lvwRiskTypeRIValues.FocusedItem.Index).Tag)
            'Developer Guide No.52
            lvwRiskTypeRIValues.FocusedItem.SubItems(1).Text = txtValue.Text


            m_vRiskTypeRIValues(2, lSelectedItem) = m_oFormFields.UnformatControl(ctlControl:=txtValue)

            '    m_lReturn = BusinessToInterface()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the Refresh Refreshs from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataRefresh", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

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

            m_oBusiness.RiskTypeId = m_lRiskTypeId

            m_oBusiness.GISHeaderIndId1 = m_lGISHeaderIndId1

            m_oBusiness.GISHeaderIndId2 = m_lGISHeaderIndId2

            m_oBusiness.GISHeaderIndId3 = m_lGISHeaderIndId3

            m_oBusiness.RiskTypeLimitVersionId = m_nRiskTypeLimitVersionId

            m_lReturn = m_oBusiness.GetRiskTypeRIValues(r_vRiskTypeRIValues:=m_vRiskTypeRIValues)

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return result
            End If

            m_bChanged = False

            m_lReturn = m_oBusiness.GetRIModelUpperLimit(r_nRIModelUpperLimit:=m_nRIModelUpperLimit)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return result
            End If
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
        Dim oListItem As ListViewItem
        Dim sTemp As String = ""
        Static bAlreadyRun As Boolean

        Const ACFindImage As String = "FindImage"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Assign the details from the business object
            ' to the data storage.
            m_lReturn = CType(BusinessToData(), gPMConstants.PMEReturnCode)

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
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtName, vControlValue:=m_sName$)
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=optChoice, vControlValue:=m_iDChoice%)
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtDate, vControlValue:=m_dtDDate)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************


            'Developer Guide No. 26
            lblpRiskType.Text = m_sDescription

            lvwRiskTypeRIValues.Items.Clear()

            If Information.IsArray(m_vRiskTypeRIValues) Then

                For lTemp As Integer = m_vRiskTypeRIValues.GetLowerBound(1) To m_vRiskTypeRIValues.GetUpperBound(1)

                    sTemp = m_sGISHeaderInd1 & _
                            m_sGISHeaderInd2 & _
                            m_sGISHeaderInd3 & _
                            CStr(m_vRiskTypeRIValues(1, lTemp)).Trim()


                    'Developer Guide No.49
                    oListItem = lvwRiskTypeRIValues.Items.Add(sTemp, ACFindImage)


                    If Convert.IsDBNull(m_vRiskTypeRIValues(2, lTemp)) Or IsNothing(m_vRiskTypeRIValues(2, lTemp)) Then
                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = ""
                    Else
                        m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtValue, vControlValue:=m_vRiskTypeRIValues(2, lTemp))

                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = txtValue.Text
                    End If

                    oListItem.Tag = CStr(lTemp)
                Next lTemp

            End If

            If bAlreadyRun Then
                Return result
            End If

            bAlreadyRun = True

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



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

        Dim result As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the business object.
            m_lReturn = CType(InterfaceToData(), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oBusiness.RiskTypeId = m_lRiskTypeId

            m_oBusiness.RiskTypeLimitVersionId = m_nRiskTypeLimitVersionId

            m_sScreenHierarchy = m_sScreenHierarchy
            m_lReturn = m_oBusiness.Update(v_vRiskTypeRIValues:=m_vRiskTypeRIValues, v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=m_sScreenHierarchy)

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
    Private Function BusinessToData() As Integer

        Dim result As Integer = 0
        Try


            ' Assign the details to the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




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


            ' {* USER DEFINED CODE (Begin) *}

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

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
            'Display the ListView Tab

            tabDetailTab.Visible = False
            tabDetailTab.Top = VB6.TwipsToPixelsY(120)
            tabMainTab.Top = VB6.TwipsToPixelsY(120)

            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the status of the Navigate button.

            m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}
            m_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwRiskTypeRIValues.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the column widths for the search list.
            lvwRiskTypeRIValues.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX((VB6.PixelsToTwipsX(lvwRiskTypeRIValues.Width) / 2) - 360))
            lvwRiskTypeRIValues.Columns.Item(1).Width = CInt(lvwRiskTypeRIValues.Columns.Item(0).Width)

            cmdEdit.Enabled = False

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


            ' Initialise the control array with the number of
            ' tabs which contain data entry fields on (Remember
            ' that arrays start from zero, therefore you must
            ' subtract one from the number of tabs).


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

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




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


            cmdEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            SSTabHelper.SetTabCaption(tabDetailTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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



            lblRiskType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCRiskType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwRiskTypeRIValues.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHBreakdown, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwRiskTypeRIValues.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHValue, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            '    lblGISObject.Caption = iPMFunc.GetResData( _
            'iLangID:=g_iLanguageID%, _
            'lID:=ACCObject, _
            'iDataType:=PMResString)


            lblValue.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCValue, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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
    '
    ' Name: CallValues
    '
    ' Description:
    '
    ' History: 30/06/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function CallValues() As Integer

        Dim result As Integer = 0
        Dim lTag As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Create address component if not already done so
            If m_oRIValues Is Nothing Then

                ' Get an instance of the contact interface object via
                ' the public object manager.
                Dim temp_m_oRIValues As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oRIValues, sClassName:="iPMURiskTypeRIValues.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oRIValues = temp_m_oRIValues

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get RI Values component", vApp:=ACApp, vClass:=ACClass, vMethod:="CallValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result

                End If

            End If


            lTag = Convert.ToString(lvwRiskTypeRIValues.FocusedItem.Tag)

            m_oRIValues.RiskTypeId = m_lRiskTypeId
            m_oRIValues.Description = m_sDescription
            m_oRIValues.RiskTypeLimitVersionId = m_nRiskTypeLimitVersionId

            m_oRIValues.GISHeaderInd1 = m_sGISHeaderInd1
            m_oRIValues.GISHeaderInd2 = m_sGISHeaderInd2
            m_oRIValues.GISHeaderInd3 = m_sGISHeaderInd3

            m_oRIValues.GISHeaderIndId1 = m_lGISHeaderIndId1
            m_oRIValues.GISHeaderIndId2 = m_lGISHeaderIndId2
            m_oRIValues.GISHeaderIndId3 = m_lGISHeaderIndId3

            m_oRIValues.MaxLevel = m_lMaxLevel
            m_oRIValues.ThisLevel = m_lThisLevel + 1

            m_lReturn = GetLevel()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Select Case m_lLevel
                Case 1
                    m_oRIValues.GISHeaderIndId1 = CInt(m_vRiskTypeRIValues(0, lTag))
                    m_oRIValues.GISHeaderInd1 = CStr(m_vRiskTypeRIValues(1, lTag)).Trim()
                Case 2
                    m_oRIValues.GISHeaderIndId2 = CInt(m_vRiskTypeRIValues(0, lTag))
                    m_oRIValues.GISHeaderInd2 = CStr(m_vRiskTypeRIValues(1, lTag)).Trim()
                Case 3
                    m_oRIValues.GISHeaderIndId3 = CInt(m_vRiskTypeRIValues(0, lTag))
                    m_oRIValues.GISHeaderInd3 = CStr(m_vRiskTypeRIValues(1, lTag)).Trim()
            End Select

            '    m_oRIValues.GISHeaderInd1 = m_vRiskTypeRIValues(ACVGISHeaderId1, lTag)
            '    m_oRIValues.GISHeaderIndId1 = m_vRiskTypeRIValues(ACVGISHeaderIndId1, lTag)

            '    m_oRIValues.GISHeaderInd2 = m_vRiskTypeRIValues(ACVGISHeaderId2, lTag)
            '    m_oRIValues.GISHeaderIndId2 = m_vRiskTypeRIValues(ACVGISHeaderIndId2, lTag)

            '    m_oRIValues.GISHeaderInd3 = m_vRiskTypeRIValues(ACVGISHeaderId3, lTag)
            '    m_oRIValues.GISHeaderIndId3 = m_vRiskTypeRIValues(ACVGISHeaderIndId3, lTag)

            m_lReturn = CType(m_oRIValues.Start(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If m_oRIValues.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CallValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CallValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetLevel
    '
    ' Description:
    '
    ' History: 28/02/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function GetLevel() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'We should never, ever, have this level...
            m_lLevel = 4

            If m_lGISHeaderIndId3 = 0 Then
                m_lLevel = 3
            End If

            If m_lGISHeaderIndId2 = 0 Then
                m_lLevel = 2
            End If

            If m_lGISHeaderIndId1 = 0 Then
                m_lLevel = 1
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLevel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLevel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PRIVATE Methods (End)

    ' PRIVATE Events (Begin)

    Private Sub cmdDetailCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDetailCancel.Click

        tabDetailTab.Visible = False
        tabMainTab.Visible = True
        cmdCancel.Enabled = True

        cmdDetailCancel.Enabled = False
        cmdDetailOK.Enabled = False

        cmdOK.Enabled = True

    End Sub

    Private Sub cmdDetailOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDetailOK.Click

        tabDetailTab.Visible = False
        tabMainTab.Visible = True
        cmdCancel.Enabled = True
        cmdOK.Enabled = True

        cmdValues.Enabled = False
        cmdDetailCancel.Enabled = False
        cmdDetailOK.Enabled = False

        m_lReturn = CType(DataRefresh(), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        cmdOK.Enabled = False
        cmdCancel.Enabled = False

        tabMainTab.Visible = False
        tabDetailTab.Top = VB6.TwipsToPixelsY(120)
        tabDetailTab.Visible = True

        txtValue.Enabled = True

        cmdDetailOK.Enabled = True
        cmdDetailCancel.Enabled = True

        cmdValues.Enabled = False
        'Developer Guide No. 52
        txtValue.Text = lvwRiskTypeRIValues.FocusedItem.SubItems(1).Text

    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        ' Fire up the help screen
        'Developer Guide No. 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = CType(PMHelpFunc.ShowHelp(cmdHelp, lContextID:=MainModule.ScreenHelpID), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub cmdValues_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdValues.Click

        m_lReturn = CType(CallValues(), gPMConstants.PMEReturnCode)

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
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRRiskTypeRIValues.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMURiskTypeRIValues.General()

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

            m_oBusiness.RiskTypeId = m_lRiskTypeId
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
                '	 Process the next set of actions depending
                '	 upon the interface task etc.
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                '			 Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    '					 Do not procced with the interface termination.
                    'Developer Guide No.7
                    eventArgs.Cancel = True
                    Cancel = 1

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

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayStatusSearching
    '
    ' Description: Display the status searching message.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (DisplayStatusSearching) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub DisplayStatusSearching()
    '
    'Static sMessage As String = ""
    '
    'Try 
    '
    ' Get message text if not already present.
    'If sMessage = "" Then

    'sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
    'End If
    '
    ' Display the status message.
    '    stbStatus.SimpleText = " " & sMessage$
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    'End Sub

    ' ***************************************************************** '
    ' Name: DisplayStatusFound
    '
    ' Description: Display the status found message.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (DisplayStatusFound) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub DisplayStatusFound()
    '
    'Static sMessage As String = ""
    'Dim lItemsFound As Integer
    '
    'Try 
    '
    '
    ' Get message text if not already present.
    'If sMessage = "" Then

    'sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
    'End If
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusFound", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    '
    'End Sub

    Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Dim iCtrlDown As Integer

        Const ACCtrlMask As Integer = 2

        Try

            ' Set the control key value.
            iCtrlDown = (Shift And ACCtrlMask) > 0

            With tabDetailTab
                ' Check the key pressed.
                Select Case KeyCode
                    Case Keys.PageUp
                        ' Page Up key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the first tab.
                            SSTabHelper.SetSelectedIndex(tabDetailTab, 0)
                        Else
                            ' Check we are not on the
                            ' first tab.
                            If SSTabHelper.GetSelectedIndex(tabDetailTab) > 0 Then
                                ' Display the previous tab.
                                SSTabHelper.SetSelectedIndex(tabDetailTab, SSTabHelper.GetSelectedIndex(tabDetailTab) - 1)
                            End If
                        End If

                    Case Keys.PageDown
                        ' Page Down key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the last tab.
                            SSTabHelper.SetSelectedIndex(tabDetailTab, SSTabHelper.GetTabCount(tabDetailTab) - 1)
                        Else
                            ' Check we are not on the
                            ' last tab.
                            If SSTabHelper.GetSelectedIndex(tabDetailTab) < (SSTabHelper.GetTabCount(tabDetailTab) - 1) Then
                                ' Display the next tab.
                                SSTabHelper.SetSelectedIndex(tabDetailTab, SSTabHelper.GetSelectedIndex(tabDetailTab) + 1)
                            End If
                        End If

                    Case Keys.Home
                        ' Home key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                        End If

                    Case Keys.End
                        ' End key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                        End If
                End Select
            End With
            'Developer Guide No 293
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
        Catch




            Exit Sub
        End Try


    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim Msg As String = ""
        ' Click event of the OK button.
        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK
            If Information.IsArray(m_vRiskTypeRIValues) AndAlso m_nRIModelUpperLimit > 0 Then
                For nRow As Integer = m_vRiskTypeRIValues.GetLowerBound(1) To m_vRiskTypeRIValues.GetUpperBound(1)
                    If m_nRIModelUpperLimit < m_vRiskTypeRIValues(2, nRow) Then
                        MessageBox.Show("Limit value can't be more than QuotaShare or Surplus upper limit", "RiskTypeRIValues", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Exit Sub
                    End If
                Next nRow
            End If
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

    'UPGRADE_NOTE: (7001) The following declaration (cmdNavigate_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub cmdNavigate_Click()
    '
    ' Click event of the Navigate button.
    '
    'Try 
    '
    ' Set the interface status.
    'm_lStatus = gPMConstants.PMEReturnCode.PMNavigate
    '
    ' Process the next set of actions depending
    ' upon the interface task etc.
    'm_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
    '
    ' Check the return value.
    'If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
    ' Everything OK, so we can hide the interface.
    'Me.Hide()
    'End If
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    'End Sub

    Private Sub lvwRiskTypeRIValues_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwRiskTypeRIValues.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No.40
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        'end

        If Task <> gPMConstants.PMEComponentAction.PMView Then
            If lvwRiskTypeRIValues.GetItemAt(x, y) Is Nothing Then
                cmdEdit.Enabled = False
                cmdValues.Enabled = False
            Else
                cmdEdit.Enabled = True
                If m_lThisLevel < m_lMaxLevel Then
                    cmdValues.Enabled = True
                End If
            End If
        End If

    End Sub

    Private Sub txtValue_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtValue.Enter

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtValue)

    End Sub

    Private Sub txtValue_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtValue.Leave

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtValue)

    End Sub

    ' PRIVATE Events (End)
End Class
