Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Application_NET.Application")> _
Public NotInheritable Class Application
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface
    ' ***************************************************************** '
    ' Class Name: Application
    '
    ' Date: 26/04/1999
    '
    ' Description:
    '
    ' Edit History:
    ' RDC26072001 - compression methods added to NBQuote
    ' RDC29072001 - client/server dataset checking
    ' JRD09032005 PN18822 - Created NewQuoteObjectInstance
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Application"

    'DC081104 PN16464
    Private Const CALLING_APP_WRAPPER As String = "RebuildGISDataModel"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}

    ' GIS Business Object

    'developer guide no. Changed as per vb code
    'Private m_oBusiness As bGIS.Application
    Private m_oBusiness As Object

    'Private m_oBusiness As bGIS.Application

    ' GIS Data Set
    Private m_oDataSet As cGISDataSetControl.Application

    ' List Manager
    'developer guide no. Changed as per vb code
    Private m_oListManager As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer
    Private m_lMaxListItems As Integer
    Private m_lSaveToDBOnExit As Integer

    'DC081104
    Private m_oSBODatabase As Object
    Private m_oOrionDatabase As Object
    Private m_iSourceID As Integer
    Private m_sUsername As String = ""
    ' Password.
    Private m_sPassword As String = ""
    ' User ID
    Private m_iUserID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer

    ' RAW 22/07/2003 : CQ1672 : added
    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    ' RAW 22/07/2003 : CQ1672 : end

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            ' Set the calling application name.
            m_sCallingAppName = Value
        End Set
    End Property

    Public WriteOnly Property MaxListItems() As Integer
        Set(ByVal Value As Integer)
            m_lMaxListItems = Value
        End Set
    End Property

    'RT100500
    ' Public property to indicate whether the contents of the GIS
    ' should be saved upon temination of the iGIS component

    Public Property SaveToDBOnExit() As Integer
        Get
            Return m_lSaveToDBOnExit
        End Get
        Set(ByVal Value As Integer)
            m_lSaveToDBOnExit = Value
        End Set
    End Property


    ' RFC121000 - Scripting Method Access Added to iGIS - START
    Public ReadOnly Property Risk() As cGISDataSetControl.Node
        Get

            Try


                Return m_oDataSet.Risk

            Catch excep As System.Exception



                Throw New System.Exception(Information.Err().Number.ToString() + ", " + excep.Source + ", " + excep.Message)

                Exit Property

            End Try
        End Get
    End Property

    Public ReadOnly Property Quote(ByVal v_lQuoteNum As Integer) As cGISDataSetControl.Node
        Get

            Try


                Return m_oDataSet.Quote(v_lQuoteNum)

            Catch excep As System.Exception



                Throw New System.Exception(Information.Err().Number.ToString() + ", " + excep.Source + ", " + excep.Message)

                Exit Property

            End Try
        End Get
    End Property

    Public ReadOnly Property QuoteCount() As Integer
        Get

            Try


                Return m_oDataSet.QuoteCount

            Catch excep As System.Exception



                Throw New System.Exception(Information.Err().Number.ToString() + ", " + excep.Source + ", " + excep.Message)

                Exit Property


            End Try
        End Get
    End Property
    ' RFC121000 - Scripting Method Access Added to iGIS - END
    'RFC121000 - Add Lookup Methods to iGIS - START
    Public Property LookupsRequiredInsurerNo() As Integer
        Get
            Return m_oDataSet.LookupsRequiredInsurerNo
        End Get
        Set(ByVal Value As Integer)
            m_oDataSet.LookupsRequiredInsurerNo = Value
        End Set
    End Property
    'RFC121000 - Add Lookup Methods to iGIS - END

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise

        'DC081104 PN16464 log on silently if run via RebuildGISDataModel
        Dim result As Integer = 0
        Dim sOrionPath As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'LogMessage _
            ''    iType:=PMLogError, _
            ''    sMsg:="In Initialise - About to Create bGIS.Application", _
            ''    vApp:=ACApp, _
            ''    vClass:=ACClass, _
            ''    vMethod:="Initialise"

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' More to do here
            ' Just create the business Object
            ' as I do not want to have to logon etc.
            '    Set m_oBusiness = CreateObject("bGIS.Application")
            '    m_lReturn = m_oBusiness.Initialise("", "", 1, 1, 1, 1, 1, ACApp)
            '    If (m_lReturn <> PMTrue) Then
            '        LogMessage _
            ''            iType:=PMLogError, _
            ''            sMsg:="Failed to create bGIS.Application", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="Initialise"
            '        Initialise = m_lReturn
            '        Exit Function
            '    End If

            'LogMessage _
            ''    iType:=PMLogError, _
            ''    sMsg:="About to create cGISDataSetControl.Application", _
            ''    vApp:=ACApp, _
            ''    vClass:=ACClass, _
            ''    vMethod:="Initialise"

            '    Set m_oDataSet = New cGISDataSetControl.Application
            '
            'LogMessage _
            ''    iType:=PMLogError, _
            ''    sMsg:="About to create iGISListManager.InterfaceNoLogin", _
            ''    vApp:=ACApp, _
            ''    vClass:=ACClass, _
            ''    vMethod:="Initialise"



            'LogMessage _
            ''    iType:=PMLogError, _
            ''    sMsg:="About to exit function", _
            ''    vApp:=ACApp, _
            ''    vClass:=ACClass, _
            ''    vMethod:="Initialise"

            '    Exit Function
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            'DC081104 PN16464 log on silently if run via RebuildGISDataModel
            If m_sCallingAppName = CALLING_APP_WRAPPER Then

                ' Set Username and Password
                m_sUsername = "siriuscomm"
                m_sPassword = "NGMBMKqSMcc5"
                m_iUserID = 1
                m_iLanguageID = 1
                m_iSourceID = 1
                m_iCurrencyID = 26
                m_iLogLevel = 9

                ' it's being called by the scheduler
                ' don't show a login prompt
                m_lReturn = g_oObjectManager.InitialiseWithUserState(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iCountryID:=1, iLanguageId:=m_iLanguageID, iLogLevel:=m_iLogLevel, iCurrencyID:=m_iCurrencyID, lPartyCnt:=0, sCallingAppName:=m_sCallingAppName)


            Else

                ' Call the initialise method.
                m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = m_lReturn

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
            End With

            ' Create the GIS Business Object
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bGIS.Application", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Create the Data Set Component
            m_oDataSet = New cGISDataSetControl.Application()

            'RT100500 - Default the SaveToDBOnExit property to No Prompt No Save
            m_lSaveToDBOnExit = CInt(iGISSharedConstants.GISSaveToDBNoPromptNoSave)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        Dim lReturn As DialogResult
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then

                If m_lSaveToDBOnExit = StringsHelper.ToDoubleSafe(iGISSharedConstants.GISSaveToDBNoPromptSave) Then
                    lReturn = SaveToDB()
                    ' Check for errors.
                    If lReturn <> System.Windows.Forms.DialogResult.OK Then
                        ' Log Error.
                        gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to save the Gis Data", vApp:=ACApp, vClass:=ACClass, vMethod:="Terminate")
                    End If
                ElseIf m_lSaveToDBOnExit = StringsHelper.ToDoubleSafe(iGISSharedConstants.GISSaveToDBPromptForSave) Then
                    lReturn = MessageBox.Show("Do you want to save the data before exiting?", "Save before exit?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
                    If lReturn = System.Windows.Forms.DialogResult.Yes Then
                        lReturn = SaveToDB()
                        ' Check for errors.
                        If lReturn <> System.Windows.Forms.DialogResult.OK Then
                            ' Log Error.
                            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to save the Gis Data", vApp:=ACApp, vClass:=ACClass, vMethod:="Terminate")
                        End If
                    End If
                End If
                If m_oDataSet IsNot Nothing Then
                    m_oDataSet.Dispose()
                    m_oDataSet = Nothing
                End If
                If m_oListManager IsNot Nothing Then
                    m_oListManager.Dispose()
                    m_oListManager = Nothing
                End If
                If m_oBusiness IsNot Nothing Then
                    m_oBusiness.Dispose()
                    m_oBusiness = Nothing
                End If
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: NewDataSet
    '
    ' Description: Creates a New Data Set for the given Data Model ID.
    ' RFC111000 - Able to Specify a RiskID when creating/loading a dataset
    ' ***************************************************************** '
    Public Function NewDataSet(ByVal v_sGisDataModelCode As String, ByRef r_lPolicyLinkID As Integer, ByRef r_sTopOIKey As String, Optional ByVal v_vRiskID As Object = Nothing, Optional ByVal v_vInsuranceFileCnt As Object = Nothing, Optional ByRef r_sQuoteRef As String = "", Optional ByRef r_sQuoteRefPassword As String = "") As Integer

        Dim result As Integer = 0
        Dim sXMLDatasetDef, sXMLDataset As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a New Policy Link Record
            ' RFC111000 - Able to Specify a RiskID when creating/loading a dataset
            m_lReturn = m_oBusiness.NewDataSet(v_sGisDataModelCode:=v_sGisDataModelCode, r_lPolicyLinkID:=r_lPolicyLinkID, r_sTopOIKey:=r_sTopOIKey, r_sXMLDataSetDef:=sXMLDatasetDef, r_sXMLDataset:=sXMLDataset, v_vRiskID:=v_vRiskID, v_vInsuranceFileCnt:=v_vInsuranceFileCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Initialise the Data Set with the Object/Properties
            m_lReturn = m_oDataSet.LoadFromXML(v_sxmldatasetdef:=sXMLDatasetDef, v_sXMLDataSet:=sXMLDataset)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewDataSetFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewDataSet", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' RFC151101 - Added the CopyQuotes parameter
    ' ***************************************************************** '
    ' Name: CopyDataSet
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function CopyDataSet(ByVal v_sDataModelCode As String, ByRef r_lNewGISPolicyLinkId As Integer, Optional ByVal v_vOldGISPolicyLinkId As Integer = -1, Optional ByVal v_vOldInsuranceFileCnt As Integer = -1, Optional ByVal v_vOldXMLDataSet As String = "", Optional ByVal v_vNewInsuranceFileCnt As Integer = -1, Optional ByVal v_vOldRiskID As Integer = -1, Optional ByVal v_vNewRiskID As Integer = -1, Optional ByVal v_vCopyQuotes As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim sXMLDatasetDef, sXMLDataset As String
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Copy The Data Set
            lReturn = CType(m_oBusiness.CopyDataSet(v_sDataModelCode:=v_sDataModelCode, r_lNewGISPolicyLinkId:=r_lNewGISPolicyLinkId, r_sXMLDataSetDef:=sXMLDatasetDef, r_sXMLDataset:=sXMLDataset, v_vOldGISPolicyLinkId:=v_vOldGISPolicyLinkId, v_vOldInsuranceFileCnt:=v_vOldInsuranceFileCnt, v_vOldXMLDataSet:=v_vOldXMLDataSet, v_vNewInsuranceFileCnt:=v_vNewInsuranceFileCnt, v_vOldRiskID:=v_vOldRiskID, v_vNewRiskID:=v_vNewRiskID, v_vCopyQuotes:=v_vCopyQuotes), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Load it up
            m_lReturn = m_oDataSet.LoadFromXML(v_sxmldatasetdef:=sXMLDatasetDef, v_sXMLDataSet:=sXMLDataset)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyDataSetFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyDataSet", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' RFC140203
    ' ***************************************************************** '
    ' Name: GenDatasetDefinitions
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function GenDatasetDefinitions(ByVal v_sGisDataModelCode As String) As Integer


        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Call GIS to generate the Data Model Definitions

            lReturn = m_oBusiness.RecreateDatasets(v_sGisDataModelCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenDatasetDefinitionsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GenDatasetDefinitions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: PolicyLinkID
    '
    ' Description: Returns the PolicyLindID Value.
    '
    ' ***************************************************************** '
    Public Function PolicyLinkID() As Integer

        Dim result As Integer = 0
        Try

            result = -1


            Return m_oDataSet.PolicyLinkID()

        Catch excep As System.Exception



            result = -1

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PolicyLinkIDFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="PolicyLinkID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: NewObjectInstance
    '
    ' Description: Create a New Instance of the Specified Input Object.
    '              The Object Instance Key for the new object is returned.
    '              If the Object has a parent, the Parent OI Key MUST
    '              be supplied.
    ' ***************************************************************** '
    Public Function NewObjectInstance(ByVal v_sObjectName As String, ByRef r_sOIKey As String, Optional ByVal v_sParentOIKey As String = "") As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Return m_oDataSet.NewObjectInstance(v_sObjectName:=v_sObjectName, r_sOIKey:=r_sOIKey, v_sParentOIKey:=v_sParentOIKey)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewObjectInstanceFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewObjectInstance", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DelObjectInstance
    '
    ' Description: Delete an Instance of the Specified Object.
    '              If the Object has Child Object Instances
    '              they will be deleted also.
    ' ***************************************************************** '
    Public Function DelObjectInstance(ByVal v_sObjectName As String, ByVal v_sOIKey As String) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Return m_oDataSet.DelObjectInstance(v_sObjectName:=v_sObjectName, v_sOIKey:=v_sOIKey)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DelObjectInstanceFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="DelObjectInstance", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: SetPropertyValue
    '
    ' Description: Set an Input Property Value
    '
    ' ***************************************************************** '
    Public Function SetPropertyValue(ByVal v_sObjectName As String, ByVal v_sPropertyName As String, ByVal v_sOIKey As String, ByVal v_vPropertyValue As Object, Optional ByVal v_bIsAssumedInfo As Boolean = False) As Integer

        Dim result As Integer = 0

        Try



            Return m_oDataSet.SetPropertyValue(v_sObjectName:=v_sObjectName, v_sPropertyName:=v_sPropertyName, v_sOIKey:=v_sOIKey, v_vPropertyValue:=CStr(v_vPropertyValue), v_bIsAssumedInfo:=v_bIsAssumedInfo)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetPropertyValueFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPropertyValue", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPropertyValue
    '
    ' Description: Get an Input Property Value
    '
    ' ***************************************************************** '
    Public Function GetPropertyValue(ByVal v_sObjectName As String, ByVal v_sPropertyName As String, ByVal v_sOIKey As String, ByRef r_vPropertyValue As Object, Optional ByRef r_bIsAssumedInfo As Boolean = False) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'developer guide no.101

            Return m_oDataSet.GetPropertyValue(v_sObjectName:=v_sObjectName, v_sPropertyName:=v_sPropertyName, v_sOIKey:=v_sOIKey, r_vPropertyValue:=r_vPropertyValue, r_bIsAssumedInfo:=r_bIsAssumedInfo)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPropertyValueFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPropertyValue", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetInstanceHierarchy
    '
    ' Description: Starting from the given ObjectName work BACK up the
    '              Object Hierarchy returning an array of ALL Object
    '              Instances at ALL levels.
    '
    '              The Array is returned with the following columns:
    '
    '              ObjectName
    '              ObjectInstanceKey
    '              IdentifyingPropertyName1
    '              IdentifyingPropertyValue1
    '              IdentifyingPropertyName2
    '              IdentifyingPropertyValue2
    '              IdentifyingPropertyName3
    '              IdentifyingPropertyValue3
    '              ParentOIKey ("" For Top level objects)
    '
    '              See constants GISHierCol.... in GISSharedConstants
    ' ***************************************************************** '
    Public Function GetInstanceHierarchy(ByVal v_sObjectName As String, ByRef r_vObjectInstanceArray As Object, Optional ByRef r_lMaxInstances As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Return m_oDataSet.GetInstanceHierarchy(v_sObjectName:=v_sObjectName, r_vObjectInstanceArray:=r_vObjectInstanceArray, r_lMaxInstances:=r_lMaxInstances)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInstanceHierarchyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInstanceHierarchy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetObjectProperties
    '
    ' Description: Returns an array of PropertyName/Is ID Property.
    '
    ' ***************************************************************** '
    Public Function GetObjectProperties(ByVal v_sObjectName As String, ByRef r_vPropertyArray As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Return m_oDataSet.GetObjectDefDetails(v_sObjectName:=v_sObjectName, r_vPropertyArray:=r_vPropertyArray)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetObjectPropertiesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetObjectProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetObjectIdentity
    '
    ' Description: For the given Object Instance return an Array of the
    '              Identifying Property Names and current values.
    '
    '              See constants GISIDCol.... in GISSharedConstants
    '
    ' ***************************************************************** '
    Public Function GetObjectIdentity(ByVal v_sObjectName As String, ByVal v_sOIKey As String, ByRef r_vPropertyArray As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Return m_oDataSet.GetObjectIdentity(v_sObjectName:=v_sObjectName, v_sOIKey:=v_sOIKey, r_vPropertyArray:=r_vPropertyArray)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetObjectIdentityFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetObjectIdentity", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetObjectDefDetails
    '
    ' Description: Returns the Definition Details for the
    '              given Object.
    '
    ' ***************************************************************** '
    Public Function GetObjectDefDetails(ByVal v_sObjectName As String, Optional ByRef r_lIsQuoteObject As Integer = 0, Optional ByRef r_lGISObjectID As Integer = 0, Optional ByRef r_sTableName As String = "", Optional ByRef r_lMaxInstances As Integer = 0, Optional ByRef r_lPolarisObjectID As Integer = 0, Optional ByRef r_sParentObjectName As String = "", Optional ByRef r_vChildObjectArray As Object = Nothing, Optional ByRef r_vPropertyArray As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try


            Return m_oDataSet.GetObjectDefDetails(v_sObjectName:=v_sObjectName, r_lIsQuoteObject:=r_lIsQuoteObject, r_lGISObjectID:=r_lGISObjectID, r_sTableName:=r_sTableName, r_lMaxInstances:=r_lMaxInstances, r_lPolarisObjectID:=r_lPolarisObjectID, r_sParentObjectName:=r_sParentObjectName, r_vChildObjectArray:=r_vChildObjectArray, r_vPropertyArray:=r_vPropertyArray)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetObjectDefDetailsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetObjectDefDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetAllOIKey
    '
    ' Description: Returns an Array of Object Instance Key Values for
    '              the given Object name.
    '
    ' ***************************************************************** '
    Public Function GetAllOIKey(ByVal v_sObjectName As String, ByRef r_vOIKeyArray As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            Return m_oDataSet.GetAllOIKey(v_sObjectName:=v_sObjectName, r_vOIKeyArray:=r_vOIKeyArray)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllOIKeyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllOIKey", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetChildOIKey
    '
    ' Description: Returns an Array of Object Instance Keys for ALL
    '              Child objects of given Object Name.
    '
    ' ***************************************************************** '
    Public Function GetChildOIKey(ByVal v_sParentObjectName As String, ByVal v_sParentOIKey As String, ByVal v_sChildObjectName As String, ByRef r_vChildOIKeyArray As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Return m_oDataSet.GetChildOIKey(v_sParentObjectName:=v_sParentObjectName, v_sParentOIKey:=v_sParentOIKey, v_sChildObjectName:=v_sChildObjectName, r_vChildOIKeyArray:=r_vChildOIKeyArray)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetChildOIKeyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetChildOIKey", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetListAndCodes
    '
    ' Description: Returns the associated List for a given Property.
    '
    ' ***************************************************************** '
    Public Function GetListAndCodes(ByVal v_sObjectName As String, ByVal v_sPropertyName As String, ByRef r_vListData As Object, ByRef r_vListDataCodes As Object, Optional ByVal v_vSearchString As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lGISListID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' More to do here

            If m_oListManager Is Nothing Then
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to Obtain List for Property " & v_sPropertyName & " as List Manager Component is not Available.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListAndCodes")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Property Definition Details
            lReturn = m_oDataSet.GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=v_sPropertyName, r_lGISListID:=lGISListID)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Is this a Polaris Property
            If lGISListID > 0 Then

                ' Yes it is, then we can get the List

                If Information.IsNothing(v_vSearchString) Then



                    m_lReturn = m_oListManager.GetListAndCodes(v_sPropertyId:=CStr(lGISListID), r_vListData:=r_vListData, r_vListDataCode:=r_vListDataCodes)

                Else




                    m_lReturn = m_oListManager.GetListAndCodes(v_sPropertyId:=CStr(lGISListID), r_vListData:=r_vListData, r_vListDataCode:=r_vListDataCodes, v_vSearchString:=CStr(v_vSearchString))

                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Error Getting list from List Manager", "iGEMWListManager", MessageBoxButtons.OK)
                    Return result
                End If

            Else

                ' No it isn't so show Error
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to Obtain List for Property " & v_sPropertyName & " as it is does NOT have a List ID defined.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListAndCodes")

            End If

            Return result

        Catch



            ' Error Section.
            MessageBox.Show("Failed to GetListAndCodes List Manager wrapper", "iGEMWListManager", MessageBoxButtons.OK)


            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function
    ' ***************************************************************** '
    ' Name: GetDescriptionFromABICode
    '
    ' Description: Returns a description for a given Property/ abi code.
    '
    ' ***************************************************************** '
    Public Function GetDescriptionFromABICode(ByVal v_sObjectName As String, ByVal v_sPropertyName As String, ByVal v_sABICode As String, ByRef r_sDescription As String) As Integer

        Dim result As Integer = 0
        Dim lReturn, lGISListID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' More to do here

            If m_oListManager Is Nothing Then
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to Obtain List for Property " & v_sPropertyName & _
               " as List Manager Component is not Available.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDescriptionFromABICode")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_lMaxListItems <> 0 Then
                m_oListManager.MaxListItems = m_lMaxListItems
            End If

            ' Get the Property Definition Details
            lReturn = m_oDataSet.GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=v_sPropertyName, r_lGISListID:=lGISListID)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Is this a Polaris Property
            If lGISListID > 0 Then

                m_lReturn = m_oListManager.GetDescriptionFromABICode(v_sPropertyId:=CStr(lGISListID), v_sABICode:=v_sABICode, r_sDescription:=r_sDescription)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    'sj 17/04/2001 - start
                    '            MsgBox "Error Getting list from List Manager", _
                    ''                vbOKOnly, _
                    ''                "iGEMWListManager"

                    iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to obtain description for Object/Property/List Id/Abi Code " & v_sObjectName & "/" & v_sPropertyName & "/" & CStr(lGISListID) & "/" & v_sABICode, vApp:=Nothing, vClass:=ACClass, vMethod:="GetABICodeFromDescription")

                    'sj 17/04/2001 - end

                    Return m_lReturn
                End If

            Else

                ' No it isn't so show Error
                'sj 17/04/2001 - start
                '        LogMessage _
                ''            iType:=PMLogInfo, _
                ''            sMsg:="Unable to Obtain List for Property " _
                ''                & v_sPropertyName _
                ''                & " as it is does NOT have a List ID defined.", _
                ''            vApp:=ACApp, _
                ''            vClass:=ACClass, _
                ''            vMethod:="GetDescriptionFromABICode"

                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to Obtain List for Object/Property " & v_sObjectName & "/" & v_sPropertyName & " as it is does NOT have a List ID defined.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDescriptionFromABICode")

                Return gPMConstants.PMEReturnCode.PMNotFound
                'sj 17/04/2001 - end

            End If

            Return result

        Catch



            ' Error Section.
            MessageBox.Show("Failed to GetDescriptionFromABICode List Manager wrapper", "iGEMWListManager", MessageBoxButtons.OK)


            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function
    ' ***************************************************************** '
    ' Name: GetABICodeFromDescription
    '
    ' Description: Returns an abi code for a given Property/description.
    '
    ' ***************************************************************** '
    Public Function GetABICodeFromDescription(ByVal v_sObjectName As String, ByVal v_sPropertyName As String, ByRef r_sABICode As String, ByVal v_sDescription As String) As Integer

        Dim result As Integer = 0
        Dim lReturn, lGISListID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' More to do here

            If m_oListManager Is Nothing Then
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to Obtain List for Property " & v_sPropertyName & _
               " as List Manager Component is not Available.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetABICodeFromDescription")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Property Definition Details
            lReturn = m_oDataSet.GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=v_sPropertyName, r_lGISListID:=lGISListID)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If m_lMaxListItems <> 0 Then
                m_oListManager.MaxListItems = m_lMaxListItems
            End If

            ' Is this a Polaris Property
            If lGISListID > 0 Then

                m_lReturn = m_oListManager.GetABICodeFromDescription(v_sPropertyId:=CStr(lGISListID), r_sABICode:=r_sABICode, v_sDescription:=v_sDescription)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' JSB 16/11/2000 - Commented out following to prevent
                    '                  error message appearing on scree
                    '            MsgBox "Error Getting list from List Manager", _
                    ''                vbOKOnly, _
                    ''                "iGEMWListManager"

                    'sj 17/04/2001 - start

                    iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to obtain abi code for Object/Property/List Id/Description " & v_sObjectName & "/" & v_sPropertyName & "/" & CStr(lGISListID) & "/" & v_sDescription, vApp:=Nothing, vClass:=ACClass, vMethod:="GetABICodeFromDescription")


                    'sj 17/04/2001 - end


                    Return m_lReturn
                End If

            Else

                'sj 17/04/2001 - start

                ' No it isn't so show Error
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to Obtain List for Object/Property " & v_sObjectName & "/" & v_sPropertyName & " as it is does NOT have a List ID defined.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetABICodeFromDescription")

                Return gPMConstants.PMEReturnCode.PMNotFound

                '        ' No it isn't so show Error
                '        LogMessage _
                ''            iType:=PMLogInfo, _
                ''            sMsg:="Unable to Obtain List for Property " _
                ''                & v_sPropertyName _
                ''                & " as it is does NOT have a List ID defined.", _
                ''            vApp:=ACApp, _
                ''            vClass:=ACClass, _
                ''            vMethod:="GetABICodeFromDescription"
                'sj 17/04/2001 - end

            End If

            Return result

        Catch



            ' Error Section.
            MessageBox.Show("Failed to GetABICodeFromDescription List Manager wrapper", "iGEMWListManager", MessageBoxButtons.OK)


            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function
    ' ***************************************************************** '
    ' Name: GetCodeDescription
    '
    ' Description: Gets the Description for a given List Code.
    '
    ' Created: 23/08/99 RFC
    ' ***************************************************************** '
    Public Function GetCodeDescription(ByVal v_sObjectName As String, ByVal v_sPropertyName As String, ByVal v_sCode As String, ByRef r_sDescription As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lGISListID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oListManager Is Nothing Then
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to Obtain List for Property " & v_sPropertyName & " as List Manager Component is not Available.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCodeDescription")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Property Definition Details
            lReturn = m_oDataSet.GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=v_sPropertyName, r_lGISListID:=lGISListID)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Is this a List Property
            If lGISListID > 0 Then

                ' Yes it is, then we can get the Description
                lReturn = CType(m_oListManager.GetDescription(sPropertyId:=CStr(lGISListID), sABICodeTarget:=v_sCode, sDescription:=r_sDescription), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

            Else

                ' No it isn't so show Error
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to get Description for Property " & v_sPropertyName & " as it is does NOT have a List ID defined.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCodeDescription")
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCodeDescriptionFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCodeDescription", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: SaveToDB
    '
    ' Description: Saves the Data Set to the Database
    '
    ' ***************************************************************** '
    Public Function SaveToDB() As Integer

        Dim result As Integer = 0
        Dim sXMLDatasetDef, sXMLDataset As String
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sDataModelCode As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Data as an XML String
            lReturn = m_oDataSet.ReturnAsXML(r_sXMLDataSetDef:=sXMLDatasetDef, r_sXMLDataset:=sXMLDataset)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sDataModelCode = m_oDataSet.GISDataModelCode

            ' Save it to the DataBase

            lReturn = m_oBusiness.SaveToDB(v_sGisDataModelCode:=sDataModelCode, r_sXMLDataset:=sXMLDataset)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    ' Create a New Data Set
            '    Set m_oDataSet = New cGISDataSetControl.Application

            ' Load the Saved to DB Results
            lReturn = m_oDataSet.LoadFromXML(v_sxmldatasetdef:=sXMLDatasetDef, v_sXMLDataSet:=sXMLDataset)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveToDBFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveToDB", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LoadFromDB
    '
    ' Description:
    '
    ' RFC160300 - DataModel code param added to LoadFromDB call.
    ' RFC111000 - Able to Specify a RiskID when creating/loading a dataset
    ' ***************************************************************** '
    Public Function LoadFromDB(ByVal v_sGisDataModelCode As String, Optional ByRef r_vInsuranceFileCnt As Object = Nothing, Optional ByRef r_vPolicyLinkID As Object = Nothing, Optional ByRef r_vRiskID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sXMLDatasetDef, sXMLDataset As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDataSet = New cGISDataSetControl.Application()

            ' Get the Data From the Database in XML
            ' RFC160300 - DataModel code param added to LoadFromDB call.
            lReturn = CType(m_oBusiness.LoadFromDB(r_sXMLDataSetDef:=sXMLDatasetDef, r_sXMLDataset:=sXMLDataset, v_sGisDataModelCode:=v_sGisDataModelCode, r_vInsuranceFileCnt:=r_vInsuranceFileCnt, r_vPolicyLinkID:=r_vPolicyLinkID, r_vRiskID:=r_vRiskID), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Load Data as XML
            lReturn = m_oDataSet.LoadFromXML(v_sxmldatasetdef:=sXMLDatasetDef, v_sXMLDataSet:=sXMLDataset)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadFromDBFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromDB", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: NBQuote
    '
    ' Description: Perform a NBQuote.
    ' RFC160300 - Add EffectiveDate Param
    ' BSJ23112001 - Added optional param Risk Group ID for cnic
    ' ***************************************************************** '


    Public Function NBQuote(ByVal v_lQuoteType As Integer, ByVal v_sGisBusinessTypeCode As String, ByVal v_dtEffectiveDate As Date, Optional ByVal v_lGisSchemeId As Integer = -1, Optional ByRef r_vAdditionalDataArray As Object = Nothing, Optional ByVal v_lRiskGroupID As Integer = -1) As Integer

        Dim result As Integer = 0
        Dim sXMLDatasetDef, sXMLDataset As String
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sDataModelCode As String = ""

        ' RDC 26072001 required by new compression methods for GEM2
        Dim bPMZipp As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' RFC300300 - Clear all Quote Output that may already exist
            ' as there is no need to Pass it back across the network.
            lReturn = m_oDataSet.ClearAllQuoteOutput()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Data as an XML String
            lReturn = m_oDataSet.ReturnAsXML(r_sXMLDataSetDef:=sXMLDatasetDef, r_sXMLDataset:=sXMLDataset)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sDataModelCode = m_oDataSet.GISDataModelCode

            ' RDC 26072001 required by new compression methods for GEM2    START
            ' RDC 29102001 ZLIB has a bug in decompress - we can't use it
            '    Set bPMZipp = CreateObject("bPMZipper.Business")
            '
            '    m_oBusiness.XMLStrLength = Len(sXMLDataSet$)
            '
            '    lReturn = bPMZipp.CompressString(sXMLDataSet$)
            '
            '    ' RDC 29102001 add error handler
            '    If lReturn <> PMTrue Then
            '        ' write to file
            '        LogMessageFile _
            ''            iType:=PMLogOnError, _
            ''            sMsg:="ZLIB CompressString has failed", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="NBQuote"
            '
            '        NBQuote = PMFalse
            '        Exit Function
            '    End If
            ' RDC 26072001 required by new compression methods for GEM2    END

            ' NBQuote
            lReturn = CType(m_oBusiness.NBQuote(v_sGisDataModelCode:=sDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_lQuoteType:=v_lQuoteType, v_dtEffectiveDate:=v_dtEffectiveDate, r_sXMLDataset:=sXMLDataset, v_lGisSchemeId:=v_lGisSchemeId, r_vAdditionalDataArray:=r_vAdditionalDataArray, v_lRiskGroupID:=v_lRiskGroupID), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' RDC 26072001 required by new compression methods for GEM2
            ' RDC 29102001 ZLIB has a bug in decompress - we can't use it
            '    lReturn = bPMZipp.DecompressString(sXMLDataSet$, m_oBusiness.XMLStrLength)
            '
            '    ' RDC 29102001 add error handler
            '    If lReturn <> PMTrue Then
            '        ' write to file
            '        LogMessageFile _
            ''            iType:=PMLogOnError, _
            ''            sMsg:="ZLIB DecompressString has failed", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="NBQuote"
            '
            '        NBQuote = PMFalse
            '        Exit Function
            '    End If

            '    ' Create a New Data Set
            '    Set m_oDataSet = New cGISDataSetControl.Application

            ' Load the NBQuote Results
            lReturn = m_oDataSet.LoadFromXML(v_sxmldatasetdef:=sXMLDatasetDef, v_sXMLDataSet:=sXMLDataset)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' RDC 26072001 required by new compression methods for GEM2
            bPMZipp = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NBQuoteFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="NBQuote", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SaveToFile
    '
    ' Description: Save the Data Set Definition and/or Data Set
    '              to a file.
    '
    ' ***************************************************************** '
    Public Function SaveToFile(Optional ByVal v_sDataSetDefFile As String = "", Optional ByVal v_sDataSetFile As String = "") As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Return m_oDataSet.SaveXMLToFile(v_sDataSetDefFile:=v_sDataSetDefFile, v_sDataSetFile:=v_sDataSetFile)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveToFileFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveToFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: InitialiseListMgmt
    '
    ' Description: init list mgmt stuff with data model code
    '
    ' Author: CL290799
    ' ***************************************************************** '
    Public Function InitialiseListMgmt(ByVal v_sGisDataModelCode As String, Optional ByVal v_sSellerCode As String = "") As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oListManager = New iGISListManager.InterfaceNoLogin()

            If m_oListManager Is Nothing Then
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to create instance of List Manager interface", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseListMgmt")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(m_oListManager, SSP.S4I.Interfaces.ILocalInterface).Initialise()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Error initialising List Manager interface", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseListMgmt")


                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            m_lReturn = m_oListManager.CheckListVersions(v_sGisDataModelCode, v_sSellerCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Error checking list versions", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseListMgmt")


                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveToFileFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseListMgmt", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: InitialiseLookups
    '
    ' Description:
    '
    'RFC121000 - Add Lookup Methods to iGIS
    ' ***************************************************************** '
    Public Function InitialiseLookups(ByRef v_sGisDataModelCode As String, ByRef v_sBusinessTypeCode As String, ByRef v_dtProcessDate As Date, ByRef v_lStatus As Integer) As Integer

        Dim result As Integer = 0

        Try


            Return m_oDataSet.InitialiseLookups(v_sDataModelCode:=v_sGisDataModelCode, v_sBusinessTypeCode:=v_sBusinessTypeCode, v_dtProcessDate:=v_dtProcessDate, v_lStatus:=v_lStatus)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InitialiseLookups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseLookups", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: ReturnAsXML
    '
    ' Description: Return the Data Set.
    '
    ' ***************************************************************** '
    Public Function ReturnAsXML(ByRef r_vXMLDataSet As String) As Integer

        Dim result As Integer = 0
        Dim sXMLDatasetDef, sXMLDataset As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            result = m_oDataSet.ReturnAsXML(r_sXMLDataSetDef:=sXMLDatasetDef, r_sXMLDataset:=sXMLDataset)

            r_vXMLDataSet = sXMLDataset

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReturnAsXMLFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReturnAsXML", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: LoadFromXML
    '
    ' Description: Loads up a Data Set using the supplied XML Streams.
    '
    ' ***************************************************************** '
    Public Function LoadFromXML(ByVal v_sGisDataModelCode As String, ByVal v_sXMLDataSet As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sDataSetDefFile, sDataSetFile As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Path/FileNames for stored EMPTY Data Set Files
            ' of this Data Model Type
            lReturn = CType(iGISSharedConstants.GetDataSetFileNames(v_sDataModelCode:=v_sGisDataModelCode, r_sDataSetDefFile:=sDataSetDefFile, r_sDataSetFile:=sDataSetFile), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a New Data Set
            m_oDataSet = New cGISDataSetControl.Application()

            ' Try to load from the Empty XML files
            lReturn = m_oDataSet.LoadFromXMLFile(v_sDataSetDefFile:=sDataSetDefFile, v_sDataSetFile:=sDataSetFile)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Update with the Data Set
            Return m_oDataSet.UpdateDataSetFromXML(v_sXMLDataSet:=v_sXMLDataSet)


        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadFromXMLFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXML", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: NBTransact
    '
    ' Description: Transact the Quote
    '
    ' ***************************************************************** '
    Public Function NBTransact(ByVal v_sGisBusinessTypeCode As String, ByVal v_lGisSchemeId As Integer) As Integer

        Dim result As Integer = 0
        Dim sXMLDatasetDef, sXMLDataset As String
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sDataModelCode As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Data as an XML String
            lReturn = m_oDataSet.ReturnAsXML(r_sXMLDataSetDef:=sXMLDatasetDef, r_sXMLDataset:=sXMLDataset)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sDataModelCode = m_oDataSet.GISDataModelCode

            ' Do the Transact

            lReturn = m_oBusiness.NBTransact(v_sGisDataModelCode:=sDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, r_sXMLDataset:=sXMLDataset, v_lGisSchemeId:=v_lGisSchemeId)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    ' Create a New Data Set
            '    Set m_oDataSet = New cGISDataSetControl.Application

            ' Load the NBQuote Results
            lReturn = m_oDataSet.LoadFromXML(v_sxmldatasetdef:=sXMLDatasetDef, v_sXMLDataSet:=sXMLDataset)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NBTransactFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="NBTransact", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PrintForm
    '
    ' Description: Print a Form
    '
    ' ***************************************************************** '
    Public Function PrintForm(ByVal v_sGisBusinessTypeCode As String, ByVal v_lFormNumber As Integer, ByVal v_lGisSchemeId As Integer) As Integer

        Dim result As Integer = 0
        Dim sXMLDatasetDef, sXMLDataset As String
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sDataModelCode As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Data as an XML String
            lReturn = m_oDataSet.ReturnAsXML(r_sXMLDataSetDef:=sXMLDatasetDef, r_sXMLDataset:=sXMLDataset)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sDataModelCode = m_oDataSet.GISDataModelCode

            ' Print the Form

            lReturn = m_oBusiness.PrintForm(v_sGisDataModelCode:=sDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sXMLDataSet:=sXMLDataset, v_lGisSchemeId:=v_lGisSchemeId, v_lFormNumber:=v_lFormNumber)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' RFC101201 - Load the Dataset up after the PrintForm Call. The Dataset param
            ' RFC101201 - in bGIS.PrintForm has been changed to ByRef so that it can be updated.
            ' Load the NBQuote Results
            lReturn = m_oDataSet.LoadFromXML(v_sxmldatasetdef:=sXMLDatasetDef, v_sXMLDataSet:=sXMLDataset)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PrintFormFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: NBPostQuoteProcess
    '
    ' Description: Transact the Quote
    '
    ' ***************************************************************** '
    Public Function NBPostQuoteProcess(ByVal v_sGisBusinessTypeCode As String, ByVal v_lProcessType As Integer, ByVal v_lGisSchemeId As Integer) As Integer

        Dim result As Integer = 0
        Dim sXMLDatasetDef, sXMLDataset As String
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sDataModelCode As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Data as an XML String
            lReturn = m_oDataSet.ReturnAsXML(r_sXMLDataSetDef:=sXMLDatasetDef, r_sXMLDataset:=sXMLDataset)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sDataModelCode = m_oDataSet.GISDataModelCode

            ' Do the Transact

            lReturn = m_oBusiness.NBPostQuoteProcess(v_sGisDataModelCode:=sDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, r_sXMLDataset:=sXMLDataset, v_lGisSchemeId:=v_lGisSchemeId, v_lProcessType:=v_lProcessType)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    ' Create a New Data Set
            '    Set m_oDataSet = New cGISDataSetControl.Application

            ' Load the NBQuote Results
            lReturn = m_oDataSet.LoadFromXML(v_sxmldatasetdef:=sXMLDatasetDef, v_sXMLDataSet:=sXMLDataset)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NBPostQuoteProcessFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="NBPostQuoteProcess", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: MTAQuote
    '
    ' Description: Perform a MTAQuote.
    '
    ' ***************************************************************** '
    Public Function MTAQuote(ByVal v_lQuoteType As Integer, ByVal v_sGisBusinessTypeCode As String, ByVal v_dtEffectiveDate As Date, ByVal v_sXMLOldRisk As String) As Integer

        Dim result As Integer = 0
        Dim sXMLDatasetDef, sXMLDataset As String
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sDataModelCode As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Data as an XML String
            lReturn = m_oDataSet.ReturnAsXML(r_sXMLDataSetDef:=sXMLDatasetDef, r_sXMLDataset:=sXMLDataset)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sDataModelCode = m_oDataSet.GISDataModelCode

            ' MTAQuote

            lReturn = m_oBusiness.MTAQuote(v_sGisDataModelCode:=sDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_lQuoteType:=v_lQuoteType, v_dtEffectiveDate:=v_dtEffectiveDate, v_sXMLOldRisk:=v_sXMLOldRisk, r_sXMLNewRisk:=sXMLDataset)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    ' Create a New Data Set
            '    Set m_oDataSet = New cGISDataSetControl.Application

            ' Load the NBQuote Results
            lReturn = m_oDataSet.LoadFromXML(v_sxmldatasetdef:=sXMLDatasetDef, v_sXMLDataSet:=sXMLDataset)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MTAQuote Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MTAQuote", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: MTATransact
    '
    ' Description: Perform a MTATransact.
    '
    ' RFC13012000 - Add Effective Date Param
    ' ***************************************************************** '
    Public Function MTATransact(ByVal v_lQuoteType As Integer, ByVal v_sGisBusinessTypeCode As String, ByVal v_dtEffectiveDate As Date, ByVal v_sXMLOldRisk As String) As Integer

        Dim result As Integer = 0
        Dim sXMLDatasetDef, sXMLDataset As String
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sDataModelCode As String

        Dim sActionReturnXML As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Data as an XML String
            lReturn = m_oDataSet.ReturnAsXML(r_sXMLDataSetDef:=sXMLDatasetDef, r_sXMLDataset:=sXMLDataset)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sDataModelCode = m_oDataSet.GISDataModelCode

            ' MTAQuote

            lReturn = m_oBusiness.MTATransact(v_sGisDataModelCode:=sDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_lQuoteType:=v_lQuoteType, v_dtEffectiveDate:=v_dtEffectiveDate, v_sXMLOldRisk:=v_sXMLOldRisk, r_sXMLNewRisk:=sXMLDataset)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    ' Create a New Data Set
            '    Set m_oDataSet = New cGISDataSetControl.Application

            ' Load the NBQuote Results
            lReturn = m_oDataSet.LoadFromXML(v_sxmldatasetdef:=sXMLDatasetDef, v_sXMLDataSet:=sXMLDataset)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MTATransact Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MTATransact", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: ReloadSpecialsFromDB
    '
    ' Description:
    '
    ' History: 03/02/03 RFC - Created.
    '
    ' RFC14082003    Added proper support for WorkClaim
    ' ***************************************************************** '
    Public Function ReloadSpecialsFromDB(Optional ByVal v_lClaimID As Integer = -1) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sXMLDataset, sXMLDatasetDef As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the XML
            lReturn = m_oDataSet.ReturnAsXML(r_sXMLDataSetDef:=sXMLDatasetDef, r_sXMLDataset:=sXMLDataset)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Reload the Specials

            lReturn = m_oBusiness.ReloadSpecialsFromDB(v_sGisDataModelCode:=m_oDataSet.GISDataModelCode, r_sXMLDataset:=sXMLDataset, v_lClaimID:=v_lClaimID)

            ' Update the Dataset with the updated XML
            lReturn = m_oDataSet.LoadFromXML(v_sxmldatasetdef:=sXMLDatasetDef, v_sXMLDataSet:=sXMLDataset)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReloadSpecialsFromDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReloadSpecialsFromDB", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name: InitialiseDatasets
    '
    ' Description: Initialise XML datasets, ensuring that client has
    '              same versions as server
    '
    ' RDC 30072001 - created
    ' ***************************************************************** '
    Public Function InitialiseDatasets(ByVal v_sGisDataModelCode As String, Optional ByVal v_sGisBusinessTypeCode As String = "") As Integer

        Dim result As Integer = 0
        Dim sDSDfilename, sDSfilename, sClientTimestamp, sServerTimestamp, sDataSetsPath As String

        Dim sDSDdata, sDSdata As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' generate new server-side datasets if required

            m_lReturn = m_oBusiness.InitialiseDataSetCheck(v_sGisDataModelCode, sServerTimestamp, v_sGisBusinessTypeCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' DSD does not contain timestamp, delete and regenerate files

                m_lReturn = m_oBusiness.RecreateDatasets(v_sGisDataModelCode:=v_sGisDataModelCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' check files exist, pick up timestamp

                m_lReturn = m_oBusiness.InitialiseDataSetCheck(v_sGisDataModelCode, sServerTimestamp)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            ' Datasets path on client
            m_lReturn = iGISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode, v_sSettingName:=iGISSharedConstants.GISRegDataSetPath, r_sSettingValue:=sDataSetsPath, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon)

            ' add \ if necessary
            If Not sDataSetsPath.EndsWith("\") Then
                sDataSetsPath = sDataSetsPath & "\"
            End If

            ' filenames for client-side DSD and DS
            sDSDfilename = sDataSetsPath & v_sGisDataModelCode.Trim().ToUpper() & iGISSharedConstants.ACDataSetDefSuffix & iGISSharedConstants.ACXMLFileExtension
            sDSfilename = sDataSetsPath & v_sGisDataModelCode.Trim().ToUpper() & iGISSharedConstants.ACDataSetSuffix & iGISSharedConstants.ACXMLFileExtension

            ' load client-side files
            m_lReturn = m_oDataSet.LoadFromXMLFile(v_sDataSetDefFile:=sDSDfilename, v_sDataSetFile:=sDSfilename)

            ' get timestamp from client-side files
            sClientTimestamp = m_oDataSet.GISDSDTimestamp

            ' Are the timestamps the same?
            If sClientTimestamp <> sServerTimestamp Then
                ' different timestamp, copy server files to client

                ' get contents of DSD and DS

                m_lReturn = m_oBusiness.GetDatasetDetails(sDSDdata, sDSdata)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' copy to client
                result = CopyServerDatasets(sDSDdata, sDSdata, sDSDfilename, sDSfilename)

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InitialiseDatasets failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseDatasets", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CopyServerDatasets
    '
    ' Description: Copy server-side DSD and DS to client.
    '
    ' RDC 30072001 - created
    ' ***************************************************************** '
    Private Function CopyServerDatasets(ByVal sDSDdata As String, ByVal sDSdata As String, ByVal sDSDfilename As String, ByVal sDSfilename As String) As Integer

        Dim result As Integer = 0
        Dim lFree As Integer


        Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Goto Label (Err_CopyServerDatasets)")

        result = gPMConstants.PMEReturnCode.PMTrue

        ' delete the files, ignore errors



        File.Delete(sDSDfilename)
        File.Delete(sDSfilename)


        Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Goto Label (Err_CopyServerDatasets)")

        ' write the Dataset Def
        lFree = FileSystem.FreeFile()

        FileSystem.FileOpen(lFree, sDSDfilename, OpenMode.Output)
        FileSystem.PrintLine(lFree, sDSDdata)
        FileSystem.FileClose(lFree)

        ' write the Dataset
        lFree = FileSystem.FreeFile()

        FileSystem.FileOpen(lFree, sDSfilename, OpenMode.Output)
        FileSystem.PrintLine(lFree, sDSdata)
        FileSystem.FileClose(lFree)

        Return result

Err_CopyServerDatasets:

        result = gPMConstants.PMEReturnCode.PMError

        iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyServerDatasets failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyServerDatasets", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: NewRiskDataset
    '
    ' Description: create a new risk dataset
    '
    ' RDC 10012003 - created
    ' ***************************************************************** '
    Public Function NewRiskDataset(ByVal v_sGisDataModelCode As String, ByRef r_lPolicyLinkID As Integer, ByRef r_sTopOIKey As String, ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String, Optional ByVal v_lRiskID As Integer = -1, Optional ByVal v_lInsuranceFileCnt As Integer = -1, Optional ByVal v_lInsuranceFolderCnt As Integer = -1, Optional ByRef r_sQuoteRef As String = "", Optional ByRef r_sQuoteRefPassword As String = "") As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse


            m_lReturn = m_oBusiness.NewRiskDataset(v_sGisDataModelCode:=v_sGisDataModelCode, r_lPolicyLinkID:=r_lPolicyLinkID, r_sTopOIKey:=r_sTopOIKey, r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset, v_lRiskID:=v_lRiskID, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, r_sQuoteRef:=r_sQuoteRef, r_sQuoteRefPassword:=r_sQuoteRefPassword)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' Initialise the Data Set with the Object/Properties
            m_lReturn = m_oDataSet.LoadFromXML(v_sxmldatasetdef:=r_sXMLDataSetDef, v_sXMLDataSet:=r_sXMLDataset)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewRiskDataset failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewRiskDataset", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: NewClaimDataset
    '
    ' Description: create a new claim dataset
    '
    ' RDC 10012003 - created
    ' RFC14082003    Added proper support for WorkClaim
    ' ***************************************************************** '
    Public Function NewClaimDataset(ByVal v_sGisDataModelCode As String, ByRef r_lPolicyLinkID As Integer, ByRef r_sTopOIKey As String, ByVal v_lWorkClaimID As Integer) As Integer

        Dim result As Integer = 0
        Dim sXMLDatasetDef, sXMLDataset As String

        Try

            result = gPMConstants.PMEReturnCode.PMFalse


            m_lReturn = m_oBusiness.NewClaimDataset(v_sGisDataModelCode:=v_sGisDataModelCode, r_lPolicyLinkID:=r_lPolicyLinkID, r_sTopOIKey:=r_sTopOIKey, r_sXMLDataSetDef:=sXMLDatasetDef, r_sXMLDataset:=sXMLDataset, v_lWorkClaimID:=v_lWorkClaimID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' Initialise the Data Set with the Object/Properties
            m_lReturn = m_oDataSet.LoadFromXML(v_sxmldatasetdef:=sXMLDatasetDef, v_sXMLDataSet:=sXMLDataset)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewClaimDataset failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewClaimDataset", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: NewPartyDataset
    '
    ' Description: create a new party dataset
    '
    ' RDC 10012003 - created
    ' ***************************************************************** '
    Public Function NewPartyDataset(ByVal v_sGisDataModelCode As String, ByRef r_lPolicyLinkID As Integer, ByRef r_sTopOIKey As String, ByVal v_lPartyCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim sXMLDatasetDef, sXMLDataset As String

        Try

            result = gPMConstants.PMEReturnCode.PMFalse


            m_lReturn = m_oBusiness.NewPartyDataset(v_sGisDataModelCode:=v_sGisDataModelCode, r_lPolicyLinkID:=r_lPolicyLinkID, r_sTopOIKey:=r_sTopOIKey, r_sXMLDataSetDef:=sXMLDatasetDef, r_sXMLDataset:=sXMLDataset, v_lPartyCnt:=v_lPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' Initialise the Data Set with the Object/Properties
            m_lReturn = m_oDataSet.LoadFromXML(v_sxmldatasetdef:=sXMLDatasetDef, v_sXMLDataSet:=sXMLDataset)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewPartyDataset failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewPartyDataset", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: NewPolicyDataset
    '
    ' Description: create a new policy dataset
    '
    ' RDC 10012003 - created
    ' ***************************************************************** '
    Public Function NewPolicyDataset(ByVal v_sGisDataModelCode As String, ByRef r_lPolicyLinkID As Integer, ByRef r_sTopOIKey As String, ByVal v_lInsuranceFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim sXMLDatasetDef, sXMLDataset As String

        Try

            result = gPMConstants.PMEReturnCode.PMFalse


            m_lReturn = m_oBusiness.NewPolicyDataset(v_sGisDataModelCode:=v_sGisDataModelCode, r_lPolicyLinkID:=r_lPolicyLinkID, r_sTopOIKey:=r_sTopOIKey, r_sXMLDataSetDef:=sXMLDatasetDef, r_sXMLDataset:=sXMLDataset, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' Initialise the Data Set with the Object/Properties
            m_lReturn = m_oDataSet.LoadFromXML(v_sxmldatasetdef:=sXMLDatasetDef, v_sXMLDataSet:=sXMLDataset)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewPolicyDataset failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewPolicyDataset", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LoadRiskFromDB
    '
    ' Description: load a risk dataset
    '
    ' RDC 10012003 - created
    ' ***************************************************************** '
    Public Function LoadRiskFromDB(ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String, ByVal v_sGisDataModelCode As String, Optional ByVal v_lRiskID As Integer = -1, Optional ByVal v_lInsuranceFolderCnt As Integer = -1, Optional ByVal v_lInsuranceFileCnt As Integer = -1) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse


            m_lReturn = m_oBusiness.LoadRiskFromDB(r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset, r_sGisDataModelCode:=v_sGisDataModelCode, v_lRiskID:=v_lRiskID, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lInsuranceFileCnt:=v_lInsuranceFileCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            m_lReturn = m_oDataSet.LoadFromXML(v_sxmldatasetdef:=r_sXMLDataSetDef, v_sXMLDataSet:=r_sXMLDataset)


            Return m_lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadRiskFromDB failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadRiskFromDB", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ValidatePostQuote
    '
    ' Description: Validate the post quote data entry
    '
    ' SPW 200904
    ' ***************************************************************** '
    Public Function ValidatePostQuote(ByVal v_sGisBusinessTypeCode As String, ByVal v_lProcessType As Integer, ByVal v_lGisSchemeId As Integer, ByVal v_vValidationArray As Object, ByRef r_vResultArray As Object, Optional ByVal v_sTransactionType As String = "") As Integer

        Dim result As Integer = 0
        Dim sXMLDatasetDef, sXMLDataset As String
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sDataModelCode As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Data as an XML String
            lReturn = m_oDataSet.ReturnAsXML(r_sXMLDataSetDef:=sXMLDatasetDef, r_sXMLDataset:=sXMLDataset)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sDataModelCode = m_oDataSet.GISDataModelCode

            ' Call the business post quote validate
            lReturn = CType(m_oBusiness.ValidatePostQuote(v_sGisDataModelCode:=sDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_lProcessType:=v_lProcessType, r_sXMLDataset:=sXMLDataset, v_lGisSchemeId:=v_lGisSchemeId, v_vValidationArray:=v_vValidationArray, r_vResultArray:=r_vResultArray, v_sTransactionType:=v_sTransactionType), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Load the Results of validate
            lReturn = m_oDataSet.LoadFromXML(v_sxmldatasetdef:=sXMLDatasetDef, v_sXMLDataSet:=sXMLDataset)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidatePostQuoteFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidatePostQuote", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LoadClaimFromDB
    '
    ' Description: load a claim dataset
    '
    ' RDC 10012003 - created
    ' RFC14082003    Added proper support for WorkClaim
    ' ***************************************************************** '
    Public Function LoadClaimFromDB(ByVal v_sGisDataModelCode As String, ByVal v_lWorkClaimID As Integer) As Integer

        Dim result As Integer = 0
        Dim sXMLDatasetDef, sXMLDataset As String

        Try

            result = gPMConstants.PMEReturnCode.PMFalse


            m_lReturn = m_oBusiness.LoadClaimFromDB(r_sXMLDataSetDef:=sXMLDatasetDef, r_sXMLDataset:=sXMLDataset, r_sGisDataModelCode:=v_sGisDataModelCode, v_lWorkClaimID:=v_lWorkClaimID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            m_lReturn = m_oDataSet.LoadFromXML(v_sxmldatasetdef:=sXMLDatasetDef, v_sXMLDataSet:=sXMLDataset)


            Return m_lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadClaimFromDB failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadClaimFromDB", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LoadPartyFromDB
    '
    ' Description: load a party dataset
    '
    ' RDC 10012003 - created
    ' ***************************************************************** '
    Public Function LoadPartyFromDB(ByVal v_sGisDataModelCode As String, ByVal v_lPartyCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim sXMLDatasetDef, sXMLDataset As String

        Try

            result = gPMConstants.PMEReturnCode.PMFalse


            m_lReturn = m_oBusiness.LoadPartyFromDB(r_sXMLDataSetDef:=sXMLDatasetDef, r_sXMLDataset:=sXMLDataset, r_sGisDataModelCode:=v_sGisDataModelCode, v_lPartyCnt:=v_lPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            m_lReturn = m_oDataSet.LoadFromXML(v_sxmldatasetdef:=sXMLDatasetDef, v_sXMLDataSet:=sXMLDataset)


            Return m_lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadRiskFromDB failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadPartyFromDB", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LoadPolicyFromDB
    '
    ' Description: load a policy dataset
    '
    ' RDC 10012003 - created
    ' ***************************************************************** '
    Public Function LoadPolicyFromDB(ByVal v_sGisDataModelCode As String, ByVal v_lInsuranceFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim sXMLDatasetDef, sXMLDataset As String

        Try

            result = gPMConstants.PMEReturnCode.PMFalse


            m_lReturn = m_oBusiness.LoadPolicyFromDB(r_sXMLDataSetDef:=sXMLDatasetDef, r_sXMLDataset:=sXMLDataset, r_sGisDataModelCode:=v_sGisDataModelCode, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            m_lReturn = m_oDataSet.LoadFromXML(v_sxmldatasetdef:=sXMLDatasetDef, v_sXMLDataSet:=sXMLDataset)


            Return m_lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadPolicyFromDB failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadPolicyFromDB", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' RAW 22/07/2003 : CQ1672 : added
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Information.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If


            ' Now pass this on
            ' ===================================================

            ' RAW 22/07/2003 : CQ1672 : added
            ' Set the process modes for the business object.
            If Not (m_oBusiness Is Nothing) Then
                m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes")
                    Return m_lReturn
                End If
            End If
            ' RAW 22/07/2003 : CQ1672 : end


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: NewQuoteObjectInstance
    ' Description: Create a New Object Instance of the Specified Input Quote Object.
    '              The Object Instance Key for the new object is returned.
    '              If the Object has a parent, the Parent OI Key MUST
    '              be supplied.
    ' Use this method when creating objects under the Quote structure to ensure
    ' the OIKeys are correctly created with the QuoteKey prefix
    'JRD09032005 PN18822 - Created
    ' ***************************************************************** '
    Public Function NewQuoteObjectInstance(ByVal v_sObjectName As String, ByRef r_sOIKey As String, ByVal v_sQuoteKey As String, Optional ByVal v_sParentOIKey As String = "") As Integer

        Const kFUNCTION_NAME As String = "NewQuoteObjectInstance"
        Dim lReturn As gPMConstants.PMEReturnCode
        Try

            lReturn = gPMConstants.PMEReturnCode.PMTrue
            lReturn = m_oDataSet.NewObjectInstance(v_sObjectName:=v_sObjectName, r_sOIKey:=r_sOIKey, v_sParentOIKey:=v_sParentOIKey, v_sQuoteKey:=v_sQuoteKey)

        Catch excep As System.Exception

            lReturn = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Run-time error occured", vApp:=ACApp, vClass:=ACClass, vMethod:=kFUNCTION_NAME, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

        Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
    End Function
End Class

