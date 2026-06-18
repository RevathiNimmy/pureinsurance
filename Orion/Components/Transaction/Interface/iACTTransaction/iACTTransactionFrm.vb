Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Globalization
Imports System.Windows.Forms
Imports Artinsoft.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Public Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 23rd June 2006
    '
    ' Description: Main interface for "Manual Journal"
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"
    Private Const vbFormCode As Integer = 0
    'developer guide no. 50
    Dim frmInterface As frmInterface
    ' PRIVATE Data Members (Begin)
    Private Const ACTGetCurrenciesInCompany As Integer = 1 '30645
    Private Const ACTAllCurrencies As Integer = 2
    Private Const ACTCurrencyISOCode As Integer = 1
    Private Const ACTCurrencyDescription As Integer = 2
    Private Const ACTCurrencyId As Integer = 0

    Private Const TDBGridCol_Account_id As Integer = 0
    Private Const TDBGridCol_Account As Integer = 1
    Private Const TDBGridCol_Accoun_Btn As Integer = 2
    Private Const TDBGridCol_Currency_id As Integer = 3
    Private Const TDBGridCol_Currency As Integer = 4
    Private Const TDBGridCol_Amount As Integer = 5
    Private Const TDBGridCol_Currency_Rate As Integer = 6
    Private Const TDBGridCol_Base_Amount As Integer = 7
    Private Const TDBGridCol_AltRef As Integer = 8 '(RC) QBENZ014
    Private Const TDBGridCol_Comment As Integer = 9
    Private Const TDBGridCol_UnderwritingYear_id As Integer = 10
    Private Const TDBGridCol_UnderwritingYear As Integer = 11
    Private Const TDBGridCol_Department_id As Integer = 12
    Private Const TDBGridCol_Department As Integer = 13
    Private Const TDBGridCol_Insurance_Ref As Integer = 14
    Private Const TDBGridCol_Purchase_Order As Integer = 15
    Private Const TDBGridCol_Purchase_Invoice As Integer = 16


    Private invalidAmount As Boolean
    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_lDocumentID As Integer
    Private m_iDocumenttypeID As Integer
    Private m_iBranchID As Integer
    Private m_sDocumentRef As String = ""
    Private m_sGroupCode As String = ""
    Private m_sRangeCode As String = ""
    Private m_lNumber As Integer
    Private m_bReversingDocument As Boolean
    Private m_lReversingDocumentID As Integer
    Private m_dtReverseDate As Date
    Private m_bRecurringDocument As Boolean
    Private m_iOccurances As Integer
    Private m_vRecurringDocumentIDs() As Object
    Private m_vRecurringDocumentDates() As Object
    Private m_dtDocumentDate As Date
    Private m_sComment As String = ""

    ' Stores the return value for the a function call.
    Private m_lReturn As Integer
    Private m_cTotalBalance As Decimal
    Private m_BtnClicked As String = ""
    Private m_bGeneratedNumberUsed As Boolean
    'PN30096 - Datasure
    Private m_iCurrencyID As Integer

    Private m_GridArray As XArrayHelper
    Private m_bPaste As Boolean

    'Modified as per vb code
    'Private m_oDocument As bACTDocument.Form
    Private m_oDocument As Object

    'Modified as per vb code
    'Private m_oDocPost As bACTDocumentPost.Form
    Private m_oDocPost As Object

    'Modified as per vb code
    'Private m_oAccount As bACTAccount.Form
    Private m_oAccount As Object

    'Modified as per vb code
    'Private m_oCCYConvert As bACTCurrencyConvert.Form
    Private m_oCCYConvert As Object

    'Modified as per vb code
    'Private m_oAuditSet As bACTAuditSet.Form
    Private m_oAuditSet As Object

    'Modified as per vb code
    'Private m_oAccountLookup As iACTFindAccount.Interface_Renamed
    Private m_oAccountLookup As Object

    'Modified as per vb code
    'Private m_oUserAuthorities As bACTUserAuthorities.Business 
    Private m_oUserAuthorities As Object
    Private m_sUnderwritingAgency As String = ""
    Private m_lCurrencyID As Integer
    Private m_sCurrency As String = ""
    Private m_nCount As Integer

    ' PUBLIC Property Procedures (Begin)

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

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            ' Standard Property.
            ' Set the calling application name.
            m_sCallingAppName = Value
        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get
            ' Standard Property.
            ' Return the interface exit status.
            Return m_lStatus
        End Get
    End Property

    Public Property Task() As Integer
        Get
            ' Standard Property.
            ' Return the task.
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            ' Standard Property.
            ' Set the objects task.
            m_iTask = Value
        End Set
    End Property

    Public Property Navigate() As Integer
        Get
            ' Standard Property.
            ' Return the navigate flag.
            Return m_lNavigate
        End Get
        Set(ByVal Value As Integer)
            ' Standard Property.
            ' Set the navigate flag.
            m_lNavigate = Value
        End Set
    End Property

    Public Property ProcessMode() As Integer
        Get
            ' Standard Property.
            ' Return the process mode.
            Return m_lProcessMode
        End Get
        Set(ByVal Value As Integer)
            ' Standard Property.
            ' Set the process mode.
            m_lProcessMode = Value
        End Set
    End Property

    Public Property TransactionType() As String
        Get
            ' Standard Property.
            ' Return the type of business.
            Return m_sTransactionType
        End Get
        Set(ByVal Value As String)
            ' Standard Property.
            ' Set the type of business.
            m_sTransactionType = Value
        End Set
    End Property

    Public Property EffectiveDate() As Date
        Get
            ' Standard Property.
            ' Return the effective date.
            Return m_dtEffectiveDate
        End Get
        Set(ByVal Value As Date)
            ' Standard Property.
            ' Set the effective date.
            m_dtEffectiveDate = Value
        End Set
    End Property

    ' PUBLIC Property Procedures (End)


    ' PUBLIC Methods (Begin)

    Public Function Initialise() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse

        Try


            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get Document business object
            Dim temp_m_oDocument As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oDocument, "bACTDocument.Form", vInstanceManager:="ClientManager")
            m_oDocument = temp_m_oDocument
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "sClassName:='bACTDocument.Form'", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Get DocumentPost business object
            Dim temp_m_oDocPost As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oDocPost, "bACTDocumentPost.Form", vInstanceManager:="ClientManager")
            m_oDocPost = temp_m_oDocPost
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "sClassName:='bACTDocumentPost.Form'", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Get Account business object
            Dim temp_m_oAccount As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oAccount, "bACTAccount.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oAccount = temp_m_oAccount
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "sClassName:='bACTAccount.Form'", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Get AccountLookup Interface object
            Dim temp_m_oAccountLookup As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oAccountLookup, sClassName:="iACTFindAccount.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oAccountLookup = temp_m_oAccountLookup
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "sClassName:='iACTFindAccount.Form'", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Get AuditSet Business object
            Dim temp_m_oAuditSet As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oAuditSet, "bACTAuditSet.Form", vInstanceManager:="ClientManager")
            m_oAuditSet = temp_m_oAuditSet
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "sClassName:='bACTAuditSet.Form'", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Get CurrencyConvert Business object
            Dim temp_m_oCCYConvert As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oCCYConvert, "bACTCurrencyConvert.Form", vInstanceManager:="ClientManager")
            m_oCCYConvert = temp_m_oCCYConvert
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "sClassName:='bACTCurrencyConvert.Form'", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Get User Authorities Business
            Dim temp_m_oUserAuthorities As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oUserAuthorities, "bACTUserAuthorities.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oUserAuthorities = temp_m_oUserAuthorities
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "sClassName:='bACTUserAuthorities.Business'", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception



            ' Error Section
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function Load_Renamed() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTUnderwriting, v_vBranch:=g_iSourceID, r_vUnderwriting:=m_sUnderwritingAgency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ShowForm (Standard Method)
    '
    ' Description: Show the form
    '
    ' ***************************************************************** '
    Public Function ShowForm(ByRef lDisplayState As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Show the the form, allow user input etc.
            VB6.ShowForm(Me, lDisplayState)
            Return result

        Catch excep As System.Exception



            ' Error Section
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to show the form", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Updates all business members from the interface details.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function InterfaceToBusiness() As Integer

        Dim nResult As Integer = 0
        Dim nAuditSetID As Integer
        Dim dtRecurringDate As Date
        Dim dtLastDate As Date
        Dim nOccurances As Integer
        Dim sDocumentRef As String
        Dim sComment As String
        Dim sOffset As String = ""
        Dim nMonths As Byte
        Dim oTransArray(,) As Object
        Dim sReference As String = ""

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Update the business object.
            ' Assign the details from the interface to the data storage.
            m_lReturn = InterfaceToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Recurring document
            If m_bRecurringDocument Then
                m_lReturn = m_oAuditSet.DirectAdd(vAuditsetID:=nAuditSetID, vCompanyID:=g_iCompanyID, vUserID:=g_iUserID, vPostedDate:=DateTime.Now, vComment:="Recurring Document")
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Reversing document
            If m_bReversingDocument Then
                m_lReturn = m_oAuditSet.DirectAdd(vAuditsetID:=nAuditSetID, vCompanyID:=g_iCompanyID, vUserID:=g_iUserID, vPostedDate:=DateTime.Now, vComment:="Reversing Document")
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
                Return nResult
            End If

            'Copy Data from XDBArray to simple Array for passing to Business Object
            ReDim oTransArray(m_GridArray.Rows.Count - 1, m_GridArray.GetUpperBound(1) - 1)

            For iRow As Integer = 0 To m_GridArray.Rows.Count - 1

                For iCol As Integer = 0 To m_GridArray.GetUpperBound(1) - 1
                    If iCol < 2 Then
                        oTransArray(iRow, iCol) = m_GridArray(iRow, iCol)
                    Else
                        oTransArray(iRow, iCol) = m_GridArray(iRow, iCol + 1)
                    End If
                Next
            Next

            If Not m_bRecurringDocument Then
                'BB Use Direct Add as we need the ID back
                m_lReturn = m_oDocPost.AddDocumentTransactions(r_vDocumentID:=m_lDocumentID, v_lDocumentTypeId:=m_iDocumenttypeID, v_sBranchID:=m_iBranchID,
                                                               v_sComment:=m_sComment, v_dtDocumentDate:=m_dtDocumentDate, v_vDocSourceID:=g_iSourceID,
                                                               v_sDocumentRef:=m_sDocumentRef, v_vOperatorID:=g_iUserID, v_vTransArray:=oTransArray, vTransdetailTypeID:=1)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
                    Return nResult
                Else
                    'Auto generated number is used, dont pool
                    m_bGeneratedNumberUsed = True
                End If

            End If

            ' its a recurring document
            If m_bRecurringDocument Then

                ' temp store document date
                dtLastDate = m_dtDocumentDate

                If optOccurs(0).Checked Then
                    ' Per Period
                    sOffset = txtOccursPer(0).Value
                    nMonths = 0
                ElseIf (optOccurs(1).Checked) Then
                    ' Per Month
                    sOffset = txtOccursPer(1).Value

                    m_lReturn = GetMonthsForward(r_vNextDate:=dtLastDate, v_vOffset:=sOffset, v_vMonths:=nMonths)
                ElseIf (optOccurs(2).Checked) Then
                    ' Per Quarter
                    sOffset = txtOccursPer(2).Value

                    m_lReturn = GetMonthsForward(r_vNextDate:=dtLastDate, v_vOffset:=sOffset, v_vMonths:=nMonths)
                Else
                    ' Something's wrong
                    sOffset = CStr(0)
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Get the date this month with the day set to what the user wants
                m_lReturn = m_oDocument.GetDatePlusXMonths(v_vCurrentDate:=DateTime.Now, r_vNextDate:=dtLastDate, v_vOffset:=sOffset, v_vMonths:=nMonths)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDocPost.AddDocumentTransactions(r_vDocumentID:=m_lDocumentID, v_lDocumentTypeId:=m_iDocumenttypeID, v_sBranchID:=m_iBranchID,
                                                               v_sComment:=m_sComment, v_dtDocumentDate:=dtLastDate, v_vDocSourceID:=g_iSourceID,
                                                               v_sDocumentRef:=m_sDocumentRef, v_vOperatorID:=g_iUserID, v_vTransArray:=oTransArray, vTransdetailTypeID:=1)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
                    Return nResult
                Else
                    'Auto generated number is used, dont pool
                    m_bGeneratedNumberUsed = True
                End If

                ' Add recurred documents
                nOccurances = m_iOccurances

                ReDim m_vRecurringDocumentIDs(0)
                ReDim m_vRecurringDocumentDates(0)

                ' Add ID to the array
                m_vRecurringDocumentIDs(0) = m_lDocumentID

                ' Add Date to the array
                m_vRecurringDocumentDates(0) = dtLastDate

                For iLoop1 As Integer = 1 To nOccurances - 1
                    'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber
                    m_lReturn = m_oDocument.GenerateDocumentReferenceNumber(v_sGroupCode:=m_sGroupCode, v_sRangeCode:=m_sRangeCode, v_iUserID:=g_iUserID, v_iCompanyID:=g_iCompanyID, r_sDocumentRef:=sReference)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If sReference.Trim() <> "" Then
                        m_lNumber = gPMFunctions.ToSafeLong(sReference)
                        sDocumentRef = m_sRangeCode & sReference
                    End If

                    ' Get Recurring Date
                    If optOccurs(0).Checked Then
                        ' Per Period
                        m_lReturn = m_oDocument.GetDateNext(v_iNextType:=1, v_vCurrentDate:=dtLastDate, v_vOffset:=sOffset, r_vNextDate:=dtRecurringDate)

                    ElseIf (optOccurs(1).Checked) Then
                        ' Per Month
                        m_lReturn = m_oDocument.GetDateNext(v_iNextType:=2, v_vCurrentDate:=dtLastDate, v_vOffset:=sOffset, r_vNextDate:=dtRecurringDate)

                    ElseIf (optOccurs(2).Checked) Then
                        ' Per Quarter
                        m_lReturn = m_oDocument.GetDateNext(v_iNextType:=3, v_vCurrentDate:=dtLastDate, v_vOffset:=sOffset, r_vNextDate:=dtRecurringDate)
                    Else
                        ' Something's wrong
                        dtRecurringDate = DateTime.Now

                    End If

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Add the document
                    m_lReturn = m_oDocPost.AddDocumentTransactions(r_vDocumentID:=m_lDocumentID, v_lDocumentTypeId:=m_iDocumenttypeID, v_sBranchID:=m_iBranchID,
                                                                   v_sComment:=m_sComment, v_dtDocumentDate:=dtRecurringDate, v_vDocSourceID:=g_iSourceID,
                                                                   v_sDocumentRef:=sDocumentRef, v_vOperatorID:=g_iUserID, v_vTransArray:=oTransArray, vTransdetailTypeID:=1)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")

                        Return nResult
                    Else
                        'Auto generated number is used, dont pool
                        m_bGeneratedNumberUsed = True
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
                m_dtReverseDate = dtpReverseDate.Value

                ' Set the comment
                sComment = "Reverses " & m_sDocumentRef

                'RJ reverse the amount on vTransArray
                For iRow As Integer = oTransArray.GetLowerBound(0) To oTransArray.GetUpperBound(0)
                    oTransArray(iRow, 4) = CDbl(oTransArray(iRow, 4)) * -1
                    oTransArray(iRow, 6) = CDbl(oTransArray(iRow, 6)) * -1
                Next

                'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber
                m_lReturn = m_oDocument.GenerateDocumentReferenceNumber(v_sGroupCode:=m_sGroupCode, v_sRangeCode:=m_sRangeCode, v_iUserID:=g_iUserID, v_iCompanyID:=g_iCompanyID, r_sDocumentRef:=sReference)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If sReference.Trim() <> "" Then
                    m_lNumber = gPMFunctions.ToSafeLong(sReference)
                    sDocumentRef = m_sRangeCode & sReference
                End If

                m_lReturn = m_oDocPost.AddDocumentTransactions(r_vDocumentID:=m_lReversingDocumentID, v_lDocumentTypeId:=m_iDocumenttypeID,
                                                               v_sBranchID:=m_iBranchID, v_sComment:=m_sComment, v_dtDocumentDate:=m_dtReverseDate,
                                                               v_vDocSourceID:=g_iSourceID, v_sDocumentRef:=sDocumentRef, v_vOperatorID:=g_iUserID,
                                                               v_vTransArray:=oTransArray, vTransdetailTypeID:=1)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
                    Return nResult

                Else
                    'Auto generated number is used, dont pool
                    m_bGeneratedNumberUsed = True
                End If

            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
            End If

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function


    ' PUBLIC Methods (End)


    ' ***************************************************************** '
    ' Name: ProcessCommand
    '
    ' Description: Determines which action to take on the details
    '              depending upon the task and interface state.
    '
    ' ***************************************************************** '
    Private Function ProcessCommand() As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check the task.
            If m_lStatus <> gPMConstants.PMEReturnCode.PMCancel Then


                Select Case m_iTask
                    Case gPMConstants.PMEComponentAction.PMAdd

                        ' Update the business from the interface.
                        m_lReturn = InterfaceToBusiness()

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to update the details
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                        End If

                End Select

            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process command", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim bModifyDate, bModifyRate As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            GetBranches(cboBranches)

            m_GridArray = New XArrayHelper()
            m_GridArray.RedimXArray(New Integer() {0, 16}, New Integer() {0, 0}) '(RC) QBENZ014


            m_lReturn = m_oUserAuthorities.GetDetails(vUserID:=g_iUserID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oUserAuthorities.GetNext(vOverrideDate:=bModifyDate, vOverrideRate:=bModifyRate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            With tdgTransactions.Columns
                While .Count < m_GridArray.Columns.Count
                    If tdgTransactions.Columns.Count = TDBGridCol_Currency Or tdgTransactions.Columns.Count = TDBGridCol_UnderwritingYear Or tdgTransactions.Columns.Count = TDBGridCol_Department Then
                        Dim newColumn As DataGridViewComboBoxColumn = New DataGridViewComboBoxColumn()

                        newColumn.Visible = True
                        'newColumn.ReadOnly = True
                        newColumn.Width = 0
                        newColumn.Resizable = False
                        newColumn.HeaderCell.Style.Font = VB6.FontChangeBold(tdgTransactions.ColumnHeadersDefaultCellStyle.Font, True)
                        newColumn.HeaderCell.Style.Font = VB6.FontChangeSize(tdgTransactions.ColumnHeadersDefaultCellStyle.Font, 8)

                        .Add(newColumn)
                    Else
                        If tdgTransactions.Columns.Count = TDBGridCol_Accoun_Btn Then
                            Dim newColumn As DataGridViewButtonColumn = New DataGridViewButtonColumn()
                            newColumn.Visible = True
                            newColumn.ReadOnly = False
                            newColumn.Width = 10
                            newColumn.Resizable = False
                            newColumn.HeaderCell.Style.Font = VB6.FontChangeBold(tdgTransactions.ColumnHeadersDefaultCellStyle.Font, True)
                            newColumn.HeaderCell.Style.Font = VB6.FontChangeSize(tdgTransactions.ColumnHeadersDefaultCellStyle.Font, 8)
                            .Add(newColumn)

                        Else
                            If tdgTransactions.Columns.Count = TDBGridCol_Account_id Or tdgTransactions.Columns.Count = TDBGridCol_Currency_id Or tdgTransactions.Columns.Count = TDBGridCol_UnderwritingYear_id Or tdgTransactions.Columns.Count = TDBGridCol_Department_id Then
                                Dim newColumn As DataGridViewExtendedColumn = New DataGridViewExtendedColumn()
                                newColumn.Visible = False
                                newColumn.ReadOnly = True
                                'newColumn.Width = 0
                                newColumn.Resizable = False
                                newColumn.HeaderCell.Style.Font = VB6.FontChangeBold(tdgTransactions.ColumnHeadersDefaultCellStyle.Font, True)
                                newColumn.HeaderCell.Style.Font = VB6.FontChangeSize(tdgTransactions.ColumnHeadersDefaultCellStyle.Font, 8)

                                .Add(newColumn)
                            Else
                                Dim newColumn As DataGridViewExtendedColumn = New DataGridViewExtendedColumn()
                                newColumn.Visible = True
                                newColumn.ReadOnly = False
                                newColumn.Width = 0
                                newColumn.Resizable = False
                                newColumn.HeaderCell.Style.Font = VB6.FontChangeBold(tdgTransactions.ColumnHeadersDefaultCellStyle.Font, True)
                                newColumn.HeaderCell.Style.Font = VB6.FontChangeSize(tdgTransactions.ColumnHeadersDefaultCellStyle.Font, 8)

                                .Add(newColumn)


                            End If
                        End If

                    End If

                End While
            End With

            With tdgTransactions

                tdgTransactions.Columns(TDBGridCol_Account_id).HeaderText = "Account_id"

                .Columns(TDBGridCol_Account_id).ReadOnly = True

                .Columns(TDBGridCol_Account_id).Visible = False

                .Columns(TDBGridCol_Account).HeaderText = "Account"

                .Columns(TDBGridCol_Account).Width = VB6.TwipsToPixelsX(2000)
                CType(.Columns(TDBGridCol_Accoun_Btn), DataGridViewButtonColumn).Text = "..."
                CType(.Columns(TDBGridCol_Accoun_Btn), DataGridViewButtonColumn).Resizable = DataGridViewTriState.False
                CType(.Columns(TDBGridCol_Accoun_Btn), DataGridViewButtonColumn).Width = 25
                CType(.Columns(TDBGridCol_Accoun_Btn), DataGridViewButtonColumn).UseColumnTextForButtonValue = True

                .Columns(TDBGridCol_Currency_id).HeaderText = "Currency_id"

                .Columns(TDBGridCol_Currency_id).ReadOnly = True

                .Columns(TDBGridCol_Currency_id).Visible = False

                .Columns(TDBGridCol_Currency).HeaderText = "Currency"

                .Columns(TDBGridCol_Currency).Width = VB6.TwipsToPixelsX(2500)

                .Columns(TDBGridCol_Amount).HeaderText = "Amount"

                .Columns(TDBGridCol_Amount).Width = VB6.TwipsToPixelsX(1500)

                .Columns(TDBGridCol_Amount).DefaultCellStyle.Format = "#,##0.00"

                .Columns(TDBGridCol_Currency_Rate).HeaderText = "Currency Rate"
                .Columns(TDBGridCol_Currency_Rate).Width = VB6.TwipsToPixelsX(1500)
                If Not bModifyRate Then
                    .Columns(TDBGridCol_Currency_Rate).ReadOnly = True
                    .Columns(TDBGridCol_Currency_Rate).DefaultCellStyle.BackColor = Color.FromArgb(200, 200, 200)
                End If
                .Columns(TDBGridCol_Currency_Rate).DefaultCellStyle.Format = "#,##0.0000000000"
                .Columns(TDBGridCol_Base_Amount).HeaderText = "Base Amount"
                .Columns(TDBGridCol_Base_Amount).ReadOnly = True
                .Columns(TDBGridCol_Base_Amount).DefaultCellStyle.BackColor = Color.FromArgb(200, 200, 200)
                .Columns(TDBGridCol_Base_Amount).DefaultCellStyle.Format = "#,##0.00"
                .Columns(TDBGridCol_Base_Amount).Width = VB6.TwipsToPixelsX(1500)
                .Columns(TDBGridCol_AltRef).HeaderText = "Alt Reference" '(RC) QBENZ014
                .Columns(TDBGridCol_AltRef).Width = VB6.TwipsToPixelsX(2000)
                .Columns(TDBGridCol_Comment).HeaderText = "Comment"
                .Columns(TDBGridCol_Comment).Width = VB6.TwipsToPixelsX(2500)
                .Columns(TDBGridCol_UnderwritingYear_id).HeaderText = "UnderwritingYear_id"
                ' .Columns(TDBGridCol_UnderwritingYear_id).ReadOnly = True
                .Columns(TDBGridCol_UnderwritingYear_id).Visible = False
                .Columns(TDBGridCol_UnderwritingYear).HeaderText = "UnderwritingYear"
                .Columns(TDBGridCol_UnderwritingYear).Width = VB6.TwipsToPixelsX(2000)
                .Columns(TDBGridCol_Department_id).HeaderText = "CostCentre_id"
                '.Columns(TDBGridCol_Department_id).Caption = "Department_id"
                '.Columns(TDBGridCol_Department_id).ReadOnly = True
                .Columns(TDBGridCol_Department_id).Visible = False
                .Columns(TDBGridCol_Department).HeaderText = "Cost Centre"
                '.Columns(TDBGridCol_Department).Caption = "Department"
                .Columns(TDBGridCol_Department).Width = VB6.TwipsToPixelsX(1500)
                .Columns(TDBGridCol_Insurance_Ref).HeaderText = "Insurance Ref"
                .Columns(TDBGridCol_Insurance_Ref).Width = VB6.TwipsToPixelsX(2000)
                .Columns(TDBGridCol_Purchase_Order).HeaderText = "Purchase Order No."
                .Columns(TDBGridCol_Purchase_Order).Width = VB6.TwipsToPixelsX(2000)
                .Columns(TDBGridCol_Purchase_Invoice).HeaderText = "Purchase Invoice No."
                .Columns(TDBGridCol_Purchase_Invoice).Width = VB6.TwipsToPixelsX(2000)

                'Account Lookup
                'developer guide no. 59 (No Solution)

                CType(.Columns(TDBGridCol_Account), DataGridViewExtendedColumn).Button = True
                CType(.Columns(TDBGridCol_Account), DataGridViewExtendedColumn).ButtonAlways = True
                CType(.Columns(TDBGridCol_Account), DataGridViewExtendedColumn).ButtonImage = pctDots.Image 'LoadPicture(App.Path & "\dots.bmp")
                CType(.Columns(TDBGridCol_Account), DataGridViewExtendedColumn).AnnotatePicture = True
                Dim oValues As ValueCollection = New ValueCollection
                m_lReturn = GetCompanyCurrencies(ACTAllCurrencies, 26, oValues, g_iCompanyID)
                For Each value As Artinsoft.Windows.Forms.DisplayValue In oValues
                    CType(.Columns(TDBGridCol_Currency), DataGridViewComboBoxColumn).Items.Add(New VB6.ListBoxItem(value.Key.ToString, Convert.ToInt32((value.Value))))
                Next
                CType(.Columns(TDBGridCol_Currency), DataGridViewComboBoxColumn).DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                oValues = New ValueCollection
                m_lReturn = GetLookupValues(oValues, "Underwriting_Year")
                For Each value As Artinsoft.Windows.Forms.DisplayValue In oValues
                    CType(.Columns(TDBGridCol_UnderwritingYear), DataGridViewComboBoxColumn).Items.Add(New VB6.ListBoxItem(value.Key.ToString, Convert.ToInt32((value.Value))))
                Next
                CType(.Columns(TDBGridCol_UnderwritingYear), DataGridViewComboBoxColumn).DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox
                oValues = New ValueCollection
                m_lReturn = GetLookupValues(oValues, "CostCentre")
                For Each value As Artinsoft.Windows.Forms.DisplayValue In oValues
                    CType(.Columns(TDBGridCol_Department), DataGridViewComboBoxColumn).Items.Add(New VB6.ListBoxItem(value.Key.ToString, Convert.ToInt32((value.Value))))
                Next
                CType(.Columns(TDBGridCol_Department), DataGridViewComboBoxColumn).DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                Dim bindingSource As BindingSource = New BindingSource(m_GridArray, "")
                .DataSource = bindingSource
                .ReBind()
                .Refresh()


            End With


            ResetForm()
            tdgTransactions.Columns(TDBGridCol_Account_id).Visible = False

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cboBranches_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboBranches.SelectedIndexChanged


        If cboBranches.SelectedIndex > -1 AndAlso tdgTransactions.Columns.Count > 0 Then

            g_iSourceID = VB6.GetItemData(cboBranches, cboBranches.SelectedIndex)
            g_iCompanyID = VB6.GetItemData(cboBranches, cboBranches.SelectedIndex)

            If CType(tdgTransactions.Columns(TDBGridCol_Currency), DataGridViewComboBoxColumn).Items.Count > 0 Then
                CType(tdgTransactions.Columns(TDBGridCol_Currency), DataGridViewComboBoxColumn).Items.Clear()
            End If
            Dim value1 As ValueCollection = New ValueCollection
            m_lReturn = GetCompanyCurrencies(ACTGetCurrenciesInCompany, 26, value1, g_iCompanyID)
            For Each value As Artinsoft.Windows.Forms.DisplayValue In value1
                CType(tdgTransactions.Columns(TDBGridCol_Currency), DataGridViewComboBoxColumn).Items.Add(New VB6.ListBoxItem(value.Key.ToString, Convert.ToInt32((value.Value))))
            Next
            For lRow As Integer = 0 To tdgTransactions.RowsCount - 1

                'developer guide no. 53 (N0 Solution)
                tdgTransactions.Rows(lRow).Cells(TDBGridCol_Currency).Value = ""
                m_lReturn = SetBaseCurrency(lRow)
                tdgTransactions.RefreshCurrentRow()
            Next
            '------END 30645
            'PN29951 - Datasure
            If g_iSourceID = 0 Then
                tdgTransactions.Rows(tdgTransactions.Rows.Count - 1).Cells(TDBGridCol_Currency_Rate).Value = 0
                tdgTransactions.Rows(tdgTransactions.Rows.Count - 1).Cells(TDBGridCol_Base_Amount).Value = 0
                Exit Sub
            End If

            For lRow As Integer = 0 To tdgTransactions.RowsCount - 1

                ' Add base amount to running total

                'developer guide no. 53 (N0 Solution)
                CalculateBaseAmount(lRow)

            Next lRow

            'calculate Total Balance

            CalcTotalBalance()
        End If

    End Sub

    Private Sub cmbDocumentType_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbDocumentType.Click

        ' Change of recur or reverse
        m_lReturn = ReactDocumentTypeChange(ctlDocType:=cmbDocumentType)

    End Sub

    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click

        With tdgTransactions

            'm_GridArray.AppendRows(1)
            m_GridArray.Rows.InsertAt(m_GridArray.NewRow, m_GridArray.Rows.Count)
            ''m_GridArray.AppendRows()

            .ReBind()

            'select last row

            'developer guide no. 53 (No Solution)

            If m_cTotalBalance = 0 Then


                .Rows(.Rows.Count - 1).Cells(TDBGridCol_Amount).Value = 0
                .Rows(.Rows.Count - 1).Cells(TDBGridCol_Currency_Rate).Value = 0
                .Rows(.Rows.Count - 1).Cells(TDBGridCol_Base_Amount).Value = 0
            Else
                .Rows(.Rows.Count - 1).Cells(TDBGridCol_Amount).Value = m_cTotalBalance * -1
                .Rows(.Rows.Count - 1).Cells(TDBGridCol_Currency_Rate).Value = 0
                .Rows(.Rows.Count - 1).Cells(TDBGridCol_Base_Amount).Value = 0
            End If


            'PN30096 - Datasure
            SetBaseCurrency(.Rows.Count - 1)

            'PN29951 - Datasure
            CalculateBaseAmount(.Rows.Count - 1)

            CalcTotalBalance()

            If Not IsNothing(.CurrentRow) Then
                .CurrentCell = .Rows(.Rows.Count - 1).Cells(1) 'PM029873
            End If

            .Refresh()
            .Focus()
            .FirstDisplayedScrollingRowIndex = .Rows.Count - 1
            '.ClearSelection()
            .CurrentCell.Selected = True

            .BeginEdit(True) 'PM029873
        End With

    End Sub

    Private Sub cmdPaste_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPaste.Click

        Dim sTempStr As String = ""
        Dim vArrayCols As Object
        Dim lAccountID As Integer
        Dim bFound As Boolean
        Dim oCurrency As VB6.ListBoxItem
        Dim oDepartment As VB6.ListBoxItem
        Dim sTextData As String = My.Computer.Clipboard.GetText()
        Dim bPerXUsed As Boolean = False

        m_bPaste = True
        'validate clipboard string
        If (sTextData.IndexOf("BRANCH") + 1) < 1 Or (sTextData.IndexOf("JOURNAL TYPE") + 1) < 1 Or (sTextData.IndexOf("Account Code") + 1) < 1 Or (sTextData.IndexOf("Underwriting Year") + 1) < 1 Then
            MessageBox.Show("The pasted spreadsheet is not valid. Please" & Strings.Chr(13) & Strings.Chr(10) & " ensure that " &
                            "you have selected the entire sheet" & Strings.Chr(13) & Strings.Chr(10) & " before copying to the clipboard." &
                            "", "Invalid spreadsheet format", MessageBoxButtons.OK, MessageBoxIcon.Error)
            m_bPaste = False
            Exit Sub
        End If

        'get rows array
        Dim vArrayRows As Object = sTextData.Split(CChar(Strings.Chr(13) & Strings.Chr(10)))


        For lRow As Integer = 0 To vArrayRows.GetUpperBound(0)


            sTempStr = CStr(vArrayRows(lRow)).Replace(Strings.Chr(9), "")
            If sTempStr.Trim() <> "" Then

                'get coloumns array


                vArrayCols = CStr(vArrayRows(lRow)).Split(CChar(Strings.Chr(9)))

                If lRow > 10 Then

                    '------ add new row in grid -----
                    cmdAdd_Click(cmdAdd, New EventArgs())

                End If


                For lCol As Integer = 0 To vArrayCols.GetUpperBound(0)


                    sTempStr = CStr(vArrayCols(lCol))

                    If sTempStr.Trim() <> "" Then

                        '------------  validate Branches  ---------
                        If lRow = 2 And lCol = 2 Then
                            bFound = False
                            For i As Integer = 0 To cboBranches.Items.Count - 1
                                If VB6.GetItemString(cboBranches, i) = sTempStr Then
                                    cboBranches.SelectedIndex = i
                                    bFound = True
                                    Exit For
                                End If
                            Next
                            If Not bFound Then
                                MessageBox.Show("Invalid Branch :" & sTempStr & Strings.Chr(9), "Validation failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                cboBranches.Focus()
                                m_bPaste = False
                                Exit Sub
                            End If

                            '------------  validate Reverse Date  ---------
                        ElseIf lRow = 2 And lCol = 5 Then
                            If Not Information.IsDate(sTempStr) Then
                                MessageBox.Show("Invalid Reverse Date :" & sTempStr & Strings.Chr(9), "Validation failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                dtpReverseDate.Focus()
                                m_bPaste = False
                                Exit Sub
                            Else
                                dtpReverseDate.Value = gPMFunctions.ToSafeDate(sTempStr)
                            End If

                            '------------  validate Occurs  ---------
                        ElseIf lRow = 2 And lCol = 7 Then
                            Dim dbNumericTemp As Double
                            If Not Double.TryParse(sTempStr, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                                MessageBox.Show("Invalid Number (Occurs) :" & sTempStr & Strings.Chr(9), "Validation failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                txtOccurs.Focus()
                                m_bPaste = False
                                Exit Sub
                            Else
                                txtOccurs.Value = CStr(gPMFunctions.ToSafeInteger(sTempStr))
                            End If

                            '------------  validate OccursPer(0)  ---------
                        ElseIf lRow = 3 And lCol = 7 Then
                            Dim dbNumericTemp2 As Double
                            If Not Double.TryParse(sTempStr, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                                MessageBox.Show("Invalid Number (Occurs) :" & sTempStr & Strings.Chr(9), "Validation failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                txtOccursPer(0).Focus()
                                m_bPaste = False
                                Exit Sub
                            Else
                                'udOccursPer(0).Value = 1
                                txtOccursPer(0).Value = CStr(gPMFunctions.ToSafeInteger(sTempStr))
                                bPerXUsed = True
                            End If

                            '------------  validate Document Type  ---------
                        ElseIf lRow = 4 And lCol = 2 Then
                            bFound = False
                            For i As Integer = 0 To cmbDocumentType.ListCount - 1
                                If cmbDocumentType.ItemDescription(i) = sTempStr Then
                                    cmbDocumentType.ItemId = i
                                    bFound = True
                                    Exit For
                                End If
                            Next
                            If Not bFound Then
                                MessageBox.Show("Invalid Document Type :" & sTempStr & Strings.Chr(9), "Validation failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                cmbDocumentType.Focus()
                                m_bPaste = False
                                Exit Sub
                            End If

                            '------------  validate OccursPer(1)  ---------
                        ElseIf lRow = 4 And lCol = 7 Then
                            If bPerXUsed Then
                                MessageBox.Show("Please Enter only one of Period / Month / Quarter " & Strings.Chr(9), "Validation failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                txtOccursPer(1).Focus()
                                m_bPaste = False
                                Exit Sub
                            Else
                                Dim dbNumericTemp3 As Double
                                If Not Double.TryParse(sTempStr, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                                    MessageBox.Show("Invalid Number (Occurs) :" & sTempStr & Strings.Chr(9), "Validation failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    txtOccursPer(1).Focus()
                                    m_bPaste = False
                                    Exit Sub
                                Else
                                    'udOccursPer(1).Value = 1
                                    txtOccursPer(1).Value = CStr(gPMFunctions.ToSafeInteger(sTempStr))
                                    bPerXUsed = True
                                End If
                            End If

                            '------------  validate Document Date  ---------
                        ElseIf lRow = 5 And lCol = 2 Then
                            If Not Information.IsDate(sTempStr) Then
                                MessageBox.Show("Invalid Reverse Date :" & sTempStr & Strings.Chr(9), "Validation failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                dtpDocumentDate.Focus()
                                m_bPaste = False
                                Exit Sub
                            Else
                                dtpDocumentDate.Value = gPMFunctions.ToSafeDate(sTempStr)
                            End If

                            '------------  validate OccursPer(2)  ---------
                        ElseIf lRow = 5 And lCol = 7 Then
                            If bPerXUsed Then
                                MessageBox.Show("Please Enter only one of Period / Month / Quarter " & Strings.Chr(9), "Validation failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                txtOccursPer(2).Focus()
                                m_bPaste = False
                                Exit Sub
                            Else
                                Dim dbNumericTemp4 As Double
                                If Not Double.TryParse(sTempStr, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                                    MessageBox.Show("Invalid Number (Occurs) :" & sTempStr & Strings.Chr(9), "Validation failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    txtOccursPer(2).Focus()
                                    m_bPaste = False
                                    Exit Sub
                                Else
                                    'udOccursPer(2).Value = 1
                                    txtOccursPer(2).Value = CStr(gPMFunctions.ToSafeInteger(sTempStr))
                                End If
                            End If

                            '------------  validate Document Comment  ---------
                        ElseIf lRow = 6 And lCol = 2 Then
                            txtComment.Text = sTempStr

                            '------------  validate Transactions  ---------
                        ElseIf lRow > 10 Then


                            Select Case lCol
                                '---- validate Account code ----
                                Case 1
                                    lAccountID = 0

                                    m_lReturn = m_oAccount.GetAccountID(sTempStr, lAccountID)
                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                                        gPMFunctions.RaiseError("m_oAccount.GetAccountID", "Function failed", gPMConstants.PMELogLevel.PMLogError)
                                    End If
                                    If lAccountID = 0 Then
                                        MessageBox.Show("Invalid Account Code :" & sTempStr & Strings.Chr(9), "Validation failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        tdgTransactions.Refresh()
                                        m_bPaste = False
                                        Exit Sub
                                    Else
                                        tdgTransactions.Rows(tdgTransactions.Rows.Count - 1).Cells(TDBGridCol_Account_id).Value = lAccountID
                                        tdgTransactions.Rows(tdgTransactions.Rows.Count - 1).Cells(TDBGridCol_Account).Value = sTempStr.ToUpper()
                                    End If

                                    '---- validate Currency ----
                                Case 2
                                    bFound = False

                                    For counter As Integer = 0 To CType(tdgTransactions.Columns(TDBGridCol_Currency), DataGridViewComboBoxColumn).Items.Count - 1
                                        oCurrency = CType(CType(tdgTransactions.Columns(TDBGridCol_Currency), DataGridViewComboBoxColumn).Items(counter), VB6.ListBoxItem)
                                        If oCurrency.ItemString = sTempStr Then
                                            tdgTransactions.Rows(tdgTransactions.Rows.Count - 1).Cells(TDBGridCol_Currency_id).Value = oCurrency.ItemData
                                            tdgTransactions.Rows(tdgTransactions.Rows.Count - 1).Cells(TDBGridCol_Currency).Value = CType(tdgTransactions.Rows(tdgTransactions.Rows.Count - 1).Cells(TDBGridCol_Currency), DataGridViewComboBoxCell).Items(counter)
                                            bFound = True
                                            Exit For
                                        End If


                                    Next

                                    If Not bFound Then
                                        MessageBox.Show("Invalid Currency :" & sTempStr & Strings.Chr(9), "Validation failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        tdgTransactions.Refresh()
                                        m_bPaste = False
                                        Exit Sub
                                    Else
                                        'tdgTransactions.Rows(tdgTransactions.Rows.Count - 1).Cells(TDBGridCol_Currency).Value = sTempStr
                                    End If

                                    '---- validate Amount ----
                                Case 3
                                    'Dim dbNumericTemp5 As Double
                                    If Not IsNumeric(sTempStr) Then
                                        MessageBox.Show("Invalid Amount :" & sTempStr & Strings.Chr(9), "Validation failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        tdgTransactions.Refresh()
                                        m_bPaste = False
                                        Exit Sub
                                    Else

                                        tdgTransactions.Rows(tdgTransactions.Rows.Count - 1).Cells(TDBGridCol_Amount).Value = sTempStr
                                        'calculate base amount
                                        CalculateBaseAmount(tdgTransactions.Rows.Count - 1)
                                    End If

                                    '---- validate Currency Rate ----
                                Case 4
                                    'Dim dbNumericTemp6 As Double
                                    If Not IsNumeric(sTempStr) Then
                                        MessageBox.Show("Invalid Currency Rate :" & sTempStr & Strings.Chr(9), "Validation failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        tdgTransactions.Refresh()
                                        m_bPaste = False
                                        Exit Sub
                                    Else

                                        tdgTransactions.Rows(tdgTransactions.Rows.Count - 1).Cells(TDBGridCol_Currency_Rate).Value = sTempStr
                                        '---Base Amount = Amount * DefinedCurrencyRate
                                        tdgTransactions.Rows(tdgTransactions.Rows.Count - 1).Cells(TDBGridCol_Base_Amount).Value = CStr(gPMFunctions.ToSafeCurrency(tdgTransactions.Rows(tdgTransactions.Rows.Count - 1).Cells(TDBGridCol_Amount).Value) * gPMFunctions.ToSafeCurrency(sTempStr))
                                    End If

                                    '---- validate Alt Reference ----  '(RC) QBENZ014
                                Case 6
                                    tdgTransactions.Rows(tdgTransactions.Rows.Count - 1).Cells(TDBGridCol_AltRef).Value = sTempStr

                                    '---- validate Comment ----
                                Case 7

                                    tdgTransactions.Rows(tdgTransactions.Rows.Count - 1).Cells(TDBGridCol_Comment).Value = sTempStr
                                    '---- validate Insurance Reference ----
                                Case 12

                                    tdgTransactions.Rows(tdgTransactions.Rows.Count - 1).Cells(TDBGridCol_Insurance_Ref).Value = sTempStr
                                    '---- validate Purchase Order ----
                                Case 14

                                    tdgTransactions.Rows(tdgTransactions.Rows.Count - 1).Cells(TDBGridCol_Purchase_Order).Value = sTempStr
                                    '---- validate Purchase Invoice ----
                                Case 15

                                    tdgTransactions.Rows(tdgTransactions.Rows.Count - 1).Cells(TDBGridCol_Purchase_Invoice).Value = sTempStr
                                    '---- validate Department ----
                                Case 16

                                    bFound = False

                                    For counter As Integer = 0 To CType(tdgTransactions.Columns(TDBGridCol_Department), DataGridViewComboBoxColumn).Items.Count - 1
                                        oDepartment = CType(CType(tdgTransactions.Columns(TDBGridCol_Department), DataGridViewComboBoxColumn).Items(counter), VB6.ListBoxItem)
                                        If oDepartment.ItemString = sTempStr Then
                                            tdgTransactions.Rows(tdgTransactions.Rows.Count - 1).Cells(TDBGridCol_Department_id).Value = oDepartment.ItemData
                                            tdgTransactions.Rows(tdgTransactions.Rows.Count - 1).Cells(TDBGridCol_Department).Value = CType(tdgTransactions.Rows(tdgTransactions.Rows.Count - 1).Cells(TDBGridCol_Department), DataGridViewComboBoxCell).Items(counter)
                                            bFound = True
                                            Exit For
                                        End If
                                    Next

                                    If Not bFound Then
                                        MessageBox.Show("Invalid Department :" & sTempStr & Strings.Chr(9), "Validation failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        tdgTransactions.Refresh()
                                        m_bPaste = False
                                        Exit Sub
                                    Else
                                        'tdgTransactions.Rows(tdgTransactions.Rows.Count - 1).Cells(TDBGridCol_Department).Value = sTempStr
                                    End If
                                    '---- validate Underwriting Year ----
                                Case 18
                                    bFound = False
                                    Dim oUnderwritingYear As New VB6.ListBoxItem(String.Empty, 0)
                                    For i As Integer = 0 To CType(tdgTransactions.Columns(TDBGridCol_UnderwritingYear), DataGridViewComboBoxColumn).Items.Count - 1
                                        oUnderwritingYear = CType(CType(tdgTransactions.Columns(TDBGridCol_UnderwritingYear), DataGridViewComboBoxColumn).Items(i), VB6.ListBoxItem)
                                        If (oUnderwritingYear.ItemString = sTempStr) Then
                                            tdgTransactions.Rows(tdgTransactions.Rows.Count - 1).Cells(TDBGridCol_UnderwritingYear_id).Value = oUnderwritingYear.ItemData
                                            tdgTransactions.Rows(tdgTransactions.Rows.Count - 1).Cells(TDBGridCol_UnderwritingYear).Value = CType(tdgTransactions.Rows(tdgTransactions.Rows.Count - 1).Cells(TDBGridCol_UnderwritingYear), DataGridViewComboBoxCell).Items(i)
                                            bFound = True
                                            Exit For
                                        End If
                                    Next
                                    If Not bFound Then
                                        MessageBox.Show("Invalid Underwriting Year :" & sTempStr & Strings.Chr(9), "Validation failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        tdgTransactions.Refresh()
                                        m_bPaste = False
                                        Exit Sub
                                    Else
                                        tdgTransactions.Rows(tdgTransactions.Rows.Count - 1).Cells(TDBGridCol_UnderwritingYear_id).Value = oUnderwritingYear.ItemData ' sTempStr
                                    End If
                            End Select

                            '---- validate Underwriting Year ----
                            '    Case 18
                            '        bFound = False
                            '        'For i As Integer = 0 To CType(tdgTransactions.Columns(TDBGridCol_UnderwritingYear), Artinsoft.Windows.Forms.DataGridViewExtendedColumn).DisplayValues.Count - 1
                            '        '    If CType(tdgTransactions.Columns(TDBGridCol_UnderwritingYear), Artinsoft.Windows.Forms.DataGridViewExtendedColumn).DisplayValues(i).Value = sTempStr Then

                            '        '        'developer guide no. TODO : To Be checked at runtime
                            '        '        'tdgTransactions.Columns(TDBGridCol_UnderwritingYear_id).Text = CType(tdgTransactions.Columns(TDBGridCol_UnderwritingYear), Artinsoft.Windows.Forms.DataGridViewExtendedColumn).DisplayValues(i).DisplayValue
                            '        '        bFound = True
                            '        '        Exit For
                            '        '    End If
                            '        'Next

                            '        For i As Integer = 0 To CType(tdgTransactions.Columns(TDBGridCol_UnderwritingYear), DataGridViewExtendedColumn).DisplayValues.Count - 1
                            '            'If CType(tdgTransactions.Columns(TDBGridCol_UnderwritingYear), DataGridViewExtendedColumn).DisplayValues(i).Value.Equals(tdgTransactions.Rows(.Rows.Count - 1).Cells(TDBGridCol_UnderwritingYear)) Then
                            '            If CType(tdgTransactions.Columns(TDBGridCol_UnderwritingYear), DataGridViewExtendedColumn).DisplayValues(i).Value.Equals(sTempStr) Then
                            '                tdgTransactions.Rows(tdgTransactions.Rows.Count - 1).Cells(TDBGridCol_UnderwritingYear_id).Value = CType(tdgTransactions.Columns(TDBGridCol_UnderwritingYear), DataGridViewExtendedColumn).DisplayValues(i).Value
                            '                bFound = True
                            '                Exit For
                            '            End If
                            '        Next
                            '        If Not bFound Then
                            '            MessageBox.Show("Invalid Underwriting Year :" & sTempStr & Strings.Chr(9), "Validation failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            '            tdgTransactions.Refresh()
                            '            Exit Sub
                            '        Else
                            '            tdgTransactions.Rows(tdgTransactions.Rows.Count - 1).Cells(TDBGridCol_UnderwritingYear).Value = sTempStr
                            '        End If

                            'End Select

                            'update XDBArray
                            tdgTransactions.UpdateCurrentRow()
                            tdgTransactions.Refresh()

                        End If
                    End If
                Next
            End If
        Next

        'recalculate Total
        CalcTotalBalance()
        m_bPaste = False
    End Sub

    Private Sub cmdRemove_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRemove.Click

        If tdgTransactions.RowsCount > 0 Then

            tdgTransactions.DeleteCurrentRow()
            tdgTransactions.UpdateCurrentRow()

            'recalculate Total
            CalcTotalBalance()

        End If
    End Sub


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



            lDocumentType = CType(ctlDocType, UserControls.TypeTable).ItemId
            ' Update the controls depending on the document type

            m_lReturn = m_oDocument.GetGroupIDFromTypeID(v_lDocumentTypeID:=lDocumentType, r_lDocTypeGroupID:=lDocTypeGroupID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

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
            dtpReverseDate.Enabled = bReverseEnabled

            lblOccurs.Enabled = bRecurEnabled
            txtOccurs.Enabled = bRecurEnabled
            txtOccurs.Enabled = bRecurEnabled
            lblTimes.Enabled = bRecurEnabled

            ' extras
            For iLoop1 As Integer = 0 To 2
                optOccurs(iLoop1).Enabled = bRecurEnabled
                txtOccursPer(iLoop1).Enabled = bRecurEnabled
                lblOccursPer(iLoop1).Enabled = bRecurEnabled
                ' udOccursPer(iLoop1).Enabled = bRecurEnabled
            Next iLoop1

            ' Default the values

            Select Case lDocTypeGroupID
                ' Recurring document
                Case gACTLibrary.ACTDocTypeGroupRecur
                    txtOccurs.Value = CStr(1)
                    txtOccursPer(0).Value = "1"
                    txtOccursPer(1).Value = Nothing
                    txtOccursPer(2).Value = Nothing

                    ' Reverse document
                Case gACTLibrary.ACTDocTypeGroupReverse
                    txtOccurs.Value = Nothing
                    dtpReverseDate.Value = DateTime.FromOADate(dtpReverseDate.Value.ToOADate()).AddDays(1)
                    For iLoop1 As Integer = 0 To 2
                        txtOccursPer(iLoop1).Value = Nothing
                    Next iLoop1

                    ' Something else!
                Case Else
                    txtOccurs.Value = Nothing
                    For iLoop1 As Integer = 0 To 2
                        txtOccursPer(iLoop1).Value = Nothing
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

    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
            Me.Activate()
        End If

    End Sub

    Private Sub Form_Initialize_Renamed()

        Try

            iPMFunc.ShowFormInTaskBar_Attach()

        Catch excep As System.Exception


            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
            ' Log Error
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        End Try

    End Sub

    ''' <summary>
    ''' Load subroutine for the form.
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        Try
            Me.cmbDocumentType.Table = UserControls.TypeTable.actTable.actDocumentType
            Me.cmbDocumentType.TableName = "DocumentType"
            Me.cmbDocumentType.FirstItem = ""
            cmbDocumentType.ItemId = gACTLibrary.ACTDocTypeJournal
            iPMFunc.ShowFormInTaskBar_Detach()
            ResetForm()
            cmbDocumentType.Focus()
            cmbDocumentType.Select()
            For lCount As Integer = 0 To cboBranches.Items.Count - 1
                If VB6.GetItemData(cboBranches, lCount) = g_iSourceID Then
                    cboBranches.SelectedIndex = lCount
                    Exit For
                End If
            Next
        Catch excep As System.Exception
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

            'Log Error
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
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


            If UnloadMode <> vbFormCode Then

                ' Set the interface status.
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel

                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Destroy business components


            m_oDocument.Dispose()

            m_oDocument = Nothing


            m_oDocPost.Dispose()

            m_oDocPost = Nothing


            m_oAccount.Dispose()

            m_oAccount = Nothing


            m_oCCYConvert.Dispose()

            m_oCCYConvert = Nothing


            m_oAuditSet.Dispose()

            m_oAuditSet = Nothing

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


    Private Sub tdgTransactions_CellEndEdit(ByVal eventSender As Object, ByVal eventArgs As DataGridViewCellEventArgs) Handles tdgTransactions.CellEndEdit
        Dim ColIndex As Integer = eventArgs.ColumnIndex
        Dim lRow As Integer = eventArgs.RowIndex
        Dim lAccountID As Integer

        With tdgTransactions
            'Validate Account Code
            If ColIndex = TDBGridCol_Account Then

                lAccountID = 0
                ''''
                If Not (IsDBNull(tdgTransactions.CurrentCell.Value) Or IsNothing(tdgTransactions.CurrentCell.Value)) Then

                    If .CurrentCell.Value.Trim() <> "" Then


                        m_lReturn = m_oAccount.GetAccountID(.CurrentCell.Value, lAccountID)
                        If m_lReturn = gPMConstants.PMEReturnCode.PMError Then
                            gPMFunctions.RaiseError("m_oAccount.GetAccountID", "Function failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        If lAccountID = 0 Then
                            cmdAccountLookup(lRow)
                        Else
                            .Rows(lRow).Cells(TDBGridCol_Account_id).Value = CStr(lAccountID)
                            .CurrentCell.Value = .CurrentCell.Value.ToUpper()
                        End If

                    End If
                End If
            End If

            'Currency conversion
            If ColIndex = TDBGridCol_Amount Or ColIndex = TDBGridCol_Currency_Rate Then

                Dim dbNumericTemp As Double
                If Double.TryParse(Convert.ToString(.Rows(lRow).Cells(TDBGridCol_Amount).Value), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    'calculate base amount
                    CalculateBaseAmount(lRow)

                    'calculate Total Balance
                    CalcTotalBalance()
                End If

            End If
        End With

    End Sub

    Sub CalculateBaseAmount(ByVal rowindx As Integer)

        Dim vConversionRate As Double

        With tdgTransactions

            Dim dbNumericTemp As Double

            If gPMFunctions.ToSafeDecimal(.Rows(rowindx).Cells(TDBGridCol_Currency_Rate).Value, 0) > 0 Then
                vConversionRate = gPMFunctions.ToSafeDecimal(.Rows(rowindx).Cells(TDBGridCol_Currency_Rate).Value, 0)
                .Rows(rowindx).Cells(TDBGridCol_Currency_Rate).Value = String.Format("{0:f10}", CType(CStr(vConversionRate), Decimal))

                .Rows(rowindx).Cells(TDBGridCol_Base_Amount).Value = String.Format("{0:f2}", CType(CStr(gPMFunctions.ToSafeCurrency(.Rows(rowindx).Cells(TDBGridCol_Amount).Value) * vConversionRate).ToString(), Decimal))
                '	'PN29951  - Datasure
            ElseIf Convert.ToString(.Rows(rowindx).Cells(TDBGridCol_Currency_id).Value) <> "" And Double.TryParse(.Rows(rowindx).Cells(TDBGridCol_Amount).Value, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) And g_iSourceID <> 0 Then
                'PN30096 and PN29951  - Datasure
                'To get the conversion Rate for selected transaction CURRENCY

                m_lReturn = m_oCCYConvert.GetCurrencyRate(.Rows(rowindx).Cells(TDBGridCol_Currency_id).Value, g_iSourceID, m_dtEffectiveDate, vConversionRate)

                '30645 'PN 33096 (RC)

                If vConversionRate.Equals(0) Then
                    MessageBox.Show("Exchange rate for this currency is not available. " &
                                    "Please set the Exchange rate!", "Manual Journal", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    m_lReturn = SetBaseCurrency(rowindx)
                    .RefreshCurrentRow()
                    Exit Sub
                End If

                '----END
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oCCYConvert.GetCURRENCYRate", "Function failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                .Rows(rowindx).Cells(TDBGridCol_Currency_Rate).Value = String.Format("{0:f10}", CType(CStr(vConversionRate), Decimal))

                .Rows(rowindx).Cells(TDBGridCol_Base_Amount).Value = String.Format("{0:f2}", CType(CStr(gPMFunctions.ToSafeCurrency(.Rows(rowindx).Cells(TDBGridCol_Amount).Value) * vConversionRate).ToString(), Decimal))
                .RefreshCurrentRow()
            End If

        End With

    End Sub


    'Private Sub tdgTransactions_CellLeave(ByVal eventSender As Object, ByVal eventArgs As DataGridViewCellEventArgs) Handles tdgTransactions.CellLeave
    '    Dim Cancel As Integer = 0

    '    If Not ValidateGridColoum() Then
    '        Cancel = 1
    '    End If

    '    If Cancel <> 0 Then
    '        tdgTransactions.CancelEdit()
    '    End If
    'End Sub

    Function ValidateGridColoum(ByVal rowindx As Integer, ByVal columnindx As Integer) As Boolean

        Dim result As Boolean = False
        result = True

        With tdgTransactions

            'validate amount
            If columnindx = TDBGridCol_Amount Then

                If tdgTransactions.CurrentCell.EditedFormattedValue = "" Then
                    .Rows(rowindx).Cells(TDBGridCol_Amount).Value = "0.00"
                End If
                If Not IsNumeric(tdgTransactions.CurrentCell.EditedFormattedValue) Then
                    If m_nCount = 1 Then
                        MessageBox.Show("Please enter valid amount" & Strings.Chr(9), "Invalid Amount", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                    invalidAmount = True
                    result = False
                End If
            ElseIf columnindx = TDBGridCol_Currency_Rate Then
                If Convert.ToString(tdgTransactions.CurrentCell.EditedFormattedValue) = "" Then
                    .Rows(rowindx).Cells(TDBGridCol_Currency_Rate).Value = "0.00"

                End If
                If Not IsNumeric(tdgTransactions.CurrentCell.EditedFormattedValue) Then
                    MessageBox.Show("Please enter valid rate" & Strings.Chr(9), "Invalid Rate", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    result = False
                End If
            End If

        End With

        Return result
    End Function

    Private Sub tdgTransactions_ComboSelect(ByVal eventSender As Object, ByVal eventArgs As DataGridViewCellEventArgs) Handles tdgTransactions.ComboSelect
        Dim ColIndex As Integer = eventArgs.ColumnIndex
        Dim oCurrency As VB6.ListBoxItem
        With tdgTransactions
            'PN30298 - Datasure


            'developer guide no. 53 (No Solution)
            If ColIndex = TDBGridCol_Currency Then



                For counter As Integer = 0 To CType(tdgTransactions.Columns(TDBGridCol_Currency), DataGridViewComboBoxColumn).Items.Count - 1
                    oCurrency = CType(CType(tdgTransactions.Columns(TDBGridCol_Currency), DataGridViewComboBoxColumn).Items(counter), VB6.ListBoxItem)
                    If oCurrency.ItemData = m_iCurrencyID Then
                        tdgTransactions.Rows(tdgTransactions.Rows.Count - 1).Cells(TDBGridCol_Currency_id).Value = oCurrency.ItemData
                        tdgTransactions.Rows(tdgTransactions.Rows.Count - 1).Cells(TDBGridCol_Currency).Value = CType(tdgTransactions.Rows(tdgTransactions.Rows.Count - 1).Cells(TDBGridCol_Currency), DataGridViewComboBoxCell).Items(counter)
                        Exit For
                    End If


                Next




                For i As Integer = 0 To CType(.Columns(TDBGridCol_Currency), DataGridViewComboBoxColumn).Items.Count - 1

                    If CType(.Rows(.Rows.Count - 1).Cells(TDBGridCol_Currency), DataGridViewComboBoxCell).Items(i).Equals(.Rows(.Rows.Count - 1).Cells(TDBGridCol_Currency)) Then
                        '        
                        If CType(.Columns(TDBGridCol_Currency), Artinsoft.Windows.Forms.DataGridViewExtendedColumn).DisplayValues(i).Value.Equals(.Rows(.Rows.Count - 1).Cells(TDBGridCol_Currency)) Then


                            .Rows(.Rows.Count - 1).Cells(TDBGridCol_Currency_Rate).Value = ""
                            'calculate base amount
                            CalculateBaseAmount(.Rows.Count - 1)

                            'calculate Total Balance
                            CalcTotalBalance()

                            Exit For
                        End If
                    End If
                Next
            End If

            ' Get Underwriting_YearId

            If ColIndex = TDBGridCol_UnderwritingYear Then
                For i As Integer = 0 To CType(.Columns(TDBGridCol_UnderwritingYear), DataGridViewExtendedColumn).DisplayValues.Count - 1
                    If CType(.Columns(TDBGridCol_UnderwritingYear), DataGridViewExtendedColumn).DisplayValues(i).Value.Equals(.Rows(.Rows.Count - 1).Cells(TDBGridCol_UnderwritingYear)) Then
                        .Rows(.Rows.Count - 1).Cells(TDBGridCol_UnderwritingYear_id).Value = CType(.Columns(TDBGridCol_UnderwritingYear), DataGridViewExtendedColumn).DisplayValues(i).Value
                        Exit For
                    End If
                Next
            End If

            'Get DepartmentId


            If ColIndex = TDBGridCol_Department Then
                For i As Integer = 0 To CType(.Columns(TDBGridCol_Department), Artinsoft.Windows.Forms.DataGridViewExtendedColumn).DisplayValues.Count - 1
                    If CType(.Columns(TDBGridCol_Department), Artinsoft.Windows.Forms.DataGridViewExtendedColumn).DisplayValues(i).Value.Equals(.Rows(.Rows.Count - 1).Cells(TDBGridCol_Department).Value) Then
                        .Rows(.Rows.Count - 1).Cells(TDBGridCol_Department_id) = CType(.Columns(TDBGridCol_Department), DataGridViewExtendedColumn).DisplayValues(i).Key
                        Exit For
                    End If
                Next
            End If

            .Enabled = False

            'developer guide no. 53 (No Solution)
            .Enabled = True
            .Focus()
        End With


    End Sub



    Private Sub cmdAccountLookup(ByVal rowindx As Integer)

        Dim vKeyArray(1, 8) As Object

        Dim sStartKey As String = ""

        With m_oAccountLookup

            ' setup the key array to call find account

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.ACTKeyNameShortCode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = Convert.ToString(tdgTransactions.Rows(rowindx).Cells(TDBGridCol_Account).Value)
            sStartKey = Convert.ToString(tdgTransactions.Rows(rowindx).Cells(TDBGridCol_Account).Value)

            m_lReturn = .Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Error. Unable to initialise FindAccount object", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            m_lReturn = .SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vTransactionType:=gPMConstants.PMTransactionTypeGeneric, vEffectiveDate:=DateTime.Now)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Error. FindAccount failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            m_lReturn = .SetKeys(vKeyArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Error. FindAccount SetKeys failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If


            m_lReturn = .Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Error. Unable to start FindAccount object", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If


            If .Status <> gPMConstants.PMEReturnCode.PMCancel Then

                m_lReturn = .GetKeys(vKeyArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Error. FindAccount GetKeys failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                    tdgTransactions.Rows(rowindx).Cells(TDBGridCol_Account_id).Value = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0))

                    tdgTransactions.Rows(rowindx).Cells(TDBGridCol_Account).Value = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3))
                End If

            End If

        End With

    End Sub
    Private Function GetCompanyCurrencies(ByVal v_lMode As Integer, ByVal v_lDefaultCurrencyId As Integer, ByRef oCurrency As Artinsoft.Windows.Forms.ValueCollection, ByVal v_lCompanyID As Integer) As Integer
        Dim result As Integer = 0

        Dim sText, sCode As String
        Dim vCompanyCurrencies(,) As Object
        Dim sMessage As String = ""

        Dim oValueItem As New Artinsoft.Windows.Forms.DisplayValue

        Dim oCompanyCurrency As bACTCompanyCurrency.Form
        Dim vValue As Byte

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get a Company Currency Business Object
            Dim temp_oCompanyCurrency As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oCompanyCurrency, "bACTCompanyCurrency.Form", vInstanceManager:="ClientManager")
            oCompanyCurrency = temp_oCompanyCurrency

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get an instance of bACTCompanyCurrency.Form.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")
                Return result
            End If

            'Set company id from parent object
            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, v_vBranch:=m_iBranchID, r_vUnderwriting:=CStr(vValue))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GETPRODUCTOPTIONVALUE Failed for option " & gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, vApp:=ACApp, vClass:=ACClass, vMethod:="GetCompanyCurrencies")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set default company id
            If vValue <> 1 Then

                oCompanyCurrency.CompanyID = 1
            Else

                oCompanyCurrency.CompanyID = g_iSourceID
            End If

            'Set company id from parameter
            If v_lCompanyID <> 0 Then

                oCompanyCurrency.CompanyID = v_lCompanyID
            End If

            ' Get list of company currencies

            m_lReturn = oCompanyCurrency.GetCompanyCurrencies(lNumberOfRecords:=0, vResultArray:=vCompanyCurrencies, vnMode:=v_lMode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'DJM 02/02/2004 : Don't display error if it didn't find any.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    sMessage = "Unable to retrieve currency details."

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="GetCompanyCurrencies")

                End If

                Return m_lReturn
            End If

            ' Load the combo box with the list
            With oCurrency


                For nRow As Integer = vCompanyCurrencies.GetLowerBound(1) To vCompanyCurrencies.GetUpperBound(1)


                    sCode = CStr(vCompanyCurrencies(ACTCurrencyISOCode, nRow)).Trim()

                    sText = sCode & " " & CStr(vCompanyCurrencies(ACTCurrencyDescription, nRow)).Trim()
                    oValueItem = New DisplayValue

                    oValueItem.Value = vCompanyCurrencies(ACTCurrencyId, nRow)
                    oValueItem.Key = sText
                    .Add(oValueItem)

                Next nRow

            End With


            oCompanyCurrency.Dispose()
            oCompanyCurrency = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get company currencies", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCompanyCurrencies", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Function GetLookupValues(ByRef oValueItems As Artinsoft.Windows.Forms.ValueCollection, ByVal sTableName As String) As Integer
        Dim result As Integer = 0

        Dim vTableArray(,) As Object
        Dim iLookupType As gPMConstants.PMELookupType
        Dim sCaption, sID As String

        Dim vLookupItems(,) As Object

        Dim oPMLookup As BPMLOOKUP.Business
        Dim oValueItem As New Artinsoft.Windows.Forms.DisplayValue

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Business
            Dim temp_oPMLookup As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMLookup, "bPMLookup.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMLookup = temp_oPMLookup

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Format the Input Array
            ReDim vTableArray(gPMConstants.PMELookupInArrayColPos.PMLookupWhereClause, 0)

            ' Set the Table Name

            vTableArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = sTableName
            If sTableName.ToUpper() = "UNDERWRITING_YEAR" Then
                iLookupType = gPMConstants.PMELookupType.PMLookupAllEffective
            Else
                iLookupType = gPMConstants.PMELookupType.PMLookupAllWithDeleted
            End If

            ' Get the lookup values

            m_lReturn = oPMLookup.GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTableArray, iLanguageID:=g_iLanguageID, dtEffectiveDate:=DateTime.Now, vResultArray:=vLookupItems)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")
                Return result
            End If

            ' Add to the drop down list box.
            ' As we are only working with one Lookup table at a time,
            ' we do not need to bother with the start and no of items
            If Information.IsArray(vLookupItems) Then

                For lRow As Integer = vLookupItems.GetLowerBound(1) To vLookupItems.GetUpperBound(1)

                    ' RAW 15/07/2003 : CQ258 : added code to handle deleted entries
                    ' extract details from result set

                    sCaption = CStr(vLookupItems(gPMConstants.PMELookupOutArrayColPos.PMLookupCaption, lRow))

                    sID = CStr(vLookupItems(gPMConstants.PMELookupOutArrayColPos.PMLookupID, lRow))
                    oValueItem = New DisplayValue
                    ' add entry
                    oValueItem.Value = CInt(sID)
                    oValueItem.Key = sCaption
                    oValueItems.Add(oValueItem)

                Next lRow
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", excep:=excep)

            Return result

        End Try
    End Function


    Private Sub tdgTransactions_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs) Handles tdgTransactions.Validating
        Dim Cancel As Boolean = eventArgs.Cancel
        'PN30298 - Datasure

        'If Not ValidateGridColoum() Then
        '    Cancel = 1
        'End If

        'PN30298 - Datasure
        With tdgTransactions
            'PN30886 BookMark must not be NULL
        End With
        'update XArrayDB
        ''  tdgTransactions.Update
        'PN30298 - Datasure
        With tdgTransactions
            .RefetchCurrentRow()
            .Enabled = False

            'developer guide no. 53 (No Solution)
            .Enabled = True
        End With



        eventArgs.Cancel = Cancel
    End Sub

    Private Function GetBranches(ByRef oBranchCombo As ComboBox) As Integer
        Dim result As Integer = 0

        Dim vSourceArray(,) As Object

        Dim oPMUser As Bpmuser.Business

        Dim oPMSource As bPMSource.Business
        Dim vIsDeleted As gPMConstants.PMEReturnCode
        Dim vAllowAccounts As gPMConstants.PMEReturnCode

        Try

            Dim temp_oPMUser As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMUser, "bPMUser.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMUser = temp_oPMUser
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            'PN 33098 (RC)
            '------------------------------------Start

            m_lReturn = oPMUser.GetUserSources(r_vSourceArray:=vSourceArray, v_vUserID:=g_iUserID, v_bIncludeDeletedSources:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Get an instance of the business object via the object manager.
            Dim temp_oPMSource As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMSource, "bPMSource.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMSource = temp_oPMSource
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            'don't extract if a Branch isdeleted and allow accounting is disabled
            For i As Integer = 0 To vSourceArray.GetUpperBound(1)

                'Developer Guide no. 162
                m_lReturn = oPMSource.GetDetails(vSourceID:=vSourceArray(0, i))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If


                m_lReturn = oPMSource.GetNext(vIsDeleted:=vIsDeleted, vAllowAccounts:=vAllowAccounts)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If


                'Developer Guide no. 162
                vSourceArray(3, i) = 0
                If (vIsDeleted = gPMConstants.PMEReturnCode.PMFalse) Or (vIsDeleted = gPMConstants.PMEReturnCode.PMTrue And vAllowAccounts = gPMConstants.PMEReturnCode.PMTrue) Then

                    'Developer Guide no 162
                    vSourceArray(3, i) = 1
                End If
            Next i

            oPMSource = Nothing
            '------------------------------------End

            With oBranchCombo

                'Clear combo.
                .Items.Clear()
                'Developer Guide no. 153
                'Dim NewIndex As Integer = .Items.Add(New VB6.ListBoxItem("(none)", 0))

                '.Items.Add("(none)")

                'VB6.SetItemData(oBranchCombo, "NewIndex", 0)


                'Populate branch combo

                'Developer Guide no. 162
                For i As Integer = 0 To vSourceArray.GetUpperBound(1)
                    'PN 33098 (RC)

                    'Developer Guide no. 162
                    If CDbl(vSourceArray(3, i)) = 1 Then

                        'Developer Guide no. 153,162
                        Dim NewIndex1 As Integer = .Items.Add(New VB6.ListBoxItem(Trim(vSourceArray(2, i)), vSourceArray(0, i)))

                        '.Items.Add(CStr(vSourceArray(3, i)).Trim())



                        'VB6.SetItemData(oBranchCombo, "NewIndex", CInt(vSourceArray(1, i)))
                    End If
                Next i

            End With

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBranches", excep:=excep)

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
            If Information.IsDate(dtpDocumentDate.Value) Then
                m_dtDocumentDate = (CDate(gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, vFieldValue:=dtpDocumentDate.Value)))
            Else

                dtpDocumentDate.Value = DateTime.Today
                m_dtDocumentDate = (CDate(gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, vFieldValue:=dtpDocumentDate.Value)))
            End If

            m_sComment = txtComment.Text.Trim()
            m_iDocumenttypeID = cmbDocumentType.ItemId
            m_iBranchID = VB6.GetItemData(cboBranches, cboBranches.SelectedIndex)

            ' Reversing document
            If dtpReverseDate.Enabled Then
                m_bReversingDocument = True
                ' Get the reverse date
                If Information.IsDate(dtpReverseDate.Value) Then
                    m_dtReverseDate = (CDate(gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, vFieldValue:=dtpReverseDate.Value)))
                End If
            Else
                m_bReversingDocument = False
            End If

            ' Recurring document
            If txtOccurs.Enabled Then
                m_bRecurringDocument = True
                ' Get the occurances
                Dim dbNumericTemp As Double
                If Double.TryParse(txtOccurs.Value, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
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


    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim sReference As String = ""
        ' Click event of the OK button.

        ' On Error GoTo Err_CmdOKClick
        If m_BtnClicked = "" Then
            m_BtnClicked = "Ok"
        End If

        ' Set the interface status.
        m_lStatus = gPMConstants.PMEReturnCode.PMOK

        m_iTask = gPMConstants.PMEComponentAction.PMAdd

        'Only do this if Adding
        If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then

            m_lReturn = ValidateFormData()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'PN32241
                m_iTask = gPMConstants.PMEComponentAction.PMView
                Exit Sub
            End If

            m_iDocumenttypeID = cmbDocumentType.ItemId

            m_lReturn = m_oDocument.GetAutoNumValues(v_iDocumentTypeID:=m_iDocumenttypeID, r_sGroupCode:=m_sGroupCode, r_sRangeCode:=m_sRangeCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDocument.GetAutoNumValues", "Function failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Call the business to generate the document reference
            '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber

            m_lReturn = m_oDocument.GenerateDocumentReferenceNumber(v_sGroupCode:=m_sGroupCode, v_sRangeCode:=m_sRangeCode, v_iUserID:=g_iUserID, v_iCompanyID:=g_iCompanyID, r_sDocumentRef:=sReference)

            'Start(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            'If m_lReturn = PMTrue And m_lNumber > 0 Then
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue And sReference <> "" Then
                m_lNumber = gPMFunctions.ToSafeLong(sReference)
                'End(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                'Reset flag
                m_bGeneratedNumberUsed = False

                ' Prefix a few zeros
                '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                sReference = m_sRangeCode & sReference
                m_sDocumentRef = sReference

                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = ProcessCommand()

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    ' staStatusBar.Text = "Last document reference: " & m_sDocumentRef
                    _staStatusBar_Panel1.Text = "Last document reference: " & m_sDocumentRef
                Else
                    'staStatusBar.Text = ""
                    _staStatusBar_Panel1.Text = ""
                End If

            Else
                MessageBox.Show("Failed to generate a document reference.", "Error: New Document Reference failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        End If

        If m_lNumber > 0 And Not m_bGeneratedNumberUsed Then

            'POOL the document reference

            m_lReturn = m_oDocument.PoolNumber(v_sGroupCode:=m_sGroupCode, v_sRangeCode:=m_sRangeCode, v_iUserID:=g_iUserID, v_iCompanyID:=g_iCompanyID, r_lNumber:=m_lNumber)

            'Reset Number
            m_lNumber = 0

        Else
            MessageBox.Show("Generated document reference " & sReference, "Document Reference", MessageBoxButtons.OK, MessageBoxIcon.Information)
            If m_BtnClicked = "Ok" Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If
        End If

        Exit Sub



        ' Error Section.

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Exit Sub

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            If MessageBox.Show("Are you sure you want to Cancel your changes?", "Cancel Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then

                ' Check the return value.
                If m_lNumber > 0 And Not m_bGeneratedNumberUsed Then

                    ' POOL the document reference

                    m_lReturn = m_oDocument.PoolNumber(v_sGroupCode:=m_sGroupCode, v_sRangeCode:=m_sRangeCode, v_iUserID:=g_iUserID, v_iCompanyID:=g_iCompanyID, r_lNumber:=m_lNumber)

                    'Reset Number
                    m_lNumber = 0

                End If

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

    Private Sub CalcTotalBalance()


        ' Loop round transactions creating a running total of base amount
        m_cTotalBalance = 0
        For lRow As Integer = 0 To tdgTransactions.RowsCount - 1

            ' Add base amount to running total

            'developer guide no. 53 (No Solution)
            m_cTotalBalance += gPMFunctions.ToSafeCurrency(tdgTransactions.Rows(lRow).Cells(TDBGridCol_Amount).Value)
        Next lRow

        DisplayDocumentBalance()

    End Sub

    Sub DisplayDocumentBalance()

        'Display Doc Balance
        panDocBalance.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_cTotalBalance))

    End Sub

    Private Function ValidateFormData() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim lAccountID As Integer
        Dim sAccountCode As String = ""
        Dim vAmount As String = ""
        Dim vCurrencyID As String = ""
        Dim vCurrencyToMatch As String = ""

        'PN 33124 (RC)
        Dim vReturn, sReturn As String
        Dim vUnderwritingYear As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        If cmbDocumentType.ListIndex = -1 Then
            MessageBox.Show("Please select Document Type" & Strings.Chr(9), "Invalid document type", MessageBoxButtons.OK, MessageBoxIcon.Information)
            cmbDocumentType.Focus()
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If cboBranches.SelectedIndex = -1 Then
            MessageBox.Show("Please select Branch" & Strings.Chr(9), "Invalid branch", MessageBoxButtons.OK, MessageBoxIcon.Information)
            cboBranches.Focus()
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Check that Journal is not post dated
        m_lReturn = CheckDates()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            dtpDocumentDate.Focus()
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'Modified as per checked at runtime
        'If m_GridArray.GetLength(0) = 0 Then
        If m_GridArray.Rows.Count = 0 Then
            MessageBox.Show("Please add transactions." & Strings.Chr(9), "Transactions not added", MessageBoxButtons.OK, MessageBoxIcon.Information)
            cmdAdd.Focus()
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        'validate transactions data

        ' Loop round transactions creating a running total of base amount
        For lRow As Integer = 0 To m_GridArray.Rows.Count - 1 'tdgTransactions.ApproxCount

            ' Add base amount to running total
            'PN30298 - Datasure


            'developer guide no. 53 (No Solution)
            'tdgTransactions.BookMark = lRow


            'Identify Account
            sAccountCode = Convert.ToString(tdgTransactions.Rows(lRow).Cells(TDBGridCol_Account).Value)
            m_lReturn = m_oAccount.GetAccountID(sAccountCode, lAccountID)
            If sAccountCode <> "" Then
                If IsDBNull(tdgTransactions.Rows(lRow).Cells(TDBGridCol_Account_id).Value) Then
                    tdgTransactions.Rows(lRow).Cells(TDBGridCol_Account_id).Value = lAccountID
                End If
            End If
            If m_lReturn = gPMConstants.PMEReturnCode.PMError Then
                If lAccountID < 1 Or sAccountCode = "" Then
                    MessageBox.Show("Invalid Account: " & sAccountCode & Strings.Chr(9), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    If Not IsNothing(tdgTransactions.CurrentRow) Then
                        tdgTransactions.CurrentCell = tdgTransactions.CurrentRow.Cells(TDBGridCol_Account)
                    End If
                    tdgTransactions.Focus()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                gPMFunctions.RaiseError("m_oAccount.GetAccountID", "Function failed", gPMConstants.PMELogLevel.PMLogError)
            ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                MessageBox.Show("Please enter valid account." & Strings.Chr(9), "Invalid Account", MessageBoxButtons.OK, MessageBoxIcon.Information)
                tdgTransactions.Focus()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If lAccountID < 1 Then
                MessageBox.Show("Invalid Account: " & sAccountCode & Strings.Chr(9), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                If Not IsNothing(tdgTransactions.CurrentRow) Then
                    tdgTransactions.CurrentCell = tdgTransactions.CurrentRow.Cells(TDBGridCol_Account)
                End If
                tdgTransactions.Focus()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'validate currency
            vCurrencyID = tdgTransactions.Rows(lRow).Cells(TDBGridCol_Currency_id).Value
            If gPMFunctions.ToSafeCurrency(vCurrencyID) < 1 Then
                MessageBox.Show("Please select Currency " & Strings.Chr(9), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                If Not IsNothing(tdgTransactions.CurrentRow) Then
                    tdgTransactions.CurrentCell = tdgTransactions.CurrentRow.Cells(TDBGridCol_Amount)
                End If 'TDBGridCol_Currency_id
                tdgTransactions.Focus()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'PN32846
            If lRow = 0 Then
                vCurrencyToMatch = tdgTransactions.Rows(lRow).Cells(TDBGridCol_Currency_id).Value
            End If

            If vCurrencyID <> vCurrencyToMatch Then
                MessageBox.Show("Can not create manual journal with different currencies. " & Strings.Chr(9), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                If Not IsNothing(tdgTransactions.CurrentRow) Then
                    tdgTransactions.CurrentCell = tdgTransactions.CurrentRow.Cells(TDBGridCol_Currency)
                End If
                tdgTransactions.Focus()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '----------32846 END

            'validate amount
            vAmount = tdgTransactions.Rows(lRow).Cells(TDBGridCol_Amount).Value
            If gPMFunctions.ToSafeCurrency(vAmount) = 0 Then
                MessageBox.Show("Please enter valid amount" & Strings.Chr(9), "Invalid Amount", MessageBoxButtons.OK, MessageBoxIcon.Information)
                If Not IsNothing(tdgTransactions.CurrentRow) Then
                    tdgTransactions.CurrentCell = tdgTransactions.Rows(lRow).Cells(TDBGridCol_Amount)  'tdgTransactions.CurrentRow.Cells(TDBGridCol_Amount)
                End If
                tdgTransactions.Focus()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'PN 33124 (RC)
            '------------------------------------
            ' Get the product option "Enable Underwriting year Labelling" (68) is enabled (set to 1)
            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTUnderwritingYear, v_vBranch:=g_iSourceID, r_vUnderwriting:=vReturn)
            If gPMFunctions.ToSafeBoolean(vReturn, False) Then

                'Get System Options "UW Year mandatory on Cash and Journals" (5012)
                m_lReturn = iPMFunc.GetSystemOption(kSystemOptionUWYearMandatory, sReturn, g_iSourceID)
                If gPMFunctions.ToSafeBoolean(sReturn, False) Then

                    'validate UnderwritingYear
                    vUnderwritingYear = Convert.ToString(tdgTransactions.Rows(lRow).Cells(TDBGridCol_UnderwritingYear).Value).Trim()
                    If gPMFunctions.ToSafeString(vUnderwritingYear) = "" Then
                        MessageBox.Show("Please select Underwriting Year" & Strings.Chr(9), "Invalid Underwriting Year", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        If Not IsNothing(tdgTransactions.CurrentRow) Then
                            tdgTransactions.CurrentCell = tdgTransactions.CurrentRow.Cells(TDBGridCol_UnderwritingYear)
                        End If
                        tdgTransactions.Focus()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            End If
            '------------------------------------

        Next lRow

        CalcTotalBalance()
        If m_cTotalBalance <> 0 Then
            MessageBox.Show("The Document must balance to zero." & Strings.Chr(9), "Invalid document balance", MessageBoxButtons.OK, MessageBoxIcon.Information)
            cmdAdd.Focus()
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result
    End Function

    Private Sub ResetForm()


        cmbDocumentType.ItemId = gACTLibrary.ACTDocTypeJournal
        cboBranches.SelectedIndex = -1
        dtpDocumentDate.Value = DateTime.Now
        dtpReverseDate.Value = DateTime.Now
        txtComment.Text = ""
        txtOccurs.Text = ""
        optOccurs(0).Checked = True
        txtOccursPer(0).Text = ""
        txtOccursPer(1).Text = ""
        txtOccursPer(2).Text = ""

        ' clear transactions grid
        For lRow As Integer = 0 To tdgTransactions.RowsCount - 1
            tdgTransactions.DeleteCurrentRow()
            tdgTransactions.UpdateCurrentRow()
        Next lRow

        'Reset Total Balance
        m_cTotalBalance = 0
        m_BtnClicked = ""

        DisplayDocumentBalance()

    End Sub

    Private Sub cmdNew_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNew.Click

        If MessageBox.Show("Creating a New Journal will remove changes already made. Do you wish to continue?", "Cancel Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then
            ResetForm()
        End If

    End Sub

    Private Sub cmdApplyNew_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApplyNew.Click

        m_BtnClicked = "ApplyNew"

        m_lReturn = ValidateFormData()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        cmdOK_Click(cmdOK, New EventArgs())
        ResetForm()

    End Sub

    'Private Sub udOccursPer_Change(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
    '    Dim Index As Integer = Array.IndexOf(udOccursPer, eventSender)

    '    If Not optOccurs(Index).Checked Then
    '        optOccurs(Index).Checked = True
    '    End If

    'End Sub

    Private isInitializingComponent As Boolean
    Private Sub optOccurs_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _optOccurs_0.CheckedChanged, _optOccurs_1.CheckedChanged, _optOccurs_2.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            Dim Index As Integer = Array.IndexOf(optOccurs, eventSender)

            If Index = 0 Then


                RemoveHandler _txtOccursPer_1.ValueChanged, AddressOf _txtOccursPer_1_ValueChanged
                RemoveHandler _txtOccursPer_2.ValueChanged, AddressOf _txtOccursPer_2_ValueChanged
                txtOccursPer(1).Value = Nothing
                txtOccursPer(2).Value = Nothing
                txtOccursPer(0).Value = "1"

                AddHandler __txtOccursPer_1.ValueChanged, AddressOf _txtOccursPer_1_ValueChanged
                AddHandler _txtOccursPer_2.ValueChanged, AddressOf _txtOccursPer_2_ValueChanged

            ElseIf Index = 1 Then
                RemoveHandler _txtOccursPer_0.ValueChanged, AddressOf _txtOccursPer_0_ValueChanged

                RemoveHandler _txtOccursPer_2.ValueChanged, AddressOf _txtOccursPer_2_ValueChanged
                txtOccursPer(0).Value = Nothing
                txtOccursPer(2).Value = Nothing
                txtOccursPer(1).Value = "1"
                AddHandler _txtOccursPer_0.ValueChanged, AddressOf _txtOccursPer_0_ValueChanged

                AddHandler _txtOccursPer_2.ValueChanged, AddressOf _txtOccursPer_2_ValueChanged
            ElseIf Index = 2 Then
                RemoveHandler _txtOccursPer_0.ValueChanged, AddressOf _txtOccursPer_0_ValueChanged
                RemoveHandler _txtOccursPer_1.ValueChanged, AddressOf _txtOccursPer_1_ValueChanged

                txtOccursPer(0).Value = Nothing
                txtOccursPer(1).Value = Nothing
                txtOccursPer(2).Value = "1"
                AddHandler _txtOccursPer_0.ValueChanged, AddressOf _txtOccursPer_0_ValueChanged
                AddHandler __txtOccursPer_1.ValueChanged, AddressOf _txtOccursPer_1_ValueChanged

            End If
        End If
    End Sub


    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: CheckDates
    ' PURPOSE: Ensures that journal is not post dated and if so that the user has authority
    ' AUTHOR:
    ' DATE:
    ' RETURNS: PMTrue for success
    ' CHANGES: 21-Jul-2006 RC
    ' ---------------------------------------------------------------------------
    Private Function CheckDates() As Integer
        Dim result As Integer = 0
        Dim bACTUserAuthorities, bACTPeriod As Object


        Dim oPeriod As bACTPeriod.Form

        Dim oUserAuthorities As bACTUserAuthorities.Business
        Dim vDetails, vAccess As Object
        Dim dtPeriodEndDate, dtPeriodDate As Date
        Dim lPeriodId, lPreviousPeriodID As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_dtDocumentDate = dtpDocumentDate.Value

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

            m_lReturn = oPeriod.GetPreviousPeriodID(lPeriodID:=lPeriodId, lPreviousPeriodID:=lPreviousPeriodID)
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
            End If


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


            m_lReturn = oUserAuthorities.GetDetails(vUserID:=g_iUserID)
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


        Catch ex As Exception
            Select Case Information.Err().Number
                'Case Constants.vbObjectError
                Case 5
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogFeedback, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckDates", excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckDates", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

                    Return result
            End Select

        Finally


        End Try
        Return result
    End Function

    'PN30096 - Datasure
    Private Function SetBaseCurrency(ByVal rowindx As Integer) As Integer

        Dim oBusiness As bSIRInsuranceFile.Business
        Dim oCurrency As VB6.ListBoxItem

        Try

            Dim temp_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bSIRInsuranceFile.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oBusiness = temp_oBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If g_iSourceID > 0 Then

                m_lReturn = oBusiness.GetBranchBaseCurrency(vSourceID:=g_iSourceID, iCurrencyID:=m_iCurrencyID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Base Currency", vApp:=ACApp, vClass:=ACClass, vMethod:="SetBaseCurrency", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Exit Function
                End If

                For counter As Integer = 0 To CType(tdgTransactions.Columns(TDBGridCol_Currency), DataGridViewComboBoxColumn).Items.Count - 1
                    oCurrency = CType(CType(tdgTransactions.Columns(TDBGridCol_Currency), DataGridViewComboBoxColumn).Items(counter), VB6.ListBoxItem)
                    If oCurrency.ItemData = m_iCurrencyID Then

                        tdgTransactions.Rows(rowindx).Cells(TDBGridCol_Currency_id).Value = oCurrency.ItemData
                        tdgTransactions.Rows(rowindx).Cells(TDBGridCol_Currency).Value = CType(tdgTransactions.Rows(tdgTransactions.Rows.Count - 1).Cells(TDBGridCol_Currency), DataGridViewComboBoxCell).Items(counter)

                        Exit For
                    End If


                Next



            End If

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get base currency", vApp:=ACApp, vClass:=ACClass, vMethod:="SetBaseCurrency", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Function

        End Try
    End Function
    Private Sub frmInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
        MemoryHelper.ReleaseMemory()
    End Sub

    Private Sub tdgTransactions_CellClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles tdgTransactions.CellClick

        If TDBGridCol_Accoun_Btn = e.ColumnIndex Then
            cmdAccountLookup(e.RowIndex)
        End If

    End Sub

    Private Sub tdgTransactions_CellLeave(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles tdgTransactions.CellLeave
        m_nCount = 0
        If Not tdgTransactions.CurrentCell Is Nothing Then
            If Not tdgTransactions.CurrentCell.Equals(tdgTransactions.Rows(e.RowIndex).Cells(e.ColumnIndex)) Then
                Exit Sub
            End If
        End If
        If tdgTransactions.CurrentColumnIndex = TDBGridCol_Account Or tdgTransactions.CurrentColumnIndex = TDBGridCol_Amount Or tdgTransactions.CurrentColumnIndex = TDBGridCol_Currency_Rate Or tdgTransactions.CurrentColumnIndex = TDBGridCol_AltRef Or tdgTransactions.CurrentColumnIndex = TDBGridCol_Insurance_Ref Or tdgTransactions.CurrentColumnIndex = TDBGridCol_Purchase_Invoice Or tdgTransactions.CurrentColumnIndex = TDBGridCol_Purchase_Order Or tdgTransactions.CurrentColumnIndex = TDBGridCol_Comment Then
            RemoveHandler tdgTransactions.CellLeave, AddressOf tdgTransactions_CellLeave
            If Not tdgTransactions.CurrentCell.EditedFormattedValue = "" Then
                If tdgTransactions.CurrentColumnIndex = TDBGridCol_Amount Then
                    tdgTransactions.CurrentCell.Value = (String.Format("{0:f2}", CType((tdgTransactions.CurrentCell.EditedFormattedValue.ToString()), String)))

                ElseIf tdgTransactions.CurrentColumnIndex = TDBGridCol_Currency_Rate Then
                    tdgTransactions.CurrentCell.Value = String.Format("{0:f10}", CType(tdgTransactions.CurrentCell.EditedFormattedValue.ToString(), String))
                Else
                    tdgTransactions.CurrentCell.Value = tdgTransactions.CurrentCell.EditedFormattedValue
                End If
            Else
                If tdgTransactions.CurrentColumnIndex = TDBGridCol_Account Or tdgTransactions.CurrentColumnIndex = TDBGridCol_AltRef Or tdgTransactions.CurrentColumnIndex = TDBGridCol_Comment Or tdgTransactions.CurrentColumnIndex = TDBGridCol_Insurance_Ref Or tdgTransactions.CurrentColumnIndex = TDBGridCol_Purchase_Order Or tdgTransactions.CurrentColumnIndex = TDBGridCol_Department Or tdgTransactions.CurrentColumnIndex = TDBGridCol_Purchase_Invoice Then
                    tdgTransactions.CurrentCell.Value = tdgTransactions.CurrentCell.FormattedValue
                End If
            End If
            AddHandler tdgTransactions.CellLeave, AddressOf tdgTransactions_CellLeave

            'CalculateBaseAmount(e.RowIndex)

            'calculate Total Balance
            ' CalcTotalBalance()
        End If

    End Sub



    Private Sub tdgTransactions_CellValueChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles tdgTransactions.CellValueChanged
        If Not tdgTransactions.CurrentCell Is Nothing Then
            If Not tdgTransactions.CurrentCell.Equals(tdgTransactions.Rows(e.RowIndex).Cells(e.ColumnIndex)) Then
                Exit Sub
            End If
        End If
        Dim ColIndex As Integer = e.ColumnIndex
        Dim lRow As Integer = e.RowIndex
        Dim oCurrency As VB6.ListBoxItem
        If e.RowIndex < 0 Then
            Exit Sub
        End If
        With tdgTransactions




            'Get CurrencyId

            If ColIndex = TDBGridCol_Currency Then



                For counter As Integer = 0 To CType(.Columns(TDBGridCol_Currency), DataGridViewComboBoxColumn).Items.Count - 1
                    oCurrency = CType(CType(.Columns(TDBGridCol_Currency), DataGridViewComboBoxColumn).Items(counter), VB6.ListBoxItem)
                    If CType(.Rows(lRow).Cells(TDBGridCol_Currency), DataGridViewComboBoxCell).Items(counter).itemString.Equals(.Rows(lRow).Cells(TDBGridCol_Currency).Value) Then
                        .Rows(lRow).Cells(TDBGridCol_Currency_id).Value = oCurrency.ItemData
                        .Rows(lRow).Cells(TDBGridCol_Currency_Rate).Value = ""

                        tdgTransactions.Rows(lRow).Cells(TDBGridCol_Currency).Value = CType(tdgTransactions.Rows(lRow).Cells(TDBGridCol_Currency), DataGridViewComboBoxCell).Items(counter)

                        'calculate base amount
                        CalculateBaseAmount(e.RowIndex)

                        'calculate Total Balance
                        CalcTotalBalance()
                        Exit For
                    End If

                Next

            End If




            ' Get Underwriting_YearId

            If ColIndex = TDBGridCol_UnderwritingYear Then
                For counter As Integer = 0 To CType(.Columns(TDBGridCol_UnderwritingYear), DataGridViewComboBoxColumn).Items.Count - 1
                    oCurrency = CType(CType(tdgTransactions.Columns(TDBGridCol_UnderwritingYear), DataGridViewComboBoxColumn).Items(counter), VB6.ListBoxItem)
                    If CType(.Rows(lRow).Cells(TDBGridCol_UnderwritingYear), DataGridViewComboBoxCell).Items(counter).itemString.Equals(.Rows(lRow).Cells(TDBGridCol_UnderwritingYear).Value) Then
                        .Rows(lRow).Cells(TDBGridCol_UnderwritingYear_id).Value = oCurrency.ItemData
                        tdgTransactions.Rows(lRow).Cells(TDBGridCol_UnderwritingYear).Value = CType(tdgTransactions.Rows(lRow).Cells(TDBGridCol_UnderwritingYear), DataGridViewComboBoxCell).Items(counter)
                        Exit For
                    End If


                Next

            End If





            'Get DepartmentId

            If ColIndex = TDBGridCol_Department Then
                For counter As Integer = 0 To CType(.Columns(TDBGridCol_Department), DataGridViewComboBoxColumn).Items.Count - 1
                    oCurrency = CType(CType(.Columns(TDBGridCol_Department), DataGridViewComboBoxColumn).Items(counter), VB6.ListBoxItem)
                    If CType(.Rows(lRow).Cells(TDBGridCol_Department), DataGridViewComboBoxCell).Items(counter).itemString.Equals(.Rows(lRow).Cells(TDBGridCol_Department).Value) Then
                        .Rows(lRow).Cells(TDBGridCol_Department_id).Value = oCurrency.ItemData
                        tdgTransactions.Rows(lRow).Cells(TDBGridCol_Department).Value = CType(tdgTransactions.Rows(lRow).Cells(TDBGridCol_Department), DataGridViewComboBoxCell).Items(counter)
                        Exit For
                    End If


                Next

            End If


            'PN30298 - Datasure
            .Enabled = False

            'developer guide no. 53 (No Solution)
            .Enabled = True
            .Focus()
        End With
    End Sub




    Private Sub _txtOccursPer_0_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _txtOccursPer_0.ValueChanged

        optOccurs(2).Checked = False
        optOccurs(1).Checked = False
        optOccurs(0).Checked = True

    End Sub

    Private Sub _txtOccursPer_1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _txtOccursPer_1.ValueChanged
        optOccurs(2).Checked = False
        optOccurs(0).Checked = False
        optOccurs(1).Checked = True

    End Sub

    Private Sub _txtOccursPer_2_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _txtOccursPer_2.ValueChanged
        optOccurs(0).Checked = False
        optOccurs(1).Checked = False
        optOccurs(2).Checked = True
    End Sub
    'Private Sub tdgTransactions_KeyDown(sender As Object, e As KeyEventArgs) Handles tdgTransactions.KeyDown
    '    If tdgTransactions.CurrentCell.ColumnIndex = TDBGridCol_Account Then
    '        tdgTransactions.CurrentCell = tdgTransactions.CurrentRow.Cells(TDBGridCol_Currency)
    '    ElseIf tdgTransactions.CurrentCell.ColumnIndex = TDBGridCol_Base_Amount Then
    '        tdgTransactions.CurrentCell = tdgTransactions.CurrentRow.Cells(TDBGridCol_AltRef)
    '        e.Handled = True
    '    End If

    '    If e.KeyCode = Keys.Enter AndAlso tdgTransactions.CurrentCell.ColumnIndex <> TDBGridCol_Purchase_Invoice AndAlso tdgTransactions.CurrentCell.ColumnIndex <> TDBGridCol_Purchase_Invoice AndAlso tdgTransactions.CurrentCell.ColumnIndex <> TDBGridCol_AltRef Then
    '        SendKeys.Send("{tab}")
    '    End If
    'End Sub


    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, ByVal keyData As Keys) As Boolean

        If tdgTransactions.RowCount > 0 AndAlso tdgTransactions.ContainsFocus AndAlso keyData = Keys.Enter Then
            SendKeys.Send("{tab}")
            Return True
        End If

        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function
    Private Sub tdgTransactions_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles tdgTransactions.CellValidating

        If tdgTransactions.CurrentColumnIndex = TDBGridCol_Amount Or tdgTransactions.CurrentColumnIndex = TDBGridCol_Currency_Rate Then
            m_nCount = m_nCount + 1
            If ValidateGridColoum(e.RowIndex, e.ColumnIndex) <> True Then
                e.Cancel = True
            End If
        End If
    End Sub
End Class
