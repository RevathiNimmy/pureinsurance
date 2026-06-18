Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 07/05/1999
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a SIREvent.
    '
    ' Edit History:
    ' SJP14062002 - getUnderWritingOrAgency uses new product options scheme and gSirLibrary.bas
    ' RAM20040224 - Bug fix for PN Issue 10592
    ' CJB20050812 - PN23134 Changed SearchWarnings to have left outer join to event_log_subject table
    '               as there may not always be subjects for notes!
    ' ***************************************************************** '





    ' ************************************************
    ' Added to replace global variables 12/01/2004
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' Collection of SIREvents (Private)
    Private m_oSIREvents As bSIREvent.SIREvents

    ' Database Class (Private)
    Public m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private lPMAuthorityLevel As Integer

    ' Primary Keys to work with
    Private m_lEventCnt As Integer

    Private m_sUnderwritingOrAgency As String = ""

    ' PM Lookup Business Component (Private)
    Private m_oLookup As bPMLookup.Business
    ' PRIVATE Data Members (End)


    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)

            Value = Value

        End Set
    End Property

    Public Property CurrentRecord() As Integer
        Get

            Return m_lCurrentRecord

        End Get
        Set(ByVal Value As Integer)

            Select Case Value
                Case Is < 1
                    m_lCurrentRecord = 0
                Case Is > m_oSIREvents.Count()
                    m_lCurrentRecord = m_oSIREvents.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Number in Collection
            Return m_oSIREvents.Count()

        End Get
    End Property


    Public Property EventCnt() As Integer
        Get

            Return m_lEventCnt

        End Get
        Set(ByVal Value As Integer)

            m_lEventCnt = Value

        End Set
    End Property

    'MSS210901 - Added from merge
    Public ReadOnly Property UnderwritingOrAgency() As String
        Get

            If m_sUnderwritingOrAgency = "" Then
                m_lReturn = getUnderwritingOrAgency()
            End If

            Return m_sUnderwritingOrAgency

        End Get
    End Property
    'MSS210901 - Merge end

    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel




            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create SIREvents Collection
            m_oSIREvents = New bSIREvent.SIREvents()

            ' Create PM Lookup Business Object
            m_oLookup = New BPMLOOKUP.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
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
                If m_oLookup IsNot Nothing Then
                    m_oLookup.Dispose()
                End If
                m_oLookup = Nothing
                m_oSIREvents = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Informations.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Informations.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Informations.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Informations.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Informations.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values for a SIREvent.
    '
    '
    ' ***************************************************************** '
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray As Object, ByRef iLanguageID As Integer, ByRef vResultArray As Object) As Integer

        'Dim oSIREvent As bSIREvent.SIREvent
        'Dim dtEffectiveDate As Date
        '
        '' {* USER DEFINED CODE (Begin) *}
        'Dim vTabArray(3, 1) As Variant
        'Dim vRepeatTypeID As Variant
        'Dim vEventTypeID As Variant
        '' {* USER DEFINED CODE (End) *}
        '
        '    On Error GoTo Err_GetLookupValues
        '
        '    GetLookupValues = PMTrue
        '
        '    ' Reset Result Array
        '    vResultArray = ""
        '    ' Reset Table Array
        '    vTableArray = ""
        '
        '    ' {* USER DEFINED CODE (Begin) *}
        '
        '    ' Setup Lookup Table Names
        '    vTabArray(PMLookupTableName, 0) = PMLookupEventRepeatType
        '    vTabArray(PMLookupTableName, 1) = PMLookupEventType
        '
        '    ' {* USER DEFINED CODE (End) *}
        '
        '    ' Do we have any records
        '    If (m_lCurrentRecord& < 1) Then
        '        ' No, we can only lookup all
        '        iLookupType = PMLookupAll
        '    Else
        '        ' Yes get current record
        '        Set oSIREvent = m_oSIREvents.Item(m_lCurrentRecord&)
        '    End If
        '
        '    Select Case iLookupType%
        '      Case PMLookupAll
        '
        '        ' Do not supply a key
        '        vTabArray(PMLookupKey, 0) = ""
        '        vTabArray(PMLookupKey, 1) = ""
        '
        '      Case PMLookupAllEffective
        '
        '        ' Use keys and effective date from current record
        '        ' Note: The keys are not used for the select, but are used by
        '        '       the iterface program to set the list index.
        '        With oSIREvent
        '
        '            ' {* USER DEFINED CODE (Begin) *}
        '            m_lReturn = .GetProperties(iStatus:=PMView)
        '
        '            vTabArray(PMLookupKey, 0) = vRepeatTypeID
        '            vTabArray(PMLookupKey, 1) = vEventTypeID
        '            ' {* USER DEFINED CODE (End) *}
        '
        '        End With
        '
        '      Case PMLookupSingle
        '
        '        ' Set keys from current record
        '        With oSIREvent
        '
        '            ' {* USER DEFINED CODE (Begin) *}
        '            m_lReturn = .GetProperties(iStatus:=PMView)
        '
        '            vTabArray(PMLookupKey, 0) = vRepeatTypeID
        '            vTabArray(PMLookupKey, 1) = vEventTypeID
        '            ' {* USER DEFINED CODE (End) *}
        '
        '        End With
        '
        '    End Select
        '
        '    ' Default Effective Date to current date/time
        '    dtEffectiveDate = Now
        '
        '    ' Release SIREvent reference
        '    Set oSIREvent = Nothing
        '
        '    ' Get the Lookup items
        '    m_lReturn = m_oLookup.GetLookupValues( _
        ''        iLookupType:=iLookupType, _
        ''        vTableArray:=vTabArray, _
        ''        iLanguageID:=iLanguageID, _
        ''        dtEffectiveDate:=dtEffectiveDate, _
        ''        vResultArray:=vResultArray)
        '
        '    If (m_lReturn <> PMTrue) Then
        '        GetLookupValues = PMFalse
        '        Exit Function
        '    End If
        '
        '    ' Return the Table Array
        '    vTableArray = vTabArray


        Dim result As Integer = 0
        Return result




        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: GetEventLogSubjectList
    '
    ' Description:
    '
    ' History: 30/09/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function GetEventLogSubjectList(ByRef r_vResultArray As Object) As Integer
        Return GetEventLogSubjectList(r_vResultArray:=r_vResultArray, v_vEventLogSubjectId:=Nothing)
    End Function


    Public Function GetEventLogSubjectList(ByRef r_vResultArray As Object, ByVal v_vEventLogSubjectId As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vTabArray(3, 0) As Object
            Dim vResultArray(,) As Object = Nothing
            Dim iLookupType As gPMConstants.PMELookupType


            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = "event_log_subject"


            If Not Informations.IsNothing(v_vEventLogSubjectId) Then
                iLookupType = gPMConstants.PMELookupType.PMLookupSingle


                vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = v_vEventLogSubjectId
            Else
                iLookupType = gPMConstants.PMELookupType.PMLookupAll

                vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""
            End If

            ' Get the Lookup items
            m_lReturn = m_oLookup.GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTabArray, iLanguageID:=m_iLanguageID, dtEffectiveDate:=DateTime.Now.AddDays(1), vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the Table Array


            r_vResultArray = vResultArray

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetEventLogSubjectList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetEventLogSubjectList", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single SIREvent directly into the database.
    '        Note: The SIREvent will NOT be added to the collection.
    ' sj  30/09/2002 - Add Event Log Subject Id , Event Type Code and Account Key
    ' CJB 07/04/2004 - Add ShortDescription
    ' 2005 Sticky Notes - Add priority_code and completion indicator
    ' ***************************************************************** '
    Public Function DirectAdd(Optional ByRef vEventCnt As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vInsuranceFolderCnt As Object = Nothing, Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vClaimCnt As Object = Nothing, Optional ByRef vDocumentCnt As Object = Nothing, Optional ByRef vOldAddressCnt As Object = Nothing, Optional ByRef vNewAddressCnt As Object = Nothing, Optional ByRef vCampaignId As Object = Nothing, Optional ByRef vDocumentType As Object = Nothing, Optional ByRef vReportType As Object = Nothing, Optional ByRef vEventType As Object = Nothing, Optional ByRef vUserId As Object = Nothing, Optional ByRef vEventDate As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vOldPartyTypeID As Object = Nothing, Optional ByRef vEventLogSubjectId As Object = Nothing, Optional ByRef vEventTypeCode As Object = Nothing, Optional ByRef vAccountKey As Object = Nothing, Optional ByRef vFSAComplaintFolderCnt As Object = Nothing, Optional ByRef vShortDescription As Object = Nothing, Optional ByRef vPriorityCode As Object = Nothing, Optional ByRef vIsCompleted As Object = Nothing, Optional ByRef vPerilId As Object = Nothing, Optional ByVal vCaseID As Object = Nothing, Optional ByVal v_vRTFText As Object = Nothing, Optional ByRef v_vIsManualDescription As Object = Nothing, Optional ByRef vBaseClaimID As Object = Nothing, Optional ByRef vBatchID As Object = Nothing, Optional ByRef vDocument_Path As String = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIREvent As bSIREvent.SIREvent
        Dim dtDateTime As Date

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new SIREvent
            oSIREvent = New bSIREvent.SIREvent()
            m_lReturn = CType(oSIREvent.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)


            'We're passing in a date, from the PC.  Really we want the date (and time) on the server.
            dtDateTime = DateTime.Now

            ' Populate SIREvent Attributes
            '2005 StickyNotes New Properties - Priority Code 7 Completion Indicator




            'developer guide no. 98
            m_lReturn = CType(oSIREvent.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vEventCnt:=vEventCnt, vPartyCnt:=vPartyCnt, vInsuranceFolderCnt:=vInsuranceFolderCnt, vInsuranceFileCnt:=vInsuranceFileCnt, vClaimCnt:=vClaimCnt, vDocumentCnt:=vDocumentCnt, vOldAddressCnt:=vOldAddressCnt, vNewAddressCnt:=vNewAddressCnt, vCampaignId:=vCampaignId, vDocumentType:=vDocumentType, vReportType:=vReportType, vEventType:=vEventType, vUserId:=vUserId, vEventDate:=dtDateTime, vDescription:=vDescription, vOldPartyTypeID:=vOldPartyTypeID, vEventLogSubjectId:=vEventLogSubjectId, vEventTypeCode:=vEventTypeCode, vAccountKey:=vAccountKey, vFSAComplaintFolderCnt:=vFSAComplaintFolderCnt, vShortDescription:=vShortDescription, vPriorityCode:=vPriorityCode, vIsCompleted:=vIsCompleted, vPerilId:=vPerilId, vCaseID:=vCaseID, v_vRTFText:=v_vRTFText, v_vIsManualDescription:=v_vIsManualDescription, vBatchID:=vBatchID, vDocument_Path:=vDocument_Path), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIREvent = Nothing
                Return result
            End If

            ' Add the SIREvent to the Database
            m_lReturn = CType(oSIREvent.AddItem(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIREvent = Nothing
                Return result
            End If

            ' Retain the Primary Key of the SIREvent Added
            With oSIREvent
                EventCnt = .EventCnt
            End With

            vEventCnt = EventCnt

            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}
            oSIREvent.Dispose()
            oSIREvent = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectDelete (Public)
    '
    ' Description: Deletes a single SIREvent directly from the database.
    '        Note: The SIREvent will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    Public Function DirectDelete() As Integer
        Return DirectDelete(vEventCnt:=0)
    End Function

    Public Function DirectDelete(ByRef vEventCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim oSIREvent As bSIREvent.SIREvent

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new SIREvent
            oSIREvent = New bSIREvent.SIREvent()
            m_lReturn = CType(oSIREvent.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            'eck240800
            oSIREvent.EventCnt = vEventCnt
            ' Set SIREvent Primary Key
            m_lReturn = CType(oSIREvent.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMDelete, vEventCnt:=vEventCnt), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIREvent = Nothing
                Return result
            End If

            ' Delete the SIREvent from the Database
            m_lReturn = CType(oSIREvent.DeleteItem(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIREvent = Nothing
                Return result
            End If

            oSIREvent = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectUpdate (Public)
    '
    ' Description: Updates a single SIREvent directly into the database.
    '        Note: The SIREvent will NOT be updated in the collection.
    ' 2005 StickyNotes Pass back IsCompleted
    ' ***************************************************************** '
    Public Function DirectUpdate(Optional ByRef vEventCnt As Object = Nothing,
                                 Optional ByRef vPartyCnt As Object = Nothing,
                                 Optional ByRef vInsuranceFolderCnt As Object = Nothing,
                                 Optional ByRef vInsuranceFileCnt As Object = Nothing,
                                 Optional ByRef vClaimCnt As Object = Nothing,
                                 Optional ByRef vDocumentCnt As Object = Nothing,
                                 Optional ByRef vOldAddressCnt As Object = Nothing,
                                 Optional ByRef vNewAddressCnt As Object = Nothing,
                                 Optional ByRef vCampaignId As Object = Nothing,
                                 Optional ByRef vDocumentType As Object = Nothing,
                                 Optional ByRef vReportType As Object = Nothing,
                                 Optional ByRef vEventType As Object = Nothing,
                                 Optional ByRef vUserId As Object = Nothing,
                                 Optional ByRef vEventDate As Object = Nothing,
                                 Optional ByRef vDescription As Object = Nothing,
                                 Optional ByRef vOldPartyTypeID As Object = Nothing,
                                 Optional ByRef vIsCompleted As Object = Nothing,
                                 Optional ByRef vPerilId As Object = Nothing,
                                 Optional ByRef vBatchID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIREvent As bSIREvent.SIREvent
        Dim dtDateTime As Date

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new SIREvent
            oSIREvent = New bSIREvent.SIREvent()
            m_lReturn = CType(oSIREvent.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            'We're passing in a date, from the PC.  Really we want the date (and time) on the server.
            dtDateTime = DateTime.Now

            ' Populate SIREvent Attributes




            'developer guide no. 98
            m_lReturn = CType(oSIREvent.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMEdit, vEventCnt:=vEventCnt, vPartyCnt:=vPartyCnt, vInsuranceFolderCnt:=vInsuranceFolderCnt, vInsuranceFileCnt:=vInsuranceFileCnt, vClaimCnt:=vClaimCnt, vDocumentCnt:=vDocumentCnt, vOldAddressCnt:=vOldAddressCnt, vNewAddressCnt:=vNewAddressCnt, vCampaignId:=vCampaignId, vDocumentType:=vDocumentType, vReportType:=vReportType, vEventType:=vEventType, vUserId:=vUserId, vEventDate:=dtDateTime, vDescription:=vDescription, vOldPartyTypeID:=vOldPartyTypeID, vPerilId:=vPerilId, vBatchID:=vBatchID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIREvent = Nothing
                Return result
            End If

            ' Add the SIREvent to the Database
            m_lReturn = CType(oSIREvent.UpdateItem(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIREvent = Nothing
                Return result
            End If

            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}
            oSIREvent.Dispose()
            oSIREvent = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectUpdate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckID (Public)
    '
    ' Description: Checks to see if the supplied ID is a valid record.
    '
    ' ***************************************************************** '
    Public Function CheckID(ByRef vID As Object) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the ID parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="id", vValue:=vID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckIDSQL, sSQLName:=ACCheckIDName, bStoredProcedure:=ACCheckIDStored, lNumberRecords:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?
            If lRecordCount < 1 Then
                ' No record found
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required SIREvents and populate the Collection
    '
    ' ***************************************************************** '
    'FSA Phase III Pass ComplaintCnt
    Public Function GetDetails(Optional ByRef vLockMode As Integer = 0, Optional ByRef vEventCnt As Integer = 0, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vInsuranceFolderCnt As Object = Nothing, Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vClaimCnt As Object = Nothing, Optional ByRef vFSAComplaintFolderCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        'developer guide no 21. 
        Dim oFields As DataRow
        Dim oSIREvent As bSIREvent.SIREvent
        Dim sSQL As String = ""
        Dim bDone As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oSIREvents.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Default to No Lock if not supplied or not numeric
            Dim dbNumericTemp As Double

            If (Informations.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                vLockMode = gPMConstants.PMELockMode.PMNoLock
            End If

            ' Check for Valid Primary Key
            Dim dbNumericTemp2 As Double

            If (Not Informations.IsNothing(vEventCnt)) And (Not Double.TryParse(CStr(vEventCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vEventCnt=" & vEventCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                Return result
            End If

            ' Check for Valid parameters

            Dim dbNumericTemp3 As Double

            If (Not Informations.IsNothing(vPartyCnt)) And (Not Double.TryParse(CStr(vPartyCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vPartyCnt=" & CStr(vPartyCnt), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                Return result
            End If


            Dim dbNumericTemp4 As Double

            If (Not Informations.IsNothing(vInsuranceFolderCnt)) And (Not Double.TryParse(CStr(vInsuranceFolderCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vInsuranceFolderCnt=" & CStr(vInsuranceFolderCnt), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                Return result
            End If


            Dim dbNumericTemp5 As Double

            If (Not Informations.IsNothing(vInsuranceFileCnt)) And (Not Double.TryParse(CStr(vInsuranceFileCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vInsuranceFileCnt=" & CStr(vInsuranceFileCnt), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                Return result
            End If


            Dim dbNumericTemp6 As Double

            If (Not Informations.IsNothing(vClaimCnt)) And (Not Double.TryParse(CStr(vClaimCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp6)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vClaimCnt=" & CStr(vClaimCnt), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                Return result
            End If


            Dim dbNumericTemp7 As Double

            If (Not Informations.IsNothing(vFSAComplaintFolderCnt)) And (Not Double.TryParse(CStr(vFSAComplaintFolderCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp7)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vFSAComplaintFolderCnt=" & CStr(vFSAComplaintFolderCnt), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                Return result
            End If



            If Not Informations.IsNothing(vEventCnt) Then

                ' Create New SIREvent
                oSIREvent = New bSIREvent.SIREvent()
                m_lReturn = CType(oSIREvent.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)


                ' Set component primary keys
                With oSIREvent
                    .EventCnt = vEventCnt

                    m_lReturn = CType(.SelectItem(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End With

                ' Add SIREvent to collection
                If (m_oSIREvents.Count = 0) Then
                    m_oSIREvents.Add(Nothing)
                End If
                m_lReturn = CType(m_oSIREvents.Add(oNewSIREvent:=oSIREvent), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                oSIREvent = Nothing

            Else

                ' No Key, Get All Records for the parameters passed
                sSQL = "SELECT event_cnt FROM event"
                bDone = False


                If Not Informations.IsNothing(vPartyCnt) Then
                    If bDone Then
                        sSQL = sSQL & " AND "
                    Else
                        sSQL = sSQL & " WHERE "
                        bDone = True
                    End If


                    sSQL = sSQL & "party_cnt = " & CStr(CInt(vPartyCnt))
                End If


                If Not Informations.IsNothing(vInsuranceFolderCnt) Then
                    If bDone Then
                        sSQL = sSQL & " AND "
                    Else
                        sSQL = sSQL & " WHERE "
                        bDone = True
                    End If


                    sSQL = sSQL & "insurance_folder_cnt = " & CStr(CInt(vInsuranceFolderCnt))
                End If



                If Not Informations.IsNothing(vClaimCnt) Then
                    If bDone Then
                        sSQL = sSQL & " AND "
                    Else
                        sSQL = sSQL & " WHERE "
                        bDone = True
                    End If


                    sSQL = sSQL & "claim_cnt = " & CStr(CInt(vClaimCnt))
                End If
                'FSA Phase III

                If Not Informations.IsNothing(vFSAComplaintFolderCnt) Then
                    If bDone Then
                        sSQL = sSQL & " AND "
                    Else
                        sSQL = sSQL & " WHERE "
                        bDone = True
                    End If

                    sSQL = sSQL & "fsa_complaint_folder_cnt = " & CStr(CInt(vFSAComplaintFolderCnt))
                End If

                sSQL = sSQL & " ORDER BY event_date DESC"

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' How many records were selected
                lRecordCount = m_oDatabase.Records.Count()

                ' Do we have any records ?
                If lRecordCount < 1 Then
                    ' No Records, return PMFalse
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If

                ' Yes, load them into the collection
                For lSub As Integer = 1 To lRecordCount

                    ' Create New
                    oSIREvent = New bSIREvent.SIREvent()
                    m_lReturn = CType(oSIREvent.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName), gPMConstants.PMEReturnCode)

                    ' Set oFields to refer to one Record
                    oFields = m_oDatabase.Records.Item(lSub - 1).Fields()

                    ' Set component primary keys from current record
                    With oSIREvent
                        .EventCnt = gPMFunctions.NullToLong(oFields("event_cnt"))

                        m_lReturn = CType(.SelectItem(), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End With

                    ' Add SIREvent to collection
                    If (m_oSIREvents.Count = 0) Then
                        m_oSIREvents.Add(Nothing)
                    End If
                    m_lReturn = CType(m_oSIREvents.Add(oNewSIREvent:=oSIREvent), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    oSIREvent.Dispose()
                    oSIREvent = Nothing
                Next lSub
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the required SIREvents and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vEventCnt As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vInsuranceFolderCnt As Object = Nothing, Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vClaimCnt As Object = Nothing, Optional ByRef vDocumentCnt As Object = Nothing, Optional ByRef vOldAddressCnt As Object = Nothing, Optional ByRef vNewAddressCnt As Object = Nothing, Optional ByRef vCampaignId As Object = Nothing, Optional ByRef vDocumentType As Object = Nothing, Optional ByRef vReportType As Object = Nothing, Optional ByRef vEventType As Object = Nothing, Optional ByRef vUserId As Object = Nothing, Optional ByRef vEventDate As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vOldPartyTypeID As Object = Nothing, Optional ByRef vPerilId As Object = Nothing, Optional ByRef vBatchID As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oSIREvent As bSIREvent.SIREvent
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oSIREvents.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oSIREvent = m_oSIREvents.Item(m_lCurrentRecord)

            ' Get the SIREvent Property Values
            m_lReturn = CType(oSIREvent.GetProperties(iStatus, vEventCnt:=vEventCnt, vPartyCnt:=vPartyCnt, vInsuranceFolderCnt:=vInsuranceFolderCnt, vInsuranceFileCnt:=vInsuranceFileCnt, vClaimCnt:=vClaimCnt, vDocumentCnt:=vDocumentCnt, vOldAddressCnt:=vOldAddressCnt, vNewAddressCnt:=vNewAddressCnt, vCampaignId:=vCampaignId, vDocumentType:=vDocumentType, vReportType:=vReportType, vEventType:=vEventType, vUserId:=vUserId, vEventDate:=vEventDate, vDescription:=vDescription, vOldPartyTypeID:=vOldPartyTypeID, vPerilId:=vPerilId, vBatchID:=vBatchID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
            oSIREvent.Dispose()
            oSIREvent = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied SIREvent into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vEventCnt As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vInsuranceFolderCnt As Object = Nothing, Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vClaimCnt As Object = Nothing, Optional ByRef vDocumentCnt As Object = Nothing, Optional ByRef vOldAddressCnt As Object = Nothing, Optional ByRef vNewAddressCnt As Object = Nothing, Optional ByRef vCampaignId As Object = Nothing, Optional ByRef vDocumentType As Object = Nothing, Optional ByRef vReportType As Object = Nothing, Optional ByRef vEventType As Object = Nothing, Optional ByRef vUserId As Object = Nothing, Optional ByRef vEventDate As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vOldPartyTypeID As Object = Nothing, Optional ByRef vPerilId As Object = Nothing, Optional ByRef vBatchID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIREvent As bSIREvent.SIREvent

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oSIREvents.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new SIREvent
            oSIREvent = New bSIREvent.SIREvent()
            m_lReturn = CType(oSIREvent.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            ' Populate SIREvent Attributes

            m_lReturn = CType(oSIREvent.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vEventCnt:=vEventCnt, vPartyCnt:=vPartyCnt, vInsuranceFolderCnt:=vInsuranceFolderCnt, vInsuranceFileCnt:=vInsuranceFileCnt, vClaimCnt:=vClaimCnt, vDocumentCnt:=vDocumentCnt, vOldAddressCnt:=vOldAddressCnt, vNewAddressCnt:=vNewAddressCnt, vCampaignId:=vCampaignId, vDocumentType:=vDocumentType, vReportType:=vReportType, vEventType:=vEventType, vUserId:=vUserId, vEventDate:=vEventDate, vDescription:=vDescription, vOldPartyTypeID:=vOldPartyTypeID, vPerilId:=vPerilId, vBatchID:=vBatchID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oSIREvent = Nothing
                Return result
            End If

            ' Add SIREvent to collection
            If (m_oSIREvents.Count = 0) Then
                m_oSIREvents.Add(Nothing)
            End If
            m_lReturn = CType(m_oSIREvents.Add(oNewSIREvent:=oSIREvent), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIREvent = Nothing
                Return result
            End If
            oSIREvent.Dispose()
            oSIREvent = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the SIREvent
    '              specified and updates the SIREvent with the new values.
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vEventCnt As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vInsuranceFolderCnt As Object = Nothing, Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vClaimCnt As Object = Nothing, Optional ByRef vDocumentCnt As Object = Nothing, Optional ByRef vOldAddressCnt As Object = Nothing, Optional ByRef vNewAddressCnt As Object = Nothing, Optional ByRef vCampaignId As Object = Nothing, Optional ByRef vDocumentType As Object = Nothing, Optional ByRef vReportType As Object = Nothing, Optional ByRef vEventType As Object = Nothing, Optional ByRef vUserId As Object = Nothing, Optional ByRef vEventDate As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vOldPartyTypeID As Object = Nothing, Optional ByRef vPerilId As Object = Nothing, Optional ByRef vBatchID As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oSIREvent As bSIREvent.SIREvent
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oSIREvents.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oSIREvent = m_oSIREvents.Item(lRow)

            ' Check the Status of the SIREvent

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oSIREvent.DatabaseStatus
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Leave Status as Add
                    iStatus = gPMConstants.PMEComponentAction.PMAdd
                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                    ' Error
                    result = gPMConstants.PMEReturnCode.PMFalse
                Case Else
                    ' Set Edit (Update) Status
                    iStatus = gPMConstants.PMEComponentAction.PMEdit
            End Select

            ' Update SIREvent Attributes

            m_lReturn = CType(oSIREvent.SetProperties(iStatus:=iStatus, vEventCnt:=vEventCnt, vPartyCnt:=vPartyCnt, vInsuranceFolderCnt:=vInsuranceFolderCnt, vInsuranceFileCnt:=vInsuranceFileCnt, vClaimCnt:=vClaimCnt, vDocumentCnt:=vDocumentCnt, vOldAddressCnt:=vOldAddressCnt, vNewAddressCnt:=vNewAddressCnt, vCampaignId:=vCampaignId, vDocumentType:=vDocumentType, vReportType:=vReportType, vEventType:=vEventType, vUserId:=vUserId, vEventDate:=vEventDate, vDescription:=vDescription, vOldPartyTypeID:=vOldPartyTypeID, vPerilId:=vPerilId, vBatchID:=vBatchID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oSIREvent = Nothing
                Return result
            End If

            ' Release reference to SIREvent
            oSIREvent = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete (Public)
    '
    ' Description: Validate that the specified SIREvent can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oSIREvent As bSIREvent.SIREvent

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oSIREvents.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oSIREvent = m_oSIREvents.Item(lRow)

            ' Check the Status of the SIREvent

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oSIREvent.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oSIREvent.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oSIREvent.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            ' Release reference to SIREvent
            oSIREvent = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Cancel (Public)
    '
    ' Description: Checks the Collection to see if Cancel is OK.
    '              i.e. Do we need any Adding, Deleting or Updating.
    '
    '              Returns PMTrue if all items are clean
    '                      (PMView or PMDummyDelete)
    '              Otherwise returns PMDataChanged.
    ' ***************************************************************** '
    Public Function Cancel() As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Loop round Collection
            For lSub As Integer = 1 To m_oSIREvents.Count()
                Select Case m_oSIREvents.Item(lSub).DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing
                    Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMDelete
                        result = gPMConstants.PMEReturnCode.PMDataChanged
                        Exit For
                End Select
            Next lSub

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Cancel", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Update (Public)
    '
    ' Description: Loops round the collection, doing any required
    '              Adds, Deletes or Updates.
    '
    ' ***************************************************************** '
    Public Function Update() As Integer

        Dim result As Integer = 0
        Dim lSub As Integer
        Dim oSIREvent As bSIREvent.SIREvent = Nothing
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oSIREvents.Count()
                oSIREvent = m_oSIREvents.Item(lSub)


                Select Case oSIREvent.DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing

                    Case gPMConstants.PMEComponentAction.PMAdd

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Add Item
                        m_lReturn = CType(oSIREvent.AddItem(), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMEdit

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Update Item
                        m_lReturn = CType(oSIREvent.UpdateItem(), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMDelete

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Delete Item
                        m_lReturn = CType(oSIREvent.DeleteItem(), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Retain the Primary Key of the SIREvent
            With oSIREvent
                EventCnt = .EventCnt
            End With

            ' Release last reference
            oSIREvent = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Refresh the Collection Items D/B Status
                    lSub = 1

                    ' For each item in the collection
                    Do While lSub <= m_oSIREvents.Count()

                        ' With the item
                        With m_oSIREvents.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oSIREvents.Delete(lSub)

                                    ' Anything Else
                                Case Else
                                    ' Set Status to view
                                    .DatabaseStatus = gPMConstants.PMEComponentAction.PMView
                                    lSub += 1

                            End Select

                        End With

                    Loop

                Else

                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SearchAll (Public)
    '
    ' Description: Gets everything as an array
    ' Edit History  :
    ' RAM20040224   : Code changes related to PN Issue 10592, to add the
    '                   date filter in too.
    '                 Added the following 2 optional parameters.
    '                   1. v_vFromDate
    '                   2. v_vToDate
    ' ***************************************************************** '
    Public Function SearchAll(ByRef r_vResultArray(,) As Object, Optional ByVal v_vPartyCnt As Object = Nothing,
                          Optional ByVal v_vInsuranceFolderCnt As Object = Nothing, Optional ByVal v_vInsuranceFileCnt As Object = Nothing,
                          Optional ByVal v_vClaimCnt As Object = Nothing, Optional ByVal v_vOldPartyTypeID As Object = Nothing,
                          Optional ByVal v_vFSAComplaintFolderCnt As Object = Nothing, Optional ByRef v_vNotesArray As Object = Nothing,
                          Optional ByRef r_vEventTypeGroupArray As Object = Nothing, Optional ByVal v_vAccountKey As Object = Nothing,
                          Optional ByVal v_vFromDate As Object = Nothing, Optional ByVal v_vToDate As Object = Nothing,
                          Optional ByVal v_vBaseClaimID As Object = Nothing, Optional ByVal v_vCaseID As Object = Nothing,
                          Optional ByVal v_vBaseCaseID As Object = Nothing, Optional ByVal v_vBgID As Object = Nothing,
                          Optional ByVal v_vEventType As Object = Nothing, Optional ByVal v_vUserId As Object = Nothing,
                          Optional ByVal v_vClaimNumber As Object = Nothing, Optional ByVal v_vAgentKey As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim bDone As Boolean

        Dim sFromDate As String = ""
        Dim sToDate As String = ""
        Dim lInsuranceFolderCnt As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Informations.IsNothing(v_vInsuranceFolderCnt) Then
                lInsuranceFolderCnt = v_vInsuranceFolderCnt
            End If

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Check for Valid parameters
            Dim dbNumericTemp As Double

            If (Not Informations.IsNothing(v_vPartyCnt)) And (Not Double.TryParse(CStr(v_vPartyCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : v_vPartyCnt=" & v_vPartyCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAll")

                Return result
            End If

            Dim dbNumericTemp2 As Double

            If (Not Informations.IsNothing(v_vInsuranceFolderCnt)) And (Not Double.TryParse(CStr(v_vInsuranceFolderCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : v_vInsuranceFolderCnt=" & v_vInsuranceFolderCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAll")

                Return result
            End If

            Dim dbNumericTemp3 As Double

            If (Not Informations.IsNothing(v_vInsuranceFileCnt)) And (Not Double.TryParse(CStr(v_vInsuranceFileCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : v_vInsuranceFileCnt=" & v_vInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAll")

                Return result
            End If

            Dim dbNumericTemp4 As Double

            If (Not Informations.IsNothing(v_vClaimCnt)) And (Not Double.TryParse(CStr(v_vClaimCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : v_vClaimCnt=" & v_vClaimCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAll")

                Return result
            End If

            Dim dbNumericTemp5 As Double

            If (Not Informations.IsNothing(v_vOldPartyTypeID)) And (Not Double.TryParse(CStr(v_vOldPartyTypeID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : v_vOldPartyTypeID=" & v_vClaimCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAll")

                Return result
            End If

            'FSAPhase III
            Dim dbNumericTemp6 As Double

            If (Not Informations.IsNothing(v_vFSAComplaintFolderCnt)) And (Not Double.TryParse(CStr(v_vFSAComplaintFolderCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp6)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : v_vFSAComplaintFolderCnt=" & v_vFSAComplaintFolderCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAll")

                Return result
            End If

            If (Not Informations.IsNothing(v_vFromDate)) And (Not Informations.IsDate(v_vFromDate)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not a Valid Date : v_vFromDate=" & CStr(v_vFromDate), vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAll")

                Return result
            End If

            If (Not Informations.IsNothing(v_vToDate)) And (Not Informations.IsDate(v_vToDate)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not a Valid Date : v_vToDate = " & CStr(v_vToDate), vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAll")
                Return result
            End If
            '2005 StickyNotes - return new prioritycode & completion indicator fields

            sSQL = "SELECT  el.event_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "el.insurance_folder_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            If v_vInsuranceFileCnt > 0 Then
                sSQL = sSQL & "(select insurance_ref from insurance_file where insurance_file_cnt= " & v_vInsuranceFileCnt & "), " & Strings.ChrW(13) & Strings.ChrW(10)
            Else
                sSQL = sSQL & "(select insurance_ref from insurance_file where insurance_file_cnt=el.insurance_file_cnt), " & Strings.ChrW(13) & Strings.ChrW(10)
            End If
            sSQL = sSQL & "el.insurance_file_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "fi.insurance_file_structure_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "el.claim_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "clm.claim_number," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "el.document_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "dt.description," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "el.old_address_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "el.new_address_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "c.description," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "null," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "et.code," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "u.username," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "el.event_date," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "el.description," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "el.old_party_type_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "tef.Reason ," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "tef.Document_Ref, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "els.description, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "et.event_type_group_id, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "et.description, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "els.event_log_subject_id, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "fsa_complaint_folder_cnt, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "fi2.alternate_reference, " & Strings.ChrW(13) & Strings.ChrW(10)
            ' CJB 191004 PN 15954 - Get source from party when we aren't listing events by policy

            sSQL = sSQL & "CASE" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHEN (" & CStr(lInsuranceFolderCnt) & "= 0) THEN" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "(SELECT p.source_id FROM party p WHERE p.party_cnt = el.party_cnt)" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "Else" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "(SELECT fi2.source_id)" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "END AS source_id," & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & "fi2.policy_type_id, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "source.underwriting_branch_ind, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "(SELECT MAX('Yes') FROM event_public_text where event_cnt = el.event_cnt), " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "el.priority_code, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "el.is_completed, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "el.rtf_text, " & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & "el.bg_id" & Strings.ChrW(13) & Strings.ChrW(10)
            'sSQL = sSQL & ", cs.Case_Number" & vbCrLf
            sSQL = sSQL & ", (SELECT TOP 1 Case_number from [CASE] C1 WHERE C1.base_case_id = CLM.base_case_id ) Case_number " & vbCrLf
            sSQL = sSQL & ", clm.base_claim_id, el.Document_Path" & vbCrLf
            sSQL = sSQL & "FROM event_log el (NOLOCK)" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "LEFT OUTER JOIN insurance_file fi" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ON el.insurance_file_cnt = fi.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "LEFT OUTER JOIN document_type dt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ON el.document_type_id = dt.document_type_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "LEFT OUTER JOIN campaign c" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ON el.campaign_id = c.campaign_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "JOIN event_type et" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ON el.event_type_id = et.event_type_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "JOIN PMUser u" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ON el.user_id = u.user_id" & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & "LEFT OUTER JOIN claim clm (nolock)" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ON el.claim_cnt = clm.claim_id" & Strings.ChrW(13) & Strings.ChrW(10)

            'sSQL = sSQL & "LEFT OUTER JOIN (SELECT DISTINCT case_number,base_case_id FROM [case] group by case_number,base_case_id) cs" & Strings.Chrw(13) & Strings.Chrw(10)
            'sSQL = sSQL & "ON cs.base_case_id = clm.base_case_id" & Strings.Chrw(13) & Strings.Chrw(10)

            sSQL = sSQL & "LEFT OUTER JOIN transaction_export_folder tef (nolock)" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ON el.transaction_export_folder_cnt = tef.transaction_export_folder_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "LEFT OUTER JOIN event_log_subject els" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ON el.event_log_subject_id = els.event_log_subject_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "LEFT OUTER JOIN event_type_group etg" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ON et.event_type_group_id = etg.event_type_group_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "LEFT OUTER JOIN source " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ON fi.source_id = source.source_id" & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & "LEFT OUTER JOIN insurance_file fi2" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ON fi2.insurance_file_cnt = (SELECT MAX(insurance_file_cnt) FROM insurance_file "
            sSQL = sSQL & "WHERE insurance_folder_cnt = " & CStr(lInsuranceFolderCnt) & ") "

            bDone = False

            If Not Informations.IsNothing(v_vPartyCnt) Then
                If v_vPartyCnt <> 0 Then
                    If bDone Then
                        sSQL = sSQL & "AND "
                    Else
                        sSQL = sSQL & "WHERE "
                        bDone = True
                    End If

                    sSQL = sSQL & "el.party_cnt = " & CStr(v_vPartyCnt) & Strings.ChrW(13) & Strings.ChrW(10)
                End If
            End If

            If Not Informations.IsNothing(v_vInsuranceFolderCnt) Then
                If v_vInsuranceFolderCnt <> 0 Then
                    If bDone Then
                        sSQL = sSQL & "AND "
                    Else
                        sSQL = sSQL & "WHERE "
                        bDone = True
                    End If

                    sSQL = sSQL & "el.insurance_folder_cnt = " & CStr(v_vInsuranceFolderCnt) & Strings.ChrW(13) & Strings.ChrW(10)
                End If
            End If

            If Not Informations.IsNothing(v_vInsuranceFileCnt) Then
                If (v_vInsuranceFileCnt <> 0) Then
                    If bDone Then
                        sSQL = sSQL & "AND "
                    Else
                        sSQL = sSQL & "WHERE "
                        bDone = True
                    End If
                    sSQL = sSQL & "el.insurance_file_cnt =" & CLng(v_vInsuranceFileCnt) & vbCrLf
                End If
            End If


            If Not Informations.IsNothing(v_vClaimCnt) And gPMFunctions.ToSafeLong(v_vBaseClaimID) = 0 Then
                If v_vClaimCnt <> 0 Then
                    If bDone Then
                        sSQL = sSQL & "AND "
                    Else
                        sSQL = sSQL & "WHERE "
                        bDone = True
                    End If
                    sSQL = sSQL & "el.claim_cnt = " & CStr(v_vClaimCnt) & Strings.ChrW(13) & Strings.ChrW(10)
                End If
            End If

            If Not Informations.IsNothing(v_vClaimNumber) Then
                If v_vClaimNumber <> "" Then
                    If bDone Then
                        sSQL = sSQL & "AND "
                    Else
                        sSQL = sSQL & "WHERE "
                        bDone = True
                    End If
                    sSQL = sSQL & "el.claim_cnt in (Select claim_id from claim where claim_number like '" & CStr(v_vClaimNumber) & "')" & Strings.ChrW(13) & Strings.ChrW(10)
                End If
            ElseIf Not Informations.IsNothing(v_vBaseClaimID) Then
                If v_vBaseClaimID <> 0 Then
                    If bDone Then
                        sSQL = sSQL & "AND "
                    Else
                        sSQL = sSQL & "WHERE "
                        bDone = True
                    End If
                    sSQL = sSQL & "el.claim_cnt in (Select claim_id from claim where base_claim_id =  " & CStr(v_vBaseClaimID) & ")" & Strings.ChrW(13) & Strings.ChrW(10)
                End If
            End If

            If Not Informations.IsNothing(v_vOldPartyTypeID) Then
                If v_vOldPartyTypeID <> 0 Then
                    If bDone Then
                        sSQL = sSQL & "AND "
                    Else
                        sSQL = sSQL & "WHERE "
                        bDone = True
                    End If
                    sSQL = sSQL & "el.old_party_type_id = " & CStr(v_vOldPartyTypeID) & Strings.ChrW(13) & Strings.ChrW(10)
                End If
            End If

            If Not Informations.IsNothing(v_vFSAComplaintFolderCnt) Then
                If v_vFSAComplaintFolderCnt <> 0 Then
                    If bDone Then
                        sSQL = sSQL & "AND "
                    Else
                        sSQL = sSQL & "WHERE "
                        bDone = True
                    End If
                    sSQL = sSQL & "el.fsa_complaint_folder_cnt = " & CStr(v_vFSAComplaintFolderCnt) & Strings.ChrW(13) & Strings.ChrW(10)
                End If
            End If

            'sj 04/10/2002 - start

            If Not Informations.IsNothing(v_vAccountKey) Then
                If v_vAccountKey <> 0 Then
                    If bDone Then
                        sSQL = sSQL & "AND "
                    Else
                        sSQL = sSQL & "WHERE "
                        bDone = True
                    End If
                    sSQL = sSQL & "el.account_key = " & CStr(v_vAccountKey) & Strings.ChrW(13) & Strings.ChrW(10)
                End If
            End If

            If Not Informations.IsNothing(v_vFromDate) Then

                sFromDate = CDate(v_vFromDate).ToString("yyyy-MM-dd") & " 00:00:00"
                If bDone Then
                    sSQL = sSQL & " AND "
                Else
                    sSQL = sSQL & " WHERE "
                    bDone = True
                End If
                sSQL = sSQL & "el.event_date >= '" & sFromDate & "'" & Strings.ChrW(13) & Strings.ChrW(10)
            End If

            If Not Informations.IsNothing(v_vToDate) Then

                sToDate = CDate(v_vToDate).ToString("yyyy-MM-dd") & " 23:59:59"
                If bDone Then
                    sSQL = sSQL & " AND "
                Else
                    sSQL = sSQL & " WHERE "
                    bDone = True
                End If
                sSQL = sSQL & "el.event_date <= '" & sToDate & "'" & Strings.ChrW(13) & Strings.ChrW(10)
            End If

            If (Not Informations.IsNothing(v_vClaimCnt)) And Not Informations.IsNothing(v_vBaseCaseID) Then
                If v_vClaimCnt <> 0 And v_vBaseCaseID <> 0 Then
                    If bDone Then
                        sSQL = sSQL & "AND "
                    Else
                        sSQL = sSQL & "WHERE "
                        bDone = True
                    End If
                    sSQL = sSQL & " ISNULL(is_dirty,0)=0 AND clm.base_case_id = " & CStr(gPMFunctions.ToSafeLong(v_vBaseCaseID)) & Strings.ChrW(13) & Strings.ChrW(10)
                End If
            End If

            If Not Informations.IsNothing(v_vBaseCaseID) Then
                If v_vBaseCaseID <> 0 And v_vClaimCnt = 0 And v_vBaseClaimID = 0 Then
                    If bDone Then
                        sSQL = sSQL & "AND "
                    Else
                        sSQL = sSQL & "WHERE "
                        bDone = True
                    End If
                    sSQL = sSQL & " EL.event_cnt in ( "
                    sSQL = sSQL & " SELECT EL.event_cnt FROM   event_log EL JOIN   Claim C  ON El.claim_cnt = C.Claim_id "
                    sSQL = sSQL & " WHERE  C.Claim_id IN(SELECT claim_id FROM   claim WHERE ISNULL(is_dirty,0)=0 AND base_case_id  =  " & CLng(v_vBaseCaseID) & ")" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & " Union  "
                    sSQL = sSQL & " SELECT el.event_cnt FROM   event_log EL "
                    sSQL = sSQL & " JOIN   [CASE] CS ON El.case_id = CS.case_id WHERE  el.case_id IN (SELECT case_id "
                    sSQL = sSQL & " FROM   [case] WHERE  base_case_id  =  " & CLng(v_vBaseCaseID) & ")" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & " ) " & Strings.ChrW(13) & Strings.ChrW(10)

                End If
            End If

            If v_vBaseCaseID <> 0 Or v_vBaseClaimID <> 0 Then
                If bDone Then
                    sSQL = sSQL & "AND "
                Else
                    sSQL = sSQL & "WHERE "
                    bDone = True
                End If
                'Claims Notes should be user specific
                sSQL = sSQL & "((et.event_type_id<>22) OR (et.event_type_id=22 AND el.user_id=" & CStr(m_iUserID) & ")" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " OR (et.event_type_id=22 AND EXISTS(SELECT UGU.user_id FROM PMUser_Group_User UGU " & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " JOIN PMUser_Group UG " & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " ON UGU.pmuser_group_id=UG.pmuser_group_id " & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " WHERE UG.code='CASESUPER' AND UGU.user_id= " & CStr(m_iUserID) & " ))) " & Strings.ChrW(13) & Strings.ChrW(10) 'AND el.user_id=" & m_iUserID & "
            End If

            'Start Praveen - Tech Spec getEventDetails_Additional Changes

            If Not Informations.IsNothing(v_vBgID) Then
                If v_vBgID <> 0 Then
                    If bDone Then
                        sSQL = sSQL & "AND "
                    Else
                        sSQL = sSQL & "WHERE "
                        bDone = True
                    End If
                    sSQL = sSQL & "el.bg_id=" & CStr(v_vBgID) & Strings.ChrW(13) & Strings.ChrW(10)
                End If
            End If


            If Not Informations.IsNothing(v_vEventType) Then
                If v_vEventType <> 0 Then
                    If bDone Then
                        sSQL = sSQL & "AND "
                    Else
                        sSQL = sSQL & "WHERE "
                        bDone = True
                    End If
                    sSQL = sSQL & "et.event_type_group_id=" & CStr(v_vEventType) & Strings.ChrW(13) & Strings.ChrW(10)
                End If
            End If


            If Not Informations.IsNothing(v_vUserId) Then
                If v_vUserId <> 0 Then
                    If bDone Then
                        sSQL = sSQL & "AND "
                    Else
                        sSQL = sSQL & "WHERE "
                        bDone = True
                    End If
                    sSQL = sSQL & "el.User_Id=" & CStr(v_vUserId) & Strings.ChrW(13) & Strings.ChrW(10)
                End If
            End If

            If ToSafeString(v_vClaimNumber) = "" AndAlso ToSafeInteger(v_vClaimCnt) = 0 AndAlso ToSafeInteger(v_vBaseClaimID) = 0 Then
                If bDone Then
                    sSQL = sSQL & "AND "
                Else
                    sSQL = sSQL & "WHERE "
                    bDone = True
                End If
                sSQL = sSQL & " ISNULL(clm.is_dirty,0)=0 " & vbCrLf
            End If

            If Not Informations.IsNothing(v_vAgentKey) Then
                If v_vAgentKey <> 0 Then
                    If bDone Then
                        sSQL = sSQL & "AND "
                    Else
                        sSQL = sSQL & "WHERE "
                        bDone = True
                    End If
                    sSQL = sSQL & "(fi.lead_agent_cnt  = " & CStr(v_vAgentKey) & Strings.ChrW(13) & Strings.ChrW(10) & ")"
                End If
            End If

            'ORDER BY
            sSQL = sSQL & "ORDER BY el.event_type_id, el.event_date DESC"
            'MKW050603 PN3372 -END- 1.6.9 to 1.8.6 catchup MODIFIED with previous 1.8.6 changes

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get lookup details for event type group
            m_lReturn = CType(GetEventTypeGroup(r_vEventTypeGroupArray:=r_vEventTypeGroupArray, v_vInsuranceFileCnt:=v_vInsuranceFileCnt, v_vClaimCnt:=v_vClaimCnt), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetEventTypeGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAll")
                Return result
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchAll Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAll", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: SearchWarnings (Public)
    '
    ' Description: Gets Outstanding Warnings For ClientManager
    ' Edit History  :
    ' ECK 13042005  : Created for 2005 StickyNote display
    '
    ' ***************************************************************** '
    Public Function SearchWarnings(ByRef r_vResultArray(,) As Object, ByVal v_vPartyCnt As Object) As Integer

        Dim result As Integer = 0

        'developer guide no 21. 
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Check for Valid parameters

            Dim dbNumericTemp As Double
            If (Not False) And (Not Double.TryParse(CStr(v_vPartyCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : v_vPartyCnt=" & CStr(v_vPartyCnt), vApp:=ACApp, vClass:=ACClass, vMethod:="SearchWarnings")

                Return result
            End If
            sSQL = ""
            sSQL = "SELECT  el.event_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "el.event_date," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "els.description," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "el.priority_code," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "el.sticky_top," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "el.sticky_left," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "us.username," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "el.event_log_subject_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "etg.description," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "el.description" & Strings.ChrW(13) & Strings.ChrW(10)

            'FROM
            sSQL = sSQL & "FROM event_log el" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "LEFT OUTER JOIN event_log_subject els" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ON els.event_log_subject_id = el.event_log_subject_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "JOIN event_type et" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ON et.event_type_id = el.event_type_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "JOIN event_type_group etg" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ON etg.event_type_group_id = et.event_type_group_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "JOIN PMUser us" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ON us.user_id = el.user_id" & Strings.ChrW(13) & Strings.ChrW(10)
            'WHERE

            sSQL = sSQL & "WHERE el.party_cnt = " & CStr(CInt(v_vPartyCnt)) & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "AND el.is_completed <> 1" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "AND et.code = 'N_WARN'" & Strings.ChrW(13) & Strings.ChrW(10)
            'ORDER BY
            sSQL = sSQL & "ORDER BY el.event_date"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="SearchWarnings", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchWarnings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchWarnings", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: PositionWarnings (Public)
    '
    ' Description: Gets Outstanding Warnings For ClientManager
    ' Edit History  :
    ' ECK 13042005  : Created for 2005 StickyNote display
    '
    ' ***************************************************************** '
    Public Function PositionWarnings(ByVal v_vEventCnt As Object, ByVal v_vStickyTop As Object, ByVal v_vStickyLeft As Object) As Integer

        Dim result As Integer = 0
        'developer guide no 21. 
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Check for Valid parameters

            Dim dbNumericTemp As Double
            If (Not False) And (Not Double.TryParse(CStr(v_vEventCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : v_vEventCnt=" & CStr(v_vEventCnt), vApp:=ACApp, vClass:=ACClass, vMethod:="PositionWarnings")

                Return result
            End If

            Dim dbNumericTemp2 As Double
            If (Not False) And (Not Double.TryParse(CStr(v_vStickyTop), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : v_vStickyTop=" & CStr(v_vStickyTop), vApp:=ACApp, vClass:=ACClass, vMethod:="PositionWarnings")

                Return result
            End If

            Dim dbNumericTemp3 As Double
            If (Not False) And (Not Double.TryParse(CStr(v_vStickyLeft), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : v_vStickyLeft=" & CStr(v_vStickyLeft), vApp:=ACApp, vClass:=ACClass, vMethod:="PositionWarnings")

                Return result
            End If
            sSQL = ""
            'UPDATE
            sSQL = "UPDATE  event_log" & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & " SET sticky_top = " & CStr(CInt(v_vStickyTop)) & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & ",sticky_left = " & CStr(CInt(v_vStickyLeft)) & Strings.ChrW(13) & Strings.ChrW(10)
            'WHERE

            sSQL = sSQL & "WHERE event_cnt = " & CStr(CInt(v_vEventCnt))

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="PositionWarnings", bStoredProcedure:=False, lRecordsAffected:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PositionWarnings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PositionWarnings", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: CloseWarnings (Public)
    '
    ' Description: Gets Outstanding Warnings For ClientManager
    ' Edit History  :
    ' ECK 13042005  : Created for 2005 StickyNote display
    '
    ' ***************************************************************** '
    Public Function CloseWarnings(ByVal v_vEventCnt As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Check for Valid parameters

            Dim dbNumericTemp As Double
            If (Not False) And (Not Double.TryParse(CStr(v_vEventCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : v_vEventCnt=" & CStr(v_vEventCnt), vApp:=ACApp, vClass:=ACClass, vMethod:="CloseWarnings")

                Return result
            End If
            sSQL = ""
            'UPDATE
            sSQL = "UPDATE  event_log" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " SET is_completed = 1 " & Strings.ChrW(13) & Strings.ChrW(10)
            'WHERE

            sSQL = sSQL & "WHERE event_cnt = " & CStr(CInt(v_vEventCnt))

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="CloseWarnings", bStoredProcedure:=False, lRecordsAffected:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CloseWarnings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CloseWarnings", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: WriteTemplate (Public)
    '
    ' Description: Writes the document template to the event log.
    '               Used for transactions MSS180601
    '
    ' ***************************************************************** '
    Public Function WriteTemplate(ByVal TemplateID As Integer) As Integer
        Return WriteTemplate(TemplateID:=TemplateID, v_lEventCnt:=0)
    End Function

    Public Function WriteTemplate(ByVal TemplateID As Integer, ByVal v_lEventCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "WriteTemplate"

        Dim sSQL As String = ""

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = ""
            sSQL = sSQL & "UPDATE event_log" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "SET document_cnt = " & CStr(TemplateID) & Strings.ChrW(13) & Strings.ChrW(10)

            If Not False Then
                sSQL = sSQL & "WHERE event_cnt = " & CStr(v_lEventCnt) & Strings.ChrW(13) & Strings.ChrW(10)
            Else
                sSQL = sSQL & "WHERE event_cnt =" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "    (" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "        SELECT" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "            MAX(event_cnt)" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "        FROM event_log" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "        WHERE user_id = " & CStr(m_iUserID) & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "    )" & Strings.ChrW(13) & Strings.ChrW(10)
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="ACUpdateEvent", bStoredProcedure:=False)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "sSQLName:=ACUpdateEvent", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here

            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SetCompleted (Public)
    '
    ' Description: Updates sticky note events as com
    ' History: Created ECK 12/04/2005
    '
    ' ***************************************************************** '
    Public Function SetCompleted() As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "UPDATE Event_Log SET Is_completed = 1 WHERE Event_Cnt = " & m_lEventCnt

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="ACUpdateEvent", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetCompleted Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Write Template", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: UpdateWarning (Public)
    '
    ' Description: Updates sticky note events as com
    ' History: Created ECK 12/04/2005
    '
    ' ***************************************************************** '
    Public Function UpdateWarning(ByRef v_vDescription As String, ByRef v_vIsCompleted As String, ByRef v_vPriority As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "UPDATE Event_Log SET "
            sSQL = sSQL & "Description = '" & v_vDescription.Replace("'", "''") & "', " 'PN23489
            sSQL = sSQL & "Is_Completed = " & v_vIsCompleted & ", "
            sSQL = sSQL & "Priority_Code = '" & v_vPriority & "' "
            sSQL = sSQL & "WHERE Event_Cnt = " & CStr(m_lEventCnt)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="ACUpdateEvent", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateWarning Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Write Template", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    Private Function BeginTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Private Function CommitTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Private Function RollbackTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLRollbackTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory (Private)
    '
    ' Description: Check Mandatory parameters have been passed.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CheckMandatory) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CheckMandatory(Optional ByRef vEventCnt As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vInsuranceFolderCnt As Object = Nothing, Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vClaimCnt As Object = Nothing, Optional ByRef vDocumentCnt As Object = Nothing, Optional ByRef vOldAddressCnt As Object = Nothing, Optional ByRef vNewAddressCnt As Object = Nothing, Optional ByRef vCampaignId As Object = Nothing, Optional ByRef vDocumentType As Object = Nothing, Optional ByRef vReportType As Object = Nothing, Optional ByRef vEventType As Object = Nothing, Optional ByRef vUserId As Object = Nothing, Optional ByRef vEventDate As Object = Nothing, Optional ByRef vDescription As Object = Nothing) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    '
    ' {* USER DEFINED CODE (Begin) *}
    '
    '
    ' {* USER DEFINED CODE (End) *}
    '
    'Return gPMConstants.PMEReturnCode.PMTrue
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckMandatory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name: UnderwritingOrAgency
    '
    ' Description:  Finds out if Underwriting or Agency business
    '
    ' History:
    ' 21/09/2001 MS - Added for merge
    ' 14/06/2002 SP - moved to uniform Product Options scheme
    ' ***************************************************************** '
    Private Function getUnderwritingOrAgency() As Integer

        Dim result As Integer = 0



        Return bPMFunc.getUnderwritingOrAgency(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, m_sUnderwritingOrAgency)

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetEventTypeGroup
    '
    ' Description:
    '
    ' History: 27/09/2002 SJ - Created.
    '
    ' ***************************************************************** '
    'developer guide no.33
    Public Function GetEventTypeGroup(ByRef r_vEventTypeGroupArray As Object) As Integer
        Return GetEventTypeGroup(r_vEventTypeGroupArray:=r_vEventTypeGroupArray, v_vInsuranceFileCnt:=Nothing, v_vClaimCnt:=Nothing)
    End Function

    Public Function GetEventTypeGroup(ByRef r_vEventTypeGroupArray As Object, ByVal v_vInsuranceFileCnt As Object, ByVal v_vClaimCnt As Object) As Integer

        Dim result As Integer = 0
        Try

            Dim sSQL As String = ""

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()


            sSQL = "SELECT description,event_type_group_id, code FROM event_type_group "
            If v_vInsuranceFileCnt <> 0 Or v_vClaimCnt <> 0 Then
                sSQL = sSQL & "WHERE exclusion_level NOT in ("
                If v_vInsuranceFileCnt <> 0 Then
                    sSQL = sSQL & "1"
                End If
                If v_vClaimCnt <> 0 Then
                    If v_vInsuranceFileCnt <> 0 Then
                        sSQL = sSQL & ","
                    End If
                    sSQL = sSQL & "2"
                End If
                sSQL = sSQL & ") OR exclusion_level IS NULL "
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetEventTypeGroup", bStoredProcedure:=False, vResultArray:=r_vEventTypeGroupArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetEventTypeGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetEventTypeGroup", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GetNoteEventType
    '
    ' Description:
    '
    ' History: 01/10/2002 SJ - Created.
    '
    ' ***************************************************************** '
    'developer guide no. 101
    Public Function GetNoteEventType(ByRef r_vNoteEventTypeArray(,) As Object, ByRef r_iListIndex As Integer) As Integer
        Return GetNoteEventType(r_vNoteEventTypeArray:=r_vNoteEventTypeArray, r_iListIndex:=r_iListIndex, v_vInsuranceFileCnt:=Nothing, v_vClaimCnt:=Nothing, v_vAccountKey:=Nothing, v_vCaseID:=Nothing, v_lBaseClaimID:=Nothing, v_bAddSticky:=False)
    End Function

    Public Function GetNoteEventType(ByRef r_vNoteEventTypeArray(,) As Object, ByRef r_iListIndex As Integer, ByVal v_vInsuranceFileCnt As Object, ByVal v_vClaimCnt As Object, ByVal v_vAccountKey As Object, ByVal v_vCaseID As Object) As Integer
        Return GetNoteEventType(r_vNoteEventTypeArray:=r_vNoteEventTypeArray, r_iListIndex:=r_iListIndex, v_vInsuranceFileCnt:=v_vInsuranceFileCnt, v_vClaimCnt:=v_vClaimCnt, v_vAccountKey:=v_vAccountKey, v_vCaseID:=v_vCaseID, v_lBaseClaimID:=Nothing, v_bAddSticky:=False)
    End Function

    Public Function GetNoteEventType(ByRef r_vNoteEventTypeArray(,) As Object, ByRef r_iListIndex As Integer, ByVal v_vInsuranceFileCnt As Object, ByVal v_vClaimCnt As Object, ByVal v_vAccountKey As Object, ByVal v_vCaseID As Object, ByVal v_lBaseClaimID As Object, ByVal v_bAddSticky As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vTabArray(3, 0) As Object
            Dim vResultArray(,) As Object = Nothing
            Dim iLookupType As gPMConstants.PMELookupType
            Dim bRequired, bDefaultSelection As Boolean
            Dim iCnt As Integer


            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = "event_type"

            iLookupType = gPMConstants.PMELookupType.PMLookupAll

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""


            ' Get the Lookup items
            m_lReturn = m_oLookup.GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTabArray, iLanguageID:=m_iLanguageID, dtEffectiveDate:=DateTime.Now.AddDays(1), vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            iCnt = 0


            For i As Integer = 0 To vResultArray.GetUpperBound(1)

                bRequired = False
                bDefaultSelection = False


                Select Case CStr(vResultArray(gPMConstants.PMELookupOutArrayColPos.PMLookupCode, i)).Trim().ToUpper()
                    Case gSIRLibrary.ACNotesCustomer
                        bRequired = True
                        If v_vClaimCnt = 0 And v_vAccountKey = 0 And v_vInsuranceFileCnt = 0 Then
                            bDefaultSelection = True
                        End If
                        '2005 StickyNotes
                    Case gSIRLibrary.ACNotesWarning
                        bRequired = True
                        If v_vClaimCnt = 0 And v_vAccountKey = 0 And v_vInsuranceFileCnt = 0 And v_bAddSticky Then
                            bDefaultSelection = True
                        End If
                    Case gSIRLibrary.ACNotesPolicy
                        If v_vInsuranceFileCnt <> 0 Then
                            bRequired = True
                            If v_vClaimCnt = 0 And v_vAccountKey = 0 Then
                                bDefaultSelection = True
                            End If
                        End If
                    Case gSIRLibrary.ACNotesClaims
                        If v_vClaimCnt <> 0 Or (v_vCaseID <> 0) Or (v_lBaseClaimID <> 0) Then
                            bRequired = True
                            bDefaultSelection = True
                        End If
                    Case gSIRLibrary.ACNotesAccount
                        If v_vAccountKey <> 0 Then
                            bRequired = True
                            bDefaultSelection = True
                        End If
                    Case gSIRLibrary.ACNotesCase
                        If v_vCaseID <> 0 Then
                            bRequired = True
                            bDefaultSelection = True
                        End If
                End Select

                If bRequired Then
                    If iCnt = 0 Then
                        ReDim r_vNoteEventTypeArray(2, iCnt)
                    Else
                        ReDim Preserve r_vNoteEventTypeArray(2, iCnt)
                    End If



                    r_vNoteEventTypeArray(gPMConstants.PMELookupOutArrayColPos.PMLookupCaption, iCnt) = vResultArray(gPMConstants.PMELookupOutArrayColPos.PMLookupCaption, i)


                    r_vNoteEventTypeArray(gPMConstants.PMELookupOutArrayColPos.PMLookupCode, iCnt) = vResultArray(gPMConstants.PMELookupOutArrayColPos.PMLookupCode, i)


                    r_vNoteEventTypeArray(gPMConstants.PMELookupOutArrayColPos.PMLookupID, iCnt) = vResultArray(gPMConstants.PMELookupOutArrayColPos.PMLookupID, i)
                    If bDefaultSelection Then
                        r_iListIndex = iCnt
                    End If
                    iCnt += 1
                End If
            Next i


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNoteEventType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNoteEventType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Find a previout event log for a documaster document and copies it
    ' with a prefixed description. e.g.
    '   Original:   Quotation Document
    '   DME Print:  Reprinted: Quotation Document   (prefix: "Reprinted:")
    '   DME Email:  Emailed: Quotation Document     (prefix: "Emailed:")
    Public Function CopyEventByDocumentCnt(ByVal v_lDocumentCnt As Integer, ByVal v_dtEventDate As Date, ByVal v_sDescriptionPrefix As String, ByRef r_lEventCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "CopyEventByDocumentCnt"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "document_cnt", v_lDocumentCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "event_date", v_dtEventDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            bPMAddParameter.AddParameterLite(m_oDatabase, "description_prefix", v_sDescriptionPrefix, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "event_cnt", 0, gPMConstants.PMEParameterDirection.PMParamInputOutput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "userid", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            ' Call copy procedure
            lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyByDocumentCntSQL, sSQLName:=ACCopyByDocumentCntName, bStoredProcedure:=ACCopyByDocumentCntStored)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Failed to copy documaster event")
            End If

            ' Get new event cnt
            r_lEventCnt = gPMFunctions.ToSafeLong(m_oDatabase.Parameters.Item("event_cnt").Value)

            ' If we have no new event it was because there was no original event to copy
            If r_lEventCnt = 0 Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If
            Return result
        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: UpdateClaimEvents (Public)
    '
    ' Description: Updates SIREvent directly into the database.
    ' ***************************************************************** '
    Public Function UpdateClaimEvents(ByRef vPartyCnt As Object, ByRef vInsuranceFolderCnt As Object, ByRef vInsuranceFileCnt As Object, ByRef vClaimCnt As Object) As Integer

        Dim result As Integer = 0
        Dim oSIREvent As bSIREvent.SIREvent
        Dim dtDateTime As Date

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new SIREvent
            oSIREvent = New bSIREvent.SIREvent()
            m_lReturn = CType(oSIREvent.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            'We're passing in a date, from the PC.  Really we want the date (and time) on the server.
            dtDateTime = DateTime.Now

            ' Populate SIREvent Attributes

            'developer guide no. 98
            m_lReturn = CType(oSIREvent.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMEdit, vPartyCnt:=vPartyCnt, vInsuranceFolderCnt:=vInsuranceFolderCnt, vInsuranceFileCnt:=vInsuranceFileCnt, vClaimCnt:=vClaimCnt, vEventDate:=dtDateTime), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIREvent = Nothing
                Return result
            End If

            ' Add the SIREvent to the Database
            m_lReturn = CType(oSIREvent.UpdateEventLogClaimPolicy(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIREvent = Nothing
                Return result
            End If

            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}
            oSIREvent.Dispose()
            oSIREvent = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateClaimEvents Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateClaimEvents", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
