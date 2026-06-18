Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
Imports System.Text
'Developer Guide No. 129
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 31/07/1997
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a Source.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 18/09/2003
    ' Username.
    Private m_sUsername As String = ""

    ' Password.
    Private m_sPassword As String = ""

    ' User ID
    Private m_iUserID As Integer

    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    ' ************************************************

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Collection of Sources (Private)
    Private m_oSources As bPMSource.Sources

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    ' Task
    Private m_iTask As Integer
    ' Navigate
    Private m_lNavigate As Integer
    ' Process Mode
    Private m_lProcessMode As Integer
    ' Type of Business
    Private m_sTransactionType As String = ""
    ' Effective
    Private m_dtEffectiveDate As Date

    ' Primary Keys to work with
    ' Source ID
    Private m_iSourceIDX As Integer
    ' Insurance File ID

    ' PM Lookup Business Component (Private)
    Private m_oLookup As BPMLOOKUP.Business

    ' PM Caption object
    Private m_oCaption As bPMCaption.Business

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public Property CurrentRecord() As Integer
        Get
            Return m_lCurrentRecord
        End Get
        Set(ByVal Value As Integer)
            Select Case Value
                Case Is < 1
                    m_lCurrentRecord = 0
                Case Is > m_oSources.Count()
                    m_lCurrentRecord = m_oSources.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select
        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get
            ' Return Numner in Collection
            Return m_oSources.Count()
        End Get
    End Property

    Public ReadOnly Property Task() As Integer
        Get
            Return m_iTask
        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get
            Return m_lNavigate
        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get
            Return m_lProcessMode
        End Get
    End Property

    Public ReadOnly Property TransactionType() As String
        Get
            Return m_sTransactionType
        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get
            Return m_dtEffectiveDate
        End Get
    End Property

    Public Property SourceID() As Integer
        Get
            Return m_iSourceIDX
        End Get
        Set(ByVal Value As Integer)
            m_iSourceIDX = Value
        End Set
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusArchitecture
        End Get
    End Property

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
    Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        ' DC 31/01/00
        ' RDC 13062002 CompServ replaced with BAS module
        '    Dim oComponentServices As PMServerBusinessCS

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialisation Code.

            m_sUsername = sUserName
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            ' Get Reference to Database

            ' DC 31/01/00
            ' change way of referencing database

            '    Set m_oDatabase = GetOrionDatabase( _
            ''                            lOpenStatus:=m_lReturn, _
            ''                            bCloseDatabase:=m_bCloseDatabase, _
            ''                            vDatabase:=vDatabase)

            '    Set oComponentServices = New PMServerBusinessCS

            '    m_lReturn& = oComponentServices.CheckDatabase( _
            'v_lPMProductFamily:=PMProductFamily, _
            'r_bNewInstanceCreated:=m_bCloseDatabase, _
            'r_oCheckedDatabase:=m_oDatabase, _
            'v_vDatabase:=vDatabase)

            m_lReturn = CType(gPMComponentServices.CheckDatabase(v_sUsername:=m_sUsername, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '        Set oComponentServices = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the caption object
            '    m_lReturn = oComponentServices.CreateBusinessObject(r_oObject:=m_oCaption, _
            'v_sClassName:="bPMCaption.Business", _
            'v_sCallingAppName:=ACApp, _
            'v_sUserName:=g_sUsername, _
            'v_sPassword:=g_sPassword, _
            'v_iUserID:=g_iUserID, _
            'v_iSourceID:=g_iSourceID, _
            'v_iLanguageId:=g_iLanguageID, _
            'v_iCurrencyID:=g_iCurrencyID, _
            'v_iLogLevel:=g_iLogLevel)

            m_oCaption = New bPMCaption.Business
            m_lReturn = m_oCaption.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '        Set oComponentServices = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    Set oComponentServices = Nothing

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Set the ProcessMode to Generic
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric

            ' Set the Type Of Business to New Business
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric

            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now

            ' Create Sources Collection
            m_oSources = New bPMSource.Sources()
            'Developer Guide No. 97
            m_lReturn = CType(m_oSources.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, ACApp), gPMConstants.PMEReturnCode)

            ' Create PM Lookup Business Object
            m_oLookup = New BPMLOOKUP.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUserName, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusArchitecture

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                If m_oCaption IsNot Nothing Then
                    m_oCaption.Dispose()
                    m_oCaption = Nothing
                End If
                If m_oLookup IsNot Nothing Then
                    m_oLookup.Dispose()
                    m_oLookup = Nothing
                End If
                m_oSources = Nothing
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



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values for a Source.
    '
    '
    ' ***************************************************************** '
    'Developer Guide No. 33(Guide)
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim oSource As bPMSource.Source = Nothing
        Dim dtEffectiveDate As Date

        ' {* USER DEFINED CODE (Begin) *}
        'MKW010903 PS255 - FSA Compliance - Added additional array level
        'TF180903 - SubBranch Table added
        Dim vTabArray(3, 2) As Object
        ' {* USER DEFINED CODE (End) *}

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array
            'Developer Guide No. 12
            vResultArray = Nothing
            ' Reset Table Array

            'Developer Guide No. 12
            vTableArray = Nothing

            ' {* USER DEFINED CODE (Begin) *}

            ' Setup Lookup Table Names

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = gPMConstants.PMLookupCountry


            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 1) = "FSA_CompanyCategory" 'MKW010903 PS255 - FSA Compliance


            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 2) = "Sub_Branch" 'TF180903

            ' {* USER DEFINED CODE (End) *}

            ' Do we have any records
            If m_lCurrentRecord < 1 Then
                ' No, we can only lookup all
                iLookupType = gPMConstants.PMELookupType.PMLookupAll
            Else
                ' Yes get current record
                oSource = m_oSources.Item(m_lCurrentRecord)
            End If

            Select Case iLookupType
                Case gPMConstants.PMELookupType.PMLookupAll

                    ' {* USER DEFINED CODE (Begin) *}
                    ' Do not supply a key

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = "" 'MKW010903 PS255 - FSA Compliance

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 2) = "" 'TF180903
                    ' {* USER DEFINED CODE (End) *}

                    ' Default Effective Date to current date/time
                    dtEffectiveDate = DateTime.Now

                Case gPMConstants.PMELookupType.PMLookupAllEffective

                    ' Use keys and effective date from current record
                    ' Note: The keys are not used for the select, but are used by
                    '       the iterface program to set the list index.
                    With oSource

                        ' {* USER DEFINED CODE (Begin) *}

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = .CountryID


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = .FSA_CompanyCategory_ID 'MKW010903 PS255 - FSA Compliance

                        'BB dtEffectiveDate = .EffectiveDate
                        'BB Default Effective Date to current date/time when not present
                        dtEffectiveDate = DateTime.Now
                        ' {* USER DEFINED CODE (End) *}

                    End With

                Case gPMConstants.PMELookupType.PMLookupSingle

                    ' Set keys from current record
                    With oSource

                        ' {* USER DEFINED CODE (Begin) *}

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = .CountryID
                        ' {* USER DEFINED CODE (End) *}

                    End With
                    ' Default Effective Date to current date/time
                    dtEffectiveDate = DateTime.Now

            End Select

            ' Release Source reference
            oSource = Nothing

            ' Get the Lookup items
            m_lReturn = m_oLookup.GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTabArray, iLanguageID:=iLanguageID, dtEffectiveDate:=dtEffectiveDate, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the Table Array

            vTableArray = vTabArray

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single Source directly into the database.
    '        Note: The Source will NOT be added to the collection.
    '
    ' ***************************************************************** '
    ' DC 31/01/00
    ' added email, vat no, sender mailbox id, broker abi id, user licence id,
    '       pm Source number and default indicator
    Public Function DirectAdd(Optional ByRef vSourceID As Integer = 0, Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vParentID As Object = Nothing, Optional ByRef vBaseCurrency As Object = Nothing, Optional ByRef vRegNo1 As Object = Nothing, Optional ByRef vRegNo2 As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vPhoneAreaCode As Object = Nothing, Optional ByRef vPhoneNumber As Object = Nothing, Optional ByRef vPhoneExtension As Object = Nothing, Optional ByRef vFaxAreaCode As Object = Nothing, Optional ByRef vFaxNumber As Object = Nothing, Optional ByRef vFaxExtension As Object = Nothing, Optional ByRef vEmail As Object = Nothing, Optional ByRef vVatNo As Object = Nothing, Optional ByRef vSenderMailboxId As Object = Nothing, Optional ByRef vBrokerABIId As Object = Nothing, Optional ByRef vUserLicenceId As Object = Nothing, Optional ByRef vPMSourceNumber As Object = Nothing, Optional ByRef vDefaultIndicator As Object = Nothing, Optional ByRef vUnderwritingBranchInd As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSource As bPMSource.Source

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Source
            oSource = New bPMSource.Source()
            m_lReturn = CType(oSource.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp), gPMConstants.PMEReturnCode)
            ' Populate Source Attributes
            ' DC 31/01/00
            ' added email, vat no, sender mailbox id, broker abi id, user licence id,
            '       pm Source number and default indicator

            m_lReturn = CType(SetProperties(oSource, gPMConstants.PMEComponentAction.PMAdd, vSourceID:=vSourceID, vCode:=CStr(vCode), vDescription:=CStr(vDescription), vCaptionID:=vCaptionID, vParentID:=CInt(vParentID), vBaseCurrency:=vBaseCurrency, vRegNo1:=CStr(vRegNo1), vRegNo2:=CStr(vRegNo2), vAddress1:=CStr(vAddress1), vAddress2:=CStr(vAddress2), vAddress3:=CStr(vAddress3), vAddress4:=CStr(vAddress4), vPostalCode:=CStr(vPostalCode), vCountryID:=CInt(vCountryID), vPhoneAreaCode:=CStr(vPhoneAreaCode), vPhoneNumber:=CStr(vPhoneNumber), vPhoneExtension:=CStr(vPhoneExtension), vFaxAreaCode:=CStr(vFaxAreaCode), vFaxNumber:=CStr(vFaxNumber), vFaxExtension:=CStr(vFaxExtension), vEmail:=CStr(vEmail), vVatNo:=CStr(vVatNo), vSenderMailboxId:=CStr(vSenderMailboxId), vBrokerABIId:=CStr(vBrokerABIId), vUserLicenceId:=CInt(vUserLicenceId), vPMSourceNumber:=CInt(vPMSourceNumber), vDefaultIndicator:=CStr(vDefaultIndicator), vUnderwritingBranchInd:=CInt(vUnderwritingBranchInd)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Source to the Database
            m_lReturn = CType(AddItem(oSource), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (Begin) *}

            ' Return the ID of the Source Added

            If Not Informations.IsNothing(vSourceID) Then
                vSourceID = oSource.SourceID
            End If

            ' {* USER DEFINED CODE (End) *}
            oSource.Dispose()
            oSource = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectDelete (Public)
    '
    ' Description: Deletes a single Source directly from the database.
    '        Note: The Source will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    ' DC 31/01/00
    ' added email, vat no, sender mailbox id, broker abi id, user licence id,
    '       pm Source number and default indicator
    Public Function DirectDelete(Optional ByRef vSourceID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vParentID As Object = Nothing, Optional ByRef vBaseCurrency As Object = Nothing, Optional ByRef vRegNo1 As Object = Nothing, Optional ByRef vRegNo2 As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vPhoneAreaCode As Object = Nothing, Optional ByRef vPhoneNumber As Object = Nothing, Optional ByRef vPhoneExtension As Object = Nothing, Optional ByRef vFaxAreaCode As Object = Nothing, Optional ByRef vFaxNumber As Object = Nothing, Optional ByRef vFaxExtension As Object = Nothing, Optional ByRef vEmail As Object = Nothing, Optional ByRef vVatNo As Object = Nothing, Optional ByRef vSenderMailboxId As Object = Nothing, Optional ByRef vBrokerABIId As Object = Nothing, Optional ByRef vUserLicenceId As Object = Nothing, Optional ByRef vPMSourceNumber As Object = Nothing, Optional ByRef vDefaultIndicator As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSource As bPMSource.Source

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Source
            oSource = New bPMSource.Source()
            m_lReturn = CType(oSource.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp), gPMConstants.PMEReturnCode)
            ' Populate Source Attributes
            ' DC 31/01/00
            ' added email, vat no, sender mailbox id, broker abi id, user licence id,
            '       pm Source number and default indicator

























            m_lReturn = CType(SetProperties(oSource, gPMConstants.PMEComponentAction.PMDelete, vSourceID:=CInt(vSourceID), vCode:=CStr(vCode), vDescription:=CStr(vDescription), vCaptionID:=vCaptionID, vParentID:=CInt(vParentID), vBaseCurrency:=vBaseCurrency, vRegNo1:=CStr(vRegNo1), vRegNo2:=CStr(vRegNo2), vAddress1:=CStr(vAddress1), vAddress2:=CStr(vAddress2), vAddress3:=CStr(vAddress3), vAddress4:=CStr(vAddress4), vPostalCode:=CStr(vPostalCode), vCountryID:=CInt(vCountryID), vPhoneAreaCode:=CStr(vPhoneAreaCode), vPhoneNumber:=CStr(vPhoneNumber), vPhoneExtension:=CStr(vPhoneExtension), vFaxAreaCode:=CStr(vFaxAreaCode), vFaxNumber:=CStr(vFaxNumber), vFaxExtension:=CStr(vFaxExtension), vEmail:=CStr(vEmail), vVatNo:=CStr(vVatNo), vSenderMailboxId:=CStr(vSenderMailboxId), vBrokerABIId:=CStr(vBrokerABIId), vUserLicenceId:=CInt(vUserLicenceId), vPMSourceNumber:=CInt(vPMSourceNumber), vDefaultIndicator:=CStr(vDefaultIndicator)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Source to the Database
            m_lReturn = CType(DeleteItem(oSource), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            oSource.Dispose()
            oSource = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the Source.
    '
    ' ***************************************************************** '
    ' DC 31/01/00
    ' added email, vat no, sender mailbox id, broker abi id, user licence id,
    '       pm Source number and default indicator
    'MKW010903 PS255 - FSA Compliance Added vfsa_companycategory and vsa_staffwording
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vParentID As Object = Nothing, Optional ByRef vBaseCurrency As Object = Nothing, Optional ByRef vRegNo1 As Object = Nothing, Optional ByRef vRegNo2 As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vPhoneAreaCode As Object = Nothing, Optional ByRef vPhoneNumber As Object = Nothing, Optional ByRef vPhoneExtension As Object = Nothing, Optional ByRef vFaxAreaCode As Object = Nothing, Optional ByRef vFaxNumber As Object = Nothing, Optional ByRef vFaxExtension As Object = Nothing, Optional ByRef vEmail As Object = Nothing, Optional ByRef vVatNo As Object = Nothing, Optional ByRef vSenderMailboxId As Object = Nothing, Optional ByRef vBrokerABIId As Object = Nothing, Optional ByRef vUserLicenceId As Object = Nothing, Optional ByRef vPMSourceNumber As Object = Nothing, Optional ByRef vDefaultIndicator As Object = Nothing, Optional ByRef vfsa_CompanyCategory As Object = Nothing, Optional ByRef vfsa_staffwording As Object = Nothing, Optional ByVal vUnderwritingBranchInd As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults
            ' DC 31/01/00
            ' added email, vat no, sender mailbox id, broker abi id, user licence id,
            '       pm Source number and default indicator
            'MKW010903 PS255 - FSA Compliance Added vfsa_companycategory and vsa_staffwording
            'Developer Guide No. 98
            m_lReturn = DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vSourceID:=vSourceID, vCode:=vCode, vDescription:=vDescription, vCaptionID:=vCaptionID, vParentID:=vParentID, vBaseCurrency:=vBaseCurrency, vRegNo1:=vRegNo1, vRegNo2:=vRegNo2, vAddress1:=vAddress1, vAddress2:=vAddress2, vAddress3:=vAddress3, vAddress4:=vAddress4, vPostalCode:=vPostalCode, vCountryID:=vCountryID, vPhoneAreaCode:=vPhoneAreaCode, vPhoneNumber:=vPhoneNumber, vPhoneExtension:=vPhoneExtension, vFaxAreaCode:=vFaxAreaCode, vFaxNumber:=vFaxNumber, vFaxExtension:=vFaxExtension, vEmail:=vEmail, vVatNo:=vVatNo, vSenderMailboxId:=vSenderMailboxId, vBrokerABIId:=vBrokerABIId, vUserLicenceId:=vUserLicenceId, vPMSourceNumber:=vPMSourceNumber, vDefaultIndicator:=vDefaultIndicator, vfsa_CompanyCategory:=vfsa_CompanyCategory, vfsa_staffwording:=vfsa_staffwording, vUnderwritingBranchInd:=vUnderwritingBranchInd)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaults", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            m_lReturn = m_oDatabase.Parameters.Add(sName:="id", vValue:=CStr(vID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

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



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetBranchBaseCountry
    '
    ' Description:  gets base country for branch
    '
    ' History: DM 09082006 created
    ' ***************************************************************** '
    Public Function GetBranchBaseCountry(ByVal v_lSourceID As Integer, ByRef r_iCountryID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sSQL = "SELECT country_id FROM source "
            sSQL = sSQL & "WHERE source_id = " & CStr(v_lSourceID)

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetBranchBaseCountry", bStoredProcedure:=False)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                r_iCountryID = .Records.Fields("country_id")

            End With


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            'Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBranchBaseCountry failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBranchBaseCountry", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetCaptions (Public)
    '
    ' Description: Get the requested caption fields for a record.
    '
    ' ***************************************************************** '
    Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object, Optional ByRef vTable As Object = Nothing) As Integer

        Dim result As Integer = 0
        'developers guide no 21		
        Dim oFields As DataRow
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Informations.IsArray(vFieldArray) Then
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:="Parameter vFieldArray must be a Variant Array", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCaptions")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have a Table name
            'BB If (IsMissing(vTable) = False) Then

            ' Is this our table
            '    If (Trim$(vTable) <> PMTableSource) Then

            '        GetCaptions = PMInvalidRequest


            '       Exit Function

            '    End If

            'End If

            ' Get the Captions ourself

            ' Check that this record exists
            m_lReturn = CType(CheckID(vID:=vID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Resize the Temporary Results Array
            Dim vResults(vFieldArray.GetUpperBound(0)) As Object

            ' Get a reference to the Fields returned
            oFields = m_oDatabase.Records.Item(1).Fields()

            With oFields

                ' For Each Field requested
                For lSub As Integer = vFieldArray.GetLowerBound(0) To vFieldArray.GetUpperBound(0)

                    ' Store the results in the Temporary results array

                    vResults(lSub) = oFields(vFieldArray(lSub))

                Next lSub

            End With

            ' Assign the results
            vResultArray = vResults

            ' Release the reference to the Fields
            oFields = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCaptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCaptions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required Sources and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vSourceID As Object = Nothing, Optional ByRef vLockMode As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oSource As bPMSource.Source

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oSources.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Default to No Lock if not supplied or not numeric
            Dim dbNumericTemp As Double

            If (Informations.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                vLockMode = gPMConstants.PMELockMode.PMNoLock
            End If

            ' Do we have a key

            If Not Informations.IsNothing(vSourceID) Then

                ' Yes, Is the key valid

                Dim dbNumericTemp2 As Double
                If Not Double.TryParse(CStr(vSourceID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message

                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vSourceID =" & CStr(vSourceID), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                    Return result

                End If

                ' Add the SourceID parameter (INPUT)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Source_id", vValue:=CStr(vSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDetailsSQL, sSQLName:=ACGetDetailsName, bStoredProcedure:=ACGetDetailsStored, lNumberRecords:=0, bKeepNulls:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else

                ' No Key, Get All Records

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllDetailsSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=0, bKeepNulls:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?

            If lRecordCount < 1 Then

                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMNotFound

            Else

                ' Yes, load them into the collection
                'Developer Guide No. 162
                For lSub As Integer = 0 To lRecordCount - 1

                    ' Create New Source
                    oSource = New bPMSource.Source()
                    m_lReturn = oSource.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, ACApp)
                    m_lReturn = CType(SetPropertiesFromDB(oSource:=oSource, lRecordNumber:=lSub), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    ' Add Source to collection
                    If (m_oSources.Count = 0) Then
                        m_oSources.Add(Nothing)
                    End If
                    m_lReturn = CType(m_oSources.Add(oNewSource:=oSource), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    oSource.Dispose()
                    oSource = Nothing

                Next lSub

            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'SJ 17/02/2004 - start
    ' ***************************************************************** '
    ' Name: CheckUnderwritingBranchPolicies (Public)
    '
    ' Description: Gets the required Sources and populate the Collection
    '
    ' ***************************************************************** '
    Public Function CheckUnderwritingBranchPolicies(ByVal vSourceID As Object, ByRef r_bPoliciesExist As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lRecordCount As Integer

            m_oDatabase.Parameters.Clear()

            ' Add the SourceID parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Source_id", vValue:=CStr(vSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckUnderwritingBranchPoliciesSQL, sSQLName:=ACCheckUnderwritingBranchPoliciesName, bStoredProcedure:=ACCheckUnderwritingBranchPoliciesStored, lNumberRecords:=0, bKeepNulls:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lRecordCount = m_oDatabase.Records.Count()

            r_bPoliciesExist = lRecordCount > 0
            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckUnderwritingBranchPolicies Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckUnderwritingBranchPolicies", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'SJ 17/02/2004 - end

    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the required Sources and populate the Collection
    '
    ' ***************************************************************** '
    ' DC 31/01/00
    ' added email, vat no, sender mailbox id, broker abi id, user licence id,
    '       pm Source number and default indicator
    'MKW010903 PS255 - FSA Compliance Added vfsa_companycategory and vsa_staffwording
    'sj 17/2/2004 - Add vUnderwritingBranchInd
    'FSA Phase III FSA_branchtype_
    Public Function GetNext(Optional ByRef vSourceID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vParentID As Object = Nothing, Optional ByRef vBaseCurrency As Object = Nothing, Optional ByRef vRegNo1 As Object = Nothing, Optional ByRef vRegNo2 As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vPhoneAreaCode As Object = Nothing, Optional ByRef vPhoneNumber As Object = Nothing, Optional ByRef vPhoneExtension As Object = Nothing, Optional ByRef vFaxAreaCode As Object = Nothing, Optional ByRef vFaxNumber As Object = Nothing, Optional ByRef vFaxExtension As Object = Nothing, Optional ByRef vEmail As Object = Nothing, Optional ByRef vVatNo As Object = Nothing, Optional ByRef vSenderMailboxId As Object = Nothing, Optional ByRef vBrokerABIId As Object = Nothing, Optional ByRef vUserLicenceId As Object = Nothing, Optional ByRef vPMSourceNumber As Object = Nothing, Optional ByRef vDefaultIndicator As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vfsa_CompanyCategory As Object = Nothing, Optional ByRef vfsa_staffwording As Object = Nothing, Optional ByRef vUnderwritingBranchInd As Object = Nothing, Optional ByRef vAllowTempMTA As Object = Nothing, Optional ByRef vAllowPermMTA As Object = Nothing, Optional ByRef vAllowReports As Object = Nothing, Optional ByRef vAllowClaims As Object = Nothing, Optional ByRef vAllowAccounts As Object = Nothing, Optional ByRef vFSABankTypeID As Object = Nothing, Optional ByRef vUniqueId As String = "", Optional ByRef vScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim oSource As bPMSource.Source
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oSources.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oSource = m_oSources.Item(m_lCurrentRecord)

            ' Get the Source Property Values
            ' DC 31/01/00
            ' added email, vat no, sender mailbox id, broker abi id, user licence id,
            '       pm Source number and default indicator
            'MKW010903 PS255 - FSA Compliance Added vfsa_companycategory and vsa_staffwording
            'FSA Phase III
            'Developer Guide No. 98
            m_lReturn = GetProperties(oSource, iStatus,
        vSourceID:=vSourceID, vCode:=vCode,
        vDescription:=vDescription, vCaptionID:=vCaptionID,
        vParentID:=vParentID, vBaseCurrency:=vBaseCurrency,
        vRegNo1:=vRegNo1, vRegNo2:=vRegNo2,
        vAddress1:=vAddress1, vAddress2:=vAddress2,
        vAddress3:=vAddress3, vAddress4:=vAddress4,
        vPostalCode:=vPostalCode, vCountryID:=vCountryID,
        vPhoneAreaCode:=vPhoneAreaCode, vPhoneNumber:=vPhoneNumber,
        vPhoneExtension:=vPhoneExtension, vFaxAreaCode:=vFaxAreaCode,
        vFaxNumber:=vFaxNumber, vFaxExtension:=vFaxExtension,
        vEmail:=vEmail, vVatNo:=vVatNo,
        vSenderMailboxId:=vSenderMailboxId, vBrokerABIId:=vBrokerABIId,
        vUserLicenceId:=vUserLicenceId, vPMSourceNumber:=vPMSourceNumber,
        vDefaultIndicator:=vDefaultIndicator, vIsDeleted:=vIsDeleted,
        vEffectiveDate:=vEffectiveDate, vfsa_CompanyCategory:=vfsa_CompanyCategory,
        vfsa_staffwording:=vfsa_staffwording, vUnderwritingBranchInd:=vUnderwritingBranchInd,
        vAllowTempMTA:=vAllowTempMTA, vAllowPermMTA:=vAllowPermMTA,
        vAllowReports:=vAllowReports, vAllowClaims:=vAllowClaims,
        vAllowAccounts:=vAllowAccounts, vFSABankTypeID:=vFSABankTypeID,
        vUniqueId:=vUniqueId, vScreenHierarchy:=vScreenHierarchy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oSource = Nothing


            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied Source into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    ' DC 31/01/00
    ' added email, vat no, sender mailbox id, broker abi id, user licence id,
    '       pm Source number and default indicator
    'MKW010903 PS255 - FSA Compliance Added vfsa_companycategory and vsa_staffwording
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vParentID As Object = Nothing, Optional ByRef vBaseCurrency As Object = Nothing, Optional ByRef vRegNo1 As Object = Nothing, Optional ByRef vRegNo2 As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vPhoneAreaCode As Object = Nothing, Optional ByRef vPhoneNumber As Object = Nothing, Optional ByRef vPhoneExtension As Object = Nothing, Optional ByRef vFaxAreaCode As Object = Nothing, Optional ByRef vFaxNumber As Object = Nothing, Optional ByRef vFaxExtension As Object = Nothing, Optional ByRef vEmail As Object = Nothing, Optional ByRef vVatNo As Object = Nothing, Optional ByRef vSenderMailboxId As Object = Nothing, Optional ByRef vBrokerABIId As Object = Nothing, Optional ByRef vUserLicenceId As Object = Nothing, Optional ByRef vPMSourceNumber As Object = Nothing, Optional ByRef vDefaultIndicator As Object = Nothing, Optional ByRef vfsa_CompanyCategory As Object = Nothing, Optional ByRef vfsa_staffwording As Object = Nothing, Optional ByRef vUnderwritingBranchInd As Object = Nothing, Optional ByRef vAllowTempMTA As Object = Nothing, Optional ByRef vAllowPermMTA As Object = Nothing, Optional ByRef vAllowReports As Object = Nothing, Optional ByRef vAllowClaims As Object = Nothing, Optional ByRef vAllowAccounts As Object = Nothing, Optional ByRef vFSABankTypeID As Object = Nothing, Optional ByVal vUniqueId As String = "", Optional ByVal vScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim oSource As bPMSource.Source

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oSources.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new Source
            oSource = New bPMSource.Source()
            'Developer Guide No.9
            m_lReturn = CType(oSource.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp), gPMConstants.PMEReturnCode)
            ' Populate Source Attributes
            ' DC 31/01/00
            ' added email, vat no, sender mailbox id, broker abi id, user licence id,
            '       pm Source number and default indicator
            'MKW010903 PS255 - FSA Compliance Added vfsa_companycategory and vsa_staffwording


































            'Developer Guide No.98
            m_lReturn = CType(SetProperties(oSource, gPMConstants.PMEComponentAction.PMAdd, vSourceID:=vSourceID, vCode:=vCode, vDescription:=vDescription, vCaptionID:=vCaptionID, vParentID:=vParentID, vBaseCurrency:=vBaseCurrency, vRegNo1:=vRegNo1, vRegNo2:=vRegNo2, vAddress1:=vAddress1, vAddress2:=vAddress2, vAddress3:=vAddress3, vAddress4:=vAddress4, vPostalCode:=vPostalCode, vCountryID:=vCountryID, vPhoneAreaCode:=vPhoneAreaCode, vPhoneNumber:=vPhoneNumber, vPhoneExtension:=vPhoneExtension, vFaxAreaCode:=vFaxAreaCode, vFaxNumber:=vFaxNumber, vFaxExtension:=vFaxExtension, vEmail:=vEmail, vVatNo:=vVatNo, vSenderMailboxId:=vSenderMailboxId, vBrokerABIId:=vBrokerABIId, vUserLicenceId:=vUserLicenceId, vPMSourceNumber:=vPMSourceNumber, vDefaultIndicator:=vDefaultIndicator, vfsa_CompanyCategory:=vfsa_CompanyCategory, vfsa_staffwording:=vfsa_staffwording, vUnderwritingBranchInd:=vUnderwritingBranchInd, vAllowTempMTA:=vAllowTempMTA, vAllowPermMTA:=vAllowPermMTA, vAllowReports:=vAllowReports, vAllowClaims:=vAllowClaims, vAllowAccounts:=vAllowAccounts, vFSABankTypeID:=vFSABankTypeID, vUniqueId:=vUniqueId, vScreenHierarchy:=vScreenHierarchy), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oSource = Nothing
                Return result
            End If

            ' Add Source to collection
            If (m_oSources.Count = 0) Then
                m_oSources.Add(Nothing)
            End If
            m_lReturn = CType(m_oSources.Add(oNewSource:=oSource), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSource = Nothing
                Return result
            End If
            oSource.Dispose()
            oSource = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the Source
    '              specified and updates the Source with the new values.
    '
    ' ***************************************************************** '
    ' DC 31/01/00
    ' added email, vat no, sender mailbox id, broker abi id, user licence id,
    '       pm Source number and default indicator
    'MKW010903 PS255 - FSA Compliance Added vfsa_companycategory and vsa_staffwording
    'FSA Phase III
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vParentID As Object = Nothing, Optional ByRef vBaseCurrency As Object = Nothing, Optional ByRef vRegNo1 As Object = Nothing, Optional ByRef vRegNo2 As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vPhoneAreaCode As Object = Nothing, Optional ByRef vPhoneNumber As Object = Nothing, Optional ByRef vPhoneExtension As Object = Nothing, Optional ByRef vFaxAreaCode As Object = Nothing, Optional ByRef vFaxNumber As Object = Nothing, Optional ByRef vFaxExtension As Object = Nothing, Optional ByRef vEmail As Object = Nothing, Optional ByRef vVatNo As Object = Nothing, Optional ByRef vSenderMailboxId As Object = Nothing, Optional ByRef vBrokerABIId As Object = Nothing, Optional ByRef vUserLicenceId As Object = Nothing, Optional ByRef vPMSourceNumber As Object = Nothing, Optional ByRef vDefaultIndicator As Object = Nothing, Optional ByRef vfsa_CompanyCategory As Object = Nothing, Optional ByRef vfsa_staffwording As Object = Nothing, Optional ByRef vUnderwritingBranchInd As Object = Nothing, Optional ByRef vAllowTempMTA As Object = Nothing, Optional ByRef vAllowPermMTA As Object = Nothing, Optional ByRef vAllowReports As Object = Nothing, Optional ByRef vAllowClaims As Object = Nothing, Optional ByRef vAllowAccounts As Object = Nothing, Optional ByRef vFSABankTypeID As Object = Nothing, Optional ByVal vUniqueId As String = "", Optional ByVal vScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim oSource As bPMSource.Source
        Dim iStatus As gPMConstants.PMEComponentAction

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oSources.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oSource = m_oSources.Item(lRow)

            ' Check the Status of the Source

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oSource.DatabaseStatus
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Leave Status as Add
                    iStatus = gPMConstants.PMEComponentAction.PMAdd
                Case gPMConstants.PMEComponentAction.PMDummyDelete
                    ' Error
                    iStatus = gPMConstants.PMEComponentAction.PMAdd
                Case Else
                    ' Set Edit (Update) Status
                    iStatus = gPMConstants.PMEComponentAction.PMEdit
            End Select

            ' Update Source Attributes
            ' DC 31/01/00
            ' added email, vat no, sender mailbox id, broker abi id, user licence id,
            '       pm Source number and default indicator
            'MKW010903 PS255 - FSA Compliance Added vfsa_companycategory and vsa_staffwording



































            'm_lReturn = CType(SetProperties(oSource, iStatus, vSourceID:=CInt(vSourceID), vCode:=CStr(vCode), vDescription:=CStr(vDescription), vCaptionID:=vCaptionID, vParentID:=CInt(vParentID), vBaseCurrency:=vBaseCurrency, vRegNo1:=CStr(vRegNo1), vRegNo2:=CStr(vRegNo2), vAddress1:=CStr(vAddress1), vAddress2:=CStr(vAddress2), vAddress3:=CStr(vAddress3), vAddress4:=CStr(vAddress4), vPostalCode:=CStr(vPostalCode), vCountryID:=CInt(vCountryID), vPhoneAreaCode:=CStr(vPhoneAreaCode), vPhoneNumber:=CStr(vPhoneNumber), vPhoneExtension:=CStr(vPhoneExtension), vFaxAreaCode:=CStr(vFaxAreaCode), vFaxNumber:=CStr(vFaxNumber), vFaxExtension:=CStr(vFaxExtension), vEmail:=CStr(vEmail), vVatNo:=CStr(vVatNo), vSenderMailboxId:=CStr(vSenderMailboxId), vBrokerABIId:=CStr(vBrokerABIId), vUserLicenceId:=CInt(vUserLicenceId), vPMSourceNumber:=CInt(vPMSourceNumber), vDefaultIndicator:=CStr(vDefaultIndicator), vfsa_CompanyCategory:=CInt(vfsa_CompanyCategory), vfsa_staffwording:=CStr(vfsa_staffwording), vUnderwritingBranchInd:=CInt(vUnderwritingBranchInd), vAllowTempMTA:=CInt(vAllowTempMTA), vAllowPermMTA:=CInt(vAllowPermMTA), vAllowReports:=CInt(vAllowReports), vAllowClaims:=CInt(vAllowClaims), vAllowAccounts:=CInt(vAllowAccounts), vFSABankTypeID:=CInt(vFSABankTypeID)), gPMConstants.PMEReturnCode)
            m_lReturn = CType(SetProperties(oSource, iStatus, vSourceID:=vSourceID, vCode:=vCode, vDescription:=vDescription, vCaptionID:=vCaptionID, vParentID:=vParentID, vBaseCurrency:=vBaseCurrency, vRegNo1:=vRegNo1, vRegNo2:=vRegNo2, vAddress1:=vAddress1, vAddress2:=vAddress2, vAddress3:=vAddress3, vAddress4:=vAddress4, vPostalCode:=vPostalCode, vCountryID:=vCountryID, vPhoneAreaCode:=vPhoneAreaCode, vPhoneNumber:=vPhoneNumber, vPhoneExtension:=vPhoneExtension, vFaxAreaCode:=vFaxAreaCode, vFaxNumber:=vFaxNumber, vFaxExtension:=vFaxExtension, vEmail:=vEmail, vVatNo:=vVatNo, vSenderMailboxId:=vSenderMailboxId, vBrokerABIId:=vBrokerABIId, vUserLicenceId:=vUserLicenceId, vPMSourceNumber:=vPMSourceNumber, vDefaultIndicator:=vDefaultIndicator, vfsa_CompanyCategory:=vfsa_CompanyCategory, vfsa_staffwording:=vfsa_staffwording, vUnderwritingBranchInd:=vUnderwritingBranchInd, vAllowTempMTA:=vAllowTempMTA, vAllowPermMTA:=vAllowPermMTA, vAllowReports:=vAllowReports, vAllowClaims:=vAllowClaims, vAllowAccounts:=vAllowAccounts, vFSABankTypeID:=vFSABankTypeID, vUniqueId:=vUniqueId, vScreenHierarchy:=vScreenHierarchy), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oSource = Nothing
                Return result
            End If

            ' Release reference to Source
            oSource = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete (Public)
    '
    ' Description: Validate that the specified Source can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer, Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim oSource As bPMSource.Source

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oSources.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oSource = m_oSources.Item(lRow)

            ' Check the Status of the Source

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oSource.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oSource.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oSource.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            ' Set is deleted and effective date
            oSource.IsDeleted = gPMConstants.PMEReturnCode.PMTrue
            oSource.EffectiveDate = DateTime.Now
            oSource.UniqueId = sUniqueId
            oSource.ScreenHierarchy = sScreenHierarchy
            ' Release reference to Source
            oSource = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUnDelete (Public)
    '
    ' Description: Validate that the specified Source can be undeleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditUnDelete(ByVal lRow As Integer, Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "") As Integer
        Dim result As Integer = 0
        Dim oSource As bPMSource.Source


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oSources.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oSource = m_oSources.Item(lRow)

            ' Check the Status of the Source

            If oSource.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete Then
                ' If status is PMDummyDelete then record has not been added to the database
                oSource.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd
            Else
                ' Set to PMEdit in case record has been changed
                oSource.DatabaseStatus = gPMConstants.PMEComponentAction.PMEdit
            End If

            ' Set is deleted and effective date
            oSource.IsDeleted = gPMConstants.PMEReturnCode.PMFalse
            oSource.EffectiveDate = DateTime.Now
            oSource.UniqueId = sUniqueId
            oSource.ScreenHierarchy = sScreenHierarchy

            ' Release reference to Source
            oSource = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUnDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUnDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            For lSub As Integer = 1 To m_oSources.Count()
                Select Case m_oSources.Item(lSub).DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing
                    Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMDelete
                        result = gPMConstants.PMEReturnCode.PMDataChanged
                        Exit For
                End Select
            Next lSub

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Cancel", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim oSource As bPMSource.Source
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oSources.Count()
                oSource = m_oSources.Item(lSub)


                Select Case oSource.DatabaseStatus
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
                        m_lReturn = CType(AddItem(oSource), gPMConstants.PMEReturnCode)
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
                        m_lReturn = CType(UpdateItem(oSource), gPMConstants.PMEReturnCode)
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
                        m_lReturn = CType(DeleteItem(oSource), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Release last reference
            oSource = Nothing

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
                    Do While lSub <= m_oSources.Count()

                        ' With the item
                        With m_oSources.Item(lSub)

                            '                    Select Case .DatabaseStatus

                            ' Delete or Dummy Delete
                            '                        Case PMDelete, PMDummyDelete
                            ' Remove from Collection
                            '                            m_oSources.Delete (lSub&)

                            ' Anything Else
                            '                        Case Else
                            ' Set Status to view
                            .DatabaseStatus = gPMConstants.PMEComponentAction.PMView
                            lSub += 1

                            '                    End Select

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



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetPrivilegeLevel
    '
    ' Description:
    '
    ' History: 10/11/1999 DAK - Created.
    '
    ' ***************************************************************** '
    Public Function GetPrivilegeLevel(ByRef r_iPrivilegeLevel As Integer) As Integer
        Dim result As Integer = 0
        Dim lPMProductID As Integer
        ' RDC 13062002 CompServ replaced with BAS module
        'Dim oCS As sPMServerCS.PMServerBusinessCS
        Dim oPMProductLookup As bPMProductLookup.Business


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(GetProductID(lPMProductID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create the User Group Component to do this work.
            '    Set oCS = New sPMServerCS.PMServerBusinessCS
            '    m_lReturn = oCS.CreateBusinessObject( _
            'r_oObject:=oPMProductLookup, _
            'v_sClassName:="bPMProductLookup.Business", _
            'v_sCallingAppName:=g_sCallingAppName, _
            'v_sUserName:=g_sUsername, _
            'v_sPassword:=g_sPassword, _
            'v_iUserID:=g_iUserID, _
            'v_iSourceID:=g_iSourceID, _
            'v_iLanguageId:=g_iLanguageID, _
            'v_iCurrencyID:=g_iCurrencyID, _
            'v_iLogLevel:=g_iLogLevel, _
            'v_oDatabase:=m_oDatabase)

            oPMProductLookup = New bPMProductLookup.Business
            m_lReturn = oPMProductLookup.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            '    Set oCS = Nothing
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oPMProductLookup.GetDetails(vPMProductId:=lPMProductID, vTableName:="Source")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oPMProductLookup.Dispose()
                oPMProductLookup = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oPMProductLookup.GetNext(vPrivilegeLevel:=r_iPrivilegeLevel)

            oPMProductLookup.Dispose()
            oPMProductLookup = Nothing
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPrivilegeLevel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPrivilegeLevel", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetProductID
    '
    ' Description:
    '
    ' History: 11/11/1999 DAK - Created.
    '
    ' ***************************************************************** '
    Public Function GetProductID(ByRef r_lPMProductID As Integer) As Integer
        ' RDC 13062002 CompServ replaced withBAS module
        'Dim oCS As sPMServerCS.PMServerBusinessCS
        Dim result As Integer = 0
        Dim oPMProduct As bPMProduct.Business


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create the User Group Component to do this work.
            '    Set oCS = New sPMServerCS.PMServerBusinessCS
            '    m_lReturn = oCS.CreateBusinessObject( _
            'r_oObject:=oPMProduct, _
            'v_sClassName:="bPMProduct.Business", _
            'v_sCallingAppName:=g_sCallingAppName, _
            'v_sUserName:=g_sUsername, _
            'v_sPassword:=g_sPassword, _
            'v_iUserID:=g_iUserID, _
            'v_iSourceID:=g_iSourceID, _
            'v_iLanguageId:=g_iLanguageID, _
            'v_iCurrencyID:=g_iCurrencyID, _
            'v_iLogLevel:=g_iLogLevel, _
            'v_oDatabase:=m_oDatabase)

            oPMProduct = New bPMProduct.Business
            m_lReturn = oPMProduct.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            '    Set oCS = Nothing
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oPMProduct.GetProductID(v_sProductCode:="Sirius", r_lPMProductID:=r_lPMProductID)

            oPMProduct.Dispose()
            oPMProduct = Nothing
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProductID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProductID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetUserAuthority
    '
    ' Description: Gets the User Groups that the current user is a
    '              Supervisor of.
    '
    ' ***************************************************************** '
    Public Function GetUserAuthority(ByRef r_bIsAdministrator As Boolean, ByRef r_vSupervisedGroups As Object) As Integer

        Dim result As Integer = 0
        Dim oUserGroup As bPMUserGroup.Utilities
        ' RDC 13062002 CompServ replaced with BAS module
        'Dim oCS As sPMServerCS.PMServerBusinessCS

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create the User Group Component to do this work.
            '    Set oCS = New sPMServerCS.PMServerBusinessCS
            '    m_lReturn = oCS.CreateBusinessObject( _
            'r_oObject:=oUserGroup, _
            'v_sClassName:="bPMUserGroup.Utilities", _
            'v_sCallingAppName:=g_sCallingAppName, _
            'v_sUserName:=g_sUsername, _
            'v_sPassword:=g_sPassword, _
            'v_iUserID:=g_iUserID, _
            'v_iSourceID:=g_iSourceID, _
            'v_iLanguageId:=g_iLanguageID, _
            'v_iCurrencyID:=g_iCurrencyID, _
            'v_iLogLevel:=g_iLogLevel, _
            'v_oDatabase:=m_oDatabase)

            oUserGroup = New bPMUserGroup.Utilities
            m_lReturn = oUserGroup.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            '    Set oCS = Nothing
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Is the User an Administrator

            m_lReturn = oUserGroup.IsUserAdministrator(v_iUserID:=m_iUserID, r_bIsAdministrator:=r_bIsAdministrator)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                oUserGroup.Dispose()
                oUserGroup = Nothing
                Return result
            End If

            ' Get the Groups they Supervise

            m_lReturn = oUserGroup.GetGroupsSupervisedByUser(v_iUserID:=m_iUserID, r_vSupervisedGroups:=r_vSupervisedGroups)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                oUserGroup.Dispose()
                oUserGroup = Nothing
                Return result
            End If

            ' Terminate

            oUserGroup.Dispose()
            oUserGroup = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserAuthorityFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserAuthority", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: AddItem (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Private Function AddItem(ByRef oSource As bPMSource.Source) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' CTAF 181200 - Re-arranged parameters for ADO

        ' Add SourceID as an OUTPUT param for an insert
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Source_id", vValue:=CStr(oSource.SourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the required INPUT parameters
        m_lReturn = CType(AddInputParam(oSource:=oSource), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the ID of the record inserted
        oSource.SourceID = m_oDatabase.Parameters.Item("Source_id").Value

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UpdateItem (Public)
    '
    ' Description: UpdateItems a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Private Function UpdateItem(ByRef oSource As bPMSource.Source) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' CTAF 181200 - Re-arranged parameters for ADO

        ' Add SourceID as an INPUT param for an update
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Source_id", vValue:=CStr(oSource.SourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the Input Parameters to the Parameteres collection
        m_lReturn = CType(AddInputParam(oSource:=oSource), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' BB Not required by Orion
        ' Add Timestamp as an INPUT param for an update
        'm_lReturn& = m_oDatabase.Parameters.Add( _
        'sName:="timestamp", _
        'vValue:=oSource.Timestamp, _
        'iDirection:=PMParamInput, _
        'iDataType:=PMBinary)

        'If (m_lReturn& <> PMTrue) Then
        '    UpdateItem = PMFalse
        '    Exit Function
        'End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateSQL, sSQLName:=ACUpdateName, bStoredProcedure:=ACUpdateStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Check to see that the record was updated OK

        If lRecordsAffected > 0 Then
            ' Updated No action required
        Else
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DeleteItem (Public)
    '
    ' Description: Deletes a single record from the database.
    '
    '
    ' ***************************************************************** '
    Private Function DeleteItem(ByRef oSource As bPMSource.Source) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the SourceID INPUT parameter
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Source_id", vValue:=CStr(oSource.SourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="unique_id", vValue:=oSource.UniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="screen_hierarchy", vValue:=oSource.ScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteSQL, sSQLName:=ACDeleteName, bStoredProcedure:=ACDeleteStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' If record wasn't deleted, error
        If lRecordsAffected > 0 Then
            ' Deleted, No action required
        Else
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetPropertiesFromDB (Private)
    '
    ' Description: Sets the supplied Source properties from a database
    '              record.
    ' ***************************************************************** '
    Private Function SetPropertiesFromDB(ByRef oSource As bPMSource.Source, ByRef lRecordNumber As Integer) As Integer
        Dim result As Integer = 0
        'Developer Guide No. 112
        Dim oFields As DataRow
        Dim sDescription As String = ""




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set oFields to refer to Field Collection for Record 1
        oFields = m_oDatabase.Records.Item(lRecordNumber).Fields()

        ' Populate Base Details

        With oSource

            .SourceID = oFields("Source_id")
            .Code = oFields("code")

            If Convert.IsDBNull(oFields("caption_id")) Or Informations.IsNothing(oFields("caption_id")) Then
                .CaptionID = 0
            Else
                .CaptionID = oFields("caption_id")
            End If

            m_lReturn = m_oCaption.GetCaptionDesc(v_lCaptionId:= .CaptionID, r_sCaption:=sDescription)
            .Description = sDescription

            If Convert.IsDBNull(oFields("parent_id")) Or Informations.IsNothing(oFields("parent_id")) Then
                .ParentID = 0
            Else
                .ParentID = oFields("parent_id")
            End If
            .IsDeleted = oFields("is_deleted")
            .EffectiveDate = oFields("effective_date")

            'developers guide no. 24
            .BaseCurrency = oFields.Item("base_currency")

            If Convert.IsDBNull(oFields("reg_no_1")) Or Informations.IsNothing(oFields("reg_no_1")) Then
                .RegNo1 = ""
            Else
                .RegNo1 = oFields.Item("reg_no_1")
            End If

            If Convert.IsDBNull(oFields("reg_no_2")) Or Informations.IsNothing(oFields("reg_no_2")) Then
                .RegNo2 = ""
            Else
                .RegNo2 = oFields("reg_no_2")
            End If

            If Convert.IsDBNull(oFields("address1")) Or Informations.IsNothing(oFields("address1")) Then
                .Address1 = ""
            Else
                .Address1 = oFields("address1")
            End If

            If Convert.IsDBNull(oFields("address2")) Or Informations.IsNothing(oFields("address2")) Then
                .Address2 = ""
            Else
                .Address2 = oFields("address2")
            End If

            If Convert.IsDBNull(oFields("address3")) Or Informations.IsNothing(oFields("address3")) Then
                .Address3 = ""
            Else
                .Address3 = oFields("address3")
            End If

            If Convert.IsDBNull(oFields("address4")) Or Informations.IsNothing(oFields("address4")) Then
                .Address4 = ""
            Else
                .Address4 = oFields("address4")
            End If

            If Convert.IsDBNull(oFields("postal_code")) Or Informations.IsNothing(oFields("postal_code")) Then
                .PostalCode = ""
            Else
                .PostalCode = oFields("postal_code")
            End If

            If Convert.IsDBNull(oFields("country_id")) Or Informations.IsNothing(oFields("country_id")) Then
                .CountryID = 0
            Else
                .CountryID = oFields("country_id")
            End If

            If Convert.IsDBNull(oFields("phone_area_code")) Or Informations.IsNothing(oFields("phone_area_code")) Then
                .PhoneAreaCode = ""
            Else
                .PhoneAreaCode = oFields("phone_area_code")
            End If

            If Convert.IsDBNull(oFields("phone_number")) Or Informations.IsNothing(oFields("phone_number")) Then
                .PhoneNumber = ""
            Else
                .PhoneNumber = oFields("phone_number")
            End If

            If Convert.IsDBNull(oFields("phone_extension")) Or Informations.IsNothing(oFields("phone_extension")) Then
                .PhoneExtension = ""
            Else
                .PhoneExtension = oFields("phone_extension")
            End If

            If Convert.IsDBNull(oFields("fax_area_code")) Or Informations.IsNothing(oFields("fax_area_code")) Then
                .FaxAreaCode = ""
            Else
                .FaxAreaCode = oFields("fax_area_code")
            End If

            If Convert.IsDBNull(oFields("fax_number")) Or Informations.IsNothing(oFields("fax_number")) Then
                .FaxNumber = ""
            Else
                .FaxNumber = oFields("fax_number")
            End If

            If Convert.IsDBNull(oFields("fax_extension")) Or Informations.IsNothing(oFields("fax_extension")) Then
                .FaxExtension = ""
            Else
                .FaxExtension = oFields("fax_extension")
            End If
            ' DC 31/01/00

            If Convert.IsDBNull(oFields("email")) Or Informations.IsNothing(oFields("email")) Then
                .Email = ""
            Else
                .Email = oFields("email")
            End If
            ' DC 31/01/00

            If Convert.IsDBNull(oFields("vat_no")) Or Informations.IsNothing(oFields("vat_no")) Then
                .VatNo = ""
            Else
                .VatNo = oFields("vat_no")
            End If
            ' DC 31/01/00

            If Convert.IsDBNull(oFields("sender_mailbox_id")) Or Informations.IsNothing(oFields("sender_mailbox_id")) Then
                .SenderMailboxId = ""
            Else
                .SenderMailboxId = oFields("sender_mailbox_id")
            End If
            ' DC 31/01/00

            If Convert.IsDBNull(oFields("broker_abi_id")) Or Informations.IsNothing(oFields("broker_abi_id")) Then
                .BrokerABIId = ""
            Else
                .BrokerABIId = oFields("broker_abi_id")
            End If
            ' DC 31/01/00

            If Convert.IsDBNull(oFields("user_licence_id")) Or Informations.IsNothing(oFields("user_licence_id")) Or Not True Then
                .UserLicenceId = 0
            Else
                .UserLicenceId = oFields("user_licence_id")
            End If
            ' DC 31/01/00

            If Convert.IsDBNull(oFields("pm_company_number")) Or Informations.IsNothing(oFields("pm_company_number")) Or Not True Then
                .PMSourceNumber = 0
            Else
                .PMSourceNumber = oFields("pm_company_number")
            End If
            ' DC 31/01/00

            If Convert.IsDBNull(oFields("default_indicator")) Or Informations.IsNothing(oFields("default_indicator")) Then
                .DefaultIndicator = ""
            Else
                .DefaultIndicator = oFields("default_indicator")
            End If


            If Convert.IsDBNull(oFields("FSA_CompanyCategory_id")) Or Informations.IsNothing(oFields("FSA_CompanyCategory_id")) Then
                .FSA_CompanyCategory_ID = 0
            Else
                .FSA_CompanyCategory_ID = oFields("FSA_CompanyCategory_id")
            End If


            If Convert.IsDBNull(oFields("FSA_StaffWording")) Or Informations.IsNothing(oFields("FSA_StaffWording")) Then
                .FSA_StaffWording = ""
            Else
                .FSA_StaffWording = oFields("FSA_StaffWording")
            End If


            If Convert.IsDBNull(oFields("underwriting_branch_ind")) Or Informations.IsNothing(oFields("underwriting_branch_ind")) Then
                .UnderwritingBranchInd = 0
            Else
                .UnderwritingBranchInd = oFields("underwriting_branch_ind")
            End If


            If Convert.IsDBNull(oFields("closed_allow_temp_mta")) Or Informations.IsNothing(oFields("closed_allow_temp_mta")) Then
                .AllowTempMTA = 0
            Else
                .AllowTempMTA = oFields("closed_allow_temp_mta")
            End If

            If Convert.IsDBNull(oFields("closed_allow_perm_mta")) Or Informations.IsNothing(oFields("closed_allow_perm_mta")) Then
                .AllowPermMTA = 0
            Else
                .AllowPermMTA = oFields("closed_allow_perm_mta")
            End If

            If Convert.IsDBNull(oFields("closed_allow_reports")) Or Informations.IsNothing(oFields("closed_allow_reports")) Then
                .AllowReports = 0
            Else
                .AllowReports = oFields("closed_allow_reports")
            End If

            If Convert.IsDBNull(oFields("closed_allow_claims")) Or Informations.IsNothing(oFields("closed_allow_claims")) Then
                .AllowClaims = 0
            Else
                .AllowClaims = oFields("closed_allow_claims")
            End If

            If Convert.IsDBNull(oFields("closed_allow_accounts")) Or Informations.IsNothing(oFields("closed_allow_accounts")) Then
                .AllowAccounts = 0
            Else
                .AllowAccounts = oFields("closed_allow_accounts")
            End If
            'FSA Phase III

            If Convert.IsDBNull(oFields("FSA_BankType_id")) Or Informations.IsNothing(oFields("FSA_BankType_id")) Then
                .FSA_BankType_ID = 0
            Else
                .FSA_BankType_ID = oFields("FSA_BankType_id")
            End If

            .DatabaseStatus = gPMConstants.PMEComponentAction.PMView

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Private)
    '
    ' Description: Sets the supplied Source property values.
    '
    ' ***************************************************************** '
    ' DC 31/01/00
    ' added email, vat no, sender mailbox id, broker abi id, user licence id,
    '       pm Source number and default indicator
    'MKW010903 PS255 - FSA Compliance Added vfsa_companycategory and vsa_staffwording
    'FSA Phase III BankType
    'Developer Guide No. 101
    Private Function SetProperties(ByVal oSource As bPMSource.Source, ByVal iStatus As Integer, Optional ByVal vSourceID As Object = Nothing, Optional ByVal vCode As Object = Nothing, Optional ByVal vDescription As Object = Nothing, Optional ByVal vCaptionID As Object = Nothing, Optional ByVal vParentID As Object = Nothing, Optional ByVal vBaseCurrency As Object = Nothing, Optional ByVal vRegNo1 As Object = Nothing, Optional ByVal vRegNo2 As Object = Nothing, Optional ByVal vAddress1 As Object = Nothing, Optional ByVal vAddress2 As Object = Nothing, Optional ByVal vAddress3 As Object = Nothing, Optional ByVal vAddress4 As Object = Nothing, Optional ByVal vPostalCode As Object = Nothing, Optional ByVal vCountryID As Object = Nothing, Optional ByVal vPhoneAreaCode As Object = Nothing, Optional ByVal vPhoneNumber As Object = Nothing, Optional ByVal vPhoneExtension As Object = Nothing, Optional ByVal vFaxAreaCode As Object = Nothing, Optional ByVal vFaxNumber As Object = Nothing, Optional ByVal vFaxExtension As Object = Nothing, Optional ByVal vEmail As Object = Nothing, Optional ByVal vVatNo As Object = Nothing, Optional ByVal vSenderMailboxId As Object = Nothing, Optional ByVal vBrokerABIId As Object = Nothing, Optional ByVal vUserLicenceId As Object = Nothing, Optional ByVal vPMSourceNumber As Object = Nothing, Optional ByVal vDefaultIndicator As Object = Nothing, Optional ByVal vfsa_CompanyCategory As Object = Nothing, Optional ByVal vfsa_staffwording As Object = Nothing, Optional ByVal vUnderwritingBranchInd As Object = Nothing, Optional ByVal vAllowTempMTA As Object = Nothing, Optional ByVal vAllowPermMTA As Object = Nothing, Optional ByVal vAllowReports As Object = Nothing, Optional ByVal vAllowClaims As Object = Nothing, Optional ByVal vAllowAccounts As Object = Nothing, Optional ByVal vFSABankTypeID As Object = Nothing, Optional ByVal vUniqueId As String = "", Optional ByVal vScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean
        Dim sDescription As String = ""
        Dim lCaptionID As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' If Add Mode
        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

            ' Check that All Mandatory Parameters have been supplied
            ' DC 31/01/00
            ' added email, vat no, sender mailbox id, broker abi id, user licence id,
            '       pm Source number and default indicator
            'MKW010903 PS255 - FSA Compliance Added vfsa_companycategory and vsa_staffwording
            m_lReturn = CType(CheckMandatory(vSourceID:=vSourceID, vCode:=vCode, vDescription:=vDescription, vCaptionID:=vCaptionID, vParentID:=vParentID, vBaseCurrency:=vBaseCurrency, vRegNo1:=vRegNo1, vRegNo2:=vRegNo2, vAddress1:=vAddress1, vAddress2:=vAddress2, vAddress3:=vAddress3, vAddress4:=vAddress4, vPostalCode:=vPostalCode, vCountryID:=vCountryID, vPhoneAreaCode:=vPhoneAreaCode, vPhoneNumber:=vPhoneNumber, vPhoneExtension:=vPhoneExtension, vFaxAreaCode:=vFaxAreaCode, vFaxNumber:=vFaxNumber, vFaxExtension:=vFaxExtension, vEmail:=vEmail, vVatNo:=vVatNo, vSenderMailboxId:=vSenderMailboxId, vBrokerABIId:=vBrokerABIId, vUserLicenceId:=vUserLicenceId, vPMSourceNumber:=vPMSourceNumber, vDefaultIndicator:=vDefaultIndicator, vfsa_CompanyCategory:=vfsa_CompanyCategory, vfsa_staffwording:=vfsa_staffwording, vUnderwritingBranchInd:=vUnderwritingBranchInd), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Default Any Missing Parameters
            ' DC 31/01/00
            ' added email, vat no, sender mailbox id, broker abi id, user licence id,
            '       pm Source number and default indicator
            'MKW010903 PS255 - FSA Compliance Added vfsa_companycategory and vsa_staffwording


            'Developer Guide No. 98
            m_lReturn = DefaultParameters(bDefaultAll:=False, vSourceID:=vSourceID, vCode:=vCode, vDescription:=vDescription, vCaptionID:=vCaptionID, vParentID:=vParentID, vBaseCurrency:=vBaseCurrency, vRegNo1:=vRegNo1, vRegNo2:=vRegNo2, vAddress1:=vAddress1, vAddress2:=vAddress2, vAddress3:=vAddress3, vAddress4:=vAddress4, vPostalCode:=vPostalCode, vCountryID:=vCountryID, vPhoneAreaCode:=vPhoneAreaCode, vPhoneNumber:=vPhoneNumber, vPhoneExtension:=vPhoneExtension, vFaxAreaCode:=vFaxAreaCode, vFaxNumber:=vFaxNumber, vFaxExtension:=vFaxExtension, vEmail:=vEmail, vVatNo:=vVatNo, vSenderMailboxId:=vSenderMailboxId, vBrokerABIId:=vBrokerABIId, vUserLicenceId:=vUserLicenceId, vPMSourceNumber:=vPMSourceNumber, vDefaultIndicator:=vDefaultIndicator, vfsa_CompanyCategory:=vfsa_CompanyCategory, vfsa_staffwording:=vfsa_staffwording, vUnderwritingBranchInd:=vUnderwritingBranchInd)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

        End If

        ' Validate Parameters
        ' DC 31/01/00
        ' added email, vat no, sender mailbox id, broker abi id, user licence id,
        '       pm Source number and default indicator
        'MKW010903 PS255 - FSA Compliance Added vfsa_companycategory and vsa_staffwording
        m_lReturn = CType(Validate(vSourceID:=vSourceID, vCode:=vCode, vDescription:=vDescription, vCaptionID:=vCaptionID, vParentID:=vParentID, vBaseCurrency:=vBaseCurrency, vRegNo1:=vRegNo1, vRegNo2:=vRegNo2, vAddress1:=vAddress1, vAddress2:=vAddress2, vAddress3:=vAddress3, vAddress4:=vAddress4, vPostalCode:=vPostalCode, vCountryID:=vCountryID, vPhoneAreaCode:=vPhoneAreaCode, vPhoneNumber:=vPhoneNumber, vPhoneExtension:=vPhoneExtension, vFaxAreaCode:=vFaxAreaCode, vFaxNumber:=vFaxNumber, vFaxExtension:=vFaxExtension, vEmail:=vEmail, vVatNo:=vVatNo, vSenderMailboxId:=vSenderMailboxId, vBrokerABIId:=vBrokerABIId, vUserLicenceId:=vUserLicenceId, vPMSourceNumber:=vPMSourceNumber, vDefaultIndicator:=vDefaultIndicator, vfsa_CompanyCategory:=vfsa_CompanyCategory, vfsa_staffwording:=vfsa_staffwording, vUnderwritingBranchInd:=vUnderwritingBranchInd), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        ' Set Data Changed Flag to False
        bDataChanged = False

        'Quick, let's go get the caption id

        If Not Informations.IsNothing(vDescription) Then
            sDescription = vDescription.Trim()


            m_lReturn = m_oCaption.GetCaptionID(v_sCaption:=sDescription, r_lCaptionId:=lCaptionID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        ' Set Property values.
        With oSource
            'MKW010903 PS255 - FSA Compliance START

            'Developer Guide No. check required for dbnull too.
            If Not Informations.IsNothing(vfsa_CompanyCategory) And Not Informations.IsDBNull(vfsa_CompanyCategory) Then
                If .FSA_CompanyCategory_ID <> vfsa_CompanyCategory Then
                    .FSA_CompanyCategory_ID = vfsa_CompanyCategory
                    bDataChanged = True
                End If
            End If


            'If Not Informations.IsNothing(vfsa_staffwording) Then
            If Not Informations.IsNothing(vfsa_staffwording) And Not Informations.IsDBNull(vfsa_staffwording) Then
                If .FSA_StaffWording <> vfsa_staffwording Then
                    .FSA_StaffWording = vfsa_staffwording
                    bDataChanged = True
                End If
            End If
            'MKW010903 PS255 - FSA Compliance END
            'FSA Phase III

            'If Not Informations.IsNothing(vFSABankTypeID) Then
            If Not Informations.IsNothing(vFSABankTypeID) And Not Informations.IsDBNull(vFSABankTypeID) Then
                If .FSA_BankType_ID <> vFSABankTypeID Then
                    .FSA_BankType_ID = vFSABankTypeID
                    bDataChanged = True
                End If
            End If
            'FSA Phase IIIEnd



            'If Not Informations.IsNothing(vSourceID) Then
            If Not Informations.IsNothing(vSourceID) And Not Informations.IsDBNull(vSourceID) Then
                If .SourceID <> vSourceID Then
                    .SourceID = vSourceID
                    bDataChanged = True
                End If
            End If


            'If Not Informations.IsNothing(vCode) Then
            If Not Informations.IsNothing(vCode) And Not Informations.IsDBNull(vCode) Then
                If .Code.Trim() <> vCode.Trim() Then
                    .Code = vCode
                    bDataChanged = True
                End If
            End If


            'If Not Informations.IsNothing(vDescription) Then
            If Not Informations.IsNothing(vDescription) And Not Informations.IsDBNull(vDescription) Then
                If .Description.Trim() <> vDescription.Trim() Then
                    .Description = vDescription
                    .CaptionID = lCaptionID
                    bDataChanged = True
                End If
            End If


            'If Not Informations.IsNothing(vParentID) Then
            If Not Informations.IsNothing(vParentID) And Not Informations.IsDBNull(vParentID) Then
                If .ParentID <> vParentID Then
                    .ParentID = vParentID
                    bDataChanged = True
                End If
            End If


            'If Not Informations.IsNothing(vBaseCurrency) Then
            If Not Informations.IsNothing(vBaseCurrency) Then



                'Developer Guide No.44
                If Convert.IsDBNull(.BaseCurrency) OrElse Informations.IsNothing(.BaseCurrency) OrElse Not .BaseCurrency.Equals(vBaseCurrency) Then



                    'Developer Guide No. 22
                    .BaseCurrency = vBaseCurrency
                    bDataChanged = True

                End If
            End If


            'If Not Informations.IsNothing(vRegNo1) Then
            If Not Informations.IsNothing(vRegNo1) And Not Informations.IsDBNull(vRegNo1) Then
                If .RegNo1.Trim() <> vRegNo1.Trim() Then
                    .RegNo1 = vRegNo1
                    bDataChanged = True
                End If
            End If


            'If Not Informations.IsNothing(vRegNo2) Then
            If Not Informations.IsNothing(vRegNo2) And Not Informations.IsDBNull(vRegNo2) Then
                If .RegNo2.Trim() <> vRegNo2.Trim() Then
                    .RegNo2 = vRegNo2
                    bDataChanged = True
                End If
            End If


            'If Not Informations.IsNothing(vAddress1) Then
            If Not Informations.IsNothing(vAddress1) And Not Informations.IsDBNull(vAddress1) Then
                If .Address1.Trim() <> vAddress1.Trim() Then
                    .Address1 = vAddress1
                    bDataChanged = True
                End If
            End If


            'If Not Informations.IsNothing(vAddress2) Then
            If Not Informations.IsNothing(vAddress2) And Not Informations.IsDBNull(vAddress2) Then
                If .Address2.Trim() <> vAddress2.Trim() Then
                    .Address2 = vAddress2
                    bDataChanged = True
                End If
            End If


            'If Not Informations.IsNothing(vAddress3) Then
            If Not Informations.IsNothing(vAddress3) And Not Informations.IsDBNull(vAddress3) Then
                If .Address3.Trim() <> vAddress3.Trim() Then
                    .Address3 = vAddress3
                    bDataChanged = True
                End If
            End If


            'If Not Informations.IsNothing(vAddress4) Then
            If Not Informations.IsNothing(vAddress4) And Not Informations.IsDBNull(vAddress4) Then
                If .Address4.Trim() <> vAddress4.Trim() Then
                    .Address4 = vAddress4
                    bDataChanged = True
                End If
            End If


            'If Not Informations.IsNothing(vPostalCode) Then
            If Not Informations.IsNothing(vPostalCode) And Not Informations.IsDBNull(vPostalCode) Then
                If .PostalCode.Trim() <> vPostalCode.Trim() Then
                    .PostalCode = vPostalCode
                    bDataChanged = True
                End If
            End If


            'If Not Informations.IsNothing(vCountryID) Then
            If Not Informations.IsNothing(vCountryID) And Not Informations.IsDBNull(vCountryID) Then
                If .CountryID <> vCountryID Then
                    .CountryID = vCountryID
                    bDataChanged = True
                End If
            End If


            'If Not Informations.IsNothing(vPhoneAreaCode) Then
            If Not Informations.IsNothing(vPhoneAreaCode) And Not Informations.IsDBNull(vPhoneAreaCode) Then
                If .PhoneAreaCode.Trim() <> vPhoneAreaCode.Trim() Then
                    .PhoneAreaCode = vPhoneAreaCode
                    bDataChanged = True
                End If
            End If


            'If Not Informations.IsNothing(vPhoneNumber) Then
            If Not Informations.IsNothing(vPhoneNumber) And Not Informations.IsDBNull(vPhoneNumber) Then
                If .PhoneNumber.Trim() <> vPhoneNumber.Trim() Then
                    .PhoneNumber = vPhoneNumber
                    bDataChanged = True
                End If
            End If


            'If Not Informations.IsNothing(vPhoneExtension) Then
            If Not Informations.IsNothing(vPhoneExtension) And Not Informations.IsDBNull(vPhoneExtension) Then
                If .PhoneExtension.Trim() <> vPhoneExtension.Trim() Then
                    .PhoneExtension = vPhoneExtension
                    bDataChanged = True
                End If
            End If


            'If Not Informations.IsNothing(vFaxAreaCode) Then
            If Not Informations.IsNothing(vFaxAreaCode) And Not Informations.IsDBNull(vFaxAreaCode) Then
                If .FaxAreaCode.Trim() <> vFaxAreaCode.Trim() Then
                    .FaxAreaCode = vFaxAreaCode
                    bDataChanged = True
                End If
            End If


            'If Not Informations.IsNothing(vFaxNumber) Then
            If Not Informations.IsNothing(vFaxNumber) And Not Informations.IsDBNull(vFaxNumber) Then
                If .FaxNumber.Trim() <> vFaxNumber.Trim() Then
                    .FaxNumber = vFaxNumber
                    bDataChanged = True
                End If
            End If


            'If Not Informations.IsNothing(vFaxExtension) Then
            If Not Informations.IsNothing(vFaxExtension) And Not Informations.IsDBNull(vFaxExtension) Then
                If .FaxExtension.Trim() <> vFaxExtension.Trim() Then
                    .FaxExtension = vFaxExtension
                    bDataChanged = True
                End If
            End If

            ' DC 31/01/00

            'If Not Informations.IsNothing(vEmail) Then
            If Not Informations.IsNothing(vEmail) And Not Informations.IsDBNull(vEmail) Then
                If .Email.Trim() <> vEmail.Trim() Then
                    .Email = vEmail
                    bDataChanged = True
                End If
            End If

            ' DC 31/01/00

            'If Not Informations.IsNothing(vVatNo) Then
            If Not Informations.IsNothing(vVatNo) And Not Informations.IsDBNull(vVatNo) Then
                If .VatNo.Trim() <> vVatNo.Trim() Then
                    .VatNo = vVatNo
                    bDataChanged = True
                End If
            End If

            ' DC 31/01/00

            'If Not Informations.IsNothing(vSenderMailboxId) Then
            If Not Informations.IsNothing(vSenderMailboxId) And Not Informations.IsDBNull(vSenderMailboxId) Then
                If .SenderMailboxId.Trim() <> vSenderMailboxId.Trim() Then
                    .SenderMailboxId = vSenderMailboxId
                    bDataChanged = True
                End If
            End If

            ' DC 31/01/00

            'If Not Informations.IsNothing(vBrokerABIId) Then
            If Not Informations.IsNothing(vBrokerABIId) And Not Informations.IsDBNull(vBrokerABIId) Then
                If .BrokerABIId.Trim() <> vBrokerABIId.Trim() Then
                    .BrokerABIId = vBrokerABIId
                    bDataChanged = True
                End If
            End If

            ' DC 31/01/00

            'If Not Informations.IsNothing(vUserLicenceId) Then
            If Not Informations.IsNothing(vUserLicenceId) And Not Informations.IsDBNull(vUserLicenceId) Then
                If .UserLicenceId <> vUserLicenceId Then
                    .UserLicenceId = vUserLicenceId
                    bDataChanged = True
                End If
            End If

            ' DC 31/01/00

            'If Not Informations.IsNothing(vPMSourceNumber) Then
            If Not Informations.IsNothing(vPMSourceNumber) And Not Informations.IsDBNull(vPMSourceNumber) Then
                If .PMSourceNumber <> vPMSourceNumber Then
                    .PMSourceNumber = vPMSourceNumber
                    bDataChanged = True
                End If
            End If

            ' DC 31/01/00

            'If Not Informations.IsNothing(vDefaultIndicator) Then
            If Not Informations.IsNothing(vDefaultIndicator) And Not Informations.IsDBNull(vDefaultIndicator) Then
                If .DefaultIndicator.Trim() <> vDefaultIndicator.Trim() Then
                    .DefaultIndicator = vDefaultIndicator
                    bDataChanged = True
                End If
            End If

            'SJ 17/02/2004 - start

            'If Not Informations.IsNothing(vUnderwritingBranchInd) Then
            If Not Informations.IsNothing(vUnderwritingBranchInd) And Not Informations.IsDBNull(vUnderwritingBranchInd) Then
                If CStr(.UnderwritingBranchInd).Trim() <> CStr(vUnderwritingBranchInd).Trim() Then
                    .UnderwritingBranchInd = vUnderwritingBranchInd
                    bDataChanged = True
                End If
            End If
            'SJ 17/02/2004 - end


            'If Not Informations.IsNothing(vAllowTempMTA) Then
            If Not Informations.IsNothing(vAllowTempMTA) And Not Informations.IsDBNull(vAllowTempMTA) Then
                If CStr(.AllowTempMTA).Trim() <> CStr(vAllowTempMTA).Trim() Then
                    .AllowTempMTA = vAllowTempMTA
                    bDataChanged = True
                End If
            End If

            'If Not Informations.IsNothing(vAllowPermMTA) Then
            If Not Informations.IsNothing(vAllowPermMTA) And Not Informations.IsDBNull(vAllowPermMTA) Then
                If CStr(.AllowPermMTA).Trim() <> CStr(vAllowPermMTA).Trim() Then
                    .AllowPermMTA = vAllowPermMTA
                    bDataChanged = True
                End If
            End If

            'If Not Informations.IsNothing(vAllowReports) Then
            If Not Informations.IsNothing(vAllowReports) And Not Informations.IsDBNull(vAllowReports) Then
                If CStr(.AllowReports).Trim() <> CStr(vAllowReports).Trim() Then
                    .AllowReports = vAllowReports
                    bDataChanged = True
                End If
            End If

            'If Not Informations.IsNothing(vAllowClaims) Then
            If Not Informations.IsNothing(vAllowClaims) And Not Informations.IsDBNull(vAllowClaims) Then
                If CStr(.AllowClaims).Trim() <> CStr(vAllowClaims).Trim() Then
                    .AllowClaims = vAllowClaims
                    bDataChanged = True
                End If
            End If

            'If Not Informations.IsNothing(vAllowAccounts) Then
            If Not Informations.IsNothing(vAllowAccounts) And Not Informations.IsDBNull(vAllowAccounts) Then
                If CStr(.AllowAccounts).Trim() <> CStr(vAllowAccounts).Trim() Then
                    .AllowAccounts = vAllowAccounts
                    bDataChanged = True
                End If
            End If

            If Not String.IsNullOrEmpty(vUniqueId) Then
                .UniqueId = vUniqueId
                .ScreenHierarchy = vScreenHierarchy
            End If
            'end
            ' If we have changed one of the properties, update the status
            If bDataChanged Then
                .DatabaseStatus = iStatus
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetProperties (Private)
    '
    ' Description: Returns the supplied Source property values.
    '
    ' ***************************************************************** '
    ' DC 31/01/00
    ' added email, vat no, sender mailbox id, broker abi id, user licence id,
    '       pm Source number and default indicator
    'MKW010903 PS255 - FSA Compliance Added vfsa_companycategory and vsa_staffwording
    'FSA Phase III
    'Developer Guide No. 33
    Private Function GetProperties(ByRef oSource As bPMSource.Source, ByRef iStatus As Integer, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vCode As Object = Nothing,
                                   Optional ByRef vDescription As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vParentID As Object = Nothing,
                                   Optional ByRef vBaseCurrency As Object = Nothing, Optional ByRef vRegNo1 As Object = Nothing, Optional ByRef vRegNo2 As Object = Nothing,
                                   Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing,
                                   Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vCountryID As Object = Nothing,
                                   Optional ByRef vPhoneAreaCode As Object = Nothing, Optional ByRef vPhoneNumber As Object = Nothing, Optional ByRef vPhoneExtension As Object = Nothing,
                                   Optional ByRef vFaxAreaCode As Object = Nothing, Optional ByRef vFaxNumber As Object = Nothing, Optional ByRef vFaxExtension As Object = Nothing,
                                   Optional ByRef vEmail As Object = Nothing, Optional ByRef vVatNo As Object = Nothing, Optional ByRef vSenderMailboxId As Object = Nothing,
                                   Optional ByRef vBrokerABIId As Object = Nothing, Optional ByRef vUserLicenceId As Object = Nothing, Optional ByRef vPMSourceNumber As Object = Nothing,
                                   Optional ByRef vDefaultIndicator As Object = Nothing, Optional ByRef vIsDeleted As Integer = 0, Optional ByRef vEffectiveDate As Object = Nothing,
                                   Optional ByRef vfsa_CompanyCategory As Object = Nothing, Optional ByRef vfsa_staffwording As Object = Nothing, Optional ByRef vUnderwritingBranchInd As Object = Nothing,
                                   Optional ByRef vAllowTempMTA As Object = Nothing, Optional ByRef vAllowPermMTA As Object = Nothing, Optional ByRef vAllowReports As Object = Nothing,
                                   Optional ByRef vAllowClaims As Object = Nothing, Optional ByRef vAllowAccounts As Object = Nothing, Optional ByRef vFSABankTypeID As Object = Nothing,
                                   Optional ByRef vUniqueId As String = "", Optional ByRef vScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set Property values.
        With oSource


            'Developer Guide No 143. 
            'FSA Phase III End
            vAllowTempMTA = .AllowTempMTA
            vAllowPermMTA = .AllowPermMTA
            vAllowReports = .AllowReports
            vAllowClaims = .AllowClaims
            vAllowAccounts = .AllowAccounts
            'MKW010903 PS255 - FSA Compliance START
            vfsa_CompanyCategory = .FSA_CompanyCategory_ID
            vfsa_staffwording = .FSA_StaffWording
            'MKW010903 PS255 - FSA Compliance END
            vSourceID = .SourceID
            vCode = .Code
            vDescription = .Description
            vCaptionID = .CaptionID
            vParentID = .ParentID
            vBaseCurrency = .BaseCurrency
            vRegNo1 = .RegNo1
            vRegNo2 = .RegNo2
            vAddress1 = .Address1
            vAddress2 = .Address2
            vAddress3 = .Address3
            vAddress4 = .Address4
            vPostalCode = .PostalCode
            vCountryID = .CountryID
            vPhoneAreaCode = .PhoneAreaCode
            vPhoneNumber = .PhoneNumber
            vPhoneExtension = .PhoneExtension
            vFaxAreaCode = .FaxAreaCode
            vFaxNumber = .FaxNumber
            vFaxExtension = .FaxExtension
            ' DC 31/01/00
            vEmail = .Email
            ' DC 31/01/00
            vVatNo = .VatNo
            ' DC 31/01/00
            vSenderMailboxId = .SenderMailboxId
            ' DC 31/01/00
            vBrokerABIId = .BrokerABIId
            ' DC 31/01/00
            vUserLicenceId = .UserLicenceId
            ' DC 31/01/00
            vPMSourceNumber = .PMSourceNumber
            ' DC 31/01/00
            vDefaultIndicator = .DefaultIndicator
            vIsDeleted = .IsDeleted
            vEffectiveDate = .EffectiveDate
            'SJ 17/02/2004 - start
            vUnderwritingBranchInd = .UnderwritingBranchInd
            'SJ 17/02/2004 - end        
            'FSA Phase III
            vFSABankTypeID = .FSA_BankType_ID
            iStatus = .DatabaseStatus
            vUniqueId = .UniqueId
            vScreenHierarchy = .ScreenHierarchy
        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddInputParam (Private)
    '
    ' Description: Adds all of the INPUT parameters
    '              required for an Insert or Update.
    '
    ' ***************************************************************** '
    Private Function AddInputParam(ByRef oSource As bPMSource.Source) As Integer
        Dim result As Integer = 0
        Dim sCaption As String = ""
        Dim lCaptionID As Integer




        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="code", vValue:=oSource.Code, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="description", vValue:=oSource.Description, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sCaption = oSource.Description


            m_lReturn = m_oCaption.GetCaptionID(v_sCaption:=sCaption, r_lCaptionId:=lCaptionID)

            m_lReturn = .Parameters.Add(sName:="caption_id", vValue:=CStr(lCaptionID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If oSource.ParentID < 1 Then

                'Developer Guide No. 85
                m_lReturn = .Parameters.Add(sName:="parent_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="parent_id", vValue:=CStr(oSource.ParentID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="base_currency", vValue:=oSource.BaseCurrency, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="reg_no_1", vValue:=oSource.RegNo1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="reg_no_2", vValue:=oSource.RegNo2, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="address1", vValue:=oSource.Address1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="address2", vValue:=oSource.Address2, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="address3", vValue:=oSource.Address3, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="address4", vValue:=oSource.Address4, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="postal_code", vValue:=oSource.PostalCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oSource.CountryID < 1 Then

                'Developer Guide No. 85
                m_lReturn = .Parameters.Add(sName:="country_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="country_id", vValue:=CStr(oSource.CountryID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="phone_area_code", vValue:=oSource.PhoneAreaCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="phone_number", vValue:=oSource.PhoneNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="phone_extension", vValue:=oSource.PhoneExtension, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="fax_area_code", vValue:=oSource.FaxAreaCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="fax_number", vValue:=oSource.FaxNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="fax_extension", vValue:=oSource.FaxExtension, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' DC 31/01/00
            m_lReturn = .Parameters.Add(sName:="email", vValue:=oSource.Email, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' DC 31/01/00
            m_lReturn = .Parameters.Add(sName:="vat_no", vValue:=oSource.VatNo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' DC 31/01/00
            m_lReturn = .Parameters.Add(sName:="sender_mailbox_id", vValue:=oSource.SenderMailboxId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' DC 31/01/00
            m_lReturn = .Parameters.Add(sName:="broker_abi_id", vValue:=oSource.BrokerABIId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' DC 31/01/00
            If oSource.UserLicenceId < 1 Then

                'Developer Guide No. 85
                m_lReturn = .Parameters.Add(sName:="user_licence_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="user_licence_id", vValue:=CStr(oSource.UserLicenceId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oSource.PMSourceNumber < 1 Then

                'Developer Guide No. 85
                m_lReturn = .Parameters.Add(sName:="pm_company_number", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="pm_company_number", vValue:=CStr(oSource.PMSourceNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="default_indicator", vValue:=oSource.DefaultIndicator, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="is_deleted", vValue:=CStr(oSource.IsDeleted), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Developer Guide No. 40
            m_lReturn = .Parameters.Add(sName:="effective_date", vValue:=oSource.EffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'MKW010903 PS255 - FSA Compliance START Added vfsa_companycategory and vsa_staffwording
            If oSource.FSA_CompanyCategory_ID < 1 Then

                'Developer Guide No. 85
                m_lReturn = .Parameters.Add(sName:="FSA_CompanyCategory_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="FSA_CompanyCategory_id", vValue:=CStr(oSource.FSA_CompanyCategory_ID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="FSA_StaffWording", vValue:=oSource.FSA_StaffWording, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'MKW010903 PS255 - FSA Compliance END Added vfsa_companycategory and vsa_staffwording

            'SJ 17/02/2004 - start
            m_lReturn = .Parameters.Add(sName:="underwriting_branch_ind", vValue:=CStr(oSource.UnderwritingBranchInd), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'SJ 17/02/2004 - end

            m_lReturn = .Parameters.Add(sName:="closed_allow_temp_mta", vValue:=CStr(oSource.AllowTempMTA), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="closed_allow_perm_mta", vValue:=CStr(oSource.AllowPermMTA), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="closed_allow_reports", vValue:=CStr(oSource.AllowReports), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="closed_allow_claims", vValue:=CStr(oSource.AllowClaims), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="closed_allow_accounts", vValue:=CStr(oSource.AllowAccounts), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'FSA Phase III
            If oSource.FSA_BankType_ID < 1 Then

                'Developer Guide No. 85
                m_lReturn = .Parameters.Add(sName:="FSA_BankType_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="FSA_BankType_id", vValue:=CStr(oSource.FSA_BankType_ID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="user_id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="unique_id", vValue:=oSource.UniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="screen_hierarchy", vValue:=oSource.ScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'FSA Phase IIIEnd

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DefaultParameters (Public)
    '
    ' Description: Sets the Default Values for a Source.
    '
    ' ***************************************************************** '
    ' DC 31/01/00
    ' added email, vat no, sender mailbox id, broker abi id, user licence id,
    '       pm Source number and default indicator
    'MKW010903 PS255 - FSA Compliance Added vfsa_companycategory and vsa_staffwording
    'Developer Guide No. 101
    Private Function DefaultParameters(ByVal bDefaultAll As Boolean, Optional ByVal vSubType As Object = Nothing, Optional ByVal vSourceID As Object = Nothing, Optional ByVal vCode As Object = Nothing, Optional ByVal vDescription As Object = Nothing, Optional ByVal vCaptionID As Object = Nothing, Optional ByVal vParentID As Object = Nothing, Optional ByVal vBaseCurrency As Object = Nothing, Optional ByVal vRegNo1 As Object = Nothing, Optional ByVal vRegNo2 As Object = Nothing, Optional ByVal vAddress1 As Object = Nothing, Optional ByVal vAddress2 As Object = Nothing, Optional ByVal vAddress3 As Object = Nothing, Optional ByVal vAddress4 As Object = Nothing, Optional ByVal vPostalCode As Object = Nothing, Optional ByVal vCountryID As Object = Nothing, Optional ByVal vPhoneAreaCode As Object = Nothing, Optional ByVal vPhoneNumber As Object = Nothing, Optional ByVal vPhoneExtension As Object = Nothing, Optional ByVal vFaxAreaCode As Object = Nothing, Optional ByVal vFaxNumber As Object = Nothing, Optional ByVal vFaxExtension As Object = Nothing, Optional ByVal vEmail As Object = Nothing, Optional ByVal vVatNo As Object = Nothing, Optional ByVal vSenderMailboxId As Object = Nothing, Optional ByVal vBrokerABIId As Object = Nothing, Optional ByVal vUserLicenceId As Object = Nothing, Optional ByVal vPMSourceNumber As Object = Nothing, Optional ByVal vDefaultIndicator As Object = Nothing, Optional ByVal vfsa_CompanyCategory As Object = Nothing, Optional ByVal vfsa_staffwording As Object = Nothing, Optional ByVal vUnderwritingBranchInd As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Informations.IsNothing(vSourceID)) Or (vSourceID.Equals(0)) Or (bDefaultAll) Then
            vSourceID = 0
        End If



        If (Informations.IsNothing(vCode)) Or (String.IsNullOrEmpty(vCode)) Or (bDefaultAll) Then
            vCode = ""
        End If



        If (Informations.IsNothing(vDescription)) Or (String.IsNullOrEmpty(vDescription)) Or (bDefaultAll) Then
            vDescription = ""
        End If



        If (Informations.IsNothing(vCaptionID)) Or (vCaptionID.Equals(0)) Or (bDefaultAll) Then
            vCaptionID = 0
        End If



        If (Informations.IsNothing(vParentID)) Or (vParentID.Equals(0)) Or (bDefaultAll) Then
            vParentID = 0
        End If



        If (Informations.IsNothing(vBaseCurrency)) Or (vBaseCurrency.Equals(0)) Or (bDefaultAll) Then
            vBaseCurrency = 0
        End If



        If (Informations.IsNothing(vRegNo1)) Or (String.IsNullOrEmpty(vRegNo1)) Or (bDefaultAll) Then
            vRegNo1 = ""
        End If



        If (Informations.IsNothing(vRegNo2)) Or (String.IsNullOrEmpty(vRegNo2)) Or (bDefaultAll) Then
            vRegNo2 = ""
        End If



        If (Informations.IsNothing(vAddress1)) Or (String.IsNullOrEmpty(vAddress1)) Or (bDefaultAll) Then
            vAddress1 = ""
        End If



        If (Informations.IsNothing(vAddress2)) Or (String.IsNullOrEmpty(vAddress2)) Or (bDefaultAll) Then
            vAddress2 = ""
        End If



        If (Informations.IsNothing(vAddress3)) Or (String.IsNullOrEmpty(vAddress3)) Or (bDefaultAll) Then
            vAddress3 = ""
        End If



        If (Informations.IsNothing(vAddress4)) Or (String.IsNullOrEmpty(vAddress4)) Or (bDefaultAll) Then
            vAddress4 = ""
        End If



        If (Informations.IsNothing(vPostalCode)) Or (String.IsNullOrEmpty(vPostalCode)) Or (bDefaultAll) Then
            vPostalCode = ""
        End If



        If (Informations.IsNothing(vCountryID)) Or (vCountryID.Equals(0)) Or (bDefaultAll) Then
            vCountryID = 0
        End If



        If (Informations.IsNothing(vPhoneAreaCode)) Or (String.IsNullOrEmpty(vPhoneAreaCode)) Or (bDefaultAll) Then
            vPhoneAreaCode = ""
        End If



        If (Informations.IsNothing(vPhoneNumber)) Or (String.IsNullOrEmpty(vPhoneNumber)) Or (bDefaultAll) Then
            vPhoneNumber = ""
        End If



        If (Informations.IsNothing(vPhoneExtension)) Or (String.IsNullOrEmpty(vPhoneExtension)) Or (bDefaultAll) Then
            vPhoneExtension = ""
        End If



        If (Informations.IsNothing(vFaxAreaCode)) Or (String.IsNullOrEmpty(vFaxAreaCode)) Or (bDefaultAll) Then
            vFaxAreaCode = ""
        End If



        If (Informations.IsNothing(vFaxNumber)) Or (String.IsNullOrEmpty(vFaxNumber)) Or (bDefaultAll) Then
            vFaxNumber = ""
        End If



        If (Informations.IsNothing(vFaxExtension)) Or (String.IsNullOrEmpty(vFaxExtension)) Or (bDefaultAll) Then
            vFaxExtension = ""
        End If

        ' DC 31/01/00


        If (Informations.IsNothing(vEmail)) Or (String.IsNullOrEmpty(vEmail)) Or (bDefaultAll) Then
            vEmail = ""
        End If

        ' DC 31/01/00


        If (Informations.IsNothing(vVatNo)) Or (String.IsNullOrEmpty(vVatNo)) Or (bDefaultAll) Then
            vVatNo = ""
        End If

        ' DC 31/01/00


        If (Informations.IsNothing(vSenderMailboxId)) Or (String.IsNullOrEmpty(vSenderMailboxId)) Or (bDefaultAll) Then
            vSenderMailboxId = ""
        End If

        ' DC 31/01/00


        If (Informations.IsNothing(vBrokerABIId)) Or (String.IsNullOrEmpty(vBrokerABIId)) Or (bDefaultAll) Then
            vBrokerABIId = ""
        End If

        ' DC 31/01/00


        If (Informations.IsNothing(vUserLicenceId)) Or (vUserLicenceId.Equals(0)) Or (bDefaultAll) Then
            vUserLicenceId = 0
        End If

        ' DC 31/01/00


        If (Informations.IsNothing(vPMSourceNumber)) Or (vPMSourceNumber.Equals(0)) Or (bDefaultAll) Then
            vPMSourceNumber = 0
        End If

        ' DC 31/01/00


        If (Informations.IsNothing(vDefaultIndicator)) Or (String.IsNullOrEmpty(vDefaultIndicator)) Or (bDefaultAll) Then
            vDefaultIndicator = ""
        End If

        'SJ 17/02/2004 - start


        If (Informations.IsNothing(vUnderwritingBranchInd)) Or (vUnderwritingBranchInd.Equals(0)) Or (bDefaultAll) Then
            vUnderwritingBranchInd = 0
        End If
        'SJ 17/02/2004 - end

        ' {* USER DEFINED CODE (End) *}


        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory (Public)
    '
    ' Description: Sets the Default Values for a Source.
    '
    ' ***************************************************************** '
    ' DC 31/01/00
    ' added email, vat no, sender mailbox id, broker abi id, user licence id,
    '       pm Source number and default indicator
    'MKW010903 PS255 - FSA Compliance Added vfsa_companycategory and vsa_staffwording
    Private Function CheckMandatory(Optional ByRef vSourceID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vParentID As Object = Nothing, Optional ByRef vBaseCurrency As Object = Nothing, Optional ByRef vRegNo1 As Object = Nothing, Optional ByRef vRegNo2 As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vPhoneAreaCode As Object = Nothing, Optional ByRef vPhoneNumber As Object = Nothing, Optional ByRef vPhoneExtension As Object = Nothing, Optional ByRef vFaxAreaCode As Object = Nothing, Optional ByRef vFaxNumber As Object = Nothing, Optional ByRef vFaxExtension As Object = Nothing, Optional ByRef vEmail As Object = Nothing, Optional ByRef vVatNo As Object = Nothing, Optional ByRef vSenderMailboxId As Object = Nothing, Optional ByRef vBrokerABIId As Object = Nothing, Optional ByRef vUserLicenceId As Object = Nothing, Optional ByRef vPMSourceNumber As Object = Nothing, Optional ByRef vDefaultIndicator As Object = Nothing, Optional ByRef vfsa_CompanyCategory As Object = Nothing, Optional ByRef vfsa_staffwording As Object = Nothing, Optional ByVal vUnderwritingBranchInd As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Informations.IsNothing(vSourceID)) Or (Object.Equals(vSourceID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vCode)) Or (Object.Equals(vCode, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vDescription)) Or (Object.Equals(vDescription, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vCaptionID)) Or (Object.Equals(vCaptionID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vParentID)) Or (Object.Equals(vParentID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vBaseCurrency)) Or (Object.Equals(vBaseCurrency, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vRegNo1)) Or (Object.Equals(vRegNo1, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vRegNo2)) Or (Object.Equals(vRegNo2, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vAddress1)) Or (Object.Equals(vAddress1, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vAddress2)) Or (Object.Equals(vAddress2, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vAddress3)) Or (Object.Equals(vAddress3, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vAddress4)) Or (Object.Equals(vAddress4, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vPostalCode)) Or (Object.Equals(vPostalCode, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vCountryID)) Or (Object.Equals(vCountryID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vPhoneAreaCode)) Or (Object.Equals(vPhoneAreaCode, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vPhoneNumber)) Or (Object.Equals(vPhoneNumber, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vPhoneExtension)) Or (Object.Equals(vPhoneExtension, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vFaxAreaCode)) Or (Object.Equals(vFaxAreaCode, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vFaxNumber)) Or (Object.Equals(vFaxNumber, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vFaxExtension)) Or (Object.Equals(vFaxExtension, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the Source for Consistency.
    '
    ' ***************************************************************** '
    ' DC 31/01/00
    ' added email, vat no, sender mailbox id, broker abi id, user licence id,
    '       pm Source number and default indicator
    'MKW010903 PS255 - FSA Compliance Added vfsa_companycategory and vsa_staffwording
    Private Function Validate(Optional ByRef vSourceID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vParentID As Object = Nothing, Optional ByRef vBaseCurrency As Object = Nothing, Optional ByRef vRegNo1 As Object = Nothing, Optional ByRef vRegNo2 As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vPhoneAreaCode As Object = Nothing, Optional ByRef vPhoneNumber As Object = Nothing, Optional ByRef vPhoneExtension As Object = Nothing, Optional ByRef vFaxAreaCode As Object = Nothing, Optional ByRef vFaxNumber As Object = Nothing, Optional ByRef vFaxExtension As Object = Nothing, Optional ByRef vEmail As Object = Nothing, Optional ByRef vVatNo As Object = Nothing, Optional ByRef vSenderMailboxId As Object = Nothing, Optional ByRef vBrokerABIId As Object = Nothing, Optional ByRef vUserLicenceId As Object = Nothing, Optional ByRef vPMSourceNumber As Object = Nothing, Optional ByRef vDefaultIndicator As Object = Nothing, Optional ByRef vfsa_CompanyCategory As Object = Nothing, Optional ByRef vfsa_staffwording As Object = Nothing, Optional ByRef vUnderwritingBranchInd As Object = Nothing) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        Dim dbNumericTemp As Double
        If Not Double.TryParse(CStr(vSourceID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp2 As Double
        If Not Double.TryParse(CStr(vCaptionID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp3 As Double
        If Not Double.TryParse(CStr(vParentID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp4 As Double
        If Not Double.TryParse(CStr(vBaseCurrency), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp5 As Double
        If Not Double.TryParse(CStr(vCountryID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' DC 31/01/00

        Dim dbNumericTemp6 As Double
        If Not Double.TryParse(CStr(vUserLicenceId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp6) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' DC 31/01/00

        Dim dbNumericTemp7 As Double
        If Not Double.TryParse(CStr(vPMSourceNumber), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp7) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'SJ 17/02/2004 - start

        Dim dbNumericTemp8 As Double
        If Not Double.TryParse(CStr(vUnderwritingBranchInd), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp8) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'SJ 17/02/2004 - end

        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

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



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
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
        ' Error.
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
    ' Name: GetSysAdminStatus
    '
    ' Description: check if user is member of a SysAdmin user group.
    '
    ' History: RDC 17102002 created
    ' ***************************************************************** '
    Public Function GetSysAdminStatus(ByRef lStatus As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_lReturn = CType(gPMComponentServices.GetSysAdminAccessStatus(v_sUsername:=m_sUsername, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, lStatus:=lStatus, oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSysAdminStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSysAdminStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: PickListLoad
    '
    ' Description: Standard method for the PickList control to call
    '
    ' ***************************************************************** '
    Public Function PickListLoad(ByVal sPickListType As String, ByVal vFKArray(,) As Object, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            m_oDatabase.Parameters.Clear()

            'Load the parameters

            For iParam As Integer = vFKArray.GetLowerBound(1) To vFKArray.GetUpperBound(1)


                m_oDatabase.Parameters.Add(sName:=CStr(vFKArray(0, iParam)), vValue:=CStr(vFKArray(1, iParam)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Next iParam

            'Call the SP
            'Developer Guide No. 39
            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_PM_Source_PLL" & sPickListType, sSQLName:=sPickListType & " PickList Load", bStoredProcedure:=True, lNumberRecords:=0, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PickListLoad Select Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PickListLoad", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return m_lReturn
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PickListLoad Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PickListLoad", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: PickListSave
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function PickListSave(ByRef sPickListType As String, ByRef vFKArray(,) As Object, ByRef vKeys() As Object) As Integer

        Dim result As Integer = 0

        Try

            BeginTrans()

            With m_oDatabase

                'clear the old data
                .Parameters.Clear()

                'Load the FK parameters

                For iParam As Integer = vFKArray.GetLowerBound(1) To vFKArray.GetUpperBound(1)

                    If CStr(vFKArray(0, iParam)) = "user_id" Then
                        .Parameters.Add(sName:=CStr(vFKArray(0, iParam)), vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    Else
                        .Parameters.Add(sName:=CStr(vFKArray(0, iParam)), vValue:=CStr(vFKArray(1, iParam)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    End If
                Next iParam
                'developers guide no 39
                m_lReturn = .SQLAction(sSQL:="spu_PM_Source_PLD" & sPickListType, sSQLName:=sPickListType & " PickList Delete", bStoredProcedure:=True)

                'See if there is anything to save
                If Informations.IsArray(vKeys) Then

                    For iKey As Integer = vKeys.GetLowerBound(0) To vKeys.GetUpperBound(0)
                        .Parameters.Clear()

                        'Load the FK parameters

                        For iParam As Integer = vFKArray.GetLowerBound(1) To vFKArray.GetUpperBound(1)
                            If CStr(vFKArray(0, iParam)) = "user_id" Then
                                .Parameters.Add(sName:=CStr(vFKArray(0, iParam)), vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                            Else
                                .Parameters.Add(sName:=CStr(vFKArray(0, iParam)), vValue:=CStr(vFKArray(1, iParam)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                            End If
                        Next iParam


                        .Parameters.Add("Key", CStr(vKeys(iKey)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                        'Call the SP
                        'Developer Guide No. 39
                        m_lReturn = .SQLAction(sSQL:="spu_PM_Source_PLS" & sPickListType, sSQLName:=sPickListType & " PickList Load", bStoredProcedure:=True)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Log Error Message
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PickListSave Write Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PickListSave", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                            RollbackTrans()
                            Return m_lReturn
                        End If
                    Next iKey
                End If
            End With

            CommitTrans()

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception


            RollbackTrans()
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PickListSave Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PickListSave", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            RollbackTrans()
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: PickListParams
    '
    ' Description: Returns a string of question marks for the SP definition
    '
    ' ***************************************************************** '
    Private Function PickListParams(ByRef vParams As Object) As String

        Dim result As String = String.Empty


        Dim sComma As String = ""
        Dim sParam As New StringBuilder

        sComma = ""
        sParam = New StringBuilder("")

        For iParam As Integer = vParams.GetLowerBound(1) To vParams.GetUpperBound(1)
            sParam.Append(sComma & "?")
            sComma = ","
        Next iParam


        Return sParam.ToString()

    End Function

    Public Function ValidateCurrencies(ByVal lSourceID As Integer, ByRef r_sCurrencies As String, ByRef r_sAutomatedCurrencies As String) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: ValidateCurrencies
        ' PURPOSE: Validates the Currencies saved against a Branch to ensure the base
        ' currency matches the scheme currency
        ' AUTHOR: Danny Davis
        ' DATE: 05 August 2004, 10:50 AM
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        'AG - 28/10/2004 - PN 15790
        Dim sSQL As String = ""
        Dim vReturnArray(,) As Object = Nothing
        Dim sAutomatedCurrencies As New StringBuilder


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                .Parameters.Clear()
                m_lReturn = .Parameters.Add(sName:="source_id", vValue:=CStr(lSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Developer Guide No. 39
                m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_PM_Source_ValidateCurrencies", sSQLName:="spu_Source_ValidateCurrencies", bStoredProcedure:=True, vResultArray:=vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateCurrencies", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return m_lReturn
                End If

                'AG - 28/10/2004 - PN 15790 - START
                'To return automattically added currrencies during branch creation
                .Parameters.Clear()
                'Developer Guide No. 39
                m_lReturn = .SQLSelect(sSQL:="spu_PM_Source_GetDefaultCurrencies", sSQLName:="spu_PM_Source_GetDefaultCurrencies", bStoredProcedure:=True, vResultArray:=vReturnArray, lNumberRecords:=gPMConstants.PMAllRecords)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateCurrencies", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return m_lReturn
                End If

            End With

            r_sAutomatedCurrencies = ""
            r_sCurrencies = ""

            If Informations.IsArray(vReturnArray) Then

                For lItem As Integer = vReturnArray.GetLowerBound(1) To vReturnArray.GetUpperBound(1)

                    sAutomatedCurrencies.Append(Strings.ChrW(13) & Strings.ChrW(10) & CStr(vReturnArray(0, lItem)).Trim())
                Next lItem
            End If

            If Informations.IsArray(vResultArray) Then

                For lItem As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                    If (sAutomatedCurrencies.ToString().IndexOf(CStr(vResultArray(0, lItem)).Trim()) + 1) = 0 Then

                        r_sCurrencies = r_sCurrencies & Strings.ChrW(13) & Strings.ChrW(10) & CStr(vResultArray(0, lItem)).Trim()
                    Else

                        r_sAutomatedCurrencies = r_sAutomatedCurrencies & Strings.ChrW(13) & Strings.ChrW(10) & CStr(vResultArray(0, lItem)).Trim()
                    End If
                Next lItem
            End If
            'AG - 28/10/2004 - PN 15790 - END

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateCurrencies", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally


        End Try
        Return result
    End Function
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
