Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 02/07/1998
    '
    ' Description: Main interface.
    '
    ' Edit History:
    '   SP161198  - Update Address User control (uses QAS now) and remove
    '   terminate call which was a temporary work around an old bug.
    '   CJB280605 - PN20336 General tidy up of tabbing and related interface issues.
    ' ***************************************************************** '
    'DEEPAK_COMMENT: Replaced iPMFunc.GetResData with GetResData in the whole document

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    'developer guide no. 7
    Private Const vbFormCode As Integer = 0

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Status members
    Private m_sProcessStatus As New FixedLengthString(2)
    Private m_sMapStatus As New FixedLengthString(2)
    Private m_sStepStatus As New FixedLengthString(2)

    ' {* USER DEFINED CODE (Begin) *}

    ' Form Constants for Captions

    Const ACInterfaceCaption As Integer = 100
    Const ACMainTabTitle0 As Integer = 101
    Const ACMainTabTitle1 As Integer = 102
    Const ACConPostCodeCaption As Integer = 103
    Const ACConReferenceCaption As Integer = 104
    Const AClbAdReferenceCaption As Integer = 105
    Const ACAdPostcodeCaption As Integer = 106

    'sj 23/07/2002 - start
    Const ACCurrentAddressCaption As Integer = 108
    Const ACFutureDatedAddressChangeCaption As Integer = 109
    Const ACEffectiveDateCaption As Integer = 110
    Const ACClearFutureAddressCaption As Integer = 111
    'sj 23/07/2002 - end

    ' Button Constants for Captions

    Const ACHelpCaption As Integer = 200
    Const ACCancelCaption As Integer = 201
    Const ACOKCaption As Integer = 202
    Const ACNextCaption0 As Integer = 203
    Const ACEditConCaption As Integer = 204
    Const ACDeleteConCaption As Integer = 205
    Const ACAddConCaption As Integer = 206


    ' Message Constants for Captions


    ' Constants for internal return flags
    Const ACAddressAdd As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue
    Const ACAddressDoNotAdd As gPMConstants.PMEReturnCode = 2
    Const ACAddressCancel As gPMConstants.PMEReturnCode = 3
    Const ACAddressEdit As gPMConstants.PMEReturnCode = 4

    Private m_lAddressCnt As Integer
    Private m_sPostalCode As String = ""
    Private m_sAddress1 As String = ""
    Private m_sAddress2 As String = ""
    Private m_sAddress3 As String = ""
    Private m_sAddress4 As String = ""
    Private m_vContacts(,) As Object
    'MS210501 Added Country ID
    Private m_lCountryID As Integer

    ' CF30499 Address Usage Type
    Private m_lAddressUsageTypeID As Integer
    Private m_sAddressUsageType As String = ""

    ' Declare an instance of the contact interface.
    Private m_oContact As iPMBContact.Interface_Renamed

    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMBAddress.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    'DC120706 PN29319 check if QAS installed before validating GBR postcode
    Private m_sQASInstalled As String = ""

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control
    'sj 23/07/2002 - start
    Private m_sCurrentPostalCode As String = ""
    Private m_sCurrentAddress1 As String = ""
    Private m_sCurrentAddress2 As String = ""
    Private m_sCurrentAddress3 As String = ""
    Private m_sCurrentAddress4 As String = ""
    Private m_lCurrentCountryId As Integer

    Private m_sFuturePostalCode As String = ""
    Private m_sFutureAddress1 As String = ""
    Private m_sFutureAddress2 As String = ""
    Private m_sFutureAddress3 As String = ""
    Private m_sFutureAddress4 As String = ""
    Private m_lFutureCountryId As Integer

    Private m_dtFutureEffectiveDate As Date

    Private m_iFutureAddressTask As gPMConstants.PMEComponentAction
    Private m_lCorrespondanceAddressUsageTypeId As Integer
    ' FutureDateAddressChanges
    Private m_bFutureDateAddressChanges As Boolean
    ' FutureDatedAddresses
    Private m_vFutureDatedAddresses(,) As Object
    ' CorrespondanceAddressExists
    Private m_bCorrespondanceAddressExists As Boolean
    'sj 23/07/2002 - end

    'sj 12/06/2002 - start
    ' IsNRMA
    Private m_bIsNRMA As Boolean
    Private m_lPostCodeVisiblilityId As Integer
    'PN 64670
    Private m_iSourceId As Integer
    Private m_sUniqueId As String = ""
    Private m_sScreenHeirarchy As String = ""


    Public Property SourceID() As Integer
        Get
            Return m_iSourceId
        End Get
        Set(ByVal Value As Integer)
            m_iSourceId = Value
        End Set
    End Property
    'PN 64670

    Public WriteOnly Property IsNRMA() As Boolean
        Set(ByVal Value As Boolean)
            m_bIsNRMA = Value
        End Set
    End Property
    'sj 12/06/2002 - end

    'sj 23/07/2002 - start
    'Public Property Get UpdateFutureDatedAddress() As Boolean
    '    UpdateFutureDatedAddress = m_bUpdateFutureDatedAddress
    'End Property
    Public WriteOnly Property FutureDateAddressChanges() As Boolean
        Set(ByVal Value As Boolean)
            m_bFutureDateAddressChanges = Value
        End Set
    End Property
    'developer guide no. 17
    Public Property FutureDatedAddresses() As Object
        Get
            'Return VB6.CopyArray(m_vFutureDatedAddresses)
            Return m_vFutureDatedAddresses
        End Get
        Set(ByVal Value As Object)
            m_vFutureDatedAddresses = Value
        End Set
    End Property
    Public WriteOnly Property CorrespondanceAddressExists() As Boolean
        Set(ByVal Value As Boolean)
            m_bCorrespondanceAddressExists = Value
        End Set
    End Property

    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)


    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Standard Property.

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property




    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)

    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub Status(ByVal Value As Integer)
    '
    ' Standard Property.
    '
    ' Set the interface exit status.
    'm_lStatus = Value
    '
    'End Sub
    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    Public Property Task() As Integer
        Get

            ' Return the objects task.
            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the objects task.
            m_iTask = Value

        End Set
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the navigate flag.
            m_lNavigate = Value

        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the process mode.
            m_lProcessMode = Value

        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the type of business.
            m_sTransactionType = Value

        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            ' Standard Property.

            ' Set the effective date.
            m_dtEffectiveDate = Value

        End Set
    End Property

    Public ReadOnly Property StepStatus() As String
        Get

            ' Standard Property.

            ' Return the Steps Status
            Return m_sStepStatus.Value

        End Get
    End Property

    ' {* USER DEFINED CODE (Begin) *}
    Public Property AddressCnt() As Integer
        Get

            ' Return the objects parameter value.
            Return m_lAddressCnt

        End Get
        Set(ByVal Value As Integer)

            ' Set the objects parameter value.
            m_lAddressCnt = Value

        End Set
    End Property
    Public ReadOnly Property Address1() As String
        Get

            ' Return the objects parameter value.
            Return m_sAddress1

        End Get
    End Property
    Public ReadOnly Property Address2() As String
        Get

            ' Return the objects parameter value.
            Return m_sAddress2

        End Get
    End Property
    Public ReadOnly Property Address3() As String
        Get

            ' Return the objects parameter value.
            Return m_sAddress3

        End Get
    End Property
    Public ReadOnly Property Address4() As String
        Get

            ' Return the objects parameter value.
            Return m_sAddress4

        End Get
    End Property
    Public ReadOnly Property PostalCode() As String
        Get

            ' Return the objects parameter value.
            Return m_sPostalCode

        End Get
    End Property
    Public Property CountryID() As Integer
        Get

            ' MS210501 Added property
            Return m_lCountryID

        End Get
        Set(ByVal Value As Integer)
            uctadd.CountryId = Value
        End Set
    End Property
    ' CF30499 - Address Usage Type
    Public Property AddressUsageTypeID() As Integer
        Get
            Return m_lAddressUsageTypeID
        End Get
        Set(ByVal Value As Integer)
            m_lAddressUsageTypeID = Value
        End Set
    End Property

    Public Property AddressUsageType() As String
        Get
            Return m_sAddressUsageType
        End Get
        Set(ByVal Value As String)
            m_sAddressUsageType = Value
        End Set
    End Property

    Public Property UniqueId() As String
        Get
            Return m_sUniqueId
        End Get
        Set(ByVal Value As String)
            m_sUniqueId = Value
        End Set
    End Property

    Public Property ScreenHeirarchy() As String
        Get
            Return m_sScreenHeirarchy
        End Get
        Set(ByVal Value As String)
            m_sScreenHeirarchy = Value
        End Set
    End Property
    'sj 23/07/2002 - end


    ' ***************************************************************** '
    ' Name: PopulateContacts
    '
    ' Description: Fills the grid control with contact details
    '
    ' ***************************************************************** '
    Private Sub PopulateContacts()

        Dim oListItem As ListViewItem


        Try

            If Not Information.IsArray(m_vContacts) Then
                Exit Sub
            End If
            lvwContacts.Items.Clear()


            ' Assign the details to the interface.
            For i As Integer = m_vContacts.GetLowerBound(1) To m_vContacts.GetUpperBound(1)

                ' {* USER DEFINED CODE (Begin) *}

                ' Assign the details to the first column.
                ' Column 1
                oListItem = lvwContacts.Items.Add(CStr(m_vContacts(1, i)).Trim())

                ' Assign details to other the columns
                ' Column 2
                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vContacts(2, i)).Trim()

                ' Column 3
                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vContacts(3, i)).Trim()

                ' Column 4
                ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vContacts(4, i)).Trim()

                ' Column 5
                ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vContacts(5, i)).Trim()


                ' Store the Contact_cnt
                oListItem.Tag = CStr(m_vContacts(0, i)).Trim()
                ' {* USER DEFINED CODE (End) *}

                ' Set the tag property with the index of
                ' the search data storage.

            Next i
            '    'Populate the cells

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateContacts", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)
    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtEffectiveDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '    m_lReturn = m_oFormFields.AddNewFormField( _
            ''                        ctlControl:=uctcontact, _
            ''                        lGridColumn:=1, _
            ''                        lFieldType:=PMString, _
            ''                        lFormat:=PMFormatString, _
            ''                        lMandatory:=PMNonMandatory)
            '
            '    If m_lReturn <> PMTrue Then
            '      SetFieldValidation = PMFalse
            '      Exit Function
            '    End If
            '
            '    m_lReturn = m_oFormFields.AddNewFormField( _
            ''                        ctlControl:=uctcontact, _
            ''                        lGridColumn:=2, _
            ''                        lFieldType:=PMString, _
            ''                        lFormat:=PMFormatString, _
            ''                        lMandatory:=PMNonMandatory)
            '
            '    If m_lReturn <> PMTrue Then
            '      SetFieldValidation = PMFalse
            '      Exit Function
            '    End If
            '
            '    m_lReturn = m_oFormFields.AddNewFormField( _
            ''                        ctlControl:=uctcontact, _
            ''                        lGridColumn:=3, _
            ''                        lFieldType:=PMString, _
            ''                        lFormat:=PMFormatString, _
            ''                        lMandatory:=PMNonMandatory)
            '
            '    If m_lReturn <> PMTrue Then
            '      SetFieldValidation = PMFalse
            '      Exit Function
            '    End If
            '    m_lReturn = m_oFormFields.AddNewFormField( _
            ''                        ctlControl:=uctcontact, _
            ''                        lGridColumn:=4, _
            ''                        lFieldType:=PMString, _
            ''                        lFormat:=PMFormatString, _
            ''                        lMandatory:=PMNonMandatory)
            '
            '    If m_lReturn <> PMTrue Then
            '      SetFieldValidation = PMFalse
            '      Exit Function
            '    End If
            '    m_lReturn = m_oFormFields.AddNewFormField( _
            ''                        ctlControl:=uctcontact, _
            ''                        lGridColumn:=5, _
            ''                        lFieldType:=PMString, _
            ''                        lFormat:=PMFormatString, _
            ''                        lMandatory:=PMNonMandatory)
            '
            '    If m_lReturn <> PMTrue Then
            '      SetFieldValidation = PMFalse
            '      Exit Function
            '    End If

            ' ************************************************************
            ' Enter your code here to assign all of the controls to
            ' PMFormControl
            '
            ' Example:-
            '
            '        ' Pass control and required settings to FormControl
            '        m_lReturn = m_oFormFields.AddNewFormField( _
            ''                       ctlControl:=<Control Name>, _
            ''                       lFieldType:=<PM field type>, _
            ''                       lFormat:=<PM format string>, _
            ''                       lMandatory:=<PMMandatory or PMNonMandatory)
            '
            '        'Error checking
            '        If m_lReturn <> PMTrue Then
            '          SetFieldValidation = PMFalse
            '          Exit Function
            '        End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Get the details from the business object.

            ' {* USER DEFINED CODE (Begin) *}

            If m_lAddressCnt = 0 Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            'developer guide no. 37(guide)
            m_lReturn = m_oBusiness.GetDetails(vAddressCnt:=m_lAddressCnt)

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return result
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDefaultTypeID
    '
    ' Description: Gets the ID for the correspondance address
    '
    ' ***************************************************************** '
    Private Function GetDefaultTypeID(ByRef r_lID As Integer) As Integer
        Dim result As Integer = 0

        Dim oLookup As bPMLookup.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the lookup object
            Dim temp_oLookup As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oLookup, "bPMLookup.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oLookup = temp_oLookup
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the product family

            oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

            ' Get the ID for the default

            'developer guide no. 37
            m_lReturn = oLookup.GetEffectiveIDFromCode(v_sTableName:="address_usage_type", v_sCode:=gSIRLibrary.SIRMainAddressABICode, v_dtEffectiveDate:=DateTime.Now, r_lID:=r_lID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Remove instance from memory

            oLookup.Dispose()
            oLookup = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaultTypeID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultTypeID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: BusinessToInterface
    '
    ' Description: Updates all interface details from the business
    '              object.
    '
    ' ***************************************************************** '
    Public Function BusinessToInterface() As Integer

        Dim result As Integer = 0
        Dim intCountryID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Assign the details from the business object
            ' to the data storage.
            m_lReturn = BusinessToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' MS220501
            uctadd.CountryId = m_lCountryID
            intCountryID = m_lCountryID


            'developer guide no. 37(guide)
            m_lReturn = m_oBusiness.GetCountry(iCountryID:=intCountryID, sCountryCode:=g_sCountryCode)

            ' Assign the details to the interface.
            If g_sCountryCode.Trim() = "GBR" Then
                uctadd.IsPostCodeRequired = 1
                ' START CHANGES - Changed By: AAB  - Changed On: 17-Mar-2004 15:12
                ' Added this the US needs Zip Code/Postcode
            ElseIf g_sCountryCode.Trim() = "USA" Then
                uctadd.IsPostCodeRequired = 1
                ' END CHANGES - Changed By: AAB  - Changed On: 17-Mar-2004 15:12
                ' this should now rely on the ispostcoderequired indicator on the country table
                ' instead of always defaulting to empty.
                '    Else
                '        'uctadd.IsPostCodeRequired = 0
            End If

            'sj 18/06/2002 - start
            If m_bIsNRMA Then
                uctadd.IsPostCodeRequired = 0
            End If

            'PN:45199
            pnlAdPostcode.Text = m_sPostalCode
            pnlConPostCode.Text = m_sPostalCode

            'Fill the Address control
            uctadd.PostCode = m_sPostalCode
            uctadd.AddressLine1 = m_sAddress1
            uctadd.AddressLine2 = m_sAddress2
            uctadd.AddressLine3 = m_sAddress3
            uctadd.AddressLine4 = m_sAddress4
            uctadd.CountryId = m_lCountryID

            'Fill the contacts List
            PopulateContacts()

            ' If in add mode then default to correspondance address
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                m_lReturn = GetDefaultTypeID(r_lID:=m_lAddressUsageTypeID)
            End If

            ' CF 300499 - Address Usage Type
            cboAddUsageType.ItemId = m_lAddressUsageTypeID

            ' ************************************************************
            ' Enter your code here to assign the all of the interface
            ' details from the business object, using the FormatField
            ' function for any type conversion.
            '
            ' Example:-
            '
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtName, vControlValue:=m_sName$)
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=optChoice, vControlValue:=m_iDChoice%)
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtDate, vControlValue:=m_dtDDate)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: InterfaceToBusiness
    '
    ' Description: Updates all business members from the interface
    '              details.
    '
    ' ***************************************************************** '
    Public Function InterfaceToBusiness() As Integer

        Dim result As Integer = 0
        Dim lBusinessDataID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the business object.

            ' Assign the details from the interface to the data storage.
            m_lReturn = InterfaceToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Set the business data ID to one because we are only
            ' dealing with one record item only.
            lBusinessDataID = 1
            If m_sScreenHeirarchy <> "" Then
                m_sScreenHeirarchy = m_sScreenHeirarchy & "/" & $"AddressType({m_sAddressUsageType})"
            End If
            ' Check the task.
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Inform the business object with a new data item.

                    ' {* USER DEFINED CODE (Begin) *}

                    'developer guide no. 37
                    m_lReturn = m_oBusiness.EditAdd(lRow:=lBusinessDataID, vAddress1:=m_sAddress1, vAddress2:=m_sAddress2, vAddress3:=m_sAddress3, vAddress4:=m_sAddress4, vPostalCode:=m_sPostalCode, vCountryID:=m_lCountryID, sUniqueId:=m_sUniqueId, sScreenHeirarchy:=m_sScreenHeirarchy)
                    ' {* USER DEFINED CODE (End) *}

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Inform the business object with an updated data item.

                    ' {* USER DEFINED CODE (Begin) *}


                    'developer guide no. 37(guide)
                    m_lReturn = m_oBusiness.EditUpdate(lRow:=lBusinessDataID, vAddress1:=m_sAddress1, vAddress2:=m_sAddress2, vAddress3:=m_sAddress3, vAddress4:=m_sAddress4, vPostalCode:=m_sPostalCode, vCountryID:=m_lCountryID, sUniqueId:=m_sUniqueId, sScreenHeirarchy:=m_sScreenHeirarchy)
                    ' {* USER DEFINED CODE (End) *}
            End Select

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
            End If


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: DisplayLookupDetails
    '
    ' Description: Displays all of the lookup details using the lookup
    '              values/details.
    '
    ' ***************************************************************** '
    Public Function DisplayLookupDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            m_lReturn = GetLookupValues()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get all of the lookup details.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to retreive all of the lookup
            ' descriptions for a given lookup type.
            ' The GetLookupDetails function will allow you to do this.
            '
            ' Example:-
            '
            '    m_lReturn& = GetLookupDetails( _
            ''        sLookupTable:=PMLookupCodeName, _
            ''        ctlLookup:=cmbCodeName)
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        DisplayLookupDetails = PMFalse
            '        Exit Function
            '    End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: SetStatus (Standard Method)
    '
    ' Description: Set the Process, Map and Step status.
    ' Note:        A Property Get is provided for the Step Status only
    '              as this is the only one which this component can
    '              alter directly.
    ' ***************************************************************** '
    Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the current Status settings.
            m_sProcessStatus.Value = sProcessStatus.Trim()
            m_sMapStatus.Value = sMapStatus.Trim()
            m_sStepStatus.Value = sStepStatus.Trim()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)
    ' ***************************************************************** '
    '
    ' Name: ValidateInputFields
    '
    ' Description:
    '
    ' History: 23/07/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function ValidateInputFields(ByVal v_sPostCode As String, ByVal v_sAddressLine1 As String, ByVal v_sAddressLine2 As String, ByVal v_sAddressLine4 As String, ByVal v_lCountryId As Integer, Optional ByVal v_vEffectiveDate As Object = Nothing, Optional ByVal v_sAddressLine3 As String = "") As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sCharacter As String = ""

            'MS210501 Check if we are in Great Britain

            'developer guide no. 37(guide)
            m_lReturn = m_oBusiness.GetCountry(iCountryID:=v_lCountryId, sCountryCode:=g_sCountryCode, r_lPostalCodeVisibilityId:=m_lPostCodeVisiblilityId)


            'Address line 1
            m_lReturn = CheckForIllegalCharacters(v_sAddressLine:=v_sAddressLine1, r_sCharacter:=sCharacter)
            If sCharacter <> "" Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("Illegal Character '" & sCharacter & "' in " & StripTrailingColon(uctadd.CaptionAddress1), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                uctadd.Focus()
                Return result
            End If
            'Address Line 2
            m_lReturn = CheckForIllegalCharacters(v_sAddressLine:=v_sAddressLine2, r_sCharacter:=sCharacter)
            If sCharacter <> "" Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("Illegal Character '" & sCharacter & "' in " & StripTrailingColon(uctadd.CaptionAddress2), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                uctadd.Focus()
                Return result
            End If
            'Address line 3
            m_lReturn = CheckForIllegalCharacters(v_sAddressLine:=v_sAddressLine3, r_sCharacter:=sCharacter)
            If sCharacter <> "" Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("Illegal Character '" & sCharacter & "' in " & StripTrailingColon(uctadd.CaptionAddress3), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                uctadd.Focus()
                Return result
            End If
            'Address line 4
            m_lReturn = CheckForIllegalCharacters(v_sAddressLine:=v_sAddressLine4, r_sCharacter:=sCharacter)
            If sCharacter <> "" Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("Illegal Character '" & sCharacter & "' in " & StripTrailingColon(uctadd.CaptionAddress4), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                uctadd.Focus()
                Return result
            End If
            'Post code
            m_lReturn = CheckForIllegalCharacters(v_sAddressLine:=v_sPostCode, r_sCharacter:=sCharacter)
            If sCharacter <> "" Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("Illegal Character '" & sCharacter & " in " & StripTrailingColon(uctadd.CaptionPostCode), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                uctadd.Focus()
                Return result
            End If

            If Not m_bIsNRMA Then
                'DC120706 PN29319 only validate postcode if QAS installed
                If g_sCountryCode.Trim() = "GBR" And m_sQASInstalled <> "0" Then
                    m_lReturn = PMBGeneralFunc.CheckValidPostCode(v_sPostCode:=v_sPostCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("The " & StripTrailingColon(uctadd.CaptionPostCode) & " '" & v_sPostCode & "' is not of a valid format.", "Post Code - " & v_sPostCode, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        uctadd.Focus()
                        SSTabHelper.SetSelectedIndex(tabMainTab, 0)

                        If Not Information.IsNothing(v_vEffectiveDate) Then
                            optAddress(ACOptFutureAddress).Checked = True
                        End If
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    'check for spaces in postcode
                    If v_sPostCode.Trim() = "" Then
                        MessageBox.Show("You must supply a " & StripTrailingColon(uctadd.CaptionPostCode), Application.ProductName)
                        uctadd.Focus()
                        SSTabHelper.SetSelectedIndex(tabMainTab, 0)

                        If Not Information.IsNothing(v_vEffectiveDate) Then
                            optAddress(ACOptFutureAddress).Checked = True
                        End If
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            'Validate the fields in the address control too
            If m_bIsNRMA Then
                If v_sAddressLine2.Trim() = "" Then
                    MessageBox.Show("You must supply " & StripTrailingColon(uctadd.CaptionAddress2), Application.ProductName)
                    uctadd.Focus()
                    SSTabHelper.SetSelectedIndex(tabMainTab, 0)

                    If Not Information.IsNothing(v_vEffectiveDate) Then
                        optAddress(ACOptFutureAddress).Checked = True
                    End If
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                If v_sAddressLine4.Trim() = "" Then
                    MessageBox.Show("You must supply " & StripTrailingColon(uctadd.CaptionAddress4), Application.ProductName)
                    uctadd.Focus()
                    SSTabHelper.SetSelectedIndex(tabMainTab, 0)

                    If Not Information.IsNothing(v_vEffectiveDate) Then
                        optAddress(ACOptFutureAddress).Checked = True
                    End If
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                If v_sAddressLine1.Trim() = "" Then
                    MessageBox.Show("You must supply " & StripTrailingColon(uctadd.CaptionAddress1), Application.ProductName)
                    uctadd.Focus()
                    SSTabHelper.SetSelectedIndex(tabMainTab, 0)

                    If Not Information.IsNothing(v_vEffectiveDate) Then
                        optAddress(ACOptFutureAddress).Checked = True
                    End If
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Additional validation for USA address.
                If g_sCountryCode.Trim() = "USA" Then
                    If v_sAddressLine2.Trim() = "" Then
                        MessageBox.Show("You must supply a " & StripTrailingColon(uctadd.CaptionAddress2), Application.ProductName)
                        uctadd.Focus()
                        SSTabHelper.SetSelectedIndex(tabMainTab, 0)

                        If Not Information.IsNothing(v_vEffectiveDate) Then
                            optAddress(ACOptFutureAddress).Checked = True
                        End If
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If (v_sAddressLine3.Trim() = "") Or v_sAddressLine3.Trim() = "(None)" Then
                        MessageBox.Show("You must Select a " & StripTrailingColon(uctadd.CaptionAddress4), Application.ProductName)
                        uctadd.Focus()
                        SSTabHelper.SetSelectedIndex(tabMainTab, 0)

                        If Not Information.IsNothing(v_vEffectiveDate) Then
                            optAddress(ACOptFutureAddress).Checked = True
                        End If
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If uctadd.IsPostCodeRequired <> 0 Then
                        'Check that they provided a zip code
                        If v_sPostCode.Trim() = "" Then
                            MessageBox.Show("You must supply a " & StripTrailingColon(uctadd.CaptionPostCode), Application.ProductName)
                            uctadd.Focus()
                            SSTabHelper.SetSelectedIndex(tabMainTab, 0)

                            If Not Information.IsNothing(v_vEffectiveDate) Then
                                optAddress(ACOptFutureAddress).Checked = True
                            End If
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        m_lReturn = PMBGeneralFunc.CheckValidUSZipCode(v_sZipCode:=v_sPostCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            MessageBox.Show("The " & StripTrailingColon(uctadd.CaptionPostCode) & " '" & v_sPostCode & "' is not of a valid format.", Application.ProductName)
                            uctadd.Focus()
                            SSTabHelper.SetSelectedIndex(tabMainTab, 0)

                            If Not Information.IsNothing(v_vEffectiveDate) Then
                                optAddress(ACOptFutureAddress).Checked = True
                            End If
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                End If
            End If


            If Not Information.IsNothing(v_vEffectiveDate) Then
                If DateTime.Now.ToString("yyyyMMdd") >= m_dtFutureEffectiveDate.ToString("yyyyMMdd") Then
                    MessageBox.Show("Effective date must be in the future", Application.ProductName)
                    SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                    optAddress(ACOptFutureAddress).Checked = True
                    txtEffectiveDate.Focus()
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateInputFields Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateInputFields", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: CheckForIllegalCharacters
    '
    ' Description:
    '
    ' History: 19/07/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function CheckForIllegalCharacters(ByVal v_sAddressLine As String, ByRef r_sCharacter As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vIllegalCharacters As Object

            r_sCharacter = ""

            If v_sAddressLine.Length = 0 Then
                Return result
            End If

            'DC020605 remove hyphen, forward slash, apostrophe, left & right brackets from illegal characters
            'vIllegalCharacters = Array("!", "£", "$", "%", "^", "&", "*", "(", ")", "-", "+", "=", "[", "]", ":", ";", "@", "~", "#", "<", ">", ",", "?", "/", "\", "|")
            'vIllegalCharacters = Array("!", "£", "$", "%", "^", "&", "*", "+", "=", ":", ";", "@", "~", "#", "<", ">", ",", "?", "\", "|")
            'PN 24267 .(#,!,$,%,^,&,@,*


            vIllegalCharacters = New Char() {"£", "+", "=", ":", ";", "~", "<", ">", "?", "|"}


            For i As Integer = 0 To vIllegalCharacters.GetUpperBound(0)
                If v_sAddressLine.IndexOf(vIllegalCharacters(i)) >= 0 Then

                    r_sCharacter = CStr(vIllegalCharacters(i))
                    Return result
                End If
            Next i

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckForIllegalCharacters Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckForIllegalCharacters", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ValidateOK
    '
    ' Description: This validates according to whether the address
    ' already exists or not
    '
    ' ***************************************************************** '
    Private Function ValidateOK() As Integer

        Dim result As Integer = 0
        Dim sAddress1 As String = ""
        Dim sAddress2 As String = ""
        Dim sAddress3 As String = ""
        Dim sAddress4 As String = ""
        Dim sPostalCode As String = ""
        Dim lCountryID As Integer
        Dim sTmp As String = ""
        'DJM 24/09/2003
        Dim bMultipleUse As Boolean


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then

                'DJM 24/09/2003 : Transferred 1.6.9 change to here. Sorts out address grouping.
                '                 Only useable if rapid address not set up.
                If uctadd.PMAddressCnt > 0 Then
                    'Address to add was got from database
                    'must check if its changed so get it

                    m_oBusiness.AddressCnt = uctadd.PMAddressCnt


                    'developer guide no. 37(guide)
                    m_lReturn = m_oBusiness.GetDetails(vAddressCnt:=uctadd.PMAddressCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    'developer guide no. 37(guide)
                    m_lReturn = m_oBusiness.GetNext(vAddress1:=sAddress1, vAddress2:=sAddress2, vAddress3:=sAddress3, vAddress4:=sAddress4, vPostalCode:=sPostalCode, vCountryID:=lCountryID)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Has user changed anything
                    If (sAddress1.Trim() <> m_sCurrentAddress1.Trim()) Or (sAddress2.Trim() <> m_sCurrentAddress2.Trim()) Or (sAddress3.Trim() <> m_sCurrentAddress3.Trim()) Or (sAddress4.Trim() <> m_sCurrentAddress4.Trim()) Or (CStr(lCountryID).Trim() <> CStr(m_lCurrentCountryId).Trim()) Or (sPostalCode.Trim() <> m_sCurrentPostalCode.Trim()) Then
                        'Something has changed

                        sTmp = CStr(MessageBox.Show("This address already exists and is in use by other parties, " & _
                               "yet you have made changes." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                               "Do you wish these changes to apply to all other parties use of " & _
                               "this address ?" & _
                               Strings.Chr(13) & Strings.Chr(10) & "(Select 'No' and a new address will be created just for " & _
                               "this party)", "Confirm Group Address Add", MessageBoxButtons.YesNoCancel))

                        Select Case sTmp
                            Case CStr(System.Windows.Forms.DialogResult.Yes)

                                result = ACAddressEdit
                                'Effectively we are doing an edit, so change the task
                                m_iTask = gPMConstants.PMEComponentAction.PMEdit

                            Case CStr(System.Windows.Forms.DialogResult.No)

                                result = ACAddressAdd
                                'Clear out stuff as we'll be doing an add

                                m_lReturn = m_oBusiness.Clear()

                                m_oBusiness.AddressCnt = 0

                            Case Else

                                result = ACAddressCancel

                                m_lReturn = m_oBusiness.Clear()

                        End Select
                    Else

                        sTmp = CStr(MessageBox.Show("This address already exists and is in use by other parties." & _
                               Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                               "Do you wish this address to be grouped to all other parties use of " & _
                               "this address ?" & _
                               Strings.Chr(13) & Strings.Chr(10) & "(Select 'No' and a new address will be created just for " & _
                               "this party)", "Confirm Group Address Add", MessageBoxButtons.YesNoCancel))

                        Select Case sTmp
                            Case CStr(System.Windows.Forms.DialogResult.Yes)
                                result = ACAddressDoNotAdd
                            Case CStr(System.Windows.Forms.DialogResult.No)
                                result = ACAddressAdd
                                'Clear out stuff as we'll be doing an add

                                m_lReturn = m_oBusiness.Clear()

                                m_oBusiness.AddressCnt = 0
                            Case Else
                                result = ACAddressCancel

                                m_lReturn = m_oBusiness.Clear()
                        End Select

                    End If
                Else
                    'Doesnt already exist, so add it
                    result = ACAddressAdd
                End If

            ElseIf (m_iTask = gPMConstants.PMEComponentAction.PMEdit) Then

                'DJM 24/09/2003 : Transferred 1.6.9 change to here. Sorts out address grouping.
                '                 Only useable if rapid address not set up.

                If uctadd.PMAddressCnt = 0 Or m_oBusiness.AddressCnt = uctadd.PMAddressCnt Then

                    'Has user changed anything
                    If (m_sAddress1.Trim() <> m_sCurrentAddress1.Trim()) Or (m_sAddress2.Trim() <> m_sCurrentAddress2.Trim()) Or (m_sAddress3.Trim() <> m_sCurrentAddress3.Trim()) Or (m_sAddress4.Trim() <> m_sCurrentAddress4.Trim()) Or (CStr(m_lCountryID).Trim() <> CStr(m_lCurrentCountryId).Trim()) Or (m_sPostalCode.Trim() <> m_sCurrentPostalCode.Trim()) Then

                        'Something has changed
                        'Check if anyone else uses this address

                        ' CTAF 110701 - Comment out for now incase it needs putting back in
                        ' PWF  251001 - Reinstated for merge (see below)
                        'sj 20/06/2002 - start
                        'developer guide no. 37
                        If m_bIsNRMA And (cboAddUsageType.ItemCaption = "Correspondence Address" Or cboAddUsageType.ItemCaption = "Home Address") Then
                            MessageBox.Show("Do Situations of Risk Require Amendment?", "Address Edit", MessageBoxButtons.OK, MessageBoxIcon.Question)
                            result = ACAddressEdit
                        Else
                            'sj 20/06/2002 - end
                            'DJM 18/11/2002 : If it is grouped, do they want to edit entire group.


                            'developer guide no. 37(guide)
                            m_lReturn = m_oBusiness.MultipleUse(m_oBusiness.AddressCnt, bMultipleUse)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            If bMultipleUse Then

                                sTmp = CStr(MessageBox.Show("This address is used by other parties, " & _
                                       "yet you have made changes." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                                       "Do you wish these changes to apply to all other parties use of " & _
                                       "this address ?" & _
                                       Strings.Chr(13) & Strings.Chr(10) & "(Select 'No' and a new address will be created just for " & _
                                       "this party)", "Confirm Group Address Edit", MessageBoxButtons.YesNoCancel))

                                Select Case sTmp
                                    Case CStr(System.Windows.Forms.DialogResult.Yes)
                                        result = ACAddressEdit
                                    Case CStr(System.Windows.Forms.DialogResult.No)
                                        result = ACAddressAdd


                                        m_lReturn = m_oBusiness.Clear()

                                        m_oBusiness.AddressCnt = 0
                                        'Effectively we are doing an add, so change the task
                                        m_iTask = gPMConstants.PMEComponentAction.PMAdd
                                    Case Else
                                        result = ACAddressCancel

                                        m_lReturn = m_oBusiness.Clear()
                                End Select

                            Else
                                result = ACAddressEdit
                            End If

                        End If
                    End If
                Else


                    m_oBusiness.AddressCnt = uctadd.PMAddressCnt


                    'developer guide no. 37(guide)
                    m_lReturn = m_oBusiness.GetDetails(vAddressCnt:=uctadd.PMAddressCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    'developer guide no. 37(guide)
                    m_lReturn = m_oBusiness.GetNext(vAddress1:=sAddress1, vAddress2:=sAddress2, vAddress3:=sAddress3, vAddress4:=sAddress4, vPostalCode:=sPostalCode, vCountryID:=lCountryID)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If (sAddress1.Trim() <> uctadd.AddressLine1.Trim()) Or (sAddress2.Trim() <> uctadd.AddressLine2.Trim()) Or (sAddress3.Trim() <> uctadd.AddressLine3.Trim()) Or (sAddress4.Trim() <> uctadd.AddressLine4.Trim()) Or (CStr(lCountryID).Trim() <> CStr(uctadd.CountryId).Trim()) Or (sPostalCode.Trim() <> uctadd.PostCode.Trim()) Then
                        'Something has changed
                        'Check if anyone else uses this address


                        sTmp = CStr(MessageBox.Show("This address is used by other parties, " & _
                               "yet you have made changes." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                               "Do you wish these changes to apply to all other parties use of " & _
                               "this address ?" & _
                               Strings.Chr(13) & Strings.Chr(10) & "(Select 'No' and a new address will be created just for " & _
                               "this party)", "Confirm Group Address Edit", MessageBoxButtons.YesNoCancel))

                        Select Case sTmp
                            Case CStr(System.Windows.Forms.DialogResult.Yes)
                                result = ACAddressEdit
                            Case CStr(System.Windows.Forms.DialogResult.No)
                                result = ACAddressAdd

                                m_lReturn = m_oBusiness.Clear()

                                m_oBusiness.AddressCnt = 0
                                'Effectively we are doing an add, so change the task
                                m_iTask = gPMConstants.PMEComponentAction.PMAdd
                            Case Else
                                result = ACAddressCancel

                                m_lReturn = m_oBusiness.Clear()
                        End Select

                    Else
                        sTmp = CStr(MessageBox.Show("This address already exists and is in use by other parties." & _
                               Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                               "Do you wish this address to be grouped to all other parties use of " & _
                               "this address ?" & _
                               Strings.Chr(13) & Strings.Chr(10) & "(Select 'No' and a new address will be created just for " & _
                               "this party)", "Confirm Group Address Add", MessageBoxButtons.YesNoCancel))

                        Select Case sTmp
                            Case CStr(System.Windows.Forms.DialogResult.Yes)
                                result = ACAddressEdit
                            Case CStr(System.Windows.Forms.DialogResult.No)
                                result = ACAddressAdd

                                m_lReturn = m_oBusiness.Clear()

                                m_oBusiness.AddressCnt = 0
                                'Effectively we are doing an add, so change the task
                                m_iTask = gPMConstants.PMEComponentAction.PMAdd
                            Case Else
                                result = ACAddressCancel

                                m_lReturn = m_oBusiness.Clear()
                        End Select

                    End If
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateOKFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateOK", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: UpdateContacts
    '
    ' Description: This goes thru all contacts in the the grid control
    ' and the original contact array and sees what the differences
    ' are. It then adds new contacts or deletes existing ones according
    ' to what user has done.
    '
    ' ***************************************************************** '
    Private Function UpdateContacts() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        Dim vNewContacts() As Object = Nothing
        Dim vOldContacts() As Object = Nothing
        Dim bFirst As Boolean
        Dim i As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'Go thru original contact array to get list of old contacts
            If Information.IsArray(m_vContacts) Then
                ReDim vOldContacts(m_vContacts.GetUpperBound(1))
                For i = m_vContacts.GetLowerBound(1) To m_vContacts.GetUpperBound(1)

                    vOldContacts(i) = CInt(m_vContacts(0, i))
                Next i
            End If

            'Go thru contacts grid to get list of new contacts
            'SP171298
            i = 1
            bFirst = True

            Do
                If i > lvwContacts.Items.Count Then
                    Exit Do
                End If
                oListItem = lvwContacts.Items.Item(i - 1)
                If ListViewHelper.GetListViewSubItem(oListItem, 1).Text.Trim() = "" Then
                    Exit Do
                Else
                    If bFirst Then
                        ReDim vNewContacts(i - 1)
                        bFirst = False
                    Else
                        ReDim Preserve vNewContacts(i - 1)
                    End If


                    vNewContacts(i - 1) = Convert.ToString(oListItem.Tag)

                End If
                i += 1
            Loop


            'Delete old contact usages in database
            If (Information.IsArray(vOldContacts)) And (Not Information.IsArray(vNewContacts)) Then

                'developer guide no. 37(guide)
                m_lReturn = m_oBusiness.UpdateContacts(vAddressCnt:=m_lAddressCnt, vDeleteContacts:=vOldContacts)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Add new contacts in database
            If (Not Information.IsArray(vOldContacts)) And (Information.IsArray(vNewContacts)) Then

                'developer guide no. 37(guide)
                m_lReturn = m_oBusiness.UpdateContacts(vAddressCnt:=m_lAddressCnt, vAddContacts:=vNewContacts)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            'If we have old and new Contacts, delete common ones
            If (Information.IsArray(vOldContacts)) And (Information.IsArray(vNewContacts)) Then

                'Delete unchanged Contacts (ie set them to 0)

                For i = vOldContacts.GetLowerBound(0) To vOldContacts.GetUpperBound(0)

                    For j As Integer = vNewContacts.GetLowerBound(0) To vNewContacts.GetUpperBound(0)


                        If vNewContacts(j).Equals(vOldContacts(i)) Then

                            vNewContacts(j) = 0

                            vOldContacts(i) = 0
                        End If
                    Next j
                Next i

                'update the database

                'developer guide no. 37(guide)
                m_lReturn = m_oBusiness.UpdateContacts(vAddressCnt:=m_lAddressCnt, vDeleteContacts:=vOldContacts, vAddContacts:=vNewContacts)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateContactsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateContacts", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: BusinessToData
    '
    ' Description: Updates the data storage from the business object.
    '
    ' ***************************************************************** '
    Private Function BusinessToData() As Integer

        Dim result As Integer = 0
        Dim sTmp As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details to the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            'developer guide no. 37(guide)
            m_lReturn = m_oBusiness.GetNext(vAddress1:=m_sAddress1, vAddress2:=m_sAddress2, vAddress3:=m_sAddress3, vAddress4:=m_sAddress4, vPostalCode:=m_sPostalCode, vCountryID:=m_lCountryID)

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If


            'developer guide no. 37(guide)
            m_lReturn = m_oBusiness.GetContacts(vContacts:=m_vContacts)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the contact details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: InterfaceToData
    '
    ' Description: Updates the data storage from the interface details.
    '
    ' ***************************************************************** '
    Private Function InterfaceToData() As Integer

        ' Temp string to hold postcode before validation
        Dim result As Integer = 0
        Dim sTempPostalCode As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            'sj 23/07/2002 - start
            '    sTempPostalCode = uctAdd.PostCode
            '    m_sAddress1$ = Trim$(uctAdd.AddressLine1)
            '    m_sAddress2$ = Trim$(uctAdd.AddressLine2)
            '    m_sAddress3$ = Trim$(uctAdd.AddressLine3)
            '    m_sAddress4$ = Trim$(uctAdd.AddressLine4)
            '    m_lCountryID = uctAdd.CountryID

            sTempPostalCode = m_sCurrentPostalCode
            m_sAddress1 = m_sCurrentAddress1
            m_sAddress2 = m_sCurrentAddress2
            m_sAddress3 = m_sCurrentAddress3
            m_sAddress4 = m_sCurrentAddress4
            m_lCountryID = m_lCurrentCountryId
            'sj 23/07/2002 - end

            'DJM 05/11/2003 : Do not shuffle address.
            '    ' CF 080199 - Shuffle up address if line 2 is missing
            '    If (m_sAddress2$ = "") Then
            '        m_sAddress2$ = m_sAddress3$
            '        m_sAddress3$ = m_sAddress4$
            '        m_sAddress4$ = ""
            '         'sj 23/07/2002 - start
            ''        m_lCountryID = uctAdd.CountryID
            '         'sj 23/07/2002 - end
            '    End If

            ' CF 080199 - Fix postcode if QAS has returned it with an
            '             inappropriate number of spaces
            m_lReturn = PMBGeneralFunc.FormatPostCode(v_sInString:=sTempPostalCode, r_sOutString:=m_sPostalCode)

            'MS220501 Blank the postcode if we are not in Britain.
            'AAB-17-Mar-2004 15:13 - USA need zip code

            If g_sCountryCode.Trim() <> "GBR" And g_sCountryCode.Trim() <> "USA" Then
                '3 - Postcode visible and mandatory
                '2 - Postcode visible but optional
                Select Case m_lPostCodeVisiblilityId
                    Case 3, 2
                        ' dont clear down postcode as it could have been set
                    Case Else
                        m_sPostalCode = ""
                End Select
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' CF 300499 - Address usage type
            'developer guide no. 37
            m_lAddressUsageTypeID = cboAddUsageType.ItemId

            'developer guide no. 37
            m_sAddressUsageType = cboAddUsageType.ItemCaption

            ' ************************************************************
            ' Enter your code here to assign all of the details from the
            ' interface to the data storage.
            '
            ' Example:-
            '
            '    m_DName$ = trim$(txtName.Text)
            '    m_DDate = CDate(txtDate.Text)
            '    m_iDCodeID% = cmbCode.ItemData(cmbCode.ListIndex)
            '    m_lReturn& = m_oFormFields.UnformatControl(txtName)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Dim vUnderwriting As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the status of the Navigate button.
            Select Case (m_lNavigate)
                Case gPMConstants.PMENavigateButtonStatus.PMNavigateEnabled
                    '            cmdNavigate.Visible = True
                    '            cmdNavigate.Enabled = True

                Case gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled
                    '            cmdNavigate.Visible = True
                    '            cmdNavigate.Enabled = False

                Case Else
                    '            cmdNavigate.Visible = False
            End Select

            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.


            '    ' {* USER DEFINED CODE (Begin) *}
            '    uctContact.Row = 0
            '    uctContact.Col = 1
            '    uctContact.Text = "Area Code"
            '    uctContact.ColWidth(1) = 7
            '    uctContact.Col = 2
            '    uctContact.Text = "Number"
            '    uctContact.ColWidth(2) = 8
            '    uctContact.Col = 3
            '    uctContact.Text = "Extension"
            '    uctContact.ColWidth(3) = 8
            '    uctContact.Col = 4
            '    uctContact.Text = "Type"
            '    uctContact.ColWidth(4) = 20
            '    uctContact.Col = 5
            '    uctContact.Text = "Description"
            '    uctContact.ColWidth(5) = 40

            '    pnlAdReference = "FULLER T"
            '    pnlAdPostcode = "TF10 7PA"
            '    pnlConReference = "FULLER T"
            '    pnlConPostCode = "TF10 7PA"
            '    grdContact.Row = 1
            '    grdContact.Col = 1
            '    grdContact.Text = "0121"
            '    grdContact.Col = 2
            '    grdContact.Text = "355 3567"
            '    grdContact.Col = 3
            '    grdContact.Text = "Telephone"
            '    grdContact.Col = 4
            '    grdContact.Text = "Main Switchboard"
            '
            '    grdContact.Row = 2
            '    grdContact.Col = 1
            '    grdContact.Text = "0121"
            '    grdContact.Col = 2
            '    grdContact.Text = "354 3469"
            '    grdContact.Col = 3
            '    grdContact.Text = "Fax"
            '    grdContact.Col = 4
            '    grdContact.Text = "Main Fax"
            ' {* USER DEFINED CODE (End) *}

            ' CF 300499 - Address Usage Type
            ' If in add mode then default to correspondance address
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                iPMFunc.getUnderwritingOrAgency(r_vUnderwriting:=vUnderwriting)
                If vUnderwriting = "A" Then
                    cboAddUsageType.WhereClause = "code <> 'BROKERLINK'"
                    cboAddUsageType.RefreshList()
                End If

                m_lReturn = GetDefaultTypeID(r_lID:=m_lAddressUsageTypeID)
                'developer guide no. 37
                cboAddUsageType.ItemId = m_lAddressUsageTypeID
                m_lCorrespondanceAddressUsageTypeId = m_lAddressUsageTypeID
            End If

            'sj 12/06/2002 - start
            If m_bIsNRMA Then
                With uctadd
                    .CaptionAddress1 = "Property Name:"
                    .CaptionAddress2 = "Street/PO box:"
                    .CaptionAddress3 = "Suburb:"
                    .CaptionAddress4 = "City:"
                End With
                uctadd.IsPostCodeRequired = 0
            End If
            'sj 12/06/2002 - end

            'sj 23/07/2002 - start
            If m_bFutureDateAddressChanges Then
                fraFutureDateAddressChanges.Visible = True
            End If
            'sj 23/07/2002 - end
            cmdDeleteCon.Enabled = False
            cmdEditCon.Enabled = False
            'DJM 18/11/2003 : Objects selected by tabbing should only be on the currently selected tab.
            SetTabStops(SSTabHelper.GetSelectedIndex(tabMainTab))

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetFirstLastControls
    '
    ' Description: Sets the first and last data entry controls for
    '              each tab to the control array, for use with the
    '              keyboard navigation.
    '
    ' ***************************************************************** '
    Private Function SetFirstLastControls() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise the control array with the number of
            ' tabs which contain data entry fields on (Remember
            ' that arrays start from zero, therefore you must
            ' subtract one from the number of tabs).
            ReDim m_ctlTabFirstLast(1, 1)

            ' Set the first and last data entry controls for
            ' all of the tabs.

            'DJM 18/11/2003 : Set the first/last controls for contact tab.
            m_ctlTabFirstLast(ACControlStart, 0) = cboAddUsageType
            m_ctlTabFirstLast(ACControlEnd, 0) = cmdNext(0)
            m_ctlTabFirstLast(ACControlStart, 1) = lvwContacts
            m_ctlTabFirstLast(ACControlEnd, 1) = cmdPrevious(1)

            ' ************************************************************
            ' Enter your code here to set the first and last data entry
            ' controls for all of the tabs.
            '
            ' Example:-
            '
            '    Set m_ctlTabFirstLast(ACControlStart, 0) = txtName
            '    Set m_ctlTabFirstLast(ACControlEnd, 0) = txtAge
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Display all language specific captions


            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMainTabTitle0, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 1, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMainTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            cmdNext(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNextCaption0, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdEditCon.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditConCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdDeleteCon.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteConCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdAddCon.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddConCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblConPostCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACConPostCodeCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblConReference.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACConReferenceCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lbAdReference.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AClbAdReferenceCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAdPostcode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAdPostcodeCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'sj 23/07/2002 - start


            optAddress(ACOptFutureAddress).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFutureDatedAddressChangeCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            optAddress(ACOptCurrentAddress).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCurrentAddressCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblEffectiveDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEffectiveDateCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblClearFutureAddress.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearFutureAddressCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            'sj 23/07/2002 - end

            ' Display all language specific captions.
            '    frmInterface.Caption = iPMFunc.GetResData( _
            ''          iLangID:=g_iLanguageID%, _
            ''          lID:=ACInterfaceCaption, _
            ''          iDataType:=PMResString)
            '
            '    cmdHelp.Caption = iPMFunc.GetResData( _
            ''          iLangID:=g_iLanguageID%, _
            ''          lID:=ACHelpCaption, _
            ''          iDataType:=PMResString)
            '
            '    cmdCancel.Caption = iPMFunc.GetResData( _
            ''          iLangID:=g_iLanguageID%, _
            ''          lID:=ACCancelCaption, _
            ''          iDataType:=PMResString)
            '
            '    cmdOK.Caption = iPMFunc.GetResData( _
            ''          iLangID:=g_iLanguageID%, _
            ''          lID:=ACOKCaption, _
            ''          iDataType:=PMResString)
            '
            '    tabMainTab.TabCaption(0) = iPMFunc.GetResData( _
            ''          iLangID:=g_iLanguageID%, _
            ''          lID:=ACMainTabTitle0, _
            ''          iDataType:=PMResString)
            '
            '    tabMainTab.TabCaption(1) = iPMFunc.GetResData( _
            ''          iLangID:=g_iLanguageID%, _
            ''          lID:=ACMainTabTitle1, _
            ''          iDataType:=PMResString)
            '
            '    cmdNext(0).Caption = iPMFunc.GetResData( _
            ''          iLangID:=g_iLanguageID%, _
            ''          lID:=ACNextCaption0, _
            ''          iDataType:=PMResString)
            '
            '    cmdEditCon.Caption = iPMFunc.GetResData( _
            ''          iLangID:=g_iLanguageID%, _
            ''          lID:=ACEditConCaption, _
            ''          iDataType:=PMResString)
            '
            '    cmdDeleteCon.Caption = iPMFunc.GetResData( _
            ''          iLangID:=g_iLanguageID%, _
            ''          lID:=ACDeleteConCaption, _
            ''          iDataType:=PMResString)
            '
            '    cmdAddCon.Caption = iPMFunc.GetResData( _
            ''          iLangID:=g_iLanguageID%, _
            ''          lID:=ACAddConCaption, _
            ''          iDataType:=PMResString)
            '
            '    lblConPostCode.Caption = iPMFunc.GetResData( _
            ''          iLangID:=g_iLanguageID%, _
            ''          lID:=ACConPostCodeCaption, _
            ''          iDataType:=PMResString)
            '
            '    lblConReference.Caption = iPMFunc.GetResData( _
            ''          iLangID:=g_iLanguageID%, _
            ''          lID:=ACConReferenceCaption, _
            ''          iDataType:=PMResString)
            '
            '    lbAdReference.Caption = iPMFunc.GetResData( _
            ''          iLangID:=g_iLanguageID%, _
            ''          lID:=AClbAdReferenceCaption, _
            ''          iDataType:=PMResString)
            '
            '    lblAdPostcode.Caption = iPMFunc.GetResData( _
            ''          iLangID:=g_iLanguageID%, _
            ''          lID:=ACAdPostcodeCaption, _
            ''          iDataType:=PMResString)
            '
            '    Exit Function

            ' Display all language specific captions

            '    Me.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACInterfaceTitle, _
            ''        iDataType:=PMResString)
            '
            '    ' Check for an error.
            '    If (Me.Caption = "") Then
            '        ' Failed to get data from the resource file.
            '        DisplayCaptions = PMFalse
            '
            '        ' Log Error.
            '        LogMessage _
            ''            iType:=PMLogError, _
            ''            sMsg:="Unable to retrieve data from the resource file." & Chr(10) & _
            ''            "Please check the file exists and the correct captions are available", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="DisplayCaptions"
            '
            '        Exit Function
            '    End If
            '
            '    cmdOK.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACOKButton, _
            ''        iDataType:=PMResString)
            '
            '    cmdCancel.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACCancelButton, _
            ''        iDataType:=PMResString)
            '
            '    cmdHelp.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACHelpButton, _
            ''        iDataType:=PMResString)
            '
            '    cmdNavigate.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACNavigateButton, _
            ''        iDataType:=PMResString)
            '
            '    tabMainTab.TabCaption(0) = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACTabTitle1, _
            ''        iDataType:=PMResString)

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to display all language specific
            ' captions.
            ' The GetResData function will allow you to do this.
            '
            ' Example:-
            '
            '    lblDesc.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACDesc, _
            ''        iDataType:=PMResString)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '
    Private Function GetLookupValues() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Gets all of the lookup values.

            ' Check the task.
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Get all of the lookup values.

                    'developer guide no. 37(guide)
                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Get all of the lookup values with the correct
                    ' effective date.

                    'developer guide no. 37(guide)
                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)

                Case gPMConstants.PMEComponentAction.PMView
                    ' Get lookup values for viewing only.

                    'developer guide no. 37(guide)
                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
            End Select

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")

                Return result
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetLookupDetails) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As Control) As Integer
    '
    'Dim result As Integer = 0
    'Dim lRow As Integer
    'Dim bFoundMatch As Boolean
    '
    ' Lookup value contants.
    'Const ACValueTableName As Integer = 0
    'Const ACValueID As Integer = 1
    'Const ACValueStartPos As Integer = 2
    'Const ACValueNumber As Integer = 3
    '
    ' Lookup detail contants.
    'Const ACDetailKey As Integer = 0
    'Const ACDetailDesc As Integer = 1
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Get the lookup values.
    '
    'bFoundMatch = False
    '
    'For 'lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
    ' Check for a match of the table name.
    'If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
    ' Found a match
    'bFoundMatch = True
    'Exit For
    'End If
    'Next lRow
    '
    ' Check if there has been a table match.
    'If Not bFoundMatch Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")
    '
    'Return result
    'End If
    '
    ' Using the lookup values, populate the control with
    ' the details from the lookup details array.
    '
    'For 'lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
    ' Add the details to the control.

    'ctlLookup.AddItem(m_vLookupDetails(ACDetailDesc, lCntr))


    'ctlLookup.ItemData(ctlLookup.NewIndex) = CInt(m_vLookupDetails(ACDetailKey, lCntr))
    '
    'SP150998 - compare long value not string
    ' Check if this is the selected index.
    'If CStr(m_vLookupValues(ACValueID, lRow)) <> "" Then
    'If CDbl(m_vLookupValues(ACValueID, lRow)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then


    'ctlLookup.ListIndex = ctlLookup.NewIndex
    'End If
    'End If
    '
    'Next lCntr
    '
    ' Check if the selected index is blank. If so,
    ' we set the controls index to zero.
    'If CStr(m_vLookupValues(ACValueID, lRow)) = "" Then

    'ctlLookup.ListIndex = 0
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    '
    ' Name: SetUpFutureDatedAddress
    '
    ' Description:
    '
    ' History: 23/07/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function SetUpFutureDatedAddress() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Show the effective date title and text box
            lblEffectiveDate.Visible = True
            txtEffectiveDate.Visible = True
            lblClearFutureAddress.Visible = True
            cmdClearFutureAddress.Visible = True

            'Store the current fields
            m_lReturn = StoreCurrentAddress()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetUpFutureDatedAddress Failed", vApp:=ACApp, vClass:=ACClass)
                Return result
            End If

            'Load the current future dated address record
            uctadd.PostCode = m_sFuturePostalCode
            uctadd.AddressLine1 = m_sFutureAddress1
            uctadd.AddressLine2 = m_sFutureAddress2
            uctadd.AddressLine3 = m_sFutureAddress3
            uctadd.AddressLine4 = m_sFutureAddress4
            uctadd.CountryId = m_lFutureCountryId

            'developer guide no. 37(guide)
            m_lReturn = m_oBusiness.GetCountry(iCountryID:=uctadd.CountryId, sCountryCode:=g_sCountryCode)

            'Load and format the effective date
            m_lReturn = m_oFormFields.FormatControl(txtEffectiveDate, m_dtFutureEffectiveDate)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetUpFutureDatedAddress Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetUpFutureDatedAddress", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: SetUpCurrentAddress
    '
    ' Description:
    '
    ' History: 23/07/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function SetUpCurrentAddress() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Store the future settings
            m_lReturn = StoreFutureAddress()


            m_dtFutureEffectiveDate = CDate(m_oFormFields.UnformatControl(txtEffectiveDate))

            'Hide effective date controls
            lblEffectiveDate.Visible = False
            txtEffectiveDate.Visible = False
            lblClearFutureAddress.Visible = False
            cmdClearFutureAddress.Visible = False

            'Reload the current address
            uctadd.PostCode = m_sCurrentPostalCode
            uctadd.AddressLine1 = m_sCurrentAddress1
            uctadd.AddressLine2 = m_sCurrentAddress2
            uctadd.AddressLine3 = m_sCurrentAddress3
            uctadd.AddressLine4 = m_sCurrentAddress4
            uctadd.CountryId = m_lCurrentCountryId

            'developer guide no. 37(guide)
            m_lReturn = m_oBusiness.GetCountry(iCountryID:=uctadd.CountryId, sCountryCode:=g_sCountryCode)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetUpCurrentAddress Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetUpCurrentAddress", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: StoreCurrentAddress
    '
    ' Description:
    '
    ' History: 23/07/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function StoreCurrentAddress() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_sCurrentPostalCode = uctadd.PostCode.Trim()
            m_sCurrentAddress1 = uctadd.AddressLine1.Trim()
            m_sCurrentAddress2 = uctadd.AddressLine2.Trim()
            m_sCurrentAddress3 = uctadd.AddressLine3.Trim()
            m_sCurrentAddress4 = uctadd.AddressLine4.Trim()
            m_lCurrentCountryId = CInt(CStr(uctadd.CountryId).Trim())

            'developer guide no. 37
            m_lReturn = m_oBusiness.GetCountry(uctadd.CountryId, g_sCountryCode)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StoreCurrentAddress Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="StoreCurrentAddress", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Private Function FutureDatedAddressEntered() As Boolean

        Dim result As Boolean = False
        result = True

        If m_sFuturePostalCode = "" And m_sFutureAddress1 = "" And m_sFutureAddress2 = "" And m_sFutureAddress3 = "" And m_sFutureAddress4 = "" And m_dtFutureEffectiveDate = #12/30/1899# Then
            result = False
        End If

        Return result
    End Function
    ' ***************************************************************** '
    '
    ' Name: StoreFutureAddress
    '
    ' Description:
    '
    ' History: 23/07/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function StoreFutureAddress() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_sFuturePostalCode = uctadd.PostCode
            m_sFutureAddress1 = uctadd.AddressLine1
            m_sFutureAddress2 = uctadd.AddressLine2
            m_sFutureAddress3 = uctadd.AddressLine3
            m_sFutureAddress4 = uctadd.AddressLine4
            m_lFutureCountryId = uctadd.CountryId

            'developer guide no. 37(guide)
            m_lReturn = m_oBusiness.GetCountry(iCountryID:=uctadd.CountryId, sCountryCode:=g_sCountryCode)

            If Information.IsDate(txtEffectiveDate.Text) Then
                m_dtFutureEffectiveDate = CDate(txtEffectiveDate.Text)
            Else
                m_dtFutureEffectiveDate = #12/30/1899#
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StoreFutureAddress Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="StoreFutureAddress", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SaveFutureDatedAddress
    '
    ' Description:
    '
    ' History: 23/07/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function SaveFutureDatedAddress() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sTempPostalCode As String = ""
            Dim lBusinessDataID, lAddressCnt As Integer

            sTempPostalCode = m_sFuturePostalCode

            If m_sFutureAddress2 = "" Then
                m_sFutureAddress2 = m_sFutureAddress3
                m_sFutureAddress3 = m_sFutureAddress4
                m_sFutureAddress4 = ""
            End If

            '  Fix postcode if QAS has returned it with an
            '  inappropriate number of spaces
            m_lReturn = PMBGeneralFunc.FormatPostCode(v_sInString:=sTempPostalCode, r_sOutString:=m_sFuturePostalCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Blank the postcode if we are not in Britain.
            If g_sCountryCode.Trim() <> "GBR" Then
                m_sFuturePostalCode = ""
            End If

            lBusinessDataID = 1

            ' Check the task.
            Select Case m_iFutureAddressTask
                Case gPMConstants.PMEComponentAction.PMAdd

                    m_oBusiness.Clear()

                    'developer guide no. 37(guide)
                    m_lReturn = m_oBusiness.EditAdd(lRow:=lBusinessDataID, vAddressCnt:=0, vAddress1:=m_sFutureAddress1, vAddress2:=m_sFutureAddress2, vAddress3:=m_sFutureAddress3, vAddress4:=m_sFutureAddress4, vPostalCode:=m_sFuturePostalCode, vCountryID:=m_lFutureCountryId)

                Case gPMConstants.PMEComponentAction.PMEdit

                    lAddressCnt = CInt(m_vFutureDatedAddresses(ACAddressCnt, 0))


                    'developer guide no. 37(guide)
                    m_lReturn = m_oBusiness.GetDetails(vAddressCnt:=lAddressCnt)


                    'developer guide no. 37(guide)
                    m_lReturn = m_oBusiness.EditUpdate(lRow:=lBusinessDataID, vAddress1:=m_sFutureAddress1, vAddress2:=m_sFutureAddress2, vAddress3:=m_sFutureAddress3, vAddress4:=m_sFutureAddress4, vPostalCode:=m_sFuturePostalCode, vCountryID:=m_lFutureCountryId)

            End Select

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveFutureDatedAddress")
                Return result
            End If

            'Update the database

            m_lReturn = m_oBusiness.Update()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed update database", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveFutureDatedAddress")
                Return result
            End If

            'Now load the array
            ReDim m_vFutureDatedAddresses(6, 0)


            m_vFutureDatedAddresses(ACAddressCnt, 0) = m_oBusiness.AddressCnt
            m_vFutureDatedAddresses(ACOriginalAddressCnt, 0) = m_lAddressCnt
            m_vFutureDatedAddresses(ACEffectiveDate, 0) = m_dtFutureEffectiveDate.ToString("dd MMM yyyy")
            m_vFutureDatedAddresses(ACDateCreated, 0) = DateTime.Now.ToString("dd MMM yyyy")
            m_vFutureDatedAddresses(ACCommitInd, 0) = 0

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveFutureDatedAddress Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveFutureDatedAddress", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function








    ' PRIVATE Methods (End)




    Private Sub cmdAddCon_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddCon.Click

        'Dim iRow As Integer
        Dim oListItem As ListViewItem

        Try

            'Create icontact if not already done so
            If m_oContact Is Nothing Then

                ' Get an instance of the contactinterface object via
                ' the public object manager.
                Dim temp_m_oContact As Object = Nothing
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oContact, sClassName:="iPMBContact.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oContact = temp_m_oContact

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get contacts", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            'set the main postcode and reference
            m_oContact.Reference = pnlConReference.Text
            m_oContact.PostCode = pnlConPostCode.Text

            m_lReturn = m_oContact.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            m_oContact.UniqueId = m_sUniqueId
            m_oContact.ScreenHeirarchy = m_sScreenHeirarchy & $"/AddressType({cboAddUsageType.ItemCaption})"

            m_lReturn = m_oContact.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, add to grid
            If m_oContact.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            'RWH(09/06/2000) - Replaced Grid with ListView.
            oListItem = lvwContacts.Items.Add(m_oContact.AreaCode)

            ' Assign details to other the columns
            ' Column 2
            oListItem.SubItems.Add(1).Text = m_oContact.Number
            ' Column 3
            oListItem.SubItems.Add(2).Text = m_oContact.Extension
            ' Column 4
            oListItem.SubItems.Add(3).Text = m_oContact.ContactType
            ' Column 5
            oListItem.SubItems.Add(4).Text = m_oContact.Description
            ' Store the Address_cnt
            oListItem.Tag = CStr(m_oContact.ContactCnt)

            If lvwContacts.Items.Count > 0 Then
                lvwContacts.Items(0).Selected = True
            End If

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAddCon_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    'sj 23/07/2002 - start
    Private Sub cmdClearFutureAddress_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdClearFutureAddress.Click

        uctadd.AddressLine1 = ""
        uctadd.AddressLine2 = ""
        uctadd.AddressLine3 = ""
        uctadd.AddressLine4 = ""
        uctadd.CountryId = 0
        uctadd.PostCode = ""
        txtEffectiveDate.Text = ""

    End Sub
    'sj 23/07/2002 - end

    Private Sub cmdDeleteCon_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteCon.Click


        Try

            'RWH(14/06/2000) - Conversion from Grid to ListView.
            'Set row to be deleted - if a valid one selected
            If lvwContacts.Items.Count < 1 Then
                Exit Sub
            End If
            'Reset Interface
            cmdEditCon.Enabled = False
            cmdDeleteCon.Enabled = False

            lvwContacts.Items.RemoveAt(lvwContacts.SelectedItems.Item(0).Index)

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDeleteCon_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdEditCon_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditCon.Click

        Dim oListItem As ListViewItem


        Try

            'RWH(09/06/2000) - Replaced grid with ListView.
            If lvwContacts.Items.Count < 1 Then
                Exit Sub
            End If

            'Create icontact if not already done so
            If m_oContact Is Nothing Then

                ' Get an instance of the contactinterface object via
                ' the public object manager.
                Dim temp_m_oContact As Object = Nothing
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oContact, sClassName:="iPMBContact.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oContact = temp_m_oContact

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get contacts", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            'set the main postcode and reference
            m_oContact.Reference = pnlConReference.Text
            m_oContact.PostCode = pnlConPostCode.Text

            m_lReturn = m_oContact.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'set the contact id as stored in the Tag in PopulateContacts.

            m_oContact.ContactCnt = Convert.ToString(lvwContacts.SelectedItems.Item(0).Tag)

            m_oContact.UniqueId = m_sUniqueId
            m_oContact.ScreenHeirarchy = m_sScreenHeirarchy & $"/AddressType({cboAddUsageType.ItemCaption})"
            m_lReturn = m_oContact.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, edit grid
            If m_oContact.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            'RWH(09/06/2000) - Replaced Grid with ListView.
            'Update the contact details
            oListItem = lvwContacts.Items.Item(lvwContacts.SelectedItems.Item(0).Index)
            ' Column 1
            oListItem.Text = m_oContact.AreaCode
            ' Column 2
            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oContact.Number
            ' Column 3
            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oContact.Extension
            ' Column 4
            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oContact.ContactType
            ' Column 5
            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = m_oContact.Description

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdEditCon_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        ' Fire up the help screen
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, lContextID:=MainModule.ScreenHelpID)
    End Sub

    Private Sub cmdPrevious_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdPrevious_1.Click
        Dim Index As Integer = Array.IndexOf(cmdPrevious, eventSender)
        Try

            ' Change to the previous tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) >= SSTabHelper.GetTabCount(tabMainTab) - 1 Then
                SSTabHelper.SetSelectedIndex(tabMainTab, Index - 1)
            End If

            ' Set focus to the first control on the tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                m_ctlTabFirstLast(ACControlStart, Index + 1).Focus()
            End If

        Catch



            ' Error Section

            Exit Sub
        End Try


    End Sub

    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
            If cboAddUsageType.Enabled Then cboAddUsageType.Focus()
        End If
    End Sub

    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRAddress.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMBAddress.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID
            'eck030101
            'm_lReturn = m_oBusiness.GetCountry(iCountryID:=g_iCountryID, sCountryCode:=g_sCountryCode)
            'Changed as country_id is needed for the branch in which user have logged in
            'PN29948

            'developer guide no. 37(guide)
            m_lReturn = m_oBusiness.GetBranchBaseCountry(v_lSourceID:=g_iSourceID, r_iCountryID:=g_iCountryID)
            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ''' <summary>
    ''' Load Form controls and its values
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Dim oOptions As bSIROptions.Business
        Dim sValue As String = ""
        Dim nAddressCnt As Integer

        ' Forms load event.

        Try
            'developer guide no.220
            Me.cboAddUsageType.FirstItem = ""
            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseBusy)

            ' Set the process modes for the busines object.

            'developer guide no. 37(guide)
            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If

            ' Check for errors.
            If m_lReturn <> PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to set the status for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}
            'TF281100 - Moved from Form_Initialize
            ' Get an instance of System Options
            Dim temp_oOptions As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oOptions, "bSIROptions.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oOptions = temp_oOptions
            If m_lReturn <> PMEReturnCode.PMTrue Then
                m_lErrorNumber = PMEReturnCode.PMFalse
                Exit Sub
            End If


            'developer guide no. 37
            m_lReturn = oOptions.GetOption(iOptionNumber:=13, sValue:=sValue)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                m_lErrorNumber = PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Set the correct QAS database
            uctadd.QASDatabaseID = CInt(Conversion.Val(sValue))

            'JMK 28/01/2002
            If Conversion.Val(sValue) <> 0 Then
                uctadd.PMDatabaseID = 0
            Else
                'sj 29/08/2002 - start
                uctadd.PMDatabaseID = 1
                'sj 29/08/2002 - end
            End If
            'JMK 28/01/2002 end

            'DC120706 PN29319 keep QAS setting for checking format of postcode later
            m_sQASInstalled = sValue

            'sj 29/08/2002 - start
            uctadd.CountryId = CountryID
            'sj 29/08/2002 - end

            'DJM 16/09/2003 : Initialise so that postcode field updates when country is changed.
            ' developer guide no. 9
            m_lReturn = uctadd.Initialise()


            oOptions.Dispose()
            oOptions = Nothing
            'End of TF281100


            m_oBusiness.AddressCnt = m_lAddressCnt

            ' {* USER DEFINED CODE (End) *}

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> PMEReturnCode.PMTrue Then
                m_lErrorNumber = PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()

            'sj 23/07/2002 - start
            If m_bFutureDateAddressChanges Then
                'Future dated address changes are turned on
                If Information.IsArray(m_vFutureDatedAddresses) Then

                    'Get the address details associated with this future dated address
                    m_iFutureAddressTask = PMEComponentAction.PMEdit

                    'load the internal collection for this address
                    nAddressCnt = CInt(m_vFutureDatedAddresses(ACAddressCnt, 0))

                    'developer guide no. 37(guide)
                    m_lReturn = m_oBusiness.GetDetails(vAddressCnt:=nAddressCnt)
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="m_oBusiness.GetDetails Failed for Future Dated Address " & nAddressCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
                        Exit Sub
                    End If

                    'developer guide no. 37(guide)
                    m_lReturn = m_oBusiness.GetNext(vAddress1:=m_sFutureAddress1, vAddress2:=m_sFutureAddress2, vAddress3:=m_sFutureAddress3, vAddress4:=m_sFutureAddress4, vPostalCode:=m_sFuturePostalCode, vCountryID:=m_lFutureCountryId)
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="m_oBusiness.GetNext Failed for Future Dated Address " & nAddressCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
                        Exit Sub
                    End If
                    m_dtFutureEffectiveDate = CDate(m_vFutureDatedAddresses(ACEffectiveDate, 0))
                Else
                    m_iFutureAddressTask = PMEComponentAction.PMAdd
                    m_lFutureCountryId = m_lCountryID
                End If
            End If
            'sj 23/07/2002 - end

            ' Check for errors.
            If m_lReturn <> PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Validate fields using Forms Control
            m_lReturn = SetFieldValidation()

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception

            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        Try

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                ' Set the interface status.
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel

                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    'developer guide no. 7
                    eventArgs.Cancel = True
                End If
            End If

        Catch excep As System.Exception



            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Dim iCtrlDown As Integer

        Const ACCtrlMask As Integer = 2

        Try

            ' Set the control key value.
            iCtrlDown = (Shift And ACCtrlMask) > 0

            With tabMainTab
                ' Check the key pressed.
                Select Case KeyCode
                    Case Keys.PageUp
                        ' Page Up key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the first tab.
                            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                        Else
                            ' Check we are not on the
                            ' first tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) > 0 Then
                                ' Display the previous tab.
                                SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) - 1)
                            End If
                        End If

                    Case Keys.PageDown
                        ' Page Down key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the last tab.
                            SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetTabCount(tabMainTab) - 1)
                        Else
                            ' Check we are not on the
                            ' last tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) < (SSTabHelper.GetTabCount(tabMainTab) - 1) Then
                                ' Display the next tab.
                                SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) + 1)
                            End If
                        End If

                    Case Keys.Home
                        ' Home key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                            End If
                        End If

                    Case Keys.End
                        ' End key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlEnd, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                            End If
                        End If
                End Select
            End With
            'Developer Guide No 293
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D2 Then
                tabMainTab.SelectedIndex = 1
            End If
        Catch



            ' Error Section.

            Exit Sub
        End Try


    End Sub

    Private Sub lvwContacts_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwContacts.Click
        If Not (lvwContacts.FocusedItem Is Nothing) Then
            cmdDeleteCon.Enabled = True
            cmdEditCon.Enabled = True
        Else
            cmdDeleteCon.Enabled = False
            cmdEditCon.Enabled = False
        End If

    End Sub

    Private Sub lvwContacts_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwContacts.Enter
        If lvwContacts.Items.Count = 0 Then
            cmdAddCon.Focus()
        End If
    End Sub

    Private isInitializingComponent As Boolean
    Private Sub optAddress_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _optAddress_1.CheckedChanged, _optAddress_0.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If

            If optAddress(ACOptFutureAddress).Checked Then
                m_lReturn = SetUpFutureDatedAddress()
            Else
                m_lReturn = SetUpCurrentAddress()
            End If

        End If
    End Sub

    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        Try

            With tabMainTab

                'DJM 18/11/2003 : Objects selected by tabbing should only be on the currently selected tab.
                SetTabStops(SSTabHelper.GetSelectedIndex(tabMainTab))

                ' Now I know this is crap, this goes against
                ' all my principles, but for some reason when
                ' using the mouse to select a tab the setfocus
                ' code below doesn't work. The cursor sticks,
                ' and you can't tab off. Therefore I've used
                ' this to get around the problem.
                Application.DoEvents()

                ' Set focus to the first control on the tab.
                If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                    m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                End If
            End With

        Catch



            ' Error Section.


            tabMainTabPreviousTab = tabMainTab.SelectedIndex
        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.
        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK


            'sj 23/07/2002 - start
            If m_bFutureDateAddressChanges Then
                'future dated address changes turned on
                If optAddress(ACOptFutureAddress).Checked Then
                    m_lReturn = StoreFutureAddress()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StoreFutureAddress Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                        Exit Sub
                    End If
                Else
                    m_lReturn = StoreCurrentAddress()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StoreCurrentAddress Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                        Exit Sub
                    End If
                End If

                If FutureDatedAddressEntered() Then
                    'Validate future dated address
                    m_lReturn = ValidateInputFields(v_sPostCode:=m_sFuturePostalCode, v_sAddressLine1:=m_sFutureAddress1, v_sAddressLine2:=m_sFutureAddress2, v_sAddressLine4:=m_sFutureAddress4, v_lCountryId:=m_lFutureCountryId, v_vEffectiveDate:=m_dtFutureEffectiveDate, v_sAddressLine3:=m_sFutureAddress3)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        'Field validation error, just exit
                        Exit Sub
                    End If

                End If
            Else
                'No future dated address changes just store the current address
                m_lReturn = StoreCurrentAddress()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StoreCurrentAddress Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                    Exit Sub
                End If
            End If
            'not using this
            '    ' Check mandatory controls have been entered into.
            '    m_lReturn = m_oFormFields.CheckMandatoryControls()
            '
            '    ' Check for errors
            '    If (m_lReturn <> PMTrue) Then
            '      Exit Sub
            '    End If

            If m_iTask = gPMConstants.PMEComponentAction.PMAdd And cboAddUsageType.ItemId = m_lCorrespondanceAddressUsageTypeId And m_bCorrespondanceAddressExists Then
                MessageBox.Show("You can have only one address of type '" & gSIRLibrary.SIRMainAddressABIDescription & "'", "Address Usage Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If


            ' START CHANGES - Changed By: AAB  - Changed On: 19-Mar-2004 10:06
            ' We need to pass in the state to ensure one is selected for USA

            'developer guide no. 37
            m_lReturn = m_oBusiness.GetCountry(iCountryID:=m_lCurrentCountryId, sCountryCode:=g_sCountryCode)

            'Validate the current address
            If g_sCountryCode.Trim() = "USA" Then
                m_lReturn = ValidateInputFields(v_sPostCode:=m_sCurrentPostalCode, v_sAddressLine1:=m_sCurrentAddress1, v_sAddressLine2:=m_sCurrentAddress2, v_sAddressLine4:=m_sCurrentAddress4, v_sAddressLine3:=m_sCurrentAddress3, v_lCountryId:=m_lCurrentCountryId)
            Else
                m_lReturn = ValidateInputFields(v_sPostCode:=m_sCurrentPostalCode, v_sAddressLine1:=m_sCurrentAddress1, v_sAddressLine2:=m_sCurrentAddress2, v_sAddressLine4:=m_sCurrentAddress4, v_lCountryId:=m_lCurrentCountryId, v_sAddressLine3:=m_sCurrentAddress3)
            End If
            ' END CHANGES - Changed By: AAB  - Changed On: 19-Mar-2004 10:06
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Field validation error, just exit
                Exit Sub
            End If

            '    'MS210501 Check if we are in Great Britain
            '    m_lReturn = m_oBusiness.getcountry(iCountryID:=uctAdd.CountryID, sCountryCode:=g_sCountryCode)
            '
            '    'eck030101
            '   'sj 18/06/2002 - start
            '    If m_bIsNRMA = False Then
            '   'sj 18/06/2002 - end
            '        If Trim$(g_sCountryCode) = "GBR" Then
            '            m_lReturn& = CheckValidPostCode(v_sPostCode:=uctAdd.PostCode)
            '            If (m_lReturn& <> PMTrue) Then
            '                MsgBox "The post code '" & uctAdd.PostCode & "' is not of a valid format.", vbExclamation, "Post Code - " & uctAdd.PostCode
            '                Exit Sub
            '            End If
            '    'check for spaces in postcode
            '            If (Trim$(uctAdd.PostCode) = "") Then
            '                MsgBox "You must supply a post code"
            '                uctAdd.SetFocus
            '                tabMainTab.Tab = 0
            '                Exit Sub
            '            End If
            '        End If
            '    End If
            '    'Validate the fields in the address control too
            '    'sp todo - remove this when validation goes into Address User Control
            '    If m_bIsNRMA = True Then
            '        If (Trim$(uctAdd.AddressLine1) = "") Then
            '            MsgBox "You must supply Property Name"
            '            uctAdd.SetFocus
            '            tabMainTab.Tab = 0
            '            Exit Sub
            '        End If
            '        If (Trim$(uctAdd.AddressLine2) = "") Then
            '            MsgBox "You must supply Street/PO Box"
            '            uctAdd.SetFocus
            '            tabMainTab.Tab = 0
            '            Exit Sub
            '        End If
            '        If (Trim$(uctAdd.AddressLine4) = "") Then
            '            MsgBox "You must supply City"
            '            uctAdd.SetFocus
            '            tabMainTab.Tab = 0
            '            Exit Sub
            '        End If
            '    Else
            '        If (Trim$(uctAdd.AddressLine1) = "") Then
            '            MsgBox "You must supply first address line"
            '            uctAdd.SetFocus
            '            tabMainTab.Tab = 0
            '            Exit Sub
            '        End If
            '    End If
            'sj 23/07/2002 - end

            'Validate some address specific stuff
            m_lReturn = ValidateOK()

            Select Case m_lReturn
                Case ACAddressEdit
                    'Fine continue and edit the address

                Case ACAddressAdd
                    'Fine continue and add the address

                Case ACAddressDoNotAdd
                    'DJM 24/09/2003 : Adding usage of address that already exists. No need to actually add the address.
                    m_lReturn = InterfaceToData()

                    m_lAddressCnt = m_oBusiness.AddressCnt
                    Me.Hide()
                    Exit Sub

                Case ACAddressCancel
                    Exit Sub

                Case Else
                    Exit Sub
            End Select

            'sj 23/07/2002 - start
            If m_bFutureDateAddressChanges Then
                If FutureDatedAddressEntered() Then
                    'save future dated address
                    m_lReturn = SaveFutureDatedAddress()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveFutureDatedAddress Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                        Exit Sub
                    End If
                Else
                    'developer guide no solution 27
                    'm_vFutureDatedAddresses = VB6.CopyArray("")
                End If
                'reload the original details

                'developer guide no. 37(guide)
                m_lReturn = m_oBusiness.GetDetails(vAddressCnt:=m_lAddressCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oBusiness.GetDetails Failed for Address_cnt " & m_lAddressCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                    Exit Sub
                End If
            End If
            'sj 23/07/2002 - end


            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                'update the contact cnt property

                m_lAddressCnt = m_oBusiness.AddressCnt

                'Update New Contacts
                m_lReturn = UpdateContacts()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If

                ' Everything OK, so we can hide the interface.
                Me.Hide()

            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    'UPGRADE_NOTE: (7001) The following declaration (cmdNavigate_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub cmdNavigate_Click()
    '
    ' Click event of the Navigate button.
    '
    'Try 
    '
    ' Set the interface status.
    'm_lStatus = gPMConstants.PMEReturnCode.PMNavigate
    '
    ' Process the next set of actions depending
    ' upon the interface task etc.
    'm_lReturn = m_oGeneral.ProcessCommand()
    '
    ' Check the return value.
    'If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
    ' Everything OK, so we can hide the interface.
    'Me.Hide()
    'End If
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    'End Sub

    Private Sub cmdNext_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdNext_0.Click
        Dim Index As Integer = Array.IndexOf(cmdNext, eventSender)

        Try

            ' Change to the next tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) < SSTabHelper.GetTabCount(tabMainTab) - 1 Then
                SSTabHelper.SetSelectedIndex(tabMainTab, Index + 1)
            End If

            ' Set focus to the first control on the tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                m_ctlTabFirstLast(ACControlStart, Index + 1).Focus()
            End If

        Catch



            ' Error Section

            Exit Sub
        End Try


    End Sub

    ' PRIVATE Events (End)

    Private Sub txtEffectiveDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEffectiveDate.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtEffectiveDate)
    End Sub

    Private Sub txtEffectiveDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEffectiveDate.Leave
        If Not (m_oFormFields Is Nothing) Then
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtEffectiveDate)
        End If
    End Sub

    'DJM 18/11/2003 : Objects selected by tabbing should only be on the currently selected tab.
    Private Sub SetTabStops(ByRef iCurrentTab As Integer)
        Dim bOnAddressTab As Boolean

        bOnAddressTab = iCurrentTab = 0

        'Address Tab
        uctadd.TabStop = bOnAddressTab
        cmdNext(0).TabStop = bOnAddressTab
        cboAddUsageType.TabStop = bOnAddressTab
        txtEffectiveDate.TabStop = bOnAddressTab And txtEffectiveDate.Visible
        cmdClearFutureAddress.TabStop = bOnAddressTab And cmdClearFutureAddress.Visible
        optAddress(0).TabStop = bOnAddressTab And optAddress(0).Visible
        optAddress(1).TabStop = bOnAddressTab And optAddress(1).Visible
        cmdClearFutureAddress.TabStop = bOnAddressTab And cmdClearFutureAddress.Visible

        'Contact Tab
        cmdAddCon.TabStop = Not bOnAddressTab
        cmdEditCon.TabStop = Not bOnAddressTab
        cmdDeleteCon.TabStop = Not bOnAddressTab
        lvwContacts.TabStop = Not bOnAddressTab
    End Sub

    Private Function StripTrailingColon(ByVal Caption As String) As String
        Caption = Caption.Trim()
        If Caption.EndsWith(":") Then
            Return Caption.Substring(0, Caption.Length - 1)
        Else
            Return Caption
        End If
    End Function

    Private Sub uctadd_AddressCleared(ByVal Sender As Object, ByVal e As EventArgs) Handles uctadd.AddressCleared
        'PN:45199 Clear pnlAdPostcode and pnlConPostCode also
        pnlAdPostcode.Text = ""
        pnlConPostCode.Text = ""
    End Sub

    Private Sub uctadd_ChosenAddress(ByVal Sender As Object, ByVal e As EventArgs) Handles uctadd.ChosenAddress
        'PN:45199 Update pnlAdPostcode and pnlConPostCode as per uctadd control
        If uctadd.IsPostCodeRequired = 1 Then
            pnlAdPostcode.Text = uctadd.PostCode
            pnlConPostCode.Text = uctadd.PostCode
        Else
            pnlAdPostcode.Text = ""
            pnlConPostCode.Text = ""
        End If
    End Sub

    Private Sub lvwContacts_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lvwContacts.MouseDown
        Dim Button As Integer = CInt(e.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(e.X)
        Dim y As Single = VB6.PixelsToTwipsY(e.Y)

        If Not (lvwContacts.GetItemAt(x, y) Is Nothing) Then
            cmdDeleteCon.Enabled = True
            cmdEditCon.Enabled = True
        Else
            cmdDeleteCon.Enabled = False
            cmdEditCon.Enabled = False
        End If

    End Sub
End Class
