Imports SiriusFS.SAM.Client
Imports Microsoft.VisualBasic
Imports System.Xml
Imports System.Xml.XPath
Imports System.Xml.XmlReader
Imports System.Web.Configuration
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Web.HttpContext
Imports Nexus.Utils
Imports Nexus.Library
Imports CMS.Library
Imports System.Globalization.CultureInfo
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Constants.Constant


Namespace Nexus
    Public Class BaseClient : Inherits CMS.Library.Frontend.clsCMSPage

#Region " PRIVATE VARIABLES "
        'Set client mode (add / view / edit)
        Protected Const ClientMode As String = "CLIENT_MODE"
        Protected oMaster As ContentPlaceHolder

        Private oOI As Collections.Stack

        Private sScreenCode As String
        Private sNextPage As String
        Private sPrevPage As String
        Private isModalOtherParty As Boolean = False
        Protected oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Protected oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
        'Party Builder Child screen 
        Protected oPartyChildScreenPanel As Panel
        Protected oPartyBuilderPanel As Panel
        Protected oPartyChildScreenPanelForView As Panel
        Protected oPartyBuilderPanelForView As Panel

#End Region

#Region " PARTY BUILDER - BASE METHODS "
        ''' <summary>
        ''' Retrieve the dataset definition for the provided DataModelCode
        ''' </summary>
        ''' <param name="v_sDataModelCode">DataModelCode of the dataset definition to be retrieved,
        ''' if no DataModelCode is provided an attempt wil be made to retrieve the DataModelCode from the current dataset</param>
        ''' <returns>xml string representation of the dataset definition</returns>
        ''' <remarks></remarks>
        Protected Function GetDataSetDefinition(Optional ByVal v_sDataModelCode As String = Nothing) As String
            Dim oParty As NexusProvider.BaseParty = Session.Item(CNParty)
            Dim sDataSetDefinition As String = String.Empty
            If oParty IsNot Nothing And oParty.XMLDataset IsNot Nothing Then

                If v_sDataModelCode Is Nothing Then

                    'Read DataModelCode from DataSet if it's not been passed

                    Dim Doc As XPathDocument = New XPathDocument(New IO.StringReader(oParty.XMLDataset))
                    Dim Navigator As XPathNavigator
                    Navigator = Doc.CreateNavigator()

                    Dim iNodeIterator As XPathNodeIterator = Navigator.Select("DATA_SET")

                    While (iNodeIterator.MoveNext)
                        v_sDataModelCode = iNodeIterator.Current.GetAttribute("DataModelCode", String.Empty)
                    End While

                    Current.Session.Item(CNPartyDataModelCode) = v_sDataModelCode

                End If


                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

                Try
                    sDataSetDefinition = oWebService.GetDatasetDefinition(v_sDataModelCode)
                Finally
                    oWebService = Nothing
                End Try

            End If
            Return sDataSetDefinition

        End Function

        ''' <summary>
        ''' Write all the party builder controls from the provided control container to the current party object.
        ''' </summary>
        ''' <param name="v_oContainer">The control to be searched for party controls to load data to,
        ''' this is usually the masterpage container</param>        
        ''' <param name="v_sScreenCode">The screencode of the current element, we need this for running the default rules</param>
        ''' <param name="v_sOI">The dataset identifier of the current element</param>
        ''' <remarks></remarks>
        Protected Sub WriteAdditionalControlsFromContainer(ByVal v_oContainer As ContentPlaceHolder,
                                    Optional ByVal v_sScreenCode As String = Nothing,
                                    Optional ByVal v_sOI As String = Nothing)

            If Current.Session(CNParty) IsNot Nothing Then

                Dim oParty As NexusProvider.BaseParty = Session.Item(CNParty)

                Select Case True
                    Case TypeOf oParty Is NexusProvider.CorporateParty
                        With CType(oParty, NexusProvider.CorporateParty)
                            For Each oControl As Control In v_oContainer.Controls
                                If oControl.ID IsNot Nothing Then
                                    Select Case oControl.ID
                                        Case "txtAlternativeId"
                                            .AlternativeId = CType(oControl, TextBox).Text
                                    End Select
                                End If
                            Next
                        End With
                    Case TypeOf oParty Is NexusProvider.PersonalParty
                        With CType(oParty, NexusProvider.PersonalParty)
                            For Each oControl As Control In v_oContainer.Controls
                                If oControl.ID IsNot Nothing Then
                                    Select Case oControl.ID
                                        Case "txtTradingName"
                                            .TradingName = CType(oControl, TextBox).Text
                                        Case "txtAltID"
                                            .AlternativeID = CType(oControl, TextBox).Text
                                    End Select
                                End If
                            Next
                        End With
                End Select


                Current.Session(CNParty) = oParty
            End If
        End Sub


        ''' <summary>
        ''' Write all the party controls from the provided control container to the current party object.
        ''' </summary>
        ''' <param name="v_oContainer">The control to be searched for party controls to load data to,
        ''' this is usually the masterpage container</param>
        ''' <param name="v_sScreenCode">The screencode of the current element, we need this for running the default rules</param>
        ''' <param name="v_sOI">The dataset identifier of the current element</param>
        ''' <remarks></remarks>
        Protected Sub WritePartyControlsFromContainerToXML(ByVal v_oContainer As Control,
                                  Optional ByVal v_sScreenCode As String = Nothing,
                                    Optional ByVal v_sOI As String = Nothing)

            If Current.Session(CNParty) IsNot Nothing And CType(Current.Session(CNParty), NexusProvider.BaseParty).XMLDataset IsNot Nothing Then

                Dim sDataSetDefinition As String = GetDataSetDefinition(Current.Session(CNPartyDataModelCode))
                Dim oDataSet As New DataSetControl.Application
                Dim oParty As NexusProvider.BaseParty = Session.Item(CNParty)

                Dim srDataset As New System.IO.StringReader(oParty.XMLDataset)
                Dim xmlTR As New XmlTextReader(srDataset)
                Dim oDoc As New XmlDocument

                oDoc.Load(xmlTR)
                xmlTR.Close()

                If v_sOI IsNot Nothing Then
                    Dim oNode As XmlNode = oDoc.SelectSingleNode("//*[@OI = '" & v_sOI & "']")
                    If oNode IsNot Nothing Then
                        oNode.Attributes("US").Value = "2"
                    End If
                End If


                Dim swContent As New System.IO.StringWriter
                Dim xmlwContent As New XmlTextWriter(swContent)

                oDoc.WriteTo(xmlwContent)
                oParty.XMLDataset = swContent.ToString()

                xmlwContent.Close()
                swContent.Close()

                oDataSet.LoadFromXML(sDataSetDefinition, oParty.XMLDataset)

                sDataSetDefinition = Nothing


                Dim oControl As Object
                Dim sControlName() As String 'Will be 2 or 3 elements, ELEMENT__ATTRIBUTE(__CONDITION)
                Dim sControlValue As String = String.Empty
                Dim bSave As Boolean 'means the save can be overridden if we have a trikxy control
                Dim bIgnore As Boolean 'allows controls to be populate but not saved into backoffice

                For Each oControl In v_oContainer.Controls
                    If oControl.ID IsNot Nothing Then

                        sControlName = Regex.Split(oControl.ID.ToUpper(), "__")
                        bSave = False
                        bIgnore = False

                        If sControlName.Length > 1 Then

                            Select Case oControl.GetType.Name

                                Case "HiddenField"

                                    sControlValue = CType(oControl, HiddenField).Value
                                    bSave = True

                                Case "HtmlInputText"

                                    sControlValue = CType(oControl, HtmlInputText).Value
                                    bSave = True

                                Case "TextBox"

                                    sControlValue = CType(oControl, TextBox).Text
                                    bSave = True

                                Case "CheckBox"

                                    sControlValue = IIf(CType(oControl, CheckBox).Checked, "1", "0")
                                    bSave = True

                                Case "RadioButtonList"

                                    If oControl.SelectedIndex = -1 Then
                                        sControlValue = 2
                                    Else
                                        sControlValue = oControl.SelectedValue
                                    End If

                                    bSave = True

                                Case "LookupList"

                                    sControlValue = oControl.Value
                                    If CType(oControl, NexusProvider.LookupList).Text.Equals(CType(oControl, NexusProvider.LookupList).DefaultText) Then
                                        'DH - 25-01-08 - Don't save the value to the dataset if its set to the default value
                                        bSave = False
                                    Else
                                        bSave = True
                                    End If

                                Case "LookupListV2"

                                    sControlValue = oControl.Value
                                    If CType(oControl, NexusProvider.LookupListV2).Text.Equals(CType(oControl, NexusProvider.LookupListV2).DefaultText) Then
                                        'DH - 25-01-08 - Don't save the value to the dataset if its set to the default value
                                        bSave = False
                                    Else
                                        bSave = True
                                    End If


                                Case "Panel"

                                    WritePartyControlsFromContainerToXML(oControl, v_sScreenCode, v_sOI)


                                Case "controls_addresscntrl_ascx"

                                    Dim iAddressKey As Integer

                                    Try
                                        iAddressKey = oControl.AddressKey()

                                        If iAddressKey <> 0 Then
                                            sControlValue = iAddressKey
                                            bSave = True
                                        End If

                                    Catch ex As ArgumentNullException

                                        'DH - 01-02-08 - Catch and ignore any argument null exceptions from
                                        'the AddAddress method, as if the address is missing a parameter we
                                        'don't want to save it to the dataset
                                    End Try

                            End Select

                            If bSave Then
                                WriteAttributeToXML(oDoc, oDataSet, sControlName, sControlValue, v_sOI, Session(CNPartyDataModelCode))
                            End If
                        End If
                    End If
                Next

                oDataSet.ReturnAsXML(oParty.XMLDataset)
                oDataSet.Terminate()
                oDataSet = Nothing
                srDataset.Close()

                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

                Try
                    If Not String.IsNullOrEmpty(v_sScreenCode) Then
                        oParty.XMLDataset = oWebService.RunDefaultRulesEdit(v_sScreenCode, oParty.XMLDataset, Nothing, oParty.BranchCode)
                    End If
                Finally
                    oWebService = Nothing
                End Try

                Current.Session(CNParty) = oParty
            End If
        End Sub
        ''' <summary>
        ''' The entry point for reading the risk controls from the provided control container
        ''' </summary>
        ''' <param name="v_oContainer">The control to be searched for risk controls to load data to,
        ''' this is usually the masterpage container</param>
        ''' <param name="v_sOI">The dataset identifier of the current element</param>
        ''' <param name="sender">The object that made the request to the function, usually content placeholder</param>
        ''' <param name="InitializeOnly">Initialize the risk controls only, e.g set attributes,
        ''' hookup events but don't read the risk data into the control, mainly used on postback</param>
        ''' <remarks></remarks>
        Protected Sub ReadPartyControlsContainerFromXML(ByVal v_oContainer As Control,
                                ByVal v_sOI As String,
                                ByVal sender As Object,
                                Optional ByVal InitializeOnly As Boolean = False)

            If Current.Session(CNParty) IsNot Nothing Then

                Dim oParty As NexusProvider.BaseParty = Session.Item(CNParty)
                ' sDataSetDefinition is created for future use
                If Not String.IsNullOrEmpty(oParty.XMLDataset) Then

                    Dim sDataSetDefinition As String = GetDataSetDefinition(Current.Session(CNPartyDataModelCode))
                    Dim xmlTR As New XmlTextReader(New System.IO.StringReader(oParty.XMLDataset))
                    Dim Doc As New XmlDocument

                    Doc.Load(xmlTR)
                    xmlTR.Close()

                    Select Case CType(Session(CNClientMode), Mode)
                        Case Mode.Add
                            LoadClientControls(Doc, v_oContainer, v_sOI, sender, False)
                            FrameWorkFunctions.EnableControls(v_oContainer)
                        Case Mode.Edit
                            If String.IsNullOrEmpty(v_sOI) = True Then
                                'if OI is blank meand user is adding the controls during add client
                                LoadClientControls(Doc, v_oContainer, v_sOI, sender, False)
                            Else
                                'if OI is not blank meand user is adding the controls during edit client
                                LoadClientControls(Doc, v_oContainer, v_sOI, sender, InitializeOnly)
                            End If
                            FrameWorkFunctions.EnableControls(v_oContainer)
                        Case Mode.View
                            LoadClientControls(Doc, v_oContainer, v_sOI, sender, False)
                            FrameWorkFunctions.DisableControls(v_oContainer, True)
                    End Select
                End If
            End If
        End Sub
        ''' <summary>
        ''' Write all the Additional party controls from the provided control container to the current party object.
        ''' </summary>
        ''' <param name="v_oContainer">The control to be searched for party controls to load data to,
        ''' this is usually the masterpage container</param>
        ''' <param name="v_sScreenCode">The screencode of the current element, we need this for running the default rules</param>
        ''' <param name="v_sOI">The dataset identifier of the current element</param>
        ''' <remarks></remarks>
        Protected Sub ReadAdditionalControlsFromContainer(ByVal v_oContainer As ContentPlaceHolder,
                                    Optional ByVal v_sScreenCode As String = Nothing,
                                    Optional ByVal v_sOI As String = Nothing)
            If Current.Session(CNParty) IsNot Nothing Then
                Dim oParty As NexusProvider.BaseParty = Session.Item(CNParty)


                Select Case True
                    Case TypeOf oParty Is NexusProvider.CorporateParty
                        With CType(oParty, NexusProvider.CorporateParty)
                            For Each oControl As Control In v_oContainer.Controls
                                If oControl.ID IsNot Nothing Then
                                    Select Case oControl.ID
                                        Case "txtAlternativeId"
                                            .AlternativeId = CType(oControl, TextBox).Text
                                    End Select
                                End If
                            Next
                        End With
                    Case TypeOf oParty Is NexusProvider.PersonalParty
                        With CType(oParty, NexusProvider.PersonalParty)
                            For Each oControl As Control In v_oContainer.Controls
                                If oControl.ID IsNot Nothing Then
                                    Select Case oControl.ID
                                        Case "txtTradingName"
                                            CType(oControl, TextBox).Text = .TradingName
                                        Case "txtAltID"
                                            CType(oControl, TextBox).Text = .AlternativeID
                                    End Select
                                End If
                            Next
                        End With
                End Select

            End If
        End Sub
        ''' <summary>
        ''' Handles the add child item event from the ItemGrid, when the ItemGrid is NOT in 'inline' mode
        ''' </summary>
        ''' <param name="v_sScreenCode">ScreenCode of the child item to be added</param>
        ''' <param name="v_sPath">Location of the first child page to be redirected to</param>
        ''' <param name="v_sParentElement">Parent Element of the child item, as we need to identifier where to
        ''' place the child item within the dataset</param>
        ''' <param name="v_sChildElement">Child Element name, need for creation of the element</param>
        ''' <remarks>The current risk page form data will also be written to the dataset</remarks>
        Public Sub AddItem(ByVal v_sScreenCode As String, ByVal v_sPath As String,
                            ByVal v_sParentElement As String, ByVal v_sChildElement As String)

            'create new element in XML
            'Dim sOI As String = DataSetFunctions.CreateElementFromXML(v_sScreenCode, _
            '    oOI.Peek.ToString(), v_sParentElement, v_sChildElement)
            Dim sOI As String = CreateElementFromXML(v_sScreenCode,
                           oOI.Peek.ToString(), v_sParentElement, v_sChildElement)
            oOI.Push(sOI)
            Session.Item(CNOI) = oOI

            Response.Redirect(v_sPath, False)

        End Sub
        ''' <summary>
        ''' Handles the edit child item event from the ItemGrid when the ItemGrids is not in 'inline' mode
        ''' </summary>
        ''' <param name="v_sPath">Location of the first child page to redirect to</param>
        ''' <param name="v_sOI">Dataset child item identifier</param>
        ''' <remarks>The current form data will be written to the dataset at this point.</remarks>
        Public Sub EditItem(ByVal v_sPath As String, ByVal v_sOI As String, ByVal v_sScreenCode As String)

            oOI.Push(v_sOI)
            Session.Item(CNOI) = oOI
            Response.Redirect(v_sPath, False)

        End Sub
        ''' <summary>
        ''' Handles the child item deletion event
        ''' </summary>
        ''' <param name="v_sOI">Dataset identifier of the selected child item</param>
        ''' <param name="v_sChildElement">Element name within the dataset of the child item</param>
        ''' <remarks></remarks>
        Public Sub DeleteItem(ByVal v_sOI As String, ByVal v_sChildElement As String)

            DeleteElementFromXML(sScreenCode, v_sOI, v_sChildElement)
            ReadPartyControlsContainerFromXML(oMaster, oOI.Peek.ToString(), Me)

        End Sub
        ''' <summary>
        ''' Handles the edit child item event from the ItemGrid when in the ItemGrid is in 'inline' mode
        ''' </summary>
        ''' <param name="v_sRiskContainer">Control Id of the RiskContainer containing the child screen controls.</param>
        ''' <param name="v_sOI">Dataset child item identifier</param>
        ''' <remarks></remarks>
        Public Sub EditItemInRiskContainer(ByVal v_sRiskContainer As String, ByVal v_sOI As String)

            Dim oRiskContainer As RiskContainer = oMaster.FindControl(v_sRiskContainer)

            If oRiskContainer IsNot Nothing Then
                oRiskContainer.Mode = RiskContainer.ChildMode.Edit
                oRiskContainer.OI = v_sOI
                ReadPartyControlsContainerFromXML(oRiskContainer, v_sOI, Me, False)
            End If

        End Sub

        Function CreateElementFromXML(ByVal v_sScreenCode As String,
                                    ByVal v_sParentOI As String,
                                    ByVal v_sParentElement As String,
                                    ByVal v_sChildElement As String,
                                   Optional ByVal v_bSkipSaveToDB As Boolean = False) As String

            Dim sOI As String = String.Empty

            If Current.Session(CNParty) IsNot Nothing Then

                Dim oParty As NexusProvider.BaseParty = Session.Item(CNParty)

                'DH - 30-01-08 - Fix screen having multiple elements at the same level with child elements.

                Dim srDataset As New System.IO.StringReader(oParty.XMLDataset)
                Dim xmlTR As New XmlTextReader(srDataset)
                Dim Doc As New XmlDocument

                Doc.Load(xmlTR)
                xmlTR.Close()

                'Dim oDefaultNodes As New Hashtable()
                Dim oNode As XmlNode = Doc.SelectSingleNode("//" & v_sParentElement & "[@OI='" & v_sParentOI & "']")

                If oNode Is Nothing Then
                    'Parent Element doesn't match the Element Name returned by the OI
                    oNode = Doc.SelectSingleNode("//" & v_sParentElement)
                    v_sParentOI = oNode.Attributes("OI").Value
                End If

                '------------------------------------------------------------------------------------------

                Dim oDataset As New DataSetControl.Application

                oDataset.LoadFromXML(GetDataSetDefinition(Current.Session(CNPartyDataModelCode)), oParty.XMLDataset)
                oDataset.NewObjectInstance(v_sChildElement, sOI, v_sParentOI)
                oDataset.ReturnAsXML(oParty.XMLDataset)

                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

                Try
                    oParty.XMLDataset = oWebService.RunDefaultRulesAdd(v_sScreenCode, oParty.XMLDataset, oParty.BranchCode, v_bSkipSaveToDB)
                Finally
                    oWebService = Nothing
                End Try

                'Set the newly create element as being deleted
                srDataset = New System.IO.StringReader(oParty.XMLDataset)
                xmlTR = New XmlTextReader(srDataset)

                Doc.Load(xmlTR)
                xmlTR.Close()

                oNode = Doc.SelectSingleNode("//*[@OI = '" & sOI & "']")
                If oNode IsNot Nothing Then
                    oNode.Attributes("US").Value = "3"
                End If

                Dim swContent As New System.IO.StringWriter
                Dim xmlwContent As New XmlTextWriter(swContent)

                Doc.WriteTo(xmlwContent)
                oParty.XMLDataset = swContent.ToString()

                xmlwContent.Close()
                swContent.Close()

                Current.Session(CNParty) = oParty

            End If

            Return sOI

        End Function
        ''' <summary>
        ''' Delete an element from the risk dataset
        ''' </summary>
        ''' <param name="v_sScreenCode">The screen code of the element to be removed</param>
        ''' <param name="v_sOI">The dataset identifier of the element to be removed</param>
        ''' <param name="v_sChildElement">The element name of the element to be removed</param>
        ''' <remarks></remarks>
        Sub DeleteElementFromXML(ByVal v_sScreenCode As String,
                                ByVal v_sOI As String,
                                ByVal v_sChildElement As String)

            If Current.Session(CNParty) IsNot Nothing Then

                Dim sDataSetDefinition As String = GetDataSetDefinition(Current.Session(CNPartyDataModelCode))
                Dim oParty As NexusProvider.BaseParty = Session.Item(CNParty)
                Dim oDataSet As New DataSetControl.Application

                oDataSet.LoadFromXML(sDataSetDefinition, oParty.XMLDataset)

                sDataSetDefinition = Nothing

                oDataSet.DelObjectInstance(v_sChildElement, v_sOI)
                oDataSet.ReturnAsXML(oParty.XMLDataset)

                oDataSet.Terminate()
                oDataSet = Nothing

                Current.Session(CNParty) = oParty

            End If

        End Sub

        ''' <summary>
        ''' Read all controls within a provided conntrol container and identify the risk controls and
        ''' populate from the current risk dataset
        ''' </summary>
        ''' <param name="oDoc">An XMLDocument object of the current SAM dataset</param>
        ''' <param name="v_oContainer">The control to be searched for risk controls to load data to,
        ''' this is usually the masterpage container</param>
        ''' <param name="v_sOI">The dataset identifier of the current element</param>
        ''' <param name="sender">The object that made the request to the function, usually content placeholder</param>
        ''' <param name="InitializeOnly">Initialize the risk controls only, e.g set attributes,
        ''' hookup events but don't read the risk data into the control, mainly used on postback</param>
        ''' <remarks>This procedure is marked private because it should only ever be called by itself of from
        ''' the ReadContainerFromXML procedure</remarks>
        Public Sub LoadClientControls(ByRef oDoc As XmlDocument,
                                      ByVal v_oContainer As Control,
                                      ByVal v_sOI As String,
                                      ByVal sender As Object,
                                Optional ByVal InitializeOnly As Boolean = False)

            Dim oControl As Object
            Dim sControlName() As String 'Will be 2 or 3 elements, ELEMENT__ATTRIBUTE(__CONDITION)
            Dim sControlValue As String = String.Empty

            For Each oControl In v_oContainer.Controls
                If oControl.id IsNot Nothing Then

                    sControlName = Regex.Split(oControl.ID.ToUpper(), "__")
                    sControlValue = String.Empty


                    If sControlName.Length > 1 Then

                        Select Case oControl.GetType.Name

                            Case "HtmlInputText"
                                If Not InitializeOnly Then
                                    'If sControlValue = "" Or sControlValue Is Nothing Then
                                    '    CType(oControl, HtmlInputText).Value = ""
                                    'End If
                                    If ReadAttributeFromXML(oDoc, sControlName, sControlValue, v_sOI, Session(CNPartyDataModelCode)) Then

                                        Dim dtAttribute As DateTime
                                        If DateTime.TryParseExact(sControlValue, "yyyy-MM-dd HH:mm:ss",
                                            InvariantCulture, System.Globalization.DateTimeStyles.None, dtAttribute) Then

                                            sControlValue = dtAttribute.ToShortDateString

                                        End If

                                        CType(oControl, HtmlInputText).Value = sControlValue
                                    End If
                                End If
                                If Session(CNClientMode) = Mode.View Then
                                    CType(oControl, HtmlInputText).Attributes.Add("readonly", "true")
                                Else
                                    CType(oControl, HtmlInputText).Attributes.Remove("readonly")
                                End If

                            Case "Label"
                                If Not InitializeOnly Then
                                    'Seems odd, but can be used to display page
                                    'titles or section titles within the layout
                                    'If sControlValue = "" Or sControlValue Is Nothing Then
                                    '    CType(oControl, Label).Text = ""
                                    'End If
                                    If ReadAttributeFromXML(oDoc, sControlName, sControlValue, v_sOI, Session(CNPartyDataModelCode)) Then


                                        Dim dtAttribute As DateTime
                                        If DateTime.TryParseExact(sControlValue, "yyyy-MM-dd HH:mm:ss",
                                            InvariantCulture, System.Globalization.DateTimeStyles.None, dtAttribute) Then

                                            sControlValue = dtAttribute.ToShortDateString

                                        End If

                                        CType(oControl, Label).Text = sControlValue
                                    End If
                                End If

                            Case "TextBox"
                                If Not InitializeOnly Then
                                    'If sControlValue = "" Or sControlValue Is Nothing Then
                                    '    CType(oControl, TextBox).Text = ""
                                    'End If
                                    If ReadAttributeFromXML(oDoc, sControlName, sControlValue, v_sOI, Session(CNPartyDataModelCode)) Then

                                        Dim dtAttribute As DateTime
                                        If DateTime.TryParseExact(sControlValue, "yyyy-MM-dd HH:mm:ss",
                                            InvariantCulture, System.Globalization.DateTimeStyles.None, dtAttribute) Then

                                            sControlValue = dtAttribute.ToShortDateString

                                        End If

                                        CType(oControl, TextBox).Text = sControlValue
                                    End If
                                End If
                                If Session(CNClientMode) = Mode.View Then
                                    CType(oControl, TextBox).Attributes.Add("readonly", "true")
                                Else
                                    CType(oControl, TextBox).Attributes.Remove("readonly")
                                End If

                            Case "HiddenField"
                                If Not InitializeOnly Then
                                    If ReadAttributeFromXML(oDoc, sControlName, sControlValue, v_sOI, Session(CNPartyDataModelCode)) Then
                                        Dim dtAttribute As DateTime
                                        If DateTime.TryParseExact(sControlValue, "yyyy-MM-dd HH:mm:ss",
                                            InvariantCulture, System.Globalization.DateTimeStyles.None, dtAttribute) Then
                                            sControlValue = dtAttribute.ToShortDateString
                                        End If
                                        CType(oControl, HiddenField).Value = sControlValue
                                    End If
                                End If

                            Case "CheckBox"
                                If Not InitializeOnly Then
                                    'If sControlValue = "" Or sControlValue Is Nothing Then
                                    '    CType(oControl, CheckBox).Checked = False
                                    'End If
                                    If ReadAttributeFromXML(oDoc, sControlName, sControlValue, v_sOI, Session(CNPartyDataModelCode)) Then
                                        'if 1 then checked, anthing else not checked, including "2" unknown
                                        CType(oControl, CheckBox).Checked = IIf(sControlValue = 1, True, False)
                                    End If
                                End If
                                If Session(CNClientMode) = Mode.View Then
                                    'CType(oControl, CheckBox).Attributes.Add("readonly", "true")
                                    CType(oControl, CheckBox).Enabled = False
                                Else
                                    CType(oControl, CheckBox).Enabled = True
                                End If

                                'ADDED: WASN'T ABLE TO FIND "HtmlInputCheckBox" - MB - 01 JUNE 07
                            Case "HtmlInputCheckBox"
                                If Not InitializeOnly Then
                                    'If sControlValue = "" Or sControlValue Is Nothing Then
                                    '    CType(oControl, HtmlInputCheckBox).Checked = False
                                    'End If
                                    If ReadAttributeFromXML(oDoc, sControlName, sControlValue, v_sOI, Session(CNPartyDataModelCode)) Then
                                        'if 1 then checked, anthing else not checked, including "2" unknown
                                        CType(oControl, HtmlInputCheckBox).Checked = IIf(sControlValue = 1, True, False)
                                    End If
                                End If
                                If Session(CNClientMode) = Mode.View Then
                                    CType(oControl, HtmlInputCheckBox).Attributes.Add("readonly", "true")
                                Else
                                    CType(oControl, HtmlInputCheckBox).Attributes.Remove("readonly")
                                End If

                            Case "RadioButtonList"
                                If Not InitializeOnly Then
                                    'If sControlValue = "" Or sControlValue Is Nothing Then
                                    '    CType(oControl, RadioButtonList).SelectedValue = "0"
                                    'End If
                                    If ReadAttributeFromXML(oDoc, sControlName, sControlValue, v_sOI, Session(CNPartyDataModelCode)) Then
                                        If sControlValue = "0" Or sControlValue = "1" Then
                                            CType(oControl, RadioButtonList).SelectedValue = sControlValue
                                        End If
                                    End If
                                End If
                                If Session(CNClientMode) = Mode.View Then
                                    CType(oControl, RadioButtonList).Enabled = False
                                Else
                                    CType(oControl, RadioButtonList).Enabled = True
                                End If

                            Case "LookupList"
                                'Check for a value set by a parent control, if so use as the ParentKey
                                'on the LookupList, this will filter the results according to the ParentKey
                                sControlValue = Current.Session.Item(oControl.ID)

                                If sControlValue IsNot Nothing Then
                                    oControl.ParentKey = sControlValue
                                ElseIf oControl IsNot Nothing AndAlso Not InitializeOnly Then
                                    oControl.Value = DirectCast(oControl, NexusProvider.LookupList).Value
                                End If

                                If Not InitializeOnly Then
                                    If ReadAttributeFromXML(oDoc, sControlName, sControlValue, v_sOI, Session(CNPartyDataModelCode)) Then
                                        oControl.Value = sControlValue
                                    End If
                                End If
                            Case "LookupListV2"
                                'Check for a value set by a parent control, if so use as the ParentKey
                                'on the LookupList, this will filter the results according to the ParentKey
                                sControlValue = Current.Session.Item(oControl.ID)

                                If sControlValue IsNot Nothing Then
                                    oControl.ParentKey = sControlValue
                                ElseIf oControl IsNot Nothing AndAlso Not InitializeOnly Then
                                    oControl.Value = DirectCast(oControl, NexusProvider.LookupListV2).Value
                                End If

                                If Not InitializeOnly Then
                                    If ReadAttributeFromXML(oDoc, sControlName, sControlValue, v_sOI, Session(CNPartyDataModelCode)) Then
                                        oControl.Value = sControlValue
                                    End If
                                End If


                            Case "LookupListV2"
                                'Check for a value set by a parent control, if so use as the ParentKey
                                'on the LookupList, this will filter the results according to the ParentKey
                                sControlValue = Current.Session.Item(oControl.ID)

                                If sControlValue IsNot Nothing Then
                                    oControl.ParentKey = sControlValue
                                Else
                                    oControl.Value = ""
                                End If

                                If Not InitializeOnly Then
                                    If ReadAttributeFromXML(oDoc, sControlName, sControlValue, v_sOI, Session(CNPartyDataModelCode)) Then
                                        oControl.Value = sControlValue
                                    End If
                                End If
                                If Session(CNClientMode) = Mode.View Then
                                    CType(oControl, NexusProvider.LookupList).Attributes.Add("readonly", "true")
                                Else
                                    CType(oControl, NexusProvider.LookupList).Attributes.Remove("readonly")
                                End If

                            Case "Panel"

                                'If Session(CNOI) IsNot Nothing AndAlso Session(CNOI).count > 0 Then
                                '    v_sOI = Session(CNOI).Peek.ToString()
                                'Else
                                '    v_sOI = String.Empty
                                'End If

                                LoadClientControls(oDoc, oControl, v_sOI, sender, InitializeOnly)
                            Case "RiskContainer"
                                If Session(CNParty) IsNot Nothing Then
                                    Dim oOI As String = v_sOI
                                    Dim oParty As NexusProvider.BaseParty = Session(CNParty)

                                    'Load the default
                                    If Session(CNClientMode) <> Mode.View Then
                                        oOI = LoadChildDefaultItem(CType(oControl, RiskContainer).ScreenCode, sControlName(0), sControlName(1))
                                        ' Doc is again populated for only newly added child with defaults
                                        Dim xmlTR As New XmlTextReader(New System.IO.StringReader(oParty.XMLDataset))
                                        Dim Doc As New XmlDocument

                                        Doc.Load(xmlTR)
                                        xmlTR.Close()
                                        'Reading the defaults from xml

                                        CType(oControl, RiskContainer).ParentElement = sControlName(0)
                                        CType(oControl, RiskContainer).ChildElement = sControlName(1)
                                        LoadClientControls(oDoc, oControl, v_sOI, sender, True)
                                        LoadClientControls(Doc, oControl, oOI, sender)

                                        'Cleanup up the xml, deleting the newly added child from xml
                                        RemoveInvalidNodeFromXML(oOI)
                                    End If
                                End If
                            Case "ItemGrid"

                                If String.IsNullOrEmpty(CType(oControl, ItemGrid).ChildContainer) Then
                                    AddHandler CType(oControl, ItemGrid).EditItem, AddressOf CType(sender, BaseClient).EditItem
                                Else
                                    AddHandler CType(oControl, ItemGrid).EditItemInRiskContainer, AddressOf CType(sender, BaseClient).EditItemInRiskContainer
                                End If

                                AddHandler CType(oControl, ItemGrid).AddItem, AddressOf CType(sender, BaseClient).AddItem
                                If Session(CNClientMode) <> Mode.View Then
                                    AddHandler CType(oControl, ItemGrid).DeleteItem, AddressOf CType(sender, BaseClient).DeleteItem
                                End If

                                Dim oItemGrid As ItemGrid = CType(oControl, ItemGrid)

                                If Session(CNClientMode) = Mode.View Then
                                    CType(oItemGrid.Columns(oItemGrid.Columns.Count - 1), CommandField).ShowDeleteButton = False
                                    CType(oItemGrid.Columns(oItemGrid.Columns.Count - 1), CommandField).ShowEditButton = False
                                    CType(oItemGrid.Columns(oItemGrid.Columns.Count - 1), CommandField).ShowSelectButton = True
                                    'CType(oControl, ItemGrid).AutoGenerateSelectButton = True
                                Else
                                    CType(oItemGrid.Columns(oItemGrid.Columns.Count - 1), CommandField).ShowDeleteButton = True
                                    CType(oItemGrid.Columns(oItemGrid.Columns.Count - 1), CommandField).ShowEditButton = True
                                    CType(oItemGrid.Columns(oItemGrid.Columns.Count - 1), CommandField).ShowSelectButton = False
                                End If

                                'DH - 09-01-08 - Always bind the grid as even when its empty we need the empty row footer
                                'creating, as this isn't created without a databind, so removed the InitializeOnly check
                                'If Not InitializeOnly Then

                                Dim oXMLSource As New XmlDataSource
                                oXMLSource.EnableCaching = False 'why? why? why? why would you enable caching as default
                                '"OI2" 
                                If ReadElementListFromXML(sControlName, oXMLSource, v_sOI) Then
                                    oControl.DataSource = oXMLSource
                                End If

                                oControl.DataBind()

                                oXMLSource = Nothing

                                'End If

                            Case "controls_addresscntrl_ascx"
                                If Not InitializeOnly Then

                                    'Dim sParentOI As String = GetElementOI(oDoc, sControlName(0), v_sOI)

                                    'If sParentOI IsNot Nothing Then
                                    If ReadAttributeFromXML(oDoc, sControlName, sControlValue, v_sOI, Session(CNPartyDataModelCode)) Then
                                        oControl.AddressKey = sControlValue

                                    End If
                                    'End If
                                End If
                        End Select
                    End If
                End If
            Next

        End Sub

        ''' <summary>
        ''' Adds a new child node with default values
        ''' </summary>
        ''' <param name="v_sScreenCode"></param>
        ''' <param name="v_sParentElement"></param>
        ''' <param name="v_sChildElement"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function LoadChildDefaultItem(ByVal v_sScreenCode As String, ByVal v_sParentElement As String, ByVal v_sChildElement As String) As String
            Dim oRiskContainer As New RiskContainer

            If Session(CNOI) IsNot Nothing AndAlso oOI.Count = 0 Then
                oOI = Session(CNOI)
            End If

            oRiskContainer.OI = CreateElementFromXML(v_sScreenCode, oOI.Peek, v_sParentElement, v_sChildElement, True)

            Return oRiskContainer.OI
        End Function

        ''' <summary>
        ''' Read all the child elements of an element with a matching element name, this used to
        ''' popualte the ItemGrid control
        ''' </summary>
        ''' <param name="v_sControlName">The parent and child element names, as defined by the ItemGrid control ID</param>
        ''' <param name="r_oXMLDataSource">An XMLDataSource to return the retrieved data</param>
        ''' <param name="v_sOI">The dataset identifier of the parent element from which we are retrieving the child elements</param>
        ''' <returns>Success or Failure of the data retrieval</returns>
        ''' <remarks></remarks>
        Function ReadElementListFromXML(ByVal v_sControlName() As String,
                                        ByRef r_oXMLDataSource As XmlDataSource,
                                        ByVal v_sOI As String) As Boolean

            Dim bSuccess As Boolean = False

            If Current.Session(CNParty) IsNot Nothing Then

                Dim oParty As NexusProvider.BaseParty = Session.Item(CNParty)
                Dim xmlTR As New XmlTextReader(New System.IO.StringReader(oParty.XMLDataset))
                Dim oDoc As New XmlDocument

                oDoc.Load(xmlTR)
                xmlTR.Close()

                Dim sXPath As String = String.Empty

                Dim oNode As XmlNode = oDoc.SelectSingleNode("//*[@OI='" & v_sOI & "']")
                If oNode IsNot Nothing Then

                    If oNode.Name.Equals(v_sControlName(0)) Then

                        'element name retrieved with the OI matches that passed in, so we are in the right element to read attributes
                        sXPath = ".//" & v_sControlName(0) & "[@OI='" & v_sOI & "']/" & v_sControlName(1)

                    Else

                        'Not matching, so we go back up the tree one level and look for the element amongst the siblings
                        Dim oOI As Collections.Stack = Current.Session.Item(CNOI)
                        v_sOI = oOI.Pop()

                        sXPath = ".//*[@OI='" & oOI.Peek() & "']/" & v_sControlName(0) & "/" & v_sControlName(1)

                        'DH - Copying a stack from session does it byref, so the previous OI popped needs to be pushed back in
                        oOI.Push(v_sOI)
                        Current.Session.Item(CNOI) = oOI

                    End If

                End If

                r_oXMLDataSource.Data = oParty.XMLDataset
                r_oXMLDataSource.XPath = sXPath

                bSuccess = True

            End If

            Return bSuccess

        End Function

#End Region

        Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            If Not Page.IsPostBack OrElse Request.QueryString("PostBack") = "RibbonMenu" Then
                If (Session(CNNoTrans) IsNot Nothing) AndAlso oMaster.FindControl("hdnManualTransfer") IsNot Nothing Then
                    Dim hdnManualTransfer As HiddenField = CType(oMaster.FindControl("hdnManualTransfer"), HiddenField)
                    hdnManualTransfer.Value = Session(CNNoTrans).ToString()
                End If

                If oMaster.FindControl("ddlBranchCode") IsNot Nothing Then 'If BranchCode field is there than bind this control
                    Dim dllBranchCode As DropDownList = CType(oMaster.FindControl("ddlBranchCode"), DropDownList)
                    Dim oBranchs As NexusProvider.BranchCollection = CType(Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches
                    If oBranchs IsNot Nothing Then
                        'Sort collection before binding
                        oBranchs.SortColumn = "Description"
                        oBranchs.SortingOrder = NexusProvider.GenericComparer.SortOrder.Ascending
                        oBranchs.Sort()

                        dllBranchCode.DataSource = oBranchs
                        dllBranchCode.DataBind()
                        If Session(CNBranchCode) IsNot Nothing Then
                            If dllBranchCode.Items.FindByValue(Session(CNBranchCode)) IsNot Nothing Then
                                dllBranchCode.SelectedValue = Session(CNBranchCode)
                            Else
                                ' Cross-branch: session branch not in user list, add it to dropdown
                                dllBranchCode.Items.Add(New ListItem(Session(CNBranchCode).ToString(), Session(CNBranchCode).ToString()))
                                dllBranchCode.SelectedValue = Session(CNBranchCode)
                            End If
                        End If
                        If oMaster.FindControl("ddlCurrency") IsNot Nothing Then
                            dllBranchCode.AutoPostBack = True
                        Else
                            dllBranchCode.AutoPostBack = False
                        End If
                    End If
                End If
                If oMaster.FindControl("ddlCurrency") IsNot Nothing Then 'If CurrencyCode field is there than bind this control
                    Dim dllCurrency As DropDownList = CType(oMaster.FindControl("ddlCurrency"), DropDownList)
                    'Currency will be populated with BaseCurrency set in Bo
                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim oCurrencyColl As NexusProvider.CurrencyCollection
                    If oMaster.FindControl("ddlBranchCode") IsNot Nothing Then
                        Dim dBranchCode As DropDownList = oMaster.FindControl("ddlBranchCode")
                        oCurrencyColl = oWebService.GetCurrenciesByBranch(dBranchCode.SelectedValue)
                    Else
                        oCurrencyColl = oWebService.GetCurrenciesByBranch(Session(CNBranchCode))
                    End If

                    'sort the collection before binding
                    oCurrencyColl.SortColumn = "Description"
                    oCurrencyColl.SortingOrder = NexusProvider.GenericComparer.SortOrder.Ascending
                    oCurrencyColl.Sort()
                    dllCurrency.Items.Clear()
                    For i As Integer = 0 To oCurrencyColl.Count - 1
                        Dim lstCurrency As New ListItem
                        lstCurrency.Text = oCurrencyColl.Item(i).Description.ToString
                        lstCurrency.Value = Trim(oCurrencyColl.Item(i).CurrencyCode.ToString)
                        dllCurrency.Items.Add(lstCurrency)
                    Next
                    dllCurrency.DataBind()
                    dllCurrency.SelectedValue = oCurrencyColl(0).BaseCurrencyCode
                End If
            End If
            'The session needs to be cleared when user redirects from the Claim overwiew page using the "Client Name" link 
             If Not isModalOtherParty AndAlso (Session(CNDoNotClearSession) Is Nothing OrElse Session(CNDoNotClearSession) <> "true") Then
                Session.Remove(CNClaim)
                Session.Remove(CNInsuranceFileKey)
            End If
        End Sub

        Private Shadows Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            oMaster = GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName)

            'In case of MTA, personal / corporate client information will be displayed in Modal dialog.
            Session(CNRequestType) = Request.QueryString(CNRequestType)
            If Session(CNRequestType) IsNot Nothing AndAlso Session(CNRequestType) = "MTA" Then
                CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
            ElseIf Session(CNRequestType) IsNot Nothing AndAlso Session(CNRequestType) = "REN" Then
                CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
            End If

            'Fill the Master Page in the oMaster Object.
            oNexusConfig = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            oMaster = GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName)

            oPartyChildScreenPanel = oMaster.FindControl("PANEL__PARTYCHILDSCREEN") ' check for partychildscreen exist
            oPartyBuilderPanel = oMaster.FindControl("PANEL__PARTYBUILDER") ' check for partybuilder exist
            oPartyChildScreenPanelForView = oMaster.FindControl("PANEL__PARTYCHILDSCREEN__DUPE") ' check for partychildscreen exist for view panel
            oPartyBuilderPanelForView = oMaster.FindControl("PANEL__PARTYBUILDER__DUPE") ' check for partybuilder exist for view panel

            If Request.Url.ToString.ToUpper.Contains("/MODAL") AndAlso Request.Url.ToString.ToUpper.Contains("/OTHERPARTYDETAILS") Then
                isModalOtherParty = True
            Else
                isModalOtherParty = False
            End If

        End Sub

        Private Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'This section is used for Duplicate Client CHeck - Start
            If Request("__EVENTARGUMENT") = "Refresh" Then
                Page.ClientScript.GetPostBackEventReference(Me, "")
                If Session(CNParty) IsNot Nothing AndAlso DirectCast(Session(CNParty), NexusProvider.BaseParty) IsNot Nothing Then
                    Dim oPartyCollection As NexusProvider.PartyCollection = Session(CNSearchData)
                    Dim sUrl As String = Nothing

                    If oPartyCollection IsNot Nothing Then

                        For iCounter As Integer = 0 To oPartyCollection.Count - 1
                            If oPartyCollection(iCounter).Key = DirectCast(Session(CNParty), NexusProvider.BaseParty).Key Then
                                Select Case True
                                    Case TypeOf oPartyCollection(iCounter) Is NexusProvider.PersonalParty
                                        sUrl = "~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & oPartyCollection(iCounter).Key & "&Code=" & oPartyCollection(iCounter).UserName & ""
                                    Case TypeOf oPartyCollection(iCounter) Is NexusProvider.CorporateParty
                                        sUrl = "~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oPartyCollection(iCounter).Key & "&Code=" & oPartyCollection(iCounter).UserName & ""
                                End Select
                                Exit For
                            End If
                        Next
                    Else
                        Select Case True
                            Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                                sUrl = "~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & DirectCast(Session(CNParty), NexusProvider.PersonalParty).Key & "&Code=" & DirectCast(Session(CNParty), NexusProvider.PersonalParty).UserName & ""
                            Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                                sUrl = "~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & DirectCast(Session(CNParty), NexusProvider.CorporateParty).Key & "&Code=" & DirectCast(Session(CNParty), NexusProvider.CorporateParty).UserName & ""
                        End Select
                    End If
                    Session(CNClientMode) = Mode.View
                    Response.Redirect(sUrl, False)
                Else
                    AddParty(sender, e)
                End If
            ElseIf Request("__EVENTARGUMENT") = "Ignore" Then
                AddParty(sender, e)
            End If
            'This section is used for Duplicate Client CHeck - End

            If Not Page.IsPostBack Then
                oOI = New Collections.Stack()
                Session.Item(CNOI) = oOI
                OnPageLoadCall()
            End If

            oOI = Session.Item(CNOI)

            If oOI Is Nothing Then
                oOI = New Collections.Stack()
            End If

            Session.Item(CNOI) = oOI

            If Request.Form("__EVENTTARGET") IsNot Nothing AndAlso Request.Form("__EVENTTARGET").Contains("btnEditClient") Then
                Session(CNClientMode) = Mode.Edit
            End If

            ShowHideStatementButton()

            If IsPostBack Then
                If oPartyBuilderPanel IsNot Nothing Or oPartyChildScreenPanel IsNot Nothing Then
                    'Dont load the data into the controls, just initialize them
                    If oOI.Count > 0 Then
                        ReadPartyControlsContainerFromXML(oMaster, oOI.Peek, Me, True)
                    Else
                        ReadPartyControlsContainerFromXML(oMaster, String.Empty, Me, True)
                    End If
                End If
            Else
                If oPartyBuilderPanel IsNot Nothing Or oPartyChildScreenPanel IsNot Nothing Then
                    If oOI.Count > 0 Then
                        ReadPartyControlsContainerFromXML(oMaster, oOI.Peek, Me)
                    Else
                        ReadPartyControlsContainerFromXML(oMaster, String.Empty, Me)
                    End If
                End If

                'When ShowCountry option is set true in Portal level configuration of AddressControl 
                'and also when control is present in the page, then make it visible.
                If oMaster.FindControl("licountry") IsNot Nothing Then
                    If CType(WebConfigurationManager.GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID().ToString()).AddressControl.ShowCountry = True Then
                        CType(oMaster.FindControl("licountry"), HtmlGenericControl).Visible = True
                    Else
                        CType(oMaster.FindControl("licountry"), HtmlGenericControl).Visible = False
                    End If
                End If

                If Session(CNClientMode) = Mode.Edit Then
                    BtnEditClientClick(sender, e)
                End If
            End If

            If Session(CNClientMode) = Mode.View Then
                'When ShowCountry option is set true in Portal level configuration of AddressControl 
                'and also when control is present in the page, then make it visible.
                If oMaster.FindControl("licountry") IsNot Nothing Then
                    If CType(WebConfigurationManager.GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID().ToString()).AddressControl.ShowCountry = True Then
                        CType(oMaster.FindControl("licountry"), HtmlGenericControl).Visible = True
                    Else
                        CType(oMaster.FindControl("licountry"), HtmlGenericControl).Visible = False
                    End If
                End If


                'Loads personal client information in view mode.
                LoadClient()
                ' If Agent is doing a QQ without selecting a Client(CNAnonymousUser)
                If Session.Item(CNQuoteMode) = QuoteMode.QuickQuote And Session(CNAnonymous) IsNot Nothing _
                And Session(CNIsAnonymous) = True Then

                    Session.Remove(CNAnonymous)
                    Response.Redirect("~/QQPremium.aspx", False)

                ElseIf Session.Item(CNQuoteMode) = QuoteMode.FullQuote _
                    And Session(CNIsAnonymous) = True Then

                    'Transfer anonymous Quote to selected party and get updated quote to populate Session(CNQuote)
                    TransferQuoteToSelectedParty()

                    Select Case Session(CNRedirectedFor)
                        ' If user is redirected by Save Quote button from Premium display page and Session(CNIsAnonymous) is true
                        Case "SaveQuote"

                            If CType(Session.Item(CNLoginType), LoginType) = LoginType.Agent Then

                                Dim oParty As NexusProvider.BaseParty = Session(CNParty)

                                Select Case True
                                    Case TypeOf oParty Is NexusProvider.PersonalParty
                                        Response.Redirect("~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & oParty.Key.ToString() & "&Code=" & oParty.UserName, False)
                                    Case TypeOf oParty Is NexusProvider.CorporateParty
                                        Response.Redirect("~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oParty.Key.ToString() & "&Code=" & oParty.UserName, False)
                                End Select

                                oParty = Nothing

                            End If
                            ' If user is redirected by Buy Now button from Premium display page and Session(CNIsAnonymous) is true
                        Case "BuyQuote"

                            ''Redirect to statement or transaction confirmation page       
                            Dim bRiskDeleted As Boolean = False
                            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                            Dim iTotalRiskCount As Integer = oQuote.Risks.Count
                            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                            Dim dTatalPremium As Decimal

                            'Remove unquoted risk from quote object before making it live
                            For iCount As Integer = 0 To iTotalRiskCount - 1
                                If bRiskDeleted = True Then
                                    bRiskDeleted = False
                                    iCount -= 1
                                End If
                                If iCount < iTotalRiskCount AndAlso oQuote.Risks(iCount).IsRisk = False Then
                                    oWebService.DeleteRisk(oQuote, iCount)
                                    oQuote.Risks.Remove(iCount)
                                    bRiskDeleted = True
                                    iTotalRiskCount = oQuote.Risks.Count
                                End If
                            Next

                            'Update Session with current oQuote object
                            Session(CNQuote) = oQuote

                            If oQuote.Risks.Count > 0 Then
                                dTatalPremium = oQuote.GrossTotal
                            End If

                            'This will allow Zero Premium to be transacted in Case of NB
                            If (dTatalPremium = 0.0) Then
                                Session(CNMode) = Mode.Edit
                                Session(CNQuoteInSync) = False
                                Session.Remove(CNOI)
                                Session(CNPaid) = True
                                Response.Redirect("~/secure/TransactionConfirmation.aspx", False)
                            End If

                            Response.Redirect("~/secure/Statements.aspx", False)

                    End Select
                End If

            ElseIf Session(CNClientMode) = Mode.Edit And Not Page.IsPostBack Then
                BtnEditClientClick(sender, e)
            End If
            If IsPostBack Then
                ReadPartyControlsContainerForAll(True)
            Else
                ReadPartyControlsContainerForAll(False)

                If Session(CNClientMode) = Mode.Edit Then
                    BtnEditClientClick(sender, e)
                End If
            End If

        End Sub

        ''' <summary>
        ''' The entry point for reading the all party builder risk controls(view+add/edit)
        ''' </summary>
        ''' <param name="bInitilaizeOnly">Initialize the risk controls only, e.g set attributes,
        ''' hookup events but don't read the risk data into the control, mainly used on postback</param>
        ''' <remarks></remarks>
        Private Sub ReadPartyControlsContainerForAll(ByVal bInitilaizeOnly As Boolean)
            'get updated oOI
            oOI = Session.Item(CNOI)

            'Load controls for View Panel
            If oPartyBuilderPanelForView IsNot Nothing Then
                'Dont load the data into the controls, just initialize them
                If oOI.Count > 0 Then
                    ReadPartyControlsContainerFromXML(oPartyBuilderPanelForView, oOI.Peek, Me, bInitilaizeOnly)
                Else
                    ReadPartyControlsContainerFromXML(oPartyBuilderPanelForView, String.Empty, Me, bInitilaizeOnly)
                End If
            End If

            'get updated oOI
            oOI = Session.Item(CNOI)

            If oPartyChildScreenPanelForView IsNot Nothing Then
                'Dont load the data into the controls, just initialize them
                If oOI.Count > 0 Then
                    ReadPartyControlsContainerFromXML(oPartyChildScreenPanelForView, oOI.Peek, Me, bInitilaizeOnly)
                Else
                    ReadPartyControlsContainerFromXML(oPartyChildScreenPanelForView, String.Empty, Me, bInitilaizeOnly)
                End If
            End If
            'get updated oOI
            oOI = Session.Item(CNOI)

            'Load controls for Edit/Add Panel
            If oPartyBuilderPanel IsNot Nothing Then
                'Dont load the data into the controls, just initialize them
                If oOI IsNot Nothing AndAlso oOI.Count > 0 Then
                    ReadPartyControlsContainerFromXML(oPartyBuilderPanel, oOI.Peek, Me, bInitilaizeOnly)
                Else
                    ReadPartyControlsContainerFromXML(oPartyBuilderPanel, String.Empty, Me, bInitilaizeOnly)
                End If
            End If
            'get updated oOI
            oOI = Session.Item(CNOI)

            If oPartyChildScreenPanel IsNot Nothing Then
                'Dont load the data into the controls, just initialize them
                If oOI IsNot Nothing AndAlso oOI.Count > 0 Then
                    ReadPartyControlsContainerFromXML(oPartyChildScreenPanel, oOI.Peek, Me, bInitilaizeOnly)
                Else
                    ReadPartyControlsContainerFromXML(oPartyChildScreenPanel, String.Empty, Me, bInitilaizeOnly)
                End If
            End If

        End Sub


        ''' <summary>
        ''' Checks for mode (add / edit /view) from Query string
        ''' </summary>
        ''' <remarks></remarks>
        Sub OnPageLoadCall()
            'Store the mode in the viewstate as it saves the repeated
            'round trips to the server just to change the mode
            Select Case Request("mode")
                Case "add"
                    Session(CNClientMode) = Mode.Add
                    Session(CNParty) = Nothing
                    ClearSessionValues()
                    'Initialize the oParty Object for add only
                    Dim RequestedPageURL As String = Request.Url.Segments(Request.Url.Segments.Length - 1).ToString

                    If Current.Session(CNParty) Is Nothing Then
                        'New Client
                        Dim oParty As New NexusProvider.BaseParty
                        If RequestedPageURL.ToUpper.Contains("PERSONALCLIENTDETAILS.ASPX") = True Then
                            '    Case "PersonalClient"
                            oParty = New NexusProvider.PersonalParty()
                            Session(CNParty) = oParty
                        ElseIf RequestedPageURL.ToUpper.Contains("CORPORATECLIENTDETAILS.ASPX") = True Then
                            '    Case "CorporateClient"
                            oParty = New NexusProvider.CorporateParty()
                            Session(CNParty) = oParty
                        ElseIf RequestedPageURL.ToUpper.Contains("OTHERPARTYDETAILS") = True AndAlso
                            RequestedPageURL.ToUpper.Contains(".ASPX") = True Then
                            '    Case "OtherParty"
                            oParty = New NexusProvider.OtherParty()
                            Session(CNParty) = oParty
                        End If
                    End If

                Case "edit"
                    Session(CNClientMode) = Mode.Edit

                Case "view"
                    Session(CNClientMode) = Mode.View

                Case Else
                    If Session(CNClientMode) Is Nothing Then
                        Session(CNClientMode) = Mode.View
                    End If

            End Select

            SetPageProgress(2)
        End Sub

        ''' <summary>
        ''' Sort out the visiblity of form elements according to the current client mode
        ''' and load the client details if in view mode
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            Select Case Session(CNClientMode)
                Case Mode.Edit ' Load personal client in edit mode
                    Session(CNClientMode) = Mode.Edit
                    'not visible during Edit Mode
                    For Each oControl As Control In oMaster.Controls
                        Select Case oControl.ID
                            Case "ctrlNewQuote", "pnlQuotesPolicies", "pnlClientClaims", "pnlClientEvents", "pnlClientAccounts",
                                 "btnEditClient", "lblNewPCClient", "pnlAddCC", "pnlViewPC", "btnAddClient", "btnUpdate",
                                 "pnlAddPC", "pnlViewCC", "lblNewCClient", "ctrlNewQuoteImproved", "cmdExtractClientData", "pnlViewOP"
                                oControl.Visible = False
                            Case "lblEditClient", "PnlRegisterPC", "PnlRegisterCC", "btnSubmit", "btnCancel", "pnlRegisterOP"
                                oControl.Visible = True
                                If oControl.ID = "btnSubmit" Then
                                    CType(oControl, LinkButton).Text = GetLocalResourceObject("lbl_UpdateClient").ToString() '"Update Client"
                                End If

                                'Cancel button should not be visible if pages is request during MTA or renewal
                                If oControl.ID = "btnCancel" And Session(CNRequestType) IsNot Nothing AndAlso Session(CNRequestType) = "MTA" Then
                                    oControl.Visible = False
                                ElseIf oControl.ID = "btnCancel" And Session(CNRequestType) IsNot Nothing AndAlso Session(CNRequestType) = "REN" Then
                                    oControl.Visible = False
                                End If
                            Case "lblReviewText"
                                CType(oControl, Label).Text = GetLocalResourceObject("lbl_update_client").ToString() '"Update Client"
                        End Select
                    Next

                Case Mode.View ' Loads personal client in view mode

                    Session(CNClientMode) = Mode.View
                    'Checks whether User can do New Business task
                    If UserCanDoTask("NewBusiness") And oMaster.FindControl("ctrlNewQuote") IsNot Nothing Then
                        'if user can do the New Bussiness and the control exist
                        If oPortal.UseCorePolicyHeader = True Then
                            If oMaster.FindControl("ctrlNewQuoteImproved") IsNot Nothing Then
                                oMaster.FindControl("ctrlNewQuoteImproved").Visible = True
                            End If
                            If oMaster.FindControl("ctrlNewQuote") IsNot Nothing Then
                                oMaster.FindControl("ctrlNewQuote").Visible = False
                            End If
                        Else
                            If oMaster.FindControl("ctrlNewQuoteImproved") IsNot Nothing Then
                                oMaster.FindControl("ctrlNewQuoteImproved").Visible = False
                            End If
                            If oMaster.FindControl("ctrlNewQuote") IsNot Nothing Then
                                oMaster.FindControl("ctrlNewQuote").Visible = True
                            End If
                        End If
                    ElseIf oMaster.FindControl("ctrlNewQuote") IsNot Nothing Then
                        'if control exist than make it visible false
                        If oMaster.FindControl("ctrlNewQuote") IsNot Nothing Then
                            oMaster.FindControl("ctrlNewQuote").Visible = False
                        End If
                        If oMaster.FindControl("ctrlNewQuoteImproved") IsNot Nothing Then
                            oMaster.FindControl("ctrlNewQuoteImproved").Visible = False
                        End If
                    End If

                    For Each oControl As Control In oMaster.Controls
                        Select Case oControl.ID
                            Case "pnlAddCC", "PnlRegisterPC", "btnSubmit", "lblNewPCClient",
                                 "lblReviewText", "btnUpdate", "PnlRegisterCC", "lblNewCClient", "btnCancel", "pnlRegisterOP"
                                oControl.Visible = False

                                'Cancel button should not be visible if pages is request during MTA or renewal
                                If oControl.ID = "btnCancel" And Session(CNRequestType) IsNot Nothing AndAlso Session(CNRequestType) = "MTA" Then
                                    oControl.Visible = False
                                ElseIf oControl.ID = "btnCancel" And Session(CNRequestType) IsNot Nothing AndAlso Session(CNRequestType) = "REN" Then
                                    oControl.Visible = False
                                End If

                            Case "pnlQuotesPolicies", "pnlClientClaims", "pnlClientEvents", "pnlClientAccounts",
                                 "pnlViewPC", "pnlViewCC", "btnAddClient", "lblViewClient", "pnlViewOP"
                                oControl.Visible = True
                        End Select
                    Next

                    If oMaster.FindControl("cmdExtractClientData") IsNot Nothing Then
                        'PBI 39413: Check CanExtractClientData authority before showing button
                        Dim oExtractAuthority As New NexusProvider.UserAuthority
                        oExtractAuthority.UserCode = Session(CNLoginName)
                        oExtractAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.CanExtractClientData
                        Dim oExtractWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                        oExtractWebService.GetUserAuthorityValue(oExtractAuthority)
                        If oExtractAuthority.UserAuthorityValue = "1" Then
                            oMaster.FindControl("cmdExtractClientData").Visible = True
                            DirectCast(oMaster.FindControl("cmdExtractClientData"), LinkButton).Attributes.Add("OnClick", String.Format("javascript: tb_show(null , '{0}/Modal/ExtractFilePassword.aspx?modal=true&KeepThis=true&TB_iframe=true' , null);return false;", AppSettings("WebRoot")))
                        Else
                            oMaster.FindControl("cmdExtractClientData").Visible = False
                        End If
                    End If

                    Dim oParty As NexusProvider.BaseParty = Nothing

                    Select Case True
                        Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                            oParty = CType(Session(CNParty), NexusProvider.PersonalParty)
                            With CType(oParty, NexusProvider.PersonalParty)
                                If oMaster.FindControl("lblName_view") IsNot Nothing Then
                                    CType(oMaster.FindControl("lblName_view"), Label).Text = .Title & " " & .Forename & " " & .Lastname
                                End If
                                If oMaster.FindControl("lblGender_view") IsNot Nothing Then
                                    CType(oMaster.FindControl("lblGender_view"), Label).Text = .GenderCode
                                End If
                                If oMaster.FindControl("lblNationality_view") IsNot Nothing Then
                                    CType(oMaster.FindControl("lblNationality_view"), Label).Text = .NationalityCode
                                End If
                                If oMaster.FindControl("lblServiceLevel_view") IsNot Nothing Then
                                    CType(oMaster.FindControl("lblServiceLevel_view"), Label).Text = .ServiceLevelCode
                                End If
                                If oMaster.FindControl("lblMaritalStatus_view") IsNot Nothing Then
                                    If .MaritalStatusCode = NexusProvider.MaritalStatusCodeTypes.Married Then
                                        CType(oMaster.FindControl("lblMaritalStatus_view"), Label).Text = "Married"
                                    ElseIf .MaritalStatusCode = NexusProvider.MaritalStatusCodeTypes.CommonLaw Then
                                        CType(oMaster.FindControl("lblMaritalStatus_view"), Label).Text = "Married - Common Law"
                                    ElseIf .MaritalStatusCode = NexusProvider.MaritalStatusCodeTypes.Divorced Then
                                        CType(oMaster.FindControl("lblMaritalStatus_view"), Label).Text = "Divorced"
                                    ElseIf .MaritalStatusCode = NexusProvider.MaritalStatusCodeTypes.NotApplicable Then
                                        CType(oMaster.FindControl("lblMaritalStatus_view"), Label).Text = "Not Applicable"
                                    ElseIf .MaritalStatusCode = NexusProvider.MaritalStatusCodeTypes.NotAvailable Then
                                        CType(oMaster.FindControl("lblMaritalStatus_view"), Label).Text = "Not Available"
                                    ElseIf .MaritalStatusCode = NexusProvider.MaritalStatusCodeTypes.Partnered Then
                                        CType(oMaster.FindControl("lblMaritalStatus_view"), Label).Text = "Partnered"
                                    ElseIf .MaritalStatusCode = NexusProvider.MaritalStatusCodeTypes.Separated Then
                                        CType(oMaster.FindControl("lblMaritalStatus_view"), Label).Text = "Separated"
                                    ElseIf .MaritalStatusCode = NexusProvider.MaritalStatusCodeTypes.Single_ Then
                                        CType(oMaster.FindControl("lblMaritalStatus_view"), Label).Text = "Single"
                                    ElseIf .MaritalStatusCode = NexusProvider.MaritalStatusCodeTypes.Widowed Then
                                        CType(oMaster.FindControl("lblMaritalStatus_view"), Label).Text = "Widowed"
                                    End If
                                End If

                                If oMaster.FindControl("lblPrimaryOccupation_view") IsNot Nothing Then
                                    CType(oMaster.FindControl("lblPrimaryOccupation_view"), Label).Text = .OccupationCode
                                End If
                                If oMaster.FindControl("lblSecondaryOccupation_view") IsNot Nothing Then
                                    CType(oMaster.FindControl("lblSecondaryOccupation_view"), Label).Text = .SecOccupationCode
                                End If

                                If oMaster.FindControl("lblDOB_view") IsNot Nothing Then
                                    If Not .DateOfBirth.ToShortDateString.Contains("1899") AndAlso
                                     Not .DateOfBirth = Date.MinValue Then
                                        CType(oMaster.FindControl("lblDOB_view"), Label).Text = .DateOfBirth
                                    End If

                                End If
                                If oMaster.FindControl("lblFileCode_view") IsNot Nothing Then
                                    If oPortal.AllowFileCodeField = True Then
                                        CType(oMaster.FindControl("lblFileCode_view"), Label).Text = .FileCode
                                    Else
                                        oMaster.FindControl("liFileCodeView").Visible = False
                                    End If
                                End If
                                If oMaster.FindControl("lblTaxRegistrationNo_view") IsNot Nothing Then
                                    CType(oMaster.FindControl("lblTaxRegistrationNo_view"), Label).Text = .TaxNumber
                                End If
                                If oMaster.FindControl("lblBlacklistingReason_view") IsNot Nothing Then
                                    CType(oMaster.FindControl("lblBlacklistingReason_view"), Label).Text = GetDescriptionForCode(NexusProvider.ListType.PMLookup, .ClientSharedData.BlacklistReasonCode, "BlackList_Reason")
                                End If
                                If oMaster.FindControl("lblBranchName_view") IsNot Nothing Then
                                    Dim sBranchName As String = ""
                                    Dim oBranchs As NexusProvider.BranchCollection = CType(Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches
                                    If oBranchs IsNot Nothing Then
                                        For Each oBranch As NexusProvider.Branch In oBranchs
                                            If oBranch.Code = .BranchCode Then
                                                sBranchName = oBranch.Description
                                                Exit For
                                            End If
                                        Next
                                    End If
                                    CType(oMaster.FindControl("lblBranchName_view"), Label).Text = sBranchName
                                End If
                                If oMaster.FindControl("lblCurrencyName_view") IsNot Nothing Then
                                    Dim oWebService1 As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                                    Dim oCurrencyColl As NexusProvider.CurrencyCollection
                                    oCurrencyColl = oWebService1.GetCurrenciesByBranch(.BranchCode)
                                    CType(oMaster.FindControl("lblCurrencyName_view"), Label).Text = oCurrencyColl(0).BaseCurrencyDesc
                                End If
                                If oMaster.FindControl("lblCode_view") IsNot Nothing Then
                                    If String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                                        CType(oMaster.FindControl("lblCode_view"), Label).Text = .ClientSharedData.ShortName
                                    ElseIf String.IsNullOrEmpty(.UserName) = False Then
                                        CType(oMaster.FindControl("lblCode_view"), Label).Text = .UserName
                                    End If
                                End If
                            End With
                        Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                            oParty = CType(Session(CNParty), NexusProvider.CorporateParty)
                            With CType(oParty, NexusProvider.CorporateParty)
                                If oMaster.FindControl("lblCompanyName_view") IsNot Nothing Then
                                    ' Display Company name & Main Contact
                                    CType(oMaster.FindControl("lblCompanyName_view"), Label).Text = .CompanyName
                                End If
                                If oMaster.FindControl("lblMainContact_view") IsNot Nothing Then
                                    CType(oMaster.FindControl("lblMainContact_view"), Label).Text = .MainContact
                                End If
                                If oMaster.FindControl("lblRegisteredName_view") IsNot Nothing Then
                                    CType(oMaster.FindControl("lblRegisteredName_view"), Label).Text = .CompanyReg
                                End If
                                If oMaster.FindControl("lblNumberOfEmployees_view") IsNot Nothing Then
                                    If oMaster.FindControl("GISCorporate_Employees") IsNot Nothing Then
                                        CType(oMaster.FindControl("GISCorporate_Employees"), NexusProvider.LookupList).Value = .NumberOfEmployees
                                        CType(oMaster.FindControl("lblNumberOfEmployees_view"), Label).Text = CType(oMaster.FindControl("GISCorporate_Employees"), NexusProvider.LookupList).Text
                                    End If

                                End If
                                If oMaster.FindControl("lblStateTaxNumber_view") IsNot Nothing Then
                                    CType(oMaster.FindControl("lblStateTaxNumber_view"), Label).Text = .StateTaxNumber
                                End If
                                If oMaster.FindControl("lblRegisterationOffice_view") IsNot Nothing Then
                                    CType(oMaster.FindControl("lblRegisterationOffice_view"), Label).Text = .RegistrationOffice
                                End If
                                If oMaster.FindControl("lblBusinessName_view") IsNot Nothing Then
                                    CType(oMaster.FindControl("lblBusinessName_view"), Label).Text = .BusinessName
                                End If
                                If oMaster.FindControl("lblBlacklistingReason_view") IsNot Nothing Then
                                    CType(oMaster.FindControl("lblBlacklistingReason_view"), Label).Text = GetDescriptionForCode(NexusProvider.ListType.PMLookup, .ClientSharedData.BlacklistReasonCode, "BlackList_Reason")
                                End If
                                If oMaster.FindControl("lblBranchName_view") IsNot Nothing Then
                                    Dim sBranchName As String = ""
                                    Dim oBranchs As NexusProvider.BranchCollection = CType(Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches
                                    If oBranchs IsNot Nothing Then
                                        For Each oBranch As NexusProvider.Branch In oBranchs
                                            If oBranch.Code = .BranchCode Then
                                                sBranchName = oBranch.Description
                                                Exit For
                                            End If
                                        Next
                                    End If
                                    CType(oMaster.FindControl("lblBranchName_view"), Label).Text = sBranchName
                                End If
                                If oMaster.FindControl("lblCurrencyName_view") IsNot Nothing Then
                                    Dim oWebService1 As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                                    Dim oCurrencyColl As NexusProvider.CurrencyCollection
                                    oCurrencyColl = oWebService1.GetCurrenciesByBranch(.BranchCode)
                                    CType(oMaster.FindControl("lblCurrencyName_view"), Label).Text = oCurrencyColl(0).BaseCurrencyDesc
                                End If
                                If oMaster.FindControl("lblServiceLevel_view") IsNot Nothing Then
                                    CType(oMaster.FindControl("lblServiceLevel_view"), Label).Text = .ServiceLevelCode
                                End If
                                If oMaster.FindControl("lblCode_view") IsNot Nothing Then
                                    If String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                                        CType(oMaster.FindControl("lblCode_view"), Label).Text = .ClientSharedData.ShortName
                                    ElseIf String.IsNullOrEmpty(.UserName) = False Then
                                        CType(oMaster.FindControl("lblCode_view"), Label).Text = .UserName
                                    End If
                                End If

                                'if field "Tax Number" exist on the page then set the value
                                If oMaster.FindControl("lblTaxNumber_view") IsNot Nothing Then
                                    CType(oMaster.FindControl("lblTaxNumber_view"), Label).Text = .TaxNumber
                                End If

                                'if field "Is Domiciled For Tax" exist on the page then set the value
                                If oMaster.FindControl("chkDomiciledForTax_view") IsNot Nothing Then
                                    CType(oMaster.FindControl("chkDomiciledForTax_view"), CheckBox).Checked = .DomiciledForTax
                                    CType(oMaster.FindControl("chkDomiciledForTax_view"), CheckBox).Enabled = False
                                End If

                                'if field "Tax Exempt" exist on the page then set the value
                                If oMaster.FindControl("chkTaxExempt_view") IsNot Nothing Then
                                    CType(oMaster.FindControl("chkTaxExempt_view"), CheckBox).Checked = .TaxExempt
                                    CType(oMaster.FindControl("chkTaxExempt_view"), CheckBox).Enabled = False
                                End If

                                'if field "Tax Percentage" exist on the page then set the value
                                If oMaster.FindControl("lblTaxPercentage_view") IsNot Nothing Then
                                    CType(oMaster.FindControl("lblTaxPercentage_view"), Label).Text = .TaxPercentage
                                End If
                                If oMaster.FindControl("lblFileCode_view") IsNot Nothing Then
                                    If oPortal.AllowFileCodeField = True Then
                                        CType(oMaster.FindControl("lblFileCode_view"), Label).Text = .FileCode
                                    Else
                                        oMaster.FindControl("liFileCodeView").Visible = False
                                    End If
                                End If
                            End With
                        Case TypeOf Session(CNParty) Is NexusProvider.OtherParty
                            oParty = CType(Session(CNParty), NexusProvider.OtherParty)
                            With CType(oParty, NexusProvider.OtherParty)
                                'GENERAL data
                                If oMaster.FindControl("lblCode_view") IsNot Nothing Then
                                    If String.IsNullOrEmpty(.ShortName) = False Then
                                        CType(oMaster.FindControl("lblCode_view"), Label).Text = .ShortName
                                    ElseIf String.IsNullOrEmpty(.code) = False Then
                                        CType(oMaster.FindControl("lblCode_view"), Label).Text = .Code
                                    End If
                                End If
                                If oMaster.FindControl("lblName_view") IsNot Nothing Then
                                    CType(oMaster.FindControl("lblName_view"), Label).Text = .Name
                                End If
                                If oMaster.FindControl("lblDOB_view") IsNot Nothing Then
                                    If Not .DateOfBirth.ToShortDateString.Contains("1899") AndAlso
                                     Not .DateOfBirth = Date.MinValue Then
                                        CType(oMaster.FindControl("lblDOB_view"), Label).Text = .DateOfBirth
                                    End If
                                End If
                                If oMaster.FindControl("lblGender_view") IsNot Nothing Then
                                    CType(oMaster.FindControl("lblGender_view"), Label).Text = .Gender
                                End If
                                If oMaster.FindControl("lblLicenceType_View") IsNot Nothing AndAlso
                                    oMaster.FindControl("Licence_Type") IsNot Nothing Then
                                    CType(oMaster.FindControl("Licence_Type"), NexusProvider.LookupList).Value = .LicenseTypeCode
                                    CType(oMaster.FindControl("lblLicenceType_View"), Label).Text = CType(oMaster.FindControl("Licence_Type"), NexusProvider.LookupList).Text.Trim
                                End If
                                If oMaster.FindControl("lblLicenceNO_View") IsNot Nothing Then
                                    CType(oMaster.FindControl("lblLicenceNO_View"), Label).Text = .LicenseNumber
                                End If
                                If oMaster.FindControl("lblStatus_View") IsNot Nothing AndAlso
                                  oMaster.FindControl("ddlDriverStatus") IsNot Nothing Then
                                    CType(oMaster.FindControl("ddlDriverStatus"), NexusProvider.LookupList).Value = .DriverStatusCode
                                    CType(oMaster.FindControl("lblStatus_View"), Label).Text = CType(oMaster.FindControl("ddlDriverStatus"), NexusProvider.LookupList).Text.Trim
                                End If
                                If oMaster.FindControl("lblRegistrationNO_View") IsNot Nothing Then
                                    CType(oMaster.FindControl("lblRegistrationNO_View"), Label).Text = .RegistrationNumber
                                End If
                                If oMaster.FindControl("lblBranch_view") IsNot Nothing Then
                                    Dim sBranchName As String = ""
                                    Dim oBranchs As NexusProvider.BranchCollection = CType(Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches
                                    If oBranchs IsNot Nothing Then
                                        For Each oBranch As NexusProvider.Branch In oBranchs
                                            If oBranch.Code = .BranchCode Then
                                                sBranchName = oBranch.Description
                                                Exit For
                                            End If
                                        Next
                                    End If
                                    CType(oMaster.FindControl("lblBranch_view"), Label).Text = sBranchName
                                End If
                                If oMaster.FindControl("lblSubBranch_View") IsNot Nothing Then
                                    CType(oMaster.FindControl("lblSubBranch_View"), Label).Text = .SubBranchCode
                                End If
                                If oMaster.FindControl("lblCurrency_view") IsNot Nothing AndAlso
                                    oMaster.FindControl("ddlCurrency") IsNot Nothing Then
                                    CType(oMaster.FindControl("ddlCurrency"), DropDownList).SelectedValue = .Currency
                                    CType(oMaster.FindControl("lblCurrency_view"), Label).Text = CType(oMaster.FindControl("ddlCurrency"), DropDownList).SelectedItem.Text.Trim
                                End If
                                If oMaster.FindControl("lblTPASettle_View") IsNot Nothing Then
                                    CType(oMaster.FindControl("lblTPASettle_View"), Label).Text = .IsTPASettleDirectly
                                End If
                                'TAX data
                                If oMaster.FindControl("lblTaxNumber_View") IsNot Nothing Then
                                    CType(oMaster.FindControl("lblTaxNumber_View"), Label).Text = .TaxNumber
                                End If
                                If oMaster.FindControl("chkIsDomiciledForTax_View") IsNot Nothing Then
                                    CType(oMaster.FindControl("chkIsDomiciledForTax_View"), CheckBox).Checked = .DomiciledForTax
                                    CType(oMaster.FindControl("chkIsDomiciledForTax_View"), CheckBox).Enabled = False
                                End If
                                If oMaster.FindControl("chkTaxExempt_View") IsNot Nothing Then
                                    CType(oMaster.FindControl("chkTaxExempt_View"), CheckBox).Checked = .TaxExempt
                                    CType(oMaster.FindControl("chkTaxExempt_View"), CheckBox).Enabled = False
                                End If
                                If oMaster.FindControl("lblTaxPercentage_View") IsNot Nothing Then
                                    CType(oMaster.FindControl("lblTaxPercentage_View"), Label).Text = .TaxPercentage
                                End If
                            End With
                    End Select

                    'Allow "Edit Client" as per BO and web.config file settings
                    'If both are TRUE than only allow "Edit Client" otherwise "Edit" button will not be visible
                    'Initalize the variables to get and set the editing fucntionality of the user. 
                    Dim bEditClient As Boolean
                    Dim bIsClientManagerViewOnly As Boolean
                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim sReturnCode As NexusProvider.OptionTypeSetting
                    Try
                        'Get the system Option "Enable Editing in Client Manager"
                        sReturnCode = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5000)

                        'Checking of User Authority for Editing the Client using client manager
                        Dim oUserAuthority As New NexusProvider.UserAuthority
                        'Get the user name from session
                        oUserAuthority.UserCode = Session(CNLoginName)
                        'set the authority options for reverse allocation
                        oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.IsClientManagerViewonly
                        oWebService = New NexusProvider.ProviderManager().Provider
                        'initiate the GetUserAuthority method
                        oWebService.GetUserAuthorityValue(oUserAuthority)
                        If oUserAuthority.UserAuthorityValue = "1" Then
                            bIsClientManagerViewOnly = True
                        Else
                            bIsClientManagerViewOnly = False
                        End If
                        bEditClient = UserCanDoTask("EditClientDetails")
                        If TypeOf Session(CNParty) Is NexusProvider.OtherParty Then
                            oParty = CType(Session(CNParty), NexusProvider.OtherParty)
                            bEditClient = True
                        End If
                        'If bEditClientViaClientManager = True and User Can has authority to edit a client
                        If oMaster.FindControl("btnEditClient") IsNot Nothing Then
                            If sReturnCode IsNot Nothing AndAlso sReturnCode.OptionValue IsNot Nothing Then
                                If sReturnCode.OptionValue = "1" AndAlso bEditClient AndAlso bIsClientManagerViewOnly = False Then
                                    CType(oMaster.FindControl("btnEditClient"), LinkButton).Visible = True

                                ElseIf bIsClientManagerViewOnly Then
                                    CType(oMaster.FindControl("btnEditClient"), LinkButton).Visible = False
                                End If
                            ElseIf bIsClientManagerViewOnly Then
                                CType(oMaster.FindControl("btnEditClient"), LinkButton).Visible = False
                            End If
                        End If
                    Catch ex As NexusProvider.NexusException
                        sReturnCode = Nothing
                        bEditClient = Nothing
                    End Try

                    'Hide the ChildScreen Panel
                    'If oPartyChildScreenPanel IsNot Nothing Then
                    '    oPartyChildScreenPanel.Visible = False
                    'End If

                    'Hide Custom data Panel    
                    ReadPartyControlsContainerFromXML(oMaster, String.Empty, Me, True)
                    'If oPartyBuilderPanel IsNot Nothing Then
                    '    oPartyBuilderPanel.Visible = False
                    'End If

                Case Else 'Add

                    Session(CNClientMode) = Mode.Add

                    'Not visible at Add Mode
                    For Each oControl As Control In oMaster.Controls
                        Select Case oControl.ID
                            Case "ctrlNewQuote",
                            "pnlQuotesPolicies", "pnlClientClaims", "pnlClientEvents", "pnlClientAccounts",
                            "pnlViewPC", "pnlViewCC", "ctrlNewQuoteImproved", "cmdExtractClientData", "pnlViewOP"
                                oControl.Visible = False
                            Case "PnlRegisterPC", "PnlRegisterCC", "pnlRegisterOP"
                                EnableControls(True, oControl)
                        End Select
                    Next

                    'To Reset The value of the page
                    If Not IsPostBack And CType(Session(CNParty), NexusProvider.BaseParty).Key = 0 Then

                        For Each oControl As Control In oMaster.Controls
                            Select Case oControl.ID
                                Case "ctrlNewQuote",
                                "pnlQuotesPolicies", "pnlClientClaims", "pnlClientEvents", "pnlClientAccounts",
                                "pnlViewPC", "pnlViewCC", "pnlViewOP"
                                    oControl.Visible = False
                                Case "PnlRegisterPC"
                                    For Each oCtrl As Control In oControl.Controls
                                        Select Case oCtrl.ID
                                            Case "txtFirstName", "txtLastname", "txtInitials", "txtTaxRegistrationNO", "txtEmployeeNumber"
                                                CType(oCtrl, TextBox).Text = String.Empty
                                            Case "ddlTitle"
                                                CType(oCtrl, NexusProvider.LookupList).Value = String.Empty
                                            Case "txtDOB"
                                                CType(oCtrl, TextBox).Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper()
                                            Case "ddlMaritalStatus"
                                                CType(oCtrl, NexusProvider.LookupList).Value = "Not Available"
                                            Case "liFileCode"
                                                If oMaster.FindControl("txtFileCode") IsNot Nothing Then
                                                    oControl = oMaster.FindControl("txtFileCode")
                                                    CType(oControl, TextBox).Text = String.Empty
                                                End If
                                        End Select
                                    Next
                                    EnableControls(True, oControl)
                                Case "PnlRegisterCC"
                                    oControl.Visible = True
                                    For Each oCtrl As Control In oControl.Controls
                                        Select Case oCtrl.ID
                                            Case "txtCompanyName", "txtMainContact", "txtRegisteredName",
                                                    "txtRegistrationOffice", "txtBusinessName", "txtStateTaxNumber"
                                                CType(oCtrl, TextBox).Text = String.Empty
                                            Case "GISCorporate_Employees"
                                                CType(oCtrl, NexusProvider.LookupList).Value = String.Empty
                                            Case "liFileCode"
                                                If oMaster.FindControl("txtFileCode") IsNot Nothing Then
                                                    oControl = oMaster.FindControl("txtFileCode")
                                                    CType(oControl, TextBox).Text = String.Empty
                                                End If

                                            Case "liTaxNumber"
                                                'if field "Tax Number" exist on the page then make it blank
                                                If oMaster.FindControl("txtTaxNumber") IsNot Nothing Then
                                                    oControl = oMaster.FindControl("txtTaxNumber")
                                                    CType(oControl, TextBox).Text = String.Empty
                                                End If

                                            Case "liDomiciledForTax"
                                                'if field "Is Domiciled ForTax" exist on the page then set it unchecked
                                                If oMaster.FindControl("chkDomiciledForTax") IsNot Nothing Then
                                                    oControl = oMaster.FindControl("chkDomiciledForTax")
                                                    CType(oControl, CheckBox).Checked = False
                                                End If

                                            Case "liTaxExempt"
                                                'if field "Tax Exempt" exist on the page then set it unchecked
                                                If oMaster.FindControl("chkTaxExempt") IsNot Nothing Then
                                                    oControl = oMaster.FindControl("chkTaxExempt")
                                                    CType(oControl, CheckBox).Checked = False
                                                End If

                                            Case "liTaxPercentage"
                                                'if field "Tax Percentage" exist on the page then make it blank
                                                If oMaster.FindControl("txtTaxPercentage") IsNot Nothing Then
                                                    oControl = oMaster.FindControl("txtTaxPercentage")
                                                    CType(oControl, TextBox).Text = String.Empty
                                                End If
                                        End Select
                                    Next
                                    EnableControls(True, oControl)
                                Case "pnlRegisterOP"
                                    oControl.Visible = True
                                    For Each oCtrl As Control In oControl.Controls
                                        Select Case oCtrl.ID
                                            Case "txtName", "txtCode", "txtLicenceNO"
                                                CType(oCtrl, TextBox).Text = String.Empty
                                            Case "Licence_Type"
                                                CType(oCtrl, NexusProvider.LookupList).Value = " "
                                            Case "liFileCode"
                                                If oMaster.FindControl("txtFileCode") IsNot Nothing Then
                                                    oControl = oMaster.FindControl("txtFileCode")
                                                    CType(oControl, TextBox).Text = String.Empty
                                                End If
                                        End Select
                                    Next
                                    EnableControls(True, oControl)

                            End Select
                        Next
                    End If
            End Select

            If Session(CNClientMode) = Mode.Add Then
                'Hide the Custom Child Screens  
                'If oPartyChildScreenPanel IsNot Nothing Then
                '    oPartyChildScreenPanel.Visible = False
                'End If
                '                        CType(oMaster.FindControl("btnUpdate"), Button).Visible = False
            ElseIf Session(CNClientMode) = Mode.Edit Then
                'Show the Custom Child Screens                 
                If oPartyChildScreenPanel IsNot Nothing Then
                    oPartyChildScreenPanel.Visible = True
                End If
                'Show the Custom data (i.e) PartyBuilder controls.
                If oPartyBuilderPanel IsNot Nothing Then
                    oPartyBuilderPanel.Visible = True
                End If
                '  CType(oMaster.FindControl("btnSubmit"), Button).Visible = False
            End If

        End Sub

#Region " CLEAR SESSION VALUE "

        ''' <summary>
        ''' Clear corporate / personal modal dialog session values.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub ClearSessionValues()
            Session.Remove(CNPartyDataModelCode)
            Session.Remove(CNOI)
            Session.Remove(CNClientMode)
            If Not isModalOtherParty Or Session(CNDoNotClearSession) Is Nothing Or Session(CNDoNotClearSession) <> "true" Then
                Session.Remove(CNMode)
            End If
        End Sub

#End Region

        ''' <summary>
        ''' Making visible / enable all the controls available in the panel.
        ''' </summary>
        ''' <param name="value"></param>
        ''' <param name="oContainer"></param>
        ''' <remarks></remarks>
        Protected Sub EnableControls(ByVal value As Boolean, ByVal oContainer As Control)
            For Each oCtrl As Control In oContainer.Controls
                Select Case oCtrl.GetType.Name
                    Case "GridView", "HyperLink"
                        oCtrl.Visible = value
                    Case "UpdatePanel", "Panel", "Control"
                        oCtrl.Visible = value
                        EnableControls(value, oCtrl)
                End Select
            Next
        End Sub



#Region " EDIT CLIENT "
        ''' <summary>
        ''' EditClient event populates Corporate client in edit mode
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Sub BtnEditClientClick(ByVal sender As Object, ByVal e As System.EventArgs)
            Session(CNClientMode) = Mode.Edit
            'Remove the OI key because it's misleading while user is trying to edit the client.
            Session(CNOI) = Nothing
            'Recreating all the session values for Modal dialog to load the corporate client in edit mode.
            ReadPartyControlsContainerFromXML(oMaster, String.Empty, Me, True)
            LoadClient()
            'Read Party builder controls after editing a client
            ReadPartyControlsContainerForAll(False)

        End Sub
#End Region
#Region "Cancel Button"
        ''' <summary>
        ''' This methods reloads the screen in view mode
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Sub BtnCancelClientClick(ByVal sender As Object, ByVal e As System.EventArgs)
            Session(CNClientMode) = Mode.View
            Session(CNIsNewClient) = Nothing
            'Recreating all the session values for Modal dialog to load the corporate client in edit mode.
            LoadClient()
            'Read Party Builder controls after clicking cancel button
            ReadPartyControlsContainerForAll(False)

        End Sub
#End Region
        ''' <summary>
        ''' Load the current party, from session if available, otherwise retrieve the party via SAM
        ''' using the PartyKey in the querystring.
        ''' </summary>
        ''' <exception cref="Exception">Party type not supported</exception>
        Sub LoadClient()
            Dim oParty As NexusProvider.BaseParty = Session(CNParty)
            Dim oPartyBankDetails As NexusProvider.BankCollection
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim bLogClientView As Boolean = False
            If oParty IsNot Nothing AndAlso Session(CNIsNewClient) IsNot Nothing _
                AndAlso Not HttpContext.Current.Session.IsCookieless Then
                'Base on the session value is personal / corporate client is loaded
                Select Case True
                    Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                        oParty = CType(Session(CNParty), NexusProvider.PersonalParty)
                    Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                        oParty = CType(Session(CNParty), NexusProvider.CorporateParty)
                    Case TypeOf Session(CNParty) Is NexusProvider.OtherParty
                        oParty = CType(Session(CNParty), NexusProvider.OtherParty)
                End Select
                'Populate Party bank Details
                oPartyBankDetails = oWebService.GetPartyBankDetails(oParty.Key)
                oParty.BankDetails = oPartyBankDetails
                Session(CNParty) = oParty

            ElseIf IsNumeric(Request("partykey")) _
                AndAlso (IsPostBack = False Or (Session(CNClientMode) IsNot Nothing AndAlso Session(CNClientMode) = Mode.View)) _
                OrElse HttpContext.Current.Session.IsCookieless Then
                'Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim iPartyKey As Integer = CType(Request("partykey"), Integer)
                If iPartyKey = 0 Then
                    iPartyKey = DirectCast(Session(CNParty), NexusProvider.BaseParty).Key
                End If

                Dim bGetPartyCalled As Boolean = False 'Change done to prevent multiple calls when data already in session
                Try
                    If Session(CNParty) Is Nothing OrElse (Request("__EVENTTARGET") IsNot Nothing AndAlso Request("__EVENTTARGET").Contains("btnCancel")) _
                        OrElse HttpContext.Current.Session.IsCookieless Then
                        oParty = oWebService.GetParty(iPartyKey)
                        bGetPartyCalled = True
                    Else
                        If iPartyKey <> CType(Session(CNParty), NexusProvider.BaseParty).Key Then
                            oParty = oWebService.GetParty(iPartyKey)
                            bGetPartyCalled = True
                        End If
                    End If
                    'retreive the Bank Details and update in Session
                    'First time it will be called and subsequently if oParty is already in session 
                    'and oParty.Key is also same dont call it.
                    If Session(CNClientMode) IsNot Nothing AndAlso Session(CNClientMode) = Mode.View Then
                        bGetPartyCalled = True
                    End If
                    If bGetPartyCalled OrElse (oParty.BankDetails Is Nothing OrElse oParty.BankDetails.Count = 0) Then
                        oPartyBankDetails = oWebService.GetPartyBankDetails(iPartyKey)
                        oParty.BankDetails = oPartyBankDetails
                    End If

                    If Session(CNParty) IsNot Nothing AndAlso TypeOf Session(CNParty) Is NexusProvider.OtherParty Then
                        oParty = oWebService.GetParty(iPartyKey)
                        oPartyBankDetails = oWebService.GetPartyBankDetails(iPartyKey)
                        oParty.BankDetails = oPartyBankDetails
                    End If

                    'Fixed against PN:42725 as we dont get UserName using GetParty so need to get it everytime from Request and update the oParty also
                    oParty.UserName = Request("Code")

                    Select Case True
                        Case TypeOf oParty Is NexusProvider.CorporateParty
                            Session.Add(CNParty, oParty)
                        Case TypeOf oParty Is NexusProvider.PersonalParty
                            Session.Add(CNParty, oParty)
                        Case TypeOf oParty Is NexusProvider.OtherParty
                            Session.Add(CNParty, oParty)
                        Case Else
                            Throw New Exception("Party type not supported")
                    End Select
                Finally
                    oWebService = Nothing
                End Try
            End If

            'Loads Client informations.
            Party = oParty

            'Log client view event
            If oParty IsNot Nothing AndAlso oParty.Key > 0 AndAlso Session(CNClientMode) = Mode.View Then
                Dim sViewedParties As String = If(Session("ViewedParties"), "")
                Dim sPartyKey As String = "|" & oParty.Key.ToString() & "|"

                If Not sViewedParties.Contains(sPartyKey) Then
                    Try
                        Dim oEventService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                        Dim oEventDetails As New NexusProvider.EventDetails
                        Dim sClientType As String = If(TypeOf oParty Is NexusProvider.PersonalParty, "Personal", "Corporate")

                        Dim iEventTypeKey As Integer = 1
                        Try
                            Dim oEventTypeList As NexusProvider.LookupListCollection = oEventService.GetList(NexusProvider.ListType.PMLookup, "Event_type", False, False)
                            For iListCount As Integer = 0 To oEventTypeList.Count - 1
                                If oEventTypeList(iListCount).Code = "CLVIEW" Then
                                    iEventTypeKey = oEventTypeList(iListCount).Key
                                    Exit For
                                End If
                            Next
                        Catch
                        End Try

                        With oEventDetails
                            .EventDate = Now()
                            .PartyKey = oParty.Key
                            .RtfText = "Client was viewed"
                            .UserName = Session(CNLoginName)
                            .EventTypeKey = iEventTypeKey
                        End With

                        oEventService.AddEvent(oEventDetails)
                        oEventService = Nothing
                        Session("ViewedParties") = sPartyKey
                    Catch ex As Exception
                        'Silently fail to avoid disrupting user experience
                    End Try
                End If
            End If
        End Sub

        Public Function UpdatePartyBank() As Boolean
            Dim bResult As Boolean = True
            Dim oParty As NexusProvider.BaseParty = Session(CNParty)
            Dim oBankDetails As NexusProvider.BankCollection

            Dim oAddPartyBankDetails As New NexusProvider.BankCollection
            Dim oEditPartyBankDetails As New NexusProvider.BankCollection
            Dim oDeletePartyBankDetails As New NexusProvider.BankCollection
            Dim oActivatePartyBankDetails As New NexusProvider.BankCollection

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

            oBankDetails = oParty.BankDetails

            Dim iCount = 0
            'Check if IsActive=True and Task Mode is Edit then Party Bank should be passed for UpdatePartyBank and ActivatePartyBankDetails both
            'Create one more collection for Making a party bank active or inactive
            For iCount = 0 To oBankDetails.Count - 1                '
                'For updating the records with newly added bank details
                If (oBankDetails(iCount).TaskMode = NexusProvider.Bank.Mode.Add) Or oBankDetails(iCount).PartyBankKey = 0 Then
                    oAddPartyBankDetails.Add(oBankDetails(iCount))
                    oBankDetails(iCount).TaskMode = NexusProvider.Bank.Mode.Add
                    'For updating the records with edited bank details
                ElseIf (oBankDetails(iCount).TaskMode = NexusProvider.Bank.Mode.Edit) Then
                    oEditPartyBankDetails.Add(oBankDetails(iCount))
                    'For updating the records with Deleted bank details
                ElseIf (oBankDetails(iCount).TaskMode = NexusProvider.Bank.Mode.Delete) Then
                    oDeletePartyBankDetails.Add(oBankDetails(iCount))
                ElseIf oBankDetails(iCount).TaskMode = NexusProvider.Bank.Mode.MakeActive Then
                    oActivatePartyBankDetails.Add(oBankDetails(iCount))
                ElseIf oBankDetails(iCount).TaskMode = NexusProvider.Bank.Mode.MakeInactive Then
                    oActivatePartyBankDetails.Add(oBankDetails(iCount))
                End If
            Next

            'A Common method is called to Add/Edit/Delete the Party Bank Details

            If oAddPartyBankDetails IsNot Nothing AndAlso oAddPartyBankDetails.Count > 0 Then
                oWebService.ManageBankDetails(oParty.Key, oAddPartyBankDetails)
            End If
            If oEditPartyBankDetails IsNot Nothing AndAlso oEditPartyBankDetails.Count > 0 Then
                oWebService.ManageBankDetails(oParty.Key, oEditPartyBankDetails)
            End If

            If oActivatePartyBankDetails IsNot Nothing AndAlso oActivatePartyBankDetails.Count > 0 Then
                oWebService.ManageBankDetails(oParty.Key, oActivatePartyBankDetails)
            End If

            If oDeletePartyBankDetails IsNot Nothing AndAlso oDeletePartyBankDetails.Count > 0 Then
                Try
                    oWebService.ManageBankDetails(oParty.Key, oDeletePartyBankDetails)
                Catch
                    bResult = False
                    'Throw New Exception("Party bank cannot be deleted as Party bank is in use")
                End Try
            End If
            'Cleaning Up
            oParty = Nothing
            oBankDetails = Nothing
            oAddPartyBankDetails = Nothing
            oEditPartyBankDetails = Nothing
            oDeletePartyBankDetails = Nothing
            oWebService = Nothing

            Return bResult
        End Function
        ''' <summary>
        ''' Display Corporate / personal client information.
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property Party() As NexusProvider.BaseParty
            'Dave - what is this property for and where would it be set / read from?            
            Set(ByVal value As NexusProvider.BaseParty)
                Select Case True
                    Case TypeOf value Is NexusProvider.PersonalParty

                        Dim oParty As NexusProvider.PersonalParty
                        oParty = CType(value, NexusProvider.PersonalParty)
                        'Reset controls when corporate party object is empty.
                        If value IsNot Nothing Then
                            With oParty
                                If oMaster.FindControl("PnlRegisterPC") IsNot Nothing Then
                                    For Each oControl As Control In oMaster.FindControl("PnlRegisterPC").Controls
                                        Select Case oControl.ID
                                            Case "txtFirstName"
                                                CType(oControl, TextBox).Text = .Forename
                                            Case "txtLastname"
                                                CType(oControl, TextBox).Text = .Lastname
                                            Case "liFileCode"
                                                If oMaster.FindControl("txtFileCode") IsNot Nothing Then
                                                    oControl = oMaster.FindControl("txtFileCode")
                                                    CType(oControl, TextBox).Text = .FileCode
                                                End If
                                            Case "txtInitials"
                                                CType(oControl, TextBox).Text = .Initials
                                            Case "txtTaxRegistrationNO"
                                                CType(oControl, TextBox).Text = .TaxNumber
                                            Case "txtEmployeeNumber"
                                                CType(oControl, TextBox).Text = .EmployeeNumber
                                            Case "ddlTitle"
                                                CType(oControl, NexusProvider.LookupList).Value = .Title
                                            Case "GISCorporate_Nationality"
                                                CType(oControl, NexusProvider.LookupList).Value = .NationalityCode
                                            Case "ddlServiceLevel"
                                                CType(oControl, NexusProvider.LookupList).Value = .ServiceLevelCode
                                            Case "ddlGender"
                                                CType(oControl, NexusProvider.LookupList).Value = .GenderCode
                                            Case "liSelectCurrency"
                                                If oMaster.FindControl("ddlCurrency") IsNot Nothing Then
                                                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                                                    Dim oCurrencyColl As NexusProvider.CurrencyCollection

                                                    Dim dBranchCode As DropDownList = oMaster.FindControl("ddlBranchCode")
                                                    oCurrencyColl = oWebService.GetCurrenciesByBranch(.BranchCode)
                                                    oControl = oMaster.FindControl("ddlCurrency")
                                                    CType(oControl, DropDownList).Items.Clear()
                                                    For i As Integer = 0 To oCurrencyColl.Count - 1
                                                        Dim lstCurrency As New ListItem
                                                        lstCurrency.Text = oCurrencyColl.Item(i).Description.ToString
                                                        lstCurrency.Value = Trim(oCurrencyColl.Item(i).CurrencyCode.ToString)
                                                        CType(oControl, DropDownList).Items.Add(lstCurrency)
                                                    Next

                                                    If CType(oControl, DropDownList).Items.FindByValue(.Currency) IsNot Nothing Then
                                                        CType(oControl, DropDownList).SelectedValue = .Currency
                                                    End If
                                                End If


                                            Case "ddlMaritalStatus"
                                                If .MaritalStatusCodeSpecified = True Then
                                                    If .MaritalStatusCode = NexusProvider.MaritalStatusCodeTypes.Married Then
                                                        CType(oControl, NexusProvider.LookupList).Value = "Married"
                                                    ElseIf .MaritalStatusCode = NexusProvider.MaritalStatusCodeTypes.CommonLaw Then
                                                        CType(oControl, NexusProvider.LookupList).Value = "Married - Common Law"
                                                    ElseIf .MaritalStatusCode = NexusProvider.MaritalStatusCodeTypes.Divorced Then
                                                        CType(oControl, NexusProvider.LookupList).Value = "Divorced"
                                                    ElseIf .MaritalStatusCode = NexusProvider.MaritalStatusCodeTypes.NotApplicable Then
                                                        CType(oControl, NexusProvider.LookupList).Value = "Not Applicable"
                                                    ElseIf .MaritalStatusCode = NexusProvider.MaritalStatusCodeTypes.NotAvailable Then
                                                        CType(oControl, NexusProvider.LookupList).Value = "Not Available"
                                                    ElseIf .MaritalStatusCode = NexusProvider.MaritalStatusCodeTypes.Partnered Then
                                                        CType(oControl, NexusProvider.LookupList).Value = "Partnered"
                                                    ElseIf .MaritalStatusCode = NexusProvider.MaritalStatusCodeTypes.Separated Then
                                                        CType(oControl, NexusProvider.LookupList).Value = "Separated"
                                                    ElseIf .MaritalStatusCode = NexusProvider.MaritalStatusCodeTypes.Single_ Then
                                                        CType(oControl, NexusProvider.LookupList).Value = "Single"
                                                    ElseIf .MaritalStatusCode = NexusProvider.MaritalStatusCodeTypes.Widowed Then
                                                        CType(oControl, NexusProvider.LookupList).Value = "Widowed"
                                                    End If
                                                End If
                                            Case "txtDOB"
                                                If .DateOfBirthSpecified Then
                                                    If Not .DateOfBirth.ToShortDateString.Contains("1899") AndAlso
                                                     Not .DateOfBirth = Date.MinValue Then
                                                        CType(oMaster.FindControl("lblDOB_view"), Label).Text = .DateOfBirth
                                                        oControl = oMaster.FindControl("txtDOB")
                                                        CType(oControl, TextBox).Text = .DateOfBirth
                                                    End If
                                                End If
                                            Case "liBranchCode"
                                                If oMaster.FindControl("ddlBranchCode") IsNot Nothing Then
                                                    oControl = oMaster.FindControl("ddlBranchCode")
                                                    If CType(oControl, DropDownList).Items.FindByValue(.BranchCode) IsNot Nothing Then
                                                        CType(oControl, DropDownList).SelectedValue = .BranchCode
                                                    ElseIf Not String.IsNullOrEmpty(.BranchCode) Then
                                                        CType(oControl, DropDownList).Items.Add(New ListItem(.BranchCode, .BranchCode))
                                                        CType(oControl, DropDownList).SelectedValue = .BranchCode
                                                    End If
                                                End If
                                            Case "ddlBlacklistingReason"
                                                CType(oControl, NexusProvider.LookupList).Value = .ClientSharedData.BlacklistReasonCode
                                            Case "ddlRenewalStopCode"
                                                CType(oControl, NexusProvider.LookupList).Value = .ClientSharedData.RenewalStopCode


                                            Case "ddlPrimaryOccupation"
                                                CType(oControl, NexusProvider.LookupList).Value = .OccupationCode
                                            Case "ddlSecondaryOccupation"
                                                CType(oControl, NexusProvider.LookupList).Value = .SecOccupationCode
                                        End Select
                                    Next

                                    'To display personal client name in view mode
                                    If oMaster.FindControl("lblName") IsNot Nothing Then
                                        CType(oMaster.FindControl("lblName"), Label).Text = .Initials & " " & .Forename & " " & .Lastname
                                    End If
                                End If

                            End With
                        End If
                    Case TypeOf value Is NexusProvider.CorporateParty

                        Dim oParty As NexusProvider.CorporateParty
                        oParty = CType(value, NexusProvider.CorporateParty)
                        'Reset controls when corporate party object is empty.
                        If value IsNot Nothing Then
                            With oParty
                                If oMaster.FindControl("PnlRegisterCC") IsNot Nothing Then
                                    For Each oControl As Control In oMaster.FindControl("PnlRegisterCC").Controls
                                        Select Case oControl.ID
                                            Case "txtCompanyName"
                                                CType(oControl, TextBox).Text = .CompanyName
                                            Case "txtMainContact"
                                                CType(oControl, TextBox).Text = .MainContact
                                            Case "txtRegisteredName"
                                                CType(oControl, TextBox).Text = .CompanyReg
                                            Case "txtRegistrationOffice"
                                                CType(oControl, TextBox).Text = .RegistrationOffice
                                            Case "txtBusinessName"
                                                CType(oControl, TextBox).Text = .BusinessName
                                            Case "txtStateTaxNumber"
                                                CType(oControl, TextBox).Text = .StateTaxNumber
                                            Case "liFileCode"
                                                If oMaster.FindControl("txtFileCode") IsNot Nothing Then
                                                    oControl = oMaster.FindControl("txtFileCode")
                                                    CType(oControl, TextBox).Text = .FileCode
                                                End If
                                            Case "GISCorporate_Employees"
                                                CType(oControl, NexusProvider.LookupList).Value = .NumberOfEmployees
                                            Case "liBranchCode"
                                                If oMaster.FindControl("ddlBranchCode") IsNot Nothing Then
                                                    oControl = oMaster.FindControl("ddlBranchCode")
                                                    If CType(oControl, DropDownList).Items.FindByValue(.BranchCode) IsNot Nothing Then
                                                        CType(oControl, DropDownList).SelectedValue = .BranchCode
                                                    ElseIf Not String.IsNullOrEmpty(.BranchCode) Then
                                                        CType(oControl, DropDownList).Items.Add(New ListItem(.BranchCode, .BranchCode))
                                                        CType(oControl, DropDownList).SelectedValue = .BranchCode
                                                    End If
                                                End If
                                            Case "liSelectCurrency"
                                                If oMaster.FindControl("ddlCurrency") IsNot Nothing Then
                                                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                                                    Dim oCurrencyColl As NexusProvider.CurrencyCollection

                                                    Dim dBranchCode As DropDownList = oMaster.FindControl("ddlBranchCode")
                                                    oCurrencyColl = oWebService.GetCurrenciesByBranch(.BranchCode)
                                                    oControl = oMaster.FindControl("ddlCurrency")
                                                    CType(oControl, DropDownList).Items.Clear()
                                                    For i As Integer = 0 To oCurrencyColl.Count - 1
                                                        Dim lstCurrency As New ListItem
                                                        lstCurrency.Text = oCurrencyColl.Item(i).Description.ToString
                                                        lstCurrency.Value = Trim(oCurrencyColl.Item(i).CurrencyCode.ToString)
                                                        CType(oControl, DropDownList).Items.Add(lstCurrency)
                                                    Next

                                                    If CType(oControl, DropDownList).Items.FindByValue(.Currency) IsNot Nothing Then
                                                        CType(oControl, DropDownList).SelectedValue = .Currency
                                                    End If
                                                End If
                                            Case "ddlCurrency"
                                                If CType(oControl, DropDownList).Items.FindByValue(oParty.Currency) IsNot Nothing Then
                                                    CType(oControl, DropDownList).SelectedValue = .Currency
                                                End If
                                            Case "ddlServiceLevel"
                                                CType(oControl, NexusProvider.LookupList).Value = .ServiceLevelCode
                                            Case "liTaxNumber"
                                                'if field "Tax Number" exist on the page then set the value receiced from SAM
                                                If oMaster.FindControl("txtTaxNumber") IsNot Nothing Then
                                                    oControl = oMaster.FindControl("txtTaxNumber")
                                                    CType(oControl, TextBox).Text = .TaxNumber
                                                End If

                                            Case "liDomiciledForTax"
                                                'if field "Is DomiciledForTax" exist on the page then set the value receiced from SAM
                                                If oMaster.FindControl("chkDomiciledForTax") IsNot Nothing Then
                                                    oControl = oMaster.FindControl("chkDomiciledForTax")
                                                    CType(oControl, CheckBox).Checked = .DomiciledForTax
                                                End If

                                            Case "liTaxExempt"
                                                'if field "Tax Exempt" exist on the page then set the value receiced from SAM
                                                If oMaster.FindControl("chkTaxExempt") IsNot Nothing Then
                                                    oControl = oMaster.FindControl("chkTaxExempt")
                                                    CType(oControl, CheckBox).Checked = .TaxExempt
                                                End If

                                            Case "liTaxPercentage"
                                                'if field "Tax Percentage" exist on the page then set the value receiced from SAM
                                                If oMaster.FindControl("txtTaxPercentage") IsNot Nothing Then
                                                    oControl = oMaster.FindControl("txtTaxPercentage")
                                                    CType(oControl, TextBox).Text = .TaxPercentage
                                                End If
                                            Case "ddlBlacklistingReason"
                                                CType(oControl, NexusProvider.LookupList).Value = .ClientSharedData.BlacklistReasonCode
                                            Case "ddlRenewalStopCode"
                                                CType(oControl, NexusProvider.LookupList).Value = .ClientSharedData.RenewalStopCode
                                        End Select
                                    Next
                                End If

                            End With
                        End If
                    Case TypeOf value Is NexusProvider.OtherParty

                        Dim oParty As NexusProvider.OtherParty
                        oParty = CType(value, NexusProvider.OtherParty)
                        Dim webService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                        Dim userDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
                        If value IsNot Nothing Then
                            With oParty
                                If oMaster.FindControl("PnlRegisterOP") IsNot Nothing Then
                                    For Each oControl As Control In oMaster.FindControl("PnlRegisterOP").Controls
                                        Select Case oControl.ID
                                            Case "txtCode"
                                                CType(oControl, TextBox).Text = .ShortName
                                            Case "txtName"
                                                CType(oControl, TextBox).Text = .Name
                                            Case "txtDOB"
                                                If Not .DateOfBirth.ToShortDateString.Contains("1899") AndAlso
                                                    Not .DateOfBirth = Date.MinValue Then
                                                    oControl = oMaster.FindControl("txtDOB")
                                                    CType(oControl, TextBox).Text = .DateOfBirth
                                                End If
                                            Case "ddlGender"
                                                If Not String.IsNullOrEmpty(.Gender) Then
                                                    CType(oControl, NexusProvider.LookupList).Value = .Gender
                                                End If
                                            Case "Licence_Type"
                                                If Not String.IsNullOrEmpty(.LicenseTypeCode) Then
                                                    CType(oControl, NexusProvider.LookupList).Value = .LicenseTypeCode
                                                End If
                                            Case "txtLicenceNO"
                                                CType(oControl, TextBox).Text = .LicenseNumber
                                            Case "ddlDriverStatus"
                                                If Not String.IsNullOrEmpty(.DriverStatusCode) Then
                                                    CType(oControl, NexusProvider.LookupList).Value = .DriverStatusCode
                                                End If
                                            Case "txtRegistrationNO"
                                                CType(oControl, TextBox).Text = .RegistrationNumber
                                            Case "liBranchCode", "ddlBranch"
                                                If oMaster.FindControl("ddlBranch") IsNot Nothing Then
                                                    oControl = oMaster.FindControl("ddlBranch")
                                                    If CType(oControl, DropDownList).Items.FindByValue(.BranchCode) IsNot Nothing Then
                                                        CType(oControl, DropDownList).SelectedValue = .BranchCode
                                                    ElseIf Not String.IsNullOrEmpty(.BranchCode) Then
                                                        CType(oControl, DropDownList).Items.Add(New ListItem(.BranchCode, .BranchCode))
                                                        CType(oControl, DropDownList).SelectedValue = .BranchCode
                                                    End If
                                                End If
                                            Case "liSubBranchCode", "ddlSubBranch"
                                                If oMaster.FindControl("ddlSubBranch") IsNot Nothing Then
                                                    oControl = oMaster.FindControl("ddlSubBranch")
                                                    CType(oControl, DropDownList).SelectedValue = .SubBranchCode
                                                End If
                                            Case "ddlCurrency"
                                                CType(oControl, DropDownList).SelectedValue = .Currency
                                            Case "ddlTPASettle"
                                                If oMaster.FindControl("ddlTPASettle") IsNot Nothing Then
                                                    oControl = oMaster.FindControl("ddlTPASettle")
                                                    CType(oControl, DropDownList).SelectedValue = .IsTPASettleDirectly
                                                End If
                                            Case "txtTaxNumber"
                                                CType(oControl, TextBox).Text = .TaxNumber
                                            Case "txtTaxPercentage"
                                                CType(oControl, TextBox).Text = .TaxPercentage
                                            Case "chkTaxExempt"
                                                CType(oControl, CheckBox).Checked = .TaxExempt
                                            Case "chkIsDomiciledForTax"
                                                CType(oControl, CheckBox).Checked = .DomiciledForTax
                                        End Select
                                    Next
                                End If

                            End With
                        End If
                End Select
            End Set
        End Property

        ''' <summary>
        ''' Update Corporate / Personal client in CorporatyParty object
        ''' </summary>
        ''' <param name="v_oParty"></param>
        ''' <remarks></remarks>
        Public Sub UpdateParty(ByRef v_oParty As NexusProvider.BaseParty)

            Select Case True
                Case TypeOf v_oParty Is NexusProvider.CorporateParty
                    'Updates the v_oParty object with corporate party from aspx page.
                    With CType(v_oParty, NexusProvider.CorporateParty)

                        .TPUserCode = HttpContext.Current.User.Identity.Name

                        For Each oControl As Control In oMaster.FindControl("PnlRegisterCC").Controls
                            Select Case oControl.ID
                                Case "txtCompanyName"
                                    .CompanyName = CType(oControl, TextBox).Text
                                Case "txtMainContact"
                                    .MainContact = CType(oControl, TextBox).Text
                                Case "txtRegisteredName"
                                    .CompanyReg = CType(oControl, TextBox).Text
                                Case "txtRegistrationOffice"
                                    .RegistrationOffice = CType(oControl, TextBox).Text
                                Case "txtBusinessName"
                                    .BusinessName = CType(oControl, TextBox).Text
                                Case "txtStateTaxNumber"
                                    .StateTaxNumber = CType(oControl, TextBox).Text
                                Case "ddlCurrency"
                                    .Currency = CType(oControl, DropDownList).SelectedValue
                                Case "ddlServiceLevel"
                                    .ClientSharedData.ServiceLevelCode = CType(oControl, NexusProvider.LookupList).Value
                                    .ServiceLevelCode = CType(oControl, NexusProvider.LookupList).Value
                                Case "ddlBlacklistingReason"
                                    .ClientSharedData.BlacklistReasonCode = CType(oControl, NexusProvider.LookupList).Value
                                Case "ddlRenewalStopCode"
                                    .ClientSharedData.RenewalStopCode = CType(oControl, NexusProvider.LookupList).Value
                                Case "liFileCode"
                                    If oMaster.FindControl("txtFileCode") IsNot Nothing Then
                                        oControl = oMaster.FindControl("txtFileCode")
                                        .FileCode = CType(oControl, TextBox).Text
                                    End If
                                Case "GISCorporate_Employees"
                                    .NumberOfEmployees = CType(oControl, NexusProvider.LookupList).Value
                                Case "liBranchCode"
                                    If oMaster.FindControl("ddlBranchCode") IsNot Nothing Then
                                        oControl = oMaster.FindControl("ddlBranchCode")
                                        .BranchCode = CType(oControl, DropDownList).SelectedValue
                                    End If

                                Case "liTaxNumber"
                                    'if field "Tax Number" exist on the page then set the value in party object
                                    If oMaster.FindControl("txtTaxNumber") IsNot Nothing Then
                                        oControl = oMaster.FindControl("txtTaxNumber")
                                        .TaxNumber = CType(oControl, TextBox).Text
                                    End If

                                Case "liDomiciledForTax"
                                    'if field "Is Domiciled For Tax" exist on the page then set the value in party object
                                    If oMaster.FindControl("chkDomiciledForTax") IsNot Nothing Then
                                        oControl = oMaster.FindControl("chkDomiciledForTax")
                                        .DomiciledForTax = CType(oControl, CheckBox).Checked
                                    End If

                                Case "liTaxExempt"
                                    'if field "Tax Exempt" exist on the page then set the value in party object
                                    If oMaster.FindControl("chkTaxExempt") IsNot Nothing Then
                                        oControl = oMaster.FindControl("chkTaxExempt")
                                        .TaxExempt = CType(oControl, CheckBox).Checked
                                    End If

                                Case "liTaxPercentage"
                                    'if field "Tax Percentage" exist on the page then set the value in party object
                                    If oMaster.FindControl("txtTaxPercentage") IsNot Nothing Then

                                        oControl = oMaster.FindControl("txtTaxPercentage")
                                        If Not String.IsNullOrEmpty(CType(oControl, TextBox).Text) Then
                                            'if user has entered any value
                                            .TaxPercentage = CType(oControl, TextBox).Text
                                        Else
                                            'if user has left blank then set as 0
                                            .TaxPercentage = 0
                                        End If

                                    End If

                            End Select
                        Next
                    End With
                Case TypeOf v_oParty Is NexusProvider.PersonalParty
                    'Updates the v_oParty object with personal party from aspx page.
                    With CType(v_oParty, NexusProvider.PersonalParty)

                        For Each oControl As Control In oMaster.FindControl("PnlRegisterPC").Controls
                            Select Case oControl.ID
                                Case "txtFirstName"
                                    .Forename = CType(oControl, TextBox).Text
                                Case "txtLastname"
                                    .Lastname = CType(oControl, TextBox).Text
                                Case "liFileCode"
                                    If oMaster.FindControl("txtFileCode") IsNot Nothing Then
                                        oControl = oMaster.FindControl("txtFileCode")
                                        .FileCode = CType(oControl, TextBox).Text
                                    End If
                                Case "txtInitials"
                                    .Initials = CType(oControl, TextBox).Text
                                Case "txtTaxRegistrationNO"
                                    .TaxNumber = CType(oControl, TextBox).Text
                                Case "ddlTitle"
                                    .Title = CType(oControl, NexusProvider.LookupList).Value
                                Case "GISCorporate_Nationality"
                                    .NationalityCode = CType(oControl, NexusProvider.LookupList).Value
                                Case "ddlServiceLevel"
                                    .ClientSharedData.ServiceLevelCode = CType(oControl, NexusProvider.LookupList).Value
                                    .ServiceLevelCode = CType(oControl, NexusProvider.LookupList).Value
                                Case "ddlBlacklistingReason"
                                    .ClientSharedData.BlacklistReasonCode = CType(oControl, NexusProvider.LookupList).Value
                                Case "ddlRenewalStopCode"
                                    .ClientSharedData.RenewalStopCode = CType(oControl, NexusProvider.LookupList).Value
                                Case "ddlGender"
                                    .GenderCode = CType(oControl, NexusProvider.LookupList).Value
                                Case "ddlCurrency"
                                    .Currency = CType(oControl, DropDownList).SelectedValue
                                Case "ddlMaritalStatus"
                                    If CType(oControl, NexusProvider.LookupList).Value IsNot Nothing Then
                                        .MaritalStatusCodeSpecified = True
                                        Select Case CType(oControl, NexusProvider.LookupList).Value.ToUpper()
                                            Case "MARRIED - COMMON LAW" 'NexusProvider.MaritalStatusCodeTypes.CommonLaw
                                                .MaritalStatusCode = NexusProvider.MaritalStatusCodeTypes.CommonLaw
                                                Exit Select
                                            Case "DIVORCED" 'NexusProvider.MaritalStatusCodeTypes.Divorced
                                                .MaritalStatusCode = NexusProvider.MaritalStatusCodeTypes.Divorced
                                                Exit Select
                                            Case "MARRIED" 'NexusProvider.MaritalStatusCodeTypes.Married
                                                .MaritalStatusCode = NexusProvider.MaritalStatusCodeTypes.Married
                                                Exit Select
                                            Case "NOT APPLICABLE" 'NexusProvider.MaritalStatusCodeTypes.NotApplicable
                                                .MaritalStatusCode = NexusProvider.MaritalStatusCodeTypes.NotApplicable
                                                Exit Select
                                            Case "NOT AVAILABLE" 'NexusProvider.MaritalStatusCodeTypes.NotAvailable
                                                .MaritalStatusCode = NexusProvider.MaritalStatusCodeTypes.NotAvailable
                                                Exit Select
                                            Case "PARTNERED" 'NexusProvider.MaritalStatusCodeTypes.Partnered
                                                .MaritalStatusCode = NexusProvider.MaritalStatusCodeTypes.Partnered
                                                Exit Select
                                            Case "SEPARATED" 'NexusProvider.MaritalStatusCodeTypes.Separated
                                                .MaritalStatusCode = NexusProvider.MaritalStatusCodeTypes.Separated
                                                Exit Select
                                            Case "SINGLE" 'NexusProvider.MaritalStatusCodeTypes.Single_
                                                .MaritalStatusCode = NexusProvider.MaritalStatusCodeTypes.Single_
                                                Exit Select
                                            Case "WIDOWED" 'NexusProvider.MaritalStatusCodeTypes.Widowed
                                                .MaritalStatusCode = NexusProvider.MaritalStatusCodeTypes.Widowed
                                                Exit Select
                                        End Select
                                    End If
                                Case "txtDOB"
                                    If Not String.IsNullOrEmpty(CType(oControl, TextBox).Text) Then
                                        If Not (CType(oControl, TextBox).Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper()) Then
                                            .DateOfBirth = CType(CType(oControl, TextBox).Text, Date)
                                        End If
                                    End If
                                Case "liBranchCode"
                                    If oMaster.FindControl("ddlBranchCode") IsNot Nothing Then
                                        oControl = oMaster.FindControl("ddlBranchCode")
                                        .BranchCode = CType(oControl, DropDownList).SelectedValue
                                    End If
                                Case "liSelectCurrency"
                                    If oMaster.FindControl("ddlCurrency") IsNot Nothing Then
                                        oControl = oMaster.FindControl("ddlCurrency")
                                        .Currency = CType(oControl, DropDownList).SelectedValue
                                    End If
                                Case "ddlPrimaryOccupation"
                                    .OccupationCode = CType(oControl, NexusProvider.LookupList).Value
                                Case "ddlSecondaryOccupation"
                                    .SecOccupationCode = CType(oControl, NexusProvider.LookupList).Value
                            End Select
                        Next
                    End With
                Case TypeOf v_oParty Is NexusProvider.OtherParty
                    'Updates the v_oParty object with Other party from aspx page.
                    With CType(v_oParty, NexusProvider.OtherParty)
                        .TypeCode = Session(CNPartyType)
                        For Each oControl As Control In oMaster.FindControl("PnlRegisterOP").Controls
                            Select Case oControl.ID
                                Case "txtName"
                                    .Name = CType(oControl, TextBox).Text
                                Case "txtCode"
                                    .Code = CType(oControl, TextBox).Text
                                    .ShortName = CType(oControl, TextBox).Text
                                Case "liFileCode"
                                    If oMaster.FindControl("txtFileCode") IsNot Nothing Then
                                        oControl = oMaster.FindControl("txtFileCode")
                                        .FileCode = CType(oControl, TextBox).Text
                                    End If
                                Case "Licence_Type"
                                    .LicenseTypeCode = CType(oControl, NexusProvider.LookupList).Value
                                Case "txtLicenceNO"
                                    .LicenseNumber = CType(oControl, TextBox).Text
                                Case "ddlDriverStatus"
                                    .DriverStatusCode = CType(oControl, NexusProvider.LookupList).Value
                                Case "txtRegistrationNO"
                                    .RegistrationNumber = CType(oControl, TextBox).Text
                                Case "ddlGender"
                                    .Gender = CType(oControl, NexusProvider.LookupList).Value
                                Case "ddlCurrency"
                                    .Currency = CType(oControl, DropDownList).SelectedValue
                                Case "txtDOB"
                                    If Not String.IsNullOrEmpty(CType(oControl, TextBox).Text) Then
                                        If Not (CType(oControl, TextBox).Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper()) Then
                                            .DateOfBirth = CType(CType(oControl, TextBox).Text, Date)
                                            .DOB = CType(CType(oControl, TextBox).Text, Date).ToString()
                                        End If
                                    Else
                                        .DateOfBirth = Nothing
                                        .DOB = Nothing
                                    End If
                                Case "liBranchCode", "ddlBranch"
                                    If oMaster.FindControl("ddlBranch") IsNot Nothing Then
                                        oControl = oMaster.FindControl("ddlBranch")
                                        .BranchCode = CType(oControl, DropDownList).SelectedValue
                                    End If
                                Case "liSubBranchCode", "ddlSubBranch"
                                    If oMaster.FindControl("ddlSubBranch") IsNot Nothing Then
                                        oControl = oMaster.FindControl("ddlSubBranch")
                                        .SubBranchCode = CType(oControl, DropDownList).SelectedValue
                                    End If
                                Case "ddlTPASettle"
                                    .IsTPASettleDirectly = IIf(String.IsNullOrEmpty(CType(oControl, DropDownList).SelectedValue.Trim()), 0, CType(oControl, DropDownList).SelectedValue.Trim())
                                Case "txtTaxNumber"
                                    .TaxNumber = CType(oControl, TextBox).Text
                                Case "txtTaxPercentage"
                                    If Not String.IsNullOrEmpty(CType(oControl, TextBox).Text) Then
                                        .TaxPercentage = CType(oControl, TextBox).Text
                                    Else
                                        .TaxPercentage = 0
                                    End If
                                    .TaxPercentage = Math.Round(.TaxPercentage, 4)
                                Case "chkTaxExempt"
                                    .TaxExempt = CType(oControl, CheckBox).Checked
                                Case "chkIsDomiciledForTax"
                                    .DomiciledForTax = CType(oControl, CheckBox).Checked
                            End Select
                        Next
                    End With
            End Select
        End Sub

#Region " SUBMIT "
        ''' <summary>
        ''' Submit button events saves the corporate / peronal client 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Sub BtnSubmitClientClick(ByVal sender As Object, ByVal e As System.EventArgs)
            If Page.IsValid Then
                Dim oParty As NexusProvider.BaseParty = Nothing
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim iPartyKey As Integer
                Dim bDuplicateClientCheck As Boolean = False
                Dim RequestedPageURL As String = Request.Url.Segments(Request.Url.Segments.Length - 1).ToString
                Dim oPartyBuilderPanel As Panel = oMaster.FindControl("PANEL__PARTYBUILDER") ' check for partybuilder exist
                Dim oPartyChildScreenPanel As Panel = oMaster.FindControl("PANEL__PARTYCHILDSCREEN")
                If Session(CNClientMode) = Mode.Edit Then

                    If Session(CNParty) IsNot Nothing Then
                        oParty = Session(CNParty)
                    End If

                    UpdateParty(oParty)

                    Dim bUpdatePartyBankResult As Boolean = UpdatePartyBank()

                    'Get the PartyBuilder panel and pass it to the method.


                    'if PartyBuilder screen is not available but partychildscreen is available then
                    'we need fresh XML to stroe the party builder data
                    If (oPartyBuilderPanel Is Nothing Or (oPartyBuilderPanel IsNot Nothing AndAlso oPartyBuilderPanel.Visible = False)) _
                    And (oPartyChildScreenPanel IsNot Nothing AndAlso oPartyChildScreenPanel.Visible = True) Then
                        'Need the fresh XMLDATASET as dataset returned from AddParty is not goof enough
                        oParty = oWebService.GetParty(oParty.Key)

                        'Store the Updated Party Object in Session.
                        Session(CNParty) = oParty
                    End If

                    If oPartyBuilderPanel IsNot Nothing AndAlso oPartyBuilderPanel.Visible = True Then
                        'Add the PartyBuilder Values from the container to the XML (if exists)
                        WritePartyControlsFromContainerToXML(oPartyBuilderPanel)
                    End If

                    If oPartyChildScreenPanel IsNot Nothing AndAlso oPartyChildScreenPanel.Visible = True Then
                        'Add the PartyBuilder Child Screen Values from the container to the XML (if exists)
                        WritePartyControlsFromContainerToXML(oPartyChildScreenPanel)
                    End If

                    'WritePartyControlsFromContainerToXML(oMaster)

                    Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
                    If oUserDetails IsNot Nothing AndAlso oUserDetails.Key > 0 Then
                        'AgentKey & Name are fetched from UserDetails
                        Select Case True
                            Case TypeOf oParty Is NexusProvider.CorporateParty
                                With CType(oParty, NexusProvider.CorporateParty)
                                    .ClientSharedData.LeadAgentKey = oUserDetails.Key
                                    .ClientSharedData.LeadAgentName = oUserDetails.PartyName
                                End With
                            Case TypeOf oParty Is NexusProvider.PersonalParty
                                With CType(oParty, NexusProvider.PersonalParty)
                                    .ClientSharedData.LeadAgentKey = oUserDetails.Key
                                    .ClientSharedData.LeadAgentName = oUserDetails.PartyName
                                End With
                        End Select
                    End If

                    If Not UpdatePartyCall(oParty, oParty.BranchCode) Then
                        Exit Sub
                    End If

                    If Request("modal") = "true" Then
                        'need to call GetParty again as in the case like Loyalty we need to get the refreshed Keys
                        iPartyKey = oParty.Key
                        oParty = oWebService.GetParty(iPartyKey)
                        If Session(CNUserName) IsNot Nothing And oParty.UserName Is Nothing Then
                            oParty.UserName = Session(CNUserName)
                        End If

                        Session(CNParty) = oParty
                        If Request("RequestType") = "MTA" Then
                            'add javascript to call script in parent page which will close modal dialog
                            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
                        ElseIf Request("RequestType") = "REN" Then
                            'add javascript to call script in parent page which will close modal dialog
                            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "closeThickBox", "self.parent.statementUpdated();", True)

                        End If
                    Else
                        If oParty.Associate IsNot Nothing AndAlso oParty.Associate.Count > 0 Then
                            oParty = oWebService.GetParty(oParty.Key)

                            'Store the Updated Party Object in Session.
                            Session(CNParty) = oParty
                        End If
                        Session(CNClientMode) = Mode.View
                        Select Case True
                            Case TypeOf oParty Is NexusProvider.CorporateParty
                                With CType(oParty, NexusProvider.CorporateParty)
                                    If String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                                        Response.Redirect("~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oParty.Key & "&Code=" & .ClientSharedData.ShortName.Trim() & "&UPR=" & bUpdatePartyBankResult.ToString(), True)
                                    ElseIf String.IsNullOrEmpty(.UserName) = False Then
                                        Response.Redirect("~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oParty.Key & "&Code=" & .UserName.Trim() & "UPR=" & bUpdatePartyBankResult.ToString(), True)
                                    End If
                                End With
                            Case TypeOf oParty Is NexusProvider.PersonalParty
                                With CType(oParty, NexusProvider.PersonalParty)
                                    If String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                                        Response.Redirect("~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & oParty.Key & "&Code=" & .ClientSharedData.ShortName.Trim() & "&UPR=" & bUpdatePartyBankResult.ToString(), True)
                                    ElseIf String.IsNullOrEmpty(.UserName) = False Then
                                        Response.Redirect("~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & oParty.Key & "&Code=" & .UserName.Trim() & "&UPR=" & bUpdatePartyBankResult.ToString(), True)
                                    End If
                                End With
                        End Select
                    End If

                ElseIf Session(CNClientMode) = Mode.Add Then

                    If Session(CNParty) IsNot Nothing Then
                        Select Case True
                            Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                                oParty = CType(Session(CNParty), NexusProvider.CorporateParty)
                            Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                                oParty = CType(Session(CNParty), NexusProvider.PersonalParty)
                            Case TypeOf Session(CNParty) Is NexusProvider.OtherParty
                                oParty = CType(Session(CNParty), NexusProvider.OtherParty)
                        End Select
                    End If

                    ' Duplicate Check before party creation
                    If oPortal.DuplicateClientCheckParameters IsNot Nothing Then
                        If oPortal.DuplicateClientCheckParameters.Trim.Length <> 0 Then
                            Dim oPartyCollection As NexusProvider.PartyCollection
                            Dim oSearchCriteria As New NexusProvider.PartySearchCriteria
                            Dim oAddressCollection As NexusProvider.AddressCollection = oParty.Addresses
                            Dim oAddress As New NexusProvider.Address
                            Dim oTempAddress As New NexusProvider.Address
                            Dim oContactCollection As NexusProvider.ContactCollection = oParty.Contacts
                            Dim oContact As New NexusProvider.Contact

                            Dim oTempContact As New NexusProvider.Contact
                            Dim sParameters() As String
                            Dim Flag_WildSearch As Boolean = False
                            Dim bLWildSearch As Boolean = False
                            Dim bRWildSearch As Boolean = False
                            Dim iDuplicateClientSearch As Integer
                            Dim sDuplicateClientSearch As String

                            Dim oDisableOptionSettings, oEnableOptionSettings As NexusProvider.OptionTypeSetting
                            oEnableOptionSettings = Nothing
                            'Disable All Wildcard Searches
                            oDisableOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5065)
                            'Enable Wildcard Searches Ending With %
                            oEnableOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5066)

                            'If SAM returns nothing for the option then set "0" -start
                            If oDisableOptionSettings.OptionValue Is Nothing Then
                                oDisableOptionSettings.OptionValue = "0"
                            End If

                            If oEnableOptionSettings.OptionValue Is Nothing Then
                                oEnableOptionSettings.OptionValue = "0"
                            End If

                            If oDisableOptionSettings IsNot Nothing AndAlso oDisableOptionSettings.OptionValue = "0" AndAlso oEnableOptionSettings IsNot Nothing AndAlso oEnableOptionSettings.OptionValue = "0" Then
                                'Allow Wildcard Searches 
                                Flag_WildSearch = True

                                ' If oPortal.DuplicateClientSearchType IsNot Nothing AndAlso oPortal.DuplicateClientSearchType.Trim.Length() > 0 Then
                                If (oPortal.DuplicateClientSearchType.Trim.Substring(0, 1) = "%" And Flag_WildSearch = True) Then
                                        'Enable Wildcard Searches Starting With %
                                        bLWildSearch = True
                                    End If

                                    If (oPortal.DuplicateClientSearchType.Trim.Substring(oPortal.DuplicateClientSearchType.Trim.Length() - 1, 1) = "%" And Flag_WildSearch = True) Then
                                        'Enable Wildcard Searches End With %
                                        bRWildSearch = True
                                    End If
                                'End If


                            ElseIf oEnableOptionSettings IsNot Nothing AndAlso oEnableOptionSettings.OptionValue = "1" Then
                                'Allow Wildcard Searches 
                                Flag_WildSearch = True
                                'Enable Wildcard Searches Starting With %
                                bLWildSearch = False

                                'If oPortal.DuplicateClientSearchType IsNot Nothing AndAlso oPortal.DuplicateClientSearchType.Trim.Length() > 0 Then
                                If (oPortal.DuplicateClientSearchType.Trim.Substring(oPortal.DuplicateClientSearchType.Trim.Length() - 1, 1) = "%" And Flag_WildSearch = True) Then
                                        'Enable Wildcard Searches End With %
                                        bRWildSearch = True
                                    End If
                                'End If

                            ElseIf oDisableOptionSettings IsNot Nothing AndAlso oDisableOptionSettings.OptionValue = "1" Then
                                'Allow Wildcard Searches 
                                Flag_WildSearch = False
                                'Enable Wildcard Searches Starting With %
                                bLWildSearch = False
                                'Enable Wildcard Searches End With %
                                bRWildSearch = False
                            End If

                            If oPortal.DuplicateClientSearchType.Trim.Length() > 0 Then
                                'removed All % sign
                                sDuplicateClientSearch = Replace(oPortal.DuplicateClientSearchType.Trim, "%", "")
                                iDuplicateClientSearch = sDuplicateClientSearch.Length
                            End If
                            'End If

                            If oAddressCollection IsNot Nothing Then
                                If oAddressCollection.Count > 0 Then
                                    For iCount As Integer = 0 To oAddressCollection.Count - 1
                                        If oAddressCollection(iCount).AddressType = NexusProvider.AddressType.CorrespondenceAddress Then
                                            oAddress = oAddressCollection(iCount)
                                        End If
                                    Next
                                End If
                            End If

                            If oContactCollection IsNot Nothing Then
                                If oContactCollection.Count > 0 Then
                                    For iCount As Integer = 0 To oContactCollection.Count - 1
                                        If oContactCollection(iCount).ContactType = NexusProvider.ContactType.HomePhone Then
                                            oContact = oContactCollection(iCount)
                                        End If
                                    Next
                                End If
                            End If

                            Dim sURL As String
                            If HttpContext.Current.Session.IsCookieless Then
                                sURL = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/DuplicateClients.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=750"
                            Else
                                sURL = AppSettings("WebRoot") & "Modal/DuplicateClients.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=750"
                            End If
                            sParameters = oPortal.DuplicateClientCheckParameters.Split(",")

                            For iCounter As Integer = 0 To sParameters.Length - 1
                                'if sParameters retrive Name option, Attached SearchCriteria with Name
                                If sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "NAME" Or sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "FIRSTNAME" Then
                                    If oMaster.FindControl("PnlRegisterPC") IsNot Nothing Then
                                        Dim sLastName, sTitle, sInitial, sFirstName As String
                                        For Each oControl As Control In oMaster.FindControl("PnlRegisterPC").Controls
                                            Select Case oControl.ID
                                                Case "txtLastname"
                                                    sLastName = CType(oControl, TextBox).Text
                                                Case "txtInitials"
                                                    sInitial = CType(oControl, TextBox).Text
                                                Case "txtFirstName"
                                                    sFirstName = CType(oControl, TextBox).Text
                                                Case "ddlTitle"
                                                    sTitle = CType(oControl, NexusProvider.LookupList).Value
                                            End Select
                                        Next
                                        Dim oEnhancedResolvedNameOption As NexusProvider.OptionTypeSetting
                                        oEnhancedResolvedNameOption = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5148)
                                        If oEnhancedResolvedNameOption IsNot Nothing AndAlso oEnhancedResolvedNameOption.OptionValue = "1" Then
                                            oSearchCriteria.Name = "%" & StrSearch(sFirstName & " " & sLastName, bLWildSearch, bRWildSearch, iDuplicateClientSearch)
                                        Else
                                            oSearchCriteria.Name = "%" & StrSearch(sInitial & " " & sLastName, bLWildSearch, bRWildSearch, iDuplicateClientSearch)
                                        End If
                                    ElseIf oMaster.FindControl("PnlRegisterCC") IsNot Nothing Then
                                        For Each oControl As Control In oMaster.FindControl("PnlRegisterCC").Controls
                                            Select Case oControl.ID
                                                Case "txtCompanyName"
                                                    oSearchCriteria.Name = StrSearch(CType(oControl, TextBox).Text, bLWildSearch, bRWildSearch, iDuplicateClientSearch)

                                            End Select
                                        Next

                                    End If
                                    'if sParameters retrive ADDRESSLINE1 option, Attached SearchCriteria with ADDRESSLINE1
                                ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "ADDRESSLINE1" Then
                                    If oAddress IsNot Nothing Then

                                        oTempAddress.Address1 = StrSearch(oAddress.Address1, bLWildSearch, bRWildSearch, iDuplicateClientSearch)
                                    End If
                                    'if sParameters retrive ADDRESSLINE2 option, Attached SearchCriteria with ADDRESSLINE2
                                ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "ADDRESSLINE2" Then
                                    If oAddress IsNot Nothing Then
                                        oTempAddress.Address2 = StrSearch(oAddress.Address2, bLWildSearch, bRWildSearch, iDuplicateClientSearch)
                                    End If
                                    'if sParameters retrive ADDRESSLINE3 option, Attached SearchCriteria with ADDRESSLINE3
                                ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "ADDRESSLINE3" Then
                                    If oAddress IsNot Nothing Then
                                        oTempAddress.Address3 = StrSearch(oAddress.Address3, bLWildSearch, bRWildSearch, iDuplicateClientSearch)
                                    End If
                                    'if sParameters retrive ADDRESSLINE4 option, Attached SearchCriteria with ADDRESSLINE4
                                ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "ADDRESSLINE4" Then
                                    If oAddress IsNot Nothing Then
                                        oTempAddress.Address4 = StrSearch(oAddress.Address4, bLWildSearch, bRWildSearch, iDuplicateClientSearch)
                                    End If
                                    'if sParameters retrive POSTCODE option, Attached SearchCriteria with POSTCODE
                                ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "POSTCODE" Then
                                    If oAddress IsNot Nothing Then
                                        oTempAddress.PostCode = StrSearch(oAddress.PostCode, bLWildSearch, bRWildSearch, iDuplicateClientSearch)
                                    End If
                                    'if sParameters retrive TELEPHONENUMBER option, Attached SearchCriteria with TELEPHONENUMBER
                                ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "TELEPHONENUMBER" Then
                                    If oContact IsNot Nothing Then
                                        If oContact IsNot Nothing Then
                                            oSearchCriteria.TelephoneNumber = StrSearch(oContact.Number, bLWildSearch, bRWildSearch, iDuplicateClientSearch)
                                        End If
                                    End If
                                ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "FILECODE" Then
                                    If oMaster.FindControl("txtFileCode") IsNot Nothing Then
                                        Dim oControl As Control = oMaster.FindControl("txtFileCode")
                                        oSearchCriteria.FileCode = StrSearch(CType(oControl, TextBox).Text, bLWildSearch, bRWildSearch, iDuplicateClientSearch)
                                    End If
                                End If
                            Next
                            oSearchCriteria.Address = oTempAddress

                            If oParty Is Nothing Then
                                'New Client
                                If RequestedPageURL.Contains("PERSONALCLIENTDETAILS.ASPX") = True Then
                                    '    Case "PersonalClient"
                                    oParty = New NexusProvider.PersonalParty()
                                ElseIf RequestedPageURL.Contains("CORPORATECLIENTDETAILS.ASPX") = True Then
                                    '    Case "CorporateClient"
                                    oParty = New NexusProvider.CorporateParty()
                                ElseIf RequestedPageURL.Contains("OTHERPARTYDETAILS") = True AndAlso
                                    RequestedPageURL.Contains(".ASPX") = True Then
                                    '    Case "OtherParty"
                                    oParty = New NexusProvider.OtherParty()
                                End If
                            Else
                                Select Case True
                                    Case TypeOf oParty Is NexusProvider.CorporateParty
                                        With CType(oParty, NexusProvider.CorporateParty)
                                            oSearchCriteria.PartyType = "CC"
                                            oSearchCriteria.PartyTypes.Add(NexusProvider.PartyType.Corporate)
                                        End With
                                    Case TypeOf oParty Is NexusProvider.PersonalParty
                                        With CType(oParty, NexusProvider.PersonalParty)
                                            oSearchCriteria.PartyType = "PC"
                                            oSearchCriteria.PartyTypes.Add(NexusProvider.PartyType.Personal)
                                        End With
                                    Case TypeOf oParty Is NexusProvider.OtherParty
                                        With CType(oParty, NexusProvider.OtherParty)
                                            oSearchCriteria.PartyType = "OTHER"
                                            oSearchCriteria.PartyTypes.Add(NexusProvider.PartyType.Other)
                                        End With
                                End Select
                            End If

                            Session.Remove(CNSearchData)
                            oPartyCollection = oWebService.FindParty(oSearchCriteria)

                            If oPartyCollection IsNot Nothing Then
                                If oPartyCollection.Count > 0 Then
                                    bDuplicateClientCheck = True
                                    Session(CNSearchData) = oPartyCollection
                                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show",
                                "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sURL & "' , null);});</script>")
                                End If
                            End If
                        End If
                    End If

                    Session(CNParty) = oParty
                    If bDuplicateClientCheck = False And Session(CNClientMode) = Mode.Add Then
                        AddParty(sender, e)

                        'Need the fresh XMLDATASET as dataset returned from AddParty is not goof enough
                        oParty = oWebService.GetParty(oParty.Key)
                        Dim oPartyBankDetails As NexusProvider.BankCollection
                        oPartyBankDetails = oWebService.GetPartyBankDetails(oParty.Key)
                        oParty.BankDetails = oPartyBankDetails
                        'Store the Updated Party Object in Session.
                        Session(CNParty) = oParty

                        If oPartyBuilderPanel IsNot Nothing AndAlso oPartyBuilderPanel.Visible = True Then
                            'Add the PartyBuilder Values from the container to the XML (if exists)
                            WritePartyControlsFromContainerToXML(oPartyBuilderPanel)
                        End If

                        If oPartyChildScreenPanel IsNot Nothing AndAlso oPartyChildScreenPanel.Visible = True Then
                            'Add the PartyBuilder Child Screen Values from the container to the XML (if exists)
                            WritePartyControlsFromContainerToXML(oPartyChildScreenPanel)
                        End If
                        If Not isModalOtherParty Then
                            If TypeOf Session(CNParty) Is NexusProvider.OtherParty AndAlso
                                (DirectCast(oParty, NexusProvider.OtherParty).TypeCode Is Nothing OrElse
                                String.IsNullOrEmpty(oParty.Type)) Then

                                DirectCast(oParty, NexusProvider.OtherParty).TypeCode = Session(CNPartyType)
                            End If
                            UpdatePartyCall(oParty, oParty.BranchCode)
                        End If
                        Select Case True
                            Case TypeOf oParty Is NexusProvider.CorporateParty
                                With CType(oParty, NexusProvider.CorporateParty)
                                    If String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                                        Response.Redirect("~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oParty.Key & "&Code=" & .ClientSharedData.ShortName.Trim(), True)
                                    ElseIf String.IsNullOrEmpty(.UserName) = False Then
                                        Response.Redirect("~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oParty.Key & "&Code=" & .UserName.Trim(), True)
                                    End If
                                End With
                            Case TypeOf oParty Is NexusProvider.PersonalParty
                                With CType(oParty, NexusProvider.PersonalParty)
                                    If String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                                        Response.Redirect("~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & oParty.Key & "&Code=" & .ClientSharedData.ShortName.Trim(), True)
                                    ElseIf String.IsNullOrEmpty(.UserName) = False Then
                                        Response.Redirect("~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & oParty.Key & "&Code=" & .UserName.Trim(), True)
                                    End If
                                End With
                            Case TypeOf oParty Is NexusProvider.OtherParty
                                If Not isModalOtherParty Then
                                    Session(CNClientMode) = Mode.View
                                End If
                        End Select
                    End If
                End If
            End If
        End Sub
#End Region
        Sub AddParty(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim oParty As NexusProvider.BaseParty = Session(CNParty)
            Dim oPartyBankDetails As NexusProvider.BankCollection
            Dim oEventDetails As New NexusProvider.EventDetails
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim iPartyKey As Integer
            Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)

            If Not isModalOtherParty Then
                UpdateParty(oParty)
            End If

            If oUserDetails IsNot Nothing AndAlso oUserDetails.Key > 0 Then
                'AgentKey & Name are fetched from UserDetails
                Select Case True
                    Case TypeOf oParty Is NexusProvider.CorporateParty
                        With CType(oParty, NexusProvider.CorporateParty)
                            .ClientSharedData.LeadAgentKey = oUserDetails.Key
                            .ClientSharedData.LeadAgentName = oUserDetails.PartyName
                        End With
                    Case TypeOf oParty Is NexusProvider.PersonalParty
                        With CType(oParty, NexusProvider.PersonalParty)
                            .ClientSharedData.LeadAgentKey = oUserDetails.Key
                            .ClientSharedData.LeadAgentName = oUserDetails.PartyName
                        End With
                End Select
            End If
            'Call the AddParty to get the XMLDATASET.
            oWebService.AddParty(oParty, oParty.BranchCode)
            Session(CNIsNewClient) = True
            iPartyKey = oParty.Key
            'Call SAM method to add the Banks
            If (oParty.BankDetails.Count > 0) Then
                oWebService.AddPartyBankDetails(iPartyKey, oParty.BankDetails)
            End If

            'Add a event for New Client Created
            With oEventDetails
                .EventDate = Now()
                .PartyKey = iPartyKey
                .RtfText = "Client Created"
                .UserName = Session(CNLoginName)
                .EventTypeKey = 1
                .EventLogSubjectKey = 1
            End With

            oWebService.AddEvent(oEventDetails)

            Session(CNUserName) = oParty.UserName
            ViewState.Add("CNUserName", oParty.UserName)

            'Update the xml for custom data -Start (if party builder is visible true)
            'Get the PartyBuilder panel and pass it to the method.
            Dim oPartyChildScreenPanel As Panel = oMaster.FindControl("PANEL__PARTYCHILDSCREEN")
            Dim oPartyBuilderPanel As Panel = oMaster.FindControl("PANEL__PARTYBUILDER") ' check for partybuilder exist

            If oPartyBuilderPanel IsNot Nothing AndAlso oPartyBuilderPanel.Visible = True Then

                'Need the fresh XMLDATASET as dataset returned from AddParty is not goof enough
                oParty = oWebService.GetParty(iPartyKey)

                oPartyBankDetails = oWebService.GetPartyBankDetails(iPartyKey)
                oParty.BankDetails = oPartyBankDetails

                'Fixed against Defect #3908: as we dont get UserName using GetParty so need to get it everytime from Request and update the oParty also
                If oParty.UserName Is Nothing OrElse oParty.UserName <> "" Then
                    oParty.UserName = Session(CNUserName)
                End If

                'Store the Updated Party Object in Session.
                Session(CNParty) = oParty

                'ReadPartyControlsContainerFromXML(oMaster, String.Empty, Me, True)

                'Add the PartyBuilder Values from the container to the XML (if exists)
                'ReadPartyControlsContainerFromXML(oMaster, String.Empty, Me, True)
                WritePartyControlsFromContainerToXML(oPartyBuilderPanel)

                If oPartyChildScreenPanel Is Nothing Then

                    If oUserDetails IsNot Nothing AndAlso oUserDetails.Key > 0 Then
                        'AgentKey & Name are fetched from UserDetails
                        Select Case True
                            Case TypeOf oParty Is NexusProvider.CorporateParty
                                With CType(oParty, NexusProvider.CorporateParty)
                                    .ClientSharedData.LeadAgentKey = oUserDetails.Key
                                    .ClientSharedData.LeadAgentName = oUserDetails.PartyName
                                End With
                            Case TypeOf oParty Is NexusProvider.PersonalParty
                                With CType(oParty, NexusProvider.PersonalParty)
                                    .ClientSharedData.LeadAgentKey = oUserDetails.Key
                                    .ClientSharedData.LeadAgentName = oUserDetails.PartyName
                                End With
                        End Select
                    End If

                    If Not UpdatePartyCall(oParty, oParty.BranchCode) Then
                        Exit Sub
                    End If

                    Session(CNClientMode) = Mode.View
                    Select Case True
                        Case TypeOf oParty Is NexusProvider.CorporateParty
                            With CType(oParty, NexusProvider.CorporateParty)
                                Response.Redirect("~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oParty.Key & "&Code=" & .ClientSharedData.ShortName.Trim(), True)
                            End With
                        Case TypeOf oParty Is NexusProvider.PersonalParty
                            With CType(oParty, NexusProvider.PersonalParty)
                                Response.Redirect("~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & oParty.Key & "&Code=" & .ClientSharedData.ShortName.Trim(), True)
                            End With
                        Case TypeOf oParty Is NexusProvider.OtherParty
                            With CType(oParty, NexusProvider.OtherParty)
                                Response.Redirect("~/secure/agent/OtherPartyDetails.aspx?PartyKey=" & oParty.Key & "&Code=" & .ShortName.Trim(), True)
                            End With
                    End Select

                ElseIf oPartyChildScreenPanel IsNot Nothing Then
                    Session(CNClientMode) = Mode.Edit
                    LoadClient()
                End If
            ElseIf oPartyBuilderPanel Is Nothing Or (oPartyBuilderPanel IsNot Nothing AndAlso oPartyBuilderPanel.Visible = False) Then
                Session(CNClientMode) = Mode.View

                If Session(CNIsNewClient) = True Then
                    oParty = oWebService.GetParty(oParty.Key)

                    oPartyBankDetails = oWebService.GetPartyBankDetails(iPartyKey)
                    oParty.BankDetails = oPartyBankDetails

                    'Store the Updated Party Object in Session.
                    Session(CNParty) = oParty
                End If

                Session(CNClientMode) = Mode.View
                Dim sCode As String = String.Empty
                If oParty.UserName IsNot Nothing AndAlso Not String.IsNullOrEmpty(oParty.UserName) Then
                    sCode = oParty.UserName.Trim
                Else
                    Select Case True
                        Case TypeOf oParty Is NexusProvider.CorporateParty
                            With CType(oParty, NexusProvider.CorporateParty)
                                sCode = .ClientSharedData.ShortName.Trim
                            End With
                        Case TypeOf oParty Is NexusProvider.PersonalParty
                            With CType(oParty, NexusProvider.PersonalParty)
                                sCode = .ClientSharedData.ShortName.Trim
                            End With
                        Case TypeOf oParty Is NexusProvider.OtherParty
                            With CType(oParty, NexusProvider.OtherParty)
                                sCode = .Code.Trim()
                            End With
                    End Select
                End If

                Select Case True
                    Case TypeOf oParty Is NexusProvider.CorporateParty
                        With CType(oParty, NexusProvider.CorporateParty)
                            Response.Redirect("~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oParty.Key & "&Code=" & CType(oParty, NexusProvider.CorporateParty).ClientSharedData.ShortName.Trim(), True)
                        End With
                    Case TypeOf oParty Is NexusProvider.PersonalParty
                        With CType(oParty, NexusProvider.PersonalParty)
                            Response.Redirect("~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & oParty.Key & "&Code=" & CType(oParty, NexusProvider.PersonalParty).ClientSharedData.ShortName.Trim(), True)
                        End With
                    Case TypeOf oParty Is NexusProvider.OtherParty
                        With CType(oParty, NexusProvider.OtherParty)
                            If Not isModalOtherParty Then
                                Response.Redirect("~/secure/agent/OtherPartyDetails.aspx?PartyKey=" & oParty.Key & "&Code=" & CType(oParty, NexusProvider.OtherParty).ShortName.Trim(), True)
                            End If
                        End With
                End Select
            End If
            'Update the xml for custom data -End
        End Sub
#Region " UPDATE "
        ''' <summary>
        ''' Updates Corporate Client
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Sub BtnUpdateClientClick(ByVal sender As Object, ByVal e As System.EventArgs)

            Dim oParty As NexusProvider.BaseParty = Nothing
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oPartyChildScreenPanel As Panel = oMaster.FindControl("PANEL__PARTYCHILDSCREEN")
            Dim oPartyBuilderPanel As Panel = oMaster.FindControl("PANEL__PARTYBUILDER")

            Try
                If oPartyBuilderPanel IsNot Nothing AndAlso oPartyBuilderPanel.Visible = True Then
                    'Add the PartyBuilder Values from the container to the XML (if exists)
                    WritePartyControlsFromContainerToXML(oPartyBuilderPanel)
                End If

                If oPartyChildScreenPanel IsNot Nothing AndAlso oPartyChildScreenPanel.Visible = False Then
                    'Add the Party Child screen Builder Values from the container to the XML (if exists)
                    WritePartyControlsFromContainerToXML(oPartyChildScreenPanel)
                End If

                'Add the AdditionalControl Values from the container to the XML (if exists)
                'WriteAdditionalControlsFromContainer(oMaster)

                If Session(CNParty) IsNot Nothing Then
                    oParty = Session(CNParty)
                End If

                Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
                If oUserDetails IsNot Nothing AndAlso oUserDetails.Key > 0 Then
                    'AgentKey & Name are fetched from UserDetails
                    Select Case True
                        Case TypeOf oParty Is NexusProvider.CorporateParty
                            With CType(oParty, NexusProvider.CorporateParty)
                                .ClientSharedData.LeadAgentKey = oUserDetails.Key
                                .ClientSharedData.LeadAgentName = oUserDetails.PartyName
                            End With
                        Case TypeOf oParty Is NexusProvider.PersonalParty
                            With CType(oParty, NexusProvider.PersonalParty)
                                .ClientSharedData.LeadAgentKey = oUserDetails.Key
                                .ClientSharedData.LeadAgentName = oUserDetails.PartyName
                            End With
                    End Select
                End If

                'SAM call
                If Not UpdatePartyCall(oParty, oParty.BranchCode) Then
                    Exit Sub
                End If
                UpdatePartyBank()

                If Session(CNUserName) IsNot Nothing Then
                    oParty.UserName = Session(CNUserName)
                End If

                Session(CNParty) = oParty

                'Change the mode to VIEW
                Session(CNClientMode) = Mode.View

                ' If Agent is doing a QQ without selecting a Client(CNAnonymousUser)
                If Session.Item(CNQuoteMode) = QuoteMode.QuickQuote And Session(CNAnonymous) IsNot Nothing _
                And Session(CNIsAnonymous) = True Then
                    Session.Remove(CNAnonymous)
                    Response.Redirect("~/QQPremium.aspx", False)
                End If

                Select Case True
                    Case TypeOf oParty Is NexusProvider.CorporateParty
                        With CType(oParty, NexusProvider.CorporateParty)
                            Response.Redirect("~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oParty.Key & "&Code=" & .ClientSharedData.ShortName.Trim(), True)
                        End With
                    Case TypeOf oParty Is NexusProvider.PersonalParty
                        With CType(oParty, NexusProvider.PersonalParty)
                            Response.Redirect("~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & oParty.Key & "&Code=" & .ClientSharedData.ShortName.Trim(), True)
                        End With
                End Select

            Finally
                oWebService = Nothing
                oParty = Nothing
            End Try
        End Sub
#End Region

#Region " SAVE CHILD BUTTON "
        Sub SavePartyBuilderData()
            If Page.IsValid Then
                Dim oParty As NexusProvider.BaseParty = Nothing
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

                Dim bDuplicateClientCheck As Boolean = False
                Dim RequestedPageURL As String = Request.Url.Segments(Request.Url.Segments.Length - 1).ToString
                If Session(CNClientMode) = Mode.Edit Then

                    If Session(CNParty) IsNot Nothing Then
                        oParty = Session(CNParty)
                    End If

                    UpdateParty(oParty)

                    Dim bUpdatePartyBankResult As Boolean = UpdatePartyBank()

                    'Get the PartyBuilder panel and pass it to the method.
                    oPartyBuilderPanel = oMaster.FindControl("PANEL__PARTYBUILDER") ' check for partybuilder exist
                    oPartyChildScreenPanel = oMaster.FindControl("PANEL__PARTYCHILDSCREEN")

                    'if PartyBuilder screen is not available but partychildscreen is available then
                    'we need fresh XML to stroe the party builder data
                    If (oPartyBuilderPanel Is Nothing Or (oPartyBuilderPanel IsNot Nothing AndAlso oPartyBuilderPanel.Visible = False)) _
                    And (oPartyChildScreenPanel IsNot Nothing AndAlso oPartyChildScreenPanel.Visible = True) Then
                        'Need the fresh XMLDATASET as dataset returned from AddParty is not good enough
                        oParty = oWebService.GetParty(oParty.Key)

                        'Store the Updated Party Object in Session.
                        Session(CNParty) = oParty

                    End If

                    If oPartyBuilderPanel IsNot Nothing AndAlso oPartyBuilderPanel.Visible = True Then
                        'Add the PartyBuilder Values from the container to the XML (if exists)
                        WritePartyControlsFromContainerToXML(oPartyBuilderPanel)
                    End If

                    If oPartyChildScreenPanel IsNot Nothing AndAlso oPartyChildScreenPanel.Visible = True Then
                        'Add the PartyBuilder Child Screen Values from the container to the XML (if exists)
                        WritePartyControlsFromContainerToXML(oPartyChildScreenPanel)
                    End If

                    'WritePartyControlsFromContainerToXML(oMaster)

                    Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
                    If oUserDetails IsNot Nothing AndAlso oUserDetails.Key > 0 Then
                        'AgentKey & Name are fetched from UserDetails
                        Select Case True
                            Case TypeOf oParty Is NexusProvider.CorporateParty
                                With CType(oParty, NexusProvider.CorporateParty)
                                    .ClientSharedData.LeadAgentKey = oUserDetails.Key
                                    .ClientSharedData.LeadAgentName = oUserDetails.PartyName
                                End With
                            Case TypeOf oParty Is NexusProvider.PersonalParty
                                With CType(oParty, NexusProvider.PersonalParty)
                                    .ClientSharedData.LeadAgentKey = oUserDetails.Key
                                    .ClientSharedData.LeadAgentName = oUserDetails.PartyName
                                End With
                        End Select
                    End If

                    oWebService.UpdateParty(oParty, oParty.BranchCode)

                    Session(CNParty) = oParty
                End If

                oWebService = Nothing
            End If

        End Sub
        Public Sub SaveChildButtonWithAllData(ByVal sender As Object, ByVal e As System.EventArgs)
            SavePartyBuilderData()
        End Sub

        ''' <summary>
        ''' Handles the Save button from a RiskContainer control, again this needs to be manually
        ''' hooked upto the OnClick event of the button to allow flexibility of the calling control.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        ''' 
        Public Overridable Sub SaveChildButton(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim oParty As NexusProvider.BaseParty = Nothing

            If Session(CNParty) IsNot Nothing Then
                oParty = Session(CNParty)
            End If

            'Get the Value from the Session
            oOI = Session.Item(CNOI)

            'If XMLDataset Exists
            If (CType(Session.Item(CNMode), Mode) <> Mode.View Or CType(Session.Item(CNClientMode), Mode) = Mode.Edit) And oParty.XMLDataset <> String.Empty Then
                Dim oRiskContainer As RiskContainer = sender.Parent

                If oRiskContainer IsNot Nothing Then

                    If oRiskContainer.Mode = RiskContainer.ChildMode.Add Then
                        oRiskContainer.OI = CreateElementFromXML(oRiskContainer.ScreenCode,
                                                   oOI.Peek, oRiskContainer.ParentElement, oRiskContainer.ChildElement)
                    End If

                    'ADD CHILD CONTROL VALUES TO XML DATASET
                    WritePartyControlsFromContainerToXML(oRiskContainer, oRiskContainer.ScreenCode, oRiskContainer.OI)

                    'RESET CHILD CONTROL
                    FrameWorkFunctions.ResetControls(oRiskContainer)

                    'RELOAD EDITED CHILD SCREEN VALUES IN ITEMGRID
                    If IsPostBack Then
                        ReadPartyControlsContainerFromXML(oMaster, oOI.Peek, Me, True)
                    Else
                        'If oPartyChildScreenPanel IsNot Nothing Then
                        'ReadPartyControlsContainerFromXML(oPartyChildScreenPanel, oOI.Peek, Me)
                        ReadPartyControlsContainerFromXML(oMaster, oOI.Peek, Me)
                    End If
                    oRiskContainer.Mode = RiskContainer.ChildMode.Add
                    HttpContext.Current.Session(CNParty) = oParty

                End If
            End If
        End Sub
#End Region

#Region " CANCEL CHILD BUTTON "
        ''' <summary>
        ''' Event to handle the cancelling of an edit/add on a child item with the RiskContainer control,
        ''' needs to be manually hooked upto the OnClick event of the calling control.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Sub CancelChildButton(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim oRiskContainer As RiskContainer = sender.Parent

            If oRiskContainer IsNot Nothing Then
                'Remove the invalid child node(if any)
                RemoveInvalidNodeFromXML(oRiskContainer.OI)

                'RESET CHILD CONTROL
                FrameWorkFunctions.ResetControls(oRiskContainer)
                oRiskContainer.Mode = RiskContainer.ChildMode.Add

                'Reload Defaults
                ReadPartyControlsContainerFromXML(oMaster, oOI.Peek.ToString(), Me)
            End If
        End Sub
#End Region
#Region " Correspondence Address - Server Validation "

        Protected Sub cusvldAddress_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
            Dim oParty As NexusProvider.BaseParty
            Dim oAddressCollection As NexusProvider.AddressCollection = Nothing
            If Session(CNParty) IsNot Nothing Then
                Select Case True
                    Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                        oParty = CType(Session(CNParty), NexusProvider.CorporateParty)
                        oAddressCollection = oParty.Addresses
                    Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                        oParty = CType(Session(CNParty), NexusProvider.PersonalParty)
                        oAddressCollection = oParty.Addresses
                    Case TypeOf Session(CNParty) Is NexusProvider.OtherParty
                        oParty = CType(Session(CNParty), NexusProvider.OtherParty)
                        oAddressCollection = oParty.Addresses
                End Select
            End If

            If oAddressCollection Is Nothing Then
                CType(source, CustomValidator).ErrorMessage = GetLocalResourceObject("MandatoryCA").ToString()
                args.IsValid = False
            ElseIf oAddressCollection.Count = 0 Then
                CType(source, CustomValidator).ErrorMessage = GetLocalResourceObject("MandatoryCA").ToString()
                args.IsValid = False
            ElseIf oAddressCollection.Count > 0 Then
                Dim iTotalCount As Integer = 0
                Dim iCount As Integer = 0
                For iCount = 0 To oAddressCollection.Count - 1
                    If oAddressCollection(iCount).AddressType = NexusProvider.AddressType.CorrespondenceAddress Then
                        iTotalCount = iTotalCount + 1
                    End If
                Next
                If iTotalCount > 0 Then
                    args.IsValid = True
                Else
                    CType(source, CustomValidator).ErrorMessage = GetLocalResourceObject("MandatoryCA").ToString()
                    args.IsValid = False
                End If
            Else
                args.IsValid = True
            End If

        End Sub

#End Region
#Region "Preferred Correspondence - Contact Type"
        Protected Sub cusvlPreferredCorrespondence_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
            Dim oParty As NexusProvider.BaseParty
            Dim oContactsCollection As NexusProvider.ContactCollection = Nothing
            Dim strPreferredCorrespondenceType As String = String.Empty
            Dim bValid As Boolean = False
            If Session(CNParty) IsNot Nothing Then
                Select Case True
                    Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                        oParty = CType(Session(CNParty), NexusProvider.CorporateParty)
                        With CType(oParty, NexusProvider.CorporateParty)
                            oContactsCollection = .Contacts
                            strPreferredCorrespondenceType = .ClientSharedData.CorrespondenceCode
                        End With
                    Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                        oParty = CType(Session(CNParty), NexusProvider.PersonalParty)
                        With CType(oParty, NexusProvider.PersonalParty)
                            oContactsCollection = .Contacts
                            strPreferredCorrespondenceType = .ClientSharedData.CorrespondenceCode
                        End With
                End Select
            End If

            ' Contact collection is empty
            'Case 1 - Preferred Correspondence  = empty : Pass
            'Case 2 - Preferred Correspondence  = LETTER : Pass
            'Case 3 - Preferred Correspondence  <> LETTER : Fail
            If oContactsCollection Is Nothing OrElse oContactsCollection.Count = 0 Then
                If (strPreferredCorrespondenceType Is Nothing OrElse String.IsNullOrEmpty(strPreferredCorrespondenceType)) Then
                    args.IsValid = True
                Else
                    If strPreferredCorrespondenceType.Trim.ToUpper = "LETTER" Then
                        args.IsValid = True
                    Else
                        CType(source, CustomValidator).ErrorMessage = GetLocalResourceObject("MatchingContactType").ToString()
                        args.IsValid = False
                    End If
                End If
            Else
                ' Contact collection is not empty
                'Case 1 - Preferred Correspondence  = empty : Pass
                'Case 2 - Preferred Correspondence  = LETTER : Pass
                'Case 3 - Preferred Correspondence  <> LETTER : Loop through the collection, if found then pass else fail
                'Modification for issue #59650 - added Condition for Email

                If (strPreferredCorrespondenceType Is Nothing OrElse String.IsNullOrEmpty(strPreferredCorrespondenceType)) Then
                    args.IsValid = True
                Else
                    If strPreferredCorrespondenceType.Trim.ToUpper = "LETTER" Then
                        args.IsValid = True
                    Else
                        For iCount As Integer = 0 To oContactsCollection.Count - 1
                            Select Case strPreferredCorrespondenceType.Trim.ToUpper
                                Case "E-MAIL", "EMAIL"
                                    If oContactsCollection(iCount).OtherContactTypeCode.ToString.Trim.ToUpper = strPreferredCorrespondenceType.Trim.ToUpper OrElse
                                       oContactsCollection(iCount).OtherContactTypeCode.ToString.Trim.ToUpper = "EMAIL" OrElse
                                       oContactsCollection(iCount).ContactType.ToString.Trim.ToUpper = "EMAIL" Then
                                        bValid = True
                                        Exit For
                                    End If
                                Case Else
                                    If oContactsCollection(iCount).ContactType.ToString.Trim.ToUpper = strPreferredCorrespondenceType.Trim.ToUpper Then
                                        bValid = True
                                        Exit For
                                    End If
                            End Select
                        Next

                        If Not bValid Then
                            CType(source, CustomValidator).ErrorMessage = GetLocalResourceObject("MatchingContactType").ToString()
                            args.IsValid = False
                        End If


                    End If
                End If
            End If


        End Sub
#End Region

        Protected Sub ddlBranchCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            If oMaster.FindControl("ddlCurrency") IsNot Nothing Then 'If CurrencyCode field is there than bind this control
                Dim dllCurrency As DropDownList = CType(oMaster.FindControl("ddlCurrency"), DropDownList)
                'Currency will be populated with BaseCurrency set in Bo
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oCurrencyColl As NexusProvider.CurrencyCollection
                If oMaster.FindControl("ddlBranchCode") IsNot Nothing Then
                    Dim dBranchCode As DropDownList = oMaster.FindControl("ddlBranchCode")
                    oCurrencyColl = oWebService.GetCurrenciesByBranch(dBranchCode.SelectedValue)
                Else
                    oCurrencyColl = oWebService.GetCurrenciesByBranch(Session(CNBranchCode))
                End If
                dllCurrency.Items.Clear()
                For i As Integer = 0 To oCurrencyColl.Count - 1
                    Dim lstCurrency As New ListItem
                    lstCurrency.Text = oCurrencyColl.Item(i).Description.ToString
                    lstCurrency.Value = Trim(oCurrencyColl.Item(i).CurrencyCode.ToString)
                    dllCurrency.Items.Add(lstCurrency)
                Next
                dllCurrency.SelectedValue = oCurrencyColl(0).BaseCurrencyCode
                dllCurrency.DataBind()

            End If
        End Sub
        ''' <summary>
        ''' This will transfer the anonymous quote to selected party from find client screen
        ''' </summary>
        ''' <remarks></remarks>
        Sub TransferQuoteToSelectedParty()

            Dim oWebService As NexusProvider.ProviderBase
            Dim oAnonymousQuote As NexusProvider.Quote = Current.Session.Item(CNQuote)
            Dim oQuote As New NexusProvider.Quote(oAnonymousQuote.CoverStartDate, oAnonymousQuote.CoverEndDate, oAnonymousQuote.Description)
            Dim sBranchCode As String
            Dim iPartyKey As Integer = CType(Request("partykey"), Integer)
            Dim sAnonPartyKey As Integer = oAnonymousQuote.PartyKey
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)

            'Find the branch code for new quote
            If oAnonymousQuote.BranchCode.Trim() = "" Then
                sBranchCode = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).BranchCode
            Else
                sBranchCode = oAnonymousQuote.BranchCode
            End If

            Try
                oWebService = New NexusProvider.ProviderManager().Provider
                'Call SAM method to transfer the anonymous quote to real party
                oWebService.TransferQuoteToRealParty(sAnonPartyKey, iPartyKey, oAnonymousQuote.InsuranceFileKey, sBranchCode)
                'Repopulate the oQuote object and session after transferring the quote to real party
                oQuote = oWebService.GetHeaderAndSummariesByKey(oAnonymousQuote.InsuranceFileKey)
                For iCount As Integer = 0 To oQuote.Risks.Count - 1
                    oWebService.GetRisk(oQuote.Risks(iCount).Key, iCount, oQuote)
                Next
                'This will retrive many other risk related details for a quote
                oWebService.GetHeaderAndRisksByKey(oQuote)

            Finally
                oWebService = Nothing
            End Try

            'If screencode is not populated by SAM method then find it and update the quote object
            '-Start getting screen code for risk screen
            Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(CMS.Library.Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
            Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name & "/"
            Dim sScreenCode As String = Nothing
            Dim oRiskType As Config.RiskType

            If oQuote.Risks(Current.Session(CNCurrentRiskKey)).RiskCode Is Nothing Then
                oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(Current.Session(CNCurrentRiskKey)).RiskTypeCode.Trim)
            Else
                oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(Current.Session(CNCurrentRiskKey)).RiskCode.Trim)
            End If
            If oQuote.Risks(Current.Session(CNCurrentRiskKey)).ScreenCode IsNot Nothing Then
                If oQuote.Risks(Current.Session(CNCurrentRiskKey)).ScreenCode.Trim.Length <> 0 Then
                    sScreenCode = oQuote.Risks(Current.Session(CNCurrentRiskKey)).ScreenCode
                Else
                    sScreenCode = GetScreenCode(sProductFolder & oRiskType.Path & "\" & oProduct.FullQuoteConfig)
                End If
            Else
                sScreenCode = GetScreenCode(sProductFolder & oRiskType.Path & "\" & oProduct.FullQuoteConfig)
            End If
            oQuote.Risks(0).ScreenCode = sScreenCode
            '-End getting screen code for risk screen

            Current.Session(CNQuote) = oQuote

            If Session(CNIsNewClient) IsNot Nothing Then
                Session.Remove(CNIsNewClient)
            End If
            Session.Remove(CNIsAnonymous)
            Session.Remove(CNAnonymous)

        End Sub


        ''' <summary>
        ''' To Remove invalid child node from XML
        ''' </summary>
        ''' <param name="sOI"></param>
        ''' <remarks></remarks>
        Private Sub RemoveInvalidNodeFromXML(ByVal sOI As String)

            Dim oParty As NexusProvider.BaseParty = Session(CNParty)
            Dim xmlTR As New XmlTextReader(New System.IO.StringReader(oParty.XMLDataset))
            Dim oPartyDoc As New XmlDocument
            Dim srDataset As New System.IO.StringReader(oParty.XMLDataset)
            xmlTR = New XmlTextReader(srDataset)
            oPartyDoc = New XmlDocument
            oPartyDoc.Load(xmlTR)
            xmlTR.Close()

            Dim oNode As XmlNode = oPartyDoc.SelectSingleNode("//*[@OI='" & sOI & "' and @US='3']")

            If oNode IsNot Nothing Then
                oNode.ParentNode.RemoveChild(oNode)

                Dim swContent As New System.IO.StringWriter
                Dim xmlwContent As New XmlTextWriter(swContent)

                oPartyDoc.WriteTo(xmlwContent)
                oParty.XMLDataset = swContent.ToString()

                xmlwContent.Close()
                swContent.Close()

                Session(CNParty) = oParty
            End If

            srDataset.Dispose()
            oPartyDoc = Nothing

            oParty = Nothing
            xmlTR = Nothing
            oPartyDoc = Nothing
            oNode = Nothing

        End Sub

         Sub BtnAccountStatementClick(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim sURL As String
            Dim oParty As NexusProvider.BaseParty = HttpContext.Current.Session(CNParty)
            Dim sEmailTemplateCode As String = ""
            Select Case True
                Case TypeOf oParty Is NexusProvider.CorporateParty
                    sEmailTemplateCode = "AccountStatement_CC"

                Case TypeOf oParty Is NexusProvider.PersonalParty
                    sEmailTemplateCode = "AccountStatement_PC"
            End Select

            If HttpContext.Current.Session.IsCookieless Then
                sURL = System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/SendEmail.aspx?PartyKey=" & oParty.Key & "&key=Issued&modal=true&CalledFrom=" & sEmailTemplateCode & "&loc=manual&n=p&Riskcheck=true&KeepThis=true&TB_iframe=true&height=300&width=750"
            Else
                sURL = ConfigurationManager.AppSettings("WebRoot") & "Modal/SendEmail.aspx?PartyKey=" & oParty.Key & "&key=Issued&modal=true&CalledFrom=" & sEmailTemplateCode & "&loc=manual&n=p&Riskcheck=true&KeepThis=true&TB_iframe=true&height=300&width=750"
            End If

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show",
            "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sURL & "' , null);});</script>")
        End Sub

        Private Sub ShowHideStatementButton()
            Dim EmailTemplates As New Nexus.Library.Config.EmailTemplates
            Dim bPCEmailTemplateExists As Boolean = False
            Dim bCCEmailTemplateExists As Boolean = False
            EmailTemplates = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID()).EmailTemplates

            If oMaster.FindControl("btnAccountStatement") IsNot Nothing Then
                If EmailTemplates IsNot Nothing And EmailTemplates.Count > 0 Then
                    For i As Integer = 0 To EmailTemplates.Count - 1
                        If EmailTemplates.EmailTemplate(i).ID.ToString().ToUpper() = "ACCOUNTSTATEMENT_PC" AndAlso EmailTemplates.EmailTemplate(i).EmailTemplateCode.Trim <> "" Then
                            bPCEmailTemplateExists = True
                            Exit For
                        End If
                        If EmailTemplates.EmailTemplate(i).ID.ToString().ToUpper() = "ACCOUNTSTATEMENT_CC" AndAlso EmailTemplates.EmailTemplate(i).EmailTemplateCode.Trim <> "" Then
                            bCCEmailTemplateExists = True
                            Exit For
                        End If
                    Next
                End If

                If bPCEmailTemplateExists OrElse bCCEmailTemplateExists Then
                    CType(oMaster.FindControl("btnAccountStatement"), LinkButton).Visible = True
                End If
            End If
        End Sub
    End Class
End Namespace

