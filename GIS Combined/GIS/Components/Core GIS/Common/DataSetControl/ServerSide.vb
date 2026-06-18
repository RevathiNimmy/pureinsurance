Option Strict Off
Option Explicit On
Imports System.Globalization
Imports System.IO
Imports System.Text
Imports System.Xml
Imports System.Xml.Schema
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Application_NET.Application")>
Public NotInheritable Class Application
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Application
    '
    ' Date: 23/07/2002
    '
    ' Description: Main Class which Controls the Creating/Setting of
    '              the Data Sets held in Memory.
    '
    ' Edit History:
    ' RFC230702 - New version based on original cGISDataSetControl but using
    '             a new format for the XML to improve performance.
    ' JRD09032005 PN18822 - Use QuoteKey as part of OIKey for QuoteObjects
    ' JRD01102005 PN24533 - Correctly process single double-quote characters in XML
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 19/09/2003
    ' Username.
    Private m_sUsername As String = ""
    Private Const vbObjectError As Integer = -2147221504

    ' Password.
    Private m_sPassword As String = ""

    ' User ID
    Private m_iUserID As Integer

    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Application"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)


    ' The Output Structure Definition
    Private m_oDataSetDef As XmlDocument
    ' The Data Set
    Private m_oDataset As XmlDocument
    'RDT 20051130 - Added creation of XSD document
    Private m_oDataSetXSD As XmlDocument

    ' Used to apply defaults to the dataset - CJB 280301
    Dim m_oDefaults As XmlDocument

    ' Temporary Arrays which are used to load up the Object/Property Definitions
    Private m_vTempObjectArray(,) As Object
    Private m_vTempPropertyArray(,) As Object

    ' Temporary String used to build up the Data Set DTD
    Private m_sTempObjectDTD As String = ""
    Private m_sTempAttribDTD As String = ""

    'Private m_oNode As cGISDataSetControl.Node

    'RFC190400 - Add Lookup Methods to GIS
    Private m_oLookupManager As cGISLookupManager.Lookup

    ' Hold the Object Instance Number outside of the XML
    Private m_lNextOINumber As Integer

    ' PRIVATE Data Members (End)
    Private m_sDataModelType As String
    Private m_bStrict As Boolean


    Friend Property Strict() As Boolean
        Get
            Return m_bStrict
        End Get
        Set(ByVal Value As Boolean)
            m_bStrict = Value
        End Set
    End Property

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property GISDataModelCode() As String
        Get
            If m_oDataSetDef Is Nothing Then
                Return ""
            Else

                Return CStr(m_oDataSetDef.DocumentElement.GetAttribute(iGISSharedConstants.GISXMLAttribDataModelCode))
            End If
        End Get
    End Property

    ' RDC 30072001 timestamp attribute determines if client/server files are in-sync
    Public ReadOnly Property GISDSDTimestamp() As String
        Get
            Try
                If m_oDataSetDef Is Nothing Then
                    Return ""
                Else
                    Return CStr(m_oDataSetDef.DocumentElement.GetAttribute(iGISSharedConstants.GISXMLAttribDataSetDefTimestamp))
                End If

            Catch
            End Try

            Return ""
        End Get
    End Property

    ' RFC090300 - Scripting Method Access Added
    Public ReadOnly Property Risk() As cGISDataSetControl.Node
        Get

            Try

                ' Set the Root to the RISK_OBJECTS
                'SetRoot

                ' Return the Node so that an Item call can be done from there

                Return SetRoot()

            Catch excep As System.Exception
                Throw New System.Exception(Informations.Err().Number.ToString() + ", " + excep.Source + ", " + excep.Message)
                Exit Property
            End Try
        End Get
    End Property

    Public ReadOnly Property Quote(ByVal v_lQuoteNum As Integer) As cGISDataSetControl.Node
        Get

            Try

                ' Set the Root to the Quote Number Specified
                'SetRoot v_lQuoteNum

                ' Return the Node so that an Item call can be done from there

                Return SetRoot(v_lQuoteNum)

            Catch excep As System.Exception

                Throw New System.Exception(SSP.Shared.Informations.Err().Number.ToString() + ", " + excep.Source + ", " + excep.Message)

                Exit Property

            End Try
        End Get
    End Property

    Public ReadOnly Property QuoteCount() As Integer
        Get
            Try

                ' Return the Number of Quote Objects Nodes in the Document

                Return m_oDataset.DocumentElement.GetElementsByTagName(ACXMLQuoteObjects).Count

            Catch excep As System.Exception
                Throw New System.Exception(Informations.Err().Number.ToString() + ", " + excep.Source + ", " + excep.Message)

                Exit Property

            End Try
        End Get
    End Property

    'RFC190400 - Add Lookup Methods to GIS
    Public Property LookupsRequiredInsurerNo() As Integer
        Get
            Return m_oLookupManager.RequiredInsurerNumber
        End Get
        Set(ByVal Value As Integer)
            m_oLookupManager.RequiredInsurerNumber = Value
        End Set
    End Property

    'RFC110501 - Next Object Instance Number
    ' This is needed so that when
    Public Property NextOINumber() As Integer
        Get
            Return m_lNextOINumber
        End Get
        Set(ByVal Value As Integer)
            m_lNextOINumber = Value
        End Set
    End Property


    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)

    ' FRIEND Property Procedures (Begin)

    ' RFC010300 - Add Scripting Class

    Friend ReadOnly Property DatasetXMLDoc() As XmlDocument
        'Public ReadOnly Property DatasetXMLDoc() As XmlDocument
        Get
            Return m_oDataset
        End Get
    End Property

    'RFC190400 - Add Lookup Methods to GIS
    Friend ReadOnly Property LookupManager() As cGISLookupManager.Lookup
        Get
            Return m_oLookupManager
        End Get
    End Property

    Public Function InitialiseLookups(ByRef v_sDataModelCode As String, ByRef v_sBusinessTypeCode As String, ByRef v_dtProcessDate As Date, ByRef v_lStatus As Integer) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oLookupManager = New cGISLookupManager.Lookup()

            lReturn = CType(m_oLookupManager.OpenFiles(sModelCode:=v_sDataModelCode, sBusinessType:=v_sBusinessTypeCode, lProcessDate:=v_dtProcessDate, lStatus:=v_lStatus, lOpenIfNotExist:=True), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            'CloseFiles() is obselete
            'lReturn = m_oLookupManager.CloseFiles()

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InitialiseLookups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseLookups", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: InitialiseDataSet
    '
    ' Description: Initialise the Memory Structure from the Object and
    '              Property Arrays supplied.
    '
    ' ***************************************************************** '
    Public Function InitialiseDataSet(ByRef v_sGisDataModelCode As String,
                                      ByRef v_vObjectArray(,) As Object,
                                      ByRef v_vPropertyArray(,) As Object,
                                      Optional ByVal v_bSupressXSDProduction As Boolean = True,
                                      Optional ByVal v_sDataModelType As String = "") As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sDataSetDTD As String = ""
        Dim bLoaded As Boolean
        Dim oActionElem, oRiskObjElem As XmlElement
        Dim sTemp As String = ""

        Dim sTopLevelObjName As String = ""
        Dim sTopLevelTableName As String = ""
        Dim lSaveToDBMode As Integer
        Dim sTopLevelQuoteObject As String = ""
        Dim sTopLevelQuoteTable As String = ""
        Dim sClearSQL As String = ""

        Dim sSpecialObject As String = ""
        Dim sSpecialTable As String = ""
        Dim lNonGISType As Integer
        Dim sChildSpecial As String = ""

        Dim vChildObjectArray As Object = Nothing
        Dim vPropertyArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_sDataModelType = v_sDataModelType
            ' Set the Tempoarary Arrays
            m_vTempObjectArray = v_vObjectArray.Clone()
            m_vTempPropertyArray = v_vPropertyArray.Clone()

            ' Initialise the Definition
            lReturn = CType(InitDataSetDef(v_sGisDataModelCode:=v_sGisDataModelCode), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Load the Objects from the Database
            lReturn = CType(LoadObjectDefs(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not v_bSupressXSDProduction Then
                ' Build the XSD structure
                lReturn = CType(BuildDataSetXSD(v_sGisDataModelCode:=v_sGisDataModelCode, v_vObjectArray:=v_vObjectArray, v_vPropertyArray:=v_vPropertyArray), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Drop the two Arrays
              m_vTempObjectArray = Nothing
              m_vTempPropertyArray = Nothing

            ' Get the Save to DB Mode for this Data Model Code
            lSaveToDBMode = iGISSharedConstants.GetLoadSaveDBMode(v_sGisDataModelCode)



            ' Build the DTD
            lReturn = CType(BuildDataSetDTD(sDataSetDTD, lSaveToDBMode), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' The top level element has to be added before we
            ' can load this DTD. In this case it is an empty DataSet element
            'sDataSetDTD = sDataSetDTD & "<" & ACXMLDataSet & "/>"

            ' Do not Validate the Document Straight Away

            'TODO: 
            'm_oDataset.validateOnParse = False

            ' Load the Data Set DTD
            Dim temp_xml_result As Boolean
            Try
                m_oDataset.LoadXml(sDataSetDTD)
                temp_xml_result = True

            Catch parseError As System.Exception
                temp_xml_result = False
            End Try
            bLoaded = temp_xml_result

            '' Check to See that the XML Loaded
            If Not bLoaded Then


                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Load XML Data Set Definition. Reason = " & m_oDataset.InnerText, vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseDataSet")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'm_oDataset = New XmlDocument()

            ' Create the RiskObjects Element
            'oRiskObjElem = m_oDataset.CreateElement(ACXMLRiskObjects)

            ' Set the OIKey Attrbiute for the RiskObjectsNode to RiskObjects
            'oRiskObjElem.SetAttribute(ACXMLAttribOIKey, ACXMLRiskObjects)
            Dim xmlNode As XmlNode = m_oDataset.CreateNode(XmlNodeType.Element, ACXMLRiskObjects, "")
            Dim xmlAttribute As XmlAttribute = m_oDataset.CreateAttribute(ACXMLAttribOIKey)
            xmlAttribute.Value = ACXMLRiskObjects
            xmlNode.Attributes.Append(xmlAttribute)
            m_oDataset.ImportNode(xmlNode, True)
            m_oDataset.DocumentElement.AppendChild(xmlNode)

            ' Append as a Child of the DataSet Element

            'm_oDataset.DocumentElement.AppendChild(xmlNode)
            'm_oDataset.AppendChild(xmlNode)

            ''RFC200400 - Add Proper Delete Functionality Start
            '' Create the DeletedObjects Element
            'oDelObjsElem = m_oDataset.CreateElement(ACXMLDeletedObjects)

            '' Set the OIKey Attrbiute for the DeletedObjectsNode to DeletedObjects
            'oDelObjsElem.SetAttribute(ACXMLAttribOIKey, ACXMLDeletedObjects)
            'xmlNode = oDelObjsElem
            '' Append as a Child of the DataSet Element

            ''m_oDataset.DocumentElement.AppendChild(oDelObjsElem)
            'm_oDataset.DocumentElement.AppendChild(xmlNode)
            ''RFC200400 - Add Proper Delete Functionality End

            xmlNode = m_oDataset.CreateNode(XmlNodeType.Element, ACXMLDeletedObjects, "")
            xmlAttribute = m_oDataset.CreateAttribute(ACXMLAttribOIKey)
            xmlAttribute.Value = ACXMLDeletedObjects
            xmlNode.Attributes.Append(xmlAttribute)
            m_oDataset.ImportNode(xmlNode, True)
            m_oDataset.DocumentElement.AppendChild(xmlNode)

            ' Clear and Recreate the Quote Output Area.
            lReturn = CType(ClearAllQuoteOutput(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the DataSet Data Model Code Attribute
            m_oDataset.DocumentElement.SetAttribute(iGISSharedConstants.GISXMLAttribDataModelCode, v_sGisDataModelCode)

            ' RFC29111999 Hold the Next Object Instance Num at the top level only.
            m_oDataset.DocumentElement.SetAttribute(ACXMLAttribNextOINumber, 1)

            ' Release References
            oActionElem = Nothing
            oRiskObjElem = Nothing

            ' Build and Assign the SQL Statements to the Object Defs

            ' Get the Top Level Object Name
            lReturn = CType(GetTopLevelRiskObject(r_sObjectName:=sTopLevelObjName, r_sTableName:=sTopLevelTableName), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Build the SQL and Add for each Object
            ' RFC240101 - This was passing the sTopLevelObjName for the TopLevelTableName param
            lReturn = CType(AddObjectDefSQL(v_sObjectName:=sTopLevelObjName, v_sTopLevelTableName:=sTopLevelTableName), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If we are saving quotes
            If lSaveToDBMode = iGISSharedConstants.GISRegLoadSaveDBModeFastWithQuotes Then
                ' Get the Top Level QUote Object Name
                lReturn = CType(GetTopLevelQuoteObject(r_sObjectName:=sTopLevelQuoteObject, r_sTableName:=sTopLevelQuoteTable), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                sTopLevelQuoteObject = ""
                sTopLevelQuoteTable = ""
            End If

            ' RFC120802 - Need to build the SetID XSL
            lReturn = CType(BuildSetIDXSL(v_sGisDataModelCode:=v_sGisDataModelCode, v_sTopLevelObjectName:=sTopLevelObjName, v_sTopLevelQuoteObject:=sTopLevelQuoteObject), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If we are not using the old slow methods then
            If lSaveToDBMode <> iGISSharedConstants.GISRegLoadSaveDBModeSlow Then

                If lSaveToDBMode = iGISSharedConstants.GISRegLoadSaveDBModeFastWithQuotesXML Then
                    lReturn = CType(BuildSelectSP_FOR_XML(v_sObjectName:=sTopLevelObjName, v_bTopLevelObject:=True, v_bQuoteObject:=False), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Else
                    ' RFC240101 - Need to Build the Select Stored Procs Here
                    lReturn = CType(BuildSelectSP(v_sObjectName:=sTopLevelObjName, v_bTopLevelObject:=True, v_bQuoteObject:=False), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

                ' Build the Create Update Delete Stored Procs
                lReturn = CType(BuildCUDSP(v_sObjectName:=sTopLevelObjName, v_bTopLevelObject:=True, v_bQuoteObject:=False), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' If we are saving Quotes in the DB also then
                '     need to gen Select SP's for Quotes
                '     AND Save XSL for Quotes
                If lSaveToDBMode >= iGISSharedConstants.GISRegLoadSaveDBModeFastWithQuotes Then

                    If lSaveToDBMode = iGISSharedConstants.GISRegLoadSaveDBModeFastWithQuotesXML Then
                        lReturn = CType(BuildSelectSP_FOR_XML(v_sObjectName:=sTopLevelQuoteObject, v_bTopLevelObject:=True, v_bQuoteObject:=True), gPMConstants.PMEReturnCode)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Else
                        ' Build Stored Procedures for Quote Objects ALSO
                        lReturn = CType(BuildSelectSP(v_sObjectName:=sTopLevelQuoteObject, v_bTopLevelObject:=True, v_bQuoteObject:=True), gPMConstants.PMEReturnCode)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If

                    ' Build the Create Update Delete Stored Procs
                    lReturn = CType(BuildCUDSP(v_sObjectName:=sTopLevelQuoteObject, v_bTopLevelObject:=True, v_bQuoteObject:=True), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' RFC170501 - Need to build Clear All Quote Outut SP
                    lReturn = CType(BuildClearAllQuoteOutputSP(v_sObjectName:=sTopLevelQuoteObject, v_bTopLevelObject:=True, r_sSQL:=sClearSQL), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' RFC270301 - Need to build the Save to DB XSL for Quotes also
                    lReturn = CType(BuildSaveXSL(v_sGisDataModelCode:=v_sGisDataModelCode, v_sTopLevelObjectName:=sTopLevelObjName, v_sTopLevelQuoteObject:=sTopLevelQuoteObject), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Else

                    ' RFC270301 - Need to build the Save to DB XSL No Quotes
                    lReturn = CType(BuildSaveXSL(v_sGisDataModelCode:=v_sGisDataModelCode, v_sTopLevelObjectName:=sTopLevelObjName), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            End If

            ' RFC121101 - Build Copy Policy SP
            ' If we are saving quotes
            If lSaveToDBMode = iGISSharedConstants.GISRegLoadSaveDBModeFastWithQuotes Then
                ' Then need to be able to copy them also
                lReturn = CType(BuildCopyDatasetSP(v_sTopRiskObjectName:=sTopLevelObjName, v_sTopQuoteObjectName:=sTopLevelQuoteObject), gPMConstants.PMEReturnCode)
            Else
                ' Copy Risk Only
                lReturn = CType(BuildCopyDatasetSP(v_sTopRiskObjectName:=sTopLevelObjName), gPMConstants.PMEReturnCode)
            End If
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Look for an Associated Client Object. This will always be at the top level.
            lReturn = CType(GetSpecialObject(sSpecialObject, lNonGISType), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Select Case lNonGISType
                ' Associated Client
                Case GISDataModelType.GISOTAssociatedClient

                    ' RFC 300103 - Associated Client and Disclosures
                    ' Get the Object Definition Details
                    lReturn = CType(GetObjectDefDetails(v_sObjectName:=sSpecialObject, r_sTableName:=sSpecialTable, r_vChildObjectArray:=vChildObjectArray, r_vPropertyArray:=vPropertyArray), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lReturn
                    End If

                    ' Need to get the details for the Disclosures Object
                    If Informations.IsArray(vChildObjectArray) Then

                        sChildSpecial = CStr(vChildObjectArray(0))
                    Else
                        iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="No Disclosures Object defined in Data Model : " & GISDataModelCode, vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseDataSet")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If



                    lReturn = CType(BuildAssocClientSelSP(v_sObjectName:=sSpecialObject, v_sTableName:=sSpecialTable, vPropertyArray:=vPropertyArray, sDiscObj:=sChildSpecial), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    lReturn = CType(BuildAssocClientCUDSP(sSpecialObject), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    lReturn = CType(BuildDisclosureCUDSP(sChildSpecial), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                ' Claims
                Case GISDataModelType.GISOTClaim

                    ' RFC 300103 - Claims
                    ' Get the Object Definition Details
                    lReturn = CType(GetObjectDefDetails(v_sObjectName:=sSpecialObject, r_sTableName:=sSpecialTable, r_vChildObjectArray:=vChildObjectArray, r_vPropertyArray:=vPropertyArray), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lReturn
                    End If

                    ' Need to get the details for the Disclosures Object
                    If Informations.IsArray(vChildObjectArray) Then

                        sChildSpecial = CStr(vChildObjectArray(0))
                    Else
                        iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="No Claim Peril Object defined in Data Model : " & GISDataModelCode, vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseDataSet")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Build the Claims Select Stored Proc

                    lReturn = CType(BuildClaimSelSP(v_sObjectName:=sSpecialObject, v_sTableName:=sSpecialTable, vPropertyArray:=vPropertyArray, sClaimPeril:=sChildSpecial), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Build Claim Create Update Delete Stored Proc
                    lReturn = CType(BuildClaimCUDSP(sSpecialObject), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Build Claim Peril Create Update Delete Stored Proc
                    lReturn = CType(BuildClaimPerilCUDSP(sChildSpecial), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' RFC260203 - Build the Copy Claim to Work (& vice versa) stored procs
                    lReturn = CType(BuildCopyClaimSP(v_sObjectName:=sSpecialObject, v_sPerilObjName:=sChildSpecial), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Not a Special
                Case Else

                    ' Nothing to do.

            End Select

            Return result

        Catch excep As System.Exception





            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InitialiseDataSetFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseDataSet", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LoadFromXMLFile
    '
    ' Description: Load the Data Set Definition and Data Set from
    '              existing files.
    '
    ' ***************************************************************** '
    Public Function LoadFromXMLFile(ByRef v_sDataSetDefFile As String, ByRef v_sDataSetFile As String) As Integer

        Dim result As Integer = 0
        Dim bLoaded As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise Data Model Definition
            m_oDataSetDef = New XmlDocument()
            m_oDataset = New XmlDocument()

            ' Use the new parser


            'm_oDataSetDef.setProperty("NewParser", True)

            ' developer guide no solution no. 38
            'm_oDataset.setProperty("NewParser", True)



            'm_oDataSetDef.validateOnParse = False
            m_oDataSetDef.PreserveWhitespace = False
            ' There is no DTD for the Data Set Def

            'TODO
            'm_oDataSetDef.resolveExternals = False
            m_oDataSetDef.XmlResolver = Nothing

            Dim temp_xml_result As Boolean
            Try
                m_oDataSetDef.Load(v_sDataSetDefFile)
                temp_xml_result = True

            Catch parseError As System.Exception
                temp_xml_result = False
            End Try
            bLoaded = temp_xml_result
            If Not bLoaded Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogDebug1, sMsg:="Unable to Load Data Set Definition from File : " & v_sDataSetDefFile, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXMLFile")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            'm_oDataset.validateOnParse = False
            m_oDataset.PreserveWhitespace = False
            ' There IS an DTD for the Data Set, but it is Internal


            'm_oDataset.resolveExternals = False
            m_oDataset.XmlResolver = Nothing

            Dim temp_xml_result2 As Boolean
            Try
                m_oDataset.Load(v_sDataSetFile)
                temp_xml_result2 = True

            Catch parseError As System.Exception
                temp_xml_result2 = False
            End Try
            bLoaded = temp_xml_result2
            If Not bLoaded Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:="Unable to Load Data Set from File : " & v_sDataSetFile, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXMLFile")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the Next Object Instance Number

            NextOINumber = CInt(m_oDataset.DocumentElement.GetAttribute(ACXMLAttribNextOINumber))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadFromXMLFileFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXMLFile", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: LoadFromXMLDefUnknown
    '
    ' Description: Calls LoadFromXML without supplying v_sXMLDataSetDef
    '              Would have preferred to change v_sXMLDataSetDef param to optional in LoadFromXML
    '              but this would have broken compat which was too big an impact at the time of writing
    ' History
    ' RAW 16/10/2003 : CQ2755 : created
    ' ***************************************************************** '
    Public Function LoadFromXMLDefUnknown(ByVal v_sGisDataModelCode As String, ByRef v_sXMLDataSet As String) As Integer

        Dim result As Integer = 0
        Dim sXMLDataSetDef As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sDataSetDefFile As String = ""
        Dim sDataSetFile As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Path/FileNames for stored EMPTY Data Set Files
            ' of this Data Model Type
            lReturn = CType(iGISSharedConstants.GetDataSetFileNames(v_sDataModelCode:=v_sGisDataModelCode, r_sDataSetDefFile:=sDataSetDefFile, r_sDataSetFile:=sDataSetFile), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetDataSetFileNames", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXMLDefUnknown")
                Return result
            End If

            ' Try to load from the Empty XML files
            lReturn = CType(LoadFromXMLFile(v_sDataSetDefFile:=sDataSetDefFile, v_sDataSetFile:=sDataSetFile), gPMConstants.PMEReturnCode)

            ' If we could NOT Load from stored Empty Files
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Note that we are not attempting to build it ourselves from DB as done in bGIS
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetDataSetFileNames", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXMLDefUnknown")
                Return result
            End If


            sXMLDataSetDef = m_oDataSetDef.InnerXml


            ' Now lets load the data that we have been given
            lReturn = CType(LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=v_sXMLDataSet), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to LoadFromXML", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXMLDefUnknown")
                Return result
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadFromXMLDefUnknownFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXMLDefUnknown", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: LoadFromXML
    '
    ' Description: Loads the Data Set & Data Set Definition from the
    '              XML strings supplied.
    '
    ' ***************************************************************** '
    Public Function LoadFromXML(ByRef v_sXMLDataSetDef As String, ByRef v_sXMLDataSet As String) As Integer

        Dim result As Integer = 0
        Dim bLoaded As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise Data Model Definition
            m_oDataSetDef = New XmlDocument()
            m_oDataset = New XmlDocument()

            ' Use the new parser


            'm_oDataSetDef.setProperty("NewParser", True)


            'm_oDataset.setProperty("NewParser", True)



            'm_oDataSetDef.validateOnParse = False
            m_oDataSetDef.PreserveWhitespace = False
            ' There is no DTD for the Data Set Def


            'm_oDataSetDef.resolveExternals = False
            m_oDataSetDef.XmlResolver = Nothing

            Dim temp_xml_result As Boolean
            Try
                m_oDataSetDef.LoadXml(v_sXMLDataSetDef)
                temp_xml_result = True

            Catch parseError As System.Exception
                temp_xml_result = False
            End Try
            bLoaded = temp_xml_result
            If Not bLoaded Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Load Data Set Definition from XML String", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXML")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' developer guide no solution no. 22
            'm_oDataset.validateOnParse = False
            m_oDataset.PreserveWhitespace = False
            ' There IS an DTD for the Data Set, but it is Internal


            'm_oDataset.resolveExternals = False
            m_oDataset.XmlResolver = Nothing

            Dim temp_xml_result2 As Boolean
            Try
                m_oDataset.LoadXml(v_sXMLDataSet)
                temp_xml_result2 = True

            Catch parseError As System.Exception
                temp_xml_result2 = False
            End Try
            bLoaded = temp_xml_result2
            If Not bLoaded Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Load Data Set from XML String", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXML")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the Next Object Instance Number

            NextOINumber = CInt(m_oDataset.DocumentElement.GetAttribute(ACXMLAttribNextOINumber))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadFromXMLFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXML", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateDataSetFromXML
    '
    ' Description: Updates the DataSet from the Supplied XML.
    '
    ' ***************************************************************** '
    Public Function UpdateDataSetFromXML(ByRef v_sXMLDataSet As String) As Integer

        Dim result As Integer = 0
        Dim bLoaded As Boolean
        Dim sDataModelCode As String = ""
        Dim oNewDataSet As XmlDocument

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise a New DataSet
            oNewDataSet = New XmlDocument()
            ' Use the new parser


            'oNewDataSet.setProperty("NewParser", True)

            ' Load the Data Set


            'oNewDataSet.validateOnParse = False
            Dim temp_xml_result As Boolean
            Try

                oNewDataSet.LoadXml(v_sXMLDataSet)
                temp_xml_result = True

            Catch parseError As System.Exception
                temp_xml_result = False
            End Try
            bLoaded = temp_xml_result
            If Not bLoaded Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Load Data Set from XML String", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateDataSetFromXML")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Data Model Code Attribute

            sDataModelCode = CStr(oNewDataSet.DocumentElement.GetAttribute(iGISSharedConstants.GISXMLAttribDataModelCode))

            ' If the Data Model Code from the Data Set MATCHES that
            ' from the currently loaded Data Set Definition then
            ' replace the currently loaded data set with the New Data Set
            ' otherwise Error.
            If sDataModelCode.ToUpper() = GISDataModelCode.ToUpper() Then
                ' CodesMatch so replace the Current Data Set
                m_oDataset = Nothing
                m_oDataset = oNewDataSet
                oNewDataSet = Nothing

                ' Get the next object instance number

                NextOINumber = CInt(m_oDataset.DocumentElement.GetAttribute(ACXMLAttribNextOINumber))
            Else
                ' Codes do NOT Match so Error
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Data Model Code from Data Set does not match that of the Data Set Definition.", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateDataSetFromXML")
                oNewDataSet = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateDataSetFromXMLFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateDataSetFromXML", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: UpdateSpecialObjects
    '
    ' Description: Updates the DataSet with the Special Objects.
    '              e.g. Associated Clients/Disclosures & Claims / ClaimPerils
    ' ***************************************************************** '
    Public Function UpdateSpecialObjects(ByVal v_sSpecialName As String, ByRef oSpecDOM As XmlDocument) As Integer

        Dim result As Integer = 0
        'Dim bLoaded As Boolean
        Dim sDataModelCode As String = ""
        Dim sMsg As String = ""
        Dim lNonGISType As Integer = 0
        Dim vOIKeyArray As Object = Nothing
        Dim sOIKey As String = ""
        Dim lFrom, lTo As Integer
        Dim oTopObj, oElem As XmlElement
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oObjInst As XmlElement
        Dim oParentInst As XmlNode

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            If oSpecDOM Is Nothing Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="DOM Supplied Is NOTHING", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateSpecialObjects")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the keys for any existing Special Objects

            lReturn = CType(GetAllOIKey(v_sSpecialName, vOIKeyArray), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' If there are any, delete them
            If Informations.IsArray(vOIKeyArray) Then

                lFrom = vOIKeyArray.GetLowerBound(0)

                lTo = vOIKeyArray.GetUpperBound(0)
                For lRow As Integer = lFrom To lTo

                    ' Get the Key

                    sOIKey = CStr(vOIKeyArray(lRow))

                    ' Get a reference to the object
                    oObjInst = GetObjectInstance(sOIKey)

                    If Not (oObjInst Is Nothing) Then

                        ' Get the Parent Object Instance
                        oParentInst = oObjInst.ParentNode

                        ' Remove the Object Instance from the Parent

                        oParentInst.RemoveChild(oObjInst)

                    End If

                Next lRow
            End If

            ' Add the new Specials into the Dataset
            If Not (oSpecDOM.DocumentElement.ChildNodes Is Nothing) Then

                ' Get a reference to the top level object,
                ' as the special sit under it
                'TBD this fails trying to add Claim which is already there
                Try

                    For Each oElem2 As XmlElement In oSpecDOM.DocumentElement.ChildNodes
                        oElem = oElem2
                        ' Set TopObj to be "???_POLICY_BINDER" node below "RISK_OBJECTS"
                        oTopObj = m_oDataset.DocumentElement.ChildNodes.Item(0).ChildNodes.Item(0)


                        oTopObj.AppendChild(m_oDataset.ImportNode(oElem, True))
                    Next oElem2
                Catch ex As Exception

                End Try
            End If

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateSpecialObjectsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateSpecialObjects", excep:=ex)

        End Try
        Return result


    End Function


    ' ***************************************************************** '
    ' Name: ReturnAsXML
    '
    ' Description: Return the Data Set and Data Set Definition as
    '              XML Strings.
    '
    ' ***************************************************************** '
    Public Function ReturnAsXML(ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataSet As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_sXMLDataSetDef = ""
            r_sXMLDataSet = ""

            r_sXMLDataSetDef = m_oDataSetDef.InnerXml

            ' RDT 06/11/02 - Check that the object exists
            If Not (m_oDataset.DocumentElement Is Nothing) Then

                ' Update the Next Object Instance number in the XML before it is returned
                m_oDataset.DocumentElement.SetAttribute(ACXMLAttribNextOINumber, m_lNextOINumber)

            End If

            r_sXMLDataSet = m_oDataset.InnerXml

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReturnAsXMLFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReturnAsXML", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: NewQuoteOutput
    '
    ' Description: Create a New Quote Output area, ready to be
    '              populated with the details from a Quotation.
    '
    ' Note: This is the initial Version of New Quote Output where the
    '       Quotes CANNOT be saved to the database.
    ' ***************************************************************** '
    Public Function NewQuoteOutput(ByRef v_lQEMNumber As Integer, ByRef v_sInsurer As String, ByRef v_lInsurerID As Integer, ByRef v_sScheme As String, ByRef v_lSchemeID As Integer, ByRef v_lSchemeVer As Integer, ByRef r_sQuoteKey As String) As Integer

        Dim result As Integer = 0
        Dim oQuotes, oQuoteObjects As XmlElement
        Dim lNextQuoteNum As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Quotes Instance to the Quotes Node
            oQuotes = GetObjectInstance(ACXMLQuotes)
            If oQuotes Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Next Quote Number

            lNextQuoteNum = CStr(oQuotes.GetAttribute(ACXMLAttribNextQuoteNum))
            If ToSafeDouble(lNextQuoteNum) < 1 Then
                lNextQuoteNum = CStr(1)
            End If

            ' Create the Quote Objects Node
            oQuoteObjects = m_oDataset.CreateElement(ACXMLQuoteObjects)

            ' Set the Scheme Out Attributes
            oQuoteObjects.SetAttribute(ACXMLAttribInsurer, v_sInsurer)
            oQuoteObjects.SetAttribute(ACXMLAttribInsurerID, v_lInsurerID)
            oQuoteObjects.SetAttribute(ACXMLAttribScheme, v_sScheme)
            oQuoteObjects.SetAttribute(ACXMLAttribSchemeID, v_lSchemeID)
            oQuoteObjects.SetAttribute(ACXMLAttribSchemeVer, v_lSchemeVer)

            ' Build the Quote Key from a combination of the
            ' Quote Engine Mapper Number and Quote Number
            v_lQEMNumber *= 1000000
            r_sQuoteKey = ACQuotePrefix & CStr(v_lQEMNumber + CDbl(lNextQuoteNum))

            ' Set the Quote Objects Key
            oQuoteObjects.SetAttribute(ACXMLAttribOIKey, r_sQuoteKey)

            ' Set the Next Quote Number Attribute
            lNextQuoteNum = CStr(CDbl(lNextQuoteNum) + 1)
            oQuotes.SetAttribute(ACXMLAttribNextQuoteNum, lNextQuoteNum)

            ' Append the Quote Objects to the Quotes Node

            oQuotes.AppendChild(oQuoteObjects)

            ' Release Local References
            oQuotes = Nothing
            oQuoteObjects = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewQuoteOutputFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewQuoteOutput", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: NewQuoteOutputSaveDB
    '
    ' Description: Create a New Quote Output area, ready to be
    '              populated with the details from a Quotation.
    '
    ' Note: This is the new version of the Quote Output where
    '       the Quotes can be saved to the Database.
    ' ***************************************************************** '
    Public Function NewQuoteOutputSaveDB(ByVal v_lGISSchemeID As Integer, ByRef r_sQuoteKey As String, ByRef r_sTopQuoteOIKey As String) As Integer

        Dim result As Integer = 0
        Dim oQuotes, oQuoteObjects As XmlElement
        Dim lNextQuoteNum As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sDataModelCode As String = ""
        Dim sTopQuoteObject As String = ""
        Dim sTopQuoteTable As String = ""
        Dim lNextOINumber As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Quotes Instance to the Quotes Node
            oQuotes = GetObjectInstance(ACXMLQuotes)
            If oQuotes Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Next Quote Number

            lNextQuoteNum = CStr(oQuotes.GetAttribute(ACXMLAttribNextQuoteNum))
            If ToSafeDouble(lNextQuoteNum) < 1 Then
                lNextQuoteNum = CStr(1)
            End If

            ' Create the Quote Objects Node
            oQuoteObjects = m_oDataset.CreateElement(ACXMLQuoteObjects)

            oQuoteObjects.SetAttribute(ACXMLAttribSchemeID, v_lGISSchemeID)

            ' Build the Quote Key from the GISSchemeID
            r_sQuoteKey = ACQuotePrefix & CStr(v_lGISSchemeID)

            ' Set the Quote Objects Key
            oQuoteObjects.SetAttribute(ACXMLAttribOIKey, r_sQuoteKey)

            ' Set the Next Quote Number Attribute
            lNextQuoteNum = CStr(CDbl(lNextQuoteNum) + 1)
            oQuotes.SetAttribute(ACXMLAttribNextQuoteNum, lNextQuoteNum)

            ' Append the Quote Objects to the Quotes Node

            oQuotes.AppendChild(oQuoteObjects)

            ' Release Local References
            oQuotes = Nothing
            oQuoteObjects = Nothing

            ' Get the Data Model Code
            sDataModelCode = GISDataModelCode
            If sDataModelCode.Trim() = "" Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Data Model Code is blank. Need it to get SaveToDBMode from registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="NewQuoteOutputSaveDB")
            End If

            ' Get the Top Level Quote Object
            lReturn = CType(GetTopLevelQuoteObject(r_sObjectName:=sTopQuoteObject, r_sTableName:=sTopQuoteTable), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Yes, so we need to create an instance of the Quote Top Level Object
            lReturn = CType(NewObjectInstance(v_sObjectName:=sTopQuoteObject, r_sOIKey:=r_sTopQuoteOIKey, v_sQuoteKey:=r_sQuoteKey), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            'IJR 2003-04-17 Start
            'Renewal save will not work without the Quote_Binder_ID

            ' Set the Quote Binder ID
            lNextOINumber = NextOINumber

            lReturn = CType(SetPropertyValue(v_sObjectName:=sTopQuoteObject, v_sPropertyName:="QUOTE_BINDER_ID", v_sOIKey:=r_sTopQuoteOIKey, v_vPropertyValue:=CStr(lNextOINumber), v_bIsAssumedInfo:=False, v_bLoadedFromDB:=True), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            NextOINumber = lNextOINumber + 1
            'IJR 2003-04-17 End

            ' Set the Policy Link ID
            lReturn = CType(SetPropertyValue(v_sObjectName:=sTopQuoteObject, v_sPropertyName:="GIS_POLICY_LINK_ID", v_sOIKey:=r_sTopQuoteOIKey, v_vPropertyValue:=CStr(PolicyLinkID()), v_bIsAssumedInfo:=False, v_bLoadedFromDB:=True), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Set the Scheme ID
            lReturn = CType(SetPropertyValue(v_sObjectName:=sTopQuoteObject, v_sPropertyName:="GIS_SCHEME_ID", v_sOIKey:=r_sTopQuoteOIKey, v_vPropertyValue:=CStr(v_lGISSchemeID), v_bIsAssumedInfo:=False, v_bLoadedFromDB:=True), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewQuoteOutputSaveDBFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewQuoteOutputSaveDB", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ClearAllQuoteOutput
    '
    ' Description: Deletes and Recreates the Quote Output node.
    '
    ' ***************************************************************** '
    Public Function ClearAllQuoteOutput() As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim oQuotesElem As XmlElement
        Dim lSaveToDBMode As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Delete Any Previous of the Quotes Area
            ' Note: Do not need to check return code,
            ' as we do not care if it was not already there.
            'BUT check before we delete as attempting to delete when not there gives an error


            If Not (m_oDataset.GetElementById(ACXMLQuotes.Trim()) Is Nothing) Then
                lReturn = DelObjectInstance(v_sObjectName:=ACXMLQuotes, v_sOIKey:=ACXMLQuotes)
            End If

            ' Create the Quotes Element
            oQuotesElem = m_oDataset.CreateElement(ACXMLQuotes)

            ' Set the Next Quote Number Attribute
            oQuotesElem.SetAttribute(ACXMLAttribNextQuoteNum, 1)
            ' Set the OI Key
            oQuotesElem.SetAttribute(ACXMLAttribOIKey, ACXMLQuotes)

            ' Get the Save to DB Mode
            lSaveToDBMode = iGISSharedConstants.GetLoadSaveDBMode(GISDataModelCode)
            ' If we are saving Quotes in the Database
            If lSaveToDBMode = iGISSharedConstants.GISRegLoadSaveDBModeFastWithQuotes Then
                ' RFC170501 - Use the Clear Quotes Attribute to mark that we need to execute the
                '             clear quote output sp at SavetoDBTime
                oQuotesElem.SetAttribute(ACXMLAttribClearQuotes, gPMConstants.PMEReturnCode.PMTrue)
            End If

            ' Append Quotes as a Child of the Data Set element

            m_oDataset.DocumentElement.AppendChild(oQuotesElem)

            oQuotesElem = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ClearAllQuoteOutputFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearAllQuoteOutput", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: NewObjectInstance
    '
    ' Description: Create a New Instance of the specified object.
    '              Return the Instance Key for the new object.
    '              The Parent Instance key MUST be supplied if this
    '              is NOT a top level object.
    '
    '              The SchemeKey MUST be supplied if this is an Output
    '              Object.
    ' ***************************************************************** '
    Public Function NewObjectInstance(ByRef v_sObjectName As String, ByRef r_sOIKey As String, Optional ByRef v_sParentOIKey As String = "", Optional ByRef v_sQuoteKey As String = "", Optional ByRef v_bLoadedFromDB As Boolean = False, Optional ByRef v_lOINumber As Integer = -1) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer = 0

        Dim oParentInst, oNewInst, xeTempInst As XmlElement
        Dim sTempKey As String = ""
        'Dim vNextChildNum As Variant
        'Dim lNewChildNum As Long
        Dim sMsg As String = ""
        Dim sTableName As String = ""
        Dim sPKColName As String = ""
        Dim sColumnName As String = ""
        Dim sPropertyName As String = ""
        Dim vPropertyArray As Object = Nothing
        Dim vPropertyValue As Object = Nothing
        Dim iIsPrimaryKey As Integer = 0
        Dim lRow As Integer = 0
        Dim bIsAssumedInfo As Boolean = 0
        Dim lOINumber As Integer
        Dim sParentObject As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Increment the Next Instance Number
            NextOINumber += 1

            lOINumber = NextOINumber

            ' If we are Loading this Object From the Database
            If v_bLoadedFromDB Then

                ' Then the Object Instance Number Should have been supplied
                If v_lOINumber < 1 Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Object Instance Number MUST be supplied to load from Database.", vApp:=ACApp, vClass:=ACClass, vMethod:="NewObjectInstance")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Use the Object Instance Number Supplied
                ' Altered the Logic(Use Supplied key only if the same doesn't exist already)
                sTempKey = BuildOIKey(v_sObjectName, v_lOINumber)
                xeTempInst = m_oDataset.GetElementById(sTempKey.Trim())
                If xeTempInst Is Nothing Then
                    lOINumber = v_lOINumber

                    ' If the Supplied Object Instance Number is greater than or equal to
                    ' the Next Object Instance Num then set the Next Instance Num
                    ' to the Supplied Instance Num plus one.
                    If v_lOINumber >= NextOINumber Then
                        NextOINumber = v_lOINumber + 1
                    End If
                End If

            End If

            If v_sParentOIKey.Trim() <> "" Then

                ' Get the Parent Instance
                oParentInst = GetObjectInstance(v_sParentOIKey)
                ' If we did NOT find the Parent Instance then Error
                If oParentInst Is Nothing Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            ElseIf (v_sQuoteKey.Trim() = "") Then

                ' Set the Parent Instance to the Risk Objects Node
                oParentInst = GetObjectInstance(ACXMLRiskObjects)
                If oParentInst Is Nothing Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else

                ' Set the Parent Instance to the Quote Node
                oParentInst = GetObjectInstance(v_sQuoteKey)
                If oParentInst Is Nothing Then
                    ' Scheme Key is Invalid
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Scheme Key " & v_sQuoteKey & " is NOT valid.", vApp:=ACApp, vClass:=ACClass, vMethod:="NewObjectInstance")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            ' Create a New Element
            oNewInst = m_oDataset.CreateElement(v_sObjectName.Trim().ToUpper())

            ' Generate the Key for the NEW Object Instance

            ' If we are Creating a New Quote Object
            If v_sQuoteKey.Trim().Length > 0 Then 'PN18822

                ' Generate the Key for the QuoteObject (Prefixed with Quote Key)
                r_sOIKey = BuildOIKey(v_sObjectName, lOINumber, v_sQuoteKey)

            Else

                ' RFC071103 - Avoid Clashing OI Values where the gis policy link id value is small (e.g. in a clean database)
                ' RFC100303 - If the Object is the Top Level (Policy Binder) then
                ' we need to add the PolicyBinder OI Offset.
                ' This is to stop the OI value clashing with other objects
                If v_sParentOIKey = "" Then
                    ' Calc the Risk Object Instance Key
                    r_sOIKey = BuildOIKey(v_sObjectName, lOINumber + iGISSharedConstants.GISPolicyBinderOIOffset)
                Else
                    ' Calc the Risk Object Instance Key
                    r_sOIKey = BuildOIKey(v_sObjectName, lOINumber)

                End If

            End If

            ' Set the Object Instance Attributes
            oNewInst.SetAttribute(ACXMLAttribOIKey, r_sOIKey)

            ' Are we loading this from the Database
            If v_bLoadedFromDB Then
                ' Yes, so it doesn't need to be added/updated
                oNewInst.SetAttribute(ACXMLAttribUpdateStatus, gPMConstants.PMEComponentAction.PMView)
            Else
                ' No, it is new so it needs to be added.
                oNewInst.SetAttribute(ACXMLAttribUpdateStatus, gPMConstants.PMEComponentAction.PMAdd)
            End If

            ' Add the New Instance as a Child of the Parent

            oParentInst.AppendChild(oNewInst)

            oParentInst = Nothing
            oNewInst = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewObjectInstanceFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewObjectInstance", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Public Function DelObjectInstance(ByRef v_sObjectName As String, ByRef v_sOIKey As String) As Integer

        Dim result As Integer = 0
        Dim oObjInst As XmlElement
        Dim oParentInst, oDelObjs As XmlNode
        Dim lUpdateStatus As gPMConstants.PMEComponentAction
        Dim vUpdateStatus As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Object Instance
            oObjInst = GetObjectInstance(v_sOIKey)
            ' If we cannot find it, Return Not Found
            If oObjInst Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Get the Parent Object Instance
            oParentInst = oObjInst.ParentNode

            ' Remove the Object Instance from the Parent

            oParentInst.RemoveChild(oObjInst)

            'RFC200400 - Add Proper Delete Functionality


            vUpdateStatus = oObjInst.GetAttribute(ACXMLAttribUpdateStatus)

            ' Some Object Instances (Like Quotes) do not have an Update Status
            ' need to trap that.

            Dim dbNumericTemp As Double
            If Double.TryParse(CStr(vUpdateStatus), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                lUpdateStatus = CType(CInt(vUpdateStatus), gPMConstants.PMEComponentAction)
            Else
                lUpdateStatus = gPMConstants.PMEComponentAction.PMAdd
            End If

            ' If this Object Instance Needs to be Added to the Database
            If lUpdateStatus = gPMConstants.PMEComponentAction.PMAdd Then

                ' Then we do not need to delete it from the database

            Else

                ' Add the Object to the Deleted Objects Element

                ' Get the Deleted Objects Node
                oDelObjs = GetObjectInstance(ACXMLDeletedObjects)

                ' Add the Deleted Object as a Parent of the Deleted Objects Element

                oDelObjs.AppendChild(oObjInst)

                ' Set the update status to Delete
                oObjInst.SetAttribute(ACXMLAttribUpdateStatus, gPMConstants.PMEComponentAction.PMDelete)

            End If

            oObjInst = Nothing
            oParentInst = Nothing
            oDelObjs = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DelObjectInstanceFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="DelObjectInstance", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateObjectInstanceStatus
    '
    ' Description: Update status of an Instance of the Specified Object.
    ' ***************************************************************** '
    Public Function UpdateObjectInstanceStatus(ByRef v_sOIKey As String) As Integer

        Dim result As Integer = 0
        Dim oObjInst As XmlElement

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Object Instance
            oObjInst = GetObjectInstance(v_sOIKey)
            ' If we cannot find it, Return Not Found
            If oObjInst Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Set the update status to Delete
            oObjInst.SetAttribute(ACXMLAttribUpdateStatus, gPMConstants.PMEComponentAction.PMDelete)

            oObjInst = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateObjectInstanceStatusFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateObjectInstanceStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' RFC270300 - Created
    ' ***************************************************************** '
    ' Name: ResetPropertyValues
    '
    ' Description: Sets all non Primary Key Property Values for this
    '              Object to empty.
    ' ***************************************************************** '
    Public Function ResetPropertyValues(ByRef v_sObjectName As String, ByRef r_sOIKey As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Dim sTableName As String = ""
        Dim sColumnName As String = ""
        Dim sPropertyName As String = ""
        Dim vPropertyArray(,) As Object = Nothing
        Dim iIsPrimaryKey As gPMConstants.PMEReturnCode
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get a List of Properties for this Object
            lReturn = CType(GetObjectDefDetails(v_sObjectName:=v_sObjectName, r_sTableName:=sTableName, r_vPropertyArray:=vPropertyArray), gPMConstants.PMEReturnCode)
            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (Not Informations.IsArray(vPropertyArray)) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' For Each Property

            For lRow As Integer = vPropertyArray.GetLowerBound(1) To vPropertyArray.GetUpperBound(1)

                ' Get the Property Name

                sPropertyName = CStr(vPropertyArray(0, lRow))

                ' Get the Property Details
                lReturn = CType(GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, r_sColumnName:=sColumnName, r_iIsPrimaryKey:=iIsPrimaryKey), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Is this a Primary Key field
                If iIsPrimaryKey = gPMConstants.PMEReturnCode.PMTrue Then

                    ' Yes, so do not reset the value

                Else

                    ' No not a PK, so reset the value
                    lReturn = CType(SetPropertyValue(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, v_sOIKey:=r_sOIKey, v_vPropertyValue:="", v_bIsAssumedInfo:=False), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            Next lRow

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ResetPropertyValuesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ResetPropertyValues", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetPropertyValue
    '
    ' Description: Set a Property Value
    '
    ' Returns    : PMInvalidRequest If the Object definition does NOT
    '                               have the Property requested.
    '              PMNotFound       If an Object Instance cannot be found
    '                               for the specified OIKey.
    '              PMTrue           All OK
    ' ***************************************************************** '
    Public Function SetPropertyValue(ByRef v_sObjectName As String, ByRef v_sPropertyName As String, ByRef v_sOIKey As String, ByRef v_vPropertyValue As String, ByRef v_bIsAssumedInfo As Boolean, Optional ByRef v_bLoadedFromDB As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim oObjectInst As XmlElement
        Dim sPropertyTag As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Object Instance it Belongs to
            oObjectInst = GetObjectInstance(v_sOIKey)
            If oObjectInst Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            PropertyValueSet(r_oObjectInst:=oObjectInst, v_sObjectName:=v_sObjectName, v_sPropertyName:=v_sPropertyName, v_vPropertyValue:=v_vPropertyValue, v_bLoadedFromDB:=v_bLoadedFromDB)

            oObjectInst = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetPropertyValueFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPropertyValue", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPropertyValue
    '
    ' Description: Get a Property Value
    '
    ' Returns    : PMInvalidRequest If the Object definition does NOT
    '                               have the Property requested.
    '              PMNotFound       If an Object Instance cannot be found
    '                               for the specified OIKey.
    '              PMTrue           If there is a Property Value it is
    '                               returned, otherwise Empty is returned.
    ' ***************************************************************** '

    Public Function GetPropertyValue(ByRef v_sObjectName As String, ByRef v_sPropertyName As String, ByRef v_sOIKey As String, ByRef r_vPropertyValue As Object, ByRef r_bIsAssumedInfo As Boolean) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oObjectInst As XmlElement
        Dim sPropertyTag As String = ""

        Dim vPropertyValue As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assumed Informations is not supported in this version of the Dataset control
            r_bIsAssumedInfo = False

            ' Get the Object Instance
            oObjectInst = GetObjectInstance(v_sOIKey)

            If oObjectInst Is Nothing Then
                ' Object Does NOT Exist, Return Not Found
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Get the Property Value
            ' RAW 17/06/2004 : added : prevent type mismatch if byref param is linked to a typed variable
            vPropertyValue = PropertyValue(oObjectInst, v_sPropertyName)
            If v_sPropertyName.ToUpper.Trim = "UID" AndAlso vPropertyValue.ToString = "" Then
                vPropertyValue = System.Guid.NewGuid.ToString()
            End If
            Dim lDataType As Integer
            If m_bStrict Then
                lReturn = CType(GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=v_sPropertyName, r_lDataType:=lDataType), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    r_vPropertyValue = Nothing
                    Throw New System.Exception(vbObjectError.ToString() + ", " + ACApp & "." & ACClass & "." & "GetPropertyValue" + ", " + "Cant find definition for property " & v_sObjectName & "." & v_sPropertyName)
                End If


                If vPropertyValue <> "" Or String.IsNullOrEmpty(vPropertyValue) Or Informations.IsNothing(vPropertyValue) Then
                    r_vPropertyValue = vPropertyValue
                Else
                    ' check type of r_vPropertyValue
                    Select Case CInt(Type.GetTypeCode(r_vPropertyValue.GetType()))
                        Case TypeCode.Boolean, TypeCode.Int16, TypeCode.Int32, TypeCode.Decimal, TypeCode.Double, TypeCode.Decimal, TypeCode.Single, TypeCode.Byte, TypeCode.DateTime
                            r_vPropertyValue = 0

                        Case Else
                            r_vPropertyValue = vPropertyValue
                    End Select
                End If
            Else
                If v_sObjectName.EndsWith("_OUTPUT") Then
                    lReturn = CType(GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=v_sPropertyName, r_lDataType:=lDataType), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_vPropertyValue = Nothing
                        Throw New System.Exception(vbObjectError.ToString() + ", " + ACApp & "." & ACClass & "." & "GetPropertyValue" + ", " + "Cant find definition for property " & v_sObjectName & "." & v_sPropertyName)
                    End If
                    If lDataType = 21 Then
                        If Not String.IsNullOrEmpty(vPropertyValue) Then
                            r_vPropertyValue = CDbl(vPropertyValue).ToString
                        Else
                            r_vPropertyValue = vPropertyValue
                        End If
                    Else
                        r_vPropertyValue = vPropertyValue
                    End If
                Else
                    r_vPropertyValue = vPropertyValue
                End If
            End If

            oObjectInst = Nothing
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPropertyValueFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPropertyValue", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ResetUpdateStatus
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function ResetUpdateStatus(ByRef v_sObjectName As String, ByRef v_sOIKey As String) As Integer

        Dim result As Integer = 0
        Dim oObjectInst, oParentInst As XmlElement
        Dim lUpdateStatus As gPMConstants.PMEComponentAction

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get a Reference to the Instance
            oObjectInst = GetObjectInstance(v_sOIKey:=v_sOIKey)
            If oObjectInst Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Get the objects Update Status

            lUpdateStatus = oObjectInst.GetAttribute(ACXMLAttribUpdateStatus)

            ' If this is a Deleted Instance
            If lUpdateStatus = gPMConstants.PMEComponentAction.PMDelete Then

                ' Then Delete it from its Parent i.e the Deleted Objects Node
                oParentInst = oObjectInst.ParentNode


                oParentInst.RemoveChild(oObjectInst)

            ElseIf lUpdateStatus = gPMConstants.PMEComponentAction.PMEdit Then

                ' Set the Objects Update Status to Edit
                oObjectInst.SetAttribute(ACXMLAttribUpdateStatus, gPMConstants.PMEComponentAction.PMEdit)
            Else
                ' Set the Objects Update Status to View
                oObjectInst.SetAttribute(ACXMLAttribUpdateStatus, gPMConstants.PMEComponentAction.PMView)
            End If

            oObjectInst = Nothing
            oParentInst = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError


            '    Set oPropertyInst = Nothing

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ResetUpdateStatusFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ResetUpdateStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: ResetUpdateStatusWholeDataset
    '
    ' Description: Sets the Update Status to View for ALL objects
    '              in the dataset.
    '
    '              This is not the best way of doing this as it
    '              would best be done with XSL, but it will do for know.
    '
    ' History: 28/03/2001 RFC - Created.
    '
    ' ***************************************************************** '
    Public Function ResetUpdateStatusWholeDataset() As Integer

        Dim result As Integer = 0
        Dim oObjectInst As XmlElement
        Dim sXMLDS As String = ""
        Dim sXMLDSD As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get a Reference to the Quotes Object
            oObjectInst = GetObjectInstance(ACXMLQuotes)
            ' If we cannot find it, Return Not Found
            If oObjectInst Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Set the ClearAllQuoteOutput Attribute to False
            oObjectInst.SetAttribute(ACXMLAttribClearQuotes, gPMConstants.PMEReturnCode.PMFalse)
            oObjectInst = Nothing

            ' Get a Reference to the Deleted Objects
            oObjectInst = GetObjectInstance(v_sOIKey:=ACXMLDeletedObjects)
            If oObjectInst Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Remove all of the Deleted Objects
            oObjectInst.InnerText = ""
            oObjectInst.RemoveChild(oObjectInst.FirstChild)

            ' Return the Data Set
            lReturn = CType(ReturnAsXML(sXMLDSD, sXMLDS), gPMConstants.PMEReturnCode)
            If oObjectInst Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Reset the Insert Flags to View
            sXMLDS = Replace(sXMLDS, " US=""1""", " US=""0""", , , CompareMethod.Text)
            'sXMLDS = sXMLDS.Replace(" US=""1""", " US=""0""",,, StringComparison.CurrentCulture)
            ' Reset the Update Flage to View
            sXMLDS = Replace(sXMLDS, " US=""2""", " US=""0""", , , CompareMethod.Text)

            ' Load the Data Set Back Up Again
            lReturn = CType(LoadFromXML(sXMLDSD, sXMLDS), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ResetUpdateStatusWholeDataset Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ResetUpdateStatusWholeDataset", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    '              See constants GISHierCol.... in iGISSharedConstants
    ' ***************************************************************** '
    Public Function GetInstanceHierarchy(ByRef v_sObjectName As String, ByRef r_vObjectInstanceArray(,) As Object, ByRef r_lMaxInstances As Integer) As Integer

        Dim result As Integer = 0
        Dim oObjectDef As XmlElement
        Dim oInstances As XmlNodeList
        Dim oInstance, oParentInst As XmlElement

        Dim lNum, lRow As Integer
        Dim vIdValueArray As Object = Nothing
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oRoot As XmlElement
        Dim lIsQuoteObject As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            r_vObjectInstanceArray = Nothing

            ' Get the Object Definition
            oObjectDef = GetDefinitionNode(v_sObjectName)
            If oObjectDef Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMInvalidRequest
            End If

            ' Is this a Quote Object or a Risk Object

            lIsQuoteObject = oObjectDef.GetAttribute(ACXMLAttribIsQuoteObject)

            If lIsQuoteObject = gPMConstants.PMEReturnCode.PMTrue Then
                oRoot = GetObjectInstance(ACXMLQuotes)
            Else
                oRoot = GetObjectInstance(ACXMLRiskObjects)
            End If

            ' Return the Max Number of Instances that we can have of this Object

            r_lMaxInstances = CInt(oObjectDef.GetAttribute(ACXMLAttribMaxInstances))

            ' Loop Until we get back to the Risk/Quote Objects Root Level
            Do While ((oObjectDef.Name <> ACXMLRiskObjects) And (oObjectDef.Name <> ACXMLQuoteObjects))

                ' Get all Instances of this Object Name
                oInstances = oRoot.GetElementsByTagName(oObjectDef.Name)

                ' Get the Number of Instances
                lNum = oInstances.Count

                ' If there are Any Instances
                If lNum > 0 Then

                    ' Resize the Array
                    If Informations.IsArray(r_vObjectInstanceArray) Then

                        lRow = r_vObjectInstanceArray.GetUpperBound(1) + 1

                        ReDim Preserve r_vObjectInstanceArray(9, r_vObjectInstanceArray.GetUpperBound(1) + lNum)
                    Else
                        lRow = 0
                        ReDim r_vObjectInstanceArray(9, lNum - 1)
                    End If

                    ' For Each Instance
                    For lInst As Integer = 0 To lNum - 1

                        'lRow = lRow + lInst

                        oInstance = oInstances.Item(lInst)


                        r_vObjectInstanceArray(iGISSharedConstants.GISHierColObjectName, lRow) = oObjectDef.Name


                        r_vObjectInstanceArray(iGISSharedConstants.GISHierColOIKey, lRow) = oInstance.GetAttribute(ACXMLAttribOIKey)
                        ' RFC29111999 - Do not need to hold the child number anymore
                        'r_vObjectInstanceArray(GISHierColChildNum, lRow) = oInstance.getAttribute(ACXMLAttribChildNum)

                        r_vObjectInstanceArray(iGISSharedConstants.GISHierColChildNum, lRow) = lInst + 1

                        ' Get the Identifying Property Values for this Object Instance
                        lReturn = CType(GetPropertyValues(v_oObjectDef:=oObjectDef, v_oObjectInst:=oInstance, r_vIdValueArray:=vIdValueArray), gPMConstants.PMEReturnCode)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                            r_vObjectInstanceArray(iGISSharedConstants.GISHierColIDName1, lRow) = Nothing

                            r_vObjectInstanceArray(iGISSharedConstants.GISHierColIDValue1, lRow) = Nothing

                            r_vObjectInstanceArray(iGISSharedConstants.GISHierColIDName2, lRow) = Nothing

                            r_vObjectInstanceArray(iGISSharedConstants.GISHierColIDValue2, lRow) = Nothing

                            r_vObjectInstanceArray(iGISSharedConstants.GISHierColIDName3, lRow) = Nothing

                            r_vObjectInstanceArray(iGISSharedConstants.GISHierColIDValue3, lRow) = Nothing
                        Else


                            r_vObjectInstanceArray(iGISSharedConstants.GISHierColIDName1, lRow) = vIdValueArray(iGISSharedConstants.GISIDColPropName, 0)


                            r_vObjectInstanceArray(iGISSharedConstants.GISHierColIDValue1, lRow) = vIdValueArray(iGISSharedConstants.GISIDColPropValue, 0)


                            r_vObjectInstanceArray(iGISSharedConstants.GISHierColIDName2, lRow) = vIdValueArray(iGISSharedConstants.GISIDColPropName, 1)


                            r_vObjectInstanceArray(iGISSharedConstants.GISHierColIDValue2, lRow) = vIdValueArray(iGISSharedConstants.GISIDColPropValue, 1)


                            r_vObjectInstanceArray(iGISSharedConstants.GISHierColIDName3, lRow) = vIdValueArray(iGISSharedConstants.GISIDColPropName, 2)


                            r_vObjectInstanceArray(iGISSharedConstants.GISHierColIDValue3, lRow) = vIdValueArray(iGISSharedConstants.GISIDColPropValue, 2)
                        End If

                        oParentInst = oInstance.ParentNode
                        If oParentInst Is Nothing Then

                            r_vObjectInstanceArray(iGISSharedConstants.GISHierColParentOIKey, lRow) = ""
                        Else
                            If (oParentInst.Name = ACXMLRiskObjects) Or (oParentInst.Name = ACXMLQuoteObjects) Then

                                r_vObjectInstanceArray(iGISSharedConstants.GISHierColParentOIKey, lRow) = ""
                            Else


                                r_vObjectInstanceArray(iGISSharedConstants.GISHierColParentOIKey, lRow) = oParentInst.GetAttribute(ACXMLAttribOIKey)
                            End If
                        End If

                        lRow += 1

                    Next lInst

                End If

                oInstances = Nothing
                oInstance = Nothing
                oParentInst = Nothing

                ' Set the ObjectDef to be the Parent of the Current
                oObjectDef = oObjectDef.ParentNode

                ' Belt and Braces Check. We should never hit this
                ' as we should always stop at the Risk/Quote Objects
                If oObjectDef Is Nothing Then
                    Exit Do
                End If

            Loop

            oObjectDef = Nothing
            oRoot = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInstanceHierarchyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInstanceHierarchy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetObjectIdentity
    '
    ' Description: For the given Object Instance return an Array of the
    '              Identifying Property Names and current values.
    '
    '              See constants GISIDCol.... in iGISSharedConstants
    '
    ' ***************************************************************** '
    Public Function GetObjectIdentity(ByRef v_sObjectName As String, ByRef v_sOIKey As String, ByRef r_vPropertyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim oObjectInst, oObjectDef As XmlElement

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Object Definition
            oObjectDef = GetDefinitionNode(v_sObjectName)
            If oObjectDef Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMInvalidRequest
            End If

            ' Get the Object Instance
            oObjectInst = GetObjectInstance(v_sOIKey)
            If oObjectInst Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            result = GetPropertyValues(v_oObjectDef:=oObjectDef, v_oObjectInst:=oObjectInst, r_vIdValueArray:=r_vPropertyArray)

            oObjectDef = Nothing
            oObjectInst = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetObjectIdentityFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetObjectIdentity", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

    Public Function GetObjectDefDetails(ByRef v_sObjectName As String, Optional ByRef r_lIsQuoteObject As Integer = 0, Optional ByRef r_lGISObjectID As Integer = 0, Optional ByRef r_sTableName As String = "", Optional ByRef r_lMaxInstances As Integer = 0, Optional ByRef r_lPolarisObjectID As Integer = 0, Optional ByRef r_sParentObjectName As String = "", Optional ByRef r_vChildObjectArray As Object = -1, Optional ByRef r_vPropertyArray As Object = -1, Optional ByRef r_sSelectSQL As String = "", Optional ByRef r_sInsertSQL As String = "", Optional ByRef r_sUpdateSQL As String = "", Optional ByRef r_sDeleteSQL As String = "", Optional ByRef r_lIsSelectableForScreen As Integer = 0, Optional ByRef r_lIsNonGIS As Integer = 0, Optional ByRef r_lEditFlags As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim oObjectDef, oParentDef, oChildDef As XmlElement
        Dim lNumOfChild As Integer
        Dim vChildObjectArray(), vPropertyArray(,) As Object
        Dim lPropertyNum, lObjectNum As Integer

        Dim vSQL As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get a Reference to the Definition Object
        oObjectDef = GetDefinitionNode(v_sObjectName)

        If oObjectDef Is Nothing Then
            Return gPMConstants.PMEReturnCode.PMInvalidRequest
        End If

        ' Return the Attrbites

        r_lIsQuoteObject = CInt(oObjectDef.GetAttribute(ACXMLAttribIsQuoteObject))

        r_lGISObjectID = CInt(oObjectDef.GetAttribute(ACXMLAttribObjectID))

        r_sTableName = CStr(oObjectDef.GetAttribute(ACXMLAttribTableName)).Trim()

        r_lMaxInstances = CInt(oObjectDef.GetAttribute(ACXMLAttribMaxInstances))

        r_lPolarisObjectID = CInt(oObjectDef.GetAttribute(ACXMLAttribPolarisObjectID))

        ' RFC300103
        'These next three lines error on GII as these attributes don't exist in the XML
        'zero values in each is an acceptable default

        Try

            r_lIsSelectableForScreen = CInt(oObjectDef.GetAttribute(ACXMLAttribIsSelForScreen))

            r_lIsNonGIS = CInt(oObjectDef.GetAttribute(ACXMLAttribIsNonGIS))

            r_lEditFlags = CInt(oObjectDef.GetAttribute(ACXMLAttribEditFlags))

            ' RFC 290200 - Store SQL Statements in Data Set Def.

            vSQL = CStr(oObjectDef.GetAttribute(ACXMLAttribSQLSelect))

            If Convert.IsDBNull(vSQL) Or Informations.IsNothing(vSQL) Then
                r_sSelectSQL = ""
            Else
                r_sSelectSQL = vSQL
            End If

            vSQL = CStr(oObjectDef.GetAttribute(ACXMLAttribSQLInsert))

            If Convert.IsDBNull(vSQL) Or Informations.IsNothing(vSQL) Then
                r_sInsertSQL = ""
            Else
                r_sInsertSQL = vSQL
            End If

            vSQL = CStr(oObjectDef.GetAttribute(ACXMLAttribSQLUpdate))

            If Convert.IsDBNull(vSQL) Or Informations.IsNothing(vSQL) Then
                r_sUpdateSQL = ""
            Else
                r_sUpdateSQL = vSQL
            End If

            vSQL = CStr(oObjectDef.GetAttribute(ACXMLAttribSQLDelete))

            If Convert.IsDBNull(vSQL) Or Informations.IsNothing(vSQL) Then
                r_sDeleteSQL = ""
            Else
                r_sDeleteSQL = vSQL
            End If

            oParentDef = oObjectDef.ParentNode
            If oParentDef Is Nothing Then
                r_sParentObjectName = ""
            Else
                If (oParentDef.Name = ACXMLRiskObjects) Or (oParentDef.Name = ACXMLQuoteObjects) Then
                    r_sParentObjectName = ""
                Else

                    r_sParentObjectName = CStr(oParentDef.GetAttribute(ACXMLAttribObjectName))
                End If
            End If

            ' If we have NOT been supplied the Child Object Array
            ' or the Property Array then EXIT


            If Not (Informations.IsArray(r_vChildObjectArray)) And Not (Informations.IsArray(r_vPropertyArray)) Then
                If (r_vChildObjectArray = -1) And (r_vPropertyArray = -1) Then
                    oObjectDef = Nothing
                    oParentDef = Nothing
                    Return result
                End If
            End If



            vChildObjectArray = Nothing


            vPropertyArray = Nothing

            ' Does this Object Definition have any Children
            lNumOfChild = oObjectDef.ChildNodes.Count
            If lNumOfChild > 0 Then

                ' Yes

                ' For each Child Definition
                For lChildNum As Integer = 0 To lNumOfChild - 1

                    ' Get a Reference to it
                    oChildDef = oObjectDef.ChildNodes.Item(lChildNum)

                    ' Does the Child Def have Children of Its Own
                    If oChildDef.HasChildNodes Then

                        ' Yes, so it is a Child Object Definition

                        ' Resize the Child Object Array
                        If Not Informations.IsArray(vChildObjectArray) Then
                            ReDim vChildObjectArray(0)
                        Else

                            ReDim Preserve vChildObjectArray(vChildObjectArray.GetUpperBound(0) + 1)
                        End If

                        ' Add the Child Object Name to the end of the Array

                        lObjectNum = vChildObjectArray.GetUpperBound(0)


                        vChildObjectArray(lObjectNum) = oChildDef.GetAttribute(ACXMLAttribObjectName)

                    Else

                        ' No, so it is a Property Definition

                        ' Resize the Property Array
                        If Not Informations.IsArray(vPropertyArray) Then
                            ReDim vPropertyArray(1, 0)
                        Else

                            ReDim Preserve vPropertyArray(1, vPropertyArray.GetUpperBound(1) + 1)
                        End If

                        ' Add the Property Name to the end of the Array

                        lPropertyNum = vPropertyArray.GetUpperBound(1)


                        'vPropertyArray(0, lPropertyNum) = oChildDef.GetAttribute(ACXMLAttribPropertyName)
                        'vPropertyArray(1, lPropertyNum) = oChildDef.GetAttribute(ACXMLAttribIsIdentProp)

                        vPropertyArray(0, lPropertyNum) = oChildDef.GetAttribute(ACXMLAttribPropertyName)
                        vPropertyArray(1, lPropertyNum) = oChildDef.GetAttribute(ACXMLAttribIsIdentProp)

                    End If

                Next lChildNum

            End If

            ' Return the Results and Tidy Up



            If Informations.IsArray(r_vChildObjectArray) OrElse r_vChildObjectArray Is Nothing Then
                ' If Not (r_vChildObjectArray = -1) Then


                r_vChildObjectArray = vChildObjectArray
                'End If
            End If


            If Informations.IsArray(r_vPropertyArray) OrElse r_vPropertyArray Is Nothing Then
                'If Not (r_vPropertyArray = -1) Then
                'If Not (r_vPropertyArray = -1) Then


                r_vPropertyArray = vPropertyArray
            End If


            vChildObjectArray = Nothing

            vPropertyArray = Nothing

            oObjectDef = Nothing
            oParentDef = Nothing
            oChildDef = Nothing

            Return result

Err_GetObjectDefDetails:

            result = gPMConstants.PMEReturnCode.PMError

            oObjectDef = Nothing
            oParentDef = Nothing
            oChildDef = Nothing

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetObjectDefDetailsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetObjectDefDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result

        Catch exc As System.Exception
            'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: GetPropertyDefDetails
    '
    ' Description: Returns the Property Attributes for the Supplied
    '              Property Name.
    '
    ' ***************************************************************** '
    Public Function GetPropertyDefDetails(ByRef v_sObjectName As String, ByRef v_sPropertyName As String, Optional ByRef r_lGISObjectID As Integer = 0, Optional ByRef r_lGISPropertyID As Integer = 0, Optional ByRef r_sColumnName As String = "", Optional ByRef r_lDataType As Integer = 0, Optional ByRef r_iIsPrimaryKey As Integer = 0, Optional ByRef r_iIsIdentifyingProperty As Integer = 0, Optional ByRef r_lGISListID As Integer = 0, Optional ByRef r_lPolarisPropertyID As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim oPropertyDef As XmlElement

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            ' Get a Reference to the Property Definition
            oPropertyDef = GetDefinitionNode(v_sObjectName, v_sPropertyName)
            If oPropertyDef Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMInvalidRequest
            End If

            ' Return the Attributes

            r_lGISObjectID = CInt(oPropertyDef.GetAttribute(ACXMLAttribObjectID))

            r_lGISPropertyID = CInt(oPropertyDef.GetAttribute(ACXMLAttribPropertyID))

            r_sColumnName = CStr(oPropertyDef.GetAttribute(ACXMLAttribColumnName))

            r_lDataType = CInt(oPropertyDef.GetAttribute(ACXMLAttribDataType))

            r_iIsPrimaryKey = CInt(oPropertyDef.GetAttribute(ACXMLAttribIsPrimaryKey))

            r_iIsIdentifyingProperty = CInt(oPropertyDef.GetAttribute(ACXMLAttribIsIdentProp))

            r_lGISListID = CInt(oPropertyDef.GetAttribute(ACXMLAttribGISListID))

            r_lPolarisPropertyID = CInt(oPropertyDef.GetAttribute(ACXMLAttribPolarisPropertyID))

            oPropertyDef = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPropertyDefDetailsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPropertyDefDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetObjectInstDetails
    '
    ' Description: Returns the details for a given Object Instance,
    '              including an Array of Property Name/Values.
    '
    ' ***************************************************************** '
    Public Function GetObjectInstDetails(ByRef v_sObjectName As String, ByRef v_sOIKey As String, Optional ByRef r_lUpdateStatus As Integer = 0, Optional ByRef r_sParentOIKey As String = "", Optional ByRef r_lChildNumber As Integer = 0, Optional ByRef r_vPropertyValueArray(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oObjectDef, oObjectInst, oParentInst As XmlElement

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get a Reference to the Object Definition
            oObjectDef = GetDefinitionNode(v_sObjectName)
            If oObjectDef Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMInvalidRequest
            End If

            ' Get a Reference to the Object Instance
            oObjectInst = GetObjectInstance(v_sOIKey)
            If oObjectInst Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Get the Object Instance UpdateStatus Attribute

            r_lUpdateStatus = CInt(oObjectInst.GetAttribute(ACXMLAttribUpdateStatus))

            ' RFC29111999 - Do not need to hold the child number anymore
            '    ' Get the Child Number
            '    r_lChildNumber = oObjectInst.getAttribute(ACXMLAttribChildNum)

            ' Get the Parent OI Key
            oParentInst = oObjectInst.ParentNode

            r_sParentOIKey = CStr(oParentInst.GetAttribute(ACXMLAttribOIKey))

            ' If we need to build the Property Value Array

            If Not Informations.IsNothing(r_vPropertyValueArray) Then

                ' Build It
                lReturn = CType(GetPropertyValues(v_oObjectDef:=oObjectDef, v_oObjectInst:=oObjectInst, r_vAllValueArray:=r_vPropertyValueArray), gPMConstants.PMEReturnCode)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            ' Release Local Refs
            oObjectDef = Nothing
            oObjectInst = Nothing
            oParentInst = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetObjectInstDetailsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetObjectInstDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: ObjectInstExists
    '
    ' Description: Checks for the existance of a particular Object Instance.
    '
    ' ***************************************************************** '
    Public Function ObjectInstExists(ByRef v_sObjectName As String, ByRef v_sOIKey As String) As Integer

        Dim result As Integer = 0
        Dim oObjectInst As XmlElement

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get a Reference to the Object Instance
            oObjectInst = GetObjectInstance(v_sOIKey)
            ' If it was not found return PMFalse
            If oObjectInst Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If it is the wrong Object Type then return Not Found
            If v_sObjectName.Trim().ToUpper() <> oObjectInst.Name.Trim().ToUpper() Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Release Local Refs
            oObjectInst = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ObjectInstExistsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ObjectInstExists", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetObjectInstChild
    '
    ' Description: Returns an Array of OIKeys for all Child Instances
    '              of the supplied Child Object Type for a given
    '              Object Instance.
    ' ***************************************************************** '
    Public Function GetObjectInstChild(ByRef v_sObjectName As String, ByRef v_sOIKey As String, ByRef v_sChildObjectName As String, ByRef r_vChildOIKeyArray As Object) As Integer

        Dim result As Integer = 0
        Dim oObjectInst As XmlElement
        Dim oChildInsts As XmlNodeList
        Dim oChildInst As XmlElement
        Dim lNumOfChild As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            r_vChildOIKeyArray = Nothing

            ' Get a Reference to the Object Instance
            oObjectInst = GetObjectInstance(v_sOIKey)
            If oObjectInst Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Get the List of Child Instances by their Object Name
            oChildInsts = oObjectInst.GetElementsByTagName(v_sChildObjectName.Trim().ToUpper())

            ' How many are there
            lNumOfChild = oChildInsts.Count - 1

            ' If we have some Child Instances
            If lNumOfChild >= 0 Then

                ' Resize the Array
                ReDim r_vChildOIKeyArray(lNumOfChild)

                ' For Each Child
                For lChildNum As Integer = 0 To lNumOfChild

                    ' Get a Reference to it
                    oChildInst = oChildInsts.Item(lChildNum)

                    ' Add the Child Key to the Array


                    r_vChildOIKeyArray(lChildNum) = oChildInst.GetAttribute(ACXMLAttribOIKey)

                Next lChildNum

            End If

            ' Release Local References
            oObjectInst = Nothing
            oChildInsts = Nothing
            oChildInst = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetObjectInstChildFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetObjectInstChild", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetAllOIKey
    '
    ' Description: Returns an Array of Object Instance Keys for ALL
    '              objects of given Object Name.
    '
    ' ***************************************************************** '

    Public Function GetAllOIKey(ByRef v_sObjectName As String, ByRef r_vOIKeyArray As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Return CommonGetAllOIKey(v_sObjectName:=v_sObjectName, r_vOIKeyArray:=r_vOIKeyArray)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllOIKeyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllOIKey", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDeletedOIKey
    '
    ' Description: Returns an Array of Object Instance Keys for ALL
    '              objects of given Object Name.
    '
    ' ***************************************************************** '
    Public Function GetDeletedOIKey(ByRef v_sObjectName As String, ByRef r_vOIKeyArray As Object, Optional ByRef v_bDeletedObjects As Boolean = False) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Return CommonGetAllOIKey(v_sObjectName:=v_sObjectName, r_vOIKeyArray:=r_vOIKeyArray, v_bDeletedObjects:=v_bDeletedObjects)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDeletedOIKeyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDeletedOIKey", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetDelObjectsOIKey
    '
    ' Description: Returns an Array of Object Names and OIKeys
    '              for the Objects which need to be deleted from the
    '              database.
    ' ***************************************************************** '
    Public Function GetDelObjectsArray(ByRef r_vDelObjArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim oDelObjs, oDelInst As XmlElement
        Dim lNumofInst As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            r_vDelObjArray = Nothing

            ' Get the Deleted Objects Node
            oDelObjs = GetObjectInstance(ACXMLDeletedObjects)
            If oDelObjs IsNot Nothing Then
                ' How many are there
                lNumofInst = oDelObjs.ChildNodes.Count - 1

                ' If we have any Instances
                If lNumofInst >= 0 Then

                    ' Resize the Array
                    ReDim r_vDelObjArray(1, lNumofInst)

                    ' For Each Instance
                    For lInstNum As Integer = 0 To lNumofInst

                        ' Get a Reference to it
                        oDelInst = oDelObjs.ChildNodes.Item(lInstNum)

                        ' Add the Deleted Object to the Array

                        r_vDelObjArray(0, lInstNum) = oDelInst.Name.Trim()


                        r_vDelObjArray(1, lInstNum) = oDelInst.GetAttribute(ACXMLAttribOIKey)

                    Next lInstNum

                End If

            End If

            oDelInst = Nothing
            oDelObjs = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDelObjectsOIKeyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDelObjectsOIKey", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Public Function GetChildOIKey(ByRef v_sParentObjectName As String, ByRef v_sParentOIKey As String, ByRef v_sChildObjectName As String, ByRef r_vChildOIKeyArray As Object) As Integer

        Dim result As Integer = 0
        Dim oAllInsts As XmlNodeList
        Dim oParentInst, oObjectInst As XmlElement
        Dim lNumofInst As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            'r_vChildOIKeyArray = ""
            r_vChildOIKeyArray = Nothing

            ' Get a Reference to the Parent Instance
            oParentInst = GetObjectInstance(v_sParentOIKey)
            If oParentInst Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the List of All Instances by their Object Name
            oAllInsts = oParentInst.GetElementsByTagName(v_sChildObjectName.Trim().ToUpper())

            ' How many are there
            lNumofInst = oAllInsts.Count - 1

            ' If we have any Instances
            If lNumofInst >= 0 Then

                ' Resize the Array
                ReDim r_vChildOIKeyArray(lNumofInst)

                ' For Each Instance
                For lInstNum As Integer = 0 To lNumofInst

                    ' Get a Reference to it
                    oObjectInst = oAllInsts.Item(lInstNum)

                    ' Add the Child Key to the Array



                    r_vChildOIKeyArray(lInstNum) = oObjectInst.GetAttribute(ACXMLAttribOIKey)

                Next lInstNum

            End If

            ' Release Local References
            oObjectInst = Nothing
            oAllInsts = Nothing
            oParentInst = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetChildOIKeyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetChildOIKey", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetAllQuoteKey
    '
    ' Description: Returns an Array of Quote Keys.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetAllQuoteKey) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetAllQuoteKey(ByRef v_sObjectName As String, ByRef r_vOIKeyArray As Object) As Integer
    '
    'Dim result As Integer = 0
    'Dim oAllInsts As XmlNodeList
    'Dim oObjectInst As XmlElement
    'Dim lNumofInst As Integer
    'Dim oRoot As XmlElement
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '

    'r_vOIKeyArray = ""
    '
    ' Set the Root to be the Quotes element
    'oRoot = GetObjectInstance(ACXMLQuotes)
    '
    ' Get the Child Elements
    'oAllInsts = oRoot.ChildNodes
    '
    'oRoot = Nothing
    '
    ' How many are there
    'lNumofInst = oAllInsts.Count - 1
    '
    ' If we have any Instances
    'If lNumofInst >= 0 Then
    '
    ' Resize the Array
    ''ReDim r_vOIKeyArray(lNumofInst)
    '
    ' For Each Instance
    'For 'lInstNum As Integer = 0 To lNumofInst
    '
    ' Get a Reference to it
    'oObjectInst = oAllInsts.Item(lInstNum)
    '
    ' Add the Child Key to the Array



    '
    'Next lInstNum
    '
    'End If
    '
    ' Release Local References
    'oObjectInst = Nothing
    'oAllInsts = Nothing
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    '
    ' Log Error Message
    'bpmfunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllQuoteKeyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllQuoteKey", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function


    ' ***************************************************************** '
    ' Name: BuildOIKey
    '
    ' Description: Builds and Return the ObjectInstance Key from the
    '              supplied Paramaters.
    ' ***************************************************************** '
    Public Function BuildOIKey(ByRef v_sObjectName As String, ByRef v_lOINumber As Integer, Optional ByRef v_sQuoteKey As String = "") As String

        Dim result As String = String.Empty
        Try

            result = ""

            result = ACXMLAttribOIKey & CStr(v_lOINumber)

            If v_sQuoteKey <> "" Then
                result = v_sQuoteKey.Trim() & ACOIKeySeparator & result
            End If

            Return result

        Catch excep As System.Exception



            result = ""

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BuildOIKeyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildOIKey", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetTopLevelRiskObject
    '
    ' Description: Returns the Risk Top Level Object and Table Name
    '
    ' ***************************************************************** '
    Public Function GetTopLevelRiskObject(ByRef r_sObjectName As String, ByRef r_sTableName As String) As Integer

        Dim result As Integer = 0
        Dim oRiskObjectsDef, oTopObjectDef As XmlElement

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Risk Objects Node
            oRiskObjectsDef = GetDefinitionNode(v_sObjectName:=ACXMLRiskObjects)

            If oRiskObjectsDef Is Nothing Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Locate Top Level Risk Object. Has the Data Set been Initialised?", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTopLevelRiskObject")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oRiskObjectsDef.ChildNodes.Count = 0 Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="There are NO Risk Objects loaded.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTopLevelRiskObject")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the First Child Node
            oTopObjectDef = oRiskObjectsDef.ChildNodes.Item(0)

            ' Return the Object Name & Table Name

            r_sObjectName = CStr(oTopObjectDef.GetAttribute(ACXMLAttribObjectName))

            r_sTableName = CStr(oTopObjectDef.GetAttribute(ACXMLAttribTableName))

            oTopObjectDef = Nothing
            oRiskObjectsDef = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTopLevelRiskObjectFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTopLevelRiskObject", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' RFC01/05/01
    ' ***************************************************************** '
    ' Name: GetTopLevelQuoteObject
    '
    ' Description: Returns the Quote Top Level Object and Table Name
    '
    ' ***************************************************************** '
    Public Function GetTopLevelQuoteObject(ByRef r_sObjectName As String, ByRef r_sTableName As String) As Integer

        Dim result As Integer = 0
        Dim oQuoteObjectsDef, oTopObjectDef As XmlElement

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Quote Objects Node
            oQuoteObjectsDef = GetDefinitionNode(v_sObjectName:=ACXMLQuoteObjects)

            If oQuoteObjectsDef Is Nothing Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Locate Top Level Quote Object. Has the Data Set been Initialised?", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTopLevelQuoteObject")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oQuoteObjectsDef.ChildNodes.Count = 0 Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="There are NO Quote Objects loaded.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTopLevelQuoteObject")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oQuoteObjectsDef.ChildNodes.Count > 1 Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="There are more than 1 Top Level Quote Objects.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTopLevelQuoteObject")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the First Child Node
            oTopObjectDef = oQuoteObjectsDef.ChildNodes.Item(0)

            ' Return the Object Name & Table Name

            r_sObjectName = CStr(oTopObjectDef.GetAttribute(ACXMLAttribObjectName))

            r_sTableName = CStr(oTopObjectDef.GetAttribute(ACXMLAttribTableName))

            oTopObjectDef = Nothing
            oQuoteObjectsDef = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTopLevelQuoteObjectFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTopLevelQuoteObject", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' RFC300103
    ' ***************************************************************** '
    ' Name: GetSpecialObject
    '
    ' Description: Returns the name and Type of the Special Object defined
    '              in this dataset (If there is one.)
    ' ***************************************************************** '
    Public Function GetSpecialObject(ByRef r_sObjectName As String, ByRef r_lNonGISType As Integer) As Integer

        Dim result As Integer = 0
        Dim oSpecialObjectsDef As XmlElement

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Risk Objects Node
            oSpecialObjectsDef = GetDefinitionNode(v_sObjectName:=ACXMLSpecialObjects)

            If oSpecialObjectsDef Is Nothing Then
                r_sObjectName = ""
                r_lNonGISType = GISDataModelType.GISOTRisk
                '        bpmfunc.LogMessage m_sUsername, _
                ''            iType:=PMLogError, _
                ''            sMsg:="Unable to Locate Special Object definition. Has the Data Set been Initialised?", _
                ''            vApp:=ACApp, _
                ''            vClass:=ACClass, _
                ''            vMethod:="GetSpecialObject"
                Return result
            End If

            ' Return the Object Name & Table Name

            r_sObjectName = CStr(oSpecialObjectsDef.GetAttribute(ACXMLAttribObjectName)).Trim()

            r_lNonGISType = CInt(oSpecialObjectsDef.GetAttribute(ACXMLAttribIsNonGIS))

            oSpecialObjectsDef = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSpecialObjectFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSpecialObject", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: MergeQuoteOutput
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function MergeQuoteOutput(ByRef v_sSourceDataSet As String) As Integer

        Dim result As Integer = 0
        Dim oSourceDataSet As XmlDocument
        Dim bLoaded As Boolean

        Dim oQuotesDestination As XmlElement
        Dim oQuotes As XmlNodeList
        Dim oQuote, oMergeQuote As XmlNode
        Dim lNumQtes As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oSourceDataSet = New XmlDocument()
            ' Use the new parser


            'oSourceDataSet.setProperty("NewParser", True)

            ' Load the Source Data Set

            ' developer guide no solution no. 22
            'oSourceDataSet.validateOnParse = False
            Dim temp_xml_result As Boolean
            Try
                oSourceDataSet.LoadXml(v_sSourceDataSet)
                temp_xml_result = True

            Catch parseError As System.Exception
                temp_xml_result = False
            End Try
            bLoaded = temp_xml_result
            If Not bLoaded Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Load Data Set from XML String", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXML")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'SJ 19/02/2004 - start

            m_lNextOINumber = CInt(oSourceDataSet.DocumentElement.GetAttribute(ACXMLAttribNextOINumber))
            m_oDataset.DocumentElement.SetAttribute(ACXMLAttribNextOINumber, m_lNextOINumber)
            'SJ 19/02/2004 - end

            ' Get the Destination Quotes Nodes
            oQuotesDestination = GetObjectInstance(ACXMLQuotes)
            If oQuotesDestination Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a Node List containing all of the Quote Outputs from the Source
            oQuotes = oSourceDataSet.GetElementsByTagName(ACXMLQuoteObjects)

            ' If there are NO Quotes then exit
            If oQuotes Is Nothing Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="No Quote Outputs Found.", vApp:=ACApp, vClass:=ACClass, vMethod:="MergeQuoteOutput")
                Return result
            End If

            ' Get the Number of Quotes
            lNumQtes = oQuotes.Count

            ' If there are NO Quotes then exit
            If lNumQtes <= 0 Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="No Quote Outputs Found.", vApp:=ACApp, vClass:=ACClass, vMethod:="MergeQuoteOutput")
                Return result
            End If

            ' For Each Quote
            For lSub As Integer = 0 To lNumQtes - 1

                ' Get a Reference to It
                oQuote = oQuotes.Item(lSub)

                ' Clone it
                oMergeQuote = oQuote.CloneNode(True)

                ' Add it to the Destination
                oQuotesDestination.AppendChild(oMergeQuote)

            Next lSub

            ' Release References
            oSourceDataSet = Nothing
            oQuotesDestination = Nothing
            oQuotes = Nothing
            oQuote = Nothing
            oMergeQuote = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MergeQuoteOutputFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="MergeQuoteOutput", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: SaveXMLToFile
    '
    ' Description: Save the Data Set definition and/or the Data Set to
    '              file/s in their XML format.
    '
    ' ***************************************************************** '
    Public Function SaveXMLToFile(Optional ByRef v_sDataSetDefFile As String = "", Optional ByRef v_sDataSetFile As String = "") As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If CBool(CStr(v_sDataSetDefFile <> "").Trim()) Then
                m_oDataSetDef.Save(v_sDataSetDefFile)
            End If

            If CBool(CStr(v_sDataSetFile <> "").Trim()) Then

                ' Update the Next Object Instance number in the XML before it is returned
                m_oDataset.DocumentElement.SetAttribute(ACXMLAttribNextOINumber, m_lNextOINumber)

                m_oDataset.Save(v_sDataSetFile)

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveXMLToFileFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveXMLToFile", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim sTopLevelObject As String = ""
        Dim sTopLevelTable As String = ""
        Dim vOIKeyArray As Object = Nothing
        Dim sOIKey As String = ""
        Dim bAssumedInfo As Boolean
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vPolicyLinkID As Object = Nothing

        Try

            result = -1

            ' Get the Top Level Table Name
            lReturn = CType(GetTopLevelRiskObject(r_sObjectName:=sTopLevelObject, r_sTableName:=sTopLevelTable), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Instance Keys for the Top Level object
            ' Note, there should only be one.

            lReturn = CType(GetAllOIKey(v_sObjectName:=sTopLevelObject, r_vOIKeyArray:=vOIKeyArray), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check to see that we have ONE Key returned
            If Not Informations.IsArray(vOIKeyArray) Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to find any instances of Top Level Object :- " & sTopLevelObject, vApp:=ACApp, vClass:=ACClass, vMethod:="PolicyLinkID")
                Return gPMConstants.PMEReturnCode.PMFalse
            Else

                If vOIKeyArray.GetUpperBound(0) > vOIKeyArray.GetLowerBound(0) Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="There should be only ONE instance of Top Level Object :- " & sTopLevelObject, vApp:=ACApp, vClass:=ACClass, vMethod:="PolicyLinkID")
                    Return gPMConstants.PMEReturnCode.PMFalse
                Else


                    sOIKey = CStr(vOIKeyArray(vOIKeyArray.GetLowerBound(0)))
                End If
            End If

            ' Get the Policy Link ID Property in the Top Level Object

            ' developer guide no. 98
            lReturn = CType(GetPropertyValue(v_sObjectName:=sTopLevelObject, v_sPropertyName:=iGISSharedConstants.GISPolLinkIDName, v_sOIKey:=sOIKey, r_vPropertyValue:=vPolicyLinkID, r_bIsAssumedInfo:=bAssumedInfo), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Dim dbNumericTemp As Double
            If Double.TryParse(CStr(vPolicyLinkID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                result = CInt(vPolicyLinkID)
            Else

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="PolicyLinkID Value is not set or NOT Numeric :-" & CStr(vPolicyLinkID), vApp:=ACApp, vClass:=ACClass, vMethod:="PolicyLinkID")
            End If

            Return result

        Catch excep As System.Exception



            result = -1

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PolicyLinkIDFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="PolicyLinkID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ''' <summary>
    ''' Function to transform the node as per the XSL provided.
    ''' </summary>
    ''' <param name="Node">The node to be transformed.</param>
    ''' <param name="Stylesheet">The XSL against which the node needs to be transformed.</param>
    ''' <returns>String</returns>
    Private Function TransformNode(ByVal Node As XmlNode, ByVal Stylesheet As XmlNode) As String
        Dim oXslTransform As Xsl.XslCompiledTransform = New Xsl.XslCompiledTransform
        Dim writer As New StringWriter()
        Dim sSQL As String = String.Empty
        Try
            oXslTransform.Load(Stylesheet)
            oXslTransform.Transform(Node, Nothing, writer)
            sSQL = writer.ToString
        Catch ex As Exception
            Throw ex
        End Try
        Return sSQL
    End Function

    ' ***************************************************************** '
    '
    ' Name: GenSaveSQLViaXSL
    '
    ' Description:
    '
    ' History: 28/03/2001 RFC - Created.
    '
    ' ***************************************************************** '
    Public Function GenSaveSQLViaXSL(ByRef r_sSQL As String) As Integer

        Dim result As Integer = 0
        Dim oSaveXSL As XmlDocument
        Dim bLoaded As Boolean
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sFilename As String = ""

        Dim oObjInst As XmlElement
        Dim vClearQuotes As gPMConstants.PMEReturnCode
        Dim sClearQuotesExec As String = ""
        Dim lSaveToDBMode As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oSaveXSL = New XmlDocument()
            ' Use the new parser


            'oSaveXSL.setProperty("NewParser", True)



            'oSaveXSL.validateOnParse = False

            ' Get the XSL File Name
            lReturn = CType(iGISSharedConstants.GetSaveXSLFileName(v_sGisDataModelCode:=GISDataModelCode, r_sSaveXSLFileName:=sFilename), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Locate Save XSL File", vApp:=ACApp, vClass:=ACClass, vMethod:="GenSaveSQLViaXSL")
                Return lReturn
            End If

            ' Load it
            Dim temp_xml_result As Boolean
            Try
                oSaveXSL.Load(sFilename)
                temp_xml_result = True

            Catch parseError As System.Exception
                temp_xml_result = False
            End Try
            bLoaded = temp_xml_result
            If Not bLoaded Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Load Data Save XSL File : " & sFilename, vApp:=ACApp, vClass:=ACClass, vMethod:="GenSaveSQLViaXSL")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDataset.InnerXml = m_oDataset.InnerXml.Replace("&quot;", "&quot;DC")
            ' Do the XSL Transform
            r_sSQL = TransformNode(m_oDataset, oSaveXSL)

            m_oDataset.InnerXml = m_oDataset.InnerXml.Replace("&quot;DC", "&quot;")
            If (r_sSQL <> String.Empty) Then
                r_sSQL = r_sSQL.Replace("&quot;DC", "&quot;")
            End If
            ' Release the XSL Document
            oSaveXSL = Nothing

            ' Get the Save to DB Mode
            lSaveToDBMode = iGISSharedConstants.GetLoadSaveDBMode(GISDataModelCode)
            ' If we are saving Quotes in the Database
            If lSaveToDBMode = iGISSharedConstants.GISRegLoadSaveDBModeFastWithQuotes Then

                ' Get the QUOTES Object Instance
                oObjInst = GetObjectInstance(ACXMLQuotes)
                ' If we cannot find it, Return Not Found
                If oObjInst Is Nothing Then
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If

                ' Get the Clear Quotes Attribute

                vClearQuotes = oObjInst.GetAttribute(ACXMLAttribClearQuotes)
                oObjInst = Nothing

                ' If we need to Clear All quotes then EXEC the stored proc.
                If vClearQuotes = gPMConstants.PMEReturnCode.PMTrue Then
                    sClearQuotesExec = "EXEC spg_" & GISDataModelCode.ToLower().Trim() & "_clear_all_quote_output" & Strings.ChrW(13) & Strings.ChrW(10)
                    sClearQuotesExec = sClearQuotesExec & "  @gis_policy_link_id = " & CStr(PolicyLinkID()) & Strings.ChrW(13) & Strings.ChrW(10)
                    ' Make sure this is the first statement in the SQL
                    r_sSQL = sClearQuotesExec & r_sSQL
                End If

            End If

            'SJ 15/07/2004 - start
            r_sSQL = r_sSQL.Replace("'", "''")
            r_sSQL = r_sSQL.Replace(SIRIUS_DELIM_START, "'")
            r_sSQL = r_sSQL.Replace(SIRIUS_DELIM_END, "'")
            'SJ 15/07/2004 - end

            ' Replace escaped doubles quotes with Two double quotes.
            ' We need two for each one so that the SQL will be valid
            r_sSQL = r_sSQL.Replace("â€œ", Strings.ChrW(34).ToString())
            r_sSQL = r_sSQL.Replace("â€", Strings.ChrW(34).ToString())
            r_sSQL = r_sSQL.Replace("&quot;", Strings.ChrW(34).ToString()) 'PN24533

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenSaveSQLViaXSL Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GenSaveSQLViaXSL", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetIDValues
    '
    ' Description:
    '
    ' History: 30/08/2002 RFC - Created.
    '
    ' ***************************************************************** '
    Public Function SetIDValues() As Integer

        Dim result As Integer = 0
        Dim oSetIDXSL As XmlDocument
        Dim bLoaded As Boolean
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sFilename As String = ""
        Dim sClearQuotesExec As String = ""
        Dim sNewXML As String = ""
        Dim sOldXML As String = ""
        Dim lPos1, lPos2 As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'TODO
            oSetIDXSL = New XmlDocument()
            ' Use the new parser

            ' developer guide no solution no. 38
            'oSetIDXSL.setProperty("NewParser", True)


            ' developer guide no solution no. 22
            'oSetIDXSL.validateOnParse = False

            ' Get the XSL File Name
            lReturn = CType(iGISSharedConstants.GetSetIDXSLFileName(v_sGisDataModelCode:=GISDataModelCode, r_sSetIDXSLFileName:=sFilename), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Locate SetID XSL File", vApp:=ACApp, vClass:=ACClass, vMethod:="SetIDValues")
                Return lReturn
            End If

            ' Load it
            Dim temp_xml_result As Boolean
            Try
                oSetIDXSL.Load(sFilename)
                temp_xml_result = True

            Catch parseError As System.Exception
                temp_xml_result = False
            End Try
            bLoaded = temp_xml_result
            If Not bLoaded Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Load SetID XSL File : " & sFilename, vApp:=ACApp, vClass:=ACClass, vMethod:="SetIDValues")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Update the Next Object Instance number in the XML before it is transformed
            m_oDataset.DocumentElement.SetAttribute(ACXMLAttribNextOINumber, m_lNextOINumber)
            ' Do the XSL Transform
            'm_oDataset.InnerXml = Replace(m_oDataset.InnerXml, Chr(128), "EUR1O")
            m_oDataset.InnerXml = Replace(m_oDataset.InnerXml, ChrW(147), "LftDblQuo")
            m_oDataset.InnerXml = Replace(m_oDataset.InnerXml, ChrW(148), "RtDblQuo")

            'GetFormattedString(m_oDataset.InnerXml)
            sNewXML = TransformNode(m_oDataset, oSetIDXSL)

            'm_oDataset.InnerXml = Replace(m_oDataset.InnerXml, "EUR1O", "&#8364;")
            'sNewXML = Replace(sNewXML, "EUR1O", "&#8364;")
            m_oDataset.InnerXml = Replace(m_oDataset.InnerXml, "LftDblQuo", "&#8220;")
            'sNewXML = Replace(sNewXML, "LftDblQuo", "&#8220;")
            m_oDataset.InnerXml = Replace(m_oDataset.InnerXml, "RtDblQuo", "&#8221;")
            'sNewXML = Replace(sNewXML, "RtDblQuo", "&#8221;")

            'sNewXML = sNewXML.Replace(EXTRASYM, "")
            'sNewXML = sNewXML.Replace(Extrasym1, "")
            ' Get the Old XML
            lReturn = CType(ReturnAsXML(r_sXMLDataSetDef:="", r_sXMLDataSet:=sOldXML), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to return old XML", vApp:=ACApp, vClass:=ACClass, vMethod:="SetIDValues")
                Return lReturn
            End If

            lPos1 = (sOldXML.IndexOf("<DATA_SET", StringComparison.CurrentCultureIgnoreCase) + 1)
            lPos2 = (sNewXML.IndexOf("<DATA_SET", StringComparison.CurrentCultureIgnoreCase) + 1)

            sOldXML = sOldXML.Substring(0, lPos1 - 1) & sNewXML.Substring(lPos2 - 1, Math.Min(sNewXML.Length, sNewXML.Length - lPos2 + 1))

            lReturn = CType(UpdateDataSetFromXML(sOldXML), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to Update the XML after the PK ID values have been generated.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetIDValues")
                Return lReturn
            End If

            ' Release the XSL Document
            oSetIDXSL = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetIDValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetIDValues", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: CommonGetAllOIKey
    '
    ' Description: Returns an Array of Object Instance Keys for ALL
    '              objects of given Object Name.
    '
    ' ***************************************************************** '
    Private Function CommonGetAllOIKey(ByRef v_sObjectName As String, ByRef r_vOIKeyArray As Object, Optional ByRef v_bDeletedObjects As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim oAllInsts As XmlNodeList
        Dim oObjectInst As XmlElement
        Dim lNumofInst As Integer
        Dim oRoot As XmlElement
        Dim lReturn As gPMConstants.PMEReturnCode

        Dim lIsQuoteObject As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue



        r_vOIKeyArray = Nothing

        ' Are we lookin for Deleted Objects
        If v_bDeletedObjects Then

            ' Yes, so set the root to be the Deleted Objects Element
            oRoot = GetObjectInstance(ACXMLDeletedObjects)

        Else

            ' Is this a Quote Object or a Risk Object
            lReturn = CType(GetObjectDefDetails(v_sObjectName:=v_sObjectName, r_lIsQuoteObject:=lIsQuoteObject), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If lIsQuoteObject = gPMConstants.PMEReturnCode.PMTrue Then
                oRoot = GetObjectInstance(ACXMLQuotes)
            Else
                oRoot = GetObjectInstance(ACXMLRiskObjects)
            End If

        End If

        ' Get the List of All Instances by their Object Name
        oAllInsts = oRoot.GetElementsByTagName(v_sObjectName.Trim().ToUpper())

        oRoot = Nothing

        ' How many are there
        lNumofInst = oAllInsts.Count - 1

        ' If we have any Instances
        If lNumofInst >= 0 Then

            ' Resize the Array
            ReDim r_vOIKeyArray(lNumofInst)

            ' For Each Instance
            For lInstNum As Integer = 0 To lNumofInst

                ' Get a Reference to it
                oObjectInst = oAllInsts.Item(lInstNum)

                ' Add the Child Key to the Array


                r_vOIKeyArray(lInstNum) = oObjectInst.GetAttribute(ACXMLAttribOIKey)

            Next lInstNum

        End If

        ' Release Local References
        oObjectInst = Nothing
        oAllInsts = Nothing

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: InitDataSetDef
    '
    ' Description: Initialises the DataSetDefinition XML Document by
    '              adding the top level enteties.
    '
    ' ***************************************************************** '
    Private Function InitDataSetDef(ByRef v_sGisDataModelCode As String) As Integer

        Dim result As Integer = 0
        Dim oDataSetDef, oRiskObjects, oQuoteObjects, oSpecialObjects As XmlElement



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Create the Root Level Element
        oDataSetDef = m_oDataSetDef.CreateElement(ACXMLDataSetDef)

        ' Set the Data Model Code Attribute
        oDataSetDef.SetAttribute(iGISSharedConstants.GISXMLAttribDataModelCode, v_sGisDataModelCode)

        ' RDC 27072001 Set the timestamp for the dataset definition
        oDataSetDef.SetAttribute(iGISSharedConstants.GISXMLAttribDataSetDefTimestamp, DateTime.Now.ToString("yyyyMMddHHmmss"))

        ' Set the Root Level Document Element
        If Not (m_oDataSetDef.DocumentElement Is Nothing) Then
            m_oDataSetDef.RemoveChild(m_oDataSetDef.DocumentElement)
        End If
        m_oDataSetDef.AppendChild(oDataSetDef)

        ' Create the Risk Objects Element
        oRiskObjects = m_oDataSetDef.CreateElement(ACXMLRiskObjects)

        ' Append Risk Objects to the Document Element

        m_oDataSetDef.DocumentElement.AppendChild(oRiskObjects)

        ' Create the Ouput Objects Element
        oQuoteObjects = m_oDataSetDef.CreateElement(ACXMLQuoteObjects)

        ' Append Quote Objects to the Document Element

        m_oDataSetDef.DocumentElement.AppendChild(oQuoteObjects)

        ' RFC300103 - Associated Client, Claims etc
        ' Create the Special Objects Element
        oSpecialObjects = m_oDataSetDef.CreateElement(ACXMLSpecialObjects)
        oSpecialObjects.SetAttribute(ACXMLAttribObjectName, "")
        oSpecialObjects.SetAttribute(ACXMLAttribIsNonGIS, 0)

        ' Append Special Objects to the Document Element

        m_oDataSetDef.DocumentElement.AppendChild(oSpecialObjects)

        oDataSetDef = Nothing
        oRiskObjects = Nothing
        oQuoteObjects = Nothing
        oSpecialObjects = Nothing

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: LoadObjectDefs
    '
    ' Description: Load the Data Set Structures using
    '              the Object/Property Arrays supplied.
    '
    ' ***************************************************************** '
    Private Function LoadObjectDefs() As Integer

        Dim result As Integer = 0

        Dim lGISObjectID, lGISModelID As Integer
        Dim sObjectName, sTableName As String
        Dim lMaxInstances, lIsQuoteObject As Integer
        Dim sParentObjectName As String = ""
        Dim lPolarisObjectID, lIsSelectableForScreen, lIsNonGIS, lEditFlags As Integer

        Dim lReturn As gPMConstants.PMEReturnCode



        result = gPMConstants.PMEReturnCode.PMTrue
        If Not (m_vTempObjectArray Is Nothing) Then
            ' For Each Object
            For lRow As Integer = m_vTempObjectArray.GetLowerBound(1) To m_vTempObjectArray.GetUpperBound(1)

                ' Get the Object attributes from the Array
                lGISObjectID = CInt(m_vTempObjectArray(iGISSharedConstants.GISObjColObjectId, lRow))
                lGISModelID = CInt(m_vTempObjectArray(iGISSharedConstants.GISObjColModelId, lRow))
                sObjectName = CStr(m_vTempObjectArray(iGISSharedConstants.GISObjColObjectName, lRow)).Trim()
                sTableName = CStr(m_vTempObjectArray(iGISSharedConstants.GISObjColTableName, lRow)).Trim()
                Dim dbNumericTemp As Double
                If Not Double.TryParse(CStr(m_vTempObjectArray(iGISSharedConstants.GISObjColMaxInstances, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    lMaxInstances = -1
                Else
                    lMaxInstances = CInt(m_vTempObjectArray(iGISSharedConstants.GISObjColMaxInstances, lRow))
                End If
                lIsQuoteObject = CInt(m_vTempObjectArray(iGISSharedConstants.GISObjColIsQuoteObject, lRow))
                sParentObjectName = CStr(m_vTempObjectArray(iGISSharedConstants.GISObjColParentObjectName, lRow)).Trim()

                Dim dbNumericTemp2 As Double
                If Not Double.TryParse(CStr(m_vTempObjectArray(iGISSharedConstants.GISObjColPolarisObjId, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                    lPolarisObjectID = -1
                Else
                    lPolarisObjectID = CInt(m_vTempObjectArray(iGISSharedConstants.GISObjColPolarisObjId, lRow))
                End If

                Dim dbNumericTemp3 As Double
                If Not Double.TryParse(CStr(m_vTempObjectArray(iGISSharedConstants.GISObjColIsSelectScreen, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                    lIsSelectableForScreen = -1
                Else
                    lIsSelectableForScreen = CInt(m_vTempObjectArray(iGISSharedConstants.GISObjColIsSelectScreen, lRow))
                End If

                Dim dbNumericTemp4 As Double
                If Not Double.TryParse(CStr(m_vTempObjectArray(iGISSharedConstants.GISObjColIsNonGIS, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                    lIsNonGIS = -1
                Else
                    lIsNonGIS = CInt(m_vTempObjectArray(iGISSharedConstants.GISObjColIsNonGIS, lRow))
                End If

                Dim dbNumericTemp5 As Double
                If Not Double.TryParse(CStr(m_vTempObjectArray(iGISSharedConstants.GISObjColEditFlags, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
                    lEditFlags = -1
                Else
                    lEditFlags = CInt(m_vTempObjectArray(iGISSharedConstants.GISObjColEditFlags, lRow))
                End If

                ' Add it to the Object Definitions
                lReturn = CType(AddObjectDefinition(v_lIsQuoteObject:=lIsQuoteObject, v_lGISObjectID:=lGISObjectID, v_sObjectName:=sObjectName, v_sTableName:=sTableName, v_lMaxInstances:=lMaxInstances, v_lPolarisObjectID:=lPolarisObjectID, v_sParentObjectName:=sParentObjectName, v_lIsSelectableForScreen:=lIsSelectableForScreen, v_lIsNonGIS:=lIsNonGIS, v_lEditFlags:=lEditFlags), gPMConstants.PMEReturnCode)

                ' Check that the Object Definition was Added.
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next lRow
        End If
        ' Load the Properties
        lReturn = CType(LoadPropertyDefs(), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: LoadPropertyDefs
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function LoadPropertyDefs() As Integer

        Dim result As Integer = 0



        Dim lGISObjectID As Integer
        Dim sObjectName As String = ""
        Dim lGISPropertyID As Integer
        Dim sPropertyName, sColumnName As String
        Dim lDataType As Integer
        Dim iIsIdentifyingProperty, iIsPrimaryKey As Integer
        Dim lGISListID, lPolarisPropertyID As Integer

        Dim lReturn As gPMConstants.PMEReturnCode

        result = gPMConstants.PMEReturnCode.PMTrue
        If Not (m_vTempPropertyArray Is Nothing) Then
            For lRow As Integer = m_vTempPropertyArray.GetLowerBound(1) To m_vTempPropertyArray.GetUpperBound(1)

                ' Get the Property Values
                lGISObjectID = CInt(m_vTempPropertyArray(iGISSharedConstants.GISPropColObjectId, lRow))
                sObjectName = CStr(m_vTempPropertyArray(iGISSharedConstants.GISPropColObjectName, lRow))
                lGISPropertyID = CInt(m_vTempPropertyArray(iGISSharedConstants.GISPropColPropertyId, lRow))
                sPropertyName = CStr(m_vTempPropertyArray(iGISSharedConstants.GISPropColPropertyName, lRow))
                sColumnName = CStr(m_vTempPropertyArray(iGISSharedConstants.GISPropColColumnName, lRow))
                lDataType = CInt(m_vTempPropertyArray(iGISSharedConstants.GISPropColDataType, lRow))
                iIsIdentifyingProperty = CInt(m_vTempPropertyArray(iGISSharedConstants.GISPropColIsIdentifyingProperty, lRow))
                iIsPrimaryKey = CInt(m_vTempPropertyArray(iGISSharedConstants.GISPropColIsPrimaryKey, lRow))

                Dim dbNumericTemp As Double
                If Double.TryParse(CStr(m_vTempPropertyArray(iGISSharedConstants.GISPropColListId, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    lGISListID = CInt(m_vTempPropertyArray(iGISSharedConstants.GISPropColListId, lRow))
                Else
                    lGISListID = -1
                End If

                Dim dbNumericTemp2 As Double
                If Double.TryParse(CStr(m_vTempPropertyArray(iGISSharedConstants.GISPropColPolarisPropId, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                    lPolarisPropertyID = CInt(m_vTempPropertyArray(iGISSharedConstants.GISPropColPolarisPropId, lRow))
                Else
                    lPolarisPropertyID = -1
                End If

                ' Add the Property Def
                lReturn = CType(AddPropertyDefinition(v_lGISPropertyID:=lGISPropertyID, v_lGISObjectID:=lGISObjectID, v_sObjectName:=sObjectName, v_sPropertyName:=sPropertyName, v_sColumnName:=sColumnName, v_lDataType:=lDataType, v_iIsPrimaryKey:=iIsPrimaryKey, v_iIsIdentifyingProperty:=iIsIdentifyingProperty, v_lGISListID:=lGISListID, v_lPolarisPropertyID:=lPolarisPropertyID), gPMConstants.PMEReturnCode)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next lRow
        End If
        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddObjectDefinition
    '
    ' Description: Adds an Object Definition in the Data Model
    '              Definition held in the XML Document.
    '
    ' ***************************************************************** '
    Private Function AddObjectDefinition(ByRef v_lIsQuoteObject As Integer, ByRef v_lGISObjectID As Integer, ByRef v_sObjectName As String, ByRef v_sTableName As String, ByRef v_lMaxInstances As Integer, ByRef v_lPolarisObjectID As Integer, ByRef v_sParentObjectName As String, ByRef v_lIsSelectableForScreen As Integer, ByRef v_lIsNonGIS As Integer, ByRef v_lEditFlags As Integer) As Integer

        Dim result As Integer = 0
        Dim oParent As XmlNode
        Dim oSpecial As XmlElement
        Dim sParentObjectName As String = ""
        Dim oNewObject As XmlElement



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Is this a Top Level Object
        ' i.e. Does it have a Parent
        If CBool(CStr(v_sParentObjectName = "").Trim()) Then
            ' NO Parent, so this is a Top Level Object

            ' Is this an Quote Object
            If v_lIsQuoteObject = gPMConstants.PMEReturnCode.PMTrue Then
                sParentObjectName = ACXMLQuoteObjects
            Else
                sParentObjectName = ACXMLRiskObjects
            End If

        Else

            ' Not Top Level Object, so use the Parent Object Name
            sParentObjectName = v_sParentObjectName.Trim()

        End If

        ' Get the Parent Object
        oParent = GetDefinitionNode(sParentObjectName)
        If oParent Is Nothing Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Create the new Object
        ' Note, Uppercase is used as XML Tags are Case sensitive
        oNewObject = m_oDataSetDef.CreateElement(v_sObjectName.Trim().ToUpper())

        ' Set the Object Attributes
        oNewObject.SetAttribute(ACXMLAttribIsQuoteObject, v_lIsQuoteObject)
        oNewObject.SetAttribute(ACXMLAttribObjectID, v_lGISObjectID)
        oNewObject.SetAttribute(ACXMLAttribObjectName, v_sObjectName.Trim().ToUpper())
        oNewObject.SetAttribute(ACXMLAttribTableName, v_sTableName.Trim())
        oNewObject.SetAttribute(ACXMLAttribMaxInstances, v_lMaxInstances)
        oNewObject.SetAttribute(ACXMLAttribPolarisObjectID, v_lPolarisObjectID)
        oNewObject.SetAttribute(ACXMLAttribNextOINumber, 1)
        ' RFC300103
        oNewObject.SetAttribute(ACXMLAttribIsSelForScreen, v_lIsSelectableForScreen)
        oNewObject.SetAttribute(ACXMLAttribIsNonGIS, v_lIsNonGIS)
        oNewObject.SetAttribute(ACXMLAttribEditFlags, v_lEditFlags)

        ' Add the new Object to the Parent

        oParent.AppendChild(oNewObject)

        ' RFC300103 - Associated Client and Disclosures
        ' If we are adding an Associated Client or Peril object then set a Property
        ' on the DataSet definition so that bGIS will no that it has to load/save to/from
        ' those tables in addition to the normal
        Select Case v_lIsNonGIS
            Case GISDataModelType.GISOTAssociatedClient, GISDataModelType.GISOTClaim, GISDataModelType.GISOTCase

                oSpecial = GetDefinitionNode(ACXMLSpecialObjects)
                If oSpecial Is Nothing Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                oSpecial.SetAttribute(ACXMLAttribObjectName, v_sObjectName.Trim().ToUpper())
                oSpecial.SetAttribute(ACXMLAttribIsNonGIS, v_lIsNonGIS)

            Case Else
        End Select

        ' Release the references
        oParent = Nothing
        oNewObject = Nothing
        oSpecial = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddPropertyDefinition
    '
    ' Description: Adds a Property Definition in the Data Model
    '              Definition held in the XML Document.
    '
    ' ***************************************************************** '
    Private Function AddPropertyDefinition(ByRef v_lGISPropertyID As Integer, ByRef v_lGISObjectID As Integer, ByRef v_sObjectName As String, ByRef v_sPropertyName As String, ByRef v_sColumnName As String, ByRef v_lDataType As Integer, ByRef v_iIsPrimaryKey As Integer, ByRef v_iIsIdentifyingProperty As Integer, ByRef v_lGISListID As Integer, ByRef v_lPolarisPropertyID As Integer) As Integer

        Dim result As Integer = 0
        Dim oParent As XmlNode
        Dim sParentObjectName As String = ""
        Dim oNewProp As XmlElement
        Dim sPropertyTag As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the Parent Object Definition
        oParent = GetDefinitionNode(v_sObjectName)
        If oParent Is Nothing Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Create the new Property with Tag Name 'OBJECTNAME.PROPERTY'
        ' Note Upercase is Used as XML Tag Names are Case Sensitive
        sPropertyTag = v_sObjectName.Trim().ToUpper() & "." & v_sPropertyName.Trim().ToUpper()

        oNewProp = m_oDataSetDef.CreateElement(sPropertyTag)

        oNewProp.SetAttribute(ACXMLAttribObjectID, v_lGISObjectID)
        oNewProp.SetAttribute(ACXMLAttribPropertyID, v_lGISPropertyID)
        oNewProp.SetAttribute(ACXMLAttribPropertyName, v_sPropertyName.Trim().ToUpper())
        oNewProp.SetAttribute(ACXMLAttribColumnName, v_sColumnName)
        oNewProp.SetAttribute(ACXMLAttribDataType, v_lDataType)
        oNewProp.SetAttribute(ACXMLAttribIsPrimaryKey, v_iIsPrimaryKey)
        oNewProp.SetAttribute(ACXMLAttribIsIdentProp, v_iIsIdentifyingProperty)
        oNewProp.SetAttribute(ACXMLAttribGISListID, v_lGISListID)
        oNewProp.SetAttribute(ACXMLAttribPolarisPropertyID, v_lPolarisPropertyID)

        ' Add the new Object to the Parent

        oParent.AppendChild(oNewProp)

        ' Release the references
        oParent = Nothing
        oNewProp = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetDefinitionNode
    '
    ' Description: Returns the Obect/Property Definition Node for the
    '              requested Object or Property.
    '
    ' ***************************************************************** '
    Private Function GetDefinitionNode(ByRef v_sObjectName As String, Optional ByRef v_sPropertyName As String = "") As XmlNode

        Dim result As XmlNode = Nothing
        Dim oNodes As XmlNodeList
        Dim oNode As XmlNode
        Dim sNodeKey As String = ""




        ' Build Up the Key

        ' Upercase the Object Name
        sNodeKey = v_sObjectName.Trim().ToUpper()

        ' If we have been supplied a Property Name then Add
        ' a full stop followed by the Property Name
        If v_sPropertyName.Trim() <> "" Then
            sNodeKey = sNodeKey & "." & v_sPropertyName.Trim().ToUpper()
        End If

        ' Find elements with the Tag Name of the Node Key
        ' This returns a List of Nodes, however there should only be one
        oNodes = m_oDataSetDef.GetElementsByTagName(sNodeKey)

        If oNodes Is Nothing Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to find Definiton for :-" & sNodeKey, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefinitionNode")
            Return result
        End If

        ' Get the Node from the List
        ' Remember there should only be one
        oNode = oNodes.Item(0)
        If oNode Is Nothing Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to find Definition for :-" & sNodeKey, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefinitionNode")
            Return result
        End If

        ' All OK so return the Object/Property Definition Node
        result = oNode

        ' Release Local References
        oNodes = Nothing
        oNode = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetObjectInstance
    '
    ' Description: Returns the Object Instance for the supplied Key.
    '
    ' ***************************************************************** '
    Private Function GetObjectInstance(ByRef v_sOIKey As String) As XmlNode

        Dim result As XmlNode = Nothing
        Dim oObjectInst As XmlElement
        'Static sOIKey As String





        ' Get the Object Instance

        If v_sOIKey = ACXMLRiskObjects Then
            oObjectInst = m_oDataset.GetElementsByTagName(v_sOIKey.Trim()).Item(0)
        Else
            oObjectInst = m_oDataset.GetElementById(v_sOIKey.Trim())
        End If

        If oObjectInst Is Nothing Then
            oObjectInst = m_oDataset.GetElementsByTagName(v_sOIKey.Trim()).Item(0)
        End If
        ' If we did NOT the Instance then Error
        If oObjectInst Is Nothing And v_sOIKey.Trim() <> "DELETED_OBJECTS" Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:="Failed to Find Instance for Key :- " & v_sOIKey, vApp:=ACApp, vClass:=ACClass, vMethod:="GetObjectInstance")
            Return result
        End If

        ' Return the Object Instance
        result = oObjectInst

        ' Release Local Reference
        oObjectInst = Nothing

        Return result

    End Function

    Private Sub AppendElement(elementName As String, mode As String, ByRef dtdWriter As StringBuilder)
        dtdWriter.AppendLine("<!ELEMENT " & elementName & " " & mode & ">")
    End Sub

    Private Sub AppendAttribute(elementName As String, attributeName As String, type As String, valueMode As String, ByRef dtdWriter As StringBuilder)
        dtdWriter.AppendLine("<!ATTLIST " & elementName & " " & attributeName & " " & type & " " & valueMode & ">")
    End Sub

    ' ***************************************************************** '
    ' Name: BuildDataSetDTD
    '
    ' Description: Build the Data Set DTD
    '
    ' ***************************************************************** '
    Private Function BuildDataSetDTD(ByRef r_sDataSetDTD As String, ByVal v_lSaveToDBMode As Integer) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sTopLevelObj As String = ""
        Dim sTopLevelTable As String = ""
        Dim sTopQuoteObj As String = ""
        Dim sTopQuoteTable As String = ""
        Dim dtdWriter As New StringBuilder

        result = gPMConstants.PMEReturnCode.PMTrue

        '<?xml version="1.0"?>
        '<!DOCTYPE DATA_SET [
        '<!ELEMENT DATA_SET (RISK_OBJECTS , DELETED_OBJECTS , QUOTES)>
        '<!ELEMENT RISK_OBJECTS ANY>
        '<!ELEMENT DELETED_OBJECTS ANY>
        '<!ELEMENT QUOTES (QUOTE_OBJECTS)>
        '<!ELEMENT QUOTE_OBJECTS ANY>
        ']>


        ' Start a Document
        dtdWriter.AppendLine("<?xml version=""1.0"" encoding=""UTF-16"" standalone=""no""?>")

        ' Start the DTD
        dtdWriter.AppendLine("<!DOCTYPE " & ACXMLDataSet & "[")

        AppendElement(ACXMLDataSet, "(RISK_OBJECTS , DELETED_OBJECTS , QUOTES)", dtdWriter)

        ' <!ATTLIST DATA_SET DataModelCode CDATA #REQUIRED>
        AppendAttribute(ACXMLDataSet, "DataModelCode", "CDATA", "#REQUIRED", dtdWriter)
        ' <!ATTLIST DATA_SET NextOINumber CDATA #REQUIRED>
        AppendAttribute(ACXMLDataSet, "NextOINumber", "CDATA", "#REQUIRED", dtdWriter)
        AppendAttribute(ACXMLDataSet, "NextOINumber", "CDATA", "#REQUIRED", dtdWriter)
        AppendAttribute(ACXMLDataSet, "TransactionType", "CDATA", "#REQUIRED", dtdWriter)
        AppendAttribute(ACXMLDataSet, "EffectiveDate", "CDATA", "#REQUIRED", dtdWriter)

        AppendAttribute(ACXMLDataSet, "CoverStartDate", "CDATA", "#REQUIRED", dtdWriter)
        AppendAttribute(ACXMLDataSet, "CoverEndDate", "CDATA", "#REQUIRED", dtdWriter)

        AppendAttribute(ACXMLDataSet, "Action", "CDATA", "#REQUIRED", dtdWriter)
        ' Add the Deleted Objects
        AppendElement(ACXMLDeletedObjects, "ANY", dtdWriter)
        ' Add an ID type Attribute for the Deleted Objects Node
        ' <!ATTLIST Deleted_Objects OIKey ID #REQUIRED>
        AppendAttribute(ACXMLDeletedObjects, ACXMLAttribOIKey, "ID", "#REQUIRED", dtdWriter)


        AppendElement(ACXMLQuotes, "(" & ACXMLQuoteObjects & "*)", dtdWriter)
        ' Add an ID type Attribute for the Quotes Node
        ' <!ATTLIST Quotes OI ID #REQUIRED>
        AppendAttribute(ACXMLQuotes, ACXMLAttribOIKey, "ID", "#REQUIRED", dtdWriter)

        '*********************************************************************************
        'Reduce the size of the DTD. Large DTD is causing performance Issues
        '*********************************************************************************
    If m_sDataModelType <> "" Then
        If (m_sDataModelType).ToUpper() = "CLAIM" Then
            ' <!ATTLIST Quotes CQ CDATA #Implied>
            AppendAttribute(ACXMLQuotes, ACXMLAttribClearQuotes, "CDATA", "#IMPLIED", dtdWriter)

            ' <!ATTLIST Quotes NextQuoteNumber ID #Implied>
            AppendAttribute(ACXMLQuotes, ACXMLAttribNextQuoteNum, "CDATA", "#IMPLIED", dtdWriter)
        End If
    End If
        ' Get the Top Level Risk Object
        lReturn = CType(GetTopLevelRiskObject(sTopLevelObj, sTopLevelTable), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If v_lSaveToDBMode = iGISSharedConstants.GISRegLoadSaveDBModeFastWithQuotes Then
            ' Get the Top Level Quote Object
            lReturn = CType(GetTopLevelQuoteObject(sTopQuoteObj, sTopQuoteTable), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        ' Add the Top Level Object to the DTD
        lReturn = CType(AddObjectToDTD(v_sObjectName:=sTopLevelObj, dtdWriter:=dtdWriter), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If


        If v_lSaveToDBMode = iGISSharedConstants.GISRegLoadSaveDBModeFastWithQuotes Then
            ' Add the Top Level Quote Object to the DTD
            lReturn = CType(AddObjectToDTD(v_sObjectName:=sTopQuoteObj, dtdWriter:=dtdWriter), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If
        End If

        ' End the DTD
        dtdWriter.AppendLine("]>")
        ' Create the Dataset Element

        dtdWriter.AppendLine("<" & ACXMLDataSet & "/>")

        ' Return the DTD as a string

        r_sDataSetDTD = dtdWriter.ToString()

        Return result

    End Function




    'Private Function BuildDataSetDTD1(ByRef r_sDataSetDTD As String, ByVal v_lSaveToDBMode As Integer) As Integer

    '    Dim schema As New XmlSchema()
    '    Dim elementDataSet As XmlSchemaElement
    '    Dim xsXMLSchemaCT1, xsXMLSchemaCT2 As XmlSchemaComplexType
    '    Dim xsSchemaSeq1, xsSchemaSeq2 As XmlSchemaSequence
    '    Dim xsSchemaElmt1, xsSchemaElmt2, xsSchemaElmt3, xsSchemaElmtTop As XmlSchemaElement
    '    Dim xsSchemaAttb1, xsSchemaAttb2, xsSchemaAttb3 As XmlSchemaAttribute
    '    Dim ct2 As XmlSchemaComplexType
    '    Dim result As Integer = 0
    '    Dim lSub, lNoOfChild As Integer
    '    Dim lReturn As gPMConstants.PMEReturnCode
    '    Dim sTopLevelObj, sTopLevelTable As String
    '    Dim vTopLevelChildren As Object
    '    Dim sTopLevelChildren, sTopQuoteObj, sTopQuoteTable As String
    '    schema.ElementFormDefault = XmlSchemaForm.Qualified

    '    ' <xs:element name="DATA_SET">
    '    elementDataSet = New XmlSchemaElement()
    '    schema.Items.Add(elementDataSet)
    '    elementDataSet.Name = "DATA_SET"

    '    ' <xs:complexType>
    '    xsXMLSchemaCT1 = New XmlSchemaComplexType()
    '    elementDataSet.SchemaType = xsXMLSchemaCT1

    '    ' <xs:Sequence>
    '    xsSchemaSeq1 = New XmlSchemaSequence()
    '    xsXMLSchemaCT1.Particle = xsSchemaSeq1

    '    xsSchemaElmt1 = New XmlSchemaElement()
    '    xsSchemaElmt1.Name = "RISK_OBJECTS"
    '    xsSchemaSeq1.Items.Add(xsSchemaElmt1)

    '    xsSchemaElmt2 = New XmlSchemaElement()
    '    xsSchemaElmt2.Name = "DELETED_OBJECTS"
    '    xsSchemaSeq1.Items.Add(xsSchemaElmt2)

    '    xsSchemaElmt3 = New XmlSchemaElement()
    '    xsSchemaElmt3.Name = "QUOTES"
    '    xsSchemaSeq1.Items.Add(xsSchemaElmt3)

    '    ' <xs:attribute name="DataModelCode" type="xs:string"/>
    '    xsSchemaAttb1 = New XmlSchemaAttribute()
    '    xsXMLSchemaCT1.Attributes.Add(xsSchemaAttb1)
    '    xsSchemaAttb1.Name = "DataModelCode"
    '    xsSchemaAttb1.Use = XmlSchemaUse.Required
    '    xsSchemaAttb1.SchemaTypeName = New XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema")

    '    ' <xs:attribute name="NextOINumber" type="xs:string"/>
    '    xsSchemaAttb2 = New XmlSchemaAttribute()
    '    xsSchemaAttb2.Name = "NextOINumber"
    '    xsSchemaAttb2.Use = XmlSchemaUse.Required
    '    xsXMLSchemaCT1.Attributes.Add(xsSchemaAttb2)
    '    xsSchemaAttb2.SchemaTypeName = New XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema")


    '    '-----------------------------------------------------------------DELETE_OBJECTS-----------------------
    '    ' <xs:complexType>
    '    xsXMLSchemaCT2 = New XmlSchemaComplexType()
    '    xsSchemaElmt2.SchemaType = xsXMLSchemaCT2

    '    ' <xs:Sequence>
    '    xsSchemaSeq2 = New XmlSchemaSequence()
    '    xsXMLSchemaCT2.Particle = xsSchemaSeq2

    '    Dim xsSchemaAny As New XmlSchemaAny()
    '    xsSchemaAny.ProcessContents = XmlSchemaContentProcessing.Lax
    '    xsSchemaAny.MinOccurs = 0
    '    xsSchemaAny.MaxOccursString = "Unbounded"
    '    xsSchemaSeq2.Items.Add(xsSchemaAny)

    '    ' <xs:attribute name="OI" type="xs:string"/>
    '    xsSchemaAttb3 = New XmlSchemaAttribute()
    '    xsSchemaAttb3.Name = "OI"
    '    xsSchemaAttb3.Use = XmlSchemaUse.Required
    '    xsSchemaAttb3.FixedValue = "DELETED_OBJECTS"
    '    xsSchemaAttb3.SchemaTypeName = New XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema")
    '    xsXMLSchemaCT2.Attributes.Add(xsSchemaAttb3)


    '    '----------------------------------------------------------------------------DELETE_OBJECTS-----------------------



    '    '--------------------------------------------------RISK_OBJECTS----------------------------------------------------------

    '    xsXMLSchemaCT1 = New XmlSchemaComplexType()
    '    xsSchemaElmt1.SchemaType = xsXMLSchemaCT1

    '    ' <xs:Sequence>
    '    xsSchemaSeq1 = New XmlSchemaSequence()
    '    xsXMLSchemaCT1.Particle = xsSchemaSeq1

    '    ' <xs:Attribute>
    '    xsSchemaAttb1 = New XmlSchemaAttribute()
    '    xsSchemaAttb1.Name = ACXMLAttribOIKey
    '    xsSchemaAttb1.Use = XmlSchemaUse.Required
    '    xsSchemaAttb1.FixedValue = ACXMLRiskObjects
    '    xsXMLSchemaCT1.Attributes.Add(xsSchemaAttb1)


    '    lReturn = CType(GetTopLevelRiskObject(sTopLevelObj, sTopLevelTable), gPMConstants.PMEReturnCode)
    '    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '        Return gPMConstants.PMEReturnCode.PMFalse
    '    End If

    '    If v_lSaveToDBMode = iGISSharedConstants.GISRegLoadSaveDBModeFastWithQuotes Then
    '        ' Get the Top Level Quote Object
    '        lReturn = CType(GetTopLevelQuoteObject(sTopQuoteObj, sTopQuoteTable), gPMConstants.PMEReturnCode)
    '        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '            Return gPMConstants.PMEReturnCode.PMFalse
    '        End If
    '    End If


    '    xsSchemaElmtTop = New XmlSchemaElement()
    '    xsSchemaElmtTop.Name = sTopLevelObj
    '    xsSchemaSeq1.Items.Add(xsSchemaElmtTop)

    '    lReturn = CType(AddObjectToDTD1(v_sObjectName:=sTopLevelObj, xsSchemaElmt:=xsSchemaElmtTop), gPMConstants.PMEReturnCode)
    '    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '        Return lReturn
    '    End If


    '    Dim fl As StreamWriter = New StreamWriter("")
    '    fl.Write(schema)
    '    fl.Flush()





    '    '' <xs:element name="cat" type="xs:short"/>
    '    'Dim elementCat As New XmlSchemaElement()
    '    'seq.Items.Add(elementCat)
    '    'elementCat.Name = "S4IDEFAULT"
    '    'elementCat.MinOccurs = 0
    '    'elementCat.MaxOccurs = 100

    '    '' <xs:complexType>
    '    'Dim scomplexType As New XmlSchemaComplexType()
    '    'elementCat.SchemaType = scomplexType

    '    '' <xs:Sequence>
    '    'Dim seq1 As New XmlSchemaSequence()
    '    'scomplexType.Particle = seq1

    '    '' <xs:attribute name="US" type="xs:short"/>
    '    'Dim attb1 As New XmlSchemaAttribute()
    '    'scomplexType.Attributes.Add(attb1)
    '    'attb1.Name = "US"
    '    'attb1.Use = XmlSchemaUse.Required
    '    'attb1.FixedValue = "1"
    '    'attb1.SchemaTypeName = New XmlQualifiedName("short", "http://www.w3.org/2001/XMLSchema")

    '    Dim nsmgr As New XmlNamespaceManager(New NameTable())
    '    nsmgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema")
    '    schema.Write(Console.Out, nsmgr)

    'End Function


    ' ***************************************************************** '
    ' Name: AddObjectToDTD
    '
    ' Description: Adds a GIS Object to the DTD.
    '
    ' ***************************************************************** '
    'Private Function AddObjectToDTD(ByVal v_sObjectName As String, ByRef wrt As MSXML2.MXXMLWriter40) As Integer


    Private Function AddObjectToDTD(ByVal v_sObjectName As String, ByRef dtdWriter As StringBuilder) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vChildren As Object = Nothing
        Dim sChildren As String = ""
        Dim vProperties As Object = Nothing
        Dim sTableName As String = ""


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the Children of the top level object
        lReturn = CType(GetObjectDefDetails(v_sObjectName:=v_sObjectName, r_sTableName:=sTableName, r_vChildObjectArray:=vChildren, r_vPropertyArray:=vProperties), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '    If (IsArray(vChildren)) Then
        '        For lSub = LBound(vChildren) To UBound(vChildren)
        '            If (sChildren <> "") Then
        '                sChildren = sChildren & " | "
        '            End If
        '            sChildren = sChildren & vChildren(lSub) & "*"
        '        Next lSub
        '        sChildren = "(" & sChildren & ")"
        '    Else
        '        sChildren = "(#PCDATA)"
        '    End If
        sChildren = "ANY"

        ' Add the GIS Object as an Element e.g.
        ' <!ELEMENT Driver (Surname,Forename)>
        AppendElement(v_sObjectName, sChildren, dtdWriter)


        ' Add the Attributes for this Object

        ' <!ATTLIST ObjectName OIKey ID #REQUIRED>
        AppendAttribute(v_sObjectName, ACXMLAttribOIKey, "ID", "#REQUIRED", dtdWriter)
    If (m_sDataModelType <> "") Then
        If (m_sDataModelType).ToUpper() = "CLAIM" Then
            ' <!ATTLIST ObjectName UpdateStatus CDATA #REQUIRED>
            AppendAttribute(v_sObjectName, ACXMLAttribUpdateStatus, "CDATA", "#REQUIRED", dtdWriter)

            ' Add the Objects Properties

            lReturn = CType(AddObjectPropertiesToDTD(v_sObjectName:=v_sObjectName, v_sTableName:=sTableName, dtdWriter:=dtdWriter, r_vPropertyArray:=vProperties), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
    End If
        ' Now we need to call ourselves for each of the Child Objects
        sChildren = ""
        If Informations.IsArray(vChildren) Then

            For lSub As Integer = vChildren.GetLowerBound(0) To vChildren.GetUpperBound(0)

                sChildren = CStr(vChildren(lSub))
                lReturn = CType(AddObjectToDTD(v_sObjectName:=sChildren, dtdWriter:=dtdWriter), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Next lSub

        End If


        Return result

    End Function



    Private Function AddObjectToDTD1(ByVal v_sObjectName As String, ByRef xsSchemaElmt As XmlSchemaElement) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vChildren As Object = Nothing
        Dim sChildren As String = ""
        Dim vProperties As Object = Nothing
        Dim sTableName As String = ""

        'We need these variables for typecasting the writer.
        Dim xsXMLSchemaCT1 As XmlSchemaComplexType
        Dim xsSchemaSeq1 As XmlSchemaSequence
        Dim xsSchemaAttb1 As XmlSchemaAttribute




        result = gPMConstants.PMEReturnCode.PMTrue

        'Set handler variables to the writer so it implements the interfaces.

        ' Get the Children of the top level object
        lReturn = CType(GetObjectDefDetails(v_sObjectName:=v_sObjectName, r_sTableName:=sTableName, r_vChildObjectArray:=vChildren, r_vPropertyArray:=vProperties), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '    If (IsArray(vChildren)) Then
        '        For lSub = LBound(vChildren) To UBound(vChildren)
        '            If (sChildren <> "") Then
        '                sChildren = sChildren & " | "
        '            End If
        '            sChildren = sChildren & vChildren(lSub) & "*"
        '        Next lSub
        '        sChildren = "(" & sChildren & ")"
        '    Else
        '        sChildren = "(#PCDATA)"
        '    End If
        sChildren = "ANY"

        ' Add the GIS Object as an Element e.g.

        ' <xs:ComplexType>
        xsXMLSchemaCT1 = New XmlSchemaComplexType()
        xsSchemaElmt.SchemaType = xsXMLSchemaCT1

        ' <xs:Sequence>
        xsSchemaSeq1 = New XmlSchemaSequence()
        xsXMLSchemaCT1.Particle = xsSchemaSeq1

        ' <xs:Attribute US  IO  >

        xsSchemaAttb1 = New XmlSchemaAttribute()
        xsSchemaAttb1.Name = ACXMLAttribUpdateStatus
        xsSchemaAttb1.Use = XmlSchemaUse.Required
        xsSchemaAttb1.SchemaTypeName = New XmlQualifiedName("short", "http://www.w3.org/2001/XMLSchema")
        xsXMLSchemaCT1.Attributes.Add(xsSchemaAttb1)

        xsSchemaAttb1 = New XmlSchemaAttribute()
        xsSchemaAttb1.Name = ACXMLAttribOIKey
        xsSchemaAttb1.Use = XmlSchemaUse.Required
        xsSchemaAttb1.SchemaTypeName = New XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema")
        xsXMLSchemaCT1.Attributes.Add(xsSchemaAttb1)



        ' Add the Objects Properties

        lReturn = CType(AddObjectPropertiesToDTD1(v_sObjectName:=v_sObjectName, v_sTableName:=sTableName, xsXMLSchemaCT:=xsXMLSchemaCT1, r_vPropertyArray:=vProperties), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Now we need to call ourselves for each of the Child Objects
        sChildren = ""
        If Informations.IsArray(vChildren) Then


            For lSub As Integer = vChildren.GetLowerBound(0) To vChildren.GetUpperBound(0)

                sChildren = CStr(vChildren(lSub))
                ' lReturn = CType(AddObjectToDTD1(v_sObjectName:=sChildren, wrt:=wrt), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Next lSub

        End If



        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddObjectPropertiesToDTD
    '
    ' Description: Build the Data Set DTD
    '
    ' ***************************************************************** '
    'Private Function AddObjectPropertiesToDTD(ByVal v_sObjectName As String, ByVal v_sTableName As String, ByRef wrt As MSXML2.MXXMLWriter40, ByRef r_vPropertyArray(,) As Object) As Integer
    Private Function AddObjectPropertiesToDTD(ByVal v_sObjectName As String, ByVal v_sTableName As String, ByRef dtdWriter As StringBuilder, ByRef r_vPropertyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sPropertyName As String = ""
        Dim sColumnName As String = String.Empty


        result = gPMConstants.PMEReturnCode.PMTrue


        sPropertyName = ""
        For lSub As Integer = r_vPropertyArray.GetLowerBound(1) To r_vPropertyArray.GetUpperBound(1)

            sPropertyName = CStr(r_vPropertyArray(0, lSub))

            lReturn = GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, r_sColumnName:=sColumnName)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            AppendAttribute(v_sObjectName, sPropertyName, "CDATA", "#IMPLIED", dtdWriter)

        Next lSub


        Return result

    End Function


    ' ***************************************************************** '
    ' Name: AddObjectPropertiesToDTD
    '
    ' Description: Build the Data Set DTD
    '
    ' ***************************************************************** '
    Private Function AddObjectPropertiesToDTD1(ByVal v_sObjectName As String, ByVal v_sTableName As String, ByRef xsXMLSchemaCT As XmlSchemaComplexType, ByRef r_vPropertyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sPropertyName As String = ""
        Dim sColumnName As String = ""

        'We need these variables for typecasting the writer.
        Dim xsSchemaAttb As XmlSchemaAttribute




        result = gPMConstants.PMEReturnCode.PMTrue


        'Set handler variables to the writer so it implements the interfaces.

        sPropertyName = ""
        For lSub As Integer = r_vPropertyArray.GetLowerBound(1) To r_vPropertyArray.GetUpperBound(1)

            sPropertyName = CStr(r_vPropertyArray(0, lSub))

            lReturn = CType(GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, r_sColumnName:=sColumnName), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            '        If (UCase(Trim$(sColumnName)) = UCase(Trim$(v_sTableName) & "_id")) Then
            '            dech.attributeDecl v_sObjectName, sPropertyName, "ID", "#REQUIRED", ""
            '        Else
            xsSchemaAttb.Name = sPropertyName
            ' xsSchemaAttb.QualifiedName
            xsSchemaAttb.Name = sPropertyName




            'dech.attributeDecl(v_sObjectName, sPropertyName, "CDATA", "#IMPLIED", "")
            '        End If
        Next lSub

        ' dech = Nothing

        Return result

    End Function



    ' ***************************************************************** '
    ' Name: GetPropertyValues
    '
    ' Description: Returns an Array of the Identifying Properties and
    '              their values for an Object Instance AND/OR an Array
    '              of All Properties and their values.
    ' ***************************************************************** '
    Private Function GetPropertyValues(ByRef v_oObjectDef As XmlElement, ByRef v_oObjectInst As XmlElement, Optional ByRef r_vIdValueArray(,) As Object = Nothing, Optional ByRef r_vAllValueArray(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lIDProperty As gPMConstants.PMEReturnCode
        Dim lNum As Integer
        Dim oChildDef As XmlElement
        Dim lIDNum, lAllNum As Integer
        Dim sObjectName, sPropertyName As String
        Dim vPropertyValue As String = ""

        Dim vIdValueArray, vAllValueArray(,) As Object



        result = gPMConstants.PMEReturnCode.PMTrue


        If (Informations.IsNothing(r_vIdValueArray)) And (Informations.IsNothing(r_vAllValueArray)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the Object Name

        sObjectName = CStr(v_oObjectDef.GetAttribute(ACXMLAttribObjectName))



        r_vIdValueArray = Nothing


        r_vAllValueArray = Nothing

        ' Redim the Array. Always Allow for 3 Name/Value Pairs
        ' Note: Always allow for 3 values. If there are not 2 ID Properties
        ' the unused rows will be Empty
        ReDim vIdValueArray(1, 2)
        lIDNum = 0

        ' We will build the All Value Array Dynamically


        vAllValueArray = Nothing
        lAllNum = 0

        ' Get the Number of Children for this Object
        lNum = v_oObjectDef.ChildNodes.Count - 1

        ' For Each Child
        For lChild As Integer = 0 To lNum

            ' Get the Child
            oChildDef = v_oObjectDef.ChildNodes.Item(lChild)

            ' If the Child doesn't have any Children of its own
            ' then it is a Property
            If Not oChildDef.HasChildNodes Then

                ' Get the Identifying Property Attribute

                lIDProperty = oChildDef.GetAttribute(ACXMLAttribIsIdentProp)

                ' Get the PropertyName

                sPropertyName = CStr(oChildDef.GetAttribute(ACXMLAttribPropertyName))

                vPropertyValue = PropertyValue(v_oObjectInst, sPropertyName)

                ' If we are building the IDValue Array

                If Not Informations.IsNothing(r_vIdValueArray) Then

                    ' If this is an Identifying Property then add it to the ID Value array
                    ' Note: We can only return 3 Identifying Properties

                    If (lIDProperty = gPMConstants.PMEReturnCode.PMTrue) And (lIDNum <= vIdValueArray.GetUpperBound(1)) Then

                        ' Build the Array

                        vIdValueArray(iGISSharedConstants.GISIDColPropName, lIDNum) = sPropertyName

                        vIdValueArray(iGISSharedConstants.GISIDColPropValue, lIDNum) = vPropertyValue

                        ' Increment the Array Row
                        lIDNum += 1

                    End If

                End If

                ' If we are building the All Value Array

                If Not Informations.IsNothing(r_vAllValueArray) Then

                    ' Resize the All Value Array
                    If Informations.IsArray(vAllValueArray) Then

                        lAllNum = vAllValueArray.GetUpperBound(1) + 1
                        ReDim Preserve vAllValueArray(1, lAllNum)
                    Else
                        lAllNum = 0
                        ReDim vAllValueArray(1, 0)
                    End If

                    ' Add the Property

                    vAllValueArray(iGISSharedConstants.GISIDColPropName, lAllNum) = sPropertyName

                    vAllValueArray(iGISSharedConstants.GISIDColPropValue, lAllNum) = vPropertyValue

                End If

            End If

        Next lChild

        ' If required, return the IDValue Array

        If Not Informations.IsNothing(r_vIdValueArray) Then


            r_vIdValueArray = vIdValueArray
        End If

        ' If required, return the ALLValue Array

        If Not Informations.IsNothing(r_vAllValueArray) Then


            r_vAllValueArray = vAllValueArray
        End If


        vIdValueArray = Nothing

        vAllValueArray = Nothing

        oChildDef = Nothing

        Return result

    End Function

    ' RFC 290200 - Add the SQL Statements as Attributes of the Object
    ' ***************************************************************** '
    ' Name: BuildAddUpdateSQL
    '
    ' Description: Build an SQL Statement for either an Insert OR
    '              Update.
    '
    ' ***************************************************************** '
    Private Function BuildAddUpdateSQL(ByVal v_sObjectName As String, ByVal v_sTableName As String, ByVal v_vPropertyArray(,) As Object, ByRef r_sAddSQL As String, ByRef r_sUpdateSQL As String, ByRef r_sAddUpdateSQL As String, ByRef r_sDeleteSQL As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sDelSQL1 As String = ""
        Dim sUpdSQL1 As New StringBuilder
        Dim sDelSQL2 As New StringBuilder
        Dim sUpdSQL2 As New StringBuilder
        Dim sAddSQL2 As New StringBuilder
        Dim sAddSQL1 As New StringBuilder

        Dim lGISPropertyID As Integer
        Dim sColumnName As String = ""
        Dim sPropertyName As String = ""
        Dim lDataType As Integer
        Dim iIsPrimaryKey As gPMConstants.PMEReturnCode
        Dim iIsIdentifyingProperty As Integer
        Dim lGISListID, lPolarisPropertyID As Integer


        result = gPMConstants.PMEReturnCode.PMTrue

        ' If there are No Properties then Error
        If Not Informations.IsArray(v_vPropertyArray) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        sAddSQL1 = New StringBuilder("")
        sAddSQL2 = New StringBuilder("")
        sUpdSQL1 = New StringBuilder("")
        sUpdSQL2 = New StringBuilder("")
        sDelSQL1 = ""
        sDelSQL2 = New StringBuilder("")

        ' For Each Property
        For lRow As Integer = v_vPropertyArray.GetLowerBound(1) To v_vPropertyArray.GetUpperBound(1)

            ' Get the Property Name from the Array

            sPropertyName = CStr(v_vPropertyArray(0, lRow)).Trim()

            ' Get the Property Details
            lReturn = CType(GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, r_lGISPropertyID:=lGISPropertyID, r_sColumnName:=sColumnName, r_lDataType:=lDataType, r_iIsPrimaryKey:=iIsPrimaryKey, r_iIsIdentifyingProperty:=iIsIdentifyingProperty, r_lGISListID:=lGISListID, r_lPolarisPropertyID:=lPolarisPropertyID), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Build the Column List

            sAddSQL1.Append(sColumnName)

            sAddSQL2.Append(ACSQLStartParam & sColumnName & ACSQLEndParam)

            If lRow < v_vPropertyArray.GetUpperBound(1) Then
                sAddSQL1.Append(ACSQLSeparator)
                sAddSQL2.Append(ACSQLSeparator)
            End If


            If iIsPrimaryKey = gPMConstants.PMEReturnCode.PMTrue Then

                If sUpdSQL2.ToString() <> "" Then
                    sUpdSQL2.Append(ACSQLAnd)
                End If
                sUpdSQL2.Append(sColumnName & ACSQLEquals & ACSQLStartParam & sColumnName & ACSQLEndParam)

                'RFC200400 - Add Proper Delete Functionality
                If sDelSQL2.ToString() <> "" Then
                    sDelSQL2.Append(ACSQLAnd)
                End If
                sDelSQL2.Append(sColumnName & ACSQLEquals & ACSQLStartParam & sColumnName & ACSQLEndParam)

            Else

                If sUpdSQL1.ToString() <> "" Then
                    sUpdSQL1.Append(ACSQLSeparator)
                End If
                sUpdSQL1.Append(sColumnName & ACSQLEquals & ACSQLStartParam & sColumnName & ACSQLEndParam)

            End If

        Next lRow

        ' Build the SQL Strings depending on the type of Command

        ' INSERT INTO tablename (col1, col2....)
        sAddSQL1 = New StringBuilder(ACAddSQLStart & v_sTableName.Trim() & ACSQLStartCol & sAddSQL1.ToString() & ACSQLEndCol)

        ' VALUES (col1, col2....)
        sAddSQL2 = New StringBuilder(ACAddSQLValues & ACSQLStartCol & sAddSQL2.ToString() & ACSQLEndCol)


        ' UPDATE tablename SET col1 = {col1}, col2 = {col2}....
        sUpdSQL1 = New StringBuilder(ACUpdSQLStart & v_sTableName.Trim() & ACUpdSQLSet & sUpdSQL1.ToString())

        ' WHERE pkcol1 = {pkcol1}, pkcol2 = {pkcol2}....
        sUpdSQL2 = New StringBuilder(ACSQLWhere & sUpdSQL2.ToString())

        'RFC200400 - Add Proper Delete Functionality
        sDelSQL1 = ACDelSQLDeleteFrom & v_sTableName.Trim() & ACDelSQLWhere

        ' Return the Full SQL String
        r_sAddSQL = sAddSQL1.ToString() & " " & sAddSQL2.ToString()
        r_sUpdateSQL = sUpdSQL1.ToString() & " " & sUpdSQL2.ToString()

        'sj 18/08/99 - start
        r_sAddUpdateSQL = "IF EXISTS (SELECT * FROM " &
                          v_sTableName.Trim() &
                          sUpdSQL2.ToString() & ") " & Strings.ChrW(13) & Strings.ChrW(10) &
                          r_sUpdateSQL & Strings.ChrW(13) & Strings.ChrW(10) &
                          " Else " & Strings.ChrW(13) & Strings.ChrW(10) &
                              r_sAddSQL
        'sj 18/08/99 - end

        'RFC200400 - Add Proper Delete Functionality
        r_sDeleteSQL = sDelSQL1 & sDelSQL2.ToString()

        Return result

    End Function

    ' RFC 290200 - Add the SQL Statements as Attributes of the Object
    ' ***************************************************************** '
    ' Name: BuildSelectSQL
    '
    ' Description: Builds SQL for a Select Statement.
    '
    ' ***************************************************************** '
    Private Function BuildSelectSQL(ByVal v_sObjectName As String, ByVal v_sTableName As String, ByVal v_vPropertyArray(,) As Object, ByVal v_sTopLevelTableName As String, ByRef r_sSelectSQL As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sSQL2 As String = ""
        Dim sSQL1 As New StringBuilder

        Dim lGISPropertyID As Integer
        Dim sColumnName As String = ""
        Dim sPropertyName As String = ""
        Dim lDataType As Integer
        Dim iIsPrimaryKey, iIsIdentifyingProperty As Integer
        Dim lGISListID, lPolarisPropertyID As Integer


        result = gPMConstants.PMEReturnCode.PMTrue

        ' If there are No Properties then Error
        If Not Informations.IsArray(v_vPropertyArray) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        sSQL1 = New StringBuilder("")
        sSQL2 = ""

        ' For Each Property
        For lRow As Integer = v_vPropertyArray.GetLowerBound(1) To v_vPropertyArray.GetUpperBound(1)

            ' Get the Property Name from the Array

            sPropertyName = CStr(v_vPropertyArray(0, lRow)).Trim()

            ' Get the Property Details
            lReturn = CType(GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, r_lGISPropertyID:=lGISPropertyID, r_sColumnName:=sColumnName, r_lDataType:=lDataType, r_iIsPrimaryKey:=iIsPrimaryKey, r_iIsIdentifyingProperty:=iIsIdentifyingProperty, r_lGISListID:=lGISListID, r_lPolarisPropertyID:=lPolarisPropertyID), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Build the Column List
            sSQL1.Append(sColumnName)

            If lRow < v_vPropertyArray.GetUpperBound(1) Then
                sSQL1.Append(ACSQLSeparator)
            End If

            ' If this is the Top Level PK Column
            'If (UCase(sColumnName) = UCase((Trim$(Left$(v_sTopLevelTableName, 27)) & ACIDCol))) Then
            ' RFC280400 - Table Name Size limit removed at behest of Marsh team
            If sColumnName.ToUpper() = (v_sTopLevelTableName.Trim() & ACIDCol).ToUpper() Then
                ' PKCol = {PKCol}
                sSQL2 = sColumnName & ACSQLEquals & ACSQLStartParam & sColumnName & ACSQLEndParam

            End If

        Next lRow

        ' Build Up the Full SQL String

        ' SELECT col1, col2, col3.... FROM Tablename
        r_sSelectSQL = ACSelSQLSelect & sSQL1.ToString() & ACSelSQLFrom & v_sTableName.Trim()

        ' WHERE pol_id = {pol_id}
        r_sSelectSQL = r_sSelectSQL & ACSQLWhere & sSQL2

        ' ORDER BY table_name_id ASC
        'r_sSelectSQL = r_sSelectSQL & ACSelSQLOrderBy & Trim$(Left$(v_sTableName, 27)) & ACIDCol & ACSelSQLAsc
        ' RFC280400 - Table Name Size limit removed at behest of Marsh team
        r_sSelectSQL = r_sSelectSQL & ACSelSQLOrderBy & v_sTableName.Trim() & ACIDCol & ACSelSQLAsc

        Return result

    End Function


    ' RFC121101
    ' ***************************************************************** '
    ' Name: BuildCopyDatasetSP
    '
    ' Description: Build an SP to copy a Dataset.
    '
    ' ***************************************************************** '
    Private Function BuildCopyDatasetSP(ByVal v_sTopRiskObjectName As String, Optional ByVal v_sTopQuoteObjectName As String = "") As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        ' Object Definition
        Dim sTableName As String = ""
        Dim vChildObjectArray As Object = Nothing
        Dim sColumnName As String = ""
        Dim vPropertyArray As Object = Nothing
        Dim sPropertyName As String = ""
        Dim lIsQuoteObject As Integer
        Dim sSQL As String = ""
        ' Used to Create the SP
        Dim iFileNum As Integer
        Dim sSPName As String = ""
        Dim sSPFileName As String = ""
        ' Output Strings
        Dim sChildObjectName As String = ""
        Dim sLoadSPPath As String = ""
        Dim sPathTest As String = ""
        Dim sTopLevelPKCol As String = ""


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the Object Definition Details
        lReturn = CType(GetObjectDefDetails(v_sObjectName:=v_sTopRiskObjectName, r_lIsQuoteObject:=lIsQuoteObject, r_sTableName:=sTableName, r_vChildObjectArray:=vChildObjectArray, r_vPropertyArray:=vPropertyArray), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' Build the SP Name
        sSPName = "spg_" & GISDataModelCode.ToLower().Trim() & "_copy_dataset"
        ' Build the SP File Name
        sSPFileName = sSPName & ".sql"

        ' RDC 10072001 START
        lReturn = CType(iGISSharedConstants.GetLoadSPPath(GISDataModelCode, sLoadSPPath), gPMConstants.PMEReturnCode)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' check path exists, create if not
        sPathTest = FileSystem.Dir(sLoadSPPath, FileAttribute.Directory)

        If sPathTest = "" Then
            Directory.CreateDirectory(sLoadSPPath)
        End If

        ' Get the Next File Number
        iFileNum = FileSystem.FreeFile()

        FileSystem.FileOpen(iFileNum, sLoadSPPath & "\" & sSPFileName, OpenMode.Output)
        ' RDC 10072001 END

        ' Write out the Stored Procedure

        ' RAM20040831 : Commented the following lines
        'Print #iFileNum, "IF EXISTS (SELECT * FROM SYSOBJECTS WHERE id = object_id('dbo." & sSPName & "') and sysstat & 0xf = 4)"
        'Print #iFileNum, "    DROP PROCEDURE dbo."; sSPName
        'Print #iFileNum, "GO"
        'Print #iFileNum,

        FileSystem.PrintLine(iFileNum, "CREATE PROCEDURE " & sSPName)
        FileSystem.PrintLine(iFileNum, "    @old_gis_policy_link_id INTEGER = NULL,")
        FileSystem.PrintLine(iFileNum, "    @old_insurance_file_cnt INTEGER = NULL ,")
        FileSystem.PrintLine(iFileNum, "    @old_risk_id INTEGER = NULL,")
        FileSystem.PrintLine(iFileNum, "    @new_insurance_file_cnt INTEGER = NULL,")
        FileSystem.PrintLine(iFileNum, "    @new_risk_id INTEGER = NULL,")
        FileSystem.PrintLine(iFileNum, "    @copy_quotes tinyint,")
        FileSystem.PrintLine(iFileNum, "    @new_quote_ref char(11) = NULL,")
        FileSystem.PrintLine(iFileNum, "    @new_quote_ref_password varchar(30) = NULL,")
        FileSystem.PrintLine(iFileNum, "    @new_gis_policy_link_id INTEGER OUTPUT")
        FileSystem.PrintLine(iFileNum)
        FileSystem.PrintLine(iFileNum, "AS")
        FileSystem.PrintLine(iFileNum, "BEGIN")
        FileSystem.PrintLine(iFileNum)
        FileSystem.PrintLine(iFileNum, "    SET NOCOUNT ON")
        FileSystem.PrintLine(iFileNum)
        FileSystem.PrintLine(iFileNum, "    /* WE MUST HAVE EITHER AND OLD POLICY_LINK_ID or INSURANCE_FILE_CNT*/")
        FileSystem.PrintLine(iFileNum, "    IF  (@old_gis_policy_link_id IS NULL)")
        FileSystem.PrintLine(iFileNum, "    AND (@old_insurance_file_cnt IS NULL)")
        FileSystem.PrintLine(iFileNum, "        RETURN -100")
        FileSystem.PrintLine(iFileNum)
        FileSystem.PrintLine(iFileNum, "    IF (@old_gis_policy_link_id IS NULL) BEGIN")
        FileSystem.PrintLine(iFileNum)
        FileSystem.PrintLine(iFileNum, "        IF (@old_insurance_file_cnt IS NULL) BEGIN")
        FileSystem.PrintLine(iFileNum)
        FileSystem.PrintLine(iFileNum, "            IF (@old_risk_id IS NULL) BEGIN")
        FileSystem.PrintLine(iFileNum, "                    SELECT @old_gis_policy_link_id = gis_policy_link_id")
        FileSystem.PrintLine(iFileNum, "                    From gis_policy_link")
        FileSystem.PrintLine(iFileNum, "                    WHERE insurance_file_cnt = @old_insurance_file_cnt")
        FileSystem.PrintLine(iFileNum, "            END Else BEGIN")
        FileSystem.PrintLine(iFileNum, "                    SELECT @old_gis_policy_link_id = gis_policy_link_id")
        FileSystem.PrintLine(iFileNum, "                    From gis_policy_link")
        FileSystem.PrintLine(iFileNum, "                    WHERE insurance_file_cnt = @old_insurance_file_cnt")
        FileSystem.PrintLine(iFileNum, "                    AND risk_id = @old_risk_id")
        FileSystem.PrintLine(iFileNum, "            END")
        FileSystem.PrintLine(iFileNum, "        END Else BEGIN")
        FileSystem.PrintLine(iFileNum, "            SELECT @old_gis_policy_link_id = gis_policy_link_id")
        FileSystem.PrintLine(iFileNum, "            From gis_policy_link")
        FileSystem.PrintLine(iFileNum, "            WHERE insurance_file_cnt = @old_insurance_file_cnt")
        FileSystem.PrintLine(iFileNum, "        END")
        FileSystem.PrintLine(iFileNum, "    END")
        FileSystem.PrintLine(iFileNum)
        FileSystem.PrintLine(iFileNum, "    IF (@old_gis_policy_link_id IS NULL)")
        FileSystem.PrintLine(iFileNum, "        RETURN -101")
        FileSystem.PrintLine(iFileNum)
        ' PW020304 - CQ4074 - get old_insurance_file_cnt if not passed: Start
        FileSystem.PrintLine(iFileNum, "    IF (@old_insurance_file_cnt IS NULL) BEGIN")
        FileSystem.PrintLine(iFileNum, "        SELECT @old_insurance_file_cnt = insurance_file_cnt")
        FileSystem.PrintLine(iFileNum, "          FROM gis_policy_link")
        FileSystem.PrintLine(iFileNum, "         WHERE gis_policy_link_id = @old_gis_policy_link_id")
        FileSystem.PrintLine(iFileNum, "    END")
        FileSystem.PrintLine(iFileNum)
        ' PW020304 - CQ4074 - get old_insurance_file_cnt if not passed: End
        FileSystem.PrintLine(iFileNum, "/* Create a New Policy Link Record */")
        FileSystem.PrintLine(iFileNum, "INSERT INTO GIS_Policy_Link (")
        FileSystem.PrintLine(iFileNum, "gis_data_model_id,")
        FileSystem.PrintLine(iFileNum, "insurance_file_cnt,")
        FileSystem.PrintLine(iFileNum, "    quote_ref,")
        FileSystem.PrintLine(iFileNum, "    quote_ref_password,")
        FileSystem.PrintLine(iFileNum, "    guaranteed_quote_date,")
        FileSystem.PrintLine(iFileNum, "    gis_scheme_id,")
        FileSystem.PrintLine(iFileNum, "    transact_date,")
        FileSystem.PrintLine(iFileNum, "    transact_type,")
        FileSystem.PrintLine(iFileNum, "    party_cnt,")
        FileSystem.PrintLine(iFileNum, "    risk_id,")
        FileSystem.PrintLine(iFileNum, "    claim_id)")
        FileSystem.PrintLine(iFileNum, "SELECT")
        FileSystem.PrintLine(iFileNum, "    gis_data_model_id,")
        FileSystem.PrintLine(iFileNum, "    @new_insurance_file_cnt,")
        FileSystem.PrintLine(iFileNum, "    @new_quote_ref,")
        FileSystem.PrintLine(iFileNum, "    @new_quote_ref_password,")
        FileSystem.PrintLine(iFileNum, "    NULL,")
        FileSystem.PrintLine(iFileNum, "    NULL,")
        FileSystem.PrintLine(iFileNum, "    NULL,")
        FileSystem.PrintLine(iFileNum, "    NULL,")
        FileSystem.PrintLine(iFileNum, "    party_cnt,")
        FileSystem.PrintLine(iFileNum, "    @new_risk_id,")
        FileSystem.PrintLine(iFileNum, "    NULL")
        FileSystem.PrintLine(iFileNum, "From GIS_Policy_Link")
        FileSystem.PrintLine(iFileNum, "WHERE gis_policy_link_id = @old_gis_policy_link_id")
        FileSystem.PrintLine(iFileNum)
        FileSystem.PrintLine(iFileNum, "/* Get the Policy LinkID */")
        FileSystem.PrintLine(iFileNum, "SELECT @new_gis_policy_link_id = @@IDENTITY")

        ' Set the Top Level PK Col
        sTopLevelPKCol = sTableName & "_ID"

        ' Build the SQL for the Risk Objects
        lReturn = CType(BuildCopyDatasetSPForObject(v_sObjectName:=v_sTopRiskObjectName, v_bTopLevelObject:=True, v_sTopLevelPKCol:=sTopLevelPKCol, r_sSQL:=sSQL), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        FileSystem.PrintLine(iFileNum)
        FileSystem.PrintLine(iFileNum, sSQL)
        FileSystem.PrintLine(iFileNum)

        sSQL = ""

        ' If we have a Top Level Quote Object
        If v_sTopQuoteObjectName.Trim() <> "" Then

            FileSystem.PrintLine(iFileNum, "IF @copy_quotes = 1")
            FileSystem.PrintLine(iFileNum, "BEGIN")

            ' Build the SQL for the Quote Objects
            lReturn = CType(BuildCopyDatasetSPForObject(v_sObjectName:=v_sTopQuoteObjectName, v_bTopLevelObject:=True, v_sTopLevelPKCol:=sTopLevelPKCol, r_sSQL:=sSQL), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            FileSystem.PrintLine(iFileNum, sSQL)
            FileSystem.PrintLine(iFileNum, "END")
        End If


        FileSystem.PrintLine(iFileNum)
        FileSystem.PrintLine(iFileNum, "END")

        ' RAM20040831 : Commented the following lines
        'Print #iFileNum, "GO"

        FileSystem.FileClose(iFileNum) ' Close file.


        Return result

    End Function


    ' RFC121101
    ' ***************************************************************** '
    ' Name: BuildCopyDatasetSPForObject
    '
    ' Description: Build an SP to copy a Dataset.
    '
    ' ***************************************************************** '
    Private Function BuildCopyDatasetSPForObject(ByVal v_sObjectName As String, ByVal v_bTopLevelObject As Boolean, ByVal v_sTopLevelPKCol As String, ByRef r_sSQL As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        ' Object Definition
        Dim sTableName As String = ""
        Dim vChildObjectArray As Object = Nothing
        Dim sColumnName As String = ""
        Dim vPropertyArray(,) As Object = Nothing
        Dim sPropertyName As String = ""
        Dim lIsQuoteObject As gPMConstants.PMEReturnCode

        Dim sFrom, sWhere As String
        Dim sSelect As New StringBuilder
        Dim sInsertInto As New StringBuilder
        ' Output Strings
        Dim sChildObjectName As String = ""
        Dim lNonGISType As Integer


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the Object Definition Details
        lReturn = CType(GetObjectDefDetails(v_sObjectName:=v_sObjectName, r_lIsQuoteObject:=lIsQuoteObject, r_sTableName:=sTableName, r_vChildObjectArray:=vChildObjectArray, r_vPropertyArray:=vPropertyArray, r_lIsNonGIS:=lNonGISType), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' If this is a SPECIAL object (i.e. Disclosures or CLaims) , exit
        ' PW010903 - CQ1912 - we want to deal with associated clients/disclosures
        If (lNonGISType <> GISDataModelType.GISOTRisk) And (lNonGISType <> GISDataModelType.GISOTNonGisSpecials) And (lNonGISType <> GISDataModelType.GISOTAssociatedClient) And (lNonGISType <> GISDataModelType.GISOTDisclosure) And (lNonGISType <> GISDataModelType.GISOTCase) Then
            Return result
        End If

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' RAM20040916 : We need to treat the Top level POLICY_BINDER Object differently.
        '               since we have the values for that row, we don't need to use
        '               INSERT INTO with a SELECT,  we can use a direct INSERT INTO
        '               eg. as shown below
        ' OLD --> INSERT INTO ProductPer_Policy_Binder (ProductPer_Policy_binder_id , gis_policy_link_id) SELECT @new_gis_policy_link_id , @new_gis_policy_link_id FROM ProductPer_Policy_Binder WHERE gis_policy_link_id = @old_gis_policy_link_id
        ' NEW --> INSERT INTO ProductPer_Policy_Binder (ProductPer_Policy_binder_id , gis_policy_link_id) VALUES (@new_gis_policy_link_id , @new_gis_policy_link_id)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If sTableName.Trim().ToLower() = (GISDataModelCode.Trim() & "_Policy_Binder").ToLower() Then
            r_sSQL = r_sSQL & "INSERT INTO " & sTableName & " (" & GISDataModelCode.Trim() & "_Policy_binder_id , gis_policy_link_id) VALUES ("
            r_sSQL = r_sSQL & "@new_gis_policy_link_id , @new_gis_policy_link_id)"
        Else
            sInsertInto = New StringBuilder("INSERT INTO " & sTableName & " (")
            sSelect = New StringBuilder("SELECT ")
            sFrom = "FROM " & sTableName & " "

            ' PW010903 - CQ1912 - associated clients / disclosures
            Select Case lNonGISType
                Case GISDataModelType.GISOTDisclosure
                    sInsertInto.Append("party_cnt , party_conviction_id , " &
                                           "insurance_folder_cnt , risk_cnt , ")

                    sSelect.Append(sTableName & ".party_cnt , " &
                                   "pc.party_conviction_id , " &
                                   sTableName & ".insurance_folder_cnt , " &
                                       "@new_risk_id , ")

                    ' RAW 10/12/2003 : CQ3534 : add join on party_conviction.original_pc_id
                    sWhere = "INNER JOIN gis_policy_link gpl ON " &
                             sTableName & "." & "insurance_folder_cnt = gpl.insurance_folder_cnt AND " &
                             sTableName & "." & "risk_cnt = gpl.risk_id " &
                             "INNER JOIN party_conviction pc ON " &
                             sTableName & "." & "party_cnt = pc.party_cnt AND " &
                                 sTableName & "." & "party_conviction_id = pc.original_pc_id "

                    ' RAW 10/12/2003 : CQ3534 : added test for disclosure.risk_cnt = old_risk_cnt
                    sWhere = sWhere & " " &
                             "WHERE gpl.gis_policy_link_id = @old_gis_policy_link_id " &
                             "AND pc.risk_cnt = @new_risk_id " &
                                 "AND " & sTableName & ".risk_cnt = @old_risk_id "

                ' PW020304 - CQ4074 - set up associated client insert statement
                Case GISDataModelType.GISOTAssociatedClient
                    sInsertInto.Append("party_cnt , " &
                                           "insurance_file_cnt , risk_cnt , ")

                    sSelect.Append(sTableName & ".party_cnt , " &
                                   "@new_insurance_file_cnt , " &
                                       "@new_risk_id , ")

                    sWhere = "WHERE " & sTableName &
                             ".insurance_file_cnt = @old_insurance_file_cnt AND " &
                                 sTableName & ".risk_cnt = @old_risk_id"

                Case Else
                    If v_bTopLevelObject Or lIsQuoteObject = gPMConstants.PMEReturnCode.PMTrue Then
                        sWhere = "WHERE gis_policy_link_id = @old_gis_policy_link_id"
                    Else
                        sWhere = "WHERE " & v_sTopLevelPKCol & " = @old_gis_policy_link_id"
                    End If
            End Select

            ' For Each Property

            For lRow As Integer = vPropertyArray.GetLowerBound(1) To vPropertyArray.GetUpperBound(1)

                ' Get the Property Name from the Array

                sPropertyName = CStr(vPropertyArray(0, lRow)).Trim()

                ' Get the Property Details
                lReturn = CType(GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, r_sColumnName:=sColumnName), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

                ' PW020903 - CQ1912 - ignore static columns for asscli/disc
                If Not ((lNonGISType = GISDataModelType.GISOTAssociatedClient Or lNonGISType = GISDataModelType.GISOTDisclosure) And IsAssocClientStaticColumn(sColumnName.ToUpper())) Then

                    ' Build the Insert Into Column List
                    sInsertInto.Append(sColumnName)


                    If lRow < vPropertyArray.GetUpperBound(1) Then
                        sInsertInto.Append(ACSQLSeparator)
                    End If

                    ' Build the Select
                    ' PW010903 - CQ1912
                    Select Case lNonGISType
                        ' PW020304 - CQ4074 - do same for AC as disclosure
                        Case GISDataModelType.GISOTDisclosure, GISDataModelType.GISOTAssociatedClient
                            ' RAW 10/12/2003 : CQ3534 : removed test for static columns which have been excluded by earlier condition
                            sSelect.Append(sTableName & "." & sColumnName)

                        Case Else
                            If sColumnName.Trim().ToLower() = "gis_policy_link_id" Or sColumnName.Trim().ToLower() = v_sTopLevelPKCol.ToLower() Then
                                sSelect.Append("@new_gis_policy_link_id")
                            Else
                                sSelect.Append(sColumnName)
                            End If
                    End Select


                    If lRow < vPropertyArray.GetUpperBound(1) Then
                        sSelect.Append(ACSQLSeparator)
                    End If

                End If

            Next lRow

            ' PW020903 - CQ1912 - remove last , if necessary
            If sInsertInto.ToString().Substring(sInsertInto.ToString().Length - ACSQLSeparator.Length) = ACSQLSeparator Then
                sInsertInto = New StringBuilder(sInsertInto.ToString().Substring(0, sInsertInto.ToString().Length - ACSQLSeparator.Length))
            End If

            sInsertInto.Append(") ")

            ' PW020903 - CQ1912 - remove last , if necessary
            If sSelect.ToString().Substring(sSelect.ToString().Length - ACSQLSeparator.Length) = ACSQLSeparator Then
                sSelect = New StringBuilder(sSelect.ToString().Substring(0, sSelect.ToString().Length - ACSQLSeparator.Length))
            End If

            sSelect.Append(" ")

            If r_sSQL.Trim() <> "" Then
                r_sSQL = r_sSQL & Strings.ChrW(13) & Strings.ChrW(10)
            End If

            ' PW010903 - CQ1912
            Select Case lNonGISType
                ' If this is an associated client, we only do an update, because the
                ' new dm_associated_client record will have been created when the
                ' policy was copied
                Case GISDataModelType.GISOTAssociatedClient
                    ' PW250204 - CQ4074 - we need to do an insert here if the associated
                    ' client record does not already exist. This can happen when a risk
                    ' is copied in the ListRisks screen in order to create a variation
                    ' PW200404 - CQ4874 - change method so it works for copying
                    ' across policies (for XYZ)
                    r_sSQL = r_sSQL & "IF NOT EXISTS (SELECT insurance_file_cnt FROM "
                    r_sSQL = r_sSQL & sTableName & " WHERE "
                    r_sSQL = r_sSQL & sTableName & ".insurance_file_cnt = @new_insurance_file_cnt AND "
                    r_sSQL = r_sSQL & sTableName & ".risk_cnt = @old_risk_id)"
                    r_sSQL = r_sSQL & Strings.ChrW(13) & Strings.ChrW(10)
                    r_sSQL = r_sSQL & "    " & sInsertInto.ToString() & sSelect.ToString() & sFrom & sWhere
                    r_sSQL = r_sSQL & Strings.ChrW(13) & Strings.ChrW(10)
                    r_sSQL = r_sSQL & "ELSE"
                    r_sSQL = r_sSQL & Strings.ChrW(13) & Strings.ChrW(10)
                    r_sSQL = r_sSQL & "    UPDATE " & sTableName & " " &
                             "SET risk_cnt = @new_risk_id " &
                             "FROM " & sTableName & " " &
                             "INNER JOIN gis_policy_link gpl ON " &
                             sTableName & ".insurance_file_cnt = gpl.insurance_file_cnt " &
                             "WHERE gpl.gis_policy_link_id = @new_gis_policy_link_id " &
                                 "AND " & sTableName & ".risk_cnt = @old_risk_id"
                Case Else
                    r_sSQL = r_sSQL & sInsertInto.ToString() & sSelect.ToString() & sFrom & sWhere
            End Select

        End If

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' RAM20040916 : END
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        ' Build the SP for Each Child Table
        If Informations.IsArray(vChildObjectArray) Then

            For lRow As Integer = vChildObjectArray.GetLowerBound(0) To vChildObjectArray.GetUpperBound(0)

                sChildObjectName = CStr(vChildObjectArray(lRow)).Trim()
                lReturn = CType(BuildCopyDatasetSPForObject(v_sObjectName:=sChildObjectName, v_bTopLevelObject:=False, v_sTopLevelPKCol:=v_sTopLevelPKCol, r_sSQL:=r_sSQL), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If
            Next lRow
        End If

        Return result

    End Function


    ' RFC240101
    ' ***************************************************************** '
    ' Name: BuildSelectSP
    '
    ' Description: Build a Select Stored Procedure for the given Object.
    ' ***************************************************************** '
    Private Function BuildSelectSP(ByVal v_sObjectName As String, ByVal v_bTopLevelObject As Boolean, ByVal v_bQuoteObject As Boolean) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        ' Object Definition
        Dim lIsQuoteObject, lGISObjectID, lGISPropertyID As Integer
        Dim sTableName As String = ""
        Dim lMaxInstances, lPolarisObjectID As Integer
        Dim sParentObjectName As String = ""
        Dim vChildObjectArray As Object = Nothing
        Dim vPropertyArray(,) As Object = Nothing
        Dim sPropertyName As String = ""
        Dim sColumnName As String = ""
        Dim iIsPrimaryKey As gPMConstants.PMEReturnCode
        Dim lNonGISType As Integer
        Dim sParentTableName As String = ""
        Dim sOIKey As String = ""
        Dim sParentOIKey As String = ""
        Dim sSelectSQL As String = ""
        Dim lDataType As Integer
        ' Used to Create the SP
        Dim iFileNum As Integer
        Dim sCursorName, sSPName, sSPFileName, sSQLType As String

        ' Output Strings
        Dim sOrderBy As String = ""
        Dim sPKColumn As String = ""
        Dim sChildObjectName As String = ""
        Dim sChildTableName As String = ""
        Dim sXMLBuild As New StringBuilder
        Dim sPKXMLBuild As New StringBuilder
        Dim sInto As New StringBuilder
        Dim sSelect As New StringBuilder
        Dim sSPExec As New StringBuilder
        Dim sWhere As New StringBuilder
        Dim sVarDecs As New StringBuilder
        Dim sParamDecs As New StringBuilder
        Dim sLoadSPPath As String = ""
        Dim sPathTest As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the Object Definition Details
        lReturn = CType(GetObjectDefDetails(v_sObjectName:=v_sObjectName, r_lIsQuoteObject:=lIsQuoteObject, r_lGISObjectID:=lGISObjectID, r_sTableName:=sTableName, r_lMaxInstances:=lMaxInstances, r_lPolarisObjectID:=lPolarisObjectID, r_sParentObjectName:=sParentObjectName, r_vChildObjectArray:=vChildObjectArray, r_vPropertyArray:=vPropertyArray, r_sSelectSQL:=sSelectSQL, r_lIsNonGIS:=lNonGISType), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' If this is a SPECIAL object (i.e. Disclosures or CLaims) , exit
        If (lNonGISType <> GISDataModelType.GISOTRisk) And (lNonGISType <> GISDataModelType.GISOTNonGisSpecials) Then
            Return result
        End If

        sParamDecs = New StringBuilder("@gis_xml_store_cnt INTEGER ," & Strings.ChrW(13) & Strings.ChrW(10))
        sParamDecs.Append("@object_count INTEGER OUTPUT")
        If v_bQuoteObject Then
            sParamDecs.Append(" ," & Strings.ChrW(13) & Strings.ChrW(10))
            sParamDecs.Append("@quote_key VARCHAR(10)")
        End If
        sVarDecs = New StringBuilder("DECLARE @xml VARCHAR(MAX)" & Strings.ChrW(13) & Strings.ChrW(10))
        sVarDecs.Append("DECLARE @ptrval VARBINARY(16)")
        If v_bTopLevelObject Then
            sVarDecs.Append(Strings.ChrW(13) & Strings.ChrW(10))
            sVarDecs.Append("DECLARE @gis_xml_store_cnt INTEGER")
            If v_bQuoteObject Then
                sVarDecs.Append(Strings.ChrW(13) & Strings.ChrW(10))
                sVarDecs.Append("DECLARE @quote_key VARCHAR(10)")
            End If
        End If

        ' Loop Round the Properties

        For lRow As Integer = vPropertyArray.GetLowerBound(1) To vPropertyArray.GetUpperBound(1)


            sPropertyName = CStr(vPropertyArray(0, lRow))

            lReturn = CType(GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, r_lDataType:=lDataType, r_sColumnName:=sColumnName, r_iIsPrimaryKey:=iIsPrimaryKey, r_lGISPropertyID:=lGISPropertyID), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            'sj 04/05/2001 - start
            sColumnName = sColumnName.Trim()
            'sj 04/05/2001 - end

            '        ' Work Out the data Type
            '        If (iIsPrimaryKey = PMTrue) Then
            '            ' If its a Primary Key field then it must be an INTEGER
            '            sSQLType = "INTEGER"
            '        Else
            '            ' Otherwise, use the data type from the gis_property
            '            Select Case lDataType
            '              Case GISDataTypeDate
            '                sSQLType = "DATETIME"
            '                Case GISDataTypeNumeric, GISDataTypeNumericOutput
            '                sSQLType = "NUMERIC(19,4)"
            '              Case GISDataTypeShortList, GISDataTypeLongList, GISDataTypeText
            '                sSQLType = "VARCHAR(255)"
            '              Case GISDataTypeShortListCode, GISDataTypeLongListCode
            '                sSQLType = "VARCHAR(70)"
            '              Case GISDataTypeOption
            '                sSQLType = "TINYINT"
            '              Case GISDataTypeCurrency
            '                sSQLType = "NUMERIC(19,4)"
            '              Case GISDataTypePercentage
            '                sSQLType = "NUMERIC(19,4)"
            '              Case Else
            '            End Select
            '        End If
            sSQLType = GetSQLType(lDataType)

            If iIsPrimaryKey = gPMConstants.PMEReturnCode.PMTrue Then

                'SJ 15/07/2004 - start
                'Force the datatype to "INT" for all primary keys
                sSQLType = "INT"
                'SJ 15/07/2004 - end

                ' Build the Where Clause
                If (sTableName.Trim().ToUpper() & "_ID") = (sColumnName.Trim().ToUpper()) Then
                    sOrderBy = " ORDER BY " & sColumnName & " ASC"
                    sPKColumn = sColumnName
                Else
                    If sWhere.ToString().Trim() <> "" Then
                        sWhere.Append(Strings.ChrW(13) & Strings.ChrW(10) & "AND ")
                    Else
                        sWhere = New StringBuilder(Strings.ChrW(13) & Strings.ChrW(10) & "WHERE ")
                    End If
                    sWhere.Append(sColumnName & " = @" & sColumnName)

                    ' Build the Parameter Declaration
                    If sParamDecs.ToString().Trim() <> "" Then
                        sParamDecs.Append(" , " & Strings.ChrW(13) & Strings.ChrW(10))
                    End If

                    ' Parameter Delcaration
                    sParamDecs.Append(" @" & sColumnName.Trim() & "    " & sSQLType)

                End If

                If sSPExec.ToString().Trim() <> "" Then
                    sSPExec.Append(" , " & Strings.ChrW(13) & Strings.ChrW(10))
                End If
                sSPExec.Append("            @" & sColumnName & " = @" & sColumnName)

            End If

            If (iIsPrimaryKey <> gPMConstants.PMEReturnCode.PMTrue) Or (sTableName.Trim().ToUpper() & "_ID") = (sColumnName.Trim().ToUpper()) Then

                If sVarDecs.ToString().Trim() <> "" Then
                    sVarDecs.Append(Strings.ChrW(13) & Strings.ChrW(10))
                End If

                ' Do not need Policy Link ID for the Top Level Object as it is a Parameter
                If (sColumnName.Trim() = "gis_policy_link_id") And (v_bTopLevelObject) Then
                Else
                    ' Start the Variable Delcaration
                    sVarDecs.Append("DECLARE @" & sColumnName.Trim() & "    " & sSQLType)
                End If

                If sSelect.ToString().Trim() <> "" Then
                    sSelect.Append(" , " & Strings.ChrW(13) & Strings.ChrW(10) & "    ")
                Else
                    sSelect = New StringBuilder("SELECT ")
                End If
                sSelect.Append("    " & sColumnName)

                If sInto.ToString().Trim() <> "" Then
                    sInto.Append(" , " & Strings.ChrW(13) & Strings.ChrW(10) & "    ")
                Else
                    sInto = New StringBuilder("INTO")
                End If

                sInto.Append("    @" & sColumnName)

            End If

            If iIsPrimaryKey = gPMConstants.PMEReturnCode.PMTrue Then

                If sPKXMLBuild.ToString().Trim() <> "" Then
                    sPKXMLBuild.Append(Strings.ChrW(13) & Strings.ChrW(10) & "        ")
                Else
                    sPKXMLBuild = New StringBuilder("        ")
                End If

                sPKXMLBuild.Append("IF @" & sColumnName & " IS NOT NULL" & Strings.ChrW(13) & Strings.ChrW(10))

                If sColumnName = "gis_policy_link_id" Then

                    sPKXMLBuild.Append("            SELECT @xml = @xml + " & Strings.ChrW(39).ToString() & " " & sPropertyName.ToUpper() & Strings.ChrW(39).ToString())
                    sPKXMLBuild.Append(" + " & Strings.ChrW(39).ToString() & "=" & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & " + CAST(@" & sColumnName & " AS VARCHAR(255)) + " & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString())

                Else

                    sPKXMLBuild.Append("            SELECT @xml = @xml + " & Strings.ChrW(39).ToString() & " " & sPropertyName.ToUpper() & Strings.ChrW(39).ToString())
                    sPKXMLBuild.Append(" + " & Strings.ChrW(39).ToString() & "=" & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & " + CAST(@" & sColumnName & " AS VARCHAR(255)) + " & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString())

                End If

            Else

                If sXMLBuild.ToString().Trim() <> "" Then
                    sXMLBuild.Append(Strings.ChrW(13) & Strings.ChrW(10) & "        ")
                Else
                    sXMLBuild = New StringBuilder("        ")
                End If

                sXMLBuild.Append("IF @" & sColumnName & " IS NOT NULL" & Strings.ChrW(13) & Strings.ChrW(10))

                If lDataType = iGISSharedConstants.GISDataTypeDate Then
                    'SELECT @xml = @xml + " stock_declaration_date" + "=""" + CONVERT(VARCHAR(20),@stock_declaration_date,120) + """"
                    sXMLBuild.Append("            SELECT @xml = @xml + " & Strings.ChrW(39).ToString() & " " & sPropertyName.ToUpper() & Strings.ChrW(39).ToString() & " + " & Strings.ChrW(39).ToString() & "=" & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & " + CONVERT(VARCHAR(20),@" & sColumnName & ",120)" & " + " & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString())


                Else

                    If sColumnName = "gis_policy_link_id" Then

                        sXMLBuild.Append("            SELECT @xml = @xml + " & Strings.ChrW(39).ToString() & " " & sPropertyName.ToUpper() & Strings.ChrW(39).ToString())
                        sXMLBuild.Append(" + " & Strings.ChrW(39).ToString() & "=" & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & " + CAST(@" & sColumnName & " AS VARCHAR(255)) + " & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString())

                    Else

                        ' If this is a Character Data Type
                        Select Case lDataType
                            Case iGISSharedConstants.GISDataTypeShortList, iGISSharedConstants.GISDataTypeLongList, iGISSharedConstants.GISDataTypeText, iGISSharedConstants.GISDataTypeComment, iGISSharedConstants.GISDataTypeShortListCode, iGISSharedConstants.GISDataTypeLongListCode
                                ' Build up the replace string
                                sXMLBuild.Append("            BEGIN" & Strings.ChrW(13) & Strings.ChrW(10))
                                sXMLBuild.Append("            SELECT @" & sColumnName & " = REPLACE(@" & sColumnName & "," & Strings.ChrW(39).ToString() & "&" & Strings.ChrW(39).ToString() & "," & Strings.ChrW(39).ToString() & "&amp;" & Strings.ChrW(39).ToString() & ")" & Strings.ChrW(13) & Strings.ChrW(10))
                                sXMLBuild.Append("            SELECT @" & sColumnName & " = REPLACE(@" & sColumnName & "," & Strings.ChrW(39).ToString() & "<" & Strings.ChrW(39).ToString() & "," & Strings.ChrW(39).ToString() & "&lt;" & Strings.ChrW(39).ToString() & ")" & Strings.ChrW(13) & Strings.ChrW(10))
                                sXMLBuild.Append("            SELECT @" & sColumnName & " = REPLACE(@" & sColumnName & "," & Strings.ChrW(39).ToString() & ">" & Strings.ChrW(39).ToString() & "," & Strings.ChrW(39).ToString() & "&gt;" & Strings.ChrW(39).ToString() & ")" & Strings.ChrW(13) & Strings.ChrW(10))
                                sXMLBuild.Append("            SELECT @" & sColumnName & " = REPLACE(@" & sColumnName & "," & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & "," & Strings.ChrW(39).ToString() & "&quot;" & Strings.ChrW(39).ToString() & ")" & Strings.ChrW(13) & Strings.ChrW(10))
                                sXMLBuild.Append("            SELECT @" & sColumnName & " = REPLACE(@" & sColumnName & "," & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & "," & Strings.ChrW(39).ToString() & "&apos;" & Strings.ChrW(39).ToString() & ")" & Strings.ChrW(13) & Strings.ChrW(10))
                            Case Else
                        End Select

                        sXMLBuild.Append("            SELECT @xml = @xml + " & Strings.ChrW(39).ToString() & " " & sPropertyName.ToUpper() & Strings.ChrW(39).ToString())

                        'need to cast if they are not strings
                        'sj 15/07/2004 - Add GISDataTypePercentage, GISDataTypeOption and GISDataTypeInteger
                        Select Case lDataType
                            Case iGISSharedConstants.GISDataTypeNumeric, iGISSharedConstants.GISDataTypeNumericOutput, iGISSharedConstants.GISDataTypeCurrency, iGISSharedConstants.GISDataTypePercentage, iGISSharedConstants.GISDataTypeOption, iGISSharedConstants.GISDataTypeInteger
                                sXMLBuild.Append(" + " & Strings.ChrW(39).ToString() & "=" & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & " + CAST(@" & sColumnName & " AS VARCHAR(255)) + " & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString())

                            Case Else
                                sXMLBuild.Append(" + " & Strings.ChrW(39).ToString() & "=" & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & " + @" & sColumnName & " + " & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString())
                        End Select

                        ' If this is a Character Data Type
                        Select Case lDataType
                            Case iGISSharedConstants.GISDataTypeShortList, iGISSharedConstants.GISDataTypeLongList, iGISSharedConstants.GISDataTypeText, iGISSharedConstants.GISDataTypeComment, iGISSharedConstants.GISDataTypeShortListCode, iGISSharedConstants.GISDataTypeLongListCode
                                ' Build up the replace string
                                sXMLBuild.Append("            END" & Strings.ChrW(13) & Strings.ChrW(10))
                            Case Else
                        End Select

                    End If

                End If

            End If

        Next lRow

        ' Build the Cursor Name
        sCursorName = "c_" & sTableName & "_sel"
        ' Build the SP Name
        sSPName = "spg_" & sTableName.ToLower() & "_sel"
        ' Build the SP File Name
        sSPFileName = sSPName & ".sql"

        ' RDC 10072001 START
        lReturn = CType(iGISSharedConstants.GetLoadSPPath(GISDataModelCode, sLoadSPPath), gPMConstants.PMEReturnCode)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' check path exists, create if not
        sPathTest = FileSystem.Dir(sLoadSPPath, FileAttribute.Directory)

        If sPathTest = "" Then
            Directory.CreateDirectory(sLoadSPPath)
        End If

        ' Get the Next File Number
        iFileNum = FileSystem.FreeFile()

        FileSystem.FileOpen(iFileNum, sLoadSPPath & "\" & sSPFileName, OpenMode.Output)
        ' RDC 10072001 END

        ' Write out the Stored Procedure
        FileSystem.PrintLine(iFileNum, "CREATE PROCEDURE " & sSPName)
        If v_bTopLevelObject Then
            FileSystem.PrintLine(iFileNum, "@gis_policy_link_id INTEGER ,")
            If v_bQuoteObject Then
                FileSystem.PrintLine(iFileNum, "@object_count INTEGER OUTPUT ,")
                FileSystem.PrintLine(iFileNum, "@quote_count INTEGER OUTPUT")
            Else
                FileSystem.PrintLine(iFileNum, "@object_count INTEGER OUTPUT")
            End If
        Else
            FileSystem.PrintLine(iFileNum, sParamDecs.ToString())
        End If
        FileSystem.PrintLine(iFileNum)
        FileSystem.PrintLine(iFileNum, "AS")
        FileSystem.PrintLine(iFileNum, "BEGIN")
        FileSystem.PrintLine(iFileNum)


        'FileSystem.PrintLine(iFileNum, "SET QUOTED_IDENTIFIER OFF")

        FileSystem.PrintLine(iFileNum, sVarDecs.ToString())
        FileSystem.PrintLine(iFileNum)
        FileSystem.PrintLine(iFileNum, "DECLARE " & sCursorName & " CURSOR FAST_FORWARD FOR")
        FileSystem.PrintLine(iFileNum, sSelect.ToString())
        FileSystem.PrintLine(iFileNum, "FROM " & sTableName)
        If v_bTopLevelObject Then
            FileSystem.PrintLine(iFileNum, "WHERE gis_policy_link_id = @gis_policy_link_id")
        Else
            FileSystem.PrintLine(iFileNum, sWhere.ToString())
        End If
        FileSystem.PrintLine(iFileNum, sOrderBy)
        FileSystem.PrintLine(iFileNum)
        If v_bTopLevelObject Then
            FileSystem.PrintLine(iFileNum, "SET NOCOUNT ON")
            FileSystem.PrintLine(iFileNum, "INSERT INTO GIS_Xml_Store (xml_data) values ('x')")
            FileSystem.PrintLine(iFileNum, "SELECT @gis_xml_store_cnt = @@IDENTITY")
        End If
        FileSystem.PrintLine(iFileNum)
        FileSystem.PrintLine(iFileNum, "OPEN " & sCursorName)
        FileSystem.PrintLine(iFileNum)
        FileSystem.PrintLine(iFileNum, "FETCH NEXT FROM " & sCursorName)
        FileSystem.PrintLine(iFileNum, sInto.ToString())
        FileSystem.PrintLine(iFileNum)
        If v_bTopLevelObject Then
            FileSystem.PrintLine(iFileNum, "    SELECT @ptrval = TEXTPTR(xml_data) FROM GIS_XML_STORE where gis_xml_store_cnt = @gis_xml_store_cnt")
            FileSystem.PrintLine(iFileNum, "    WRITETEXT GIS_XML_STORE.xml_data @ptrval " & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString())
            FileSystem.PrintLine(iFileNum)
        End If
        FileSystem.PrintLine(iFileNum, "    SELECT @xml = ''")
        FileSystem.PrintLine(iFileNum)
        FileSystem.PrintLine(iFileNum, "    WHILE (@@FETCH_STATUS = 0)")
        FileSystem.PrintLine(iFileNum, "    BEGIN")
        FileSystem.PrintLine(iFileNum)
        If v_bQuoteObject And v_bTopLevelObject Then
            FileSystem.PrintLine(iFileNum, "        SELECT @quote_count = @quote_count  + 1")
        End If
        ' RFC310701 - Use the PK Value rather than just count the number of objects,
        '             as you have to cater for deleted objects
        FileSystem.PrintLine(iFileNum, "         IF @" & sPKColumn & " > @object_count")
        FileSystem.PrintLine(iFileNum, "             SELECT @object_count = @" & sPKColumn)
        If v_bTopLevelObject And v_bQuoteObject Then
            FileSystem.PrintLine(iFileNum)
            FileSystem.PrintLine(iFileNum, "        SELECT @quote_key = " & Strings.ChrW(39).ToString() & "S" & Strings.ChrW(39).ToString() & " + CAST(@gis_scheme_id AS VARCHAR(255))")
            FileSystem.PrintLine(iFileNum)
            FileSystem.PrintLine(iFileNum, "        SELECT @xml = @xml + " & Strings.ChrW(39).ToString() & "<QUOTE_OBJECTS SchemeID=" & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() &
                                     " + CAST(@gis_scheme_id AS VARCHAR(255)) + " & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & " OI=" & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & " + @quote_key + " & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & ">" & Strings.ChrW(39).ToString())
        End If
        FileSystem.PrintLine(iFileNum)
        ' RFC071103 - Avoid Clashing OI Values where the gis policy link id value is small (e.g. in a clean database)
        If v_bQuoteObject Then 'PN18822
            FileSystem.PrintLine(iFileNum, "        SELECT @xml = @xml + " & Strings.ChrW(39).ToString() & "<" & v_sObjectName.Trim() & " OI=" & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & " + " & "@quote_key" & " + " & Strings.ChrW(39).ToString() & "_OI" & Strings.ChrW(39).ToString() & " + " & "CAST(@" & sPKColumn & " AS VARCHAR(255)) + " & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & " + " & Strings.ChrW(39).ToString() & " US=" & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & "0" & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString())
        ElseIf (v_bTopLevelObject) Then
            FileSystem.PrintLine(iFileNum, "        SELECT @xml = @xml + " & Strings.ChrW(39).ToString() & "<" & v_sObjectName.Trim() & " OI=" & Strings.ChrW(39).ToString() & " + " & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & "OI" & Strings.ChrW(39).ToString() & " + " & "CAST(@" & sPKColumn & " + " & CStr(iGISSharedConstants.GISPolicyBinderOIOffset) & " AS VARCHAR(255)) + " & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & " + " & Strings.ChrW(39).ToString() & " US=" & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & "0" & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString())
        Else
            FileSystem.PrintLine(iFileNum, "        SELECT @xml = @xml + " & Strings.ChrW(39).ToString() & "<" & v_sObjectName.Trim() & " OI=" & Strings.ChrW(39).ToString() & " + " & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & "OI" & Strings.ChrW(39).ToString() & " + " & "CAST(@" & sPKColumn & " AS VARCHAR(255)) + " & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & " + " & Strings.ChrW(39).ToString() & " US=" & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & "0" & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString() & Strings.ChrW(39).ToString())
        End If
        FileSystem.PrintLine(iFileNum)
        FileSystem.PrintLine(iFileNum, sPKXMLBuild.ToString())
        FileSystem.PrintLine(iFileNum, sXMLBuild.ToString())
        FileSystem.PrintLine(iFileNum)
        FileSystem.PrintLine(iFileNum, "    SELECT @xml = @xml + " & Strings.ChrW(39).ToString() & ">" & Strings.ChrW(39).ToString())
        FileSystem.PrintLine(iFileNum)
        FileSystem.PrintLine(iFileNum, "    SELECT @ptrval = TEXTPTR(xml_data) FROM GIS_XML_STORE where gis_xml_store_cnt = @gis_xml_store_cnt")
        FileSystem.PrintLine(iFileNum, "    UPDATETEXT GIS_XML_STORE.xml_data @ptrval Null Null @xml")
        FileSystem.PrintLine(iFileNum)

        ' Exec the SP for Each Child Table
        If Informations.IsArray(vChildObjectArray) Then

            For lRow As Integer = vChildObjectArray.GetLowerBound(0) To vChildObjectArray.GetUpperBound(0)

                sChildObjectName = CStr(vChildObjectArray(lRow)).Trim()

                'sj 04/05/2001 - start
                lReturn = CType(GetObjectDefDetails(v_sObjectName:=sChildObjectName, r_sTableName:=sChildTableName, r_lIsNonGIS:=lNonGISType), gPMConstants.PMEReturnCode)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

                ' Exclude Special Objects
                If (lNonGISType = GISDataModelType.GISOTRisk) Or (lNonGISType = GISDataModelType.GISOTNonGisSpecials) Then

                    'Print #iFileNum, "        EXEC " & "spg_" & sChildObjectName & "_sel"
                    FileSystem.PrintLine(iFileNum, "        EXEC " & "spg_" & sChildTableName & "_sel")
                    'sj 04/05/2001 - end
                    FileSystem.PrintLine(iFileNum, "            @gis_xml_store_cnt=@gis_xml_store_cnt,")
                    FileSystem.PrintLine(iFileNum, "            @object_count=@object_count OUTPUT ,")
                    If v_bQuoteObject Then
                        FileSystem.PrintLine(iFileNum, "            @quote_key=@quote_key ,")
                    End If
                    FileSystem.PrintLine(iFileNum, sSPExec.ToString())
                End If
            Next lRow
        End If
        FileSystem.PrintLine(iFileNum)
        FileSystem.PrintLine(iFileNum, "        SELECT @xml = " & Strings.ChrW(39).ToString() & "</" & v_sObjectName.Trim() & ">" & Strings.ChrW(39).ToString())
        If v_bTopLevelObject And v_bQuoteObject Then
            FileSystem.PrintLine(iFileNum)
            FileSystem.PrintLine(iFileNum, "        SELECT @xml = @xml + " & Strings.ChrW(39).ToString() & "</QUOTE_OBJECTS>" & Strings.ChrW(39).ToString())
        End If
        FileSystem.PrintLine(iFileNum)
        FileSystem.PrintLine(iFileNum, "    SELECT @ptrval = TEXTPTR(xml_data) FROM GIS_XML_STORE where gis_xml_store_cnt = @gis_xml_store_cnt")
        FileSystem.PrintLine(iFileNum, "    UPDATETEXT GIS_XML_STORE.xml_data @ptrval Null Null @xml")
        FileSystem.PrintLine(iFileNum, "    SELECT @xml = ''")
        FileSystem.PrintLine(iFileNum)
        FileSystem.PrintLine(iFileNum)
        FileSystem.PrintLine(iFileNum, "        " & "FETCH NEXT FROM " & sCursorName)
        FileSystem.PrintLine(iFileNum, "        " & sInto.ToString())
        FileSystem.PrintLine(iFileNum, "    END")
        FileSystem.PrintLine(iFileNum)
        FileSystem.PrintLine(iFileNum, "    CLOSE " & sCursorName)
        FileSystem.PrintLine(iFileNum, "    DEALLOCATE " & sCursorName)
        FileSystem.PrintLine(iFileNum)
        If v_bTopLevelObject Then
            FileSystem.PrintLine(iFileNum, "    SET NOCOUNT OFF")
            FileSystem.PrintLine(iFileNum, "    SELECT xml_data from gis_xml_store WHERE gis_xml_store_cnt = @gis_xml_store_cnt")
            FileSystem.PrintLine(iFileNum)
            FileSystem.PrintLine(iFileNum, "    SET NOCOUNT ON")
            FileSystem.PrintLine(iFileNum, "    DELETE FROM gis_xml_store WHERE gis_xml_store_cnt = @gis_xml_store_cnt")
            FileSystem.PrintLine(iFileNum)
        End If
        ' developer guide no. 
        'FileSystem.PrintLine(iFileNum, "SET QUOTED_IDENTIFIER OFF")
        'FileSystem.PrintLine(iFileNum)
        FileSystem.PrintLine(iFileNum, "END")

        FileSystem.FileClose(iFileNum) ' Close file.

        ' Build the SP for Each Child Table
        If Informations.IsArray(vChildObjectArray) Then

            For lRow As Integer = vChildObjectArray.GetLowerBound(0) To vChildObjectArray.GetUpperBound(0)

                sChildObjectName = CStr(vChildObjectArray(lRow)).Trim()
                lReturn = CType(BuildSelectSP(sChildObjectName, False, v_bQuoteObject), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If
            Next lRow
        End If

        Return result

    End Function

    ' RFC240101
    ' ***************************************************************** '
    ' Name: BuildCUDSP
    '
    ' Description: Build an Insert, Update, Delete Stored Procedure
    '              for the given Object.
    '
    ' ***************************************************************** '
    Private Function BuildCUDSP(ByVal v_sObjectName As String, ByVal v_bTopLevelObject As Boolean, ByVal v_bQuoteObject As Boolean) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        ' Object Definition
        Dim lIsQuoteObject As gPMConstants.PMEReturnCode
        Dim lGISObjectID, lGISPropertyID As Integer
        Dim sTableName As String = ""
        Dim lMaxInstances, lPolarisObjectID As Integer
        Dim sParentObjectName As String = ""
        Dim vChildObjectArray As Object = Nothing
        Dim vPropertyArray(,) As Object = Nothing
        Dim sPropertyName As String = ""
        Dim sColumnName As String = ""
        Dim iIsPrimaryKey As gPMConstants.PMEReturnCode
        Dim sParentTableName As String = ""

        Dim sOIKey As String = ""
        Dim sParentOIKey As String = ""

        Dim sSelectSQL As String = ""

        Dim lDataType As Integer

        ' Used to Create the SP
        Dim iFileNum As Integer
        Dim sSPName As String = ""
        Dim sSPFileName As String = ""
        Dim sSQLType As String = ""

        ' Output Strings
        Dim sReplace As String = ""
        Dim sChildObjectName As String = ""
        Dim sParamDecs As New StringBuilder

        ' RDC 10072001 LoadSP path
        Dim sLoadSPPath As String = ""
        Dim sPathTest As String = ""

        Dim sDelSQL1 As String = ""
        Dim sUpdSQL1 As New StringBuilder
        Dim sDelSQL2 As New StringBuilder
        Dim sUpdSQL2 As New StringBuilder
        Dim sAddSQL2 As New StringBuilder
        Dim sAddSQL1 As New StringBuilder

        Dim sAddSQL As String = ""
        Dim sUpdSQL As String = ""
        Dim sDelSQL As String = ""
        Dim lNonGISType As Integer = 0
        Dim lRiskColCount As Integer
        Dim sRiskColumnName As String = ""
        Dim sRiskSQLType As String = ""
        Dim sRiskUpdSQL As String = ""
        Dim sRiskUpdSQL2 As New StringBuilder
        Dim sRiskUpdSQL1 As New StringBuilder


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the Object Definition Details
        lReturn = CType(GetObjectDefDetails(v_sObjectName:=v_sObjectName, r_lIsQuoteObject:=lIsQuoteObject, r_lGISObjectID:=lGISObjectID, r_sTableName:=sTableName, r_lMaxInstances:=lMaxInstances, r_lPolarisObjectID:=lPolarisObjectID, r_sParentObjectName:=sParentObjectName, r_vChildObjectArray:=vChildObjectArray, r_vPropertyArray:=vPropertyArray, r_sSelectSQL:=sSelectSQL, r_lIsNonGIS:=lNonGISType), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' If this is a SPECIAL object (i.e. Disclosures or CLaims) , exit
        If (lNonGISType <> GISDataModelType.GISOTRisk) And (lNonGISType <> GISDataModelType.GISOTNonGisSpecials) Then
            Return result
        End If

        sAddSQL1 = New StringBuilder("")
        sAddSQL2 = New StringBuilder("")
        sUpdSQL1 = New StringBuilder("")
        sUpdSQL2 = New StringBuilder("")
        sDelSQL1 = ""
        sDelSQL2 = New StringBuilder("")
        sRiskUpdSQL1 = New StringBuilder("") ' RAW 03/07/2003 : CQ1581 : added
        sRiskUpdSQL2 = New StringBuilder("") ' RAW 03/07/2003 : CQ1581 : added




        sParamDecs = New StringBuilder("@US INTEGER" & Strings.ChrW(13) & Strings.ChrW(10))
        'sParamDecs = sParamDecs & "@OI VARCHAR(255)"
        sReplace = ""
        ' Loop Round the Properties

        For lRow As Integer = vPropertyArray.GetLowerBound(1) To vPropertyArray.GetUpperBound(1)


            sPropertyName = CStr(vPropertyArray(0, lRow))

            lReturn = CType(GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, r_lDataType:=lDataType, r_sColumnName:=sColumnName, r_iIsPrimaryKey:=iIsPrimaryKey, r_lGISPropertyID:=lGISPropertyID), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            'sj 04/05/2001 - start
            sColumnName = sColumnName.Trim()
            'sj 04/05/2001 - end

            ' Work Out the data Type
            If iIsPrimaryKey = gPMConstants.PMEReturnCode.PMTrue Then
                ' If its a Primary Key field then it must be an INTEGER
                sSQLType = "INTEGER"
            Else
                ' Otherwise, use the data type from the gis_property
                sSQLType = GetSQLType(lDataType)
            End If



            ' RAW 03/07/2003 : CQ1581 : added
            sRiskColumnName = ""

            ' does the Risk table also need updating
            If lIsQuoteObject <> gPMConstants.PMEReturnCode.PMTrue Then
                ' This is a Risk related object
                ' RAW 21/06/2004 : CQ3761 : use property name instead of column name
                If IsRiskStaticColumn(v_sParentObjectName:=sParentObjectName, v_sPropertyName:=sPropertyName) Then
                    ' this column also exists in the Risk table
                    lRiskColCount += 1
                    ' default to the same def as the equivalent GIS column
                    sRiskColumnName = sColumnName
                    sRiskSQLType = sSQLType

                    ' if column name or sql data type differs between risk table and GIS table then handle it here
                    Select Case sColumnName.ToUpper()
                        Case "ACCUMULATION"
                            sRiskColumnName = "accumulation_id"
                    End Select

                End If
            End If
            ' RAW 03/07/2003 : CQ1581 : End


            ' Build the Parameter Declaration
            If sParamDecs.ToString().Trim() <> "" Then
                sParamDecs.Append(" , " & Strings.ChrW(13) & Strings.ChrW(10))
            End If

            ' Parameter Delcaration
            sParamDecs.Append(" @" & sColumnName.Trim() & "    " & sSQLType)
            If iIsPrimaryKey <> gPMConstants.PMEReturnCode.PMTrue Then
                sParamDecs.Append(" = NULL")
            End If

            ' Do not need this as the XSL will un-escape the &amp; etc into & etc
            '        ' If this is a Character Data Type
            '        Select Case lDataType
            '          Case GISDataTypeShortList, GISDataTypeLongList, GISDataTypeText, GISDataTypeShortListCode, GISDataTypeLongListCode
            '            ' Build up the replace string
            '            sReplace = sReplace & "SELECT " & sColumnName & " = REPLACE(@" & sColumnName & "," & Chr(34) & "&amp;" & Chr(34) & "," & Chr(34) & "&" & Chr(34) & ")" & vbCrLf
            '            sReplace = sReplace & "SELECT " & sColumnName & " = REPLACE(@" & sColumnName & "," & Chr(34) & "&lt;" & Chr(34) & "," & Chr(34) & "<" & Chr(34) & ")" & vbCrLf
            '            sReplace = sReplace & "SELECT " & sColumnName & " = REPLACE(@" & sColumnName & "," & Chr(34) & "&gt;" & Chr(34) & "," & Chr(34) & ">" & Chr(34) & ")" & vbCrLf
            '            sReplace = sReplace & "SELECT " & sColumnName & " = REPLACE(@" & sColumnName & "," & Chr(34) & "&quot;" & Chr(34) & "," & Chr(34) & Chr(34) & Chr(34) & Chr(34) & ")" & vbCrLf
            '            sReplace = sReplace & "SELECT " & sColumnName & " = REPLACE(@" & sColumnName & "," & Chr(34) & "&apos;" & Chr(34) & "," & Chr(34) & "'" & Chr(34) & ")" & vbCrLf
            '          Case Else
            '        End Select

            ' Build the Column List

            sAddSQL1.Append(sColumnName)

            If lDataType = iGISSharedConstants.GISDataTypeDate Then
                sAddSQL2.Append("CONVERT(datetime," & ACSQLParamPrefix & sColumnName & ",120)")
            Else
                sAddSQL2.Append(ACSQLParamPrefix & sColumnName)
            End If


            If lRow < vPropertyArray.GetUpperBound(1) Then
                sAddSQL1.Append(ACSQLSeparator)
                sAddSQL2.Append(ACSQLSeparator)
            End If


            If iIsPrimaryKey = gPMConstants.PMEReturnCode.PMTrue Then
                If (sColumnName).ToUpper() <> "UID" Then
                    If sUpdSQL2.ToString() <> "" Then
                        sUpdSQL2.Append(ACSQLAnd)
                    End If

                    sUpdSQL2.Append(sColumnName & ACSQLEquals & ACSQLParamPrefix & sColumnName)
                End If
                'RFC200400 - Add Proper Delete Functionality
                If sDelSQL2.ToString() <> "" Then
                    sDelSQL2.Append(ACSQLAnd)
                End If
                sDelSQL2.Append(sColumnName & ACSQLEquals & ACSQLParamPrefix & sColumnName)

            Else
                If (sColumnName).ToUpper() <> "UID" Then
                    If sUpdSQL1.ToString() <> "" Then
                        sUpdSQL1.Append(ACSQLSeparator)
                    End If

                    If lDataType = iGISSharedConstants.GISDataTypeDate Then
                        sUpdSQL1.Append(sColumnName & ACSQLEquals & "CONVERT(datetime," & ACSQLParamPrefix & sColumnName & ",120)")
                    Else
                        sUpdSQL1.Append(sColumnName & ACSQLEquals & ACSQLParamPrefix & sColumnName)
                    End If
                End If

            End If

            ' RAW 03/07/2003 : CQ1581 : added
            If sRiskColumnName <> "" Then
                If sRiskUpdSQL1.ToString() <> "" Then
                    sRiskUpdSQL1.Append(ACSQLSeparator)
                End If

                If sRiskSQLType = "DATETIME" Then
                    sRiskUpdSQL1.Append(sRiskColumnName & ACSQLEquals & "CONVERT(datetime," & ACSQLParamPrefix & sColumnName & ",120)")
                Else
                    If sRiskColumnName = "accumulation_id" Then
                        ' default to null if 0
                        sRiskUpdSQL1.Append(sRiskColumnName & ACSQLEquals &
                                            "(CASE " &
                                            "WHEN " & ACSQLParamPrefix & sColumnName & " = 0 THEN NULL " &
                                            "ELSE " & ACSQLParamPrefix & sColumnName & " " &
                                                "END)")
                    Else
                        sRiskUpdSQL1.Append(sRiskColumnName & ACSQLEquals & ACSQLParamPrefix & sColumnName)
                    End If
                End If

                ' always use risk_cnt to locate the Risk row to update
                If lRiskColCount = 1 Then
                    ' only set this once for the first risk column detected
                    sRiskUpdSQL2.Append("risk_cnt = @iRiskCnt")
                End If
            End If
            ' RAW 03/07/2003 : CQ1581 : end

        Next lRow

        ' Build the SQL Strings depending on the type of Command

        ' INSERT INTO tablename (col1, col2....)
        sAddSQL1 = New StringBuilder(ACAddSQLStart & sTableName.Trim() & ACSQLStartCol & sAddSQL1.ToString() & ACSQLEndCol)

        ' VALUES (col1, col2....)
        sAddSQL2 = New StringBuilder(ACAddSQLValues & ACSQLStartCol & sAddSQL2.ToString() & ACSQLEndCol)


        ' UPDATE tablename SET col1 = {col1}, col2 = {col2}....
        If sUpdSQL1.ToString() <> "" Then
            sUpdSQL1 = New StringBuilder(ACUpdSQLStart & sTableName.Trim() & ACUpdSQLSet & sUpdSQL1.ToString())

            ' WHERE pkcol1 = {pkcol1}, pkcol2 = {pkcol2}....
            sUpdSQL2 = New StringBuilder(ACSQLWhere & sUpdSQL2.ToString())
        End If

        'RFC200400 - Add Proper Delete Functionality
        sDelSQL1 = ACDelSQLDeleteFrom & sTableName.Trim() & ACDelSQLWhere

        ' Build the complete sql statements for each action
        sAddSQL = sAddSQL1.ToString() & " " & sAddSQL2.ToString()
        If sUpdSQL1.ToString() <> "" Then
            sUpdSQL = sUpdSQL1.ToString() & " " & sUpdSQL2.ToString()
        End If

        sDelSQL = sDelSQL1 & sDelSQL2.ToString()

        ' RAW 03/07/2003 : CQ1581 : added
        ' Build the statement to update the risk table
        ' note that this will only work if the risk already exists in the risk table
        If sRiskUpdSQL2.ToString() <> "" Then
            ' UPDATE Risk SET col1 = {col1}, col2 = {col2}....
            sRiskUpdSQL1 = New StringBuilder(ACUpdSQLStart & "Risk" & ACUpdSQLSet & sRiskUpdSQL1.ToString())

            ' WHERE Risk = risk_cnt....
            sRiskUpdSQL2 = New StringBuilder(ACSQLWhere & sRiskUpdSQL2.ToString())

            ' join the two together
            sRiskUpdSQL = sRiskUpdSQL1.ToString() & " " & sRiskUpdSQL2.ToString()
        End If
        ' RAW 03/07/2003 : CQ1581 : end

        ' Build the SP Name
        sSPName = "spg_" & sTableName.ToLower() & "_cud"
        ' Build the SP File Name
        sSPFileName = sSPName & ".sql"

        ' RDC 10072001 START
        lReturn = CType(iGISSharedConstants.GetLoadSPPath(GISDataModelCode, sLoadSPPath), gPMConstants.PMEReturnCode)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' check path exists, create if not
        sPathTest = FileSystem.Dir(sLoadSPPath, FileAttribute.Directory)

        If sPathTest = "" Then
            Directory.CreateDirectory(sLoadSPPath)
        End If

        ' Get the Next File Number
        iFileNum = FileSystem.FreeFile()

        FileSystem.FileOpen(iFileNum, sLoadSPPath & "\" & sSPFileName, OpenMode.Output)
        ' RDC 10072001 END

        ' Write out the Stored Procedure

        ' RAM20040831 : Commented the following lines
        'Print #iFileNum, "SET QUOTED_IDENTIFIER OFF"
        'Print #iFileNum, "IF EXISTS (SELECT * FROM SYSOBJECTS WHERE id = object_id('dbo." & sSPName & "') and sysstat & 0xf = 4)"
        'Print #iFileNum, "    DROP PROCEDURE dbo."; sSPName
        'Print #iFileNum, "GO"
        'Print #iFileNum,

        FileSystem.PrintLine(iFileNum, "CREATE PROCEDURE " & sSPName)
        FileSystem.PrintLine(iFileNum, sParamDecs.ToString())
        FileSystem.PrintLine(iFileNum)
        FileSystem.PrintLine(iFileNum, "AS")
        FileSystem.PrintLine(iFileNum, "BEGIN")
        FileSystem.PrintLine(iFileNum)

        ' RAW 03/07/2003 : CQ1581 : added
        If sRiskUpdSQL <> "" Then
            FileSystem.PrintLine(iFileNum, "    DECLARE @iRiskCnt INTEGER")
            FileSystem.PrintLine(iFileNum)
        End If
        ' RAW 03/07/2003 : CQ1581 : end

        FileSystem.PrintLine(iFileNum, "    IF @US = 1")
        FileSystem.PrintLine(iFileNum, "BEGIN")
        FileSystem.PrintLine(iFileNum, " Select @UID=NEWID() ")
        FileSystem.PrintLine(iFileNum, sReplace)
        FileSystem.PrintLine(iFileNum)
        FileSystem.PrintLine(iFileNum, sAddSQL)
        FileSystem.PrintLine(iFileNum, "END")
        FileSystem.PrintLine(iFileNum)
        If sUpdSQL IsNot Nothing Then
            FileSystem.PrintLine(iFileNum, "    IF @US = 2")
            FileSystem.PrintLine(iFileNum, sUpdSQL)
        End If

        FileSystem.PrintLine(iFileNum)
        FileSystem.PrintLine(iFileNum, "    IF @US = 3")
        FileSystem.PrintLine(iFileNum, sDelSQL)
        FileSystem.PrintLine(iFileNum)

        ' RAW 03/07/2003 : CQ1581 : added
        If sRiskUpdSQL <> "" Then
            ' build sql to update risk table
            FileSystem.PrintLine(iFileNum, "    IF (@US = 1) OR (@US = 2) BEGIN")
            FileSystem.PrintLine(iFileNum)
            FileSystem.PrintLine(iFileNum, "        SELECT DISTINCT")
            FileSystem.PrintLine(iFileNum, "               @iRiskCnt = gpl.risk_id")
            FileSystem.PrintLine(iFileNum, "        FROM   gis_policy_link gpl")
            FileSystem.PrintLine(iFileNum, "        WHERE  gpl.gis_policy_link_id = @" & GISDataModelCode & "_policy_binder_id")
            FileSystem.PrintLine(iFileNum)
            FileSystem.PrintLine(iFileNum, "        IF ( ISNULL(@iRiskCnt,0) > 0 ) BEGIN")
            FileSystem.PrintLine(iFileNum, "            " & sRiskUpdSQL)
            FileSystem.PrintLine(iFileNum, "        END")
            FileSystem.PrintLine(iFileNum, "    END")
            FileSystem.PrintLine(iFileNum)
        End If
        ' RAW 03/07/2003 : CQ1581 : end

        FileSystem.PrintLine(iFileNum, "END")

        ' RAM20040831 : Commented the following lines
        'Print #iFileNum, "GO"

        FileSystem.FileClose(iFileNum) ' Close file.

        ' Build the SP for Each Child Table
        If Informations.IsArray(vChildObjectArray) Then

            For lRow As Integer = vChildObjectArray.GetLowerBound(0) To vChildObjectArray.GetUpperBound(0)

                sChildObjectName = CStr(vChildObjectArray(lRow)).Trim()
                lReturn = CType(BuildCUDSP(sChildObjectName, False, v_bQuoteObject), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If
            Next lRow
        End If

        Return result

    End Function

    ' RFC240101
    ' ***************************************************************** '
    ' Name: BuildClearAllQuoteOutputSP
    '
    ' Description: Build an SP to clear all Quote Output
    '
    ' ***************************************************************** '
    Private Function BuildClearAllQuoteOutputSP(ByVal v_sObjectName As String, ByVal v_bTopLevelObject As Boolean, ByRef r_sSQL As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        ' Object Definition
        Dim sTableName As String = ""
        Dim vChildObjectArray As Object = Nothing
        Dim sColumnName As String = ""
        Dim sDelSQL As String = ""
        ' Used to Create the SP
        Dim iFileNum As Integer
        Dim sSPName As String = ""
        Dim sSPFileName As String = ""

        ' Output Strings
        Dim sChildObjectName As String = ""

        ' RDC 10072001 LoadSP path
        Dim sLoadSPPath As String = ""
        Dim sPathTest As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the Object Definition Details
        lReturn = CType(GetObjectDefDetails(v_sObjectName:=v_sObjectName, r_sTableName:=sTableName, r_vChildObjectArray:=vChildObjectArray), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        If v_bTopLevelObject Then

            ' Build the SP Name
            sSPName = "spg_" & GISDataModelCode.ToLower().Trim() & "_clear_all_quote_output"
            ' Build the SP File Name
            sSPFileName = sSPName & ".sql"

            ' RDC 10072001 START
            lReturn = CType(iGISSharedConstants.GetLoadSPPath(GISDataModelCode, sLoadSPPath), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' check path exists, create if not
            sPathTest = FileSystem.Dir(sLoadSPPath, FileAttribute.Directory)

            If sPathTest = "" Then
                Directory.CreateDirectory(sLoadSPPath)
            End If

            ' Get the Next File Number
            iFileNum = FileSystem.FreeFile()

            FileSystem.FileOpen(iFileNum, sLoadSPPath & "\" & sSPFileName, OpenMode.Output)
            ' RDC 10072001 END

            ' Write out the Stored Procedure
            ' RAM20040831 : Commented the following lines
            'Print #iFileNum, "IF EXISTS (SELECT * FROM SYSOBJECTS WHERE id = object_id('dbo." & sSPName & "') and sysstat & 0xf = 4)"
            'Print #iFileNum, "    DROP PROCEDURE dbo."; sSPName
            'Print #iFileNum, "GO"
            'Print #iFileNum,

            FileSystem.PrintLine(iFileNum, "CREATE PROCEDURE " & sSPName)
            FileSystem.PrintLine(iFileNum, " @gis_policy_link_id INTEGER")
            FileSystem.PrintLine(iFileNum)
            FileSystem.PrintLine(iFileNum, "AS")
            FileSystem.PrintLine(iFileNum, "BEGIN")
            FileSystem.PrintLine(iFileNum)

        End If

        sDelSQL = "    DELETE FROM " & sTableName.ToLower().Trim() & Strings.ChrW(13) & Strings.ChrW(10)
        sDelSQL = sDelSQL & "    WHERE gis_policy_link_id = @gis_policy_link_id" & Strings.ChrW(13) & Strings.ChrW(10)
        r_sSQL = sDelSQL & r_sSQL

        ' Build the SP for Each Child Table
        If Informations.IsArray(vChildObjectArray) Then

            For lRow As Integer = vChildObjectArray.GetLowerBound(0) To vChildObjectArray.GetUpperBound(0)

                sChildObjectName = CStr(vChildObjectArray(lRow)).Trim()
                lReturn = CType(BuildClearAllQuoteOutputSP(v_sObjectName:=sChildObjectName, v_bTopLevelObject:=False, r_sSQL:=r_sSQL), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If
            Next lRow
        End If

        If v_bTopLevelObject Then
            FileSystem.PrintLine(iFileNum, r_sSQL)
            FileSystem.PrintLine(iFileNum)
            FileSystem.PrintLine(iFileNum, "END")

            ' RAM20040831  : Commented the following code
            'Print #iFileNum, "GO"

            FileSystem.FileClose(iFileNum) ' Close file.
        End If


        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: BuildSaveXSL
    '
    ' Description:
    '
    ' History: 27/03/2001 RFC - Created.
    '
    ' ***************************************************************** '
    Private Function BuildSaveXSL(ByVal v_sGisDataModelCode As String, ByVal v_sTopLevelObjectName As String, Optional ByVal v_sTopLevelQuoteObject As String = "") As Integer

        Dim result As Integer = 0
        Dim iFileNum As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sXSLFileName As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the Next File Number
        iFileNum = FileSystem.FreeFile()

        ' Get the File Name for this Data Model Code
        lReturn = CType(iGISSharedConstants.GetSaveXSLFileName(v_sGisDataModelCode:=v_sGisDataModelCode, r_sSaveXSLFileName:=sXSLFileName), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' Open the File
        FileSystem.FileOpen(iFileNum, sXSLFileName, OpenMode.Output)

        ' Start the XSL Stylesheet
        FileSystem.PrintLine(iFileNum, "<?xml version =" & Strings.ChrW(34).ToString() & "1.0" & Strings.ChrW(34).ToString() & "?>")
        FileSystem.PrintLine(iFileNum, "<xsl:stylesheet version=" & Strings.ChrW(34).ToString() & "1.0" & Strings.ChrW(34).ToString() & " xmlns:xsl=" & Strings.ChrW(34).ToString() & "http://www.w3.org/1999/XSL/Transform" & Strings.ChrW(34).ToString() & " xmlns:fo=" & Strings.ChrW(34).ToString() & "http://www.w3.org/1999/XSL/Format" & Strings.ChrW(34).ToString() & ">")
        FileSystem.PrintLine(iFileNum)
        FileSystem.PrintLine(iFileNum, "<xsl:output method=" & Strings.ChrW(34).ToString() & "text" & Strings.ChrW(34).ToString() & "/>")

        FileSystem.PrintLine(iFileNum, "<xsl:template match=" & Strings.ChrW(34).ToString() & "/" & Strings.ChrW(34).ToString() & ">")
        FileSystem.PrintLine(iFileNum, "  <xsl:apply-templates/>")
        FileSystem.PrintLine(iFileNum, "</xsl:template>")
        FileSystem.PrintLine(iFileNum)

        ' Create the XSL for the Objects starting with the Top one.
        lReturn = CType(BuildSaveXSLForObject(v_sObjectName:=v_sTopLevelObjectName, v_iFileNum:=iFileNum), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' If we have been supplied a Top Level Quote Object
        If v_sTopLevelQuoteObject <> "" Then
            ' Create the XSL for the Objects starting with the Top one.
            lReturn = CType(BuildSaveXSLForObject(v_sObjectName:=v_sTopLevelQuoteObject, v_iFileNum:=iFileNum), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

        End If

        ' Closing Styleshet Tag
        FileSystem.PrintLine(iFileNum)
        FileSystem.PrintLine(iFileNum, "</xsl:stylesheet>")

        FileSystem.FileClose(iFileNum) ' Close file.

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: BuildSaveXSLForObject
    '
    ' Description:
    '
    ' History: 27/03/2001 RFC - Created.
    '
    ' ***************************************************************** '
    Private Function BuildSaveXSLForObject(ByVal v_sObjectName As String, ByVal v_iFileNum As Integer) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sPropertyName As String = ""
        Dim lDataType As Integer
        Dim sColumnName As String = ""
        Dim iIsPrimaryKey As Integer
        Dim lGISPropertyID As Integer

        ' Object Definition
        Dim vPropertyArray(,) As Object = Nothing
        Dim sTableName As String = ""
        Dim lIsQuoteObject, lGISObjectID As Integer
        Dim sParentObjectName As String = ""
        Dim vChildObjectArray As Object = Nothing
        Dim sChildObjectName As String = ""

        Dim sDelim_Start As String = ""
        Dim sDelim_End As String = ""
        Dim sFormat As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the Object Definition Details
        lReturn = CType(GetObjectDefDetails(v_sObjectName:=v_sObjectName, r_lIsQuoteObject:=lIsQuoteObject, r_lGISObjectID:=lGISObjectID, r_sTableName:=sTableName, r_sParentObjectName:=sParentObjectName, r_vChildObjectArray:=vChildObjectArray, r_vPropertyArray:=vPropertyArray), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        'Start the Template
        FileSystem.PrintLine(v_iFileNum, "    <xsl:template match=" & Strings.ChrW(34).ToString() & v_sObjectName.Trim().ToUpper() & "[@US != 0]" & Strings.ChrW(34).ToString() & ">")
        FileSystem.PrintLine(v_iFileNum)
        FileSystem.PrintLine(v_iFileNum, "        EXEC spg_" & sTableName & "_cud")
        FileSystem.PrintLine(v_iFileNum, "            @US=<xsl:value-of select=" & Strings.ChrW(34).ToString() & "./@US" & Strings.ChrW(34).ToString() & "/>")
        FileSystem.PrintLine(v_iFileNum, "            <xsl:for-each select = " & Strings.ChrW(34).ToString() & "./@*" & Strings.ChrW(34).ToString() & ">")

        ' For Each Property

        For lRow As Integer = vPropertyArray.GetLowerBound(1) To vPropertyArray.GetUpperBound(1)

            sPropertyName = CStr(vPropertyArray(0, lRow)).Trim()

            ' Get the Property Details
            lReturn = CType(GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, r_lDataType:=lDataType, r_sColumnName:=sColumnName, r_iIsPrimaryKey:=iIsPrimaryKey, r_lGISPropertyID:=lGISPropertyID), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Upercase the Propertyname
            sPropertyName = sPropertyName.Trim().ToUpper()

            ' RFC The default format for the Data is just the value
            sFormat = "<xsl:value-of select=" & Strings.ChrW(34).ToString() & "current()" & Strings.ChrW(34).ToString() & "/>"
            '@!S_D<xsl:value-of select="current()"/>@!S_D

            Select Case lDataType
                ' Dates are passed as string and converted in the Stored procedure
                Case iGISSharedConstants.GISDataTypeDate
                    'SJ 15/07/2004 - start
                    'sDelim = Chr(34)
                    sDelim_Start = SIRIUS_DELIM_START
                    sDelim_End = SIRIUS_DELIM_END
                'SJ 15/07/2004 - end
                Case iGISSharedConstants.GISDataTypeNumeric, iGISSharedConstants.GISDataTypeNumericOutput, iGISSharedConstants.GISDataTypeOption, iGISSharedConstants.GISDataTypeCurrency
                    sDelim_Start = ""
                    sDelim_End = ""
                Case iGISSharedConstants.GISDataTypeShortList, iGISSharedConstants.GISDataTypeLongList, iGISSharedConstants.GISDataTypeText
                    'SJ 15/07/2004 - start
                    'sDelim_Start = Chr(34)
                    sDelim_Start = SIRIUS_DELIM_START
                    sDelim_End = SIRIUS_DELIM_END
                'SJ 15/07/2004 - end
                Case iGISSharedConstants.GISDataTypeComment
                    'SJ 16/07/2004 - start
                    'sDelim_Start = "'"
                    sDelim_Start = SIRIUS_DELIM_START
                    sDelim_End = SIRIUS_DELIM_END
                'SJ 16/07/2004 - end
                Case iGISSharedConstants.GISDataTypeShortListCode, iGISSharedConstants.GISDataTypeLongListCode
                    'SJ 15/07/2004 - start
                    sDelim_Start = SIRIUS_DELIM_START
                    sDelim_End = SIRIUS_DELIM_END
                'sDelim = "'"
                'SJ 15/07/2004 - end
                Case iGISSharedConstants.GISDataTypePercentage
                    sDelim_Start = ""
                    sDelim_End = ""
                'SJ 15/07/2004 - start
                Case iGISSharedConstants.GISDataTypeInteger
                    sDelim_Start = ""
                    sDelim_End = ""
                    'SJ 15/07/2004 - end
                Case Else
            End Select

            FileSystem.PrintLine(v_iFileNum, "            <xsl:if test=" & Strings.ChrW(34).ToString() & "local-name()='" & sPropertyName & "'" & Strings.ChrW(34).ToString() & "> , @" & sColumnName & "=<xsl:choose><xsl:when test=" & Strings.ChrW(34).ToString() & "current()[. = '']" & Strings.ChrW(34).ToString() & ">NULL</xsl:when><xsl:otherwise>" & sDelim_Start & sFormat & sDelim_End & "</xsl:otherwise></xsl:choose></xsl:if>")

        Next lRow

        ' End the Template
        FileSystem.PrintLine(v_iFileNum, "            </xsl:for-each>")
        FileSystem.PrintLine(v_iFileNum, "            ;")
        FileSystem.PrintLine(v_iFileNum)
        FileSystem.PrintLine(v_iFileNum, "        <xsl:apply-templates/>")
        FileSystem.PrintLine(v_iFileNum, "    </xsl:template>")


        ' Build the SP for Each Child Table
        If Informations.IsArray(vChildObjectArray) Then


            For lRow As Integer = vChildObjectArray.GetLowerBound(0) To vChildObjectArray.GetUpperBound(0)


                sChildObjectName = CStr(vChildObjectArray(lRow)).Trim()

                ' Create the XSL for each Child Object
                lReturn = CType(BuildSaveXSLForObject(v_sObjectName:=sChildObjectName, v_iFileNum:=v_iFileNum), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

            Next lRow

        End If

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: BuildSetIDXSL
    '
    ' Description: Build XSL that will poulate the ID values for each Object.
    '              The XSL produced here will be executed against the XML
    '              as the first step of the SaveToDB.
    '
    ' History: 12/08/2008 RFC - Created.
    '
    ' ***************************************************************** '
    Private Function BuildSetIDXSL(ByVal v_sGisDataModelCode As String, ByVal v_sTopLevelObjectName As String, Optional ByVal v_sTopLevelQuoteObject As String = "") As Integer

        Dim result As Integer = 0
        Dim iFileNum As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sXSLFileName As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the Next File Number
        iFileNum = FileSystem.FreeFile()

        ' Get the File Name for this Data Model Code
        lReturn = CType(iGISSharedConstants.GetSetIDXSLFileName(v_sGisDataModelCode:=v_sGisDataModelCode, r_sSetIDXSLFileName:=sXSLFileName), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' Open the File
        FileSystem.FileOpen(iFileNum, sXSLFileName, OpenMode.Output)

        ' Start the XSL Stylesheet
        FileSystem.PrintLine(iFileNum, "<?xml version=" & Strings.ChrW(34).ToString() & "1.0" & Strings.ChrW(34).ToString() & " encoding=" & Strings.ChrW(34).ToString() & "ISO-8859-1" & Strings.ChrW(34).ToString() & "?>")
        FileSystem.PrintLine(iFileNum, "<xsl:stylesheet version=" & Strings.ChrW(34).ToString() & "1.0" & Strings.ChrW(34).ToString() & " xmlns:xsl=" & Strings.ChrW(34).ToString() & "http://www.w3.org/1999/XSL/Transform" & Strings.ChrW(34).ToString() & " xmlns:fo=" & Strings.ChrW(34).ToString() & "http://www.w3.org/1999/XSL/Format" & Strings.ChrW(34).ToString() & ">")
        FileSystem.PrintLine(iFileNum, "<xsl:output method=" & Strings.ChrW(34).ToString() & "xml" & Strings.ChrW(34).ToString() & " encoding=" & Strings.ChrW(34).ToString() & "ISO-8859-1" & Strings.ChrW(34).ToString() & "/>")

        FileSystem.PrintLine(iFileNum, "    <xsl:template match=" & Strings.ChrW(34).ToString() & "/" & Strings.ChrW(34).ToString() & ">")
        FileSystem.PrintLine(iFileNum, "        <xsl:apply-templates/>")
        FileSystem.PrintLine(iFileNum, "    </xsl:template>")

        FileSystem.PrintLine(iFileNum, "    <xsl:template match=" & Strings.ChrW(34).ToString() & "DATA_SET" & Strings.ChrW(34).ToString() & ">")
        FileSystem.PrintLine(iFileNum, "        <xsl:copy>")
        FileSystem.PrintLine(iFileNum, "        <xsl:for-each select=" & Strings.ChrW(34).ToString() & "./@*" & Strings.ChrW(34).ToString() & ">")
        FileSystem.PrintLine(iFileNum, "            <xsl:attribute name=" & Strings.ChrW(34).ToString() & "{local-name()}" & Strings.ChrW(34).ToString() & ">")
        FileSystem.PrintLine(iFileNum, "                    <xsl:value-of select=" & Strings.ChrW(34).ToString() & "current()" & Strings.ChrW(34).ToString() & "/>")
        FileSystem.PrintLine(iFileNum, "            </xsl:attribute>")
        FileSystem.PrintLine(iFileNum, "        </xsl:for-each>")
        FileSystem.PrintLine(iFileNum, "        <xsl:apply-templates/>")
        FileSystem.PrintLine(iFileNum, "        </xsl:copy>")
        FileSystem.PrintLine(iFileNum, "    </xsl:template>")

        FileSystem.PrintLine(iFileNum, "    <xsl:template match=" & Strings.ChrW(34).ToString() & "RISK_OBJECTS" & Strings.ChrW(34).ToString() & ">")
        FileSystem.PrintLine(iFileNum, "        <xsl:copy>")
        FileSystem.PrintLine(iFileNum, "        <xsl:for-each select=" & Strings.ChrW(34).ToString() & "./@*" & Strings.ChrW(34).ToString() & ">")
        FileSystem.PrintLine(iFileNum, "            <xsl:attribute name=" & Strings.ChrW(34).ToString() & "{local-name()}" & Strings.ChrW(34).ToString() & ">")
        FileSystem.PrintLine(iFileNum, "                    <xsl:value-of select=" & Strings.ChrW(34).ToString() & "current()" & Strings.ChrW(34).ToString() & "/>")
        FileSystem.PrintLine(iFileNum, "            </xsl:attribute>")
        FileSystem.PrintLine(iFileNum, "        </xsl:for-each>")
        FileSystem.PrintLine(iFileNum, "        <xsl:apply-templates/>")
        FileSystem.PrintLine(iFileNum, "        </xsl:copy>")
        FileSystem.PrintLine(iFileNum, "    </xsl:template>")
        FileSystem.PrintLine(iFileNum)

        ' Create the XSL for the Objects starting with the Top one.
        lReturn = CType(BuildSetIDXSLForObject(v_sTopLevelObjName:=v_sTopLevelObjectName, v_sObjectName:=v_sTopLevelObjectName, v_iFileNum:=iFileNum, v_lLevel:=1, v_bQuoteObject:=False), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' Finish off with the Deleted & Quote Objects
        FileSystem.PrintLine(iFileNum)
        FileSystem.PrintLine(iFileNum, "    <xsl:template match=" & Strings.ChrW(34).ToString() & "DELETED_OBJECTS" & Strings.ChrW(34).ToString() & ">")

        FileSystem.PrintLine(iFileNum, "        <xsl:copy>")
        FileSystem.PrintLine(iFileNum, "        <xsl:for-each select=" & Strings.ChrW(34).ToString() & "./@*" & Strings.ChrW(34).ToString() & ">")
        FileSystem.PrintLine(iFileNum, "            <xsl:attribute name=" & Strings.ChrW(34).ToString() & "{local-name()}" & Strings.ChrW(34).ToString() & ">")
        FileSystem.PrintLine(iFileNum, "                    <xsl:value-of select=" & Strings.ChrW(34).ToString() & "current()" & Strings.ChrW(34).ToString() & "/>")
        FileSystem.PrintLine(iFileNum, "            </xsl:attribute>")
        FileSystem.PrintLine(iFileNum, "        </xsl:for-each>")
        FileSystem.PrintLine(iFileNum, "        <xsl:apply-templates/>")
        FileSystem.PrintLine(iFileNum, "        </xsl:copy>")

        FileSystem.PrintLine(iFileNum, "    </xsl:template>")

        FileSystem.PrintLine(iFileNum, "    <xsl:template match=" & Strings.ChrW(34).ToString() & "QUOTES" & Strings.ChrW(34).ToString() & ">")

        FileSystem.PrintLine(iFileNum, "        <xsl:copy>")
        FileSystem.PrintLine(iFileNum, "        <xsl:for-each select=" & Strings.ChrW(34).ToString() & "./@*" & Strings.ChrW(34).ToString() & ">")
        FileSystem.PrintLine(iFileNum, "            <xsl:attribute name=" & Strings.ChrW(34).ToString() & "{local-name()}" & Strings.ChrW(34).ToString() & ">")
        FileSystem.PrintLine(iFileNum, "                    <xsl:value-of select=" & Strings.ChrW(34).ToString() & "current()" & Strings.ChrW(34).ToString() & "/>")
        FileSystem.PrintLine(iFileNum, "            </xsl:attribute>")
        FileSystem.PrintLine(iFileNum, "        </xsl:for-each>")
        FileSystem.PrintLine(iFileNum, "        <xsl:apply-templates/>")
        FileSystem.PrintLine(iFileNum, "        </xsl:copy>")

        FileSystem.PrintLine(iFileNum, "    </xsl:template>")

        FileSystem.PrintLine(iFileNum, "    <xsl:template match=" & Strings.ChrW(34).ToString() & "QUOTE_OBJECTS" & Strings.ChrW(34).ToString() & ">")

        FileSystem.PrintLine(iFileNum, "        <xsl:copy>")
        FileSystem.PrintLine(iFileNum, "        <xsl:for-each select=" & Strings.ChrW(34).ToString() & "./@*" & Strings.ChrW(34).ToString() & ">")
        FileSystem.PrintLine(iFileNum, "            <xsl:attribute name=" & Strings.ChrW(34).ToString() & "{local-name()}" & Strings.ChrW(34).ToString() & ">")
        FileSystem.PrintLine(iFileNum, "                    <xsl:value-of select=" & Strings.ChrW(34).ToString() & "current()" & Strings.ChrW(34).ToString() & "/>")
        FileSystem.PrintLine(iFileNum, "            </xsl:attribute>")
        FileSystem.PrintLine(iFileNum, "        </xsl:for-each>")
        FileSystem.PrintLine(iFileNum, "        <xsl:apply-templates/>")
        FileSystem.PrintLine(iFileNum, "        </xsl:copy>")

        FileSystem.PrintLine(iFileNum, "    </xsl:template>")

        ' If we have been supplied a Top Level Quote Object
        If v_sTopLevelQuoteObject <> "" Then
            ' Create the XSL for the Objects starting with the Top one.
            lReturn = CType(BuildSetIDXSLForObject(v_sTopLevelObjName:=v_sTopLevelQuoteObject, v_sObjectName:=v_sTopLevelQuoteObject, v_iFileNum:=iFileNum, v_lLevel:=1, v_bQuoteObject:=True), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

        End If

        ' Closing Styleshet Tag
        FileSystem.PrintLine(iFileNum, "</xsl:stylesheet>")

        FileSystem.FileClose(iFileNum) ' Close file.

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: BuildSetIDXSLForObject
    '
    ' Description:
    '
    ' History: 12/08/2008 RFC - Created.
    '          2002-12-11 IJR - Added code to write QUOTE_BINDER_ID to xsl
    ' ***************************************************************** '
    Private Function BuildSetIDXSLForObject(ByVal v_sTopLevelObjName As String, ByVal v_sObjectName As String, ByVal v_iFileNum As Integer, ByVal v_lLevel As Integer, ByVal v_bQuoteObject As Boolean) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sPropertyName As String = ""
        Dim lDataType As Integer
        Dim sColumnName As String = ""
        Dim iIsPrimaryKey As gPMConstants.PMEReturnCode
        Dim lGISPropertyID As Integer

        ' Object Definition
        Dim vPropertyArray(,) As Object = Nothing
        Dim sTableName As String = ""
        Dim lIsQuoteObject, lGISObjectID As Integer
        Dim sParentObjectName As String = ""
        Dim vChildObjectArray As Object = Nothing
        Dim sChildObjectName As String = ""
        Dim sDelim As String = ""
        Dim lNonGISType As Integer

        Dim sVariable, sPLXPath, sPLXPath2 As String
        Dim sXPath As New StringBuilder
        Dim lCount As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the Object Definition Details
        lReturn = CType(GetObjectDefDetails(v_sObjectName:=v_sObjectName, r_lIsQuoteObject:=lIsQuoteObject, r_lGISObjectID:=lGISObjectID, r_sTableName:=sTableName, r_sParentObjectName:=sParentObjectName, r_vChildObjectArray:=vChildObjectArray, r_vPropertyArray:=vPropertyArray, r_lIsNonGIS:=lNonGISType), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' Top Level
        If (v_lLevel = 1) And (Not v_bQuoteObject) Then

            FileSystem.PrintLine(v_iFileNum, "    <xsl:template match=" & Strings.ChrW(34).ToString() & v_sObjectName.Trim().ToUpper() & Strings.ChrW(34).ToString() & ">")
            FileSystem.PrintLine(v_iFileNum, "        <xsl:copy>")
            FileSystem.PrintLine(v_iFileNum, "        <xsl:for-each select=" & Strings.ChrW(34).ToString() & "./@*" & Strings.ChrW(34).ToString() & ">")
            FileSystem.PrintLine(v_iFileNum, "            <xsl:attribute name=" & Strings.ChrW(34).ToString() & "{local-name()}" & Strings.ChrW(34).ToString() & ">")
            FileSystem.PrintLine(v_iFileNum, "                    <xsl:value-of select=" & Strings.ChrW(34).ToString() & "current()" & Strings.ChrW(34).ToString() & "/>")
            FileSystem.PrintLine(v_iFileNum, "            </xsl:attribute>")
            FileSystem.PrintLine(v_iFileNum, "        </xsl:for-each>")
            FileSystem.PrintLine(v_iFileNum, "        <xsl:apply-templates/>")
            FileSystem.PrintLine(v_iFileNum, "        </xsl:copy>")
            FileSystem.PrintLine(v_iFileNum, "    </xsl:template>")

        Else

            ' Match for Insert Objects
            FileSystem.PrintLine(v_iFileNum, "    <xsl:template match=" & Strings.ChrW(34).ToString() & v_sObjectName.Trim().ToUpper() & "[@US = '1']" & Strings.ChrW(34).ToString() & ">")

            FileSystem.PrintLine(v_iFileNum, "        <xsl:copy>")


            lCount = 0
            sPLXPath = ""
            sPLXPath2 = ""

            For lLoop1 As Integer = v_lLevel To 1 Step -1
                sXPath = New StringBuilder("")
                lCount += 1
                sVariable = "InstNum" & lCount

                If lLoop1 = 1 Then
                    sXPath = New StringBuilder("./")
                Else

                    For lLoop2 As Integer = lLoop1 - 1 To 1 Step -1
                        sXPath.Append("../")
                    Next lLoop2

                End If

                If sPLXPath = "" Then
                    sPLXPath = sXPath.ToString() & "@GIS_POLICY_LINK_ID"
                End If

                'SJ 15/07/2004 - start
                If sPLXPath2 = "" Then
                    sPLXPath2 = sXPath.ToString() & "@QUOTE_BINDER_ID"
                End If
                'SJ 15/07/2004 - end

                sXPath.Append("@OI")

                If lCount > 1 Then
                    FileSystem.PrintLine(v_iFileNum, "        <xsl:variable name=" & Strings.ChrW(34).ToString() & sVariable & Strings.ChrW(34).ToString() & " select=" & Strings.ChrW(34).ToString() & sXPath.ToString() & Strings.ChrW(34).ToString() & "/>")
                End If

            Next lLoop1

            ' For Each Property
            For lRow As Integer = vPropertyArray.GetLowerBound(1) To vPropertyArray.GetUpperBound(1)

                sPropertyName = CStr(vPropertyArray(0, lRow)).Trim()

                ' Get the Property Details
                lReturn = GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, r_lDataType:=lDataType, r_sColumnName:=sColumnName, r_iIsPrimaryKey:=iIsPrimaryKey, r_lGISPropertyID:=lGISPropertyID)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

                ' Upercase the Propertyname
                sPropertyName = sPropertyName.Trim().ToUpper()

                If (iIsPrimaryKey = gPMConstants.PMEReturnCode.PMTrue) And (lNonGISType = GISDataModelType.GISOTRisk Or lNonGISType = GISDataModelType.GISOTNonGisSpecials Or lNonGISType = GISDataModelType.GISOTCase) Then
                    'Print #v_iFileNum, "                    <xsl:when test=" & Chr$(34) & "local-name() = '" & sPropertyName & "'" & Chr$(34) & "><xsl:value-of select=" & Chr$(34) & "substring-after($InstNum" & lRow + 1 & ",'OI')" & Chr$(34) & "/></xsl:when>"
                    If v_bQuoteObject Then
                        If sPropertyName = "GIS_POLICY_LINK_ID" Then
                            ' Top Level Quote Object will already have had the GIS_POLICY_LINK_ID set
                            If v_lLevel > 1 Then
                                FileSystem.PrintLine(v_iFileNum, "                    <xsl:attribute name=" & Strings.ChrW(34).ToString() & sPropertyName & Strings.ChrW(34).ToString() & "><xsl:value-of select=" & Strings.ChrW(34).ToString() & sPLXPath & Strings.ChrW(34).ToString() & "/></xsl:attribute>")
                            End If
                        ElseIf (sPropertyName = "QUOTE_BINDER_ID") Then
                            If v_lLevel > 1 Then
                                FileSystem.PrintLine(v_iFileNum, "                    <xsl:attribute name=" & Strings.ChrW(34).ToString() & sPropertyName & Strings.ChrW(34).ToString() & "><xsl:value-of select=" & Strings.ChrW(34).ToString() & sPLXPath2 & Strings.ChrW(34).ToString() & "/></xsl:attribute>")
                            End If
                        Else
                            FileSystem.PrintLine(v_iFileNum, "                    <xsl:attribute name=" & Strings.ChrW(34).ToString() & sPropertyName & Strings.ChrW(34).ToString() & "><xsl:value-of select=" & Strings.ChrW(34).ToString() & "substring-after($InstNum" & CStr(lRow) & ", 'OI')" & Strings.ChrW(34).ToString() & "/></xsl:attribute>")
                        End If

                    Else
                        If sPropertyName = v_sTopLevelObjName & "_ID" Then
                            FileSystem.PrintLine(v_iFileNum, "                    <xsl:attribute name=" & Strings.ChrW(34).ToString() & sPropertyName & Strings.ChrW(34).ToString() & "><xsl:value-of select=" & Strings.ChrW(34).ToString() & "//" & v_sTopLevelObjName & "/@GIS_POLICY_LINK_ID" & Strings.ChrW(34).ToString() & "/></xsl:attribute>")
                        Else
                            FileSystem.PrintLine(v_iFileNum, "                    <xsl:attribute name=" & Strings.ChrW(34).ToString() & sPropertyName & Strings.ChrW(34).ToString() & "><xsl:value-of select=" & Strings.ChrW(34).ToString() & "substring-after($InstNum" & CStr(lRow + 1) & ", 'OI')" & Strings.ChrW(34).ToString() & "/></xsl:attribute>")
                        End If
                    End If
                End If

            Next lRow
            Dim bIsValueAdded As Boolean = False
            Dim sPrintLineString As String = ""

            FileSystem.PrintLine(v_iFileNum, "        <xsl:for-each select=" & ChrW(34) & "./@*" & ChrW(34) & ">")
            FileSystem.PrintLine(v_iFileNum, "            <xsl:attribute name=" & ChrW(34) & "{local-name()}" & ChrW(34) & ">")

            ' For Each Property
            For lRow As Integer = vPropertyArray.GetLowerBound(1) To vPropertyArray.GetUpperBound(1)

                sPropertyName = ((vPropertyArray(0, lRow)).ToString()).Trim()

                ' Get the Property Details
                lReturn = GetPropertyDefDetails(
                    v_sObjectName:=v_sObjectName,
                    v_sPropertyName:=sPropertyName,
                    r_lDataType:=lDataType,
                    r_sColumnName:=sColumnName,
                    r_iIsPrimaryKey:=iIsPrimaryKey,
                        r_lGISPropertyID:=lGISPropertyID)
                If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    Return lReturn
                End If

                ' Upercase the Propertyname
                sPropertyName = ((sPropertyName).Trim()).ToUpper()

                If (iIsPrimaryKey = gPMConstants.PMEReturnCode.PMTrue) _
                And (lNonGISType = GISOTRisk Or lNonGISType = GISOTNonGisSpecials _
                Or lNonGISType = GISOTCase) Then
                    If (v_bQuoteObject) Then
                        If (sPropertyName = "GIS_POLICY_LINK_ID") Then
                            ' Top Level Quote Object will already have had the GIS_POLICY_LINK_ID set
                            If (v_lLevel > 1) Then
                                sPrintLineString = sPrintLineString & "                    <xsl:attribute name=" & Strings.ChrW(34).ToString() & sPropertyName & Strings.ChrW(34).ToString() & "><xsl:value-of select=" & Strings.ChrW(34).ToString() & sPLXPath & Strings.ChrW(34).ToString() & "/></xsl:attribute>" & vbNewLine
                            End If
                        ElseIf (sPropertyName = "QUOTE_BINDER_ID") Then
                            If (v_lLevel > 1) Then
                                sPrintLineString = sPrintLineString & "                    <xsl:attribute name=" & Strings.ChrW(34).ToString() & sPropertyName & Strings.ChrW(34).ToString() & "><xsl:value-of select=" & Strings.ChrW(34).ToString() & sPLXPath2 & Strings.ChrW(34).ToString() & "/></xsl:attribute>" & vbNewLine
                            End If
                        Else
                            sPrintLineString = sPrintLineString & "                    <xsl:attribute name=" & Strings.ChrW(34).ToString() & sPropertyName & Strings.ChrW(34).ToString() & "><xsl:value-of select=" & Strings.ChrW(34).ToString() & "substring-after($InstNum" & lRow & ", 'OI')" & Strings.ChrW(34).ToString() & "/></xsl:attribute>" & vbNewLine
                        End If

                    Else
                        bIsValueAdded = True
                        If sPropertyName = v_sTopLevelObjName & "_ID" Then
                            sPrintLineString = sPrintLineString & "                    <xsl:when test=" & Strings.ChrW(34).ToString() & "local-name() = '" & sPropertyName & "'" & Strings.ChrW(34).ToString() & "><xsl:value-of select=" & Strings.ChrW(34).ToString() & "//" & v_sTopLevelObjName & "/@GIS_POLICY_LINK_ID" & Strings.ChrW(34).ToString() & "/></xsl:when>" & vbNewLine
                        Else
                            sPrintLineString = sPrintLineString & "                    <xsl:when test=" & Strings.ChrW(34).ToString() & "local-name() = '" & sPropertyName & "'" & Strings.ChrW(34).ToString() & "><xsl:value-of select=" & Strings.ChrW(34).ToString() & "substring-after($InstNum" & lRow + 1 & ",'OI')" & Strings.ChrW(34).ToString() & "/></xsl:when>" & vbNewLine
                        End If
                    End If
                End If
            Next lRow

            ' RFC040203 - Special Objects need to get the keys differently
            Select Case v_sObjectName
                Case "DISCLOSURE"
                    sPrintLineString = sPrintLineString & "<xsl:attribute name=" & Strings.ChrW(34).ToString() & "PARTY_CNT" & Strings.ChrW(34).ToString() & "><xsl:value-of select=" & Strings.ChrW(34).ToString() & "../@PARTY_CNT" & Strings.ChrW(34).ToString() & "/></xsl:attribute>" & vbNewLine
                    sPrintLineString = sPrintLineString & "<xsl:attribute name=" & Strings.ChrW(34).ToString() & "BUSCOMB_DISCLOSURE_ID" & Strings.ChrW(34).ToString() & ">0</xsl:attribute>" & vbNewLine
                Case Else
            End Select

            If bIsValueAdded = True Then
                FileSystem.PrintLine(v_iFileNum, "                <xsl:choose>")
                FileSystem.PrintLine(v_iFileNum, sPrintLineString)
                FileSystem.PrintLine(v_iFileNum, "            <xsl:otherwise> <xsl:value-of select=" & Strings.ChrW(34).ToString() & "current()" & Strings.ChrW(34).ToString() & "/> </xsl:otherwise>")
                FileSystem.PrintLine(v_iFileNum, "         </xsl:choose>")
            Else
                FileSystem.PrintLine(v_iFileNum, sPrintLineString)
            End If

            FileSystem.PrintLine(v_iFileNum, "         </xsl:attribute>")
            FileSystem.PrintLine(v_iFileNum, "        </xsl:for-each>")
            FileSystem.PrintLine(v_iFileNum, "        <xsl:apply-templates/>")
            FileSystem.PrintLine(v_iFileNum, "        </xsl:copy>")
            FileSystem.PrintLine(v_iFileNum, "        ")
            FileSystem.PrintLine(v_iFileNum, "    </xsl:template>")

            ' Match for a Non Insert Object
            FileSystem.PrintLine(v_iFileNum, "    <xsl:template match=" & Strings.ChrW(34).ToString() & v_sObjectName.Trim().ToUpper() & "[@US != 1]" & Strings.ChrW(34).ToString() & ">")
            FileSystem.PrintLine(v_iFileNum, "        <xsl:copy>")
            FileSystem.PrintLine(v_iFileNum, "        <xsl:for-each select=" & Strings.ChrW(34).ToString() & "./@*" & Strings.ChrW(34).ToString() & ">")
            FileSystem.PrintLine(v_iFileNum, "            <xsl:attribute name=" & Strings.ChrW(34).ToString() & "{local-name()}" & Strings.ChrW(34).ToString() & ">")
            FileSystem.PrintLine(v_iFileNum, "                    <xsl:value-of select=" & Strings.ChrW(34).ToString() & "current()" & Strings.ChrW(34).ToString() & "/>")
            FileSystem.PrintLine(v_iFileNum, "            </xsl:attribute>")
            FileSystem.PrintLine(v_iFileNum, "        </xsl:for-each>")
            FileSystem.PrintLine(v_iFileNum, "        <xsl:apply-templates/>")
            FileSystem.PrintLine(v_iFileNum, "        </xsl:copy>")
            FileSystem.PrintLine(v_iFileNum, "    </xsl:template>")

        End If

        FileSystem.PrintLine(v_iFileNum, "")

        ' Build the SP for Each Child Table
        If Informations.IsArray(vChildObjectArray) Then

            v_lLevel += 1

            For lRow As Integer = vChildObjectArray.GetLowerBound(0) To vChildObjectArray.GetUpperBound(0)

                sChildObjectName = CStr(vChildObjectArray(lRow)).Trim()
                ' Create the XSL for each Child Object
                lReturn = CType(BuildSetIDXSLForObject(v_sTopLevelObjName:=v_sTopLevelObjName, v_sObjectName:=sChildObjectName, v_iFileNum:=v_iFileNum, v_lLevel:=v_lLevel, v_bQuoteObject:=v_bQuoteObject), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

            Next lRow

        End If

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: AddObjectDefSQL
    '
    ' Description: Builds and adds the SQL Statements required for the
    '              object to the Data Set Def.
    ' ***************************************************************** '
    Private Function AddObjectDefSQL(ByVal v_sObjectName As String, ByVal v_sTopLevelTableName As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        ' Object Definition
        Dim lIsQuoteObject, lGISObjectID As Integer
        Dim sTableName As String = ""
        Dim lMaxInstances, lPolarisObjectID As Integer
        Dim sParentObjectName As String = ""
        Dim vChildObjectArray As Object = Nothing
        Dim vPropertyArray(,) As Object = Nothing
        Dim sObjectName As String = ""


        Dim sAddSQL As String = ""
        Dim sUpdateSQL As String = ""
        Dim sAddUpdateSQL As String = ""
        Dim sDeleteSQL As String = ""
        Dim sSelectSQL As String = ""

        Dim oObjDef As XmlElement



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the Object Definition Details
        lReturn = CType(GetObjectDefDetails(v_sObjectName:=v_sObjectName, r_lIsQuoteObject:=lIsQuoteObject, r_lGISObjectID:=lGISObjectID, r_sTableName:=sTableName, r_lMaxInstances:=lMaxInstances, r_lPolarisObjectID:=lPolarisObjectID, r_sParentObjectName:=sParentObjectName, r_vChildObjectArray:=vChildObjectArray, r_vPropertyArray:=vPropertyArray), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' Build SQL Strings

        lReturn = CType(BuildAddUpdateSQL(v_sObjectName:=v_sObjectName, v_sTableName:=sTableName, v_vPropertyArray:=vPropertyArray, r_sAddSQL:=sAddSQL, r_sUpdateSQL:=sUpdateSQL, r_sAddUpdateSQL:=sAddUpdateSQL, r_sDeleteSQL:=sDeleteSQL), gPMConstants.PMEReturnCode)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If


        lReturn = CType(BuildSelectSQL(v_sObjectName:=v_sObjectName, v_sTableName:=sTableName, v_vPropertyArray:=vPropertyArray, v_sTopLevelTableName:=v_sTopLevelTableName, r_sSelectSQL:=sSelectSQL), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' Assign the SQL Statements to the Object Def
        oObjDef = GetDefinitionNode(v_sObjectName)
        If oObjDef Is Nothing Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        oObjDef.SetAttribute(ACXMLAttribSQLInsert, sAddSQL)
        ' At the moment store the Add Update SQL in the Update bit.
        'oObjDef.setAttribute ACXMLAttribSQLUpdate, sAddUpdateSQL
        ' RFC260400 - Correct save to db functionality
        oObjDef.SetAttribute(ACXMLAttribSQLUpdate, sUpdateSQL)
        oObjDef.SetAttribute(ACXMLAttribSQLDelete, sDeleteSQL)
        oObjDef.SetAttribute(ACXMLAttribSQLSelect, sSelectSQL)

        oObjDef = Nothing

        ' If there Are NO Child Objects for this Object Type then EXIT
        If Not Informations.IsArray(vChildObjectArray) Then
            Return result
        End If

        ' For Each Child Object

        For lRow As Integer = vChildObjectArray.GetLowerBound(0) To vChildObjectArray.GetUpperBound(0)

            sObjectName = CStr(vChildObjectArray(lRow))
            lReturn = CType(AddObjectDefSQL(v_sObjectName:=sObjectName, v_sTopLevelTableName:=v_sTopLevelTableName), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If
        Next lRow

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: SetRoot
    '
    ' Description: Sets the Root Level of the Document. If a Quote Number
    '              is supplied then that Quote is set to be the top
    '              level, otherwise the RISK_OBJECTS node is the top.
    '
    ' History: 09/03/2000 RFC - Created.
    '          29/01/2002 RFC - Changed to a function, return instance of Root Node.
    '          10/12/2002 RDC - Changed to Public to support Control class
    ' ****************************************************************************** '
    Public Function SetRoot(Optional ByVal v_lQuoteNumber As Integer = 0) As cGISDataSetControl.Node

        Dim result As cGISDataSetControl.Node = Nothing
        Dim oNode As cGISDataSetControl.Node

        Try

            ' Create  a new Node element
            oNode = New cGISDataSetControl.Node()

            ' Set its root
            oNode.Root = m_oDataset.DocumentElement

            ' Set its Dataset
            oNode.Dataset = Me

            ' Is a Quote Number Supplied
            If v_lQuoteNumber < 1 Then
                ' No so set the Root to be "???_POLICY_BINDER" node below "RISK_OBJECTS"
                oNode.Root = m_oDataset.DocumentElement.ChildNodes.Item(0).ChildNodes.Item(0)
                ' As this is a RISK Object set the Quote Key to spaces
                oNode.QuoteKey = ""
            Else
                ' Set the Root to be the "QUOTES" node
                ' RFC200900 - Because of the addition of the DELETED OBJECTS node,
                ' QUOUTES is not the second element anymore. Therefore need to set
                ' the root to the third (2) element here.
                oNode.Root = m_oDataset.DocumentElement.ChildNodes.Item(2)
                ' Then set the Root to be the "QUOTE_OBJECTS" specified
                oNode.Root = oNode.Root.ChildNodes.Item(v_lQuoteNumber - 1)
                ' Set the Quote Key

                oNode.QuoteKey = CStr(oNode.Root.GetAttribute(ACXMLAttribOIKey))
            End If

            result = oNode
            oNode = Nothing

            Return result

        Catch excep As System.Exception



            Throw New System.Exception(Informations.Err().Number.ToString() + ", " + excep.Source + ", " + excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (Initialise) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function Initialise() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    '
    ' Initialisation Code.
    '
    'Return gPMConstants.PMEReturnCode.PMTrue
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bpmfunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise()", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

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
                m_oDataSetDef = Nothing
                m_oDataset = Nothing
                m_oDefaults = Nothing
            End If
            If Not (m_oLookupManager Is Nothing) Then
                'closeFiles() is obselete
                'm_oLookupManager.CloseFiles()
            End If

            m_oLookupManager = Nothing

        End If
        Me.disposedValue = True
    End Sub



    ' ***************************************************************** '
    ' Name: PropertyValue
    '
    ' Description: Returns the Property Value from the Attribute
    '
    ' ***************************************************************** '
    Friend Function PropertyValue(ByRef r_oObjectInst As XmlElement, ByRef v_sPropertyName As String) As String
        Dim result As String = String.Empty
        Dim lReturn As gPMConstants.PMEReturnCode

        Dim vPropertyValue As Object = Nothing

        Try



            ' Get the Property Value
            Dim lDataType As Integer
            If m_bStrict Then
                lReturn = CType(GetPropertyDefDetails(v_sObjectName:=r_oObjectInst.Name, v_sPropertyName:=v_sPropertyName, r_lDataType:=lDataType), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(vbObjectError.ToString() + ", " + ACApp & "." & ACClass & "." & "PropertyValue" + ", " + "Cant find definition for property " & r_oObjectInst.Name & "." & v_sPropertyName)
                End If
                Return result
            Else

            End If
            Try
                result = CStr(r_oObjectInst.GetAttribute(v_sPropertyName.Trim().ToUpper()))
            Catch ex As Exception

            End Try


            If Convert.IsDBNull(result) Or Informations.IsNothing(result) Then
                result = String.Empty
            End If


            If Not String.IsNullOrEmpty(result) Then
                ' More to do here, may have to unconvert ampersands etc
                result = result.Replace("&amp;", "&")
                result = result.Replace("&lt;", "<")
                result = result.Replace("&gt;", ">")
                result = result.Replace("&apos;", "'")
                result = result.Replace("&quot;", """")
                result = result.Replace("&#xD;", Global.Microsoft.VisualBasic.ChrW(13))
                result = result.Replace("&#xA;", Global.Microsoft.VisualBasic.ChrW(10))
            End If

            Return result

        Catch ex As Exception
            Dim lErrorNumber As Integer
            Dim sErrorDesc As String = ""

            lErrorNumber = Informations.Err().Number
            sErrorDesc = Informations.Err().Description

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PropertyValueFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertyValue", excep:=ex)

            Throw New System.Exception(lErrorNumber.ToString() + ", " + ACApp & "." & ACClass & "." & "PropertyValue" + ", " + sErrorDesc)

        End Try

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: PropertyValueSet
    '
    ' Description: Set a Property Value
    '
    ' ***************************************************************** '
    Friend Sub PropertyValueSet(ByRef r_oObjectInst As XmlElement, ByRef v_sObjectName As String, ByRef v_sPropertyName As String, ByRef v_vPropertyValue As Object, Optional ByRef v_bLoadedFromDB As Boolean = False)

        Dim lReturn As gPMConstants.PMEReturnCode

        Dim lObjectUpdateStatus As gPMConstants.PMEComponentAction

        Dim sPropertyTag As String = ""
        'Dim istrlength As Integer
        'Dim istrparloc As Integer
        'Dim istrparloc1 As Integer
        Dim binsertspace As Boolean = False
        Dim istrstart As Integer = 0
        Dim vValue As String = ""
        Dim lDataType As Integer
        Dim sErrorDesc As String = ""
        Dim iErrorNumber As Integer


        ' RAW 20/09/2004 : CQ6832 : added test for v_vPropertyValue = null

        If Convert.IsDBNull(v_vPropertyValue) Or Informations.IsNothing(v_vPropertyValue) Then

            v_vPropertyValue = ""
        End If

        If m_bStrict Then
            ' RAW 14/01/2004 : CQ3720 : added
            ' RAW 04/02/2004 : CQ4074 : added case statement to handle 'special' properties that do not exist in definition object
            Select Case v_sPropertyName.ToUpper()
                Case "OI", "US"
                    ' These properties only exist in the data object and not in the definition
                    ' so only validate the object
                    lReturn = CType(GetObjectDefDetails(v_sObjectName:=v_sObjectName), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(vbObjectError.ToString() + ", " + ACApp & "." & ACClass & "." & "SetPropertyValue" + ", " + "Cant find definition for object " & v_sObjectName & "(for property " & v_sPropertyName & ")")
                    End If

                    Select Case v_sPropertyName.ToUpper()
                        Case "US"
                            lDataType = iGISSharedConstants.GISDataTypeNumeric
                        Case Else
                            lDataType = iGISSharedConstants.GISDataTypeText
                    End Select

                Case Else
                    ' Get the Property Details
                    lReturn = CType(GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=v_sPropertyName, r_lDataType:=lDataType), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(vbObjectError.ToString() + ", " + ACApp & "." & ACClass & "." & "SetPropertyValue" + ", " + "Cant find definition for property " & v_sObjectName & "." & v_sPropertyName)
                    End If
            End Select
            ' RAW 14/01/2004 : CQ3720 : end
        End If

        ' RFC110700 - Check to see if the value is the same
        Try
            vValue = CStr(r_oObjectInst.GetAttribute(v_sPropertyName.Trim().ToUpper())).Trim()
        Catch
        End Try


        If vValue = CStr(v_vPropertyValue).Trim() Then
            ' Value is the same so just exit.
            Exit Sub
        End If

        If m_bStrict Then

            If (Not (Convert.IsDBNull(v_vPropertyValue) Or Informations.IsNothing(v_vPropertyValue))) And (Not Object.Equals(v_vPropertyValue, Nothing)) And (CStr(v_vPropertyValue) <> "") Then

                Select Case lDataType
                    Case iGISSharedConstants.GISDataTypeDate

                        If Not Informations.IsDate(v_vPropertyValue) Then
                            Throw New System.Exception(vbObjectError.ToString() + ", " + ACApp & "." & ACClass & "." & "PropertyValueSet" + ", " + "Invalid Value for Date property " & v_sObjectName & "." & v_sPropertyName)
                        End If

                    Case iGISSharedConstants.GISDataTypeNumeric, iGISSharedConstants.GISDataTypeNumericOutput, iGISSharedConstants.GISDataTypeOption, iGISSharedConstants.GISDataTypeCurrency, iGISSharedConstants.GISDataTypePercentage


                        Dim dbNumericTemp As Double
                        If Not Double.TryParse(CStr(v_vPropertyValue), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                            Throw New System.Exception(vbObjectError.ToString() + ", " + ACApp & "." & ACClass & "." & "PropertyValueSet" + ", " + "Invalid Value for Numeric property " & v_sObjectName & "." & v_sPropertyName)
                        End If
                End Select
            End If
            ' RAW 14/01/2004 : CQ3720 : end
        End If

        ' Now reformat the value

        Dim oPropertyDef As XmlElement
        If (Not (Convert.IsDBNull(v_vPropertyValue) Or Informations.IsNothing(v_vPropertyValue))) And (Not Object.Equals(v_vPropertyValue, Nothing)) And (CStr(v_vPropertyValue) <> "") Then

            ' Escape ampersands etc
            ' Double Quotes are doubled up so that the save to DB will work
            '       v_vPropertyValue = Replace(v_vPropertyValue, "&", "&amp;")
            '       v_vPropertyValue = Replace(v_vPropertyValue, "<", "&lt;")
            '       v_vPropertyValue = Replace(v_vPropertyValue, ">", "&gt;")
            '       v_vPropertyValue = Replace(v_vPropertyValue, "'", "&apos;")


            v_vPropertyValue = CStr(v_vPropertyValue).Replace("""", "&quot;")

            v_vPropertyValue = CStr(v_vPropertyValue).Replace("&apos;", "'")

            'CLG 22/01/04 PN9429 Intergers are being converted to dates in risk screens

            Dim dbNumericTemp2 As Double
            If Not Double.TryParse(CStr(v_vPropertyValue), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                ' CTAF 021002 - Tighter check for dates
                ' If the value is a date it needs to be formatted to be locale neutral

                'CLG 20050916
                'Previous tests to prevent a string being turned into dates have failed.
                'The only way to be sure is to check the property data type. To get this is time expensive so
                'we should only do it if
                '1) The field contents "looks like a date"
                '2) We are not in strict mode (where we already have the data type)
                If Informations.IsDate(v_vPropertyValue) Then
                    If Not m_bStrict Then
                        'For speed, run an inline reduced version of GetPropertyDefDetails
                        lDataType = -1 'not a date
                        oPropertyDef = GetDefinitionNode(v_sObjectName, v_sPropertyName)
                        If Not (oPropertyDef Is Nothing) Then
                            lDataType = CInt(oPropertyDef.GetAttribute(ACXMLAttribDataType))
                            oPropertyDef = Nothing
                        End If
                    End If

                    Dim dateValue As DateTime
                    dateValue = CDate(v_vPropertyValue.Replace("T", " ").Replace("Z", ""))
                    Dim Year As Double = dateValue.Date.Year
                    If (lDataType = iGISSharedConstants.GISDataTypeDate) AndAlso (Not Year = ("1899")) Then
                        v_vPropertyValue = CDate(v_vPropertyValue.Replace("T", " ").Replace("Z", "")).ToString("yyyy-MM-dd HH:mm:ss") '- original pre DRE code                        
                        'For DRE we must have the format below and change the xsd type date to dateTime
                        'v_vPropertyValue = CDate(v_vPropertyValue).ToString("yyyy-MM-dd'T'HH:mm:ss")
                    End If
                End If
            End If
            v_vPropertyValue = CStr(v_vPropertyValue).Trim()
            'If Convert.ToString(v_vPropertyValue).Contains("{\rtf") Then
            '    v_vPropertyValue = Convert.ToString(v_vPropertyValue).Replace(Global.Microsoft.VisualBasic.ChrW(13), "")
            '    v_vPropertyValue = Convert.ToString(v_vPropertyValue).Replace(Global.Microsoft.VisualBasic.ChrW(10), "")
            'End If
        End If

        r_oObjectInst.SetAttribute(v_sPropertyName.Trim().ToUpper(), v_vPropertyValue) ' RAW 14/01/2004 : CQ3720 : remove trim

        ' Are we loading this from the Database
        If Not v_bLoadedFromDB Then

            ' No, so we need to set the Objects Update Status

            ' Get the Objects current Update Status

            lObjectUpdateStatus = r_oObjectInst.GetAttribute(ACXMLAttribUpdateStatus)

            ' If the Update Status is View
            ' Note: If the current Status is Add, Edit or Delete leave it alone
            If lObjectUpdateStatus = gPMConstants.PMEComponentAction.PMView Then
                ' Set it to Edit
                r_oObjectInst.SetAttribute(ACXMLAttribUpdateStatus, gPMConstants.PMEComponentAction.PMEdit)
            End If

        End If

        Exit Sub

Err_PropertyValueSet:

        iErrorNumber = Informations.Err().Number
        sErrorDesc = Informations.Err().Description
        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PropertyValueSetFailed for property " & v_sObjectName & "." & v_sPropertyName, vApp:=ACApp, vClass:=ACClass, vMethod:="PropertyValueSet", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        ' RAW 22/01/2004 : CQ3720 : bpmfunc.LogMessage clears err object so use original details
        Throw New System.Exception(iErrorNumber.ToString() + ", " + ACApp & "." & ACClass & "." & "PropertyValueSet" + ", " + sErrorDesc)

        Exit Sub

    End Sub


    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        ' Class Initialise

        Try

            ' Initialise Data Model Definition
            m_oDataSetDef = New XmlDocument()
            m_oDataset = New XmlDocument()

            ' Use the new parser

            ' developer guide no solution no. 38
            'm_oDataSetDef.setProperty("NewParser", True)

            ' developer guide no solution no. 38
            'm_oDataset.setProperty("NewParser", True)

            '    g_lClassInitLevel = g_lClassInitLevel + 1
            '    Debug.Print "Initialise = " & g_lClassInitLevel

        Catch excep As System.Exception



            ' Error.

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub



    ' ***************************************************************** '
    ' Name: LoadDefaultsXMLFile
    '
    ' Description: Load the Defaults XML Document file into memory.
    '              The file path and name is passed in as v_sGISDefaultsFile.
    '
    ' Created    : 130301 CJB
    '
    ' ***************************************************************** '
    Private Function LoadDefaultsXMLFile(ByVal v_sGISDefaultsFile As String) As Integer

        Dim result As Integer = 0
        Dim bLoaded As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue


        'Create a new XML Document object in which to 'load' the data from the
        'XML document (flat) file
        m_oDefaults = New XmlDocument()

        ' Use the new parser


        'm_oDefaults.setProperty("NewParser", True)

        'Attempt the load of data from the file into the object

        ' developer guide no.  37(no solution)
        'm_oDefaults.validateOnParse = False
        Dim temp_xml_result As Boolean
        Try
            m_oDefaults.Load(v_sGISDefaultsFile)
            temp_xml_result = True

        Catch parseError As System.Exception
            temp_xml_result = False
        End Try
        bLoaded = temp_xml_result
        If Not bLoaded Then
            iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Load Defaults from File : " & v_sGISDefaultsFile, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadDefaultsXMLFile")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: LoadAndApplyGISDefaults
    '
    ' Description: Load the Defaults XML Document file into memory for
    '              the v_sDataModelCode that is passed in. Note that the
    '              location of the XML Defaults file will be determined by
    '              the XML Dataset files and if no file is found then exit
    '              function without errors (as presence is optional).
    '              Apply any object/property GIS Dataset defaults to the
    '              current dataset. Please read in-line comments for
    '              conditions regarding this.
    '              v_sTransactionType (NB, MTA etc) is for future use.
    '
    ' Created    : 130301 CJB
    '
    ' ***************************************************************** '
    Public Function LoadAndApplyGISDefaults(ByVal v_sTransactionType As String) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Dim sGISDefaultsFile As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oObject, oProperty, oCurrentObject As XmlElement
        Dim lObjCount As Integer = 0
        Dim lPropCount As Integer = 0
        Dim sCurrentObjectName As String = ""
        Dim sCurrentPropertyName As String = ""
        Dim sCurrentDefaultValue As String = ""
        Dim sCurrentPropertyValue As String = ""
        Dim sMandatoryPropertyName As String = ""
        Dim sMandatoryPropertyValue As String = ""

        Dim vObjectKeyArray As Object = Nothing
        Dim sCurrentObjectKey As String = ""

        Dim bIsAssumedInfo As Boolean

        Try

            iGISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadAndApplyGISDefaults - Start", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            'Determine the name (and path) to the current solution specific GIS
            'defaults xml document
            lReturn = CType(iGISSharedConstants.GetDefaultsFileName(GISDataModelCode, sGISDefaultsFile), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Check if the file actually exists as it is optional - if not then exit without
            'errors and continue processing
            If FileSystem.Dir(sGISDefaultsFile, FileAttribute.Normal) = "" Then
                iGISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Warning:Defaults XML file not found at " & sGISDefaultsFile, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            'Load the data model specific Defaults XML document data into memory
            lReturn = CType(LoadDefaultsXMLFile(v_sGISDefaultsFile:=sGISDefaultsFile), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Now use the MSXML2.dll functions to traverse the XML defaults document we
            'have in memory

            'Get count of OBJECTS to loop around
            lObjCount = m_oDefaults.DocumentElement.ChildNodes.Count

            If lObjCount = 0 Then
                'Log a DEBUG message that Defaults file was found but no objects defined in it, exit and continue
                iGISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Warning:The following Defaults XML file was found but had no object definitions in:" & sGISDefaultsFile, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            'Loop around zero based objects collection
            For lObjectLoopCounter As Integer = 0 To lObjCount - 1

                'Set current object, note that documentElement refers to the top most
                'element (GIS_DEFAULTS)
                oObject = m_oDefaults.DocumentElement.ChildNodes.Item(lObjectLoopCounter)

                'This shouldn't happen unless the document has no objects
                If Not (oObject.FirstChild Is Nothing) Then

                    'The name of the current OBJECT
                    sCurrentObjectName = oObject.FirstChild.Name

                    'Check an object name has been specified ! If not, error and exit
                    If sCurrentObjectName.Trim() = "" Then
                        iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ERROR:An object with no name has been defined in the Defaults XML file.", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    'We set this object to be able to get the Mandatory property attribute later
                    oCurrentObject = oObject.FirstChild

                    'Get count of PROPERTIES for the current object to loop around
                    lPropCount = oObject.ChildNodes.Count

                    'Loop around property collection (ignore first instance (0) as
                    'this holds the object name ! Notice we don't log any warnings etc if no properties found
                    'as this is valid.
                    For lPropertyLoopCounter As Integer = 1 To lPropCount - 1

                        'Set current property instance
                        oProperty = oObject.ChildNodes.Item(lPropertyLoopCounter)

                        'This shouldn't happen unless current object has no matching
                        'property and default value pairs
                        If (Not (oProperty.ChildNodes.Item(0) Is Nothing)) And (Not (oProperty.ChildNodes.Item(1) Is Nothing)) Then

                            'The name of the Property (in 0) and the default value (in 1)
                            sCurrentPropertyName = oProperty.ChildNodes.Item(0).InnerText
                            sCurrentDefaultValue = oProperty.ChildNodes.Item(1).InnerText

                            'Check a property name has been specified ! If not, error and exit
                            If sCurrentPropertyName.Trim() = "" Then
                                iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ERROR:The following object has a property with no name in the Defaults XML file:" & sCurrentObjectName, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            'Check a property value has been specified ! If not, error and exit
                            If sCurrentDefaultValue.Trim() = "" Then
                                iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ERROR:The following object has a property with no value in the Defaults XML file. Object:" & sCurrentObjectName & " Property:" & sCurrentPropertyName, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            'At this point we have an object name, property name and default
                            'value so we need to apply this Informations to the dataset, then
                            'continue with reading and applying any others.

                            'Get an array of keys for the object first - we'll apply the
                            'default value to EACH instance of the object

                            lReturn = CType(GetAllOIKey(v_sObjectName:=sCurrentObjectName, r_vOIKeyArray:=vObjectKeyArray), gPMConstants.PMEReturnCode)
                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            'If objects exist loop around them setting the default property
                            'value - but only if the mandatory property for this object
                            '(which we get from the MANDATORY_PROPERTY attribute on the
                            'OBJECT_NAME element in the Defaults XML document) has
                            'a value (i.e. is not "" - which indicates that it is just a dummy
                            'object instance that should remain that way and not be populated
                            'with default values). Also only apply the default value if the
                            'property does not already have a value (i.e. it is "")
                            If Informations.IsArray(vObjectKeyArray) Then

                                For lInstanceCounter As Integer = vObjectKeyArray.GetLowerBound(0) To vObjectKeyArray.GetUpperBound(0)


                                    sCurrentObjectKey = CStr(vObjectKeyArray(lInstanceCounter))

                                    'Check there is a mandatory property defined for this object in the Defaults
                                    'xml file - if none, error and exit

                                    Dim auxVar As Object = oCurrentObject.GetAttribute(iGISSharedConstants.GISDefaultsMandatoryProperty)


                                    If Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar) Or CStr(oCurrentObject.GetAttribute(iGISSharedConstants.GISDefaultsMandatoryProperty)).Trim() = "" Then
                                        iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ERROR:No mandatory property has been defined in the Defaults XML file for the object:" & sCurrentObjectName, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                                        Return gPMConstants.PMEReturnCode.PMFalse
                                    End If

                                    'What is the mandatory property first?

                                    sMandatoryPropertyName = CStr(oCurrentObject.GetAttribute(iGISSharedConstants.GISDefaultsMandatoryProperty))

                                    'Get the value of the mandatory property from this object
                                    'instance in the dataset and if it is "" do not apply
                                    'default value to this instance (as it's just an emplt slot)
                                    lReturn = CType(GetPropertyValue(v_sObjectName:=sCurrentObjectName, v_sPropertyName:=sMandatoryPropertyName, v_sOIKey:=sCurrentObjectKey, r_vPropertyValue:=sMandatoryPropertyValue, r_bIsAssumedInfo:=bIsAssumedInfo), gPMConstants.PMEReturnCode)
                                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        Return gPMConstants.PMEReturnCode.PMFalse
                                    End If
                                    If sMandatoryPropertyValue.Trim() <> "" Then

                                        'There is a mandatory property value, so next check the
                                        'property we're going to default a value in hasn't already
                                        'got one - if not then apply it !
                                        lReturn = CType(GetPropertyValue(v_sObjectName:=sCurrentObjectName, v_sPropertyName:=sCurrentPropertyName, v_sOIKey:=sCurrentObjectKey, r_vPropertyValue:=sCurrentPropertyValue, r_bIsAssumedInfo:=bIsAssumedInfo), gPMConstants.PMEReturnCode)
                                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                            Return gPMConstants.PMEReturnCode.PMFalse
                                        End If

                                        If sCurrentPropertyValue.Trim() = "" Then

                                            lReturn = CType(SetPropertyValue(sCurrentObjectName, sCurrentPropertyName, sCurrentObjectKey, sCurrentDefaultValue, False), gPMConstants.PMEReturnCode)
                                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                Return gPMConstants.PMEReturnCode.PMFalse
                                            End If

                                            'Log a DEBUG message that we've just set a default value
                                            iGISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="The following property default in the Defaults XML file has been applied - Object:" & sCurrentObjectName & "(" & CStr(lInstanceCounter + 1) & ") Property:" & sCurrentPropertyName & " Value:" & sCurrentDefaultValue & "     ", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                                        Else
                                            'Log a DEBUG message that current property value is NOT "" so this default has been ignored
                                            iGISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Warning:The following property default in the Defaults XML file has NOT been applied since the current property already has a value (of " & sCurrentPropertyValue & ") - Object:" & sCurrentObjectName & "(" & CStr(lInstanceCounter + 1) & ") Property:" & sCurrentPropertyName & " Value:" & sCurrentDefaultValue & "     ", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                                        End If
                                    Else
                                        'Log a DEBUG message that Mandatory property is "" so this default has been ignored
                                        iGISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Warning:The following property default in the Defaults XML file has NOT been applied since the specified mandatory property value for this object (" & sMandatoryPropertyName & ") was not set - Object:" & sCurrentObjectName & "(" & CStr(lInstanceCounter + 1) & ") Property:" & sCurrentPropertyName & " Value:" & sCurrentDefaultValue & "     ", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                                    End If

                                Next

                            Else

                                'Object doesn't exist so ignore (but log)
                                iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Warning:The following property default in the Defaults XML file has NOT been applied since an object instance did not exist to apply it to - Object:" & sCurrentObjectName & " Property:" & sCurrentPropertyName & " Value:" & sCurrentDefaultValue & "     ", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                            End If

                        Else
                            'Current object has no matching property and default value pairs i.e. it
                            'does NOT have a <PROPERTY_NAME> and <DEFAULT_VALUE> defined for it.
                            'Error and exit
                            iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error:The following object in the Defaults XML file has no matching <PROPERTY_NAME> and <DEFAULT_VALUE> values defined:" & sCurrentObjectName, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Next
                Else
                    'Log a DEBUG message that Defaults file was found but no objects defined in it, exit and continue
                    iGISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Warning:The following Defaults XML file was found but had no object definitions in:" & sGISDefaultsFile, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                End If
            Next

            iGISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadAndApplyGISDefaults - End", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            ' Release References
            oObject = Nothing
            oProperty = Nothing
            oCurrentObject = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadAndApplyGISDefaultsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetMandatoryPropertyName
    '
    ' Description: Attempt to find the mandatory property for a given
    '              object from the Defaults XML Document file. If no
    '              file is found, no object definition is found or no
    '              mandatory property is defined then return as error.
    '
    ' Created    : 210301 CJB
    '
    ' ***************************************************************** '
    Public Function GetMandatoryPropertyName(ByVal v_sObjectName As String, ByRef r_sMandatoryPropertyName As String) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Dim sGISDefaultsFile As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode

        Dim oElements As XmlNodeList
        Dim oObject As XmlElement

        Try

            'Load the data model specific Defaults XML document data into memory if not done already
            If m_oDefaults Is Nothing Then

                'Determine the name (and path) to the current solution specific GIS
                'defaults xml document
                lReturn = CType(iGISSharedConstants.GetDefaultsFileName(GISDataModelCode, sGISDefaultsFile), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Check if the file actually exists - if not then error & exit
                If FileSystem.Dir(sGISDefaultsFile, FileAttribute.Normal) = "" Then
                    iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error:Defaults XML file not found at " & sGISDefaultsFile & ". Note this is required in order for QMM Cobol Linkage to be correctly populated with 'real' instances only and not object instances that are defaults (or slots).", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMandatoryPropertyName", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Load the data model specific Defaults XML document data into memory
                lReturn = CType(LoadDefaultsXMLFile(v_sGISDefaultsFile:=sGISDefaultsFile), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Now use the MSXML2.dll functions on the XML defaults document we have in memory

            'Return the elements for the given object name
            oElements = m_oDefaults.GetElementsByTagName(v_sObjectName.ToUpper())

            'Check one was found - if not, error and exit.
            If oElements.Count < 1 Then

                iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ERROR:No object has been defined in the Defaults XML file for:" & v_sObjectName & ". Note this is required in order for QMM Cobol Linkage to be correctly populated with 'real' instances only and not object instances that are defaults (or slots).", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'We set this object to be able to get the Mandatory property attribute
            oObject = oElements.Item(0)

            'Check there is a mandatory property defined for this object in the Defaults
            'xml file - if none, error and exit

            Dim auxVar As Object = oObject.GetAttribute(iGISSharedConstants.GISDefaultsMandatoryProperty)


            If Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar) Or CStr(oObject.GetAttribute(iGISSharedConstants.GISDefaultsMandatoryProperty)).Trim() = "" Then
                iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ERROR:No mandatory property has been defined in the Defaults XML file for the object:" & v_sObjectName, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get and return the mandatory property

            r_sMandatoryPropertyName = CStr(oObject.GetAttribute(iGISSharedConstants.GISDefaultsMandatoryProperty))

            'Release resources
            oElements = Nothing
            oObject = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMandatoryPropertyNameFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMandatoryPropertyName", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    Function GetSQLType(ByRef lDataType As Integer) As String
        Dim result As String = String.Empty
        Select Case lDataType
            ' Dates will be passed as strings and converted by the SP
            Case iGISSharedConstants.GISDataTypeDate
                result = "DATETIME"
            Case iGISSharedConstants.GISDataTypeNumericOutput, iGISSharedConstants.GISDataTypeCurrency
                result = "NUMERIC(19,4)"
            Case iGISSharedConstants.GISDataTypeShortList, iGISSharedConstants.GISDataTypeLongList, iGISSharedConstants.GISDataTypeText
                result = "VARCHAR(255)"
            Case iGISSharedConstants.GISDataTypeComment
                result = "VARCHAR(4000)"
            Case iGISSharedConstants.GISDataTypeShortListCode, iGISSharedConstants.GISDataTypeLongListCode
                result = "VARCHAR(70)"
            Case iGISSharedConstants.GISDataTypeOption
                result = "TINYINT"
            Case iGISSharedConstants.GISDataTypePercentage
                result = "NUMERIC(7,4)"
            Case iGISSharedConstants.GISDataTypeNumeric, iGISSharedConstants.GISDataTypeInteger
                result = "INT"
            Case Else
                result = "VARCHAR(255)"
        End Select

        Return result
    End Function


    ' RFC290103
    '**************************************************************************
    ' Name: BuildAssocClientSelSP
    '
    ' Description: Build a Select Stored Procedure for
    '              Associated Client and Disclosure Data.
    '
    ' History: ???????? - Created
    '          PW280803 - CQ1912 - use ins_file_cnt instead of folder_cnt
    '                     (initial changes devised by RFC)
    '          PW250903 - various AC CQ's - tweaks to SP
    ' RAM20040831  : Removed all the code, related to dropping of Stored procedures
    '**************************************************************************
    Private Function BuildAssocClientSelSP(ByVal v_sObjectName As String, ByVal v_sTableName As String, ByRef vPropertyArray(,) As Object, ByVal sDiscObj As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        ' Object Definition
        Dim vDiscPropertyArray(,) As Object = Nothing
        Dim sPropertyName As String = ""
        Dim sColumnName As String = ""
        Dim iIsPrimaryKey As gPMConstants.PMEReturnCode

        Dim sParentTableName As String = ""
        Dim sOIKey As String = ""
        Dim lDataType As Integer = 0

        ' Used to Create the SP
        Dim iFileNum As Integer
        Dim sSPName As String = ""
        Dim sSPFileName As String = ""

        Dim sLoadSPPath As String = ""
        Dim sPathTest As String = ""

        Dim sSQL As String = ""
        Dim sTag1Disc As New StringBuilder
        Dim sTag2Disc As New StringBuilder
        Dim sTag2AC As New StringBuilder
        Dim sTag1AC As New StringBuilder

        Dim sDiscTable As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Build the Associated Client strings

        ' Loop Round the Properties
        For lRow As Integer = vPropertyArray.GetLowerBound(1) To vPropertyArray.GetUpperBound(1)


            sPropertyName = CStr(vPropertyArray(0, lRow))

            lReturn = CType(GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, r_lDataType:=lDataType, r_sColumnName:=sColumnName, r_iIsPrimaryKey:=iIsPrimaryKey), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            sColumnName = sColumnName.ToUpper()
            sPropertyName = sPropertyName.ToUpper()

            ' Ignore the Primary Keys as they have already been included in the Static Header Object (Policy_Client)
            If iIsPrimaryKey <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Ignore the Static Fields
                If Not IsAssocClientStaticColumn(sColumnName) Then

                    ' If the column is datetime then we need to covert it into ODBC conical format
                    If lDataType = iGISSharedConstants.GISDataTypeDate Then
                        sColumnName = "convert(varchar(30)," & sColumnName & ",120)"
                    End If

                    sTag1AC.Append("           " & sColumnName & "                  As [" & v_sObjectName & "!2!" & sPropertyName & "]," & Strings.ChrW(13) & Strings.ChrW(10))
                    sTag2AC.Append("           NULL                       As [" & v_sObjectName & "!2!" & sPropertyName & "]," & Strings.ChrW(13) & Strings.ChrW(10))

                End If

            End If

        Next lRow

        ' Get the Object Definition Details
        lReturn = CType(GetObjectDefDetails(v_sObjectName:=sDiscObj, r_sTableName:=sDiscTable, r_vPropertyArray:=vDiscPropertyArray), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' Build the Disclosure strings

        ' Loop Round the Properties

        For lRow As Integer = vDiscPropertyArray.GetLowerBound(1) To vDiscPropertyArray.GetUpperBound(1)


            sPropertyName = CStr(vDiscPropertyArray(0, lRow))

            lReturn = CType(GetPropertyDefDetails(v_sObjectName:=sDiscObj, v_sPropertyName:=sPropertyName, r_lDataType:=lDataType, r_sColumnName:=sColumnName, r_iIsPrimaryKey:=iIsPrimaryKey), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            sColumnName = sColumnName.ToUpper()
            sPropertyName = sPropertyName.ToUpper()

            ' Ignore the Primary Keys as they have already been included in the Static Header Object (Policy_Client)
            If iIsPrimaryKey <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Ignore the Static Fields
                If Not IsAssocClientStaticColumn(sColumnName) Then

                    ' If the column is datetime then we need to covert it into ODBC conical format
                    If lDataType = iGISSharedConstants.GISDataTypeDate Then
                        sColumnName = "convert(varchar(30)," & sColumnName & ",120)"
                    End If

                    sTag2Disc.Append("           " & sColumnName & "                  As [" & sDiscObj & "!3!" & sPropertyName & "]")
                    sTag1Disc.Append("           NULL                       As [" & sDiscObj & "!3!" & sPropertyName & "]")

                    If lRow <> vDiscPropertyArray.GetUpperBound(1) Then
                        sTag2Disc.Append("," & Strings.ChrW(13) & Strings.ChrW(10))
                        sTag1Disc.Append("," & Strings.ChrW(13) & Strings.ChrW(10))
                    Else
                        sTag2Disc.Append(Strings.ChrW(13) & Strings.ChrW(10))
                        sTag1Disc.Append(Strings.ChrW(13) & Strings.ChrW(10))
                    End If

                End If

            End If

        Next lRow


        ' RAW 30/09/2003 : CQ2673 : remove last comma if necessary
        ' RAW 15/10/2004 : CQ5119 : removed code that removes last comma from sTag1AC and sTag2AC
        If sTag1Disc.ToString().Substring(sTag1Disc.ToString().Length - 3) = "," & Strings.ChrW(13) & Strings.ChrW(10) Then
            sTag1Disc = New StringBuilder(sTag1Disc.ToString().Substring(0, sTag1Disc.ToString().Length - 3) & Strings.ChrW(13) & Strings.ChrW(10))
        End If
        If sTag2Disc.ToString().Substring(sTag2Disc.ToString().Length - 3) = "," & Strings.ChrW(13) & Strings.ChrW(10) Then
            sTag2Disc = New StringBuilder(sTag2Disc.ToString().Substring(0, sTag2Disc.ToString().Length - 3) & Strings.ChrW(13) & Strings.ChrW(10))
        End If
        ' RAW 30/09/2003 : CQ2673 : end


        ' RAW 20/06/2003 : CQ786 : replaced Disclosure column aliases to match property names instead of column names

        ' RAM20040831 : Commented the following lines of code
        'sSQL = sSQL & "SET QUOTED_IDENTIFIER OFF" & vbCrLf
        'sSQL = sSQL & vbCrLf
        'sSQL = sSQL & "GO" & vbCrLf
        'sSQL = sSQL & "SET ANSI_NULLS ON" & vbCrLf
        'sSQL = sSQL & "GO" & vbCrLf
        'sSQL = sSQL & vbCrLf
        ''sSQL = sSQL & "EXEC DDLDropProcedure 'spg_DMC_Associated_Client_sel'" & vbCrLf
        'sSQL = sSQL & "EXEC DDLDropProcedure 'spg_" & v_sTableName & "_sel'" & vbCrLf
        'sSQL = sSQL & "GO" & vbCrLf
        'sSQL = sSQL & vbCrLf
        ''sSQL = sSQL & "CREATE PROCEDURE spg_DMC_Associated_Client_sel" & vbCrLf

        sSQL = sSQL & "CREATE PROCEDURE spg_" & v_sTableName & "_sel" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    @gis_policy_link_id INTEGER" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "AS" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "BEGIN" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "/* Local Variables */" & Strings.ChrW(13) & Strings.ChrW(10)
        '    sSQL = sSQL & "declare @disclosure_type_id int" & vbCrLf   ' RAW 20/06/2003 : CQ786 : removed
        sSQL = sSQL & "declare @insurance_folder_cnt integer" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "declare @risk_cnt integer" & Strings.ChrW(13) & Strings.ChrW(10)
        ' PW280803 - CQ1912: Start - add variables
        sSQL = sSQL & "declare @insurance_file_cnt integer" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "declare @cover_start_date datetime" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "declare @risk_type_id integer" & Strings.ChrW(13) & Strings.ChrW(10)
        ' PW280803 - CQ1912: End
        sSQL = sSQL & "" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "/* Get the Insurance Folder and Risk Cnt */" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "SELECT @insurance_folder_cnt = insurance_folder_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
        ' PW280803 - CQ1912: Start - select ins_file_cnt
        sSQL = sSQL & "       @risk_cnt = risk_id," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "       @insurance_file_cnt = insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
        ' PW280803 - CQ1912: End
        sSQL = sSQL & "FROM gis_policy_link" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "WHERE  gis_policy_link_id = @gis_policy_link_id" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "" & Strings.ChrW(13) & Strings.ChrW(10)
        ' PW280803 - CQ1912: Start - check ins_file_cnt too
        sSQL = sSQL & "IF (@insurance_folder_cnt IS NULL OR @risk_cnt IS NULL OR @insurance_file_cnt IS NULL)" & Strings.ChrW(13) & Strings.ChrW(10)
        ' PW280803 - CQ1912: End
        sSQL = sSQL & "    BEGIN" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        /* Return NOT FOUND */" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        SELECT" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            1 as tag," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            Null as parent," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            Null                       As [HEADER!1]" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        FOR XML explicit" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        RETURN -100" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    END" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "" & Strings.ChrW(13) & Strings.ChrW(10)
        ' RAW 20/06/2003 : CQ786 : removed
        '    sSQL = sSQL & "SELECT @disclosure_type_id = dtrt.disclosure_type_id" & vbCrLf
        '    sSQL = sSQL & "FROM   disclosure_type_risk_type dtrt" & vbCrLf
        '    sSQL = sSQL & "INNER JOIN risk r on r.risk_type_id = dtrt.risk_type_id" & vbCrLf
        '    sSQL = sSQL & "WHERE      r.risk_cnt = @risk_cnt" & vbCrLf
        ' RAW 20/06/2003 : CQ786 : end
        ' PW280803 - CQ1912: Start - get details for later use
        sSQL = sSQL & "-- Need to get the cover_start_date from the Policy Version so that we" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "-- can filter the disclosures (party_convictions) by their effective_date" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "select  @cover_start_date = cover_start_date" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "From insurance_file" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "where insurance_file_cnt = @insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "-- Get the risk type for the Risk that we are editing, so we can filter the" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "-- Global disclosures (non risk specific) to only show ones that apply" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "select @risk_type_id = risk_type_id" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "From Risk" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "where  risk_cnt = @risk_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
        ' PW280803 - CQ1912: End
        sSQL = sSQL & "" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "select" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           1                          As Tag ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                       As Parent ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                       As [HEADER!1]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                       As [" & v_sObjectName & "!2!OI]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                       As [" & v_sObjectName & "!2!US]," & Strings.ChrW(13) & Strings.ChrW(10)
        ' PW280803 - CQ1912 - change folder to file
        sSQL = sSQL & "           Null                       As [" & v_sObjectName & "!2!INSURANCE_FILE_CNT]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                       As [" & v_sObjectName & "!2!PARTY_CNT]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                       As [" & v_sObjectName & "!2!RISK_CNT]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                       As [" & v_sObjectName & "!2!IS_INSURED]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                       As [" & v_sObjectName & "!2!PARTY_TITLE_CODE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                       As [" & v_sObjectName & "!2!FORENAME]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                       As [" & v_sObjectName & "!2!NAME]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                       As [" & v_sObjectName & "!2!RESOLVED_NAME]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                       As [" & v_sObjectName & "!2!INITIALS]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                       As [" & v_sObjectName & "!2!DATE_OF_BIRTH]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                       As [" & v_sObjectName & "!2!GENDER_CODE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                       As [" & v_sObjectName & "!2!PARTY_TYPE_CODE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                       As [" & v_sObjectName & "!2!PARTY_TYPE_DESCRIPTION]," & Strings.ChrW(13) & Strings.ChrW(10)
        '    sSQL = sSQL & "           ac_field1                  As [DMC_Associated_Client!2!ac_field1]," & vbCrLf
        '    sSQL = sSQL & "           ac_field2                  As [DMC_Associated_Client!2!ac_field2]," & vbCrLf
        sSQL = sSQL & sTag2AC.ToString()
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!OI]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!US]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!PARTY_CNT]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!PARTY_CONVICTION_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!CONVICTION_TYPE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!CONVICTION_DATE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!DESCRIPTION]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!FINE_AMT]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!SENTENCE_TYPE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!SENTENCE_DESCRIPTION]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!SENTENCE_DURATION]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!TIME_UNIT]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!SENTENCE_EFFECTIVE_DATE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!CONVICTION_STATUS]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!ALCOHOL_LEVEL]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!ALCOHOL_MEASUREMENT_METHOD]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!DRIVING_LICENCE_PENALTY_PTS]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!DISCLOSURE_TYPE]," & Strings.ChrW(13) & Strings.ChrW(10)
        ' PW280803 - CQ1912 - add new 'fixed' fields: Start
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!EFFECTIVE_DATE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!EXPIRY_DATE]," & Strings.ChrW(13) & Strings.ChrW(10)
        ' RAW 30/09/2003 : CQ2673 : prevent dangling comma when sTag1Disc is empty
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!REPLACED_BY_ID]"
        ' PW280803 - CQ1912: end
        If sTag1Disc.ToString() <> "" Then
            sSQL = sSQL & "," & Strings.ChrW(13) & Strings.ChrW(10) & sTag1Disc.ToString()
        Else
            sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        End If
        ' RAW 30/09/2003 : CQ2673 : end
        sSQL = sSQL & "" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "Union All" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "select" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           2              As Tag ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           1           As Parent ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                       As [HEADER!1]," & Strings.ChrW(13) & Strings.ChrW(10)
        ' PW280803 - CQ1912 - change folder to file
        sSQL = sSQL & "           'IF' + convert(varchar(15),policy_client.insurance_file_cnt) + 'P' + convert(varchar(15),policy_client.party_cnt) As [" & v_sObjectName & "!2!OI]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           0                          As [" & v_sObjectName & "!2!US]," & Strings.ChrW(13) & Strings.ChrW(10)
        ' PW280803 - CQ1912 - change folder to file
        sSQL = sSQL & "           policy_client.insurance_file_cnt       As [" & v_sObjectName & "!2!INSURANCE_FILE_CNT]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           policy_client.party_cnt    As [" & v_sObjectName & "!2!PARTY_CNT]," & Strings.ChrW(13) & Strings.ChrW(10)
        ' RAW 30/09/2003 : CQ2673 : return variable risk_cnt instead of policy_client which may be null
        sSQL = sSQL & "           @risk_cnt                  As [" & v_sObjectName & "!2!RISK_CNT]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           is_insured                 As [" & v_sObjectName & "!2!IS_INSURED]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           ppc.party_title_code       As [" & v_sObjectName & "!2!PARTY_TITLE_CODE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           ppc.forename               As [" & v_sObjectName & "!2!FORENAME]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           p.name                     As [" & v_sObjectName & "!2!NAME]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           p.resolved_name            As [" & v_sObjectName & "!2!RESOLVED_NAME]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           ppc.initials               As [" & v_sObjectName & "!2!INITIALS]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           convert(varchar(30),pl.date_of_birth,120)           As [" & v_sObjectName & "!2!DATE_OF_BIRTH]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           pl.gender_code             As [" & v_sObjectName & "!2!GENDER_CODE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           pt.code                    As [" & v_sObjectName & "!2!PARTY_TYPE_CODE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           pt.description            As [" & v_sObjectName & "!2!PARTY_TYPE_DESCRIPTION]," & Strings.ChrW(13) & Strings.ChrW(10)
        '    sSQL = sSQL & "           ac_field1                  As [DMC_Associated_Client!2!ac_field1]," & vbCrLf
        '    sSQL = sSQL & "           ac_field2                  As [DMC_Associated_Client!2!ac_field2]," & vbCrLf
        sSQL = sSQL & sTag1AC.ToString()
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!OI]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!US]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!PARTY_CNT]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!PARTY_CONVICTION_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!CONVICTION_TYPE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!CONVICTION_DATE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!DESCRIPTION]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!FINE_AMT]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!SENTENCE_TYPE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!SENTENCE_DESCRIPTION]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!SENTENCE_DURATION]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!TIME_UNIT]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!SENTENCE_EFFECTIVE_DATE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!CONVICTION_STATUS]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!ALCOHOL_LEVEL]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!ALCOHOL_MEASUREMENT_METHOD]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!DRIVING_LICENCE_PENALTY_PTS]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!DISCLOSURE_TYPE]," & Strings.ChrW(13) & Strings.ChrW(10)
        ' PW280803 - CQ1912 - add new 'fixed' fields: Start
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!EFFECTIVE_DATE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!EXPIRY_DATE]," & Strings.ChrW(13) & Strings.ChrW(10)
        ' RAW 30/09/2003 : CQ2673 : prevent dangling comma when sTag1Disc is empty
        sSQL = sSQL & "           Null               As [" & sDiscObj & "!3!REPLACED_BY_ID]"
        ' PW280803 - CQ1912: end
        If sTag1Disc.ToString() <> "" Then
            sSQL = sSQL & "," & Strings.ChrW(13) & Strings.ChrW(10) & sTag1Disc.ToString()
        Else
            sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        End If
        ' RAW 30/09/2003 : CQ2673 : end
        sSQL = sSQL & "from policy_client as policy_client" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "inner join party p on policy_client.party_cnt = p.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "inner join party_type pt on pt.party_type_id = p.party_type_id" & Strings.ChrW(13) & Strings.ChrW(10)
        ' PW050803 - CQ1793 - left join to personal_client and lifestyle
        ' tables, as not all policy_clients are personal clients
        sSQL = sSQL & "left join party_personal_client ppc on policy_client.party_cnt = ppc.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "left join party_lifestyle pl on policy_client.party_cnt = pl.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
        ' RAW 15/10/2003 : CQ2847 : added to ignore dependants rows
        sSQL = sSQL & "                            and pl.party_lifestyle_id = 1" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "left outer join " & v_sTableName & " on policy_client.party_cnt = " & v_sTableName & ".party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
        ' PW280803 - CQ1912 - change folder to file
        sSQL = sSQL & "and policy_client.insurance_file_cnt = " & v_sTableName & ".insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
        ' PW280803 - CQ1912 - and where risk_cnt matches ass_cli risk_cnt
        sSQL = sSQL & "and @risk_cnt = " & v_sTableName & ".risk_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
        ' PW280803 - CQ1912 - change folder to file
        sSQL = sSQL & "where policy_client.insurance_file_cnt = @insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
        ' RAW 30/09/2003 : CQ2673 : modified
        ' sSQL = sSQL & "AND (policy_client.risk_cnt IS NULL OR policy_client.risk_cnt = @risk_cnt)" & vbCrLf
        sSQL = sSQL & "AND  (" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        (    policy_client.is_insured <> 0" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "         AND policy_client.risk_cnt IS NULL)" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        OR" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        (    policy_client.risk_cnt = @risk_cnt)" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        OR" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        (    " & v_sTableName & ".risk_cnt = @risk_cnt)" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      )" & Strings.ChrW(13) & Strings.ChrW(10)
        ' RAW 30/09/2003 : CQ2673 : end
        sSQL = sSQL & "" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "Union All" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "/* Global Disclosures" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "For All Insured Clients on the current Policy, look for Global Disclosures" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "that are applicable to the current Risk Type (Disclosure Types match)." & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "Global Disclosures are those that have been disclosed by the insured" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "but are NOT specified against a single RISK instance. */" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "SELECT" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           3                       As Tag," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           2                       As Parent," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                    As [HEADER!1]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                    As [" & v_sObjectName & "!2!OI]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                    As [" & v_sObjectName & "!2!US]," & Strings.ChrW(13) & Strings.ChrW(10)
        ' PW280803 - CQ1912 - change folder to file
        sSQL = sSQL & "           Null                    As [" & v_sObjectName & "!2!INSURANCE_FILE_CNT]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           policy_client.party_cnt As [" & v_sObjectName & "!2!PARTY_CNT!hide]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                    As [" & v_sObjectName & "!2!RISK_CNT]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                    As [" & v_sObjectName & "!2!IS_INSURED]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                    As [" & v_sObjectName & "!2!PARTY_TITLE_CODE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                    As [" & v_sObjectName & "!2!FORENAME]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                    As [" & v_sObjectName & "!2!NAME]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                    As [" & v_sObjectName & "!2!RESOLVED_NAME]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                    As [" & v_sObjectName & "!2!INITIALS]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                    As [" & v_sObjectName & "!2!DATE_OF_BIRTH]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                    As [" & v_sObjectName & "!2!GENDER_CODE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                    As [" & v_sObjectName & "!2!PARTY_TYPE_CODE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                    As [" & v_sObjectName & "!2!PARTY_TYPE_DESCRIPTION]," & Strings.ChrW(13) & Strings.ChrW(10)
        '    sSQL = sSQL & "           Null                   As [{DMC}_Associated_Client!2!ac_field1]," & vbCrLf
        '    sSQL = sSQL & "           Null                   As [{DMC}_Associated_Client!2!ac_field2]," & vbCrLf
        sSQL = sSQL & sTag2AC.ToString()
        sSQL = sSQL & "           'P' + convert(varchar(15),pcv.party_cnt) + 'PC' + convert(varchar(15),pcv.party_conviction_id) As [" & sDiscObj & "!3!OI]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           0                       As [" & sDiscObj & "!3!US]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           pcv.party_cnt           As [" & sDiscObj & "!3!PARTY_CNT]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           pcv.party_conviction_id As [" & sDiscObj & "!3!PARTY_CONVICTION_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           code                    As [" & sDiscObj & "!3!CONVICTION_TYPE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           conviction_date         As [" & sDiscObj & "!3!CONVICTION_DATE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           pcv.description         As [" & sDiscObj & "!3!DESCRIPTION]," & Strings.ChrW(13) & Strings.ChrW(10) ' RAW 20/06/2003 : CQ786 : added qualifier
        sSQL = sSQL & "           fine_amt                As [" & sDiscObj & "!3!FINE_AMT]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           sentence_code           As [" & sDiscObj & "!3!SENTENCE_TYPE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           sentence_description    As [" & sDiscObj & "!3!SENTENCE_DESCRIPTION]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           sentence_duration   As [" & sDiscObj & "!3!SENTENCE_DURATION]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           sentence_duration_qualifier As [" & sDiscObj & "!3!TIME_UNIT]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           sentence_effective_date As [" & sDiscObj & "!3!SENTENCE_EFFECTIVE_DATE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           status_code             As [" & sDiscObj & "!3!CONVICTION_STATUS]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           alcohol_level           As [" & sDiscObj & "!3!ALCOHOL_LEVEL]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           alcohol_measurement_method As [" & sDiscObj & "!3!ALCOHOL_MEASUREMENT_METHOD]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           driving_licence_penalty_pts As [" & sDiscObj & "!3!DRIVING_LICENCE_PENALTY_PTS]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           pcv.disclosure_type_id  As [" & sDiscObj & "!3!DISCLOSURE_TYPE]," & Strings.ChrW(13) & Strings.ChrW(10) ' RAW 20/06/2003 : CQ786 : added qualifier
        ' PW280803 - CQ1912 - add new 'fixed' fields: Start
        sSQL = sSQL & "           CONVERT(CHAR(20),pcv.effective_date,120) As [" & sDiscObj & "!3!EFFECTIVE_DATE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           CONVERT(CHAR(20),pcv.expiry_date,120)    As [" & sDiscObj & "!3!EXPIRY_DATE]," & Strings.ChrW(13) & Strings.ChrW(10)
        ' RAW 30/09/2003 : CQ2673 : prevent dangling comma when sTag2Disc is empty
        sSQL = sSQL & "           pcv.replaced_by_id      As [" & sDiscObj & "!3!REPLACED_BY_ID]"
        ' PW280803 - CQ1912: end
        If sTag2Disc.ToString() <> "" Then
            sSQL = sSQL & "," & Strings.ChrW(13) & Strings.ChrW(10) & sTag2Disc.ToString()
        Else
            sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        End If
        ' RAW 30/09/2003 : CQ2673 : end
        sSQL = sSQL & "from party_conviction pcv" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "inner join policy_client on pcv.party_cnt = policy_client.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
        ' RAW 15/10/2004 : CQ5119 : replaced inner join with left outer
        sSQL = sSQL & "LEFT OUTER join " & sDiscTable & " on pcv.party_cnt = " & sDiscTable & ".party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "          and pcv.party_conviction_id = " & sDiscTable & ".party_conviction_id" & Strings.ChrW(13) & Strings.ChrW(10)
        ' PW280803 - CQ1912 - change folder to file
        sSQL = sSQL & "where policy_client.insurance_file_cnt = @insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
        ' PW050803 - CQ2111 - need to pick up non-insured as well as insured
        ' where risk_cnt is null. This is to include disclosures where the
        ' party has been deleted and gone from insured to non-insured.
        'sSQL = sSQL & "and   policy_client.is_insured = 1" & vbCrLf
        sSQL = sSQL & "and   pcv.risk_cnt IS NULL" & Strings.ChrW(13) & Strings.ChrW(10)
        '    sSQL = sSQL & "and   pcv.disclosure_type_id = @disclosure_type_id" & vbCrLf    ' RAW 20/06/2003 : CQ786 : removed
        ' PW280803 - CQ1912: Start - extra where clauses
        sSQL = sSQL & "-- Party_conviction MUST be effective based on the Cover_start_date of this Policy Version" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "and   pcv.effective_date <= @cover_start_date" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "and   (pcv.expiry_date > @cover_start_date OR pcv.expiry_date IS NULL)" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "-- Filter the disclosures (party_convictions) to only show ones that Apply to the Risk being edited" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "and   pcv.disclosure_type_id IN (select disclosure_type_id from disclosure_type_risk_type where risk_type_id = @risk_type_id)" & Strings.ChrW(13) & Strings.ChrW(10)
        ' PW280803 - CQ1912: End
        sSQL = sSQL & "" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "Union All" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "/* Risk Disclosures" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "For All Clients on the current Policy (Insured or NOT), look for Risk Disclosures" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "that are applicable to the current Risk Type (Disclosure Types match)." & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "Risk Disclosures are those that have been disclosed" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "and are specified against a single RISK instance. */" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "SELECT" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           3                       As Tag," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           2                       As Parent," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                       As [HEADER!1]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                    As [" & v_sObjectName & "!2!OI]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                    As [" & v_sObjectName & "!2!US]," & Strings.ChrW(13) & Strings.ChrW(10)
        ' PW280803 - CQ1912 - change folder to file
        sSQL = sSQL & "           Null                    As [" & v_sObjectName & "!2!INSURANCE_FILE_CNT]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           policy_client.party_cnt As [" & v_sObjectName & "!2!PARTY_CNT!hide]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                    As [" & v_sObjectName & "!2!RISK_CNT]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                    As [" & v_sObjectName & "!2!IS_INSURED]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                    As [" & v_sObjectName & "!2!PARTY_TITLE_CODE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                    As [" & v_sObjectName & "!2!FORENAME]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                    As [" & v_sObjectName & "!2!NAME]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                    As [" & v_sObjectName & "!2!RESOLVED_NAME]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                    As [" & v_sObjectName & "!2!INITIALS]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                    As [" & v_sObjectName & "!2!DATE_OF_BIRTH]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                    As [" & v_sObjectName & "!2!GENDER_CODE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                    As [" & v_sObjectName & "!2!PARTY_TYPE_CODE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           Null                    As [" & v_sObjectName & "!2!PARTY_TYPE_DESCRIPTION]," & Strings.ChrW(13) & Strings.ChrW(10)
        '    sSQL = sSQL & "           Null                    As [{DMC}_Associated_Client!2!ac_field1]," & vbCrLf
        '    sSQL = sSQL & "           Null                    As [{DMC}_Associated_Client!2!ac_field2]," & vbCrLf
        sSQL = sSQL & sTag2AC.ToString()
        sSQL = sSQL & "           'P' + convert(varchar(15),pcv.party_cnt) + 'PC' + convert(varchar(15),pcv.party_conviction_id) As [" & sDiscObj & "!3!OI]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           0                       As [" & sDiscObj & "!3!US]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           pcv.party_cnt           As [" & sDiscObj & "!3!PARTY_CNT]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           pcv.party_conviction_id As [" & sDiscObj & "!3!PARTY_CONVICTION_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           code                    As [" & sDiscObj & "!3!CONVICTION_TYPE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           conviction_date         As [" & sDiscObj & "!3!CONVICTION_DATE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           pcv.description         As [" & sDiscObj & "!3!DESCRIPTION]," & Strings.ChrW(13) & Strings.ChrW(10) ' RAW 20/06/2003 : CQ786 : added qualifier
        sSQL = sSQL & "           fine_amt                As [" & sDiscObj & "!3!FINE_AMT]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           sentence_code           As [" & sDiscObj & "!3!SENTENCE_TYPE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           sentence_description    As [" & sDiscObj & "!3!SENTENCE_DESCRIPTION]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           sentence_duration       As [" & sDiscObj & "!3!SENTENCE_DURATION]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           sentence_duration_qualifier As [" & sDiscObj & "!3!TIME_UNIT]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           sentence_effective_date As [" & sDiscObj & "!3!SENTENCE_EFFECTIVE_DATE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           status_code             As [" & sDiscObj & "!3!CONVICTION_STATUS]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           alcohol_level           As [" & sDiscObj & "!3!ALCOHOL_LEVEL]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           alcohol_measurement_method As [" & sDiscObj & "!3!ALCOHOL_MEASUREMENT_METHOD]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           driving_licence_penalty_pts As [" & sDiscObj & "!3!DRIVING_LICENCE_PENALTY_PTS]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           pcv.disclosure_type_id  As [" & sDiscObj & "!3!DISCLOSURE_TYPE]," & Strings.ChrW(13) & Strings.ChrW(10) ' RAW 20/06/2003 : CQ786 : added qualifier
        ' PW280803 - CQ1912 - add new 'fixed' fields: Start
        sSQL = sSQL & "           CONVERT(CHAR(20),pcv.effective_date,120) As [" & sDiscObj & "!3!EFFECTIVE_DATE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           CONVERT(CHAR(20),pcv.expiry_date,120) As [" & sDiscObj & "!3!EXPIRY_DATE]," & Strings.ChrW(13) & Strings.ChrW(10)
        ' RAW 30/09/2003 : CQ2673 : prevent dangling comma when sTag2Disc is empty
        sSQL = sSQL & "           pcv.replaced_by_id      As [" & sDiscObj & "!3!REPLACED_BY_ID]"
        ' PW280803 - CQ1912: end
        If sTag2Disc.ToString() <> "" Then
            sSQL = sSQL & "," & Strings.ChrW(13) & Strings.ChrW(10) & sTag2Disc.ToString()
        Else
            sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        End If
        ' RAW 30/09/2003 : CQ2673 : end
        sSQL = sSQL & "from party_conviction pcv" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "inner join policy_client on pcv.party_cnt = policy_client.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
        ' RAW 15/10/2004 : CQ5119 : replaced inner join with left outer
        sSQL = sSQL & "LEFT OUTER join " & sDiscTable & " on pcv.party_cnt = " & sDiscTable & ".party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "and pcv.party_conviction_id = " & sDiscTable & ".party_conviction_id" & Strings.ChrW(13) & Strings.ChrW(10)
        ' RAW 20/06/2003 : CQ786 : added
        sSQL = sSQL & "INNER JOIN risk r on pcv.risk_cnt = r.risk_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
        ' PW290803 - CQ1912 - these are risk specific - remove join to disclosure
        '    sSQL = sSQL & "INNER JOIN disclosure_type_risk_type dtrt on r.risk_type_id = dtrt.risk_type_id" & vbCrLf
        ' RAW 20/06/2003 : CQ786 : end
        ' PW280803 - CQ1912 - change folder to file
        sSQL = sSQL & "where policy_client.insurance_file_cnt = @insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "and   pcv.insurance_folder_cnt = @insurance_folder_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "and   pcv.risk_cnt = @risk_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
        ' PW290803 - CQ1912 - these are risk specific - remove join to disclosure
        '    sSQL = sSQL & "and   pcv.disclosure_type_id = dtrt.disclosure_type_id" & vbCrLf     ' RAW 20/06/2003 : CQ786 : replaced variable with table join
        sSQL = sSQL & "ORDER BY [" & v_sObjectName & "!2!party_cnt], parent" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "for xml explicit" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & " " & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "End" & Strings.ChrW(13) & Strings.ChrW(10)

        ' RAM20040831 : Commented the following line of code
        'sSQL = sSQL & "GO" & vbCrLf

        ' Build the SP Name
        sSPName = "spg_" & v_sTableName & "_sel"
        ' Build the SP File Name
        sSPFileName = sSPName & ".sql"

        ' RDC 10072001 START
        lReturn = CType(iGISSharedConstants.GetLoadSPPath(GISDataModelCode, sLoadSPPath), gPMConstants.PMEReturnCode)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' check path exists, create if not
        sPathTest = FileSystem.Dir(sLoadSPPath, FileAttribute.Directory)

        If sPathTest = "" Then
            Directory.CreateDirectory(sLoadSPPath)
        End If

        ' Get the Next File Number
        iFileNum = FileSystem.FreeFile()

        FileSystem.FileOpen(iFileNum, sLoadSPPath & "\" & sSPFileName, OpenMode.Output)
        ' RDC 10072001 END

        ' Write out the Stored Procedure
        FileSystem.PrintLine(iFileNum, sSQL)
        FileSystem.FileClose(iFileNum) ' Close file.


        Return result

    End Function



    ' RFC290103
    ' ***************************************************************** '
    ' Name: BuildAssocClientCUDSP
    '
    ' Description: Build a Create Update Delete Stored Procedure for
    '              Associated Client.
    '
    ' Edit History :
    ' RAM20040831  : Removed all the code, related to dropping of Stored procedures
    ' ***************************************************************** '
    Private Function BuildAssocClientCUDSP(ByVal v_sObjectName As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        ' Object Definition
        Dim sTableName As String = ""
        Dim vChildObjectArray As Object = Nothing
        Dim vPropertyArray(,) As Object = Nothing
        Dim sPropertyName As String = ""
        Dim sColumnName As String = ""
        Dim iIsPrimaryKey As gPMConstants.PMEReturnCode
        Dim sParentTableName As String = ""
        Dim sOIKey As String = ""
        Dim lDataType As Integer
        Dim sSQLType As String = ""

        ' Used to Create the SP
        Dim iFileNum As Integer
        Dim sSPName As String = ""
        Dim sSPFileName As String = ""
        Dim sLoadSPPath As String = ""
        Dim sPathTest As String = ""
        Dim sSQL As String
        Dim sValueList As New StringBuilder
        Dim sInsertList As New StringBuilder
        Dim sSetList As New StringBuilder
        Dim sParamList As New StringBuilder



        result = gPMConstants.PMEReturnCode.PMTrue

        sParamList = New StringBuilder("")
        sSetList = New StringBuilder("")
        sInsertList = New StringBuilder("")
        sValueList = New StringBuilder("")

        ' Get the Object Definition Details
        lReturn = CType(GetObjectDefDetails(v_sObjectName:=v_sObjectName, r_sTableName:=sTableName, r_vChildObjectArray:=vChildObjectArray, r_vPropertyArray:=vPropertyArray), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' Build the Associated Client strings

        ' Loop Round the Properties

        For lRow As Integer = vPropertyArray.GetLowerBound(1) To vPropertyArray.GetUpperBound(1)


            sPropertyName = CStr(vPropertyArray(0, lRow))

            lReturn = CType(GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, r_lDataType:=lDataType, r_sColumnName:=sColumnName, r_iIsPrimaryKey:=iIsPrimaryKey), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            sColumnName = sColumnName.ToUpper()
            sPropertyName = sPropertyName.ToUpper()

            ' Ignore the Primary Keys as they have already been included in SP Template
            If iIsPrimaryKey <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Use the data type from the gis_property
                sSQLType = GetSQLType(lDataType)

                ' Ignore the Static Fields
                If Not IsAssocClientStaticColumn(sColumnName) Then

                    ' Parameter List
                    sParamList.Append("@" & sColumnName & " As " & sSQLType & " = NULL , " & Strings.ChrW(13) & Strings.ChrW(10))

                    ' Set List
                    If lDataType = iGISSharedConstants.GISDataTypeDate Then
                        sSetList.Append(sColumnName & " = " & "CONVERT(datetime,@" & sColumnName & ",120)")
                    Else
                        sSetList.Append(sColumnName & " = @" & sColumnName)
                    End If

                    If lRow = vPropertyArray.GetUpperBound(1) Then
                        sSetList.Append(Strings.ChrW(13) & Strings.ChrW(10))
                    Else
                        sSetList.Append(" , " & Strings.ChrW(13) & Strings.ChrW(10))
                    End If

                    ' Insert List
                    sInsertList.Append(sColumnName)

                    If lRow = vPropertyArray.GetUpperBound(1) Then
                        sInsertList.Append(") " & Strings.ChrW(13) & Strings.ChrW(10))
                    Else
                        sInsertList.Append(" , " & Strings.ChrW(13) & Strings.ChrW(10))
                    End If

                    ' Value List
                    If lDataType = iGISSharedConstants.GISDataTypeDate Then
                        sValueList.Append("CONVERT(datetime,@" & sColumnName & ",120)")
                    Else
                        sValueList.Append("@" & sColumnName)
                    End If

                    If lRow = vPropertyArray.GetUpperBound(1) Then
                        sValueList.Append(") " & Strings.ChrW(13) & Strings.ChrW(10))
                    Else
                        sValueList.Append(" , " & Strings.ChrW(13) & Strings.ChrW(10))
                    End If

                End If

            End If

        Next lRow

        ' PW050903 - CQ1912 - remove last comma if necessary
        If sSetList.ToString().Substring(sSetList.ToString().Length - 5) = " , " & Strings.ChrW(13) & Strings.ChrW(10) Then
            sSetList = New StringBuilder(sSetList.ToString().Substring(0, sSetList.ToString().Length - 5) & Strings.ChrW(13) & Strings.ChrW(10))
        End If
        If sInsertList.ToString().Substring(sInsertList.ToString().Length - 5) = " , " & Strings.ChrW(13) & Strings.ChrW(10) Then
            sInsertList = New StringBuilder(sInsertList.ToString().Substring(0, sInsertList.ToString().Length - 5) & ") " & Strings.ChrW(13) & Strings.ChrW(10))
        End If
        If sValueList.ToString().Substring(sValueList.ToString().Length - 5) = " , " & Strings.ChrW(13) & Strings.ChrW(10) Then
            sValueList = New StringBuilder(sValueList.ToString().Substring(0, sValueList.ToString().Length - 5) & ") " & Strings.ChrW(13) & Strings.ChrW(10))
        End If

        sSQL = ""

        ' RAM20040831 : Commented the following lines of code
        'sSQL = sSQL & "SET QUOTED_IDENTIFIER OFF" & vbCrLf
        'sSQL = sSQL & "GO" & vbCrLf
        'sSQL = sSQL & "SET ANSI_NULLS ON" & vbCrLf
        'sSQL = sSQL & "GO" & vbCrLf
        'sSQL = sSQL & vbCrLf
        ''sSQL = sSQL & "EXEC DDLDropProcedure 'spg_DMC_Associated_Client_cud'" & vbCrLf
        'sSQL = sSQL & "EXEC DDLDropProcedure 'spg_" & sTableName & "_cud'" & vbCrLf
        'sSQL = sSQL & "GO" & vbCrLf
        'sSQL = sSQL & vbCrLf

        'sSQL = sSQL & "CREATE PROCEDURE spg_DMC_Associated_Client_cud" & vbCrLf
        sSQL = sSQL & "CREATE PROCEDURE spg_" & sTableName & "_cud" & Strings.ChrW(13) & Strings.ChrW(10)
        ' PW280803 - CQ1912 - change folder to file
        sSQL = sSQL & "  @insurance_file_cnt integer ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "  @party_cnt integer ," & Strings.ChrW(13) & Strings.ChrW(10)
        ' PW280803 - CQ1912 - add risk_cnt
        sSQL = sSQL & "  @risk_cnt integer ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "  @Is_insured As NUMERIC(19,4)  = NULL," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "  @Party_title_code As VARCHAR(70)  = NULL," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "  @Forename As VARCHAR(255)  = NULL," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "  @Name As VARCHAR(255)  = NULL," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "  @resolved_name As VARCHAR(255)  = NULL," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "  @Initials As VARCHAR(255)  = NULL," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "  @Date_Of_Birth As VARCHAR(255)  = NULL," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "  @Gender_code As VARCHAR(255)  = NULL," & Strings.ChrW(13) & Strings.ChrW(10)

        sSQL = sSQL & "  @party_type_code As VARCHAR(255)  = NULL," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "  @party_type_description As VARCHAR(255)  = NULL," & Strings.ChrW(13) & Strings.ChrW(10)

        'sSQL = sSQL & "  @ac_field1 varchar(255) ," & vbCrLf
        'sSQL = sSQL & "  @ac_field2 varchar(255) ," & vbCrLf
        sSQL = sSQL & sParamList.ToString()
        sSQL = sSQL & "  @US tinyint" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "AS" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "BEGIN" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "DECLARE @ErrNo integer" & Strings.ChrW(13) & Strings.ChrW(10)
        ' RAW 30/09/2003 : CQ2673 : added
        sSQL = sSQL & "DECLARE @iExcludePolicyClientFlag tinyint" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "SET @iExcludePolicyClientFlag = 0" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)

        sSQL = sSQL & "    /* How many risks are referencing this policy_client entry */" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    IF @US = 3 BEGIN" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        SELECT @iExcludePolicyClientFlag = " & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            CASE" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "                WHEN (count(*) > 1) THEN" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "                    1 -- do not delete from policy_client to prevent delete cascading to other risks. Will be handled later" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "                ELSE 0" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            END" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        FROM " & sTableName & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        WHERE  insurance_file_cnt = @insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "          AND  party_cnt = @party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        IF @iExcludePolicyClientFlag = 1 BEGIN" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            -- There is more than one risk hanging off this policy_client so just delete the risk concerned" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            -- Note - this will not be handled by spu_associated_client_cud later" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            DELETE FROM " & sTableName & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            WHERE  insurance_file_cnt = @insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "              AND  party_cnt = @party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "              AND  risk_cnt = @risk_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        END" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    END" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        ' RAW 30/09/2003 : CQ2673 : end

        sSQL = sSQL & "    /* Call the Header SP First */" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    EXEC spu_associated_client_cud" & Strings.ChrW(13) & Strings.ChrW(10)
        ' PW280803 - CQ1912 - change folder to file
        sSQL = sSQL & "        @insurance_file_cnt=@insurance_file_cnt ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @risk_cnt=@risk_cnt ," & Strings.ChrW(13) & Strings.ChrW(10) ' RAW 30/09/2003 : CQ2673 : added
        sSQL = sSQL & "        @party_cnt=@party_cnt ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @is_insured=@is_insured ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @party_title_code=@party_title_code ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @forename=@forename ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @name=@name ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @resolved_name=@resolved_name ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @initials=@initials ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @date_of_birth=@date_of_birth ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @gender_code=@gender_code ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @US=@US ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @exclude_policy_client=@iExcludePolicyClientFlag" & Strings.ChrW(13) & Strings.ChrW(10) ' RAW 30/09/2003 : CQ2673 : added

        sSQL = sSQL & "    SELECT @ErrNo = @@ERROR" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    /* If the Header SP Failed then return the Error */" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    IF (@ErrNo <> 0)" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        RETURN @ErrNo" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    IF @US = 0" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        Return" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    IF @US = 1" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        /* Insert */" & Strings.ChrW(13) & Strings.ChrW(10)

        'CJR 18/2/2003 do nothing if there are no values to go to the database.
        If sInsertList.ToString() <> "" And sValueList.ToString() <> "" Then
            sSQL = sSQL & "        INSERT INTO " & sTableName & Strings.ChrW(13) & Strings.ChrW(10)
            ' PW280803 - CQ1912: Start - use file instead of folder / add risk_cnt
            sSQL = sSQL & "            (insurance_file_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "             party_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "             risk_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            ' PW280803 - CQ1912: End
            'sSQL = sSQL & "                 ac_field1," & vbCrLf
            'sSQL = sSQL & "                 ac_field2)" & vbCrLf

            'CJR the insert list may be empty
            sSQL = sSQL & sInsertList.ToString()

            ' PW280803 - CQ1912: Start - use file instead of folder / add risk_cnt
            sSQL = sSQL & "        VALUES (@insurance_file_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "                @party_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "                @risk_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            ' PW280803 - CQ1912: End
            'sSQL = sSQL & "                    @ac_field1," & vbCrLf
            'sSQL = sSQL & "                    @ac_field2)" & vbCrLf

            sSQL = sSQL & sValueList.ToString()
        Else
            sSQL = sSQL & "        /*Do nothing as we have no editable attributes*/" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        return" & Strings.ChrW(13) & Strings.ChrW(10)
        End If

        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    ELSE IF @US = 2" & Strings.ChrW(13) & Strings.ChrW(10)

        'CJR 18/2/2003 do nothing if there are no values to go to the database.
        If sInsertList.ToString() <> "" And sValueList.ToString() <> "" Then

            sSQL = sSQL & "    BEGIN" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        /* Does the disclosure already exist */" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        IF EXISTS" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "          (Select *" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "           From " & sTableName & Strings.ChrW(13) & Strings.ChrW(10)
            ' PW280803 - CQ1912: Start - use file instead of folder / add risk_cnt
            sSQL = sSQL & "           Where insurance_file_cnt = @insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "             And party_cnt = @party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "             And risk_cnt = @risk_cnt)" & Strings.ChrW(13) & Strings.ChrW(10)
            ' PW280803 - CQ1912: End
            sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            /* Yes, so Update */" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            Update " & sTableName & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            SET " & Strings.ChrW(13) & Strings.ChrW(10)
            'sSQL = sSQL & "              ac_field1 = @ac_field1," & vbCrLf
            'sSQL = sSQL & "              ac_field2 = @ac_field2" & vbCrLf
            sSQL = sSQL & sSetList.ToString()
            ' PW280803 - CQ1912: Start - use file instead of folder / add risk_cnt
            sSQL = sSQL & "            WHERE  insurance_file_cnt = @insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "              AND  party_cnt = @party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "              AND  risk_cnt = @risk_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            ' PW280803 - CQ1912: End
            sSQL = sSQL & "        Else" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            /* No so Insert */" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            INSERT INTO " & sTableName & Strings.ChrW(13) & Strings.ChrW(10)
            ' PW280803 - CQ1912: Start - use file instead of folder / add risk_cnt
            sSQL = sSQL & "                (insurance_file_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "                 party_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "                 risk_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            ' PW280803 - CQ1912: End
            'sSQL = sSQL & "                 ac_field1," & vbCrLf
            'sSQL = sSQL & "                 ac_field2)" & vbCrLf
            sSQL = sSQL & sInsertList.ToString()
            ' PW280803 - CQ1912: Start - use file instead of folder / add risk_cnt
            sSQL = sSQL & "            VALUES (@insurance_file_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "                    @party_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "                    @risk_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            ' PW280803 - CQ1912: End
            'sSQL = sSQL & "                    @ac_field1," & vbCrLf
            'sSQL = sSQL & "                    @ac_field2)" & vbCrLf
            sSQL = sSQL & sValueList.ToString()
            sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    End" & Strings.ChrW(13) & Strings.ChrW(10)
        Else
            sSQL = sSQL & "        /*Do nothing as we have no editable attributes*/" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        return" & Strings.ChrW(13) & Strings.ChrW(10)
        End If

        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        Else" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "                /* Deletes are done directly via Product Builder */" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        RETURN -901" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "End" & Strings.ChrW(13) & Strings.ChrW(10)

        ' RAM20040831 : Commented the following line of code
        'sSQL = sSQL & "GO" & vbCrLf

        ' Build the SP Name
        sSPName = "spg_" & sTableName & "_cud"
        ' Build the SP File Name
        sSPFileName = sSPName & ".sql"

        ' RDC 10072001 START
        lReturn = CType(iGISSharedConstants.GetLoadSPPath(GISDataModelCode, sLoadSPPath), gPMConstants.PMEReturnCode)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' check path exists, create if not
        sPathTest = FileSystem.Dir(sLoadSPPath, FileAttribute.Directory)

        If sPathTest = "" Then
            Directory.CreateDirectory(sLoadSPPath)
        End If

        ' Get the Next File Number
        iFileNum = FileSystem.FreeFile()

        FileSystem.FileOpen(iFileNum, sLoadSPPath & "\" & sSPFileName, OpenMode.Output)
        ' RDC 10072001 END

        ' Write out the Stored Procedure
        FileSystem.PrintLine(iFileNum, sSQL)
        FileSystem.FileClose(iFileNum) ' Close file.


        Return result

    End Function



    ' RFC290103
    ' ***************************************************************** '
    ' Name: BuildDisclosureCUDSP
    '
    ' Description: Build a Create Update Delete Stored Procedure for
    '              Disclosure.
    ' Edit History  :
    ' RAM20040831   : Removed all the code, related to dropping of Stored procedures
    ' ***************************************************************** '
    Private Function BuildDisclosureCUDSP(ByVal v_sObjectName As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        ' Object Definition
        Dim sTableName As String = ""
        Dim vChildObjectArray As Object = Nothing
        Dim vPropertyArray(,) As Object = Nothing
        Dim sPropertyName As String = ""
        Dim sColumnName As String = ""
        Dim iIsPrimaryKey As gPMConstants.PMEReturnCode
        Dim sParentTableName As String = ""
        Dim sOIKey As String = ""

        Dim lDataType As Integer
        Dim sSQLType As String = ""
        ' Used to Create the SP
        Dim iFileNum As Integer
        Dim sSPName As String = ""
        Dim sSPFileName As String = ""
        Dim sLoadSPPath As String = ""
        Dim sPathTest As String = ""
        Dim sSQL As String
        Dim sValueList As New StringBuilder
        Dim sInsertList As New StringBuilder
        Dim sSetList As New StringBuilder
        Dim sParamList As New StringBuilder



        result = gPMConstants.PMEReturnCode.PMTrue

        sParamList = New StringBuilder("")
        sSetList = New StringBuilder("")
        sInsertList = New StringBuilder("")
        sValueList = New StringBuilder("")

        ' Get the Object Definition Details
        lReturn = CType(GetObjectDefDetails(v_sObjectName:=v_sObjectName, r_sTableName:=sTableName, r_vChildObjectArray:=vChildObjectArray, r_vPropertyArray:=vPropertyArray), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' Build the Associated Client strings

        ' Loop Round the Properties

        For lRow As Integer = vPropertyArray.GetLowerBound(1) To vPropertyArray.GetUpperBound(1)


            sPropertyName = CStr(vPropertyArray(0, lRow))

            lReturn = CType(GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, r_lDataType:=lDataType, r_sColumnName:=sColumnName, r_iIsPrimaryKey:=iIsPrimaryKey), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            sColumnName = sColumnName.ToUpper()
            sPropertyName = sPropertyName.ToUpper()

            ' Ignore the Primary Keys as they have already been included in SP Template
            If iIsPrimaryKey <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Use the data type from the gis_property
                sSQLType = GetSQLType(lDataType)

                ' Ignore the Static Fields
                If Not IsAssocClientStaticColumn(sColumnName) Then

                    ' Parameter List

                    'INSURANCE_FOLDER_CNT And RISK_CNT already in column list but need them everywhre else
                    If sColumnName <> "INSURANCE_FOLDER_CNT" And sColumnName <> "RISK_CNT" Then
                        sParamList.Append("@" & sColumnName & " As " & sSQLType & " = NULL , " & Strings.ChrW(13) & Strings.ChrW(10))
                    End If

                    ' Set List
                    If lDataType = iGISSharedConstants.GISDataTypeDate Then
                        sSetList.Append(sColumnName & " = " & "CONVERT(datetime,@" & sColumnName & ",120)")
                    Else
                        sSetList.Append(sColumnName & " = @" & sColumnName)
                    End If
                    ' RAW 15/10/2004 : CQ5119 : removed test for last property that added different punctuation
                    sSetList.Append(" , " & Strings.ChrW(13) & Strings.ChrW(10))

                    ' Insert List
                    sInsertList.Append(sColumnName)
                    ' RAW 15/10/2004 : CQ5119 : removed test for last property that added different punctuation
                    sInsertList.Append(" , " & Strings.ChrW(13) & Strings.ChrW(10))

                    ' Value List
                    If lDataType = iGISSharedConstants.GISDataTypeDate Then
                        sValueList.Append("CONVERT(datetime,@" & sColumnName & ",120)")
                    Else
                        sValueList.Append("@" & sColumnName)
                    End If
                    ' RAW 15/10/2004 : CQ5119 : removed test for last property that added different punctuation
                    sValueList.Append(" , " & Strings.ChrW(13) & Strings.ChrW(10))

                End If

            End If

        Next lRow

        ' RAW 15/10/2004 : CQ5119 : removed code that removed last comma

        sSQL = ""

        ' RAM20040831 : Commented the following lines(s) of code
        'sSQL = sSQL & "SET QUOTED_IDENTIFIER OFF" & vbCrLf
        'sSQL = sSQL & "GO" & vbCrLf
        'sSQL = sSQL & "SET ANSI_NULLS ON" & vbCrLf
        'sSQL = sSQL & "GO" & vbCrLf
        'sSQL = sSQL & vbCrLf
        ''sSQL = sSQL & "EXEC DDLDropProcedure 'spg_DMC_Disclosure_cud'" & vbCrLf
        'sSQL = sSQL & "EXEC DDLDropProcedure 'spg_" & sTableName & "_cud'" & vbCrLf
        'sSQL = sSQL & "GO" & vbCrLf
        ''sSQL = sSQL & "CREATE PROCEDURE spg_DMC_Disclosure_cud" & vbCrLf

        sSQL = sSQL & "CREATE PROCEDURE spg_" & sTableName & "_cud" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "  @party_cnt integer ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "  @party_conviction_id integer = NULL," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "  @code As VARCHAR(255)  = NULL," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "  @conviction_date As VARCHAR(255)  = NULL," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "  @description As VARCHAR(255)  = NULL," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "  @fine_amt As NUMERIC(19,4)  = NULL," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "  @sentence_code As VARCHAR(255)  = NULL," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "  @sentence_description As VARCHAR(255)  = NULL," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "  @sentence_duration As NUMERIC(19,4)  = NULL," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "  @sentence_duration_qualifier As VARCHAR(255)  = NULL," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "  @sentence_effective_date As VARCHAR(255)  = NULL," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "  @status_code As VARCHAR(255)  = NULL," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "  @alcohol_level As NUMERIC(19,4)  = NULL," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "  @alcohol_measurement_method As VARCHAR(255)  = NULL," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "  @driving_licence_penalty_pts As NUMERIC(19,4)  = NULL," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "  @Disclosure_Type_Id As NUMERIC(19,4)  = NULL," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "  @insurance_folder_cnt as integer = NULL," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "  @risk_cnt as integer = NULL," & Strings.ChrW(13) & Strings.ChrW(10)

        ' PW080903 - CQ1912 - new fields
        sSQL = sSQL & "  @effective_date as DATETIME = NULL," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "  @expiry_date as DATETIME = NULL," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "  @replaced_by_id as INTEGER = NULL," & Strings.ChrW(13) & Strings.ChrW(10)

        'sSQL = sSQL & "  @d_field1 varchar(255) ," & vbCrLf
        'sSQL = sSQL & "  @d_field2 varchar(255) ," & vbCrLf
        sSQL = sSQL & sParamList.ToString()
        sSQL = sSQL & "  @US tinyint" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "AS" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "BEGIN" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "DECLARE @ErrNo integer"
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)

        ' PW090903 - CQ1912
        sSQL = sSQL & "DECLARE @new_party_conviction_id integer" & Strings.ChrW(13) & Strings.ChrW(10)

        ' PW250903 - CQ2665
        sSQL = sSQL & "DECLARE @editing_flag tinyint" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "IF ISNULL(@editable_flag, '') = ''" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    SELECT @editable_flag = 0" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)

        sSQL = sSQL & "    /* Call the Header SP First */" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    EXEC spu_disclosure_cud" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @party_cnt=@party_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @party_conviction_id=@party_conviction_id OUTPUT," & Strings.ChrW(13) & Strings.ChrW(10)

        ' PW090903 - CQ1912
        sSQL = sSQL & "        @new_party_conviction_id=@new_party_conviction_id OUTPUT," & Strings.ChrW(13) & Strings.ChrW(10)

        ' PW250903 - CQ2665
        sSQL = sSQL & "        @editing_flag=@editing_flag OUTPUT," & Strings.ChrW(13) & Strings.ChrW(10)

        sSQL = sSQL & "        @code=@code," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @conviction_date=@conviction_date," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @description=@description," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @fine_amt=@fine_amt," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @sentence_code=@sentence_code," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @sentence_description=@sentence_description," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @sentence_duration=@sentence_duration," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @sentence_duration_qualifier=@sentence_duration_qualifier," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @sentence_effective_date=@sentence_effective_date," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @status_code=@status_code," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @alcohol_level=@alcohol_level," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @alcohol_measurement_method=@alcohol_measurement_method," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @driving_licence_penalty_pts=@driving_licence_penalty_pts," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @insurance_folder_cnt=@insurance_folder_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @risk_cnt=@risk_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @disclosure_type_id=@disclosure_type_id," & Strings.ChrW(13) & Strings.ChrW(10)
        ' PW080903 - CQ1912 - new fields
        sSQL = sSQL & "        @effective_date=@effective_date," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @expiry_date=@expiry_date," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @replaced_by_id=@replaced_by_id," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @editable_flag=@editable_flag," & Strings.ChrW(13) & Strings.ChrW(10)
        ' PW141003 - CQ2855 - Pass talked_to flag into disclosure_cud
        sSQL = sSQL & "        @TALKED_TO_PERSON=@TALKED_TO_PERSON," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @US=@US" & Strings.ChrW(13) & Strings.ChrW(10)

        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)

        sSQL = sSQL & "    SELECT @ErrNo = @@ERROR" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    /* If the Header SP Failed then return the Error */" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    IF (@ErrNo <> 0)" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        RETURN @ErrNo" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    IF @US = 0" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        Return" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    IF @US = 1" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        /* Insert */" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        INSERT INTO " & sTableName & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            (party_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "             party_conviction_id," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & sInsertList.ToString() ' RAW 15/10/2004 : CQ5119 : moved from after risk_cnt
        ' PW080903 - CQ1912
        sSQL = sSQL & "             insurance_folder_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "             risk_cnt)" & Strings.ChrW(13) & Strings.ChrW(10) ' RAW 15/10/2004 : CQ5119 : replaced , with )

        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        VALUES (@party_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "                @party_conviction_id," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & sValueList.ToString() ' RAW 15/10/2004 : CQ5119 : moved from after risk_cnt
        ' PW080903 - CQ1912
        sSQL = sSQL & "                @insurance_folder_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "                @risk_cnt)" & Strings.ChrW(13) & Strings.ChrW(10) ' RAW 15/10/2004 : CQ5119 : replaced , with )

        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    ELSE IF @US = 2" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    BEGIN" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)

        ' PW090903 - CQ1912 - check editbale flag - if 1 then just update as
        ' usual. if 0, create a new version
        ' PW250903 - CQ2665 - use editing_flag
        sSQL = sSQL & "        IF @editing_flag = 1" & Strings.ChrW(13) & Strings.ChrW(10)

        sSQL = sSQL & "        /* Does the disclosure already exist */" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        IF EXISTS" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "          (SELECT *" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           FROM " & sTableName & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           WHERE party_cnt = @party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "             AND party_conviction_id = @party_conviction_id)" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            /* Yes, so Update */" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            UPDATE " & sTableName & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "               SET  " & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & sSetList.ToString() ' RAW 15/10/2004 : CQ5119 : moved from after risk_cnt
        sSQL = sSQL & "                    insurance_folder_cnt = @insurance_folder_cnt," & Strings.ChrW(13) & Strings.ChrW(10) ' RAW 15/10/2004 : CQ5119 : added
        sSQL = sSQL & "                    risk_cnt = @risk_cnt" & Strings.ChrW(13) & Strings.ChrW(10) ' RAW 15/10/2004 : CQ5119 : added
        sSQL = sSQL & "            WHERE  party_cnt = @party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "              AND  party_conviction_id = @party_conviction_id" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        ELSE" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            /* No so Insert */" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            INSERT INTO " & sTableName & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "                (party_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "                 party_conviction_id," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & sInsertList.ToString() ' RAW 15/10/2004 : CQ5119 : moved from after risk_cnt
        ' PW080903 - CQ1912
        sSQL = sSQL & "                 insurance_folder_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "                 risk_cnt)" & Strings.ChrW(13) & Strings.ChrW(10) ' RAW 15/10/2004 : CQ5119 : replaced , with )

        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            VALUES (@party_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "                    @party_conviction_id," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & sValueList.ToString() ' RAW 15/10/2004 : CQ5119 : moved from after risk_cnt
        ' PW080903 - CQ1912
        sSQL = sSQL & "                    @insurance_folder_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "                    @risk_cnt)" & Strings.ChrW(13) & Strings.ChrW(10) ' RAW 15/10/2004 : CQ5119 : replaced , with )

        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)

        ' PW090903 - CQ1912 - check editbale flag - if 1 then just update as
        ' usual. if 0, create a new version
        ' PW250903 - CQ2665 - use editing_flag
        sSQL = sSQL & "        IF @editing_flag = 0" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            INSERT INTO " & sTableName & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "                (party_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "                 party_conviction_id," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & sInsertList.ToString() ' RAW 15/10/2004 : CQ5119 : moved from after risk_cnt
        sSQL = sSQL & "                 insurance_folder_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "                 risk_cnt)" & Strings.ChrW(13) & Strings.ChrW(10) ' RAW 15/10/2004 : CQ5119 : replaced , with )
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            VALUES (@party_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "                    @new_party_conviction_id," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & sValueList.ToString() ' RAW 15/10/2004 : CQ5119 : moved from after risk_cnt
        sSQL = sSQL & "                    @insurance_folder_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "                    @risk_cnt)" & Strings.ChrW(13) & Strings.ChrW(10) ' RAW 15/10/2004 : CQ5119 : replaced , with )
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)

        sSQL = sSQL & "    END" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        ELSE" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            /* Delete */" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            DELETE FROM " & sTableName & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            WHERE  party_cnt = @party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "              AND  party_conviction_id = @party_conviction_id" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    " & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "END" & Strings.ChrW(13) & Strings.ChrW(10)

        ' RAM20040831 - Commented the following line of code
        'sSQL = sSQL & "GO" & vbCrLf

        ' Build the SP Name
        sSPName = "spg_" & sTableName & "_cud"
        ' Build the SP File Name
        sSPFileName = sSPName & ".sql"

        ' RDC 10072001 START
        lReturn = CType(iGISSharedConstants.GetLoadSPPath(GISDataModelCode, sLoadSPPath), gPMConstants.PMEReturnCode)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' check path exists, create if not
        sPathTest = FileSystem.Dir(sLoadSPPath, FileAttribute.Directory)

        If sPathTest = "" Then
            Directory.CreateDirectory(sLoadSPPath)
        End If

        ' Get the Next File Number
        iFileNum = FileSystem.FreeFile()

        FileSystem.FileOpen(iFileNum, sLoadSPPath & "\" & sSPFileName, OpenMode.Output)
        ' RDC 10072001 END

        ' Write out the Stored Procedure
        FileSystem.PrintLine(iFileNum, sSQL)
        FileSystem.FileClose(iFileNum) ' Close file.

        Return result

    End Function



    ' RFC290103
    ' ***************************************************************** '
    ' Name: BuildClaimSelSP
    '
    ' Description: Build a Select Stored Procedure for
    '              Claim and Claim Peril Data.
    ' Edit History  :
    ' RAM20040831   : Removed all the code, related to dropping of Stored procedures
    ' ***************************************************************** '
    Private Function BuildClaimSelSP(ByVal v_sObjectName As String, ByVal v_sTableName As String, ByRef vPropertyArray(,) As Object, ByVal sClaimPeril As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        ' Object Definition
        Dim vDiscPropertyArray(,) As Object = Nothing
        Dim sPropertyName As String = ""
        Dim sColumnName As String = ""
        Dim iIsPrimaryKey As gPMConstants.PMEReturnCode
        Dim sParentTableName As String = ""
        Dim sOIKey As String = ""
        Dim lDataType As Integer

        ' Used to Create the SP
        Dim iFileNum As Integer
        Dim sSPName, sSPFileName As String
        Dim sLoadSPPath As String = ""
        Dim sPathTest As String = ""
        Dim sTag1Disc As New StringBuilder
        Dim sTag2Disc As New StringBuilder
        Dim sTag2AC As New StringBuilder
        Dim sTag1AC As New StringBuilder

        Dim sClaimPerilTable As String = ""

        '**********
        ' MEvans : 26-09-2003 : CQ2664
        Dim sSQL As String = ""
        Dim sSQLWorkClaim As String = ""
        Dim sSQLHeader As String = ""
        Dim sSQLFooter As String = ""
        Dim sSQLLiveClaim As String = ""
        '**********



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Build the Associated Client strings

        ' Loop Round the Properties
        For lRow As Integer = vPropertyArray.GetLowerBound(1) To vPropertyArray.GetUpperBound(1)


            sPropertyName = CStr(vPropertyArray(0, lRow))

            lReturn = CType(GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, r_lDataType:=lDataType, r_sColumnName:=sColumnName, r_iIsPrimaryKey:=iIsPrimaryKey), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            sColumnName = sColumnName.ToUpper()
            sPropertyName = sPropertyName.ToUpper()

            ' Ignore the Primary Keys as they have already been included in the Static Header Object (Policy_Client)
            If iIsPrimaryKey <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Ignore the Static Fields
                If Not IsClaimStaticColumn(sColumnName) Then

                    ' If the column is datetime then we need to covert it into ODBC conical format
                    If lDataType = iGISSharedConstants.GISDataTypeDate Then
                        sColumnName = "convert(varchar(30)," & sColumnName & ",120)"
                    End If

                    sTag1AC.Append("           " & sColumnName & "                  As [" & v_sObjectName & "!2!" & sPropertyName & "]," & Strings.ChrW(13) & Strings.ChrW(10))
                    sTag2AC.Append("           NULL                       As [" & v_sObjectName & "!2!" & sPropertyName & "]," & Strings.ChrW(13) & Strings.ChrW(10))

                End If


            End If

        Next lRow

        ' Get the Object Definition Details
        lReturn = CType(GetObjectDefDetails(v_sObjectName:=sClaimPeril, r_sTableName:=sClaimPerilTable, r_vPropertyArray:=vDiscPropertyArray), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' Build the Disclosure strings

        ' Loop Round the Properties

        For lRow As Integer = vDiscPropertyArray.GetLowerBound(1) To vDiscPropertyArray.GetUpperBound(1)


            sPropertyName = CStr(vDiscPropertyArray(0, lRow))

            lReturn = CType(GetPropertyDefDetails(v_sObjectName:=sClaimPeril, v_sPropertyName:=sPropertyName, r_lDataType:=lDataType, r_sColumnName:=sColumnName, r_iIsPrimaryKey:=iIsPrimaryKey), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            sColumnName = sColumnName.ToUpper()
            sPropertyName = sPropertyName.ToUpper()

            ' Ignore the Primary Keys as they have already been included in the Static Header Object (Policy_Client)
            If iIsPrimaryKey <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Ignore the Static Fields
                If Not IsClaimStaticColumn(sColumnName) Then

                    ' If the column is datetime then we need to covert it into ODBC conical format
                    If lDataType = iGISSharedConstants.GISDataTypeDate Then
                        sColumnName = "convert(varchar(30)," & sColumnName & ",120)"
                    End If

                    sTag2Disc.Append("           " & sColumnName & "                  As [" & sClaimPeril & "!3!" & sPropertyName & "]")
                    sTag1Disc.Append("           NULL                       As [" & sClaimPeril & "!3!" & sPropertyName & "]")

                    'If lRow <> UBound(vDiscPropertyArray, 2) Then
                    sTag2Disc.Append("," & Strings.ChrW(13) & Strings.ChrW(10))
                    sTag1Disc.Append("," & Strings.ChrW(13) & Strings.ChrW(10))
                    'Else
                    '    sTag2Disc = sTag2Disc & vbCrLf
                    '    sTag1Disc = sTag1Disc & vbCrLf
                    'End If

                End If

            End If

        Next lRow

        ' broke sql into 4 subgroups to handle live claims						
        sSQLHeader = sSQLHeader & "CREATE PROCEDURE SPG_" & v_sTableName & "_SEL" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLHeader = sSQLHeader & "    @CLAIM_ID INTEGER" & Strings.ChrW(13) & Strings.ChrW(10)
        'sSQLHeader = sSQLHeader & "    @LIVE_CLAIM BIT = 0" & vbCrLf
        sSQLHeader = sSQLHeader & "As" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLHeader = sSQLHeader & "BEGIN" & Strings.ChrW(13) & Strings.ChrW(10)

        sSQLWorkClaim = sSQLWorkClaim & "    Select" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        1                   As TAG ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As PARENT ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [HEADER!1]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!OI]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!US]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!POLICY_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!CLAIM_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!POLICY_NUMBER]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!CLAIM_NUMBER]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!DESCRIPTION]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!CLAIM_STATUS_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!PROGRESS_STATUS_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!PRIMARY_CAUSE_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!SECONDARY_CAUSE_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!CATASTROPHE_CODE_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!COINSURANCE_TREATMENT_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!LOSS_FROM_DATE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!LOSS_TO_DATE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!REPORTED_DATE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!REPORTED_TO_DATE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!LAST_MODIFIED_DATE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!HANDLER_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!CURRENCY_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!INFO_ONLY]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!LIKELY_CLAIM]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!LOCATION]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!TOWN]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!RISK_TYPE_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!CLIENT_NAME]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!CLIENT_ADDRESS]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!CLIENT_TEL_NO]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!CLIENT_FAX_NO]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!CLIENT_MOBILE_NO]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!CLIENT_EMAIL]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!CLIENT_CLAIM_NUMBER]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!INSURER_NAME]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!INSURER_ADDRESS]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!INSURER_TEL_NO]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!INSURER_FAX_NO]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!INSURER_EMAIL]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!INSURER_CLAIM_NUMBER]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!INSURER_CONTACT]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!VAT_REGISTERED]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!VAT_REG_NO]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!COMMENTS]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!CLAIMS_STATUS_DATE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!CLIENT_SHORT_NAME]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!INSURER_SHORT_NAME]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!CLIENT_TEL_NO_OFF]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!USER_DEFINED_FIELD_A]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!USER_DEFINED_FIELD_B]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!USER_DEFINED_FIELD_C]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!USER_DEFINED_FIELD_D]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!USER_DEFINED_FIELD_E]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!ORIGINAL_CLAIM_ID] ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!CLIENT_ID] ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!CLAIM_FOLDER_ID] ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!CLAIM_VERSION_NUMBER] ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!CLAIM_VERSION_STATUS_ID] ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!CREATE_DATE] ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!CREATED_BY_ID] ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!MODIFIED_BY_ID] ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!ACCEPTANCE_STATUS_ID] ," & Strings.ChrW(13) & Strings.ChrW(10)

        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & v_sObjectName & "!2!GIS_SCREEN_ID] ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & sTag2AC.ToString()
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & sClaimPeril & "!3!OI]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & sClaimPeril & "!3!US]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & sClaimPeril & "!3!CLAIM_PERIL_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & sClaimPeril & "!3!CLAIM_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & sClaimPeril & "!3!PERIL_TYPE_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & sClaimPeril & "!3!DESCRIPTION]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & sClaimPeril & "!3!COMMENTS]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & sClaimPeril & "!3!SUM_INSURED]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & sClaimPeril & "!3!RI_BAND]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & sTag1Disc.ToString()
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [" & sClaimPeril & "!3!GIS_SCREEN_ID]" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "UNION ALL" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "Select" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        2                   As TAG ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        1                   As PARENT ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                As [HEADER!1]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        'C' + CONVERT(VARCHAR(15),CLAIM.CLAIM_ID) AS [" & v_sObjectName & "!2!OI]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        0                   AS [" & v_sObjectName & "!2!US]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.POLICY_ID           AS [" & v_sObjectName & "!2!POLICY_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.CLAIM_ID      AS [" & v_sObjectName & "!2!CLAIM_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.POLICY_NUMBER       AS [" & v_sObjectName & "!2!POLICY_NUMBER]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.CLAIM_NUMBER        AS [" & v_sObjectName & "!2!CLAIM_NUMBER]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.DESCRIPTION         AS [" & v_sObjectName & "!2!DESCRIPTION]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.CLAIM_STATUS_ID     AS [" & v_sObjectName & "!2!CLAIM_STATUS_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.PROGRESS_STATUS_ID  AS [" & v_sObjectName & "!2!PROGRESS_STATUS_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.PRIMARY_CAUSE_ID    AS [" & v_sObjectName & "!2!PRIMARY_CAUSE_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.SECONDARY_CAUSE_ID  AS [" & v_sObjectName & "!2!SECONDARY_CAUSE_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.CATASTROPHE_CODE_ID AS [" & v_sObjectName & "!2!CATASTROPHE_CODE_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.COINSURANCE_TREATMENT_ID    AS [" & v_sObjectName & "!2!COINSURANCE_TREATMENT_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CONVERT(VARCHAR(30),CLAIM.LOSS_FROM_DATE,120)     AS [" & v_sObjectName & "!2!LOSS_FROM_DATE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CONVERT(VARCHAR(30),CLAIM.LOSS_TO_DATE,120)       AS [" & v_sObjectName & "!2!LOSS_TO_DATE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CONVERT(VARCHAR(30),CLAIM.REPORTED_DATE,120)      AS [" & v_sObjectName & "!2!REPORTED_DATE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CONVERT(VARCHAR(30),CLAIM.REPORTED_TO_DATE,120)   AS [" & v_sObjectName & "!2!REPORTED_TO_DATE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CONVERT(VARCHAR(30),CLAIM.LAST_MODIFIED_DATE,120) AS [" & v_sObjectName & "!2!LAST_MODIFIED_DATE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.HANDLER_ID          AS [" & v_sObjectName & "!2!HANDLER_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.CURRENCY_ID         AS [" & v_sObjectName & "!2!CURRENCY_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.INFO_ONLY           AS [" & v_sObjectName & "!2!INFO_ONLY]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.LIKELY_CLAIM        AS [" & v_sObjectName & "!2!LIKELY_CLAIM]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.LOCATION            AS [" & v_sObjectName & "!2!LOCATION]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.TOWN                AS [" & v_sObjectName & "!2!TOWN]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.RISK_TYPE_ID        AS [" & v_sObjectName & "!2!RISK_TYPE_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.CLIENT_NAME         AS [" & v_sObjectName & "!2!CLIENT_NAME]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.CLIENT_ADDRESS      AS [" & v_sObjectName & "!2!CLIENT_ADDRESS]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.CLIENT_TEL_NO       AS [" & v_sObjectName & "!2!CLIENT_TEL_NO]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.CLIENT_FAX_NO       AS [" & v_sObjectName & "!2!CLIENT_FAX_NO]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.CLIENT_MOBILE_NO    AS [" & v_sObjectName & "!2!CLIENT_MOBILE_NO]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.CLIENT_EMAIL        AS [" & v_sObjectName & "!2!CLIENT_EMAIL]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.CLIENT_CLAIM_NUMBER AS [" & v_sObjectName & "!2!CLIENT_CLAIM_NUMBER]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.INSURER_NAME        AS [" & v_sObjectName & "!2!INSURER_NAME]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.INSURER_ADDRESS     AS [" & v_sObjectName & "!2!INSURER_ADDRESS]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.INSURER_TEL_NO      AS [" & v_sObjectName & "!2!INSURER_TEL_NO]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.INSURER_FAX_NO      AS [" & v_sObjectName & "!2!INSURER_FAX_NO]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.INSURER_EMAIL       AS [" & v_sObjectName & "!2!INSURER_EMAIL]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.INSURER_CLAIM_NUMBER AS [" & v_sObjectName & "!2!INSURER_CLAIM_NUMBER]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.INSURER_CONTACT     AS [" & v_sObjectName & "!2!INSURER_CONTACT]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.VAT_REGISTERED      AS [" & v_sObjectName & "!2!VAT_REGISTERED]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.VAT_REG_NO          AS [" & v_sObjectName & "!2!VAT_REG_NO]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.COMMENTS            AS [" & v_sObjectName & "!2!COMMENTS]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CONVERT(VARCHAR(30),CLAIM.CLAIMS_STATUS_DATE,120)     AS [" & v_sObjectName & "!2!CLAIMS_STATUS_DATE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.CLIENT_SHORT_NAME       AS [" & v_sObjectName & "!2!CLIENT_SHORT_NAME]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.INSURER_SHORT_NAME      AS [" & v_sObjectName & "!2!INSURER_SHORT_NAME]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.CLIENT_TEL_NO_OFF       AS [" & v_sObjectName & "!2!CLIENT_TEL_NO_OFF]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.USER_DEFINED_FIELD_A        AS [" & v_sObjectName & "!2!USER_DEFINED_FIELD_A]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.USER_DEFINED_FIELD_B        AS [" & v_sObjectName & "!2!USER_DEFINED_FIELD_B]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.USER_DEFINED_FIELD_C        AS [" & v_sObjectName & "!2!USER_DEFINED_FIELD_C]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.USER_DEFINED_FIELD_D        AS [" & v_sObjectName & "!2!USER_DEFINED_FIELD_D]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.USER_DEFINED_FIELD_E        AS [" & v_sObjectName & "!2!USER_DEFINED_FIELD_E]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.CLAIM_ID                    AS [" & v_sObjectName & "!2!ORIGINAL_CLAIM_ID] ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.CLIENT_ID                   AS [" & v_sObjectName & "!2!CLIENT_ID] ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.CLAIM_FOLDER_ID             AS [" & v_sObjectName & "!2!CLAIM_FOLDER_ID] ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.CLAIM_VERSION_NUMBER        AS [" & v_sObjectName & "!2!CLAIM_VERSION_NUMBER] ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.CLAIM_VERSION_STATUS_ID     AS [" & v_sObjectName & "!2!CLAIM_VERSION_STATUS_ID] ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CONVERT(VARCHAR(30),CLAIM.CREATE_DATE,120)                 AS [" & v_sObjectName & "!2!CREATE_DATE] ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.CREATED_BY_ID               AS [" & v_sObjectName & "!2!CREATED_BY_ID] ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.MODIFIED_BY_ID              AS [" & v_sObjectName & "!2!MODIFIED_BY_ID] ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.ACCEPTANCE_STATUS_ID        AS [" & v_sObjectName & "!2!ACCEPTANCE_STATUS_ID] ," & Strings.ChrW(13) & Strings.ChrW(10)

        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM.GIS_SCREEN_ID    AS [" & v_sObjectName & "!2!GIS_SCREEN_ID] ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & sTag1AC.ToString()
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & sClaimPeril & "!3!OI]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & sClaimPeril & "!3!US]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & sClaimPeril & "!3!CLAIM_PERIL_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & sClaimPeril & "!3!CLAIM_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & sClaimPeril & "!3!PERIL_TYPE_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & sClaimPeril & "!3!DESCRIPTION]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & sClaimPeril & "!3!COMMENTS]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & sClaimPeril & "!3!SUM_INSURED]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & sClaimPeril & "!3!RI_BAND]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & sTag1Disc.ToString()
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & sClaimPeril & "!3!GIS_SCREEN_ID]" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "FROM CLAIM " & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "LEFT OUTER JOIN " & v_sTableName & " ON CLAIM.CLAIM_ID = " & v_sTableName & ".CLAIM_ID " & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "WHERE CLAIM.CLAIM_ID = @CLAIM_ID" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "UNION ALL" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "" & Strings.ChrW(13) & Strings.ChrW(10)
        'TR - removing the "distinct" as there is a text field now which cannot be included in a select distinct
        'sSQLWorkClaim = sSQLWorkClaim & "    SELECT DISTINCT " & vbCrLf
        sSQLWorkClaim = sSQLWorkClaim & "    SELECT " & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        3                   AS TAG ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        2                   AS PARENT ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [HEADER!1]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!OI]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!US]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!POLICY_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!CLAIM_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!POLICY_NUMBER]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!CLAIM_NUMBER]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!DESCRIPTION]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!CLAIM_STATUS_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!PROGRESS_STATUS_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!PRIMARY_CAUSE_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!SECONDARY_CAUSE_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!CATASTROPHE_CODE_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!COINSURANCE_TREATMENT_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!LOSS_FROM_DATE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!LOSS_TO_DATE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!REPORTED_DATE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!REPORTED_TO_DATE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!LAST_MODIFIED_DATE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!HANDLER_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!CURRENCY_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!INFO_ONLY]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!LIKELY_CLAIM]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!LOCATION]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!TOWN]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!RISK_TYPE_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!CLIENT_NAME]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!CLIENT_ADDRESS]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!CLIENT_TEL_NO]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!CLIENT_FAX_NO]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!CLIENT_MOBILE_NO]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!CLIENT_EMAIL]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!CLIENT_CLAIM_NUMBER]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!INSURER_NAME]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!INSURER_ADDRESS]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!INSURER_TEL_NO]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!INSURER_FAX_NO]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!INSURER_EMAIL]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!INSURER_CLAIM_NUMBER]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!INSURER_CONTACT]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!VAT_REGISTERED]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!VAT_REG_NO]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!COMMENTS]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!CLAIMS_STATUS_DATE]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!CLIENT_SHORT_NAME]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!INSURER_SHORT_NAME]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!CLIENT_TEL_NO_OFF]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!USER_DEFINED_FIELD_A]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!USER_DEFINED_FIELD_B]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!USER_DEFINED_FIELD_C]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!USER_DEFINED_FIELD_D]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!USER_DEFINED_FIELD_E]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!ORIGINAL_CLAIM_ID] ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!CLIENT_ID] ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!CLAIM_FOLDER_ID] ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!CLAIM_VERSION_NUMBER] ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!CLAIM_VERSION_STATUS_ID] ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!CREATE_DATE] ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!CREATED_BY_ID] ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!MODIFIED_BY_ID] ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!ACCEPTANCE_STATUS_ID] ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        NULL                AS [" & v_sObjectName & "!2!GIS_SCREEN_ID] ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & sTag2AC.ToString()
        sSQLWorkClaim = sSQLWorkClaim & "        'CP' + CONVERT(VARCHAR(15),CLAIM_PERIL.CLAIM_PERIL_ID) AS [" & sClaimPeril & "!3!OI]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        0                   AS [" & sClaimPeril & "!3!US]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM_PERIL.CLAIM_PERIL_ID      AS [" & sClaimPeril & "!3!CLAIM_PERIL_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM_PERIL.CLAIM_ID            AS [" & sClaimPeril & "!3!CLAIM_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        PERIL_TYPE_ID       AS [" & sClaimPeril & "!3!PERIL_TYPE_ID]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        DESCRIPTION         AS [" & sClaimPeril & "!3!DESCRIPTION]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        COMMENTS            AS [" & sClaimPeril & "!3!COMMENTS]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        SUM_INSURED         AS [" & sClaimPeril & "!3!SUM_INSURED]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "        RI_BAND             AS [" & sClaimPeril & "!3!RI_BAND]," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & sTag2Disc.ToString()
        sSQLWorkClaim = sSQLWorkClaim & "        CLAIM_PERIL.GIS_SCREEN_ID AS [" & sClaimPeril & "!3!GIS_SCREEN_ID]" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "     FROM CLAIM_PERIL " & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "     LEFT OUTER JOIN " & sClaimPerilTable & " ON CLAIM_PERIL.CLAIM_PERIL_ID = " & sClaimPerilTable & ".CLAIM_PERIL_ID" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "     WHERE CLAIM_PERIL.CLAIM_ID = @CLAIM_ID" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "     ORDER BY [" & sClaimPeril & "!3!CLAIM_PERIL_ID]" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & "FOR XML EXPLICIT" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & " END" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & " " & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLWorkClaim = sSQLWorkClaim & " " & Strings.ChrW(13) & Strings.ChrW(10)

        sSQLLiveClaim = sSQLLiveClaim & "BEGIN" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLLiveClaim = sSQLLiveClaim & sSQLWorkClaim
        'sSQLLiveClaim = sSQLLiveClaim.Substring(0).Replace(" ORIGINAL_CLAIM_ID ", "CLAIM.CLAIM_ID")
        'sSQLLiveClaim = Replace(sSQLLiveClaim, "FROM CLAIM ", "FROM CLAIM AS CLAIM", 1)
        'sSQLLiveClaim = Replace(sSQLLiveClaim, "FROM CLAIM_PERIL ", "FROM CLAIM_PERIL AS CLAIM_PERIL", 1)
        sSQLLiveClaim = sSQLLiveClaim & Strings.ChrW(13) & Strings.ChrW(10)
        sSQLFooter = sSQLFooter & "END" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQLHeader & sSQLLiveClaim & sSQLFooter

        ' Build the SP Name
        sSPName = "spg_" & v_sTableName & "_sel"
        ' Build the SP File Name
        sSPFileName = sSPName & ".sql"

        lReturn = iGISSharedConstants.GetLoadSPPath(GISDataModelCode, sLoadSPPath)

        ' RDC 10072001 START
        lReturn = CType(iGISSharedConstants.GetLoadSPPath(GISDataModelCode, sLoadSPPath), gPMConstants.PMEReturnCode)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' check path exists, create if not
        sPathTest = FileSystem.Dir(sLoadSPPath, FileAttribute.Directory)

        If sPathTest = "" Then
            Directory.CreateDirectory(sLoadSPPath)
        End If

        ' Get the Next File Number
        iFileNum = FileSystem.FreeFile()
        FileSystem.FileOpen(iFileNum, sLoadSPPath & "\" & sSPFileName, OpenMode.Output)

        ' Write out the Stored Procedure
        FileSystem.PrintLine(iFileNum, sSQL)
        FileSystem.FileClose(iFileNum) ' Close file.

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: BuildClaimCUDSP
    '
    ' Description: Build a Create Update Delete Stored Procedure for
    '              Claim.
    ' Edit History  :
    ' RAM20040831   : Removed all the code, related to dropping of Stored procedures
    ' ***************************************************************** '
    Private Function BuildClaimCUDSP(ByVal v_sObjectName As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        ' Object Definition
        Dim sTableName As String = ""
        Dim vChildObjectArray As Object = Nothing
        Dim vPropertyArray(,) As Object = Nothing
        Dim sPropertyName As String = ""
        Dim sColumnName As String = ""
        Dim iIsPrimaryKey As gPMConstants.PMEReturnCode
        Dim sParentTableName As String = ""
        Dim sOIKey As String = ""
        Dim lDataType As Integer
        Dim sSQLType As String = ""

        ' Used to Create the SP
        Dim iFileNum As Integer
        Dim sSPName, sSPFileName As String
        Dim sLoadSPPath As String = ""
        Dim sPathTest As String = ""
        Dim sSQL As String
        Dim sValueList As New StringBuilder
        Dim sInsertList As New StringBuilder
        Dim sSetList As New StringBuilder
        Dim sParamList As New StringBuilder



        result = gPMConstants.PMEReturnCode.PMTrue

        sParamList = New StringBuilder("")
        sSetList = New StringBuilder("")
        sInsertList = New StringBuilder("")
        sValueList = New StringBuilder("")

        ' Get the Object Definition Details
        lReturn = CType(GetObjectDefDetails(v_sObjectName:=v_sObjectName, r_sTableName:=sTableName, r_vChildObjectArray:=vChildObjectArray, r_vPropertyArray:=vPropertyArray), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' Build the Associated Client strings

        ' Loop Round the Properties

        For lRow As Integer = vPropertyArray.GetLowerBound(1) To vPropertyArray.GetUpperBound(1)


            sPropertyName = CStr(vPropertyArray(0, lRow))

            lReturn = CType(GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, r_lDataType:=lDataType, r_sColumnName:=sColumnName, r_iIsPrimaryKey:=iIsPrimaryKey), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            sColumnName = sColumnName.ToUpper()
            sPropertyName = sPropertyName.ToUpper()

            ' Ignore the Primary Keys as they have already been included in SP Template
            If iIsPrimaryKey <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Use the data type from the gis_property
                sSQLType = GetSQLType(lDataType)

                ' Ignore the Static Fields
                If Not IsClaimStaticColumn(sColumnName) Then

                    ' Parameter List
                    sParamList.Append("@" & sColumnName & " As " & sSQLType & " = NULL , " & Strings.ChrW(13) & Strings.ChrW(10))

                    ' PW150702 - PS68 change final checks 'cos they won't work if the last
                    ' field is a static field

                    ' Set List
                    If lDataType = iGISSharedConstants.GISDataTypeDate Then
                        sSetList.Append(sColumnName & " = " & "CONVERT(datetime,@" & sColumnName & ",120)")
                    Else
                        sSetList.Append(sColumnName & " = @" & sColumnName)
                    End If
                    sSetList.Append(" , " & Strings.ChrW(13) & Strings.ChrW(10))

                    ' Insert List
                    sInsertList.Append(sColumnName)
                    sInsertList.Append(" , " & Strings.ChrW(13) & Strings.ChrW(10))

                    ' Value List
                    If lDataType = iGISSharedConstants.GISDataTypeDate Then
                        sValueList.Append("CONVERT(datetime,@" & sColumnName & ",120)")
                    Else
                        sValueList.Append("@" & sColumnName)
                    End If
                    sValueList.Append(" , " & Strings.ChrW(13) & Strings.ChrW(10))

                End If

            End If

        Next lRow

        ' PW150702 - PS68 change final checks 'cos they won't work if the last
        ' field is a static field
        If sSetList.ToString() <> "" Then
            sSetList = New StringBuilder(sSetList.ToString().Substring(0, sSetList.ToString().Length - 5))
            sInsertList = New StringBuilder(sInsertList.ToString().Substring(0, sInsertList.ToString().Length - 5) & ") " & Strings.ChrW(13) & Strings.ChrW(10))
            sValueList = New StringBuilder(sValueList.ToString().Substring(0, sValueList.ToString().Length - 5) & ") " & Strings.ChrW(13) & Strings.ChrW(10))
        End If

        sSQL = ""

        ' RAM20040831 : Commented the following line(s) of Code
        'sSQL = sSQL & "SET QUOTED_IDENTIFIER OFF" & vbCrLf
        'sSQL = sSQL & "GO" & vbCrLf
        'sSQL = sSQL & "SET ANSI_NULLS ON" & vbCrLf
        'sSQL = sSQL & "GO" & vbCrLf
        'sSQL = sSQL & vbCrLf
        ''sSQL = sSQL & "EXEC DDLDropProcedure 'spg_DMC_Associated_Client_cud'" & vbCrLf
        'sSQL = sSQL & "EXEC DDLDropProcedure 'spg_" & sTableName & "_cud'" & vbCrLf
        'sSQL = sSQL & "GO" & vbCrLf
        'sSQL = sSQL & vbCrLf
        ''sSQL = sSQL & "CREATE PROCEDURE spg_DMC_Associated_Client_cud" & vbCrLf

        sSQL = sSQL & "CREATE PROCEDURE spg_" & sTableName & "_cud" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Claim_id int OUTPUT ,"
        sSQL = sSQL & "      @Policy_id int ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Policy_Number varchar(30) ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Claim_Number varchar(30) ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Description varchar(1000)  ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Claim_Status_id int ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Progress_Status_id int ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Primary_Cause_id int ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Secondary_Cause_id int = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Catastrophe_code_id int = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Coinsurance_treatment_id int = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Loss_from_date datetime ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Loss_to_date datetime = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Reported_date datetime ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Reported_to_date datetime = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Last_modified_date datetime  ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Handler_id int ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Currency_id int ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Info_only Boolean ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Likely_claim Boolean ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Location varchar(50) = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Town int = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Risk_type_id int ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Client_name varchar(60)  ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Client_address int ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Client_tel_no varchar(50) = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Client_fax_no varchar(50) = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Client_mobile_no varchar(50) = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Client_email varchar(50) = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Client_claim_number varchar(20) = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Insurer_name varchar(60) = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @insurer_address int = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @insurer_tel_no varchar(50) = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @insurer_fax_no varchar(50) = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @insurer_email varchar(50) = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @insurer_claim_number varchar(20) = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Insurer_Contact varchar(50) = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @VAT_registered bit ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @VAT_reg_no varchar(20) = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Comments varchar(255) = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Claims_status_date datetime = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Client_short_name char(20) = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Insurer_short_name char(20) = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Client_tel_no_off varchar(50) = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @user_defined_field_A int = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @user_defined_field_B int = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @user_defined_field_C int = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @user_defined_field_D int = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @user_defined_field_E int = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Client_id int = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Original_Claim_id int = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Claim_folder_id int = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Claim_version_number int = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @claim_version_status_id int = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @create_date datetime = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @created_by_id  int = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Modified_by_id int = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Acceptance_Status_id int = NULL," & Strings.ChrW(13) & Strings.ChrW(10)

        ' PW150703 - PS68 - Add GIS_SCREEN_ID
        sSQL = sSQL & "      @gis_screen_id int = NULL," & Strings.ChrW(13) & Strings.ChrW(10)
        ' PW150703 - PS68: end
        sSQL = sSQL & sParamList.ToString()
        sSQL = sSQL & "      @US tinyint" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "AS" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "BEGIN" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "DECLARE @ErrNo integer" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    /* Call the Header SP First */" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    EXEC spu_claim_cud" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Claim_id            = @Claim_id ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Policy_id           = @Policy_id  ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Policy_Number       = @Policy_Number ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Claim_Number        = @Claim_Number ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Description         = @Description  ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Claim_Status_id     = @Claim_Status_id ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Progress_Status_id      = @Progress_Status_id ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Primary_Cause_id        = @Primary_Cause_id ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Secondary_Cause_id      = @Secondary_Cause_id ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Catastrophe_code_id     = @Catastrophe_code_id ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Coinsurance_treatment_id    = @Coinsurance_treatment_id ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Loss_from_date      = @Loss_from_date ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Loss_to_date        = @Loss_to_date ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Reported_date       = @Reported_date ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Reported_to_date        = @Reported_to_date ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Last_modified_date      = @Last_modified_date ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Handler_id          = @Handler_id ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Currency_id         = @Currency_id ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Info_only           = @Info_only ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Likely_claim        = @Likely_claim ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Location            = @Location ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Town            = @Town ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Risk_type_id        = @Risk_type_id ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Client_name         = @Client_name ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Client_address      = @Client_address ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Client_tel_no       = @Client_tel_no ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Client_fax_no       = @Client_fax_no ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Client_mobile_no        = @Client_mobile_no ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Client_email        = @Client_email ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Client_claim_number     = @Client_claim_number ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Insurer_name        = @Insurer_name ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @insurer_address     = @insurer_address ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @insurer_tel_no      = @insurer_tel_no ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @insurer_fax_no      = @insurer_fax_no ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @insurer_email       = @insurer_email ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @insurer_claim_number    = @insurer_claim_number ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Insurer_Contact     = @Insurer_Contact ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @VAT_registered      = @VAT_registered ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @VAT_reg_no          = @VAT_reg_no ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Comments            = @Comments ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Claims_status_date      = @Claims_status_date ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Client_short_name       = @Client_short_name ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Insurer_short_name      = @Insurer_short_name ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Client_tel_no_off       = @Client_tel_no_off ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @user_defined_field_A    = @user_defined_field_A ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @user_defined_field_B    = @user_defined_field_B ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @user_defined_field_C    = @user_defined_field_C ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @user_defined_field_D    = @user_defined_field_D ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @user_defined_field_E    = @user_defined_field_E ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Client_id               = @Client_id ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Original_Claim_id       = @Original_Claim_id ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Claim_folder_id         = @Claim_folder_id ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Claim_version_number    = @Claim_version_number ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @claim_version_status_id = @claim_version_status_id ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @create_date             = @create_date ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @created_by_id           = @created_by_id ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Modified_by_id          = @Modified_by_id ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        @Acceptance_Status_id    =@Acceptance_Status_id ," & Strings.ChrW(13) & Strings.ChrW(10)

        ' PW150703 - PS68 - Add GIS_SCREEN_ID
        sSQL = sSQL & "        @gis_Screen_id           = @gis_screen_id ," & Strings.ChrW(13) & Strings.ChrW(10)
        ' PW150703 - PS68: end
        sSQL = sSQL & "        @US                      = @US" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    SELECT @ErrNo = @@ERROR" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    /* If the Header SP Failed then return the Error */" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    IF (@ErrNo <> 0)" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        RETURN @ErrNo" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    IF @US = 0" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        Return" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    IF @US = 1" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "                /* Inserts are done directly via Product Builder */" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        /* Insert */" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        INSERT INTO " & sTableName & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            (claim_id," & Strings.ChrW(13) & Strings.ChrW(10)
        'sSQL = sSQL & "            ac_field1," & vbCrLf
        'sSQL = sSQL & "            ac_field2)" & vbCrLf
        sSQL = sSQL & sInsertList.ToString()
        sSQL = sSQL & "        VALUES (@claim_id," & Strings.ChrW(13) & Strings.ChrW(10)
        'sSQL = sSQL & "               @ac_field1," & vbCrLf
        'sSQL = sSQL & "               @ac_field2)" & vbCrLf
        sSQL = sSQL & sValueList.ToString()
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    ELSE IF @US = 2" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    BEGIN" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        /* Does the disclosure already exist */" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        IF EXISTS" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "          (Select *" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           From " & sTableName & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           WHERE claim_id = @claim_id)" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            /* Yes, so Update */" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            Update " & sTableName & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            SET " & Strings.ChrW(13) & Strings.ChrW(10)
        'sSQL = sSQL & "              ac_field1 = @ac_field1," & vbCrLf
        'sSQL = sSQL & "              ac_field2 = @ac_field2" & vbCrLf
        sSQL = sSQL & sSetList.ToString()
        sSQL = sSQL & "            WHERE  claim_id = @claim_id" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        Else" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            /* No so Insert */" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            INSERT INTO " & sTableName & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "                (claim_id," & Strings.ChrW(13) & Strings.ChrW(10)
        'sSQL = sSQL & "                 ac_field1," & vbCrLf
        'sSQL = sSQL & "                 ac_field2)" & vbCrLf
        sSQL = sSQL & sInsertList.ToString()
        sSQL = sSQL & "            VALUES (@claim_id," & Strings.ChrW(13) & Strings.ChrW(10)
        'sSQL = sSQL & "                    @ac_field1," & vbCrLf
        'sSQL = sSQL & "                    @ac_field2)" & vbCrLf
        sSQL = sSQL & sValueList.ToString()
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    End" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        Else" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "                /* Deletes are done directly via Product Builder */" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        RETURN -901" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "End" & Strings.ChrW(13) & Strings.ChrW(10)

        ' RAM20040831  : Commented the following line(s) of code
        'sSQL = sSQL & "GO" & vbCrLf

        ' Build the SP Name
        sSPName = "spg_" & sTableName & "_cud"
        ' Build the SP File Name
        sSPFileName = sSPName & ".sql"

        ' RDC 10072001 START
        lReturn = CType(iGISSharedConstants.GetLoadSPPath(GISDataModelCode, sLoadSPPath), gPMConstants.PMEReturnCode)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' check path exists, create if not
        sPathTest = FileSystem.Dir(sLoadSPPath, FileAttribute.Directory)

        If sPathTest = "" Then
            Directory.CreateDirectory(sLoadSPPath)
        End If

        ' Get the Next File Number
        iFileNum = FileSystem.FreeFile()

        FileSystem.FileOpen(iFileNum, sLoadSPPath & "\" & sSPFileName, OpenMode.Output)
        ' RDC 10072001 END

        ' Write out the Stored Procedure
        FileSystem.PrintLine(iFileNum, sSQL)
        FileSystem.FileClose(iFileNum) ' Close file.


        Return result

    End Function

    ' RFC290103
    ' ***************************************************************** '
    ' Name: BuildClaimPerilCUDSP
    '
    ' Description: Build a Create Update Delete Stored Procedure for
    '              Disclosure.
    ' Edit History  :
    ' RAM20040831   : Removed all the code, related to dropping of Stored procedures
    ' ***************************************************************** '
    Private Function BuildClaimPerilCUDSP(ByVal v_sObjectName As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        ' Object Definition
        Dim sTableName As String = ""
        Dim vChildObjectArray As Object = Nothing
        Dim vPropertyArray(,) As Object = Nothing
        Dim sPropertyName As String = ""
        Dim sColumnName As String = ""
        Dim iIsPrimaryKey As gPMConstants.PMEReturnCode
        Dim sParentTableName As String = ""
        Dim sOIKey As String = ""
        Dim lDataType As Integer
        Dim sSQLType As String = ""

        ' Used to Create the SP
        Dim iFileNum As Integer
        Dim sSPName As String = ""
        Dim sSPFileName As String = ""
        Dim sLoadSPPath As String = ""
        Dim sPathTest As String = ""
        Dim sSQL As String = ""
        Dim sValueList As New StringBuilder
        Dim sInsertList As New StringBuilder
        Dim sSetList As New StringBuilder
        Dim sParamList As New StringBuilder



        result = gPMConstants.PMEReturnCode.PMTrue

        sParamList = New StringBuilder("")
        sSetList = New StringBuilder("")
        sInsertList = New StringBuilder("")
        sValueList = New StringBuilder("")

        ' Get the Object Definition Details
        lReturn = CType(GetObjectDefDetails(v_sObjectName:=v_sObjectName, r_sTableName:=sTableName, r_vChildObjectArray:=vChildObjectArray, r_vPropertyArray:=vPropertyArray), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' Build the Associated Client strings

        ' Loop Round the Properties

        For lRow As Integer = vPropertyArray.GetLowerBound(1) To vPropertyArray.GetUpperBound(1)


            sPropertyName = CStr(vPropertyArray(0, lRow))

            lReturn = CType(GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, r_lDataType:=lDataType, r_sColumnName:=sColumnName, r_iIsPrimaryKey:=iIsPrimaryKey), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            sColumnName = sColumnName.ToUpper()
            sPropertyName = sPropertyName.ToUpper()

            ' Ignore the Primary Keys as they have already been included in SP Template
            If iIsPrimaryKey <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Use the data type from the gis_property
                sSQLType = GetSQLType(lDataType)

                ' Ignore the Static Fields
                If Not IsClaimStaticColumn(sColumnName) Then

                    ' Parameter List
                    sParamList.Append("@" & sColumnName & " As " & sSQLType & " = NULL , " & Strings.ChrW(13) & Strings.ChrW(10))

                    ' PW150702 - PS68 change final checks 'cos they won't work if the last
                    ' field is a static field

                    ' Set List
                    If lDataType = iGISSharedConstants.GISDataTypeDate Then
                        sSetList.Append(sColumnName & " = " & "CONVERT(datetime,@" & sColumnName & ",120)")
                    Else
                        sSetList.Append(sColumnName & " = @" & sColumnName)
                    End If
                    sSetList.Append(" , " & Strings.ChrW(13) & Strings.ChrW(10))

                    ' Insert List
                    sInsertList.Append(sColumnName)
                    sInsertList.Append(" , " & Strings.ChrW(13) & Strings.ChrW(10))

                    ' Value List
                    If lDataType = iGISSharedConstants.GISDataTypeDate Then
                        sValueList.Append("CONVERT(datetime,@" & sColumnName & ",120)")
                    Else
                        sValueList.Append("@" & sColumnName)
                    End If
                    sValueList.Append(" , " & Strings.ChrW(13) & Strings.ChrW(10))

                End If

            End If

        Next lRow

        ' PW150702 - PS68 change final checks 'cos they won't work if the last
        ' field is a static field
        If sSetList.ToString() <> "" Then
            sSetList = New StringBuilder(sSetList.ToString().Substring(0, sSetList.ToString().Length - 5))
            sInsertList = New StringBuilder(sInsertList.ToString().Substring(0, sInsertList.ToString().Length - 5) & ") " & Strings.ChrW(13) & Strings.ChrW(10))
            sValueList = New StringBuilder(sValueList.ToString().Substring(0, sValueList.ToString().Length - 5) & ") " & Strings.ChrW(13) & Strings.ChrW(10))
        End If

        sSQL = ""

        ' RAM20040831 : Commented the following lines of code
        'sSQL = sSQL & "SET QUOTED_IDENTIFIER OFF" & vbCrLf
        'sSQL = sSQL & "GO" & vbCrLf
        'sSQL = sSQL & "SET ANSI_NULLS ON" & vbCrLf
        'sSQL = sSQL & "GO" & vbCrLf
        'sSQL = sSQL & vbCrLf
        ''sSQL = sSQL & "EXEC DDLDropProcedure 'spg_DMC_Disclosure_cud'" & vbCrLf
        'sSQL = sSQL & "EXEC DDLDropProcedure 'spg_" & sTableName & "_cud'" & vbCrLf
        'sSQL = sSQL & "GO" & vbCrLf
        ''sSQL = sSQL & "CREATE PROCEDURE spg_DMC_Disclosure_cud" & vbCrLf

        sSQL = sSQL & "CREATE PROCEDURE spg_" & sTableName & "_cud" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @claim_Peril_id int OUTPUT ,"
        sSQL = sSQL & "      @Claim_id int ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Peril_type_id int ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Description varchar(255) = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @Comments varchar(255) = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @sum_insured numeric(19, 4) = NULL," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @ri_band int = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      @original_claim_Peril_id int = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        ' PW150703 - PS68 - Add GIS_SCREEN_ID
        sSQL = sSQL & "      @gis_Screen_id int = NULL ," & Strings.ChrW(13) & Strings.ChrW(10)
        ' PW150703 - PS68: end
        'sSQL = sSQL & "  @d_field1 varchar(255) ," & vbCrLf
        'sSQL = sSQL & "  @d_field2 varchar(255) ," & vbCrLf
        sSQL = sSQL & sParamList.ToString()
        sSQL = sSQL & "  @US tinyint" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "AS" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "BEGIN" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "DECLARE @ErrNo integer"
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    /* Call the Header SP First */" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    EXEC spu_claim_peril_cud" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            @claim_Peril_id=@claim_Peril_id OUTPUT ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            @Claim_id=@Claim_id ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            @Peril_type_id=@Peril_type_id ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            @Description=@Description ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            @Comments=@Comments ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            @sum_insured=@sum_insured ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            @ri_band=@ri_band ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            @original_claim_Peril_id=@original_claim_Peril_id ," & Strings.ChrW(13) & Strings.ChrW(10)
        ' PW150703 - PS68 - Add GIS_SCREEN_ID
        sSQL = sSQL & "            @gis_screen_id=@gis_screen_id ," & Strings.ChrW(13) & Strings.ChrW(10)
        ' PW150703 - PS68: end
        sSQL = sSQL & "            @US=@US" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    SELECT @ErrNo = @@ERROR" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    /* If the Header SP Failed then return the Error */" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    IF (@ErrNo <> 0)" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        RETURN @ErrNo" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    IF @US = 0" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        Return" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    IF @US = 1" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        /* Insert */" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        INSERT INTO " & sTableName & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            (claim_peril_id," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "             claim_id," & Strings.ChrW(13) & Strings.ChrW(10)
        'sSQL = sSQL & "             d_field1," & vbCrLf
        'sSQL = sSQL & "             d_field2)" & vbCrLf
        sSQL = sSQL & sInsertList.ToString()
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        VALUES (@claim_peril_id," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "                @claim_id," & Strings.ChrW(13) & Strings.ChrW(10)
        'sSQL = sSQL & "                @d_field1," & vbCrLf
        'sSQL = sSQL & "                @d_field2)" & vbCrLf
        sSQL = sSQL & sValueList.ToString()
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    ELSE IF @US = 2" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    BEGIN" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        /* Does the disclosure already exist */" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        IF EXISTS" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "          (SELECT *" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           FROM " & sTableName & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           WHERE claim_peril_id = @claim_peril_id)" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            /* Yes, so Update */" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            UPDATE " & sTableName & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "               SET  " & Strings.ChrW(13) & Strings.ChrW(10)
        'sSQL = sSQL & "                    d_field1 = @d_field1," & vbCrLf
        'sSQL = sSQL & "                    d_field2 = @d_field2" & vbCrLf
        sSQL = sSQL & sSetList.ToString()
        sSQL = sSQL & "             WHERE claim_peril_id = @claim_peril_id" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        ELSE" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            /* No so Insert */" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            INSERT INTO " & sTableName & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "                (claim_peril_id," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "                 claim_id," & Strings.ChrW(13) & Strings.ChrW(10)
        'sSQL = sSQL & "                d_field1," & vbCrLf
        'sSQL = sSQL & "                d_field2)" & vbCrLf
        sSQL = sSQL & sInsertList.ToString()
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            VALUES (@claim_peril_id," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "                    @claim_id," & Strings.ChrW(13) & Strings.ChrW(10)
        'SQL = sSQL & "                  @d_field1," & vbCrLf
        'SQL = sSQL & "                  @d_field2)" & vbCrLf
        sSQL = sSQL & sValueList.ToString()
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    END" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        ELSE" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            /* Delete */" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "            DELETE FROM " & sTableName & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "             WHERE claim_peril_id = @claim_peril_id" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    " & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "END" & Strings.ChrW(13) & Strings.ChrW(10)

        ' RAM20040831 : Commented the following line of code
        'sSQL = sSQL & "GO" & vbCrLf

        ' Build the SP Name
        sSPName = "spg_" & sTableName & "_cud"
        ' Build the SP File Name
        sSPFileName = sSPName & ".sql"

        ' RDC 10072001 START
        lReturn = CType(iGISSharedConstants.GetLoadSPPath(GISDataModelCode, sLoadSPPath), gPMConstants.PMEReturnCode)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' check path exists, create if not
        sPathTest = FileSystem.Dir(sLoadSPPath, FileAttribute.Directory)

        If sPathTest = "" Then
            Directory.CreateDirectory(sLoadSPPath)
        End If

        ' Get the Next File Number
        iFileNum = FileSystem.FreeFile()

        FileSystem.FileOpen(iFileNum, sLoadSPPath & "\" & sSPFileName, OpenMode.Output)
        ' RDC 10072001 END

        ' Write out the Stored Procedure
        FileSystem.PrintLine(iFileNum, sSQL)
        FileSystem.FileClose(iFileNum) ' Close file.

        Return result

    End Function

    ' RFC260203 - Build the Copy Claim to Work (& vice versa) stored procs
    ' ***************************************************************** '
    ' Name: BuildCopyClaimSP
    '
    ' Description: Create two stored procedures that copy from the
    '              DataModelCode_Claim and DataModelCode_Claim_Peril tables to
    '              DataModelCode_Claim and DataModelCode_Claim_Peril
    '              The second SP copies back in the other direction.
    '
    ' Edit History :
    ' RAM20040831  : Removed all the code, related to dropping of Stored procedures
    ' ***************************************************************** '
    Private Function BuildCopyClaimSP(ByVal v_sObjectName As String, ByVal v_sPerilObjName As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        ' Object Definition
        Dim sTableName As String = ""
        Dim vChildObjectArray As Object = Nothing
        Dim vPropertyArray(,) As Object = Nothing
        Dim sPropertyName As String = ""
        Dim sColumnName As String = ""
        Dim iIsPrimaryKey As gPMConstants.PMEReturnCode
        Dim sPerilTableName As String = ""

        Dim sParentTableName As String = ""
        Dim sOIKey As String = ""
        Dim lDataType As Integer
        Dim sSQLType As String = ""

        ' Used to Create the SP
        Dim iFileNum As Integer
        Dim sSPName As String = ""
        Dim sSPFileName As String = ""
        Dim sLoadSPPath As String = ""
        Dim sPathTest As String = ""
        Dim sSQL As String
        Dim sPerilValueList As New StringBuilder
        Dim sPerilInsertList As New StringBuilder
        Dim sValueList As New StringBuilder
        Dim sInsertList As New StringBuilder



        result = gPMConstants.PMEReturnCode.PMTrue

        sInsertList = New StringBuilder("")
        sValueList = New StringBuilder("")
        sPerilInsertList = New StringBuilder("")
        sPerilValueList = New StringBuilder("")

        ' Get the Object Definition Details for Claim
        lReturn = CType(GetObjectDefDetails(v_sObjectName:=v_sObjectName, r_sTableName:=sTableName, r_vChildObjectArray:=vChildObjectArray, r_vPropertyArray:=vPropertyArray), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' Build the Insert and Value Lists for the Claim Object

        ' Loop Round the Properties

        For lRow As Integer = vPropertyArray.GetLowerBound(1) To vPropertyArray.GetUpperBound(1)
            sPropertyName = CStr(vPropertyArray(0, lRow))
            lReturn = CType(GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, r_lDataType:=lDataType, r_sColumnName:=sColumnName, r_iIsPrimaryKey:=iIsPrimaryKey), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            sColumnName = sColumnName.ToUpper()
            sPropertyName = sPropertyName.ToUpper()

            ' Ignore the Primary Keys as they have already been included in SP Template
            If iIsPrimaryKey <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Use the data type from the gis_property
                sSQLType = GetSQLType(lDataType)

                ' Ignore the Static Fields
                If Not IsClaimStaticColumn(sColumnName) Then

                    ' Insert List
                    sInsertList.Append(sColumnName & " , " & Strings.ChrW(13) & Strings.ChrW(10))

                    ' Value List
                    If lDataType = iGISSharedConstants.GISDataTypeDate Then
                        sValueList.Append("CONVERT(datetime," & sColumnName & ",120)" & " , " & Strings.ChrW(13) & Strings.ChrW(10))
                    Else
                        sValueList.Append(sColumnName & " , " & Strings.ChrW(13) & Strings.ChrW(10))
                    End If
                End If
            End If
        Next lRow

        ' Get the Object Definition Details for Claim Peril
        lReturn = CType(GetObjectDefDetails(v_sObjectName:=v_sPerilObjName, r_sTableName:=sPerilTableName, r_vChildObjectArray:=vChildObjectArray, r_vPropertyArray:=vPropertyArray), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' Build the Insert and Value Lists for the Claim Peril Object

        ' Loop Round the Properties

        For lRow As Integer = vPropertyArray.GetLowerBound(1) To vPropertyArray.GetUpperBound(1)
            sPropertyName = CStr(vPropertyArray(0, lRow))

            lReturn = CType(GetPropertyDefDetails(v_sObjectName:=v_sPerilObjName, v_sPropertyName:=sPropertyName, r_lDataType:=lDataType, r_sColumnName:=sColumnName, r_iIsPrimaryKey:=iIsPrimaryKey), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            sColumnName = sColumnName.ToUpper()
            sPropertyName = sPropertyName.ToUpper()

            ' Ignore the Primary Keys as they have already been included in SP Template
            If iIsPrimaryKey <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Use the data type from the gis_property
                sSQLType = GetSQLType(lDataType)

                ' Ignore the Static Fields
                If Not IsClaimStaticColumn(sColumnName) Then

                    ' Insert List
                    sPerilInsertList.Append(sColumnName & " , " & Strings.ChrW(13) & Strings.ChrW(10))

                    ' Value List
                    If lDataType = iGISSharedConstants.GISDataTypeDate Then
                        sPerilValueList.Append("CONVERT(datetime," & "dmccp." & sColumnName & ",120)" & " , " & Strings.ChrW(13) & Strings.ChrW(10))
                    Else
                        sPerilValueList.Append("dmccp." & sColumnName & " , " & Strings.ChrW(13) & Strings.ChrW(10))
                    End If
                End If
            End If
        Next lRow

        sSQL = ""

        sSQL = sSQL & "CREATE PROCEDURE spg_" & GISDataModelCode & "_copy_claim" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    @claim_id int ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    @copy_claim_id int" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "AS" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "BEGIN" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    -- Copy over the Claim" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    INSERT" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      INTO " & GISDataModelCode & "_claim (" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & sInsertList.ToString()

        sSQL = sSQL & "        claim_id)" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      SELECT" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & sValueList.ToString()

        sSQL = sSQL & "        @copy_claim_id" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      FROM " & GISDataModelCode & "_claim" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      WHERE claim_id = @claim_id" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "        " & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      -- Copy All Claim Perils" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      -- Need to select from the claim_peril table so that we get the correct claim_peril_id's" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      INSERT" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      INTO " & GISDataModelCode & "_claim_peril (" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & sPerilInsertList.ToString()
        '    sSQL = sSQL & "            reserve_details ,"
        '    sSQL = sSQL & "            payment_details ,"
        '    sSQL = sSQL & "            extra ,"
        sSQL = sSQL & "          claim_peril_id ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "          claim_id)" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      SELECT" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & sPerilValueList.ToString()
        '    sSQL = sSQL & "            tccp.reserve_details ,"
        '    sSQL = sSQL & "            tccp.payment_details ,"
        '    sSQL = sSQL & "            tccp.extra ,"
        sSQL = sSQL & "          copy_claim_peril.claim_peril_id ," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "          @copy_claim_id" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "      FROM  " & GISDataModelCode & "_claim_peril dmccp " & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "         INNER JOIN (Select claim_peril_id, base_claim_peril_id FROM claim_peril) claim_peril ON " & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           dmccp.claim_peril_id = claim_peril.claim_peril_id " & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "           INNER JOIN (Select claim_peril_id, base_claim_peril_id FROM claim_peril " & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "             WHERE claim_id = @copy_claim_id) copy_claim_peril ON " & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "             claim_peril.base_claim_peril_id = copy_claim_peril.base_claim_peril_id "
        sSQL = sSQL & "      WHERE dmccp.claim_id = @claim_id" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "" & Strings.ChrW(13) & Strings.ChrW(10)
        ' RFC090403 - Fix the Claim/WorkClaimID on the GISPolicyLink
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)

        '***********
        ' MEvans : 07-01-2004 : CQ3414
        ' claims versioning now works differently
        ' so this code is no longer appropriate..
        'sSQL = sSQL & "      UPDATE gis_policy_link" & vbCrLf
        'sSQL = sSQL & "      SET    work_claim_id = @work_claim_id" & vbCrLf
        'sSQL = sSQL & "      WHERE  claim_id = @claim_id" & vbCrLf
        '***********

        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "END" & Strings.ChrW(13) & Strings.ChrW(10)

        ' RAM20040831 : Commented the following code
        'sSQL = sSQL & "GO" & vbCrLf

        ' Build the SP Name
        sSPName = "spg_" & GISDataModelCode & "_copy_claim"
        ' Build the SP File Name
        sSPFileName = sSPName & ".sql"

        ' RDC 10072001 START
        lReturn = CType(iGISSharedConstants.GetLoadSPPath(GISDataModelCode, sLoadSPPath), gPMConstants.PMEReturnCode)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' check path exists, create if not
        sPathTest = FileSystem.Dir(sLoadSPPath, FileAttribute.Directory)

        If sPathTest = "" Then
            Directory.CreateDirectory(sLoadSPPath)
        End If

        ' Get the Next File Number
        iFileNum = FileSystem.FreeFile()

        FileSystem.FileOpen(iFileNum, sLoadSPPath & "\" & sSPFileName, OpenMode.Output)
        ' RDC 10072001 END

        ' Write out the Stored Procedure
        FileSystem.PrintLine(iFileNum, sSQL)
        FileSystem.FileClose(iFileNum) ' Close file.

        sSQL = ""

        ' RAM20040831 : Commented the following code
        'sSQL = sSQL & "    SET QUOTED_IDENTIFIER OFF" & vbCrLf
        'sSQL = sSQL & "    GO" & vbCrLf
        'sSQL = sSQL & "    SET ANSI_NULLS ON" & vbCrLf
        'sSQL = sSQL & "    GO" & vbCrLf
        'sSQL = sSQL & "    " & vbCrLf
        'sSQL = sSQL & "    EXEC DDLDropProcedure 'spg_" & GISDataModelCode & "_copy_work_to_claim'" & vbCrLf
        'sSQL = sSQL & "    GO" & vbCrLf
        'sSQL = sSQL & "    " & vbCrLf

        '    sSQL = sSQL & "    CREATE PROCEDURE spg_" & GISDataModelCode & "_copy_work_to_claim" & vbCrLf
        '    sSQL = sSQL & "        @claim_id int ," & vbCrLf
        '    sSQL = sSQL & "        @work_claim_id int" & vbCrLf
        '    sSQL = sSQL & "    AS" & vbCrLf
        '    sSQL = sSQL & "    BEGIN" & vbCrLf
        '    sSQL = sSQL & "    " & vbCrLf
        '    sSQL = sSQL & "        -- Delete existing Claim" & vbCrLf
        '    sSQL = sSQL & "        IF EXISTS(select 1 from " & GISDataModelCode & "_claim where claim_id = @claim_id)" & vbCrLf
        '    sSQL = sSQL & "        BEGIN" & vbCrLf
        '    sSQL = sSQL & "            DELETE FROM " & GISDataModelCode & "_claim WHERE claim_id = @claim_id" & vbCrLf
        '    sSQL = sSQL & "        END" & vbCrLf
        '    sSQL = sSQL & "    " & vbCrLf
        '    sSQL = sSQL & "        -- Delete existing Claim Perils" & vbCrLf
        '    sSQL = sSQL & "        IF EXISTS(select 1 from " & GISDataModelCode & "_claim_peril where claim_id = @claim_id)" & vbCrLf
        '    sSQL = sSQL & "        BEGIN" & vbCrLf
        '    sSQL = sSQL & "            DELETE FROM " & GISDataModelCode & "_claim_peril WHERE claim_id = @claim_id" & vbCrLf
        '    sSQL = sSQL & "        END" & vbCrLf
        '    sSQL = sSQL & "    " & vbCrLf
        '    sSQL = sSQL & "        -- Copy over the Claim" & vbCrLf
        '    sSQL = sSQL & "        INSERT" & vbCrLf
        '    sSQL = sSQL & "          INTO " & GISDataModelCode & "_claim (" & vbCrLf
        '    sSQL = sSQL & sInsertList
        ''    sSQL = sSQL & "            reallow_ncd ," & vbCrLf
        ''    sSQL = sSQL & "            ncd_status_changed ," & vbCrLf
        ''    sSQL = sSQL & "            extra ," & vbCrLf
        '    sSQL = sSQL & "            claim_id)" & vbCrLf
        '    sSQL = sSQL & "          SELECT" & vbCrLf
        '    sSQL = sSQL & sValueList
        ''    sSQL = sSQL & "            reallow_ncd ," & vbCrLf
        ''    sSQL = sSQL & "            ncd_status_changed ," & vbCrLf
        ''    sSQL = sSQL & "            extra ," & vbCrLf
        '    sSQL = sSQL & "            @claim_id" & vbCrLf
        '    sSQL = sSQL & "          From " & GISDataModelCode & "_work_claim" & vbCrLf
        '    sSQL = sSQL & "          WHERE claim_id = @work_claim_id" & vbCrLf
        '    sSQL = sSQL & "            " & vbCrLf
        '    sSQL = sSQL & "    " & vbCrLf
        '    sSQL = sSQL & "          -- Copy All Claim Perils" & vbCrLf
        '    sSQL = sSQL & "          INSERT" & vbCrLf
        '    sSQL = sSQL & "            INTO " & GISDataModelCode & "_claim_peril (" & vbCrLf
        ''    sSQL = sSQL & "              reserve_details ," & vbCrLf
        ''    sSQL = sSQL & "              payment_details ," & vbCrLf
        ''    sSQL = sSQL & "              extra ," & vbCrLf
        '    sSQL = sSQL & sPerilInsertList
        '    sSQL = sSQL & "              claim_peril_id ," & vbCrLf
        '    sSQL = sSQL & "              claim_id)" & vbCrLf
        '    sSQL = sSQL & "            SELECT" & vbCrLf
        '    sSQL = sSQL & sPerilValueList
        ''    sSQL = sSQL & "              tcwcp.reserve_details ," & vbCrLf
        ''    sSQL = sSQL & "              tcwcp.payment_details ," & vbCrLf
        ''    sSQL = sSQL & "              tcwcp.extra , " & vbCrLf
        '    sSQL = sSQL & "              cp.claim_peril_id ," & vbCrLf
        '    sSQL = sSQL & "              cp.claim_id" & vbCrLf
        '    sSQL = sSQL & "            FROM  claim_peril cp" & vbCrLf
        '    sSQL = sSQL & "            INNER JOIN work_claim_peril wcp ON wcp.original_claim_peril_id = cp.claim_peril_id" & vbCrLf
        '    sSQL = sSQL & "            INNER JOIN " & GISDataModelCode & "_work_claim_peril dmccp ON dmccp.claim_peril_id = wcp.claim_peril_id" & vbCrLf
        '    sSQL = sSQL & "            WHERE cp.claim_id = @claim_id" & vbCrLf
        '    sSQL = sSQL & "    " & vbCrLf
        '    ' RFC090403 - Fix the Claim/WorkClaimID on the GISPolicyLink
        '    ' Set the claim_id on GPL (it will be NULL at the moment for new Claims) and
        '    ' also set the work_claim_to NULL as we have finished with the work_claim now
        '    sSQL = sSQL & "        UPDATE gis_policy_link" & vbCrLf
        '    sSQL = sSQL & "        SET    claim_id = @claim_id ," & vbCrLf
        '    sSQL = sSQL & "               work_claim_id = NULL" & vbCrLf
        '    sSQL = sSQL & "        WHERE  work_claim_id = @work_claim_id" & vbCrLf
        '    sSQL = sSQL & "    END" & vbCrLf
        '
        '    ' RAM20040831 : Commented the following code
        '    'sSQL = sSQL & "    GO" & vbCrLf
        '
        '    ' Build the SP Name
        '    sSPName = "spg_" & GISDataModelCode & "_copy_work_to_claim"
        '    ' Build the SP File Name
        '    sSPFileName = sSPName & ".sql"
        '
        '    ' Get the Next File Number
        '    iFileNum = FreeFile
        '
        '    Open sLoadSPPath$ & "\" & sSPFileName For Output As #iFileNum
        '    ' RDC 10072001 END
        '
        '    ' Write out the Stored Procedure
        '    Print #iFileNum, sSQL
        '    Close #iFileNum   ' Close file.

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: IsClaimStaticColumn
    '
    ' Description: Returns True if the Column is one of the Static Header
    '              Fields on Claim or Claim_Peril
    ' ***************************************************************** '
    Private Function IsClaimStaticColumn(ByVal v_sColumnName As String) As Boolean

        Dim result As Boolean = False


        ' Ignore the Static Fields

        Select Case v_sColumnName
            ' Claim & Claim_Peril Static Header Fields
            ' PW150703 - PS68 - Add GIS_SCREEN_ID to static fields
            Case "CLAIM_ID", "POLICY_ID", "POLICY_NUMBER", "CLAIM_NUMBER", "DESCRIPTION", "CLAIM_STATUS_ID", "PROGRESS_STATUS_ID", "PRIMARY_CAUSE_ID", "SECONDARY_CAUSE_ID", "CATASTROPHE_CODE_ID", "COINSURANCE_TREATMENT_ID", "LOSS_FROM_DATE", "LOSS_TO_DATE", "REPORTED_DATE", "REPORTED_TO_DATE", "LAST_MODIFIED_DATE", "HANDLER_ID", "CURRENCY_ID", "INFO_ONLY", "LIKELY_CLAIM", "LOCATION", "TOWN", "RISK_TYPE_ID", "CLIENT_NAME", "CLIENT_ADDRESS", "CLIENT_TEL_NO", "CLIENT_FAX_NO", "CLIENT_MOBILE_NO", "CLIENT_EMAIL", "CLIENT_CLAIM_NUMBER", "INSURER_NAME", "INSURER_ADDRESS", "INSURER_TEL_NO", "INSURER_FAX_NO", "INSURER_EMAIL", "INSURER_CLAIM_NUMBER", "INSURER_CONTACT", "VAT_REGISTERED", "VAT_REG_NO", "COMMENTS", "CLAIMS_STATUS_DATE", "CLIENT_SHORT_NAME", "INSURER_SHORT_NAME", "CLIENT_TEL_NO_OFF", "USER_DEFINED_FIELD_A", "USER_DEFINED_FIELD_B", "USER_DEFINED_FIELD_C", "USER_DEFINED_FIELD_D", "USER_DEFINED_FIELD_E", "CLIENT_ID", "CLAIM_FOLDER_ID", "CLAIM_VERSION_NUMBER", "CLAIM_VERSION_STATUS_ID", "CREATE_DATE", "CREATED_BY_ID", "MODIFIED_BY_ID", "ACCEPTANCE_STATUS_ID", "CLAIM_PERIL_ID", "CLAIMS_PERIL_ID", "PERIL_TYPE_ID", "DESCRIPTION", "COMMENTS", "SUM_INSURED", "RI_BAND", "ORIGINAL_CLAIM_ID", "GIS_SCREEN_ID"

                result = True

                ' User Defined (PB) Field
            Case Else

                result = False

        End Select


        Return result

    End Function

    ' ***************************************************************** '
    ' Name: IsAssocClientStaticColumn
    '
    ' Description: Returns True if the Column is one of the Static Header
    '              Fields on Associated Client or Disclosure
    ' ***************************************************************** '
    Private Function IsAssocClientStaticColumn(ByVal v_sColumnName As String) As Boolean

        Dim result As Boolean = False


        ' Ignore the Static Fields

        Select Case v_sColumnName
            ' PW280803 - CQ1912 - add new fields
            ' PW020304 - CQ4074 - add ins file cnt
            Case "INSURANCE_FOLDER_CNT", "PARTY_CNT", "IS_INSURED", "PARTY_TITLE_CODE", "FORENAME", "NAME", "RESOLVED_NAME", "INITIALS", "DATE_OF_BIRTH", "GENDER_CODE", "IS_INSURED", "PARTY_TITLE_CODE", "FORENAME", "NAME", "RESOLVED_NAME", "INITIALS", "DATE_OF_BIRTH", "GENDER_CODE", "PARTY_TYPE_CODE", "PARTY_TYPE_DESCRIPTION", "PARTY_CONVICTION_ID", "CODE", "CONVICTION_DATE", "DESCRIPTION", "FINE_AMT", "SENTENCE_CODE", "SENTENCE_DESCRIPTION", "SENTENCE_DURATION", "SENTENCE_DURATION_QUALIFIER", "SENTENCE_EFFECTIVE_DATE", "STATUS_CODE", "ALCOHOL_LEVEL", "ALCOHOL_MEASUREMENT_METHOD", "DRIVING_LICENCE_PENALTY_PTS", "DISCLOSURE_TYPE_ID", "RISK_CNT", "EFFECTIVE_DATE", "EXPIRY_DATE", "REPLACED_BY_ID", "INSURANCE_FILE_CNT"

                result = True

                ' User Defined (PB) Field
            Case Else

                result = False

        End Select

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: IsRiskStaticColumn
    '
    ' Description: Returns True if the Column is one of the Static Header
    '              Fields on Risk table
    ' RAW 03/07/2003 : CQ1581 :  Added
    ' RAW 21/06/2004 : CQ3761 : renamed v_sColumnName param as v_sPropertyName
    ' ***************************************************************** '
    Private Function IsRiskStaticColumn(ByVal v_sParentObjectName As String, ByVal v_sPropertyName As String) As Boolean

        Dim result As Boolean = False




        If Not v_sParentObjectName.EndsWith("_POLICY_BINDER") Then
            ' This is not from a top level object within the Policy Binder
            Return result
        End If


        ' RAW 21/06/2004 : CQ3761 : renamed property


        Select Case v_sPropertyName.ToUpper()
            Case "DESCRIPTION", "ACCUMULATION", "EML_PERCENTAGE", "COVERAGE", "INSURED_ITEM", "EXTENSIONS", "PACKAGE"
                Return True

                ' User Defined (PB) Field
            Case Else

                Return False

        End Select




        result = False

        ' Log Error Message
        iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsRiskStaticColumnFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsRiskStaticColumn", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function




    ' ***************************************************************** '
    ' Name          : BuildSelectSP_FOR_XML
    '
    ' Description   : Build a Select Stored Procedure for the given Object (For XML)
    ' Edit History  :
    ' RAM20040824   : Created
    ' RAM20041011   : Added the OPTION (KEEP PLAN)
    ' RAW20041020   : CQ7129 : Bug fix (for child objects at > level 2
    ' ***************************************************************** '
    Private Function BuildSelectSP_FOR_XML(ByVal v_sObjectName As String, ByVal v_bTopLevelObject As Boolean, ByVal v_bQuoteObject As Boolean) As Integer

        'Dim result As Integer = 0
        Const ACColObjectID As Integer = 0
        Const ACColObjectName As Integer = 1
        Const ACColTableName As Integer = 2
        Const ACColParentObjectID As Integer = 3
        'Const ACColParentObjectName As Integer = 4
        'Const ACColParentTableName As Integer = 5
        Const ACColParentColumnArray As Integer = 6 ' RAW 20/10/2004 : CQ7129 : added
        Const ACColPropertiesArray As Integer = 7
        'Const ACColMax As Integer = 7

        Const ACColPropertyName As Integer = 0
        Const ACColColumnName As Integer = 1
        Const ACColDataType As Integer = 2
        Const ACColGISObjectID As Integer = 3 ' RAW 20/10/2004 : CQ7129 : added

        Const ACOIKeyConst As Integer = 500000 ' This constant helps to make sure that the NextOI Key will be const accross the XML document

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sTableName As String = ""
        Dim sParentObjectName As String = ""
        Dim sParentTableName As String = ""
        Dim sOIKey As String = ""
        Dim sParentOIKey As String = ""
        Dim sSelectSQL As String = ""
        ' Used to Create the SP
        Dim iFileNum As Integer
        Dim sSPName, sSPFileName As String

        ' Output Strings
        Dim sLoadSPPath As String = ""
        Dim sPathTest As String = ""
        Dim sOrderBy As New StringBuilder

        Dim vGISObjectsAndProperties As Object = Nothing
        Dim lFromOuter, lToOuter, lFromInner, lToInner As Integer
        Dim vObjectPropertiesArray As Object = Nothing
        Dim vObjectPropertiesArrayOuter As Object = Nothing
        Dim vOuterParentColumnArray As Object = Nothing
        Dim vPropertiesArray(,) As Object = Nothing
        Dim lFrom, lTo As Integer
        Dim sCommaSeparator, sColumnNameToUse, sColumnAliasToUse As String




        ' First Build the Array of Objects and Properties.

        lReturn = BuildGISObjectsPropertiesArray(v_sObjectName:=v_sObjectName, r_vFinalArray:=vGISObjectsAndProperties)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' Check if we got an array
        If Not Informations.IsArray(vGISObjectsAndProperties) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the details of the top level object the main array


        vObjectPropertiesArray = vGISObjectsAndProperties(0)

        sTableName = CStr(vObjectPropertiesArray(ACColTableName))

        ' Build the SP Name
        sSPName = "spg_" & sTableName.ToLower() & "_sel"
        ' Build the SP File Name
        sSPFileName = sSPName & ".sql"

        ' Get the Path from the Registry
        lReturn = CType(iGISSharedConstants.GetLoadSPPath(GISDataModelCode, sLoadSPPath), gPMConstants.PMEReturnCode)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' check path exists, create if not
        sPathTest = FileSystem.Dir(sLoadSPPath, FileAttribute.Directory)

        If sPathTest = "" Then
            Directory.CreateDirectory(sLoadSPPath)
        End If


        lFromOuter = vGISObjectsAndProperties.GetLowerBound(0)

        lToOuter = vGISObjectsAndProperties.GetUpperBound(0)
        lFromInner = lFromOuter
        lToInner = lToOuter

        ' Get the Next File Number
        iFileNum = FileSystem.FreeFile()

        FileSystem.FileOpen(iFileNum, sLoadSPPath & "\" & sSPFileName, OpenMode.Output)
        ' RDC 10072001 END

        ' Write out the Stored Procedure
        '    Print #iFileNum, "SET QUOTED_IDENTIFIER OFF"
        '    Print #iFileNum,
        '    Print #iFileNum, "EXEC DDLDropProcedure '"; sSPName; "'"
        '    Print #iFileNum, "GO"
        '    Print #iFileNum,
        '
        FileSystem.PrintLine(iFileNum, "CREATE PROCEDURE " & sSPName)
        FileSystem.PrintLine(iFileNum, "@gis_policy_link_id INTEGER")

        '    If (v_bTopLevelObject) Then
        '        Print #iFileNum, "@gis_policy_link_id INTEGER ,"
        '        If (v_bQuoteObject) Then
        '            Print #iFileNum, "@object_count INTEGER OUTPUT ,"
        '            Print #iFileNum, "@quote_count INTEGER OUTPUT"
        '        Else
        '            Print #iFileNum, "@object_count INTEGER OUTPUT"
        '        End If
        '    Else
        '        Print #iFileNum, sParamDecs
        '    End If

        FileSystem.PrintLine(iFileNum, "AS")
        FileSystem.PrintLine(iFileNum, "BEGIN")
        FileSystem.PrintLine(iFileNum)
        FileSystem.PrintLine(iFileNum, "SET NOCOUNT ON")
        FileSystem.PrintLine(iFileNum)

        '''''''''' REFERENCE ''''''''''''''''''''''''''''''''''''''''''''''''
        '    vObjectPropertiesArray(ACColObjectID)
        '    vObjectPropertiesArray(ACColObjectName)
        '    vObjectPropertiesArray(ACColTableName)
        '    vObjectPropertiesArray(ACColParentObjectID)
        '    vObjectPropertiesArray(ACColParentObjectName)
        '    vObjectPropertiesArray(ACColParentTableName)
        '    vObjectPropertiesArray(ACColPropertiesArray)
        '''''''''' REFERENCE ''''''''''''''''''''''''''''''''''''''''''''''''

        For lCounterOuter As Integer = lFromOuter To lToOuter

            ' Make sure that we add UNION ALL from the Second Object
            If lCounterOuter > 0 Then
                FileSystem.PrintLine(iFileNum)
                FileSystem.PrintLine(iFileNum, "UNION ALL")
                FileSystem.PrintLine(iFileNum)
            End If

            ' Get the details from the main array


            vObjectPropertiesArrayOuter = vGISObjectsAndProperties(lCounterOuter)

            ' Add Tag (GIS Object ID is the Tag)

            FileSystem.PrintLine(iFileNum, "SELECT " & CStr(vObjectPropertiesArrayOuter(ACColObjectID)) & " As Tag, ")

            ' Add the Parent

            If CDbl(vObjectPropertiesArrayOuter(ACColParentObjectID)) = -1 Then
                ' Then we have a Top Level Object, so the parent is NULL
                FileSystem.PrintLine(iFileNum, "NULL As Parent, ")
            Else

                FileSystem.PrintLine(iFileNum, CStr(vObjectPropertiesArrayOuter(ACColParentObjectID)) & " As Parent, ")
            End If


            sTableName = CStr(vObjectPropertiesArrayOuter(ACColTableName)).Trim()


            vOuterParentColumnArray = vObjectPropertiesArrayOuter(ACColParentColumnArray) ' RAW 20/10/2004 : CQ7129 : added

            For lCounterInner As Integer = lFromInner To lToInner


                ' Get the details from the main array


                vObjectPropertiesArray = vGISObjectsAndProperties(lCounterInner)

                ' Table specific fields


                vPropertiesArray = vObjectPropertiesArray(ACColPropertiesArray)

                lFrom = vPropertiesArray.GetLowerBound(1)

                lTo = vPropertiesArray.GetUpperBound(1)
                For lCounter As Integer = lFrom To lTo

                    ' Check for NULL or ColumnName to Use
                    ' set the defaults
                    sColumnNameToUse = "NULL"



                    sColumnAliasToUse = "[" & CStr(vObjectPropertiesArray(ACColObjectName)).Trim() & "!" &
                                        CStr(vObjectPropertiesArray(ACColObjectID)) & "!" &
                                        CStr(vPropertiesArray(ACColPropertyName, lCounter)).Trim() & "]" ' RAW 20/10/2004 : CQ7129 : added

                    If lCounterInner = lCounterOuter Then
                        ' this is the table that we are processing
                        ' We need to fill in the Column Name

                        sColumnNameToUse = CStr(vPropertiesArray(ACColColumnName, lCounter)).Trim()
                    Else
                        ' this is not the table we are processing but we still need to populate the columns for it in the select statement
                        ' RAW 20/10/2004 : CQ7129 :
                        If Informations.IsArray(vOuterParentColumnArray) Then
                            ' we have links to a  parent table
                            ' Process each of the parents properties that this table links to

                            For lXXX As Integer = vOuterParentColumnArray.GetLowerBound(1) To vOuterParentColumnArray.GetUpperBound(1)




                                If vObjectPropertiesArray(ACColObjectID).Equals(vOuterParentColumnArray(ACColGISObjectID, lXXX)) And vPropertiesArray(ACColColumnName, lCounter).Equals(vOuterParentColumnArray(ACColColumnName, lXXX)) Then
                                    ' we have found one parent property that is a link so we must populate it

                                    sColumnNameToUse = CStr(vPropertiesArray(ACColColumnName, lCounter)).Trim()

                                    If sColumnAliasToUse <> "" And (sOrderBy.ToString().IndexOf(sColumnAliasToUse) + 1) <= 0 Then
                                        sOrderBy.Append("," & sColumnAliasToUse) ' RAW 20/10/2004 : CQ7129 : added
                                    End If

                                    Exit For
                                End If
                            Next lXXX
                        End If
                    End If

                    If lCounterInner = lToInner And lCounter = lTo Then
                        ' We have reached the last element, so no need for a comma
                        sCommaSeparator = ""
                    Else
                        sCommaSeparator = ","
                    End If

                    ' We have to add 2 additional properties, OI and US to be added at the top of each object
                    If lCounter = lFrom Then
                        If sColumnNameToUse <> "NULL" Then

                            If lCounterOuter = lFromOuter Then
                                ' We have to add the constant for the first object property ONLY
                                'OI


                                FileSystem.PrintLine(iFileNum, "'OI' + CAST(" & sColumnNameToUse & " + " & CStr(ACOIKeyConst) & " AS VARCHAR(10)) As " &
                                                     "[" & CStr(vObjectPropertiesArray(ACColObjectName)).Trim() & "!" & CStr(vObjectPropertiesArray(ACColObjectID)) &
                                                         "!OI]" & sCommaSeparator)
                            Else


                                FileSystem.PrintLine(iFileNum, "'OI' + CAST(" & sTableName & "_ID AS VARCHAR(10)) As " &
                                                     "[" & CStr(vObjectPropertiesArray(ACColObjectName)).Trim() & "!" & CStr(vObjectPropertiesArray(ACColObjectID)) &
                                                         "!OI]" & sCommaSeparator)
                            End If

                            'US


                            FileSystem.PrintLine(iFileNum, "0 As " &
                                                 "[" & CStr(vObjectPropertiesArray(ACColObjectName)).Trim() & "!" & CStr(vObjectPropertiesArray(ACColObjectID)) &
                                                     "!US]" & sCommaSeparator)
                        Else
                            'OI


                            FileSystem.PrintLine(iFileNum, "NULL As " &
                                                 "[" & CStr(vObjectPropertiesArray(ACColObjectName)).Trim() & "!" & CStr(vObjectPropertiesArray(ACColObjectID)) &
                                                     "!OI]" & sCommaSeparator)

                            'US


                            FileSystem.PrintLine(iFileNum, "NULL As " &
                                                 "[" & CStr(vObjectPropertiesArray(ACColObjectName)).Trim() & "!" & CStr(vObjectPropertiesArray(ACColObjectID)) &
                                                     "!US]" & sCommaSeparator)
                        End If

                    End If

                    ' Other standard properties for the objects

                    ' Check if we have a date field, if so, USE Convert function to get the output in appropriate format

                    If CDbl(vPropertiesArray(ACColDataType, lCounter)) = iGISSharedConstants.GISDataTypeDate Then
                        ' Check if we have a valid field name
                        If sColumnNameToUse <> "NULL" Then
                            sColumnNameToUse = "CONVERT(VARCHAR(20), " & sColumnNameToUse & ", 120)"
                        End If
                    End If

                    ' RAW 20/10/2004 : CQ7129 : replaced literal with sColumnAliasToUse
                    FileSystem.PrintLine(iFileNum, sColumnNameToUse & " As " & sColumnAliasToUse & sCommaSeparator)

                Next lCounter ' For every properties in the object

            Next lCounterInner ' For every object (Inner Loop)

            FileSystem.PrintLine(iFileNum, "FROM  " & sTableName.Trim())
            FileSystem.PrintLine(iFileNum, "WHERE " & GISDataModelCode.Trim() & "_Policy_Binder_ID = @gis_policy_link_id")

        Next lCounterOuter ' For every object (Outer Loop)

        ' ORDER BY
        ' RAW 20/10/2004 : CQ7129 : added
        FileSystem.PrintLine(iFileNum)
        FileSystem.PrintLine(iFileNum, "ORDER BY " & sOrderBy.ToString().Substring(sOrderBy.ToString().Length - (sOrderBy.ToString().Length - 1)) & ", parent")
        FileSystem.PrintLine(iFileNum)

        ' FOOTER
        FileSystem.PrintLine(iFileNum)
        FileSystem.PrintLine(iFileNum, "FOR XML EXPLICIT")
        FileSystem.PrintLine(iFileNum)

        ' RAM20041011 - Added the following Option to the stored procedure
        FileSystem.PrintLine(iFileNum, "OPTION (KEEP PLAN)")
        FileSystem.PrintLine(iFileNum)

        FileSystem.PrintLine(iFileNum, "SET NOCOUNT OFF")
        FileSystem.PrintLine(iFileNum)
        FileSystem.PrintLine(iFileNum, "END")
        '    Print #iFileNum, "GO"
        FileSystem.PrintLine(iFileNum)
        FileSystem.FileClose(iFileNum) ' Close file.

        ' Return PMTure, since everything is OK

        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    ' ***************************************************************** '
    ' Name          : BuildGISObjectsPropertiesArray
    '
    ' Description   : Build a Array of GIS Objects and Properties, for the given Object Name
    '
    ' Notes         : Called from BuildSelectSP_FOR_XML
    ' Edit History  :
    ' RAM20040824   : Created
    ' ***************************************************************** '
    Private Function BuildGISObjectsPropertiesArray(ByVal v_sObjectName As String, ByRef r_vFinalArray() As Object) As gPMConstants.PMEReturnCode


        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Const ACColObjectID As Integer = 0
        Const ACColObjectName As Integer = 1
        Const ACColTableName As Integer = 2
        Const ACColParentObjectID As Integer = 3
        Const ACColParentObjectName As Integer = 4
        Const ACColParentTableName As Integer = 5
        Const ACColParentColumnArray As Integer = 6 ' RAW 20/10/2004 : CQ7129 : added
        Const ACColPropertiesArray As Integer = 7
        Const ACColMax As Integer = 7

        Const ACColPropertyName As Integer = 0
        Const ACColColumnName As Integer = 1
        Const ACColDataType As Integer = 2
        Const ACColGISObjectID As Integer = 3 ' RAW 20/10/2004 : CQ7129 : added

        ' Object Definition
        Dim lIsQuoteObject, lGISObjectID, lGISPropertyID As Integer
        Dim iIsPrimaryKey As Integer
        Dim sTableName As String = ""
        Dim lMaxInstances, lPolarisObjectID As Integer
        Dim sParentObjectName As String = ""
        Dim vChildObjectArray As Object = Nothing
        Dim vPropertyArray(,) As Object = Nothing
        Dim sPropertyName As String = ""
        Dim sColumnName As String = ""
        Dim sSelectSQL As String = ""
        Dim lNonGISType As Integer
        Dim sChildObjectName As String = ""
        Dim lDataType As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lFrom, lTo As Integer
        Dim vObjectPropertiesArray() As Object = Nothing
        Dim vPropertiesArray(,) As Object = Nothing
        Dim lParentGISObjectID As Integer
        Dim sParentTableName As String = ""
        Dim sParentPropertyName As String = ""
        Dim sParentColumnName As String = ""
        Dim lParentDataType As Integer
        Dim iParentIsPrimaryKey As Integer
        Dim vParentPropertyArray As Object = Nothing
        Dim vParentColumnArray(,) As Object = Nothing
        Dim sParentsParentObjectName As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the Object Definition Details
        lReturn = CType(GetObjectDefDetails(v_sObjectName:=v_sObjectName, r_lIsQuoteObject:=lIsQuoteObject, r_lGISObjectID:=lGISObjectID, r_sTableName:=sTableName, r_lMaxInstances:=lMaxInstances, r_lPolarisObjectID:=lPolarisObjectID, r_sParentObjectName:=sParentObjectName, r_vChildObjectArray:=vChildObjectArray, r_vPropertyArray:=vPropertyArray, r_sSelectSQL:=sSelectSQL, r_lIsNonGIS:=lNonGISType), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' If this is a SPECIAL object (i.e. Disclosures or CLaims) , exit
        If (lNonGISType <> GISDataModelType.GISOTRisk) And (lNonGISType <> GISDataModelType.GISOTNonGisSpecials) Then
            Return result
        End If

        ' Loop Round the Properties

        lFrom = vPropertyArray.GetLowerBound(1)

        lTo = vPropertyArray.GetUpperBound(1)

        ReDim vPropertiesArray(3, lTo) ' RAW 20/10/2004 : CQ7129 : increased dimension 1 by 1

        For lRow As Integer = lFrom To lTo


            sPropertyName = CStr(vPropertyArray(0, lRow)).Trim()

            lReturn = CType(GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, r_lDataType:=lDataType, r_sColumnName:=sColumnName, r_iIsPrimaryKey:=iIsPrimaryKey, r_lGISPropertyID:=lGISPropertyID), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If


            vPropertiesArray(ACColPropertyName, lRow) = sPropertyName

            vPropertiesArray(ACColColumnName, lRow) = sColumnName.Trim()

            vPropertiesArray(ACColDataType, lRow) = lDataType

        Next lRow

        sParentObjectName = sParentObjectName.Trim()

        ReDim vObjectPropertiesArray(ACColMax)

        vObjectPropertiesArray(ACColObjectID) = lGISObjectID

        vObjectPropertiesArray(ACColObjectName) = v_sObjectName.Trim()

        vObjectPropertiesArray(ACColTableName) = sTableName.Trim()

        vObjectPropertiesArray(ACColParentObjectName) = sParentObjectName


        vObjectPropertiesArray(ACColPropertiesArray) = vPropertiesArray

        If sParentObjectName.Length > 0 Then

            ' RAW 20/10/2004 : CQ7129 : this should have been written as a recursive routine but did not have time at time of writing
            Do While sParentObjectName.Length > 0
                ' Get the Parent Object Definition Details, to get the table Name, GIS Object ID
                lReturn = CType(GetObjectDefDetails(v_sObjectName:=sParentObjectName, r_lGISObjectID:=lParentGISObjectID, r_sTableName:=sParentTableName, r_sParentObjectName:=sParentsParentObjectName, r_vPropertyArray:=vParentPropertyArray), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

                ' RAW 20/10/2004 : CQ7129 : added
                ' only do this for the immediate parent

                If Object.Equals(vObjectPropertiesArray(ACColParentObjectID), Nothing) Then

                    vObjectPropertiesArray(ACColParentObjectID) = lParentGISObjectID

                    vObjectPropertiesArray(ACColParentTableName) = sParentTableName.Trim()
                End If

                If Informations.IsArray(vParentPropertyArray) Then

                    For lRow As Integer = vParentPropertyArray.GetLowerBound(1) To vParentPropertyArray.GetUpperBound(1)


                        sParentPropertyName = CStr(vParentPropertyArray(0, lRow)).Trim()

                        ' get additional details for this property (from the parent object)
                        lReturn = CType(GetPropertyDefDetails(v_sObjectName:=sParentObjectName, v_sPropertyName:=sParentPropertyName, r_lDataType:=lParentDataType, r_sColumnName:=sParentColumnName, r_iIsPrimaryKey:=iParentIsPrimaryKey), gPMConstants.PMEReturnCode)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return lReturn
                        End If

                        If iParentIsPrimaryKey = ToSafeDouble("1") Then

                            If Informations.IsArray(vParentColumnArray) Then

                                ReDim Preserve vParentColumnArray(3, vParentColumnArray.GetUpperBound(1) + 1)
                            Else
                                ReDim vParentColumnArray(3, 0)
                            End If



                            vParentColumnArray(ACColPropertyName, vParentColumnArray.GetUpperBound(1)) = sParentPropertyName


                            vParentColumnArray(ACColColumnName, vParentColumnArray.GetUpperBound(1)) = sParentColumnName.Trim()

                            vParentColumnArray(ACColDataType, lRow) = lParentDataType


                            vParentColumnArray(ACColGISObjectID, vParentColumnArray.GetUpperBound(1)) = lParentGISObjectID
                        End If
                    Next lRow
                End If

                ' now look for properties from the parent's parent
                sParentObjectName = sParentsParentObjectName
            Loop



            vObjectPropertiesArray(ACColParentColumnArray) = vParentColumnArray
        Else
            ' No parent object, means it is the top level object

            vObjectPropertiesArray(ACColParentObjectID) = -1

            vObjectPropertiesArray(ACColParentTableName) = ""
        End If

        ' Return the values to the array
        If Not Informations.IsArray(r_vFinalArray) Then
            ReDim r_vFinalArray(0)
        Else
            ReDim Preserve r_vFinalArray(r_vFinalArray.GetUpperBound(0) + 1)
        End If


        r_vFinalArray(r_vFinalArray.GetUpperBound(0)) = vObjectPropertiesArray

        ' Build the Array for Each Child Table, below the object name passed in
        If Informations.IsArray(vChildObjectArray) Then

            For lRow As Integer = vChildObjectArray.GetLowerBound(0) To vChildObjectArray.GetUpperBound(0)

                sChildObjectName = CStr(vChildObjectArray(lRow)).Trim()
                ' RECURSIVE
                lReturn = BuildGISObjectsPropertiesArray(sChildObjectName, r_vFinalArray)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If
            Next lRow
        End If

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: InitialiseDataSet
    '
    ' Description: Initialise the Memory Structure from the Object and
    '              Property Arrays supplied.
    '
    ' ***************************************************************** '
    Public Function BuildDataSetXSD(ByRef v_sGisDataModelCode As String, ByRef v_vObjectArray(,) As Object, ByRef v_vPropertyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sXSDFileName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Build the Standard XSD

            m_oDataSetXSD = New XmlDocument()

            ' Get the File Name for this Data Model Code
            lReturn = CType(iGISSharedConstants.GetDataSetXSDFileName(v_sDataModelCode:=v_sGisDataModelCode, r_sDataSetXSDFile:=sXSDFileName), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Initialise the Definition
            lReturn = CType(InitDataSetXSD(v_sGisDataModelCode:=v_sGisDataModelCode), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Load the Objects from the Database

            lReturn = CType(LoadObjectXSDDefs(v_vObjectArray:=v_vObjectArray, v_vPropertyArray:=v_vPropertyArray), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDataSetXSD.Save(sXSDFileName)

            'Build the Data Transfer XSD

            m_oDataSetXSD = New XmlDocument()

            ' Get the File Name for this Data Model Code
            lReturn = CType(iGISSharedConstants.GetDataSetDTXSDFileName(v_sDataModelCode:=v_sGisDataModelCode, r_sDataSetDTXSDFile:=sXSDFileName), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Initialise the Definition
            lReturn = CType(InitDataSetXSD(v_sGisDataModelCode:=v_sGisDataModelCode), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Load the Objects from the Database

            lReturn = CType(LoadObjectXSDDefs(v_vObjectArray:=v_vObjectArray, v_vPropertyArray:=v_vPropertyArray, v_blDataTransferXSD:=True), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDataSetXSD.Save(sXSDFileName)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BuildDataSetXSD Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildDataSetXSD", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result


            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: InitDataSetXSD
    '
    ' Description: Initialises the DataSetDefinition XML Document by
    '              adding the top level enteties.
    '
    ' ***************************************************************** '
    Private Function InitDataSetXSD(ByRef v_sGisDataModelCode As String, Optional ByVal v_blDataTransferXSD As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim oElement, oDataset, oChildObject, oDSComplexType, oDSSequence, oCOComplexType, oCOSequence, oAttribute, oAny As XmlElement
        Dim pi As XmlProcessingInstruction



        result = gPMConstants.PMEReturnCode.PMTrue

        pi = m_oDataSetXSD.CreateProcessingInstruction("xml", "version='1.0'")

        m_oDataSetXSD.InsertBefore(pi, m_oDataSetXSD.ChildNodes.Item(0))

        Dim namespaceUri As String = "http://www.w3.org/2001/XMLSchema"
        '<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" elementFormDefault="qualified">
        'DRE requires both xmlns and xmlns:xs
        ' Create the Root Level Schema Element
        oElement = m_oDataSetXSD.CreateElement(ACXSDSchema, namespaceUri)
        ' Set the namespace Attribute
        oElement.SetAttribute(ACXSDAttribSchemaNS, ACXSDAttribSchemaNSValue)
        oElement.SetAttribute(ACXSDAttribSchemaNSXS, ACXSDAttribSchemaNSValue)
        ' Set the elementform attribute
        oElement.SetAttribute(ACXSDAttribSchemaEF, ACXSDAttribSchemaEFValue)

        ' Create the Dataset Schema Element
        oDataset = m_oDataSetXSD.CreateElement(ACXSDElement, namespaceUri)
        ' Set the name Attribute
        oDataset.SetAttribute(ACXSDAttribName, ACXSDDataSet)

        ' Create the Dataset Schema Element
        oDSComplexType = m_oDataSetXSD.CreateElement(ACXSDComplexType, namespaceUri)

        oDataset.AppendChild(oDSComplexType)
        oDSSequence = m_oDataSetXSD.CreateElement(ACXSDSequence, namespaceUri)

        oDSComplexType.AppendChild(oDSSequence)

        ' Create an Attribute element
        oAttribute = m_oDataSetXSD.CreateElement(ACXSDAttribute, namespaceUri)
        oAttribute.SetAttribute(ACXSDAttribName, ACXSDAttribDataModelCode)
        oAttribute.SetAttribute(ACXSDAttribUse, ACXSDAttribUseRequired)
        oAttribute.SetAttribute(ACXSDAttribType, ACXSDAttribTypeString)

        oDSComplexType.AppendChild(oAttribute)
        oAttribute = Nothing

        ' Create an Attribute element
        oAttribute = m_oDataSetXSD.CreateElement(ACXSDAttribute, namespaceUri)
        oAttribute.SetAttribute(ACXSDAttribName, ACXSDAttribNextOINumber)
        oAttribute.SetAttribute(ACXSDAttribUse, ACXSDAttribUseRequired)
        oAttribute.SetAttribute(ACXSDAttribType, ACXSDAttribTypeString)

        oDSComplexType.AppendChild(oAttribute)
        oAttribute = Nothing
        ' Create an Attribute element
        oAttribute = m_oDataSetXSD.CreateElement(ACXSDAttribute, namespaceUri)
        oAttribute.SetAttribute(ACXSDAttribName, ACXSDAttribTransactionType)
        oAttribute.SetAttribute(ACXSDAttribUse, ACXSDAttribUseRequired)
        oAttribute.SetAttribute(ACXSDAttribType, ACXSDAttribTypeString)
        oDSComplexType.AppendChild(oAttribute)
        oAttribute = Nothing

        ' Create an Attribute element
        oAttribute = m_oDataSetXSD.CreateElement(ACXSDAttribute, namespaceUri)
        oAttribute.SetAttribute(ACXSDAttribName, ACXSDAttribEffectiveDate)

        oAttribute.SetAttribute(ACXSDAttribUse, ACXSDAttribUseRequired)
        oAttribute.SetAttribute(ACXSDAttribType, ACXSDAttribTypeDateTime)
        oDSComplexType.AppendChild(oAttribute)
        oAttribute = Nothing

        ' Create an Attribute element
        oAttribute = m_oDataSetXSD.CreateElement(ACXSDAttribute, namespaceUri)
        oAttribute.SetAttribute(ACXSDAttribName, sACXSDAttribCoverStartDate)
        oAttribute.SetAttribute(ACXSDAttribUse, ACXSDAttribUseRequired)
        oAttribute.SetAttribute(ACXSDAttribType, ACXSDAttribTypeDateTime)
        oDSComplexType.AppendChild(oAttribute)
        oAttribute = Nothing

        ' Create an Attribute element
        oAttribute = m_oDataSetXSD.CreateElement(ACXSDAttribute, namespaceUri)
        oAttribute.SetAttribute(ACXSDAttribName, sACXSDAttribCoverEndDate)

        oAttribute.SetAttribute(ACXSDAttribUse, ACXSDAttribUseRequired)
        oAttribute.SetAttribute(ACXSDAttribType, ACXSDAttribTypeDateTime)
        oDSComplexType.AppendChild(oAttribute)
        oAttribute = Nothing

        ' Create an Attribute element
        oAttribute = m_oDataSetXSD.CreateElement(ACXSDAttribute, namespaceUri)
        oAttribute.SetAttribute(ACXSDAttribName, ACXSDAttribAction)
        oAttribute.SetAttribute(ACXSDAttribUse, ACXSDAttribUseRequired)
        oAttribute.SetAttribute(ACXSDAttribType, ACXSDAttribTypeString)
        oDSComplexType.AppendChild(oAttribute)
        oAttribute = Nothing


        ' Create the Risk_Object Schema Element
        oChildObject = m_oDataSetXSD.CreateElement(ACXSDElement, namespaceUri)
        ' Set the names Attribute
        oChildObject.SetAttribute(ACXSDAttribName, ACXSDRiskObjects)
        ' Create the Dataset Schema Element
        oCOComplexType = m_oDataSetXSD.CreateElement(ACXSDComplexType, namespaceUri)

        oChildObject.AppendChild(oCOComplexType)
        oCOSequence = m_oDataSetXSD.CreateElement(ACXSDSequence, namespaceUri)

        oCOComplexType.AppendChild(oCOSequence)

        ' Create an Attribute element
        oAttribute = m_oDataSetXSD.CreateElement(ACXSDAttribute, namespaceUri)
        oAttribute.SetAttribute(ACXSDAttribName, ACXSDAttribOIKey)
        oAttribute.SetAttribute(ACXSDAttribUse, ACXSDAttribUseRequired)
        oAttribute.SetAttribute(ACXSDAttribFixed, ACXSDRiskObjects)

        oCOComplexType.AppendChild(oAttribute)
        oAttribute = Nothing

        ' Set the Root Level Document Element

        oDSSequence.AppendChild(oChildObject)

        ' Create the Deleted_Object Schema Element
        oChildObject = m_oDataSetXSD.CreateElement(ACXSDElement, namespaceUri)
        ' Set the names Attribute
        oChildObject.SetAttribute(ACXSDAttribName, ACXSDDeletedObjects)
        ' Create the Dataset Schema Element
        oCOComplexType = m_oDataSetXSD.CreateElement(ACXSDComplexType, namespaceUri)

        oChildObject.AppendChild(oCOComplexType)
        oCOSequence = m_oDataSetXSD.CreateElement(ACXSDSequence, namespaceUri)
        oAny = m_oDataSetXSD.CreateElement(ACXSDAny, namespaceUri)
        oAny.SetAttribute(ACXSDAttribProcessContents, ACXSDAttribProcessContentsLax)
        oAny.SetAttribute(ACXSDAttribMinOccurs, "0")
        oAny.SetAttribute(ACXSDAttribMaxOccurs, ACXSDAttribMaxOccursUnbounded)


        oCOSequence.AppendChild(oAny)

        oCOComplexType.AppendChild(oCOSequence)

        ' Create an Attribute element
        oAttribute = m_oDataSetXSD.CreateElement(ACXSDAttribute, namespaceUri)
        oAttribute.SetAttribute(ACXSDAttribName, ACXSDAttribOIKey)
        oAttribute.SetAttribute(ACXSDAttribUse, ACXSDAttribUseRequired)
        oAttribute.SetAttribute(ACXSDAttribType, ACXSDAttribTypeString)
        oAttribute.SetAttribute(ACXSDAttribFixed, ACXSDDeletedObjects)

        oCOComplexType.AppendChild(oAttribute)
        oAttribute = Nothing

        ' Set the Root Level Document Element

        oDSSequence.AppendChild(oChildObject)

        ' Create the Quotes_Object Schema Element
        oChildObject = m_oDataSetXSD.CreateElement(ACXSDElement, namespaceUri)
        ' Set the names Attribute
        oChildObject.SetAttribute(ACXSDAttribName, ACXSDQuotes)
        ' Create the Dataset Schema Element
        oCOComplexType = m_oDataSetXSD.CreateElement(ACXSDComplexType, namespaceUri)

        oChildObject.AppendChild(oCOComplexType)
        oCOSequence = m_oDataSetXSD.CreateElement(ACXSDSequence, namespaceUri)
        oAny = m_oDataSetXSD.CreateElement(ACXSDAny, namespaceUri)
        oAny.SetAttribute(ACXSDAttribProcessContents, ACXSDAttribProcessContentsLax)
        oAny.SetAttribute(ACXSDAttribMinOccurs, "0")
        oAny.SetAttribute(ACXSDAttribMaxOccurs, ACXSDAttribMaxOccursUnbounded)


        oCOSequence.AppendChild(oAny)

        oCOComplexType.AppendChild(oCOSequence)

        ' Create an Attribute element
        oAttribute = m_oDataSetXSD.CreateElement(ACXSDAttribute, namespaceUri)
        oAttribute.SetAttribute(ACXSDAttribName, ACXSDAttribOIKey)
        oAttribute.SetAttribute(ACXSDAttribUse, ACXSDAttribUseRequired)
        oAttribute.SetAttribute(ACXSDAttribType, ACXSDAttribTypeString)
        oAttribute.SetAttribute(ACXSDAttribFixed, ACXSDQuotes)

        oCOComplexType.AppendChild(oAttribute)
        oAttribute = Nothing

        ' Create an Attribute element
        oAttribute = m_oDataSetXSD.CreateElement(ACXSDAttribute, namespaceUri)
        oAttribute.SetAttribute(ACXSDAttribName, ACXSDAttribNextQuoteNum)
        oAttribute.SetAttribute(ACXSDAttribUse, ACXSDAttribUseRequired)
        oAttribute.SetAttribute(ACXSDAttribType, ACXSDAttribTypeString)

        oCOComplexType.AppendChild(oAttribute)
        oAttribute = Nothing

        ' Set the Root Level Document Element

        oDSSequence.AppendChild(oChildObject)


        oElement.AppendChild(oDataset)
        ' Set the Root Level Document Element
        If Not (m_oDataSetXSD.DocumentElement Is Nothing) Then
            m_oDataSetXSD.RemoveChild(m_oDataSetXSD.DocumentElement)
        End If
        m_oDataSetXSD.AppendChild(oElement)


        Return result

    End Function


    ' ***************************************************************** '
    ' Name: LoadObjectXSDDefs
    '
    ' Description: Load the Data Set Structures using
    '              the Object/Property Arrays supplied.
    '
    ' ***************************************************************** '
    Private Function LoadObjectXSDDefs(ByRef v_vObjectArray(,) As Object, ByRef v_vPropertyArray(,) As Object, Optional ByVal v_blDataTransferXSD As Boolean = False) As Integer

        Dim result As Integer = 0

        Dim lGISObjectID, lGISModelID As Integer
        Dim sObjectName, sTableName As String
        Dim lMaxInstances, lIsQuoteObject As Integer
        Dim sParentObjectName As String = ""
        Dim lPolarisObjectID, lIsSelectableForScreen, lIsNonGIS, lEditFlags As Integer

        Dim lReturn As gPMConstants.PMEReturnCode



        result = gPMConstants.PMEReturnCode.PMTrue

        ' For Each Object
        For lRow As Integer = v_vObjectArray.GetLowerBound(1) To v_vObjectArray.GetUpperBound(1)

            ' Get the Object attributes from the Array

            lGISObjectID = CInt(v_vObjectArray(iGISSharedConstants.GISObjColObjectId, lRow))

            lGISModelID = CInt(v_vObjectArray(iGISSharedConstants.GISObjColModelId, lRow))

            sObjectName = CStr(v_vObjectArray(iGISSharedConstants.GISObjColObjectName, lRow)).Trim().ToUpper()

            sTableName = CStr(v_vObjectArray(iGISSharedConstants.GISObjColTableName, lRow)).Trim()

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(v_vObjectArray(iGISSharedConstants.GISObjColMaxInstances, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                lMaxInstances = -1
            Else

                lMaxInstances = CInt(v_vObjectArray(iGISSharedConstants.GISObjColMaxInstances, lRow))
            End If

            lIsQuoteObject = CInt(v_vObjectArray(iGISSharedConstants.GISObjColIsQuoteObject, lRow))

            sParentObjectName = CStr(v_vObjectArray(iGISSharedConstants.GISObjColParentObjectName, lRow)).Trim().ToUpper()


            Dim dbNumericTemp2 As Double
            If Not Double.TryParse(CStr(v_vObjectArray(iGISSharedConstants.GISObjColPolarisObjId, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                lPolarisObjectID = -1
            Else

                lPolarisObjectID = CInt(v_vObjectArray(iGISSharedConstants.GISObjColPolarisObjId, lRow))
            End If


            Dim dbNumericTemp3 As Double
            If Not Double.TryParse(CStr(v_vObjectArray(iGISSharedConstants.GISObjColIsSelectScreen, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                lIsSelectableForScreen = -1
            Else

                lIsSelectableForScreen = CInt(v_vObjectArray(iGISSharedConstants.GISObjColIsSelectScreen, lRow))
            End If


            Dim dbNumericTemp4 As Double
            If Not Double.TryParse(CStr(v_vObjectArray(iGISSharedConstants.GISObjColIsNonGIS, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                lIsNonGIS = -1
            Else

                lIsNonGIS = CInt(v_vObjectArray(iGISSharedConstants.GISObjColIsNonGIS, lRow))
            End If


            Dim dbNumericTemp5 As Double
            If Not Double.TryParse(CStr(v_vObjectArray(iGISSharedConstants.GISObjColEditFlags, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
                lEditFlags = -1
            Else

                lEditFlags = CInt(v_vObjectArray(iGISSharedConstants.GISObjColEditFlags, lRow))
            End If

            ' Add it to the Object Definitions
            lReturn = CType(AddObjectXSDDefinition(v_lIsQuoteObject:=lIsQuoteObject, v_lGISObjectID:=lGISObjectID, v_sObjectName:=sObjectName, v_sTableName:=sTableName, v_lMaxInstances:=lMaxInstances, v_lPolarisObjectID:=lPolarisObjectID, v_sParentObjectName:=sParentObjectName, v_lIsSelectableForScreen:=lIsSelectableForScreen, v_lIsNonGIS:=lIsNonGIS, v_lEditFlags:=lEditFlags, v_blDataTransferXSD:=v_blDataTransferXSD), gPMConstants.PMEReturnCode)

            ' Check that the Object Definition was Added.
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Next lRow

        ' Load the Properties

        lReturn = CType(LoadPropertyXSDDefs(v_vPropertyArray:=v_vPropertyArray, v_blDataTransferXSD:=v_blDataTransferXSD), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: LoadPropertyXSDDefs
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function LoadPropertyXSDDefs(ByRef v_vPropertyArray(,) As Object, Optional ByVal v_blDataTransferXSD As Boolean = False) As Integer


        Dim result As Integer = 0



        Dim lGISObjectID As Integer
        Dim sObjectName As String = ""
        Dim lGISPropertyID As Integer
        Dim sPropertyName, sColumnName As String
        Dim lDataType As Integer
        Dim iIsIdentifyingProperty, iIsPrimaryKey As Integer
        Dim lGISListID, lPolarisPropertyID, lSpecialsType, lSpecialsTypeReference, lEditFlags As Integer

        Dim lReturn As gPMConstants.PMEReturnCode

        result = gPMConstants.PMEReturnCode.PMTrue

        For lRow As Integer = v_vPropertyArray.GetLowerBound(1) To v_vPropertyArray.GetUpperBound(1)

            ' Get the Property Values

            lGISObjectID = CInt(v_vPropertyArray(iGISSharedConstants.GISPropColObjectId, lRow))

            sObjectName = CStr(v_vPropertyArray(iGISSharedConstants.GISPropColObjectName, lRow)).ToUpper()

            lGISPropertyID = CInt(v_vPropertyArray(iGISSharedConstants.GISPropColPropertyId, lRow))

            sPropertyName = CStr(v_vPropertyArray(iGISSharedConstants.GISPropColPropertyName, lRow)).ToUpper()

            sColumnName = CStr(v_vPropertyArray(iGISSharedConstants.GISPropColColumnName, lRow))

            lDataType = CInt(v_vPropertyArray(iGISSharedConstants.GISPropColDataType, lRow))

            iIsIdentifyingProperty = CInt(v_vPropertyArray(iGISSharedConstants.GISPropColIsIdentifyingProperty, lRow))

            iIsPrimaryKey = CInt(v_vPropertyArray(iGISSharedConstants.GISPropColIsPrimaryKey, lRow))


            Dim dbNumericTemp As Double
            If Double.TryParse(CStr(v_vPropertyArray(iGISSharedConstants.GISPropColListId, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                lGISListID = CInt(v_vPropertyArray(iGISSharedConstants.GISPropColListId, lRow))
            Else
                lGISListID = -1
            End If


            Dim dbNumericTemp2 As Double
            If Double.TryParse(CStr(v_vPropertyArray(iGISSharedConstants.GISPropColPolarisPropId, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                lPolarisPropertyID = CInt(v_vPropertyArray(iGISSharedConstants.GISPropColPolarisPropId, lRow))
            Else
                lPolarisPropertyID = -1
            End If


            Dim dbNumericTemp3 As Double
            If Double.TryParse(CStr(v_vPropertyArray(iGISSharedConstants.GISPropColSpecialsType, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then

                lSpecialsType = CInt(v_vPropertyArray(iGISSharedConstants.GISPropColSpecialsType, lRow))
            Else
                lSpecialsType = -1
            End If


            Dim dbNumericTemp4 As Double
            If Double.TryParse(CStr(v_vPropertyArray(iGISSharedConstants.GISPropColSpecialsTypeReference, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then

                lSpecialsTypeReference = CInt(v_vPropertyArray(iGISSharedConstants.GISPropColSpecialsTypeReference, lRow))
            Else
                lSpecialsTypeReference = -1
            End If


            Dim dbNumericTemp5 As Double
            If Double.TryParse(CStr(v_vPropertyArray(iGISSharedConstants.GISPropColEditFlags, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then

                lEditFlags = CInt(v_vPropertyArray(iGISSharedConstants.GISPropColEditFlags, lRow))
            Else
                lEditFlags = -1
            End If


            ' Add the Property Def
            lReturn = CType(AddPropertyXSDDefinition(v_lGISPropertyID:=lGISPropertyID, v_lGISObjectID:=lGISObjectID, v_sObjectName:=sObjectName, v_sPropertyName:=sPropertyName, v_sColumnName:=sColumnName, v_lDataType:=lDataType, v_iIsPrimaryKey:=iIsPrimaryKey, v_iIsIdentifyingProperty:=iIsIdentifyingProperty, v_lGISListID:=lGISListID, v_lPolarisPropertyID:=lPolarisPropertyID, v_lSpecialsType:=lSpecialsType, v_lSpecialsTypeReference:=lSpecialsTypeReference, v_lEditFlags:=lEditFlags, v_blDataTransferXSD:=v_blDataTransferXSD), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Next lRow

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddObjectXSDDefinition
    '
    ' Description: Adds an Object Definition in the XML Schema.
    '
    ' ***************************************************************** '
    Private Function AddObjectXSDDefinition(ByRef v_lIsQuoteObject As Integer, ByRef v_lGISObjectID As Integer, ByRef v_sObjectName As String, ByRef v_sTableName As String, ByRef v_lMaxInstances As Integer, ByRef v_lPolarisObjectID As Integer, ByRef v_sParentObjectName As String, ByRef v_lIsSelectableForScreen As Integer, ByRef v_lIsNonGIS As Integer, ByRef v_lEditFlags As Integer, Optional ByVal v_blDataTransferXSD As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim oParent As XmlNode
        Dim oSpecial As XmlElement
        Dim sParentObjectName As String = ""
        Dim oNewObject, oComplexType, oSequence, oUpdateStatus, oOIKey As XmlElement




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Is this a Top Level Object
        ' i.e. Does it have a Parent
        If CBool(CStr(v_sParentObjectName = "").Trim()) Then
            ' NO Parent, so this is a Top Level Object

            ' Is this an Quote Object
            If v_lIsQuoteObject = gPMConstants.PMEReturnCode.PMTrue Then
                sParentObjectName = ACXMLQuotes
            Else
                sParentObjectName = ACXMLRiskObjects
            End If

        Else
            ' Not Top Level Object, so use the Parent Object Name
            sParentObjectName = v_sParentObjectName.Trim()
        End If

        ' Get the Parent Object
        oParent = GetXSDDefinitionNode(sParentObjectName)
        If oParent Is Nothing Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '<xs:element name="ADDCONTENTS">
        Dim namespaceUri As String = "http://www.w3.org/2001/XMLSchema"
        ' Create the new Object
        ' Note, Uppercase is used as XML Tags are Case sensitive
        oNewObject = m_oDataSetXSD.CreateElement(ACXSDElement, namespaceUri)

        ' Set the Object Attributes
        oNewObject.SetAttribute(ACXSDAttribName, v_sObjectName)
        If Not v_sObjectName.EndsWith("_POLICY_BINDER") Then
            oNewObject.SetAttribute(ACXSDAttribMinOccurs, "0")
            oNewObject.SetAttribute(ACXSDAttribMaxOccurs, If(v_lMaxInstances > 0, v_lMaxInstances, ACXSDAttribMaxOccursUnbounded))
        End If
        '    oNewObject.setAttribute ACXMLAttribObjectID, v_lGISObjectID
        '    oNewObject.setAttribute ACXMLAttribObjectName, (UCase(Trim$(v_sObjectName)))
        '    oNewObject.setAttribute ACXMLAttribTableName, Trim$(v_sTableName)
        '    oNewObject.setAttribute ACXMLAttribMaxInstances, v_lMaxInstances
        '    oNewObject.setAttribute ACXMLAttribPolarisObjectID, v_lPolarisObjectID
        '    oNewObject.setAttribute ACXMLAttribNextOINumber, 1
        '    ' RFC300103
        '    oNewObject.setAttribute ACXMLAttribIsSelForScreen, v_lIsSelectableForScreen
        '    oNewObject.setAttribute ACXMLAttribIsNonGIS, v_lIsNonGIS
        '    oNewObject.setAttribute ACXMLAttribEditFlags, v_lEditFlags

        oComplexType = m_oDataSetXSD.CreateElement(ACXSDComplexType, namespaceUri)
        oSequence = m_oDataSetXSD.CreateElement(ACXSDSequence, namespaceUri)

        oComplexType.AppendChild(oSequence)

        oUpdateStatus = m_oDataSetXSD.CreateElement(ACXSDAttribute, namespaceUri)
        oUpdateStatus.SetAttribute(ACXSDAttribName, ACXSDAttribUpdateStatus)
        oUpdateStatus.SetAttribute(ACXSDAttribUse, ACXSDAttribUseRequired)
        oUpdateStatus.SetAttribute(ACXSDAttribType, ACXSDAttribTypeShort)
        If (v_blDataTransferXSD) And (Not v_sObjectName.EndsWith("_POLICY_BINDER")) Then
            oUpdateStatus.SetAttribute(ACXSDAttribFixed, 1)
        End If

        oOIKey = m_oDataSetXSD.CreateElement(ACXSDAttribute, namespaceUri)
        oOIKey.SetAttribute(ACXSDAttribName, ACXSDAttribOIKey)
        oOIKey.SetAttribute(ACXSDAttribUse, ACXSDAttribUseRequired)
        oOIKey.SetAttribute(ACXSDAttribType, ACXSDAttribTypeString)


        oComplexType.AppendChild(oUpdateStatus)

        oComplexType.AppendChild(oOIKey)

        oNewObject.AppendChild(oComplexType)

        ' Add the new Object to the Parent

        oParent.AppendChild(oNewObject)

        ' RFC300103 - Associated Client and Disclosures
        ' If we are adding an Associated Client or Peril object then set a Property
        ' on the DataSet definition so that bGIS will no that it has to load/save to/from
        ' those tables in addition to the normal
        Select Case v_lIsNonGIS
            Case GISDataModelType.GISOTAssociatedClient ', GISOTClaim

                oSpecial = GetXSDDefinitionNode(ACXMLSpecialObjects)
                If oSpecial Is Nothing Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                oSpecial.SetAttribute(ACXMLAttribObjectName, v_sObjectName.Trim().ToUpper())
                oSpecial.SetAttribute(ACXMLAttribIsNonGIS, v_lIsNonGIS)

            Case Else
        End Select

        ' Release the references
        oParent = Nothing
        oNewObject = Nothing
        oSpecial = Nothing
        oComplexType = Nothing
        oSequence = Nothing
        oUpdateStatus = Nothing
        oOIKey = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddPropertyXSDDefinition
    '
    ' Description: Adds a Property Definition in the XML Schema.
    '
    ' ***************************************************************** '
    Private Function AddPropertyXSDDefinition(ByRef v_lGISPropertyID As Integer, ByRef v_lGISObjectID As Integer, ByRef v_sObjectName As String, ByRef v_sPropertyName As String, ByRef v_sColumnName As String, ByRef v_lDataType As Integer, ByRef v_iIsPrimaryKey As Integer, ByRef v_iIsIdentifyingProperty As Integer, ByRef v_lGISListID As Integer, ByRef v_lPolarisPropertyID As Integer, ByVal v_lSpecialsType As Integer, ByVal v_lSpecialsTypeReference As Integer, ByVal v_lEditFlags As Integer, Optional ByVal v_blDataTransferXSD As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim oParent As XmlNode
        Dim sParentObjectName As String = ""
        Dim oNewProp As XmlElement
        Dim sPropertyTag As String = ""
        Dim oAddress, oComplexType, oSequence, oAttribute, oSumInsured, oSumInsuredChild, oSIComplexType, oSISequence, oStdWording, oSWComplexType, oSWSequence As XmlElement
        Dim sDatatype As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the Parent Object Definition
        oParent = GetXSDDefinitionNode(v_sObjectName)
        If oParent Is Nothing Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Dim namespaceUri As String = "http://www.w3.org/2001/XMLSchema"
        ' Create the new Object
        'Create an Address Structure
        If v_sPropertyName.IndexOf("ADDRESS_CNT") >= 0 Then

            oAddress = m_oDataSetXSD.CreateElement(ACXSDElement, namespaceUri)
            oAddress.SetAttribute(ACXSDAttribName, v_sPropertyName)
            oAddress.SetAttribute(ACXSDAttribMinOccurs, "0")
            oAddress.SetAttribute(ACXSDAttribMaxOccurs, ACXSDAttribMaxOccursUnbounded)

            oComplexType = m_oDataSetXSD.CreateElement(ACXSDComplexType, namespaceUri)
            oSequence = m_oDataSetXSD.CreateElement(ACXSDSequence, namespaceUri)

            oComplexType.AppendChild(oSequence)

            oAttribute = m_oDataSetXSD.CreateElement(ACXSDAttribute, namespaceUri)
            oAttribute.SetAttribute(ACXSDAttribName, ACXSDAddressLine1)
            oAttribute.SetAttribute(ACXSDAttribType, ACXSDAttribTypeString)

            oComplexType.AppendChild(oAttribute)

            oAttribute = m_oDataSetXSD.CreateElement(ACXSDAttribute, namespaceUri)
            oAttribute.SetAttribute(ACXSDAttribName, ACXSDAddressLine2)
            oAttribute.SetAttribute(ACXSDAttribType, ACXSDAttribTypeString)

            oComplexType.AppendChild(oAttribute)

            oAttribute = m_oDataSetXSD.CreateElement(ACXSDAttribute, namespaceUri)
            oAttribute.SetAttribute(ACXSDAttribName, ACXSDAddressLine3)
            oAttribute.SetAttribute(ACXSDAttribType, ACXSDAttribTypeString)

            oComplexType.AppendChild(oAttribute)

            oAttribute = m_oDataSetXSD.CreateElement(ACXSDAttribute, namespaceUri)
            oAttribute.SetAttribute(ACXSDAttribName, ACXSDAddressLine4)
            oAttribute.SetAttribute(ACXSDAttribType, ACXSDAttribTypeString)

            oComplexType.AppendChild(oAttribute)

            oAttribute = m_oDataSetXSD.CreateElement(ACXSDAttribute, namespaceUri)
            oAttribute.SetAttribute(ACXSDAttribName, ACXSDPostCode)
            oAttribute.SetAttribute(ACXSDAttribType, ACXSDAttribTypeString)

            oComplexType.AppendChild(oAttribute)

            oAttribute = m_oDataSetXSD.CreateElement(ACXSDAttribute, namespaceUri)
            oAttribute.SetAttribute(ACXSDAttribName, ACXSDCountryCode)
            oAttribute.SetAttribute(ACXSDAttribType, ACXSDAttribTypeString)

            oComplexType.AppendChild(oAttribute)


            oAddress.AppendChild(oComplexType)

            oParent.AppendChild(oAddress)

        End If

        'Create a Sums Insured structure
        If v_lSpecialsType = iGISSharedConstants.ACSpecialsTypeSumsInsured Then

            ' Create the Element for the Sums Insured Property
            oSumInsured = m_oDataSetXSD.CreateElement(ACXSDElement, namespaceUri)
            oSumInsured.SetAttribute(ACXSDAttribName, v_sPropertyName)
            oSumInsured.SetAttribute(ACXSDAttribMinOccurs, "0")
            oSumInsured.SetAttribute(ACXSDAttribMaxOccurs, ACXSDAttribMaxOccursUnbounded)

            oSIComplexType = m_oDataSetXSD.CreateElement(ACXSDComplexType, namespaceUri)
            oSISequence = m_oDataSetXSD.CreateElement(ACXSDSequence, namespaceUri)

            'Add Sums Insured detail structure
            oSumInsuredChild = m_oDataSetXSD.CreateElement(ACXSDElement, namespaceUri)
            oSumInsuredChild.SetAttribute(ACXSDAttribName, ACXSDSumInsured)
            oSumInsuredChild.SetAttribute(ACXSDAttribMinOccurs, "0")
            oSumInsuredChild.SetAttribute(ACXSDAttribMaxOccurs, ACXSDAttribMaxOccursUnbounded)

            oComplexType = m_oDataSetXSD.CreateElement(ACXSDComplexType, namespaceUri)
            oSequence = m_oDataSetXSD.CreateElement(ACXSDSequence, namespaceUri)

            oComplexType.AppendChild(oSequence)

            oAttribute = m_oDataSetXSD.CreateElement(ACXSDAttribute, namespaceUri)
            oAttribute.SetAttribute(ACXSDAttribName, ACXSDSumInsuredSumInsuredType)
            oAttribute.SetAttribute(ACXSDAttribType, ACXSDAttribTypeString)

            oComplexType.AppendChild(oAttribute)

            oAttribute = m_oDataSetXSD.CreateElement(ACXSDAttribute, namespaceUri)
            oAttribute.SetAttribute(ACXSDAttribName, ACXSDSumInsuredDescription)
            oAttribute.SetAttribute(ACXSDAttribType, ACXSDAttribTypeString)

            oComplexType.AppendChild(oAttribute)

            oAttribute = m_oDataSetXSD.CreateElement(ACXSDAttribute, namespaceUri)
            oAttribute.SetAttribute(ACXSDAttribName, ACXSDSumInsuredReference)
            oAttribute.SetAttribute(ACXSDAttribType, ACXSDAttribTypeString)

            oComplexType.AppendChild(oAttribute)

            oAttribute = m_oDataSetXSD.CreateElement(ACXSDAttribute, namespaceUri)
            oAttribute.SetAttribute(ACXSDAttribName, ACXSDSumInsuredSumInsured)
            oAttribute.SetAttribute(ACXSDAttribType, ACXSDAttribTypeDecimal)

            oComplexType.AppendChild(oAttribute)

            oAttribute = m_oDataSetXSD.CreateElement(ACXSDAttribute, namespaceUri)
            oAttribute.SetAttribute(ACXSDAttribName, ACXSDSumInsuredDateAdded)
            oAttribute.SetAttribute(ACXSDAttribType, ACXSDAttribTypeDateTime)

            oComplexType.AppendChild(oAttribute)

            oAttribute = m_oDataSetXSD.CreateElement(ACXSDAttribute, namespaceUri)
            oAttribute.SetAttribute(ACXSDAttribName, ACXSDSumInsuredDateDeleted)
            oAttribute.SetAttribute(ACXSDAttribType, ACXSDAttribTypeDateTime)

            oComplexType.AppendChild(oAttribute)

            oAttribute = m_oDataSetXSD.CreateElement(ACXSDAttribute, namespaceUri)
            oAttribute.SetAttribute(ACXSDAttribName, ACXSDSumInsuredIsValuationReqd)
            oAttribute.SetAttribute(ACXSDAttribType, ACXSDAttribTypeString)

            oComplexType.AppendChild(oAttribute)

            oAttribute = m_oDataSetXSD.CreateElement(ACXSDAttribute, namespaceUri)
            oAttribute.SetAttribute(ACXSDAttribName, ACXSDSumInsuredValuationDate)
            oAttribute.SetAttribute(ACXSDAttribType, ACXSDAttribTypeDateTime)

            oComplexType.AppendChild(oAttribute)


            oSumInsuredChild.AppendChild(oComplexType)

            oSISequence.AppendChild(oSumInsuredChild)

            oSIComplexType.AppendChild(oSISequence)

            'Add the Attributes for the parent
            oAttribute = m_oDataSetXSD.CreateElement(ACXSDAttribute, namespaceUri)
            oAttribute.SetAttribute(ACXSDAttribName, ACXSDAttribUpdateStatus)
            oAttribute.SetAttribute(ACXSDAttribType, ACXSDAttribTypeString)

            oSIComplexType.AppendChild(oAttribute)

            oAttribute = m_oDataSetXSD.CreateElement(ACXSDAttribute, namespaceUri)
            oAttribute.SetAttribute(ACXSDAttribName, ACXSDSumInsuredRate)
            oAttribute.SetAttribute(ACXSDAttribType, ACXSDAttribTypeDecimal)

            oSIComplexType.AppendChild(oAttribute)

            oAttribute = m_oDataSetXSD.CreateElement(ACXSDAttribute, namespaceUri)
            oAttribute.SetAttribute(ACXSDAttribName, ACXSDSumInsuredPremium)
            oAttribute.SetAttribute(ACXSDAttribType, ACXSDAttribTypeDecimal)

            oSIComplexType.AppendChild(oAttribute)


            oSumInsured.AppendChild(oSIComplexType)

            oParent.AppendChild(oSumInsured)

        End If

        'Create a Sums Insured structure
        If v_lSpecialsType = iGISSharedConstants.ACSpecialsTypeStandardWordings Then

            ' Create the Element for the Sums Insured Property
            oStdWording = m_oDataSetXSD.CreateElement(ACXSDElement, namespaceUri)
            oStdWording.SetAttribute(ACXSDAttribName, v_sPropertyName)
            oStdWording.SetAttribute(ACXSDAttribMinOccurs, "0")
            oStdWording.SetAttribute(ACXSDAttribMaxOccurs, ACXSDAttribMaxOccursUnbounded)

            oSWComplexType = m_oDataSetXSD.CreateElement(ACXSDComplexType, namespaceUri)
            oSWSequence = m_oDataSetXSD.CreateElement(ACXSDSequence, namespaceUri)

            oSWComplexType.AppendChild(oSWSequence)

            'Add Standard Wordings detail structure
            oAttribute = m_oDataSetXSD.CreateElement(ACXSDAttribute, namespaceUri)
            oAttribute.SetAttribute(ACXSDAttribName, ACXSDStdWordingCode)
            oAttribute.SetAttribute(ACXSDAttribType, ACXSDAttribTypeString)

            oSWComplexType.AppendChild(oAttribute)


            oStdWording.AppendChild(oSWComplexType)

            oParent.AppendChild(oStdWording)

        End If

        oParent = oParent.ParentNode

        ' Create the new Property with Tag Name 'OBJECTNAME.PROPERTY'
        ' Note Upercase is Used as XML Tag Names are Case Sensitive
        sPropertyTag = v_sObjectName.Trim().ToUpper() & "." & v_sPropertyName.Trim().ToUpper()

        oNewProp = m_oDataSetXSD.CreateElement(ACXSDAttribute, namespaceUri)

        oNewProp.SetAttribute(ACXSDAttribName, v_sPropertyName)
        Select Case v_lDataType
            Case iGISSharedConstants.GISDataTypeText, iGISSharedConstants.GISDataTypeShortListCode, iGISSharedConstants.GISDataTypeLongListCode, iGISSharedConstants.GISDataTypeComment
                sDatatype = ACXSDAttribTypeString
            Case iGISSharedConstants.GISDataTypeDate
                sDatatype = ACXSDAttribTypeDateTime
            Case iGISSharedConstants.GISDataTypeNumeric, iGISSharedConstants.GISDataTypeInteger, iGISSharedConstants.GISDataTypeShortList, iGISSharedConstants.GISDataTypeLongList, iGISSharedConstants.GISDataTypeOption
                sDatatype = ACXSDAttribTypeLong
            Case iGISSharedConstants.GISDataTypeCurrency, iGISSharedConstants.GISDataTypePercentage
                sDatatype = ACXSDAttribTypeDecimal
            Case Else
                sDatatype = ACXSDAttribTypeString
        End Select

        oNewProp.SetAttribute(ACXSDAttribType, sDatatype)

        If (v_iIsPrimaryKey = 1) Or (v_sPropertyName = "GIS_POLICY_LINK_ID") Then
            oNewProp.SetAttribute(ACXSDAttribUse, ACXSDAttribUseRequired)
        End If

        If (v_blDataTransferXSD) And ((Not v_sObjectName.EndsWith("_POLICY_BINDER") And v_sPropertyName.EndsWith("_POLICY_BINDER_ID")) Or ((v_sObjectName = "WORK_CLAIM" Or v_sObjectName = "WORK_CLAIM_PERIL") And v_lEditFlags = 6) Or ((v_sObjectName = "WORK_CLAIM_PERIL") And (v_sPropertyName = "RESERVE_DETAILS" Or v_sPropertyName = "PAYMENT_DETAILS"))) Then
            oNewProp.SetAttribute(ACXSDAttribUse, ACXSDAttribUseProhibited)
        End If

        ' Add the new Object to the Parent

        oParent.AppendChild(oNewProp)

        ' Release the references
        oParent = Nothing
        oNewProp = Nothing

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: GetXSDDefinitionNode
    '
    ' Description: Returns the Obect/Property Definition Node for the
    '              requested Object or Property.
    '
    ' ***************************************************************** '
    Private Function GetXSDDefinitionNode(ByRef v_sObjectName As String, Optional ByRef v_sPropertyName As String = "") As XmlNode

        Dim result As XmlNode = Nothing
        Dim oNodes As XmlNodeList
        Dim oNode As XmlNode
        Dim sNodeKey As String = ""




        ' Build Up the Key

        ' Upercase the Object Name
        sNodeKey = v_sObjectName.Trim().ToUpper()

        ' If we have been supplied a Property Name then Add
        ' a full stop followed by the Property Name
        If v_sPropertyName.Trim() <> "" Then
            sNodeKey = sNodeKey & "." & v_sPropertyName.Trim().ToUpper()
        End If

        Dim manager As XmlNamespaceManager = New XmlNamespaceManager(m_oDataSetXSD.NameTable)
        manager.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema")
        ' Find elements with the Tag Name of the Node Key
        ' This returns a List of Nodes, however there should only be one
        oNodes = m_oDataSetXSD.SelectNodes("//xs:sequence[../../@name='" & v_sObjectName & "']", manager)

        If oNodes Is Nothing Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to find Definiton for :-" & sNodeKey, vApp:=ACApp, vClass:=ACClass, vMethod:="GetXSDDefinitionNode")
            Return result
        End If

        ' Get the Node from the List
        ' Remember there should only be one
        oNode = oNodes.Item(0)
        If oNode Is Nothing Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to find Definition for :-" & sNodeKey, vApp:=ACApp, vClass:=ACClass, vMethod:="GetXSDDefinitionNode")
            Return result
        End If

        ' All OK so return the Object/Property Definition Node
        result = oNode

        ' Release Local References
        oNodes = Nothing
        oNode = Nothing

        Return result

    End Function

    Public Function GetFormattedString(ByRef r_sXML As String) As Integer
        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue

        If r_sXML Is Nothing Then
            r_sXML = ""
        End If
        If (r_sXML).Length > 0 Then
            'r_sXML = Replace(r_sXML, "?", Chr(194) & "?", , , vbTextCompare)
            'r_sXML = Replace(r_sXML, ChrW(145), ChrW(39), , , vbTextCompare)
            r_sXML = r_sXML.Replace(ChrW(145), ChrW(39))

            r_sXML = Replace(r_sXML, ChrW(146), ChrW(39), , , vbTextCompare)
            r_sXML = Replace(r_sXML, ChrW(147), "&#8220;", , , vbTextCompare)
            r_sXML = Replace(r_sXML, ChrW(148), "&#8221;", , , vbTextCompare)
            r_sXML = Replace(r_sXML, ChrW(150), ChrW(45), , , vbTextCompare)
            r_sXML = Replace(r_sXML, ChrW(151), ChrW(45), , , vbTextCompare)
            r_sXML = Replace(r_sXML, ChrW(160), "", , , vbTextCompare)
            r_sXML = Replace(r_sXML, "&", "&amp;", , , vbTextCompare)
            r_sXML = Replace(r_sXML, "&amp;lt;", "&lt;", , , vbTextCompare)
            r_sXML = Replace(r_sXML, "&amp;gt;", "&gt;", , , vbTextCompare)
            r_sXML = Replace(r_sXML, "&amp;amp;", "&amp;", , , vbTextCompare)
            'r_sXML = Replace(r_sXML, ChrW(-4056), "(", , , vbTextCompare)
            'r_sXML = Replace(r_sXML, ChrW(-3913), "(", , , vbTextCompare)
            r_sXML = Replace(r_sXML, ChrW(133), "...", , , vbTextCompare)
            r_sXML = Replace(r_sXML, ChrW(183), "", , , vbTextCompare)
            ' r_sXML = Replace(r_sXML, Chr(148), Chr(34))
            'r_sXML = Replace(r_sXML, Chr(147), Chr(34))
            r_sXML = Replace(r_sXML, "???", ChrW(147))
            r_sXML = Replace(r_sXML, "??", ChrW(148))
            r_sXML = Replace(r_sXML, ChrW(128), "&#8364;")



        End If
        Return result

    End Function
End Class


