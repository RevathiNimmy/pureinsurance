Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports System.Xml

Imports SharedFiles
Module XMLFunc

    Private Const ACClass As String = "XMLFunc"

    Public Const ACXMLNodeMap As String = "MAP"
    Public Const ACXMLNodeSubMap As String = "SUBMAP"
    Public Const ACXMLNodeStep As String = "STEP"
    Public Const ACXMLNodeKey As String = "KEY"

    Private m_sRoadmapPath As String = ""
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_lStepCounter As Integer

    ' SET 27/01/2004 - bSIROptions.business object

    Private m_oOptions As bSIROptions.Business
    'Private m_oPMCaption As Object
    Public Enum pmeXMLMapTypes
        WMTaskCode = 0
        WMTaskDescription = 1
        ImageURL = 2
        TransactionType = 3
        ProcessMode = 4
        RoadmapName = 5
        AutoClose = 6
        NavigatorDriven = 7
        Title = 8
        ResetKeysOnRestart = 9 'PN17474
    End Enum

    ' ***************************************************************** '
    '
    ' Name: SetElementValue
    '
    ' Description: Sets the value on an element
    '
    ' History: 11/07/2002 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function SetElementValue(ByVal r_oNode As XmlElement, ByVal v_sName As String, ByVal v_vValue As Object) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the value
            r_oNode.SetAttribute(v_sName, v_vValue)

            Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetElementValue
    '
    ' Description: Gets the value of an element, and applies a default if needed
    '
    ' History: 09/07/2002 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function GetElementValue(ByVal v_oNode As XmlElement, ByVal v_sName As String, Optional ByVal v_vDefault As Object = "") As Object

        Dim vValue As Object

        

            ' Get the value
            vValue = v_oNode.GetAttribute(v_sName)

            ' Check if we need a default

            If Convert.IsDBNull(vValue) Or IsNothing(vValue) Then
                vValue = v_vDefault
            End If
            If CStr(vValue) = "" Then
                vValue = v_vDefault
            End If

            ' Return the value

            Return vValue

    End Function

    ' ***************************************************************** '
    '
    ' Name: LoadRoadmap
    '
    ' Description: Loads a roadmap specified in v_sRoadmap, into an XML DOM
    '
    ' History: 09/07/2002 CTAF - Created.
    '          10/04/2003 Kevin Renshaw (CMG) Recursive calls to LoadSubMap not working
    '          13/05/2003 CTAF - Fixed recursive calls to LoadSupMap
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (LoadSubmap) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function LoadSubmap(ByVal v_sSubmap As String, ByRef r_oNode As XmlNode, ByRef r_oMap As XmlNode, ByVal v_lParentID As Integer, ByRef r_oCurrentStep As XmlNode) As Integer
    '
    'Dim result As Integer = 0
    'Dim oXML As XmlDocument
    'Dim oNode As XmlNode
    'Dim oSteps As XmlNodeList
    'Dim oStep As XmlNode
    'Dim oAttribute As XmlAttribute
    'Dim bReturn As Boolean
    'Dim sSubMap As String = ""
    'Dim lLoop1 As Integer
    'Dim sFilename As String = ""
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'Try 
    '
    'oXML = New XmlDocument()
    '
    'Call oXML.setProperty("NewParser", True)
    '

    'oXML.async = False

    'oXML.validateOnParse = True
    '
    ' Construct the filename
    'sFilename = m_sRoadmapPath & v_sSubmap
    '
    ' Load the xml
    'Dim temp_xml_result As Boolean
    'Try 
    'oXML.Load(sFilename)
    'temp_xml_result = True
    '
    'Catch parseError As System.Exception
    'temp_xml_result = False

    'bReturn = temp_xml_result
    '
    ' Check for errors

    'If oXML.parseError.errorCode <> 0 Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Display the error message

    'iPMFunc.LogExcepMessage(iType:=gPMConstants.PMELogLevel.PMLogFatal, sMsg:="Failed to load XML file: " & sFilename & Environment.NewLine &  _
    '                   oXML.parseError.Message &  _
    '                   "Line : " & CStr(CType(oXML.parseError, XmlException).LineNumber) & Environment.NewLine &  _
    '                   "Column : " & CStr(CType(oXML.parseError, XmlException).LinePosition), vApp:=ACApp, vClass:=ACClass, vMethod:="LoadXMLRoadmap", vErrNo:=Information.Err().Number, vErrDesc:="Please see Application Error.")
    ' error here
    'Return result

    '
    ' Select the map
    'oNode = oXML.SelectSingleNode(ACXMLNodeMap)
    '
    ' Get the list of steps
    'oSteps = oNode.SelectNodes(ACXMLNodeStep)
    '
    ' Append the steps
    'Dim oNextNode As XmlNode
    'For	Each oStep2 As XmlNode In oSteps
    'oStep = oStep2
    '
    ' Set the step counter value

    'm_lReturn = SetElementValue(r_oNode:=oStep, v_sName:="StepID", v_vValue:=m_lStepCounter)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse

    '
    ' Increment the step counter
    'm_lStepCounter += 1
    '
    ' Set the parent ID

    'm_lReturn = SetElementValue(r_oNode:=oStep, v_sName:="ParentID", v_vValue:=v_lParentID)
    '
    ' Get any submap to insert
    'sSubMap = CStr(GetElementValue(v_oNode:=oStep, v_sName:="Submap"))
    '
    '
    ' CTAF 20030313 - Changed insertion logic as we dont always want the map's
    '                 nextSibling, just the node's nextSibling
    'oNextNode = r_oNode.NextSibling
    'If oNextNode Is Nothing Then
    'r_oCurrentStep = r_oMap.AppendChild(oStep)
    'Set r_oCurrentStep = oStep
    'Else
    'r_oMap.InsertBefore(oStep, oNextNode)
    'r_oCurrentStep = r_oCurrentStep.NextSibling

    '
    '        ' DD 03/01/2003: Fixed insertion logic
    '        ' Insert/Append the step
    '        If r_oMap.nextSibling Is Nothing Then
    '            'The submap is at the end of the elements
    '            Call r_oMap.appendChild(newChild:=oStep)
    '            Set r_oCurrentStep = oStep
    '        Else
    '            'The submap between two elements so insert it.
    '            Call r_oMap.insertBefore(newChild:=oStep, refChild:=r_oCurrentStep.nextSibling)
    ''
    '            'The current step doesn't dynamically move down so advance it manually
    '            Set r_oCurrentStep = r_oCurrentStep.nextSibling
    '        End If
    '
    ' Do we have one?
    'If sSubMap <> "" Then
    '
    ' Set the parent ID to 0 as we're at root level in here

    'm_lReturn = SetElementValue(r_oNode:=oStep, v_sName:="IsHeader", v_vValue:=True)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse

    '
    ' Check we're not trying to load the same file again
    'If sSubMap.ToLower() <> v_sSubmap.ToLower() Then
    ' CTAF 20030513 - Changed to pass in the current step. not the parent step
    'm_lReturn = CType(LoadSubmap(v_sSubmap:=sSubMap, r_oNode:=r_oCurrentStep, r_oMap:=r_oMap, v_lParentID:=m_lStepCounter - 1, r_oCurrentStep:=r_oCurrentStep), gPMConstants.PMEReturnCode)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse

    'Else
    'result = gPMConstants.PMEReturnCode.PMFalse
    ' Log Error Message
    'iPMFunc.LogExcepMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="The current map is trying to load itself as a submap. This isn't supported." & Environment.NewLine &  _
    '                   "Current Map : " & v_sSubmap & Environment.NewLine &  _
    '                   "Submap : " & sSubMap, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadSubmap", vErrNo:=Information.Err().Number, vErrDesc:="Please see Application Error.")
    'Return result

    '
    'Else
    ' Remove the submap tag. not needed
    'oStep.Attributes.RemoveNamedItem("Submap")

    '
    'Next oStep2
    '
    ' Clear up working objects
    'oSteps = Nothing
    'oStep = Nothing
    'oXML = Nothing
    'oNode = Nothing
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error Message
    'iPMFunc.LogExcepMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadSubmap Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadSubmap", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    '
    'Return result

    ' ***************************************************************** '
    '
    ' Name: LoadRoadmap
    '
    ' Description: Loads a roadmap specified in v_sRoadmap, into an XML DOM
    '
    ' History: 09/07/2002 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function LoadXMLRoadmap(ByVal v_sRoadmap As String, ByRef r_oXMLDOM As XmlDocument) As Integer

        Dim result As Integer = 0
        Dim oXML As XmlDocument
        Dim oMap, oNode As XmlNode
        Dim oSteps As XmlNodeList

        Dim bReturn As Boolean
        Dim sSubMap As String = ""
        Dim sFilename As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oXML = New XmlDocument()

            ' Construct the filename
            sFilename = m_sRoadmapPath & v_sRoadmap

            ' Load the XML file
            Dim temp_xml_result As Boolean

            Try
                oXML.Load(sFilename)
                temp_xml_result = True
            Catch parseError As System.Exception
                temp_xml_result = False
            End Try

            bReturn = temp_xml_result

            ' Return the XML
            r_oXMLDOM = oXML

            oNode = Nothing
            oXML = Nothing
            oMap = Nothing
            oSteps = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadXMLRoadmap Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadXMLRoadmap", excep:=excep)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RoadmapXMLtoArray
    '
    ' Description: Converts an XML Document Object Model to an array
    '              that Navigator understands
    '
    ' History: 09/07/2002 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function RoadmapXMLtoArray(ByVal v_oXMLDOM As XmlDocument, ByRef r_vMapArray() As Object, ByRef r_vStepArray() As MainModule.Step_Renamed) As Integer

        Dim result As Integer = 0
        Dim oMap As XmlNode
        Dim lReturn As gPMConstants.PMEReturnCode

        Const kMethodname As String = "RoadmapXMLtoArray"
        

            result = gPMConstants.PMEReturnCode.PMTrue

            ' This is a new way of storing map details. Bit tidier

            ' ************************************************************************

            ' Get the map node
            oMap = v_oXMLDOM.SelectSingleNode(ACXMLNodeMap)

            ReDim r_vMapArray(ACMapArraySize)

            ' Get the map values

            r_vMapArray(ACMapWMTaskCode) = GetElementValue(v_oNode:=oMap, v_sName:="WMTaskCode")

            r_vMapArray(ACMapWMTaskDesc) = GetElementValue(v_oNode:=oMap, v_sName:="WMTaskDescription")

            r_vMapArray(ACMapImageURL) = GetElementValue(v_oNode:=oMap, v_sName:="ImageURL")

            r_vMapArray(ACMapTransactionType) = GetElementValue(v_oNode:=oMap, v_sName:="TransactionType", v_vDefault:=gPMConstants.PMTransactionTypeNB)

            r_vMapArray(ACMapProcessMode) = GetElementValue(v_oNode:=oMap, v_sName:="ProcessMode")

            r_vMapArray(ACMapRoadmapName) = GetElementValue(v_oNode:=oMap, v_sName:="RoadmapName")

            r_vMapArray(ACMapAutoClose) = GetElementValue(v_oNode:=oMap, v_sName:="AutoClose", v_vDefault:=False)

            r_vMapArray(ACMapNavigatorDriven) = GetElementValue(v_oNode:=oMap, v_sName:="NavigatorDriven", v_vDefault:=False)

            r_vMapArray(ACMapTitle) = GetElementValue(v_oNode:=oMap, v_sName:="Title", v_vDefault:="Sirius")

            r_vMapArray(ACMapResetKeysOnRestart) = GetElementValue(v_oNode:=oMap, v_sName:="ResetKeysOnRestart", v_vDefault:=True) 'PN17474
            r_vMapArray(ACMapResourceId) = GetElementValue(v_oNode:=oMap, v_sName:="ResourceId")
            ' ************************************************************************
            ' SET 27/01/2004 - this object is required by the MapXMLtoArray function
            '                  but it is created here because that function is recursive.
            If m_oOptions Is Nothing Then
                ' Create the business object
                Dim temp_m_oOptions As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oOptions, "bSIROptions.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oOptions = temp_m_oOptions

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn

                    ' Log Error Message
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSIROptions", vApp:=ACApp, vClass:=ACClass, vMethod:="RoadmapXMLToArray", excep:=New Exception(Information.Err().Description))

                    m_oOptions = Nothing
                    Return result
                End If

            End If

            Dim sCaption As String
            Dim lLanguageId As Integer
            If gPMFunctions.ToSafeInteger(r_vMapArray(ACMapResourceId)) > 0 Then

                m_lReturn = CType(gPMFunctions.GetUserIsAmericanLanguageID(lLanguageId), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodname, "GetUserIsAmericanLanguageID method failed", gPMConstants.PMELogLevel.PMLogError)
                    m_oOptions = Nothing
                    Return result
                End If
                sCaption = CStr(iPMFunc.GetResData(iLangID:=gPMFunctions.ToSafeInteger(lLanguageId), lId:=gPMFunctions.ToSafeLong(r_vMapArray(ACMapResourceId)), iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                If sCaption <> "" Then

                    r_vMapArray(ACMapWMTaskDesc) = sCaption
                End If
            End If

            ' Add the Steps
            lReturn = CType(MapXMLtoArray(oMap, r_vStepArray, 0), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If Not (m_oOptions Is Nothing) Then

            m_oOptions.Dispose()
                m_oOptions = Nothing
            End If

            oMap = Nothing

            Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: MapXMLtoArray
    '
    ' Description: Converts an XML Document Object Model to an array
    '              that Navigator understands
    '
    ' History: 11/06/2003 RFC - Created.
    '
    ' ***************************************************************** '
    Private Function MapXMLtoArray(ByRef oMap As XmlNode, ByRef r_vStepArray() As MainModule.Step_Renamed, ByVal v_lParent As Integer) As Integer

        'Dim oMap As msxml2.IXMLDOMNode
        Dim result As Integer = 0
        Dim oSteps As XmlNodeList
        Dim oStep As XmlNode
        Dim oKeys As XmlNodeList
        Dim oKey As XmlNode
        Dim lCounter, lCounter2 As Integer
        Dim vKeyArray(,) As Object
        Dim sSubMap As String = ""
        Dim lReturn As Integer
        Dim bIgnore As Boolean
        Dim lRemovedSteps As Integer
        Dim iSysOption As Integer
        Dim sSysOptionValue As String = ""
        Dim iProductOption As Integer
        Dim sProductOptionValue As String = ""
        Dim vOptionValue, sTemp As String
        Dim iLanguageId As Integer
        Dim sCaptionId As String = ""

        Dim bRemovedByOption As Boolean
        Dim vArray(,) As Object
        Dim bCorrectStepsNumber As Boolean
        Dim lElementId, lNewElementID As Integer

        Const kMethodname As String = "MapXMLtoArray"

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Check for Comments put in by the converter

        ' Get the first element
        oStep = oMap.FirstChild
        ' Remove it if its a Comment
        If oStep.NodeType = XmlNodeType.Comment Then
            oMap.RemoveChild(oStep)
        End If

        oSteps = oMap.ChildNodes
        
            If r_vStepArray Is Nothing Then
                lCounter = 0
                ReDim r_vStepArray(oSteps.Count - 1)
            Else
                lCounter = r_vStepArray.GetUpperBound(0) + 1
                ReDim Preserve r_vStepArray(r_vStepArray.GetUpperBound(0) + oSteps.Count)
            End If
            ReDim vArray(1, 0)
            lRemovedSteps = 0
            bRemovedByOption = False
            bCorrectStepsNumber = False

            For Each oStep2 As XmlNode In oSteps
                oStep = oStep2
                bIgnore = False
                bRemovedByOption = False
                ' SET 27/01/2004 - check if this is a comment
                If oStep.NodeType = XmlNodeType.Comment Then
                    bIgnore = True
                    lRemovedSteps += 1
                Else
                    iSysOption = CInt(GetElementValue(v_oNode:=oStep, v_sName:="CheckSystemOption", v_vDefault:=-1))
                    If iSysOption <> -1 Then
                        sSysOptionValue = CStr(GetElementValue(v_oNode:=oStep, v_sName:="SystemOptionValue", v_vDefault:=""))

                        If sSysOptionValue.Length > 0 Then

                            lReturn = m_oOptions.getOption(iOptionNumber:=iSysOption, sValue:=sTemp, v_iSourceID:=g_oObjectManager.SourceID)

                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = lReturn

                                ' Log Error Message
                            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to read system option " & iSysOption, vApp:=ACApp, vClass:=ACClass, vMethod:="MapXMLtoArray", excep:=New Exception(Information.Err().Description))

                                oSteps = Nothing
                                oStep = Nothing
                                oKeys = Nothing
                                oKey = Nothing
                                Return result
                            End If

                            If sTemp <> sSysOptionValue Then
                                ' don't add this step cos this option is not allowed
                                bIgnore = True
                                lRemovedSteps += 1
                                bRemovedByOption = True
                            End If
                        End If

                    Else

                        iProductOption = CInt(GetElementValue(v_oNode:=oStep, v_sName:="CheckProductOption", v_vDefault:=-1))

                        If iProductOption <> -1 Then

                            sProductOptionValue = CStr(GetElementValue(v_oNode:=oStep, v_sName:="ProductOptionValue", v_vDefault:=""))

                            If sProductOptionValue.Length > 0 Then

                                lReturn = iPMFunc.getProductOptionValue(iProductOption, 1, vOptionValue)

                                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = lReturn

                                    ' Log Error Message
                                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to read product option " & iProductOption, vApp:=ACApp, vClass:=ACClass, vMethod:="MapXMLtoArray", excep:=New Exception(Information.Err().Description))

                                    oSteps = Nothing
                                    oStep = Nothing
                                    oKeys = Nothing
                                    oKey = Nothing
                                    Return result
                                End If

                                If gPMFunctions.ToSafeString(vOptionValue, "") <> sProductOptionValue Then
                                    bIgnore = True
                                    lRemovedSteps += 1
                                    bRemovedByOption = True
                                End If

                            End If

                        End If

                    End If
                End If
                If Not bIgnore Then
                    If lCounter <> 0 Then
                        ReDim Preserve vArray(1, vArray.GetUpperBound(1) + 1)
                    End If
                    ' Not a comment, so act on it
                    r_vStepArray(lCounter).CancelAction = CStr(GetElementValue(v_oNode:=oStep, v_sName:="CancelAction"))
                    r_vStepArray(lCounter).CancelSteps = CInt(GetElementValue(v_oNode:=oStep, v_sName:="CancelSteps", v_vDefault:=0))

                    r_vStepArray(lCounter).Component = CStr(GetElementValue(v_oNode:=oStep, v_sName:="Component"))
                    r_vStepArray(lCounter).ComponentAction = CInt(GetElementValue(v_oNode:=oStep, v_sName:="ComponentAction", v_vDefault:=0))

                    ' If theres no Work Manager Task Code, then don't let them create one
                    r_vStepArray(lCounter).CreateWorkManagerTask = CBool(GetElementValue(v_oNode:=oStep, v_sName:="CreateWMTask", v_vDefault:=False))
                    ' SET 27/01/2004 - read the 'show WM interface' setting
                    r_vStepArray(lCounter).ShowWMTaskInterface = CBool(GetElementValue(v_oNode:=oStep, v_sName:="ShowWMTaskInterface", v_vDefault:=True))

                    r_vStepArray(lCounter).Description = CStr(GetElementValue(v_oNode:=oStep, v_sName:="Description"))
                    r_vStepArray(lCounter).OKAction = CStr(GetElementValue(v_oNode:=oStep, v_sName:="OKAction"))
                    r_vStepArray(lCounter).OKSteps = CInt(GetElementValue(v_oNode:=oStep, v_sName:="OKSteps", v_vDefault:=0))
                    r_vStepArray(lCounter).ServerSide = CBool(GetElementValue(v_oNode:=oStep, v_sName:="ServerSide", v_vDefault:=False))
                    r_vStepArray(lCounter).StepID = CInt(CStr(GetElementValue(v_oNode:=oStep, v_sName:="ElementID")).Substring(1))

                    r_vStepArray(lCounter).ResumeStep = CInt(GetElementValue(v_oNode:=oStep, v_sName:="ResumeStep", v_vDefault:=-1)) ' -1 = current step
                    r_vStepArray(lCounter).Type = CStr(GetElementValue(v_oNode:=oStep, v_sName:="Type"))
                    r_vStepArray(lCounter).ParentID = v_lParent
                    r_vStepArray(lCounter).OKNewRoadmap = CStr(GetElementValue(v_oNode:=oStep, v_sName:="OKNewRoadmap"))
                    r_vStepArray(lCounter).CancelNewRoadmap = CStr(GetElementValue(v_oNode:=oStep, v_sName:="CancelNewRoadmap"))
                    r_vStepArray(lCounter).Action1Action = CStr(GetElementValue(v_oNode:=oStep, v_sName:="Action1Action"))
                    r_vStepArray(lCounter).Action1Steps = CInt(GetElementValue(v_oNode:=oStep, v_sName:="Action1Steps", v_vDefault:=0))
                    r_vStepArray(lCounter).Action2Action = CStr(GetElementValue(v_oNode:=oStep, v_sName:="Action2Action"))
                    r_vStepArray(lCounter).Action2Steps = CInt(GetElementValue(v_oNode:=oStep, v_sName:="Action2Steps", v_vDefault:=0))
                    r_vStepArray(lCounter).ResourceId = CInt(GetElementValue(v_oNode:=oStep, v_sName:="ResourceId", v_vDefault:=0))
                    vArray(0, vArray.GetUpperBound(1)) = r_vStepArray(lCounter).StepID

                    vArray(1, vArray.GetUpperBound(1)) = r_vStepArray(lCounter).Description
                    If r_vStepArray(lCounter).ResourceId > 0 Then

                        m_lReturn = CType(gPMFunctions.GetUserIsAmericanLanguageID(iLanguageId), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodname, "GetUserIsAmericanLanguageID method failed", gPMConstants.PMELogLevel.PMLogError)
                            m_oOptions = Nothing
                            Return result
                        End If
                        r_vStepArray(lCounter).Description = CStr(iPMFunc.GetResData(iLanguageId, gPMFunctions.ToSafeLong(r_vStepArray(lCounter).ResourceId), gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                    End If
                    ' Is this Step a Sub Map?
                    sSubMap = CStr(GetElementValue(v_oNode:=oStep, v_sName:="Submap"))

                    r_vStepArray(lCounter).IsHeader = (sSubMap <> "")

                    If sSubMap <> "" Then
                        ' Yes, so call this function recursively to add it to the array
                        lReturn = MapXMLtoArray(oMap:=oStep.FirstChild, r_vStepArray:=r_vStepArray, v_lParent:=r_vStepArray(lCounter).StepID)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return lReturn
                        End If

                    End If
                    ' Default Keys
                    oKeys = oStep.SelectNodes(ACXMLNodeKey)

                    If oKeys.Count > 0 Then

                        ' Resize the array
                        ReDim vKeyArray(1, oKeys.Count - 1)

                        lCounter2 = 0

                        For Each oKey2 As XmlNode In oKeys
                            oKey = oKey2

                            ' Save the key

                            vKeyArray(0, lCounter2) = GetElementValue(v_oNode:=oKey, v_sName:="Name")

                            vKeyArray(1, lCounter2) = GetElementValue(v_oNode:=oKey, v_sName:="Value")

                            lCounter2 += 1

                        Next oKey2

                        r_vStepArray(lCounter).DefaultKeys = vKeyArray

                    End If
                    lCounter += 1
                ElseIf bRemovedByOption Then
                    bCorrectStepsNumber = True
                    ReDim Preserve vArray(1, vArray.GetUpperBound(1) + 1)

                    vArray(0, vArray.GetUpperBound(1)) = CStr(GetElementValue(v_oNode:=oStep, v_sName:="ElementID")).Substring(1)

                    vArray(1, vArray.GetUpperBound(1)) = "Removed By Option"
                End If ' bIgnore
            Next oStep2

            ' SET 27/01/2004 - were any steps removed
            If lRemovedSteps > 0 Then
                ReDim Preserve r_vStepArray(r_vStepArray.GetUpperBound(0) - lRemovedSteps)
            End If

            'Align the step numbers if any step is removed by any sys or product option
            If bCorrectStepsNumber Then
                For lCount As Integer = r_vStepArray.GetLowerBound(0) To r_vStepArray.GetUpperBound(0)
                    If r_vStepArray(lCount).OKAction = gPMConstants.PMNavActionForwardX And r_vStepArray(lCount).OKSteps > 1 Then
                        lElementId = r_vStepArray(lCount).StepID ' Original ID
                        For lCount1 As Integer = 0 To vArray.GetUpperBound(1)

                            If CDbl(vArray(0, lCount1)) = lElementId Then 'Search for position of the original id in master array

                                lNewElementID = CInt(vArray(0, lCount1 + r_vStepArray(lCount).OKSteps)) 'Pointing to which step ID
                                For lCount2 As Integer = r_vStepArray.GetLowerBound(0) To r_vStepArray.GetUpperBound(0)
                                    If r_vStepArray(lCount2).StepID = lNewElementID Then
                                        r_vStepArray(lCount).OKSteps = lCount2 - lCount
                                        Exit For
                                    End If
                                Next lCount2
                                Exit For
                            End If
                        Next lCount1
                    End If

                    If r_vStepArray(lCount).CancelAction = gPMConstants.PMNavActionForwardX And r_vStepArray(lCount).CancelSteps > 1 Then
                        lElementId = r_vStepArray(lCount).StepID
                        For lCount1 As Integer = 0 To vArray.GetUpperBound(1)

                            If CDbl(vArray(0, lCount1)) = lElementId Then

                                lNewElementID = CInt(vArray(0, lCount1 + r_vStepArray(lCount).CancelSteps)) 'Pointing to which step
                                For lCount2 As Integer = r_vStepArray.GetLowerBound(0) To r_vStepArray.GetUpperBound(0)
                                    If r_vStepArray(lCount2).StepID = lNewElementID Then
                                        r_vStepArray(lCount).CancelSteps = lCount2 - lCount
                                        Exit For
                                    End If
                                Next lCount2
                                Exit For
                            End If
                        Next lCount1
                    End If

                    If r_vStepArray(lCount).OKAction = gPMConstants.PMNavActionBackX And r_vStepArray(lCount).OKSteps > 1 Then
                        lElementId = r_vStepArray(lCount).StepID
                        For lCount1 As Integer = 0 To vArray.GetUpperBound(1)

                            If CDbl(vArray(0, lCount1)) = lElementId Then

                                lNewElementID = CInt(vArray(0, lCount1 - r_vStepArray(lCount).OKSteps)) 'Pointing to which step
                                For lCount2 As Integer = r_vStepArray.GetLowerBound(0) To r_vStepArray.GetUpperBound(0)
                                    If r_vStepArray(lCount2).StepID = lNewElementID Then
                                        r_vStepArray(lCount).OKSteps = lCount - lCount2
                                        Exit For
                                    End If
                                Next lCount2
                                Exit For
                            End If
                        Next lCount1
                    End If

                    If r_vStepArray(lCount).CancelAction = gPMConstants.PMNavActionBackX And r_vStepArray(lCount).CancelSteps > 1 Then
                        lElementId = r_vStepArray(lCount).StepID
                        For lCount1 As Integer = 0 To vArray.GetUpperBound(1)

                            If CDbl(vArray(0, lCount1)) = lElementId Then

                                lNewElementID = CInt(vArray(0, lCount1 - r_vStepArray(lCount).CancelSteps)) 'Pointing to which step
                                For lCount2 As Integer = r_vStepArray.GetLowerBound(0) To r_vStepArray.GetUpperBound(0)
                                    If r_vStepArray(lCount2).StepID = lNewElementID Then
                                        r_vStepArray(lCount).CancelSteps = lCount - lCount2
                                        Exit For
                                    End If
                                Next lCount2
                                Exit For
                            End If
                        Next lCount1
                    End If
                Next lCount
            End If

            ' Check if the last step tries to jump outside the map
            Select Case r_vStepArray(lCounter - 1).OKAction
                Case gPMConstants.PMNavActionForwardOne, gPMConstants.PMNavActionForwardX
                    ' Default it to complete
                    r_vStepArray(lCounter - 1).OKAction = gPMConstants.PMNavActionCompleteProcess
                Case Else
                    ' Fine by me
            End Select

            oMap = Nothing
            oSteps = Nothing
            oStep = Nothing
            oKeys = Nothing
            oKey = Nothing

            Return result

Err_MapXMLtoArray:

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
        gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MapXMLtoArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MapXMLtoArray", excep:=New Exception(Information.Err().Description))

            Return result
            Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: LoadRoadmapFromXML
    '
    ' Description: Loads a roadmap from an XML file and returns arrays
    '
    ' History: 11/07/2002 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function LoadRoadmapFromXML(ByVal v_sRoadmapPath As String, ByVal v_sXMLFile As String, ByRef r_vMapProperties As Object, ByRef r_vSteps() As MainModule.Step_Renamed) As Integer

        Dim result As Integer = 0
        Dim oXMLDOM As XmlDocument

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store the path to the maps
            m_sRoadmapPath = v_sRoadmapPath

            ' Load it into XML and sort it out first
            m_lReturn = CType(LoadXMLRoadmap(v_sRoadmap:=v_sXMLFile, r_oXMLDOM:=oXMLDOM), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Then convert the XML to an array that Navigator understands
            m_lReturn = CType(RoadmapXMLtoArray(v_oXMLDOM:=oXMLDOM, r_vMapArray:=r_vMapProperties, r_vStepArray:=r_vSteps), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Remove this as we dont need it now
            oXMLDOM = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadRoadmapFromXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadRoadmapFromXML", excep:=excep)
            Return result

        End Try
    End Function
End Module

