Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
'developer guide no.  added as per requirement
Imports PMLookupControl
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("uctPMAddressControl_NET.uctPMAddressControl")> _
Partial Public Class uctPMAddressControl
    Inherits System.Windows.Forms.UserControl
    Implements IDisposable
    Public Event IsPostCodeRequiredChange()
    Public Event IsCountryRequiredChange()
    Public Event CountryIdChange()
    Public Event CaptionCountryChange()
    Public Event ClearButtonFontChange()
    Public Event ClearButtonWidthChange()
    Public Event ClearButtonCaptionChange()
    Public Event FaceFontChange()
    Public Event SearchButtonCaptionChange()
    Public Event SearchButtonFontChange()
    Public Event SearchButtonHeightChange()
    Public Event SearchButtonWidthChange()
    Public Event SearchButtonLeftChange()
    Public Event SearchButtonTopChange()
    Public Event CaptionAddress4Change()
    Public Event CaptionAddress3Change()
    Public Event CaptionAddress2Change()
    Public Event CaptionAddress1Change()
    Public Event CaptionPostCodeChange()
    Public Event PMAddressCntChange()
    Public Event QAS2PMAddress1Change()
    Public Event QAS2PMAddress4Change()
    Public Event QAS2PMAddress3Change()
    Public Event QAS2PMAddress2Change()
    Public Event QASDatabaseIDChange()
    Public Event PMDatabaseIDChange()
    Public Event OrganisationChange()
    Public Event AddressLine4Change()
    Public Event AddressLine3Change()
    Public Event AddressLine2Change()
    Public Event AddressLine1Change()
    Public Event PostCodeChange()
    ' ***************************************************************** '
    ' Control Name: uctPMAddressControl
    '
    ' Date: 05/06/1998
    '
    ' Description: User control.
    '
    ' Edit History: TF050698 - Created
    ' DAK130700 - Add Country pull down
    ' ***************************************************************** '
    'DEEPAK_COMMENT: Replaced iPMFunc.GetResData with GetResData in the whole document

    ' ***************************************************************** '
    ' Constant for the functions to identify which class this is.
    ' ***************************************************************** '
    Private Const ACClass As String = "uctPMAddressControl"


    ' ***************************************************************** '
    ' Variable declarations
    ' ***************************************************************** '
    Private m_lReturn As Integer
    Private m_oBusiness As Object

    ' Database Search Engines
    Private m_lPMDatabaseID As Integer
    Private m_lQASDatabaseID As Integer

    'QAS to PM Address line mapping
    Private m_sQAS2PMAddress1 As String = ""
    Private m_sQAS2PMAddress2 As String = ""
    Private m_sQAS2PMAddress3 As String = ""
    Private m_sQAS2PMAddress4 As String = ""

    ' Keep as is retrieved from QAS Pro
    Private m_sOrganisation As String = ""

    ' ID of Address retrieved from PM Database
    Private m_lPMAddressCnt As Integer

    ' Font properties
    Private m_fSearchFont As Font
    Private m_fFaceFont As Font
    Private m_fClearFont As Font

    Private m_iIsCountryRequired As Integer
    Private m_iIsPostCodeRequired As Integer
    Private m_vCountryConfig(,) As Object
    Private m_bCountryComboClicked As Boolean
    Private m_sWarningMessage As String = ""

    ' Name properties for QAS names
    Private m_sInitial As String = ""
    Private m_sForename As String = ""
    Private m_sSurname As String = ""
    Private m_sTitle As String = ""
    Private m_sOrgname As String = ""


    ' ***************************************************************** '
    ' Event Declarations
    ' ***************************************************************** '
    Public Event ChosenAddress(ByVal Sender As Object, ByVal e As EventArgs)
    'PN:45199
    Public Event AddressCleared(ByVal Sender As Object, ByVal e As EventArgs)

    ' ***************************************************************** '
    ' Public Properties
    ' ***************************************************************** '
    <Browsable(True)> _
    Public Property Caption() As String
        Get
            Return fraAddress.Text
        End Get
        Set(ByVal Value As String)
            fraAddress.Text = Value
        End Set
    End Property

    <Browsable(False)> _
    Public ReadOnly Property Title() As String
        Get
            Return m_sTitle
        End Get
    End Property
    <Browsable(False)> _
    Public ReadOnly Property Forename() As String
        Get
            Return m_sForename
        End Get
    End Property
    <Browsable(False)> _
    Public ReadOnly Property Initial() As String
        Get
            Return m_sInitial
        End Get
    End Property
    <Browsable(False)> _
    Public ReadOnly Property Surname() As String
        Get
            Return m_sSurname
        End Get
    End Property
    <Browsable(False)> _
    Public ReadOnly Property OrgName() As String
        Get
            Return m_sOrgname
        End Get
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=txtPostCode,txtPostCode,-1,Text

    <Browsable(True)> _
    Public Property PostCode() As String
        Get
            Return txtPostCode.Text
        End Get
        Set(ByVal Value As String)
            txtPostCode.Text = Value
            RaiseEvent PostCodeChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=txtAddress1,txtAddress1,-1,Text

    <Browsable(True)> _
    <Category("Default")> _
    Public Property AddressLine1() As String
        Get
            Return txtAddress1.Text
        End Get
        Set(ByVal Value As String)
            txtAddress1.Text = Value
            RaiseEvent AddressLine1Change()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=txtAddress2,txtAddress2,-1,Text

    <Browsable(True)> _
    <Category("Default")> _
    Public Property AddressLine2() As String
        Get
            Return txtAddress2.Text
        End Get
        Set(ByVal Value As String)
            txtAddress2.Text = Value
            RaiseEvent AddressLine2Change()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=txtAddress3,txtAddress3,-1,Text

    <Browsable(True)> _
    <Category("Default")> _
    Public Property AddressLine3() As String
        Get
            Return txtAddress3.Text
        End Get
        Set(ByVal Value As String)
            txtAddress3.Text = Value
            RaiseEvent AddressLine3Change()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=txtAddress4,txtAddress4,-1,Text

    <Browsable(True)> _
    <Category("Default")> _
    Public Property AddressLine4() As String
        Get
            Return txtAddress4.Text
        End Get
        Set(ByVal Value As String)
            ' Store to text field and check combo
            txtAddress4.Text = Value
            lblStateDesc.Text = Value
            SelectComboItem(r_ctl:=cboState, v_sState:=Value)
            RaiseEvent AddressLine4Change()
        End Set
    End Property

    <Browsable(False)> _
    Public ReadOnly Property StateID() As Integer
        Get
            ' Return the state id only if the control is active
            If cboState.Visible Then
                Return cboState.ItemId
            Else
                Return 0
            End If
        End Get
    End Property


    <Browsable(True)> _
    Public Property Organisation() As String
        Get
            Return m_sOrganisation
        End Get
        Set(ByVal Value As String)
            m_sOrganisation = Value
            RaiseEvent OrganisationChange()
        End Set
    End Property


    <Browsable(True)> _
    <Editor()> _
    Public Property PMDatabaseID() As Integer
        Get
            Return m_lPMDatabaseID
        End Get
        Set(ByVal Value As Integer)
            m_lPMDatabaseID = Value
            RaiseEvent PMDatabaseIDChange()
        End Set
    End Property

    <Browsable(True)> _
    <Editor()> _
    Public Property QASDatabaseID() As Integer
        Get
            Return m_lQASDatabaseID
        End Get
        Set(ByVal Value As Integer)
            m_lQASDatabaseID = Value
            RaiseEvent QASDatabaseIDChange()
        End Set
    End Property


    <Browsable(True)> _
    <Editor()> _
    Public Property QAS2PMAddress2() As String
        Get
            Return m_sQAS2PMAddress2
        End Get
        Set(ByVal Value As String)
            m_sQAS2PMAddress2 = Value
            RaiseEvent QAS2PMAddress2Change()
        End Set
    End Property


    <Browsable(True)> _
    <Editor()> _
    Public Property QAS2PMAddress3() As String
        Get
            Return m_sQAS2PMAddress3
        End Get
        Set(ByVal Value As String)
            m_sQAS2PMAddress3 = Value
            RaiseEvent QAS2PMAddress3Change()
        End Set
    End Property


    <Browsable(True)> _
    <Editor()> _
    Public Property QAS2PMAddress4() As String
        Get
            Return m_sQAS2PMAddress4
        End Get
        Set(ByVal Value As String)
            m_sQAS2PMAddress4 = Value
            RaiseEvent QAS2PMAddress4Change()
        End Set
    End Property
    <Browsable(True)> _
    <Editor()> _
    Public Property QAS2PMAddress1() As String
        Get
            Return m_sQAS2PMAddress1
        End Get
        Set(ByVal Value As String)
            m_sQAS2PMAddress1 = Value
            RaiseEvent QAS2PMAddress1Change()
        End Set
    End Property


    <Browsable(True)> _
    Public Property PMAddressCnt() As Integer
        Get
            Return m_lPMAddressCnt
        End Get
        Set(ByVal Value As Integer)

            If Value = m_lPMAddressCnt Then
                Exit Property
            End If

            m_lPMAddressCnt = Value

            If Value > 0 Then
                ' Set up business object etc.
                m_lReturn = Initialise()

                ' Process Search
                m_lReturn = SearchDatabase()
            End If

            RaiseEvent PMAddressCntChange()

        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=lblPostCode,lblPostCode,-1,Caption

    <Browsable(True)> _
    <Editor()> _
    <Category("Appearance")> _
    Public Property CaptionPostCode() As String
        Get
            Return lblPostCode.Text
        End Get
        Set(ByVal Value As String)
            If Value.Length Then
                lblPostCode.Text = Value
            Else

                lblPostCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionPostCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If
            uctPMAddressControl_Resize(Me, New EventArgs())
            RaiseEvent CaptionPostCodeChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=lblAddress1,lblAddress1,-1,Caption

    <Browsable(True)> _
    <Editor()> _
    <Category("Appearance")> _
    Public Property CaptionAddress1() As String
        Get
            Return lblAddress1.Text
        End Get
        Set(ByVal Value As String)
            If Value.Length Then
                lblAddress1.Text = Value
            Else

                lblAddress1.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionAddress1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If
            RaiseEvent CaptionAddress1Change()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=lblAddress2,lblAddress2,-1,Caption

    <Browsable(True)> _
    <Editor()> _
    <Category("Appearance")> _
    Public Property CaptionAddress2() As String
        Get
            Return lblAddress2.Text
        End Get
        Set(ByVal Value As String)
            If Value.Length Then
                lblAddress2.Text = Value
            Else

                lblAddress2.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionAddress2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If
            RaiseEvent CaptionAddress2Change()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=lblAddress3,lblAddress3,-1,Caption

    <Browsable(True)> _
    <Editor()> _
    <Category("Appearance")> _
    Public Property CaptionAddress3() As String
        Get
            Return lblAddress3.Text
        End Get
        Set(ByVal Value As String)
            If Value.Length Then
                lblAddress3.Text = Value
            Else

                lblAddress3.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionAddress3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If
            RaiseEvent CaptionAddress3Change()
        End Set
    End Property


    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=lblAddress4,lblAddress4,-1,Caption

    <Browsable(True)> _
    <Category("Appearance")> _
    Public Property CaptionAddress4() As String
        Get
            Return lblAddress4.Text
        End Get
        Set(ByVal Value As String)
            If Value.Length Then
                lblAddress4.Text = Value
            Else

                lblAddress4.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionAddress4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If
            RaiseEvent CaptionAddress4Change()
        End Set
    End Property

    'SJ 20/08/2004 - start
    <Browsable(True)> _
    Public Property CaptionFontBoldAddress1() As Boolean
        Get
            Return lblAddress1.Font.Bold
        End Get
        Set(ByVal Value As Boolean)
            lblAddress1.Font = VB6.FontChangeBold(lblAddress1.Font, Value)
        End Set
    End Property

    <Browsable(True)> _
    Public Property CaptionFontBoldPostCode() As Boolean
        Get
            Return lblPostCode.Font.Bold
        End Get
        Set(ByVal Value As Boolean)
            lblPostCode.Font = VB6.FontChangeBold(lblPostCode.Font, Value)
        End Set
    End Property
    'SJ 20/08/2004 - end
    ' CF 260499 - SearchButton.Top
    <Browsable(True)> _
    <Description("Returns/sets the distance between the internal top edge of the search button and it's container. Default: 1500")> _
    <Category("Layout")> _
    Public Property SearchButtonTop() As Integer
        Get
            Return CInt(VB6.PixelsToTwipsY(cmdSearch.Top))
        End Get
        Set(ByVal Value As Integer)
            cmdSearch.Top = VB6.TwipsToPixelsY(Value)
            RaiseEvent SearchButtonTopChange()
        End Set
    End Property

    ' CF 260499 - SearchButton.Left
    <Browsable(True)> _
    <Description("Returns/sets the distance between the internal left edge of the search button and it's container.")> _
    <Category("Layout")> _
    Public Property SearchButtonLeft() As Integer
        Get
            Return CInt(VB6.PixelsToTwipsX(cmdSearch.Left))
        End Get
        Set(ByVal Value As Integer)
            cmdSearch.Left = VB6.TwipsToPixelsX(Value)
            RaiseEvent SearchButtonLeftChange()
        End Set
    End Property

    ' CF 260499 - SearchButton.Width
    <Browsable(True)> _
    <Description("Returns/sets the width for the search button. Default : 360")> _
    <Category("Layout")> _
    Public Property SearchButtonWidth() As Integer
        Get
            Return CInt(VB6.PixelsToTwipsX(cmdSearch.Width))
        End Get
        Set(ByVal Value As Integer)
            cmdSearch.Width = VB6.TwipsToPixelsX(Value)
            RaiseEvent SearchButtonWidthChange()
        End Set
    End Property

    ' CF260499 - SearchButton.Height
    <Browsable(True)> _
    <Description("Returns/sets the height for the search button. Default: 285")> _
    <Category("Layout")> _
    Public Property SearchButtonHeight() As Integer
        Get
            Return CInt(VB6.PixelsToTwipsY(cmdSearch.Height))
        End Get
        Set(ByVal Value As Integer)
            cmdSearch.Height = VB6.TwipsToPixelsY(Value)
            RaiseEvent SearchButtonHeightChange()
        End Set
    End Property

    ' CF 260499 - SearchButton.Font
    <Browsable(True)> _
    <Description("Returns/sets the font for the search button.")> _
    <Category("Appearance")> _
    Public Property SearchButtonFont() As Font
        Get
            Return m_fSearchFont
        End Get
        Set(ByVal Value As Font)
            m_fSearchFont = Value
            cmdSearch.Font = m_fSearchFont
            RaiseEvent SearchButtonFontChange()
        End Set
    End Property

    ' CF 260499 - SearchButton.Caption
    <Browsable(True)> _
    <Description("Returns/sets the caption for the search button.")> _
    <Category("Appearance")> _
    Public Property SearchButtonCaption() As String
        Get
            Return cmdSearch.Text
        End Get
        Set(ByVal Value As String)
            cmdSearch.Text = Value
            RaiseEvent SearchButtonCaptionChange()
        End Set
    End Property

    ' CF260499 - Form/Labels/etc... .Font
    <Browsable(True)> _
    <Description("Returns/sets the font used by controls on the form. Doesn't include the Search button.")> _
    <Category("Appearance")> _
    Public Property FaceFont() As Font
        Get
            Return m_fFaceFont
        End Get
        Set(ByVal Value As Font)
            m_fFaceFont = Value
            ' Update the fonts on the form
            UpdateFaceFont()
            RaiseEvent FaceFontChange()
        End Set
    End Property

    ' CF270499 - ClearButton.Caption
    <Browsable(True)> _
    <Editor()> _
    <Category("Appearance")> _
    Public Property ClearButtonCaption() As String
        Get
            Return cmdDelete.Text
        End Get
        Set(ByVal Value As String)
            cmdDelete.Text = Value
            RaiseEvent ClearButtonCaptionChange()
        End Set
    End Property

    ' CF270499 - ClearButton.Left
    <Browsable(True)> _
    <Category("Layout")> _
    Public Property ClearButtonLeft() As Integer
        Get
            Return CInt(VB6.PixelsToTwipsX(cmdDelete.Left))
        End Get
        Set(ByVal Value As Integer)
            cmdDelete.Left = VB6.TwipsToPixelsX(Value)
        End Set
    End Property

    ' CF270499 - ClearButton.Width
    <Browsable(True)> _
    <Category("Layout")> _
    Public Property ClearButtonWidth() As Integer
        Get
            Return CInt(VB6.PixelsToTwipsX(cmdDelete.Width))
        End Get
        Set(ByVal Value As Integer)
            cmdDelete.Width = VB6.TwipsToPixelsX(Value)
            RaiseEvent ClearButtonWidthChange()
        End Set
    End Property

    ' CF270499 - ClearButton.Font
    <Browsable(True)> _
    <Category("Appearance")> _
    Public Property ClearButtonFont() As Font
        Get
            Return m_fClearFont
        End Get
        Set(ByVal Value As Font)
            m_fClearFont = Value
            cmdDelete.Font = m_fClearFont
            RaiseEvent ClearButtonFontChange()
        End Set
    End Property

    'DAK130700
    <Browsable(True)> _
    Public Property CaptionCountry() As String
        Get
            Return lblCountry.Text
        End Get
        Set(ByVal Value As String)
            lblCountry.Text = Value
            RaiseEvent CaptionCountryChange()
        End Set
    End Property

    <Browsable(True)> _
    Public Property CountryId() As Integer
        Get
            Return cboCountry.ItemId
        End Get
        Set(ByVal Value As Integer)
            cboCountry.ItemId = Value
            RaiseEvent CountryIdChange()
        End Set
    End Property

    <Browsable(False)> _
    Public ReadOnly Property CountryName() As String
        Get
            Return cboCountry.ItemCaption
        End Get
    End Property

    <Browsable(True)> _
    Public Property IsCountryRequired() As Integer
        Get
            Return m_iIsCountryRequired
        End Get
        Set(ByVal Value As Integer)
            m_iIsCountryRequired = Value
            RaiseEvent IsCountryRequiredChange()
            uctPMAddressControl_Resize(Me, New EventArgs())
        End Set
    End Property

    <Browsable(True)> _
    Public Property IsPostCodeRequired() As Integer
        Get
            Return m_iIsPostCodeRequired
        End Get
        Set(ByVal Value As Integer)
            m_iIsPostCodeRequired = Value
            RaiseEvent IsPostCodeRequiredChange()
            uctPMAddressControl_Resize(Me, New EventArgs())
        End Set
    End Property

    'DJM 08/05/2002 : Allow Disabling of control for read only modes
    <Browsable(True)> _
    Public Shadows Property Enabled() As Boolean
        Get
            Return fraAddress.Enabled
        End Get
        Set(ByVal Value As Boolean)
            fraAddress.Enabled = Value
        End Set
    End Property

    ' RDC 11072002 Warnng message may returned from the search - not an error

    <Browsable(True)> _
    Public Property WarningMessage() As String
        Get
            Return m_sWarningMessage
        End Get
        Set(ByVal Value As String)
            m_sWarningMessage = Value
        End Set
    End Property

    <Browsable(False)> _
    Public WriteOnly Property SetMandatoryFlagAddressLine1() As Boolean
        Set(ByVal Value As Boolean)
            lblAddress1.Font = VB6.FontChangeBold(lblAddress1.Font, Value)
        End Set
    End Property

    ' ***************************************************************** '
    ' Name: UpdateFaceFont
    '
    ' Description: Sets the fonts on the form to m_fFaceFont
    '
    ' ***************************************************************** '
    Private Sub UpdateFaceFont()


        Try

            ' Set fonts on form
            ' developer guide no. Mybase is changed to Mybase.controls collection
            For Each Control As Object In MyBase.Controls

                If (Control.Name <> "cmdSearch") And (Control.Name <> "cmdDelete") Then
                    'SJ 20/08/2004 - start
                    'Set Control.Font = m_fFaceFont

                    'Control.Font.Name = m_fFaceFont.Name
                    Control.Font = m_fFaceFont
                    'SJ 20/08/2004 - end
                End If
            Next Control

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateFaceFont Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFaceFont", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: SearchDatabase (Public)
    '
    ' Description: Process Address Search via business object.
    '
    ' ***************************************************************** '
    Public Function SearchDatabase() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sPostCode, sAddress1, sAddress2, sAddress3, sAddress4 As String
        Dim lPMAddressCnt As Integer
        Dim vAddressArray, vPickList As Object
        Dim lSelectedItem As Integer
        'DAK140700
        Dim iCountryId As Integer
        Dim sCountryCode As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set properties on Business object
            With m_oBusiness

                .PMDatabaseID = PMDatabaseID

                .QASDatabaseID = QASDatabaseID

                .QAS2PMAddress1 = QAS2PMAddress1

                .QAS2PMAddress2 = QAS2PMAddress2

                .QAS2PMAddress3 = QAS2PMAddress3

                .QAS2PMAddress4 = QAS2PMAddress4
            End With
            If Information.IsArray(m_vCountryConfig) Then
                For iCountry As Integer = 0 To m_vCountryConfig.GetUpperBound(1)
                    If CDbl(m_vCountryConfig(0, iCountry)) = cboCountry.ItemId Then
                        sCountryCode = CStr(m_vCountryConfig(1, iCountry)).Trim()
                        Exit For
                    End If
                Next iCountry
            Else
                sCountryCode = ACUKCountryCode
            End If

            sPostCode = PostCode
            sAddress1 = AddressLine1
            sAddress2 = AddressLine2
            sAddress3 = AddressLine3
            sAddress4 = AddressLine4
            lPMAddressCnt = PMAddressCnt

            iCountryId = CountryId


            m_lReturn = m_oBusiness.FindAddress(r_vAddressArray:=vAddressArray, r_vPickList:=vPickList, r_sPostCode:=sPostCode, r_sAddressLine1:=sAddress1, r_sAddressLine2:=sAddress2, r_sAddressLine3:=sAddress3, r_sAddressLine4:=sAddress4, r_lPMAddressCnt:=lPMAddressCnt, r_iCountryId:=iCountryId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to m_oBusiness.FindAddress", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchDatabase", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' RDC 11072002 new property indicates if search produced a warning or not

            WarningMessage = m_oBusiness.WarningMessage

            If Information.IsArray(vPickList) Then

                If vPickList.GetUpperBound(1) = 1 Then
                    lSelectedItem = 1
                ElseIf (vPickList.GetUpperBound(1) > 1) Then

                    m_lReturn = DisplayPickList(v_vPickList:=vPickList, r_lSelectedItem:=lSelectedItem)
                Else
                    lSelectedItem = 0
                End If
            Else
                ' CF290499
                MessageBox.Show("No match found.", "PMFind Address")
                Return result
            End If

            ' RDT071298
            ' Added further condition to trap if the user may have
            ' selected Cancel from frmPickList.
            ' If the SelectItem < 1 And there are no results in vPickList then

            If (lSelectedItem < 1) And (vPickList.GetUpperBound(1) < 1) Then
                MessageBox.Show("No match found.", "PMFind Address")
                Return result
                ' ElseIf no item has been selected but there ARE results in vPickList then
                ' the user MUST have selected Cancel from frmPickList.
            ElseIf (lSelectedItem < 1) And (vPickList.GetUpperBound(1) > 1) Then
                Return result
            End If

            'DAK140700 - add iCountryId
            'PSL 10/10/02 Issue 999 removed extra r_iCountryId parameter

            m_lReturn = m_oBusiness.GetDetails(v_lPickListNodeID:=lSelectedItem, r_sPostCode:=sPostCode, r_sAddressLine1:=sAddress1, r_sAddressLine2:=sAddress2, r_sAddressLine3:=sAddress3, r_sAddressLine4:=sAddress4, r_lPMAddressCnt:=lPMAddressCnt, r_sTitle:=m_sTitle, r_sForename:=m_sForename, r_sInitial:=m_sInitial, r_sSurname:=m_sSurname, r_sOrganisation:=m_sOrgname, r_iCountryId:=iCountryId)

            ' Assign returned values to controls
            PostCode = sPostCode
            AddressLine1 = sAddress1
            AddressLine2 = sAddress2
            AddressLine3 = sAddress3
            AddressLine4 = sAddress4
            PMAddressCnt = lPMAddressCnt
            'DAK140700
            CountryId = iCountryId

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchDatabase Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchDatabase", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayPickList (Private)
    '
    ' Description: Displays PickList array in Tree View
    '
    ' ***************************************************************** '
    Private Function DisplayPickList(ByVal v_vPickList(,) As Object, ByRef r_lSelectedItem As Integer) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sRelative, sKey, sCaption As String

        'developer guide no.95
        Dim lRelation As mscomctl.TreeRelationshipConstants
        Dim oNode As TreeNode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' RDT071298
            ' The following few changes Ensure that the form is loaded
            ' and destroyed neatly.  Unexpected behaviour was being experienced.
            ' When frmPickList was being shown and the user selected Cancel
            ' the next time they click Search, the form did not show.

            ' Load the form neatly
            ' developer guide no. New added for the form instance
            Dim tempLoadForm As frmPickList = New frmPickList

            'developer guide no. form 'frmPickList' is replaced with object instance of the form 'tempLoadForm'
            tempLoadForm.trvPickList.Nodes.Clear()

            For lCount As Integer = 1 To v_vPickList.GetUpperBound(1)

                sRelative = "N" & CStr(v_vPickList(3, lCount))

                sKey = "N" & CStr(v_vPickList(0, lCount))

                sCaption = CStr(v_vPickList(2, lCount))
                If sRelative <> "N0" Then

                    'developer guide no.95
                    lRelation = mscomctl.TreeRelationshipConstants.tvwChild

                    'developer guide no.95
                    oNode = tempLoadForm.trvPickList.Nodes.Add(sRelative, lRelation.ToString(), sKey, sCaption)
                    'developer guide no. form 'frmPickList' is replaced with object instance of the form 'tempLoadForm'
                    tempLoadForm.trvPickList.Nodes.Item(lCount - 1).ImageIndex = 1
                Else
                    sRelative = ""

                    'developer guide no.95
                    lRelation = mscomctl.TreeRelationshipConstants.tvwFirst
                    'developer guide no. form 'frmPickList' is replaced with object instance of the form 'tempLoadForm'
                    oNode = tempLoadForm.trvPickList.Nodes.Add(sKey, sCaption)
                    'developer guide no. form 'frmPickList' is replaced with object instance of the form 'tempLoadForm'
                    tempLoadForm.trvPickList.Nodes.Item(lCount - 1).ImageIndex = 0
                End If
            Next lCount

            If QASDatabaseID = 3 Then
                For lCount As Integer = 1 To v_vPickList.GetUpperBound(1)
                    'developer guide no. form 'frmPickList' is replaced with object instance of the form 'tempLoadForm'
                    If tempLoadForm.trvPickList.Nodes.Item(lCount - 1).GetNodeCount(False) = 0 Then
                        'developer guide no. form 'frmPickList' is replaced with object instance of the form 'tempLoadForm'
                        tempLoadForm.trvPickList.Nodes.Item(lCount - 1).ImageIndex = 2
                    End If
                Next lCount
            End If
            'developer guide no. form 'frmPickList' is replaced with object instance of the form 'tempLoadForm'
            m_lReturn = iPMFunc.SetWindowPlacement(tempLoadForm.Handle.ToInt32(), True)

            'developer guide no. form 'frmPickList' is replaced with object instance of the form 'tempLoadForm'
            tempLoadForm.ShowDialog()

            'developer guide no. form 'frmPickList' is replaced with object instance of the form 'tempLoadForm'
            r_lSelectedItem = tempLoadForm.SelectedNodeID

            ' Unload the form from memory.
            'form 'frmPickList' is replaced with object instance of the form 'tempLoadForm'
            tempLoadForm.Close()

            Return result

        Catch



            Return result
        End Try

    End Function

    'UPGRADE_NOTE: (7001) The following declaration (cboCountry_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    Private Sub cboCountry_Click() Handles cboCountry.Click

        Try

            m_bCountryComboClicked = True

            RefreshCountry()

        Catch excep As System.Exception



            'Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "cboCountry_Click Failed", ACApp, ACClass, "cboCountry_Click", Information.Err().Number, excep.Message, excep:=excep)

        End Try

    End Sub

    Private Sub cboState_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboState.Click

        ' Don't display "(none)"
        If cboState.ListIndex > 0 Then
            lblStateDesc.Text = cboState.ItemCaption
            txtAddress4.Text = cboState.ItemCaption
        Else
            If cboState.Visible Then
                lblStateDesc.Text = ""
                txtAddress4.Text = ""
            End If
        End If
    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click

        ' Clear all text boxes
        txtAddress1.Text = ""
        txtAddress2.Text = ""
        txtAddress3.Text = ""
        txtAddress4.Text = ""
        txtPostCode.Text = ""
        PMAddressCnt = 0

        cboCountry.ItemId = g_iCountryId

        ' Set focus to postcode text box
        If txtPostCode.Visible And txtPostCode.Enabled Then
            txtPostCode.Focus()
        End If

        'PN:45199 raise event to inform container of the control
        RaiseEvent AddressCleared(Me, Nothing)

    End Sub

    Private Sub cmdSearch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSearch.Click

        ' Set up business object etc.
        m_lReturn = Initialise()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the business object.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdSearch_Click")
            Exit Sub
        End If

        ' Process Search
        m_lReturn = SearchDatabase()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to search the database.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdSearch_Click")
            Exit Sub
        Else
            ' PKH(CMG)25/07/02 - Added for QASNames Integration
            RaiseEvent ChosenAddress(Me, Nothing)
        End If
        MyBase.ParentForm.Focus()
    End Sub

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Initialise"
        Dim lReturn As gPMConstants.PMEReturnCode

        Static bIsInitialised As Boolean

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if already initialised
            If bIsInitialised Then
                Return result
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.Initialise", "Unable to initialise the object manager")
            End If

            ' If UserID is 0 assume that user cancelled logon
            If g_oObjectManager.UserID = 0 Then
                ' Exit application
                result = gPMConstants.PMEReturnCode.PMFalse
                g_oObjectManager = Nothing
            End If

            ' Store the language ID from the object manager to the public
            ' variables, to enable us to use them throughout the object.
            g_iLanguageID = g_oObjectManager.LanguageID
            g_iSourceID = g_oObjectManager.SourceID
            g_iCountryId = g_oObjectManager.CountryID

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bPMAddressControl.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "Failed to get instance of bPMAddressControl.Business")
            End If


            lReturn = m_oBusiness.GetCountryCodes(m_vCountryConfig)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.GetCountryCodes", "Failed to get country configurations")
            End If

            ' Refresh the default country
            lReturn = CType(RefreshCountry(), gPMConstants.PMEReturnCode)

            ' hold Initialised status
            bIsInitialised = True

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            g_oObjectManager = Nothing

        Finally
            ' Do any tidy up, e.g. Set x = Nothing here
            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
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
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
                If m_oBusiness IsNot Nothing Then
                    m_oBusiness.Dispose()
                    m_oBusiness = Nothing
                End If
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub



    Private Sub UserControl_InitProperties()

        'developer guide no solution 2
        m_fSearchFont = Me.Font

        'developer guide no solution 2
        m_fFaceFont = Me.Font

        'developer guide no solution 2
        m_fClearFont = Me.Font

        IsPostCodeRequired = CheckState.Checked
        IsCountryRequired = CheckState.Unchecked
        'developer guide no. 220
        'start'
        Me.cboCountry.FirstItem = ""
        Me.cboState.FirstItem = "(None)"
        'end'
        ' Set focus to 1st text box
        txtPostCode.Focus()
    End Sub

    Private Sub uctPMAddressControl_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize

        ' Maintain minimum width
        If VB6.PixelsToTwipsX(Width) < 4320 Then
            Width = VB6.TwipsToPixelsX(4320)
        End If

        If IsPostCodeRequired = CheckState.Unchecked Then
            lblPostCode.Enabled = False
            lblPostCode.Visible = False
            txtPostCode.Enabled = False
            txtPostCode.Visible = False
        Else
            lblPostCode.Enabled = True
            lblPostCode.Visible = True
            txtPostCode.Enabled = True
            txtPostCode.Visible = True
        End If

        If IsCountryRequired = CheckState.Unchecked Then
            lblCountry.Visible = False
            lblCountry.Enabled = False
            cboCountry.Visible = False
            cboCountry.Enabled = False
            Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(txtPostCode.Top) + VB6.PixelsToTwipsY(txtPostCode.Height) + 120)
        Else
            lblCountry.Visible = True
            lblCountry.Enabled = True
            cboCountry.Visible = True
            cboCountry.Enabled = True
            'developer guide no. 74(Guide)
        End If
        'developer guide no. 74(Guide)
        fraAddress.Top = 0
        fraAddress.Left = 0
        fraAddress.Width = ClientRectangle.Width
        fraAddress.Height = ClientRectangle.Height

        'cmdDelete.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(fraAddress.Width) - VB6.PixelsToTwipsX(cmdDelete.Width) - 180)

        txtAddress1.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(fraAddress.Width) - VB6.PixelsToTwipsX(txtAddress1.Left) - 180)
        txtAddress2.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(fraAddress.Width) - VB6.PixelsToTwipsX(txtAddress2.Left) - 180)
        txtAddress3.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(fraAddress.Width) - VB6.PixelsToTwipsX(txtAddress3.Left) - 180)
        txtAddress4.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(fraAddress.Width) - VB6.PixelsToTwipsX(txtAddress4.Left) - 180)
        cboCountry.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(fraAddress.Width) - VB6.PixelsToTwipsX(cboCountry.Left) - 165)

        cboState.Width = VB6.TwipsToPixelsX((VB6.PixelsToTwipsX(fraAddress.Width) - VB6.PixelsToTwipsX(txtAddress4.Left) - 180) / 2 - 15)
        lblStateDesc.Width = VB6.TwipsToPixelsX((VB6.PixelsToTwipsX(fraAddress.Width) - VB6.PixelsToTwipsX(txtAddress4.Left) - 180) / 2 - 30)
        lblStateDesc.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cboState.Left) + VB6.PixelsToTwipsX(cboState.Width) + 45)

        If VB6.PixelsToTwipsX(txtPostCode.Left) > VB6.PixelsToTwipsX(cmdSearch.Left) Then
            txtPostCode.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(txtAddress4.Width) - VB6.PixelsToTwipsX(cmdDelete.Width) - 80)
        Else
            txtPostCode.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(txtAddress4.Width) - VB6.PixelsToTwipsX(cmdDelete.Width) - VB6.PixelsToTwipsX(cmdSearch.Width) - 140)
        End If

        '' If theres no post code caption, then we want the post code button to the left
        'If Strings.Len(lblPostCode.Text) = 0 Then
        '	cmdSearch.Left = lblPostCode.Left
        'Else
        '	cmdSearch.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdDelete.Left) - VB6.PixelsToTwipsX(cmdSearch.Width) - 80)
        'End If

    End Sub

    Private Sub UserControl_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Try
            'Commented the code and moved to UserControl_initProperties so that the combobox properties can be loaded once we initialte the object
            ''developer guide no. 220
            ''start'
            'Me.cboCountry.FirstItem = ""
            'Me.cboState.FirstItem = "(None)"
            ''end'
            '' Set focus to 1st text box
            'txtPostCode.Focus()

        Catch
        End Try

    End Sub

    'Load property values from storage


    'developer no solution. 1

    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)



        fraAddress.Text = CStr(PropBag.ReadProperty("Caption", ""))



        txtPostCode.Text = CStr(PropBag.ReadProperty("PostCode", ""))


        txtAddress1.Text = CStr(PropBag.ReadProperty("AddressLine1", ""))


        txtAddress2.Text = CStr(PropBag.ReadProperty("AddressLine2", ""))


        txtAddress3.Text = CStr(PropBag.ReadProperty("AddressLine3", ""))


        txtAddress4.Text = CStr(PropBag.ReadProperty("AddressLine4", ""))



        cboCountry.ItemId = CInt(PropBag.ReadProperty("CountryId", g_iCountryId))


        lblPostCode.Text = CStr(PropBag.ReadProperty("CaptionPostCode", "Post Code:"))


        lblAddress1.Text = CStr(PropBag.ReadProperty("CaptionAddress1", "No./Name Street:"))


        lblAddress2.Text = CStr(PropBag.ReadProperty("CaptionAddress2", "Locality:"))


        lblAddress3.Text = CStr(PropBag.ReadProperty("CaptionAddress3", "Post Town:"))


        lblAddress4.Text = CStr(PropBag.ReadProperty("CaptionAddress4", "County:"))



        lblCountry.Text = CStr(PropBag.ReadProperty("CaptionCountry", "Country:"))


        m_sOrganisation = CStr(PropBag.ReadProperty("Organisation", ""))


        m_lPMDatabaseID = CInt(PropBag.ReadProperty("PMDatabaseID", 0))


        m_lQASDatabaseID = CInt(PropBag.ReadProperty("QASDatabaseID", 0))


        m_lPMAddressCnt = CInt(PropBag.ReadProperty("PMAddressCnt", 0))



        m_sQAS2PMAddress1 = CStr(PropBag.ReadProperty("QAS2PMAddress1", "12"))


        m_sQAS2PMAddress2 = CStr(PropBag.ReadProperty("QAS2PMAddress2", "9"))


        m_sQAS2PMAddress3 = CStr(PropBag.ReadProperty("QAS2PMAddress3", "8"))


        m_sQAS2PMAddress4 = CStr(PropBag.ReadProperty("QAS2PMAddress4", "10"))



        m_iIsCountryRequired = CInt(PropBag.ReadProperty("IsCountryRequired", CheckState.Unchecked))


        m_iIsPostCodeRequired = CInt(PropBag.ReadProperty("IsPostCodeRequired", CheckState.Checked))



        'developer guide no.2 
        m_fFaceFont = PropBag.ReadProperty("FaceFont", Me.Font)
        cmdSearch.Font = m_fFaceFont
        UpdateFaceFont()

        'cmdSearch.Top = PropBag.ReadProperty("SearchButtonTop", 1380)



        cmdSearch.Left = VB6.TwipsToPixelsX(CDbl(PropBag.ReadProperty("SearchButtonLeft", 4380)))



        cmdSearch.Height = VB6.TwipsToPixelsY(CDbl(PropBag.ReadProperty("SearchButtonHeight", 285)))



        cmdSearch.Width = VB6.TwipsToPixelsX(CDbl(PropBag.ReadProperty("SearchButtonWidth", 360)))


        cmdSearch.Text = CStr(PropBag.ReadProperty("SearchButtonCaption", ".."))


        'developer guide no solution 2
        m_fSearchFont = PropBag.ReadProperty("SearchButtonFont", Me.Font)
        cmdSearch.Font = m_fSearchFont



        cmdDelete.Text = CStr(PropBag.ReadProperty("ClearButtonCaption", "X"))



        cmdDelete.Left = VB6.TwipsToPixelsX(CDbl(PropBag.ReadProperty("ClearButtonLeft", 4800)))



        cmdDelete.Width = VB6.TwipsToPixelsX(CDbl(PropBag.ReadProperty("ClearButtonWidth", 360)))


        'developer guide no solution 2
        m_fClearFont = PropBag.ReadProperty("ClearButtonFont", Me.Font)
        cmdDelete.Font = m_fClearFont

    End Sub

    'Write property values to storage


    'developer guide no solution 1
    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)


        PropBag.WriteProperty("Caption", fraAddress.Text, "")


        PropBag.WriteProperty("PostCode", txtPostCode.Text, "")

        PropBag.WriteProperty("AddressLine1", txtAddress1.Text, "")

        PropBag.WriteProperty("AddressLine2", txtAddress2.Text, "")

        PropBag.WriteProperty("AddressLine3", txtAddress3.Text, "")

        PropBag.WriteProperty("AddressLine4", txtAddress4.Text, "")


        PropBag.WriteProperty("CountryId", cboCountry.ItemId, g_iCountryId)

        PropBag.WriteProperty("CaptionPostCode", lblPostCode.Text, "Post Code:")

        PropBag.WriteProperty("CaptionAddress1", lblAddress1.Text, "No./Name Street:")

        PropBag.WriteProperty("CaptionAddress2", lblAddress2.Text, "Locality:")

        PropBag.WriteProperty("CaptionAddress3", lblAddress3.Text, "Post Town:")

        PropBag.WriteProperty("CaptionAddress4", lblAddress4.Text, "County:")


        PropBag.WriteProperty("CaptionCountry", lblCountry.Text, "Country:")

        PropBag.WriteProperty("Organisation", m_sOrganisation, "")

        PropBag.WriteProperty("PMDatabaseID", m_lPMDatabaseID, 0)

        PropBag.WriteProperty("QASDatabaseID", m_lQASDatabaseID, 0)

        PropBag.WriteProperty("PMAddressCnt", m_lPMAddressCnt, 0)


        PropBag.WriteProperty("QAS2PMAddress1", m_sQAS2PMAddress1, "12")

        PropBag.WriteProperty("QAS2PMAddress2", m_sQAS2PMAddress2, "9")

        PropBag.WriteProperty("QAS2PMAddress3", m_sQAS2PMAddress3, "8")

        PropBag.WriteProperty("QAS2PMAddress4", m_sQAS2PMAddress4, "10")


        PropBag.WriteProperty("IsCountryRequired", m_iIsCountryRequired, CheckState.Unchecked)

        PropBag.WriteProperty("IsPostCodeRequired", m_iIsPostCodeRequired, CheckState.Checked)



        'developer guide no solution 2
        PropBag.WriteProperty("FaceFont", m_fFaceFont, Me.Font)

        'Call PropBag.WriteProperty("SearchButtonTop", cmdSearch.Top, 1500)

        PropBag.WriteProperty("SearchButtonLeft", VB6.PixelsToTwipsX(cmdSearch.Left), 4380)

        PropBag.WriteProperty("SearchButtonHeight", VB6.PixelsToTwipsY(cmdSearch.Height), 285)

        PropBag.WriteProperty("SearchButtonWidth", VB6.PixelsToTwipsX(cmdSearch.Width), 360)

        PropBag.WriteProperty("SearchButtonCaption", cmdSearch.Text, "..")


        'developer guide no solution 2
        PropBag.WriteProperty("SearchButtonFont", m_fSearchFont, Me.Font)


        PropBag.WriteProperty("ClearButtonCaption", cmdDelete.Text, "X")

        PropBag.WriteProperty("ClearButtonLeft", VB6.PixelsToTwipsX(cmdDelete.Left), 4800)

        PropBag.WriteProperty("ClearButtonWidth", VB6.PixelsToTwipsX(cmdDelete.Width), 360)


        'developer guide no solution 2
        PropBag.WriteProperty("ClearButtonFont", m_fClearFont, Me.Font)

    End Sub

    ' ***************************************************************** '
    ' Refresh the address layout based on the configuration of the
    ' selected country
    ' ***************************************************************** '
    Private Function RefreshCountry() As Integer

        Dim result As Integer = 0
        Dim sState As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions
            If Information.IsArray(m_vCountryConfig) Then
                ' Read captions configured on the array
                For lCountry As Integer = m_vCountryConfig.GetLowerBound(1) To m_vCountryConfig.GetUpperBound(1)
                    If CDbl(m_vCountryConfig(ACCCountryID, lCountry)) = cboCountry.ItemId Then
                        ' Set address line captions
                        CaptionAddress1 = CStr(m_vCountryConfig(ACCAddressLine1Caption, lCountry))
                        CaptionAddress2 = CStr(m_vCountryConfig(ACCAddressLine2Caption, lCountry))
                        CaptionAddress3 = CStr(m_vCountryConfig(ACCAddressLine3Caption, lCountry))
                        CaptionAddress4 = CStr(m_vCountryConfig(ACCAddressLine4Caption, lCountry))
                        CaptionPostCode = CStr(m_vCountryConfig(ACCPostcodeCaption, lCountry))

                        ' If we are using the state lookup refresh the list
                        If gPMFunctions.ToSafeBoolean(CStr(m_vCountryConfig(ACCIsStateLookup, lCountry))) Then
                            ' Save current value
                            sState = txtAddress4.Text

                            ' Limit states to the ones available for the current country
                            cboState.WhereClause = "is_deleted = 0 AND country_id = " & cboCountry.ItemId
                            cboState.RefreshList()

                            ' Check if we have any states
                            If cboState.ListCount = 1 Then
                                ' We don't so turn off the state lookup for this country
                                m_vCountryConfig(ACCIsStateLookup, lCountry) = False
                                txtAddress4.Text = sState
                            Else
                                m_lReturn = SelectComboItem(r_ctl:=cboState, v_sState:=sState)
                                lblStateDesc.Text = sState
                                txtAddress4.Text = sState
                            End If
                        End If

                        ' If we are still using the state lookup (i.e. we have states) display it
                        If gPMFunctions.ToSafeBoolean(CStr(m_vCountryConfig(ACCIsStateLookup, lCountry))) Then
                            lblStateDesc.Visible = True
                            cboState.Visible = True
                            txtAddress4.Visible = False
                        Else
                            lblStateDesc.Visible = False
                            cboState.Visible = False
                            txtAddress4.Visible = True
                            cboState.ItemId = 0
                        End If

                        ' Check postcode usage
                        If Strings.Len(CStr(m_vCountryConfig(ACCPostcodeVisibility, lCountry))) = 0 Then
                            ' Not yet set continue with original usage
                            Select Case CStr(m_vCountryConfig(ACCISOCode, lCountry)).Trim()
                                Case ACUKCountryCode, ACUSCountryCode
                                    IsPostCodeRequired = CheckState.Checked
                                Case Else
                                    IsPostCodeRequired = CheckState.Unchecked
                            End Select
                        Else
                            ' Configured on the country so map it to appropriate value
                            Select Case m_vCountryConfig(ACCPostcodeVisibility, lCountry)
                                Case 3 ' Postcode visible and mandatory
                                    IsPostCodeRequired = CheckState.Checked
                                Case 2 ' Postcode visible but optional
                                    IsPostCodeRequired = CheckState.Indeterminate
                                Case Else ' Postcode hidden
                                    IsPostCodeRequired = CheckState.Unchecked
                            End Select
                        End If

                        Exit For
                    End If
                Next lCountry
            End If

            ' Set standard country caption

            CaptionCountry = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionCountry, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RefreshCountry Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshCountry", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function


    Private Function SelectComboItem(ByRef r_ctl As PMLookupControl.cboPMLookup, ByVal v_sState As String) As Integer

        Dim result As Integer = 0
        Dim lItemID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set it to = 0 to not selected till we find the right one
            r_ctl.ItemId = 0

            If Not (v_sState Is Nothing) Then
                For lCount As Integer = 0 To r_ctl.ListCount - 1
                    lItemID = r_ctl.ItemData(lCount)
                    If r_ctl.ItemCaption(lItemID).ToUpper() = v_sState.ToUpper() Then
                        r_ctl.ItemId = lItemID
                    End If
                Next
            End If
            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the combo item", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectComboItem", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
End Class
