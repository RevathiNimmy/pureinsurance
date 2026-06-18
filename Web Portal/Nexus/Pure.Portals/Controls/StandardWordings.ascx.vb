Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Utils
Imports Nexus.Library
Imports CMS.Library
Imports System.IO
Imports System.Xml
Imports System.Exception
Imports System.Web.Configuration.WebConfigurationManager

Namespace Nexus
    Partial Class Controls_StandardWordings
        Inherits System.Web.UI.UserControl
        Dim FolderPath As String
        Dim Pagename As String
        ''' used to capture level of standarwording used -policy level or risk level
        Dim bSupportRiskLevel As Boolean
        ''' used to capture editing rights of standarwording 
        Dim bAllowStandardWordingEdit As Boolean

        Dim bAllowEdit As Boolean
        Dim bAllowPreview As Boolean
        Dim bAllowAdd As Boolean = True
        Dim bAllowReorder As Boolean = True
        Dim bUsePickList As Boolean = True
        Dim bAllowDelete As Boolean = False
        Dim bSetDefault As Boolean
        Dim bAllowEditInControl As Boolean = True

        ''' <summary>
        ''' will set the property in bSupportRiskLevel
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public Property SupportRiskLevel() As Boolean
            Get
                Return bSupportRiskLevel
            End Get
            Set(ByVal value As Boolean)
                bSupportRiskLevel = value
            End Set
        End Property


        ''' <summary>
        ''' will set the property in bAllowEdit
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property AllowEdit() As Boolean
            Set(ByVal value As Boolean)
                bAllowEditInControl = value
            End Set
        End Property

        ''' <summary>
        ''' will set the property in bAllowPreview
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property AllowPreview() As Boolean
            Set(ByVal value As Boolean)
                bAllowPreview = value
            End Set
        End Property

        ''' <summary>
        ''' will set the property in bAllowAdd
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property AllowAdd() As Boolean
            Set(ByVal value As Boolean)
                bAllowAdd = value
            End Set
        End Property

        ''' <summary>
        ''' will set the property in bAllowReorder
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property AllowReorder() As Boolean
            Set(ByVal value As Boolean)
                bAllowReorder = value
            End Set
        End Property

        ''' <summary>
        ''' will set the property in bUsePickList
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property UsePickList() As Boolean
            Set(ByVal value As Boolean)
                bUsePickList = value
            End Set
        End Property

        ''' <summary>
        ''' will set the property in bUsePickList
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property AllowDelete() As Boolean
            Set(ByVal value As Boolean)
                bAllowDelete = value
            End Set
        End Property

        '''' <summary>
        '''' will set the property in bShowDefaultButton
        '''' </summary>
        '''' <value></value>
        '''' <remarks></remarks>
        'Public WriteOnly Property ShowDefaultButton() As Boolean
        '    Set(ByVal value As Boolean)
        '        bShowDefaultButton = value
        '    End Set
        'End Property
        ''' <summary>
        ''' Populate gridview with templates that are selected for the current insurance file
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub FillGrid(Optional ByVal sSWOI As String = "")
            'Populate the grid
            'This method is called on page_load as well as from LoadRiskCOntrol method
            Dim dtc As NexusProvider.DocumentTemplateCollection

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)

            bAllowEdit = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.AllowStandardWordingEdit, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
            Dim sChild As String
            Dim sNewRiskStdWrdSession As String
            sChild = Regex.Split(Me.ID.ToUpper(), "__")(1)
            sChild = "SW." + sChild
            sNewRiskStdWrdSession = CNRiskStandardWordingsTemplate + sChild
            'If viewstate is nothing means data is loading first time
            'Check whether we are proceeding for Risk or Policy Level
            'If (Session(sNewRiskStdWrdSession) Is Nothing Or CType(Session(sNewRiskStdWrdSession), NexusProvider.DocumentTemplateCollection).Count = 0) AndAlso bSupportRiskLevel Then
            If (Session(sNewRiskStdWrdSession) Is Nothing Or (Session(sNewRiskStdWrdSession) IsNot Nothing AndAlso CType(Session(sNewRiskStdWrdSession), NexusProvider.DocumentTemplateCollection).Count = 0)) AndAlso bSupportRiskLevel Then
                If oQuote IsNot Nothing Then
                    'check the value of bSupportRiskLevel, if it is true then it risk level Standardwording else Policy level
                    If bSupportRiskLevel Then
                        'initialize the xml documment to retrive xml from risk xml dataset
                        Dim Doc As New XmlDocument
                        Dim sOIForControl As String = ""
                        'declare a local variable to get the StandardWording Property Value
                        Dim sPropertyName As String = String.Empty
                        'Load the xml in doc
                        Doc.LoadXml(oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)

                        'Extarcting the Parent and Child Element
                        Dim v_sParent, v_sChild As String
                        v_sParent = Regex.Split(Me.ID.ToUpper(), "__")(0)
                        v_sChild = Regex.Split(Me.ID.ToUpper(), "__")(1)
                        'Change the control name by preffixing "SW." as SAM returns the xml node using this prefix
                        v_sChild = "SW." + v_sChild

                        If String.IsNullOrEmpty(sSWOI) Then
                            Dim oOI As Collections.Stack = Session(CNOI)
                            sOIForControl = oOI.Peek.ToString()
                        Else
                            sOIForControl = sSWOI
                        End If

                        'get the nodes in a list which has OI value = session(CNOI)
                        Dim oNodelist As XmlNodeList = Doc.SelectNodes("//" & v_sParent & "[@OI='" & sOIForControl & "']/*[@TYPE='SW-5']")
                        For Each oNode As XmlNode In oNodelist
                            v_sChild = Regex.Split(Me.ID.ToUpper(), "__")(1)
                            'Change the control name by preffixing "SW." as SAM returns the xml node using this prefix
                            v_sChild = "SW." + v_sChild
                            If Trim$(sPropertyName) <> Trim$(oNode.Name) AndAlso Trim$(v_sChild) = Trim$(oNode.Name) Then
                                sPropertyName = oNode.Name

                                'declare a node which will get parent node of standard wording
                                Dim ParentNode As XmlNode = Nothing
                                Dim oDocumentTemplates As NexusProvider.DocumentTemplateCollection = Nothing
                                ' check the value of standard wording property 
                                If Not String.IsNullOrEmpty(sPropertyName) Then
                                    'get the parent nodes related to SW property value.
                                    Dim oParentNodelist As XmlNodeList = Doc.SelectNodes("//" & v_sParent & "[@OI='" & sOIForControl & "']/" & sPropertyName)

                                    oDocumentTemplates = New NexusProvider.DocumentTemplateCollection
                                    Dim oNewDocumentTemplate As NexusProvider.DocumentTemplate
                                    'now process through all node and make the collection of standardwording nodes
                                    For Each oParentNode As XmlNode In oParentNodelist
                                        ''Make dtc collection
                                        oNewDocumentTemplate = New NexusProvider.DocumentTemplate()

                                        With oNewDocumentTemplate
                                            .Code = oParentNode.Attributes("CODE").Value
                                            .Description = oParentNode.Attributes("DESCRIPTION").Value
                                            .OriginalCode = oParentNode.Attributes("ORIGINALDOCUMENTCODE").Value

                                            If oParentNode.Attributes("ID") IsNot Nothing Then
                                                If Not (String.IsNullOrEmpty(Convert.ToString(oParentNode.Attributes("ID").Value))) Then
                                                    .DocumentTemplateId = Convert.ToInt32(oParentNode.Attributes("ID").Value)
                                                End If
                                            End If
                                            If Not (String.IsNullOrEmpty(.Code)) Then
                                                oDocumentTemplates.Add(oNewDocumentTemplate)
                                            End If

                                        End With

                                    Next
                                    dtc = oDocumentTemplates
                                End If

                            End If
                        Next

                    Else
                        'get the Policy level standard wording
                        dtc = oWebService.GetStandardPolicyWordings(oQuote.InsuranceFileKey)
                    End If
                End If
                If bSupportRiskLevel Then
                    'Risk Level standardwording session 
                    Session(sNewRiskStdWrdSession) = dtc
                Else
                    'policy Level standardwording session 
                    Session(CNPolicyStandardWordingsTemplate) = dtc
                    Session(CNFreshPolicySW) = 0
                End If
            Else
                If bSupportRiskLevel = False Then
                    'policy level
                    'check the session variable whose value will be set to 1 at time od addition of new quote
                    If Session(CNFreshPolicySW) IsNot Nothing Then
                        If CType(Session(CNFreshPolicySW), String) = "1" Then
                            'if fresh is called then pass an addition vale true 
                            dtc = oWebService.GetStandardPolicyWordings(oQuote.InsuranceFileKey, True)
                            'save the collection into session
                            Session(CNPolicyStandardWordingsTemplate) = dtc
                        Else
                            'else get the collection from session.
                            dtc = CType(Session(CNPolicyStandardWordingsTemplate), NexusProvider.DocumentTemplateCollection)
                        End If
                    Else
                        '' This will be excuted when transaction type is not NB
                        If CType(Session(CNPolicyStandardWordingsTemplate), NexusProvider.DocumentTemplateCollection) Is Nothing Then
                            'get the collection of standard wording and save it into session
                            dtc = oWebService.GetStandardPolicyWordings(oQuote.InsuranceFileKey)
                            Session(CNPolicyStandardWordingsTemplate) = dtc
                        Else
                            'else get the collection from session.
                            dtc = CType(Session(CNPolicyStandardWordingsTemplate), NexusProvider.DocumentTemplateCollection)
                        End If
                    End If

                Else
                    'if not nothing meand data is available in view stat
                    dtc = CType(Session(sNewRiskStdWrdSession), NexusProvider.DocumentTemplateCollection)
                End If
            End If

            'Set The selected Values in picklist
            SetSelectedValues()

            'Population of the grid 
            grdWordings.DataSource = dtc
            grdWordings.DataBind()

        End Sub

        ''' <summary>
        ''' Populate the picklist control with tempaltes that are available for selection
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub FillPickList()
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oDocumentTemplate As New NexusProvider.DocumentTemplate
            Dim oDocumentTemplateColl As NexusProvider.DocumentTemplateCollection = Nothing
            Dim oRiskType As NexusProvider.RiskType = Session(CNRiskType)
            'Initialize xml doc to get the risk xml from xml dataset
            Dim Doc As New XmlDocument

            'if oQuote object is not nothing then fire the sam call to find the available caluses 
            If oQuote IsNot Nothing Then
                oDocumentTemplate.EffectiveDate = oQuote.CoverEndDate
                If bSupportRiskLevel Then
                    'Risk level 
                    oDocumentTemplate.ProductCode = String.Empty
                    'Extarcting the Parent and Child Element

                    oDocumentTemplate.EffectiveDate = oQuote.CoverEndDate
                    'Load xml dataset of current risk into DOC element
                    Doc.LoadXml(oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)

                    Dim v_sParent, v_sChild As String
                    v_sParent = Regex.Split(Me.ID.ToUpper(), "__")(0)
                    v_sChild = Regex.Split(Me.ID.ToUpper(), "__")(1)
                    Dim oOI As Collections.Stack = Session(CNOI)
                    'get the nodes in a list which has OI value = session(CNOI)
                    Dim oNodelist As XmlNodeList = Doc.SelectNodes("//" & v_sParent & "[@OI='" & oOI.Peek.ToString() & "']/*[@TYPE='SW-5']")

                    For Each oNode As XmlNode In oNodelist
                        'set the property name with cirrent node name
                        oDocumentTemplate.PropertyName = oNode.Name.Substring(3)
                        oDocumentTemplate.ObjectName = Trim$(v_sParent)
                        Exit For
                    Next
                    'find the doc via sam as per the Level of SW 
                    oDocumentTemplateColl = oWebService.FindDocumentTemplates(oDocumentTemplate, oRiskType.RiskCode)
                Else
                    'policy level
                    oDocumentTemplate.ProductCode = oQuote.ProductCode
                End If
                oDocumentTemplate.EffectiveDate = oQuote.CoverStartDate
                'find the doc via sam as per the Level of SW 
                oDocumentTemplateColl = oWebService.FindDocumentTemplates(oDocumentTemplate, oRiskType.RiskCode)
                Dim sNewRiskStdWrdSession As String
                Dim sChild As String
                sChild = Regex.Split(Me.ID.ToUpper(), "__")(1)
                sChild = "SW." + sChild
                sNewRiskStdWrdSession = CNRiskStandardWordingsTemplate + sChild
                If Session(sNewRiskStdWrdSession) IsNot Nothing Then
                    Dim oTempDocumentTemplateColl As NexusProvider.DocumentTemplateCollection = Nothing
                    oTempDocumentTemplateColl = Session(sNewRiskStdWrdSession) 'ViewState("MyDocTemplates")               
                    If oTempDocumentTemplateColl IsNot Nothing Then
                        If oTempDocumentTemplateColl.Count > 0 Then
                            For iCount As Integer = 0 To oTempDocumentTemplateColl.Count - 1
                                If oTempDocumentTemplateColl(iCount).DocumentTemplateId < 0 Then
                                    oDocumentTemplateColl.Add(oTempDocumentTemplateColl(iCount))
                                End If
                            Next
                        End If
                    End If
                End If
            End If

            If oDocumentTemplateColl IsNot Nothing AndAlso oDocumentTemplateColl.Count > 0 Then
                'Population of the pick list based on the results
                PckTemplates.DataSource = oDocumentTemplateColl
                PckTemplates.DataTextField = "Description"
                PckTemplates.DataValueField = "DocumentTemplateId"
                PckTemplates.DataCodeField = "Code"
                PckTemplates.ToolTip = "Description"
                PckTemplates.DataBind()

                ViewState("FindDocTemplates") = oDocumentTemplateColl
            End If

            'Set The selected Values in picklist
            SetSelectedValues()
        End Sub

        Sub SetSelectedValues()

            'Re-Arrange the picklist control values as per the selected doc template
            'If doc template is selected then re-arrange the picklist collection as per the grid order
            Dim sNewRiskStdWrdSession As String
            Dim sChild As String
            sChild = Regex.Split(Me.ID.ToUpper(), "__")(1)
            sChild = "SW." + sChild
            sNewRiskStdWrdSession = CNRiskStandardWordingsTemplate + sChild
            If ViewState("FindDocTemplates") IsNot Nothing AndAlso (Session(sNewRiskStdWrdSession) IsNot Nothing Or Session(CNPolicyStandardWordingsTemplate) IsNot Nothing) Then      'Old Document template collection
                Dim oldDocTempColl As NexusProvider.DocumentTemplateCollection = Nothing
                oldDocTempColl = ViewState("FindDocTemplates")
                'Selected Document template collection
                Dim selectedDocTempColl As NexusProvider.DocumentTemplateCollection = Nothing

                If bSupportRiskLevel Then

                    selectedDocTempColl = Session(sNewRiskStdWrdSession)
                Else
                    selectedDocTempColl = Session(CNPolicyStandardWordingsTemplate)
                End If


                If oldDocTempColl IsNot Nothing AndAlso selectedDocTempColl IsNot Nothing Then
                    'New (Re-arranged)  Document template collection
                    Dim NewDocTempColl As New NexusProvider.DocumentTemplateCollection

                    'Reset the Pick list
                    Dim oTotalRows As Integer = PckTemplates.Items.Count
                    For iCount As Integer = 0 To oTotalRows - 1
                        Dim i As Integer = 0
                        PckTemplates.Items.RemoveAt(i) '.Selected = False
                    Next

                    'Addition of the selected doc template into the new template with new re-order
                    For iCount As Integer = 0 To selectedDocTempColl.Count - 1
                        NewDocTempColl.Add(selectedDocTempColl(iCount))
                    Next

                    For iCount As Integer = 0 To oldDocTempColl.Count - 1
                        Dim bFound As Boolean = False
                        For jCount As Integer = 0 To NewDocTempColl.Count - 1
                            If NewDocTempColl(jCount).Code.Trim.ToUpper = oldDocTempColl(iCount).Code.Trim.ToUpper Then
                                bFound = True
                                Exit For
                            End If
                        Next
                        If bFound = False Then
                            NewDocTempColl.Add(oldDocTempColl(iCount))
                        End If
                    Next
                    If NewDocTempColl.Count < oldDocTempColl.Count Then
                        If oldDocTempColl.Count - NewDocTempColl.Count > 0 Then
                            NewDocTempColl = oldDocTempColl
                        End If
                    End If
                    'Population of the pick list based on the results
                    PckTemplates.DataSource = NewDocTempColl
                    PckTemplates.DataTextField = "Description"
                    PckTemplates.DataValueField = "DocumentTemplateId"
                    PckTemplates.DataCodeField = "Code"
                    PckTemplates.ToolTip = "Description"
                    PckTemplates.DataBind()

                    ViewState("FindDocTemplates") = NewDocTempColl
                End If
            End If

            'Setting of the selected item into pick list, if ViewState("MyDocTemplates") is not nothing
            'means values are retreived from sam call
            If Session(sNewRiskStdWrdSession) IsNot Nothing Or Session(CNPolicyStandardWordingsTemplate) IsNot Nothing Then
                Dim oDocumentTemplateColl As NexusProvider.DocumentTemplateCollection = Nothing
                Dim sValues As New ArrayList
                If bSupportRiskLevel Then
                    oDocumentTemplateColl = Session(sNewRiskStdWrdSession) 'ViewState("MyDocTemplates")
                Else
                    oDocumentTemplateColl = Session(CNPolicyStandardWordingsTemplate)
                End If
                If oDocumentTemplateColl IsNot Nothing Then
                    If oDocumentTemplateColl.Count > 0 Then
                        For iCount As Integer = 0 To oDocumentTemplateColl.Count - 1
                            sValues.Add(oDocumentTemplateColl(iCount).DocumentTemplateId)
                        Next
                        'Passing the seled item into array to the pick list control
                        PckTemplates.SetSelectedValues(sValues.ToArray)
                    End If
                End If
            End If
        End Sub
        ''' <summary>
        ''' Public method to trigger the update via SAM of the selected document templates
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub SubmitSelections(Optional ByRef r_bSkipSave As Boolean = False, Optional ByVal sSWOI As String = "", Optional ByRef r_bMultiStdWrd As Boolean = False)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oDocumentTemplateColl As NexusProvider.DocumentTemplateCollection = Nothing
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
            Dim sNewRiskStdWrdSession As String
            Dim sChild As String
            Dim bIsTxTextControlEnabled As Boolean = oPortal.EnableTXTextControl
            sChild = Regex.Split(Me.ID.ToUpper(), "__")(1)
            sChild = "SW." + sChild
            sNewRiskStdWrdSession = CNRiskStandardWordingsTemplate + sChild
            If bSupportRiskLevel Then
                ' if SW is at risk level then get the collection from risk SW session
                oDocumentTemplateColl = Session(sNewRiskStdWrdSession)
            Else
                'At policy level update the collection with current session
                oDocumentTemplateColl = Session(CNPolicyStandardWordingsTemplate)
                If Session(CNFreshPolicySW) IsNot Nothing Then
                    'Update the below session once we have clicked on next button.
                    Session(CNFreshPolicySW) = 0
                End If
            End If
            'Update the Risk level XML with selected standard document template
            If bSupportRiskLevel = True Then

                Dim Doc As New XmlDocument
                Dim SWXMLDOC As New XmlDocument
                Dim sPropertyName As String = String.Empty
                Dim sParentOI As String
                'Load xml dataset of current risk into DOC element
                Doc.LoadXml(oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)

                'Extarcting the Parent and Child Element
                Dim v_sParent, v_sChild As String

                v_sParent = Regex.Split(Me.ID.ToUpper(), "__")(0)
                v_sChild = Regex.Split(Me.ID.ToUpper(), "__")(1)
                'Change the control name by preffixing "SW." as SAM returns the xml node using this prefix
                v_sChild = "SW." + v_sChild

                Dim oNodelist As XmlNodeList
                If Not String.IsNullOrEmpty(sSWOI) Then
                    sParentOI = sSWOI
                    oNodelist = Doc.SelectNodes("//" & v_sParent & "[@OI='" & sSWOI & "']/*[@TYPE='SW-5']")
                Else
                    Dim oOI As Collections.Stack = Session(CNOI)
                    sParentOI = oOI.Peek.ToString()
                End If
                'get the nodes which have standardwording element
                oNodelist = Doc.SelectNodes("//" & v_sParent & "[@OI='" & sParentOI & "']/*[@TYPE='SW-5']")

                'travel through each node to get the property name and update it in the database.
                For Each oNode As XmlNode In oNodelist
                    r_bMultiStdWrd = True
                    'check to verify that current cotrol is being hit and a property is access only once.
                    If Trim$(sPropertyName) <> Trim$(oNode.Name) AndAlso Trim$(v_sChild) = Trim$(oNode.Name) Then
                        sPropertyName = oNode.Name
                        'Deletion of all child
                        Dim ParentNode As XmlNode = Nothing
                        Dim oParentNodelist As XmlNodeList = Doc.SelectNodes("//" & v_sParent & "[@OI='" & sParentOI & "']/" & sPropertyName & "[@TYPE='SW-5']")
                        For Each oParentNode As XmlNode In oParentNodelist
                            If ParentNode Is Nothing Then
                                'get the parent node 
                                ParentNode = oParentNode.ParentNode
                            End If
                            'remove all the SW from current paraent node
                            ParentNode.RemoveChild(oParentNode)
                        Next
                        Dim sSWRow As String = sPropertyName
                        Dim SWRow As XmlElement
                        'Addition of Child Node
                        If oDocumentTemplateColl IsNot Nothing AndAlso oDocumentTemplateColl.Count > 0 Then
                            For iCount As Integer = 0 To oDocumentTemplateColl.Count - 1

                                If oDocumentTemplateColl(iCount).UpdateFilePath IsNot Nothing AndAlso String.IsNullOrEmpty(oDocumentTemplateColl(iCount).UpdateFilePath) = False Then
                                    Dim v_sTempFileLocation As String

                                    v_sTempFileLocation = oPortal.TempFileLocation
                                    'Check whether file is still open or not, if file is still open DO NOT Process it
                                    Dim sourceDir As DirectoryInfo = New DirectoryInfo(oDocumentTemplateColl(iCount).UpdateFilePath)
                                    Dim file As System.IO.FileInfo = Nothing
                                    Dim sFileName As String = Nothing
                                    Dim Completepath As String
                                    Dim sReturnString As String

                                    For Each file In sourceDir.GetFiles()
                                        If file.Extension = ".doc" Or file.Extension.ToUpper = ".DOCX" Or file.Extension.ToUpper = ".XML Then" Or file.Extension.ToUpper = ".PDF" Or file.Extension.ToUpper = ".HTML" Or file.Extension.ToUpper = ".HTM" Then
                                            'set sFileName to the name of the rpt file
                                            sFileName = file.Name
                                            Exit For
                                        End If
                                    Next

                                    ' proceed below when we are not in view mode
                                    If Session(CNMode) <> Mode.View Then
                                        Dim iExtStartPoint As Integer = 0
                                        'get the page name
                                        iExtStartPoint = InStr(Pagename, ".")
                                        'get the updated file location from the collection
                                        If Not (String.IsNullOrEmpty(oDocumentTemplateColl(iCount).ExistingDocumentPath)) Then
                                            Completepath = oDocumentTemplateColl(iCount).ExistingDocumentPath
                                        Else
                                            'If existing path is not found get the path from UpdateFile path
                                            Completepath = oDocumentTemplateColl(iCount).UpdateFilePath & "\" & sFileName
                                        End If
                                        If (file.Exists) Then
                                            Dim fsInputFile1 As IO.FileStream
                                            Try
                                                fsInputFile1 = New IO.FileStream(Completepath, FileMode.Open, FileAccess.Read)
                                                custVldIsDocumentAlreadyOpen.Enabled = False
                                                custVldIsDocumentAlreadyOpen.IsValid = True
                                                'fsInputFile1.Flush()
                                                fsInputFile1.Close()
                                                fsInputFile1.Dispose()
                                            Catch ex As System.Exception
                                                If fsInputFile1 IsNot Nothing AndAlso fsInputFile1.CanRead Then
                                                    'fsInputFile1.Flush()
                                                    fsInputFile1.Close()
                                                    fsInputFile1.Dispose()
                                                End If
                                                custVldIsDocumentAlreadyOpen.Enabled = True
                                                custVldIsDocumentAlreadyOpen.IsValid = False
                                                Exit Sub
                                            End Try
                                        End If
                                        ' change the ext. of file if document type is "xml" or word" so that this could be open for editing
                                        If oPortal.EditEndorsements.ToUpper = "HTML" Or oPortal.EditEndorsements.ToUpper = "HTM" Then
                                            'for other format type return the unammended location
                                            sReturnString = Completepath
                                        Else
                                            If bIsTxTextControlEnabled Then
                                                sReturnString = Completepath
                                            Else
                                                sReturnString = Completepath.Replace(".doc", ".xml")
                                            End If
                                            'copy the edited doc to updated file location.
                                            IO.File.Move(Completepath, sReturnString)
                                        End If

                                        'Initialize the documenttemplate
                                        Dim oDTemplate As NexusProvider.DocumentTemplate
                                        Dim sDocumentType As String
                                        'select the document type on the basis of configured values
                                        If oPortal.EditEndorsements.ToUpper.ToString.Trim = "HTML" Or oPortal.EditEndorsements.ToUpper.ToString.Trim = "HTM" Then
                                            sDocumentType = "HTML"
                                        ElseIf oPortal.EditEndorsements.ToUpper.ToString.Trim = "PDF" Then
                                            sDocumentType = "PDF"
                                        Else
                                            sDocumentType = "None"
                                        End If
                                        'update the doc template
                                        oDTemplate = oWebService.UpdateStandardWordingsTemplate(oDocumentTemplateColl(iCount).Code,
                                    oDocumentTemplateColl(iCount).DocumentTemplateId, oDocumentTemplateColl(iCount).UpdateFilePath, v_sTempFileLocation, DirectCast([Enum].Parse(GetType(NexusProvider.DocumentFormatType), sDocumentType), NexusProvider.DocumentFormatType),
                                    v_IsTxTextControlEnabled:=bIsTxTextControlEnabled)
                                        'get the new code, description and id
                                        oDocumentTemplateColl(iCount).Code = oDTemplate.Code
                                        'oDocumentTemplateColl(iCount).Description = oDTemplate.Description
                                        oDocumentTemplateColl(iCount).DocumentTemplateId = oDTemplate.DocumentTemplateId
                                        'get the temp file location from web config
                                        Dim sTempFileLocation As String = v_sTempFileLocation & "\"
                                        Dim strText As String
                                        'replace the edited doc to temp file location
                                        strText = Replace(oDocumentTemplateColl(iCount).UpdateFilePath, sTempFileLocation, "")
                                        Dim astrSplitItems As String() = Split(strText, "\")
                                        Dim sOldGUID As String = Nothing
                                        Dim intX As Integer
                                        For intX = 0 To UBound(astrSplitItems)
                                            If intX = 0 Then
                                                sOldGUID = astrSplitItems(intX)
                                                Exit For
                                            End If
                                        Next
                                        'apend the old guid to temp location
                                        sTempFileLocation = sTempFileLocation & sOldGUID
                                        If IO.Directory.Exists(sTempFileLocation) Then
                                            Try
                                                IO.Directory.Delete(sTempFileLocation, True)
                                            Catch ex As System.Exception
                                                custVldIsDocumentAlreadyOpen.Enabled = True
                                                custVldIsDocumentAlreadyOpen.IsValid = False
                                                Exit Sub
                                            End Try
                                        End If
                                        'empty the update file path in collection
                                        oDocumentTemplateColl(iCount).UpdateFilePath = String.Empty
                                    End If
                                End If
                                'set the new element containing the edited doc and set its US to 2
                                SWRow = Doc.CreateElement(sSWRow)
                                SWRow.SetAttribute("CODE", oDocumentTemplateColl(iCount).Code.Trim)
                                If oDocumentTemplateColl(iCount).Description IsNot Nothing Then
                                    SWRow.SetAttribute("DESCRIPTION", oDocumentTemplateColl(iCount).Description.Trim)
                                Else
                                    SWRow.SetAttribute("DESCRIPTION", "")
                                End If
                                SWRow.SetAttribute("TYPE", "SW-5")
                                SWRow.SetAttribute("US", "2")
                                SWRow.SetAttribute("ID", Convert.ToString(oDocumentTemplateColl(iCount).DocumentTemplateId))
                                'Add into Parent
                                ParentNode.AppendChild(SWRow)
                                If ParentNode.Attributes("US").Value = "0" Then ParentNode.Attributes("US").Value = "2"
                                'set back the collection into session
                                Session(sNewRiskStdWrdSession) = oDocumentTemplateColl

                            Next
                            'Write Data Back to XMLDataSet
                            Dim tempswContent As New System.IO.StringWriter
                            Dim tempxmlwContent As New XmlTextWriter(tempswContent)

                            Doc.WriteTo(tempxmlwContent)
                            'get the updated xml into quote collection
                            oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset = Doc.OuterXml.ToString
                            tempxmlwContent.Close()
                            tempswContent.Close()
                            'update  the quote session
                            Session(CNQuote) = oQuote
                        Else
                            ' this will be executed in the case of deletion of SW
                            SWRow = Doc.CreateElement(sSWRow)
                            SWRow.SetAttribute("CODE", "")
                            SWRow.SetAttribute("ORIGINALDOCUMENTCODE", "")
                            SWRow.SetAttribute("DESCRIPTION", "")
                            SWRow.SetAttribute("TYPE", "SW-5")
                            SWRow.SetAttribute("US", "3")
                            SWRow.SetAttribute("ID", "")
                            'Add into Parent
                            If ParentNode.Attributes("US").Value = "0" Then ParentNode.Attributes("US").Value = "2"
                            ParentNode.AppendChild(SWRow)
                            Session(sNewRiskStdWrdSession) = oDocumentTemplateColl
                            oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset = Doc.OuterXml.ToString
                        End If
                    End If
                Next
                'set this value to skip the updation of quote xml dataset from main dataset. this is done to save the sw in xml dataset
                r_bSkipSave = True
                'Session(sNewRiskStdWrdSession) = Nothing

            Else

                If oDocumentTemplateColl IsNot Nothing AndAlso oDocumentTemplateColl.Count > 0 Then

                    'Clear all the previous values from oQuote
                    If oQuote.StandardPolicyWordings IsNot Nothing AndAlso oQuote.StandardPolicyWordings.Count > 0 Then
                        Dim oTotalRows As Integer = oQuote.StandardPolicyWordings.Count
                        For iCount As Integer = 0 To oTotalRows - 1
                            Dim i As Integer = 0
                            oQuote.StandardPolicyWordings.RemoveAt(i)
                        Next
                    End If

                    'Putting back the selected values into oQuote object
                    Dim oSW As NexusProvider.StandardWording
                    Dim oDocTemplate As NexusProvider.DocumentTemplate
                    For iCount As Integer = 0 To oDocumentTemplateColl.Count - 1
                        'Add functionality to this method so that if the temp doc location is set for a given template,
                        ' then the update SAM method is called, zipping up whatever is in the “UpdateFilePath” location and 
                        'passing it back to the (new) update SAM method. 
                        If String.IsNullOrEmpty(oDocumentTemplateColl(iCount).UpdateFilePath) = False Then
                            Try
                                oSW = New NexusProvider.StandardWording

                                'The new SAM method will return the updated doc template code, which will need to be used to 
                                'set the relevant property of the doc template before we call UpdateStandardPolicyWordings
                                Dim v_sTempFileLocation As String


                                v_sTempFileLocation = oPortal.TempFileLocation

                                'Check whether file is still open or not, if file is still open DO NOT Process it
                                Dim sourceDir As DirectoryInfo = New DirectoryInfo(oDocumentTemplateColl(iCount).UpdateFilePath)
                                Dim file As System.IO.FileInfo = Nothing
                                Dim sFileName As String = Nothing
                                Dim Completepath As String
                                Dim sReturnString As String

                                For Each file In sourceDir.GetFiles()
                                    If file.Extension = ".doc" Or file.Extension.ToUpper = ".DOCX" Or file.Extension.ToUpper = ".XML" Or file.Extension.ToUpper = ".PDF" Or file.Extension.ToUpper = ".HTML" Or file.Extension.ToUpper = ".HTM" Then
                                        'set sFileName to the name of the rpt file
                                        sFileName = file.Name
                                        Exit For
                                    End If
                                Next

                                If Session(CNMode) <> Mode.View Then
                                    'get the updated file location from the collection
                                    If Not (String.IsNullOrEmpty(oDocumentTemplateColl(iCount).ExistingDocumentPath)) Then
                                        Completepath = oDocumentTemplateColl(iCount).ExistingDocumentPath
                                    Else

                                        'If exiting path is not found get the path from UpdateFile path
                                        Completepath = oDocumentTemplateColl(iCount).UpdateFilePath & "\" & sFileName
                                    End If
                                    If (file.Exists) Then
                                        Dim fsInputFile1 As IO.FileStream
                                        Try
                                            fsInputFile1 = New IO.FileStream(Completepath, FileMode.Open, FileAccess.Read)
                                            custVldIsDocumentAlreadyOpen.Enabled = False
                                            custVldIsDocumentAlreadyOpen.IsValid = True
                                            fsInputFile1.Close()
                                            fsInputFile1.Dispose()
                                        Catch ex As Exception
                                            If fsInputFile1 IsNot Nothing AndAlso fsInputFile1.CanRead Then
                                                fsInputFile1.Close()
                                                fsInputFile1.Dispose()
                                            End If
                                            custVldIsDocumentAlreadyOpen.Enabled = True
                                            custVldIsDocumentAlreadyOpen.IsValid = False
                                            Exit Sub
                                        End Try
                                    End If
                                    ' change the ext. of file if document type is "xml" or word" so that this could be open for editing
                                    If oPortal.EditEndorsements.ToUpper = "HTML" Or oPortal.EditEndorsements.ToUpper = "HTM" Then
                                        'for other format type return the unammended location
                                        sReturnString = Completepath
                                    Else
                                        If bIsTxTextControlEnabled Then
                                            sReturnString = Completepath
                                        Else
                                            sReturnString = Completepath.Replace(".doc", ".xml")
                                        End If

                                        'copy the edited doc to updated file location.
                                        IO.File.Move(Completepath, sReturnString)
                                    End If

                                    'select the document type on the basis of configured values
                                    Dim sDocumentType As String
                                    If oPortal.EditEndorsements.ToUpper.ToString.Trim = "HTML" Or oPortal.EditEndorsements.ToUpper.ToString.Trim = "HTM" Then
                                        sDocumentType = "HTML"
                                    ElseIf oPortal.EditEndorsements.ToUpper.ToString.Trim = "PDF" Then
                                        sDocumentType = "PDF"
                                    Else
                                        sDocumentType = "None"
                                    End If
                                    oDocTemplate = oWebService.UpdateStandardWordingsTemplate(oDocumentTemplateColl(iCount).Code,
                                    oDocumentTemplateColl(iCount).DocumentTemplateId, oDocumentTemplateColl(iCount).UpdateFilePath, v_sTempFileLocation, DirectCast([Enum].Parse(GetType(NexusProvider.DocumentFormatType), sDocumentType), NexusProvider.DocumentFormatType),
                                    v_IsTxTextControlEnabled:=bIsTxTextControlEnabled)

                                    oSW.StandardPolicyWordingCode = oDocTemplate.Code
                                    oSW.StandardPolicyWordingID = oDocTemplate.DocumentTemplateId
                                    'oSW.StandardPolicyDiscription = oDocTemplate.Description
                                    oQuote.StandardPolicyWordings.Add(oSW)

                                    'Once the template has been returned successfully delete the entire UpdateFilePath to clean up 
                                    'and also reset the property from the document template object.

                                    Dim sTempFileLocation As String = v_sTempFileLocation & "\"
                                    Dim strText As String
                                    strText = Replace(oDocumentTemplateColl(iCount).UpdateFilePath, sTempFileLocation, "")
                                    Dim astrSplitItems As String() = Split(strText, "\")
                                    Dim sOldGUID As String = Nothing
                                    Dim intX As Integer
                                    For intX = 0 To UBound(astrSplitItems)
                                        If intX = 0 Then
                                            sOldGUID = astrSplitItems(intX)
                                            Exit For
                                        End If
                                    Next

                                    sTempFileLocation = sTempFileLocation & sOldGUID
                                    If IO.Directory.Exists(sTempFileLocation) Then
                                        IO.Directory.Delete(sTempFileLocation, True)
                                    End If
                                    oDocumentTemplateColl(iCount).UpdateFilePath = String.Empty
                                End If
                                Session(CNPolicyStandardWordingsTemplate) = oDocumentTemplateColl
                            Catch
                                oDocumentTemplateColl(iCount).UpdateFilePath = String.Empty
                                Session(CNPolicyStandardWordingsTemplate) = oDocumentTemplateColl
                                oSW = New NexusProvider.StandardWording
                                oSW.StandardPolicyWordingCode = oDocumentTemplateColl(iCount).Code
                                oQuote.StandardPolicyWordings.Add(oSW)

                            End Try
                        Else
                            oSW = New NexusProvider.StandardWording
                            oSW.StandardPolicyWordingCode = oDocumentTemplateColl(iCount).Code
                            oQuote.StandardPolicyWordings.Add(oSW)
                        End If
                    Next
                    If Session(CNMode) <> Mode.View Then
                        'sam call  to update the selected wordings in BO
                        oWebService.UpdateStandardPolicyWordings(oQuote)
                    End If
                    'get the update SW
                    Dim dtc As NexusProvider.DocumentTemplateCollection
                    dtc = oWebService.GetStandardPolicyWordings(oQuote.InsuranceFileKey)

                    Session(CNPolicyStandardWordingsTemplate) = dtc
                ElseIf oDocumentTemplateColl IsNot Nothing AndAlso oDocumentTemplateColl.Count = 0 Then
                    'Clear all the previous values from oQuote
                    If oQuote.StandardPolicyWordings IsNot Nothing Then
                        Dim oTotalRows As Integer = oQuote.StandardPolicyWordings.Count
                        For iCount As Integer = 0 To oTotalRows - 1
                            Dim i As Integer = 0
                            oQuote.StandardPolicyWordings.RemoveAt(i)
                        Next
                        'update the selected wordings in BO
                        oWebService.UpdateStandardPolicyWordings(oQuote)

                        Dim dtc As NexusProvider.DocumentTemplateCollection
                        dtc = oWebService.GetStandardPolicyWordings(oQuote.InsuranceFileKey)
                        Session(CNEditStandardWordingsTemplate) = dtc
                    End If
                Else
                    'when we delete the policy standard wording then below code will ge executed
                    oQuote.StandardPolicyWordings.Clear()
                    oWebService.UpdateStandardPolicyWordings(oQuote)
                End If
            End If

        End Sub

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            If Not Page.IsPostBack Then
                Dim sChild As String
                Dim sNewRiskStdWrdSession As String
                sChild = Regex.Split(Me.ID.ToUpper(), "__")(1)
                sChild = "SW." + sChild
                sNewRiskStdWrdSession = CNRiskStandardWordingsTemplate + sChild
                Session(sNewRiskStdWrdSession) = Nothing

            End If
            If Session(CNMode) = Mode.View Or Session(CNRiskMode) = RiskMode.View Or Session(CNMode) = Mode.Review Then
                hdSelect.Value = "VIEW"
            End If
        End Sub

        ''' <summary>
        ''' Handles page load event. Calls methods to populate grid and picklist on initial load
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Response.Clear()
            'get the product risk option enable or disable the standard wording edit link
            bAllowEdit = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.AllowStandardWordingEdit, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)

            If Not IsPostBack Then
                'populate the gridview and picklist accordingly
                If bSetDefault = False Then
                    FillGrid()
                End If
                FillPickList()
            End If
            If Session(CNMode) = Mode.View Then
                '_btnShowSelect.Visible = False

            End If
        End Sub
        ''' <summary>
        ''' this will set the visibility of Edit link
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdWordings_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdWordings.DataBound

            'Show the Edit link if task is available


            If UserCanDoTask("EditEndorsments") = False Then
                grdWordings.Columns(4).Visible = False
            ElseIf (bAllowEdit = True AndAlso UserCanDoTask("EditEndorsments") = True AndAlso bAllowEditInControl = True) Or (Session(CNMode) = Mode.View) Then
                grdWordings.Columns(4).Visible = True
            Else
                grdWordings.Columns(4).Visible = False
            End If

            If UserCanDoTask("ViewEndorsments") = True Then
                grdWordings.Columns(5).Visible = True
            Else
                grdWordings.Columns(5).Visible = False
            End If

            If (Session(CNMode) = Mode.View Or Session(CNMode) = Mode.Review) Then
                grdWordings.Columns(4).Visible = False
                grdWordings.Columns(5).Visible = True
                grdWordings.Columns(7).Visible = False

            End If

            If UserCanDoTask("ReorderEndorsments") = False Then
                grdWordings.Columns(2).Visible = False
                grdWordings.Columns(3).Visible = False
            ElseIf (bAllowReorder AndAlso UserCanDoTask("ReorderEndorsments") = True And Session(CNMode) <> Mode.View And Session(CNMode) <> Mode.Review) Then
                grdWordings.Columns(2).Visible = True
                grdWordings.Columns(3).Visible = True
            Else
                grdWordings.Columns(2).Visible = False
                grdWordings.Columns(3).Visible = False
            End If
            If UserCanDoTask("PreviewEndorsments") = False Then
                grdWordings.Columns(6).Visible = False
            ElseIf (bAllowPreview AndAlso UserCanDoTask("PreviewEndorsments") = True And Session(CNMode) <> Mode.View And Session(CNMode) <> Mode.Review) Then
                grdWordings.Columns(6).Visible = True
            Else
                grdWordings.Columns(6).Visible = False
            End If

            If Session(CNMode) <> Mode.View Then
                If UserCanDoTask("DeleteEndorsments") = False Then
                    grdWordings.Columns(7).Visible = False
                ElseIf (bAllowDelete AndAlso UserCanDoTask("DeleteEndorsments") = True) Then
                    grdWordings.Columns(7).Visible = True
                Else
                    grdWordings.Columns(7).Visible = False
                End If

            End If

        End Sub
        ''' <summary>
        ''' Handles commands to move a given template either up or down in the grid
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdWordings_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdWordings.RowCommand
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())

            'first get the current row index, this will be the same as the index in the document template collection
            Dim iSelectedIndex As Integer = CType(CType(e.CommandSource, LinkButton).NamingContainer, GridViewRow).RowIndex
            'get the template collection from viewstate from selected values
            Dim dtc As NexusProvider.DocumentTemplateCollection
            Dim sNewRiskStdWrdSession As String
            Dim sChild As String
            Dim bIsTxTextControlEnabled As Boolean = oPortal.EnableTXTextControl

            sChild = Regex.Split(Me.ID.ToUpper(), "__")(1)
            sChild = "SW." + sChild
            sNewRiskStdWrdSession = CNRiskStandardWordingsTemplate + sChild
            If bSupportRiskLevel Then
                dtc = CType(Session(sNewRiskStdWrdSession), NexusProvider.DocumentTemplateCollection)
            Else
                dtc = CType(Session(CNPolicyStandardWordingsTemplate), NexusProvider.DocumentTemplateCollection)
            End If
            'store the selected template
            Dim dt As NexusProvider.DocumentTemplate = dtc.Item(iSelectedIndex)
            Select Case e.CommandName
                Case "MoveUp"
                    'replace the current template with the one above (one less for index)
                    dtc.Item(iSelectedIndex) = dtc.Item(iSelectedIndex - 1)
                    'then place the current template into the position above
                    dtc.Item(iSelectedIndex - 1) = dt
                    'add the modifed collection back into viewstate and then rebind the grid
                    'ViewState("MyDocTemplates") = dtc
                    If bSupportRiskLevel Then
                        Session(sNewRiskStdWrdSession) = dtc
                    Else
                        Session(CNPolicyStandardWordingsTemplate) = dtc
                        Session(CNFreshPolicySW) = 0
                    End If
                    FillGrid()
                Case "MoveDown"
                    'replace the current template with the one below (one greate for index)
                    dtc.Item(iSelectedIndex) = dtc.Item(iSelectedIndex + 1)
                    'then place the current template into the position below
                    dtc.Item(iSelectedIndex + 1) = dt
                    If bSupportRiskLevel Then
                        Session(sNewRiskStdWrdSession) = dtc
                    Else
                        Session(CNPolicyStandardWordingsTemplate) = dtc
                        Session(CNFreshPolicySW) = 0
                    End If
                    FillGrid()
                Case "EditEndorsement", "View"
                    If dtc.Item(iSelectedIndex).UpdateFilePath Is Nothing Then
                        'We need to record that this template is being edited, and location of temp doc so add a new property
                        ' “UpdateFilePath” on document template  for location of temp doc (relative to the “TempFileLocation”).
                        ' This will be the guid generated above
                        'Store the document in a unique folder inside the TempFileLocation, do this by creating a new guid.
                        ' i.e if the TempFileLocation=”c:\tempdocs”, and we create a new guid of xxxaaasss 
                        '(ok, so that’s not a guid, but you get the idea!) then the zip file would be unzipped to “c:\tempdocs\ xxxaaasss”
                        Dim NewGUID1 As Guid
                        NewGUID1 = Guid.NewGuid()
                        Dim sDocumentType As String
                        'set the document type as the mode.
                        If Session(CNMode) <> Mode.View Then
                            If oPortal.EditEndorsements.ToUpper.ToString.Trim = "HTML" Or oPortal.EditEndorsements.ToUpper.ToString.Trim = "HTM" Then
                                sDocumentType = "HTML"
                            ElseIf oPortal.EditEndorsements.ToUpper.ToString.Trim = "PDF" Then
                                sDocumentType = "PDF"
                            Else ' default is None
                                sDocumentType = "None"
                            End If
                        Else ' View mode
                            If oPortal.ViewEndorsements.ToUpper.ToString.Trim = "HTML" Or oPortal.ViewEndorsements.ToUpper.ToString.Trim = "HTM" Then
                                sDocumentType = "HTML"
                            Else
                                'default is PDF
                                sDocumentType = "PDF"
                            End If
                        End If
                        dtc.Item(iSelectedIndex).UpdateFilePath = oPortal.TempFileLocation & "\" & NewGUID1.ToString
                        'Call new SAM method to edit endorsement, this will return zipped word doc
                        'Unzip word doc returned by SAM. Doc will be stored at a file share 
                        'location, so when the user edits and saves it will be saved back to this same location
                        If Session(CNMode) <> Mode.View Then
                            dtc.Item(iSelectedIndex).UpdateFilePath = oWebService.GetStandardWordingsTemplate(dtc.Item(iSelectedIndex).Code,
                                dtc.Item(iSelectedIndex).DocumentTemplateId, dtc.Item(iSelectedIndex).UpdateFilePath, DirectCast([Enum].Parse(GetType(NexusProvider.DocumentFormatType), sDocumentType), NexusProvider.DocumentFormatType),
                                v_IsTXTextControlEnabled:=bIsTxTextControlEnabled)
                            Session(CNEditStandardWordingsTemplate) = dtc
                        Else
                            dtc.Item(iSelectedIndex).UpdateFilePath = oWebService.GetStandardWordingsTemplate(dtc.Item(iSelectedIndex).Code,
                           dtc.Item(iSelectedIndex).DocumentTemplateId, dtc.Item(iSelectedIndex).UpdateFilePath, DirectCast([Enum].Parse(GetType(NexusProvider.DocumentFormatType), sDocumentType), NexusProvider.DocumentFormatType),
                           v_IsTXTextControlEnabled:=False)
                        End If
                    End If
                    'We need to record that this template is being edited, and location of temp doc so add a new property
                    ' “UpdateFilePath” on document template  for location of temp doc (relative to the “TempFileLocation”).
                    ' This will be the guid generated above
                    'Store the document in a unique folder inside the TempFileLocation, do this by creating a new guid.
                    ' i.e if the TempFileLocation=”c:\tempdocs”, and we create a new guid of xxxaaasss 
                    '(ok, so that’s not a guid, but you get the idea!) then the zip file would be unzipped to “c:\tempdocs\ xxxaaasss”

                    'Check if document exists in local disc
                    Dim sExistingDocumentPath As String = String.Empty
                    If Not (String.IsNullOrEmpty(dtc.Item(iSelectedIndex).ExistingDocumentPath)) Then
                        sExistingDocumentPath = dtc.Item(iSelectedIndex).ExistingDocumentPath
                    Else
                        If Not (File.Exists(dtc.Item(iSelectedIndex).ExistingDocumentPath)) Then
                            Dim NewGUID As Guid
                            NewGUID = Guid.NewGuid()
                            dtc.Item(iSelectedIndex).UpdateFilePath = oPortal.TempFileLocation & "\" & NewGUID.ToString
                            'Call new SAM method to edit endorsement, this will return zipped word doc
                            'Unzip word doc returned by SAM. Doc will be stored at a file share 
                            'location, so when the user edits and saves it will be saved back to this same location
                            Dim sDocumentType As String
                            'set the document type as the mode.
                            If Session(CNMode) <> Mode.View Then
                                If oPortal.EditEndorsements.ToUpper.ToString.Trim = "HTML" Or oPortal.EditEndorsements.ToUpper.ToString.Trim = "HTM" Then
                                    sDocumentType = "HTML"
                                ElseIf oPortal.EditEndorsements.ToUpper.ToString.Trim = "PDF" Then
                                    sDocumentType = "PDF"
                                Else ' default is None
                                    sDocumentType = "None"
                                End If
                            Else ' View mode
                                If oPortal.ViewEndorsements.ToUpper.ToString.Trim = "HTML" Or oPortal.ViewEndorsements.ToUpper.ToString.Trim = "HTM" Then
                                    sDocumentType = "HTML"
                                Else
                                    'default is PDF
                                    sDocumentType = "PDF"
                                End If
                            End If
                            If Session(CNMode) <> Mode.View Then
                                dtc.Item(iSelectedIndex).UpdateFilePath = oWebService.GetStandardWordingsTemplate(dtc.Item(iSelectedIndex).Code,
                                dtc.Item(iSelectedIndex).DocumentTemplateId, dtc.Item(iSelectedIndex).UpdateFilePath, DirectCast([Enum].Parse(GetType(NexusProvider.DocumentFormatType), sDocumentType), NexusProvider.DocumentFormatType),
                                v_IsTXTextControlEnabled:=bIsTxTextControlEnabled)
                            Else
                                dtc.Item(iSelectedIndex).UpdateFilePath = oWebService.GetStandardWordingsTemplate(dtc.Item(iSelectedIndex).Code,
                           dtc.Item(iSelectedIndex).DocumentTemplateId, dtc.Item(iSelectedIndex).UpdateFilePath, DirectCast([Enum].Parse(GetType(NexusProvider.DocumentFormatType), sDocumentType), NexusProvider.DocumentFormatType),
                           v_IsTXTextControlEnabled:=False)
                            End If

                            Dim strComleteFileURL As String = dtc.Item(iSelectedIndex).UpdateFilePath
                            Dim astrSplitItems As String() = Split(strComleteFileURL, "\")
                            ' Split FolderPath & Pagename.
                            Dim intX As Integer
                            For intX = 0 To UBound(astrSplitItems)
                                If intX = UBound(astrSplitItems) Then
                                    'Find file name of word doc file returned by SAM  
                                    Pagename = astrSplitItems(intX)
                                Else
                                    'Find Full FolderPath of word doc file returned by SAM
                                    If intX <> 0 Then
                                        FolderPath = FolderPath & "\" & astrSplitItems(intX)
                                    Else
                                        FolderPath = astrSplitItems(intX)
                                    End If
                                End If
                            Next
                            'UpdateFilePath store only FolderPath
                            dtc.Item(iSelectedIndex).UpdateFilePath = FolderPath
                            If bSupportRiskLevel Then
                                'At risk level update the session value with current doc template collection 
                                Session(sNewRiskStdWrdSession) = dtc
                            Else
                                'At policy level update the session value with current doc template collection 
                                Session(CNPolicyStandardWordingsTemplate) = dtc
                                'set the session value to 0 once we edited the selected document
                                Session(CNFreshPolicySW) = 0
                            End If
                            'set the exiting path with the UpdateFilePath
                            dtc.Item(iSelectedIndex).ExistingDocumentPath = dtc.Item(iSelectedIndex).UpdateFilePath & "/" & Pagename
                            'set the above path in below parameter
                            sExistingDocumentPath = dtc.Item(iSelectedIndex).UpdateFilePath & "/" & Pagename
                            FillGrid()
                            If e.CommandName = "View" Then
                                dtc.Item(iSelectedIndex).UpdateFilePath = String.Empty
                            End If
                        End If
                    End If
                    Dim sUrl As String
                    Dim sViewDocumentPath As String = String.Empty

                    If Session(CNMode) <> Mode.View Then
                        'if the editing is configured as "HTML" then open the document in html editor
                        If (oPortal.EditEndorsements.ToUpper = "HTML" Or oPortal.EditEndorsements.ToUpper = "HTM") Then
                            If e.CommandName = "View" Then
                                If bIsTxTextControlEnabled Then
                                    sViewDocumentPath = CopyDocumentForView(sExistingDocumentPath)
                                    OpenDocumentInTxTextControl("View", sViewDocumentPath, sUrl)
                                Else
                                    If HttpContext.Current.Session.IsCookieless Then
                                        sUrl = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/EditDocument.aspx?DocPath=" & Server.UrlEncode(sExistingDocumentPath) & "&Pagename=" & Pagename & "&bSupportRiskLevel=" & bSupportRiskLevel.ToString() & "&Mode=View" & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=700"
                                    Else
                                        sUrl = AppSettings("WebRoot") & "/Modal/EditDocument.aspx?DocPath=" & Server.UrlEncode(sExistingDocumentPath) & "&Pagename=" & Pagename & "&bSupportRiskLevel=" & bSupportRiskLevel.ToString() & "&Mode=View" & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=700"
                                    End If
                                End If

                                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show",
                                  "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sUrl & "' , null);});</script>")
                                
                            Else
                                If bIsTxTextControlEnabled Then
                                    OpenDocumentInTxTextControl("Edit", sExistingDocumentPath, sUrl)
                                Else
                                    If HttpContext.Current.Session.IsCookieless Then
                                        sUrl = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/EditDocument.aspx?DocPath=" & Server.UrlEncode(sExistingDocumentPath) & "&Pagename=" & Pagename & "&bSupportRiskLevel=" & bSupportRiskLevel.ToString() & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=700"
                                    Else
                                        sUrl = AppSettings("WebRoot") & "/Modal/EditDocument.aspx?DocPath=" & Server.UrlEncode(sExistingDocumentPath) & "&Pagename=" & Pagename & "&bSupportRiskLevel=" & bSupportRiskLevel.ToString() & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=700"
                                    End If
                                End If
                                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show",
                                  "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sUrl & "' , null);});</script>")
                            End If
                        Else
                            If e.CommandName = "View" Then
                                 If bIsTxTextControlEnabled Then
                                    sViewDocumentPath = CopyDocumentForView(sExistingDocumentPath)
                                    OpenDocumentInTxTextControl("View", sViewDocumentPath, sUrl)
                                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show",
                                   "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sUrl & "' , null);});</script>")
                                 Else

                                Dim sDocPath As String = (sExistingDocumentPath.Replace("\\", "\").Replace("/", "\")).Replace("\", "\\")

                                'Open word doc file returned by SAM
                                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "editpath",
                                    "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){OpenDoc('file:///" & sDocPath & "')});</script>")
                                 End If
                            
                            Else
                                'if the editing is configured not as "HTML" then open the document in doc format
                                If bIsTxTextControlEnabled Then
                                    OpenDocumentInTxTextControl("Edit", sExistingDocumentPath, sUrl)
                                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show",
                                 "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sUrl & "' , null);});</script>")
                                Else
                                    Dim sDocPath As String = (sExistingDocumentPath.Replace("\\", "\").Replace("/", "\")).Replace("\", "\\")

                                    'Open word doc file returned by SAM
                                    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "editpath",
                                    "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){OpenDoc('file:///" & sDocPath & "')});</script>")
                                End If
                            End If
                        End If
                    Else
                        'This is the only postback during view mode from risk page. BaseRisk is not loading controls again in view mode.
                        'So loading controls from XML again
                        Dim oOI As System.Collections.Stack = Session(CNOI)
                        Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
                        Dim oMaster As ContentPlaceHolder = CType(GetMasterPlaceHolder(Me.Parent.Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder)

                        If oOI.Count > 0 Then
                            Nexus.DataSetFunctions.ReadContainerFromXML(oMaster, oOI.Peek, Me.Parent.Page)
                        Else
                            Nexus.DataSetFunctions.ReadContainerFromXML(oMaster, String.Empty, Me.Parent.Page, True, True)
                        End If

                        'if the editing is configured as "HTML" then open the document in html editor in view mode
                        If oPortal.ViewEndorsements.ToUpper = "HTML" Or oPortal.ViewEndorsements.ToUpper = "HTM" Then

                            If HttpContext.Current.Session.IsCookieless Then
                                sUrl = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/EditDocument.aspx?DocPath=" & Server.UrlEncode(sExistingDocumentPath) & "&Pagename=" & Pagename & "&bSupportRiskLevel=" & bSupportRiskLevel.ToString() & "&Mode=View" & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=700"
                            Else
                                sUrl = AppSettings("WebRoot") & "/Modal/EditDocument.aspx?DocPath=" & Server.UrlEncode(sExistingDocumentPath) & "&Pagename=" & Pagename & "&bSupportRiskLevel=" & bSupportRiskLevel.ToString() & "&Mode=View" & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=700"
                            End If

                            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show",
                            "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sUrl & "' , null);});</script>")
                        Else

                            'if the editing is configured not as "HTML" then open the document in doc format
                            Dim sDocPath As String = (sExistingDocumentPath.Replace("\\", "\").Replace("/", "\")).Replace("\", "\\")
                            'Open word doc file returned by SAM
                            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "editpath",
                                "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){OpenDoc('file:///" & sDocPath & "')});</script>")

                        End If
                    End If

                Case "Preview"

                    Dim sUrl As String
                    'We need to record that this template is being edited, and location of temp doc so add a new property
                    ' “UpdateFilePath” on document template  for location of temp doc (relative to the “TempFileLocation”).
                    ' This will be the guid generated above
                    'Store the document in a unique folder inside the TempFileLocation, do this by creating a new guid.
                    ' i.e if the TempFileLocation=”c:\tempdocs”, and we create a new guid of xxxaaasss 
                    '(ok, so that’s not a guid, but you get the idea!) then the zip file would be unzipped to “c:\tempdocs\ xxxaaasss”
                    If dtc.Item(iSelectedIndex).FileURL Is Nothing AndAlso String.IsNullOrEmpty(dtc.Item(iSelectedIndex).FileURL) Then

                        Dim NewGUID As Guid
                        NewGUID = Guid.NewGuid()
                        dtc.Item(iSelectedIndex).FileURL = oPortal.TempFileLocation & "\" & NewGUID.ToString
                        'Call new SAM method to edit endorsement, this will return zipped word doc
                        'Unzip word doc returned by SAM. Doc will be stored at a file share 
                        'location, so when the user edits and saves it will be saved back to this same location
                        Dim sDocumentType As String
                        If oPortal.ViewEndorsements.ToUpper.ToString.Trim = "HTML" Or oPortal.ViewEndorsements.ToUpper.ToString.Trim = "HTM" Then
                            sDocumentType = "HTML"
                        Else
                            'default is PDF
                            sDocumentType = "PDF"
                        End If

                        dtc.Item(iSelectedIndex).FileURL = oWebService.GetStandardWordingsTemplate(dtc.Item(iSelectedIndex).Code, dtc.Item(iSelectedIndex).DocumentTemplateId, dtc.Item(iSelectedIndex).FileURL, DirectCast([Enum].Parse(GetType(NexusProvider.DocumentFormatType), sDocumentType), NexusProvider.DocumentFormatType))

                        Dim strComleteFileURL As String = dtc.Item(iSelectedIndex).FileURL
                        Dim astrSplitItems As String() = Split(strComleteFileURL, "\")
                        ' Split FolderPath & Pagename.
                        Dim intX As Integer
                        For intX = 0 To UBound(astrSplitItems)
                            If intX = UBound(astrSplitItems) Then
                                'Find file name of word doc file returned by SAM  
                                Pagename = astrSplitItems(intX)
                            Else
                                'Find Full FolderPath of word doc file returned by SAM
                                If intX <> 0 Then
                                    FolderPath = FolderPath & "\" & astrSplitItems(intX)
                                Else
                                    FolderPath = astrSplitItems(intX)
                                End If
                            End If
                        Next
                        'if the viewing is configured as "HTML" then open the document in html editor in view mode else pdf
                        If HttpContext.Current.Session.IsCookieless Then
                            sUrl = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/EditDocument.aspx?DocPath=" & Server.UrlEncode(dtc.Item(iSelectedIndex).FileURL) & "&Pagename=" & Pagename & "&bSupportRiskLevel=" & bSupportRiskLevel.ToString() & "&Mode=View" & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=700"
                        Else
                            sUrl = AppSettings("WebRoot") & "/Modal/EditDocument.aspx?DocPath=" & Server.UrlEncode(dtc.Item(iSelectedIndex).FileURL) & "&Pagename=" & Pagename & "&bSupportRiskLevel=" & bSupportRiskLevel.ToString() & "&Mode=View" & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=700"
                        End If
                        Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show",
                        "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sUrl & "' , null);});</script>")
                    Else

                        'This is the only postback during view mode from risk page. BaseRisk is not loading controls again in view mode.
                        'So loading controls from XML again
                        Dim oOI As System.Collections.Stack = Session(CNOI)
                        Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
                        Dim oMaster As ContentPlaceHolder = CType(GetMasterPlaceHolder(Me.Parent.Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder)

                        If oOI.Count > 0 Then
                            Nexus.DataSetFunctions.ReadContainerFromXML(oMaster, oOI.Peek, Me.Parent.Page)
                        Else
                            Nexus.DataSetFunctions.ReadContainerFromXML(oMaster, String.Empty, Me.Parent.Page, True, True)
                        End If
                        'if the viewing is configured as "HTML" then open the document in html editor in view mode else pdf
                        If HttpContext.Current.Session.IsCookieless Then
                            sUrl = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/EditDocument.aspx?DocPath=" & Server.UrlEncode(dtc.Item(iSelectedIndex).FileURL) & "&Pagename=" & Pagename & "&bSupportRiskLevel=" & bSupportRiskLevel.ToString() & "&Mode=View" & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=700"
                        Else
                            sUrl = AppSettings("WebRoot") & "/Modal/EditDocument.aspx?DocPath=" & Server.UrlEncode(dtc.Item(iSelectedIndex).FileURL) & "&Pagename=" & Pagename & "&bSupportRiskLevel=" & bSupportRiskLevel.ToString() & "&Mode=View" & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=700"
                        End If
                        Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show",
                        "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sUrl & "' , null);});</script>")
                    End If

                Case "Delete"

                    For iCount As Integer = 0 To grdWordings.Rows.Count - 1
                        Dim sCode As String = grdWordings.Rows(iCount).Cells(0).Text.Trim
                        If CStr(e.CommandArgument).ToString.Trim = sCode Then
                            dtc.Remove(iCount)
                            Exit For
                        End If
                    Next
                    'Population of the grid 
                    grdWordings.DataSource = dtc
                    grdWordings.DataBind()
                    If bSupportRiskLevel Then
                        'At risk level update the session value with current doc template collection 
                        Session(sNewRiskStdWrdSession) = dtc
                    Else
                        'At policy level update the session value with current doc template collection 
                        Session(CNPolicyStandardWordingsTemplate) = dtc
                        'set the session value to 0 once we edited the selected document
                        Session(CNFreshPolicySW) = 0
                    End If
            End Select

        End Sub

        ''' <summary>
        ''' Handles gridview row data binding. Hides up or down control if either first or last row.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdWordings_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdWordings.RowDataBound
            Dim sNewRiskStdWrdSession As String
            Dim sChild As String
            sChild = Regex.Split(Me.ID.ToUpper(), "__")(1)
            sChild = "SW." + sChild
            sNewRiskStdWrdSession = CNRiskStandardWordingsTemplate + sChild
            If e.Row.RowType = DataControlRowType.DataRow Then
                If e.Row.DataItemIndex = 0 Then
                    'first row, hide the up link
                    Dim lnkUp As LinkButton = CType(e.Row.FindControl("lnkUp"), LinkButton)
                    If lnkUp IsNot Nothing Then
                        lnkUp.Visible = False
                    End If
                    'if there is only one record then Down link shold not be visible
                    Dim lnkDown As LinkButton = CType(e.Row.FindControl("lnkDown"), LinkButton)
                    If bSupportRiskLevel Then
                        If CType(Session(sNewRiskStdWrdSession), NexusProvider.DocumentTemplateCollection).Count = 1 Then
                            lnkDown.Visible = False
                        End If
                    Else
                        If CType(Session(CNPolicyStandardWordingsTemplate), NexusProvider.DocumentTemplateCollection).Count = 1 Then
                            lnkDown.Visible = False
                        End If
                    End If
                Else
                    'last row so hide the down link
                    Dim lnkDown As LinkButton = CType(e.Row.FindControl("lnkDown"), LinkButton)

                    If bSupportRiskLevel Then
                        If e.Row.DataItemIndex = CType(Session(sNewRiskStdWrdSession), NexusProvider.DocumentTemplateCollection).Count - 1 Then
                            'last row so hide the down link
                            If lnkDown IsNot Nothing Then
                                lnkDown.Visible = False
                            End If
                        End If
                    Else
                        If e.Row.DataItemIndex = CType(Session(CNPolicyStandardWordingsTemplate), NexusProvider.DocumentTemplateCollection).Count - 1 Then
                            'last row so hide the down link
                            If lnkDown IsNot Nothing Then
                                lnkDown.Visible = False
                            End If
                        End If
                    End If
                End If
                'Mark the "Edited" if IsEdit is set to True
                If (CInt(grdWordings.DataKeys(e.Row.RowIndex).Value.ToString()) < 0) Then
                    e.Row.Cells(1).Text = e.Row.Cells(1).Text & " " & GetLocalResourceObject("lbl_Edited")
                End If

                Dim lnkView As LinkButton = CType(e.Row.FindControl("lnkView"), LinkButton)
                If lnkView IsNot Nothing Then
                    If UserCanDoTask("ViewEndorsments") = False AndAlso Session(CNMode) <> Mode.View Then
                        lnkView.Visible = False
                    Else
                        lnkView.Visible = True
                    End If
                End If
            End If
        End Sub

        Protected Sub btnApplySelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApplySelection.Click

            Dim sNewRiskStdWrdSession As String
            Dim sChild As String
            sChild = Regex.Split(Me.ID.ToUpper(), "__")(1)
            sChild = "SW." + sChild
            sNewRiskStdWrdSession = CNRiskStandardWordingsTemplate + sChild
            Dim oDocumentTemplateColl As New NexusProvider.DocumentTemplateCollection
            Dim oListItemColl As ListItemCollection

            Dim oTempDocumentTemplateColl As NexusProvider.DocumentTemplateCollection
            'Storing the selected values in viewstate
            If bSupportRiskLevel Then
                'At risk level update the session value with current doc template collection 
                oTempDocumentTemplateColl = Session(sNewRiskStdWrdSession)
            Else
                'At policy level update the session value with current doc template collection 
                oTempDocumentTemplateColl = Session(CNPolicyStandardWordingsTemplate)
            End If
            'Pull all the selected wordings into oQuote
            oListItemColl = PckTemplates.GetSelectedItems()

            If oListItemColl IsNot Nothing AndAlso oListItemColl.Count > 0 Then
                For iCount As Integer = 0 To oListItemColl.Count - 1
                    Dim oDocumentTemplate As New NexusProvider.DocumentTemplate
                    oDocumentTemplate.Code = oListItemColl(iCount).Attributes("Code").ToString()
                    oDocumentTemplate.DocumentTemplateId = oListItemColl(iCount).Value
                    oDocumentTemplate.Description = oListItemColl(iCount).Text
                    oDocumentTemplateColl.Add(oDocumentTemplate)
                Next
            End If

            'Storing the selected values in viewstate
            If bSupportRiskLevel Then
                'At risk level update the session value with current doc template collection 
                Session(sNewRiskStdWrdSession) = oDocumentTemplateColl
            Else
                'At policy level update the session value with current doc template collection 
                Session(CNPolicyStandardWordingsTemplate) = oDocumentTemplateColl
            End If
            'Grid population
            grdWordings.DataSource = oDocumentTemplateColl
            grdWordings.DataBind()
        End Sub
        Public Sub SetDefault(ByVal sCodes As String)
            Dim dtc As NexusProvider.DocumentTemplateCollection
            Dim v_sChild As String, sNewRiskStdWrdSession As String
            v_sChild = Regex.Split(Me.ID.ToUpper(), "__")(1)
            v_sChild = "SW." + v_sChild
            sNewRiskStdWrdSession = CNRiskStandardWordingsTemplate + v_sChild
            Session(sNewRiskStdWrdSession) = Nothing


            If Not String.IsNullOrEmpty(sCodes) Then
                FillGrid()
                Dim arrCodes As String()
                arrCodes = sCodes.Split(",")
                'get the template collection from viewstate from selected values
                If bSupportRiskLevel Then

                    dtc = CType(Session(sNewRiskStdWrdSession), NexusProvider.DocumentTemplateCollection)
                Else
                    dtc = CType(Session(CNPolicyStandardWordingsTemplate), NexusProvider.DocumentTemplateCollection)
                End If
                'Find the codes which are pass in sCodes and marked IsDefault to true else False
                If dtc IsNot Nothing Then
                    'Set all codes as IsDefault = False 
                    For Each oItem As NexusProvider.DocumentTemplate In dtc
                        oItem.IsDefault = False
                    Next
                    'check codes exists in collection and set IsDefault = False 
                    For cCount As Integer = 0 To arrCodes.Length - 1
                        For Each oItem As NexusProvider.DocumentTemplate In dtc
                            If oItem.Code = arrCodes(cCount) Then
                                oItem.IsDefault = True
                            End If
                        Next
                    Next
                    'Now remove codes from collection who are not defaulted
                    For iCount As Integer = 0 To dtc.Count - 1
                        If iCount < dtc.Count AndAlso dtc(iCount).IsDefault = False Then
                            dtc.RemoveAt(iCount)
                            iCount = iCount - 1
                        End If
                    Next
                    'For Each oItem As NexusProvider.DocumentTemplate In dtc
                    '    If oItem.IsDefault = False Then
                    '        dtc.Remove(oItem)
                    '    End If
                    'Next
                End If
            Else
                dtc = Nothing
                Dim sValues As New ArrayList
                PckTemplates.SetSelectedValues(sValues.ToArray)
            End If
            'update the session again
            If bSupportRiskLevel Then
                Session(sNewRiskStdWrdSession) = dtc
            Else
                Session(CNPolicyStandardWordingsTemplate) = dtc
            End If
            'rebind the grid
            grdWordings.DataSource = dtc
            grdWordings.DataBind()
            bSetDefault = True
        End Sub


        Protected Sub grdWordings_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdWordings.RowDeleting
            '' add this code to handle the row deleting event of endorsement wording grid.
        End Sub

        Protected Sub grdWordings_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles grdWordings.RowEditing
            grdWordings.EditIndex = -1
            'grdWordings.SetEditRow(-1)
            '' add this code to handle the row editing event of endorsement wording grid.
        End Sub
        Sub AttachEditedStandardWordings(ByVal sFromSession As String, ByRef r_oToDocumentTemplateColl As NexusProvider.DocumentTemplateCollection)
            If ViewState(sFromSession) IsNot Nothing Then
                Dim oEditedDocumentTemplate As NexusProvider.DocumentTemplateCollection = CType(ViewState(sFromSession), NexusProvider.DocumentTemplateCollection)

                For nEditedCol As Integer = 0 To oEditedDocumentTemplate.Count - 1
                    If oEditedDocumentTemplate.Item(nEditedCol).Code.Contains("_ED") Then
                        Dim bAddTemplateToCollection As Boolean = True

                        'CHECK IF IT IS ALREADY ADDDED TO COLLECTION
                        For nAddedCol As Integer = 0 To r_oToDocumentTemplateColl.Count - 1
                            If r_oToDocumentTemplateColl.Item(nAddedCol).Code.ToUpper.Trim =
                                oEditedDocumentTemplate(nEditedCol).Code.ToUpper.Trim Then
                                bAddTemplateToCollection = False
                                Exit For
                            End If
                        Next

                        If bAddTemplateToCollection Then
                            Dim oDocumentTemplate As New NexusProvider.DocumentTemplate
                            oDocumentTemplate.Code = oEditedDocumentTemplate(nEditedCol).Code
                            oDocumentTemplate.Description = oEditedDocumentTemplate(nEditedCol).Description
                            r_oToDocumentTemplateColl.Add(oDocumentTemplate)
                        End If
                    End If
                Next
            End If
        End Sub

         Private Sub OpenDocumentInTxTextControl(ByVal mode As String, ByVal sDocPath As String, ByRef sURL As String)
            If HttpContext.Current.Session.IsCookieless Then
                sURL = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/OpenTXTextControl.aspx?DocPath=" & Server.UrlEncode(sDocPath) & "&Pagename=" & Pagename & "&bSupportRiskLevel=" & bSupportRiskLevel.ToString() & "&Mode=" & mode & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=700"
            Else
                sURL = AppSettings("WebRoot") & "Modal/OpenTXTextControl.aspx?DocPath=" & Server.UrlEncode(sDocPath) & "&Pagename=" & Pagename & "&bSupportRiskLevel=" & bSupportRiskLevel.ToString() & "&Mode=" & mode & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=700"
            End If

        End Sub

       Private Function CopyDocumentForView(ByVal sExistingDocumentPath As String) As String
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
            Dim sPath As String = ""
            Dim sFileExt As String = ""
            If sExistingDocumentPath IsNot Nothing AndAlso Not String.IsNullOrEmpty(sExistingDocumentPath) Then
                sFileExt = Path.GetExtension(sExistingDocumentPath)
                If File.Exists(sExistingDocumentPath) Then
                    sPath = oPortal.TempFileLocation & "\TmpViewClauses"
                    If Directory.Exists(sPath) Then
                        DeleteDirectory(sPath)
                    End If

                    If Not Directory.Exists(sPath) Then
                        IO.Directory.CreateDirectory(sPath)
                    End If
                    sPath = sPath & "\" & Guid.NewGuid().ToString & sFileExt
                    Try
                        IO.File.Copy(sExistingDocumentPath, sPath)
                    Catch ex As Exception

                    End Try

                End If
            End If
            Return sPath
        End Function

        Private Sub DeleteDirectory(ByVal sDirPath As String)
            Try
                Directory.Delete(sDirPath, True)
            Catch ex As Exception

            End Try
        End Sub
    End Class

End Namespace
