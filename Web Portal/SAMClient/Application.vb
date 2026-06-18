
Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.IO
Imports System.Text
Imports System.Xml
Imports System.Xml.Schema
Imports System.Xml.XPath

Namespace DataSetControl

    <System.Runtime.InteropServices.ProgId("Application_NET.Application")> _
    Public Class Application
        ' ***************************************************************** '
        ' Class Name: Application
        ' Date: 23/07/2002
        ' Description: Main Class which Controls the Creating/Setting of
        '              the Data Sets held in Memory.
        ' Edit History:
        ' RFC230702 - New version based on original DataSetControl but using
        '             a new format for the XML to improve performance.
        ' JRD09032005 PN18822 - Use QuoteKey as part of OIKey for QuoteObjects
        ' JRD01102005 PN24533 - Correctly process single double-quote characters in XML
        ' ***************************************************************** '


        ' ************************************************
        ' Added to replace global variables 19/09/2003
        Private m_sUsername As String = ""
        Private m_sPassword As String = ""
        Private m_iUserID As Integer
        Private m_sCallingAppName As String = ""
        Private m_iSourceID As Integer
        Private m_iLanguageID As Integer
        Private m_iCurrencyID As Integer
        Private m_iLogLevel As Integer
        ' ************************************************

        Private Const ACClass As String = "Application"
        Private m_oDataSetDef As XmlDocument
        Private m_oDataset As XmlDocument
        Private m_oDataSetXSD As XmlDocument
        Dim m_oDefaults As XmlDocument

        ' Temporary Arrays which are used to load up the Object/Property Definitions
        Private m_vTempObjectArray(,) As Object
        Private m_vTempPropertyArray(,) As Object

        ' Temporary String used to build up the Data Set DTD
        Private m_sTempObjectDTD As String = ""
        Private m_sTempAttribDTD As String = ""
        ' Hold the Object Instance Number outside of the XML
        Private m_lNextOINumber As Integer
        Private m_bStrict As Boolean


        Friend Property Strict() As Boolean
            Get
                Return m_bStrict
            End Get
            Set(ByVal Value As Boolean)
                m_bStrict = Value
            End Set
        End Property

        Public ReadOnly Property GISDataModelCode() As String
            Get
                If m_oDataSetDef Is Nothing Then
                    Return ""
                Else

                    Return CStr(m_oDataSetDef.DocumentElement.GetAttribute(GISSharedConstants.GISXMLAttribDataModelCode))
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
                        Return CStr(m_oDataSetDef.DocumentElement.GetAttribute(GISSharedConstants.GISXMLAttribDataSetDefTimestamp))
                    End If
                Catch excep As System.Exception

                End Try

                Return ""

            End Get
        End Property

        ' RFC090300 - Scripting Method Access Added
        Public ReadOnly Property Risk() As DataSetControl.Node
            Get

                Try
                    ' Set the Root to the RISK_OBJECTS
                    ' Return the Node so that an Item call can be done from there
                    Return SetRoot()
                Catch excep As System.Exception
                    Throw
                    Exit Property
                End Try
            End Get
        End Property

        Public ReadOnly Property Quote(ByVal v_lQuoteNum As Integer) As DataSetControl.Node
            Get
                Try
                    ' Set the Root to the Quote Number Specified
                    ' Return the Node so that an Item call can be done from there
                    Return SetRoot(v_lQuoteNum)
                Catch excep As System.Exception
                    Throw
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
                    Throw
                    Exit Property
                End Try
            End Get
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

        Friend ReadOnly Property DatasetXMLDoc() As XmlDocument
            Get
                Return m_oDataset
            End Get
        End Property

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
                m_oDataSetDef.PreserveWhitespace = False
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
                    MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogDebug1, sMsg:="Unable to Load Data Set Definition from File : " & v_sDataSetDefFile, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXMLFile")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'developer guide no. 38 (no solution)
                'm_oDataset.validateOnParse = False
                m_oDataset.PreserveWhitespace = False
                ' There IS an DTD for the Data Set, but it is Internal
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
                    MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:="Unable to Load Data Set from File : " & v_sDataSetFile, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXMLFile")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Set the Next Object Instance Number

                NextOINumber = CInt(m_oDataset.DocumentElement.GetAttribute(ACXMLAttribNextOINumber))

                Return result

            Catch excep As System.Exception

                result = gPMConstants.PMEReturnCode.PMError
                ' Log Error Message
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadFromXMLFileFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXMLFile", excep:=excep)

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
            Dim sDataSetDefFile, sDataSetFile As String

            Try

                result = gPMConstants.PMEReturnCode.PMTrue

                ' Get the Path/FileNames for stored EMPTY Data Set Files
                ' of this Data Model Type
                lReturn = CType(GISSharedConstants.GetDataSetFileNames(v_sDataModelCode:=v_sGisDataModelCode, r_sDataSetDefFile:=sDataSetDefFile, r_sDataSetFile:=sDataSetFile), gPMConstants.PMEReturnCode)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetDataSetFileNames", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXMLDefUnknown")
                    Return result
                End If

                ' Try to load from the Empty XML files
                lReturn = CType(LoadFromXMLFile(v_sDataSetDefFile:=sDataSetDefFile, v_sDataSetFile:=sDataSetFile), gPMConstants.PMEReturnCode)

                ' If we could NOT Load from stored Empty Files
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Note that we are not attempting to build it ourselves from DB as done in bGIS
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetDataSetFileNames", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXMLDefUnknown")
                    Return result
                End If


                sXMLDataSetDef = m_oDataSetDef.InnerXml


                ' Now lets load the data that we have been given
                lReturn = CType(LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=v_sXMLDataSet), gPMConstants.PMEReturnCode)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to LoadFromXML", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXMLDefUnknown")
                    Return result
                End If


                Return result

            Catch excep As System.Exception
                result = gPMConstants.PMEReturnCode.PMError
                ' Log Error Message
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadFromXMLDefUnknownFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXMLDefUnknown", excep:=excep)
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
                m_oDataSetDef.PreserveWhitespace = False
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
                    MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Load Data Set Definition from XML String", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXML")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_oDataset.PreserveWhitespace = False
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
                    MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Load Data Set from XML String", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXML")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                NextOINumber = CInt(m_oDataset.DocumentElement.GetAttribute(ACXMLAttribNextOINumber))

                Return result

            Catch excep As System.Exception
                result = gPMConstants.PMEReturnCode.PMError
                ' Log Error Message
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadFromXMLFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXML", excep:=excep)
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
                Dim temp_xml_result As Boolean
                Try
                    oNewDataSet.LoadXml(v_sXMLDataSet)
                    temp_xml_result = True

                Catch parseError As System.Exception
                    temp_xml_result = False
                End Try
                bLoaded = temp_xml_result
                If Not bLoaded Then
                    MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Load Data Set from XML String", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateDataSetFromXML")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Get the Data Model Code Attribute

                sDataModelCode = CStr(oNewDataSet.DocumentElement.GetAttribute(GISSharedConstants.GISXMLAttribDataModelCode))

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
                    MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Data Model Code from Data Set does not match that of the Data Set Definition.", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateDataSetFromXML")
                    oNewDataSet = Nothing
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                Return result

            Catch excep As System.Exception
                result = gPMConstants.PMEReturnCode.PMError
                ' Log Error Message
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateDataSetFromXMLFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateDataSetFromXML", excep:=excep)

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
            Dim bLoaded As Boolean
            Dim sDataModelCode, sMsg As String
            Dim lNonGISType As Integer
            Dim vOIKeyArray() As Object
            Dim sOIKey As String = ""
            Dim lFrom, lTo As Integer
            Dim oTopObj, oElem As XmlElement
            Dim lReturn As gPMConstants.PMEReturnCode
            Dim oObjInst As XmlElement
            Dim oParentInst As XmlNode

            Try

                result = gPMConstants.PMEReturnCode.PMTrue

                If oSpecDOM Is Nothing Then
                    'Developer Guide no 180
                    MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="DOM Supplied Is NOTHING", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateSpecialObjects")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Get the keys for any existing Special Objects

                lReturn = CType(GetAllOIKey(v_sSpecialName, vOIKeyArray), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

                ' If there are any, delete them
                If Information.IsArray(vOIKeyArray) Then

                    lFrom = vOIKeyArray.GetLowerBound(0)

                    lTo = vOIKeyArray.GetUpperBound(0)
                    For lRow As Integer = lFrom To lTo

                        ' Get the Key

                        sOIKey = CStr(vOIKeyArray.GetValue(lRow))

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
                    'On Error Resume Next 'TBD this fails trying to add Claim which is already there
                    Try
                        For Each oElem2 As XmlElement In oSpecDOM.DocumentElement.ChildNodes
                            Try
                                oElem = oElem2
                                ' Set TopObj to be "???_POLICY_BINDER" node below "RISK_OBJECTS"
                                oTopObj = m_oDataset.DocumentElement.ChildNodes.Item(0).ChildNodes.Item(0)

                                'developer guide no.296
                                oTopObj.AppendChild(m_oDataset.ImportNode(oElem, True))
                            Catch
                            End Try
                        Next oElem2
                    Catch
                    End Try
                End If

                Return result

            Catch ex As Exception

                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error Message
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateSpecialObjectsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateSpecialObjects", excep:=ex)
                'Return result

                'Resume

                Return result
            End Try
        End Function


        Public Function ReturnAsXML(ByRef r_sXMLDataSet As String) As Integer

            Dim sDummy As String = String.Empty

            Return Me.ReturnAsXML(r_sXMLDataSetDef:=sDummy, _
                                  r_sXMLDataSet:=r_sXMLDataSet)

        End Function

        Public Function ReturnAsXML(ByRef r_sXMLDataSet As String, ByVal bPRE As Boolean) As Integer

            Dim sDummy As String = String.Empty

            Return Me.ReturnAsXML(r_sXMLDataSetDef:=sDummy, _
                                  r_sXMLDataSet:=r_sXMLDataSet, _
                                  bViaPRE:=bPRE)

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
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReturnAsXMLFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReturnAsXML", excep:=excep)

                Return result

            End Try
        End Function

        ''' <summary>
        ''' Returns the InnerXML of the Dataset Object and adds the standard wording dropped by PRE.
        ''' </summary>
        ''' <param name="r_sXMLDataSetDef"></param>
        ''' <param name="r_sXMLDataSet"></param>
        ''' <param name="bViaPRE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ReturnAsXML(ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataSet As String, ByVal bViaPRE As Boolean) As Integer

            Dim nResult As Integer = PMEReturnCode.PMTrue
            Dim oBeforePRECallXML As XmlDocument = New XmlDocument()
            Dim oAfterPRECallXML As XmlDocument = New XmlDocument()
            Try



                oBeforePRECallXML.LoadXml(r_sXMLDataSet)
                oAfterPRECallXML.LoadXml(m_oDataset.InnerXml)

                'Select StandardWording Nodes
                Dim nodeList As XmlNodeList = oBeforePRECallXML.SelectNodes("//node() [@TYPE='SW-5']")
                Dim navigator As XPathNavigator = oAfterPRECallXML.CreateNavigator()
                Dim sChild As String = String.Empty


                'Apend SW node in m_oDataSetDef.InnerXml
                For Each node As XmlNode In nodeList
                    sChild = String.Empty
                    navigator.MoveToRoot()
                    navigator.MoveToFollowing(node.ParentNode.Name, "")
                    sChild = node.OuterXml.ToString()
                    navigator.AppendChild(sChild)
                Next
                m_oDataset.InnerXml = oAfterPRECallXML.InnerXml


                r_sXMLDataSetDef = ""
                r_sXMLDataSet = ""

                r_sXMLDataSetDef = m_oDataSetDef.InnerXml

                ' RDT 06/11/02 - Check that the object exists
                If Not (m_oDataset.DocumentElement Is Nothing) Then

                    ' Update the Next Object Instance number in the XML before it is returned
                    m_oDataset.DocumentElement.SetAttribute(ACXMLAttribNextOINumber, m_lNextOINumber)

                End If

                r_sXMLDataSet = m_oDataset.InnerXml

                Return nResult

            Catch excep As System.Exception
                nResult = PMEReturnCode.PMError
                '   MainModule.LogMessage(iType:=PMELogLevel.PMLogOnError, _
                '  sMsg:="ReturnAsXMLFailed", vApp:=ACApp, vClass:=ACClass, _
                '   vMethod:="ReturnAsXML", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
                Return nResult
            Finally
                oBeforePRECallXML = Nothing
                oAfterPRECallXML = Nothing

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
                If oQuotes.GetAttribute(ACXMLAttribNextQuoteNum) IsNot Nothing Then
                    lNextQuoteNum = oQuotes.GetAttribute(ACXMLAttribNextQuoteNum).ToString
                    If lNextQuoteNum = String.Empty Then
                        lNextQuoteNum = "1"
                    End If
                Else
                    lNextQuoteNum = "1"
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
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewQuoteOutputFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewQuoteOutput", excep:=excep)

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
            Dim sOutObjsOIKey, lNextQuoteNum As String

            Dim lReturn As gPMConstants.PMEReturnCode
            Dim sDataModelCode, sTopQuoteObject, sTopQuoteTable As String
            'IJR 2003-04-17 Start
            Dim lNextOINumber As Integer
            'IJR 2003-04-17 End
            Try

                result = gPMConstants.PMEReturnCode.PMTrue

                ' Set the Quotes Instance to the Quotes Node
                oQuotes = GetObjectInstance(ACXMLQuotes)
                If oQuotes Is Nothing Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Get the Next Quote Number
                If oQuotes.GetAttribute(ACXMLAttribNextQuoteNum) IsNot Nothing Then
                    lNextQuoteNum = oQuotes.GetAttribute(ACXMLAttribNextQuoteNum).ToString
                    If lNextQuoteNum = String.Empty Then
                        lNextQuoteNum = "1"
                    End If
                Else
                    lNextQuoteNum = "1"
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
                    MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Data Model Code is blank. Need it to get SaveToDBMode from registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="NewQuoteOutputSaveDB")
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
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewQuoteOutputSaveDBFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewQuoteOutputSaveDB", excep:=excep)

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

            Dim lQuoteCount, lQuote As Integer

            Dim sTopQuoteObject, sTopQuoteTable As String

            Try

                result = gPMConstants.PMEReturnCode.PMTrue

                ' Delete Any Previous of the Quotes Area
                ' Note: Do not need to check return code,
                ' as we do not care if it was not already there.
                'BUT check before we delete as attempting to delete when not there gives an error

                'developer guide no solution no 254
                If Not (m_oDataset.GetElementById(ACXMLQuotes.Trim()) Is Nothing) Then
                    lReturn = DelObjectInstance(v_sObjectName:=ACXMLQuotes, v_sOIKey:=ACXMLQuotes)
                End If

                ' Create the Quotes Element
                oQuotesElem = m_oDataset.CreateElement(ACXMLQuotes)

                ' Set the Next Quote Number Attribute
                oQuotesElem.SetAttribute(ACXMLAttribNextQuoteNum, 1)
                ' Set the OI Key
                oQuotesElem.SetAttribute(ACXMLAttribOIKey, ACXMLQuotes)

                oQuotesElem.SetAttribute(ACXMLAttribClearQuotes, gPMConstants.PMEReturnCode.PMTrue)


                ' Append Quotes as a Child of the Data Set element

                m_oDataset.DocumentElement.AppendChild(oQuotesElem)

                oQuotesElem = Nothing

                Return result

            Catch excep As System.Exception




                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error Message
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ClearAllQuoteOutputFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearAllQuoteOutput", excep:=excep)

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
            Dim lReturn As Integer

            Dim oParentInst, oNewInst As XmlElement
            Dim sMsg, sTableName, sPKColName, sColumnName, sPropertyName As String
            Dim vPropertyArray, vPropertyValue As Object
            Dim iIsPrimaryKey As Integer
            Dim lRow As Integer
            Dim bIsAssumedInfo As Boolean
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
                        MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Object Instance Number MUST be supplied to load from Database.", vApp:=ACApp, vClass:=ACClass, vMethod:="NewObjectInstance")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Use the Object Instance Number Supplied
                    lOINumber = v_lOINumber

                    ' If the Supplied Object Instance Number is greater than or equal to
                    ' the Next Object Instance Num then set the Next Instance Num
                    ' to the Supplied Instance Num plus one.
                    If v_lOINumber >= NextOINumber Then
                        NextOINumber = v_lOINumber + 1
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
                        MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Scheme Key " & v_sQuoteKey & " is NOT valid.", vApp:=ACApp, vClass:=ACClass, vMethod:="NewObjectInstance")
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
                    'If v_sParentOIKey = "" Then
                    '    ' Calc the Risk Object Instance Key
                    '    r_sOIKey = BuildOIKey(v_sObjectName, lOINumber + GISSharedConstants.GISPolicyBinderOIOffset)
                    'Else
                    ' Calc the Risk Object Instance Key
                    r_sOIKey = BuildOIKey(v_sObjectName, lOINumber)

                    'End If

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
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewObjectInstanceFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewObjectInstance", excep:=excep)

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
            Dim lReturn As Integer
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
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DelObjectInstanceFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="DelObjectInstance", excep:=excep)

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
            Dim lReturn As Integer
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
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateObjectInstanceStatusFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateObjectInstanceStatus", excep:=excep)

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

            Dim sTableName, sColumnName, sPropertyName As String
            Dim vPropertyArray, vPropertyValue As Object
            Dim iIsPrimaryKey As gPMConstants.PMEReturnCode

            Try

                result = gPMConstants.PMEReturnCode.PMTrue

                ' Get a List of Properties for this Object
                lReturn = CType(GetObjectDefDetails(v_sObjectName:=v_sObjectName, r_sTableName:=sTableName, r_vPropertyArray:=vPropertyArray), gPMConstants.PMEReturnCode)
                If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (Not Information.IsArray(vPropertyArray)) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' For Each Property

                For lRow As Integer = vPropertyArray.GetLowerBound(1) To vPropertyArray.GetUpperBound(1)

                    ' Get the Property Name

                    sPropertyName = CStr(vPropertyArray.GetValue(0, lRow))

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
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ResetPropertyValuesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ResetPropertyValues", excep:=excep)

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
            Dim lReturn As Integer
            Dim oObjectInst As XmlElement
            Dim lObjectUpdateStatus, lUpdateStatus As Integer
            Dim sPropertyTag As String = ""

            Dim lAssumedInfo As Integer

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
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetPropertyValueFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPropertyValue", excep:=excep)

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
        'developer guide no.33
        Public Function GetPropertyValue(ByRef v_sObjectName As String, ByRef v_sPropertyName As String, ByRef v_sOIKey As String, ByRef r_vPropertyValue As Object, ByRef r_bIsAssumedInfo As Boolean) As Integer

            Dim result As Integer = 0
            Dim lReturn As gPMConstants.PMEReturnCode
            Dim oObjectInst As XmlElement
            Dim vAssumedInfo As Object
            Dim sPropertyTag As String = ""
            'developer guide no. 17
            Dim vPropertyValue As Object

            Try

                result = gPMConstants.PMEReturnCode.PMTrue

                ' Assumed information is not supported in this version of the Dataset control
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

                Dim lDataType As Integer
                If m_bStrict Then
                    lReturn = CType(GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=v_sPropertyName, r_lDataType:=lDataType), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        'developer guide no. String.Empty is not hendled in .NET
                        r_vPropertyValue = Nothing
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACApp & "." & ACClass & "." & "GetPropertyValue" + ", " + "Cant find definition for property " & v_sObjectName & "." & v_sPropertyName)
                    End If


                    If vPropertyValue <> "" Or String.IsNullOrEmpty(vPropertyValue) Or IsNothing(vPropertyValue) Then
                        r_vPropertyValue = vPropertyValue
                    Else
                        ' check type of r_vPropertyValue
                        Select Case Information.VarType(r_vPropertyValue)
                            Case VariantType.Boolean, VariantType.Short, VariantType.Integer, VariantType.Decimal, VariantType.Double, VariantType.Decimal, VariantType.Single, VariantType.Byte, VariantType.Date
                                r_vPropertyValue = 0

                            Case Else
                                r_vPropertyValue = vPropertyValue
                        End Select
                    End If
                Else
                    r_vPropertyValue = vPropertyValue
                End If

                oObjectInst = Nothing
                Return result

            Catch excep As System.Exception



                result = gPMConstants.PMEReturnCode.PMError


                ' Log Error Message
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPropertyValueFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPropertyValue", excep:=excep)

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
            Dim lReturn As Integer
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
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ResetUpdateStatusFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ResetUpdateStatus", excep:=excep)

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
            Dim lPos As Integer
            Dim sXMLDS, sXMLDSD As String
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
                sXMLDS = Strings.Replace(sXMLDS, " US=""1""", " US=""0""", , , CompareMethod.Text)
                ' Reset the Update Flage to View
                sXMLDS = Strings.Replace(sXMLDS, " US=""2""", " US=""0""", , , CompareMethod.Text)

                ' Load the Data Set Back Up Again
                lReturn = CType(LoadFromXML(sXMLDSD, sXMLDS), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

                Return result

            Catch excep As System.Exception



                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error Message
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ResetUpdateStatusWholeDataset Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ResetUpdateStatusWholeDataset", excep:=excep)

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
            Dim oObjectDef As XmlElement
            Dim oInstances As XmlNodeList
            Dim oInstance, oParentInst As XmlElement

            Dim lNum, lRow As Integer
            Dim vIdValueArray(,) As Object
            Dim lReturn As gPMConstants.PMEReturnCode
            Dim oRoot As XmlElement
            Dim lIsQuoteObject As gPMConstants.PMEReturnCode

            Try

                result = gPMConstants.PMEReturnCode.PMTrue


                'developer guide no. 12
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
                        If Information.IsArray(r_vObjectInstanceArray) Then

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


                            r_vObjectInstanceArray(GISSharedConstants.GISHierColObjectName, lRow) = oObjectDef.Name


                            r_vObjectInstanceArray(GISSharedConstants.GISHierColOIKey, lRow) = oInstance.GetAttribute(ACXMLAttribOIKey)
                            ' RFC29111999 - Do not need to hold the child number anymore
                            'r_vObjectInstanceArray(GISHierColChildNum, lRow) = oInstance.getAttribute(ACXMLAttribChildNum)

                            r_vObjectInstanceArray(GISSharedConstants.GISHierColChildNum, lRow) = lInst + 1

                            ' Get the Identifying Property Values for this Object Instance
                            lReturn = CType(GetPropertyValues(v_oObjectDef:=oObjectDef, v_oObjectInst:=oInstance, r_vIdValueArray:=vIdValueArray), gPMConstants.PMEReturnCode)
                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                                r_vObjectInstanceArray(GISSharedConstants.GISHierColIDName1, lRow) = Nothing

                                r_vObjectInstanceArray(GISSharedConstants.GISHierColIDValue1, lRow) = Nothing

                                r_vObjectInstanceArray(GISSharedConstants.GISHierColIDName2, lRow) = Nothing

                                r_vObjectInstanceArray(GISSharedConstants.GISHierColIDValue2, lRow) = Nothing

                                r_vObjectInstanceArray(GISSharedConstants.GISHierColIDName3, lRow) = Nothing

                                r_vObjectInstanceArray(GISSharedConstants.GISHierColIDValue3, lRow) = Nothing
                            Else


                                r_vObjectInstanceArray(GISSharedConstants.GISHierColIDName1, lRow) = vIdValueArray.GetValue(GISSharedConstants.GISIDColPropName, 0)


                                r_vObjectInstanceArray(GISSharedConstants.GISHierColIDValue1, lRow) = vIdValueArray.GetValue(GISSharedConstants.GISIDColPropValue, 0)


                                r_vObjectInstanceArray(GISSharedConstants.GISHierColIDName2, lRow) = vIdValueArray.GetValue(GISSharedConstants.GISIDColPropName, 1)


                                r_vObjectInstanceArray(GISSharedConstants.GISHierColIDValue2, lRow) = vIdValueArray.GetValue(GISSharedConstants.GISIDColPropValue, 1)


                                r_vObjectInstanceArray(GISSharedConstants.GISHierColIDName3, lRow) = vIdValueArray.GetValue(GISSharedConstants.GISIDColPropName, 2)


                                r_vObjectInstanceArray(GISSharedConstants.GISHierColIDValue3, lRow) = vIdValueArray.GetValue(GISSharedConstants.GISIDColPropValue, 2)
                            End If

                            oParentInst = oInstance.ParentNode
                            If oParentInst Is Nothing Then

                                r_vObjectInstanceArray(GISSharedConstants.GISHierColParentOIKey, lRow) = ""
                            Else
                                If (oParentInst.Name = ACXMLRiskObjects) Or (oParentInst.Name = ACXMLQuoteObjects) Then

                                    r_vObjectInstanceArray(GISSharedConstants.GISHierColParentOIKey, lRow) = ""
                                Else


                                    r_vObjectInstanceArray(GISSharedConstants.GISHierColParentOIKey, lRow) = oParentInst.GetAttribute(ACXMLAttribOIKey)
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
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInstanceHierarchyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInstanceHierarchy", excep:=excep)

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
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetObjectIdentityFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetObjectIdentity", excep:=excep)

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
        'developer guide no 263.
        Public Function GetObjectDefDetails(ByRef v_sObjectName As String, Optional ByRef r_lIsQuoteObject As Integer = 0, Optional ByRef r_lGISObjectID As Integer = 0, Optional ByRef r_sTableName As String = "", Optional ByRef r_lMaxInstances As Integer = 0, Optional ByRef r_lPolarisObjectID As Integer = 0, Optional ByRef r_sParentObjectName As String = "", Optional ByRef r_vChildObjectArray As Object = -1, Optional ByRef r_vPropertyArray As Object = -1, Optional ByRef r_sSelectSQL As String = "", Optional ByRef r_sInsertSQL As String = "", Optional ByRef r_sUpdateSQL As String = "", Optional ByRef r_sDeleteSQL As String = "", Optional ByRef r_lIsSelectableForScreen As Integer = 0, Optional ByRef r_lIsNonGIS As Integer = 0, Optional ByRef r_lEditFlags As Integer = 0) As Integer

            Dim result As Integer = 0
            Dim oObjectDef, oParentDef, oChildDef As XmlElement
            Dim lNumOfChild As Integer
            Dim vChildObjectArray, vPropertyArray As Object
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

                If Convert.IsDBNull(vSQL) Or IsNothing(vSQL) Then
                    r_sSelectSQL = ""
                Else
                    r_sSelectSQL = vSQL
                End If

                vSQL = CStr(oObjectDef.GetAttribute(ACXMLAttribSQLInsert))

                If Convert.IsDBNull(vSQL) Or IsNothing(vSQL) Then
                    r_sInsertSQL = ""
                Else
                    r_sInsertSQL = vSQL
                End If

                vSQL = CStr(oObjectDef.GetAttribute(ACXMLAttribSQLUpdate))

                If Convert.IsDBNull(vSQL) Or IsNothing(vSQL) Then
                    r_sUpdateSQL = ""
                Else
                    r_sUpdateSQL = vSQL
                End If

                vSQL = CStr(oObjectDef.GetAttribute(ACXMLAttribSQLDelete))

                If Convert.IsDBNull(vSQL) Or IsNothing(vSQL) Then
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

                'developer guide no 263.
                If Not (Information.IsArray(r_vChildObjectArray)) And Not (Information.IsArray(r_vPropertyArray)) Then
                    If (r_vChildObjectArray = -1) And (r_vPropertyArray = -1) Then
                        oObjectDef = Nothing
                        oParentDef = Nothing
                        Return result
                    End If
                End If


                'developer guide no. 12
                vChildObjectArray = Nothing

                'developer guide no. 12
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
                            If Not Information.IsArray(vChildObjectArray) Then
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
                            If Not Information.IsArray(vPropertyArray) Then
                                ReDim vPropertyArray(1, 0)
                            Else

                                ReDim Preserve vPropertyArray(1, vPropertyArray.GetUpperBound(1) + 1)
                            End If

                            ' Add the Property Name to the end of the Array

                            lPropertyNum = vPropertyArray.GetUpperBound(1)


                            vPropertyArray(0, lPropertyNum) = oChildDef.GetAttribute(ACXMLAttribPropertyName)


                            vPropertyArray(1, lPropertyNum) = oChildDef.GetAttribute(ACXMLAttribIsIdentProp)

                        End If

                    Next lChildNum

                End If

                ' Return the Results and Tidy Up


                'developer guide no 263.
                If Information.IsArray(r_vChildObjectArray) OrElse r_vChildObjectArray Is Nothing Then
                    ' If Not (r_vChildObjectArray = -1) Then


                    r_vChildObjectArray = vChildObjectArray
                    'End If
                End If

                'developer guide no 263.
                If Information.IsArray(r_vPropertyArray) OrElse r_vPropertyArray Is Nothing Then
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


                result = gPMConstants.PMEReturnCode.PMError

                oObjectDef = Nothing
                oParentDef = Nothing
                oChildDef = Nothing

                ' Log Error Message
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetObjectDefDetailsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetObjectDefDetails", excep:=New Exception(Information.Err().Description))

                Return result

            Catch exc As System.Exception
                ' Log Error Message
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=exc.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="GetObjectDefDetails", excep:=exc)

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
            Dim lReturn As Integer
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
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPropertyDefDetailsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPropertyDefDetails", excep:=excep)

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

                If Not Information.IsNothing(r_vPropertyValueArray) Then

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
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetObjectInstDetailsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetObjectInstDetails", excep:=excep)

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
            Dim lReturn As Integer

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
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ObjectInstExistsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ObjectInstExists", excep:=excep)

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
        Public Function GetObjectInstChild(ByRef v_sObjectName As String, ByRef v_sOIKey As String, ByRef v_sChildObjectName As String, ByRef r_vChildOIKeyArray() As Object) As Integer

            Dim result As Integer = 0
            Dim oObjectInst As XmlElement
            Dim oChildInsts As XmlNodeList
            Dim oChildInst As XmlElement
            Dim lNumOfChild As Integer

            Try

                result = gPMConstants.PMEReturnCode.PMTrue


                'developer guide no. 12
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
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetObjectInstChildFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetObjectInstChild", excep:=excep)

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
        'developer guide no. Change as per vb code
        Public Function GetAllOIKey(ByRef v_sObjectName As String, ByRef r_vOIKeyArray() As Object) As Integer

            Dim result As Integer = 0
            Try

                result = gPMConstants.PMEReturnCode.PMTrue


                Return CommonGetAllOIKey(v_sObjectName:=v_sObjectName, r_vOIKeyArray:=r_vOIKeyArray)

            Catch excep As System.Exception



                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error Message
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllOIKeyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllOIKey", excep:=excep)

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
        Public Function GetDeletedOIKey(ByRef v_sObjectName As String, ByRef r_vOIKeyArray() As Object, Optional ByRef v_bDeletedObjects As Boolean = False) As Integer

            Dim result As Integer = 0
            Try

                result = gPMConstants.PMEReturnCode.PMTrue


                Return CommonGetAllOIKey(v_sObjectName:=v_sObjectName, r_vOIKeyArray:=r_vOIKeyArray, v_bDeletedObjects:=v_bDeletedObjects)

            Catch excep As System.Exception



                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error Message
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDeletedOIKeyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDeletedOIKey", excep:=excep)

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


                'developer guide no. 12
                r_vDelObjArray = Nothing

                ' Get the Deleted Objects Node
                oDelObjs = GetObjectInstance(ACXMLDeletedObjects)

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

                oDelInst = Nothing
                oDelObjs = Nothing

                Return result

            Catch excep As System.Exception



                result = gPMConstants.PMEReturnCode.PMError


                ' Log Error Message
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDelObjectsOIKeyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDelObjectsOIKey", excep:=excep)

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
        Public Function GetChildOIKey(ByRef v_sParentObjectName As String, ByRef v_sParentOIKey As String, ByRef v_sChildObjectName As String, ByRef r_vChildOIKeyArray() As Object) As Integer

            Dim result As Integer = 0
            Dim oAllInsts As XmlNodeList
            Dim oParentInst, oObjectInst As XmlElement
            Dim lNumofInst As Integer

            Try

                result = gPMConstants.PMEReturnCode.PMTrue


                'developer guide no. 12
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
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetChildOIKeyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetChildOIKey", excep:=excep)

                Return result

            End Try
        End Function


        ' ***************************************************************** '
        ' Name: GetAllQuoteKey
        '
        ' Description: Returns an Array of Quote Keys.
        '
        ' ***************************************************************** '
        'UPGRADE_NOTE: (7001) The following declaration (GetAllQuoteKey) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
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


        'r_vOIKeyArray.SetValue(oObjectInst.GetAttribute(ACXMLAttribOIKey), lInstNum)
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
        'mainmodule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllQuoteKeyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllQuoteKey", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
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
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BuildOIKeyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildOIKey", excep:=excep)

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
                    'Developer Guide no 180
                    MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Locate Top Level Risk Object. Has the Data Set been Initialised?", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTopLevelRiskObject")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If oRiskObjectsDef.ChildNodes.Count = 0 Then
                    'Developer Guide no 180
                    MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="There are NO Risk Objects loaded.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTopLevelRiskObject")
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
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTopLevelRiskObjectFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTopLevelRiskObject", excep:=excep)

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
                    MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Locate Top Level Quote Object. Has the Data Set been Initialised?", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTopLevelQuoteObject")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If oQuoteObjectsDef.ChildNodes.Count = 0 Then
                    MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="There are NO Quote Objects loaded.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTopLevelQuoteObject")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If oQuoteObjectsDef.ChildNodes.Count > 1 Then
                    MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="There are more than 1 Top Level Quote Objects.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTopLevelQuoteObject")
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
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTopLevelQuoteObjectFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTopLevelQuoteObject", excep:=excep)

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

                'developer guide no solution no. 38
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
                    MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Load Data Set from XML String", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXML")
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
                    MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="No Quote Outputs Found.", vApp:=ACApp, vClass:=ACClass, vMethod:="MergeQuoteOutput")
                    Return result
                End If

                ' Get the Number of Quotes
                lNumQtes = oQuotes.Count

                ' If there are NO Quotes then exit
                If lNumQtes <= 0 Then
                    MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="No Quote Outputs Found.", vApp:=ACApp, vClass:=ACClass, vMethod:="MergeQuoteOutput")
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
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MergeQuoteOutputFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="MergeQuoteOutput", excep:=excep)

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
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveXMLToFileFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveXMLToFile", excep:=excep)

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
            Dim sTopLevelObject, sTopLevelTable As String
            Dim vOIKeyArray() As Object
            Dim sOIKey As String = ""
            Dim bAssumedInfo As Boolean
            Dim lReturn As gPMConstants.PMEReturnCode
            Dim vPolicyLinkID As Object

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
                If Not Information.IsArray(vOIKeyArray) Then
                    MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to find any instances of Top Level Object :- " & sTopLevelObject, vApp:=ACApp, vClass:=ACClass, vMethod:="PolicyLinkID")
                    Return gPMConstants.PMEReturnCode.PMFalse
                Else

                    If vOIKeyArray.GetUpperBound(0) > vOIKeyArray.GetLowerBound(0) Then
                        MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="There should be only ONE instance of Top Level Object :- " & sTopLevelObject, vApp:=ACApp, vClass:=ACClass, vMethod:="PolicyLinkID")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    Else


                        sOIKey = CStr(vOIKeyArray.GetValue(vOIKeyArray.GetLowerBound(0)))
                    End If
                End If

                ' Get the Policy Link ID Property in the Top Level Object

                ' developer guide no. 98
                lReturn = CType(GetPropertyValue(v_sObjectName:=sTopLevelObject, v_sPropertyName:=GISSharedConstants.GISPolLinkIDName, v_sOIKey:=sOIKey, r_vPropertyValue:=vPolicyLinkID, r_bIsAssumedInfo:=bAssumedInfo), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                Dim dbNumericTemp As Double
                If Double.TryParse(CStr(vPolicyLinkID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                    result = CInt(vPolicyLinkID)
                Else

                    MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="PolicyLinkID Value is not set or NOT Numeric :-" & CStr(vPolicyLinkID), vApp:=ACApp, vClass:=ACClass, vMethod:="PolicyLinkID")
                End If

                Return result

            Catch excep As System.Exception



                result = -1

                ' Log Error Message
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PolicyLinkIDFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="PolicyLinkID", excep:=excep)

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
        Private Function CommonGetAllOIKey(ByRef v_sObjectName As String, ByRef r_vOIKeyArray() As Object, Optional ByRef v_bDeletedObjects As Boolean = False) As Integer

            Dim result As Integer = 0
            Dim oAllInsts As XmlNodeList
            Dim oObjectInst As XmlElement
            Dim lNumofInst As Integer
            Dim oRoot As XmlElement
            Dim lReturn As gPMConstants.PMEReturnCode
            'developer guide no. As per vb code
            Dim lIsQuoteObject As Integer = 0

            Try

                result = gPMConstants.PMEReturnCode.PMTrue


                'developer guide no. 12
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

            Catch excep As System.Exception



                result = gPMConstants.PMEReturnCode.PMError


                ' Log Error Message
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommonGetAllOIKeyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommonGetAllOIKey", excep:=excep)

                Return result

            End Try
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

            Try

                result = gPMConstants.PMEReturnCode.PMTrue

                ' Create the Root Level Element
                oDataSetDef = m_oDataSetDef.CreateElement(ACXMLDataSetDef)

                ' Set the Data Model Code Attribute
                oDataSetDef.SetAttribute(GISSharedConstants.GISXMLAttribDataModelCode, v_sGisDataModelCode)

                ' RDC 27072001 Set the timestamp for the dataset definition
                oDataSetDef.SetAttribute(GISSharedConstants.GISXMLAttribDataSetDefTimestamp, DateTime.Now.ToString("yyyyMMddHHmmss"))

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
                'oSpecialObjects = m_oDataSetDef.CreateElement(ACXMLSpecialObjects)
                'oSpecialObjects.SetAttribute(ACXMLAttribObjectName, "")
                'oSpecialObjects.SetAttribute(ACXMLAttribIsNonGIS, 0)

                ' Append Special Objects to the Document Element

                'm_oDataSetDef.DocumentElement.AppendChild(oSpecialObjects)

                oDataSetDef = Nothing
                oRiskObjects = Nothing
                oQuoteObjects = Nothing
                oSpecialObjects = Nothing

                Return result

            Catch excep As System.Exception




                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error Message
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InitDataSetDefFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="InitDataSetDef", excep:=excep)

                Return result

            End Try
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

            Try


                ' Build Up the Key

                ' Upercase the Object Name
                sNodeKey = v_sObjectName.Trim().Trim("_").ToUpper()

                ' If we have been supplied a Property Name then Add
                ' a full stop followed by the Property Name
                If v_sPropertyName.Trim() <> "" Then
                    sNodeKey = sNodeKey & "." & v_sPropertyName.Trim().ToUpper()
                End If

                ' Find elements with the Tag Name of the Node Key
                ' This returns a List of Nodes, however there should only be one
                oNodes = m_oDataSetDef.GetElementsByTagName(sNodeKey)

                If oNodes Is Nothing Then
                    MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to find Definiton for :-" & sNodeKey, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefinitionNode")
                    Return result
                End If

                ' Get the Node from the List
                ' Remember there should only be one
                oNode = oNodes.Item(0)
                If oNode Is Nothing Then
                    MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to find Definition for :-" & sNodeKey, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefinitionNode")
                    Return result
                End If

                ' All OK so return the Object/Property Definition Node
                result = oNode

                ' Release Local References
                oNodes = Nothing
                oNode = Nothing

                Return result

            Catch excep As System.Exception




                result = Nothing

                ' Log Error Message
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefinitionNodeFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefinitionNode", excep:=excep)

                Return result

            End Try
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

            Try



                ' Get the Object Instance

                'developer guide no 254.
                oObjectInst = m_oDataset.GetElementById(v_sOIKey.Trim())
                If oObjectInst Is Nothing Then
                    oObjectInst = m_oDataset.GetElementsByTagName(v_sOIKey.Trim()).Item(0)
                End If
                ' If we did NOT the Instance then Error
                If oObjectInst Is Nothing And v_sOIKey.Trim() <> "DELETED_OBJECTS" Then
                    MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:="Failed to Find Instance for Key :- " & v_sOIKey, vApp:=ACApp, vClass:=ACClass, vMethod:="GetObjectInstance")
                    Return result
                End If

                ' Return the Object Instance
                result = oObjectInst

                ' Release Local Reference
                oObjectInst = Nothing

                Return result

            Catch excep As System.Exception



                result = Nothing

                ' Log Error Message
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetObjectInstanceFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetObjectInstance", excep:=excep)

                Return result

            End Try
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

            Dim vIdValueArray(,) As Object
            Dim vAllValueArray(,) As Object

            Try

                result = gPMConstants.PMEReturnCode.PMTrue


                If (Information.IsNothing(r_vIdValueArray)) And (Information.IsNothing(r_vAllValueArray)) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Get the Object Name

                sObjectName = CStr(v_oObjectDef.GetAttribute(ACXMLAttribObjectName))


                'developer guide no. 12
                r_vIdValueArray = Nothing

                'developer guide no. 12
                r_vAllValueArray = Nothing

                ' Redim the Array. Always Allow for 3 Name/Value Pairs
                ' Note: Always allow for 3 values. If there are not 2 ID Properties
                ' the unused rows will be Empty
                ReDim vIdValueArray(1, 2)
                lIDNum = 0

                ' We will build the All Value Array Dynamically

                'developer guide no. 12
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

                        If Not Information.IsNothing(r_vIdValueArray) Then

                            ' If this is an Identifying Property then add it to the ID Value array
                            ' Note: We can only return 3 Identifying Properties

                            If (lIDProperty = gPMConstants.PMEReturnCode.PMTrue) And (lIDNum <= vIdValueArray.GetUpperBound(1)) Then

                                ' Build the Array

                                vIdValueArray(GISSharedConstants.GISIDColPropName, lIDNum) = sPropertyName

                                vIdValueArray(GISSharedConstants.GISIDColPropValue, lIDNum) = vPropertyValue

                                ' Increment the Array Row
                                lIDNum += 1

                            End If

                        End If

                        ' If we are building the All Value Array

                        If Not Information.IsNothing(r_vAllValueArray) Then

                            ' Resize the All Value Array
                            If Information.IsArray(vAllValueArray) Then

                                lAllNum = vAllValueArray.GetUpperBound(1) + 1
                                ReDim Preserve vAllValueArray(1, lAllNum)
                            Else
                                lAllNum = 0
                                ReDim vAllValueArray(1, 0)
                            End If

                            ' Add the Property

                            vAllValueArray(GISSharedConstants.GISIDColPropName, lAllNum) = sPropertyName

                            vAllValueArray(GISSharedConstants.GISIDColPropValue, lAllNum) = vPropertyValue

                        End If

                    End If

                Next lChild

                ' If required, return the IDValue Array

                If Not Information.IsNothing(r_vIdValueArray) Then


                    r_vIdValueArray = vIdValueArray
                End If

                ' If required, return the ALLValue Array

                If Not Information.IsNothing(r_vAllValueArray) Then


                    r_vAllValueArray = vAllValueArray
                End If


                vIdValueArray = Nothing

                vAllValueArray = Nothing

                oChildDef = Nothing

                Return result

            Catch excep As System.Exception



                result = gPMConstants.PMEReturnCode.PMError





                ' Log Error Message
                'Developer Guide no 180
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPropertyValuesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPropertyValues", excep:=excep)

                Return result

            End Try
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
        Public Function SetRoot(Optional ByVal v_lQuoteNumber As Integer = 0) As DataSetControl.Node

            Dim result As DataSetControl.Node = Nothing
            Dim oNode As DataSetControl.Node

            Try

                ' Create  a new Node element
                oNode = New DataSetControl.Node()

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
                    oNode.Root = oNode.Root.childNodes.item(v_lQuoteNumber - 1)
                    ' Set the Quote Key

                    oNode.QuoteKey = CStr(oNode.Root.getAttribute(ACXMLAttribOIKey))
                End If

                result = oNode
                oNode = Nothing

                Return result

            Catch excep As System.Exception



                Throw

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
        'UPGRADE_NOTE: (7001) The following declaration (Initialise) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
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
        'mainmodule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise()", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
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
        Public Function Terminate() As Integer

            Dim result As Integer = 0
            Static bTerminated As Boolean
            Dim lReturn As Integer

            Try

                result = gPMConstants.PMEReturnCode.PMTrue

                ' Check if this method has already been called.
                If bTerminated Then
                    Return result
                End If

                ' Set the terminated flag to true to indicate
                ' this method has been called.
                bTerminated = True

                ' Termination Code.

                ' Release References
                m_oDataSetDef = Nothing
                m_oDataset = Nothing
                m_oDefaults = Nothing 'CJB 280301

                '    ' RFC140700 - Terminate Everything
                '    If (m_oNode Is Nothing = False) Then
                '        m_oNode.Root = Nothing
                '        m_oNode.Dataset = Nothing
                '    End If
                '    Set m_oNode = Nothing

                Return result

            Catch excep As System.Exception



                ' Error.

                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error Message
                mainmodule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Terminate()", vApp:=ACApp, vClass:=ACClass, vMethod:="Terminate", excep:=excep)

                Return result

            End Try
        End Function


        ' ***************************************************************** '
        ' Name: PropertyValue
        '
        ' Description: Returns the Property Value from the Attribute
        '
        ' ***************************************************************** '
        Friend Function PropertyValue(ByRef r_oObjectInst As XmlElement, ByRef v_sPropertyName As String) As String
            Dim result As String = String.Empty
            Dim lReturn As gPMConstants.PMEReturnCode

            Try
                ' Get the Property Value
                Dim lDataType As Integer
                If m_bStrict Then
                    lReturn = CType(GetPropertyDefDetails(v_sObjectName:=r_oObjectInst.Name, v_sPropertyName:=v_sPropertyName, r_lDataType:=lDataType), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACApp & "." & ACClass & "." & "PropertyValue" + ", " + "Cant find definition for property " & r_oObjectInst.Name & "." & v_sPropertyName)
                    End If
                    Return result
                Else

                End If
                Try
                    result = CStr(r_oObjectInst.GetAttribute(v_sPropertyName.Trim().ToUpper()))
                Catch ex As Exception

                End Try

                If Convert.IsDBNull(result) Or IsNothing(result) Then
                    result = String.Empty
                End If


                If Not String.IsNullOrEmpty(result) Then
                    ' More to do here, may have to unconvert ampersands etc
                    result = result.Replace("&amp;", "&")
                    result = result.Replace("&lt;", "<")
                    result = result.Replace("&gt;", ">")
                    result = result.Replace("&apos;", "'")
                    result = result.Replace("&quot;", """")
                End If



            Catch ex As Exception
                Dim lErrorNumber As Integer
                Dim sErrorDesc As String = ""

                lErrorNumber = Information.Err().Number
                sErrorDesc = Information.Err().Description

                ' Log Error Message
                MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PropertyValueFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertyValue", excep:=New Exception(Information.Err().Description))

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
            Dim lUpdateStatus As Integer
            Dim sPropertyTag As String = ""

            Dim lAssumedInfo As Integer
            Dim vValue As String = ""
            Dim lDataType, lErrorNumber As Integer
            Dim sErrorDesc As String = ""



            ' RAW 20/09/2004 : CQ6832 : added test for v_vPropertyValue = null

            If Convert.IsDBNull(v_vPropertyValue) Or IsNothing(v_vPropertyValue) Then

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
                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACApp & "." & ACClass & "." & "SetPropertyValue" + ", " + "Cant find definition for object " & v_sObjectName & "(for property " & v_sPropertyName & ")")
                        End If

                        Select Case v_sPropertyName.ToUpper()
                            Case "US"
                                lDataType = GISSharedConstants.GISDataTypeNumeric
                            Case Else
                                lDataType = GISSharedConstants.GISDataTypeText
                        End Select

                    Case Else
                        ' Get the Property Details
                        lReturn = CType(GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=v_sPropertyName, r_lDataType:=lDataType), gPMConstants.PMEReturnCode)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACApp & "." & ACClass & "." & "SetPropertyValue" + ", " + "Cant find definition for property " & v_sObjectName & "." & v_sPropertyName)
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
                ' RAW 14/01/2004 : CQ3720 : added
                ' validate value against DT



                If (Not (Convert.IsDBNull(v_vPropertyValue) Or IsNothing(v_vPropertyValue))) And (Not Object.Equals(v_vPropertyValue, Nothing)) And (CStr(v_vPropertyValue) <> "") Then


                    Select Case lDataType
                        Case GISSharedConstants.GISDataTypeDate

                            If Not Information.IsDate(v_vPropertyValue) Then
                                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACApp & "." & ACClass & "." & "PropertyValueSet" + ", " + "Invalid Value for Date property " & v_sObjectName & "." & v_sPropertyName)
                            End If

                        Case GISSharedConstants.GISDataTypeNumeric, GISSharedConstants.GISDataTypeNumericOutput, GISSharedConstants.GISDataTypeOption, GISSharedConstants.GISDataTypeCurrency, GISSharedConstants.GISDataTypePercentage


                            Dim dbNumericTemp As Double
                            If Not Double.TryParse(CStr(v_vPropertyValue), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACApp & "." & ACClass & "." & "PropertyValueSet" + ", " + "Invalid Value for Numeric property " & v_sObjectName & "." & v_sPropertyName)
                            End If
                    End Select
                End If
                ' RAW 14/01/2004 : CQ3720 : end
            End If

            ' Now reformat the value




            Dim oPropertyDef As XmlElement
            If (Not (Convert.IsDBNull(v_vPropertyValue) Or IsNothing(v_vPropertyValue))) And (Not Object.Equals(v_vPropertyValue, Nothing)) And (CStr(v_vPropertyValue) <> "") Then

                ' Escape ampersands etc
                ' Double Quotes are doubled up so that the save to DB will work
                '       v_vPropertyValue = Replace(v_vPropertyValue, "&", "&amp;")
                '       v_vPropertyValue = Replace(v_vPropertyValue, "<", "&lt;")
                '       v_vPropertyValue = Replace(v_vPropertyValue, ">", "&gt;")
                '       v_vPropertyValue = Replace(v_vPropertyValue, "'", "&apos;")


                v_vPropertyValue = CStr(v_vPropertyValue).Replace("""", "&quot;")

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
                    If Information.IsDate(v_vPropertyValue) Then
                        If Not m_bStrict Then
                            'For speed, run an inline reduced version of GetPropertyDefDetails
                            lDataType = -1 'not a date
                            oPropertyDef = GetDefinitionNode(v_sObjectName, v_sPropertyName)
                            If Not (oPropertyDef Is Nothing) Then

                                lDataType = CInt(oPropertyDef.GetAttribute(ACXMLAttribDataType))
                                oPropertyDef = Nothing
                            End If
                        End If

                        If (lDataType = GISSharedConstants.GISDataTypeDate) And (Not (DateAndTime.DatePart("yyyy", CDate(v_vPropertyValue), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) = 1899)) Then
                            v_vPropertyValue = CDate(v_vPropertyValue).ToString("yyyy-MM-dd HH:mm:ss")
                        End If
                    End If

                End If


                v_vPropertyValue = CStr(v_vPropertyValue).Trim() ' RAW 14/01/2004 : CQ3720 : added
            End If
            ' Set the Value
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

            lErrorNumber = Information.Err().Number
            sErrorDesc = Information.Err().Description
            ' Log Error Message
            MainModule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PropertyValueSetFailed for property " & v_sObjectName & "." & v_sPropertyName, vApp:=ACApp, vClass:=ACClass, vMethod:="PropertyValueSet", excep:=New Exception(Information.Err().Description))

            ' RAW 22/01/2004 : CQ3720 : mainmodule.LogMessage clears err object so use original details
            Throw New System.Exception(lErrorNumber.ToString() + ", " + ACApp & "." & ACClass & "." & "PropertyValueSet" + ", " + sErrorDesc)

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
                mainmodule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", excep:=excep)

                Exit Sub

            End Try

        End Sub

        Protected Overrides Sub Finalize()

            Dim lReturn As gPMConstants.PMEReturnCode

            ' Class Terminate

            Try

                ' Terminate this Class
                lReturn = CType(Terminate(), gPMConstants.PMEReturnCode)

                '    g_lClassInitLevel = g_lClassInitLevel - 1
                '    Debug.Print "Terminate = " & g_lClassInitLevel

            Catch excep As System.Exception



                ' Error.

                ' Log Error Message
                mainmodule.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Terminate", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Terminate", excep:=excep)

                Exit Sub

            End Try

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

            Try

                result = gPMConstants.PMEReturnCode.PMTrue


                'Create a new XML Document object in which to 'load' the data from the
                'XML document (flat) file
                m_oDefaults = New XmlDocument()

                ' Use the new parser

                'developer guide no. 38(no solution)
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
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Load Defaults from File : " & v_sGISDefaultsFile, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadDefaultsXMLFile")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                Return result

            Catch excep As System.Exception



                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadDefaultsXMLFileFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadDefaultsXMLFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

                Return result

            End Try
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

            Dim lObjCount, lPropCount As Integer

            Dim sCurrentObjectName, sCurrentPropertyName, sCurrentDefaultValue, sCurrentPropertyValue As String

            Dim sMandatoryPropertyName, sMandatoryPropertyValue As String

            Dim vObjectKeyArray() As Object
            Dim sCurrentObjectKey, sParentObjectKey As String

            Dim bIsAssumedInfo As Boolean

            Try

                GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadAndApplyGISDefaults - Start", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                'Determine the name (and path) to the current solution specific GIS
                'defaults xml document
                lReturn = CType(GISSharedConstants.GetDefaultsFileName(GISDataModelCode, sGISDefaultsFile), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Check if the file actually exists as it is optional - if not then exit without
                'errors and continue processing
                If FileSystem.Dir(sGISDefaultsFile, FileAttribute.Normal) = "" Then
                    GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Warning:Defaults XML file not found at " & sGISDefaultsFile, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

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
                    GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Warning:The following Defaults XML file was found but had no object definitions in:" & sGISDefaultsFile, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

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
                            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ERROR:An object with no name has been defined in the Defaults XML file.", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

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
                                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ERROR:The following object has a property with no name in the Defaults XML file:" & sCurrentObjectName, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                                    Return gPMConstants.PMEReturnCode.PMFalse
                                End If

                                'Check a property value has been specified ! If not, error and exit
                                If sCurrentDefaultValue.Trim() = "" Then
                                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ERROR:The following object has a property with no value in the Defaults XML file. Object:" & sCurrentObjectName & " Property:" & sCurrentPropertyName, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                                    Return gPMConstants.PMEReturnCode.PMFalse
                                End If

                                'At this point we have an object name, property name and default
                                'value so we need to apply this information to the dataset, then
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
                                If Information.IsArray(vObjectKeyArray) Then

                                    For lInstanceCounter As Integer = vObjectKeyArray.GetLowerBound(0) To vObjectKeyArray.GetUpperBound(0)


                                        sCurrentObjectKey = CStr(vObjectKeyArray.GetValue(lInstanceCounter))

                                        'Check there is a mandatory property defined for this object in the Defaults
                                        'xml file - if none, error and exit

                                        Dim auxVar As Object = oCurrentObject.GetAttribute(GISSharedConstants.GISDefaultsMandatoryProperty)


                                        If Convert.IsDBNull(auxVar) Or IsNothing(auxVar) Or CStr(oCurrentObject.GetAttribute(GISSharedConstants.GISDefaultsMandatoryProperty)).Trim() = "" Then
                                            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ERROR:No mandatory property has been defined in the Defaults XML file for the object:" & sCurrentObjectName, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                                            Return gPMConstants.PMEReturnCode.PMFalse
                                        End If

                                        'What is the mandatory property first?

                                        sMandatoryPropertyName = CStr(oCurrentObject.GetAttribute(GISSharedConstants.GISDefaultsMandatoryProperty))

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
                                                GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="The following property default in the Defaults XML file has been applied - Object:" & sCurrentObjectName & "(" & CStr(lInstanceCounter + 1) & ") Property:" & sCurrentPropertyName & " Value:" & sCurrentDefaultValue & "     ", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                                            Else
                                                'Log a DEBUG message that current property value is NOT "" so this default has been ignored
                                                GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Warning:The following property default in the Defaults XML file has NOT been applied since the current property already has a value (of " & sCurrentPropertyValue & ") - Object:" & sCurrentObjectName & "(" & CStr(lInstanceCounter + 1) & ") Property:" & sCurrentPropertyName & " Value:" & sCurrentDefaultValue & "     ", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                                            End If
                                        Else
                                            'Log a DEBUG message that Mandatory property is "" so this default has been ignored
                                            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Warning:The following property default in the Defaults XML file has NOT been applied since the specified mandatory property value for this object (" & sMandatoryPropertyName & ") was not set - Object:" & sCurrentObjectName & "(" & CStr(lInstanceCounter + 1) & ") Property:" & sCurrentPropertyName & " Value:" & sCurrentDefaultValue & "     ", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                                        End If

                                    Next

                                Else

                                    'Object doesn't exist so ignore (but log)
                                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Warning:The following property default in the Defaults XML file has NOT been applied since an object instance did not exist to apply it to - Object:" & sCurrentObjectName & " Property:" & sCurrentPropertyName & " Value:" & sCurrentDefaultValue & "     ", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                                End If

                            Else
                                'Current object has no matching property and default value pairs i.e. it
                                'does NOT have a <PROPERTY_NAME> and <DEFAULT_VALUE> defined for it.
                                'Error and exit
                                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error:The following object in the Defaults XML file has no matching <PROPERTY_NAME> and <DEFAULT_VALUE> values defined:" & sCurrentObjectName, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                        Next
                    Else
                        'Log a DEBUG message that Defaults file was found but no objects defined in it, exit and continue
                        GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Warning:The following Defaults XML file was found but had no object definitions in:" & sGISDefaultsFile, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    End If
                Next

                GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadAndApplyGISDefaults - End", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                ' Release References
                oObject = Nothing
                oProperty = Nothing
                oCurrentObject = Nothing

                Return result

            Catch excep As System.Exception



                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadAndApplyGISDefaultsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

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
                    lReturn = CType(GISSharedConstants.GetDefaultsFileName(GISDataModelCode, sGISDefaultsFile), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Check if the file actually exists - if not then error & exit
                    If FileSystem.Dir(sGISDefaultsFile, FileAttribute.Normal) = "" Then
                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error:Defaults XML file not found at " & sGISDefaultsFile & ". Note this is required in order for QMM Cobol Linkage to be correctly populated with 'real' instances only and not object instances that are defaults (or slots).", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMandatoryPropertyName", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

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

                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ERROR:No object has been defined in the Defaults XML file for:" & v_sObjectName & ". Note this is required in order for QMM Cobol Linkage to be correctly populated with 'real' instances only and not object instances that are defaults (or slots).", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'We set this object to be able to get the Mandatory property attribute
                oObject = oElements.Item(0)

                'Check there is a mandatory property defined for this object in the Defaults
                'xml file - if none, error and exit

                Dim auxVar As Object = oObject.GetAttribute(GISSharedConstants.GISDefaultsMandatoryProperty)


                If Convert.IsDBNull(auxVar) Or IsNothing(auxVar) Or CStr(oObject.GetAttribute(GISSharedConstants.GISDefaultsMandatoryProperty)).Trim() = "" Then
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ERROR:No mandatory property has been defined in the Defaults XML file for the object:" & v_sObjectName, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Get and return the mandatory property

                r_sMandatoryPropertyName = CStr(oObject.GetAttribute(GISSharedConstants.GISDefaultsMandatoryProperty))

                'Release resources
                oElements = Nothing
                oObject = Nothing

                Return result

            Catch excep As System.Exception



                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMandatoryPropertyNameFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMandatoryPropertyName", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

                Return result

            End Try
        End Function

    End Class
End Namespace

