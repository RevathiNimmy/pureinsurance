Option Strict Off
Option Explicit On
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Form_NET.Form")> _
Public NotInheritable Class Form
    Implements IDisposable


    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 07/10/1998
    '
    ' Description: Creatable Form class which contains all the
    '              methods, business rules required for the
    '              SIRFindInsurance summary form.
    '
    ' Edit History: TF071098 - Created from bFindInsurance
    '               sj040800  - Add "PolicyTypeId" parameter to "SearchAllGIIM"
    '                           method ( used by Gemini only)
    ' SJP14062002 moved to uniform Product Options scheme and gSIRLibrary.bas
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 19/12/2003
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

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    Private m_oGISDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_lError As Integer

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Primary Keys to work with
    ' Insurance File ID
    Private m_lInsuranceFileCnt As Integer
    ' Insurance File ID
    'ECK 080699
    Private m_lPartyCnt As Integer
    ' Insurance Folder ID
    Private m_lInsuranceFolderCnt As Integer 'TF100398

    Private m_bGeminiLink As Boolean
    Private m_bGeminiIILink As Boolean
    Private m_bSwiftLink As Boolean

    Private m_sUnderwritingOrAgency As String = ""

    'RJG 21/06/2000 - I have duplicated this E-Num in our Sirius Link object so
    'that calling gemini net apps can get to it.
    Public Enum InsuranceFileSearchType
        IFSTQuote = 1
        IFSTPolicy = 2
        IFSTRenewal = 3
        IFSTQuotePolicy = 4
        IFSTQuotePolicyRenewal = 5
        IFSTMTAQuote = 6
        IFSTMTAQuoteMTATempQuote = 7
    End Enum

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

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


    Public Property InsuranceFileCnt() As Integer
        Get

            Return m_lInsuranceFileCnt

        End Get
        Set(ByVal Value As Integer)

            m_lInsuranceFileCnt = Value

        End Set
    End Property

    Public Property PartyCnt() As Integer
        Get

            Return m_lPartyCnt

        End Get
        Set(ByVal Value As Integer)

            m_lPartyCnt = Value

        End Set
    End Property

    Public ReadOnly Property GeminiLink() As Boolean
        Get

            Return m_bGeminiLink

        End Get
    End Property

    Public ReadOnly Property GeminiIILink() As Boolean
        Get

            Return m_bGeminiIILink

        End Get
    End Property

    Public ReadOnly Property SwiftLink() As Boolean
        Get

            Return m_bSwiftLink

        End Get
    End Property

    Public ReadOnly Property UnderwritingOrAgency() As String
        Get

            If m_sUnderwritingOrAgency = "" Then
                m_lReturn = getUnderwritingOrAgency()
            End If

            Return m_sUnderwritingOrAgency

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


            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now



            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_bSwiftLink = False

            m_bGeminiLink = False

            m_bGeminiIILink = False

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
            If disposing Then
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
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

                m_iTask = ToSafeInteger(vTask)
            End If


            If Not Informations.IsNothing(vNavigate) Then

                m_lNavigate = ToSafeInteger(vNavigate)
            End If


            If Not Informations.IsNothing(vProcessMode) Then

                m_lProcessMode = ToSafeInteger(vProcessMode)
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
    ' Name: SearchByQuery
    '
    ' Description: TF041298 - SQL Query to Select Insurance File details
    '
    ' ***************************************************************** '
    Public Function SearchByQuery(ByRef r_vResultArray As Object, Optional ByVal v_vInsuranceRef As Object = Nothing, Optional ByVal v_vInsFileType As Object = Nothing, Optional ByVal v_vShortName As Object = Nothing, Optional ByVal v_vVehicleRegNo As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchByQuery Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchByQuery", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SearchAll
    '
    ' Description: CT 13/09/00 - SQL Query to Select Risk details
    '
    ' ***************************************************************** '
    Public Function SearchAll(ByRef r_vResultArray(,) As Object, Optional ByVal v_vInsuranceFileCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            If Informations.IsNothing(v_vInsuranceFileCnt) Or v_vInsuranceFileCnt.Equals(0) Then
                v_vInsuranceFileCnt = 0
            Else
                v_vInsuranceFileCnt = v_vInsuranceFileCnt
            End If

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_vInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            ' NS-28/04/2002-F0033454 - Do not limit no of records returned - Added lnumberRecords = PMAllrecords
            result = m_oDatabase.SQLSelect(sSQL:=ACGetPolicyRiskSQL, sSQLName:=ACGetPolicyRiskName, bStoredProcedure:=ACGetPolicyRiskStored, vResultArray:=r_vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)

            ' If NO records were found return PMFalse
            If Not Informations.IsArray(r_vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
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
    ' Name: SearchInsuranceFile
    '
    ' Description: CT 13/09/00 - SQL Query to Select Risk details
    '
    ' ***************************************************************** '
    Public Function SearchInsuranceFile(ByRef r_vResultArray(,) As Object, Optional ByVal v_vInsuranceFileCnt As String = "") As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Build the SQL select statement according to the parameters passed
            ' Select statement to select all details relating to values entered
            sSQL = ""
            sSQL = sSQL & "SELECT ifi.product_id" & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & "FROM insurance_file ifi" & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & "WHERE ifi.insurance_file_cnt = " & v_vInsuranceFileCnt & Strings.ChrW(13) & Strings.ChrW(10)

            ' Execute SQL Statement - use array for speed
            With m_oDatabase

                .Parameters.Clear()

                m_lError = .SQLSelect(sSQL:=sSQL, sSQLName:="FindRisksQuery", bStoredProcedure:=False, vResultArray:=r_vResultArray, bKeepNulls:=True)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchInsuranceFile")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' If NO records were found return PMFalse
                If Not Informations.IsArray(r_vResultArray) Then
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchInsuranceFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchInsuranceFile", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SearchAllGIIM
    '
    ' Description: SQL Query to Get all GIIM policies
    '              Based on SearchAll
    '
    ' ***************************************************************** '
    Public Function SearchAllGIIM(ByRef r_vResultArray As Object, Optional ByVal v_lPartyCnt As Integer = 0, Optional ByVal v_lPolicyTypeId As Integer = 0) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchAllGIIM Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAllGIIM", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SearchAllByType
    '
    ' Description: SQL Query to Get all policies
    '              Based on SearchAll
    '
    ' ***************************************************************** '
    Public Function SearchAllByType(ByRef r_vResultArray As Object, Optional ByVal v_lPartyCnt As Integer = 0, Optional ByVal v_sPolicyType As String = "", Optional ByVal v_IFSTInsuranceFileType As InsuranceFileSearchType = 0, Optional ByVal v_bIncludeLapsedAndCancelled As Boolean = False) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchAllByType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAllByType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FindLikeRef (Public)
    '
    ' Description: Selects Insurance Files with a reference like the
    '              one supplied.
    '
    ' ***************************************************************** '
    Public Function FindLikeRef(ByRef sInsuranceRef As String, ByRef lNumberOfRecords As Integer, ByRef vResultArray As Object) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindLikeRef Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindLikeRef", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FindLikeRefAndHolder (Public)
    '
    ' Description: Selects Insurance Files with a reference like the
    '              one supplied and equal to Insurance Holder ID
    '
    ' ***************************************************************** '
    Public Function FindLikeRefAndHolder(ByRef sInsuranceRef As String, ByRef lInsuranceHolderCnt As Integer, ByRef lNumberOfRecords As Integer, ByRef vResultArray As Object) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindLikeRefAndHolder Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindLikeRefAndHolder", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FindLikeVehicle (Public)
    '
    ' Description: Selects Insurance Files with a vehicle like the
    '              one supplied.
    '
    ' ***************************************************************** '
    Public Function FindLikeVehicle(ByRef sRegistration As String, ByRef vResultArray As Object) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindLikeVehicle Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindLikeVehicle", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FindLikeIndex (Public)
    '
    ' Description: Selects Index Description from the value supplied
    '
    ' ***************************************************************** '
    Public Function FindLikeIndex(ByRef sIndex As String, ByRef lNumberOfRecords As Integer, ByRef vResultArray As Object) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindLikeIndex Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindLikeIndex", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetInsuranceFolder (Public) - TF100398
    '
    ' Description: Gets the InsuranceFolderCnt using InsuranceFile.Detail
    '
    ' ***************************************************************** '
    Public Function GetInsuranceFolder(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lInsuranceFolderCnt As Integer) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInsuranceFolder Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsuranceFolder", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetVersionArray
    '
    ' Description:
    '
    ' History: 22/05/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function GetVersionArray(ByRef r_lInsuranceFileCnt As Integer, ByRef r_vResultArray As Object, Optional ByVal v_lInsuranceFolderCnt As Integer = 0, Optional ByVal v_sPolicyNumber As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetVersionArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVersionArray", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetVersionByDate
    '
    ' Description:
    '
    ' History: 22/05/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function GetVersionByDate(ByRef r_lInsuranceFileCnt As Integer, ByVal v_dtStartDate As Date, ByRef r_lPolicyVersion As Integer, ByRef r_lErrorCode As Integer, Optional ByVal v_lInsuranceFolderCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetVersionByDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVersionByDate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CalcCombinedKey
    '
    ' Description: This is a wrapper to the component services function
    ' to derive the Unique Invariant Key from source and entity ID.
    '
    ' ***************************************************************** '
    Public Function calccombinedkey(ByVal v_lSourceID As Integer, ByVal v_lKeyID As Integer, ByRef r_lCombinedKeyID As Integer) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CalcCombinedKeyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CalcCombinedKey", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPolicyInterface (Public)
    '
    ' Description: Gets the Solution Specific Interface name.
    '
    ' ***************************************************************** '
    Public Function GetPolicyInterface(ByVal v_lInsuranceFileCnt As Integer, ByRef r_sClassName As String) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyInterface Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyInterface", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetDefaultSearchFields
    '
    ' Description:  Populate search fields from supplied ID's
    '
    ' ***************************************************************** '
    Public Function SetDefaultSearchFields(ByRef r_sInsRef As String, ByRef r_sShortName As String, Optional ByVal v_lInsuranceFileCnt As Object = Nothing, Optional ByVal v_lInsuranceHolderCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetDefaultSearchFieldsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetDefaultSearchFields", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PW051102
    '**********************************************************************************
    ' Name : GetRiskDescription
    '
    ' Desc : Admiral Fix Only - For the given risks, pass back the required risk
    '        descriptions
    '**********************************************************************************
    Public Function GetRiskDescription(ByVal v_sBankingCode As String, ByVal v_sRetailCode As String, ByVal v_sTradeCode As String, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="BankingDesc", vValue:=v_sBankingCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                m_lReturn = .Parameters.Add(sName:="RetailDesc", vValue:=v_sRetailCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                m_lReturn = .Parameters.Add(sName:="TradeDesc", vValue:=v_sTradeCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                m_lReturn = .SQLSelect(sSQL:=ACGetRiskDescriptionSQL, sSQLName:=ACGetRiskDescription, bStoredProcedure:=ACGetRiskDesc, vResultArray:=r_vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)
            End With

            ' If NO records were found return PMFalse
            If Not Informations.IsArray(r_vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskDescription Failed - Could not get risk descriptions from the database", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskTypes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' PW051102
    '**********************************************************************************
    ' Name : GetRiskTypes
    '
    ' Desc : Admiral Fix Only - Retrieve the required reg keys
    '**********************************************************************************
    Public Function GetRiskTypes(ByRef r_sBankingCode As String, ByRef r_sRetailCode As String, ByRef r_sTradeCode As String) As Integer
        Dim result As Integer = 0
        Const SUBKEY As String = "Admiral"
        Const BANKINGCODE As String = "BankingCode"
        Const RETAILCODE As String = "RetailCode"
        Const TRADECODE As String = "TradeCode"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get Banking Code
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=BANKINGCODE, r_sSettingValue:=r_sBankingCode, v_sSubKey:=SUBKEY), gPMConstants.PMEReturnCode)

            ' Create the registry key if doesn't exist
            If r_sBankingCode = "" Then
                r_sBankingCode = "MODULUS"
                m_lReturn = CType(gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=BANKINGCODE, v_sSettingValue:=r_sBankingCode, v_sSubKey:=SUBKEY), gPMConstants.PMEReturnCode)
            End If

            ' Get Retail Code
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=RETAILCODE, r_sSettingValue:=r_sRetailCode, v_sSubKey:=SUBKEY), gPMConstants.PMEReturnCode)

            ' Create the registry key if doesn't exist
            If r_sRetailCode = "" Then
                r_sRetailCode = "RETAIL"
                m_lReturn = CType(gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=RETAILCODE, v_sSettingValue:=r_sRetailCode, v_sSubKey:=SUBKEY), gPMConstants.PMEReturnCode)
            End If

            ' Get Trade Code
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=TRADECODE, r_sSettingValue:=r_sTradeCode, v_sSubKey:=SUBKEY), gPMConstants.PMEReturnCode)

            ' Create the registry key if doesn't exist
            If r_sTradeCode = "" Then
                r_sTradeCode = "TRADE"
                m_lReturn = CType(gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=TRADECODE, v_sSettingValue:=r_sTradeCode, v_sSubKey:=SUBKEY), gPMConstants.PMEReturnCode)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskTypes Failed - Could not get registry keys", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskTypes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UnderwritingOrAgency
    '
    ' Description:  Finds out if Underwriting or Agency business
    '
    ' SJP14062002 - getUnderWritingOrAgency uses new product options scheme
    ' ***************************************************************** '
    Public Function getUnderwritingOrAgency() As Integer

        Dim result As Integer = 0
        Try


            Return bPMFunc.getUnderwritingOrAgency(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, m_sUnderwritingOrAgency)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnderwritingOrAgencyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnderwritingOrAgency", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: DeleteRisk
    '
    ' Description:
    '
    ' History: 16/11/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function DeleteRisk(ByRef lInsuranceFileCnt As Integer, ByRef lRiskId As Integer) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=CStr(lRiskId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateInsuranceFileRiskLinkDetailsStatusSQL, sSQLName:=ACUpdateInsuranceFileRiskLinkDetailsStatusName, bStoredProcedure:=ACUpdateInsuranceFileRiskLinkDetailsStatusStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Now we need to know if we deleted the record.  The easiest way to tell is to try and select it...

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=CStr(lRiskId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectInsuranceFileRiskLinkDetailsSQL, sSQLName:=ACSelectInsuranceFileRiskLinkDetailsName, bStoredProcedure:=ACSelectInsuranceFileRiskLinkDetailsStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vArray) Then
                lRiskId = 0
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRisk", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '**********************************************************************************
    '
    ' Name : IsRIAtRiskLevel
    '
    ' Desc : is re-insurance at risk level?
    '
    ' Hist : 11/01/2001 Created - Tinny
    '
    '**********************************************************************************
    Public Function IsRIAtRiskLevel(ByVal lRiskTypeID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=CStr(lRiskTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_oDatabase.SQLSelect(sSQL:=ACIsReInsuranceAtRiskLevelSQL, sSQLName:=ACIsReInsuranceAtRiskLevelName, bStoredProcedure:=ACIsReInsuranceAtRiskLevelStored, vResultArray:=vResultArray, bKeepNulls:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'do we have any data
            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            'ReInsurance at risk level?

            Dim auxVar As Object = vResultArray(0, 0)


            If Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar) Or CDbl(vResultArray(0, 0)) <> 1 Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsRIAtRiskLevel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsRIAtRiskLevel", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


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


    '**********************************************************************************
    '
    ' Name : GetTransactionCurrency
    '
    ' Desc : Get the transaction currency from InsuranceFile
    '
    ' Hist : 14052004 RDC
    '
    '**********************************************************************************
    Public Function GetTransactionCurrency(ByVal v_lInsuranceFileCnt As Integer, ByRef r_iCurrencyID As Integer) As Integer

        Dim result As Integer = 0
        Dim vValue
        Dim vResult(,) As Object = Nothing
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_oDatabase.Parameters.Clear()

            ' add parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsFileCnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter InsuranceFileCnt", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTransactionCurrency")

                Return result
            End If

            ' call s.proc
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetTranCurrencySQL, sSQLName:=ACGetTranCurrencyDescription, bStoredProcedure:=ACGetTranCurrencyStored, vResultArray:=vResult)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Informations.IsArray(vResult) Then
                ' error, or no result
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Call to " & ACGetTranCurrencySQL & " failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTransactionCurrency")

                Return result
            End If



            vValue = vResult(0, 0)


            If Convert.IsDBNull(vValue) Or Informations.IsNothing(vValue) Then
                ' can't be doing with nulls
                r_iCurrencyID = -1
                Return result
            End If

            ' valid

            r_iCurrencyID = ToSafeInteger(vValue)


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTransactionCurrency Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTransactionCurrency", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetOption
    '
    ' Description: Get a system option - lifted from bOpenClaim.Business
    '
    ' History: PW221102 - Created (PS411)
    '
    ' ***************************************************************** '
    Public Function GetOption(ByVal v_iOptionNumber As Integer, ByRef r_nOptionValue As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim m_oSystemOption As bSIROptions.Business
            Dim sOptionValue As String = ""

            If m_oSystemOption Is Nothing Then

                m_oSystemOption = New bSIROptions.Business()

                m_lReturn = m_oSystemOption.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the system option object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result
                End If
            End If

            m_lReturn = m_oSystemOption.GetOption(iOptionNumber:=v_iOptionNumber, sValue:=sOptionValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = m_lReturn

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get system option " & v_iOptionNumber, vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            r_nOptionValue = ToSafeInteger(sOptionValue)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetHasCurrencyChanged(ByVal v_lInsuranceFileCnt As Integer, ByRef r_bHasCurrencyChanged As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="has_currency_changed", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetHasCurrencyChangedSQL, sSQLName:=ACGetHasCurrencyChangedName, bStoredProcedure:=ACGetHasCurrencyChangedStored)

            r_bHasCurrencyChanged = m_oDatabase.Parameters.Item("has_currency_changed").Value

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetHasCurrencyChanged Failed - Could not get risk descriptions from the database", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskTypes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function


    'Start (Venkatesh Raman) Tech Spec WR19 - Cover Note Functionality.doc section(4.8.2.1.1)

    Public Function AttachCoverNotes(ByRef r_vCoverNoteArray(,) As Object, ByVal iCounter As Integer) As Integer
        Dim Catch_Renamed As Boolean = False

        Dim result As Integer = 0
        Const kMethodName As String = "AttachCoverNotes"

        ' Dim iCounter As Integer

        Try
            Catch_Renamed = True
            result = gPMConstants.PMEReturnCode.PMTrue

            ' For iCounter = LBound(r_vCoverNoteArray, 2) To UBound(r_vCoverNoteArray, 2)

            If CBool(r_vCoverNoteArray(ACColCNAttach, iCounter)) Then

                bPMAddParameter.AddParameterLite(m_oDatabase, "Risk_Id", CType(gPMFunctions.ToSafeLong(r_vCoverNoteArray(ACColCNRiskId, iCounter)), gPMConstants.PMEReturnCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

                Dim auxVar As Object = Informations.BlankToNull(r_vCoverNoteArray(ACColCNRef, iCounter))


                If Not (Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar)) Then

                    bPMAddParameter.AddParameterLite(m_oDatabase, "Cover_Note_Ref", r_vCoverNoteArray(ACColCNRef, iCounter), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                End If


                Dim auxVar_2 As Object = Informations.BlankToNull(r_vCoverNoteArray(ACColCNRef, iCounter))


                If Not (Convert.IsDBNull(auxVar_2) Or Informations.IsNothing(auxVar_2)) Then

                    bPMAddParameter.AddParameterLite(m_oDatabase, "Cover_Note_From", r_vCoverNoteArray(ACColCNFrom, iCounter), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
                End If

                Dim auxVar_3 As Object = Informations.BlankToNull(r_vCoverNoteArray(ACColCNRef, iCounter))


                If Not (Convert.IsDBNull(auxVar_3) Or Informations.IsNothing(auxVar_3)) Then

                    bPMAddParameter.AddParameterLite(m_oDatabase, "Cover_Note_To", r_vCoverNoteArray(ACColCNTo, iCounter), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
                End If


                bPMAddParameter.AddParameterLite(m_oDatabase, "Risk_Cover_Note_Link_Id", r_vCoverNoteArray(ACColCNRiskLinkId, iCounter), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAttachCoverNoteDetailsSQL, sSQLName:=ACAttachCoverNoteDetailsName, bStoredProcedure:=ACAttachCoverNoteDetailsStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, ACAttachCoverNoteDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If
            'Next iCounter

            Return result

        Catch excep As System.Exception
            If Not Catch_Renamed Then
                Throw excep
            End If

            GoTo Finally_Renamed

            If Catch_Renamed Then


                ' DO Not Call any functions before here or the error will be lost
                bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=excep)

            End If
Finally_Renamed:
        End Try
    End Function

    'End (Venkatesh Raman) Tech Spec WR19 - Cover Note Functionality.doc section(4.8.2.1.1)


    'Start (Venkatesh Raman) Tech Spec WR19 - Cover Note Functionality.doc section(4.8.2.1.2)

    Public Function DetachCoverNotes(ByVal v_vCoverNoteArray(,) As Object, ByVal iCounter As Integer) As Integer
        Dim Catch_Renamed As Boolean = False


        Dim result As Integer = 0
        Const kMethodName As String = "DetachCoverNotes"

        'Dim iCounter As Integer

        Try
            Catch_Renamed = True


            result = gPMConstants.PMEReturnCode.PMTrue


            'For iCounter = LBound(v_vCoverNoteArray, 2) To UBound(v_vCoverNoteArray, 2)


            If Not CBool(v_vCoverNoteArray(ACColCNAttach, iCounter)) Then



                bPMAddParameter.AddParameterLite(m_oDatabase, "Risk_Id", v_vCoverNoteArray(ACColCNRiskId, iCounter), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDetachCoverNoteDetailsSQL, sSQLName:=ACDetachCoverNoteDetailsName, bStoredProcedure:=ACDetachCoverNoteDetailsStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, ACDetachCoverNoteDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            ' Next iCounter

            Return result

        Catch excep As System.Exception
            If Not Catch_Renamed Then
                Throw excep
            End If

            GoTo Finally_Renamed

            If Catch_Renamed Then


                ' DO Not Call any functions before here or the error will be lost
                bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=excep)

            End If
Finally_Renamed:
        End Try
    End Function

    'End (Venkatesh Raman) Tech Spec WR19 - Cover Note Functionality.doc section(4.8.2.1.2)
End Class
