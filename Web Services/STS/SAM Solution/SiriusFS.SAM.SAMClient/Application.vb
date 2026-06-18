Option Strict On
Option Explicit On 

Namespace DataSetControl
Public Class Application 
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
        ' ***************************************************************** '


        ' Constant for the functions to identify which class this is.
Private Const ACClass As String = "Application" 

        ' PUBLIC Data Members (Begin)
        ' PUBLIC Data Members (End)


        ' PRIVATE Data Members (Begin)


        ' The Output Structure Definition
        Private m_oDataSetDef As MSXML2.DOMDocument40
        ' The Data Set
        Private m_oDataset As MSXML2.DOMDocument40

        ' Used to apply defaults to the dataset - CJB 280301
        Private m_oDefaults As MSXML2.DOMDocument40

        ' Temporary Arrays which are used to load up the Object/Property Definitions
        Private m_vTempObjectArray() As Object
        Private m_vTempPropertyArray() As Object

        ' Temporary String used to build up the Data Set DTD
        Private m_sTempObjectDTD As String
        Private m_sTempAttribDTD As String

        'Private m_oNode As cGISDataSetControl.Node

        ' Hold the Object Instance Number outside of the XML
        Private m_lNextOINumber As Integer

        ' PRIVATE Data Members (End)

        ' PUBLIC Property Procedures (Begin)
        Public ReadOnly Property GISDataModelCode() As String
            Get
                If (m_oDataSetDef Is Nothing = True) Then
                    GISDataModelCode = ""
                Else
                    'UPGRADE_WARNING: Couldn't resolve default property of object m_oDataSetDef.documentElement.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    GISDataModelCode = CType(m_oDataSetDef.documentElement.getAttribute(GISXMLAttribDataModelCode), String)
                End If
            End Get
        End Property

        ' RDC 30072001 timestamp attribute determines if client/server files are in-sync
        Public ReadOnly Property GISDSDTimestamp() As String
            Get

                On Error GoTo Err_GISDSDTimestamp

                If (m_oDataSetDef Is Nothing = True) Then
                    GISDSDTimestamp = ""
                Else
                    'UPGRADE_WARNING: Couldn't resolve default property of object m_oDataSetDef.documentElement.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    GISDSDTimestamp = CType(m_oDataSetDef.documentElement.getAttribute(GISXMLAttribDataSetDefTimestamp), String)
                End If

                Exit Property

Err_GISDSDTimestamp:

                GISDSDTimestamp = ""

            End Get
        End Property

        ' RFC090300 - Scripting Method Access Added
        Public ReadOnly Property Risk() As DataSetControl.Node
            Get

                On Error GoTo Err_Risk

                ' Set the Root to the RISK_OBJECTS
                'SetRoot

                ' Return the Node so that an Item call can be done from there
                Risk = SetRoot()

                Exit Property

Err_Risk:

                Err.Raise(Err.Number, Err.Source, Err.Description)

                Exit Property

            End Get
        End Property

        Public ReadOnly Property Quote(ByVal v_lQuoteNum As Integer) As DataSetControl.Node
            Get

                On Error GoTo Err_Quote

                ' Set the Root to the Quote Number Specified
                'SetRoot v_lQuoteNum

                ' Return the Node so that an Item call can be done from there
                Quote = SetRoot(v_lQuoteNum)

                Exit Property

Err_Quote:

                Err.Raise(Err.Number, Err.Source, Err.Description)

                Exit Property

            End Get
        End Property

        Public ReadOnly Property QuoteCount() As Integer
            Get

                On Error GoTo Err_QuoteCount

                ' Return the Number of Quote Objects Nodes in the Document
                QuoteCount = m_oDataset.documentElement.getElementsByTagName(ACXMLQuoteObjects).length

                Exit Property

Err_QuoteCount:

                Err.Raise(Err.Number, Err.Source, Err.Description)

                Exit Property


            End Get
        End Property


        'RFC110501 - Next Object Instance Number
        ' This is needed so that when
        Private Property NextOINumber() As Integer
            Get
                NextOINumber = m_lNextOINumber
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
        Friend ReadOnly Property DatasetXMLDoc() As MSXML2.DOMDocument40
            Get
                DatasetXMLDoc = m_oDataset
            End Get
        End Property

        ' FRIEND Property Procedures (End)

        ' PUBLIC Methods (Begin)

        ' ***************************************************************** '
        ' Name: LoadFromXMLFile
        '
        ' Description: Load the Data Set Definition and Data Set from
        '              existing files.
        '
        ' ***************************************************************** '
        Public Function LoadFromXMLFile( _
            ByRef v_sDataSetDefFile As String, _
            ByRef v_sDataSetFile As String) As Integer

            Dim bLoaded As Boolean

            On Error GoTo Err_LoadFromXMLFile

            LoadFromXMLFile = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise Data Model Definition
            m_oDataSetDef = New MSXML2.DOMDocument40()
            m_oDataset = New MSXML2.DOMDocument40()

            ' Use the new parser
            Call m_oDataSetDef.setProperty("NewParser", True)
            Call m_oDataset.setProperty("NewParser", True)

            m_oDataSetDef.validateOnParse = False
            m_oDataSetDef.preserveWhiteSpace = False
            ' There is no DTD for the Data Set Def
            m_oDataSetDef.resolveExternals = False

            bLoaded = m_oDataSetDef.load(v_sDataSetDefFile)
            If (bLoaded = False) Then
                LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Load Data Set Definition from File : " & v_sDataSetDefFile, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXMLFile")
                LoadFromXMLFile = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_oDataset.validateOnParse = False
            m_oDataset.preserveWhiteSpace = False
            ' There IS an DTD for the Data Set, but it is Internal
            m_oDataset.resolveExternals = False

            bLoaded = m_oDataset.load(v_sDataSetFile)
            If (bLoaded = False) Then
                LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Load Data Set from File : " & v_sDataSetFile, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXMLFile")
                LoadFromXMLFile = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' Set the Next Object Instance Number
            'UPGRADE_WARNING: Couldn't resolve default property of object m_oDataset.documentElement.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            NextOINumber = CType(m_oDataset.documentElement.getAttribute(ACXMLAttribNextOINumber), Integer)

            Exit Function

Err_LoadFromXMLFile:

            LoadFromXMLFile = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadFromXMLFileFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXMLFile", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

        End Function


        ' ***************************************************************** '
        ' Name: LoadFromXML
        '
        ' Description: Loads the Data Set & Data Set Definition from the
        '              XML strings supplied.
        '
        ' ***************************************************************** '
        Public Function LoadFromXML(ByRef v_sXMLDataSetDef As String, ByRef v_sXMLDataSet As String) As Integer

            Dim bLoaded As Boolean

            On Error GoTo Err_LoadFromXML

            LoadFromXML = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise Data Model Definition
            m_oDataSetDef = New MSXML2.DOMDocument40()
            m_oDataset = New MSXML2.DOMDocument40()

            ' Use the new parser
            Call m_oDataSetDef.setProperty("NewParser", True)
            Call m_oDataset.setProperty("NewParser", True)

            m_oDataSetDef.validateOnParse = False
            m_oDataSetDef.preserveWhiteSpace = False
            ' There is no DTD for the Data Set Def
            m_oDataSetDef.resolveExternals = False
            bLoaded = m_oDataSetDef.loadXML(v_sXMLDataSetDef)
            If (bLoaded = False) Then
                LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Load Data Set Definition from XML String", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXML")
                LoadFromXML = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_oDataset.validateOnParse = False
            m_oDataset.preserveWhiteSpace = False
            ' There IS an DTD for the Data Set, but it is Internal
            m_oDataset.resolveExternals = False
            bLoaded = m_oDataset.loadXML(v_sXMLDataSet)
            If (bLoaded = False) Then
                LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Load Data Set from XML String", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXML")
                LoadFromXML = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' Set the Next Object Instance Number
            'UPGRADE_WARNING: Couldn't resolve default property of object m_oDataset.documentElement.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            NextOINumber = CType(m_oDataset.documentElement.getAttribute(ACXMLAttribNextOINumber), Integer)

            Exit Function

Err_LoadFromXML:

            LoadFromXML = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadFromXMLFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXML", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

        End Function

        ' ***************************************************************** '
        ' Name: LoadFromXML
        '
        ' Description: Loads the Data Set & Data Set Definition from the
        '              XML strings supplied.
        '
        ' ***************************************************************** '
        Public Function LoadFromXMLDataset(ByVal v_sGISDataModelCode As String, ByRef v_sXMLDataSet As String) As Integer

            Dim bLoaded As Boolean
            Dim iRet As Int32
            Dim sDataSetFile As String
            Dim sDataSetDefFile As String

            On Error GoTo Err_LoadFromXMLDataset

            LoadFromXMLDataset = gPMConstants.PMEReturnCode.PMTrue

            iRet = GetDataSetFileNames(v_sGISDataModelCode, sDataSetDefFile, sDataSetFile)
            If (iRet <> gPMConstants.PMEReturnCode.PMTrue) Then
                LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Get Data Set FileNames", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXMLDataset")
                LoadFromXMLDataset = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            iRet = LoadFromXMLFile(sDataSetDefFile, sDataSetFile)
            If (iRet <> gPMConstants.PMEReturnCode.PMTrue) Then
                LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Load From XML File.", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXMLDataset")
                LoadFromXMLDataset = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            iRet = UpdateDataSetFromXML(v_sXMLDataSet)
            If (iRet <> gPMConstants.PMEReturnCode.PMTrue) Then
                LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Update XML Dataset.", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXMLDataset")
                LoadFromXMLDataset = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            Exit Function

Err_LoadFromXMLDataset:

            LoadFromXMLDataset = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadFromXMLDatasetFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXMLDataset", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

        End Function

        ' ***************************************************************** '
        ' Name: UpdateDataSetFromXML
        '
        ' Description: Updates the DataSet from the Supplied XML.
        '
        ' ***************************************************************** '
        Private Function UpdateDataSetFromXML(ByRef v_sXMLDataSet As String) As Integer

            Dim bLoaded As Boolean
            Dim sDataModelCode As String
            Dim oNewDataSet As MSXML2.DOMDocument40

            On Error GoTo Err_UpdateDataSetFromXML

            UpdateDataSetFromXML = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise a New DataSet
            oNewDataSet = New MSXML2.DOMDocument40()
            ' Use the new parser
            Call oNewDataSet.setProperty("NewParser", True)

            ' Load the Data Set
            oNewDataSet.validateOnParse = False
            bLoaded = oNewDataSet.loadXML(v_sXMLDataSet)
            If (bLoaded = False) Then
                LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Load Data Set from XML String", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateDataSetFromXML")
                UpdateDataSetFromXML = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' Get the Data Model Code Attribute
            'UPGRADE_WARNING: Couldn't resolve default property of object oNewDataSet.documentElement.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            sDataModelCode = CType(oNewDataSet.documentElement.getAttribute(GISXMLAttribDataModelCode), String)

            ' If the Data Model Code from the Data Set MATCHES that
            ' from the currently loaded Data Set Definition then
            ' replace the currently loaded data set with the New Data Set
            ' otherwise Error.
            If (UCase(sDataModelCode) = UCase(GISDataModelCode)) Then
                ' CodesMatch so replace the Current Data Set
                'UPGRADE_NOTE: Object m_oDataset may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
                m_oDataset = Nothing
                m_oDataset = oNewDataSet
                'UPGRADE_NOTE: Object oNewDataSet may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
                oNewDataSet = Nothing
            Else
                ' Codes do NOT Match so Error
                LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Data Model Code from Data Set does not match that of the Data Set Definition.", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateDataSetFromXML")
                'UPGRADE_NOTE: Object oNewDataSet may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
                oNewDataSet = Nothing
                UpdateDataSetFromXML = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' Set the Next Object Instance Number
            'UPGRADE_WARNING: Couldn't resolve default property of object m_oDataset.documentElement.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            If (IsDBNull(m_oDataset.documentElement.getAttribute(ACXMLAttribNextOINumber)) = False) Then
                NextOINumber = CType(m_oDataset.documentElement.getAttribute(ACXMLAttribNextOINumber), Integer)
            Else
                NextOINumber = 1
            End If

            Exit Function

Err_UpdateDataSetFromXML:

            UpdateDataSetFromXML = gPMConstants.PMEReturnCode.PMError

            'UPGRADE_NOTE: Object oNewDataSet may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oNewDataSet = Nothing

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateDataSetFromXMLFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateDataSetFromXML", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

        End Function

        Public Function ReturnAsXML(ByRef r_sXMLDataSet As String) As Integer

            Dim sDummy As String
            
            Return Me.ReturnAsXML(r_sXMLDataSetDef:=sDummy, _
                                  r_sXMLDataSet:=r_sXMLDataSet)

        End Function

        ' ***************************************************************** '
        ' Name: ReturnAsXML
        '
        ' Description: Return the Data Set and Data Set Definition as
        '              XML Strings.
        '
        ' ***************************************************************** '
        Public Function ReturnAsXML(ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataSet As String) As Integer

            On Error GoTo Err_ReturnAsXML

            ReturnAsXML = gPMConstants.PMEReturnCode.PMTrue

            r_sXMLDataSetDef = ""
            r_sXMLDataSet = ""

            r_sXMLDataSetDef = m_oDataSetDef.xml

            ' Update the Next Object Instance number in the XML before it is returned
            m_oDataset.documentElement.setAttribute(ACXMLAttribNextOINumber, m_lNextOINumber)


            r_sXMLDataSet = m_oDataset.xml

            Exit Function

Err_ReturnAsXML:

            ReturnAsXML = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReturnAsXMLFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReturnAsXML", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

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

            Dim oQuotes As MSXML2.IXMLDOMElement
            Dim oQuoteObjects As MSXML2.IXMLDOMElement
            Dim sOutObjsOIKey As String
            Dim lNextQuoteNum As String

            On Error GoTo Err_NewQuoteOutput

            NewQuoteOutput = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Quotes Instance to the Quotes Node
            oQuotes = CType(GetObjectInstance(ACXMLQuotes), MSXML2.IXMLDOMElement)
            If (oQuotes Is Nothing = True) Then
                NewQuoteOutput = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' Get the Next Quote Number
            'UPGRADE_WARNING: Couldn't resolve default property of object oQuotes.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            lNextQuoteNum = CType(oQuotes.getAttribute(ACXMLAttribNextQuoteNum), String)
            If (CDbl(lNextQuoteNum) < 1) Then
                lNextQuoteNum = CStr(1)
            End If

            ' Create the Quote Objects Node
            oQuoteObjects = m_oDataset.createElement(ACXMLQuoteObjects)

            ' Set the Scheme Out Attributes
            oQuoteObjects.setAttribute(ACXMLAttribInsurer, v_sInsurer)
            oQuoteObjects.setAttribute(ACXMLAttribInsurerID, v_lInsurerID)
            oQuoteObjects.setAttribute(ACXMLAttribScheme, v_sScheme)
            oQuoteObjects.setAttribute(ACXMLAttribSchemeID, v_lSchemeID)
            oQuoteObjects.setAttribute(ACXMLAttribSchemeVer, v_lSchemeVer)

            ' Build the Quote Key from a combination of the
            ' Quote Engine Mapper Number and Quote Number
            v_lQEMNumber = v_lQEMNumber * 1000000
            r_sQuoteKey = ACQuotePrefix & CStr(v_lQEMNumber + CDbl(lNextQuoteNum))

            ' Set the Quote Objects Key
            oQuoteObjects.setAttribute(ACXMLAttribOIKey, r_sQuoteKey)

            ' Set the Next Quote Number Attribute
            lNextQuoteNum = CStr(CDbl(lNextQuoteNum) + 1)
            oQuotes.setAttribute(ACXMLAttribNextQuoteNum, lNextQuoteNum)

            ' Append the Quote Objects to the Quotes Node
            'UPGRADE_WARNING: Couldn't resolve default property of object oQuoteObjects. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            oQuotes.appendChild(oQuoteObjects)

            ' Release Local References
            'UPGRADE_NOTE: Object oQuotes may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oQuotes = Nothing
            'UPGRADE_NOTE: Object oQuoteObjects may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oQuoteObjects = Nothing

            Exit Function

Err_NewQuoteOutput:

            NewQuoteOutput = gPMConstants.PMEReturnCode.PMError

            'UPGRADE_NOTE: Object oQuotes may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oQuotes = Nothing
            'UPGRADE_NOTE: Object oQuotes may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oQuotes = Nothing
            'UPGRADE_NOTE: Object oQuoteObjects may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oQuoteObjects = Nothing

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewQuoteOutputFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewQuoteOutput", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

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

            Dim oQuotes As MSXML2.IXMLDOMElement
            Dim oQuoteObjects As MSXML2.IXMLDOMElement
            Dim sOutObjsOIKey As String
            Dim lNextQuoteNum As String

            Dim lReturn As Integer
            Dim sDataModelCode As String
            Dim sTopQuoteObject As String
            Dim sTopQuoteTable As String

            On Error GoTo Err_NewQuoteOutputSaveDB

            NewQuoteOutputSaveDB = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Quotes Instance to the Quotes Node
            oQuotes = CType(GetObjectInstance(ACXMLQuotes), MSXML2.IXMLDOMElement)
            If (oQuotes Is Nothing = True) Then
                NewQuoteOutputSaveDB = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' Get the Next Quote Number
            'UPGRADE_WARNING: Couldn't resolve default property of object oQuotes.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            lNextQuoteNum = CType(oQuotes.getAttribute(ACXMLAttribNextQuoteNum), String)
            If (CDbl(lNextQuoteNum) < 1) Then
                lNextQuoteNum = CStr(1)
            End If

            ' Create the Quote Objects Node
            oQuoteObjects = m_oDataset.createElement(ACXMLQuoteObjects)

            oQuoteObjects.setAttribute(ACXMLAttribSchemeID, v_lGISSchemeID)

            ' Build the Quote Key from the GISSchemeID
            r_sQuoteKey = ACQuotePrefix & CStr(v_lGISSchemeID)

            ' Set the Quote Objects Key
            oQuoteObjects.setAttribute(ACXMLAttribOIKey, r_sQuoteKey)

            ' Set the Next Quote Number Attribute
            lNextQuoteNum = CStr(CDbl(lNextQuoteNum) + 1)
            oQuotes.setAttribute(ACXMLAttribNextQuoteNum, lNextQuoteNum)

            ' Append the Quote Objects to the Quotes Node
            'UPGRADE_WARNING: Couldn't resolve default property of object oQuoteObjects. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            oQuotes.appendChild(oQuoteObjects)

            ' Release Local References
            'UPGRADE_NOTE: Object oQuotes may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oQuotes = Nothing
            'UPGRADE_NOTE: Object oQuoteObjects may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oQuoteObjects = Nothing

            ' Get the Data Model Code
            sDataModelCode = GISDataModelCode
            If (Trim(sDataModelCode) = "") Then
                LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Data Model Code is blank. Need it to get SaveToDBMode from registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="NewQuoteOutputSaveDB")
            End If

            ' Get the Top Level Quote Object
            lReturn = GetTopLevelQuoteObject(r_sObjectName:=sTopQuoteObject, r_sTableName:=sTopQuoteTable)
            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                NewQuoteOutputSaveDB = lReturn
                Exit Function
            End If

            ' Yes, so we need to create an instance of the Quote Top Level Object
            lReturn = NewObjectInstance(v_sObjectName:=sTopQuoteObject, r_sOIKey:=r_sTopQuoteOIKey, v_sQuoteKey:=r_sQuoteKey)
            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                NewQuoteOutputSaveDB = lReturn
                Exit Function
            End If

            ' Set the Policy Link ID
            lReturn = SetPropertyValue(v_sObjectName:=sTopQuoteObject, v_sPropertyName:="GIS_POLICY_LINK_ID", v_sOIKey:=r_sTopQuoteOIKey, v_vPropertyValue:=PolicyLinkID, v_bIsAssumedInfo:=False, v_bLoadedFromDB:=True)
            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                NewQuoteOutputSaveDB = lReturn
                Exit Function
            End If
            Dim lGISSchemeID As Object = v_lGISSchemeID
            ' Set the Scheme ID
            lReturn = SetPropertyValue(v_sObjectName:=sTopQuoteObject, v_sPropertyName:="GIS_SCHEME_ID", v_sOIKey:=r_sTopQuoteOIKey, v_vPropertyValue:=lGISSchemeID, v_bIsAssumedInfo:=False, v_bLoadedFromDB:=True)
            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                NewQuoteOutputSaveDB = lReturn
                Exit Function
            End If
            v_lGISSchemeID = CInt(lGISSchemeID)

            Exit Function

Err_NewQuoteOutputSaveDB:

            NewQuoteOutputSaveDB = gPMConstants.PMEReturnCode.PMError

            'UPGRADE_NOTE: Object oQuotes may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oQuotes = Nothing
            'UPGRADE_NOTE: Object oQuotes may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oQuotes = Nothing
            'UPGRADE_NOTE: Object oQuoteObjects may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oQuoteObjects = Nothing

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewQuoteOutputSaveDBFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewQuoteOutputSaveDB", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

        End Function

        ' ***************************************************************** '
        ' Name: ClearAllQuoteOutput
        '
        ' Description: Deletes and Recreates the Quote Output node.
        '
        ' ***************************************************************** '
        Public Function ClearAllQuoteOutput() As Integer

            Dim lReturn As Integer
            Dim oQuotesElem As MSXML2.IXMLDOMElement
            Dim lSaveToDBMode As Integer

            Dim lQuoteCount As Integer
            Dim lQuote As Integer

            Dim sTopQuoteObject As String
            Dim sTopQuoteTable As String

            On Error GoTo Err_ClearAllQuoteOutput

            ClearAllQuoteOutput = gPMConstants.PMEReturnCode.PMTrue

            ' Delete Any Previous of the Quotes Area
            ' Note: Do not need to check return code,
            ' as we do not care if it was not already there.
            lReturn = DelObjectInstance(v_sObjectName:=ACXMLQuotes, v_sOIKey:=ACXMLQuotes)

            ' Create the Quotes Element
            oQuotesElem = m_oDataset.createElement(ACXMLQuotes)

            ' Set the Next Quote Number Attribute
            oQuotesElem.setAttribute(ACXMLAttribNextQuoteNum, 1)
            ' Set the OI Key
            oQuotesElem.setAttribute(ACXMLAttribOIKey, ACXMLQuotes)

            ' Get the Save to DB Mode
            '        lSaveToDBMode = GetLoadSaveDBMode(GISDataModelCode)
            ' If we are saving Quotes in the Database
            '        If (lSaveToDBMode = GISRegLoadSaveDBModeFastWithQuotes) Then
            ' RFC170501 - Use the Clear Quotes Attribute to mark that we need to execute the
            '             clear quote output sp at SavetoDBTime
            oQuotesElem.setAttribute(ACXMLAttribClearQuotes, gPMConstants.PMEReturnCode.PMTrue)
            '       End If

            ' Append Quotes as a Child of the Data Set element
            'UPGRADE_WARNING: Couldn't resolve default property of object oQuotesElem. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            m_oDataset.documentElement.appendChild(oQuotesElem)

            'UPGRADE_NOTE: Object oQuotesElem may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oQuotesElem = Nothing

            Exit Function

Err_ClearAllQuoteOutput:

            'UPGRADE_NOTE: Object oQuotesElem may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oQuotesElem = Nothing

            ClearAllQuoteOutput = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ClearAllQuoteOutputFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearAllQuoteOutput", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

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

            Dim lReturn As Integer

            Dim oParentInst As MSXML2.IXMLDOMElement
            Dim oNewInst As MSXML2.IXMLDOMElement
            'Dim vNextChildNum As Variant
            'Dim lNewChildNum As Long
            Dim sMsg As String
            Dim lIsQuoteObject As Integer
            Dim sTableName As String
            Dim sPKColName As String
            Dim sColumnName As String
            Dim sPropertyName As String
            Dim vPropertyArray As Object
            Dim vPropertyValue As Object
            Dim iIsPrimaryKey As Short
            Dim lRow As Integer
            Dim bIsAssumedInfo As Boolean
            Dim lOINumber As Integer
            Dim sParentObject As String

            On Error GoTo Err_NewObjectInstance

            NewObjectInstance = gPMConstants.PMEReturnCode.PMTrue

            ' Increment the Next Instance Number
            NextOINumber = NextOINumber + 1

            lOINumber = NextOINumber

            ' If we are Loading this Object From the Database
            If (v_bLoadedFromDB = True) Then

                ' Then the Object Instance Number Should have been supplied
                If (v_lOINumber < 1) Then
                    LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Object Instance Number MUST be supplied to load from Database.", vApp:=ACApp, vClass:=ACClass, vMethod:="NewObjectInstance")
                    NewObjectInstance = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                ' Use the Object Instance Number Supplied
                lOINumber = v_lOINumber

                ' If the Supplied Object Instance Number is greater than or equal to
                ' the Next Object Instance Num then set the Next Instance Num
                ' to the Supplied Instance Num plus one.
                If (v_lOINumber >= NextOINumber) Then
                    NextOINumber = v_lOINumber + 1
                End If

            End If

            If (Trim(v_sParentOIKey) <> "") Then

                ' Get the Parent Instance
                oParentInst = CType(GetObjectInstance(v_sParentOIKey), MSXML2.IXMLDOMElement)
                ' If we did NOT find the Parent Instance then Error
                If (oParentInst Is Nothing = True) Then
                    NewObjectInstance = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

            ElseIf (Trim(v_sQuoteKey) = "") Then

                ' Set the Parent Instance to the Risk Objects Node
                oParentInst = CType(GetObjectInstance(ACXMLRiskObjects), MSXML2.IXMLDOMElement)
                If (oParentInst Is Nothing = True) Then
                    NewObjectInstance = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

            Else

                ' Set the Parent Instance to the Quote Node
                oParentInst = CType(GetObjectInstance(v_sQuoteKey), MSXML2.IXMLDOMElement)
                If (oParentInst Is Nothing = True) Then
                    ' Scheme Key is Invalid
                    LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Scheme Key " & v_sQuoteKey & " is NOT valid.", vApp:=ACApp, vClass:=ACClass, vMethod:="NewObjectInstance")
                    NewObjectInstance = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

            End If

            ' Create a New Element
            oNewInst = m_oDataset.createElement(Trim(UCase(v_sObjectName)))

            ' Generate the Key for the NEW Object Instance

            ' If we are Creating a New Quote Object
            If (lIsQuoteObject = gPMConstants.PMEReturnCode.PMTrue) Then

                ' Generate the Key for the QuoteObject (Prefixed with Quote Key)
                r_sOIKey = BuildOIKey(v_sObjectName, lOINumber, v_sQuoteKey)

            Else

                ' Calc the Risk Object Instance Key
                r_sOIKey = BuildOIKey(v_sObjectName, lOINumber)

            End If

            ' Set the Object Instance Attributes
            oNewInst.setAttribute(ACXMLAttribOIKey, r_sOIKey)

            ' Are we loading this from the Database
            If (v_bLoadedFromDB = True) Then
                ' Yes, so it doesn't need to be added/updated
                oNewInst.setAttribute(ACXMLAttribUpdateStatus, gPMConstants.PMEComponentAction.PMView)
            Else
                ' No, it is new so it needs to be added.
                oNewInst.setAttribute(ACXMLAttribUpdateStatus, gPMConstants.PMEComponentAction.PMAdd)
            End If

            ' Add the New Instance as a Child of the Parent
            'UPGRADE_WARNING: Couldn't resolve default property of object oNewInst. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            oParentInst.appendChild(oNewInst)

            'UPGRADE_NOTE: Object oParentInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oParentInst = Nothing
            'UPGRADE_NOTE: Object oNewInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oNewInst = Nothing

            Exit Function

Err_NewObjectInstance:

            NewObjectInstance = gPMConstants.PMEReturnCode.PMError

            'UPGRADE_NOTE: Object oParentInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oParentInst = Nothing
            'UPGRADE_NOTE: Object oNewInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oNewInst = Nothing

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewObjectInstanceFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewObjectInstance", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

        End Function

        ' ***************************************************************** '
        ' Name: DelObjectInstance
        '
        ' Description: Delete an Instance of the Specified Object.
        '              If the Object has Child Object Instances
        '              they will be deleted also.
        ' ***************************************************************** '
        Public Function DelObjectInstance(ByRef v_sObjectName As String, ByRef v_sOIKey As String) As Integer

            Dim lReturn As Integer
            Dim oObjInst As MSXML2.IXMLDOMElement
            Dim oParentInst As MSXML2.IXMLDOMNode
            Dim oDelObjs As MSXML2.IXMLDOMNode
            Dim lUpdateStatus As Integer
            Dim vUpdateStatus As Object

            On Error GoTo Err_DelObjectInstance

            DelObjectInstance = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Object Instance
            oObjInst = CType(GetObjectInstance(v_sOIKey), MSXML2.IXMLDOMElement)
            ' If we cannot find it, Return Not Found
            If (oObjInst Is Nothing = True) Then
                DelObjectInstance = gPMConstants.PMEReturnCode.PMNotFound
                Exit Function
            End If

            ' Get the Parent Object Instance
            oParentInst = oObjInst.parentNode

            ' Remove the Object Instance from the Parent
            'UPGRADE_WARNING: Couldn't resolve default property of object oObjInst. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            oParentInst.removeChild(oObjInst)

            'RFC200400 - Add Proper Delete Functionality
            'UPGRADE_WARNING: Couldn't resolve default property of object oObjInst.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            'UPGRADE_WARNING: Couldn't resolve default property of object vUpdateStatus. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            vUpdateStatus = oObjInst.getAttribute(ACXMLAttribUpdateStatus)

            ' Some Object Instances (Like Quotes) do not have an Update Status
            ' need to trap that.
            If (IsNumeric(vUpdateStatus) = True) Then
                'UPGRADE_WARNING: Couldn't resolve default property of object vUpdateStatus. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                lUpdateStatus = CInt(vUpdateStatus)
            Else
                lUpdateStatus = gPMConstants.PMEComponentAction.PMAdd
            End If

            ' If this Object Instance Needs to be Added to the Database
            If (lUpdateStatus = gPMConstants.PMEComponentAction.PMAdd) Then

                ' Then we do not need to delete it from the database

            Else

                ' Add the Object to the Deleted Objects Element

                ' Get the Deleted Objects Node
                oDelObjs = GetObjectInstance(ACXMLDeletedObjects)

                ' Add the Deleted Object as a Parent of the Deleted Objects Element
                'UPGRADE_WARNING: Couldn't resolve default property of object oObjInst. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                oDelObjs.appendChild(oObjInst)

                ' Set the update status to Delete
                oObjInst.setAttribute(ACXMLAttribUpdateStatus, gPMConstants.PMEComponentAction.PMDelete)

            End If

            'UPGRADE_NOTE: Object oObjInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oObjInst = Nothing
            'UPGRADE_NOTE: Object oParentInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oParentInst = Nothing
            'UPGRADE_NOTE: Object oDelObjs may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oDelObjs = Nothing

            Exit Function

Err_DelObjectInstance:

            DelObjectInstance = gPMConstants.PMEReturnCode.PMError

            'UPGRADE_NOTE: Object oObjInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oObjInst = Nothing
            'UPGRADE_NOTE: Object oParentInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oParentInst = Nothing

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DelObjectInstanceFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="DelObjectInstance", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

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
        Public Function SetPropertyValue(ByRef v_sObjectName As String, ByRef v_sPropertyName As String, ByRef v_sOIKey As String, ByRef v_vPropertyValue As Object, ByRef v_bIsAssumedInfo As Boolean, Optional ByRef v_bLoadedFromDB As Boolean = False) As Integer

            Dim lReturn As Integer
            Dim oObjectInst As MSXML2.IXMLDOMElement
            Dim lObjectUpdateStatus As Integer
            Dim lUpdateStatus As Integer
            Dim sPropertyTag As String

            Dim lAssumedInfo As Integer

            On Error GoTo Err_SetPropertyValue

            SetPropertyValue = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Object Instance it Belongs to
            oObjectInst = CType(GetObjectInstance(v_sOIKey), MSXML2.IXMLDOMElement)
            If (oObjectInst Is Nothing = True) Then
                SetPropertyValue = gPMConstants.PMEReturnCode.PMNotFound
                Exit Function
            End If

            PropertyValueSet(v_sObjectName:=v_sObjectName, r_oObjectInst:=oObjectInst, v_sPropertyName:=v_sPropertyName, v_vPropertyValue:=v_vPropertyValue, v_bLoadedFromDB:=v_bLoadedFromDB)

            'UPGRADE_NOTE: Object oObjectInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oObjectInst = Nothing

            Exit Function

Err_SetPropertyValue:

            SetPropertyValue = gPMConstants.PMEReturnCode.PMError

            'UPGRADE_NOTE: Object oObjectInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oObjectInst = Nothing

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetPropertyValueFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPropertyValue", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

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

            Dim lReturn As Integer
            Dim oObjectInst As MSXML2.IXMLDOMElement
            Dim vAssumedInfo As Object
            Dim sPropertyTag As String

            On Error GoTo Err_GetPropertyValue

            GetPropertyValue = gPMConstants.PMEReturnCode.PMTrue

            ' Assumed information is not supported in this version of the Dataset control
            r_bIsAssumedInfo = False

            ' Get the Object Instance
            oObjectInst = CType(GetObjectInstance(v_sOIKey), MSXML2.IXMLDOMElement)

            If (oObjectInst Is Nothing = True) Then
                ' Object Does NOT Exist, Return Not Found
                GetPropertyValue = gPMConstants.PMEReturnCode.PMNotFound
                Exit Function
            End If

            ' Get the Property Value
            'UPGRADE_WARNING: Couldn't resolve default property of object PropertyValue(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            'UPGRADE_WARNING: Couldn't resolve default property of object r_vPropertyValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            r_vPropertyValue = PropertyValue(oObjectInst, v_sPropertyName)

            'UPGRADE_NOTE: Object oObjectInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oObjectInst = Nothing

            Exit Function

Err_GetPropertyValue:

            GetPropertyValue = gPMConstants.PMEReturnCode.PMError

            'UPGRADE_NOTE: Object oObjectInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oObjectInst = Nothing

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPropertyValueFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPropertyValue", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

        End Function

        ' ***************************************************************** '
        ' Name: ResetUpdateStatus
        '
        ' Description:
        '
        '
        ' ***************************************************************** '
        Private Function ResetUpdateStatus(ByRef v_sObjectName As String, ByRef v_sOIKey As String) As Integer

            Dim lReturn As Integer
            Dim oObjectInst As MSXML2.IXMLDOMElement
            Dim oParentInst As MSXML2.IXMLDOMElement
            Dim lUpdateStatus As Integer

            On Error GoTo Err_ResetUpdateStatus

            ResetUpdateStatus = gPMConstants.PMEReturnCode.PMTrue

            ' Get a Reference to the Instance
            oObjectInst = CType(GetObjectInstance(v_sOIKey:=v_sOIKey), MSXML2.IXMLDOMElement)
            If (oObjectInst Is Nothing = True) Then
                ResetUpdateStatus = gPMConstants.PMEReturnCode.PMNotFound
                Exit Function
            End If

            ' Get the objects Update Status
            'UPGRADE_WARNING: Couldn't resolve default property of object oObjectInst.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            lUpdateStatus = CType(oObjectInst.getAttribute(ACXMLAttribUpdateStatus), Integer)

            ' If this is a Deleted Instance
            If (lUpdateStatus = gPMConstants.PMEComponentAction.PMDelete) Then

                ' Then Delete it from its Parent i.e the Deleted Objects Node
                oParentInst = CType(oObjectInst.parentNode, MSXML2.IXMLDOMElement)

                'UPGRADE_WARNING: Couldn't resolve default property of object oObjectInst. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                oParentInst.removeChild(oObjectInst)

            Else

                ' Set the Objects Update Status to View
                oObjectInst.setAttribute(ACXMLAttribUpdateStatus, gPMConstants.PMEComponentAction.PMView)

            End If

            'UPGRADE_NOTE: Object oObjectInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oObjectInst = Nothing
            'UPGRADE_NOTE: Object oParentInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oParentInst = Nothing

            Exit Function

Err_ResetUpdateStatus:

            ResetUpdateStatus = gPMConstants.PMEReturnCode.PMError

            'UPGRADE_NOTE: Object oObjectInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oObjectInst = Nothing
            'UPGRADE_NOTE: Object oParentInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oParentInst = Nothing

            '    Set oPropertyInst = Nothing

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ResetUpdateStatusFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ResetUpdateStatus", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

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
        Private Function ResetUpdateStatusWholeDataset() As Integer

            Dim oObjectInst As MSXML2.IXMLDOMElement
            Dim lPos As Integer
            Dim sXMLDS As String
            Dim sXMLDSD As String
            Dim lReturn As Integer

            On Error GoTo Err_ResetUpdateStatusWholeDataset

            ResetUpdateStatusWholeDataset = gPMConstants.PMEReturnCode.PMTrue

            ' Get a Reference to the Quotes Object
            oObjectInst = CType(GetObjectInstance(ACXMLQuotes), MSXML2.IXMLDOMElement)
            ' If we cannot find it, Return Not Found
            If (oObjectInst Is Nothing = True) Then
                ResetUpdateStatusWholeDataset = gPMConstants.PMEReturnCode.PMNotFound
                Exit Function
            End If

            ' Set the ClearAllQuoteOutput Attribute to False
            oObjectInst.setAttribute(ACXMLAttribClearQuotes, gPMConstants.PMEReturnCode.PMFalse)
            'UPGRADE_NOTE: Object oObjectInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oObjectInst = Nothing

            ' Get a Reference to the Deleted Objects
            oObjectInst = CType(GetObjectInstance(v_sOIKey:=ACXMLDeletedObjects), MSXML2.IXMLDOMElement)
            If (oObjectInst Is Nothing = True) Then
                ResetUpdateStatusWholeDataset = gPMConstants.PMEReturnCode.PMNotFound
                Exit Function
            End If

            ' Remove all of the Deleted Objects
            oObjectInst.text = ""
            oObjectInst.removeChild(oObjectInst.firstChild)

            ' Return the Data Set
            lReturn = ReturnAsXML(sXMLDSD, sXMLDS)
            If (oObjectInst Is Nothing = True) Then
                ResetUpdateStatusWholeDataset = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' Reset the Insert Flags to View
            sXMLDS = Replace(sXMLDS, "US=""1""", "US=""0""", , , CompareMethod.Text)
            ' Reset the Update Flage to View
            sXMLDS = Replace(sXMLDS, "US=""2""", "US=""0""", , , CompareMethod.Text)

            ' Load the Data Set Back Up Again
            lReturn = LoadFromXML(sXMLDSD, sXMLDS)
            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ResetUpdateStatusWholeDataset = lReturn
                Exit Function
            End If

            Exit Function

Err_ResetUpdateStatusWholeDataset:

            ResetUpdateStatusWholeDataset = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ResetUpdateStatusWholeDataset Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ResetUpdateStatusWholeDataset", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

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
        Public Function GetInstanceHierarchy( _
            ByRef v_sObjectName As String, _
            ByRef r_vObjectInstanceArray(,) As Object, _
            ByRef r_lMaxInstances As Integer) As Integer

            Dim oObjectDef As MSXML2.IXMLDOMElement
            Dim oInstances As MSXML2.IXMLDOMNodeList
            Dim oInstance As MSXML2.IXMLDOMElement
            Dim oParentInst As MSXML2.IXMLDOMElement

            Dim lNum As Integer
            Dim lInst As Integer
            Dim lRow As Integer
            Dim vIdValueArray(,) As Object
            Dim lReturn As Integer
            Dim oRoot As MSXML2.IXMLDOMElement
            Dim lIsQuoteObject As Integer

            On Error GoTo Err_GetInstanceHierarchy

            GetInstanceHierarchy = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Object Definition
            oObjectDef = CType(GetDefinitionNode(v_sObjectName), MSXML2.IXMLDOMElement)
            If (oObjectDef Is Nothing = True) Then
                GetInstanceHierarchy = gPMConstants.PMEReturnCode.PMInvalidRequest
                Exit Function
            End If

            ' Is this a Quote Object or a Risk Object
            'UPGRADE_WARNING: Couldn't resolve default property of object oObjectDef.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            lIsQuoteObject = CType(oObjectDef.getAttribute(ACXMLAttribIsQuoteObject), Integer)

            If (lIsQuoteObject = gPMConstants.PMEReturnCode.PMTrue) Then
                oRoot = CType(GetObjectInstance(ACXMLQuotes), MSXML2.IXMLDOMElement)
            Else
                oRoot = CType(GetObjectInstance(ACXMLRiskObjects), MSXML2.IXMLDOMElement)
            End If

            ' Return the Max Number of Instances that we can have of this Object
            'UPGRADE_WARNING: Couldn't resolve default property of object oObjectDef.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            r_lMaxInstances = CType(oObjectDef.getAttribute(ACXMLAttribMaxInstances), Integer)

            ' Loop Until we get back to the Risk/Quote Objects Root Level
            Do While ((oObjectDef.nodeName <> ACXMLRiskObjects) And (oObjectDef.nodeName <> ACXMLQuoteObjects))

                ' Get all Instances of this Object Name
                oInstances = oRoot.getElementsByTagName(oObjectDef.nodeName)

                ' Get the Number of Instances
                lNum = oInstances.length

                ' If there are Any Instances
                If (lNum > 0) Then

                    ' Resize the Array
                    If (IsArray(r_vObjectInstanceArray) = True) Then
                        lRow = UBound(r_vObjectInstanceArray, 2) + 1
                        ReDim Preserve r_vObjectInstanceArray(9, UBound(r_vObjectInstanceArray, 2) + lNum)
                    Else
                        lRow = 0
                        ReDim r_vObjectInstanceArray(9, lNum - 1)
                    End If

                    ' For Each Instance
                    For lInst = 0 To lNum - 1

                        'lRow = lRow + lInst

                        oInstance = CType(oInstances.item(lInst), MSXML2.IXMLDOMElement)

                        r_vObjectInstanceArray(GISHierColObjectName, lRow) = oObjectDef.nodeName
                        r_vObjectInstanceArray(GISHierColOIKey, lRow) = oInstance.getAttribute(ACXMLAttribOIKey)
                        r_vObjectInstanceArray(GISHierColChildNum, lRow) = lInst + 1

                        ' Get the Identifying Property Values for this Object Instance
                        lReturn = GetPropertyValues(v_oObjectDef:=oObjectDef, v_oObjectInst:=oInstance, r_vIdValueArray:=vIdValueArray)
                        If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                            r_vObjectInstanceArray(GISHierColIDName1, lRow) = Nothing
                            r_vObjectInstanceArray(GISHierColIDValue1, lRow) = Nothing
                            r_vObjectInstanceArray GISHierColIDName2, lRow)(=Nothing
                            r_vObjectInstanceArray(GISHierColIDValue2, lRow) = Nothing
                            r_vObjectInstanceArray(GISHierColIDName3, lRow) = Nothing
                            r_vObjectInstanceArray(GISHierColIDValue3, lRow) = Nothing
                        Else
                            r_vObjectInstanceArray(GISHierColIDName1, lRow) = vIdValueArray.GetValue(GISIDColPropName, 0)
                            r_vObjectInstanceArray(GISHierColIDValue1, lRow) = vIdValueArray.GetValue(GISIDColPropValue, 0)
                            r_vObjectInstanceArray(GISHierColIDName2, lRow) = vIdValueArray(GISIDColPropName, 1)
                            r_vObjectInstanceArray(GISHierColIDValue2, lRow) = vIdValueArray.GetValue(GISIDColPropValue, 1)
                            r_vObjectInstanceArray(GISHierColIDName3, lRow) = vIdValueArray(GISIDColPropName, 2)
                            r_vObjectInstanceArray(GISHierColIDValue3, lRow) = vIdValueArray(GISIDColPropValue, 2)

                        End If

                        oParentInst = CType(oInstance.parentNode, MSXML2.IXMLDOMElement)
                        If (oParentInst Is Nothing = True) Then
                            'UPGRADE_WARNING: Couldn't resolve default property of object r_vObjectInstanceArray(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                            r_vObjectInstanceArray(GISHierColParentOIKey, lRow) = ""
                        Else
                            If ((oParentInst.nodeName = ACXMLRiskObjects) Or (oParentInst.nodeName = ACXMLQuoteObjects)) Then
                                'UPGRADE_WARNING: Couldn't resolve default property of object r_vObjectInstanceArray(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                                r_vObjectInstanceArray(GISHierColParentOIKey, lRow) = ""
                            Else
                                'UPGRADE_WARNING: Couldn't resolve default property of object oParentInst.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                                'UPGRADE_WARNING: Couldn't resolve default property of object r_vObjectInstanceArray(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                                r_vObjectInstanceArray(GISHierColParentOIKey, lRow) = oParentInst.getAttribute(ACXMLAttribOIKey)
                            End If
                        End If

                        lRow = lRow + 1

                    Next lInst

                End If

                'UPGRADE_NOTE: Object oInstances may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
                oInstances = Nothing
                'UPGRADE_NOTE: Object oInstance may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
                oInstance = Nothing
                'UPGRADE_NOTE: Object oParentInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
                oParentInst = Nothing

                ' Set the ObjectDef to be the Parent of the Current
                oObjectDef = CType(oObjectDef.parentNode, MSXML2.IXMLDOMElement)

                ' Belt and Braces Check. We should never hit this
                ' as we should always stop at the Risk/Quote Objects
                If (oObjectDef Is Nothing = True) Then
                    Exit Do
                End If

            Loop

            'UPGRADE_NOTE: Object oObjectDef may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oObjectDef = Nothing
            'UPGRADE_NOTE: Object oRoot may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oRoot = Nothing

            Exit Function

Err_GetInstanceHierarchy:

            GetInstanceHierarchy = gPMConstants.PMEReturnCode.PMError

            'UPGRADE_NOTE: Object oObjectDef may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oObjectDef = Nothing
            'UPGRADE_NOTE: Object oInstances may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oInstances = Nothing
            'UPGRADE_NOTE: Object oInstance may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oInstance = Nothing
            'UPGRADE_NOTE: Object oParentInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oParentInst = Nothing
            'UPGRADE_NOTE: Object oRoot may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oRoot = Nothing

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInstanceHierarchyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInstanceHierarchy", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

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
        Public Function GetObjectIdentity( _
            ByRef v_sObjectName As String, _
            ByRef v_sOIKey As String, _
            ByRef r_vPropertyArray(,) As Object) As Integer

            Dim oObjectInst As MSXML2.IXMLDOMElement
            Dim oObjectDef As MSXML2.IXMLDOMElement

            On Error GoTo Err_GetObjectIdentity

            GetObjectIdentity = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Object Definition
            oObjectDef = GetDefinitionNode(v_sObjectName)
            If (oObjectDef Is Nothing = True) Then
                GetObjectIdentity = gPMConstants.PMEReturnCode.PMInvalidRequest
                Exit Function
            End If

            ' Get the Object Instance
            oObjectInst = GetObjectInstance(v_sOIKey)
            If (oObjectInst Is Nothing = True) Then
                GetObjectIdentity = gPMConstants.PMEReturnCode.PMNotFound
                Exit Function
            End If

            GetObjectIdentity = GetPropertyValues(v_oObjectDef:=oObjectDef, v_oObjectInst:=oObjectInst, r_vIdValueArray:=r_vPropertyArray)

            'UPGRADE_NOTE: Object oObjectDef may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oObjectDef = Nothing
            'UPGRADE_NOTE: Object oObjectInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oObjectInst = Nothing

            Exit Function

Err_GetObjectIdentity:

            GetObjectIdentity = gPMConstants.PMEReturnCode.PMError

            'UPGRADE_NOTE: Object oObjectDef may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oObjectDef = Nothing
            'UPGRADE_NOTE: Object oObjectInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oObjectInst = Nothing

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetObjectIdentityFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetObjectIdentity", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

        End Function

        ' ***************************************************************** '
        ' Name: GetObjectDefDetails
        '
        ' Description: Returns the Definition Details for the
        '              given Object.
        '
        ' ***************************************************************** '
        Public Function GetObjectDefDetails( _
            ByRef v_sObjectName As String, _
            Optional ByRef r_lIsQuoteObject As Integer = 0, _
            Optional ByRef r_lGISObjectID As Integer = 0, _
            Optional ByRef r_sTableName As String = "", _
            Optional ByRef r_lMaxInstances As Integer = 0, _
            Optional ByRef r_lPolarisObjectID As Integer = 0, _
            Optional ByRef r_sParentObjectName As String = "", _
            Optional ByRef r_vChildObjectArray() As String = Nothing, _
            Optional ByRef r_vPropertyArray(,) As Object = Nothing, _
            Optional ByRef r_sSelectSQL As String = "", _
            Optional ByRef r_sInsertSQL As String = "", _
            Optional ByRef r_sUpdateSQL As String = "", _
            Optional ByRef r_sDeleteSQL As String = "") As Integer

            Dim oObjectDef As MSXML2.IXMLDOMElement
            Dim oParentDef As MSXML2.IXMLDOMElement
            Dim oChildDef As MSXML2.IXMLDOMElement
            Dim lNumOfChild As Integer
            Dim lChildNum As Integer
            Dim vChildObjectArray() As String
            Dim vPropertyArray(,) As Object
            Dim lPropertyNum As Integer
            Dim lObjectNum As Integer

            Dim vSQL As Object

            On Error GoTo Err_GetObjectDefDetails

            GetObjectDefDetails = gPMConstants.PMEReturnCode.PMTrue

            ' Get a Reference to the Definition Object
            oObjectDef = CType(GetDefinitionNode(v_sObjectName), MSXML2.IXMLDOMElement)

            If (oObjectDef Is Nothing = True) Then
                GetObjectDefDetails = gPMConstants.PMEReturnCode.PMInvalidRequest
                Exit Function
            End If

            ' Return the Attrbites
            'UPGRADE_WARNING: Couldn't resolve default property of object oObjectDef.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            r_lIsQuoteObject = CType(oObjectDef.getAttribute(ACXMLAttribIsQuoteObject), Integer)
            'UPGRADE_WARNING: Couldn't resolve default property of object oObjectDef.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            r_lGISObjectID = CType(oObjectDef.getAttribute(ACXMLAttribObjectID), Integer)
            'UPGRADE_WARNING: Couldn't resolve default property of object oObjectDef.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            r_sTableName = Trim(CType(oObjectDef.getAttribute(ACXMLAttribTableName), String))
            'UPGRADE_WARNING: Couldn't resolve default property of object oObjectDef.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            r_lMaxInstances = CType(oObjectDef.getAttribute(ACXMLAttribMaxInstances), Integer)
            'UPGRADE_WARNING: Couldn't resolve default property of object oObjectDef.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            r_lPolarisObjectID = CType(oObjectDef.getAttribute(ACXMLAttribPolarisObjectID), Integer)

            ' RFC 290200 - Store SQL Statements in Data Set Def.
            'UPGRADE_WARNING: Couldn't resolve default property of object oObjectDef.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            'UPGRADE_WARNING: Couldn't resolve default property of object vSQL. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            vSQL = oObjectDef.getAttribute(ACXMLAttribSQLSelect)
            'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1049"'
            If (IsDBNull(vSQL) = True) Then
                r_sSelectSQL = ""
            Else
                'UPGRADE_WARNING: Couldn't resolve default property of object vSQL. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                r_sSelectSQL = CType(vSQL, String)
            End If
            'UPGRADE_WARNING: Couldn't resolve default property of object oObjectDef.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            'UPGRADE_WARNING: Couldn't resolve default property of object vSQL. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            vSQL = oObjectDef.getAttribute(ACXMLAttribSQLInsert)
            'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1049"'
            If (IsDBNull(vSQL) = True) Then
                r_sInsertSQL = ""
            Else
                'UPGRADE_WARNING: Couldn't resolve default property of object vSQL. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                r_sInsertSQL = CType(vSQL, String)
            End If
            'UPGRADE_WARNING: Couldn't resolve default property of object oObjectDef.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            'UPGRADE_WARNING: Couldn't resolve default property of object vSQL. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            vSQL = oObjectDef.getAttribute(ACXMLAttribSQLUpdate)
            'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1049"'
            If (IsDBNull(vSQL) = True) Then
                r_sUpdateSQL = ""
            Else
                'UPGRADE_WARNING: Couldn't resolve default property of object vSQL. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                r_sUpdateSQL = CType(vSQL, String)
            End If
            'UPGRADE_WARNING: Couldn't resolve default property of object oObjectDef.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            'UPGRADE_WARNING: Couldn't resolve default property of object vSQL. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            vSQL = oObjectDef.getAttribute(ACXMLAttribSQLDelete)
            'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1049"'
            If (IsDBNull(vSQL) = True) Then
                r_sDeleteSQL = ""
            Else
                'UPGRADE_WARNING: Couldn't resolve default property of object vSQL. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                r_sDeleteSQL = CType(vSQL, String)
            End If

            oParentDef = CType(oObjectDef.parentNode, MSXML2.IXMLDOMElement)
            If (oParentDef Is Nothing = True) Then
                r_sParentObjectName = ""
            Else
                If (oParentDef.nodeName = ACXMLRiskObjects) Or (oParentDef.nodeName = ACXMLQuoteObjects) Then
                    r_sParentObjectName = ""
                Else
                    'UPGRADE_WARNING: Couldn't resolve default property of object oParentDef.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    r_sParentObjectName = CType(oParentDef.getAttribute(ACXMLAttribObjectName), String)
                End If
            End If

            ' If we have NOT been supplied the Child Object Array
            ' or the Property Array then EXIT
            'UPGRADE_NOTE: IsMissing() was changed to IsNothing(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1021"'
            If (IsNothing(r_vChildObjectArray) = True) And (IsNothing(r_vPropertyArray) = True) Then
                'UPGRADE_NOTE: Object oObjectDef may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
                oObjectDef = Nothing
                'UPGRADE_NOTE: Object oParentDef may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
                oParentDef = Nothing
                Exit Function
            End If

            ' Does this Object Definition have any Children
            lNumOfChild = oObjectDef.childNodes.length
            If (lNumOfChild > 0) Then

                ' Yes

                ' For each Child Definition
                For lChildNum = 0 To lNumOfChild - 1

                    ' Get a Reference to it
                    oChildDef = CType(oObjectDef.childNodes.item(lChildNum), MSXML2.IXMLDOMElement)

                    ' Does the Child Def have Children of Its Own
                    If (oChildDef.hasChildNodes = True) Then

                        ' Yes, so it is a Child Object Definition

                        ' Resize the Child Object Array
                        If (IsArray(vChildObjectArray) = False) Then
                            ReDim vChildObjectArray(0)
                        Else
                            ReDim Preserve vChildObjectArray(UBound(CType(vChildObjectArray, System.Array)) + 1)
                        End If

                        ' Add the Child Object Name to the end of the Array
                        lObjectNum = UBound(vChildObjectArray)
                        'UPGRADE_WARNING: Couldn't resolve default property of object oChildDef.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object vChildObjectArray(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                        vChildObjectArray(lObjectNum) = CType(oChildDef.getAttribute(ACXMLAttribObjectName), String)

                    Else

                        ' No, so it is a Property Definition

                        ' Resize the Property Array
                        If (IsArray(vPropertyArray) = False) Then
                            ReDim vPropertyArray(1, 0)
                        Else
                            ReDim Preserve vPropertyArray(1, UBound(vPropertyArray, 2) + 1)
                        End If

                        ' Add the Property Name to the end of the Array
                        lPropertyNum = UBound(vPropertyArray, 2)
                        'UPGRADE_WARNING: Couldn't resolve default property of object oChildDef.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object vPropertyArray(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                        vPropertyArray(0, lPropertyNum) = oChildDef.getAttribute(ACXMLAttribPropertyName)
                        'UPGRADE_WARNING: Couldn't resolve default property of object oChildDef.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object vPropertyArray(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                        vPropertyArray(1, lPropertyNum) = oChildDef.getAttribute(ACXMLAttribIsIdentProp)

                    End If

                Next lChildNum

            End If

            ' Return the Results and Tidy Up

            'UPGRADE_NOTE: IsMissing() was changed to IsNothing(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1021"'
            If (IsNothing(r_vChildObjectArray) = False) Then
                'UPGRADE_WARNING: Couldn't resolve default property of object vChildObjectArray. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                'UPGRADE_WARNING: Couldn't resolve default property of object r_vChildObjectArray. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                r_vChildObjectArray = vChildObjectArray
            End If

            'UPGRADE_NOTE: IsMissing() was changed to IsNothing(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1021"'
            If (IsNothing(r_vPropertyArray) = False) Then
                'UPGRADE_WARNING: Couldn't resolve default property of object vPropertyArray. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                'UPGRADE_WARNING: Couldn't resolve default property of object r_vPropertyArray. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                r_vPropertyArray = vPropertyArray
            End If

            'UPGRADE_WARNING: Couldn't resolve default property of object vChildObjectArray. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            vChildObjectArray = Nothing
            'UPGRADE_WARNING: Couldn't resolve default property of object vPropertyArray. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            vPropertyArray = Nothing

            'UPGRADE_NOTE: Object oObjectDef may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oObjectDef = Nothing
            'UPGRADE_NOTE: Object oParentDef may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oParentDef = Nothing
            'UPGRADE_NOTE: Object oChildDef may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oChildDef = Nothing

            Exit Function

Err_GetObjectDefDetails:

            GetObjectDefDetails = gPMConstants.PMEReturnCode.PMError

            'UPGRADE_NOTE: Object oObjectDef may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oObjectDef = Nothing
            'UPGRADE_NOTE: Object oParentDef may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oParentDef = Nothing
            'UPGRADE_NOTE: Object oChildDef may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oChildDef = Nothing

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetObjectDefDetailsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetObjectDefDetails", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

        End Function

        ' ***************************************************************** '
        ' Name: GetPropertyDefDetails
        '
        ' Description: Returns the Property Attributes for the Supplied
        '              Property Name.
        '
        ' ***************************************************************** '
        Public Function GetPropertyDefDetails( _
            ByRef v_sObjectName As String, _
            ByRef v_sPropertyName As String, _
            Optional ByRef r_lGISObjectID As Integer = 0, _
            Optional ByRef r_lGISPropertyID As Integer = 0, _
            Optional ByRef r_sColumnName As String = "", _
            Optional ByRef r_lDataType As Integer = 0, _
            Optional ByRef r_iIsPrimaryKey As Short = 0, _
            Optional ByRef r_iIsIdentifyingProperty As Short = 0, _
            Optional ByRef r_lGISListID As Integer = 0, _
            Optional ByRef r_lPolarisPropertyID As Integer = 0) As Integer

            Dim lReturn As Integer
            Dim oPropertyDef As MSXML2.IXMLDOMElement

            On Error GoTo Err_GetPropertyDefDetails

            GetPropertyDefDetails = gPMConstants.PMEReturnCode.PMTrue

            ' Get a Reference to the Property Definition
            oPropertyDef = CType(GetDefinitionNode(v_sObjectName, v_sPropertyName), MSXML2.IXMLDOMElement)
            If (oPropertyDef Is Nothing = True) Then
                GetPropertyDefDetails = gPMConstants.PMEReturnCode.PMInvalidRequest
                Exit Function
            End If

            ' Return the Attributes
            'UPGRADE_WARNING: Couldn't resolve default property of object oPropertyDef.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            r_lGISObjectID = CType(oPropertyDef.getAttribute(ACXMLAttribObjectID), Integer)
            'UPGRADE_WARNING: Couldn't resolve default property of object oPropertyDef.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            r_lGISPropertyID = CType(oPropertyDef.getAttribute(ACXMLAttribPropertyID), Integer)
            'UPGRADE_WARNING: Couldn't resolve default property of object oPropertyDef.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            r_sColumnName = Trim$(CType(oPropertyDef.getAttribute(ACXMLAttribColumnName), String))
            'UPGRADE_WARNING: Couldn't resolve default property of object oPropertyDef.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            r_lDataType = CType(oPropertyDef.getAttribute(ACXMLAttribDataType), Integer)
            'UPGRADE_WARNING: Couldn't resolve default property of object oPropertyDef.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            r_iIsPrimaryKey = CType(oPropertyDef.getAttribute(ACXMLAttribIsPrimaryKey), Short)
            'UPGRADE_WARNING: Couldn't resolve default property of object oPropertyDef.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            r_iIsIdentifyingProperty = CType(oPropertyDef.getAttribute(ACXMLAttribIsIdentProp), Short)
            'UPGRADE_WARNING: Couldn't resolve default property of object oPropertyDef.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            r_lGISListID = CType(oPropertyDef.getAttribute(ACXMLAttribGISListID), Integer)
            'UPGRADE_WARNING: Couldn't resolve default property of object oPropertyDef.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            r_lPolarisPropertyID = CType(oPropertyDef.getAttribute(ACXMLAttribPolarisPropertyID), Integer)

            'UPGRADE_NOTE: Object oPropertyDef may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oPropertyDef = Nothing

            Exit Function

Err_GetPropertyDefDetails:

            GetPropertyDefDetails = gPMConstants.PMEReturnCode.PMError

            'UPGRADE_NOTE: Object oPropertyDef may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oPropertyDef = Nothing

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPropertyDefDetailsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPropertyDefDetails", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

        End Function

        ' ***************************************************************** '
        ' Name: GetObjectInstDetails
        '
        ' Description: Returns the details for a given Object Instance,
        '              including an Array of Property Name/Values.
        '
        ' ***************************************************************** '
        Public Function GetObjectInstDetails( _
            ByRef v_sObjectName As String, _
            ByRef v_sOIKey As String, _
            Optional ByRef r_lUpdateStatus As Integer = 0, _
            Optional ByRef r_sParentOIKey As String = "", _
            Optional ByRef r_lChildNumber As Integer = 0, _
            Optional ByRef r_vPropertyValueArray(,) As Object = Nothing) As Integer

            Dim oObjectDef As MSXML2.IXMLDOMElement
            Dim oObjectInst As MSXML2.IXMLDOMElement
            Dim oParentInst As MSXML2.IXMLDOMElement

            Dim lReturn As Integer

            On Error GoTo Err_GetObjectInstDetails

            GetObjectInstDetails = gPMConstants.PMEReturnCode.PMTrue

            ' Get a Reference to the Object Definition
            oObjectDef = GetDefinitionNode(v_sObjectName)
            If (oObjectDef Is Nothing = True) Then
                GetObjectInstDetails = gPMConstants.PMEReturnCode.PMInvalidRequest
                Exit Function
            End If

            ' Get a Reference to the Object Instance
            oObjectInst = GetObjectInstance(v_sOIKey)
            If (oObjectInst Is Nothing = True) Then
                GetObjectInstDetails = gPMConstants.PMEReturnCode.PMNotFound
                Exit Function
            End If

            ' Get the Object Instance UpdateStatus Attribute
            'UPGRADE_WARNING: Couldn't resolve default property of object oObjectInst.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            r_lUpdateStatus = CType(oObjectInst.getAttribute(ACXMLAttribUpdateStatus), Integer)

            ' RFC29111999 - Do not need to hold the child number anymore
            '    ' Get the Child Number
            '    r_lChildNumber = oObjectInst.getAttribute(ACXMLAttribChildNum)

            ' Get the Parent OI Key
            oParentInst = CType(oObjectInst.parentNode, MSXML2.IXMLDOMElement)
            'UPGRADE_WARNING: Couldn't resolve default property of object oParentInst.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            r_sParentOIKey = CType(oParentInst.getAttribute(ACXMLAttribOIKey), String)

            ' If we need to build the Property Value Array
            'UPGRADE_NOTE: IsMissing() was changed to IsNothing(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1021"'
            If (IsNothing(r_vPropertyValueArray) = False) Then

                ' Build It
                lReturn = GetPropertyValues( _
                    v_oObjectDef:=oObjectDef, _
                    v_oObjectInst:=oObjectInst, _
                    r_vAllValueArray:=r_vPropertyValueArray)

                If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    GetObjectInstDetails = gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            ' Release Local Refs
            'UPGRADE_NOTE: Object oObjectDef may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oObjectDef = Nothing
            'UPGRADE_NOTE: Object oObjectInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oObjectInst = Nothing
            'UPGRADE_NOTE: Object oParentInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oParentInst = Nothing

            Exit Function

Err_GetObjectInstDetails:

            GetObjectInstDetails = gPMConstants.PMEReturnCode.PMError

            'UPGRADE_NOTE: Object oObjectDef may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oObjectDef = Nothing
            'UPGRADE_NOTE: Object oObjectInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oObjectInst = Nothing
            'UPGRADE_NOTE: Object oParentInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oParentInst = Nothing

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetObjectInstDetailsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetObjectInstDetails", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

        End Function

        ' ***************************************************************** '
        ' Name: GetObjectInstChild
        '
        ' Description: Returns an Array of OIKeys for all Child Instances
        '              of the supplied Child Object Type for a given
        '              Object Instance.
        ' ***************************************************************** '
        Public Function GetObjectInstChild( _
            ByRef v_sObjectName As String, _
            ByRef v_sOIKey As String, _
            ByRef v_sChildObjectName As Object, _
            ByRef r_vChildOIKeyArray() As String) As Integer

            Dim oObjectInst As MSXML2.IXMLDOMElement
            Dim oChildInsts As MSXML2.IXMLDOMNodeList
            Dim oChildInst As MSXML2.IXMLDOMElement
            Dim lNumOfChild As Integer
            Dim lChildNum As Integer

            On Error GoTo Err_GetObjectInstChild

            GetObjectInstChild = gPMConstants.PMEReturnCode.PMTrue


            ' Get a Reference to the Object Instance
            oObjectInst = GetObjectInstance(v_sOIKey)
            If (oObjectInst Is Nothing = True) Then
                GetObjectInstChild = gPMConstants.PMEReturnCode.PMNotFound
                Exit Function
            End If

            ' Get the List of Child Instances by their Object Name
            'UPGRADE_WARNING: Couldn't resolve default property of object v_sChildObjectName. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            oChildInsts = oObjectInst.getElementsByTagName(UCase(CType(v_sChildObjectName, String).Trim))

            ' How many are there
            lNumOfChild = oChildInsts.length - 1

            ' If we have some Child Instances
            If (lNumOfChild >= 0) Then

                ' Resize the Array
                ReDim r_vChildOIKeyArray(lNumOfChild)

                ' For Each Child
                For lChildNum = 0 To lNumOfChild

                    ' Get a Reference to it
                    oChildInst = CType(oChildInsts.item(lChildNum), MSXML2.IXMLDOMElement)

                    ' Add the Child Key to the Array
                    'UPGRADE_WARNING: Couldn't resolve default property of object oChildInst.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_vChildOIKeyArray(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    r_vChildOIKeyArray(lChildNum) = CType(oChildInst.getAttribute(ACXMLAttribOIKey), String)

                Next lChildNum

            End If

            ' Release Local References
            'UPGRADE_NOTE: Object oObjectInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oObjectInst = Nothing
            'UPGRADE_NOTE: Object oChildInsts may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oChildInsts = Nothing
            'UPGRADE_NOTE: Object oChildInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oChildInst = Nothing

            Exit Function

Err_GetObjectInstChild:

            GetObjectInstChild = gPMConstants.PMEReturnCode.PMError

            'UPGRADE_NOTE: Object oObjectInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oObjectInst = Nothing
            'UPGRADE_NOTE: Object oChildInsts may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oChildInsts = Nothing
            'UPGRADE_NOTE: Object oChildInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oChildInst = Nothing

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetObjectInstChildFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetObjectInstChild", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

        End Function


        ' ***************************************************************** '
        ' Name: GetAllOIKey
        '
        ' Description: Returns an Array of Object Instance Keys for ALL
        '              objects of given Object Name.
        '
        ' ***************************************************************** '
        Public Function GetAllOIKey( _
            ByRef v_sObjectName As String, _
            ByRef r_vOIKeyArray() As String) As Integer

            On Error GoTo Err_GetAllOIKey

            GetAllOIKey = gPMConstants.PMEReturnCode.PMTrue

            GetAllOIKey = CommonGetAllOIKey( _
                v_sObjectName:=v_sObjectName, _
                r_vOIKeyArray:=r_vOIKeyArray)

            Exit Function

Err_GetAllOIKey:

            GetAllOIKey = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllOIKeyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllOIKey", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

        End Function


        ' ***************************************************************** '
        ' Name: GetDelObjectsOIKey
        '
        ' Description: Returns an Array of Object Names and OIKeys
        '              for the Objects which need to be deleted from the
        '              database.
        ' ***************************************************************** '
        Private Function GetDelObjectsArray(ByRef r_vDelObjArray(,) As Object) As Integer

            Dim oDelObjs As MSXML2.IXMLDOMElement
            Dim oDelInst As MSXML2.IXMLDOMElement
            Dim lNumofInst As Integer
            Dim lInstNum As Integer

            On Error GoTo Err_GetDelObjectsOIKey

            GetDelObjectsArray = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Deleted Objects Node
            oDelObjs = GetObjectInstance(ACXMLDeletedObjects)

            ' How many are there
            lNumofInst = oDelObjs.childNodes.length - 1

            ' If we have any Instances
            If (lNumofInst >= 0) Then

                ' Resize the Array
                ReDim r_vDelObjArray(1, lNumofInst)

                ' For Each Instance
                For lInstNum = 0 To lNumofInst

                    ' Get a Reference to it
                    oDelInst = CType(oDelObjs.childNodes.item(lInstNum), MSXML2.IXMLDOMElement)

                    ' Add the Deleted Object to the Array
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_vDelObjArray(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    r_vDelObjArray(0, lInstNum) = Trim(oDelInst.nodeName)
                    'UPGRADE_WARNING: Couldn't resolve default property of object oDelInst.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_vDelObjArray(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    r_vDelObjArray(1, lInstNum) = oDelInst.getAttribute(ACXMLAttribOIKey)

                Next lInstNum

            End If

            'UPGRADE_NOTE: Object oDelInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oDelInst = Nothing
            'UPGRADE_NOTE: Object oDelObjs may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oDelObjs = Nothing

            Exit Function

Err_GetDelObjectsOIKey:

            GetDelObjectsArray = gPMConstants.PMEReturnCode.PMError

            'UPGRADE_NOTE: Object oDelInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oDelInst = Nothing
            'UPGRADE_NOTE: Object oDelObjs may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oDelObjs = Nothing

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDelObjectsOIKeyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDelObjectsOIKey", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

        End Function


        ' ***************************************************************** '
        ' Name: GetChildOIKey
        '
        ' Description: Returns an Array of Object Instance Keys for ALL
        '              Child objects of given Object Name.
        '
        ' ***************************************************************** '
        Public Function GetChildOIKey( _
            ByRef v_sParentObjectName As String, _
            ByRef v_sParentOIKey As String, _
            ByRef v_sChildObjectName As String, _
            ByRef r_vChildOIKeyArray() As String) As Integer

            Dim oAllInsts As MSXML2.IXMLDOMNodeList
            Dim oParentInst As MSXML2.IXMLDOMElement
            Dim oObjectInst As MSXML2.IXMLDOMElement
            Dim lNumofInst As Integer
            Dim lInstNum As Integer

            On Error GoTo Err_GetChildOIKey

            GetChildOIKey = gPMConstants.PMEReturnCode.PMTrue

            ' Get a Reference to the Parent Instance
            oParentInst = GetObjectInstance(v_sParentOIKey)
            If (oParentInst Is Nothing = True) Then
                GetChildOIKey = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' Get the List of All Instances by their Object Name
            oAllInsts = oParentInst.getElementsByTagName(UCase(Trim(v_sChildObjectName)))

            ' How many are there
            lNumofInst = oAllInsts.length - 1

            ' If we have any Instances
            If (lNumofInst >= 0) Then

                ' Resize the Array
                ReDim r_vChildOIKeyArray(lNumofInst)

                ' For Each Instance
                For lInstNum = 0 To lNumofInst

                    ' Get a Reference to it
                    oObjectInst = CType(oAllInsts.item(lInstNum), MSXML2.IXMLDOMElement)

                    ' Add the Child Key to the Array
                    'UPGRADE_WARNING: Couldn't resolve default property of object oObjectInst.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_vChildOIKeyArray(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    r_vChildOIKeyArray(lInstNum) = CType(oObjectInst.getAttribute(ACXMLAttribOIKey), String)

                Next lInstNum

            End If

            ' Release Local References
            'UPGRADE_NOTE: Object oObjectInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oObjectInst = Nothing
            'UPGRADE_NOTE: Object oAllInsts may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oAllInsts = Nothing
            'UPGRADE_NOTE: Object oParentInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oParentInst = Nothing

            Exit Function

Err_GetChildOIKey:

            GetChildOIKey = gPMConstants.PMEReturnCode.PMError

            'UPGRADE_NOTE: Object oObjectInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oObjectInst = Nothing
            'UPGRADE_NOTE: Object oAllInsts may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oAllInsts = Nothing
            'UPGRADE_NOTE: Object oParentInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oParentInst = Nothing

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetChildOIKeyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetChildOIKey", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

        End Function


        ' ***************************************************************** '
        ' Name: GetAllQuoteKey
        '
        ' Description: Returns an Array of Quote Keys.
        '
        ' ***************************************************************** '
        Private Function GetAllQuoteKey( _
            ByRef v_sObjectName As String, _
            ByRef r_vOIKeyArray() As String) As Integer

            Dim oAllInsts As MSXML2.IXMLDOMNodeList
            Dim oObjectInst As MSXML2.IXMLDOMElement
            Dim lNumofInst As Integer
            Dim lInstNum As Integer
            Dim oRoot As MSXML2.IXMLDOMElement

            On Error GoTo Err_GetAllQuoteKey

            GetAllQuoteKey = gPMConstants.PMEReturnCode.PMTrue


            ' Set the Root to be the Quotes element
            oRoot = GetObjectInstance(ACXMLQuotes)

            ' Get the Child Elements
            oAllInsts = oRoot.childNodes

            'UPGRADE_NOTE: Object oRoot may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oRoot = Nothing

            ' How many are there
            lNumofInst = oAllInsts.length - 1

            ' If we have any Instances
            If (lNumofInst >= 0) Then

                ' Resize the Array
                ReDim r_vOIKeyArray(lNumofInst)

                ' For Each Instance
                For lInstNum = 0 To lNumofInst

                    ' Get a Reference to it
                    oObjectInst = CType(oAllInsts.item(lInstNum), MSXML2.IXMLDOMElement)

                    ' Add the Child Key to the Array
                    'UPGRADE_WARNING: Couldn't resolve default property of object oObjectInst.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_vOIKeyArray(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    r_vOIKeyArray(lInstNum) = CType(oObjectInst.getAttribute(ACXMLAttribOIKey), String)

                Next lInstNum

            End If

            ' Release Local References
            'UPGRADE_NOTE: Object oObjectInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oObjectInst = Nothing
            'UPGRADE_NOTE: Object oAllInsts may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oAllInsts = Nothing

            Exit Function

Err_GetAllQuoteKey:

            GetAllQuoteKey = gPMConstants.PMEReturnCode.PMError

            'UPGRADE_NOTE: Object oObjectInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oObjectInst = Nothing
            'UPGRADE_NOTE: Object oAllInsts may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oAllInsts = Nothing
            'UPGRADE_NOTE: Object oRoot may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oRoot = Nothing

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllQuoteKeyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllQuoteKey", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

        End Function


        ' ***************************************************************** '
        ' Name: BuildOIKey
        '
        ' Description: Builds and Return the ObjectInstance Key from the
        '              supplied Paramaters.
        ' ***************************************************************** '
        Private Function BuildOIKey(ByRef v_sObjectName As String, ByRef v_lOINumber As Integer, Optional ByRef v_sQuoteKey As String = "") As String

            On Error GoTo Err_BuildOIKey

            BuildOIKey = ""

            BuildOIKey = ACXMLAttribOIKey & v_lOINumber

            If (v_sQuoteKey <> "") Then
                BuildOIKey = Trim(v_sQuoteKey) & ACOIKeySeparator & BuildOIKey
            End If

            Exit Function

Err_BuildOIKey:

            BuildOIKey = ""

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BuildOIKeyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildOIKey", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

        End Function


        ' ***************************************************************** '
        ' Name: GetTopLevelRiskObject
        '
        ' Description: Returns the Risk Top Level Object and Table Name
        '
        ' ***************************************************************** '
        Public Function GetTopLevelRiskObject(ByRef r_sObjectName As String, ByRef r_sTableName As String) As Integer

            Dim oRiskObjectsDef As MSXML2.IXMLDOMElement
            Dim oTopObjectDef As MSXML2.IXMLDOMElement

            On Error GoTo Err_GetTopLevelRiskObject

            GetTopLevelRiskObject = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Risk Objects Node
            oRiskObjectsDef = GetDefinitionNode(v_sObjectName:=ACXMLRiskObjects)

            If (oRiskObjectsDef Is Nothing = True) Then
                LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Locate Top Level Risk Object. Has the Data Set been Initialised?", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTopLevelRiskObject")
                GetTopLevelRiskObject = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            If (oRiskObjectsDef.childNodes.length = 0) Then
                LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="There are NO Risk Objects loaded.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTopLevelRiskObject")
                GetTopLevelRiskObject = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' Get the First Child Node
            oTopObjectDef = CType(oRiskObjectsDef.childNodes.item(0), MSXML2.IXMLDOMElement)

            ' Return the Object Name & Table Name
            'UPGRADE_WARNING: Couldn't resolve default property of object oTopObjectDef.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            r_sObjectName = CType(oTopObjectDef.getAttribute(ACXMLAttribObjectName), String)
            'UPGRADE_WARNING: Couldn't resolve default property of object oTopObjectDef.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            r_sTableName = CType(oTopObjectDef.getAttribute(ACXMLAttribTableName), String)

            'UPGRADE_NOTE: Object oTopObjectDef may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oTopObjectDef = Nothing
            'UPGRADE_NOTE: Object oRiskObjectsDef may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oRiskObjectsDef = Nothing

            Exit Function

Err_GetTopLevelRiskObject:

            'UPGRADE_NOTE: Object oTopObjectDef may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oTopObjectDef = Nothing
            'UPGRADE_NOTE: Object oRiskObjectsDef may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oRiskObjectsDef = Nothing

            GetTopLevelRiskObject = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTopLevelRiskObjectFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTopLevelRiskObject", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

        End Function

        ' RFC01/05/01
        ' ***************************************************************** '
        ' Name: GetTopLevelQuoteObject
        '
        ' Description: Returns the Quote Top Level Object and Table Name
        '
        ' ***************************************************************** '
        Public Function GetTopLevelQuoteObject(ByRef r_sObjectName As String, ByRef r_sTableName As String) As Integer

            Dim oQuoteObjectsDef As MSXML2.IXMLDOMElement
            Dim oTopObjectDef As MSXML2.IXMLDOMElement

            On Error GoTo Err_GetTopLevelQuoteObject

            GetTopLevelQuoteObject = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Quote Objects Node
            oQuoteObjectsDef = GetDefinitionNode(v_sObjectName:=ACXMLQuoteObjects)

            If (oQuoteObjectsDef Is Nothing = True) Then
                LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Locate Top Level Quote Object. Has the Data Set been Initialised?", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTopLevelQuoteObject")
                GetTopLevelQuoteObject = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            If (oQuoteObjectsDef.childNodes.length = 0) Then
                LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="There are NO Quote Objects loaded.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTopLevelQuoteObject")
                GetTopLevelQuoteObject = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            If (oQuoteObjectsDef.childNodes.length > 1) Then
                LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="There are more than 1 Top Level Quote Objects.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTopLevelQuoteObject")
                GetTopLevelQuoteObject = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' Get the First Child Node
            oTopObjectDef = CType(oQuoteObjectsDef.childNodes.item(0), MSXML2.IXMLDOMElement)

            ' Return the Object Name & Table Name
            'UPGRADE_WARNING: Couldn't resolve default property of object oTopObjectDef.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            r_sObjectName = CType(oTopObjectDef.getAttribute(ACXMLAttribObjectName), String)
            'UPGRADE_WARNING: Couldn't resolve default property of object oTopObjectDef.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            r_sTableName = CType(oTopObjectDef.getAttribute(ACXMLAttribTableName), String)

            'UPGRADE_NOTE: Object oTopObjectDef may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oTopObjectDef = Nothing
            'UPGRADE_NOTE: Object oQuoteObjectsDef may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oQuoteObjectsDef = Nothing

            Exit Function

Err_GetTopLevelQuoteObject:

            'UPGRADE_NOTE: Object oTopObjectDef may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oTopObjectDef = Nothing
            'UPGRADE_NOTE: Object oQuoteObjectsDef may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oQuoteObjectsDef = Nothing

            GetTopLevelQuoteObject = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTopLevelQuoteObjectFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTopLevelQuoteObject", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

        End Function


        ' ***************************************************************** '
        ' Name: MergeQuoteOutput
        '
        ' Description:
        '
        '
        ' ***************************************************************** '
        Private Function MergeQuoteOutput(ByRef v_sSourceDataSet As String) As Integer

            Dim oSourceDataSet As MSXML2.DOMDocument40
            Dim bLoaded As Boolean

            Dim oQuotesDestination As MSXML2.IXMLDOMElement
            Dim oQuotes As MSXML2.IXMLDOMNodeList
            Dim oQuote As MSXML2.IXMLDOMNode
            Dim oMergeQuote As MSXML2.IXMLDOMNode
            Dim lNumQtes As Integer
            Dim lSub As Integer

            On Error GoTo Err_MergeQuoteOutput

            MergeQuoteOutput = gPMConstants.PMEReturnCode.PMTrue

            oSourceDataSet = New MSXML2.DOMDocument40()
            ' Use the new parser
            Call oSourceDataSet.setProperty("NewParser", True)

            ' Load the Source Data Set
            oSourceDataSet.validateOnParse = False
            bLoaded = oSourceDataSet.loadXML(v_sSourceDataSet)
            If (bLoaded = False) Then
                LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Load Data Set from XML String", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXML")
                MergeQuoteOutput = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' Get the Destination Quotes Nodes
            oQuotesDestination = GetObjectInstance(ACXMLQuotes)
            If (oQuotesDestination Is Nothing = True) Then
                MergeQuoteOutput = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' Get a Node List containing all of the Quote Outputs from the Source
            oQuotes = oSourceDataSet.getElementsByTagName(ACXMLQuoteObjects)

            ' If there are NO Quotes then exit
            If (oQuotes Is Nothing = True) Then
                LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="No Quote Outputs Found.", vApp:=ACApp, vClass:=ACClass, vMethod:="MergeQuoteOutput")
                Exit Function
            End If

            ' Get the Number of Quotes
            lNumQtes = oQuotes.length

            ' If there are NO Quotes then exit
            If (lNumQtes <= 0) Then
                LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="No Quote Outputs Found.", vApp:=ACApp, vClass:=ACClass, vMethod:="MergeQuoteOutput")
                Exit Function
            End If

            ' For Each Quote
            For lSub = 0 To lNumQtes - 1

                ' Get a Reference to It
                oQuote = oQuotes.item(lSub)

                ' Clone it
                oMergeQuote = oQuote.cloneNode(True)

                ' Add it to the Destination
                oQuotesDestination.appendChild(oMergeQuote)

            Next lSub

            ' Release References
            'UPGRADE_NOTE: Object oSourceDataSet may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oSourceDataSet = Nothing
            'UPGRADE_NOTE: Object oQuotesDestination may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oQuotesDestination = Nothing
            'UPGRADE_NOTE: Object oQuotes may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oQuotes = Nothing
            'UPGRADE_NOTE: Object oQuote may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oQuote = Nothing
            'UPGRADE_NOTE: Object oMergeQuote may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oMergeQuote = Nothing

            Exit Function

Err_MergeQuoteOutput:

            MergeQuoteOutput = gPMConstants.PMEReturnCode.PMError

            'UPGRADE_NOTE: Object oSourceDataSet may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oSourceDataSet = Nothing
            'UPGRADE_NOTE: Object oQuotesDestination may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oQuotesDestination = Nothing
            'UPGRADE_NOTE: Object oQuotes may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oQuotes = Nothing
            'UPGRADE_NOTE: Object oQuote may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oQuote = Nothing
            'UPGRADE_NOTE: Object oMergeQuote may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oMergeQuote = Nothing

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MergeQuoteOutputFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="MergeQuoteOutput", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

        End Function


        ' ***************************************************************** '
        ' Name: SaveXMLToFile
        '
        ' Description: Save the Data Set definition and/or the Data Set to
        '              file/s in their XML format.
        '
        ' ***************************************************************** '
        Public Function SaveXMLToFile(Optional ByRef v_sDataSetDefFile As String = "", Optional ByRef v_sDataSetFile As String = "") As Integer

            On Error GoTo Err_SaveXMLToFile

            SaveXMLToFile = gPMConstants.PMEReturnCode.PMTrue

            If CBool(Trim(CStr(v_sDataSetDefFile <> ""))) Then
                m_oDataSetDef.save((v_sDataSetDefFile))
            End If

            If CBool(Trim(CStr(v_sDataSetFile <> ""))) Then

                ' Update the Next Object Instance number in the XML before it is returned
                m_oDataset.documentElement.setAttribute(ACXMLAttribNextOINumber, m_lNextOINumber)

                m_oDataset.save((v_sDataSetFile))

            End If

            Exit Function

Err_SaveXMLToFile:

            SaveXMLToFile = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveXMLToFileFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveXMLToFile", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

        End Function

        ' ***************************************************************** '
        ' Name: PolicyLinkID
        '
        ' Description: Returns the PolicyLindID Value.
        '
        ' ***************************************************************** '
        Public Function PolicyLinkID() As Integer

            Dim sTopLevelObject As String
            Dim sTopLevelTable As String
            Dim vOIKeyArray() As String
            Dim sOIKey As String
            Dim bAssumedInfo As Boolean
            Dim lReturn As Integer
            Dim vPolicyLinkID As Object

            On Error GoTo Err_PolicyLinkID

            PolicyLinkID = -1

            ' Get the Top Level Table Name
            lReturn = GetTopLevelRiskObject(r_sObjectName:=sTopLevelObject, r_sTableName:=sTopLevelTable)
            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                PolicyLinkID = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' Get the Instance Keys for the Top Level object
            ' Note, there should only be one.
            lReturn = GetAllOIKey(v_sObjectName:=sTopLevelObject, r_vOIKeyArray:=vOIKeyArray)
            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                PolicyLinkID = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' Check to see that we have ONE Key returned
            If (IsArray(vOIKeyArray) = False) Then
                LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to find any instances of Top Level Object :- " & sTopLevelObject, vApp:=ACApp, vClass:=ACClass, vMethod:="PolicyLinkID")
                PolicyLinkID = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            Else
                If (UBound(vOIKeyArray) > LBound(vOIKeyArray)) Then
                    LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="There should be only ONE instance of Top Level Object :- " & sTopLevelObject, vApp:=ACApp, vClass:=ACClass, vMethod:="PolicyLinkID")
                    PolicyLinkID = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                Else
                    'UPGRADE_WARNING: Couldn't resolve default property of object vOIKeyArray(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    sOIKey = vOIKeyArray(LBound(vOIKeyArray))
                End If
            End If

            ' Get the Policy Link ID Property in the Top Level Object
            lReturn = GetPropertyValue(v_sObjectName:=sTopLevelObject, v_sPropertyName:=GISPolLinkIDName, v_sOIKey:=sOIKey, r_vPropertyValue:=vPolicyLinkID, r_bIsAssumedInfo:=bAssumedInfo)
            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                PolicyLinkID = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            If (IsNumeric(vPolicyLinkID) = True) Then
                'UPGRADE_WARNING: Couldn't resolve default property of object vPolicyLinkID. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                PolicyLinkID = CInt(vPolicyLinkID)
            Else
                'UPGRADE_WARNING: Couldn't resolve default property of object vPolicyLinkID. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="PolicyLinkID Value is not set or NOT Numeric :-" & CStr(vPolicyLinkID), vApp:=ACApp, vClass:=ACClass, vMethod:="PolicyLinkID")
            End If

            Exit Function

Err_PolicyLinkID:

            PolicyLinkID = -1

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PolicyLinkIDFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="PolicyLinkID", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

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
        Private Function CommonGetAllOIKey( _
            ByRef v_sObjectName As String, _
            ByRef r_vOIKeyArray() As String, _
            Optional ByRef v_bDeletedObjects As Boolean = False) As Integer

            Dim oAllInsts As MSXML2.IXMLDOMNodeList
            Dim oObjectInst As MSXML2.IXMLDOMElement
            Dim lNumofInst As Integer
            Dim lInstNum As Integer
            Dim oRoot As MSXML2.IXMLDOMElement
            Dim lReturn As Integer
            Dim lIsQuoteObject As Integer

            On Error GoTo Err_CommonGetAllOIKey

            CommonGetAllOIKey = gPMConstants.PMEReturnCode.PMTrue

            ' Are we lookin for Deleted Objects
            If (v_bDeletedObjects = True) Then

                ' Yes, so set the root to be the Deleted Objects Element
                oRoot = GetObjectInstance(ACXMLDeletedObjects)

            Else

                ' Is this a Quote Object or a Risk Object
                lReturn = GetObjectDefDetails(v_sObjectName:=v_sObjectName, r_lIsQuoteObject:=lIsQuoteObject)
                If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    CommonGetAllOIKey = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                If (lIsQuoteObject = gPMConstants.PMEReturnCode.PMTrue) Then
                    oRoot = GetObjectInstance(ACXMLQuotes)
                Else
                    oRoot = GetObjectInstance(ACXMLRiskObjects)
                End If

            End If

            ' Get the List of All Instances by their Object Name
            oAllInsts = oRoot.getElementsByTagName(UCase(Trim(v_sObjectName)))

            'UPGRADE_NOTE: Object oRoot may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oRoot = Nothing

            ' How many are there
            lNumofInst = oAllInsts.length - 1

            ' If we have any Instances
            If (lNumofInst >= 0) Then

                ' Resize the Array
                ReDim r_vOIKeyArray(lNumofInst)

                ' For Each Instance
                For lInstNum = 0 To lNumofInst

                    ' Get a Reference to it
                    oObjectInst = CType(oAllInsts.item(lInstNum), MSXML2.IXMLDOMElement)

                    ' Add the Child Key to the Array
                    'UPGRADE_WARNING: Couldn't resolve default property of object oObjectInst.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_vOIKeyArray(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    r_vOIKeyArray(lInstNum) = CType(oObjectInst.getAttribute(ACXMLAttribOIKey), String)

                Next lInstNum

            End If

            ' Release Local References
            'UPGRADE_NOTE: Object oObjectInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oObjectInst = Nothing
            'UPGRADE_NOTE: Object oAllInsts may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oAllInsts = Nothing

            Exit Function

Err_CommonGetAllOIKey:

            CommonGetAllOIKey = gPMConstants.PMEReturnCode.PMError

            'UPGRADE_NOTE: Object oObjectInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oObjectInst = Nothing
            'UPGRADE_NOTE: Object oAllInsts may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oAllInsts = Nothing

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommonGetAllOIKeyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommonGetAllOIKey", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

        End Function


        ' ***************************************************************** '
        ' Name: InitDataSetDef
        '
        ' Description: Initialises the DataSetDefinition XML Document by
        '              adding the top level enteties.
        '
        ' ***************************************************************** '
        Private Function InitDataSetDef(ByRef v_sGisDataModelCode As String) As Integer

            Dim oDataSetDef As MSXML2.IXMLDOMElement
            Dim oRiskObjects As MSXML2.IXMLDOMElement
            Dim oQuoteObjects As MSXML2.IXMLDOMElement

            On Error GoTo Err_InitDataSetDef

            InitDataSetDef = gPMConstants.PMEReturnCode.PMTrue

            ' Create the Root Level Element
            oDataSetDef = m_oDataSetDef.createElement(ACXMLDataSetDef)

            ' Set the Data Model Code Attribute
            oDataSetDef.setAttribute(GISXMLAttribDataModelCode, v_sGisDataModelCode)

            ' RDC 27072001 Set the timestamp for the dataset definition
            oDataSetDef.setAttribute(GISXMLAttribDataSetDefTimestamp, Format(Now, "yyyyMMddhhmmss"))

            ' Set the Root Level Document Element
            m_oDataSetDef.documentElement = oDataSetDef

            ' Create the Risk Objects Element
            oRiskObjects = m_oDataSetDef.createElement(ACXMLRiskObjects)

            ' Append Risk Objects to the Document Element
            'UPGRADE_WARNING: Couldn't resolve default property of object oRiskObjects. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            m_oDataSetDef.documentElement.appendChild(oRiskObjects)

            ' Create the Ouput Objects Element
            oQuoteObjects = m_oDataSetDef.createElement(ACXMLQuoteObjects)

            ' Append Quote Objects to the Document Element
            'UPGRADE_WARNING: Couldn't resolve default property of object oQuoteObjects. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            m_oDataSetDef.documentElement.appendChild(oQuoteObjects)

            'UPGRADE_NOTE: Object oDataSetDef may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oDataSetDef = Nothing
            'UPGRADE_NOTE: Object oRiskObjects may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oRiskObjects = Nothing
            'UPGRADE_NOTE: Object oQuoteObjects may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oQuoteObjects = Nothing

            Exit Function

Err_InitDataSetDef:

            'UPGRADE_NOTE: Object oDataSetDef may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oDataSetDef = Nothing
            'UPGRADE_NOTE: Object oRiskObjects may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oRiskObjects = Nothing
            'UPGRADE_NOTE: Object oQuoteObjects may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oQuoteObjects = Nothing

            InitDataSetDef = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InitDataSetDefFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="InitDataSetDef", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

        End Function


        ' ***************************************************************** '
        ' Name: GetDefinitionNode
        '
        ' Description: Returns the Obect/Property Definition Node for the
        '              requested Object or Property.
        '
        ' ***************************************************************** '
        Private Function GetDefinitionNode(ByRef v_sObjectName As String, Optional ByRef v_sPropertyName As String = "") As MSXML2.IXMLDOMElement

            Dim oNodes As MSXML2.IXMLDOMNodeList
            Dim oNode As MSXML2.IXMLDOMNode
            Dim sNodeKey As String

            On Error GoTo Err_GetDefinitionNode

            'UPGRADE_NOTE: Object GetDefinitionNode may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            GetDefinitionNode = Nothing

            ' Build Up the Key

            ' Upercase the Object Name
            sNodeKey = UCase(Trim(v_sObjectName))

            ' If we have been supplied a Property Name then Add
            ' a full stop followed by the Property Name
            If (Trim(v_sPropertyName) <> "") Then
                sNodeKey = sNodeKey & "." & UCase(Trim(v_sPropertyName))
            End If

            ' Find elements with the Tag Name of the Node Key
            ' This returns a List of Nodes, however there should only be one
            oNodes = m_oDataSetDef.getElementsByTagName(sNodeKey)

            If (oNodes Is Nothing = True) Then
                LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to find Definiton for :-" & sNodeKey, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefinitionNode")
                Exit Function
            End If

            ' Get the Node from the List
            ' Remember there should only be one
            oNode = oNodes.item(0)
            If (oNode Is Nothing = True) Then
                LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to find Definition for :-" & sNodeKey, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefinitionNode")
                Exit Function
            End If

            ' All OK so return the Object/Property Definition Node
            GetDefinitionNode = CType(oNode, MSXML2.IXMLDOMElement)

            ' Release Local References
            'UPGRADE_NOTE: Object oNodes may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oNodes = Nothing
            'UPGRADE_NOTE: Object oNode may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oNode = Nothing

            Exit Function

Err_GetDefinitionNode:

            'UPGRADE_NOTE: Object oNodes may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oNodes = Nothing
            'UPGRADE_NOTE: Object oNode may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oNode = Nothing

            'UPGRADE_NOTE: Object GetDefinitionNode may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            GetDefinitionNode = Nothing

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefinitionNodeFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefinitionNode", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

        End Function

        ' ***************************************************************** '
        ' Name: GetObjectInstance
        '
        ' Description: Returns the Object Instance for the supplied Key.
        '
        ' ***************************************************************** '
        Private Function GetObjectInstance(ByRef v_sOIKey As String) As MSXML2.IXMLDOMElement

            Dim oObjectInst As MSXML2.IXMLDOMElement
            'Static sOIKey As String

            On Error GoTo Err_GetObjectInstance

            'UPGRADE_NOTE: Object GetObjectInstance may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            GetObjectInstance = Nothing


            ' Get the Object Instance 
            oObjectInst = CType(m_oDataset.nodeFromID(Trim(v_sOIKey)), MSXML2.IXMLDOMElement)

            ' If we did NOT the Instance then Error
            If oObjectInst Is Nothing And v_sOIKey.Trim() <> "DELETED_OBJECTS" Then
                LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Find Instance for Key :- " & v_sOIKey, vApp:=ACApp, vClass:=ACClass, vMethod:="GetObjectInstance")
                Exit Function
            End If

            ' Return the Object Instance
            GetObjectInstance = oObjectInst

            ' Release Local Reference
            'UPGRADE_NOTE: Object oObjectInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oObjectInst = Nothing

            Exit Function

Err_GetObjectInstance:

            'UPGRADE_NOTE: Object GetObjectInstance may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            GetObjectInstance = Nothing
            'UPGRADE_NOTE: Object oObjectInst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oObjectInst = Nothing

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetObjectInstanceFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetObjectInstance", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

        End Function


        ' ***************************************************************** '
        ' Name: GetPropertyValues
        '
        ' Description: Returns an Array of the Identifying Properties and
        '              their values for an Object Instance AND/OR an Array
        '              of All Properties and their values.
        ' ***************************************************************** '
        Private Function GetPropertyValues( _
            ByRef v_oObjectDef As MSXML2.IXMLDOMElement, _
            ByRef v_oObjectInst As MSXML2.IXMLDOMElement, _
            Optional ByRef r_vIdValueArray(,) As Object = Nothing, _
            Optional ByRef r_vAllValueArray(,) As Object = Nothing) As Integer

            Dim lIDProperty As Integer
            Dim lNum As Integer
            Dim lChild As Integer
            Dim oChildDef As MSXML2.IXMLDOMElement
            Dim lIDNum As Integer
            Dim lAllNum As Integer
            Dim sObjectName As String
            Dim sPropertyName As String
            Dim vPropertyValue As Object

            Dim vIdValueArray(,) As Object
            Dim vAllValueArray(,) As Object

            On Error GoTo Err_GetPropertyValues

            GetPropertyValues = gPMConstants.PMEReturnCode.PMTrue

            'UPGRADE_NOTE: IsMissing() was changed to IsNothing(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1021"'
            If (IsNothing(r_vIdValueArray) = True) And (IsNothing(r_vAllValueArray) = True) Then
                GetPropertyValues = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' Get the Object Name
            'UPGRADE_WARNING: Couldn't resolve default property of object v_oObjectDef.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            sObjectName = CType(v_oObjectDef.getAttribute(ACXMLAttribObjectName), String)

            ' Redim the Array. Always Allow for 3 Name/Value Pairs
            ' Note: Always allow for 3 values. If there are not 2 ID Properties
            ' the unused rows will be Empty
            ReDim vIdValueArray(1, 2)
            lIDNum = 0

            ' We will build the All Value Array Dynamically
            'UPGRADE_WARNING: Couldn't resolve default property of object vAllValueArray. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            lAllNum = 0

            ' Get the Number of Children for this Object
            lNum = v_oObjectDef.childNodes.length - 1

            ' For Each Child
            For lChild = 0 To lNum

                ' Get the Child
                oChildDef = CType(v_oObjectDef.childNodes.item(lChild), MSXML2.IXMLDOMElement)

                ' If the Child doesn't have any Children of its own
                ' then it is a Property
                If (oChildDef.hasChildNodes = False) Then

                    ' Get the Identifying Property Attribute
                    'UPGRADE_WARNING: Couldn't resolve default property of object oChildDef.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    lIDProperty = CType(oChildDef.getAttribute(ACXMLAttribIsIdentProp), Integer)

                    ' Get the PropertyName
                    'UPGRADE_WARNING: Couldn't resolve default property of object oChildDef.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    sPropertyName = CType(oChildDef.getAttribute(ACXMLAttribPropertyName), String)

                    'UPGRADE_WARNING: Couldn't resolve default property of object PropertyValue(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object vPropertyValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    vPropertyValue = PropertyValue(v_oObjectInst, sPropertyName)

                    ' If we are building the IDValue Array
                    'UPGRADE_NOTE: IsMissing() was changed to IsNothing(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1021"'
                    If (IsNothing(r_vIdValueArray) = False) Then

                        ' If this is an Identifying Property then add it to the ID Value array
                        ' Note: We can only return 3 Identifying Properties
                        If (lIDProperty = gPMConstants.PMEReturnCode.PMTrue) And (lIDNum <= UBound(vIdValueArray, 2)) Then

                            ' Build the Array
                            'UPGRADE_WARNING: Couldn't resolve default property of object vIdValueArray(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                            vIdValueArray(GISIDColPropName, lIDNum) = sPropertyName
                            'UPGRADE_WARNING: Couldn't resolve default property of object vPropertyValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                            'UPGRADE_WARNING: Couldn't resolve default property of object vIdValueArray(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                            vIdValueArray(GISIDColPropValue, lIDNum) = vPropertyValue

                            ' Increment the Array Row
                            lIDNum = lIDNum + 1

                        End If

                    End If

                    ' If we are building the All Value Array
                    'UPGRADE_NOTE: IsMissing() was changed to IsNothing(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1021"'
                    If (IsNothing(r_vAllValueArray) = False) Then

                        ' Resize the All Value Array
                        If (IsArray(vAllValueArray) = True) Then
                            lAllNum = UBound(vAllValueArray, 2) + 1
                            ReDim Preserve vAllValueArray(1, lAllNum)
                        Else
                            lAllNum = 0
                            ReDim vAllValueArray(1, 0)
                        End If

                        ' Add the Property
                        'UPGRADE_WARNING: Couldn't resolve default property of object vAllValueArray(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                        vAllValueArray(GISIDColPropName, lAllNum) = sPropertyName
                        'UPGRADE_WARNING: Couldn't resolve default property of object vPropertyValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object vAllValueArray(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                        vAllValueArray(GISIDColPropValue, lAllNum) = vPropertyValue

                    End If

                End If

            Next lChild

            ' If required, return the IDValue Array
            'UPGRADE_NOTE: IsMissing() was changed to IsNothing(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1021"'
            If (IsNothing(r_vIdValueArray) = False) Then
                'UPGRADE_WARNING: Couldn't resolve default property of object vIdValueArray. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                'UPGRADE_WARNING: Couldn't resolve default property of object r_vIdValueArray. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                r_vIdValueArray = vIdValueArray
            End If

            ' If required, return the ALLValue Array
            'UPGRADE_NOTE: IsMissing() was changed to IsNothing(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1021"'
            If (IsNothing(r_vAllValueArray) = False) Then
                'UPGRADE_WARNING: Couldn't resolve default property of object vAllValueArray. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                'UPGRADE_WARNING: Couldn't resolve default property of object r_vAllValueArray. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                r_vAllValueArray = vAllValueArray
            End If

            'UPGRADE_WARNING: Couldn't resolve default property of object vIdValueArray. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            vIdValueArray = Nothing
            'UPGRADE_WARNING: Couldn't resolve default property of object vAllValueArray. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            vAllValueArray = Nothing

            'UPGRADE_NOTE: Object oChildDef may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oChildDef = Nothing

            Exit Function

Err_GetPropertyValues:

            GetPropertyValues = gPMConstants.PMEReturnCode.PMError

            'UPGRADE_NOTE: Object oChildDef may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oChildDef = Nothing

            'UPGRADE_WARNING: Couldn't resolve default property of object vIdValueArray. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            vIdValueArray = Nothing
            'UPGRADE_WARNING: Couldn't resolve default property of object vAllValueArray. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            vAllValueArray = Nothing

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPropertyValuesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPropertyValues", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

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
        ' ***************************************************************** '
        Private Function SetRoot(Optional ByVal v_lQuoteNumber As Integer = 0) As DataSetControl.Node

            Dim oNode As DataSetControl.Node

            On Error GoTo Err_SetRoot

            ' Create  a new Node element
            oNode = New DataSetControl.Node()

            ' Set its root
            oNode.Root = m_oDataset.documentElement

            ' Set its Dataset
            oNode.Dataset = Me

            ' Is a Quote Number Supplied
            If (v_lQuoteNumber < 1) Then
                ' No so set the Root to be "???_POLICY_BINDER" node below "RISK_OBJECTS"
                oNode.Root = CType(m_oDataset.documentElement.childNodes(0).childNodes(0), MSXML2.IXMLDOMElement)
                ' As this is a RISK Object set the Quote Key to spaces
                oNode.QuoteKey = ""
            Else
                ' Set the Root to be the "QUOTES" node
                ' RFC200900 - Because of the addition of the DELETED OBJECTS node,
                ' QUOUTES is not the second element anymore. Therefore need to set
                ' the root to the third (2) element here.
                oNode.Root = CType(m_oDataset.documentElement.childNodes(2), MSXML2.IXMLDOMElement)
                ' Then set the Root to be the "QUOTE_OBJECTS" specified
                oNode.Root = CType(oNode.Root.childNodes(v_lQuoteNumber - 1), MSXML2.IXMLDOMElement)
                ' Set the Quote Key
                'UPGRADE_WARNING: Couldn't resolve default property of object oNode.Root.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                oNode.QuoteKey = CType(oNode.Root.getAttribute(ACXMLAttribOIKey), String)
            End If

            SetRoot = oNode
            'UPGRADE_NOTE: Object oNode may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oNode = Nothing

            Exit Function

Err_SetRoot:

            Err.Raise(Err.Number, Err.Source, Err.Description)

            Exit Function

        End Function

        ' ***************************************************************** '
        ' Name: Initialise (Standard Method)
        '
        ' Description: Entry point for any initialisation code for this
        '              object.
        '
        ' ***************************************************************** '
        Private Function Initialise() As Integer

            On Error GoTo Err_Initialise

            Initialise = gPMConstants.PMEReturnCode.PMTrue

            ' Initialisation Code.

            Exit Function

Err_Initialise:

            ' Error.

            Initialise = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise()", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

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
			 If disposing Then
				m_oDataSetDef = Nothing
				m_oDataset = Nothing
				m_oDefaults = Nothing
			End If
		End If
		Me.disposedValue = True
	End Sub



        ' ***************************************************************** '
        ' Name: PropertyValue
        '
        ' Description: Returns the Property Value from the Attribute
        '
        ' ***************************************************************** '
        Friend Function PropertyValue(ByRef r_oObjectInst As MSXML2.IXMLDOMElement, ByRef v_sPropertyName As String) As Object

            Dim vPropertyValue As Object

            On Error GoTo Err_PropertyValue

            ' Get the Property Value
            On Error Resume Next
            'UPGRADE_WARNING: Couldn't resolve default property of object PropertyValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            PropertyValue = Nothing
            'UPGRADE_WARNING: Couldn't resolve default property of object r_oObjectInst.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            'UPGRADE_WARNING: Couldn't resolve default property of object PropertyValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            PropertyValue = r_oObjectInst.getAttribute(UCase(Trim(v_sPropertyName)))
            On Error GoTo Err_PropertyValue

            'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1049"'
            If (IsDBNull(PropertyValue) = True) Then
                'UPGRADE_WARNING: Couldn't resolve default property of object PropertyValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                PropertyValue = Nothing
            End If

            'UPGRADE_WARNING: IsEmpty was upgraded to IsNothing and has a new behavior. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1041"'
            If (IsNothing(PropertyValue) = False) Then
                ' More to do here, may have to unconvert ampersands etc
                PropertyValue = Replace(CType(PropertyValue, String), "&amp;", "&")
                PropertyValue = Replace(CType(PropertyValue, String), "&lt;", "<")
                PropertyValue = Replace(CType(PropertyValue, String), "&gt;", ">")
                PropertyValue = Replace(CType(PropertyValue, String), "&apos;", "'")
                PropertyValue = Replace(CType(PropertyValue, String), "&quot;", """")
            End If

            Exit Function

Err_PropertyValue:

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PropertyValueFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertyValue", vErrNo:=Err.Number, vErrDesc:=Err.Description)

Err.Raise(Err.Number, ACApp & "." & ACClass & "." & "PropertyValue", Err.Description) 

            Exit Function

        End Function

        ' ***************************************************************** '
        ' Name: PropertyValueSet
        '
        ' Description: Set a Property Value
        '
        ' ***************************************************************** '
        Friend Sub PropertyValueSet(ByVal v_sObjectName As String, ByRef r_oObjectInst As MSXML2.IXMLDOMElement, ByRef v_sPropertyName As String, ByRef v_vPropertyValue As Object, Optional ByRef v_bLoadedFromDB As Boolean = False)

            Dim lReturn As Integer

            Dim lObjectUpdateStatus As Integer
            Dim lUpdateStatus As Integer
            Dim sPropertyTag As String

            Dim lAssumedInfo As Integer
            Dim vValue As Object
            Dim lDataType As Integer

            On Error GoTo Err_PropertyValueSet

            ' RFC110700 - Check to see if the value is the same
            On Error Resume Next
            'UPGRADE_WARNING: Couldn't resolve default property of object r_oObjectInst.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            'UPGRADE_WARNING: Couldn't resolve default property of object vValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            vValue = Trim(CType(r_oObjectInst.getAttribute(UCase(Trim(v_sPropertyName))), String))
            'UPGRADE_WARNING: Couldn't resolve default property of object v_vPropertyValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            'UPGRADE_WARNING: Couldn't resolve default property of object vValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            If Not vValue Is Nothing Then
                If (CType(vValue, String).Trim = CType(v_vPropertyValue, String).Trim) Then
                    ' Value is the same so just exit.
                    Exit Sub
                End If
            End If
            On Error GoTo Err_PropertyValueSet

            ''UPGRADE_WARNING: IsEmpty was upgraded to IsNothing and has a new behavior. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1041"'
            ''UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1049"'
            'If (IsDBNull(v_vPropertyValue) = False) And (IsNothing(v_vPropertyValue) = False) Then
            '    ' Escape ampersands etc
            '    ' Double Quotes are doubled up so that the save to DB will work
            '    '       v_vPropertyValue = Replace(v_vPropertyValue, "&", "&amp;")
            '    '       v_vPropertyValue = Replace(v_vPropertyValue, "<", "&lt;")
            '    '       v_vPropertyValue = Replace(v_vPropertyValue, ">", "&gt;")
            '    '       v_vPropertyValue = Replace(v_vPropertyValue, "'", "&apos;")
            '    'UPGRADE_WARNING: Couldn't resolve default property of object v_vPropertyValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            '    v_vPropertyValue = CType(CType(v_vPropertyValue, String).Replace("""", "&quot;"), Object)
            'End If

            '' CTAF 021002 - Tighter check for dates
            '' If the value is a date it needs to be formatted to be locale neutral
            'If (IsDate(v_vPropertyValue)) Then
            '    'UPGRADE_WARNING: Couldn't resolve default property of object v_vPropertyValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            '    If ((DatePart(Microsoft.VisualBasic.DateInterval.Year, CDate(v_vPropertyValue)) = CDbl("1899")) = False) Then
            '        'UPGRADE_WARNING: Couldn't resolve default property of object v_vPropertyValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            '        v_vPropertyValue = Format(CDate(v_vPropertyValue), "yyyy-MM-dd hh:mm:ss")
            '    End If
            'End If


            Dim oPropertyDef As MSXML2.IXMLDOMElement
            If (IsDBNull(v_vPropertyValue) = False) And (IsNothing(v_vPropertyValue) = False) And (v_vPropertyValue.ToString <> "") Then

                ' Escape ampersands etc
                ' Double Quotes are doubled up so that the save to DB will work
                '       v_vPropertyValue = Replace(v_vPropertyValue, "&", "&amp;")
                '       v_vPropertyValue = Replace(v_vPropertyValue, "<", "&lt;")
                '       v_vPropertyValue = Replace(v_vPropertyValue, ">", "&gt;")
                '       v_vPropertyValue = Replace(v_vPropertyValue, "'", "&apos;")
                v_vPropertyValue = Replace(v_vPropertyValue.ToString, """", "&quot;")

                'CLG 22/01/04 PN9429 Intergers are being converted to dates in risk screens
                If Not IsNumeric(v_vPropertyValue) Then
            ' CTAF 021002 - Tighter check for dates
            ' If the value is a date it needs to be formatted to be locale neutral

                    'CLG 20050916
                    'Previous tests to prevent a string being turned into dates have failed.
                    'The only way to be sure is to check the property data type. To get this is time expensive so
                    'we should only do it if
                    '1) The field contents "looks like a date"
                    '2) We are not in strict mode (where we already have the data type)
                    If IsDate(v_vPropertyValue) Then
                        'For speed, run an inline reduced version of GetPropertyDefDetails
                        lDataType = -1 'not a date

                        oPropertyDef = GetDefinitionNode(v_sObjectName, v_sPropertyName)

                        If (oPropertyDef Is Nothing = False) Then
                            'UPGRADE_WARNING: Couldn't resolve default property of object oPropertyDef.getAttribute. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            'UPGRADE_WARNING: Couldn't resolve default property of object lDataType. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            lDataType = CInt(oPropertyDef.getAttribute(ACXMLAttribDataType))
                            'UPGRADE_NOTE: Object oPropertyDef may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                            oPropertyDef = Nothing
                End If
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_vPropertyValue. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object GISDataTypeDate. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object lDataType. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        If (lDataType = GISDataTypeDate) And ((DatePart(Microsoft.VisualBasic.DateInterval.Year, CDate(v_vPropertyValue)) = CDbl("1899")) = False) Then
                            v_vPropertyValue = VB6.Format(v_vPropertyValue.ToString, "yyyy-mm-dd hh:nn:ss")
                        End If
                    End If

                End If
                'UPGRADE_WARNING: Couldn't resolve default property of object v_vPropertyValue. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                v_vPropertyValue = Trim(v_vPropertyValue.ToString) ' RAW 14/01/2004 : CQ3720 : added
            End If

            ' Set the Value
            'UPGRADE_WARNING: Couldn't resolve default property of object v_vPropertyValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            r_oObjectInst.setAttribute(UCase(Trim(v_sPropertyName)), CType(v_vPropertyValue, String).Trim)

            ' Are we loading this from the Database
            If (v_bLoadedFromDB = False) Then

                ' No, so we need to set the Objects Update Status

                ' Get the Objects current Update Status
                'UPGRADE_WARNING: Couldn't resolve default property of object r_oObjectInst.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                lObjectUpdateStatus = CType(r_oObjectInst.getAttribute(ACXMLAttribUpdateStatus), Integer)

                ' If the Update Status is View
                ' Note: If the current Status is Add, Edit or Delete leave it alone
                If (lObjectUpdateStatus = gPMConstants.PMEComponentAction.PMView) Then
                    ' Set it to Edit
                    r_oObjectInst.setAttribute(ACXMLAttribUpdateStatus, gPMConstants.PMEComponentAction.PMEdit)
                End If

            End If

            Exit Sub

Err_PropertyValueSet:

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PropertyValueSetFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertyValueSet", vErrNo:=Err.Number, vErrDesc:=Err.Description)

Err.Raise(Err.Number, ACApp & "." & ACClass & "." & "PropertyValueSet", Err.Description) 

            Exit Sub

        End Sub

        ' PRIVATE Methods (End)


        'UPGRADE_NOTE: Class_Initialize was upgraded to Class_Initialize_Renamed. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1061"'
        Private Sub Class_Initialize_Renamed()

            Dim lReturn As Integer

            ' Class Initialise

            On Error GoTo Err_ClassInitialise

            ' Initialise Data Model Definition
            m_oDataSetDef = New MSXML2.DOMDocument40()
            m_oDataset = New MSXML2.DOMDocument40()

            ' Use the new parser
            Call m_oDataSetDef.setProperty("NewParser", True)
            Call m_oDataset.setProperty("NewParser", True)

            '    g_lClassInitLevel = g_lClassInitLevel + 1
            '    Debug.Print "Initialise = " & g_lClassInitLevel

            Exit Sub

Err_ClassInitialise:

            ' Error.

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Sub

        End Sub
        Public Sub New()
            MyBase.New()
            Class_Initialize_Renamed()
        End Sub

        'UPGRADE_NOTE: Class_Terminate was upgraded to Class_Terminate_Renamed. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1061"'
        Private Sub Class_Terminate_Renamed()

            Dim lReturn As Integer

            ' Class Terminate

            On Error GoTo Err_ClassTerminate

            ' Terminate this Class
            lReturn = Terminate()

            '    g_lClassInitLevel = g_lClassInitLevel - 1
            '    Debug.Print "Terminate = " & g_lClassInitLevel

            Exit Sub

Err_ClassTerminate:

            ' Error.

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Terminate", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Terminate", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Sub

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

            Dim bLoaded As Boolean

            On Error GoTo Err_LoadDefaultsXMLFile

            LoadDefaultsXMLFile = gPMConstants.PMEReturnCode.PMTrue


            'Create a new XML Document object in which to 'load' the data from the
            'XML document (flat) file
            m_oDefaults = New MSXML2.DOMDocument40()

            ' Use the new parser
            Call m_oDefaults.setProperty("NewParser", True)

            'Attempt the load of data from the file into the object
            m_oDefaults.validateOnParse = False
            bLoaded = m_oDefaults.load(v_sGISDefaultsFile)
            If (bLoaded = False) Then
                LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Load Defaults from File : " & v_sGISDefaultsFile, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadDefaultsXMLFile")
                LoadDefaultsXMLFile = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            Exit Function

Err_LoadDefaultsXMLFile:

            LoadDefaultsXMLFile = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadDefaultsXMLFileFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadDefaultsXMLFile", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

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
        Private Function LoadAndApplyGISDefaults(ByVal v_sTransactionType As String) As Integer

            LoadAndApplyGISDefaults = gPMConstants.PMEReturnCode.PMTrue

            Dim sGISDefaultsFile As String
            Dim lReturn As Integer

            Dim oObject As MSXML2.IXMLDOMElement
            Dim oProperty As MSXML2.IXMLDOMElement
            Dim oCurrentObject As MSXML2.IXMLDOMElement

            Dim lObjCount As Integer
            Dim lPropCount As Integer
            Dim lObjectLoopCounter As Integer
            Dim lPropertyLoopCounter As Integer

            Dim sCurrentObjectName As String
            Dim sCurrentPropertyName As String
            Dim sCurrentDefaultValue As String
            Dim sCurrentPropertyValue As String

            Dim sMandatoryPropertyName As String
            Dim sMandatoryPropertyValue As String

            Dim vObjectKeyArray() As String
            Dim sCurrentObjectKey As String
            Dim sParentObjectKey As String

            Dim lInstanceCounter As Integer
            Dim bIsAssumedInfo As Boolean

            On Error GoTo Err_LoadAndApplyGISDefaults

            DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadAndApplyGISDefaults - Start", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            'Determine the name (and path) to the current solution specific GIS
            'defaults xml document
            lReturn = GetDefaultsFileName(GISDataModelCode, sGISDefaultsFile)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                LoadAndApplyGISDefaults = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            'Check if the file actually exists as it is optional - if not then exit without
            'errors and continue processing
            'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1041"'
            If Dir(sGISDefaultsFile) = "" Then
                DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Warning:Defaults XML file not found at " & sGISDefaultsFile, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Err.Number, vErrDesc:=Err.Description)

                Exit Function
            End If

            'Load the data model specific Defaults XML document data into memory
            lReturn = LoadDefaultsXMLFile(v_sGISDefaultsFile:=sGISDefaultsFile)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                LoadAndApplyGISDefaults = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            'Now use the MSXML2.dll functions to traverse the XML defaults document we
            'have in memory

            'Get count of OBJECTS to loop around
            lObjCount = m_oDefaults.documentElement.childNodes.length

            If lObjCount = 0 Then
                'Log a DEBUG message that Defaults file was found but no objects defined in it, exit and continue
                DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Warning:The following Defaults XML file was found but had no object definitions in:" & sGISDefaultsFile, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Err.Number, vErrDesc:=Err.Description)

                Exit Function
            End If

            'Loop around zero based objects collection
            For lObjectLoopCounter = 0 To lObjCount - 1

                'Set current object, note that documentElement refers to the top most
                'element (GIS_DEFAULTS)
                oObject = CType(m_oDefaults.documentElement.childNodes(lObjectLoopCounter), MSXML2.IXMLDOMElement)

                'This shouldn't happen unless the document has no objects
                If Not oObject.firstChild Is Nothing Then

                    'The name of the current OBJECT
                    sCurrentObjectName = oObject.firstChild.nodeName

                    'Check an object name has been specified ! If not, error and exit
                    If Trim(sCurrentObjectName) = "" Then
                        LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ERROR:An object with no name has been defined in the Defaults XML file.", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Err.Number, vErrDesc:=Err.Description)

                        LoadAndApplyGISDefaults = gPMConstants.PMEReturnCode.PMFalse
                        Exit Function
                    End If


                    'We set this object to be able to get the Mandatory property attribute later
                    oCurrentObject = CType(oObject.firstChild, MSXML2.IXMLDOMElement)

                    'Get count of PROPERTIES for the current object to loop around
                    lPropCount = oObject.childNodes.length

                    'Loop around property collection (ignore first instance (0) as
                    'this holds the object name ! Notice we don't log any warnings etc if no properties found
                    'as this is valid.
                    For lPropertyLoopCounter = 1 To lPropCount - 1

                        'Set current property instance
                        oProperty = CType(oObject.childNodes(lPropertyLoopCounter), MSXML2.IXMLDOMElement)

                        'This shouldn't happen unless current object has no matching
                        'property and default value pairs
                        If (Not oProperty.childNodes(0) Is Nothing) And (Not oProperty.childNodes(1) Is Nothing) Then

                            'The name of the Property (in 0) and the default value (in 1)
                            sCurrentPropertyName = oProperty.childNodes(0).text
                            sCurrentDefaultValue = oProperty.childNodes(1).text

                            'Check a property name has been specified ! If not, error and exit
                            If Trim(sCurrentPropertyName) = "" Then
                                LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ERROR:The following object has a property with no name in the Defaults XML file:" & sCurrentObjectName, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Err.Number, vErrDesc:=Err.Description)

                                LoadAndApplyGISDefaults = gPMConstants.PMEReturnCode.PMFalse
                                Exit Function
                            End If

                            'Check a property value has been specified ! If not, error and exit
                            If Trim(sCurrentDefaultValue) = "" Then
                                LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ERROR:The following object has a property with no value in the Defaults XML file. Object:" & sCurrentObjectName & " Property:" & sCurrentPropertyName, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Err.Number, vErrDesc:=Err.Description)

                                LoadAndApplyGISDefaults = gPMConstants.PMEReturnCode.PMFalse
                                Exit Function
                            End If

                            'At this point we have an object name, property name and default
                            'value so we need to apply this information to the dataset, then
                            'continue with reading and applying any others.

                            'Get an array of keys for the object first - we'll apply the
                            'default value to EACH instance of the object
                            lReturn = GetAllOIKey(v_sObjectName:=sCurrentObjectName, r_vOIKeyArray:=vObjectKeyArray)
                            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                LoadAndApplyGISDefaults = gPMConstants.PMEReturnCode.PMFalse
                                Exit Function
                            End If

                            'If objects exist loop around them setting the default property
                            'value - but only if the mandatory property for this object
                            '(which we get from the MANDATORY_PROPERTY attribute on the
                            'OBJECT_NAME element in the Defaults XML document) has
                            'a value (i.e. is not "" - which indicates that it is just a dummy
                            'object instance that should remain that way and not be populated
                            'with default values). Also only apply the default value if the
                            'property does not already have a value (i.e. it is "")
                            If IsArray(vObjectKeyArray) Then
                                For lInstanceCounter = LBound(vObjectKeyArray) To UBound(vObjectKeyArray)

                                    'UPGRADE_WARNING: Couldn't resolve default property of object vObjectKeyArray(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                                    sCurrentObjectKey = vObjectKeyArray(lInstanceCounter)

                                    'Check there is a mandatory property defined for this object in the Defaults
                                    'xml file - if none, error and exit
                                    'UPGRADE_WARNING: Couldn't resolve default property of object oCurrentObject.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                                    'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1049"'
                                    If IsDBNull(oCurrentObject.getAttribute(GISDefaultsMandatoryProperty)) Or Trim(CType(oCurrentObject.getAttribute(GISDefaultsMandatoryProperty), String)) = "" Then
                                        LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ERROR:No mandatory property has been defined in the Defaults XML file for the object:" & sCurrentObjectName, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Err.Number, vErrDesc:=Err.Description)

                                        LoadAndApplyGISDefaults = gPMConstants.PMEReturnCode.PMFalse
                                        Exit Function
                                    End If

                                    'What is the mandatory property first?
                                    'UPGRADE_WARNING: Couldn't resolve default property of object oCurrentObject.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                                    sMandatoryPropertyName = CType(oCurrentObject.getAttribute(GISDefaultsMandatoryProperty), String)

                                    'Get the value of the mandatory property from this object
                                    'instance in the dataset and if it is "" do not apply
                                    'default value to this instance (as it's just an emplt slot)
                                    Dim _sMandatoryPropertyValue As Object = sMandatoryPropertyValue
                                    lReturn = GetPropertyValue(v_sObjectName:=sCurrentObjectName, v_sPropertyName:=sMandatoryPropertyName, v_sOIKey:=sCurrentObjectKey, r_vPropertyValue:=_sMandatoryPropertyValue, r_bIsAssumedInfo:=bIsAssumedInfo)
                                    If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                        LoadAndApplyGISDefaults = gPMConstants.PMEReturnCode.PMFalse
                                        Exit Function
                                    End If
                                    sMandatoryPropertyValue = CStr(_sMandatoryPropertyValue)
                                    If Trim(sMandatoryPropertyValue) <> "" Then

                                        'There is a mandatory property value, so next check the
                                        'property we're going to default a value in hasn't already
                                        'got one - if not then apply it !
                                        Dim _sCurrentPropertyValue As Object = sCurrentPropertyValue
                                        lReturn = GetPropertyValue(v_sObjectName:=sCurrentObjectName, v_sPropertyName:=sCurrentPropertyName, v_sOIKey:=sCurrentObjectKey, r_vPropertyValue:=_sCurrentPropertyValue, r_bIsAssumedInfo:=bIsAssumedInfo)
                                        If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                            LoadAndApplyGISDefaults = gPMConstants.PMEReturnCode.PMFalse
                                            Exit Function
                                        End If
                                        sCurrentPropertyValue = CStr(_sCurrentPropertyValue)

                                        If Trim(sCurrentPropertyValue) = "" Then

                                            lReturn = SetPropertyValue(sCurrentObjectName, sCurrentPropertyName, sCurrentObjectKey, sCurrentDefaultValue, False)
                                            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                                LoadAndApplyGISDefaults = gPMConstants.PMEReturnCode.PMFalse
                                                Exit Function
                                            End If

                                            'Log a DEBUG message that we've just set a default value
                                            DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="The following property default in the Defaults XML file has been applied - Object:" & sCurrentObjectName & "(" & lInstanceCounter + 1 & ") Property:" & sCurrentPropertyName & " Value:" & sCurrentDefaultValue & "     ", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Err.Number, vErrDesc:=Err.Description)

                                        Else
                                            'Log a DEBUG message that current property value is NOT "" so this default has been ignored
                                            DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Warning:The following property default in the Defaults XML file has NOT been applied since the current property already has a value (of " & sCurrentPropertyValue & ") - Object:" & sCurrentObjectName & "(" & lInstanceCounter + 1 & ") Property:" & sCurrentPropertyName & " Value:" & sCurrentDefaultValue & "     ", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Err.Number, vErrDesc:=Err.Description)
                                        End If
                                    Else
                                        'Log a DEBUG message that Mandatory property is "" so this default has been ignored
                                        DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Warning:The following property default in the Defaults XML file has NOT been applied since the specified mandatory property value for this object (" & sMandatoryPropertyName & ") was not set - Object:" & sCurrentObjectName & "(" & lInstanceCounter + 1 & ") Property:" & sCurrentPropertyName & " Value:" & sCurrentDefaultValue & "     ", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Err.Number, vErrDesc:=Err.Description)
                                    End If

                                Next

                            Else

                                'Object doesn't exist so ignore (but log)
                                LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Warning:The following property default in the Defaults XML file has NOT been applied since an object instance did not exist to apply it to - Object:" & sCurrentObjectName & " Property:" & sCurrentPropertyName & " Value:" & sCurrentDefaultValue & "     ", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Err.Number, vErrDesc:=Err.Description)
                            End If

                        Else
                            'Current object has no matching property and default value pairs i.e. it
                            'does NOT have a <PROPERTY_NAME> and <DEFAULT_VALUE> defined for it.
                            'Error and exit
                            LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error:The following object in the Defaults XML file has no matching <PROPERTY_NAME> and <DEFAULT_VALUE> values defined:" & sCurrentObjectName, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Err.Number, vErrDesc:=Err.Description)

                            LoadAndApplyGISDefaults = gPMConstants.PMEReturnCode.PMFalse
                            Exit Function
                        End If
                    Next
                Else
                    'Log a DEBUG message that Defaults file was found but no objects defined in it, exit and continue
                    DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Warning:The following Defaults XML file was found but had no object definitions in:" & sGISDefaultsFile, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Err.Number, vErrDesc:=Err.Description)
                End If
            Next

            DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadAndApplyGISDefaults - End", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            ' Release References
            'UPGRADE_NOTE: Object oObject may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oObject = Nothing
            'UPGRADE_NOTE: Object oProperty may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oProperty = Nothing
            'UPGRADE_NOTE: Object oCurrentObject may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oCurrentObject = Nothing

            Exit Function

Err_LoadAndApplyGISDefaults:

            LoadAndApplyGISDefaults = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadAndApplyGISDefaultsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

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
        Private Function GetMandatoryPropertyName(ByVal v_sObjectName As String, ByRef r_sMandatoryPropertyName As String) As Integer

            GetMandatoryPropertyName = gPMConstants.PMEReturnCode.PMTrue

            Dim sGISDefaultsFile As String
            Dim lReturn As Integer

            Dim oElements As MSXML2.IXMLDOMNodeList
            Dim oObject As MSXML2.IXMLDOMElement

            On Error GoTo Err_GetMandatoryPropertyName

            'Load the data model specific Defaults XML document data into memory if not done already
            If m_oDefaults Is Nothing Then

                'Determine the name (and path) to the current solution specific GIS
                'defaults xml document
                lReturn = GetDefaultsFileName(GISDataModelCode, sGISDefaultsFile)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    GetMandatoryPropertyName = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                'Check if the file actually exists - if not then error & exit
                'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1041"'
                If Dir(sGISDefaultsFile) = "" Then
                    LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error:Defaults XML file not found at " & sGISDefaultsFile & ". Note this is required in order for QMM Cobol Linkage to be correctly populated with 'real' instances only and not object instances that are defaults (or slots).", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMandatoryPropertyName", vErrNo:=Err.Number, vErrDesc:=Err.Description)

                    GetMandatoryPropertyName = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                'Load the data model specific Defaults XML document data into memory
                lReturn = LoadDefaultsXMLFile(v_sGISDefaultsFile:=sGISDefaultsFile)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    GetMandatoryPropertyName = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
            End If

            'Now use the MSXML2.dll functions on the XML defaults document we have in memory

            'Return the elements for the given object name
            oElements = m_oDefaults.getElementsByTagName(UCase(v_sObjectName))

            'Check one was found - if not, error and exit.
            If oElements.length < 1 Then

                LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ERROR:No object has been defined in the Defaults XML file for:" & v_sObjectName & ". Note this is required in order for QMM Cobol Linkage to be correctly populated with 'real' instances only and not object instances that are defaults (or slots).", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Err.Number, vErrDesc:=Err.Description)
                GetMandatoryPropertyName = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            'We set this object to be able to get the Mandatory property attribute
            oObject = CType(oElements.item(0), MSXML2.IXMLDOMElement)

            'Check there is a mandatory property defined for this object in the Defaults
            'xml file - if none, error and exit
            'UPGRADE_WARNING: Couldn't resolve default property of object oObject.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1049"'
            If IsDBNull(oObject.getAttribute(GISDefaultsMandatoryProperty)) Or Trim(CType(oObject.getAttribute(GISDefaultsMandatoryProperty), String)) = "" Then
                LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ERROR:No mandatory property has been defined in the Defaults XML file for the object:" & v_sObjectName, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndApplyGISDefaults", vErrNo:=Err.Number, vErrDesc:=Err.Description)

                GetMandatoryPropertyName = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            'Get and return the mandatory property
            'UPGRADE_WARNING: Couldn't resolve default property of object oObject.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            r_sMandatoryPropertyName = CType(oObject.getAttribute(GISDefaultsMandatoryProperty), String)

            'Release resources
            'UPGRADE_NOTE: Object oElements may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oElements = Nothing
            'UPGRADE_NOTE: Object oObject may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            oObject = Nothing

            Exit Function

Err_GetMandatoryPropertyName:

            GetMandatoryPropertyName = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMandatoryPropertyNameFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMandatoryPropertyName", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            Exit Function

        End Function

    End Class

End Namespace

