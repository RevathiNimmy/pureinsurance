Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Application_NET.Application")> _
Public NotInheritable Class Application
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Application
    '
    ' Date: 29/07/1999
    '
    ' Description:
    '
    ' Edit History:
    ' RFC13012000 - Add Effective Date to NB & MTA and
    '               Guaranteed Quote Date to LoadFromDB
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Application"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}

    ' GIS Data Set
    Private m_oDataSet As cGISDataSetControl.Application

    ' List Manager
    Private m_oListManager As iGISListManager.InterfaceNoLogin

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    Private m_oGIS As bGIS.Application

    ' PRIVATE Data Members (End)
    '**** Added By: AAB  -  Added On:  06-Sep-2002 09:47 ****
    Public Function AddQuote(ByVal v_sGisDataModelCode As String, ByVal v_sGISBusinessTypeCode As String, ByVal v_dtEffectiveDate As Date, ByVal v_dtExpirationDate As Date, ByVal v_sInsuredName As String, ByVal v_lPartyCnt As Integer, ByVal v_lAgentCnt As Integer, ByRef r_lInsuranceFolderCnt As Object, ByRef r_lInsuranceFileCnt As Object, Optional ByVal v_sInsuranceFolderDescription As String = "", Optional ByRef r_vAdditionalDataArray(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lReturn As gPMConstants.PMEReturnCode

            lReturn = CType(GISCall("APPLICATION", "AddQuote", v_sGisDataModelCode, v_sGISBusinessTypeCode, v_dtEffectiveDate, v_dtExpirationDate, v_sInsuredName, v_lPartyCnt, v_lAgentCnt, r_lInsuranceFolderCnt, r_lInsuranceFileCnt, v_sInsuranceFolderDescription, r_vAdditionalDataArray), gPMConstants.PMEReturnCode)

            ' Check the Return Value
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddQuote Failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddQuote", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    '**** Added By: AAB  -  Added On:  06-Sep-2002 09:47 ****
    Public Function AddRisk(ByVal v_sBackOfficeMapperCode As String, ByVal v_sGisDataModelCode As String, ByVal v_sGISBusinessTypeCode As String, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lRiskTypeId As Integer, ByVal v_lRiskScreenId As Integer, ByVal v_sRiskDescription As String, ByVal v_lProductID As Integer, ByRef r_lRiskFolderCnt As Object, ByRef r_lRiskCnt As Object, ByRef r_sXMLDataSetDef As Object, ByRef r_sXMLDataset As Object, ByRef r_vPolicyLinkID As Object, ByRef r_vTopOIKey As Object, ByRef r_vQuoteRef As Object, ByRef r_vQuoteRefPassword As Object, Optional ByRef r_vAdditionalDataArray(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lReturn As gPMConstants.PMEReturnCode

            lReturn = CType(GISCall("APPLICATION", "AddRisk", v_sBackOfficeMapperCode, v_sGisDataModelCode, v_sGISBusinessTypeCode, v_lInsuranceFolderCnt, v_lInsuranceFileCnt, v_lPartyCnt, v_lRiskTypeId, v_lRiskScreenId, v_sRiskDescription, v_lProductID, r_lRiskFolderCnt, r_lRiskCnt, r_sXMLDataSetDef, r_sXMLDataset, r_vPolicyLinkID, r_vTopOIKey, r_vQuoteRef, r_vQuoteRefPassword, r_vAdditionalDataArray), gPMConstants.PMEReturnCode)

            ' Check the Return Value
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddRisk Failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddRisk", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
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


    ' RFC090300 - Scripting Method Access Added
    Public ReadOnly Property Risk() As cGISDataSetControl.Node
        Get

            Try


                Return m_oDataSet.Risk

            Catch excep As System.Exception



                Throw New System.Exception(Information.Err().Number.ToString() + ", " + excep.Source + ", " + excep.Message)

                Exit Property

                Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

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

    'RFC190400 - Add Lookup Methods to GIS
    Public Property LookupsRequiredInsurerNo() As Integer
        Get
            Return m_oDataSet.LookupsRequiredInsurerNo
        End Get
        Set(ByVal Value As Integer)
            m_oDataSet.LookupsRequiredInsurerNo = Value
        End Set
    End Property
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
    Public Function Initialise() As Integer




        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' RDC 37062001 commented out - see InitialiseLookups method
            'Set m_oDataSet = New cGISDataSetControl.Application

            m_oGIS = New bGIS.Application()

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
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If m_oDataSet IsNot Nothing Then
                    m_oDataSet.Dispose()
                    m_oDataSet = Nothing
                End If
                If m_oListManager IsNot Nothing Then
                    m_oListManager.Dispose()
                    m_oListManager = Nothing
                End If
                If Not (m_oGIS Is Nothing) Then
                    m_oGIS = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    '
    ' Name: InitialiseLookups
    '
    ' Description:
    '
    ' History: 19/04/2000 RFC - Created.
    'RFC190400 - Add Lookup Methods to GIS
    ' ***************************************************************** '
    Public Function InitialiseLookups(ByRef v_sGisDataModelCode As String, ByRef v_sBusinessTypeCode As String, ByRef v_dtProcessDate As Object, ByRef v_lStatus As Integer) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sDataSetDefFile, sDataSetFile As String

        Try

            ' RDC 27062001 Create blank dataset ONLY if not already done by NewDataset (START)
            If m_oDataSet Is Nothing Then

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

            End If
            ' RDC 27062001 (END)



            Return m_oDataSet.InitialiseLookups(v_sDataModelCode:=v_sGisDataModelCode, v_sBusinessTypeCode:=v_sBusinessTypeCode, v_dtProcessDate:=CDate(v_dtProcessDate), v_lStatus:=v_lStatus)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InitialiseLookups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseLookups", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: NewDataSet
    '
    ' Description: Creates a New Data Set for the given Data Model ID.
    '
    ' NOTE: I changed r_vPolicyLinkID from long to variant to get ASP to work! CL130899
    '
    ' ***************************************************************** '
    Public Function NewDataSet(ByVal v_sGisDataModelCode As String, ByRef r_vPolicyLinkID As Integer, ByRef r_vTopOIKey As Object, ByRef r_vQuoteRef As String, ByRef r_vQuoteRefPassword As String, Optional ByVal v_vInsuranceFileCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lGISDataModelID As Integer
        Dim sXMLDataSetDef, sXMLDataSet, sTopLevelObject, sTopLevelTable As String

        Dim sDataSetDefFile, sDataSetFile As String
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sTopOIKey, sActionXML As String
        Dim vOIKeyArray() As Object

        Dim sQuoteRef, sQuoteRefPassword As String

        Dim sActionReturnXML As String = ""
        Dim lReturnValue As gPMConstants.PMEReturnCode

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

            ' Format the Action XML
            lReturn = CType(FormatActionXML(v_lAction:=iGISSharedConstants.GISDSActionNewDataSet, v_sSellerGUID:="", r_sActionXML:=sActionXML, v_sDataModelCode:=v_sGisDataModelCode), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Process The Action
            lReturn = CType(ProcessActionViaHTTP(v_oDataSet:=m_oDataSet, v_sActionXML:=sActionXML, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the ActionReturn Value
            ' Unformat the Action Return XML
            lReturn = CType(UnFormatActionReturnXML(v_sActionReturnXML:=sActionReturnXML, r_lReturnValue:=lReturnValue, r_sQuoteReference:=sQuoteRef, r_sQuoteRefPassword:=sQuoteRefPassword), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Check the Return Value
            If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturnValue
            End If

            ' Get the Top Level Object
            lReturn = m_oDataSet.GetTopLevelRiskObject(r_sObjectName:=sTopLevelObject, r_sTableName:=sTopLevelTable)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Instances of this Object
            ' Note: There should be one ONLY

            lReturn = m_oDataSet.GetAllOIKey(v_sObjectName:=sTopLevelObject, r_vOIKeyArray:=vOIKeyArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the Top Level Object Key

            ' Do we have an Array
            If Not Information.IsArray(vOIKeyArray) Then
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Find an Instance of Object : " & sTopLevelObject, vApp:=ACApp, vClass:=ACClass, vMethod:="NewDataSet")
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                ' Return the first one



                r_vTopOIKey = vOIKeyArray(vOIKeyArray.GetLowerBound(0))
            End If

            ' Return the Policy Link ID
            r_vPolicyLinkID = PolicyLinkID()

            ' Return the Quote Ref details
            r_vQuoteRef = sQuoteRef
            r_vQuoteRefPassword = sQuoteRefPassword

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewDataSetFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewDataSet", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    ' Name: NewQuoteOutput
    '
    ' Description: Wraps NewQuoteOutput() of cGISDataSetControl (for Marsh)
    '
    ' Author: CL260500 (home)
    '
    ' ***************************************************************** '
    Public Function NewQuoteOutput(ByRef r_lQEMNumber As Integer, ByRef r_sInsurer As String, ByRef r_lInsurerID As Integer, ByRef r_sScheme As String, ByRef r_lSchemeID As Integer, ByRef r_lSchemeVer As Integer, ByRef r_sQuoteKey As String) As Integer

        Dim result As Integer = 0
        Try


            Return m_oDataSet.NewQuoteOutput(v_lQEMNumber:=r_lQEMNumber, v_sInsurer:=r_sInsurer, v_lInsurerId:=r_lInsurerID, v_sScheme:=r_sScheme, v_lSchemeID:=r_lSchemeID, v_lSchemeVer:=r_lSchemeVer, r_sQuoteKey:=r_sQuoteKey)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewQuoteOutput Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewQuoteOutput", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'RFC160501
    ' ***************************************************************** '
    ' Name: NewQuoteOutputSaveDB
    '
    ' Description: Create a New Quote Output area, ready to be
    '              populated with the details from a Quotation.
    '
    ' Note: This is the new version of the Quote Output where
    '       the Quotes can be saved to the Database.
    ' ***************************************************************** '
    Public Function NewQuoteOutputSaveDB(ByVal v_lGISSchemeID As Integer, ByRef r_vQuoteKey As String, ByRef r_vTopQuoteOIKey As String) As Integer

        Dim result As Integer = 0
        Dim sQuoteKey, sTopQuoteOIKey As String

        Try

            result = m_oDataSet.NewQuoteOutputSaveDB(v_lGISSchemeID:=v_lGISSchemeID, r_sQuoteKey:=sQuoteKey, r_sTopQuoteOIKey:=sTopQuoteOIKey)

            r_vQuoteKey = sQuoteKey
            r_vTopQuoteOIKey = sTopQuoteOIKey

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewQuoteOutputSaveDBFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewQuoteOutputSaveDB", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Public Function NewObjectInstance(ByVal v_sObjectName As String, ByRef r_vOIKey As String, Optional ByVal v_sParentOIKey As String = "") As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim sOIKey As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create the New Object Instance
            result = m_oDataSet.NewObjectInstance(v_sObjectName:=v_sObjectName, r_sOIKey:=sOIKey, v_sParentOIKey:=v_sParentOIKey)

            ' Return the OIKey
            ' Note had to use a variant return param because of ASP
            r_vOIKey = sOIKey

            Return result

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
        Dim lReturn As Integer

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
    ' Name: ClearObject
    '
    ' Description: Set all non-key properties (for given object) to empty string
    '
    ' Date: CL090600
    '
    ' ***************************************************************** '
    Public Function ClearObjectByKey(ByVal v_sObjectName As String, ByVal v_sOIKey As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vPropertyArray(,) As Object
        Dim sPropName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = m_oDataSet.GetObjectDefDetails(v_sObjectName:=v_sObjectName, r_vPropertyArray:=vPropertyArray)

            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (Not Information.IsArray(vPropertyArray)) Then
                Return lReturn
            End If


            For i As Integer = vPropertyArray.GetLowerBound(1) To vPropertyArray.GetUpperBound(1)


                sPropName = CStr(vPropertyArray(0, i)).ToUpper()

                If Not sPropName.EndsWith("_ID") Then

                    ' Set non-key property to empty string

                    lReturn = CType(SetPropertyValue(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropName, v_sOIKey:=v_sOIKey, v_vPropertyValue:=""), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lReturn
                    End If

                End If

            Next

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ClearObjectByKey Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearObjectByKey", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ClearAllObjects
    '
    ' Description: Clear ALL instances of all objects in risk XML
    '
    ' Date: CL090600
    '
    ' ***************************************************************** '
    Public Function ClearAllObjects() As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vChildObjectArray(,) As Object
        Dim i As Integer
        Dim sPropName, sTopObjectName, sTopTableName As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            '
            ' Get top level object (i.e. root)
            '

            lReturn = m_oDataSet.GetTopLevelRiskObject(r_sObjectName:=sTopObjectName, r_sTableName:=sTopTableName)

            '
            ' Get immediate descendants of this root
            '
            lReturn = m_oDataSet.GetObjectDefDetails(v_sObjectName:=sTopObjectName, r_vChildObjectArray:=vChildObjectArray)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If Information.IsArray(vChildObjectArray) Then

                ' Clear the XML recursively

                lReturn = CType(ClearAllObjects2(vChildObjectArray), gPMConstants.PMEReturnCode)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ClearAllObjects Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearAllObjects", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ClearAllObjects2
    '
    ' Description: Recursive part of ClearAllObjects()
    '               Clear objects in given array and all children objects
    '
    ' Date: CL090600
    '
    ' ***************************************************************** '
    Private Function ClearAllObjects2(ByRef v_ObjectArray As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vChildObjectArray(,) As Object
        Dim sObjName As String = ""
        Dim vOIKeyArray As Object
        Dim sOIKey As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        If Not Information.IsArray(v_ObjectArray) Then
            Return lReturn
        End If

        '
        ' Loop through the given object array
        '

        For i As Integer = v_ObjectArray.GetLowerBound(0) To v_ObjectArray.GetUpperBound(0)


            sObjName = CStr(v_ObjectArray(i)).ToUpper()

            '
            ' Get all OI keys for this object
            '


            lReturn = m_oDataSet.GetAllOIKey(v_sObjectName:=sObjName, r_vOIKeyArray:=vOIKeyArray)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            '
            ' Clear all instances of this object
            '

            If Information.IsArray(vOIKeyArray) Then


                For j As Integer = vOIKeyArray.GetLowerBound(0) To vOIKeyArray.GetUpperBound(0)


                    sOIKey = CStr(vOIKeyArray(j))

                    lReturn = CType(ClearObjectByKey(v_sObjectName:=sObjName, v_sOIKey:=sOIKey), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lReturn
                    End If

                Next

            End If


            '
            ' Now see if this object has any children
            '

            lReturn = m_oDataSet.GetObjectDefDetails(v_sObjectName:=sObjName, r_vChildObjectArray:=vChildObjectArray)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If Information.IsArray(vChildObjectArray) Then

                ' Zap children recursively

                lReturn = CType(ClearAllObjects2(vChildObjectArray), gPMConstants.PMEReturnCode) ' RECURSE

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

            End If


        Next

        Return result

    End Function

    'RFC150501 - Added Clear All Quote Output
    ' ***************************************************************** '
    ' Name: ClearAllQuoteOutput
    '
    ' Description: Deletes and Recreates the Quote Output node.
    '
    ' ***************************************************************** '
    Public Function ClearAllQuoteOutput() As Integer


        Dim result As Integer = 0
        Try

            ' Just call the Dataset Clear AllQuote Output

            Return m_oDataSet.ClearAllQuoteOutput()

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ClearAllQuoteOutputFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearAllQuoteOutput", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim lReturn As Integer

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
        Dim lReturn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            Return m_oDataSet.GetPropertyValue(v_sObjectName:=v_sObjectName, v_sPropertyName:=v_sPropertyName, v_sOIKey:=v_sOIKey, r_vPropertyValue:=CStr(r_vPropertyValue), r_bIsAssumedInfo:=r_bIsAssumedInfo)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPropertyValueFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPropertyValue", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Public Function GetAllOIKey(ByVal v_sObjectName As String, ByRef r_vOIKeyArray(,) As Object) As Integer

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
    Public Function GetChildOIKey(ByVal v_sParentObjectName As String, ByVal v_sParentOIKey As String, ByVal v_sChildObjectName As String, ByRef r_vChildOIKeyArray(,) As Object) As Integer

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
    ' Name: GetListAndCodes
    '
    ' Description: Returns the associated List for a given Property.
    '
    ' ***************************************************************** '
    Public Function GetListAndCodes(ByVal v_sObjectName As String, ByVal v_sPropertyName As String, ByRef r_vListData As Array, ByRef r_vListDataCodes As Array, Optional ByVal v_vSearchString As String = "", Optional ByVal v_bMultiSearch As Boolean = False, Optional ByRef r_vGISListID As Integer = 0) As Integer

        Dim result As Integer = 0

        Dim lGISListID As Integer


        result = gPMConstants.PMEReturnCode.PMTrue

        ' More to do here

        If m_oListManager Is Nothing Then
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to Obtain List for Property " & v_sPropertyName & " as List Manager Component is not Available.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListAndCodes")
            Return result
        End If

        Dim sObjectName As String = v_sObjectName
        Dim sPropertyName As String = v_sPropertyName

        ' Get the Property Definition Details
        Dim lReturn As gPMConstants.PMEReturnCode = m_oDataSet.GetPropertyDefDetails(v_sObjectName:=sObjectName, v_sPropertyName:=sPropertyName, r_lGISListID:=lGISListID)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If


        If Not Information.IsNothing(r_vGISListID) Then
            r_vGISListID = lGISListID
        End If

        ' Is this a Polaris Property
        If lGISListID > 0 Then

            ' Yes it is, then we can get the List

            If Information.IsNothing(v_vSearchString) Then

                m_lReturn = m_oListManager.GetListAndCodes(v_sPropertyId:=CStr(lGISListID), r_vListData:=r_vListData, r_vListDataCode:=r_vListDataCodes, v_bMultiSearch:=v_bMultiSearch)

            Else

                m_lReturn = m_oListManager.GetListAndCodes(v_sPropertyId:=CStr(lGISListID), r_vListData:=r_vListData, r_vListDataCode:=r_vListDataCodes, v_vSearchString:=v_vSearchString, v_bMultiSearch:=v_bMultiSearch)

            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get list for PropertyID:" & lGISListID, vApp:=ACApp, vClass:=ACClass, vMethod:="GetListAndCodes", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

        Else

            '        ' No it isn't so show Error
            '        iPMFunc.LogMessage _
            ''            iType:=PMLogInfo, _
            ''            sMsg:="Unable to Obtain List for Property " & v_sPropertyName & " as it is NOT a Polaris Property", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="GetListAndCodes"


            ' RDC30052001 search PM Lookup or User Defined list
            m_lReturn = GetLookupList(sObjectName, sPropertyName, r_vListData, r_vListDataCodes, v_vSearchString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get list for Object:" & sObjectName & ", Property:" & sPropertyName, vApp:=ACApp, vClass:=ACClass, vMethod:="GetListAndCodes", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

        End If

        Return result



        ' Log Error Message
        iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error " & Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:="GetListAndCodes", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)


        Return gPMConstants.PMEReturnCode.PMFalse

    End Function

    ' ***************************************************************** '
    ' Name: GetVehicleList
    '
    ' Description: Returns the associated Vehicle List
    '
    ' ***************************************************************** '
    Public Function GetVehicleList(ByVal v_sObjectName As String, ByVal v_sPropertyName As String, ByRef r_vListData As Object, ByVal v_vMake As String, Optional ByVal v_vModelChosen As Object = Nothing, Optional ByVal v_vYear As Object = Nothing, Optional ByVal v_vCC As Object = Nothing, Optional ByVal v_vDoors As Object = Nothing, Optional ByVal v_vFuelType As Object = Nothing, Optional ByVal v_vTransType As Object = Nothing) As Integer


        Dim result As Integer = 0
        Const PROPERTY_ID As Integer = 393220 'Property id for the Vehicle Search


        Dim lGISListID As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        ' More to do here

        If m_oListManager Is Nothing Then
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to Obtain List for Property " & v_sPropertyName & " as List Manager Component is not Available.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicleList")
            Return result
        End If

        ' Get the Property Definition Details
        Dim lReturn As gPMConstants.PMEReturnCode = m_oDataSet.GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=v_sPropertyName, r_lGISListID:=lGISListID)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' Is this the Vehicle List Polaris Property ?

        If lGISListID = PROPERTY_ID Then

            m_lReturn = m_oListManager.GetVehicleList(r_vListData:=r_vListData, v_vMake:=v_vMake, v_vModelChoosen:=v_vModelChosen, v_vYear:=v_vYear, v_vCC:=v_vCC, v_vDoors:=v_vDoors, v_vFuelType:=v_vFuelType, v_vTransType:=v_vTransType)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error Getting list from List Manager", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicleList")
                Return result
            End If

        Else

            ' No it isn't so show Error
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to obtain Vehicle List as the property" & Environment.NewLine & _
              "given is NOT the Vehicle List Polaris Property", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicleList")

        End If

        Return result



        ' Error Section.
        iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error:" & Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicleList")


        Return gPMConstants.PMEReturnCode.PMFalse

    End Function

    ' ***************************************************************** '
    ' Name: GetVehicleModels
    '
    ' Description: Returns the associated Vehicle Models
    '
    ' 'DB 31/1/2000
    ' ***************************************************************** '
    Public Function GetVehicleModels(ByVal v_sObjectName As String, ByVal v_sPropertyName As String, ByRef r_vListData As Object, ByVal v_vMake As String) As Integer


        Dim result As Integer = 0
        Const PROPERTY_ID As Integer = 393220 'Property id for the Vehicle Search


        Dim lGISListID As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        ' More to do here

        If m_oListManager Is Nothing Then
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to Obtain List for Property " & v_sPropertyName & " as List Manager Component is not Available.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicleModels")
            Return result
        End If

        ' Get the Property Definition Details
        Dim lReturn As gPMConstants.PMEReturnCode = m_oDataSet.GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=v_sPropertyName, r_lGISListID:=lGISListID)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' Is this the Vehicle List Polaris Property ?

        If lGISListID = PROPERTY_ID Then


            m_lReturn = m_oListManager.GetVehicleModels(r_vListData:=r_vListData, v_vMake:=v_vMake)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error getting list from ListManager", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicleModels")
                Return result
            End If

        Else

            ' No it isn't so show Error
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to obtain Vehicle List as the property" & Environment.NewLine & _
              "given is NOT the Vehicle List Polaris Property", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicleModels")

        End If

        Return result



        ' Error Section.
        iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error:" & Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicleModels")


        Return gPMConstants.PMEReturnCode.PMFalse

    End Function

    ' ***************************************************************** '
    ' Name: GetLookupList
    '
    ' Description: Where list is set up as PM or User Defined lookup
    '
    ' RDC 30052001
    ' ***************************************************************** '
    Private Function GetLookupList(ByVal sObjectName As String, ByVal sPropertyName As String, ByRef r_vListData As Array, ByRef r_vListDataCodes As Array, Optional ByVal v_vSearchString As String = "") As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sActionXML, sDataModelCode As String

        Dim sActionReturnXML As String = ""
        Dim lReturnValue As gPMConstants.PMEReturnCode

        Dim vListDataMerged(,) As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        ' size array and load parameters into spare array

        If Information.IsNothing(v_vSearchString) Then
            ReDim vListDataMerged(1, 1)

            ' names

            vListDataMerged(0, 0) = "ObjectName"

            vListDataMerged(0, 1) = "PropertyName"

            ' values

            vListDataMerged(1, 0) = sObjectName

            vListDataMerged(1, 1) = sPropertyName
        Else
            ReDim vListDataMerged(1, 2)

            ' names

            vListDataMerged(0, 0) = "ObjectName"

            vListDataMerged(0, 1) = "PropertyName"

            vListDataMerged(0, 2) = "SearchString"

            ' values

            vListDataMerged(1, 0) = sObjectName

            vListDataMerged(1, 1) = sPropertyName

            vListDataMerged(1, 2) = v_vSearchString
        End If

        sDataModelCode = m_oDataSet.GISDataModelCode

        ' convert to XML
        lReturn = CType(FormatActionXML(v_lAction:=iGISSharedConstants.GISDSActionGetLookup, v_sSellerGUID:="", v_sDataModelCode:=sDataModelCode, r_sActionXML:=sActionXML, v_vAdditionalDataArray:=vListDataMerged), gPMConstants.PMEReturnCode)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' send it across
        lReturn = CType(ProcessActionViaHTTP(v_oDataSet:=m_oDataSet, v_sActionXML:=sActionXML, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' retrieve response
        lReturn = CType(UnFormatActionReturnXML(v_sActionReturnXML:=sActionReturnXML, r_lReturnValue:=lReturnValue, r_vAdditionalDataArray:=vListDataMerged), gPMConstants.PMEReturnCode)

        ' Check return codes
        If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (Not Information.IsArray(vListDataMerged)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturnValue
        End If

        ' convert merged array to separate codes and description arrays

        r_vListData = Array.CreateInstance(GetType(Object), New Integer() {vListDataMerged.GetUpperBound(1) - vListDataMerged.GetLowerBound(1) + 1}, New Integer() {vListDataMerged.GetLowerBound(1)})

        r_vListDataCodes = Array.CreateInstance(GetType(Object), New Integer() {vListDataMerged.GetUpperBound(1) - vListDataMerged.GetLowerBound(1) + 1}, New Integer() {vListDataMerged.GetLowerBound(1)})


        For lRowNum As Integer = vListDataMerged.GetLowerBound(1) To vListDataMerged.GetUpperBound(1)


            r_vListDataCodes(lRowNum) = vListDataMerged(0, lRowNum)


            r_vListData(lRowNum) = vListDataMerged(1, lRowNum)
        Next

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetLookupListDirect
    '
    ' Description:  Retrieves a ID, Description array from a table
    '               directly without going via the GIS Property. This
    '               allows the developer to pull data directly from tables
    '               without having the lookups defined in the GIS.
    '
    ' History: 05/12/2001 DD - Created.
    '
    ' ***************************************************************** '
    Public Function GetLookupListDirect(ByVal v_sLookupListName As String, ByRef r_vListData As Object, Optional ByVal v_vSearchString As String = "") As Integer

        Dim result As Integer = 0
        Dim lRowNum As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sActionXML, sDataModelCode As String

        Dim sActionReturnXML As String = ""
        Dim lReturnValue As gPMConstants.PMEReturnCode

        Dim vListDataMerged(1, 3) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' size array and load parameters into array
            ' the second dimension of 3 will trigger the GIS to
            ' handle the direct call

            ' names

            vListDataMerged(0, 0) = "ObjectName"

            vListDataMerged(0, 1) = "NotUsed" 'not used for direct calls

            vListDataMerged(0, 2) = "SearchString"

            vListDataMerged(0, 3) = "IsDirect"

            ' values

            vListDataMerged(1, 0) = v_sLookupListName

            vListDataMerged(1, 1) = "" 'not used for direct calls

            vListDataMerged(1, 2) = v_vSearchString

            vListDataMerged(1, 3) = True

            sDataModelCode = m_oDataSet.GISDataModelCode

            ' convert to XML
            lReturn = CType(FormatActionXML(v_lAction:=iGISSharedConstants.GISDSActionGetLookup, v_sSellerGUID:="", v_sDataModelCode:=sDataModelCode, r_sActionXML:=sActionXML, v_vAdditionalDataArray:=vListDataMerged), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' send it across
            lReturn = CType(ProcessActionViaHTTP(v_oDataSet:=m_oDataSet, v_sActionXML:=sActionXML, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' retrieve response
            lReturn = CType(UnFormatActionReturnXML(v_sActionReturnXML:=sActionReturnXML, r_lReturnValue:=lReturnValue, r_vAdditionalDataArray:=r_vListData), gPMConstants.PMEReturnCode)

            ' Check return codes
            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (Not Information.IsArray(vListDataMerged)) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturnValue
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupListDirect Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupListDirect", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetCodeDescription
    '
    ' Description: Gets the Description for a given List Code.
    '
    ' Created: 23/08/99 RFC
    ' ***************************************************************** '
    Public Function GetCodeDescription(ByVal v_sObjectName As String, ByVal v_sPropertyName As String, ByVal v_sCode As Object, ByRef r_sDescription As String) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lGISListID As Integer
        Dim s, sCode, sObjectName, sPropertyName As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oListManager Is Nothing Then
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to Obtain List for Property " & v_sPropertyName & " as List Manager Component is not Available.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCodeDescription")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sObjectName = v_sObjectName
            sPropertyName = v_sPropertyName

            ' Get the Property Definition Details
            lReturn = m_oDataSet.GetPropertyDefDetails(v_sObjectName:=sObjectName, v_sPropertyName:=sPropertyName, r_lGISListID:=lGISListID)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Is this a List Property
            If lGISListID > 0 Then


                sCode = CStr(v_sCode)
                ' Yes it is, then we can get the Description
                lReturn = CType(m_oListManager.GetDescription(sPropertyId:=CStr(lGISListID), sABICodeTarget:=sCode, sDescription:=s), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

                r_sDescription = s

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
    ' Name: GetCodeDescriptionByPropID
    '
    ' Description: Gets the Description for a given List Code given the PropID
    '
    ' Notes: This avoids having to look in the XML to resolve polaris object name,
    ' property name to a polaris ID. Previously we *had* to do a NBStart or
    ' NewDataSet before we can even use list management!!!
    '
    ' Created: 08/08/00 CL
    ' ***************************************************************** '
    Public Function GetCodeDescriptionByPropID(ByVal v_lPolarisPropID As Integer, ByVal v_sCode As Object, ByRef r_sDescription As String) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lGISListID As Integer
        Dim s, sCode, sObjectName, sPropertyName As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oListManager Is Nothing Then
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to Obtain List for Property ID" & v_lPolarisPropID & " as List Manager Component is not Available.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCodeDescriptionByPropID")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lGISListID = v_lPolarisPropID

            ' Is this a List Property
            If lGISListID > 0 Then


                sCode = CStr(v_sCode)
                ' Yes it is, then we can get the Description
                lReturn = CType(m_oListManager.GetDescription(sPropertyId:=CStr(lGISListID), sABICodeTarget:=sCode, sDescription:=s), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

                r_sDescription = s

            Else

                ' No it isn't so show Error
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to get Description for Property ID " & v_lPolarisPropID & " as it is does NOT have a List ID defined.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCodeDescriptionByPropID")
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCodeDescriptionByPropID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCodeDescriptionByPropID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sActionXML, sDataModelCode, sDataSetXML, sDatasetDefXML As String

        Dim sActionReturnXML As String = ""
        Dim lReturnValue As Integer

        Dim oGIS As bGIS.Application


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sDataModelCode = m_oDataSet.GISDataModelCode
            lReturn = m_oDataSet.ReturnAsXML(sDatasetDefXML, sDataSetXML)


            oGIS = New bGIS.Application()

            lReturn = CType(oGIS, SSP.S4I.Interfaces.IBusiness).Initialise("", "", 1, 1, 1, 1, iGISSharedConstants.GetGISLogLevel(), ACApp)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If


            lReturn = oGIS.SaveToDB(v_sGisDataModelCode:=sDataModelCode, r_sXMLDataset:=sDataSetXML)


            oGIS = Nothing
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            'update the dataset
            lReturn = m_oDataSet.UpdateDataSetFromXML(sDataSetXML)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
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
    'RFC13012000 - Return Guaranteed Quote Date
    'RFC271000 - Add optional RiskID as required by underwriting.
    'DD21112001 - Added loading of the Quote Object
    'RFC051201 - Removed previous change DD21112001 as it was superflous
    '            and was causing problems were quotes were not stored in the db
    ' ***************************************************************** '
    Public Function LoadFromDB(ByVal v_sGisDataModelCode As String, ByVal v_sQuoteRef As String, ByVal v_sQuoteRefPassword As String, ByRef r_vTopOIKey As Object, ByRef r_vGuaranteedQuoteDate As Date, Optional ByVal v_lInsuranceFileCnt As Integer = -1, Optional ByVal v_lRiskID As Integer = -1) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sActionXML, sTopLevelObject, sTopLevelTable As String
        Dim vOIKeyArray() As Object
        Dim sDataSetDefFile, sDataSetFile As String
        Dim dtGuaranteedQuoteDate As Date
        Dim sActionReturnXML As String = ""
        Dim lReturnValue As gPMConstants.PMEReturnCode
        Dim sDataSetXML, sDatasetDefXML As String


        'DD 21/11/2001 - for Quote object
        Dim sTopLevelQuoteObject, sTopLevelQuoteTable As String
        Dim vOIKeyQuoteArray As Object
        Dim oGIS As bGIS.Application

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

            ' If we are loading via the Ins File Cnt then set
            ' Quote Ref & Password to nothing
            If v_lInsuranceFileCnt > 0 Then
                v_sQuoteRef = ""
                v_sQuoteRefPassword = ""
            End If

            '    ' Set the Action
            '    lReturn = FormatActionXML( _
            ''        v_lAction:=GISDSActionLoadFromDB, _
            ''        v_sSellerGUID:="", _
            ''        v_sDataModelCode:=v_sGisDataModelCode, _
            ''        v_sQuoteReference:=v_sQuoteRef, _
            ''        v_sQuoteRefPassword:=v_sQuoteRefPassword, _
            ''        v_lInsuranceFileCnt:=v_lInsuranceFileCnt, _
            ''        v_lRiskID:=v_lRiskID, _
            ''        r_sActionXML:=sActionXML)
            '    If (lReturn <> PMTrue) Then
            '        LoadFromDB = PMFalse
            '        Exit Function
            '    End If
            '
            '    ' Process the Action
            '    lReturn = ProcessActionViaHTTP( _
            ''        v_oDataSet:=m_oDataSet, _
            ''        v_sActionXML:=sActionXML, _
            ''        r_sActionReturnXML:=sActionReturnXML)
            '    If (lReturn <> PMTrue) Then
            '        LoadFromDB = PMFalse
            '        Exit Function
            '    End If
            '
            '    ' Check the ActionReturn Value
            '    ' Unformat the Action Return XML
            '    lReturn = UnFormatActionReturnXML( _
            ''        v_sActionReturnXML:=sActionReturnXML, _
            ''        r_lReturnValue:=lReturnValue, _
            ''        r_dtGuaranteedQuoteDate:=dtGuaranteedQuoteDate)
            '    If (lReturn <> PMTrue) Then
            '        LoadFromDB = PMFalse
            '        Exit Function
            '    End If

            'return the dataset as XML to pass to bGIS
            lReturn = m_oDataSet.ReturnAsXML(sDatasetDefXML, sDataSetXML)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturnValue
            End If

            oGIS = New bGIS.Application()

            lReturn = CType(oGIS, SSP.S4I.Interfaces.IBusiness).Initialise("", "", 1, 1, 1, 1, iGISSharedConstants.GetGISLogLevel(), ACApp)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturnValue
            End If


            lReturn = oGIS.LoadFromDB(r_sXMLDataSetDef:=sDatasetDefXML, r_sXMLDataset:=sDataSetXML, v_sGisDataModelCode:=v_sGisDataModelCode, r_vInsuranceFileCnt:=v_lInsuranceFileCnt, r_vQuoteRef:=v_sQuoteRef, r_vQuoteRefPassword:=v_sQuoteRefPassword, r_dtGuaranteedQuoteDate:=r_vGuaranteedQuoteDate, r_vRiskID:=v_lRiskID)

            oGIS = Nothing

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturnValue
            End If


            lReturn = m_oDataSet.UpdateDataSetFromXML(sDataSetXML)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturnValue
            End If

            'RFC13012000 - Return Guaranteed Quote Date
            r_vGuaranteedQuoteDate = dtGuaranteedQuoteDate

            ' Check the Return Value
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturnValue
            End If

            ' Get the Top Level Object
            lReturn = m_oDataSet.GetTopLevelRiskObject(r_sObjectName:=sTopLevelObject, r_sTableName:=sTopLevelTable)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Instances of this Object
            ' Note: There should be one ONLY

            lReturn = m_oDataSet.GetAllOIKey(v_sObjectName:=sTopLevelObject, r_vOIKeyArray:=vOIKeyArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the Top Level Object Key

            ' Do we have an Array
            If Not Information.IsArray(vOIKeyArray) Then
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Find an Instance of Object : " & sTopLevelObject, vApp:=ACApp, vClass:=ACClass, vMethod:="NewDataSet")
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                ' Return the first one



                r_vTopOIKey = vOIKeyArray(vOIKeyArray.GetLowerBound(0))
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
    ' Name: NBStart
    '
    ' Description: Start a New Business Quote
    '
    ' RFC260600 - Added
    ' ***************************************************************** '
    Public Function NBStart(ByVal v_sGisDataModelCode As String, ByVal v_sGISBusinessTypeCode As String, ByRef r_vPolicyLinkID As Integer, ByRef r_vTopOIKey As Object, ByRef r_vQuoteRef As String, ByRef r_vQuoteRefPassword As String, Optional ByVal v_lPartyCnt As Integer = -1, Optional ByRef r_vInsuranceFileCnt As Integer = 0, Optional ByRef r_vAdditionalDataArray(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim sXMLDataSetDef, sXMLDataSet As String
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sActionXML As String = ""

        Dim sActionReturnXML As String = ""
        Dim lReturnValue As gPMConstants.PMEReturnCode

        Dim lPolicyLinkID As Integer
        Dim sTopOIKey, sQuoteRef, sQuoteRefPassword As String
        Dim lInsuranceFileCnt As Integer

        Dim sTopLevelObject, sTopLevelTable As String
        Dim vOIKeyArray() As Object

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

            ' Set the Action Properties
            lReturn = CType(FormatActionXML(v_lAction:=iGISSharedConstants.GISDSActionNBStart, v_sSellerGUID:="", r_sActionXML:=sActionXML, v_sBusinessTypeCode:=v_sGISBusinessTypeCode, v_sDataModelCode:=v_sGisDataModelCode, v_lPartyCnt:=v_lPartyCnt, v_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Send and Process The Command
            lReturn = CType(ProcessActionViaHTTP(v_oDataSet:=m_oDataSet, v_sActionXML:=sActionXML, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the ActionReturn Value
            ' Unformat the Action Return XML
            lReturn = CType(UnFormatActionReturnXML(v_sActionReturnXML:=sActionReturnXML, r_lReturnValue:=lReturnValue, r_sQuoteReference:=sQuoteRef, r_sQuoteRefPassword:=sQuoteRefPassword, r_lInsuranceFileCnt:=lInsuranceFileCnt, r_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the Return Value
            If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturnValue
            End If

            ' Get the Top Level Object
            lReturn = m_oDataSet.GetTopLevelRiskObject(r_sObjectName:=sTopLevelObject, r_sTableName:=sTopLevelTable)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Instances of this Object
            ' Note: There should be one ONLY

            lReturn = m_oDataSet.GetAllOIKey(v_sObjectName:=sTopLevelObject, r_vOIKeyArray:=vOIKeyArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the Top Level Object Key

            ' Do we have an Array
            If Not Information.IsArray(vOIKeyArray) Then
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Find an Instance of Object : " & sTopLevelObject, vApp:=ACApp, vClass:=ACClass, vMethod:="NewDataSet")
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                ' Return the first one



                r_vTopOIKey = vOIKeyArray(vOIKeyArray.GetLowerBound(0))
            End If

            ' Return the Policy Link ID
            r_vPolicyLinkID = PolicyLinkID()

            ' Return the Quote Ref details
            r_vQuoteRef = sQuoteRef
            r_vQuoteRefPassword = sQuoteRefPassword

            ' Return the Insurance File Cnt
            r_vInsuranceFileCnt = lInsuranceFileCnt

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NBStartFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="NBStart", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PrintForm
    '
    ' Description: RJG 13082001 - added.
    '              Created for the Continuation cover notes in the
    '              Towergate system. Calls existing PrintForm
    '              function in GIS
    '
    ' ***************************************************************** '
    Public Function PrintForm(ByVal v_sGisDataModelCode As String, ByVal v_sGISBusinessTypeCode As String, ByVal v_lFormNumber As Integer, ByVal v_lGISSchemeID As Integer) As Integer

        Dim result As Integer = 0
        Try

            Dim lReturn As gPMConstants.PMEReturnCode
            Dim sActionXML As String = ""

            Dim sActionReturnXML As String = ""
            Dim lReturnValue As gPMConstants.PMEReturnCode

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Action Properties
            lReturn = CType(FormatActionXML(v_lAction:=iGISSharedConstants.GISDSActionPrintForm, v_sSellerGUID:="", r_sActionXML:=sActionXML, v_sBusinessTypeCode:=v_sGISBusinessTypeCode, v_sDataModelCode:=v_sGisDataModelCode, v_lSchemeID:=v_lGISSchemeID, v_lFormNumber:=v_lFormNumber), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Send and Process The Command
            lReturn = CType(ProcessActionViaHTTP(v_oDataSet:=m_oDataSet, v_sActionXML:=sActionXML, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the ActionReturn Value
            ' Unformat the Action Return XML
            lReturn = CType(UnFormatActionReturnXML(v_sActionReturnXML:=sActionReturnXML, r_lReturnValue:=lReturnValue), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the Return Value
            If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturnValue
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PrintForm Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: MTAStart
    '
    ' Description: Start a Mid Term Adjustment Quote
    '
    ' RFC260600 - Added
    '
    ' CL061200 - v_dtCoverStartDate & v_dtExpiryDate changed to strings otherwise ASP
    '            pages will swap month and day
    ' ***************************************************************** '
    Public Function MTAStart(ByVal v_sGisDataModelCode As String, ByVal v_sGISBusinessTypeCode As String, ByVal v_iType As Integer, ByVal v_dtCoverStartDate As Object, ByVal v_dtExpiryDate As Object, ByVal v_lPolicyVersion As Integer, Optional ByVal v_lOldPolicyLinkID As Integer = -1, Optional ByVal v_lOldInsuranceFileCnt As Integer = -1, Optional ByRef r_vNewPolicyLinkID As Integer = 0, Optional ByRef r_vNewInsuranceFileCnt As Integer = 0, Optional ByRef r_vAdditionalDataArray(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim sXMLDataSetDef, sXMLDataSet As String
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sActionXML As String = ""

        Dim sActionReturnXML As String = ""
        Dim lReturnValue As gPMConstants.PMEReturnCode

        Dim lNewPolicyLinkID, lNewInsuranceFileCnt As Integer

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

            ' Set the Action Properties


            lReturn = CType(FormatActionXML(v_lAction:=iGISSharedConstants.GISDSActionMTAStart, v_sSellerGUID:="", r_sActionXML:=sActionXML, v_sBusinessTypeCode:=v_sGISBusinessTypeCode, v_sDataModelCode:=v_sGisDataModelCode, v_iType:=v_iType, v_dtCoverStartDate:=CDate(v_dtCoverStartDate), v_dtExpiryDate:=CDate(v_dtExpiryDate), v_lPolicyVersion:=v_lPolicyVersion, v_lPolicyLinkID:=v_lOldPolicyLinkID, v_lInsuranceFileCnt:=v_lOldInsuranceFileCnt, v_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Send and Process The Command
            lReturn = CType(ProcessActionViaHTTP(v_oDataSet:=m_oDataSet, v_sActionXML:=sActionXML, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the ActionReturn Value
            ' Unformat the Action Return XML
            lReturn = CType(UnFormatActionReturnXML(v_sActionReturnXML:=sActionReturnXML, r_lReturnValue:=lReturnValue, r_lInsuranceFileCnt:=lNewInsuranceFileCnt, r_lPolicyLinkID:=lNewPolicyLinkID, r_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the Return Value
            If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturnValue
            End If

            ' Return the New Policy Link ID and InsuranceFileCnt
            r_vNewPolicyLinkID = lNewPolicyLinkID
            r_vNewInsuranceFileCnt = lNewInsuranceFileCnt

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MTAStartFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="MTAStart", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: NBQuote
    '
    ' Description: Perform a NBQuote.
    '
    ' RFC13012000 - Add Effective Date Param
    ' ***************************************************************** '
    Public Function NBQuote(ByVal v_lQuoteType As Integer, ByVal v_sGISBusinessTypeCode As String, ByVal v_dtEffectiveDate As Object, Optional ByVal v_lGISSchemeID As Integer = -1, Optional ByRef r_vAdditionalDataArray(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim sXMLDataSetDef, sXMLDataSet As String
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lPolicyLinkID As Integer
        Dim sActionXML, sDataModelCode As String

        Dim sActionReturnXML As String = ""
        Dim lReturnValue As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' RFC300300 - Clear all Quote Output that may already exist
            ' as there is no need to Pass it back across the network.
            lReturn = m_oDataSet.ClearAllQuoteOutput()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sDataModelCode = m_oDataSet.GISDataModelCode

            ' Set the Action Properties

            lReturn = CType(FormatActionXML(v_lAction:=iGISSharedConstants.GISDSActionQuote, v_sSellerGUID:="", r_sActionXML:=sActionXML, v_lQuoteType:=v_lQuoteType, v_sBusinessTypeCode:=v_sGISBusinessTypeCode, v_sDataModelCode:=sDataModelCode, v_lSchemeID:=v_lGISSchemeID, v_dtEffectiveDate:=CDate(v_dtEffectiveDate), v_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Send and Process The Command
            lReturn = CType(ProcessActionViaHTTP(v_oDataSet:=m_oDataSet, v_sActionXML:=sActionXML, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the ActionReturn Value
            ' Unformat the Action Return XML
            lReturn = CType(UnFormatActionReturnXML(v_sActionReturnXML:=sActionReturnXML, r_lReturnValue:=lReturnValue, r_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the Return Value
            If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturnValue
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NBQuoteFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="NBQuote", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: MTAQuote
    '
    ' Description: Perform a MTAQuote.
    '
    ' RFC13012000 - Add Effective Date Param
    ' ***************************************************************** '
    Public Function MTAQuote(ByVal v_lQuoteType As Integer, ByVal v_sGISBusinessTypeCode As String, ByVal v_dtEffectiveDate As Object, ByVal sXMLOldRisk As String, Optional ByVal v_lGISSchemeID As Integer = -1, Optional ByRef r_vAdditionalDataArray(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim sXMLDataSetDef, sXMLDataSet As String
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lPolicyLinkID As Integer
        Dim sActionXML, sDataModelCode As String

        Dim sActionReturnXML As String = ""
        Dim lReturnValue As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sDataModelCode = m_oDataSet.GISDataModelCode

            'iPMFunc.LogMessage _
            'iType:=PMLogOnError, _
            'sMsg:="sXMLOldRisk = " & sXMLOldRisk, _
            'vApp:=ACApp, _
            'vClass:=ACClass, _
            'vMethod:="MTAQuote"


            ' Set the Action Properties

            lReturn = CType(FormatActionXML(v_lAction:=iGISSharedConstants.GISDSActionMTAQuote, v_sSellerGUID:="", r_sActionXML:=sActionXML, v_lQuoteType:=v_lQuoteType, v_sBusinessTypeCode:=v_sGISBusinessTypeCode, v_sDataModelCode:=sDataModelCode, v_lSchemeID:=v_lGISSchemeID, v_dtEffectiveDate:=CDate(v_dtEffectiveDate), v_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Send and Process The Command
            lReturn = CType(ProcessActionViaHTTP(v_oDataSet:=m_oDataSet, v_sActionXML:=sActionXML, r_sActionReturnXML:=sActionReturnXML, v_sXMLOldRisk:=sXMLOldRisk), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the ActionReturn Value
            ' Unformat the Action Return XML
            lReturn = CType(UnFormatActionReturnXML(v_sActionReturnXML:=sActionReturnXML, r_lReturnValue:=lReturnValue, r_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the Return Value
            If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturnValue
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
    Public Function MTATransact(ByVal v_lQuoteType As Integer, ByVal v_sGISBusinessTypeCode As String, ByVal v_dtEffectiveDate As Object, ByVal sXMLOldRisk As String, Optional ByVal v_lGISSchemeID As Integer = -1, Optional ByRef r_vAdditionalDataArray(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim sXMLDataSetDef, sXMLDataSet As String
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lPolicyLinkID As Integer
        Dim sActionXML, sDataModelCode As String

        Dim sActionReturnXML As String = ""
        Dim lReturnValue As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sDataModelCode = m_oDataSet.GISDataModelCode

            ' Set the Action Properties

            lReturn = CType(FormatActionXML(v_lAction:=iGISSharedConstants.GISDSActionMTATransact, v_sSellerGUID:="", r_sActionXML:=sActionXML, v_lQuoteType:=v_lQuoteType, v_sBusinessTypeCode:=v_sGISBusinessTypeCode, v_sDataModelCode:=sDataModelCode, v_lSchemeID:=v_lGISSchemeID, v_dtEffectiveDate:=CDate(v_dtEffectiveDate), v_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Send and Process The Command
            lReturn = CType(ProcessActionViaHTTP(v_oDataSet:=m_oDataSet, v_sActionXML:=sActionXML, r_sActionReturnXML:=sActionReturnXML, v_sXMLOldRisk:=sXMLOldRisk), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the ActionReturn Value
            ' Unformat the Action Return XML
            lReturn = CType(UnFormatActionReturnXML(v_sActionReturnXML:=sActionReturnXML, r_lReturnValue:=lReturnValue, r_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the Return Value
            If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturnValue
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
    ' Name: UpdateQuoteRef
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function UpdateQuoteRef(ByVal v_sQuoteRef As String, ByVal v_sQuoteRefPassword As String) As Integer

        Dim result As Integer = 0
        Dim lPolicyLinkID As Integer
        Dim sActionXML, sActionReturnXML As String
        Dim lReturnValue As gPMConstants.PMEReturnCode
        Dim sDataModelCode As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lPolicyLinkID = m_oDataSet.PolicyLinkID()
            sDataModelCode = m_oDataSet.GISDataModelCode

            ' Set the Action
            lReturn = CType(FormatActionXML(v_lAction:=iGISSharedConstants.GISDSActionUpdateQtePassword, v_sSellerGUID:="", v_sDataModelCode:=sDataModelCode, v_lPolicyLinkID:=lPolicyLinkID, v_sQuoteReference:=v_sQuoteRef, v_sQuoteRefPassword:=v_sQuoteRefPassword, r_sActionXML:=sActionXML), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Process the Action
            lReturn = CType(ProcessActionViaHTTP(v_oDataSet:=m_oDataSet, v_sActionXML:=sActionXML, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the ActionReturn Value
            ' Unformat the Action Return XML
            lReturn = CType(UnFormatActionReturnXML(v_sActionReturnXML:=sActionReturnXML, r_lReturnValue:=lReturnValue), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the Return Value
            If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturnValue
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateQuoteRefFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateQuoteRef", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdatePartyCnt
    '
    ' Description: Store SBO Party Cnt in GIS_policy_link
    '
    ' RAG 15/06/2000
    ' ***************************************************************** '
    Public Function UpdatePartyCnt(ByVal v_lPartyCnt As Object, ByVal v_lInsuranceFileCnt As Object) As Integer

        Dim result As Integer = 0
        Dim lPolicyLinkID As Integer
        Dim sActionXML, sActionReturnXML As String
        Dim lReturnValue As gPMConstants.PMEReturnCode
        Dim sDataModelCode As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lPolicyLinkID = m_oDataSet.PolicyLinkID()
            sDataModelCode = m_oDataSet.GISDataModelCode

            ' Set the Action


            lReturn = CType(FormatActionXML(v_lAction:=iGISSharedConstants.GISDSActionUpdatePartyCnt, v_sSellerGUID:="", v_sDataModelCode:=sDataModelCode, v_lPolicyLinkID:=lPolicyLinkID, v_lPartyCnt:=CInt(v_lPartyCnt), v_lInsuranceFileCnt:=CInt(v_lInsuranceFileCnt), r_sActionXML:=sActionXML), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Process the Action
            lReturn = CType(ProcessActionViaHTTP(v_oDataSet:=m_oDataSet, v_sActionXML:=sActionXML, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the ActionReturn Value
            ' Unformat the Action Return XML
            lReturn = CType(UnFormatActionReturnXML(v_sActionReturnXML:=sActionReturnXML, r_lReturnValue:=lReturnValue), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the Return Value
            If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturnValue
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePartyCntFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePartyCnt", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: NBTransact
    '
    ' Description: Make the New Policy Live
    '
    '
    ' ***************************************************************** '
    Public Function NBTransact(ByVal v_lGISSchemeID As Integer, Optional ByVal v_sGISBusinessTypeCode As String = "", Optional ByRef r_vAdditionalDataArray(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sActionXML, sDataModelCode As String

        Dim sActionReturnXML As String = ""
        Dim lReturnValue As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sDataModelCode = m_oDataSet.GISDataModelCode

            ' Set the Action Properties
            lReturn = CType(FormatActionXML(v_lAction:=iGISSharedConstants.GISDSActionNBTransact, v_sSellerGUID:="", r_sActionXML:=sActionXML, v_sDataModelCode:=sDataModelCode, v_lSchemeID:=v_lGISSchemeID, v_sBusinessTypeCode:=v_sGISBusinessTypeCode, v_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Send and Process The Command
            lReturn = CType(ProcessActionViaHTTP(v_oDataSet:=m_oDataSet, v_sActionXML:=sActionXML, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the ActionReturn Value
            ' Unformat the Action Return XML
            lReturn = CType(UnFormatActionReturnXML(v_sActionReturnXML:=sActionReturnXML, r_lReturnValue:=lReturnValue, r_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the Return Value
            If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturnValue
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
    ' Name: Refer
    '
    ' Description: Refer the Risk to the Specified Insurer OR
    '              Neutral Broker if no insurer specified.
    '
    '               RAG201100 - Changed Parameters
    '
    ' ***************************************************************** '
    Public Function Refer(ByVal v_sGisDataModelCode As String, ByVal v_sGISBusinessTypeCode As String, ByVal v_sInsurerCode As String, Optional ByRef r_vAdditionalDataArray(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sActionXML, sDataModelCode As String

        Dim sActionReturnXML As String = ""
        Dim lReturnValue As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'sDataModelCode = m_oDataSet.GISDataModelCode
            sDataModelCode = v_sGisDataModelCode

            ' Set the Action Properties
            lReturn = CType(FormatActionXML(v_lAction:=iGISSharedConstants.GISDSActionRefer, v_sSellerGUID:="", r_sActionXML:=sActionXML, v_sDataModelCode:=sDataModelCode, v_sInsurerCode:=v_sInsurerCode, v_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Send and Process The Command
            lReturn = CType(ProcessActionViaHTTP(v_oDataSet:=m_oDataSet, v_sActionXML:=sActionXML, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the ActionReturn Value
            ' Unformat the Action Return XML
            ' RFC040501 - Added Additional Data Array Parameter
            lReturn = CType(UnFormatActionReturnXML(v_sActionReturnXML:=sActionReturnXML, r_lReturnValue:=lReturnValue, r_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the Return Value
            If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturnValue
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReferFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="Refer", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: PostcodeSearch
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function PostcodeSearch(ByVal v_sNameNum As String, ByVal v_sPostcode As String, ByRef r_vMatchArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lReturnValue As gPMConstants.PMEReturnCode
        Dim sDataModelCode, sActionXML, sActionReturnXML As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sDataModelCode = m_oDataSet.GISDataModelCode

            ' Set the Action
            lReturn = CType(FormatActionXML(v_lAction:=iGISSharedConstants.GISDSActionSearchForAddress, v_sSellerGUID:="", v_sDataModelCode:=sDataModelCode, v_sSearchNameNum:=v_sNameNum, v_sSearchPostCode:=v_sPostcode, r_sActionXML:=sActionXML), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Process the Action
            lReturn = CType(ProcessActionViaHTTP(v_oDataSet:=m_oDataSet, v_sActionXML:=sActionXML, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the ActionReturn Value
            ' Unformat the Action Return XML

            lReturn = CType(UnFormatActionReturnXML(v_sActionReturnXML:=sActionReturnXML, r_lReturnValue:=lReturnValue, r_vAddressArray:=r_vMatchArray), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the Return Value
            If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturnValue
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostcodeSearchFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="PostcodeSearch", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    ' Name: ReturnAsXML
    '
    ' Description: Return the Data Set.
    '
    ' ***************************************************************** '
    Public Function ReturnAsXML(ByRef r_vXMLDataSet As String) As Integer

        Dim result As Integer = 0
        Dim sXMLDataSetDef, sXMLDataSet As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' RFC170701 - If there is no Dataset then just return empty string
            If m_oDataSet Is Nothing Then
                sXMLDataSet = ""
                sXMLDataSetDef = ""
            Else
                result = m_oDataSet.ReturnAsXML(r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet)
            End If

            r_vXMLDataSet = sXMLDataSet

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReturnAsXMLFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReturnAsXML", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    '              ChildNumber
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
    Public Function GetInstanceHierarchy(ByRef v_sObjectName As String, ByRef r_vObjectInstanceArray(,) As Object, ByRef r_lMaxInstances As Integer) As Integer

        Dim result As Integer = 0
        Try


            Return m_oDataSet.GetInstanceHierarchy(v_sObjectName:=v_sObjectName, r_vObjectInstanceArray:=r_vObjectInstanceArray, r_lMaxInstances:=r_lMaxInstances)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInstanceHierarchyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInstanceHierarchy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Public Function GetObjectIdentity(ByRef v_sObjectName As String, ByRef v_sOIKey As String, ByRef r_vPropertyArray(,) As Object) As Integer

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
    ' Name:         VehicleLookup
    '
    ' Description:  Look up vehicle details from the Registration Mark.
    '               Vehicle details will be stored directly into the
    '               dataset if match is successful.
    '
    ' Author:       RAG171100
    '
    ' ***************************************************************** '
    Public Function VehicleLookup(ByVal v_sGisDataModelCode As String, ByVal v_sGISBusinessTypeCode As String, ByVal v_sRegistrationNumber As String, Optional ByRef r_vAdditionalDataArray(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sActionXML, sDataModelCode As String

        Dim sActionReturnXML As String = ""
        Dim lReturnValue As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'sDataModelCode = m_oDataSet.GISDataModelCode
            sDataModelCode = v_sGisDataModelCode

            ' Set the Action Properties
            lReturn = CType(FormatActionXML(v_lAction:=iGISSharedConstants.GISDSActionVehicleLookup, v_sSellerGUID:="", r_sActionXML:=sActionXML, v_sDataModelCode:=sDataModelCode, v_sVehicleReg:=v_sRegistrationNumber, v_sBusinessTypeCode:=v_sGISBusinessTypeCode, v_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Send and Process The Command
            lReturn = CType(ProcessActionViaHTTP(v_oDataSet:=m_oDataSet, v_sActionXML:=sActionXML, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the ActionReturn Value
            ' Unformat the Action Return XML
            lReturn = CType(UnFormatActionReturnXML(v_sActionReturnXML:=sActionReturnXML, r_lReturnValue:=lReturnValue), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the Return Value
            If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturnValue
            End If


            Return result

        Catch excep As System.Exception



            ' Error
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="VehicleLookup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="VehicleLookup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetVehicleStyleList
    '
    ' Description: Returns the associated Vehicle Models
    '
    ' 'DB 31/1/2000
    ' 'IJM 17/07/2003 Added optional parameter to filter by year of
    ' '               manufacture
    ' ***************************************************************** '
    Public Function GetVehicleStyleList(ByVal v_sObjectName As String, ByVal v_sPropertyName As String, ByRef r_vListData As Object, ByVal v_vMake As String, ByVal v_vModel As String, ByVal v_lYear As Integer) As Integer

        Dim result As Integer = 0
        Const PROPERTY_ID As Integer = 393220 'Property id for the Vehicle Search


        Dim lGISListID As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        ' More to do here

        If m_oListManager Is Nothing Then
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to Obtain List for Property " & v_sPropertyName & " as List Manager Component is not Available.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicleStyleList")
            Return result
        End If

        ' Get the Property Definition Details
        Dim lReturn As gPMConstants.PMEReturnCode = m_oDataSet.GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=v_sPropertyName, r_lGISListID:=lGISListID)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' Is this the Vehicle List Polaris Property ?

        If lGISListID = PROPERTY_ID Then


            m_lReturn = m_oListManager.GetVehicleStyleList(r_vListData:=r_vListData, v_vMake:=v_vMake, v_sModel:=v_vModel, v_lYear:=v_lYear)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error getting list from ListManager", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicleStyleList")
                Return result
            End If

        Else

            ' No it isn't so show Error
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to obtain Vehicle List as the property" & Environment.NewLine & _
              "given is NOT the Vehicle List Polaris Property", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicleStyleList")

        End If

        Return result



        ' Error Section.
        iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error:" & Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicleStyleList")


        Return gPMConstants.PMEReturnCode.PMFalse

    End Function

    ' ***************************************************************** '
    ' Name: GetVehicleModelsByYear
    '
    ' Description: Returns the associated Vehicle List
    '
    ' ***************************************************************** '
    Public Function GetVehicleModelsByYear(ByVal v_sObjectName As String, ByVal v_sPropertyName As String, ByRef r_vListData As Object, ByVal v_vMake As String, ByVal v_vYear As Integer) As Integer

        Dim result As Integer = 0
        Const PROPERTY_ID As Integer = 393220 'Property id for the Vehicle Search

        Dim lGISListID As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        ' More to do here
        If m_oListManager Is Nothing Then
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to Obtain List for Property " & v_sPropertyName & " as List Manager Component is not Available.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicleModelsByYear")
            Return result
        End If

        ' Get the Property Definition Details
        Dim lReturn As gPMConstants.PMEReturnCode = m_oDataSet.GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=v_sPropertyName, r_lGISListID:=lGISListID)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' Is this the Vehicle List Polaris Property ?

        If lGISListID = PROPERTY_ID Then


            m_lReturn = m_oListManager.GetVehicleModelsByYear(r_vListData:=r_vListData, v_vMake:=v_vMake, v_vYear:=v_vYear)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error Getting list from List Manager", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicleModelsByYear")
                Return result
            End If

        Else

            ' No it isn't so show Error
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to obtain Vehicle List as the property" & Environment.NewLine & _
              "given is NOT the Vehicle List Polaris Property", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicleModelsByYear")

        End If

        Return result



        ' Error Section.
        iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error:" & Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicleModelsByYear")


        Return gPMConstants.PMEReturnCode.PMFalse

    End Function

    ' ***************************************************************** '
    ' Name: GetVehicleEngineCapacity
    '
    ' Description: Returns the associated Vehicle Models
    '
    ' 'DB 31/1/2000
    ' 'IJM 17/07/2003 Added optional parameter to filter by year of
    ' '               manufacture
    ' ***************************************************************** '
    Public Function GetVehicleEngineCapacity(ByVal v_sObjectName As String, ByVal v_sPropertyName As String, ByRef r_vListData As Object, ByVal v_vMake As String, ByVal v_vModel As String, ByVal v_lYear As Integer, ByVal v_vStyleCode As String) As Integer

        Dim result As Integer = 0
        Const PROPERTY_ID As Integer = 393220 'Property id for the Vehicle Search


        Dim lGISListID As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        ' More to do here

        If m_oListManager Is Nothing Then
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to Obtain List for Property " & v_sPropertyName & " as List Manager Component is not Available.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicleEngineCapacity")
            Return result
        End If

        ' Get the Property Definition Details
        Dim lReturn As gPMConstants.PMEReturnCode = m_oDataSet.GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=v_sPropertyName, r_lGISListID:=lGISListID)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' Is this the Vehicle List Polaris Property ?

        If lGISListID = PROPERTY_ID Then


            m_lReturn = m_oListManager.GetVehicleEngineCapacity(r_vListData:=r_vListData, v_vMake:=v_vMake, v_sModel:=v_vModel, v_lYear:=v_lYear, v_vStyleCode:=v_vStyleCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error getting list from ListManager", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicleEngineCapacity")
                Return result
            End If

        Else

            ' No it isn't so show Error
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to obtain Vehicle List as the property" & Environment.NewLine & _
              "given is NOT the Vehicle List Polaris Property", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicleEngineCapacity")

        End If

        Return result



        ' Error Section.
        iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error:" & Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicleEngineCapacity")


        Return gPMConstants.PMEReturnCode.PMFalse

    End Function


    ' ***************************************************************** '
    ' Name: GetVehicleTrim
    '
    ' Description: Returns the associated Vehicle Models
    '
    ' 'DB 31/1/2000
    ' 'IJM 17/07/2003 Added optional parameter to filter by year of
    ' '               manufacture
    ' ***************************************************************** '
    Public Function GetVehicleTrim(ByVal v_sObjectName As String, ByVal v_sPropertyName As String, ByRef r_vListData As Object, ByVal v_vMake As String, ByVal v_vModel As String, ByVal v_lYear As Integer, ByVal v_vStyleCode As String, ByVal v_vEngineType As String) As Integer

        Dim result As Integer = 0
        Const PROPERTY_ID As Integer = 393220 'Property id for the Vehicle Search


        Dim lGISListID As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        ' More to do here

        If m_oListManager Is Nothing Then
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to Obtain List for Property " & v_sPropertyName & " as List Manager Component is not Available.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicleTrim")
            Return result
        End If

        ' Get the Property Definition Details
        Dim lReturn As gPMConstants.PMEReturnCode = m_oDataSet.GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=v_sPropertyName, r_lGISListID:=lGISListID)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' Is this the Vehicle List Polaris Property ?

        If lGISListID = PROPERTY_ID Then


            m_lReturn = m_oListManager.GetVehicleTrim(r_vListData:=r_vListData, v_vMake:=v_vMake, v_sModel:=v_vModel, v_lYear:=v_lYear, v_vStyleCode:=v_vStyleCode, v_vEngineType:=v_vEngineType)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error getting list from ListManager", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicleTrim")
                Return result
            End If

        Else

            ' No it isn't so show Error
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to obtain Vehicle List as the property" & Environment.NewLine & _
              "given is NOT the Vehicle List Polaris Property", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicleTrim")

        End If

        Return result



        ' Error Section.
        iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error:" & Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicleTrim")


        Return gPMConstants.PMEReturnCode.PMFalse

    End Function

    ' ***************************************************************** '
    ' Name: GetTransmissionType
    '
    ' Description: Returns the associated Vehicle Models
    '
    ' 'DB 31/1/2000
    ' 'IJM 17/07/2003 Added optional parameter to filter by year of
    ' '               manufacture
    ' ***************************************************************** '
    Public Function GetTransmissionType(ByVal v_sObjectName As String, ByVal v_sPropertyName As String, ByRef r_vListData As Object, ByVal v_vMake As String, ByVal v_vModel As String, ByVal v_lYear As Integer, ByVal v_vStyleCode As String, ByVal v_vEngineType As String, ByVal v_vTrimName As String) As Integer

        Dim result As Integer = 0
        Const PROPERTY_ID As Integer = 393220 'Property id for the Vehicle Search


        Dim lGISListID As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        ' More to do here

        If m_oListManager Is Nothing Then
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to Obtain List for Property " & v_sPropertyName & " as List Manager Component is not Available.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTransmissionType")
            Return result
        End If

        ' Get the Property Definition Details
        Dim lReturn As gPMConstants.PMEReturnCode = m_oDataSet.GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=v_sPropertyName, r_lGISListID:=lGISListID)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' Is this the Vehicle List Polaris Property ?

        If lGISListID = PROPERTY_ID Then


            m_lReturn = m_oListManager.GetTransmissionType(r_vListData:=r_vListData, v_vMake:=v_vMake, v_sModel:=v_vModel, v_lYear:=v_lYear, v_vStyleCode:=v_vStyleCode, v_vEngineType:=v_vEngineType, v_vTrimName:=v_vTrimName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error getting list from ListManager", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTransmissionType")
                Return result
            End If

        Else

            ' No it isn't so show Error
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to obtain Vehicle List as the property" & Environment.NewLine & _
              "given is NOT the Vehicle List Polaris Property", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTransmissionType")

        End If

        Return result



        ' Error Section.
        iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error:" & Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:="GetTransmissionType")


        Return gPMConstants.PMEReturnCode.PMFalse

    End Function

    ' ***************************************************************** '
    ' Name: GetVehicle
    '
    ' Description: Returns the associated Vehicle Models
    '
    ' 'DB 31/1/2000
    ' 'IJM 17/07/2003 Added optional parameter to filter by year of
    ' '               manufacture
    ' ***************************************************************** '
    Public Function GetVehicle(ByVal v_sObjectName As String, ByVal v_sPropertyName As String, ByRef r_vListData As Object, ByVal v_vMake As String, ByVal v_vModel As String, ByVal v_lYear As Integer, ByVal v_vStyleCode As String, ByVal v_vEngineType As String, ByVal v_vTrimName As String, ByVal v_vTransmissionType As String) As Integer

        Dim result As Integer = 0
        Const PROPERTY_ID As Integer = 393220 'Property id for the Vehicle Search


        Dim lGISListID As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        ' More to do here

        If m_oListManager Is Nothing Then
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to Obtain List for Property " & v_sPropertyName & " as List Manager Component is not Available.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicle")
            Return result
        End If

        ' Get the Property Definition Details
        Dim lReturn As gPMConstants.PMEReturnCode = m_oDataSet.GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=v_sPropertyName, r_lGISListID:=lGISListID)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' Is this the Vehicle List Polaris Property ?

        If lGISListID = PROPERTY_ID Then


            m_lReturn = m_oListManager.GetVehicle(r_vListData:=r_vListData, v_vMake:=v_vMake, v_sModel:=v_vModel, v_lYear:=v_lYear, v_vStyleCode:=v_vStyleCode, v_vEngineType:=v_vEngineType, v_vTrimName:=v_vTrimName, v_vTransmissionType:=v_vTransmissionType)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error getting list from ListManager", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicle")
                Return result
            End If

        Else

            ' No it isn't so show Error
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to obtain Vehicle List as the property" & Environment.NewLine & _
              "given is NOT the Vehicle List Polaris Property", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicle")

        End If

        Return result



        ' Error Section.
        iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error:" & Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicle")


        Return gPMConstants.PMEReturnCode.PMFalse

    End Function

    '' ***************************************************************** '
    '' Name: GetVehicleModels
    ''
    '' Description: Returns the associated Vehicle Models
    ''
    '' 'DB 31/1/2000
    '' 'IJM 17/07/2003 Added optional parameter to filter by year of
    '' '               manufacture
    '' ***************************************************************** '
    'Public Function GetVehicleModels( _
    ''    ByVal v_sObjectName As String, _
    ''    ByVal v_sPropertyName As String, _
    ''    ByRef r_vListData As Variant, _
    ''    ByVal v_vMake As String, _
    ''    Optional ByVal v_lYear As Long = 0) As Long
    '
    'Const PROPERTY_ID = 393220 'Property id for the Vehicle Search
    '
    'Dim lReturn As Long
    '
    'Dim lGISListID As Long
    '
    '    GetVehicleModels = PMTrue
    '
    '    ' More to do here
    '
    '    If (m_oListManager Is Nothing = True) Then
    '        iPMFunc.LogMessage _
    ''            sUsername:="", _
    ''            iType:=PMLogInfo, _
    ''            sMsg:="Unable to Obtain List for Property " & v_sPropertyName & " as List Manager Component is not Available.", _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="GetVehicleModels"
    '        Exit Function
    '    End If
    '
    '    ' Get the Property Definition Details
    '    lReturn = m_oDataSet.GetPropertyDefDetails( _
    ''        v_sObjectName:=v_sObjectName, _
    ''        v_sPropertyName:=v_sPropertyName, _
    ''        r_lGISListID:=lGISListID)
    '    If (lReturn <> PMTrue) Then
    '        GetVehicleModels = lReturn
    '        Exit Function
    '    End If
    '
    '    ' Is this the Vehicle List Polaris Property ?
    '
    '    If (lGISListID = PROPERTY_ID) Then
    '
    '        m_lReturn = m_oListManager.GetVehicleModels( _
    ''        r_vListData:=r_vListData, _
    ''        v_vMake:=v_vMake)
    '
    '        If m_lReturn <> PMTrue Then
    '            iPMFunc.LogMessage _
    ''                sUsername:="", _
    ''                iType:=PMLogOnError, _
    ''                sMsg:="Error getting list from ListManager", _
    ''                vApp:=ACApp, _
    ''                vClass:=ACClass, _
    ''                vMethod:="GetVehicleModels"
    '            Exit Function
    '        End If
    '
    '    Else
    '
    '        ' No it isn't so show Error
    '        iPMFunc.LogMessage _
    ''            sUsername:="", _
    ''            iType:=PMLogInfo, _
    ''            sMsg:="Unable to obtain Vehicle List as the property" & vbNewLine & _
    ''                  "given is NOT the Vehicle List Polaris Property", _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="GetVehicleModels"
    '
    '    End If
    '
    '    Exit Function
    '
    'Err_GetVehicleModels:
    '
    '    ' Error Section.
    '    iPMFunc.LogMessage _
    ''        sUsername:="", _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="Error:" & Err.Description, _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="GetVehicleModels"
    '
    '    GetVehicleModels = PMFalse
    '
    '    Exit Function
    '
    'End Function


    ' ***************************************************************** '
    ' Name: GetVehicleModelByID
    '
    ' Description: Returns the associated Vehicle List
    '
    ' ***************************************************************** '
    Public Function GetVehicleModelByID(ByVal v_sObjectName As String, ByVal v_sPropertyName As String, ByRef r_sListData As String, ByVal v_vMake As String, ByVal v_vModel As String, ByVal v_lYear As Integer, ByVal v_lVehicleID As Integer) As Integer

        Dim result As Integer = 0
        Const PROPERTY_ID As Integer = 393220 'Property id for the Vehicle Search

        Dim sListData As String = ""
        Dim lGISListID As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        ' More to do here
        If m_oListManager Is Nothing Then
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to Obtain List for Property " & v_sPropertyName & " as List Manager Component is not Available.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicleModelByID")
            Return result
        End If

        ' Get the Property Definition Details
        Dim lReturn As gPMConstants.PMEReturnCode = m_oDataSet.GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=v_sPropertyName, r_lGISListID:=lGISListID)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' Is this the Vehicle List Polaris Property ?

        If lGISListID = PROPERTY_ID Then

            m_lReturn = m_oListManager.GetVehicleModelByID(r_sListData:=sListData, v_vMake:=v_vMake, v_sModel:=v_vModel, v_lYear:=v_lYear, v_lVehicleID:=v_lVehicleID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error Getting list from List Manager", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicleModelByID")
                Return result
            End If

        Else
            ' No it isn't so show Error
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to obtain Vehicle List as the property" & Environment.NewLine & _
              "given is NOT the Vehicle List Polaris Property", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicleModelByID")
        End If

        r_sListData = sListData

        Return result



        ' Error Section.
        iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error:" & Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicleModelByID")


        Return gPMConstants.PMEReturnCode.PMFalse

    End Function


    ' ***************************************************************** '
    '
    ' Name: IsInsurerQMM
    '
    ' Description:  Returns true if the Insurer uses QMM Schemes
    '
    ' History: 28/11/2001 DD - Created.
    '
    ' ***************************************************************** '
    Public Function IsInsurerQMM(ByVal v_sGisDataModelCode As String, ByVal v_vInsurerNo As Object, ByRef r_bIsInsurerQMM As Object) As Integer

        Dim result As Integer = 0
        Try


            Return GISCall("Application", "IsInsurerQMM", v_sGisDataModelCode, v_vInsurerNo, r_bIsInsurerQMM)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsInsurerQMM Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsInsurerQMM", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name:         SendEmail
    '
    ' Description:  Look up vehicle details from the Registration Mark.
    '               Vehicle details will be stored directly into the
    '               dataset if match is successful.
    '
    ' Author:       RAG171100
    '
    ' ***************************************************************** '
    Public Function SendEmail(ByVal v_sGisDataModelCode As String, ByVal v_sGISBusinessTypeCode As String, ByVal v_lEMailType As Integer, ByVal v_sEMailFrom As String, ByVal v_sEMailTo As String, ByVal v_sEMailCC As String, ByVal v_sEMailSubject As String, ByVal v_sEMailText As String, Optional ByRef r_vAdditionalDataArray(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sActionXML, sDataModelCode, sDataSetDefFile, sDataSetFile, sActionReturnXML As String
        Dim lReturnValue As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'sDataModelCode = m_oDataSet.GISDataModelCode
            sDataModelCode = v_sGisDataModelCode

            ' Set the Action Properties
            lReturn = CType(FormatActionXML(v_lAction:=iGISSharedConstants.GISDSActionSendEmail, v_sSellerGUID:="", r_sActionXML:=sActionXML, v_sDataModelCode:=sDataModelCode, v_sBusinessTypeCode:=v_sGISBusinessTypeCode, v_lEMailType:=v_lEMailType, v_sEMailFrom:=v_sEMailFrom, v_sEMailTo:=v_sEMailTo, v_sEMailCC:=v_sEMailCC, v_sEMailSubject:=v_sEMailSubject, v_sEMailText:=v_sEMailText, v_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' CJB070302 - If m_oDataSet not set then load a new one to prevent errors later
            ' (this is required due to a change in the parameters to ProcessActionViaHTTP)
            If m_oDataSet Is Nothing Then

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

            End If

            ' Send and Process The Command
            lReturn = CType(ProcessActionViaHTTP(v_oDataSet:=m_oDataSet, v_sActionXML:=sActionXML, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the ActionReturn Value
            ' Unformat the Action Return XML
            lReturn = CType(UnFormatActionReturnXML(v_sActionReturnXML:=sActionReturnXML, r_lReturnValue:=lReturnValue), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the Return Value
            If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturnValue
            End If

            Return result

        Catch excep As System.Exception



            ' Error
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SendEmail Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SendEmail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetRegSetting
    '
    ' Description: Call through to bGIS to allow an ASP page
    '              to retrieve a PM Registry setting on the server.
    '
    ' History: 15/01/2002 DD - Created.
    '
    ' ***************************************************************** '
    Public Function GetRegSetting(ByVal v_lPMERegSettingRoot As Object, ByVal v_lPMEProductFamily As Object, ByVal v_lPMERegSettingLevel As Object, ByVal v_sSettingName As Object, ByRef r_sSettingValue As Object, Optional ByVal v_sSubKey As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try


            Return GISCall("Application", "GetRegSetting", v_lPMERegSettingRoot, v_lPMEProductFamily, v_lPMERegSettingLevel, v_sSettingName, r_sSettingValue, v_sSubKey)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRegSetting Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRegSetting", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GetAddressFromAddressCnt
    '
    ' Description:
    '
    ' History: 08/08/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Public Function GetAddressFromAddressCnt(ByRef v_lAddressCnt As Object, ByRef r_vAddressArray(,) As Object) As Integer

        ' Debug message
        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".GetAddressFromAddressCnt")

        Try


            result = GISCall("Application", "GetAddress", v_lAddressCnt, r_vAddressArray)

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".GetAddressFromAddressCnt")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".GetAddressFromAddressCnt")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAddressFromAddressCnt Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAddressFromAddressCnt", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    '**** Added By: AAB  -  Added On:  19-Sep-2002 08:31 ****
    '****************************************************************** '
    ' Name: GetObjectDefDetails
    '
    ' Description: Returns the Definition Details for the
    '              given Object.
    '
    ' ***************************************************************** '
    Public Function GetObjectDefDetails(ByRef v_sObjectName As String, Optional ByRef r_lIsQuoteObject As Integer = 0, Optional ByRef r_lGISObjectID As Integer = 0, Optional ByRef r_sTableName As String = "", Optional ByRef r_lMaxInstances As Integer = 0, Optional ByRef r_lPolarisObjectID As Integer = 0, Optional ByRef r_sParentObjectName As String = "", Optional ByRef r_vChildObjectArray(,) As Object = Nothing, Optional ByRef r_vPropertyArray As Object = Nothing, Optional ByRef r_sSelectSQL As String = "", Optional ByRef r_sInsertSQL As String = "", Optional ByRef r_sUpdateSQL As String = "", Optional ByRef r_sDeleteSQL As String = "") As Integer

        Dim result As Integer = 0
        Try


            Return m_oDataSet.GetObjectDefDetails(v_sObjectName:=v_sObjectName, r_lIsQuoteObject:=r_lIsQuoteObject, r_lGISObjectID:=r_lGISObjectID, r_sTableName:=r_sTableName, r_lMaxInstances:=r_lMaxInstances, r_lPolarisObjectID:=r_lPolarisObjectID, r_sParentObjectName:=r_sParentObjectName, r_vChildObjectArray:=r_vChildObjectArray, r_vPropertyArray:=r_vPropertyArray, r_sSelectSQL:=r_sSelectSQL, r_sInsertSQL:=r_sInsertSQL, r_sUpdateSQL:=r_sUpdateSQL, r_sDeleteSQL:=r_sDeleteSQL)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetObjectDefDetailsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetObjectDefDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class

