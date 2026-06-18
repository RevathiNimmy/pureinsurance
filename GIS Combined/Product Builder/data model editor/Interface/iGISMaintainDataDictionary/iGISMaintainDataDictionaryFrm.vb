Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Artinsoft.VB6.VB
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 10/05/1999
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' RKS 27/04/2005 354-Standard Wording Control Enchancements
    ' CJB 21/06/2005 PN20948 Changed LockDataModel to return PMMAlreadyInUse when rec locked &
    '                Form_Load to pass the value back too...
    ' CJB 28/06/2005 PN21998 In CheckObjects ensure that deleted properties are not included in checks!
    ' PW120406 - PN28024 - Add AccessibleViaSAM checkbox.
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As Integer

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_sStepStatus As String = ""

    ' {* USER DEFINED CODE (Begin) *}

    Private m_vGISObject(,) As Object 'these are the original risk objects
    Private m_vGISProperty() As Object 'these are the original risk propertys
    Private m_vPartyObject As Object
    Private m_vPartyProperty As Object
    Private m_vClaimObject As Object
    Private m_vCLaimProperty As Object
    Private m_vPolicyObject As Object
    Private m_vPOlicyProperty As Object

    Private m_vPartyTypeArray(,) As Object
    Private m_vSumInsuredTypeArray(,) As Object
    Private m_vDocumentFilterArray As Object
    Private m_vPMLookupList As Object
    Private m_vGISUserDefHeaderArray(,) As Object
    Private m_vProductArray(,) As Object
    Private m_vIndexLinking As Object

    Private m_lGISDataModelId As Integer
    Private m_sGISDataModel As String = ""
    Private m_sGISDataModelName As String = ""
    Private m_sGISDataModelDescription As String = ""
    Private m_lGISDataModelType As Integer
    Private m_lObjectTag As Integer

    Private m_bSomethingChanged As Boolean

    Private m_bAccessibleViaSAMOption As Boolean 'PN28024
    Private m_sBOMRequired As String = "" 'PN28024
    Private m_sDefaultBOMRequired As String = "" 'PN28024

    Private Const ACBOMRequiredAOL As String = "AOL" 'PN28024
    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the Business object.

    Private m_oBusiness As bGISMaintainDataDictionary.Business
    Private m_oBusinessGlobalTransaction As Object

    'Private m_oBusiness As bGISMaintainDataDictionary.Business

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iGISMaintainDataDictionary.General


    Private m_oGISObject As iGISObject.Interface_Renamed
    'Private m_oGISObject As iGISObject.Interface


    Private m_oGISProperty As iGISProperty.Interface_Renamed
    'Private m_oGISProperty As iGISProperty.Interface

    Private m_lSwiftIntegration As Integer

    ' Stores the return value for the a
    ' function call.
    Private m_nReturn As Integer

    Private m_iSourceId As Integer

    Private m_bMoving As Boolean
    Private m_lMOuseOffset As Integer

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control
    Private m_bCanEditObject As Boolean
    ' developer guide no. 7
    Private Const vbFormCode As Integer = 0
    ' Stores the details from the business object.
    Private m_bIsMarketplaceDM As Boolean = False
    Private m_bIsImportedMarketplaceDM As Boolean = False
    Private m_sUniqueId As String
    Private m_sScreenHierarchy As String
    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)


    ' PUBLIC Property Procedures (Begin)
    Public Property GISDataModelId() As Integer
        Get
            Return m_lGISDataModelId
        End Get
        Set(ByVal Value As Integer)
            m_lGISDataModelId = Value
        End Set
    End Property

    Public Property GISDataModel() As String
        Get
            Return m_sGISDataModel
        End Get
        Set(ByVal Value As String)
            m_sGISDataModel = Value
        End Set
    End Property

    Public Property GISDataModelDescription() As String
        Get
            Return m_sGISDataModelDescription
        End Get
        Set(ByVal Value As String)
            m_sGISDataModelDescription = Value
        End Set
    End Property

    Public ReadOnly Property StepStatus() As String
        Get
            Return m_sStepStatus
        End Get
    End Property

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

    Public Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
        Set(ByVal Value As Integer)

            ' Set the interface exit status.
            m_lStatus = Value

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

    Public Property SourceId() As Integer
        Get
            Return m_iSourceId
        End Get
        Set(ByVal Value As Integer)
            m_iSourceId = Value
        End Set
    End Property

    Public WriteOnly Property SwiftIntegration() As Integer
        Set(ByVal Value As Integer)
            m_lSwiftIntegration = Value
        End Set
    End Property

    ''' <summary>
    ''' To read and write property whether this is a Market Place data model or not
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsMarketplaceDM() As Boolean
        Get
            Return m_bIsMarketplaceDM
        End Get
        Set(ByVal Value As Boolean)
            m_bIsMarketplaceDM = Value
        End Set
    End Property

    ''' <summary>
    ''' To read and write property whether this is a imported Market Place data model or not
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsImportedMarketplaceDM() As Boolean
        Get
            Return m_bIsImportedMarketplaceDM
        End Get
        Set(ByVal Value As Boolean)
            m_bIsImportedMarketplaceDM = Value
        End Set
    End Property

    ''' <summary>
    ''' To read and write property whether any changes done in data model while editing
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SomethingChanged() As Boolean
        Get
            Return m_bSomethingChanged
        End Get
        Set(ByVal Value As Boolean)
            m_bSomethingChanged = Value
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
    Public Function SetFieldValidation() As Integer

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
            ''                       lMandatory:=<PMMandatory or PMNonMandatory)
            '
            '        'Error checking
            '        If m_lReturn <> PMTrue Then
            '          SetFieldValidation = PMFalse
            '          Exit Function
            '        End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            '    m_lReturn = m_oFormFields.AddNewFormField( _
            ''                           ctlControl:=txtScreenCode, _
            ''                           lFieldType:=PMString, _
            ''                           lFormat:=PMFormatString, _
            ''                           lMandatory:=PMMandatory)
            '
            '    'Error checking
            '    If m_lReturn <> PMTrue Then
            '      SetFieldValidation = PMFalse
            '      Exit Function
            '    End If

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim sTitle, sMessage As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.

            ' {* USER DEFINED CODE (Begin) *}

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_nReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bGISMaintainDataDictionary.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                ' developer guide no. 243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                ' developer guide no. 243
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)


                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_nReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return result
            End If

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}

            m_oBusiness.GISDataModelID = GISDataModelId

            m_oBusiness.GISDataModel = GISDataModel

            ' {* USER DEFINED CODE (End) *}

            'get the details for what ever data model has been asked for

            m_nReturn = m_oBusiness.GetDataModelDetails()
            'get the data model type from the business object

            m_lGISDataModelType = m_oBusiness.GISDataModelType


            'even if this is not a risk data model type, load it into that array

            m_nReturn = m_oBusiness.GetObjectAndPropertyDetails(r_vGISObject:=m_vGISObject, r_vGISProperty:=m_vGISProperty)


            m_sGISDataModelName = m_oBusiness.GISDataModelName

            ' Check for errors
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                If m_nReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                    'Don't exit, we need to terminate
                    '            Exit Function
                End If
            End If

            ' Get the BOMRequired setting for our Data Model. PN28024.

            m_nReturn = m_oBusiness.GetDataModelBOMRequired(v_sDataModelCode:=m_sGISDataModel, r_sBOMRequired:=m_sBOMRequired)

            ' Check for errors
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the BOMRequired setting from the business object for Data Model " & m_sGISDataModel, vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")
                m_oBusiness = Nothing
                Return result
            End If

            ' Get the BOMRequired setting for DEFAULT Data Model. PN28024.

            m_nReturn = m_oBusiness.GetDataModelBOMRequired(v_sDataModelCode:="DEF", r_sBOMRequired:=m_sDefaultBOMRequired)

            ' Check for errors
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the BOMRequired setting from the business object for Data Model " & m_sGISDataModel, vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")
                m_oBusiness = Nothing
                Return result
            End If


            m_nReturn = m_oBusiness.GetOtherDetails(r_vPartyType:=m_vPartyTypeArray, r_vSumInsuredType:=m_vSumInsuredTypeArray, r_vGISUserDefHeader:=m_vGISUserDefHeaderArray, r_vProduct:=m_vProductArray, r_vIndexLinking:=m_vIndexLinking, r_vDocumentFilter:=m_vDocumentFilterArray, r_vPMLookupList:=m_vPMLookupList)

            ' Check for errors
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                If m_nReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get other details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                    'Don't exit, we need to terminate
                    '            Exit Function
                End If
            End If

            ' Terminate the business object

            m_oBusiness.Dispose()
            ' Destroy the instance of the business object
            ' from memory.
            m_oBusiness = Nothing

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

            ' Update the interface details.

            ' Assign the details from the business object
            ' to the data storage.
            m_nReturn = BusinessToData()

            ' Check for errors
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
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

            txtDataModel.Text = m_sGISDataModelDescription

            ' Set the state of the SAM checkbox. PN28024.
            If m_bAccessibleViaSAMOption Then
                If m_sBOMRequired = ACBOMRequiredAOL Then
                    chkAccessibleViaSAM.CheckState = CheckState.Checked
                End If
            End If

            m_nReturn = PopulateRiskObjectsListView()

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (End) *}

            'MKW010803 PN4514 START
            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                cmdObjectAdd.Enabled = False
                cmdOK.Enabled = False

                cmdObjectEdit.Text = "View"
                cmdPropertyEdit.Text = "View"

            Else
                If m_lSwiftIntegration <> 0 Then
                    ' cannot add new objects if called from swift
                    cmdObjectAdd.Enabled = False
                    cmdObjectEdit.Text = "View"
                Else
                    cmdObjectAdd.Enabled = True
                    cmdObjectEdit.Text = "Edit"
                End If
                cmdOK.Enabled = True

                cmdPropertyEdit.Text = "Edit"

            End If
            'MKW010803 PN4514 END


            m_nReturn = LoadGisLists(cboGISListId, m_sGISDataModel)



            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: PopulateObjects
    '
    ' Description:
    '
    ' History: 15/08/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function PopulateObjects(ByRef tvwObjects As TreeView, ByVal v_lGISDataModelType As Integer, Optional ByVal v_lIndex As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim nodTemp As TreeNode

        Dim selNode As TreeNode = New TreeNode
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(m_vGISObject) Then


                Return result
            End If

            'Clear this down
            lvwProperties.Items.Clear()
            cmdPropertyEdit.Enabled = False
            cmdPropertyDelete.Enabled = False


            For lTemp As Integer = m_vGISObject.GetLowerBound(1) To m_vGISObject.GetUpperBound(1)

                If Convert.IsDBNull(m_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTemp)) Or IsNothing(m_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTemp)) Or m_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTemp) = 0 Then
                    nodTemp = tvwObjects.Nodes.Add(m_vGISObject(ACOObjectName, lTemp), m_vGISObject(ACOObjectName, lTemp))
                    ''developer guide no.  no conversion required
                    nodTemp.Tag = New Object() {v_lGISDataModelType, lTemp}

                    'TODO: no solution found todolist
                    nodTemp.TreeView.Sort()

                Else
                    'find the parent
                    For lTemp2 As Integer = m_vGISObject.GetLowerBound(1) To m_vGISObject.GetUpperBound(1)
                        If Convert.ToString(m_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTemp)) = Convert.ToString(m_vGISObject(pbObjectAndPropertyConsts.ACOGISObjectId, lTemp2)) Then
                            nodTemp = tvwObjects.Nodes.Find(m_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lTemp2), True)(0).Nodes.Add(m_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lTemp), m_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lTemp), 0)
                            tvwObjects.Nodes.Find(m_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lTemp2), True)(0).ImageKey = ACOpenFolder
                            tvwObjects.Nodes.Find(m_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lTemp2), True)(0).Expand()
                            ''developer guide no.conversion required
                            nodTemp.Tag = New Object() {v_lGISDataModelType, lTemp}
                            lTemp2 = m_vGISObject.GetUpperBound(1)

                            'TODO: NIIT
                            nodTemp.Parent.TreeView.Sort()
                        End If
                    Next lTemp2
                End If

                If lTemp = v_lIndex Then 'set focus to this one

                    'developer guide no. 35
                    If tvwObjects.Nodes.Find(m_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lTemp), True).Length <> 0 Then
                        selNode = tvwObjects.Nodes.Find(m_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lTemp), True)(0)
                    End If
                    PopulateProperties(lTag:=v_lIndex)
                    m_lObjectTag = v_lIndex
                End If

            Next lTemp
            tvwObjects.SelectedNode = selNode ' tvwObjects.Nodes.Find(m_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lTemp), True)(0)
            tvwObjects.ShowLines = True

            tvwObjects_NodeMouseClick(tvwObjects, New TreeNodeMouseClickEventArgs(tvwObjects.SelectedNode, Windows.Forms.MouseButtons.Left, 1, 0, 0))
            Return result


        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateObjects Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateObjects", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_nReturn = ValidateForm()

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Update the business object.

            ' Assign the details from the interface to the data storage.
            m_nReturn = InterfaceToData()

            ' Check for errors
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' {* USER DEFINED CODE (Begin) *}
            '    m_lReturn& = m_oBusiness.EditUpdate(r_vDataDictionary:=m_vDataDictionary, _
            'r_vScreenHeader:=m_vScreenHeader, _
            'r_vScreenDetails:=m_vScreenDetails)

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors.
            '    If m_lReturn <> PMTrue Then
            '        InterfaceToBusiness = PMFalse
            '
            '        ' Log Error.
            '        LogMessage _
            ''            iType:=PMLogError, _
            ''            sMsg:="Failed to assign the interface details to business object", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="InterfaceToBusiness"
            '    End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateBusiness
    '
    ' Description:
    '
    ' History: 13/07/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function UpdateBusiness() As Integer

        Dim result As Integer = 0
        Dim sTitle As String = String.Empty
        Dim sMessage As String = String.Empty
        Dim nReturn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not m_bSomethingChanged Then
                Return result
            Else
                If (m_bIsImportedMarketplaceDM AndAlso m_bIsMarketplaceDM) Then
                    If MessageBox.Show("Please note that this is a Marketplace standard product. Any changes made in Pure Insurance to this product may render it invalid for future Marketplace transactions." & _
                            " Please contact your system administrator or the relevant SSP Business Manager before proceeding with any product changes.", _
                            "Market Place Data Model", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        nReturn = m_oBusinessGlobalTransaction.UpdateMPDataModel(sDataModelCode:=m_sGISDataModel,
                                                                                 bIsMPDataModel:=Not m_bSomethingChanged)
                        If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError,
                                               sMsg:="Failed to update Market Place Data Model Check (UpdateMPDataModel)",
                                               vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateBusiness")
                        End If
                    Else
                        m_bSomethingChanged = False
                        Return result
                    End If
                End If
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get an instance of the business object via the public object manager.
            Dim temp_m_oBusiness As Object
            m_nReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bGISMaintainDataDictionary.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                ' Display error stating the problem.
                ' Get description from the resource file.

                ' developer guide no. 243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' developer guide no. 243
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_nReturn = m_oBusinessGlobalTransaction.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateBusiness")
                Return result
            End If

            ' Set the business keys.

            m_oBusinessGlobalTransaction.GISDataModel = GISDataModel

            m_oBusinessGlobalTransaction.GISDataModelId = GISDataModelId

            m_oBusinessGlobalTransaction.GISDataModelType = m_lGISDataModelType
            m_oBusinessGlobalTransaction.IsMarketplaceDM = m_bIsMarketplaceDM
            m_oBusinessGlobalTransaction.IsImportedMarketplaceDM = m_bIsImportedMarketplaceDM

            Dim lSingleObjectId As Integer
            If m_lStatus = gPMConstants.PMEReturnCode.PMRecordChanged Then 'updating a single object

                'lSingleObjectId = CInt(Convert.ToString(ACTagGisObjectId))
                lSingleObjectId = tvwObjects.SelectedNode.Tag(ACTagGisObjectId)
                pnlStatus.Text = "Updating " & m_sGISDataModelName
                pnlStatus.Refresh()

                m_nReturn = m_oBusinessGlobalTransaction.Update(r_vGISObject:=m_vGISObject, r_vGISProperty:=m_vGISProperty, v_lSingleObjectId:=lSingleObjectId, v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=m_sScreenHierarchy)
                pnlStatus.Text = ""
                pnlStatus.Refresh()
            Else
                If m_sGISDataModel.Length <> 0 Then
                    pnlStatus.Text = "Updating " & m_sGISDataModelName
                    pnlStatus.Refresh()
                    m_oBusinessGlobalTransaction.GISDataModel = m_sGISDataModel

                    m_nReturn = m_oBusinessGlobalTransaction.Update(r_vGISObject:=m_vGISObject, r_vGISProperty:=m_vGISProperty, v_lSingleObjectId:=-1, v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=m_sScreenHierarchy)
                    pnlStatus.Text = ""
                    pnlStatus.Refresh()
                End If
            End If
            ' Check for errors.
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the " & "business object", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateBusiness")
            Else

                ' Update the BOMRequired reg setting, if applicable. PN28024.
                If m_bAccessibleViaSAMOption Then
                    If chkAccessibleViaSAM.CheckState = CheckState.Checked And m_sBOMRequired <> ACBOMRequiredAOL Then
                        ' Set to use the online BOM

                        m_nReturn = m_oBusiness.SetDataModelBOMRequired(sDataModelCode:=m_sGISDataModel, sBOMRequired:=ACBOMRequiredAOL)
                    ElseIf chkAccessibleViaSAM.CheckState = CheckState.Unchecked And m_sBOMRequired = ACBOMRequiredAOL Then
                        ' Set to the default BOM

                        m_nReturn = m_oBusiness.SetDataModelBOMRequired(sDataModelCode:=m_sGISDataModel, sBOMRequired:=m_sDefaultBOMRequired)
                    End If
                End If
            End If

            ' Terminate the business object

            m_oBusiness.Dispose()
            ' Destroy the instance of the business object from memory.
            m_oBusiness = Nothing

            If m_lStatus = gPMConstants.PMEReturnCode.PMRecordChanged Then 'updating a single object
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            ' RAM20040914 : Update the status
            pnlStatus.Text = "Creating datamodel specific registry settings..."
            ' developer guide no.298 
            pnlStatus.Refresh()

            m_nReturn = CreateRegistrySettings()
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create the " & "registry settings", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateBusiness")
            End If

            ' RAM20040914 : Update the status
            pnlStatus.Text = "Creating datasets ..."
            ' developer guide no. 298
            pnlStatus.Refresh()

            m_nReturn = CreateDataSet(m_sGISDataModel)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create the data set", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateBusiness")
            End If

            ' RAM20040914 : Update the status
            pnlStatus.Text = ""
            pnlStatus.Refresh()
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateBusiness Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: ValidateForm
    '
    ' Description: Is the data on the interface ok
    '
    '
    ' ***************************************************************** '
    Public Function ValidateForm() As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to validate the form", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            m_nReturn = GetLookupValues()

            ' Check for errors.
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get all of the lookup details.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to retrieve all of the lookup
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
            '    If m_lReturn <> PMTrue Then
            '        DisplayLookupDetails = PMFalse
            '        Exit Function
            '    End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




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

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details to the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            '    m_lReturn& = m_oBusiness.GetNext(r_vDataDictionary:=m_vDataDictionary, _
            'r_vScreenHeader:=m_vScreenHeader, _
            'r_vScreenDetails:=m_vScreenDetails, _
            'r_vChildScreenDetails:=m_vChildScreenDetails)

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors.
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            Return result

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
            '    m_lReturn& = m_oFormFields.UnformatControl(txtName)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            'No need to do anything - it's all sorted out as we go

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



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
        Dim sUnderwritingAgency As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            m_nReturn = DisplayCaptions()

            ' Check for errors.
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
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

            m_nReturn = SetFirstLastControls()

            ' Check for errors.
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            m_nReturn = SetExtraListViewProperties(v_hWndList:=tvwObjects.Handle.ToInt32(), v_vShowRowSelect:=True)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_nReturn = SetExtraListViewProperties(v_hWndList:=lvwProperties.Handle.ToInt32(), v_vShowRowSelect:=True)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_bSomethingChanged = False

            ' Set the visibility of the Accessible Via SAM checkbox. PN28024.
            If m_bAccessibleViaSAMOption Then
                If m_sGISDataModel.Substring(0, 3).ToLower() = "gii" Or m_sGISDataModel.ToLower() = "sbo" Or m_lGISDataModelType = 4 Then
                    m_bAccessibleViaSAMOption = False
                End If
            End If

            lblAccessibleViaSAM.Visible = m_bAccessibleViaSAMOption
            chkAccessibleViaSAM.Visible = m_bAccessibleViaSAMOption

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

            m_ctlTabFirstLast(ACControlStart, 0) = tvwObjects
            m_ctlTabFirstLast(ACControlEnd, 0) = lvwProperties

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


            ' developer guide no. 243
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
            ' developer guide no. 243 starts

            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Ends



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
            ' developer guide no. 243 starts
            lblDataModel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDataModel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblObjects.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACObjectsTables, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblProperties.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPropertiesColumns, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwProperties.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACProperty, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwProperties.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColumnName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwProperties.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSPropertyType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwProperties.Columns.Item(3).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSPropertySpecial, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdObjectAdd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdObjectEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdPropertyAdd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdPropertyEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblStatus.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            chkShowKeys.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACShowKeys, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            ' developer guide no. 243 Ends

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
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '
    Private Function GetLookupValues() As Integer

        Dim result As Integer = 0
        Try


            ' Gets all of the lookup values.

            ' Check the task.
            '    Select Case (m_iTask)
            '        Case PMAdd
            '            ' Get all of the lookup values.
            '            m_lReturn& = m_oBusiness.GetLookupValues( _
            ''                iLookupType:=PMLookupAll, _
            ''                vTableArray:=m_vLookupValues, _
            ''                iLanguageID:=g_iLanguageID%, _
            ''                vResultArray:=m_vLookupDetails)
            '
            '        Case PMEdit
            '            ' Get all of the lookup values with the correct
            '            ' effective date.
            '            m_lReturn& = m_oBusiness.GetLookupValues( _
            ''                iLookupType:=PMLookupAllEffective, _
            ''                vTableArray:=m_vLookupValues, _
            ''                iLanguageID:=g_iLanguageID%, _
            ''                vResultArray:=m_vLookupDetails)
            '
            '        Case PMView
            '            ' Get lookup values for viewing only.
            '            m_lReturn& = m_oBusiness.GetLookupValues( _
            ''                iLookupType:=PMLookupSingle, _
            ''                vTableArray:=m_vLookupValues, _
            ''                iLanguageID:=g_iLanguageID%, _
            ''                vResultArray:=m_vLookupDetails)
            '    End Select
            '
            '    ' Check for errors.
            '    If m_lReturn <> PMTrue Then
            '        GetLookupValues = PMFalse
            '
            '        ' Log Error.
            '        LogMessagePopup _
            ''            iType:=PMLogError, _
            ''            sMsg:="Failed to get the lookup values from the business object", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="GetLookupValues"
            '
            '        Exit Function
            '    End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




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

    'Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As Control) As Integer
    '
    'Dim result As Integer = 0
    'Dim lRow, lCntr As Integer
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
    '
    ' Get the lookup values.
    '
    '    bFoundMatch = False
    ''
    '    For lRow& = LBound(m_vLookupValues, 2) To UBound(m_vLookupValues, 2)
    '        ' Check for a match of the table name.
    '        If (Trim$(m_vLookupValues(ACValueTableName, lRow&)) = _
    ''        Trim$(sLookupTable$)) Then
    '            ' Found a match
    '            bFoundMatch = True
    '            Exit For
    '        End If
    '    Next lRow&
    ''
    '    ' Check if there has been a table match.
    '    If (bFoundMatch = False) Then
    '        GetLookupDetails = PMFalse
    ''
    '        ' Log Error.
    '        LogMessage _
    ''            iType:=PMLogOnError, _
    ''            sMsg:="Failed to get details for the table, " & sLookupTable$, _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="GetLookupDetails"
    ''
    '        Exit Function
    '    End If
    ''
    '    ' Using the lookup values, populate the control with
    '    ' the details from the lookup details array.
    ''
    '    For lCntr& = m_vLookupValues(ACValueStartPos, lRow&) To _
    ''    (m_vLookupValues(ACValueStartPos, lRow&) + m_vLookupValues(ACValueNumber, lRow&)) - 1
    '        ' Add the details to the control.
    '        ctlLookup.AddItem m_vLookupDetails(ACDetailDesc, lCntr&)
    '        ctlLookup.ItemData(ctlLookup.NewIndex) = CLng(m_vLookupDetails(ACDetailKey, lCntr&))
    ''
    '        'SP150998 - compare long value not string
    '        ' Check if this is the selected index.
    '        If (m_vLookupValues(ACValueID, lRow&) <> "") Then
    '            If (m_vLookupValues(ACValueID, lRow&) = _
    ''            CLng(m_vLookupDetails(ACDetailKey, lCntr&))) Then
    '                ctlLookup.ListIndex = ctlLookup.NewIndex
    '            End If
    '        End If
    ''
    '    Next lCntr&
    ''
    '    ' Check if the selected index is blank. If so,
    '    ' we set the controls index to zero.
    '    If (m_vLookupValues(ACValueID, lRow&) = "") Then
    '        ctlLookup.ListIndex = 0
    '    End If
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    '
    ' Name: PopulateProperties
    '
    ' Description:
    '
    ' History: 15/08/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function PopulateProperties(ByRef lTag As Integer) As Integer
        Dim result As Integer = 0
        ' developer guide no. 33
        Dim vSpecialsTypeReference As Object

        Dim vArray(,) As Object
        Dim oListItem As ListViewItem
        ' developer guide no. 33
        Dim vValue As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lvwProperties.Items.Clear()

            If lTag = -1 Then
                Return result
            End If


            vArray = m_vGISProperty(lTag)

            If Not Information.IsArray(vArray) Then
                Return result
            End If


            For lTemp As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)



                If CStr(vArray(pbObjectAndPropertyConsts.ACPPropertyName, lTemp)) <> "dElEtEd" And (CDbl(vArray(pbObjectAndPropertyConsts.ACPIsPrimaryKey, lTemp)) <> 1 Or chkShowKeys.CheckState = CheckState.Checked) Then

                    oListItem = lvwProperties.Items.Add(CStr(vArray(pbObjectAndPropertyConsts.ACPPropertyName, lTemp)))

                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, lTemp))
                    ' developer guide no.change "" to nothing 
                    vValue = Nothing

                    If CDbl(vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp)) = 0 Then
                        Select Case vArray(pbObjectAndPropertyConsts.ACPDataType, lTemp)
                            Case GISDataTypeText
                                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "String"

                            Case GISDataTypeComment
                                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Comment"

                            Case GISDataTypeNumeric, GISDataTypeInteger
                                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Integer"

                            Case GISDataTypeDate
                                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Date"

                            Case GISDataTypeOption
                                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Boolean"

                            Case GISDataTypeCurrency
                                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Currency"

                            Case GISDataTypePercentage
                                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Percentage"

                            Case GISDataTypecode
                                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "G2Integer"

                            Case Else
                        End Select

                    Else


                        vSpecialsTypeReference = vArray(pbObjectAndPropertyConsts.ACPSpecialsTypeReference, lTemp)
                        Select Case vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp)
                            Case GISSharedPropertyConstants.ACOGISListID
                                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "GIS List"

                                ' now format the controls dependent upon this option
                                For lTemp2 As Integer = 0 To cboGISListId.Items.Count - 1
                                    'If vSpecialsTypeReference = VB6.GetItemData(cboGISListId, lTemp2) Then
                                    If Convert.ToString(vSpecialsTypeReference) = Convert.ToString(CType(cboGISListId.Items(lTemp2), VB6.ListBoxItem).ItemData) Then
                                        cboGISListId.SelectedIndex = lTemp2

                                        vValue = cboGISListId.Text
                                        Exit For
                                    End If
                                Next lTemp2

                            Case GISSharedPropertyConstants.ACOPMLookupTableName
                                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "PM Lookup"

                                vValue = vSpecialsTypeReference

                            Case GISSharedPropertyConstants.ACOComboLookup
                                ' now format the controls dependent upon this option
                                '          m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtComboLookupTableName, vControlValue:=vValue)

                            Case GISSharedPropertyConstants.ACOGISUserDefHeaderID
                                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "User Defined"
                                For lTemp2 As Integer = 0 To m_vGISUserDefHeaderArray.GetUpperBound(1)
                                    If CStr(vSpecialsTypeReference) = CStr(m_vGISUserDefHeaderArray(0, lTemp2)) Then
                                        'vValue = CInt(m_vGISUserDefHeaderArray(1, lTemp2))
                                        vValue = (m_vGISUserDefHeaderArray(1, lTemp2))
                                        Exit For
                                    End If

                                Next
                            Case GISSharedPropertyConstants.ACOPartyTypeID
                                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Party"
                                For lTemp2 As Integer = 0 To m_vPartyTypeArray.GetUpperBound(1)
                                    If CStr(vSpecialsTypeReference) = CStr(m_vPartyTypeArray(0, lTemp2)) Then
                                        'vValue = CInt(m_vPartyTypeArray(1, lTemp2))
                                        vValue = (m_vPartyTypeArray(1, lTemp2))
                                        Exit For
                                    End If

                                Next
                            Case GISSharedPropertyConstants.ACOProductID
                                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Policy"
                                For lTemp2 As Integer = 0 To m_vProductArray.GetUpperBound(1)
                                    If CStr(vSpecialsTypeReference) = CStr(m_vProductArray(0, lTemp2)) Then
                                        'vValue = CInt(m_vProductArray(1, lTemp2))
                                        vValue = (m_vProductArray(1, lTemp2))
                                        Exit For
                                    End If

                                Next
                            Case GISSharedPropertyConstants.ACOSumInsuredTypeID
                                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Sum Insured"
                                For lTemp2 As Integer = 0 To m_vSumInsuredTypeArray.GetUpperBound(1)
                                    If CStr(vSpecialsTypeReference) = CStr(m_vSumInsuredTypeArray(0, lTemp2)) Then
                                        ' vValue = CInt(m_vSumInsuredTypeArray(1, lTemp2))
                                        vValue = (m_vSumInsuredTypeArray(1, lTemp2))
                                        Exit For
                                    End If
                                Next
                            Case GISSharedPropertyConstants.ACOStdWordingType
                                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Standard Wording"
                                vValue = vSpecialsTypeReference

                            Case GISSharedPropertyConstants.ACOReserveID
                                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Claim Reserve"
                                'vValue = CType("", gPMConstants.PMEReturnCode)
                                vValue = Nothing

                            Case GISSharedPropertyConstants.ACOPaymentID
                                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Claim Payment"
                                'vValue = CType("", gPMConstants.PMEReturnCode)
                                vValue = Nothing
                            Case GISSharedPropertyConstants.ACOCaseHeader
                                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Case Header"
                                'vValue = CType("", gPMConstants.PMEReturnCode)
                                vValue = Nothing
                            Case GISSharedPropertyConstants.ACOCaseClaimList
                                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Case Claim Links"
                                'vValue = CType("", gPMConstants.PMEReturnCode)
                                vValue = Nothing

                            Case Else

                        End Select
                    End If


                    ListViewHelper.GetListViewSubItem(oListItem, 3).Text = gPMFunctions.ToSafeString(vValue, "")
                    oListItem.Tag = CStr(lTemp)
                End If
            Next lTemp

            vArray = Nothing
            ListViewHelper.SetSortKeyProperty(lvwProperties, 0)
            ListViewHelper.SetSortedProperty(lvwProperties, True)

            If lvwProperties.Items.Count > 0 Then
                lvwProperties.FocusedItem = lvwProperties.Items.Item(0)
            End If

            cmdPropertyEdit.Enabled = False
            cmdPropertyDelete.Enabled = False


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CallObject
    '
    ' Description:
    '
    ' History: 30/06/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function CallObject(ByRef lTag As Integer, ByVal lTask As Integer) As Integer

        Dim nResult As Integer = 0
        Dim vArray, vParentArray(,) As Object
        Dim lTemp As Integer
        Dim bOldGis As Boolean

#If old_gis = 1 Then

		bOldGis = True
#End If

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            'Create address component if not already done so
            If m_oGISObject Is Nothing Then

                ' Get an instance of the contact interface object via
                ' the public object manager.
                Dim temp_m_oGISObject As Object
                m_nReturn = g_oObjectManager.GetInstance(temp_m_oGISObject, sClassName:="iGISObject.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oGISObject = temp_m_oGISObject

                ' Check for errors.
                If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get GIS Object component", vApp:=ACApp, vClass:=ACClass, vMethod:="CallObject", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return nResult

                End If

            End If

            m_nReturn = m_oGISObject.SetProcessModes(vTask:=lTask)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            If lTask = gPMConstants.PMEComponentAction.PMAdd Then

                m_oGISObject.GISObjectId = -1

                m_oGISObject.GISDataModelId = m_vGISObject(pbObjectAndPropertyConsts.ACOGISDataModelId, 0)

                m_oGISObject.GISDataModel = m_sGISDataModel

                m_oGISObject.ObjectName = ""

                m_oGISObject.TableName = ""


                m_oGISObject.MaxInstances = Nothing

                m_oGISObject.IsQuoteObject = gPMConstants.PMEReturnCode.PMFalse


                'm_oGISObject.ParentObjectId = m_vGISObject(pbObjectAndPropertyConsts.ACOGISObjectId, CInt(Convert.ToString(ACTagGisObjectId)))
                m_oGISObject.ParentObjectId = m_vGISObject(pbObjectAndPropertyConsts.ACOGISObjectId, CInt(Convert.ToString(tvwObjects.SelectedNode.Tag(ACTagGisObjectId))))


                m_oGISObject.PolarisObjectId = Nothing

                m_oGISObject.IsSelectableForScreen = True

                m_oGISObject.ObjectType = GISDataModelType.GISOTRisk

                m_oGISObject.EditFlags = GISSharedPropertyConstants.GISDSEditNone
            Else
                If m_lSwiftIntegration <> 0 Then
                    lTask = gPMConstants.PMEComponentAction.PMView
                ElseIf Not m_bCanEditObject Then
                    lTask = gPMConstants.PMEComponentAction.PMView
                ElseIf (CBool(m_vGISObject(pbObjectAndPropertyConsts.ACOEditFlags, lTag)) And GISSharedPropertyConstants.GISDSEditReadOnly) = GISSharedPropertyConstants.GISDSEditReadOnly Then
                    lTask = gPMConstants.PMEComponentAction.PMView
                End If

                m_nReturn = m_oGISObject.SetProcessModes(vTask:=lTask)

                m_oGISObject.GISObjectId = m_vGISObject(pbObjectAndPropertyConsts.ACOGISObjectId, lTag)

                m_oGISObject.GISDataModelId = m_vGISObject(pbObjectAndPropertyConsts.ACOGISDataModelId, lTag)
                'MSB110902 - Pass this in...

                m_oGISObject.GISDataModel = m_sGISDataModel

                m_oGISObject.ObjectName = m_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lTag)

                m_oGISObject.TableName = m_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTag)

                m_oGISObject.MaxInstances = m_vGISObject(pbObjectAndPropertyConsts.ACOMaxInstances, lTag)

                m_oGISObject.IsQuoteObject = m_vGISObject(pbObjectAndPropertyConsts.ACOIsQuoteObject, lTag)

                m_oGISObject.ParentObjectId = m_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTag)

                m_oGISObject.PolarisObjectId = m_vGISObject(pbObjectAndPropertyConsts.ACOPolarisObjectId, lTag)

                m_oGISObject.IsSelectableForScreen = m_vGISObject(pbObjectAndPropertyConsts.ACOIsSelectableForScreen, lTag)

                m_oGISObject.ObjectType = m_vGISObject(pbObjectAndPropertyConsts.ACOIsNonGIS, lTag)

                m_oGISObject.EditFlags = m_vGISObject(pbObjectAndPropertyConsts.ACOEditFlags, lTag)
            End If

            'For now let's do this...

            m_oGISObject.AllowedParents = VB6.CopyArray(m_vGISObject)

            m_oGISObject.DataModelType = m_lGISDataModelType


            m_oGISObject.SQLServerVersion = m_oBusinessGlobalTransaction.SQLServerVersion


            m_nReturn = m_oGISObject.Start()

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            'MKW010803 PN4514 START Viewing only nothing to changed
            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                Return nResult
            End If
            'MKW010803 PN4514 END


            If m_oGISObject.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Return nResult
            End If

            If lTask = gPMConstants.PMEComponentAction.PMView Then
                Return nResult
            End If



            If lTask = gPMConstants.PMEComponentAction.PMEdit Then
                'Refresh the array
                If m_vGISObject(pbObjectAndPropertyConsts.ACOGISObjectId, lTag) <> m_oGISObject.GISObjectId Then
                    m_bSomethingChanged = True
                End If
                m_vGISObject(pbObjectAndPropertyConsts.ACOGISObjectId, lTag) = m_oGISObject.GISObjectId

                If m_vGISObject(pbObjectAndPropertyConsts.ACOGISDataModelId, lTag) <> m_oGISObject.GISDataModelId Then
                    m_bSomethingChanged = True
                End If
                m_vGISObject(pbObjectAndPropertyConsts.ACOGISDataModelId, lTag) = m_oGISObject.GISDataModelId

                If m_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lTag) <> m_oGISObject.ObjectName Then
                    m_bSomethingChanged = True
                End If
                m_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lTag) = m_oGISObject.ObjectName

                If m_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTag) <> m_oGISObject.TableName Then
                    m_bSomethingChanged = True
                End If
                m_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTag) = m_oGISObject.TableName

                If m_vGISObject(pbObjectAndPropertyConsts.ACOMaxInstances, lTag) <> m_oGISObject.MaxInstances Then
                    m_bSomethingChanged = True
                End If
                m_vGISObject(pbObjectAndPropertyConsts.ACOMaxInstances, lTag) = m_oGISObject.MaxInstances

                If m_vGISObject(pbObjectAndPropertyConsts.ACOIsQuoteObject, lTag) <> m_oGISObject.IsQuoteObject Then
                    m_bSomethingChanged = True
                End If
                m_vGISObject(pbObjectAndPropertyConsts.ACOIsQuoteObject, lTag) = m_oGISObject.IsQuoteObject

                If m_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTag) <> m_oGISObject.ParentObjectId Then
                    m_bSomethingChanged = True
                End If
                m_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTag) = m_oGISObject.ParentObjectId

                If m_vGISObject(pbObjectAndPropertyConsts.ACOPolarisObjectId, lTag) <> m_oGISObject.PolarisObjectId Then
                    m_bSomethingChanged = True
                End If
                m_vGISObject(pbObjectAndPropertyConsts.ACOPolarisObjectId, lTag) = m_oGISObject.PolarisObjectId

                If m_vGISObject(pbObjectAndPropertyConsts.ACOIsSelectableForScreen, lTag) <> m_oGISObject.IsSelectableForScreen Then
                    m_bSomethingChanged = True
                End If
                m_vGISObject(pbObjectAndPropertyConsts.ACOIsSelectableForScreen, lTag) = m_oGISObject.IsSelectableForScreen

                If m_vGISObject(pbObjectAndPropertyConsts.ACOIsNonGIS, lTag) <> m_oGISObject.ObjectType Then
                    m_bSomethingChanged = True
                End If
                m_vGISObject(pbObjectAndPropertyConsts.ACOIsNonGIS, lTag) = m_oGISObject.ObjectType

                If m_vGISObject(pbObjectAndPropertyConsts.ACOEditFlags, lTag) <> m_oGISObject.EditFlags Then
                    m_bSomethingChanged = True
                End If

                m_vGISObject(pbObjectAndPropertyConsts.ACOEditFlags, lTag) = m_oGISObject.EditFlags

                tvwObjects.Nodes.Clear()
                m_nReturn = PopulateObjects(tvwObjects, m_lGISDataModelType, lTag)

                Return nResult
            End If

            If lTask = gPMConstants.PMEComponentAction.PMAdd OrElse lTask = gPMConstants.PMEComponentAction.PMDelete Then
                m_bSomethingChanged = True
            End If

            'Now we've a new object.
            If Information.IsArray(m_vGISObject) Then
                lTag = m_vGISObject.GetUpperBound(1) + 1
                ReDim Preserve m_vGISObject(pbObjectAndPropertyConsts.ACOLastElement, lTag)
            Else
                lTag = 0
                ReDim m_vGISObject(pbObjectAndPropertyConsts.ACOLastElement, lTag)
            End If

            m_vGISObject(pbObjectAndPropertyConsts.ACOGISObjectId, lTag) = -1 ' get this when updating
            m_vGISObject(pbObjectAndPropertyConsts.ACOGISDataModelId, lTag) = m_lGISDataModelId

            m_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lTag) = m_oGISObject.ObjectName

            m_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTag) = m_oGISObject.TableName

            m_vGISObject(pbObjectAndPropertyConsts.ACOMaxInstances, lTag) = m_oGISObject.MaxInstances

            m_vGISObject(pbObjectAndPropertyConsts.ACOIsQuoteObject, lTag) = m_oGISObject.IsQuoteObject

            m_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTag) = m_oGISObject.ParentObjectId

            m_vGISObject(pbObjectAndPropertyConsts.ACOPolarisObjectId, lTag) = m_oGISObject.PolarisObjectId

            m_vGISObject(pbObjectAndPropertyConsts.ACOIsSelectableForScreen, lTag) = m_oGISObject.IsSelectableForScreen

            m_vGISObject(pbObjectAndPropertyConsts.ACOIsNonGIS, lTag) = m_oGISObject.ObjectType

            m_vGISObject(pbObjectAndPropertyConsts.ACOEditFlags, lTag) = m_oGISObject.EditFlags

            'We also need some properties for the key if it is a risk object

            '25/04/2003 - PWC - (408) User Definable Fields
            'Added check for GISOTParty
            ' PW080503 - Terms needs to be a standard object (ENDVR00000841)

            If m_oGISObject.ObjectType = GISDataModelType.GISOTRisk Or m_oGISObject.ObjectType = GISDataModelType.GISOTParty Or m_oGISObject.ObjectType = GISDataModelType.GISOTCase Or m_oGISObject.ObjectType = GISDataModelType.GISOTNonGisSpecials Or bOldGis Then
                'First - is there a parent.
                If CDbl(m_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTag)) <> 0 Then
                    'If so, we need the parent's keys
                    For lTemp2 As Integer = m_vGISObject.GetLowerBound(1) To m_vGISObject.GetUpperBound(1)
                        'If m_vGISObject(pbObjectAndPropertyConsts.ACOGISObjectId, lTemp2).Equals(m_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTag)) Then
                        If m_vGISObject(pbObjectAndPropertyConsts.ACOGISObjectId, lTemp2) = m_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTag) Then
                            vParentArray = m_vGISProperty(lTemp2)
                            lTemp = -1

                            For lTemp3 As Integer = vParentArray.GetLowerBound(1) To vParentArray.GetUpperBound(1)
                                If vParentArray(pbObjectAndPropertyConsts.ACPIsPrimaryKey, lTemp3) = gPMConstants.PMEReturnCode.PMTrue Then
                                    AddProperty(lTemp, vArray, -1, -1, CStr(vParentArray(pbObjectAndPropertyConsts.ACPPropertyName, lTemp3)), CStr(vParentArray(pbObjectAndPropertyConsts.ACPColumnName, lTemp3)), CInt(vParentArray(pbObjectAndPropertyConsts.ACPDataType, lTemp3)), CInt(vParentArray(pbObjectAndPropertyConsts.ACPIsInputProperty, lTemp3)), CInt(vParentArray(pbObjectAndPropertyConsts.ACPIsIdentifyingProperty, lTemp3)), CInt(vParentArray(pbObjectAndPropertyConsts.ACPIsPrimaryKey, lTemp3)), CInt(vParentArray(pbObjectAndPropertyConsts.ACPEditFlags, lTemp3)), CInt(vParentArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp3)), vParentArray(pbObjectAndPropertyConsts.ACPSpecialsTypeReference, lTemp3), vParentArray(pbObjectAndPropertyConsts.ACPPolarisPropertyId, lTemp3), vParentArray(pbObjectAndPropertyConsts.ACPIsDeleted, lTemp3), vParentArray(pbObjectAndPropertyConsts.ACPIsSearchProperty, lTemp3), vParentArray(pbObjectAndPropertyConsts.ACPIndexLinkingId, lTemp3), vParentArray(pbObjectAndPropertyConsts.ACPIsChaseCycleProperty, lTemp3), CInt(vParentArray(pbObjectAndPropertyConsts.ACPISClaim360Display, lTemp3)))
                                End If
                            Next lTemp3
                            Exit For
                        End If
                    Next lTemp2
                Else
                    'If not, we use only GIS Policy link
                    lTemp = -1

                    AddProperty(lTemp, vArray, -1, -1, m_sGISDataModel & "_Policy_binder_id", m_sGISDataModel & "_Policy_binder_id", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMTrue, GISSharedPropertyConstants.GISDSEditNone, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)

                End If

                'Then we add SCR_Driver_id (or whatever)


                AddProperty(lTemp, vArray, -1, -1, CStr(m_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTag)) & "_id", CStr(m_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTag)) & "_id", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMTrue, GISSharedPropertyConstants.GISDSEditNone, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)
                If IsPropertyAdded(vArray, m_vGISObject(pbObjectAndPropertyConsts.ACOGISObjectId, lTag), "UID") = False Then
                    AddProperty(lTemp, vArray, -1, -1, "UID", "UID", GISDataTypeText, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditNone, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)
                End If
            Else
                'special objects have no pre-defined keys, all attributes are defined below
                lTemp = -1 'adding attributes below
            End If

            'are we looking at a special object type
            Select Case m_vGISObject(pbObjectAndPropertyConsts.ACOIsNonGIS, lTag)
                Case GISDataModelType.GISOTRisk
                    ' PW110303 - add new 'Terms' object type
                    ' PSCR22
                    ' PW080503 - Terms needs to be a standard object (ENDVR00000841)
                    If CStr(m_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lTag)) = "Terms" Then
                        AddProperty(lTemp, vArray, -1, -1, "terms_for", "", GISDataTypeText, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditNone, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)
                        AddProperty(lTemp, vArray, -1, -1, "excess_Type", "", GISDataTypeText, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditNone, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)
                        AddProperty(lTemp, vArray, -1, -1, "excess", "", GISDataTypeCurrency, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditNone, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)
                        AddProperty(lTemp, vArray, -1, -1, "excess_category", "", GISDataTypeText, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditNone, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)
                        AddProperty(lTemp, vArray, -1, -1, "excluded_from_risk", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditNone, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)
                        ' PW210303 - change to property name in spec + new property


                        AddProperty(lTemp, vArray, -1, -1, "special_conditions", "", GISDataTypeText, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditNone, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                        AddProperty(lTemp, vArray, -1, -1, "special_excess", "", GISDataTypeText, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditNone, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)
                    End If

                Case GISDataModelType.GISOTClaim


                    AddProperty(lTemp, vArray, -1, -1, "Claim_id", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMTrue, GISSharedPropertyConstants.GISDSEditReadOnly, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "Policy_id", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "Policy_Number", "", GISDataTypeText, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "Claim_Number", "", GISDataTypeText, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "Description", "", GISDataTypeText, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "Claim_Status_id", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)
                    '(1049) Use of Null/IsNull() detected. More Information: http://www.vbtonet.com/ewis/ewi1049.aspx
                    '(1068) vArray of type Variant is being forced to Array(Variant). More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                    'AddProperty(lTemp, vArray, -1, -1, "Progress_Status_id", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOPMLookupTableName, CType("progress_status", gPMConstants.PMEReturnCode), DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)
                    AddProperty(lTemp, vArray, -1, -1, "Progress_Status_id", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOPMLookupTableName, "progress_status", DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)
                    '(1049) Use of Null/IsNull() detected. More Information: http://www.vbtonet.com/ewis/ewi1049.aspx
                    '(1068) vArray of type Variant is being forced to Array(Variant). More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                    ' AddProperty(lTemp, vArray, -1, -1, "Primary_Cause_id", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOPMLookupTableName, CType("primary_cause", gPMConstants.PMEReturnCode), DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)
                    AddProperty(lTemp, vArray, -1, -1, "Primary_Cause_id", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOPMLookupTableName, "primary_cause", DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)
                    '(1049) Use of Null/IsNull() detected. More Information: http://www.vbtonet.com/ewis/ewi1049.aspx
                    '(1068) vArray of type Variant is being forced to Array(Variant). More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                    'AddProperty(lTemp, vArray, -1, -1, "Secondary_Cause_id", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOPMLookupTableName, CType("secondary_cause", gPMConstants.PMEReturnCode), DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)
                    AddProperty(lTemp, vArray, -1, -1, "Secondary_Cause_id", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOPMLookupTableName, "secondary_cause", DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)
                    '(1049) Use of Null/IsNull() detected. More Information: http://www.vbtonet.com/ewis/ewi1049.aspx
                    '(1068) vArray of type Variant is being forced to Array(Variant). More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                    '' AddProperty(lTemp, vArray, -1, -1, "Catastrophe_code_id", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOPMLookupTableName, CType("Catastrophe_code", gPMConstants.PMEReturnCode), DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)
                    AddProperty(lTemp, vArray, -1, -1, "Catastrophe_code_id", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOPMLookupTableName, "Catastrophe_code", DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)
                    '(1049) Use of Null/IsNull() detected. More Information: http://www.vbtonet.com/ewis/ewi1049.aspx
                    '(1068) vArray of type Variant is being forced to Array(Variant). More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                    'AddProperty(lTemp, vArray, -1, -1, "Coinsurance_treatment_id", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOPMLookupTableName, CType("Coinsurance_treatment", gPMConstants.PMEReturnCode), DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)
                    AddProperty(lTemp, vArray, -1, -1, "Coinsurance_treatment_id", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOPMLookupTableName, "Coinsurance_treatment", DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)
                    '(1049) Use of Null/IsNull() detected. More Information: http://www.vbtonet.com/ewis/ewi1049.aspx
                    '(1068) vArray of type Variant is being forced to Array(Variant). More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                    AddProperty(lTemp, vArray, -1, -1, "Loss_from_date", "", GISDataTypeDate, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "Loss_to_date", "", GISDataTypeDate, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "Reported_date", "", GISDataTypeDate, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "Reported_to_date", "", GISDataTypeDate, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "Last_modified_date", "", GISDataTypeDate, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)
                    '(1049) Use of Null/IsNull() detected. More Information: http://www.vbtonet.com/ewis/ewi1049.aspx
                    '(1068) vArray of type Variant is being forced to Array(Variant). More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                    AddProperty(lTemp, vArray, -1, -1, "Handler_id", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOPMLookupTableName, "handler", DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)
                    '(1049) Use of Null/IsNull() detected. More Information: http://www.vbtonet.com/ewis/ewi1049.aspx
                    '(1068) vArray of type Variant is being forced to Array(Variant). More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                    AddProperty(lTemp, vArray, -1, -1, "Currency_id", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOPMLookupTableName, "currency", DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)
                    '(1049) Use of Null/IsNull() detected. More Information: http://www.vbtonet.com/ewis/ewi1049.aspx
                    '(1068) vArray of type Variant is being forced to Array(Variant). More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                    AddProperty(lTemp, vArray, -1, -1, "Info_only", "", GISDataTypeOption, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "Likely_claim", "", GISDataTypeOption, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "Location", "", GISDataTypeText, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)
                    '(1049) Use of Null/IsNull() detected. More Information: http://www.vbtonet.com/ewis/ewi1049.aspx
                    '(1068) vArray of type Variant is being forced to Array(Variant). More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                    AddProperty(lTemp, vArray, -1, -1, "Town", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOPMLookupTableName, "town", DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)
                    '(1049) Use of Null/IsNull() detected. More Information: http://www.vbtonet.com/ewis/ewi1049.aspx
                    '(1068) vArray of type Variant is being forced to Array(Variant). More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                    AddProperty(lTemp, vArray, -1, -1, "Risk_type_id", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOPMLookupTableName, "risk_type", DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)
                    '(1049) Use of Null/IsNull() detected. More Information: http://www.vbtonet.com/ewis/ewi1049.aspx
                    '(1068) vArray of type Variant is being forced to Array(Variant). More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                    AddProperty(lTemp, vArray, -1, -1, "Client_name", "", GISDataTypeText, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "Client_address", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "Client_tel_no", "", GISDataTypeText, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "Client_fax_no", "", GISDataTypeText, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "Client_mobile_no", "", GISDataTypeText, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "Client_email", "", GISDataTypeText, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "Client_claim_number", "", GISDataTypeText, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "Insurer_name", "", GISDataTypeText, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "insurer_address", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "insurer_tel_no", "", GISDataTypeText, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "insurer_fax_no", "", GISDataTypeText, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "insurer_email", "", GISDataTypeText, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "insurer_claim_number", "", GISDataTypeText, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "Insurer_Contact", "", GISDataTypeText, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "VAT_registered", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "VAT_reg_no", "", GISDataTypeText, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "Comments", "", GISDataTypeText, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "Claims_status_date", "", GISDataTypeDate, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "Client_short_name", "", GISDataTypeText, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "Insurer_short_name", "", GISDataTypeText, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "Client_tel_no_off", "", GISDataTypeText, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "user_defined_field_A", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "user_defined_field_B", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "user_defined_field_C", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "user_defined_field_D", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "user_defined_field_E", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "Client_id", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "Claim_folder_id", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "Claim_version_number", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)
                    '(1049) Use of Null/IsNull() detected. More Information: http://www.vbtonet.com/ewis/ewi1049.aspx
                    '(1068) vArray of type Variant is being forced to Array(Variant). More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                    AddProperty(lTemp, vArray, -1, -1, "claim_version_status_id", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOPMLookupTableName, "claim_version_status", DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)
                    '(1049) Use of Null/IsNull() detected. More Information: http://www.vbtonet.com/ewis/ewi1049.aspx
                    '(1068) vArray of type Variant is being forced to Array(Variant). More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                    AddProperty(lTemp, vArray, -1, -1, "create_date", "", GISDataTypeDate, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "created_by_id", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "Modified_by_id", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "Acceptance_Status_id", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "Original_Claim_id", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "ReAllow_NCD", "", GISDataTypeText, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "NCD_Status_changed", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)

                    ' PW140703 - PS68 Date Effective rating and rules -
                    ' Add gis_screen_id property


                    AddProperty(lTemp, vArray, -1, -1, "gis_screen_id", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)

                Case GISDataModelType.GISOTPeril


                    AddProperty(lTemp, vArray, -1, -1, "Claim_peril_id", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMTrue, GISSharedPropertyConstants.GISDSEditReadOnly, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "claim_id", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)
                    '(1049) Use of Null/IsNull() detected. More Information: http://www.vbtonet.com/ewis/ewi1049.aspx
                    '(1068) vArray of type Variant is being forced to Array(Variant). More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                    AddProperty(lTemp, vArray, -1, -1, "peril_type_id", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOPMLookupTableName, "peril_type", DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)
                    '(1049) Use of Null/IsNull() detected. More Information: http://www.vbtonet.com/ewis/ewi1049.aspx
                    '(1068) vArray of type Variant is being forced to Array(Variant). More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                    AddProperty(lTemp, vArray, -1, -1, "description", "", GISDataTypeText, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "comments", "", GISDataTypeText, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "sum_insured", "", GISDataTypeCurrency, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "ri_band", "", GISDataTypeOption, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "Reserve_details", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly, GISSharedPropertyConstants.ACOReserveID, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)


                    AddProperty(lTemp, vArray, -1, -1, "Payment_details", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly, GISSharedPropertyConstants.ACOPaymentID, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)
                    ' PW140703 - PS68 Date Effective rating and rules -
                    ' Add gis_screen_id property


                    AddProperty(lTemp, vArray, -1, -1, "gis_screen_id", "", GISDataTypeNumeric, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMFalse, GISSharedPropertyConstants.GISDSEditReadOnly Or GISSharedPropertyConstants.GISDSEditNoDBColumn, GISSharedPropertyConstants.ACOSpecialNone, DBNull.Value, DBNull.Value, gPMConstants.PMEReturnCode.PMFalse, DBNull.Value, DBNull.Value)
            End Select

            If lTag = 0 Then
                ReDim m_vGISProperty(lTag)
            Else
                ReDim Preserve m_vGISProperty(lTag)
            End If

            m_vGISProperty(lTag) = vArray

            vArray = Nothing

            'Now let's repopulate the listview

            m_nReturn = PopulateRiskObjectsListView(lTag)


            Return nResult

        Catch ex As System.Exception



            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CallObject Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CallObject", vErrNo:=Information.Err().Number, vErrDesc:=ex.Message, excep:=ex)

            Return nResult


        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CallProperty
    '
    ' Description:
    '
    ' History: 30/06/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function CallProperty(ByRef lTag As Integer, ByRef lTask As Integer) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Create address component if not already done so
            If m_oGISProperty Is Nothing Then

                ' Get an instance of the contact interface object via
                ' the public object manager.
                Dim temp_m_oGISProperty As Object
                m_nReturn = g_oObjectManager.GetInstance(temp_m_oGISProperty, sClassName:="iGISProperty.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oGISProperty = temp_m_oGISProperty

                ' Check for errors.
                If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get GIS Object component", vApp:=ACApp, vClass:=ACClass, vMethod:="CallProperty", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result

                End If

            End If

            With m_oGISProperty


                m_nReturn = m_oGISProperty.SetProcessModes(vTask:=lTask)

                If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If
                'SJ 21/01/2004 - start

                .GisDataModel = m_sGISDataModel
                'SJ 21/01/2004 - end

                .PartyTypeArray = VB6.CopyArray(m_vPartyTypeArray)

                .SumInsuredTypeArray = VB6.CopyArray(m_vSumInsuredTypeArray)


                .DocumentFilterArray = m_vDocumentFilterArray

                .GISUserDefHeaderArray = VB6.CopyArray(m_vGISUserDefHeaderArray)

                .ProductArray = VB6.CopyArray(m_vProductArray)


                .IndexLinkingArray = m_vIndexLinking

                .IsNonGIS = m_vGISObject(pbObjectAndPropertyConsts.ACOIsNonGIS, m_lObjectTag)

                .SwiftIntegration = m_lSwiftIntegration

                .GISDataModelTypeID = m_lGISDataModelType


                .PMLookupList = m_vPMLookupList

                If lTask = gPMConstants.PMEComponentAction.PMAdd Then


                    .GISPropertyId = -1
                    .GISObjectId = m_vGISObject(pbObjectAndPropertyConsts.ACOGISObjectId, m_lObjectTag)
                    .PropertyName = ""
                    .ColumnName = ""
                    .DataType = gPMConstants.PMEDataType.PMString
                    .IsInputProperty = gPMConstants.PMEReturnCode.PMTrue
                    .IsIdentifyingProperty = gPMConstants.PMEReturnCode.PMFalse
                    .IsPrimaryKey = gPMConstants.PMEReturnCode.PMFalse
                    m_oGISProperty.GISListId = Nothing
                    m_oGISProperty.PolarisPropertyId = Nothing
                    .IsDeleted = gPMConstants.PMEReturnCode.PMFalse
                    .IsSearchProperty = gPMConstants.PMEReturnCode.PMFalse
                    .IsChaseCycleProperty = gPMConstants.PMEReturnCode.PMFalse
                    .IsClaim360Display = gPMConstants.PMEReturnCode.PMFalse
                    If .IsNonGIS = 4 Then
                        .DisableClaim360Display = True
                    Else
                        .DisableClaim360Display = False
                    End If
                    m_oGISProperty.PMLookupTableName = Nothing


                    m_oGISProperty.PartyTypeId = Nothing
                    m_oGISProperty.PMUSumInsuredType = Nothing
                    m_oGISProperty.PMUStdWordingType = Nothing
                    m_oGISProperty.GISUserDefHeaderId = Nothing
                    m_oGISProperty.PMUProductId = Nothing
                    m_oGISProperty.IndexLinkingId = Nothing
                    .GISProperty = m_vGISProperty(m_lObjectTag)
                    .EditFlags = GISSharedPropertyConstants.GISDSEditNone
                    .SpecialsType = GISSharedPropertyConstants.ACOSpecialNone
                    m_oGISProperty.SpecialsTypeReference = Nothing

                    .IsInMISExport = gPMConstants.PMEReturnCode.PMFalse
                    .IsFormattedText = gPMConstants.PMEReturnCode.PMFalse
                    If .IsClaim360Display <> 1 Then
                        Dim m_Object(,) As Object
                        For Each m_Object In m_vGISProperty

                            If Information.IsArray(m_Object) Then

                                For index As Integer = 0 To m_Object.GetUpperBound(1)

                                    If m_Object(pbObjectAndPropertyConsts.ACPISClaim360Display, index) = 1 Then
                                        .DisableClaim360Display = True
                                        Exit For
                                    End If
                                Next
                            End If
                            If .DisableClaim360Display = True Then
                                Exit For
                            End If
                        Next
                    Else
                        .DisableClaim360Display = False
                    End If


                Else

                    vArray = m_vGISProperty(m_lObjectTag)

                    If Not m_bCanEditObject Then
                        m_nReturn = m_oGISProperty.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView)
                    End If


                    If ((vArray(pbObjectAndPropertyConsts.ACPEditFlags, lTag)) And GISSharedPropertyConstants.GISDSEditReadOnly) = GISSharedPropertyConstants.GISDSEditReadOnly Then
                        m_nReturn = m_oGISProperty.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView)
                    End If


                    .GISPropertyId = vArray(pbObjectAndPropertyConsts.ACPGISPropertyId, lTag)


                    .GISObjectId = vArray(pbObjectAndPropertyConsts.ACPGISObjectId, lTag)


                    .PropertyName = vArray(pbObjectAndPropertyConsts.ACPPropertyName, lTag)


                    .ColumnName = vArray(pbObjectAndPropertyConsts.ACPColumnName, lTag)


                    .DataType = vArray(pbObjectAndPropertyConsts.ACPDataType, lTag)


                    .IsInputProperty = vArray(pbObjectAndPropertyConsts.ACPIsInputProperty, lTag)


                    .IsIdentifyingProperty = vArray(pbObjectAndPropertyConsts.ACPIsIdentifyingProperty, lTag)


                    .IsPrimaryKey = vArray(pbObjectAndPropertyConsts.ACPIsPrimaryKey, lTag)


                    .EditFlags = vArray(pbObjectAndPropertyConsts.ACPEditFlags, lTag)


                    .SpecialsType = vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTag)


                    .SpecialsTypeReference = vArray(pbObjectAndPropertyConsts.ACPSpecialsTypeReference, lTag)


                    .PolarisPropertyId = vArray(pbObjectAndPropertyConsts.ACPPolarisPropertyId, lTag)


                    .IsDeleted = vArray(pbObjectAndPropertyConsts.ACPIsDeleted, lTag)


                    .IsSearchProperty = vArray(pbObjectAndPropertyConsts.ACPIsSearchProperty, lTag)

                    .IsChaseCycleProperty = vArray(pbObjectAndPropertyConsts.ACPIsChaseCycleProperty, lTag)
                    .IndexLinkingId = vArray(pbObjectAndPropertyConsts.ACPIndexLinkingId, lTag)

                    .GISProperty = m_vGISProperty(m_lObjectTag)


                    .IsInMISExport = gPMFunctions.ToSafeLong(vArray(pbObjectAndPropertyConsts.ACPIsInMISExport, lTag), 0)
                    .IsFormattedText = gPMFunctions.ToSafeLong(vArray(pbObjectAndPropertyConsts.ACPIsFormattedText, lTag), 0)
                    .IsClaim360Display = vArray(pbObjectAndPropertyConsts.ACPISClaim360Display, lTag)
                    If .IsClaim360Display <> 1 Then
                        .DisableClaim360Display = False
                        Dim m_Object(,) As Object
                        For Each m_Object In m_vGISProperty

                            If Information.IsArray(m_Object) Then

                                For index As Integer = 0 To m_Object.GetUpperBound(1)

                                    If m_Object(pbObjectAndPropertyConsts.ACPISClaim360Display, index) = 1 Then
                                        .DisableClaim360Display = True
                                        Exit For
                                    End If
                                Next
                            End If
                            If .DisableClaim360Display = True Then
                                Exit For
                            End If
                        Next
                    Else
                        .DisableClaim360Display = False
                    End If
                End If


                m_nReturn = .Start()

                If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    vArray = Nothing
                    Return result
                End If


                If .Status = gPMConstants.PMEReturnCode.PMCancel Then
                    vArray = Nothing
                    Return result
                End If

                If lTask = gPMConstants.PMEComponentAction.PMAdd OrElse lTask = gPMConstants.PMEComponentAction.PMDelete Then
                    m_bSomethingChanged = True
                End If

                If lTask = gPMConstants.PMEComponentAction.PMAdd Then

                    vArray = m_vGISProperty(m_lObjectTag)


                    lTag = vArray.GetUpperBound(1) + 1

                    ReDim Preserve vArray(pbObjectAndPropertyConsts.ACPISClaim360Display, lTag)

                End If


                If vArray(pbObjectAndPropertyConsts.ACPGISPropertyId, lTag) <> .GISPropertyId Then
                    m_bSomethingChanged = True
                End If
                vArray(pbObjectAndPropertyConsts.ACPGISPropertyId, lTag) = .GISPropertyId

                If vArray(pbObjectAndPropertyConsts.ACPGISObjectId, lTag) <> .GISObjectId Then
                    m_bSomethingChanged = True
                End If
                vArray(pbObjectAndPropertyConsts.ACPGISObjectId, lTag) = .GISObjectId

                If vArray(pbObjectAndPropertyConsts.ACPPropertyName, lTag) <> .PropertyName Then
                    m_bSomethingChanged = True
                End If
                vArray(pbObjectAndPropertyConsts.ACPPropertyName, lTag) = .PropertyName

                If vArray(pbObjectAndPropertyConsts.ACPColumnName, lTag) <> .ColumnName Then
                    m_bSomethingChanged = True
                End If
                vArray(pbObjectAndPropertyConsts.ACPColumnName, lTag) = .ColumnName

                If vArray(pbObjectAndPropertyConsts.ACPDataType, lTag) <> .DataType Then
                    m_bSomethingChanged = True
                End If
                vArray(pbObjectAndPropertyConsts.ACPDataType, lTag) = .DataType

                If vArray(pbObjectAndPropertyConsts.ACPIsInputProperty, lTag) <> .IsInputProperty Then
                    m_bSomethingChanged = True
                End If
                vArray(pbObjectAndPropertyConsts.ACPIsInputProperty, lTag) = .IsInputProperty

                If vArray(pbObjectAndPropertyConsts.ACPIsIdentifyingProperty, lTag) <> .IsIdentifyingProperty Then
                    m_bSomethingChanged = True
                End If
                vArray(pbObjectAndPropertyConsts.ACPIsIdentifyingProperty, lTag) = .IsIdentifyingProperty

                If vArray(pbObjectAndPropertyConsts.ACPIsPrimaryKey, lTag) <> .IsPrimaryKey Then
                    m_bSomethingChanged = True
                End If
                vArray(pbObjectAndPropertyConsts.ACPIsPrimaryKey, lTag) = .IsPrimaryKey

                If vArray(pbObjectAndPropertyConsts.ACPEditFlags, lTag) <> .EditFlags Then
                    m_bSomethingChanged = True
                End If
                vArray(pbObjectAndPropertyConsts.ACPEditFlags, lTag) = .EditFlags

                If vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTag) <> .SpecialsType Then
                    m_bSomethingChanged = True
                End If
                vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTag) = .SpecialsType

                If vArray(pbObjectAndPropertyConsts.ACPSpecialsTypeReference, lTag) <> .SpecialsTypeReference Then
                    m_bSomethingChanged = True
                End If
                vArray(pbObjectAndPropertyConsts.ACPSpecialsTypeReference, lTag) = .SpecialsTypeReference

                If vArray(pbObjectAndPropertyConsts.ACPPolarisPropertyId, lTag) <> .PolarisPropertyId Then
                    m_bSomethingChanged = True
                End If
                vArray(pbObjectAndPropertyConsts.ACPPolarisPropertyId, lTag) = .PolarisPropertyId

                If vArray(pbObjectAndPropertyConsts.ACPIsDeleted, lTag) <> .IsDeleted Then
                    m_bSomethingChanged = True
                End If
                vArray(pbObjectAndPropertyConsts.ACPIsDeleted, lTag) = .IsDeleted

                If vArray(pbObjectAndPropertyConsts.ACPIsSearchProperty, lTag) <> .IsSearchProperty Then
                    m_bSomethingChanged = True
                End If
                vArray(pbObjectAndPropertyConsts.ACPIsSearchProperty, lTag) = .IsSearchProperty

                If vArray(pbObjectAndPropertyConsts.ACPIsChaseCycleProperty, lTag) <> .IsChaseCycleProperty Then
                    m_bSomethingChanged = True
                End If
                vArray(pbObjectAndPropertyConsts.ACPIsChaseCycleProperty, lTag) = .IsChaseCycleProperty

                If vArray(pbObjectAndPropertyConsts.ACPIndexLinkingId, lTag) <> .IndexLinkingId Then
                    m_bSomethingChanged = True
                End If
                vArray(pbObjectAndPropertyConsts.ACPIndexLinkingId, lTag) = .IndexLinkingId

                If vArray(pbObjectAndPropertyConsts.ACPIsInMISExport, lTag) <> .IsInMISExport Then
                    m_bSomethingChanged = True
                End If
                vArray(pbObjectAndPropertyConsts.ACPIsInMISExport, lTag) = .IsInMISExport

                If vArray(pbObjectAndPropertyConsts.ACPIsFormattedText, lTag) <> .IsFormattedText Then
                    m_bSomethingChanged = True
                End If
                vArray(pbObjectAndPropertyConsts.ACPIsFormattedText, lTag) = .IsFormattedText
                If vArray(pbObjectAndPropertyConsts.ACPISClaim360Display, lTag) <> .IsClaim360Display Then
                    m_bSomethingChanged = True
                End If
                vArray(pbObjectAndPropertyConsts.ACPISClaim360Display, lTag) = .IsClaim360Display
            End With


            m_vGISProperty(m_lObjectTag) = vArray

            vArray = Nothing

            m_nReturn = PopulateProperties(lTag:=m_lObjectTag)

            Return result

        Catch ex As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CallProperty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CallProperty", vErrNo:=Information.Err().Number, vErrDesc:=ex.Message, excep:=ex)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CreateRegistrySettings
    '
    ' Description:
    '
    ' History: 06/02/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function CreateRegistrySettings() As Integer

        Dim result As Integer = 0
        Dim sDefSubKey, sSubKey, sTemp As String

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            'We don't do this here - we have to do it in the user control instead...

            sDefSubKey = ACOIMGISSubKey & "\" & "DEF" & "\" & "ListManagement"

            sSubKey = ACOIMGISSubKey & "\" & m_sGISDataModel & "\" & "ListManagement"

            'Get list file path
            m_nReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="ClientListFilePath", r_sSettingValue:=sTemp, v_sSubKey:=sSubKey)

            'Already done?
            If sTemp <> "" Then
                Return result
            End If

            'Get list file compressed - not on client
            '    m_lReturn = GetPMRegSetting( _
            'v_lPMERegSettingRoot:=pmeRSRLocalMachine, _
            'v_lPMEProductFamily:=pmePFSiriusSolutions, _
            'v_lPMERegSettingLevel:=pmeRSLClient, _
            'v_sSettingName:="ClientListFileCompressed", _
            'r_sSettingValue:=sTemp, _
            'v_sSubKey:=sDefSubKey)

            'Set list file compressed - not on client
            '    m_lReturn = SetPMRegSetting( _
            'v_lPMERegSettingRoot:=pmeRSRLocalMachine, _
            'v_lPMEProductFamily:=pmePFSiriusSolutions, _
            'v_lPMERegSettingLevel:=pmeRSLClient, _
            'v_sSettingName:="ClientListFileCompressed", _
            'v_sSettingValue:=sTemp, _
            'v_sSubKey:=sSubKey)

            'Get list file path
            m_nReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="ClientListFilePath", r_sSettingValue:=sTemp, v_sSubKey:=sDefSubKey)

            'Set list file path
            m_nReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="ClientListFilePath", v_sSettingValue:=sTemp, v_sSubKey:=sSubKey)

            'Get list pref version
            m_nReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="ClientListPrefVersion", r_sSettingValue:=sTemp, v_sSubKey:=sDefSubKey)

            'Set list pref version
            m_nReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="ClientListPrefVersion", v_sSettingValue:=sTemp, v_sSubKey:=sSubKey)

            'Get list version
            m_nReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="ClientListVersion", r_sSettingValue:=sTemp, v_sSubKey:=sDefSubKey)

            'Set list version
            m_nReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="ClientListVersion", v_sSettingValue:=sTemp, v_sSubKey:=sSubKey)

            Return m_nReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateRegistrySettings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateRegistrySettings", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: LockDataModel
    '
    ' Description:
    '
    ' History: 17/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function LockDataModel() As Integer
        Dim result As Integer = 0


        Dim oPMLock As bPMLock.User
        Dim sLockedBy As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get bPMLock
            Dim temp_oPMLock As Object
            m_nReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMLock = temp_oPMLock

            ' Check for errors.
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMLock", vApp:=ACApp, vClass:=ACClass, vMethod:="LockDataModel", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If


            m_nReturn = oPMLock.LockKey(sKeyName:="data_model", vKeyValue:=m_lGISDataModelId, iUserID:=g_oObjectManager.UserID, sCurrentlyLockedBy:=sLockedBy)


            Select Case m_nReturn
                Case gPMConstants.PMEReturnCode.PMTrue
                    'OK

                Case gPMConstants.PMEReturnCode.PMFalse
                    'Locked or error
                    If sLockedBy = "ERROR" Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error trying to lock record", vApp:=ACApp, vClass:=ACClass, vMethod:="LockDataModel", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    Else
                        result = gPMConstants.PMEReturnCode.PMMAlreadyInUse 'PN20948
                        MessageBox.Show("Data Model currently locked by " & sLockedBy & _
                                        Strings.Chr(13) & Strings.Chr(10) & "Please try later", "Lock Data Model")
                        Return result
                    End If


                Case Else
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lock the model", vApp:=ACApp, vClass:=ACClass, vMethod:="LockDataModel", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result

            End Select

            oPMLock = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LockDataModel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LockDataModel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UnlockDataModel
    '
    ' Description:
    '
    ' History: 17/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function UnlockDataModel() As Integer
        Dim result As Integer = 0


        Dim oPMLock As bPMLock.User

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get bPMLock
            Dim temp_oPMLock As Object
            m_nReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMLock = temp_oPMLock

            ' Check for errors.
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMLock", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockDataModel", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If


            m_nReturn = oPMLock.UnLockKey(sKeyName:="data_model", vKeyValue:=m_lGISDataModelId, iUserID:=g_oObjectManager.UserID)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to unlock the data model", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockDataModel", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result

            End If

            oPMLock = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnlockDataModel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockDataModel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: PrintDataModel
    '
    ' Description:
    '
    ' History: 05/01/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function PrintDataModel() As Integer

        Dim result As Integer = 0
        Dim nodTop As TreeNode
        Dim sTemp As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    iFreeFile = FreeFile()
            '
            '    Open "D:\Temp\Output.prn" For Output As #iFreeFile
            '
            '    Print #iFreeFile, "Data Model Code: " & m_sGISDataModel
            '    Print #iFreeFile, "Data Model Description: " & m_sGISDataModelDescription
            '
            '    Close #iFreeFile


            'Modified by Archana Tokas on 31/03/2010 05:46:15 nosolution found todolist
            'dlgPrint.CancelError = True
            If DialogResult.Yes = dlgPrintPrint.ShowDialog() Then
                PrinterHelper.Printer.FontName = "Arial"
                PrinterHelper.Printer.FontSize = 10

                PrinterHelper.Printer.FontUnderline = True
                PrinterHelper.Printer.Print(True, "Data Model Code:")
                PrinterHelper.Printer.FontUnderline = False
                PrinterHelper.Printer.Print(True, " " & m_sGISDataModel & Strings.Chr(9) & Strings.Chr(9))
                PrinterHelper.Printer.FontUnderline = True
                PrinterHelper.Printer.Print(True, "Data Model Description:")
                PrinterHelper.Printer.FontUnderline = False
                PrinterHelper.Printer.Print(" " & m_sGISDataModelDescription)

                PrinterHelper.Printer.Print("")

                nodTop = tvwObjects.Nodes.Item(0)

                m_nReturn = PrintNode(nodNode:=nodTop, lLevel:=0)

                PrinterHelper.Printer.EndDoc() ' Printing is finished.

            Else
                Return result
            End If



            Return result

        Catch excep As System.Exception



            If Information.Err().Number = 32755 Then
                Return result
            End If

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PrintDataModel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintDataModel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: PrintNode
    '
    ' Description:
    '
    ' History: 05/01/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function PrintNode(ByRef nodNode As TreeNode, ByRef lLevel As Integer) As Integer

        Dim result As Integer = 0
        Dim lTemp, lTemp2, lDataType As Integer
        Dim sTemp As String = ""
        Dim vArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            PrinterHelper.Printer.Print("")


            lTemp = CInt(Convert.ToString(ACTagGisObjectId))

            PrinterHelper.Printer.FontUnderline = True
            PrinterHelper.Printer.Print(True, FileSystem.TAB(10 + (lLevel * 5)), "Object:")
            PrinterHelper.Printer.FontUnderline = False
            PrinterHelper.Printer.Print(True, " " & CStr(m_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lTemp)))
            PrinterHelper.Printer.FontUnderline = True
            PrinterHelper.Printer.Print(True, FileSystem.TAB(50 + (lLevel * 5)), "Table:")
            PrinterHelper.Printer.FontUnderline = False
            PrinterHelper.Printer.Print(" " & CStr(m_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)))

            PrinterHelper.Printer.FontUnderline = True
            'Modified by Archana Tokas on 30/03/2010 05:57:30 instead of & expression is expected todolist
            'PrinterHelper.Printer.Print(FileSystem.TAB(10 + (lLevel * 5)), "Property", &, FileSystem.TAB(40 + (lLevel * 5)), "Column", &, FileSystem.TAB(70 + (lLevel * 5)), "Description")
            PrinterHelper.Printer.Print(TAB(10 + (lLevel * 5)), "Property", TAB(40 + (lLevel * 5)), "Column", TAB(70 + (lLevel * 5)), "Description")
            PrinterHelper.Printer.FontUnderline = False
            PrinterHelper.Printer.Print()


            vArray = m_vGISProperty(lTemp)

            If Information.IsArray(vArray) Then


                For lTemp2 = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)


                    If CDbl(vArray(pbObjectAndPropertyConsts.ACPIsPrimaryKey, lTemp2)) = 0 Then


                        lDataType = CInt(vArray(pbObjectAndPropertyConsts.ACPDataType, lTemp2))

                        Select Case lDataType
                            Case GISDataTypeText
                                sTemp = "Text"
                            Case GISDataTypeNumeric
                                sTemp = "Number"
                                Select Case vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp2)
                                    Case GISSharedPropertyConstants.ACOSpecialNone
                                        sTemp = "Number"
                                    Case GISSharedPropertyConstants.ACOGISListID

                                        sTemp = "ACOGISListID " & CStr(vArray(pbObjectAndPropertyConsts.ACPSpecialsTypeReference, lTemp2))
                                    Case GISSharedPropertyConstants.ACOPMLookupTableName

                                        sTemp = "ACOPMLookupTableName " & CStr(vArray(pbObjectAndPropertyConsts.ACPSpecialsTypeReference, lTemp2))
                                    Case GISSharedPropertyConstants.ACOComboLookup

                                        sTemp = "ACOComboLookupTableName " & CStr(vArray(pbObjectAndPropertyConsts.ACPSpecialsTypeReference, lTemp2))
                                    Case GISSharedPropertyConstants.ACOPartyTypeID

                                        sTemp = "ACOPartyTypeID " & CStr(vArray(pbObjectAndPropertyConsts.ACPSpecialsTypeReference, lTemp2))
                                    Case GISSharedPropertyConstants.ACOSumInsuredTypeID

                                        sTemp = "ACOSumInsuredTypeID " & CStr(vArray(pbObjectAndPropertyConsts.ACPSpecialsTypeReference, lTemp2))
                                    Case GISSharedPropertyConstants.ACOStdWordingType

                                        sTemp = "ACOStdWordingType " & CStr(vArray(pbObjectAndPropertyConsts.ACPSpecialsTypeReference, lTemp2))
                                    Case GISSharedPropertyConstants.ACOGISUserDefHeaderID

                                        sTemp = "ACOGISUserDefHeaderID " & CStr(vArray(pbObjectAndPropertyConsts.ACPSpecialsTypeReference, lTemp2))
                                    Case GISSharedPropertyConstants.ACOProductID

                                        sTemp = "ACOProductID " & CStr(vArray(pbObjectAndPropertyConsts.ACPSpecialsTypeReference, lTemp2))
                                    Case GISSharedPropertyConstants.ACOReserveID

                                        sTemp = "ACOReserveID " & CStr(vArray(pbObjectAndPropertyConsts.ACPSpecialsTypeReference, lTemp2))
                                    Case GISSharedPropertyConstants.ACOPaymentID

                                        sTemp = "ACOPaymentID " & CStr(vArray(pbObjectAndPropertyConsts.ACPSpecialsTypeReference, lTemp2))
                                    Case Else
                                        sTemp = "Number"
                                End Select
                            Case GISDataTypeDate
                                sTemp = "Date"
                            Case GISDataTypeOption
                                sTemp = "Boolean"
                            Case GISDataTypeCurrency
                                sTemp = "Currency"
                            Case GISDataTypeInteger
                                sTemp = "Integer"
                            Case GISDataTypePercentage
                                sTemp = "Percentage"
                            Case GISDataTypecode
                                sTemp = "G2Integer"
                            Case Else
                                sTemp = "Unknown"
                        End Select

                        'Modified by Archana Tokas on 30/03/2010 05:57:30 instead of & expression is expected todolist
                        'PrinterHelper.Printer.Print(FileSystem.TAB(10 + (lLevel * 5)), vArray(pbObjectAndPropertyConsts.ACPPropertyName, lTemp2), &, FileSystem.TAB(40 + (lLevel * 5)), vArray(pbObjectAndPropertyConsts.ACPColumnName, lTemp2), &, FileSystem.TAB(70 + (lLevel * 5)), sTemp)
                        PrinterHelper.Printer.Print(FileSystem.TAB(10 + (lLevel * 5)), vArray(pbObjectAndPropertyConsts.ACPPropertyName, lTemp2), FileSystem.TAB(40 + (lLevel * 5)), vArray(pbObjectAndPropertyConsts.ACPColumnName, lTemp2), FileSystem.TAB(70 + (lLevel * 5)), sTemp)
                    End If
                Next lTemp2
            End If
            PrinterHelper.Printer.Print("")

            If nodNode.GetNodeCount(False) > 0 Then

                lTemp2 = nodNode.FirstNode.Index

                'TODO. string can not be converted to treenode
                'm_lReturn = PrintNode(tvwObjects.Nodes.Item(lTemp2 - 1).Text, lLevel + 1)
                m_nReturn = PrintNode(nodNode:=tvwObjects.Nodes.Item(lTemp2 - 1), lLevel:=lLevel + 1)


                'developer guide no.34
                While lTemp2 <> nodNode.FirstNode.LastNode.Index

                    lTemp2 = tvwObjects.Nodes.Item(lTemp2 - 1).NextNode.Index

                    'TODO string can not be converted to treenode
                    'm_lReturn = PrintNode(tvwObjects.Nodes.Item(lTemp2 - 1).Text, lLevel + 1)
                    m_nReturn = PrintNode(nodNode:=tvwObjects.Nodes.Item(lTemp2 - 1), lLevel:=lLevel + 1)
                End While

            End If

            Return result

        Catch excep As System.Exception



            If Information.Err().Number = 32755 Then
                Return result
            End If

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PrintNode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintNode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' PN28024
    Private Sub chkAccessibleViaSAM_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkAccessibleViaSAM.CheckStateChanged

        If Me.Visible Then m_bSomethingChanged = True

    End Sub

    Private Sub chkShowKeys_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkShowKeys.CheckStateChanged
        m_nReturn = PopulateProperties(lTag:=m_lObjectTag)
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            If m_bSomethingChanged Then

                ' Set the interface status.
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel

                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_nReturn = m_oGeneral.ProcessCommand()
            Else
                m_nReturn = gPMConstants.PMEReturnCode.PMTrue
            End If
            ' Check the return value.
            If m_nReturn = gPMConstants.PMEReturnCode.PMTrue Then

                'rollback and kill global transaction

                m_oBusinessGlobalTransaction.RollbackTrans()

                m_oBusinessGlobalTransaction.Dispose()
                m_oBusinessGlobalTransaction = Nothing

                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdList_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdList.Click


        Dim oObject As iPMUListMaint.Interface_Renamed

        Try

            Dim temp_oObject As Object
            m_nReturn = g_oObjectManager.GetInstance(temp_oObject, sClassName:="iPMUListMaint.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oObject = temp_oObject

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            If oObject Is Nothing Then
                Exit Sub
            End If


            oObject.GISDataModelCode = m_sGISDataModel


            oObject.Task = m_iTask 'MKW010803 PN4514


            m_nReturn = oObject.Start


            oObject.Dispose()

            oObject = Nothing

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the List command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdList_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            m_nReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_nReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdObjectAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdObjectAdd.Click


        Dim lTag As Integer = -1

        m_nReturn = CallObject(lTag:=lTag, lTask:=gPMConstants.PMEComponentAction.PMAdd)


        If m_oGISObject.Status = gPMConstants.PMEReturnCode.PMOK Then
            m_lStatus = gPMConstants.PMEReturnCode.PMRecordChanged 'signal single object

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_oGeneral.ProcessCommand()
        End If

        'CMG/PB 16082002 Disable Property Add Button to stop error when clicked
        cmdPropertyAdd.Enabled = False

    End Sub

    Private Sub cmdObjectEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdObjectEdit.Click


        '(1068) tvwObjects.SelectedItem.Tag() of type Variant is being forced to Integer. More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
        Dim lTag As Integer = CInt(tvwObjects.SelectedNode.Tag(ACTagGisObjectId)) 'CInt(Convert.ToString(ACTagGisObjectId))

        'MKW010803 PN4514 START
        If m_iTask = gPMConstants.PMEComponentAction.PMView Then
            m_nReturn = CallObject(lTag:=lTag, lTask:=gPMConstants.PMEComponentAction.PMView)
        Else
            m_nReturn = CallObject(lTag:=lTag, lTask:=gPMConstants.PMEComponentAction.PMEdit)
        End If
        'MKW010803 PN4514 END


        If m_oGISObject.Status = gPMConstants.PMEReturnCode.PMOK Then
            m_lStatus = gPMConstants.PMEReturnCode.PMRecordChanged 'signal single object

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_oGeneral.ProcessCommand()
        End If
    End Sub
    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Check mandatory controls have been entered into.
            m_nReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            m_nReturn = ValidateForm()

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            m_nReturn = CheckObjects()

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            If m_nReturn = gPMConstants.PMEReturnCode.PMTrue Then

                m_oBusinessGlobalTransaction.CommitTrans()
            End If

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_nReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_nReturn = gPMConstants.PMEReturnCode.PMTrue Then

                m_oBusinessGlobalTransaction.Dispose()
                m_oBusinessGlobalTransaction = Nothing

                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdPrint_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPrint.Click

        m_nReturn = PrintDataModel()

    End Sub

    Private Sub cmdPropertyAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPropertyAdd.Click
        Dim lTag As Integer
        m_nReturn = CallProperty(lTag:=0, lTask:=gPMConstants.PMEComponentAction.PMAdd)
    End Sub
    Private Sub cmdPropertyDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPropertyDelete.Click

        Dim idelete As DialogResult

        If m_lObjectTag = -1 Then
            Exit Sub
        End If

        Dim lTag As Integer = Convert.ToString(lvwProperties.FocusedItem.Tag)

        'show message if property has already been saved
        If CDbl(m_vGISProperty(m_lObjectTag)(pbObjectAndPropertyConsts.ACPGISPropertyId, lTag)) <> -1 Then
            ' Issue 765 (CMG\SJP) 03-10-2002
            ' If property has been saved to database, should no longer be possible to delete it, as this
            ' will upset the data model import export facility.  A deleted property could be re-added as a
            ' different format of column which raises data problems in the target database when importing.
            MessageBox.Show("This property has already been saved to database and can not be deleted.", "Delete Property", MessageBoxButtons.OK, MessageBoxIcon.Information)
            'idelete = MsgBox("This property will be removed from any forms which it appears on." & vbNewLine & vbNewLine & "You must manually remove references to this property from any scripts and reports." & vbNewLine & vbNewLine & "Do you wish to continue?", vbOKCancel + vbQuestion, "Delete Property")
            Exit Sub
        End If
        If idelete <> System.Windows.Forms.DialogResult.Cancel Then
            m_vGISProperty(m_lObjectTag)(pbObjectAndPropertyConsts.ACPPropertyName, lTag) = "dElEtEd"

            PopulateProperties(m_lObjectTag)

            m_bSomethingChanged = True
        End If

    End Sub
    Private Sub cmdPropertyEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPropertyEdit.Click


        If m_lObjectTag = -1 Or lvwProperties.Items.Count <= 0 Then
            Exit Sub
        End If


        Dim lTag As Integer = Convert.ToString(lvwProperties.FocusedItem.Tag)

        'MKW010803 PN4514 START
        If m_iTask = gPMConstants.PMEComponentAction.PMView Then
            m_nReturn = CallProperty(lTag:=lTag, lTask:=gPMConstants.PMEComponentAction.PMView)
        Else
            m_nReturn = CallProperty(lTag:=lTag, lTask:=gPMConstants.PMEComponentAction.PMEdit)
        End If
        'MKW010803 PN4514 END
    End Sub
    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
            'If m_lGISDataModelType = GISDataModelType.GISDMTypeNotSet Then
            '    MessageBox.Show("Select a Data Model type before editing the model.", "GIS Object", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '    Me.Close()
            'End If

        End If
    End Sub
    Private Sub Form_Initialize_Renamed()

        Dim vTemp As String = "" 'PN28024

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            '    ' Get an instance of the business object via
            '    ' the public object manager.
            '    m_lReturn& = g_oObjectManager.GetInstance( _
            ''        oObject:=m_oBusiness, _
            ''        sClassName:="bSIRMaintainScreenData.Business", _
            ''        vInstanceManager:=PMGetViaClientManager)
            '
            '    ' Check for errors.
            '    If m_lReturn <> PMTrue Then
            '        ' Failed to get an instance of the business object.
            '        m_lErrorNumber& = PMFalse
            '
            '        ' Display error stating the problem.
            '
            '        ' Get description from the resource file.
            '        sTitle$ = iPMFunc.GetResData( _
            ''            iLangID:=g_iLanguageID%, _
            ''            lID:=ACBusinessFailTitle, _
            ''            iDataType:=PMResString)
            '
            '        sMessage$ = iPMFunc.GetResData( _
            ''            iLangID:=g_iLanguageID%, _
            ''            lID:=ACBusinessFail, _
            ''            iDataType:=PMResString)
            '
            '        ' Display message.
            '        MsgBox sMessage$, vbCritical, sTitle$
            '
            '        Exit Sub
            '    End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iGISMaintainDataDictionary.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_nReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

            ' Check for errors.
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            '    m_lStatus& = PMCancel

            ' Get the AccessibleViaSAM hidden option. PN28024.
            m_nReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTAllowDataModelSAMAccess, v_vBranch:=m_iSourceId, r_vUnderwriting:=vTemp)

            If m_nReturn = gPMConstants.PMEReturnCode.PMTrue Then
                If vTemp = "1" Then
                    m_bAccessibleViaSAMOption = True
                End If
            End If

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



        'try and force window to be visible
        'iPMFunc.ForceForegroundWindow(Me.Handle.ToInt32())

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
        '    m_lReturn& = m_oBusiness.SetProcessModes( _
        ''        vTask:=CVar(m_iTask%), _
        ''        vNavigate:=CVar(m_lNavigate&), _
        ''        vProcessMode:=CVar(m_lProcessMode&), _
        ''        vTransactionType:=CVar(m_sTransactionType$), _
        ''        vEffectiveDate:=CVar(m_dtEffectiveDate))
        '
        '    ' Check for errors.
        '    If m_lReturn <> PMTrue Then
        '        ' Failed to process the interface.
        '        m_lErrorNumber& = PMFalse
        '
        '        ' Log Error Message
        '        LogMessage _
        ''            iType:=PMLogOnError, _
        ''            sMsg:="Failed to set the process modes for the business object", _
        ''            vApp:=ACApp, _
        ''            vClass:=ACClass, _
        ''            vMethod:="Form_Load"
        '
        '        Exit Sub
        '    End If
        '
        '    ' Set the business keys.
        '    ' {* USER DEFINED CODE (Begin) *}
        '    ' {* USER DEFINED CODE (End) *}


        Dim temp_m_oBusinessGlobalTransaction As Object
        m_nReturn = g_oObjectManager.GetInstance(temp_m_oBusinessGlobalTransaction, "bGISMaintainDataDictionary.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        m_oBusinessGlobalTransaction = temp_m_oBusinessGlobalTransaction

        'm_oBusinessGlobalTransaction.BeginTrans

        'Lock the data model
        m_nReturn = LockDataModel()

        If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lErrorNumber = m_nReturn 'PN20948

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Exit Sub
        End If

        'signal if we are in Swift integration mode

        Try

            m_oBusinessGlobalTransaction.SwiftIntegration = m_lSwiftIntegration
            If Information.Err().Number = 429 Then
                MessageBox.Show("Cannot run in Swift Integration Mode (iSWSirius.CProductBuilder)", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error)
                m_lSwiftIntegration = 0
            End If



            m_oFormFields = New iPMFormControl.FormFields()

            ' Validate fields using Forms Control
            m_nReturn = SetFieldValidation()
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the interface default values.
            m_nReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Gets the interface details to be displayed.
            m_nReturn = m_oGeneral.GetInterfaceDetails()



            ' Check for errors.
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the mouse pointer to normal.
            'iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Exit Sub


Err_FormLoad:

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Exit Sub


        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
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

                If m_bSomethingChanged Then
                    m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                End If

                m_nReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    ' developer guide no. 7
                    eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()
            'Unlock the data model
            m_nReturn = UnlockDataModel()

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            End If

            'if here and still have a m_oBusinessGlobalTransaction then rollback
            If Not (m_oBusinessGlobalTransaction Is Nothing) Then

                m_oBusinessGlobalTransaction.RollbackTrans()

                m_oBusinessGlobalTransaction.Dispose()
                m_oBusinessGlobalTransaction = Nothing
            End If

            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            If Not (m_oBusiness Is Nothing) Then
                ' Terminate the business object

                m_oBusiness.Dispose()
                ' Destroy the instance of the business object
                ' from memory.
                m_oBusiness = Nothing
            End If

            If Not (m_oGISObject Is Nothing) Then
                ' Terminate the GIS object object

                m_oGISObject.Dispose()
                ' Destroy the instance of the GIS object object
                ' from memory.
                m_oGISObject = Nothing
            End If

            If Not (m_oGISProperty Is Nothing) Then
                ' Terminate the GIS property object

                m_oGISProperty.Dispose()

                ' Destroy the instance of the GIS property object
                ' from memory.
                m_oGISProperty = Nothing
            End If

            If Not (m_oFormFields Is Nothing) Then

                ' Terminate the form control object.
                m_oFormFields.Dispose()
                ' Destroy the instance of the form control object
                ' from memory.
                m_oFormFields = Nothing

            End If

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


            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
        Catch




            Exit Sub
        End Try


    End Sub

    Private Sub imgSplitterV_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles imgSplitterV.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        'imgSplitterV.Height = tvwObjects.Height

        With imgSplitterV
            picSplitterV.SetBounds(.Left, .Top, VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(.Width) \ 2), VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(.Height) - 20))
        End With

        picSplitterV.Visible = True
        m_bMoving = True

    End Sub


    Private Sub imgSplitterV_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles imgSplitterV.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        Dim sglPos As Single

        Dim sglSplitLimit As Single = 3000

        If m_bMoving Then
            sglPos = x + VB6.PixelsToTwipsX(imgSplitterV.Left)
            If sglPos < sglSplitLimit Then
                picSplitterV.Left = VB6.TwipsToPixelsX(sglSplitLimit)
            ElseIf sglPos > VB6.PixelsToTwipsX(Me.Width) - sglSplitLimit Then
                picSplitterV.Left = Me.Width - VB6.TwipsToPixelsX(sglSplitLimit)
            Else
                picSplitterV.Left = VB6.TwipsToPixelsX(sglPos)
            End If
        End If

    End Sub

    Private Sub imgSplitterV_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles imgSplitterV.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        'imgSplitter.Height = tvwObjects.Height
        tvwObjects.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(picSplitterV.Left) - VB6.PixelsToTwipsX(tvwObjects.Left) - 50)
        picSplitterH.Width = tvwObjects.Width
        imgSplitterH.Width = tvwObjects.Width

        lvwProperties.Left = picSplitterV.Left + VB6.TwipsToPixelsX(50)
        lvwProperties.Width = VB6.TwipsToPixelsX(13335) - lvwProperties.Left

        imgSplitterV.Left = picSplitterV.Left
        lblProperties.Left = lvwProperties.Left
        cmdPropertyAdd.Left = lvwProperties.Left
        cmdPropertyEdit.Left = lvwProperties.Left + VB6.TwipsToPixelsX(1200)
        cmdPropertyDelete.Left = lvwProperties.Left + VB6.TwipsToPixelsX(2400)

        picSplitterV.Visible = False
        m_bMoving = False

    End Sub


    Private Sub lvwProperties_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwProperties.Click
        Dim lEditFlags As Integer
        Dim bPrimaryKey As Boolean

        'AK 250603 - attempt to any of this only if list view has got any member
        If lvwProperties.Items.Count > 0 Then
            'lEditFlags = CInt(Conversion.Val(CStr(m_vGISProperty(m_lObjectTag)(pbObjectAndPropertyConsts.ACPEditFlags, Convert.ToString(lvwProperties.FocusedItem.Tag)))))
            'bPrimaryKey = Conversion.Val(CStr(m_vGISProperty(m_lObjectTag)(pbObjectAndPropertyConsts.ACPIsPrimaryKey, Convert.ToString(lvwProperties.FocusedItem.Tag)))) <> 0
            lEditFlags = CInt(m_vGISProperty(m_lObjectTag)(pbObjectAndPropertyConsts.ACPEditFlags, Convert.ToString(lvwProperties.FocusedItem.Tag)))
            bPrimaryKey = CStr(m_vGISProperty(m_lObjectTag)(pbObjectAndPropertyConsts.ACPIsPrimaryKey, Convert.ToString(lvwProperties.FocusedItem.Tag))) <> 0

            If cmdPropertyEdit.Enabled Then
                If (lEditFlags And GISSharedPropertyConstants.GISDSEditReadOnly) Or bPrimaryKey Or cmdObjectEdit.Text = "View" Then
                    cmdPropertyEdit.Text = "View"
                Else
                    cmdPropertyEdit.Text = "Edit"

                    If Not bPrimaryKey Then
                        cmdPropertyDelete.Enabled = m_bCanEditObject
                        Exit Sub
                    End If
                End If
            End If
        End If
        cmdPropertyDelete.Enabled = False
    End Sub

    Private Sub lvwProperties_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwProperties.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwProperties.Columns(eventArgs.Column)
        lvwProperties_Click(lvwProperties, New EventArgs())
    End Sub

    Private Sub lvwProperties_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwProperties.DoubleClick
        lvwProperties_Click(lvwProperties, New EventArgs())
        cmdPropertyEdit_Click(cmdPropertyEdit, New EventArgs())
    End Sub

    'Private Sub lvwProperties_ItemClick(ByVal Item As ListViewItem)
    '    lvwProperties_Click(lvwProperties, New EventArgs())
    'End Sub

    Private Sub tvwObjects_AfterCollapse(ByVal eventSender As Object, ByVal eventArgs As TreeViewEventArgs) Handles tvwObjects.AfterCollapse
        Dim Node As TreeNode = eventArgs.Node

        Node.ImageKey = ACClosedFolder


    End Sub

    Private Sub tvwObjects_AfterExpand(ByVal eventSender As Object, ByVal eventArgs As TreeViewEventArgs) Handles tvwObjects.AfterExpand
        Dim Node As TreeNode = eventArgs.Node

        Node.ImageKey = ACOpenFolder

    End Sub

    Private Sub tvwObjects_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles tvwObjects.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        ' developer guide no. 74
        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)
        If tvwObjects.GetNodeAt(x, y) Is Nothing Then
            ' Nothing selected
            cmdObjectEdit.Enabled = False
        Else
            'Not if we're viewing...
            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                cmdObjectEdit.Enabled = True
            End If
            tvwObjects.SelectedNode = tvwObjects.GetNodeAt(x, y)
        End If

    End Sub

    Private Sub lvwProperties_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwProperties.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        ' developer guide no. 74
        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)

        Dim bSelectable As Boolean

        If Not (lvwProperties.GetItemAt(x, y) Is Nothing) Then
            bSelectable = True
        End If

        If Not bSelectable Then
            ' Nothing selected
            cmdPropertyEdit.Enabled = False
            cmdPropertyDelete.Enabled = False
        Else
            'Not if we're viewing...
            '        If (m_iTask <> pmview) Then
            'And not if there's no parent, i.e. it's the policy binder

            If Not (Convert.IsDBNull(m_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, m_lObjectTag)) Or IsNothing(m_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, m_lObjectTag))) Or (m_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, m_lObjectTag) = 0) Then
                cmdPropertyEdit.Enabled = True
            End If
            '        End If
            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                cmdPropertyDelete.Enabled = False
            End If
        End If
    End Sub

    Private Sub imgSplitterH_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles imgSplitterH.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)


        With imgSplitterH
            picSplitterH.SetBounds(.Left, .Top, .Width, .Height)
        End With

        picSplitterH.Visible = True
        m_bMoving = True
        m_lMOuseOffset = CInt(y)

    End Sub


    Private Sub imgSplitterH_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles imgSplitterH.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        Dim sglPos As Single

        Dim sglSplitLimit As Single = 2500

        If m_bMoving Then
            sglPos = y - m_lMOuseOffset + VB6.PixelsToTwipsY(imgSplitterH.Top)
            If sglPos < sglSplitLimit Then
                picSplitterH.Top = VB6.TwipsToPixelsY(sglSplitLimit)
            ElseIf sglPos > VB6.PixelsToTwipsY(Me.Height) - sglSplitLimit Then
                picSplitterH.Top = Me.Height - VB6.TwipsToPixelsY(sglSplitLimit)
            Else
                picSplitterH.Top = VB6.TwipsToPixelsY(sglPos)
            End If
        End If

    End Sub

    Private Sub imgSplitterH_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles imgSplitterH.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        tvwObjects.Height = picSplitterH.Top - tvwObjects.Top

        imgSplitterH.Top = picSplitterH.Top
        picSplitterH.Visible = False
        m_bMoving = False

    End Sub

    ' ***************************************************************** '
    '
    ' Name: PopulateRiskObjectsListView
    '
    ' Description:
    '
    ' History: 18/12/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Private Function PopulateRiskObjectsListView(Optional ByVal lTag As Integer = 0) As Integer

        ' Debug message
        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".PopulateRiskObjectsListView")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            tvwObjects.Nodes.Clear()
            m_nReturn = PopulateObjects(tvwObjects, m_lGISDataModelType, lTag)

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".PopulateRiskObjectsListView")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".PopulateRiskObjectsListView")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateRiskObjectsListView Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateRiskObjectsListView", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: AddProperty
    '
    ' Description:
    '
    ' History: 15/01/2003 CLG - Created.
    '
    ' ***************************************************************** '
    'Modified by Archana Tokas on 30/03/2010 05:57:30 the parameters declared as gPMConstants.PMEReturnCode type are declared as varient in vb changes to be checked at run time todolist
    'Private Function AddProperty(ByRef r_lTemp As Integer, ByRef r_vArray(,) As gPMConstants.PMEReturnCode, ByVal v_lGISPropertyId As Integer, ByVal v_lGISObjectId As Integer, ByVal v_sPropertyName As String, ByVal v_sColumnName As String, ByVal v_lDataType As Integer, ByVal v_lIsInputProperty As Integer, ByVal v_lIsIdentifyingProperty As Integer, ByVal v_lIsPrimaryKey As Integer, ByVal v_lEditFlags As Integer, ByVal v_lSpecialsType As Integer, ByVal v_vSpecialsTypeReference As gPMConstants.PMEReturnCode, ByVal v_vPolarisPropertyId As gPMConstants.PMEReturnCode, ByVal v_vIsDeleted As gPMConstants.PMEReturnCode, ByVal v_vIsSearchProperty As gPMConstants.PMEReturnCode, ByVal v_vIndexLinkingId As gPMConstants.PMEReturnCode) As Integer
    Private Function AddProperty(ByRef r_lTemp As Integer, ByRef r_vArray(,) As Object, ByVal v_lGISPropertyId As Integer, ByVal v_lGISObjectId As Integer, ByVal v_sPropertyName As String, ByVal v_sColumnName As String, ByVal v_lDataType As Integer, ByVal v_lIsInputProperty As Integer, ByVal v_lIsIdentifyingProperty As Integer, ByVal v_lIsPrimaryKey As Integer, ByVal v_lEditFlags As Integer, ByVal v_lSpecialsType As Integer, ByVal v_vSpecialsTypeReference As Object, ByVal v_vPolarisPropertyId As Object, ByVal v_vIsDeleted As gPMConstants.PMEReturnCode, ByVal v_vIsSearchProperty As Object, ByVal v_vIndexLinkingId As Object, Optional ByVal v_vIsChaseCycle As Object = Nothing, Optional ByVal v_vISClaim360Display As Object = Nothing) As Integer

        ' Debug message
        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".AddProperty")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_lTemp += 1
            If r_lTemp = 0 Then
                ReDim r_vArray(pbObjectAndPropertyConsts.ACPLastElement, r_lTemp)
            Else
                ReDim Preserve r_vArray(pbObjectAndPropertyConsts.ACPLastElement, r_lTemp)
            End If

            If v_sColumnName = "" Then
                v_sColumnName = v_sPropertyName
            End If


            r_vArray(pbObjectAndPropertyConsts.ACPGISPropertyId, r_lTemp) = v_lGISPropertyId

            r_vArray(pbObjectAndPropertyConsts.ACPGISObjectId, r_lTemp) = v_lGISObjectId

            r_vArray(pbObjectAndPropertyConsts.ACPPropertyName, r_lTemp) = v_sPropertyName

            r_vArray(pbObjectAndPropertyConsts.ACPColumnName, r_lTemp) = v_sColumnName

            r_vArray(pbObjectAndPropertyConsts.ACPDataType, r_lTemp) = v_lDataType

            r_vArray(pbObjectAndPropertyConsts.ACPIsInputProperty, r_lTemp) = v_lIsInputProperty

            r_vArray(pbObjectAndPropertyConsts.ACPIsIdentifyingProperty, r_lTemp) = v_lIsIdentifyingProperty

            r_vArray(pbObjectAndPropertyConsts.ACPIsPrimaryKey, r_lTemp) = v_lIsPrimaryKey

            r_vArray(pbObjectAndPropertyConsts.ACPEditFlags, r_lTemp) = v_lEditFlags

            r_vArray(pbObjectAndPropertyConsts.ACPSpecialsType, r_lTemp) = v_lSpecialsType

            r_vArray(pbObjectAndPropertyConsts.ACPSpecialsTypeReference, r_lTemp) = v_vSpecialsTypeReference

            r_vArray(pbObjectAndPropertyConsts.ACPPolarisPropertyId, r_lTemp) = v_vPolarisPropertyId

            r_vArray(pbObjectAndPropertyConsts.ACPIsDeleted, r_lTemp) = v_vIsDeleted

            r_vArray(pbObjectAndPropertyConsts.ACPIsSearchProperty, r_lTemp) = v_vIsSearchProperty

            r_vArray(pbObjectAndPropertyConsts.ACPIndexLinkingId, r_lTemp) = v_vIndexLinkingId

            r_vArray(pbObjectAndPropertyConsts.ACPIsChaseCycleProperty, r_lTemp) = v_vIsChaseCycle
            r_vArray(pbObjectAndPropertyConsts.ACPISClaim360Display, r_lTemp) = v_vISClaim360Display


            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".AddProperty")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".AddProperty")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddProperty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddProperty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub tvwObjects_AfterSelect(ByVal eventSender As Object, ByVal eventArgs As TreeViewEventArgs) Handles tvwObjects.AfterSelect

        Dim Node As TreeNode = eventArgs.Node


        '(1068) Node.Tag() of type Variant is being forced to Integer. More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
        Dim lTag As Integer = Node.Tag(ACTagGisObjectId)   'CInt(Convert.ToString(ACTagGisObjectId))
        '(1068) Node.Tag() of type Variant is being forced to Integer. More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
        'm_lGISDataModelType = Node.Tag(ACTagGisObjectId)  'CInt(Convert.ToString(ACTagGisModelType))
        m_lGISDataModelType = Node.Tag(ACTagGisModelType)  'CInt(Convert.ToString(ACTagGisModelType))

        m_nReturn = PopulateProperties(lTag:=lTag)

        m_lObjectTag = lTag

        '(1049) Use of Null/IsNull() detected. More Information: http://www.vbtonet.com/ewis/ewi1049.aspx
        If Convert.IsDBNull(m_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTag)) Or IsNothing(m_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTag)) Or (m_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTag) = 0) Then

            cmdObjectEdit.Enabled = False
            cmdPropertyAdd.Enabled = False
        Else

            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                m_bCanEditObject = False
            Else
                m_bCanEditObject = Not ((CBool(m_vGISObject(pbObjectAndPropertyConsts.ACOEditFlags, lTag)) And GISSharedPropertyConstants.GISDSEditReadOnly) = GISSharedPropertyConstants.GISDSEditReadOnly)
            End If

            cmdObjectEdit.Enabled = True
            cmdObjectEdit.Text = IIf((m_bCanEditObject And m_lSwiftIntegration = 0), "Edit", "View")
            cmdPropertyEdit.Text = IIf(m_bCanEditObject, "Edit", "View")

            cmdPropertyAdd.Enabled = m_bCanEditObject
        End If

        cmdPropertyEdit.Enabled = False
        cmdPropertyDelete.Enabled = False

    End Sub



    ' ***************************************************************** '
    '
    ' Name: CheckObjects
    '
    ' Description:
    '
    ' History: 17/03/2003 CLG - Created.
    '
    ' ***************************************************************** '
    Private Function CheckObjects() As Integer

        Dim result As Integer = 0
        Dim sObjectName As String = ""
        Dim sPropertyName As String = ""
        Dim iCount As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'sanity check the objects
            For lObjectCount As Integer = 1 To m_vGISObject.GetUpperBound(1)
                sObjectName = m_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lObjectCount)
                sPropertyName = ""
                result = gPMConstants.PMEReturnCode.PMFalse
                For lPropertyCOunt As Integer = 0 To m_vGISProperty(lObjectCount).GetUpperBound(1)
                    sPropertyName = m_vGISProperty(lObjectCount)(pbObjectAndPropertyConsts.ACPPropertyName, lPropertyCOunt)
                    If CDbl(m_vGISProperty(lObjectCount)(pbObjectAndPropertyConsts.ACPIsIdentifyingProperty, lPropertyCOunt)) <> 1 And CStr(m_vGISProperty(lObjectCount)(pbObjectAndPropertyConsts.ACPPropertyName, lPropertyCOunt)) <> "dElEtEd" Then 'PN21998
                        result = gPMConstants.PMEReturnCode.PMTrue
                        Exit For
                    End If
                Next

                If result = gPMConstants.PMEReturnCode.PMFalse Then
                    MessageBox.Show("Please ensure table """ & CStr(m_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lObjectCount)) & """ has at least one user defined property", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result
                End If
            Next




            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckObjects Failed: Please check object " + sObjectName + ", and property " + sPropertyName, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckObjects", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: CreateDataSet
    '
    ' Description:
    '
    ' History: 17/09/2000 Tomo - Created.
    '          12/05/2005 CLG -  Re-written to use bGIS method
    '
    ' ***************************************************************** '

    Private Function CreateDataSet(ByRef v_sGISDataModel As String) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'call the GIS to recreate then

            m_nReturn = m_oBusinessGlobalTransaction.RecreateDatasets(v_sGISDataModel)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create the new data set", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDataSet")


                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateDataSet Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDataSet", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function


    Private Sub lvwProperties_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwProperties.SelectedIndexChanged

    End Sub

    Private Sub tvwObjects_NodeMouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwObjects.NodeMouseClick
        Dim Node As TreeNode = e.Node


        Dim lTag As Integer = Node.Tag(ACTagGisObjectId)   'CInt(Convert.ToString(ACTagGisObjectId))

        m_lGISDataModelType = Node.Tag(ACTagGisModelType)  'CInt(Convert.ToString(ACTagGisModelType))
        'm_lGISDataModelType = Node.Tag(lTag)  'CInt(Convert.ToString(ACTagGisModelType))

        m_nReturn = PopulateProperties(lTag:=lTag)

        m_lObjectTag = lTag


        If Convert.IsDBNull(m_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTag)) Or IsNothing(m_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTag)) Or (m_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTag) = 0) Then
            cmdObjectEdit.Enabled = False
            cmdPropertyAdd.Enabled = False
        Else

            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                m_bCanEditObject = False
            Else
                m_bCanEditObject = Not ((CBool(m_vGISObject(pbObjectAndPropertyConsts.ACOEditFlags, lTag)) And GISSharedPropertyConstants.GISDSEditReadOnly) = GISSharedPropertyConstants.GISDSEditReadOnly)
            End If

            cmdObjectEdit.Enabled = True
            cmdObjectEdit.Text = IIf((m_bCanEditObject And m_lSwiftIntegration = 0), "Edit", "View")
            cmdPropertyEdit.Text = IIf(m_bCanEditObject, "Edit", "View")

            cmdPropertyAdd.Enabled = m_bCanEditObject
        End If

        cmdPropertyEdit.Enabled = False
        cmdPropertyDelete.Enabled = False


    End Sub

    Private Sub frmInterface_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        If m_lGISDataModelType = GISDataModelType.GISDMTypeNotSet Then
            MessageBox.Show("Select a Data Model type before editing the model.", "GIS Object", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
        End If
    End Sub

    Function IsPropertyAdded(ByVal oArray(,) As Object, ByVal lGISObjectID As Integer, ByVal sPropertyName As String) As Boolean
        Dim nRecords As Integer
        nRecords = UBound(oArray, 2)

        If lGISObjectID = -1 Then
            Return False
        End If

        For iRecord As Integer = 0 To nRecords
            If oArray(pbObjectAndPropertyConsts.ACPGISObjectId, iRecord) = lGISObjectID AndAlso oArray(pbObjectAndPropertyConsts.ACPPropertyName, iRecord) = sPropertyName Then
                Return True
            End If
        Next
        Return False
    End Function
End Class
