Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
'developer guide no 129. 
Imports System.IO
Imports Microsoft.Practices.EnterpriseLibrary.Caching
Imports Microsoft.Practices.EnterpriseLibrary.Caching.Expirations
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Form_NET.Form")>
Public NotInheritable Class Form
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 31/07/1997
    '
    ' Description: Creatable Form class which contains all the
    '              methods, Form rules required to manipulate
    '              a Company.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 09/12/2003
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
    Private Const ACClass As String = "Form"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Collection of Companys (Private)
    Private m_oCompanys As bACTCompany.Companys

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As Integer

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
    ' Insurance File ID
    Private m_lInsuranceFileID As Integer
    ' Risk ID
    Private m_lRiskID As Integer


    ' PM Lookup Business Component (Private)
    Private m_oLookup As bPMLookup.Business
    Public Shared iCache As ICacheManager
    Private m_sCachePath As String = String.Empty
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
                Case Is > m_oCompanys.Count()
                    m_lCurrentRecord = m_oCompanys.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Numner in Collection
            Return m_oCompanys.Count()

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

            Return m_iSourceID

        End Get
        Set(ByVal Value As Integer)

            m_iSourceID = Value

        End Set
    End Property

    Public Property InsuranceFileID() As Integer
        Get

            Return m_lInsuranceFileID

        End Get
        Set(ByVal Value As Integer)

            m_lInsuranceFileID = Value

        End Set
    End Property

    Public Property RiskID() As Integer
        Get

            Return m_lRiskID

        End Get
        Set(ByVal Value As Integer)

            m_lRiskID = Value

        End Set
    End Property
    ' DC 31/01/00
    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFOrion

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

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUserName
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            ' Initialisation Code.

            ' Set Username and Password

            ' Set UserID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            ' Get Reference to Database

            ' DC 31/01/00
            ' change way of referencing database

            '    Set m_oDatabase = GetOrionDatabase( _
            ''                            lOpenStatus:=m_lReturn, _
            ''                            bCloseDatabase:=m_bCloseDatabase, _
            ''                            vDatabase:=vDatabase)



            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Set the ProcessMode to Generic
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric

            ' Set the Type Of Business to New Business
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric

            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now

            ' Default RiskID to 0 (So we will work at Ins File Level by default)
            m_lRiskID = 0

            ' Create Companys Collection
            m_oCompanys = New bACTCompany.Companys()


            ' Create PM Lookup Business Object
            m_oLookup = New bPMLookup.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUserName:=sUserName, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFOrion
            Try
                iCache = CacheFactory.GetCacheManager("PureCache")
            Catch ex As Exception

            End Try

            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine,
                                                   v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture,
                                                   v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer,
                                                   v_sSettingName:=gPMConstants.PMRegKeyCachePath, r_sSettingValue:=m_sCachePath)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Right(m_sCachePath, 1) <> "\" Then
                m_sCachePath += "\"
            End If
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
                    m_oLookup = Nothing
                End If
                m_oCompanys = Nothing
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values for a Company.
    '
    '
    ' ***************************************************************** '
    'developer guide no. 71(Guide)
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim oCompany As bACTCompany.Company = Nothing
        Dim dtEffectiveDate As Date

        ' {* USER DEFINED CODE (Begin) *}
        Dim vTabArray(3, 0) As Object
        ' {* USER DEFINED CODE (End) *}

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array
            vResultArray = Nothing
            ' Reset Table Array

            vTableArray = Nothing

            ' {* USER DEFINED CODE (Begin) *}

            ' Setup Lookup Table Names

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = gPMConstants.PMLookupCountry

            ' {* USER DEFINED CODE (End) *}

            ' Do we have any records
            If m_lCurrentRecord < 1 Then
                ' No, we can only lookup all
                iLookupType = gPMConstants.PMELookupType.PMLookupAll
            Else
                ' Yes get current record
                oCompany = m_oCompanys.Item(m_lCurrentRecord)
            End If

            Select Case iLookupType
                Case gPMConstants.PMELookupType.PMLookupAll

                    ' {* USER DEFINED CODE (Begin) *}
                    ' Do not supply a key

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""
                    ' {* USER DEFINED CODE (End) *}

                    ' Default Effective Date to current date/time
                    dtEffectiveDate = DateTime.Now

                Case gPMConstants.PMELookupType.PMLookupAllEffective

                    ' Use keys and effective date from current record
                    ' Note: The keys are not used for the select, but are used by
                    '       the iterface program to set the list index.
                    With oCompany

                        ' {* USER DEFINED CODE (Begin) *}

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = .CountryID

                        'BB dtEffectiveDate = .EffectiveDate
                        'BB Default Effective Date to current date/time when not present
                        dtEffectiveDate = DateTime.Now
                        ' {* USER DEFINED CODE (End) *}

                    End With

                Case gPMConstants.PMELookupType.PMLookupSingle

                    ' Set keys from current record
                    With oCompany

                        ' {* USER DEFINED CODE (Begin) *}

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = .CountryID
                        ' {* USER DEFINED CODE (End) *}

                    End With
                    ' Default Effective Date to current date/time
                    dtEffectiveDate = DateTime.Now

            End Select

            ' Release Company reference
            oCompany = Nothing

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single Company directly into the database.
    '        Note: The Company will NOT be added to the collection.
    '
    ' ***************************************************************** '
    ' DC 31/01/00
    ' added email, vat no, sender mailbox id, broker abi id, user licence id,
    '       pm company number and default indicator
    Public Function DirectAdd(Optional ByRef vCompanyID As Integer = 0, Optional ByRef vBaseCurrency As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vParentID As Object = Nothing, Optional ByRef vRegNo1 As Object = Nothing, Optional ByRef vRegNo2 As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vPhoneAreaCode As Object = Nothing, Optional ByRef vPhoneNumber As Object = Nothing, Optional ByRef vPhoneExtension As Object = Nothing, Optional ByRef vFaxAreaCode As Object = Nothing, Optional ByRef vFaxNumber As Object = Nothing, Optional ByRef vFaxExtension As Object = Nothing, Optional ByRef vEmail As Object = Nothing, Optional ByRef vVatNo As Object = Nothing, Optional ByRef vSenderMailboxId As Object = Nothing, Optional ByRef vBrokerABIId As Object = Nothing, Optional ByRef vUserLicenceId As Object = Nothing, Optional ByRef vPMCompanyNumber As Object = Nothing, Optional ByRef vDefaultIndicator As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oCompany As bACTCompany.Company

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Company
            oCompany = New bACTCompany.Company()

            ' Populate Company Attributes
            ' DC 31/01/00
            ' added email, vat no, sender mailbox id, broker abi id, user licence id,
            '       pm company number and default indicator


























            m_lReturn = SetProperties(oCompany, gPMConstants.PMEComponentAction.PMAdd, vCompanyID:=vCompanyID, vBaseCurrency:=vBaseCurrency, vCode:=vCode, vDescription:=vDescription, vCaptionID:=vCaptionID, vParentID:=vParentID, vRegNo1:=vRegNo1, vRegNo2:=vRegNo2, vAddress1:=vAddress1, vAddress2:=vAddress2, vAddress3:=vAddress3, vAddress4:=vAddress4, vPostalCode:=vPostalCode, vCountryID:=vCountryID, vPhoneAreaCode:=vPhoneAreaCode, vPhoneNumber:=vPhoneNumber, vPhoneExtension:=vPhoneExtension, vFaxAreaCode:=vFaxAreaCode, vFaxNumber:=vFaxNumber, vFaxExtension:=vFaxExtension, vEmail:=vEmail, vVatNo:=vVatNo, vSenderMailboxId:=vSenderMailboxId, vBrokerABIId:=vBrokerABIId, vUserLicenceId:=vUserLicenceId, vPMCompanyNumber:=vPMCompanyNumber, vDefaultIndicator:=vDefaultIndicator)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Company to the Database
            m_lReturn = AddItem(oCompany)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (Begin) *}

            ' Return the ID of the Company Added

            If Not Informations.IsNothing(vCompanyID) Then
                vCompanyID = oCompany.CompanyID
            End If

            ' {* USER DEFINED CODE (End) *}

            oCompany = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectDelete (Public)
    '
    ' Description: Deletes a single Company directly from the database.
    '        Note: The Company will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    ' DC 31/01/00
    ' added email, vat no, sender mailbox id, broker abi id, user licence id,
    '       pm company number and default indicator
    Public Function DirectDelete(Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vBaseCurrency As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vParentID As Object = Nothing, Optional ByRef vRegNo1 As Object = Nothing, Optional ByRef vRegNo2 As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vPhoneAreaCode As Object = Nothing, Optional ByRef vPhoneNumber As Object = Nothing, Optional ByRef vPhoneExtension As Object = Nothing, Optional ByRef vFaxAreaCode As Object = Nothing, Optional ByRef vFaxNumber As Object = Nothing, Optional ByRef vFaxExtension As Object = Nothing, Optional ByRef vEmail As Object = Nothing, Optional ByRef vVatNo As Object = Nothing, Optional ByRef vSenderMailboxId As Object = Nothing, Optional ByRef vBrokerABIId As Object = Nothing, Optional ByRef vUserLicenceId As Object = Nothing, Optional ByRef vPMCompanyNumber As Object = Nothing, Optional ByRef vDefaultIndicator As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oCompany As bACTCompany.Company

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Company
            oCompany = New bACTCompany.Company()

            ' Populate Company Attributes
            ' DC 31/01/00
            ' added email, vat no, sender mailbox id, broker abi id, user licence id,
            '       pm company number and default indicator



























            m_lReturn = SetProperties(oCompany, gPMConstants.PMEComponentAction.PMDelete, vCompanyID:=vCompanyID, vBaseCurrency:=vBaseCurrency, vCode:=vCode, vDescription:=vDescription, vCaptionID:=vCaptionID, vParentID:=vParentID, vRegNo1:=vRegNo1, vRegNo2:=vRegNo2, vAddress1:=vAddress1, vAddress2:=vAddress2, vAddress3:=vAddress3, vAddress4:=vAddress4, vPostalCode:=vPostalCode, vCountryID:=vCountryID, vPhoneAreaCode:=vPhoneAreaCode, vPhoneNumber:=vPhoneNumber, vPhoneExtension:=vPhoneExtension, vFaxAreaCode:=vFaxAreaCode, vFaxNumber:=vFaxNumber, vFaxExtension:=vFaxExtension, vEmail:=vEmail, vVatNo:=vVatNo, vSenderMailboxId:=vSenderMailboxId, vBrokerABIId:=vBrokerABIId, vUserLicenceId:=vUserLicenceId, vPMCompanyNumber:=vPMCompanyNumber, vDefaultIndicator:=vDefaultIndicator)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Company to the Database
            m_lReturn = DeleteItem(oCompany)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oCompany = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the Company.
    '
    ' ***************************************************************** '
    ' DC 31/01/00
    ' added email, vat no, sender mailbox id, broker abi id, user licence id,
    '       pm company number and default indicator
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vBaseCurrency As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vParentID As Object = Nothing, Optional ByRef vRegNo1 As Object = Nothing, Optional ByRef vRegNo2 As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vPhoneAreaCode As Object = Nothing, Optional ByRef vPhoneNumber As Object = Nothing, Optional ByRef vPhoneExtension As Object = Nothing, Optional ByRef vFaxAreaCode As Object = Nothing, Optional ByRef vFaxNumber As Object = Nothing, Optional ByRef vFaxExtension As Object = Nothing, Optional ByRef vEmail As Object = Nothing, Optional ByRef vVatNo As Object = Nothing, Optional ByRef vSenderMailboxId As Object = Nothing, Optional ByRef vBrokerABIId As Object = Nothing, Optional ByRef vUserLicenceId As Object = Nothing, Optional ByRef vPMCompanyNumber As Object = Nothing, Optional ByRef vDefaultIndicator As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults
            ' DC 31/01/00
            ' added email, vat no, sender mailbox id, broker abi id, user licence id,
            '       pm company number and default indicator



























            m_lReturn = DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vCompanyID:=vCompanyID, vBaseCurrency:=vBaseCurrency, vCode:=vCode, vDescription:=vDescription, vCaptionID:=vCaptionID, vParentID:=vParentID, vRegNo1:=vRegNo1, vRegNo2:=vRegNo2, vAddress1:=vAddress1, vAddress2:=vAddress2, vAddress3:=vAddress3, vAddress4:=vAddress4, vPostalCode:=vPostalCode, vCountryID:=vCountryID, vPhoneAreaCode:=vPhoneAreaCode, vPhoneNumber:=vPhoneNumber, vPhoneExtension:=vPhoneExtension, vFaxAreaCode:=vFaxAreaCode, vFaxNumber:=vFaxNumber, vFaxExtension:=vFaxExtension, vEmail:=vEmail, vVatNo:=vVatNo, vSenderMailboxId:=vSenderMailboxId, vBrokerABIId:=vBrokerABIId, vUserLicenceId:=vUserLicenceId, vPMCompanyNumber:=vPMCompanyNumber, vDefaultIndicator:=vDefaultIndicator)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaults", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim oFields As DataRow

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Informations.IsArray(vFieldArray) Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:="Parameter vFieldArray must be a Variant Array", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCaptions")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have a Table name
            'BB If (IsMissing(vTable) = False) Then

            ' Is this our table
            '    If (Trim$(vTable) <> PMTableCompany) Then

            '        GetCaptions = PMInvalidRequest


            '       Exit Function

            '    End If

            'End If

            ' Get the Captions ourself

            ' Check that this record exists
            m_lReturn = CheckID(vID:=vID)

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

                    'AK 230702 - check for null value

                    If Not (Convert.IsDBNull(oFields(vFieldArray(lSub))) Or Informations.IsNothing(oFields(vFieldArray(lSub)))) Then
                        ' Store the results in the Temporary results array

                        vResults(lSub) = oFields(vFieldArray(lSub))
                    Else
                        Select Case oFields(vFieldArray(lSub)).Type
                            'developer guide no. 47(No Solutions)
                            'Case DbType.String, DbType.String, DbType.String, adLongVarWChar, DbType.String, DbType.String, adWChar
                            Case DbType.String, DbType.String, DbType.String, DbType.String, DbType.String

                                vResults(lSub) = ""
                                'developer guide no. 47(No Solutions)
                                'Case DbType.Date, adDBDate 
                            Case DbType.Date

                                vResults(lSub) = -1
                            Case Else

                                vResults(lSub) = 0
                        End Select
                    End If

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCaptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCaptions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required Companys and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vLockMode As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oCompany As bACTCompany.Company
        Dim sContent(1) As String
        Dim sCacheFilename As String = "Company"
        Dim sFilePath As String = ""
        Dim oFieldNameArray As Object = Nothing
        Dim sKey As String = String.Empty
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oCompanys.Clear()

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

            If Not Informations.IsNothing(vCompanyID) Then

                ' Yes, Is the key valid

                Dim dbNumericTemp2 As Double
                If Not Double.TryParse(CStr(vCompanyID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vCompanyID =" & CStr(vCompanyID), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                    Return result

                End If

                ReDim oFieldNameArray(0)
                Dim nRowCount As Integer = 0
                sKey = sCacheFilename.ToUpper() + CStr(vCompanyID) + nRowCount.ToString() 'FileName + CompanyID + nRowCount

                'Check for Zero th row and keep on increasing nRowCount till we do not find
                Do While Not iCache Is Nothing AndAlso iCache.Contains(sKey) AndAlso Not String.IsNullOrEmpty(Convert.ToString(iCache.GetData(sKey)))
                    oCompany = New bACTCompany.Company()
                    oCompany = iCache.GetData(sKey)
                    If (m_oCompanys.Count = 0) Then
                        m_oCompanys.Add(Nothing)
                    End If
                    m_lReturn = m_oCompanys.Add(oNewCompany:=oCompany)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    oCompany = Nothing
                    nRowCount = nRowCount + 1
                    sKey = sCacheFilename.ToUpper() + CStr(vCompanyID) + (nRowCount).ToString.Trim.ToUpper()
                Loop

                If nRowCount > 0 Then
                    Return result
                End If

                ' Add the CompanyID parameter (INPUT)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Company_id", vValue:=CStr(vCompanyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDetailsSQL, sSQLName:=ACGetDetailsName, bStoredProcedure:=ACGetDetailsStored, lNumberRecords:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else

                ' No Key, Get All Records

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllDetailsSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=0)

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
                sFilePath = m_sCachePath + sCacheFilename + ".xml"
                If Not FileExists(sFilePath) Then
                    Dim fileIO As FileStream
                    fileIO = File.Create(sFilePath)
                    fileIO.Close()
                    File.WriteAllLines(sFilePath, sContent)
                End If
                ' Yes, load them into the collection

                For lSub As Integer = 0 To lRecordCount - 1

                    ' Create New Company
                    oCompany = New bACTCompany.Company()

                    'developer guide no. 162(Guide)
                    m_lReturn = SetPropertiesFromDB(oCompany:=oCompany, lRecordNumber:=lSub)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    ' Add Company to collection
                    If (m_oCompanys.Count = 0) Then
                        m_oCompanys.Add(Nothing)
                    End If
                    m_lReturn = m_oCompanys.Add(oNewCompany:=oCompany)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    Dim sCompanyID As Integer = "0"
                    If Not Informations.IsNothing(vCompanyID) Then
                        sCompanyID = CStr(vCompanyID)
                    End If
                    sKey = sCacheFilename.ToUpper() + sCompanyID.ToString() + lSub.ToString()
                    If Not iCache Is Nothing Then
                        Dim atTimePeriod As AbsoluteTime = New AbsoluteTime(TimeSpan.FromMinutes(10))
                        iCache.Add(sKey, oCompany, CacheItemPriority.Normal, Nothing, New FileDependency(sFilePath), atTimePeriod)
                    End If
                    oCompany = Nothing

                Next lSub

            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the required Companys and populate the Collection
    '
    ' ***************************************************************** '
    ' DC 31/01/00
    ' added email, vat no, sender mailbox id, broker abi id, user licence id,
    '       pm company number and default indicator
    Public Function GetNext(Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vBaseCurrency As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vParentID As Object = Nothing, Optional ByRef vRegNo1 As Object = Nothing, Optional ByRef vRegNo2 As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vPhoneAreaCode As Object = Nothing, Optional ByRef vPhoneNumber As Object = Nothing, Optional ByRef vPhoneExtension As Object = Nothing, Optional ByRef vFaxAreaCode As Object = Nothing, Optional ByRef vFaxNumber As Object = Nothing, Optional ByRef vFaxExtension As Object = Nothing, Optional ByRef vEmail As Object = Nothing, Optional ByRef vVatNo As Object = Nothing, Optional ByRef vSenderMailboxId As Object = Nothing, Optional ByRef vBrokerABIId As Object = Nothing, Optional ByRef vUserLicenceId As Object = Nothing, Optional ByRef vPMCompanyNumber As Object = Nothing, Optional ByRef vDefaultIndicator As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oCompany As bACTCompany.Company
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oCompanys.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oCompany = m_oCompanys.Item(m_lCurrentRecord)

            ' Get the Company Property Values
            ' DC 31/01/00
            ' added email, vat no, sender mailbox id, broker abi id, user licence id,
            '       pm company number and default indicator



























            'developer guide no. 98(Guide)
            m_lReturn = GetProperties(oCompany, iStatus,
          vCompanyID:=vCompanyID, vBaseCurrency:=vBaseCurrency, vCode:=vCode,
          vDescription:=vDescription, vCaptionID:=vCaptionID, vParentID:=vParentID,
          vRegNo1:=vRegNo1, vRegNo2:=vRegNo2, vAddress1:=vAddress1,
          vAddress2:=vAddress2, vAddress3:=vAddress3, vAddress4:=vAddress4,
          vPostalCode:=vPostalCode, vCountryID:=vCountryID,
          vPhoneAreaCode:=vPhoneAreaCode,
          vPhoneNumber:=vPhoneNumber, vPhoneExtension:=vPhoneExtension,
          vFaxAreaCode:=vFaxAreaCode,
          vFaxNumber:=vFaxNumber, vFaxExtension:=vFaxExtension,
          vEmail:=vEmail, vVatNo:=vVatNo, vSenderMailboxId:=vSenderMailboxId,
          vBrokerABIId:=vBrokerABIId, vUserLicenceId:=vUserLicenceId,
          vPMCompanyNumber:=vPMCompanyNumber, vDefaultIndicator:=vDefaultIndicator)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oCompany = Nothing


            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied Company into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    ' DC 31/01/00
    ' added email, vat no, sender mailbox id, broker abi id, user licence id,
    '       pm company number and default indicator
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vBaseCurrency As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vParentID As Object = Nothing, Optional ByRef vRegNo1 As Object = Nothing, Optional ByRef vRegNo2 As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vPhoneAreaCode As Object = Nothing, Optional ByRef vPhoneNumber As Object = Nothing, Optional ByRef vPhoneExtension As Object = Nothing, Optional ByRef vFaxAreaCode As Object = Nothing, Optional ByRef vFaxNumber As Object = Nothing, Optional ByRef vFaxExtension As Object = Nothing, Optional ByRef vEmail As Object = Nothing, Optional ByRef vVatNo As Object = Nothing, Optional ByRef vSenderMailboxId As Object = Nothing, Optional ByRef vBrokerABIId As Object = Nothing, Optional ByRef vUserLicenceId As Object = Nothing, Optional ByRef vPMCompanyNumber As Object = Nothing, Optional ByRef vDefaultIndicator As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oCompany As bACTCompany.Company

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oCompanys.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new Company
            oCompany = New bACTCompany.Company()

            ' Populate Company Attributes
            ' DC 31/01/00
            ' added email, vat no, sender mailbox id, broker abi id, user licence id,
            '       pm company number and default indicator



























            m_lReturn = SetProperties(oCompany, gPMConstants.PMEComponentAction.PMAdd, vCompanyID:=vCompanyID, vBaseCurrency:=vBaseCurrency, vCode:=vCode, vDescription:=vDescription, vCaptionID:=vCaptionID, vParentID:=vParentID, vRegNo1:=vRegNo1, vRegNo2:=vRegNo2, vAddress1:=vAddress1, vAddress2:=vAddress2, vAddress3:=vAddress3, vAddress4:=vAddress4, vPostalCode:=vPostalCode, vCountryID:=vCountryID, vPhoneAreaCode:=vPhoneAreaCode, vPhoneNumber:=vPhoneNumber, vPhoneExtension:=vPhoneExtension, vFaxAreaCode:=vFaxAreaCode, vFaxNumber:=vFaxNumber, vFaxExtension:=vFaxExtension, vEmail:=vEmail, vVatNo:=vVatNo, vSenderMailboxId:=vSenderMailboxId, vBrokerABIId:=vBrokerABIId, vUserLicenceId:=vUserLicenceId, vPMCompanyNumber:=vPMCompanyNumber, vDefaultIndicator:=vDefaultIndicator)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oCompany = Nothing
                Return result
            End If

            ' Add Company to collection
            If (m_oCompanys.Count = 0) Then
                m_oCompanys.Add(Nothing)
            End If
            m_lReturn = m_oCompanys.Add(oNewCompany:=oCompany)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oCompany = Nothing
                Return result
            End If

            oCompany = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the Company
    '              specified and updates the Company with the new values.
    '
    ' ***************************************************************** '
    ' DC 31/01/00
    ' added email, vat no, sender mailbox id, broker abi id, user licence id,
    '       pm company number and default indicator
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vBaseCurrency As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vParentID As Object = Nothing, Optional ByRef vRegNo1 As Object = Nothing, Optional ByRef vRegNo2 As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vPhoneAreaCode As Object = Nothing, Optional ByRef vPhoneNumber As Object = Nothing, Optional ByRef vPhoneExtension As Object = Nothing, Optional ByRef vFaxAreaCode As Object = Nothing, Optional ByRef vFaxNumber As Object = Nothing, Optional ByRef vFaxExtension As Object = Nothing, Optional ByRef vEmail As Object = Nothing, Optional ByRef vVatNo As Object = Nothing, Optional ByRef vSenderMailboxId As Object = Nothing, Optional ByRef vBrokerABIId As Object = Nothing, Optional ByRef vUserLicenceId As Object = Nothing, Optional ByRef vPMCompanyNumber As Object = Nothing, Optional ByRef vDefaultIndicator As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oCompany As bACTCompany.Company
        Dim iStatus As gPMConstants.PMEComponentAction

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oCompanys.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oCompany = m_oCompanys.Item(lRow)

            ' Check the Status of the Company

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oCompany.DatabaseStatus
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

            ' Update Company Attributes
            ' DC 31/01/00
            ' added email, vat no, sender mailbox id, broker abi id, user licence id,
            '       pm company number and default indicator



























            m_lReturn = SetProperties(oCompany, iStatus, vCompanyID:=vCompanyID, vBaseCurrency:=vBaseCurrency, vCode:=vCode, vDescription:=vDescription, vCaptionID:=vCaptionID, vParentID:=vParentID, vRegNo1:=vRegNo1, vRegNo2:=vRegNo2, vAddress1:=vAddress1, vAddress2:=vAddress2, vAddress3:=vAddress3, vAddress4:=vAddress4, vPostalCode:=vPostalCode, vCountryID:=vCountryID, vPhoneAreaCode:=vPhoneAreaCode, vPhoneNumber:=vPhoneNumber, vPhoneExtension:=vPhoneExtension, vFaxAreaCode:=vFaxAreaCode, vFaxNumber:=vFaxNumber, vFaxExtension:=vFaxExtension, vEmail:=vEmail, vVatNo:=vVatNo, vSenderMailboxId:=vSenderMailboxId, vBrokerABIId:=vBrokerABIId, vUserLicenceId:=vUserLicenceId, vPMCompanyNumber:=vPMCompanyNumber, vDefaultIndicator:=vDefaultIndicator)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oCompany = Nothing
                Return result
            End If

            ' Release reference to Company
            oCompany = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete (Public)
    '
    ' Description: Validate that the specified Company can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oCompany As bACTCompany.Company

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oCompanys.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oCompany = m_oCompanys.Item(lRow)

            ' Check the Status of the Company

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oCompany.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oCompany.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oCompany.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            ' Release reference to Company
            oCompany = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
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
            For lSub As Integer = 1 To m_oCompanys.Count()
                Select Case m_oCompanys.Item(lSub).DatabaseStatus
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
        Dim oCompany As bACTCompany.Company
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oCompanys.Count()
                oCompany = m_oCompanys.Item(lSub)


                Select Case oCompany.DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing

                    Case gPMConstants.PMEComponentAction.PMAdd

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = BeginTrans()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Add Item
                        m_lReturn = AddItem(oCompany)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMEdit

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = BeginTrans()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Update Item
                        m_lReturn = UpdateItem(oCompany)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMDelete

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = BeginTrans()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Delete Item
                        m_lReturn = DeleteItem(oCompany)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Release last reference
            oCompany = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CommitTrans()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Refresh the Collection Items D/B Status
                    lSub = 1

                    ' For each item in the collection
                    Do While lSub <= m_oCompanys.Count()

                        ' With the item
                        With m_oCompanys.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oCompanys.Delete(lSub)

                                    ' Anything Else
                                Case Else
                                    ' Set Status to view
                                    .DatabaseStatus = gPMConstants.PMEComponentAction.PMView
                                    lSub += 1

                            End Select

                        End With

                    Loop

                Else

                    m_lReturn = RollbackTrans()
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Private Function AddItem(ByRef oCompany As bACTCompany.Company) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the required INPUT parameters
        m_lReturn = AddInputParam(oCompany:=oCompany)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add CompanyID as an OUTPUT param for an insert
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Company_id", vValue:=CStr(oCompany.CompanyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the ID of the record inserted
        oCompany.CompanyID = m_oDatabase.Parameters.Item("Company_id").Value

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
    Private Function UpdateItem(ByRef oCompany As bACTCompany.Company) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the Input Parameters to the Parameteres collection
        m_lReturn = AddInputParam(oCompany:=oCompany)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add CompanyID as an INPUT param for an update
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Company_id", vValue:=CStr(oCompany.CompanyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' BB Not required by Orion
        ' Add Timestamp as an INPUT param for an update
        'm_lReturn& = m_oDatabase.Parameters.Add( _
        'sName:="timestamp", _
        'vValue:=oCompany.Timestamp, _
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
    Private Function DeleteItem(ByRef oCompany As bACTCompany.Company) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the CompanyID INPUT parameter
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Company_id", vValue:=CStr(oCompany.CompanyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

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
    ' Description: Sets the supplied Company properties from a database
    '              record.
    ' ***************************************************************** '
    Private Function SetPropertiesFromDB(ByRef oCompany As bACTCompany.Company, ByRef lRecordNumber As Integer) As Integer

        Dim result As Integer = 0
        'developer guide no. 112(Guide)
        Dim oFields As DataRow



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set oFields to refer to Field Collection for Record 1
        oFields = m_oDatabase.Records.Item(lRecordNumber).Fields()

        ' Populate Base Details

        With oCompany

            .CompanyID = oFields("company_id")

            If Convert.IsDBNull(oFields("base_currency")) Or Informations.IsNothing(oFields("base_currency")) Then
                .BaseCurrency = 0
            Else
                .BaseCurrency = oFields("base_currency")
            End If
            .Code = oFields("code")

            If Convert.IsDBNull(oFields("description")) Or Informations.IsNothing(oFields("description")) Then
                .Description = ""
            Else
                .Description = oFields("description")
            End If

            If Convert.IsDBNull(oFields("caption_id")) Or Informations.IsNothing(oFields("caption_id")) Then
                .CaptionID = 0
            Else
                .CaptionID = oFields("caption_id")
            End If

            If Convert.IsDBNull(oFields("parent_id")) Or Informations.IsNothing(oFields("parent_id")) Then
                .ParentID = 0
            Else
                .ParentID = oFields("parent_id")
            End If

            If Convert.IsDBNull(oFields("reg_no_1")) Or Informations.IsNothing(oFields("reg_no_1")) Then
                .RegNo1 = ""
            Else
                .RegNo1 = oFields("reg_no_1")
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
                .PMCompanyNumber = 0
            Else
                .PMCompanyNumber = oFields("pm_company_number")
            End If
            ' DC 31/01/00

            If Convert.IsDBNull(oFields("default_indicator")) Or Informations.IsNothing(oFields("default_indicator")) Then
                .DefaultIndicator = ""
            Else
                .DefaultIndicator = oFields("default_indicator")
            End If

            .DatabaseStatus = gPMConstants.PMEComponentAction.PMView

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Private)
    '
    ' Description: Sets the supplied Company property values.
    '
    ' ***************************************************************** '
    ' DC 31/01/00
    ' added email, vat no, sender mailbox id, broker abi id, user licence id,
    '       pm company number and default indicator
    Private Function SetProperties(ByRef oCompany As bACTCompany.Company, ByRef iStatus As Integer, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vBaseCurrency As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vParentID As Object = Nothing, Optional ByRef vRegNo1 As Object = Nothing, Optional ByRef vRegNo2 As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vPhoneAreaCode As Object = Nothing, Optional ByRef vPhoneNumber As Object = Nothing, Optional ByRef vPhoneExtension As Object = Nothing, Optional ByRef vFaxAreaCode As Object = Nothing, Optional ByRef vFaxNumber As Object = Nothing, Optional ByRef vFaxExtension As Object = Nothing, Optional ByRef vEmail As Object = Nothing, Optional ByRef vVatNo As Object = Nothing, Optional ByRef vSenderMailboxId As Object = Nothing, Optional ByRef vBrokerABIId As Object = Nothing, Optional ByRef vUserLicenceId As Object = Nothing, Optional ByRef vPMCompanyNumber As Object = Nothing, Optional ByRef vDefaultIndicator As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        ' If Add Mode
        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

            ' Check that All Mandatory Parameters have been supplied
            ' DC 31/01/00
            ' added email, vat no, sender mailbox id, broker abi id, user licence id,
            '       pm company number and default indicator
            m_lReturn = CheckMandatory(vCompanyID:=vCompanyID, vBaseCurrency:=vBaseCurrency, vCode:=vCode, vDescription:=vDescription, vCaptionID:=vCaptionID, vParentID:=vParentID, vRegNo1:=vRegNo1, vRegNo2:=vRegNo2, vAddress1:=vAddress1, vAddress2:=vAddress2, vAddress3:=vAddress3, vAddress4:=vAddress4, vPostalCode:=vPostalCode, vCountryID:=vCountryID, vPhoneAreaCode:=vPhoneAreaCode, vPhoneNumber:=vPhoneNumber, vPhoneExtension:=vPhoneExtension, vFaxAreaCode:=vFaxAreaCode, vFaxNumber:=vFaxNumber, vFaxExtension:=vFaxExtension, vEmail:=vEmail, vVatNo:=vVatNo, vSenderMailboxId:=vSenderMailboxId, vBrokerABIId:=vBrokerABIId, vUserLicenceId:=vUserLicenceId, vPMCompanyNumber:=vPMCompanyNumber, vDefaultIndicator:=vDefaultIndicator)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Default Any Missing Parameters
            ' DC 31/01/00
            ' added email, vat no, sender mailbox id, broker abi id, user licence id,
            '       pm company number and default indicator
            m_lReturn = DefaultParameters(bDefaultAll:=False, vCompanyID:=vCompanyID, vBaseCurrency:=vBaseCurrency, vCode:=vCode, vDescription:=vDescription, vCaptionID:=vCaptionID, vParentID:=vParentID, vRegNo1:=vRegNo1, vRegNo2:=vRegNo2, vAddress1:=vAddress1, vAddress2:=vAddress2, vAddress3:=vAddress3, vAddress4:=vAddress4, vPostalCode:=vPostalCode, vCountryID:=vCountryID, vPhoneAreaCode:=vPhoneAreaCode, vPhoneNumber:=vPhoneNumber, vPhoneExtension:=vPhoneExtension, vFaxAreaCode:=vFaxAreaCode, vFaxNumber:=vFaxNumber, vFaxExtension:=vFaxExtension, vEmail:=vEmail, vVatNo:=vVatNo, vSenderMailboxId:=vSenderMailboxId, vBrokerABIId:=vBrokerABIId, vUserLicenceId:=vUserLicenceId, vPMCompanyNumber:=vPMCompanyNumber, vDefaultIndicator:=vDefaultIndicator)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

        End If

        ' Validate Parameters
        ' DC 31/01/00
        ' added email, vat no, sender mailbox id, broker abi id, user licence id,
        '       pm company number and default indicator
        m_lReturn = Validate(vCompanyID:=vCompanyID, vBaseCurrency:=vBaseCurrency, vCode:=vCode, vDescription:=vDescription, vCaptionID:=vCaptionID, vParentID:=vParentID, vRegNo1:=vRegNo1, vRegNo2:=vRegNo2, vAddress1:=vAddress1, vAddress2:=vAddress2, vAddress3:=vAddress3, vAddress4:=vAddress4, vPostalCode:=vPostalCode, vCountryID:=vCountryID, vPhoneAreaCode:=vPhoneAreaCode, vPhoneNumber:=vPhoneNumber, vPhoneExtension:=vPhoneExtension, vFaxAreaCode:=vFaxAreaCode, vFaxNumber:=vFaxNumber, vFaxExtension:=vFaxExtension, vEmail:=vEmail, vVatNo:=vVatNo, vSenderMailboxId:=vSenderMailboxId, vBrokerABIId:=vBrokerABIId, vUserLicenceId:=vUserLicenceId, vPMCompanyNumber:=vPMCompanyNumber, vDefaultIndicator:=vDefaultIndicator)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        ' Set Data Changed Flag to False
        bDataChanged = False


        ' Set Property values.
        With oCompany


            If Not Informations.IsNothing(vCompanyID) Then
                If .CompanyID <> vCompanyID Then
                    .CompanyID = vCompanyID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vBaseCurrency) Then
                If .BaseCurrency <> vBaseCurrency Then
                    .BaseCurrency = vBaseCurrency
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vCode) Then
                If .Code.Trim() <> vCode.Trim() Then
                    .Code = vCode
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vDescription) Then
                If .Description.Trim() <> vDescription.Trim() Then
                    .Description = vDescription
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vCaptionID) Then
                If .CaptionID <> vCaptionID Then
                    .CaptionID = vCaptionID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vParentID) Then
                If .ParentID <> vParentID Then
                    .ParentID = vParentID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vRegNo1) Then
                If .RegNo1.Trim() <> vRegNo1.Trim() Then
                    .RegNo1 = vRegNo1
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vRegNo2) Then
                If .RegNo2.Trim() <> vRegNo2.Trim() Then
                    .RegNo2 = vRegNo2
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vAddress1) Then
                If .Address1.Trim() <> vAddress1.Trim() Then
                    .Address1 = vAddress1
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vAddress2) Then
                If .Address2.Trim() <> vAddress2.Trim() Then
                    .Address2 = vAddress2
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vAddress3) Then
                If .Address3.Trim() <> vAddress3.Trim() Then
                    .Address3 = vAddress3
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vAddress4) Then
                If .Address4.Trim() <> vAddress4.Trim() Then
                    .Address4 = vAddress4
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vPostalCode) Then
                If .PostalCode.Trim() <> vPostalCode.Trim() Then
                    .PostalCode = vPostalCode
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vCountryID) Then
                If .CountryID <> vCountryID Then
                    .CountryID = vCountryID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vPhoneAreaCode) Then
                If .PhoneAreaCode.Trim() <> vPhoneAreaCode.Trim() Then
                    .PhoneAreaCode = vPhoneAreaCode
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vPhoneNumber) Then
                If .PhoneNumber.Trim() <> vPhoneNumber.Trim() Then
                    .PhoneNumber = vPhoneNumber
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vPhoneExtension) Then
                If .PhoneExtension.Trim() <> vPhoneExtension.Trim() Then
                    .PhoneExtension = vPhoneExtension
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vFaxAreaCode) Then
                If .FaxAreaCode.Trim() <> vFaxAreaCode.Trim() Then
                    .FaxAreaCode = vFaxAreaCode
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vFaxNumber) Then
                If .FaxNumber.Trim() <> vFaxNumber.Trim() Then
                    .FaxNumber = vFaxNumber
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vFaxExtension) Then
                If .FaxExtension.Trim() <> vFaxExtension.Trim() Then
                    .FaxExtension = vFaxExtension
                    bDataChanged = True
                End If
            End If

            ' DC 31/01/00

            If Not Informations.IsNothing(vEmail) Then
                If .Email.Trim() <> vEmail.Trim() Then
                    .Email = vEmail
                    bDataChanged = True
                End If
            End If

            ' DC 31/01/00

            If Not Informations.IsNothing(vVatNo) Then
                If .VatNo.Trim() <> vVatNo.Trim() Then
                    .VatNo = vVatNo
                    bDataChanged = True
                End If
            End If

            ' DC 31/01/00

            If Not Informations.IsNothing(vSenderMailboxId) Then
                If .SenderMailboxId.Trim() <> vSenderMailboxId.Trim() Then
                    .SenderMailboxId = vSenderMailboxId
                    bDataChanged = True
                End If
            End If

            ' DC 31/01/00

            If Not Informations.IsNothing(vBrokerABIId) Then
                If .BrokerABIId.Trim() <> vBrokerABIId.Trim() Then
                    .BrokerABIId = vBrokerABIId
                    bDataChanged = True
                End If
            End If

            ' DC 31/01/00

            If Not Informations.IsNothing(vUserLicenceId) Then
                If .UserLicenceId <> vUserLicenceId Then
                    .UserLicenceId = vUserLicenceId
                    bDataChanged = True
                End If
            End If

            ' DC 31/01/00

            If Not Informations.IsNothing(vPMCompanyNumber) Then
                If .PMCompanyNumber <> vPMCompanyNumber Then
                    .PMCompanyNumber = vPMCompanyNumber
                    bDataChanged = True
                End If
            End If

            ' DC 31/01/00

            If Not Informations.IsNothing(vDefaultIndicator) Then
                If .DefaultIndicator.Trim() <> vDefaultIndicator.Trim() Then
                    .DefaultIndicator = vDefaultIndicator
                    bDataChanged = True
                End If
            End If

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
    ' Description: Returns the supplied Company property values.
    '
    ' ***************************************************************** '
    ' DC 31/01/00
    ' added email, vat no, sender mailbox id, broker abi id, user licence id,
    '       pm company number and default indicator
    Private Function GetProperties(ByRef oCompany As bACTCompany.Company, ByRef iStatus As Integer, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vBaseCurrency As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vParentID As Object = Nothing, Optional ByRef vRegNo1 As Object = Nothing, Optional ByRef vRegNo2 As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vPhoneAreaCode As Object = Nothing, Optional ByRef vPhoneNumber As Object = Nothing, Optional ByRef vPhoneExtension As Object = Nothing, Optional ByRef vFaxAreaCode As Object = Nothing, Optional ByRef vFaxNumber As Object = Nothing, Optional ByRef vFaxExtension As Object = Nothing, Optional ByRef vEmail As Object = Nothing, Optional ByRef vVatNo As Object = Nothing, Optional ByRef vSenderMailboxId As Object = Nothing, Optional ByRef vBrokerABIId As Object = Nothing, Optional ByRef vUserLicenceId As Object = Nothing, Optional ByRef vPMCompanyNumber As Object = Nothing, Optional ByRef vDefaultIndicator As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set Property values.
        With oCompany


            vCompanyID = .CompanyID

            vBaseCurrency = .BaseCurrency

            vCode = .Code

            vDescription = .Description

            vCaptionID = .CaptionID

            vParentID = .ParentID

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

            vPMCompanyNumber = .PMCompanyNumber
            ' DC 31/01/00

            vDefaultIndicator = .DefaultIndicator

            iStatus = .DatabaseStatus

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
    Private Function AddInputParam(ByRef oCompany As bACTCompany.Company) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase


            m_lReturn = .Parameters.Add(sName:="base_currency", vValue:=CStr(oCompany.BaseCurrency), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="code", vValue:=oCompany.Code, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="description", vValue:=oCompany.Description, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oCompany.CaptionID < 1 Then

                'developer guide no 85. 
                m_lReturn = .Parameters.Add(sName:="caption_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="caption_id", vValue:=CStr(oCompany.CaptionID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oCompany.ParentID < 1 Then

                'developer guide no 85.  
                m_lReturn = .Parameters.Add(sName:="parent_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="parent_id", vValue:=CStr(oCompany.ParentID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="reg_no_1", vValue:=oCompany.RegNo1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="reg_no_2", vValue:=oCompany.RegNo2, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="address1", vValue:=oCompany.Address1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="address2", vValue:=oCompany.Address2, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="address3", vValue:=oCompany.Address3, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="address4", vValue:=oCompany.Address4, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="postal_code", vValue:=oCompany.PostalCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oCompany.CountryID < 1 Then

                'developer guide no 85. 
                m_lReturn = .Parameters.Add(sName:="country_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="country_id", vValue:=CStr(oCompany.CountryID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="phone_area_code", vValue:=oCompany.PhoneAreaCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="phone_number", vValue:=oCompany.PhoneNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="phone_extension", vValue:=oCompany.PhoneExtension, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="fax_area_code", vValue:=oCompany.FaxAreaCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="fax_number", vValue:=oCompany.FaxNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="fax_extension", vValue:=oCompany.FaxExtension, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' DC 31/01/00
            m_lReturn = .Parameters.Add(sName:="email", vValue:=oCompany.Email, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' DC 31/01/00
            m_lReturn = .Parameters.Add(sName:="vat_no", vValue:=oCompany.VatNo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' DC 31/01/00
            m_lReturn = .Parameters.Add(sName:="sender_mailbox_id", vValue:=oCompany.SenderMailboxId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' DC 31/01/00
            m_lReturn = .Parameters.Add(sName:="broker_abi_id", vValue:=oCompany.BrokerABIId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' DC 31/01/00
            If oCompany.UserLicenceId < 1 Then

                'developer guide no 85. 
                m_lReturn = .Parameters.Add(sName:="user_licence_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="user_licence_id", vValue:=CStr(oCompany.UserLicenceId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' DC 31/01/00
            If oCompany.PMCompanyNumber < 1 Then

                'developer guide no 85. 
                m_lReturn = .Parameters.Add(sName:="pm_company_number", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="pm_company_number", vValue:=CStr(oCompany.PMCompanyNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' DC 31/01/00
            m_lReturn = .Parameters.Add(sName:="default_indicator", vValue:=oCompany.DefaultIndicator, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DefaultParameters (Public)
    '
    ' Description: Sets the Default Values for a Company.
    '
    ' ***************************************************************** '
    ' DC 31/01/00
    ' added email, vat no, sender mailbox id, broker abi id, user licence id,
    '       pm company number and default indicator
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vBaseCurrency As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vParentID As Object = Nothing, Optional ByRef vRegNo1 As Object = Nothing, Optional ByRef vRegNo2 As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vPhoneAreaCode As Object = Nothing, Optional ByRef vPhoneNumber As Object = Nothing, Optional ByRef vPhoneExtension As Object = Nothing, Optional ByRef vFaxAreaCode As Object = Nothing, Optional ByRef vFaxNumber As Object = Nothing, Optional ByRef vFaxExtension As Object = Nothing, Optional ByRef vEmail As Object = Nothing, Optional ByRef vVatNo As Object = Nothing, Optional ByRef vSenderMailboxId As Object = Nothing, Optional ByRef vBrokerABIId As Object = Nothing, Optional ByRef vUserLicenceId As Object = Nothing, Optional ByRef vPMCompanyNumber As Object = Nothing, Optional ByRef vDefaultIndicator As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Informations.IsNothing(vCompanyID)) Or (vCompanyID.Equals(0)) Or (bDefaultAll) Then
            vCompanyID = 0
        End If



        If (Informations.IsNothing(vBaseCurrency)) Or (vBaseCurrency.Equals(0)) Or (bDefaultAll) Then
            vBaseCurrency = 0
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


        If (Informations.IsNothing(vPMCompanyNumber)) Or (vPMCompanyNumber.Equals(0)) Or (bDefaultAll) Then
            vPMCompanyNumber = 0
        End If

        ' DC 31/01/00


        If (Informations.IsNothing(vDefaultIndicator)) Or (String.IsNullOrEmpty(vDefaultIndicator)) Or (bDefaultAll) Then
            vDefaultIndicator = ""
        End If

        ' {* USER DEFINED CODE (End) *}


        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory (Public)
    '
    ' Description: Sets the Default Values for a Company.
    '
    ' ***************************************************************** '
    ' DC 31/01/00
    ' added email, vat no, sender mailbox id, broker abi id, user licence id,
    '       pm company number and default indicator
    Private Function CheckMandatory(Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vBaseCurrency As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vParentID As Object = Nothing, Optional ByRef vRegNo1 As Object = Nothing, Optional ByRef vRegNo2 As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vPhoneAreaCode As Object = Nothing, Optional ByRef vPhoneNumber As Object = Nothing, Optional ByRef vPhoneExtension As Object = Nothing, Optional ByRef vFaxAreaCode As Object = Nothing, Optional ByRef vFaxNumber As Object = Nothing, Optional ByRef vFaxExtension As Object = Nothing, Optional ByRef vEmail As Object = Nothing, Optional ByRef vVatNo As Object = Nothing, Optional ByRef vSenderMailboxId As Object = Nothing, Optional ByRef vBrokerABIId As Object = Nothing, Optional ByRef vUserLicenceId As Object = Nothing, Optional ByRef vPMCompanyNumber As Object = Nothing, Optional ByRef vDefaultIndicator As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Informations.IsNothing(vCompanyID)) Or (Object.Equals(vCompanyID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vBaseCurrency)) Or (Object.Equals(vBaseCurrency, Nothing)) Then
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
    ' Description: Checks the Company for Consistency.
    '
    ' ***************************************************************** '
    ' DC 31/01/00
    ' added email, vat no, sender mailbox id, broker abi id, user licence id,
    '       pm company number and default indicator
    Private Function Validate(Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vBaseCurrency As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vParentID As Object = Nothing, Optional ByRef vRegNo1 As Object = Nothing, Optional ByRef vRegNo2 As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vPhoneAreaCode As Object = Nothing, Optional ByRef vPhoneNumber As Object = Nothing, Optional ByRef vPhoneExtension As Object = Nothing, Optional ByRef vFaxAreaCode As Object = Nothing, Optional ByRef vFaxNumber As Object = Nothing, Optional ByRef vFaxExtension As Object = Nothing, Optional ByRef vEmail As Object = Nothing, Optional ByRef vVatNo As Object = Nothing, Optional ByRef vSenderMailboxId As Object = Nothing, Optional ByRef vBrokerABIId As Object = Nothing, Optional ByRef vUserLicenceId As Object = Nothing, Optional ByRef vPMCompanyNumber As Object = Nothing, Optional ByRef vDefaultIndicator As Object = Nothing) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        Dim dbNumericTemp As Double
        If Not Double.TryParse(CStr(vCompanyID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp2 As Double
        If Not Double.TryParse(CStr(vBaseCurrency), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp3 As Double
        If Not Double.TryParse(CStr(vCaptionID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp4 As Double
        If Not Double.TryParse(CStr(vParentID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
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
        If Not Double.TryParse(CStr(vPMCompanyNumber), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp7) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


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



            ' Error.
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



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class

