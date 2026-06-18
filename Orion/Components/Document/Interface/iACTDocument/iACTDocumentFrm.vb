Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 16/09/97
    '
    ' Description: Main interface.
    '
    ' Edit History: KN(CMG) 26/02/03 PN2486 Recurring Journals Effective
    '               Date determined by txtDocumentDate
    '               CJB     28/09/05 PN24364 Removed all client manager
    '               security checks from cmbDocumentType_LostFocus as they
    '               are now done only via client manager.
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"


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

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iACTDocument.General
    'developer guide no.88
    Private m_oInterface As Object
    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    ' Form control object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Audit Set
    Private m_oAuditSet As Object

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lDocumentID As Integer
    Private m_iPostingstatusID As Integer
    Private m_iDocumenttypeID As Integer
    Private m_lAuditsetID As Integer
    Private m_lBatchID As Integer
    Private m_sDocumentRef As String = ""
    Private m_dtDocumentDate As Date
    Private m_dtCreatedDate As Date
    Private m_dtAuthorisedDate As Date
    Private m_sComment As String = ""

    Private m_bReversingDocument As Boolean
    Private m_lReversingDocumentId As Integer
    Private m_dtReverseDate As Date
    Private m_bRecurringDocument As Boolean
    Private m_iOccurances As Integer
    Private m_vRecurringDocumentIDs() As Object
    Private m_vRecurringDocumentDates() As Object
    'eck110500
    Private m_iCompanyID As Integer
    'EK 230200
    Private m_sGroupCode As String = ""
    Private m_sRangeCode As String = ""
    Private m_lNumber As Integer
    '2005 Client Manager Security
    Private m_vDebitDocumentTypes As Object
    Private m_vCreditDocumentTypes As Object
    Private m_vFeeDocumentTypes As Object
    Private m_vCashDocumentTypes As Object
    Private m_vManualDIDDocumentTypes As Object
    'Developer Guide No.7
    Private Const vbFormCode As Integer = 0
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

    ' {* USER DEFINED CODE (Begin) *}
    'eck040500
    Public WriteOnly Property CompanyID() As Integer
        Set(ByVal Value As Integer)

            ' Set the valid sources for the user
            m_iCompanyID = Value

        End Set
    End Property


    Public Property DocumentId() As Integer
        Get
            Return m_lDocumentID
        End Get
        Set(ByVal Value As Integer)
            m_lDocumentID = Value
        End Set
    End Property
    Public Property DocumenttypeID() As Integer
        Get
            Return m_iDocumenttypeID
        End Get
        Set(ByVal Value As Integer)
            m_iDocumenttypeID = Value
        End Set
    End Property
    'JK091298
    Public Property DocumentDate() As Date
        Get
            ' Return the effective date.
            Return m_dtDocumentDate
        End Get
        Set(ByVal Value As Date)
            ' Set the object parameter value.
            m_dtDocumentDate = Value
        End Set
    End Property
    'JK091298
    Public Property DocumentRef() As String
        Get
            ' Return the objects parameter value.
            Return m_sDocumentRef
        End Get
        Set(ByVal Value As String)
            ' Set the object parameter value.
            m_sDocumentRef = Value
        End Set
    End Property
    'JK091298

    Public Property Comment() As String
        Get
            Return m_sComment
        End Get
        Set(ByVal Value As String)
            m_sComment = Value
        End Set
    End Property
    'JK091298

    Public Property Postingstatus() As Integer
        Get
            Return m_iPostingstatusID
        End Get
        Set(ByVal Value As Integer)
            m_iPostingstatusID = Value
        End Set
    End Property

    ' CF240998
    Public Property ReversingDocument() As Boolean
        Get
            Return m_bReversingDocument
        End Get
        Set(ByVal Value As Boolean)
            m_bReversingDocument = Value
        End Set
    End Property

    ' CF240998
    Public Property ReversingDocumentID() As Integer
        Get
            Return m_lReversingDocumentId
        End Get
        Set(ByVal Value As Integer)
            m_lReversingDocumentId = Value
        End Set
    End Property

    ' CF240998
    Public Property ReverseDate() As Date
        Get
            Return m_dtReverseDate
        End Get
        Set(ByVal Value As Date)
            m_dtReverseDate = Value
        End Set
    End Property

    ' CF240998
    Public Property RecurringDocument() As Boolean
        Get
            Return m_bRecurringDocument
        End Get
        Set(ByVal Value As Boolean)
            m_bRecurringDocument = Value
        End Set
    End Property

    ' CF240998
    Public Property Occurances() As Integer
        Get
            Return m_iOccurances
        End Get
        Set(ByVal Value As Integer)
            m_iOccurances = Value
        End Set
    End Property

    ' CF240998
    Public Property RecurringDocumentIDs() As Object
        Get
            Return VB6.CopyArray(m_vRecurringDocumentIDs)
        End Get
        Set(ByVal Value As Object)
            m_vRecurringDocumentIDs = Value
        End Set
    End Property

    ' CF240998
    Public Property RecurringDocumentDates() As Object
        Get
            Return VB6.CopyArray(m_vRecurringDocumentDates)
        End Get
        Set(ByVal Value As Object)
            m_vRecurringDocumentDates = Value
        End Set
    End Property
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)
    'eck110500
    ' ***************************************************************** '
    ' Name: GetCompany (Standard Method)
    '
    ' Description: Gets valid Source ID's  and if nessessary displays selection
    '
    ' ***************************************************************** '
    Public Function GetCompany(ByRef m_iCompanyID As Integer) As Integer
        Dim result As Integer = 0
        Dim m_oBranch As Object
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_m_oBranch As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBranch, sClassName:="iPMBBranch.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oBranch = temp_m_oBranch

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'RKS PN14431
            'changed from GetSource to GetSourceAccounts
            'GetSourceAccounts will take care of selection of
            'closed branches also if accounting is allowed on that Branch

            m_lReturn = m_oBranch.GetSourceAccounts(iSourceID:=m_iCompanyID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oBranch.Dispose()
            m_oBranch = Nothing
            Return result

        Catch excep As System.Exception



            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Company", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCompany", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'eck110500

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


            m_lReturn = m_oBusiness.GetDetails(vDocumentId:=m_lDocumentID)

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
    ' Name: BusinessToInterface
    '
    ' Description: Updates all interface details from the business
    '              object.
    '
    ' ***************************************************************** '
    Public Function BusinessToInterface() As Integer

        Dim result As Integer = 0
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

            ' Assign the details to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            '    Set m_oInterface = New iACTDocument.Interface
            '
            ''        m_lReturn = m_oInterface.Initialise()
            '
            '    If (m_lReturn <> PMTrue) Then
            '        BusinessToInterface = PMFalse
            '        Exit Function
            '    End If
            '
            cmbDocumentType.ItemId = m_iDocumenttypeID

            txtDocumentRef.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sDocumentRef)

            txtDocumentDate.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, vFieldValue:=m_dtDocumentDate)

            txtComment.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sComment)

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
    ' History: CF181198 - Added vWriteOffReasonID parameters
    '
    ' ***************************************************************** '
    Public Function InterfaceToBusiness() As Integer

        Dim result As Integer = 0
        Dim lBusinessDataID, lAuditSetID As Integer
        Dim dtReverseDate, dtRecurringDate, dtLastDate As Date
        Dim iOccurances As Integer
        Dim sDocumentRef, sComment As String
        Dim vOffset As String = ""
        Dim vMonths As Byte 'KN(CMG) 26/02/03 PN2486
        Dim vWriteOffReasonID As Byte
        Dim dtAuthorisedDate As Date

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

            ' Set the authorised date
            dtAuthorisedDate = DateTime.Now

            ' Set the write off reason to be null
            vWriteOffReasonID = 0

            ' Set the business data ID to one because we are only
            ' dealing with one record item only.
            lBusinessDataID = 1

            ' Check the task.

            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd

                    ' {* USER DEFINED CODE (Begin) *}

                    ' Inform the business object with a new data item.
                    m_dtCreatedDate = DateTime.Now
                    m_iPostingstatusID = gACTLibrary.ACTPostStatusRegistered

                    ' Recurring document
                    If m_bRecurringDocument Then
                        'eck110500 replaced g_iCompanyID with m_iCompanyID

                        m_lReturn = m_oAuditSet.DirectAdd(vAuditsetID:=lAuditSetID, vCompanyID:=m_iCompanyID, vUserId:=g_iUserID, vPostedDate:=DateTime.Now, vComment:="Recurring Document")
                    End If

                    ' Reversing document
                    If m_bReversingDocument Then
                        'eck110500 replaced g_iCompanyID with m_iCompanyID

                        m_lReturn = m_oAuditSet.DirectAdd(vAuditsetID:=lAuditSetID, vCompanyID:=m_iCompanyID, vUserId:=g_iUserID, vPostedDate:=DateTime.Now, vComment:="Reversing Document")
                    End If

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse

                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")

                        Return result
                    End If


                    If Not m_bRecurringDocument Then
                        'BB Use Direct Add as we need the ID back
                        'eck110500 replaced g_iCompanyID with m_iCompanyID

                        m_lReturn = m_oBusiness.DirectAdd(vDocumentId:=m_lDocumentID, vCompanyID:=m_iCompanyID, vPostingstatusID:=m_iPostingstatusID, vDocumenttypeID:=m_iDocumenttypeID, vAuditsetID:=lAuditSetID, vDocumentRef:=m_sDocumentRef, vDocumentDate:=m_dtDocumentDate, vCreatedDate:=m_dtCreatedDate, vAuthorisedDate:=dtAuthorisedDate, vComment:=m_sComment, vWriteOffReasonID:=vWriteOffReasonID)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")

                            Return result
                        End If
                    End If

                    ' its a recurring document
                    If m_bRecurringDocument Then

                        ' temp
                        dtLastDate = m_dtDocumentDate

                        If optOccurs(0).Checked Then
                            ' Per Period
                            vOffset = txtOccursPer(0).Text
                            vMonths = 0 'KN(CMG) 26/02/03 PN2486
                        ElseIf (optOccurs(1).Checked) Then
                            ' Per Month
                            vOffset = txtOccursPer(1).Text
                            'KN(CMG) 26/02/03 PN2486 start
                            m_lReturn = GetMonthsForward(r_vNextDate:=dtLastDate, v_vOffset:=vOffset, v_vMonths:=vMonths)
                            '                    If Day(dtLastDate) > CInt(vOffset) Then
                            '                        vMonths = 1
                            '                    Else
                            '                        vMonths = 0
                            '                    End If
                            'KN(CMG) 26/02/03 PN2486 end
                        ElseIf (optOccurs(2).Checked) Then
                            ' Per Quarter
                            vOffset = txtOccursPer(2).Text
                            'KN(CMG) 26/02/03 PN2486 start
                            m_lReturn = GetMonthsForward(r_vNextDate:=dtLastDate, v_vOffset:=vOffset, v_vMonths:=vMonths)
                            '                    If Day(dtLastDate) > CInt(vOffset) Then
                            '                        vMonths = 1
                            '                    Else
                            '                        vMonths = 0
                            '                    End If
                            'KN(CMG) 26/02/03 PN2486 end
                        Else
                            ' Something's wrong
                            ' dtRecurringDate = Now
                            vOffset = CStr(0)
                        End If

                        ' Get the date this month with the day set to what the user wants
                        'KN(CMG) 26/02/03 PN2486 start
                        'm_lReturn = m_oBusiness.GetDatePlusXMonths( _
                        'v_vCurrentDate:=Now, _
                        'r_vNextDate:=dtLastDate, _
                        'v_vOffset:=vOffset, _
                        'v_vMonths:=0)

                        m_lReturn = m_oBusiness.GetDatePlusXMonths(v_vCurrentDate:=DateTime.Now, r_vNextDate:=dtLastDate, v_vOffset:=vOffset, v_vMonths:=vMonths)
                        'KN(CMG) 26/02/03 PN2486 end
                        'eck110500 replaced g_iCompanyID with m_iCompanyID

                        m_lReturn = m_oBusiness.DirectAdd(vDocumentId:=m_lDocumentID, vCompanyID:=m_iCompanyID, vPostingstatusID:=m_iPostingstatusID, vDocumenttypeID:=m_iDocumenttypeID, vAuditsetID:=lAuditSetID, vDocumentRef:=m_sDocumentRef, vDocumentDate:=dtLastDate, vCreatedDate:=m_dtCreatedDate, vAuthorisedDate:=dtAuthorisedDate, vComment:=m_sComment, vWriteOffReasonID:=vWriteOffReasonID)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")

                            Return result
                        End If

                        ' Add recurred documents
                        iOccurances = m_iOccurances

                        ReDim m_vRecurringDocumentIDs(0)
                        ReDim m_vRecurringDocumentDates(0)

                        ' Add ID to the array
                        m_vRecurringDocumentIDs(0) = m_lDocumentID

                        ' Add Date to the array
                        ' CF 220399 Fixed for UAT 98
                        m_vRecurringDocumentDates(0) = dtLastDate

                        For iLoop1 As Integer = 1 To iOccurances - 1

                            ' Call the business to generate the document reference
                            'EK 230200
                            'eck180500 Pass Company
                            ' Call the business to generate the document reference
                            'Start(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                            'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber

                            m_lReturn = m_oBusiness.GenerateDocumentReferenceNumber(v_sGroupCode:=m_sGroupCode, v_sRangeCode:=m_sRangeCode, v_iUserID:=g_iUserID, v_iCompanyID:=m_iCompanyID, r_sDocumentRef:=sDocumentRef)
                            'End(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            ' Prefix a few zeros
                            '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                            sDocumentRef = m_sRangeCode & sDocumentRef

                            ' Get Recurring Date
                            If optOccurs(0).Checked Then
                                ' Per Period

                                m_lReturn = m_oBusiness.GetDateNext(v_iNextType:=1, v_vCurrentDate:=dtLastDate, v_vOffset:=vOffset, r_vNextDate:=dtRecurringDate)

                            ElseIf (optOccurs(1).Checked) Then
                                ' Per Month

                                m_lReturn = m_oBusiness.GetDateNext(v_iNextType:=2, v_vCurrentDate:=dtLastDate, v_vOffset:=vOffset, r_vNextDate:=dtRecurringDate)

                            ElseIf (optOccurs(2).Checked) Then
                                ' Per Quarter

                                m_lReturn = m_oBusiness.GetDateNext(v_iNextType:=3, v_vCurrentDate:=dtLastDate, v_vOffset:=vOffset, r_vNextDate:=dtRecurringDate)
                            Else
                                ' Something's wrong
                                dtRecurringDate = DateTime.Now

                            End If

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            ' Add the document
                            'eck110500 replaced g_iCompanyID with m_iCompanyID

                            m_lReturn = m_oBusiness.DirectAdd(vDocumentId:=m_lDocumentID, vCompanyID:=m_iCompanyID, vPostingstatusID:=m_iPostingstatusID, vDocumenttypeID:=m_iDocumenttypeID, vAuditsetID:=lAuditSetID, vDocumentRef:=sDocumentRef, vDocumentDate:=dtRecurringDate, vCreatedDate:=m_dtCreatedDate, vAuthorisedDate:=dtAuthorisedDate, vComment:=m_sComment, vWriteOffReasonID:=vWriteOffReasonID)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse

                                ' Log Error.
                                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")

                                Return result
                            End If

                            ' Add ID to the array
                            ReDim Preserve m_vRecurringDocumentIDs(iLoop1)
                            m_vRecurringDocumentIDs(iLoop1) = m_lDocumentID

                            ' Add Date to the array
                            ReDim Preserve m_vRecurringDocumentDates(iLoop1)
                            m_vRecurringDocumentDates(iLoop1) = dtRecurringDate

                            dtLastDate = dtRecurringDate

                        Next iLoop1

                    End If

                    If m_bReversingDocument Then
                        ' Add the reversing document

                        dtReverseDate = CDate(txtReverseDate.Text)

                        ' Set the comment
                        sComment = "Reverses " & m_sDocumentRef
                        'EK 230200
                        'eck180500 New company parameter
                        ' Call the business to generate the number
                        'Start (Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                        'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber

                        m_lReturn = m_oBusiness.GenerateDocumentReferenceNumber(v_sGroupCode:=m_sGroupCode, v_sRangeCode:=m_sRangeCode, v_iUserID:=g_iUserID, v_iCompanyID:=m_iCompanyID, r_sDocumentRef:=sDocumentRef)
                        'End (Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        ' Prefix a few zeros
                        'EK 230200      '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                        sDocumentRef = m_sRangeCode & sDocumentRef

                        'SP010299
                        'eck110500 replaced g_iCompanyID with m_iCompanyID

                        m_lReturn = m_oBusiness.DirectAdd(vDocumentId:=m_lReversingDocumentId, vCompanyID:=m_iCompanyID, vPostingstatusID:=m_iPostingstatusID, vDocumenttypeID:=m_iDocumenttypeID, vAuditsetID:=lAuditSetID, vDocumentRef:=sDocumentRef, vDocumentDate:=dtReverseDate, vCreatedDate:=m_dtCreatedDate, vAuthorisedDate:=dtAuthorisedDate, vComment:=sComment, vWriteOffReasonID:=vWriteOffReasonID)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")

                            Return result
                        End If

                    End If

                    ' {* USER DEFINED CODE (End) *}

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Inform the business object with an updated data item.
                    'JK091298 - Pass in the current postingstatus + documentType
                    m_iPostingstatusID = Postingstatus
                    m_iDocumenttypeID = DocumenttypeID

                    ' {* USER DEFINED CODE (Begin) *}
                    'eck110500 replaced g_iCompanyID with m_iCompanyID

                    m_lReturn = m_oBusiness.EditUpdate(lRow:=lBusinessDataID, vDocumentId:=m_lDocumentID, vCompanyID:=m_iCompanyID, vPostingstatusID:=m_iPostingstatusID, vDocumenttypeID:=m_iDocumenttypeID, vDocumentRef:=m_sDocumentRef, vDocumentDate:=m_dtDocumentDate, vCreatedDate:=m_dtCreatedDate, vComment:=m_sComment, vWriteOffReasonID:=vWriteOffReasonID)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse

                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")

                        Return result
                    End If

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
    ' Name: GetMonthsForward
    '
    ' Description: Determines the month in which the journal is to be transacted
    '
    ' ***************************************************************** '

    Private Function GetMonthsForward(ByRef r_vNextDate As Date, ByRef v_vOffset As Object, ByRef v_vMonths As Double) As Integer

        Dim result As Integer = 0
        Dim iYear, iMonth As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iYear = r_vNextDate.Year - DateTime.Now.Year
            iMonth = r_vNextDate.Month - DateTime.Now.Month

            If iMonth > -1 And iYear = 0 Then

                If DateAndTime.Day(r_vNextDate) > CInt(v_vOffset) Then
                    v_vMonths = iMonth + 1
                Else
                    v_vMonths = iMonth
                End If
            Else

                If DateAndTime.Day(r_vNextDate) > CInt(v_vOffset) Then
                    v_vMonths = (iYear * 12) + iMonth + 1
                Else
                    v_vMonths = (iYear * 12) + iMonth
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMonthsForwardFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMonthsForward", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        'TF061102 - change to dynamic array
        Dim sInvalidTypes() As Object

        Dim oUserAuthorities As Object
        Dim vHasWriteOffAuthority As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            'Don't need to there are none on this form
            '    m_lReturn = GetLookupValues()

            ' Check for errors.
            '    If (m_lReturn <> PMTrue) Then
            '        DisplayLookupDetails = PMFalse
            '        Exit Function
            '    End If

            ' Get all of the lookup details.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to retreive all of the lookup
            ' descriptions for a given lookup type.
            ' The GetLookupDetails function will allow you to do this.
            '
            ' Example:-
            '
            '    m_lReturn = GetLookupDetails( _
            ''        sLookupTable:=PMLookupCodeName, _
            ''        ctlLookup:=cmbCodeName)
            '
            '    ' Check for errors.
            '    If (m_lReturn <> PMTrue) Then
            '        DisplayLookupDetails = PMFalse
            '        Exit Function
            '    End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************
            'eck180800 Code to remove some document types from the list
            ReDim sInvalidTypes(18)

            sInvalidTypes(1) = "Debit note"

            sInvalidTypes(2) = "Credit note"

            sInvalidTypes(3) = "NB Debit"

            sInvalidTypes(4) = "NB Credit"

            sInvalidTypes(5) = "Renewal Debit"

            sInvalidTypes(6) = "Renewal Credit"

            sInvalidTypes(7) = "Endorsement Debit"

            sInvalidTypes(8) = "Endorsement Credit"

            sInvalidTypes(9) = "Agency Takeover"

            sInvalidTypes(10) = "Sales Invoice"

            sInvalidTypes(11) = "Short Period Debit"

            sInvalidTypes(12) = "Short Period Credit"

            sInvalidTypes(13) = "Direct To Insurer"

            sInvalidTypes(14) = "Direct To Insurer Credit"

            sInvalidTypes(15) = "Transferred Debit"

            sInvalidTypes(16) = "Transferred Credit"
            'eck060801 Remove more document types from the list

            sInvalidTypes(17) = "Cash Credit"

            sInvalidTypes(18) = "Cash Debit"

            'TF061102 - Write offs also invalid if not authorised
            Dim temp_oUserAuthorities As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oUserAuthorities, "bACTUserAuthorities.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oUserAuthorities = temp_oUserAuthorities
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bACTUserAuthorities.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails")
                Return result
            End If


            m_lReturn = oUserAuthorities.GetDetails(vUserId:=g_iUserID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process oUserAuthorities.GetDetails(" & g_iUserID & ")", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails")

                oUserAuthorities.Dispose()
                oUserAuthorities = Nothing
                Return result
            End If


            m_lReturn = oUserAuthorities.GetNext(vHasWriteOffAuthority:=vHasWriteOffAuthority)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process oUserAuthorities.GetNext()", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails")

                oUserAuthorities.Dispose()
                oUserAuthorities = Nothing
                Return result
            End If

            If gPMFunctions.NullToInteger(vHasWriteOffAuthority) <> 1 Then
                ReDim Preserve sInvalidTypes(19)

                sInvalidTypes(19) = "Write Off"
            End If


            oUserAuthorities.Dispose()
            oUserAuthorities = Nothing

            For iInvalidType As Integer = 1 To sInvalidTypes.GetUpperBound(0)
                For ilookupCount As Integer = 1 To cmbDocumentType.ListCount
                    If cmbDocumentType.List(ilookupCount) = sInvalidTypes(iInvalidType) Then

                        cmbDocumentType.RemoveItem(ilookupCount)
                        Exit For
                    End If
                Next ilookupCount
            Next iInvalidType

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

    ' ***************************************************************************
    '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************************
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            m_oFormFields = New iPMFormControl.FormFields()

            m_oFormFields.LanguageID = g_iLanguageID

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add the controls...

            ' Reference
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDocumentRef, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Document Date
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDocumentDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Comments
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtComment, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Reversal date
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtReverseDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Occurances
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOccurs, lFieldType:=gPMConstants.PMEDataType.PMInteger, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' txtOccursPer
            ' Commented out till index is handled by FormControl
            '    m_lReturn = m_oFormFields.AddNewFormField( _
            ''        ctlControl:=txtOccursPer(0), _
            ''        lFieldType:=PMInteger, _
            ''        lFormat:=PMFormatInteger, _
            ''        lMandatory:=PMNonMandatory)
            '    If (m_lReturn <> PMTrue) Then
            '        SetFieldValidation = PMFalse
            '        Exit Function
            '    End If
            '
            '    m_lReturn = m_oFormFields.AddNewFormField( _
            ''        ctlControl:=txtOccursPer(1), _
            ''        lFieldType:=PMInteger, _
            ''        lFormat:=PMFormatInteger, _
            ''        lMandatory:=PMNonMandatory)
            '    If (m_lReturn <> PMTrue) Then
            '        SetFieldValidation = PMFalse
            '        Exit Function
            '    End If
            '
            '    m_lReturn = m_oFormFields.AddNewFormField( _
            ''        ctlControl:=txtOccursPer(2), _
            ''        lFieldType:=PMInteger, _
            ''        lFormat:=PMFormatInteger, _
            ''        lMandatory:=PMNonMandatory)
            '    If (m_lReturn <> PMTrue) Then
            '        SetFieldValidation = PMFalse
            '        Exit Function
            '    End If

            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: BusinessToData
    '
    ' Description: Updates the data storage from the business object.
    '
    ' ***************************************************************** '
    Private Function BusinessToData() As Integer

        Dim result As Integer = 0
        Try


            ' Assign the details to the data storage.

            ' {* USER DEFINED CODE (Begin) *}


            ' {* USER DEFINED CODE (End) *}

            ' Check for errors.
            '    If (m_lReturn <> PMTrue) Then
            '        BusinessToData = PMFalse
            '
            '        ' Log Error.
            '        LogMessage _
            ''            iType:=PMLogError, _
            ''            sMsg:="Failed to retreive the details from the business object", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="BusinessToData"
            '    End If

            Return gPMConstants.PMEReturnCode.PMTrue

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

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the data storage.

            ' {* USER DEFINED CODE (Begin) *}
            'eck060801
            '    m_sDocumentRef = Trim$(txtDocumentRef.Text)
            'eck PN8449
            If Information.IsDate(txtDocumentDate.Text) Then
                m_dtDocumentDate = (CDate(gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, vFieldValue:=CDate(txtDocumentDate.Text))))
            Else
                txtDocumentDate.Text = DateTimeHelper.ToString(DateTime.Today)
                m_dtDocumentDate = (CDate(gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, vFieldValue:=CDate(txtDocumentDate.Text))))
            End If

            m_sComment = txtComment.Text.Trim()
            m_iDocumenttypeID = cmbDocumentType.ItemId

            ' Reversing document
            If txtReverseDate.Enabled Then
                m_bReversingDocument = True
                ' Get the reverse date
                If Information.IsDate(txtReverseDate.Text) Then
                    m_dtReverseDate = (CDate(gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, vFieldValue:=CDate(txtReverseDate.Text))))
                End If
            Else
                m_bReversingDocument = False
            End If

            ' Recurring document
            If txtOccurs.Enabled Then
                m_bRecurringDocument = True
                ' Get the occurances
                Dim dbNumericTemp As Double
                If Double.TryParse(txtOccurs.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    m_iOccurances = CInt(txtOccurs.Text)
                End If
            Else
                m_bRecurringDocument = False
            End If


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
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            'm_lReturn = DisplayCaptions()

            ' Check for errors.
            'If (m_lReturn <> PMTrue) Then
            'SetInterfaceDefaults = PMFalse
            'Exit Function
            'End If

            ' Set the status of the Navigate button.
            Select Case (m_lNavigate)
                Case gPMConstants.PMENavigateButtonStatus.PMNavigateEnabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = True

                Case gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = False

                Case Else
                    cmdNavigate.Visible = False

            End Select

            'eck PN8449
            txtDocumentDate.Text = DateTimeHelper.ToString(DateTime.Today)


            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}
            ' Setup default data for Add
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then

                cmbDocumentType.ItemId = gACTLibrary.ACTDocTypeJournal
                txtDocumentDate.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, DateTime.Today)
                'eck110500 replaced g_iCompanyID with m_iCompanyID

                m_lReturn = m_oBusiness.GetNext(vDocumentId:=m_lDocumentID, vCompanyID:=m_iCompanyID, vPostingstatusID:=m_iPostingstatusID, vDocumenttypeID:=m_iDocumenttypeID, vDocumentRef:=m_sDocumentRef, vDocumentDate:=m_dtDocumentDate, vCreatedDate:=m_dtCreatedDate, vAuthorisedDate:=m_dtAuthorisedDate, vComment:=m_sComment)
                'EK 220200 Don't do this until we have the document type
                '        ' Call the business to generate the document reference
                '        m_lReturn = m_oBusiness.GenerateNumber( _
                ''            v_iUserID:=g_iUserID%, _
                ''            r_lNumber:=lNumber&)
                '        If (m_lReturn <> PMTrue) Then
                '            SetInterfaceDefaults = PMFalse
                '        End If
                '
                '        ' Prefix a few zeros
                '        txtDocumentRef.Text = Format(lNumber&, "00000000")

            End If

            ' CF300998 - Disable reversing and recurring controls by default
            lblReversesOn.Enabled = False
            lblOccurs.Enabled = False
            lblTimes.Enabled = False
            txtReverseDate.Enabled = False
            txtOccurs.Enabled = False
            udOccurs.Enabled = False
            ' {* USER DEFINED CODE (End) *}

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
            ReDim m_ctlTabFirstLast(1, 0)

            ' Set the first and last data entry controls for
            ' all of the tabs.

            ' {* USER DEFINED CODE (Begin) *}

            m_ctlTabFirstLast(ACControlStart, 0) = txtDocumentRef
            m_ctlTabFirstLast(ACControlEnd, 0) = udOccurs

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
    'UPGRADE_NOTE: (7001) The following declaration (DisplayCaptions) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function DisplayCaptions() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Display all language specific captions.
    '

    'Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    '
    ' Check for an error.
    'If Me.Text = "" Then
    ' Failed to get data from the resource file.
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &  _
    '                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")
    '
    'Return result
    'End If
    '

    'cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    '

    'cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    '

    'cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    '

    'cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    '

    'SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    '
    ' {* USER DEFINED CODE (Begin) *}
    '

    'lblDocumentRef.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDocumentRefCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    '

    'lblDocumentDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDocumentDateCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    '

    'lblComment.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCommentCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    '

    'lblDocumentType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDocumentTypeCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    '
    ' {* USER DEFINED CODE (End) *}
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetLookupValues) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetLookupValues() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Gets all of the lookup values.
    '
    ' Check the task.
    'Select Case (m_iTask)
    'Case gPMConstants.PMEComponentAction.PMAdd
    ' Get all of the lookup values.

    'm_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
    '
    'Case gPMConstants.PMEComponentAction.PMEdit
    ' Get all of the lookup values with the correct
    ' effective date.

    'm_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
    '
    'Case gPMConstants.PMEComponentAction.PMView
    ' Get lookup values for viewing only.

    'm_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
    'End Select
    '
    ' Check for errors.
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error.
    'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")
    '
    'Return result
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

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
    ' Check if this is the selected index.
    'If m_vLookupValues(ACValueID, lRow).Equals(m_vLookupDetails(ACDetailKey, lCntr)) Then


    'ctlLookup.ListIndex = ctlLookup.NewIndex
    'End If
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
    ' Name: ReactDocumentTypeChange
    '
    ' Description: Enables or disables the "reverses on" and "occurs"
    '              controls depending on the document type.
    '
    ' ***************************************************************** '
    Private Function ReactDocumentTypeChange(ByRef ctlDocType As Control) As Integer

        Dim result As Integer = 0
        Dim lDocumentType, lDocTypeGroupID As Integer
        Dim bRecurEnabled, bReverseEnabled As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'TODOLIST to be handled at runtime
            'lDocumentType = ctlDocType.ItemId

            ' Update the controls depending on the document type

            m_lReturn = m_oBusiness.GetGroupIDFromTypeID(v_lDocumentTypeID:=lDocumentType, r_lDocTypeGroupID:=lDocTypeGroupID)

            Select Case lDocTypeGroupID
                Case gACTLibrary.ACTDocTypeGroupRecur
                    ' Disable reverses
                    bReverseEnabled = False
                    ' Enable recurs
                    bRecurEnabled = True

                Case gACTLibrary.ACTDocTypeGroupReverse
                    ' Enable reverses
                    bReverseEnabled = True
                    ' Disable recurs
                    bRecurEnabled = False

                Case Else
                    ' Disable reverses
                    bReverseEnabled = False
                    ' Disable recurs
                    bRecurEnabled = False

            End Select

            lblReversesOn.Enabled = bReverseEnabled
            txtReverseDate.Enabled = bReverseEnabled

            lblOccurs.Enabled = bRecurEnabled
            txtOccurs.Enabled = bRecurEnabled
            udOccurs.Enabled = bRecurEnabled
            lblTimes.Enabled = bRecurEnabled

            ' extras
            For iLoop1 As Integer = 0 To 2
                optOccurs(iLoop1).Enabled = bRecurEnabled
                txtOccursPer(iLoop1).Enabled = bRecurEnabled
                lblOccursPer(iLoop1).Enabled = bRecurEnabled
                udOccursPer(iLoop1).Enabled = bRecurEnabled
            Next iLoop1

            ' Default the values

            Select Case lDocTypeGroupID
                ' Recurring document
                Case gACTLibrary.ACTDocTypeGroupRecur
                    txtOccurs.Text = CStr(1)
                    txtReverseDate.Text = ""
                    txtOccursPer(0).Text = "1"
                    txtOccursPer(1).Text = ""
                    txtOccursPer(2).Text = ""

                    ' Reverse document
                Case gACTLibrary.ACTDocTypeGroupReverse
                    txtOccurs.Text = ""
                    txtReverseDate.Text = DateTimeHelper.ToString(CDate(txtDocumentDate.Text).AddDays(1))
                    txtReverseDate.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, txtReverseDate.Text)
                    For iLoop1 As Integer = 0 To 2
                        txtOccursPer(iLoop1).Text = ""
                    Next iLoop1

                    ' Something else!
                Case Else
                    txtOccurs.Text = ""
                    txtReverseDate.Text = ""
                    For iLoop1 As Integer = 0 To 2
                        txtOccursPer(iLoop1).Text = ""
                    Next iLoop1

            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReactDocumentTypeChange Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReactDocumentTypeChange", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LeapYearCheck
    '
    ' Description: Checks if the date entered in the Document date
    '              textbox is a leap year, if it is the number of
    '              days in period is set to 366 else it's set 365
    '
    ' ***************************************************************** '
    Private Function LeapYearCheck() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If (txtDocumentDate.Text = "") Or (Not Information.IsDate(txtDocumentDate.Text)) Then
                Return result
            Else
                If (CDate(txtDocumentDate.Text).Year Mod 4) = 0 Then
                    udOccursPer(0).Max = 366
                    Return result
                Else
                    udOccursPer(0).Max = 365
                    Return result
                End If
            End If

        Catch
        End Try



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LeapyearcheckFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="Leapyearcheck", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function
    'UPGRADE_NOTE: (7001) The following declaration (GetAutoNumValues) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetAutoNumValues(ByVal iDocumenttypeID As Integer, ByRef sGroupCode As String, ByRef sRangeCode As String) As Integer
    'Select Case iDocumenttypeID
    'Case 1
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef1
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeJn
    'Case 2
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef2
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSdn
    'Case 3
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef3
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeScn
    'Case 4
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef4
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSnd
    'Case 5
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef5
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSnc
    'Case 6
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef6
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeCcr
    'Case 7
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef7
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeCdr
    'Case 8
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef8
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeACc
    'Case 9
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef9
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodePpt
    'Case 10
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef10
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeRvj
    'Case 11
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef11
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeDpj
    'Case 12
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef12
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeRcj
    'Case 13
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef13
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodePin
    'Case 14
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef14
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSwd
    'Case 15
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef15
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSrd
    'Case 16
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef16
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSrc
    'Case 17
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef17
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSed
    'Case 18
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef18
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSec
    'Case 19
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef19
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSat
    'Case 20
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef20
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSaj
    'Case 21
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef21
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSbd
    'Case 22
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef22
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSrp
    'Case 23
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef23
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSpy
    'Case 24
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef24
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSin
    'Case 25
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef25
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodePcn
    'Case 26
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef26
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeDia
    'Case 27
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef27
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeDir
    'EK 100300
    'Case 28
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef28
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeClp
    'Case 29
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef29
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeClr
    'Case 30
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef30
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeFee
    'Case 31
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef31
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeShd
    'Case 32
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef32
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeShc
    'Case 32
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef32
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeShc
    'EK 200300 And theres more!
    'Case 33
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef33
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeDid
    'Case 34
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef34
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeDic
    'Case 35
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef35
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeTrd
    'Case 35
    'sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef36
    'sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeTrc
    'End Select
    'End Function
    'eck240102


    Private Function CheckDates() As Integer
        Dim result As Integer = 0
        Dim bACTUserAuthorities, bACTPeriod As Object
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: CheckDates
        ' PURPOSE: Ensures that journal is not post dated and if so that the user has authority
        ' AUTHOR:
        ' DATE:
        ' RETURNS: PMTrue for success
        ' CHANGES: 17-Oct-02 PWF
        ' ---------------------------------------------------------------------------


        Dim oPeriod As Object

        Dim oUserAuthorities As Object
        Dim vDetails As Object
        Dim dtPeriodEndDate As Date
        Dim lPeriodId As Integer
        Dim dtPeriodDate As Date
        Dim vAccess As Object
        Dim lPreviousPeriodID As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Information.IsDate(txtDocumentDate.Text) Then
                m_dtDocumentDate = (CDate(gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, vFieldValue:=CDate(txtDocumentDate.Text))))
            End If

            Dim temp_oPeriod As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPeriod, "bACTPeriod.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPeriod = temp_oPeriod
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", CheckDates, Unable to create business object: bACTPeriod")
            End If


            m_lReturn = oPeriod.GetCurrentPeriodDetails(r_vDetails:=vDetails)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", CheckDates, Unable to get current period details")
            End If


            lPeriodId = CInt(vDetails(0, 0))

            ' PWF 17/10/2002: Get previous period id properly.
            'TF211102 - Use separate variable to avoid conflict
            'PN1381

            m_lReturn = oPeriod.GetPreviousPeriodID(lPeriodId:=lPeriodId, lPreviousPeriodID:=lPreviousPeriodID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", CheckDates, Unable to get previous period id")
            End If
            'eckPN4198 If this is the first Period set previous period end date to 1900
            If lPreviousPeriodID = 0 Then
                dtPeriodEndDate = DateTime.FromOADate(1900 / 1 / 1)
            Else

                lPeriodId = lPreviousPeriodID

                m_lReturn = oPeriod.GetDetails(vPeriodID:=lPeriodId)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", CheckDates, Unable to get previous period details")
                End If


                m_lReturn = oPeriod.GetNext(vPeriodEndDate:=dtPeriodEndDate)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", CheckDates, Unable to read previous period end date")
                End If
            End If 'eckPN4198

            oPeriod.Dispose()
            oPeriod = Nothing
            If m_dtDocumentDate > dtPeriodEndDate Then
                Return result
            End If

            Dim temp_oUserAuthorities As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oUserAuthorities, "bACTUserAuthorities.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oUserAuthorities = temp_oUserAuthorities
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", CheckDates, Unable to create business object: bACTUserAuthorities")
            End If


            m_lReturn = oUserAuthorities.GetDetails(vUserId:=g_iUserID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", CheckDates, You do not have authority to create a post dated journal")
            End If


            m_lReturn = oUserAuthorities.GetNext(vHasUnrestrictedUpdate:=vAccess)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", CheckDates, You do not have authority to create a post dated journal")
            End If


            oUserAuthorities.Dispose()
            oUserAuthorities = Nothing
            If gPMFunctions.NullToLong(vAccess) > 0 Then
                Return result
            Else
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", CheckDates, You do not have authority to create a post dated journal")
            End If



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------



        Catch ex As Exception
            Select Case Information.Err().Number
                Case Constants.vbObjectError
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogFeedback, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckDates", excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckDates", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError


            End Select

        Finally


        End Try
        Return result
    End Function


    ' PRIVATE Methods (End)

    Private Sub cmbDocumentType_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbDocumentType.Click

        ' Change of recur or reverse
        If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
            m_lReturn = ReactDocumentTypeChange(ctlDocType:=cmbDocumentType)
        End If

    End Sub

    'Private Sub cmbDocumentType_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles cmbDocumentType.KeyPress
    '    ' Private Sub cmbDocumentType_KeyPress(ByVal eventSender As Object, ByVal eventArgs As UserControls.TypeTable.KeyPressEventArgs) Handles cmbDocumentType.KeyPress
    '    Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
    '    '    If txtDocumentRef.Text > "" Then
    '    '        KeyAscii = 0
    '    '        Exit Sub
    '    '    End If

    '    If KeyAscii = 0 Then
    '        eventArgs.Handled = True
    '    End If
    '    eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    'End Sub

    'EK 220200 We now have doc type so get the reference
    Private Sub cmbDocumentType_LostFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbDocumentType.LostFocus
        'eck210800 generate the number when you press OK
        '    m_iDocumenttypeID = cmbDocumentType.ItemId
        '    m_lReturn = GetAutoNumValues(iDocumenttypeID:=m_iDocumenttypeID, sGroupCode:=m_sGroupCode, sRangeCode:=m_sRangeCode)
        ''eck180500 Pass company id parameter
        '        ' Call the business to generate the document reference
        '        m_lReturn = m_oBusiness.GenerateNumber( _
        ''            v_sGroupCode:=m_sGroupCode, _
        ''            v_sRangeCode:=m_sRangeCode, _
        ''            v_iUserID:=g_iUserID%, _
        ''            v_iCompanyID:=m_iCompanyID, _
        ''            r_lNumber:=m_lNumber&)
        '
        '        ' Prefix a few zeros
        '        txtDocumentRef.Text = m_sRangeCode & Format(m_lNumber&, "00000000")
        '        cmbDocumentType.Visible = False
        '        cmbDocumentType.Enabled = False
        '        cmbDocumentType.Visible = True

        ' PN24364 Removed all checks as this is now done in client manager.
        '2005 Client Manager Security
        '    Select Case cmbDocumentType.ItemId
        '    Case 2, 4, 7, 15, 17, 31, 35, 37, 42, 44, 46, 52, 54
        '        If g_bRaiseDebitAuthority = False Then
        '            MsgBox "You do not have authority to create this document type", vbOKOnly, "Access Denied"
        '            cmbDocumentType.SetFocus
        '            cmbDocumentType.ItemId = 1
        '        End If
        '     Case 3, 5, 6, 16, 18, 25, 32, 36, 38, 43, 45, 47, 53, 55
        '        If g_bRaiseCreditAuthority = False Then
        '            MsgBox "You do not have authority to create this document type", vbOKOnly, "Access Denied"
        '            cmbDocumentType.SetFocus
        '            cmbDocumentType.ItemId = 1
        '         End If
        '     Case 30
        '        If g_bRaiseFeeAuthority = False Then
        '            MsgBox "You do not have authority to create this document type", vbOKOnly, "Access Denied"
        '            cmbDocumentType.SetFocus
        '            cmbDocumentType.ItemId = 1
        '        End If
        '    Case 22, 23, 28, 29, 39
        '        If g_bRaiseCashAuthority = False Then
        '            MsgBox "You do not have authority to create this document type", vbOKOnly, "Access Denied"
        '            cmbDocumentType.SetFocus
        '            cmbDocumentType.ItemId = 1
        '        End If
        '     Case 33, 34
        '        If g_bRaiseManualDIDAuthority = False Then
        '            MsgBox "You do not have authority to create this document type", vbOKOnly, "Access Denied"
        '            cmbDocumentType.SetFocus
        '            cmbDocumentType.ItemId = 1
        '        End If
        '
        '
        '    End Select


    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        ' Fire up the help screen
        'Developer Guide No.184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, lContextID:=ScreenHelpID)

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
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bACTDocument.Form", vInstanceManager:="ClientManager")
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

            ' Get an instance of the business object via
            ' the public object manager.

            Dim temp_m_oAuditSet As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oAuditSet, "bACTAuditSet.Form", vInstanceManager:="ClientManager")
            m_oAuditSet = temp_m_oAuditSet

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
            m_oGeneral = New iACTDocument.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

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


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.

        Try

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the process modes for the busines object.

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If



            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            m_lReturn = SetFieldValidation()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            'Check for leap year
            m_lReturn = LeapYearCheck()

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            'developer guide No.7
            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If


            m_oAuditSet.Dispose()

           


            ' Terminate the general object.
            m_oGeneral.Dispose()

           


            m_oFormFields.Dispose()
            


            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Terminate the business object

            m_oBusiness.Dispose()


            ' Destroy the instance of the business object
            ' from memory.
            m_oBusiness = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

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

        Catch



            ' Error Section.

            Exit Sub
        End Try


    End Sub

    Private isInitializingComponent As Boolean
    Private Sub optOccurs_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _optOccurs_2.CheckedChanged, _optOccurs_1.CheckedChanged, _optOccurs_0.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            Dim Index As Integer = Array.IndexOf(optOccurs, eventSender)


            ' Default the selected text to "1" if it isnt already
            ' set, and set the rest to nothing.
            For iLoop1 As Integer = 0 To 2
                If iLoop1 = Index Then
                    If txtOccursPer(iLoop1).Text = "" Then
                        txtOccursPer(iLoop1).Text = CStr(1)
                    End If
                Else
                    txtOccursPer(iLoop1).Text = ""
                End If
            Next iLoop1

        End If
    End Sub

    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        Try

            With tabMainTab
                ' Set the default button.
                VB6.SetDefault(cmdOK, True)

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

        Dim sReference As String = ""
        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            m_lReturn = m_oFormFields.CheckMandatoryControls()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'Only do this if Adding
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                'Check that Journal is not post dated
                m_lReturn = CheckDates()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If

                m_iDocumenttypeID = cmbDocumentType.ItemId

                m_lReturn = m_oBusiness.GetAutoNumValues(v_iDocumenttypeID:=m_iDocumenttypeID, r_sGroupCode:=m_sGroupCode, r_sRangeCode:=m_sRangeCode)

                ' Call the business to generate the document reference
                'Start(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber

                m_lReturn = m_oBusiness.GenerateDocumentReferenceNumber(v_sGroupCode:=m_sGroupCode, v_sRangeCode:=m_sRangeCode, v_iUserID:=g_iUserID, v_iCompanyID:=m_iCompanyID, r_sDocumentRef:=sReference)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue And sReference.Trim() <> "" Then

                    ' Prefix a few zeros

                    m_sDocumentRef = m_sRangeCode & sReference

                    m_sDocumentRef = sReference
                    'End(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                    MessageBox.Show("Generated document reference " & sReference, "Document " & _
                                    "Reference", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    ' Process the next set of actions depending
                    ' upon the interface task etc.
                    m_lReturn = m_oGeneral.ProcessCommand()
                Else
                    MessageBox.Show("Failed to generate a document reference.", "Error: New Document Reference failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
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
                'eck180500 Pass company parameter
                ' Call the business to put the document reference

                m_lReturn = m_oBusiness.PoolNumber(v_sGroupCode:=m_sGroupCode, v_sRangeCode:=m_sRangeCode, v_iUserID:=g_iUserID, v_iCompanyID:=m_iCompanyID, r_lNumber:=m_lNumber)



            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click

        ' Click event of the Navigate button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMNavigate

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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    'UPGRADE_NOTE: (7001) The following declaration (cmdNext_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub cmdNext_Click(ByRef Index As Integer)
    '
    'Try 
    '
    ' Change to the next tab.
    'If SSTabHelper.GetSelectedIndex(tabMainTab) < SSTabHelper.GetTabCount(tabMainTab) - 1 Then
    'SSTabHelper.SetSelectedIndex(tabMainTab, Index + 1)
    'End If
    '
    ' Set focus to the first control on the tab.
    'If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
    'm_ctlTabFirstLast(ACControlStart, Index + 1).Focus()
    'End If
    '
    'Catch 
    '
    '
    '
    ' Error Section
    '
    'Exit Sub
    'End Try
    '
    '
    'End Sub

    Private Sub txtComment_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtComment.Leave

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtComment)

    End Sub



    Private Sub txtDocumentRef_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDocumentRef.Enter

        iPMFunc.SelectText(txtDocumentRef)

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDocumentRef)

    End Sub
    Private Sub txtDocumentDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDocumentDate.Enter
        ' Check date.
        'CheckDateGotFocus txtDocumentDate

        ' Hightlight any text.
        iPMFunc.SelectText(txtDocumentDate)

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDocumentDate)

    End Sub

    Private Sub txtDocumentDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDocumentDate.Leave
        ' Check date.
        'CheckDateLostFocus txtDocumentDate

        'Check for leap year
        m_lReturn = LeapYearCheck()

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDocumentDate)

    End Sub

    Private Sub txtComment_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtComment.Enter

        iPMFunc.SelectText(txtComment)

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtComment)

    End Sub

    ' PRIVATE Events (End)

    Private Sub txtDocumentRef_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDocumentRef.Leave

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDocumentRef)

    End Sub

    Private Sub txtOccurs_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtOccurs.Enter

        iPMFunc.SelectText(txtOccurs)

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtOccurs)

    End Sub

    Private Sub txtOccurs_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtOccurs.Leave

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtOccurs)

    End Sub

    Private Sub txtOccursPer_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _txtOccursPer_2.Enter, _txtOccursPer_1.Enter, _txtOccursPer_0.Enter

        'CF011098 Commented out till index is handled by FormControl

        'SelectText txtOccursPer(Index)

        'm_lReturn = m_oFormFields.GotFocus(ctlControl:=txtOccursPer(Index))

    End Sub

    Private Sub txtOccursPer_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _txtOccursPer_2.Leave, _txtOccursPer_1.Leave, _txtOccursPer_0.Leave

        ' Commented out till index is handled by FormControl

        'm_lReturn = m_oFormFields.LostFocus(ctlControl:=txtOccursPer(Index))

    End Sub

    Private Sub txtReverseDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtReverseDate.Enter

        iPMFunc.SelectText(txtReverseDate)

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtReverseDate)

    End Sub

    Private Sub txtReverseDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtReverseDate.Leave

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtReverseDate)

    End Sub

    Private Sub udOccursPer_Change(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _udOccursPer_2.Change, _udOccursPer_1.Change, _udOccursPer_0.Change
        Dim Index As Integer = Array.IndexOf(udOccursPer, eventSender)

        If Not optOccurs(Index).Checked Then
            optOccurs(Index).Checked = True
        End If

    End Sub
    Private Sub frmInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
        MemoryHelper.ReleaseMemory()
    End Sub
End Class
