Imports SiriusFS.SAM.Client
Imports Microsoft.VisualBasic
Imports System.IO
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
Imports System.Linq
Imports System.Xml.Linq

Namespace Nexus

    ''' <summary>
    ''' this control will show all the referrals which have been generated in a grid and will allow 
    ''' the loggedin users to authorise or dealt the referrals if the loggedin user belongs to the
    ''' usergroup which has the authority to authorise or dealt the referrals 
    ''' </summary>
    ''' <remarks></remarks>
    Partial Class Controls_Referral : Inherits System.Web.UI.UserControl

        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim sDataModelCode As String = String.Empty 'Data Model Code
        Dim sUserName As String 'Logged in user

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            sDataModelCode = Session(CNDataModelCode) 'Data model code
            sUserName = Session(CNLoginName) 'Logged in username is used to authorise or dealt the referral and XML is updated accordingly
            BindData() 'Bind control is used to bind the referral grid

        End Sub

        ''' <summary>
        ''' Binds the referral grid 
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub BindData()

            'set oQuote from session
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim srDataset As New System.IO.StringReader(oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)
            Dim xmlTR As New XmlTextReader(srDataset)
            Dim Doc As New XmlDocument
            Doc.Load(xmlTR) 'load the XML
            xmlTR.Close() 'close the text reader
            Dim oNode As XmlNode
            'check if this object exists in XML
            oNode = Doc.SelectSingleNode("/DATA_SET/RISK_OBJECTS/" & sDataModelCode & "_POLICY_BINDER/" & sDataModelCode & "_OUTPUT_REFERRALS")
            'If the node exists in the XML
            If oNode IsNot Nothing Then
                Dim docX As XDocument = XDocument.Parse(Doc.OuterXml) 'Convert from XML document to XDocument
                'Fetch the data from XML which is there in output_referrals object
                Dim ReferralOutput = _
                       From ReferralOutputs In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sDataModelCode & "_OUTPUT_REFERRALS")
                'Bind(GridView) if the data is there in the XML in regard to referrals
                grdvReferral.DataSource = ReferralOutput
                grdvReferral.DataBind()
            End If

        End Sub

        Protected Sub grdvReferral_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvReferral.RowDataBound

            If e.Row.RowType = DataControlRowType.DataRow Then
                'set oQuote from session
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                Dim srDataset As New System.IO.StringReader(oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)
                Dim xmlTR As New XmlTextReader(srDataset)
                Dim Doc As New XmlDocument
                Doc.Load(xmlTR) 'Load the XML
                xmlTR.Close()  'close the text reader
                Dim docX As XDocument = XDocument.Parse(Doc.OuterXml) 'Convert from XML document to XDocument
                'Fetch the data from XML which is there in output_referrals object
                Dim ReferralOutput = _
                   From ReferralOutputs In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sDataModelCode & "_OUTPUT_REFERRALS") _
                   Where ReferralOutputs.Attribute("OI").Value = CType(e.Row.Cells(5).FindControl("lblOI"), Label).Text
                'set oUserDetails with User Details from session
                Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
                'set oUserGroups with usergroups attached with the user
                Dim oUserGroups As NexusProvider.UserGroupCollection = oUserDetails.AvailableUsergroups
                Dim bUserGroupExist As Boolean
                Dim sAllUserGroups As String
                'check if the referral output object has got data
                If ReferralOutput IsNot Nothing Then
                    If ReferralOutput.Count > 0 Then
                        'check if the property exists in the object
                        If ReferralOutput.ElementAt(0).Attribute("USER_GROUP") Is Nothing Then
                            sAllUserGroups = 0
                        Else
                            sAllUserGroups = ReferralOutput.ElementAt(0).Attribute("USER_GROUP").Value
                        End If
                        Dim aUserGroups As String() = sAllUserGroups.Split(",")
                        'check if the user belongs to any one of the usergroups defined by the product rules
                        For uCount As Integer = 0 To aUserGroups.Length - 1
                            For iCount As Integer = 0 To oUserGroups.Count - 1
                                If InStr(oUserGroups(iCount).Code, aUserGroups(uCount)) > 0 Then
                                    bUserGroupExist = True 'set true when user belongs to any of the usergroups defined by the product rules
                                    Exit For
                                End If
                            Next
                        Next
                        'Fetch OI Key of the parent
                        Dim strId As String = Mid(CType(e.Row.Cells(5).FindControl("lblOI"), Label).Text, 3)
                        'check if the child object exists in the xml
                        If Doc.SelectSingleNode("/DATA_SET/RISK_OBJECTS/" & sDataModelCode & "_POLICY_BINDER/" & sDataModelCode & "_OUTPUT_REFERRALS/" & sDataModelCode & "_OUTPUT_REFERRALS_AUDIT") IsNot Nothing Then
                            'Fetch data from XML which is there in output_referrals_audit object and has type as "AUTH"
                            Dim ReferralAuditOutputAuth = _
                             From ReferralAuditOutputs In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sDataModelCode & "_OUTPUT_REFERRALS").Descendants(sDataModelCode & "_OUTPUT_REFERRALS_AUDIT") _
                           Where ReferralAuditOutputs.Attribute(sDataModelCode & "_OUTPUT_REFERRALS_ID").Value = strId And Convert.ToString(ReferralAuditOutputs.Attribute("TYPE").Value).ToUpper = "AUTH"
                            'Fetch data from XML which is there in output_referrals_audit object and has type as "DEALT"
                            Dim ReferralAuditOutputDealt = _
                             From ReferralAuditOutputs In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sDataModelCode & "_OUTPUT_REFERRALS").Descendants(sDataModelCode & "_OUTPUT_REFERRALS_AUDIT") _
                           Where ReferralAuditOutputs.Attribute(sDataModelCode & "_OUTPUT_REFERRALS_ID").Value = strId And Convert.ToString(ReferralAuditOutputs.Attribute("TYPE").Value).ToUpper = "DEALT"
                            Dim strQueryAuth As String
                            Dim strQueryDealt As String
                            'Fetch the row based on OI Key of object and parent object
                            strQueryAuth = "/DATA_SET/RISK_OBJECTS/" & sDataModelCode & "_POLICY_BINDER/" & sDataModelCode & "_OUTPUT_REFERRALS/" & sDataModelCode & "_OUTPUT_REFERRALS_AUDIT[@TYPE='AUTH' and @" & sDataModelCode & "_OUTPUT_REFERRALS_ID='" & strId & "']"
                            strQueryDealt = "/DATA_SET/RISK_OBJECTS/" & sDataModelCode & "_POLICY_BINDER/" & sDataModelCode & "_OUTPUT_REFERRALS/" & sDataModelCode & "_OUTPUT_REFERRALS_AUDIT[@TYPE='DEALT' and @" & sDataModelCode & "_OUTPUT_REFERRALS_ID='" & strId & "']"
                            'fetch the existing status of the referrals
                            'check if ReferralAuditOutputAuth has got some data
                            If ReferralAuditOutputAuth IsNot Nothing Then
                                If ReferralAuditOutputAuth.Count > 0 Then
                                    CType(e.Row.Cells(1).FindControl("chkAuthorize"), CheckBox).Checked = ReferralAuditOutputAuth.ElementAt(0).Attribute("APPROVED").Value
                                    CType(e.Row.Cells(1).FindControl("AuthorisedBy"), Label).Text = ReferralAuditOutputAuth.ElementAt(0).Attribute("USERNAME").Value
                                End If
                            End If
                            'check if ReferralAuditOutputDealt has got some data
                            If ReferralAuditOutputDealt IsNot Nothing Then
                                If ReferralAuditOutputDealt.Count > 0 Then
                                    CType(e.Row.Cells(1).FindControl("chkDealtwith"), CheckBox).Checked = ReferralAuditOutputDealt.ElementAt(0).Attribute("APPROVED").Value
                                    CType(e.Row.Cells(1).FindControl("DealtWithBy"), Label).Text = ReferralAuditOutputDealt.ElementAt(0).Attribute("USERNAME").Value
                                End If
                            End If
                            If bUserGroupExist Then
                                If Not IsPostBack Then
                                    'By default the approved checkbox should be checked if loggedin user is member of the user group and update the xml
                                    If CType(e.Row.Cells(1).FindControl("chkAuthorize"), CheckBox).Checked = False Then
                                        CType(e.Row.Cells(1).FindControl("chkAuthorize"), CheckBox).Checked = True
                                        'update XML- update the value of approved to 1 if the user has checked the authorise? checkbox
                                        UpdateXML(strQueryAuth, sDataModelCode & "_OUTPUT_REFERRALS/" & sDataModelCode & "_OUTPUT_REFERRALS_AUDIT", "APPROVED", 1)
                                        CType(e.Row.Cells(1).FindControl("AuthorisedBy"), Label).Text = sUserName
                                        'update XML- update the value of username to the loggedin user if the logged in user checked the authorise? checkbox
                                        UpdateXML(strQueryAuth, sDataModelCode & "_OUTPUT_REFERRALS/" & sDataModelCode & "_OUTPUT_REFERRALS_AUDIT", "USERNAME", sUserName)
                                        CType(e.Row.Cells(1).FindControl("chkDealtwith"), CheckBox).Enabled = True
                                    End If
                                End If
                                'Authorised? should be read only for all the users apart from the one who authorised it earlier
                                If (CType(e.Row.Cells(1).FindControl("chkAuthorize"), CheckBox).Checked = True And CType(e.Row.Cells(1).FindControl("AuthorisedBy"), Label).Text = sUserName) Or (CType(e.Row.Cells(1).FindControl("AuthorisedBy"), Label).Text = "0" Or CType(e.Row.Cells(1).FindControl("AuthorisedBy"), Label).Text = "") Then
                                    CType(e.Row.Cells(1).FindControl("chkAuthorize"), CheckBox).Enabled = True
                                Else
                                    CType(e.Row.Cells(1).FindControl("chkAuthorize"), CheckBox).Enabled = False
                                End If
                                'DealtWithBy? should be read only for all the users apart from the one who Dealt with it earlier
                                If CType(e.Row.Cells(1).FindControl("chkAuthorize"), CheckBox).Checked = True And (CType(e.Row.Cells(1).FindControl("DealtWithBy"), Label).Text = sUserName Or CType(e.Row.Cells(1).FindControl("DealtWithBy"), Label).Text = "0" Or CType(e.Row.Cells(1).FindControl("DealtWithBy"), Label).Text = "") Then
                                    CType(e.Row.Cells(1).FindControl("chkDealtwith"), CheckBox).Enabled = True
                                Else
                                    CType(e.Row.Cells(1).FindControl("chkDealtwith"), CheckBox).Enabled = False
                                End If
                            Else
                                'Only the user which belongs to the user group mentioned in the rating should be able to authorise
                                CType(e.Row.Cells(1).FindControl("chkAuthorize"), CheckBox).Enabled = False
                                CType(e.Row.Cells(1).FindControl("chkDealtwith"), CheckBox).Enabled = False
                            End If

                            'By default when Authorised by And dealt with by usernames are passed as 0 by product rules show them blank and update the xml
                            If CType(e.Row.Cells(1).FindControl("AuthorisedBy"), Label).Text = "0" Or CType(e.Row.Cells(1).FindControl("AuthorisedBy"), Label).Text = "" Then
                                CType(e.Row.Cells(1).FindControl("AuthorisedBy"), Label).Text = ""
                                'update XML- update the value of username usernames are passed as 0 by product rules
                                UpdateXML(strQueryAuth, sDataModelCode & "_OUTPUT_REFERRALS/" & sDataModelCode & "_OUTPUT_REFERRALS_AUDIT", "USERNAME", "0")
                            End If

                            If CType(e.Row.Cells(1).FindControl("DealtWithBy"), Label).Text = "0" Or CType(e.Row.Cells(1).FindControl("DealtWithBy"), Label).Text = "" Then
                                CType(e.Row.Cells(1).FindControl("DealtWithBy"), Label).Text = ""
                                'update XML- update the value of username usernames are passed as 0 by product rules
                                UpdateXML(strQueryDealt, sDataModelCode & "_OUTPUT_REFERRALS/" & sDataModelCode & "_OUTPUT_REFERRALS_AUDIT", "USERNAME", "0")
                            End If
                        End If
                    End If
                End If
            End If

        End Sub

        ''' <summary>
        ''' if Authorised checkbox is checked then only dealt checkbox will be enabled 
        ''' o.w dealt checkbox will be disabled and update XML with the status of the referral and 
        ''' username of the user who authorised the referral
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub chkAuth_OnCheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

            For iTempVar As Integer = 0 To grdvReferral.Rows.Count - 1
                'Fetch OI Key of the parent
                Dim strId As String = Mid(CType(grdvReferral.Rows(iTempVar).Cells(5).FindControl("lblOI"), Label).Text, 3)
                Dim strQueryAuth As String
                Dim strQueryDealt As String
                'Fetch the row based on OI Keys of object and parent object
                strQueryAuth = "/DATA_SET/RISK_OBJECTS/" & sDataModelCode & "_POLICY_BINDER/" & sDataModelCode & "_OUTPUT_REFERRALS/" & sDataModelCode & "_OUTPUT_REFERRALS_AUDIT[@TYPE='AUTH' and @" & sDataModelCode & "_OUTPUT_REFERRALS_ID='" & strId & "']"
                'Fetch the row based on OI Key of object and parent object
                strQueryDealt = "/DATA_SET/RISK_OBJECTS/" & sDataModelCode & "_POLICY_BINDER/" & sDataModelCode & "_OUTPUT_REFERRALS/" & sDataModelCode & "_OUTPUT_REFERRALS_AUDIT[@TYPE='DEALT' and @" & sDataModelCode & "_OUTPUT_REFERRALS_ID='" & strId & "']"
                'set oUserDetails with User Details from session
                Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
                'set oUserGroups with usergroups attached with the user
                Dim oUserGroups As NexusProvider.UserGroupCollection = oUserDetails.AvailableUsergroups

                'if Authorise? checkbox is checked by the user, Authorised by should show the username and update the xml
                If CType(grdvReferral.Rows(iTempVar).Cells(1).FindControl("chkAuthorize"), CheckBox).Checked = True And (CType(grdvReferral.Rows(iTempVar).FindControl("AuthorisedBy"), Label).Text = "0" Or CType(grdvReferral.Rows(iTempVar).FindControl("AuthorisedBy"), Label).Text = "") Then
                    'update XML- update the value of approved to 1 if the user has checked the authorise? checkbox
                    UpdateXML(strQueryAuth, sDataModelCode & "_OUTPUT_REFERRALS/" & sDataModelCode & "_OUTPUT_REFERRALS_AUDIT", "APPROVED", 1)
                    CType(grdvReferral.Rows(iTempVar).FindControl("AuthorisedBy"), Label).Text = sUserName 'display the username who checked the authorise? checkbox
                    'update XML- update the value of username to the loggedin user if the logged in user checked the authorise? checkbox
                    UpdateXML(strQueryAuth, sDataModelCode & "_OUTPUT_REFERRALS/" & sDataModelCode & "_OUTPUT_REFERRALS_AUDIT", "USERNAME", sUserName)
                    'If authrised? is checked then dealt? checkbox should be enabled
                    CType(grdvReferral.Rows(iTempVar).FindControl("chkDealtwith"), CheckBox).Enabled = True
                ElseIf CType(grdvReferral.Rows(iTempVar).Cells(1).FindControl("chkAuthorize"), CheckBox).Checked = False Then
                    'update XML- update the value of approved to 0 if the user has unchecked the authorise? checkbox
                    UpdateXML(strQueryAuth, sDataModelCode & "_OUTPUT_REFERRALS/" & sDataModelCode & "_OUTPUT_REFERRALS_AUDIT", "APPROVED", 0)
                    CType(grdvReferral.Rows(iTempVar).Cells(1).FindControl("AuthorisedBy"), Label).Text = "" 'referral has not been authorised by any user
                    'update XML- update the value of username to 0 for authorisedby
                    UpdateXML(strQueryAuth, sDataModelCode & "_OUTPUT_REFERRALS/" & sDataModelCode & "_OUTPUT_REFERRALS_AUDIT", "USERNAME", "0")
                    'If authorise? checkbox is unchecked, dealtWithBy? is unckecked and should be readonly and update the xml
                    CType(grdvReferral.Rows(iTempVar).Cells(1).FindControl("chkDealtwith"), CheckBox).Enabled = False
                    CType(grdvReferral.Rows(iTempVar).Cells(1).FindControl("chkDealtwith"), CheckBox).Checked = False
                    'update XML- update the value of approved to 0 if the user has unchecked the dealt? checkbox
                    UpdateXML(strQueryDealt, sDataModelCode & "_OUTPUT_REFERRALS/" & sDataModelCode & "_OUTPUT_REFERRALS_AUDIT", "APPROVED", 0)
                    CType(grdvReferral.Rows(iTempVar).Cells(1).FindControl("DealtWithBy"), Label).Text = "" 'referral has not been dealt with by any user
                    'update XML- update the value of username to 0 for dealtwithby
                    UpdateXML(strQueryDealt, sDataModelCode & "_OUTPUT_REFERRALS/" & sDataModelCode & "_OUTPUT_REFERRALS_AUDIT", "USERNAME", "0")
                End If
            Next

        End Sub

        ''' <summary>
        ''' update the xml with the status of the referral and 
        ''' username of the user who dealt the referral 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub chkDealt_OnCheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

            Dim iAppDealt As Integer 'variable used to count if there is some referral which has not been dealt yet
            Dim iNotDealt As Integer 'variable used to check if some referral has not been dealt with yet
            iAppDealt = 0
            iNotDealt = 0
            'for every row of the grid
            For iTempVar As Integer = 0 To grdvReferral.Rows.Count - 1
                'Fetch OI Key of the parent
                Dim strId As String = Mid(CType(grdvReferral.Rows(iTempVar).Cells(5).FindControl("lblOI"), Label).Text, 3)
                Dim strQueryDealt As String
                'Fetch the row based on OI Key of object and parent object
                strQueryDealt = "/DATA_SET/RISK_OBJECTS/" & sDataModelCode & "_POLICY_BINDER/" & sDataModelCode & "_OUTPUT_REFERRALS/" & sDataModelCode & "_OUTPUT_REFERRALS_AUDIT[@TYPE='DEALT' and @" & sDataModelCode & "_OUTPUT_REFERRALS_ID='" & strId & "']"
                'set oUserDetails with User Details from session
                Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
                'set oUserGroups with usergroups attached with the user
                Dim oUserGroups As NexusProvider.UserGroupCollection = oUserDetails.AvailableUsergroups
                'if dealtWithBy? checkbox is checked by the user, Dealt with by should show the username and update the xml
                If CType(grdvReferral.Rows(iTempVar).Cells(3).FindControl("chkDealtwith"), CheckBox).Checked = True And (CType(grdvReferral.Rows(iTempVar).FindControl("DealtWithBy"), Label).Text = "0" Or CType(grdvReferral.Rows(iTempVar).FindControl("DealtWithBy"), Label).Text = "") Then
                    'update XML- update the value of approved to 1 if the user has checked the dealt? checkbox
                    UpdateXML(strQueryDealt, sDataModelCode & "_OUTPUT_REFERRALS/" & sDataModelCode & "_OUTPUT_REFERRALS_AUDIT", "APPROVED", 1)
                    CType(grdvReferral.Rows(iTempVar).FindControl("DealtWithBy"), Label).Text = sUserName 'display the username who checked the dealt? checkbox
                    'update XML- update the value of username to the loggedin user if the logged in user checked the dealt? checkbox
                    UpdateXML(strQueryDealt, sDataModelCode & "_OUTPUT_REFERRALS/" & sDataModelCode & "_OUTPUT_REFERRALS_AUDIT", "USERNAME", sUserName)
                    'if  dealtWithBy? checkbox is unchecked, username should be blank and update the xml
                ElseIf CType(grdvReferral.Rows(iTempVar).Cells(3).FindControl("chkDealtwith"), CheckBox).Checked = False Then
                    'update XML- update the value of approved to 0 if the user has unchecked the dealt? checkbox
                    UpdateXML(strQueryDealt, sDataModelCode & "_OUTPUT_REFERRALS/" & sDataModelCode & "_OUTPUT_REFERRALS_AUDIT", "APPROVED", 0)
                    CType(grdvReferral.Rows(iTempVar).Cells(1).FindControl("DealtWithBy"), Label).Text = "" 'referral has not been dealt with by any user
                    'update XML- update the value of username to 0 for dealtwithby
                    UpdateXML(strQueryDealt, sDataModelCode & "_OUTPUT_REFERRALS/" & sDataModelCode & "_OUTPUT_REFERRALS_AUDIT", "USERNAME", "0")
                End If
                'check if all the referrals are approved and dealt with.
                If CType(grdvReferral.Rows(iTempVar).Cells(3).FindControl("chkDealtwith"), CheckBox).Checked = True And CType(grdvReferral.Rows(iTempVar).Cells(1).FindControl("chkAuthorize"), CheckBox).Checked = True Then
                    iAppDealt = 1 + iAppDealt 'count of the referrals which has been dealt with
                ElseIf CType(grdvReferral.Rows(iTempVar).Cells(3).FindControl("chkDealtwith"), CheckBox).Checked = False Then
                    iNotDealt = 1 'if some referral has not been dealt with
                End If
            Next
            'If count of referrals which has been dealt with is equal to total number of referrals and no referral is therer which has not been dealt with
            If iAppDealt = grdvReferral.Rows.Count And iNotDealt <> 1 Then
                ApplySave() 'If all referrals are approved and dealt with then run updaterisk
            End If

        End Sub

        ''' <summary>
        ''' updaterisk is called if all referrals are approved and dealt
        ''' </summary>
        ''' <remarks></remarks>
        Sub ApplySave()

            Dim oWebService As NexusProvider.ProviderBase
            'retrieve oQuote from session
            Dim oQuote As NexusProvider.Quote = System.Web.HttpContext.Current.Session(CNQuote)
            'create new instance of proxy
            oWebService = New NexusProvider.ProviderManager().Provider
            Try
                'save the risk changes to the database- rerun the scripts
                oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey))
                System.Web.HttpContext.Current.Session(CNQuote) = oQuote
                'error handling - in case quote is referred or declined
            Catch ex As NexusProvider.NexusException
                If ex.Errors(0).Code = "277" Or ex.Errors(0).Code = "279" Then 'in case the qoute is referred
                ElseIf ex.Errors(0).Code = "278" Or ex.Errors(0).Code = "280" Then 'in case the qoute is declined
                End If
            Finally
                oWebService = Nothing  'clear the object
            End Try

        End Sub

    End Class

End Namespace
