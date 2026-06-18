Option Strict Off
Option Explicit On
'developer guide no. 129
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Business
    ' Date: 14-Jun-00
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a OpenClaim.
    ' Edit History:
    ' DJM 14/06/2002 : In function NewAddPrimaryKeyID, added a (long) parameter to return number.
    ' Pandu-Added Methods-GetRiskDetails,RiskDesc,ClmAdd,AddAddress
    '                     Get Policy details,AddAdressInputParam,AddAddressKeyOutputParam
    ' Written by Sravan Kumar.G
    ' SJP14062002 - getUnderWritingOrAgency uses new product options scheme
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 20/10/2003
    ' Username.
    Private m_sUsername As String = ""

    ' Password.
    Private m_sPassword As String = ""

    ' User ID
    Private m_iUserID As Integer

    ' Calling Application
    Private m_sCallingAppNAme As String = ""
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

    ' Collection of OpenClaims (Private)
    'Private m_oOpenClaims As bOpenClaim.Business

    Private m_odOpenClaim As dOpenClaim.OpenClaim

    Private m_oEvent As bSIREvent.Business

    Private m_oBOLink As bBackOfficeLink.bBOLink

    'RWH(10/11/2000) claim numbering
    Private m_oPolicyNumMaint As bSIRPolicyNumMaint.Business

    Private m_lClaimid As Integer
    'DC240402
    Private m_vClaimComments As Object
    'DC290702
    Private m_sClaimNo As String = ""

    ' Database Class (Private)

    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_sSiriusProduct As String = ""

    Private lPMAuthorityLevel As Integer

    Private m_oLookup As bPMLookup.Business

    'RWH(13/11/2000) Claim numbering
    Private m_sUnderwritingOrAgency As String = ""

    Private m_lInsuranceFolderCnt As Integer
    Private m_lInsuranceFileCnt As Integer

    Private Const ACGetNumberingSchemeIdsFromProductSQL As String = "spu_get_prod_auto_num_ids"
    Private Const ACGetNumberingSchemeIdsFromProductName As String = "GetNumberingSchemeIdsFromProduct"
    Private Const ACGetNumberingSchemeFromProductStored As Boolean = True
    Private Const ACGetNumberingSchemeSQL As String = "spu_numbering_scheme_saa"
    Private Const ACGetAbandonedNumberingSchemeSQL As String = "spu_abandoned_numbers_saa"


    Private Const iNUM_SCHEME_ID As Integer = 0
    Private Const iNUM_SCHEME_CAPTION_ID As Integer = 1
    Private Const iNUM_SCHEME_CODE As Integer = 2
    Private Const iNUM_SCHEME_DESCRIPTION As Integer = 3
    Private Const iNUM_SCHEME_IS_DEL As Integer = 4
    Private Const iNUM_SCHEME_EFF_DATE As Integer = 5
    Private Const iNUM_SCHEME_TYPE_ID As Integer = 6
    Private Const iNUM_SCHEME As Integer = 7
    Private Const iNUM_SCHEME_IS_GEN As Integer = 8
    Private Const iNUM_SCHEME_MASK As Integer = 9
    Private Const iNUM_SCHEME_FIXED_CODE As Integer = 10
    Private Const iNUM_SCHEME_NEXT_NUM As Integer = 11
    Private Const iNUM_SCHEME_HIGH_NUM As Integer = 12
    Private Const iNUM_SCHEME_STEP As Integer = 13
    Private Const iNUM_SCHEME_REUSE_ABANDONED As Integer = 14
    Private Const iNUM_SCHEME_TYPE_DESC As Integer = 15


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

    'RWH(09/11/2000) Claims Numbering.
    Public ReadOnly Property UnderwritingOrAgency() As String
        Get

            If m_sUnderwritingOrAgency = "" Then
                m_lReturn = getUnderwritingOrAgency()
            End If

            Return m_sUnderwritingOrAgency

        End Get
    End Property

    'Added Claimid  Properties -Pandu

    Public Property Claimid() As Integer
        Get
            Return m_lClaimid
        End Get
        Set(ByVal Value As Integer)
            m_lClaimid = Value
        End Set
    End Property

    'DC290702 -start

    Public Property ClaimNo() As String
        Get
            Return m_sClaimNo
        End Get
        Set(ByVal Value As String)
            m_sClaimNo = Value
        End Set
    End Property
    'DC290702 -end
    'DC240402 -Start


    'Private Function ClaimComments() As Object
    'Return m_vClaimComments
    'End Function

    'Private Sub ClaimComments(ByVal Value As Object)


    'm_vClaimComments = Value
    'End Sub


    Public ReadOnly Property SiriusProduct() As String
        Get

            Return m_sSiriusProduct

        End Get
    End Property

    Public ReadOnly Property UserName() As String
        Get

            Return m_sUsername

        End Get
    End Property
    'DC240402 -End


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
            ' Set Username and Password
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppNAme = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now



            ' Create PM Lookup Business Object
            m_oLookup = New BPMLOOKUP.Business()

            'RWH(18/04/2001) Create dOpenClaim with component services.

            m_odOpenClaim = New dOpenClaim.OpenClaim
            m_lReturn = m_odOpenClaim.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=sCallingAppName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the Open Claim data component", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions


            If m_oBOLink Is Nothing Then

                m_oBOLink = New bBackOfficeLink.bBOLink()

                '******Changed Here To Make It Comatible For Client Server Model ******
                '******Added By Pandu Date :12-10-2000 ********************************

                m_lReturn = m_oBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceId:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                '******End of Change Here To Make It Comatible For Client Server Model ******
                '******Added By Pandu Date :12-10-2000 ********************************

                If m_oBOLink Is Nothing Then


                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the BackOffice Link Object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result
                End If
            End If

            m_sSiriusProduct = m_oBOLink.Sirius_Product

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
                If m_oEvent IsNot Nothing Then
                    m_oEvent.Dispose()
                    m_oEvent = Nothing
                End If
                If m_oPolicyNumMaint IsNot Nothing Then
                    m_oPolicyNumMaint.Dispose()
                    m_oPolicyNumMaint = Nothing
                End If
                If m_oBOLink IsNot Nothing Then
                    m_oBOLink.Dispose()
                    m_oBOLink = Nothing
                End If
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
            If Not (m_oBOLink Is Nothing) Then
                m_lReturn = m_oBOLink.SetProcessModes(vTask:=vTask, vNavigate:=vNavigate, vProcessMode:=vProcessMode, vTransactionType:=vTransactionType, vEffectiveDate:=vEffectiveDate)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to process the interface.

                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the back office link object", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes")

                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
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
    ' Description: Gets the Lookup values for a OpenClaim.
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function GetLookupValues(ByVal iLookupType As Integer, ByRef vTableArray(,) As Object, ByVal iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim vTabArray(3, 8) As Object
        Dim dtEffectiveDate As Date

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Table Array

            vTableArray = Nothing


            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = "Handler"

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 1) = "Progress_Status"

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 2) = "Primary_Cause"

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 3) = "Catastrophe_Code"

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 4) = "Currency"

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 5) = "Town"

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 6) = "Underwriting_Year"
            'S4B Claims Enhancements R&D 2005

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 7) = "Claim_At_Fault"

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 8) = "Policy_Deductible"



            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = ""

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 2) = ""

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 3) = ""

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 4) = ""

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 5) = ""

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 6) = ""
            'S4B Claims Enhancements R&D 2005

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 7) = ""

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 8) = ""

            ' Default Effective Date to current date/time
            dtEffectiveDate = DateTime.Now

            ' Get the Lookup items
            m_lReturn = m_oLookup.GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTabArray, iLanguageID:=iLanguageID, dtEffectiveDate:=dtEffectiveDate, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the Table Array

            vTableArray = vTabArray

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' TB Function to retrieve Country name
    ' RSA supports issue 31398 - country name required on Form
    Public Function GetCountryName(ByRef v_sCountryName As String, ByVal v_CountryID As Integer) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            Dim sSQL As String = ""
            Dim vArray(,) As Object = Nothing

            sSQL = "SELECT description FROM country WHERE country_id = " & v_CountryID

            ' Execute SQL Statement

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetCountryName", bStoredProcedure:=False, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            v_sCountryName = CStr(vArray(0, 0))


            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCountryName Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCountryName", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: CheckID (Private)
    '
    ' Description: Checks to see if the Claim No is a valid record.
    '
    ' ***************************************************************** '
    Private Function CheckID(ByRef vClaimid As Object) As Boolean

        Dim result As Boolean = False
        Dim lRecordCount As Integer



        result = True

        ' Clear the Database Parameters Collection

        m_oDatabase.Parameters.Clear()

        ' Add the ID parameter (INPUT)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_id", vValue:=vClaimid, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return False
        End If

        ' Execute SQL Statement

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckIDSQL, sSQLName:=ACCheckIDName, bStoredProcedure:=ACCheckIDStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' How many records were selected

        lRecordCount = m_oDatabase.Records.Count

        ' Do we have any records ?
        If lRecordCount < 1 Then
            ' No record found
            Return False
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SelectSecondaryCause (Public)
    '
    ' Description: Selects all the Secondary Cause records in the
    '               Secondary_Cause Table
    '
    ' ***************************************************************** '
    Public Function SelectSecondaryCause(ByRef rvResultArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection

            m_oDatabase.Parameters.Clear()
            'PN: 73770
            result = m_oDatabase.Parameters.Add(sName:="mode", vValue:=m_iTask, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If
            ' Execute SQL Statement

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetSecondaryCauseSQL, sSQLName:=ACGetSecondaryCauseName, bStoredProcedure:=ACGetSecondaryCauseStored, vResultArray:=rvResultArray, lNumberRecords:=gPMConstants.PMAllRecords)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectSecondaryCause Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectSecondaryCause", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' PUBLIC Methods (End)


    ' ***************************************************************** '
    ' Name: Add (Public)
    '
    ' Description: Adds a Record to the Database
    '
    ' ***************************************************************** '
    Public Function Add() As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Try


            m_lReturn = m_odOpenClaim.Add

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Claimid = m_odOpenClaim.Claimid
            'DC290702

            ClaimNo = m_odOpenClaim.ClaimNo
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Update (Public)
    '
    ' Description: Updates a Record in the Database
    '
    ' ***************************************************************** '
    Public Function Update() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_odOpenClaim.Update

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'DC240402 -Start
    ' ***************************************************************** '
    ' Name: AddComments (Public)
    '
    ' Description: Adds a Comments to the Database
    '
    ' ***************************************************************** '
    Public Function AddClaimComments(ByRef vClaimComments As Object) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Try


            m_lReturn = m_odOpenClaim.AddClaimComments(vClaimComments)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddClaimComments Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddClaimComments", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'DC240402 -End
    'DC240402 -Start
    ' ***************************************************************** '
    ' Name: UpdateClaimComments (Public)
    '
    ' Description: Updates Comments to the Database
    '
    ' ***************************************************************** '
    Public Function UpdateClaimComments(ByRef vClaimComments As Object) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Try


            m_lReturn = m_odOpenClaim.UpdateClaimComments(vClaimComments)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateClaimComments Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateClaimComments", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'DC240402 -End
    'DC240402 -Start
    ' ***************************************************************** '
    ' Name: GetClaimComments(Public)
    '
    ' Description: Gets Comments For Claims
    '
    '***************************************************************** '
    Public Function GetClaimComments(ByRef m_vClaimComments As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_odOpenClaim.GetClaimComments(m_vClaimComments)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                Return m_lReturn
            End If

        Catch
        End Try



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimComments Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimComments", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function
    'DC240402 -End

    ' ***************************************************************** '
    ' Name: SetKeyID (Public)
    '
    ' Description: Sets the Claim No to retrieve the Record
    '
    ' ***************************************************************** '
    Public Function SetKeyID(ByVal vvntClaimNo As Object) As Integer

        Dim result As Integer = 0
        Dim bReturn As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bReturn = CheckID(vvntClaimNo)

            If bReturn Then


                m_lReturn = m_odOpenClaim.SetKeyID(vvntClaimNo)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeyID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeyID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: SelectSingle(Public)
    '
    ' Description: Select a Record from the Database based on the SetKeyID
    '               value passed
    '
    ' ***************************************************************** '
    Public Function SelectSingle() As Integer

        Dim result As Integer = 0
        Try


            m_lReturn = m_odOpenClaim.SelectSingle

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectSingle Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectSingle", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetClientDetails(Public)
    '
    ' Description: Gets Client Details using BOLink
    '
    ' ***************************************************************** '
    Public Function GetClientDetails(ByVal vvntPolicyNo As Integer, ByVal vvntClaimDate As String, ByRef rvntResultArray() As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oBOLink.GetClientDetails(rvntResultArray, vvntPolicyNo, vvntClaimDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClientDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetInsurerDetails(Public)
    '
    ' Description: Gets Insurer Details using BOLink
    '
    ' Date :24-07-2000
    '
    ' Edit History :Pandu
    '***************************************************************** '
    Public Function GetInsurerDetails(ByVal vvntPolicyNo As Integer, ByVal vvntClaimDate As String, ByRef rvntResultArray() As Object, Optional ByVal lTransactionMode As Integer = gPMConstants.PMEComponentAction.PMView) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oBOLink.GetInsurerDetails(rvntResultArray, vvntPolicyNo, vvntClaimDate, lTransactionMode)



            'JMK 16/05/2001 - OK not to have an agent, so return either PMTrue or PMNotFound

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                Return m_lReturn
            End If

        Catch
        End Try



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInsurerDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsurerDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function

    'DC251001 -start -added to get details of new insurer selected (BROKING ONLY)
    'DJM 01/05/2002 : Changed whole function to  with both client and insurer and fixed it so that contact numbers are worked out correctly.
    Public Function GetPartyDetails(ByVal v_vShortname As Object, ByVal v_iAddressType As Integer, ByRef r_vResultArray() As Object) As Integer

        Dim result As Integer = 0
        Const COL_PRTY_NM As Integer = 0
        Const COL_PRTY_SHRTNM As Integer = 1
        Const COL_ADD1 As Integer = 2
        Const COL_ADD2 As Integer = 3
        Const COL_ADD3 As Integer = 4
        Const COL_ADD4 As Integer = 5
        Const COL_POSTCODE As Integer = 6
        Const COL_AREACODE As Integer = 7
        Const COL_TELENO As Integer = 8
        Const COL_EXTN As Integer = 9
        Const COL_CONTACT_ID As Integer = 10
        'Const COL_CONTACT_TYPE As Integer = 11
        'Const COL_COUNTRY_ID As Integer = 12
        'Const COL_ADD_USAGE As Integer = 13
        Const COL_ADDRESS_ID As Integer = 13 'AR200504040 - PN15644

        Const TELEPHONE As Integer = 1
        Const FAX As Integer = 2
        Const EMAIL As Integer = 3
        Const MOBILE As Integer = 4
        Const BUSTEL As Integer = 9

        Dim sPrtyNm, sPrtyShrtNm, sAdd1, sAdd2, sAdd3, sAdd4 As String
        Dim sPostCode As String = "", sFullTeleHome As String = "", sFullTeleOff As String = "", sFullTeleBus As String = "", sFullFax As String = "", sFullMobile As String = "", sFullEmail As String = ""
        Dim lAddressId As Integer 'AR200504040 - PN15644 

        Dim vSqlArray(,) As Object = Nothing
        Dim varray1d(13) As Object

        Dim sContactType As String = ""

        Dim bTele As Boolean = False, bFax As Boolean = False, bEmail As Boolean = False, bMobile As Boolean = False

        Dim iTeleCnt As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection

            m_oDatabase.Parameters.Clear()

            'Add the Shortname parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="shortname", vValue:=v_vShortname, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyDetails")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Add the AddressType parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="addresstype", vValue:=v_iAddressType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyDetails")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Execute SQL Statement

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetClmPartyDtlsSQL, sSQLName:=ACGetClmPartyDtlsName, bStoredProcedure:=ACGetClmPartyDtlsStored, vResultArray:=vSqlArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyDetails")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            'Assign the party & Address Details from the 1st row to the variables

            sPrtyShrtNm = CStr(vSqlArray(COL_PRTY_SHRTNM, 0)).Trim()

            sPrtyNm = CStr(vSqlArray(COL_PRTY_NM, 0)).Trim()

            sAdd1 = CStr(vSqlArray(COL_ADD1, 0)).Trim()

            sAdd2 = CStr(vSqlArray(COL_ADD2, 0)).Trim()

            sAdd3 = CStr(vSqlArray(COL_ADD3, 0)).Trim()

            sAdd4 = CStr(vSqlArray(COL_ADD4, 0)).Trim()

            sPostCode = CStr(vSqlArray(COL_POSTCODE, 0)).Trim()
            'AR20050404 - PN15644

            lAddressId = gPMFunctions.ToSafeLong(CStr(vSqlArray(COL_ADDRESS_ID, 0)), 0)

            iTeleCnt = 0


            For iRow As Integer = 0 To vSqlArray.GetUpperBound(1)


                sContactType = CStr(vSqlArray(COL_CONTACT_ID, iRow)).Trim()

                Select Case sContactType
                    Case CStr(TELEPHONE)
                        If iTeleCnt < 2 Then 'if it has not been repeated more than twice
                            If iTeleCnt = 0 Then 'if it is comming for the 1st time

                                sFullTeleHome = CStr(vSqlArray(COL_AREACODE, iRow)).Trim() & " " & CStr(vSqlArray(COL_TELENO, iRow)).Trim() & " " & CStr(vSqlArray(COL_EXTN, iRow)).Trim()
                            ElseIf iTeleCnt = 1 Then  'if it is comming for the 2nd time

                                sFullTeleOff = CStr(vSqlArray(COL_AREACODE, iRow)).Trim() & " " & CStr(vSqlArray(COL_TELENO, iRow)).Trim() & " " & CStr(vSqlArray(COL_EXTN, iRow)).Trim()
                            End If
                            iTeleCnt += 1
                        End If

                    Case CStr(BUSTEL)
                        If Not bTele Then 'if it has not been repeated more than twice

                            sFullTeleBus = CStr(vSqlArray(COL_AREACODE, iRow)).Trim() & " " & CStr(vSqlArray(COL_TELENO, iRow)).Trim() & " " & CStr(vSqlArray(COL_EXTN, iRow)).Trim()
                            bTele = True
                        End If

                    Case CStr(FAX)
                        If Not bFax Then

                            sFullFax = CStr(vSqlArray(COL_AREACODE, iRow)).Trim() & " " & CStr(vSqlArray(COL_TELENO, iRow)).Trim() & " " & CStr(vSqlArray(COL_EXTN, iRow)).Trim()
                            bFax = True
                        End If

                    Case CStr(EMAIL)
                        If Not bEmail Then

                            sFullEmail = CStr(vSqlArray(COL_TELENO, iRow)).Trim()
                            bEmail = True
                        End If

                    Case CStr(MOBILE)
                        If Not bMobile Then

                            sFullMobile = CStr(vSqlArray(COL_AREACODE, iRow)).Trim() & " " & CStr(vSqlArray(COL_TELENO, iRow)).Trim() & "  " & CStr(vSqlArray(COL_EXTN, iRow)).Trim()
                            bMobile = True
                        End If
                End Select

            Next iRow

            'Fill array with values

            varray1d(0) = sPrtyNm

            varray1d(1) = sPrtyShrtNm

            varray1d(2) = sAdd1

            varray1d(3) = sAdd2

            varray1d(4) = sAdd3

            varray1d(5) = sAdd4

            varray1d(6) = sPostCode

            varray1d(7) = sFullTeleHome

            varray1d(8) = sFullTeleOff

            varray1d(9) = sFullFax

            varray1d(10) = sFullMobile

            varray1d(11) = sFullEmail

            varray1d(12) = sFullTeleBus

            varray1d(13) = lAddressId 'AR20050404 - PN15644

            r_vResultArray = varray1d

            ' If NO record was found return Not Found
            If Not Informations.IsArray(r_vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'DC251001 -end

    ' ***************************************************************** '
    '
    ' Name: DeleteClaim
    '
    ' Description:
    '
    ' History: 08/05/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function DeleteClaim() As Integer

        Dim result As Integer = 0
        Dim lStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=m_lClaimid, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="status", vValue:=lStatus, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteClaimSQL, sSQLName:=ACDeleteClaimName, bStoredProcedure:=ACDeleteClaimStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                m_lReturn = m_oDatabase.SQLRollbackTrans()

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            lStatus = m_oDatabase.Parameters.Item("status").value

            If lStatus <> 0 Then

                m_lReturn = m_oDatabase.SQLRollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteClaim", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
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

            m_lReturn = m_oDatabase.SQLBeginTrans

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

            m_lReturn = m_oDatabase.SQLCommitTrans

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

            m_lReturn = m_oDatabase.SQLRollbackTrans

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

    'Private Function CheckMandatory() As Boolean
    '
    'Dim result As Boolean = False
    'Dim bReturn As Boolean
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '

    'bReturn = m_odOpenClaim.CheckMandatory
    '
    '
    'Return bReturn
    '
    'Catch 
    'End Try
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckMandatory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
    '
    'Return result
    '
    'End Function
    ' PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()


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
    ' Name: SetProperties (Public)
    '
    ' Description: Sets the Properties to the Data Object
    'DN 27/03/01 - Added SourceID and LanguageID
    'AB - Added ClaimHandled
    ' ***************************************************************** '
    Public Function SetProperties(ByVal PMMode As Integer, ByVal vvntClaimNo As Object, ByVal vvntPolicyNo As Object, ByVal vvntPolicyID As Object, ByVal vvntDescription As Object, ByVal vvntClaimStatusID As Object, ByVal vvntProgressStatusID As Object, ByVal vvntPrimaryCauseID As Object, ByVal vvntSecondaryCauseID As Object, ByVal vvntCatastropheCodeID As Object, ByVal vvntLossFromDate As Object, ByRef vvntLossToDate As Object, ByVal vvntReportedDate As Object, ByVal vvntReportedToDate As Object, ByVal vvntLastModifiedDate As Object, ByVal vvntHandlerID As Object, ByVal vvntCurrencyID As Object, ByVal vvntInfoOnly As Object, ByVal vvntLikelyClaim As Object, ByVal vvntLocation As Object, ByVal vvntTown As Object, ByVal vvntRiskTypeID As Object, ByVal vvntClientName As Object, ByVal vvntClientAddress As Object, ByVal vvntClientTelNo As Object, ByVal vvntClientFaxNo As Object, ByVal vvntClientMobileNo As Object, ByVal vvntClientEmail As Object, ByVal vvntClientClaimNo As Object, ByVal vvntInsurerName As Object, ByVal vvntInsurerAddress As Integer, ByVal vvntInsurerTelNo As Object, ByVal vvntInsurerFaxNo As Object, ByVal vvntInsurerEmail As Object, ByVal vvntInsurerClaimNo As Object, ByVal vvntInsurerContact As Object, ByVal vvntVATRegistered As Object, ByVal vvntVATRegisteredNo As Object, ByVal vvntComments As Object, ByVal vvntClaimsStatusDate As Object, ByVal vvntClientShortName As Object, ByVal vvntInsurerShortName As Object, ByVal vvntClientTelNooff As Object, ByVal vvntClaimID As Object, ByVal vvntUserDefFldA As Object, ByVal vvntUserDefFldB As Object, ByVal vvntUserDefFldC As Object, ByVal vvntUserDefFldD As Object, ByVal vvntUserDefFldE As Object, ByVal vvntSourceID As Object, ByVal vvntLanguageID As Object, ByVal vvntUnderwritingYearID As Object, ByVal vvntClaimHandled As Object, ByVal v_iUserOtherPartyID As Object, Optional ByVal v_vBaseCaseID As Object = Nothing) As Object

        Dim m_Return As Object = Nothing

        m_Return = m_odOpenClaim.SetProperties(PMMode, vvntClaimNo, vvntPolicyNo, vvntPolicyID, vvntDescription, vvntClaimStatusID, vvntProgressStatusID, vvntPrimaryCauseID, vvntSecondaryCauseID, vvntCatastropheCodeID, vvntLossFromDate, vvntLossToDate, vvntReportedDate, vvntReportedToDate, vvntLastModifiedDate, vvntHandlerID, vvntCurrencyID, vvntInfoOnly, vvntLikelyClaim, vvntLocation, vvntTown, vvntRiskTypeID, vvntClientName, vvntClientAddress, vvntClientTelNo, vvntClientFaxNo, vvntClientMobileNo, vvntClientEmail, vvntClientClaimNo, vvntInsurerName, vvntInsurerAddress, vvntInsurerTelNo, vvntInsurerFaxNo, vvntInsurerEmail, vvntInsurerClaimNo, vvntInsurerContact, vvntVATRegistered, vvntVATRegisteredNo, vvntComments, vvntClaimsStatusDate, vvntClientShortName, vvntInsurerShortName, vvntClientTelNooff, vvntClaimID, vvntUserDefFldA, vvntUserDefFldB, vvntUserDefFldC, vvntUserDefFldD, vvntUserDefFldE, vvntSourceID, vvntLanguageID, vvntUnderwritingYearID, vvntClaimHandled, v_vBaseCaseID, v_iUserOtherPartyID)

        Return m_Return

    End Function

    ' ***************************************************************** '
    ' Name: SetAdditionalProperties
    '
    ' Description: Populate additional claim properties
    '
    ' History: A.Robinson S4B Claim Enhancements R&D 2005
    ' ***************************************************************** '
    Public Function SetAdditionalProperties(ByVal vvDriverTitle As Object, ByVal vvDriverForename As Object, ByVal vvDriverSurname As Object, ByVal vvDatePassedTest As Object, ByVal vvEmployeeTitle As Object, ByVal vvEmployeeForename As Object, ByVal vvEmployeeSurname As Object, ByVal vvEmployeeLengthOfService As Object, ByVal vvEmployeePreviousClaim As Object, ByVal vvEmployeePreviousClaimDetails As Object, ByVal vvULR As Object, ByVal vvRecoveryAgent As Object, ByVal vvSolicitorAppointed As Object, ByVal vvSolicitorName As Object, ByVal vvULRLossDetails As Object, ByVal vvClaimAtFaultId As Object, ByVal vvBonusAffected As Object, ByVal vvPolicyDeductibleId As Object, ByVal vvNonStandardExcess As Object, ByVal vvSubsidiaryCompanyName As Object) As Integer
        Dim m_Return As Object = Nothing

        m_Return = m_odOpenClaim.SetAdditionalProperties(vvDriverTitle, vvDriverForename, vvDriverSurname, vvDatePassedTest, vvEmployeeTitle, vvEmployeeForename, vvEmployeeSurname, vvEmployeeLengthOfService, vvEmployeePreviousClaim, vvEmployeePreviousClaimDetails, vvULR, vvRecoveryAgent, vvSolicitorAppointed, vvSolicitorName, vvULRLossDetails, vvClaimAtFaultId, vvBonusAffected, vvPolicyDeductibleId, vvNonStandardExcess, vvSubsidiaryCompanyName)

        Return m_Return
    End Function

    ' ***************************************************************** '
    ' Name: GetProperties (Public)
    '
    ' Description: Gets the Properties from the Data Object
    'DN 27/03/01 - Added SourceID and LanguageID
    'AB - Added ClaimHandled
    ' ***************************************************************** '
    Public Function GetProperties(ByVal PMMode As Integer, ByRef rsClaimNo As String, ByRef rsPolicyNo As String, ByRef rlPolicyID As Integer, ByRef rsDescription As String, ByRef rlClaimStatusID As Integer, ByRef rlProgressStatusID As Integer, ByRef rlPrimaryCauseID As Integer, ByRef rlSecondaryCauseID As Integer, ByRef rlCatastropheCodeID As Integer, ByRef rsLossFromDate As String, ByRef rsLossToDate As String, ByRef rsReportedDate As String, ByRef rsReportedToDate As String, ByRef rsLastModifiedDate As String, ByRef rlHandlerID As Integer, ByRef rlCurrencyID As Integer, ByRef rnInfoOnly As Integer, ByRef rnLikelyClaim As Integer, ByRef rsLocation As String, ByRef rlTown As Integer, ByRef rlRiskTypeID As Integer, ByRef rsClientName As String, ByRef rsClientAddress As Integer, ByRef rsClientTelNo As String, ByRef rsClientFaxNo As String, ByRef rsClientMobileNo As String, ByRef rsClientEMail As String, ByRef rsClientClaimNo As String, ByRef rsInsurerName As String, ByRef rsInsurerAddress As Integer, ByRef rsInsurerTelNo As String, ByRef rsInsurerFaxNo As String, ByRef rsInsurerEmail As String, ByRef rsInsurerClaimNo As String, ByRef rsInsurerContact As String, ByRef rnVATRegistered As Integer, ByRef rsVATRegisteredNo As String, ByRef rsComments As String, ByRef rsClaimsStatusDate As Object, ByRef rsClientShortName As Object, ByRef rsInsurerShortName As Object, ByRef rsClientTelNoOff As String, ByRef rsUserDefFldA As Integer, ByRef rsUserDefFldB As Integer, ByRef rsUserDefFldC As Integer, ByRef rsUserDefFldD As Integer, ByRef rsUserDefFldE As Integer, ByRef rsSourceID As Integer, ByRef rsLanguageID As Integer, ByRef rvUnderwritingYearID As Object, ByRef rlVersionId As Integer, ByRef rvClaimHandled As Object, Optional ByRef r_sCaseNumber As String = "", Optional ByRef r_lCaseID As Integer = 0, Optional ByRef otherpartyID As Object = Nothing, Optional ByRef otherpartyName As Object = Nothing) As Object

        Dim m_Return As Object = Nothing

        m_Return = m_odOpenClaim.GetProperties(PMMode, rsClaimNo, rsPolicyNo, rlPolicyID, rsDescription, rlClaimStatusID, rlProgressStatusID, rlPrimaryCauseID, rlSecondaryCauseID, rlCatastropheCodeID, rsLossFromDate, rsLossToDate, rsReportedDate, rsReportedToDate, rsLastModifiedDate, rlHandlerID, rlCurrencyID, rnInfoOnly, rnLikelyClaim, rsLocation, rlTown, rlRiskTypeID, rsClientName, rsClientAddress, rsClientTelNo, rsClientFaxNo, rsClientMobileNo, rsClientEMail, rsClientClaimNo, rsInsurerName, rsInsurerAddress, rsInsurerTelNo, rsInsurerFaxNo, rsInsurerEmail, rsInsurerClaimNo, rsInsurerContact, rnVATRegistered, rsVATRegisteredNo, rsComments, rsClaimsStatusDate, rsClientShortName, rsInsurerShortName, rsClientTelNoOff, rsUserDefFldA, rsUserDefFldB, rsUserDefFldC, rsUserDefFldD, rsUserDefFldE, rsSourceID, rsLanguageID, rvUnderwritingYearID, rlVersionId, rvClaimHandled, r_sCaseNumber, r_lCaseID, otherpartyID, otherpartyName)

        Return m_Return
    End Function

    ' ***************************************************************** '
    ' Name: GetAdditionalProperties
    '
    ' Description: Retrieve additional claim properties
    '
    ' History: A.Robinson S4B Claim Enhancements R&D 2005
    ' ***************************************************************** '
    Public Function GetAdditionalProperties(ByRef rsDriverTitle As String, ByRef rsDriverForename As String, ByRef rsDriverSurname As String, ByRef rvDatePassedTest As Object, ByRef rsEmployeeTitle As String, ByRef rsEmployeeForename As String, ByRef rsEmployeeSurname As String, ByRef rlEmployeeLengthOfService As Integer, ByRef rbEmployeePreviousClaim As Boolean, ByRef rsEmployeePreviousClaimDetails As String, ByRef rbULR As Boolean, ByRef rsRecoveryAgent As String, ByRef rbSolicitorAppointed As Boolean, ByRef rsSolicitorName As String, ByRef rsULRLossDetails As String, ByRef rlClaimAtFaultId As Integer, ByRef rbBonusAffected As Boolean, ByRef rlPolicyDeductibleId As Integer, ByRef rdNonStandardExcess As Double, ByRef rsSubsidiaryCompanyName As String) As Integer
        Dim m_Return As Object = Nothing

        m_Return = m_odOpenClaim.GetAdditionalProperties(rsDriverTitle, rsDriverForename, rsDriverSurname, rvDatePassedTest, rsEmployeeTitle, rsEmployeeForename, rsEmployeeSurname, rlEmployeeLengthOfService, rbEmployeePreviousClaim, rsEmployeePreviousClaimDetails, rbULR, rsRecoveryAgent, rbSolicitorAppointed, rsSolicitorName, rsULRLossDetails, rlClaimAtFaultId, rbBonusAffected, rlPolicyDeductibleId, rdNonStandardExcess, rsSubsidiaryCompanyName)
        Return m_Return
    End Function

    ' ***************************************************************** '
    ' Name: GetRiskDetails(Public)
    '
    ' Description: Gets Risk Details using BOLink
    '
    ' ***************************************************************** '
    Public Function GetRiskDetails(ByVal vvntPolicyNo As Integer, ByVal vvntClaimDate As String, ByRef rvntResultArray(,) As Object, Optional ByVal vvntClaimID As Object = Nothing) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    Set g_oObjectManager = New bObjectManager.ObjectManager
            '
            '    g_oObjectManager.Initialise "bOpenClaim"
            '
            '    m_lReturn& = g_oObjectManager.GetInstance( _
            ''        oObject:=m_oBOLink, _
            ''        sClassName:="bBackOfficeLink.bBOLink") ', _
            ''        'vInstanceManager:=PMGetViaClientManager)

            m_lReturn = m_oBOLink.GetRiskDetails(rvntResultArray, vvntPolicyNo, vvntClaimDate, vvntClaimID)

            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                m_lReturn = GetRiskDetailsForClaim(rvntResultArray, vvntPolicyNo)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         GetClmAdd (Public)
    ' Description:  Gets the Claim Address for the given Claim Id
    ' Returns    :  r_vResultArray: (2-dimensional Array) containing
    '               values in the following order
    '               0-Address1,
    '               1-Address2,
    '               2-Address3,
    '               3-Address4,
    '               4-postcode,
    ' Date:         20/06/2000
    ' Author:       SK
    ' ***************************************************************** '
    Public Function GetClmAdd(ByRef r_vResultArray(,) As Object, ByVal v_lAdd_cnt As Integer) As Integer

        'TF120803 - changed datatype to Long

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection

            m_oDatabase.Parameters.Clear()

            'Add the Risk Code Id parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="add_cnt", vValue:=v_lAdd_cnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClmAdd")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Execute SQL Statement

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetClmAddSQL, sSQLName:=ACGetClmAddName, bStoredProcedure:=ACGetClmAddStored, vResultArray:=r_vResultArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClmAdd")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If


            ' If NO record was found return Not Found
            If Not Informations.IsArray(r_vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClmAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClmAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetRiskDetails(Public)
    '
    ' Description: Gets Risk Details using BOLink
    '
    ' ***************************************************************** '
    Public Function GetRiskDesc(ByRef rvntResultArray(,) As Object, ByVal vvntRiskId As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    Set g_oObjectManager = New bObjectManager.ObjectManager
            '
            '    g_oObjectManager.Initialise "bOpenClaim"
            '
            '    m_lReturn& = g_oObjectManager.GetInstance( _
            ''        oObject:=m_oBOLink, _
            ''        sClassName:="bBackOfficeLink.bBOLink") ', _
            ''        'vInstanceManager:=PMGetViaClientManager)

            m_lReturn = m_oBOLink.GetRiskDesc(rvntResultArray, vvntRiskId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskDesc Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskDesc", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateAddress (Public)
    '
    ' Description: Update Address to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Public Function UpdateAddress(ByVal v_lAddress_Cnt As Integer, ByVal v_sAddress1 As String, ByVal v_sAddress2 As String, ByVal v_sAddress3 As String, ByVal v_sAddress4 As String, ByVal v_sPostalCode As String, ByVal v_lAddressUsage As Integer, ByVal v_lAddressId As Integer, Optional ByVal v_lCountryID As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection

                .Parameters.Clear()


                m_lReturn = .Parameters.Add(sName:="address_cnt", vValue:=v_lAddress_Cnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = .Parameters.Add(sName:="source_id", vValue:=m_iSourceID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = .Parameters.Add(sName:="address_id", vValue:=v_lAddressId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = .Parameters.Add(sName:="address1", vValue:=v_sAddress1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = .Parameters.Add(sName:="address2", vValue:=v_sAddress2, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = .Parameters.Add(sName:="address3", vValue:=v_sAddress3, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = .Parameters.Add(sName:="address4", vValue:=v_sAddress4, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = .Parameters.Add(sName:="postal_code", vValue:=v_sPostalCode.ToUpper(), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If



                m_lReturn = .Parameters.Add(sName:="country_id", vValue:=v_lCountryID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = .Parameters.Add(sName:="created_by_id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = .Parameters.Add(sName:="date_created", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If m_iUserID < 1 Then


                    m_lReturn = .Parameters.Add(sName:="modified_by_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                Else

                    m_lReturn = .Parameters.Add(sName:="modified_by_id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = .Parameters.Add(sName:="last_modified", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'DJM 07/05/2002 : Now requires address_usage_type_id (default to correspondence address)

                m_lReturn = .Parameters.Add(sName:="address_usage_type_id", vValue:=v_lAddressUsage, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'AR20050404 - PN15664 No requirement to update other addresses

                m_lReturn = .Parameters.Add(sName:="update_address", vValue:=0, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement

                m_lReturn = .SQLAction(sSQL:=ACUpdateSQL, sSQLName:=ACUpdateName, bStoredProcedure:=ACUpdateStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateAddress Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAddress", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddAddress (Public)
    '
    ' Description: AddAddresss to the Database from the Base Details.
    '
    ' ***************************************************************** '
    'AR20050303 - PN15644 Add AddressUsage,AddressId and UpdateAddress parameters
    Public Function AddAddress(ByVal v_sAddress1 As String, ByVal v_sAddress2 As String, ByVal v_sAddress3 As String, ByVal v_sAddress4 As String, ByVal v_sPostalCode As String, ByVal v_lAddressUsage As Integer, ByVal v_lAddressId As Integer, ByRef r_iAddCnt As Integer, ByVal v_bUpdateAddress As Boolean, Optional ByVal v_lCountryID As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection

                .Parameters.Clear()

                ' AddAddress the required INPUT parameters
                'AR20050303 - PN15644 Pass AddressUsage and AddressId params
                m_lReturn = AddAddressInputParam(v_sAddress1, v_sAddress2, v_sAddress3, v_sAddress4, v_sPostalCode, v_lAddressUsage, v_lAddressId, v_bUpdateAddress, v_lCountryID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Add PrimaryKey as OUTPUT parameters
                m_lReturn = AddAddressKeyOutputParam()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement

                m_lReturn = .SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'DJM 14/06/2002 : Change function to use (long) parameter.
                ' Get the Primary Key of the record inserted
                m_lReturn = NewAddPrimaryKeyID(r_lAddPrimaryKeyID:=r_iAddCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddAddress Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddAddress", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddAddressInputParam (Private)
    '
    ' Description: Adds all of the NON-KEY INPUT parameters
    '              required for an Insert or Update.
    '
    ' ***************************************************************** '
    Private Function AddAddressInputParam(ByVal v_sAddress1 As String, ByVal v_sAddress2 As String, ByVal v_sAddress3 As String, ByVal v_sAddress4 As String, ByVal v_sPostalCode As String, ByVal v_lAddressUsage As Integer, ByVal v_lAddressId As Integer, ByVal v_bUpdateAddress As Boolean, Optional ByVal v_lCountryID As Integer = 0) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase


            m_lReturn = .Parameters.Add(sName:="source_id", vValue:=m_iSourceID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            '              vValue:=SourceID, _
            '
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = .Parameters.Add(sName:="address_id", vValue:=v_lAddressId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            '              vValue:=AddressID, _
            '
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = .Parameters.Add(sName:="address1", vValue:=v_sAddress1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = .Parameters.Add(sName:="address2", vValue:=v_sAddress2, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = .Parameters.Add(sName:="address3", vValue:=v_sAddress3, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = .Parameters.Add(sName:="address4", vValue:=v_sAddress4, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = .Parameters.Add(sName:="postal_code", vValue:=v_sPostalCode.ToUpper(), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'JMK 04/06/2001


            m_lReturn = .Parameters.Add(sName:="country_id", vValue:=v_lCountryID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            m_lReturn = .Parameters.Add(sName:="created_by_id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            '              vValue:=CreatedByID, _
            '
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = .Parameters.Add(sName:="date_created", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            '              vValue:=DateCreated, _
            '
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_iUserID < 1 Then


                m_lReturn = .Parameters.Add(sName:="modified_by_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else

                m_lReturn = .Parameters.Add(sName:="modified_by_id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                '                  vValue:=ModifiedByID,
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = .Parameters.Add(sName:="last_modified", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            '              vValue:=LastModified, _
            '
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DJM 07/05/2002 : Now requires address_usage_type_id (default to correspondence address)

            m_lReturn = .Parameters.Add(sName:="address_usage_type_id", vValue:=v_lAddressUsage, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'AR20050404 - PN15664 Add update_address parameter
            'PN_71897 Start  --- vValue:=IIf(v_bUpdateAddress, 1, 0)

            m_lReturn = .Parameters.Add(sName:="update_address", vValue:=0, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            'PN_71897 End
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function



    ' ***************************************************************** '
    ' Name: AddAddressKeyOutputParam (Private)
    '
    ' Description: Adds all of the PRIMARY KEY OUTPUT parameters
    '              required for an Add.
    '
    ' ***************************************************************** '
    Private Function AddAddressKeyOutputParam() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase


            m_lReturn = .Parameters.Add(sName:="address_cnt", vValue:=0, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        End With

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: NewAddPrimaryKeyID (Private)
    '
    ' Description: Returns the new PRIMARY KEY values from an Add.
    '
    ' DJM 14/06/2002 : Added a (long) parameter to return number.
    ' ***************************************************************** '
    'DC020403 ISS2485 was Long now Integer
    Private Function NewAddPrimaryKeyID(ByRef r_lAddPrimaryKeyID As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase



            If Not (Convert.IsDBNull(.Parameters.Item("address_cnt").value) Or Informations.IsNothing(.Parameters.Item("address_cnt").value)) Then


                r_lAddPrimaryKeyID = .Parameters.Item("address_cnt").value
            Else
                'TO delete remark AddressCnt = 0
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetInsurerDetails(Public)
    '
    ' Description: Gets Insurer Details using BOLink
    '
    ' Date :24-07-2000
    '
    ' Edit History :Pandu
    '***************************************************************** '
    Public Function GetPolicyDetails(ByRef r_vResultArray(,) As Object, ByVal v_lpol_id As Integer, Optional ByVal v_vclm_dt As String = "") As Integer



        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBOLink.GetPolicyDetails(r_vResultArray, v_lpol_id, v_vclm_dt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'The best way I can think of in a _very_ bad situation
            'And this had better be an array...
            If Informations.IsArray(r_vResultArray) Then
                m_lInsuranceFileCnt = v_lpol_id

                m_lInsuranceFolderCnt = CInt(r_vResultArray(3, 0))
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CreateEvent (Private)
    '
    ' Description: Create an event record.
    '
    ' ***************************************************************** '
    Public Function CreateEvent(ByRef r_lEventCnt As Integer, ByVal v_vClaimCnt As Object, ByVal v_vDescription As Object, ByVal v_lEventTypeId As Object, ByVal v_lPartyid As Integer) As Integer

        Dim result As Integer = 0
        Dim vInsuranceFileCnt As Integer
        Dim vInsuranceFolderCnt As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oEvent Is Nothing Then
                m_oEvent = New bSIREvent.Business()

                m_lReturn = m_oEvent.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the event object", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result
                End If
            End If

            If m_lInsuranceFileCnt = 0 Then

                vInsuranceFileCnt = Nothing
            Else
                vInsuranceFileCnt = m_lInsuranceFileCnt
            End If

            If m_lInsuranceFolderCnt = 0 Then

                vInsuranceFolderCnt = Nothing
            Else
                vInsuranceFolderCnt = m_lInsuranceFolderCnt
            End If


            m_lReturn = m_oEvent.DirectAdd(vEventCnt:=r_lEventCnt, vPartyCnt:=v_lPartyid, vInsuranceFolderCnt:=vInsuranceFolderCnt, vInsuranceFileCnt:=vInsuranceFileCnt, vClaimCnt:=v_vClaimCnt, vDocumentCnt:=DBNull.Value, vOldAddressCnt:=DBNull.Value, vNewAddressCnt:=DBNull.Value, vCampaignId:=DBNull.Value, vDocumentType:=DBNull.Value, vReportType:=DBNull.Value, vEventType:=v_lEventTypeId, vUserId:=m_iUserID, vEventDate:=DateTime.Today, vDescription:=v_vDescription)



            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the event", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetOption (Private)
    '
    ' Description: Get an option.
    '
    ' ***************************************************************** '
    Public Function GetOption(ByVal v_iOptionNumber As Integer, ByRef r_nOptionValue As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim m_oSystemOption As bSIROptions.Business = Nothing
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

                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the event", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            If sOptionValue = "" Then
                r_nOptionValue = 0
            Else
                r_nOptionValue = gPMFunctions.NullToInteger(sOptionValue)
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name:         GetUserDefinedCaption (Public)
    ' Description:  Gets the UserDefined Caption From the gis_user_def_header
    ' Returns    :  r_vResultArray: (2-dimensional Array) containing
    ' Date       :  20/10/2000
    ' Author     :  Pandu
    ' ***************************************************************** '
    Public Function GetUserDefinedCaption(ByRef r_vResultArray(,) As Object, ByVal Tableid As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection

            m_oDatabase.Parameters.Clear()

            'Add the Risk Code Id parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Tableid", vValue:=Tableid, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserDefinedCaption")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Execute SQL Statement

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetUserDefinedCaptionSQL, sSQLName:=ACGetUserDefinedCaptionName, bStoredProcedure:=ACGetUserDefinedCaptionStored, vResultArray:=r_vResultArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserDefinedCaption")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If


            ' If NO record was found return Not Found
            If Not Informations.IsArray(r_vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserDefinedCaption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserDefinedCaption", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: UnderwritingOrAgency
    '
    ' Description:  Finds out if Underwriting or Agency business
    '
    ' Created By:  'RWH(09/11/2000) For Claims Numbering.
    '
    ' SJP14062002 - getUnderWritingOrAgency uses new product options scheme
    ' ***************************************************************** '
    Public Function getUnderwritingOrAgency() As Integer

        Dim result As Integer = 0
        Try

            'PSL This sub has been moved 20/10/2003

            Return bPMFunc.getUnderwritingOrAgency(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppNAme, m_sUnderwritingOrAgency)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnderwritingOrAgencyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnderwritingOrAgency", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetInsuranceFolderCnt
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : Prabodh : 02-02-2007 : PN32798
    ' ***************************************************************** '
    Public Function GetInsuranceFolderCnt(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetInsuranceFolderCnt"

        'Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=kGetInsuranceFolderCntSQL, sSQLName:=kGetInsuranceFolderCntName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetInsuranceFolderCntSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetInsuranceFile
    '
    ' Description: Retrieves the InsuranceFile record for the current
    '               PolicyId. (PolicyId = InsuranceFileCnt)
    '
    ' History: 10/11/2000 RWH - Created.
    ' RAM20030729   : Changed the datatype from PMInteger to PMLong for the
    '                 Parameter 'event_cnt'. Ref. Issuue No. 4640
    ' ***************************************************************** '
    Public Function GetInsuranceFile(ByVal v_lEventCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lInsuranceFileCnt As Integer

        'DC070201 get from insurance_file and not event_log
        'Const sSQL = "SELECT insurance_file_cnt FROM Event_log WHERE event_cnt = {event_cnt}"
        'Const sSQL1 As String = "SELECT insurance_file_cnt FROM Event_log WHERE event_cnt = {event_cnt}"
        'Const sSQL2 As String = "SELECT insurance_file_cnt FROM insurance_file WHERE insurance_file_cnt = {event_cnt}"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'TN20001207 - Start
            'only get insurance_file_cnt from event log for Broking system

            lInsuranceFileCnt = v_lEventCnt

            'Clear the Database Parameters Collection

            m_oDatabase.Parameters.Clear()

            'Add the Risk Code Id parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsuranceFile")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Execute SQL Statement

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectInsuranceFileSQL, sSQLName:=ACSelectInsuranceFileName, bStoredProcedure:=ACSelectInsuranceFileStored, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsuranceFile")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' If NO record was found return Not Found
            If Not Informations.IsArray(r_vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInsuranceFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsuranceFile", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetCoInsurerDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : E Knott : 11-2005 : Datasure
    ' ***************************************************************** '
    Public Function GetCoInsurerDetails(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lClaimId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetCoInsurerDetails"

        ' Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            bPMAddParameter.AddParameterLite(m_oDatabase, "InsuranceFileCnt", v_lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            'DC070606 Change the way Coinsurers are used in claims for datasure
            bPMAddParameter.AddParameterLite(m_oDatabase, "ClaimId", v_lClaimId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=kGetCoInsurerDetailsSQL, sSQLName:=kGetCoInsurerDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetCoInsurerDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: GenerateClaimNumber
    '
    ' Description: Uses the PolicyNumMaint component to generate a claim
    '               number.
    '
    ' History:  10/11/2000 RWH  - Created.
    '           15/10/2001 JMK  - add 2 optional claim date parameters
    ' ***************************************************************** '
    Public Function GenerateClaimNumber(ByVal v_lBusinessType As Integer, ByVal v_iBranchId As Integer, ByVal v_lProductID As Integer, ByVal v_lAgentId As Integer, ByRef r_sGeneratedClaimNumber As String, ByVal v_sLossYear As String, ByVal v_sReportedYear As String, Optional ByVal v_nPartyCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oPolicyNumMaint Is Nothing Then
                m_oPolicyNumMaint = New bSIRPolicyNumMaint.Business()
            End If
            'developer guide no.9
            m_lReturn = m_oPolicyNumMaint.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppNAme)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' JMK 15/10/2001
            'Start - Renuka - (WPR87 Paralleling)
            'developer guide no.70
            m_lReturn = m_oPolicyNumMaint.GeneratePolicyNumber(v_lBusinessType, v_iBranchId, v_lProductID, v_lAgentId, r_sGeneratedClaimNumber, v_sLossYear, v_sReportedYear, DateTime.Parse(DateTime.Now), v_lPartyCnt:=v_nPartyCnt)
            'End - Renuka - (WPR87 Paralleling)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateClaimNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateClaimNumber", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '***********************************************************************
    ' Name : GetOriginalClaimID
    '
    ' Desc : get the original claim ID from  table
    '
    ' Hist : 15 June 2001 Tinny - Created
    '***********************************************************************
    Public Function GetOriginalClaimNo(ByVal v_lClaimId As Integer, ByRef r_lOriginalClaimID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase.Parameters.Clear()


            result = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=v_lClaimId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            result = m_oDatabase.SQLSelect(sSQL:=ACGetOriginalClaimIDSQL, sSQLName:=ACGetOriginalClaimIDName, bStoredProcedure:=ACGetOriginalClaimIDStored, vResultArray:=vResultArray)


            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            r_lOriginalClaimID = gPMFunctions.ToSafeLong(CStr(vResultArray(0, 0)))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOriginalClaimNo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOriginalClaimNo", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '***********************************************************************
    ' Name : CheckClaimNumber
    '
    ' Desc : Is this claim number already in use?
    '
    ' Hist : 19 April 2002 by Tomo
    '***********************************************************************
    Public Function CheckClaimNumber(ByVal v_sClaimNumber As String, ByRef r_lClaimID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_lClaimID = 0

            sSQL = "SELECT Claim_id FROM Claim WHERE Claim_Number = {Claim_Number}"

            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_Number", vValue:=v_sClaimNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="CheckClaimNumber", bStoredProcedure:=False, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vResultArray) Then

                r_lClaimID = CInt(vResultArray(0, 0))
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckClaimNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckClaimNumber", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '***********************************************************************
    ' Name : GetValidPrimaryCauses
    '
    ' Desc : get the the valid primary causes for a product
    '
    ' Hist : 08 Jan 2003 AMB - Created
    '      : 08 Apr 2003 RDT - Added to 1.8.6 Branch
    '***********************************************************************
    Public Function GetValidPrimaryCauses(ByVal v_lInsFileCnt As Integer, ByRef r_vValidPrimaryCauses As Object) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase.Parameters.Clear()


            result = m_oDatabase.Parameters.Add(sName:="ins_file_cnt", vValue:=v_lInsFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            'PN: 73770
            result = m_oDatabase.Parameters.Add(sName:="mode", vValue:=m_iTask, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = m_oDatabase.SQLSelect(sSQL:=ACGetValidPrimaryCausesSQL, sSQLName:=ACGetValidPrimaryCausesName, bStoredProcedure:=ACGetValidPrimaryCausesStored, vResultArray:=vResultArray)


            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If



            r_vValidPrimaryCauses = vResultArray

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetValidPrimaryCauses Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetValidPrimaryCauses", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '***********************************************************************
    ' Name : GetPolicyStatus
    '
    ' Desc : get policy status
    '
    ' Hist : 23 August 2001 Tinny - Created
    '***********************************************************************
    Public Function GetPolicyStatus(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lPolicyStatus As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT insurance_file_status_id FROM insurance_file" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE insurance_file_cnt = {insurance_file_cnt}"


            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
                result = gPMConstants.PMEReturnCode.PMFalse
            End If



            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetPolicyStatus", bStoredProcedure:=False, vResultArray:=vResultArray, bKeepNulls:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Dim auxVar As Object = vResultArray(0, 0)


            If Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar) Then
                r_lPolicyStatus = 0
                Return result
            End If


            r_lPolicyStatus = CInt(vResultArray(0, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function

    'MSS240901 - Added below procedures from SFORB for merge

    'AK - 180401
    'Function to return Policy Type for a given Policy

    Public Function GetPolicyType(ByVal v_lPolicyId As Integer, ByRef r_sType As String) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing


        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            'Clear the Database Parameters Collection

            m_oDatabase.Parameters.Clear()

            'Add the Risk Code Id parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_id", vValue:=v_lPolicyId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyType")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Execute SQL Statement

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPolicyTypeSQL, sSQLName:=ACGetPolicyTypeName, bStoredProcedure:=ACGetPolicyTypeStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyType")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vArray) Then

                r_sType = CStr(vArray(0, 0)).Trim()
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function


    '---------------------------------------------------------------------------------------
    ' Procedure : GetClaimRiskStatus
    ' DateTime  : 10 Sep 03 16:07
    ' Author    : AMB
    ' Purpose   : Returns TRUE if the risk_status of the risk associated with the claim
    '             is 'Reinsurance Deferred'
    '--------------------------------------------------------------------------------------
    Public Function GetClaimRiskStatus(ByVal v_lRiskCnt As Integer, ByRef r_bIsDeferred As Boolean) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing

        Try

            'Clear the Database Parameters Collection

            m_oDatabase.Parameters.Clear()

            'Add the Risk Code Id parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=v_lRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimRiskStatus")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Execute SQL Statement

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetClaimRiskStatusSQL, sSQLName:=ACGetClaimRiskStatusName, bStoredProcedure:=ACGetClaimRiskStatusStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimRiskStatus")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' check the status returned
            If Informations.IsArray(vArray) Then

                r_bIsDeferred = gPMFunctions.NullToString(vArray(1, 0)).Trim().ToUpper() = ksDeferredRIRiskStatus
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimRiskStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimRiskStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    'AK - 310701
    'Function to Check if the policy is in Renewal Cycle
    ' Returns Insurance_Folder_Cnt, if yes else returns 0

    Public Function CheckRenewal(ByVal v_lInsurance_File_Cnt As Integer, ByRef r_lInsurance_Folder_Cnt As Integer, Optional ByRef r_lRenewal_Frequency_Id As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing


        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            'Default the insurance folder count to 0
            r_lInsurance_Folder_Cnt = 0

            'Clear the Database Parameters Collection

            m_oDatabase.Parameters.Clear()

            'Add the Risk Code Id parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_lInsurance_File_Cnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckRenewal")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Execute SQL Statement

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckRenewalsSQL, sSQLName:=ACCheckRenewalsName, bStoredProcedure:=ACCheckRenewalsStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckRenewal")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vArray) Then

                r_lInsurance_Folder_Cnt = CInt(CStr(vArray(0, 0)).Trim())

                r_lRenewal_Frequency_Id = CInt(CStr(vArray(1, 0)).Trim())
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckRenewal Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckRenewal", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetExistingRenQuotesToReplaced
    '
    ' Description:
    '
    ' History: 03/08/2001 AK - Created.
    '
    ' ***************************************************************** '
    Public Function SetExistingRenQuotesToReplaced(ByRef v_lInsuranceFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lRecordsAffected As Integer

            ' Clear the Parameters

            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=v_lInsuranceFolderCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACSetExistRenToRepSQL, sSQLName:=ACSetExistRenToRepName, bStoredProcedure:=ACSetExistRenToRepStored, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetExistingRenQuotesToReplaced Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetExistingRenQuotesToReplaced", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: UpdateRenewalStatusType
    '
    ' Description:  Calls an SP to update the Renewal_Status_Type for
    '               a Renewal_Control row.
    '
    ' AK - 03/08/01 created
    ' RAM20030729   : Changed the datatype from PMInteger to PMLong for the
    '                 Parameter 'insurance_folder_cnt'. Ref. Issuue No. 4640
    ' ***************************************************************** '
    Public Function UpdateRenewalStatusType(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_sRenewalStatusTypeCode As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase.Parameters.Clear()

            ' Add the Insurance Folder Cnt parameter (INPUT)
            ' RAM20030729   : Changed the datatype from PMInteger to PMLong
            '                   Ref. Issue No. 4640

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=v_lInsuranceFolderCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Insurance Folder Cnt parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="renewal_status_type_code", vValue:=v_sRenewalStatusTypeCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateRenewalControlSQL, sSQLName:=ACUpdateRenewalControlName, bStoredProcedure:=ACUpdateRenewalControlStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRenewalStatusType SQL Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRenewalStatusType", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRenewalStatusType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRenewalStatusType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetShowBrokingRiskDetails
    '
    ' Description: Calls an sp to fetch back broking risk details.
    '
    ' CJB - 05/09/02 Created - Added from the old SourceSafe as part of
    '       parallel maintenance that had been missed...
    '
    ' ***************************************************************** '
    Public Function GetShowBrokingRiskDetails(ByVal v_lClaimId As Integer, ByRef r_vDataArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase.Parameters.Clear()
            'DC130701 was iClaimId now lClaimId

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimId", vValue:=v_lClaimId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure.

            m_lReturn = m_oDatabase.SQLSelect(ACGetShowBRRiskDetailsSQL, ACGetShowBRRiskDetailsName, ACGetShowBRRiskDetailsStored, , r_vDataArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetShowBrokingRiskDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetShowBrokingRiskDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetClientPolicyDetails
    '
    ' Description:
    '
    ' History: 02/10/2002 sj - Created.
    '
    ' ***************************************************************** '
    Public Function GetClientPolicyDetails(ByVal v_lInsuranceFileCnt As Integer, Optional ByRef r_lPartyCnt As Integer = 0, Optional ByRef r_lInsuranceFolderCnt As Integer = 0, Optional ByRef r_sInsuranceRef As String = "", Optional ByRef r_vRenewalDate As String = "", Optional ByRef r_lPolicyTypeId As Integer = 0, Optional ByRef r_sPartyShortName As String = "") As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vResultArray(,) As Object = Nothing


            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add Failed for insurance_file_cnt " & v_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientPolicyDetails")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetClientPolicyDetailsSQL, sSQLName:=ACGetClientPolicyDetailsName, bStoredProcedure:=ACGetClientPolicyDetailsStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.SQLSelect Failed calling " & ACGetClientPolicyDetailsSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientPolicyDetails")
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            r_lPartyCnt = CInt(vResultArray(0, 0))
            'DN 09/12/02 ISS 1454

            r_lInsuranceFolderCnt = CInt(vResultArray(2, 0))

            r_sInsuranceRef = CStr(vResultArray(3, 0))
            'S4B Claim Enhancements R&D 2005

            r_vRenewalDate = CStr(vResultArray(4, 0))

            r_lPolicyTypeId = gPMFunctions.ToSafeLong(CStr(vResultArray(5, 0)), 0)

            r_sPartyShortName = CStr(vResultArray(1, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClientPolicyDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientPolicyDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetDefaultContacts
    '
    ' Description: Retrieves default contact details
    '
    ' ***************************************************************** '
    Public Function GetDefaultContacts(ByVal v_lPolicyId As Integer, ByRef r_vResults() As Object, ByVal v_bIsClient As Boolean) As Integer

        Dim result As Integer = 0
        Dim sSQLSearch As String = ""
        Dim vContactResults(,) As Object = Nothing
        Dim vContact As Object = Nothing
        Dim sContactType As String = ""

        Const ACConTelephoneHomeArea As Integer = 0
        Const ACConTelephoneHomeNumber As Integer = 1
        Const ACConTelephoneHomeExt As Integer = 2

        Const ACConTelephoneOfficeArea As Integer = 3
        Const ACConTelephoneOfficeNumber As Integer = 4
        Const ACConTelephoneOfficeExt As Integer = 5

        Const ACConFaxArea As Integer = 6
        Const ACConFaxNumber As Integer = 7
        Const ACConFaxExt As Integer = 8

        Const ACConMobileArea As Integer = 9
        Const ACConMobileNumber As Integer = 10
        Const ACConMobileExt As Integer = 11

        Const ACConEmail As Integer = 12

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQLSearch = "SELECT [cnt].code, [con].area_code," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQLSearch = sSQLSearch & "[con].number, [con].extension" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQLSearch = sSQLSearch & "FROM contact [con]" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQLSearch = sSQLSearch & "LEFT JOIN contact_type [cnt] ON [cnt].contact_type_id = [con].contact_type_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQLSearch = sSQLSearch & "LEFT JOIN party_contact_usage [pcu] ON [pcu].contact_cnt = [con].contact_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQLSearch = sSQLSearch & "LEFT JOIN party [pty] ON [pty].party_cnt = [pcu].party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)

            If v_bIsClient Then
                'Get contact details linked to client
                sSQLSearch = sSQLSearch & "LEFT JOIN insurance_file [ifi] ON [ifi].insured_cnt = [pty].party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            Else
                'Else must contact details linked to insurer.
                sSQLSearch = sSQLSearch & "LEFT JOIN insurance_file [ifi] ON [ifi].lead_insurer_cnt = [pty].party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            End If

            sSQLSearch = sSQLSearch & "WHERE [ifi].insurance_file_cnt = " & CStr(v_lPolicyId)

            With m_oDatabase

                .Parameters.Clear()


                m_lReturn = .SQLSelect(sSQL:=sSQLSearch, sSQLName:="GetContactDetails", bStoredProcedure:=False, vResultArray:=vContactResults)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve contact details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultContacts")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            If Informations.IsArray(vContactResults) Then
                ReDim vContact(12)

                For iLoop As Integer = vContactResults.GetLowerBound(1) To vContactResults.GetUpperBound(1)
                    ' Take first three letters of contact type code for identifying what
                    ' has been retrieved.  As this is from a look up list there is the
                    ' possibility that the list of contact types could change, so several
                    ' other options for each type have been entered here.

                    sContactType = CStr(vContactResults(0, iLoop)).Substring(0, 3)

                    Select Case sContactType.ToUpper()
                        Case "TEL", "HOM" 'Home, Telephone


                            vContact(ACConTelephoneHomeArea) = vContactResults(1, iLoop)


                            vContact(ACConTelephoneHomeNumber) = vContactResults(2, iLoop)


                            vContact(ACConTelephoneHomeExt) = vContactResults(3, iLoop)
                        Case "FAX", "FAC" 'FAX, Facsimile


                            vContact(ACConFaxArea) = vContactResults(1, iLoop)


                            vContact(ACConFaxNumber) = vContactResults(2, iLoop)


                            vContact(ACConFaxExt) = vContactResults(3, iLoop)
                        Case "WAP", "BUS", "OFF" 'WAP, Business, Office


                            vContact(ACConTelephoneOfficeArea) = vContactResults(1, iLoop)


                            vContact(ACConTelephoneOfficeNumber) = vContactResults(2, iLoop)


                            vContact(ACConTelephoneOfficeExt) = vContactResults(3, iLoop)
                        Case "E-M", "EMA" 'E-mail, Email


                            vContact(ACConEmail) = vContactResults(2, iLoop)
                        Case "MOB" 'Mobile


                            vContact(ACConMobileArea) = vContactResults(1, iLoop)


                            vContact(ACConMobileNumber) = vContactResults(2, iLoop)


                            vContact(ACConMobileExt) = vContactResults(3, iLoop)
                    End Select
                Next iLoop
            End If

            If Informations.IsArray(vContact) Then

                r_vResults = vContact
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section
            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaultContacts Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultContacts", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '*******************************************************************************
    ' Name : GetPolicyForClaimDate
    ' Desc : get closest version of policy for claim date
    ' Hist : 23 May 2001 - Created  Tinny
    '        15/02/2004 - Alix - Process Sheet 52/53
    '*******************************************************************************
    Public Function GetPolicyForClaimDate(ByVal v_dtClaimDate As Date, ByRef r_lInsuranceFileCnt As Integer, ByRef r_sPolicyNumber As String, ByRef r_dtStartDate As Date, ByRef r_dtEndDate As Date, Optional ByRef r_lReturnCode As Integer = 0, Optional ByRef r_dtInceptionDate As Date = #12:00:00 PM#) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim lCurrentPosition As Integer

        Dim dtFirstStartDate, dtLastExpiryDate As Date
        Dim bFoundVoid, bFoundDate As Boolean

        Const ACInsuranceFileCnt As Integer = 0
        Const ACStartDate As Integer = 1
        Const ACEndDate As Integer = 2
        Const ACPolicyRef As Integer = 3
        Const ACLapsedReason As Integer = 4

        Const ReturnCode_Error As Integer = 0
        Const ReturnCode_Ok As Integer = 1
        Const ReturnCode_TooEarly As Integer = 2
        Const ReturnCode_TooLate As Integer = 3
        Const ReturnCode_Voided As Integer = 4

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Pass insurance file cnt in

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=r_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                r_lReturnCode = ReturnCode_Error
                Return result
            End If

            ' Get all policy versions

            result = m_oDatabase.SQLSelect(sSQL:=ACGetPolicyForClaimDateSQL, sSQLName:=ACGetPolicyForClaimDateName, bStoredProcedure:=ACGetPolicyForClaimDateStored, vResultArray:=vResultArray, bKeepNulls:=False)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                r_lReturnCode = ReturnCode_Error
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                r_lReturnCode = ReturnCode_Error
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Get start and end date of current policy version

            For lCount As Integer = 0 To vResultArray.GetUpperBound(1)

                If r_lInsuranceFileCnt = CDbl(vResultArray(ACInsuranceFileCnt, lCount)) Then

                    r_dtStartDate = CDate(vResultArray(ACStartDate, lCount))

                    r_dtEndDate = CDate(vResultArray(ACEndDate, lCount))
                    bFoundDate = True
                    Exit For
                End If
            Next lCount

            ' if we havent found a date yet then
            If Not bFoundDate Then

                For lCount As Integer = 0 To vResultArray.GetUpperBound(1)

                    r_dtStartDate = CDate(vResultArray(ACStartDate, lCount))

                    r_dtEndDate = CDate(vResultArray(ACEndDate, lCount))
                    bFoundDate = True
                    Exit For
                Next lCount
            End If

            ' Find a version of the policy which encompasses the claim date
            lCurrentPosition = -1

            For lCount As Integer = 0 To vResultArray.GetUpperBound(1)


                If (CDate(vResultArray(ACStartDate, lCount)) <= v_dtClaimDate) And (v_dtClaimDate <= CDate(vResultArray(ACEndDate, lCount))) Then

                    If CStr(vResultArray(ACLapsedReason, lCount)).Trim() <> "VOIDED" Then
                        ' We found a valid one, note its position and exit!
                        lCurrentPosition = lCount
                        Exit For
                    Else
                        ' We found a void one, we carry on searching but note this to report to user
                        bFoundVoid = True
                    End If
                End If
                ' Save the earliest start date and the latest expiry date

                If CDate(vResultArray(ACStartDate, lCount)) < dtFirstStartDate Or lCount = 0 Then

                    dtFirstStartDate = CDate(vResultArray(ACStartDate, lCount))
                End If

                If CDate(vResultArray(ACEndDate, lCount)) > dtLastExpiryDate Or lCount = 0 Then

                    dtLastExpiryDate = CDate(vResultArray(ACEndDate, lCount))
                End If
            Next lCount

            'Commented as per 1.13SR2, not required. InceptionDate not commented because it is used for message purpose.
            'r_lInsuranceFileCnt = 0
            r_dtInceptionDate = dtFirstStartDate

            If lCurrentPosition <> -1 Then

                ' Found one, we returns its details

                r_sPolicyNumber = CStr(vResultArray(ACPolicyRef, lCurrentPosition))

                r_lInsuranceFileCnt = CInt(vResultArray(ACInsuranceFileCnt, lCurrentPosition))

                r_lReturnCode = ReturnCode_Ok

            Else

                ' Else, we check why we haven't found one, to report to user
                If v_dtClaimDate > dtLastExpiryDate Then
                    ' The claim date if after the latest expiry date
                    r_lReturnCode = ReturnCode_TooLate
                ElseIf v_dtClaimDate < dtFirstStartDate Then
                    ' The claim date is before the earliest start date
                    r_lReturnCode = ReturnCode_TooEarly
                ElseIf bFoundVoid Then
                    ' We found one but it was VOID
                    r_lReturnCode = ReturnCode_Voided
                Else
                    ' We can't find any policy, probably because the claim date is in between
                    ' two valid policy versions
                    r_lReturnCode = ReturnCode_Error
                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyForClaimDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyForClaimDate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '*******************************************************************************
    ' Name : GetClaimTaskUserGroup
    ' Desc : Returns claims user group ID and task group ID
    ' Hist : Alix - 25/02/2004
    '*******************************************************************************
    Public Function GetClaimTaskUserGroup(ByRef r_lTaskGroupID As Integer, ByRef r_lUserGroupID As Integer) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            'Clear the Database Parameters Collection

            m_oDatabase.Parameters.Clear()

            'Execute SQL Statement

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetClaimUserTaskGroupSQL, sSQLName:=ACGetClaimUserTaskGroupName, bStoredProcedure:=ACGetClaimUserTaskGroupStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimTaskUserGroup")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vArray) Then

                If CStr(vArray(0, 0)).Trim() = "" Then
                    r_lTaskGroupID = 0
                Else

                    r_lTaskGroupID = CInt(CStr(vArray(0, 0)).Trim())
                End If

                If CStr(vArray(1, 0)).Trim() = "" Then
                    r_lUserGroupID = 0
                Else

                    r_lUserGroupID = CInt(CStr(vArray(1, 0)).Trim())
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimTaskUserGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimTaskUserGroup", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function


    Public Function GetDefaultUnderwritingYear(ByVal v_lPolicyId As Integer, ByRef r_vUnderwritingYearID As Object) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetDefaultUnderwritingYear
        ' PURPOSE:
        ' AUTHOR: Danny Davis
        ' DATE: 29 March 2004, 12:00:34
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                .Parameters.Add("insurance_file_cnt", v_lPolicyId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                If .SQLSelect("spe_insurance_file_sel", "Get default underwriting year", True) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Return the result

                'developer guide no.111
                r_vUnderwritingYearID = .Records.Item(0).Fields("underwriting_year_id")
            End With


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultUnderwritingYear", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: RetrieveCurrenciesForBranch
    '
    ' Description: get currencies used by branch
    '
    ' History:
    '     RDC 02062004 created
    ' ***************************************************************** '
    Public Function RetrieveCurrenciesForBranch(ByRef iSourceID As Integer, ByRef vReturnArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_lReturn = bPMFunc.GetBranchCurrencies(v_iSourceID:=iSourceID, v_oDatabase:=m_oDatabase, r_vReturnArray:=vReturnArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Informations.IsArray(vReturnArray) Then
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            'Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RetrieveCurrenciesForBranch failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RetrieveCurrenciesForBranch", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '***********************************************************************
    ' Name : GetClaimNumber
    '
    ' Desc : get the claim number for the passed claim
    '
    ' Hist : 4th July 2003 RVH - Created
    '***********************************************************************
    Public Function GetClaimNumber(ByVal v_lClaimId As Integer, ByRef r_sClaimNumber As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase.Parameters.Clear()


            result = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=v_lClaimId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            result = m_oDatabase.SQLSelect(sSQL:=ACGetClaimNumberSQL, sSQLName:=ACGetClaimNumberName, bStoredProcedure:=ACGetClaimNumberStored, vResultArray:=vResultArray)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            r_sClaimNumber = CStr(vResultArray(0, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimNumber", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetCancellationDate
    '
    ' Parameters: n/a
    '
    ' Description: Returns the cancellation date that needs to be used
    '               by the specified insurance file
    '
    ' History:
    '           Created : MEvans : 01-04-2005 : PN14529
    ' ***************************************************************** '
    Public Function GetCancellationDate(ByVal v_lInsuranceFileCnt As Integer, ByRef r_dtCancellationDate As Date) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetCancellationDate"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim llBound, lUBound As Integer
        Dim vPolicyVersions(,) As Object = Nothing
        Dim tmpCancellationDate, origCoverStartDate As Date

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get all policy versions that occur after the specified insurance file
            lReturn = CType(GetPolicyVersions(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vResults:=vPolicyVersions), gPMConstants.PMEReturnCode)

            ' raise error and quit if no policy versions found
            ' NB: this includes the specified version so it always should return one item at least
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetPolicyVersions Failed for Insurance File Cnt:" & v_lInsuranceFileCnt, gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(vPolicyVersions) Then


                llBound = vPolicyVersions.GetLowerBound(1)

                lUBound = vPolicyVersions.GetUpperBound(1)

                ' for each policy version
                For lPolicyVersion As Integer = llBound To lUBound

                    tmpCancellationDate = CDate("00:00:00")

                    ' store the original cover start date as this process is looking for
                    ' cancellation dates that occur after or equal to this date
                    If lPolicyVersion = llBound Then

                        origCoverStartDate = CDate(vPolicyVersions(kPolVersArrayItemCoverStartDate, lPolicyVersion))
                    End If

                    ' if the item is a cancellation

                    If CDbl(vPolicyVersions(kPolVersArrayItemTypeId, lPolicyVersion)) = kInsFileTypeMTACancellation Then

                        ' is there a policy version after this one
                        If lUBound >= lPolicyVersion + 1 Then

                            ' is it a reinstatement

                            If CDbl(vPolicyVersions(kPolVersArrayItemTypeId, lPolicyVersion + 1)) = kInsFileTypeMTAReinstatement Then

                                ' is the reinstatement date different to the cancellation date


                                If Not vPolicyVersions(kPolVersArrayItemCoverStartDate, lPolicyVersion).Equals(vPolicyVersions(kPolVersArrayItemCoverStartDate, lPolicyVersion + 1)) Then
                                    ' is there a period when the policy isnt providing cover (dead zone)
                                    ' e.g.      LIVE POL 01/01
                                    '      Cancelled POL 01/02
                                    '      Reinstate POL 01/03
                                    ' Therefore the period 01/02 to 01/03 is a dead zone (a period without cover - so no claims can be made)



                                    If vPolicyVersions(kPolVersArrayItemCoverStartDate, lPolicyVersion) < vPolicyVersions(kPolVersArrayItemCoverStartDate, lPolicyVersion + 1) Then

                                        tmpCancellationDate = CDate(vPolicyVersions(kPolVersArrayItemCoverStartDate, lPolicyVersion))
                                    End If

                                    ' If the reinstament date is the same as the cancellation date then
                                    ' the period of cover is uninterupted as the cancellation and reinstament effectively
                                    ' cancel each other out; so although the policy has its status marked as cancelled
                                    ' it effectively isnt, so there wont be a cancellation date...
                                Else
                                    ' do nothing
                                End If


                            End If

                        Else
                            ' if not the cover start date of the cancellation is the cancellation
                            ' date of the live policy

                            tmpCancellationDate = CDate(vPolicyVersions(kPolVersArrayItemCoverStartDate, lPolicyVersion))
                        End If
                    End If

                    ' if the cancellation date has been modified
                    If tmpCancellationDate <> CDate("00:00:00") Then
                        ' This process is looking for the earliest possible cancellation date
                        ' after the original (specified) policy's versions cover start date
                        If (tmpCancellationDate < r_dtCancellationDate) Or r_dtCancellationDate = CDate("00:00:00") Then
                            r_dtCancellationDate = tmpCancellationDate
                        End If
                    End If

                Next

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: GetPolicyVersions
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-04-2005 : PN19529
    ' ***************************************************************** '
    Private Function GetPolicyVersions(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPolicyVersions"

        'Dim lReturn As Integer




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters

        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        m_lReturn = AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong)

        ' Execute selection Query

        If m_oDatabase.SQLSelect(sSQL:=kGetPolicyVersionsSQL, sSQLName:=kGetPolicyVersionsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

            gPMFunctions.RaiseError(kMethodName, kGetPolicyVersionsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

        End If

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: AddInputParameter
    '
    ' Parameters: v_sName   : Parameter Name
    '             v_vValue  : Parameter Value
    '             v_iType   : Parameter DataType
    '
    ' Description: Adds an input parameter to the database parameters
    '
    ' History:
    '           Created : MEvans : 18-12-2002 : 202
    ' ***************************************************************** '
    Private Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "AddInputParameter"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Add Parameter to database object

        If m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=v_vValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType) <> gPMConstants.PMEReturnCode.PMTrue Then

            ' Log Error.
            result = gPMConstants.PMEReturnCode.PMFalse


            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to add parameter name:" & v_sName &
                                      ", values :" & CStr(v_vValue) & ", type:" & CStr(v_iType), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Informations.Err().Description))

        End If

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: GetDuplicateClaims
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function GetDuplicateClaims(ByVal v_sPolicyNumber As String, ByVal v_dtLossDate As Date, ByVal v_lRiskTypeId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetDuplicateClaims"

        ' Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters

            ' Policy number
            m_lReturn = AddInputParameter(v_sName:="policy_number", v_vValue:=v_sPolicyNumber, v_iType:=gPMConstants.PMEDataType.PMString)

            ' Loss Date
            m_lReturn = AddInputParameter(v_sName:="loss_date", v_vValue:=v_dtLossDate, v_iType:=gPMConstants.PMEDataType.PMDate)

            ' Risk Type Id
            m_lReturn = AddInputParameter(v_sName:="risk_type_id", v_vValue:=v_lRiskTypeId, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=kGetDuplicateClaimsSQL, sSQLName:=kGetDuplicateClaimsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetDuplicateClaimsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetDuplicateClaimOverrideUsers
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function GetDuplicateClaimOverrideUsers(ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetDuplicateClaimOverrideUsers"

        'Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=kGetDuplicateClaimOverrideUsersSQL, sSQLName:=kGetDuplicateClaimOverrideUsersName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetDuplicateClaimOverrideUsersSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: AddClaimLink
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function AddClaimLink(ByVal v_lClaimId As Integer, ByVal v_lLinkTypeId As Integer, ByVal v_lLinkId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddClaimLink"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = AddInputParameter(v_sName:="claim_id", v_vValue:=v_lClaimId, v_iType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = AddInputParameter(v_sName:="link_type_id", v_vValue:=v_lLinkTypeId, v_iType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = AddInputParameter(v_sName:="link_id", v_vValue:=v_lLinkId, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' Execute Action Query

            lReturn = m_oDatabase.SQLAction(sSQL:=kAddClaimLinkSQL, sSQLName:=kAddClaimLinkName, bStoredProcedure:=True)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kAddClaimLinkSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function



    ' ***************************************************************** '
    ' Name: GetClaimTransactionSuppressionInd
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 28-10-2005 : Process ID
    ' ***************************************************************** '
    Public Function GetClaimTransactionSuppressionInd(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lClaimId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimTransactionSuppressionInd"

        ' Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters

            ' insurance file cnt
            m_lReturn = AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' claim id
            m_lReturn = AddInputParameter(v_sName:="claim_id", v_vValue:=v_lClaimId, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=kGetClaimTransactionSuppressionIndSQL, sSQLName:=kGetClaimTransactionSuppressionIndName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetClaimTransactionSuppressionIndSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ******************************************************************** '
    ' Name: IsStatusClosed
    ' Description: Retrieve if a progress status indicates the claim is closed
    '
    ' History:
    '   Created : A.Robinson, 20051104 S4B Claim Enhancements
    ' ******************************************************************** '

    Public Function IsProgressStatusClosed(ByVal v_lClaimStatusId As Integer, ByRef r_bIsClosed As Boolean) As Integer

        Dim result As Integer = 0
        Const kPROCEDURE_NAME As String = "IsProgressStatusClosed"
        Try

            Dim vResults(,) As Object = Nothing

            result = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase.Parameters.Clear()
            m_lReturn = AddInputParameter(v_sName:="StatusID", v_vValue:=v_lClaimStatusId, v_iType:=gPMConstants.PMEDataType.PMLong)


            If m_oDatabase.SQLSelect(sSQL:=kCLM_SQL_IS_CLOSED_STATUS_SQL, sSQLName:=kCLM_SQL_IS_CLOSED_STATUS_NAME, bStoredProcedure:=kCLM_SQL_IS_CLOSED_STATUS_SP, vResultArray:=vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(ACClass & "." & kPROCEDURE_NAME, kCLM_SQL_IS_CLOSED_STATUS_SQL & " failed", gPMConstants.PMELogLevel.PMLogError)

            End If

            If Informations.IsArray(vResults) Then

                r_bIsClosed = (gPMFunctions.ToSafeLong(CStr(vResults(0, 0)), 0) = 1)
            Else
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch ex As Exception



            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kPROCEDURE_NAME, r_lFunctionReturn:=result, excep:=ex)

            Return result
        End Try
    End Function

    ' ******************************************************************** '
    ' Name: GetClaimReserves
    ' Description: Retrieve the reserves of a claim
    '
    ' History:
    '   Created : A.Robinson, 20051109 S4B Claim Enhancements
    ' ******************************************************************** '
    Public Function GetClaimReserves(ByVal v_lClaimId As Integer, ByVal v_lRiskTypeId As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim oRiskDetails As bCLMRiskDetails.Business = Nothing
        Const kPROCEDURE_NAME As String = "GetClaimReserves"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            oRiskDetails = New bCLMRiskDetails.Business()

            m_lReturn = Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppNAme)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = oRiskDetails.GetPerilForClaimRisk(v_lClaimId:=v_lClaimId, v_lRiskTypeId:=v_lRiskTypeId, r_vDataArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not (oRiskDetails Is Nothing) Then
                oRiskDetails.Dispose()
                oRiskDetails = Nothing

            End If

            Return result

        Catch ex As Exception



            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kPROCEDURE_NAME, r_lFunctionReturn:=result, excep:=ex)

            If Not (oRiskDetails Is Nothing) Then
                oRiskDetails.Dispose()
                oRiskDetails = Nothing

            End If

            Return result
        End Try
    End Function

    ' ******************************************************************** '
    ' Name: ResetClaimReserves
    ' Description: Reset the reserves of a claim to zero
    '
    ' History:
    '   Created : A.Robinson, 20051109 S4B Claim Enhancements
    ' ******************************************************************** '
    Public Function ResetClaimReserves(ByVal v_lClaimId As Integer) As Integer

        Dim result As Integer = 0
        Const kPROCEDURE_NAME As String = "ResetClaimReserves"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase.Parameters.Clear()
            m_lReturn = AddInputParameter(v_sName:="ClaimId", v_vValue:=v_lClaimId, v_iType:=gPMConstants.PMEDataType.PMLong)


            If m_oDatabase.SQLAction(sSQL:=kCLM_SQL_ZERO_RESERVE_SQL, sSQLName:=kCLM_SQL_ZERO_RESERVE_NAME, bStoredProcedure:=kCLM_SQL_ZERO_RESERVE_SP) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(ACClass & "." & kPROCEDURE_NAME, kCLM_SQL_ZERO_RESERVE_SQL & " failed", gPMConstants.PMELogLevel.PMLogError)

            End If

            Return result

        Catch ex As Exception



            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kPROCEDURE_NAME, r_lFunctionReturn:=result, excep:=ex)

            Return result
        End Try
    End Function

    ' Gets the total outstanding reserve and recovery reserve values for the supplied claim
    Public Function GetCurrentReserveRecovery(ByVal v_lClaimId As Integer, ByRef r_vDataArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetCurrentReserveRecovery"
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "ClaimID", v_lClaimId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ' Execute the stored procedure.

            lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCurrentReserveRecoverySQL, sSQLName:=ACGetCurrentReserveRecoveryName, bStoredProcedure:=True, vResultArray:=r_vDataArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect()", "Failed to get reserve and recovery information")
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
    End Function


    ' ******************************************************************** '
    ' Name: GetPolicyAccountHandlers
    ' Description: Get the account handler names for a policy
    '
    ' History:
    '   Created : A.Robinson, 20051219 S4B Claim Enhancements
    ' ******************************************************************** '
    Public Function GetPolicyAccountHandlers(ByVal v_lPolicyId As Integer, ByRef r_sAccountHandler As String, ByRef r_sAccountExecutive As String) As Integer

        Dim result As Integer = 0
        Const kPROCEDURE_NAME As String = "GetPolicyAccountHandlers"
        Try

            Const kCOL_ACCOUNT_HANDLER As Integer = 1
            Const kCOL_ACCOUNT_EXECUTIVE As Integer = 3

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vResultArray(,) As Object = Nothing


            m_oDatabase.Parameters.Clear()
            m_lReturn = AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lPolicyId, v_iType:=gPMConstants.PMEDataType.PMLong)


            If m_oDatabase.SQLSelect(sSQL:=kCLM_SQL_ACCOUNT_HANDLER_SQL, sSQLName:=kCLM_SQL_ACCOUNT_HANDLER_NAME, bStoredProcedure:=kCLM_SQL_ACCOUNT_HANDLER_SP, vResultArray:=vResultArray, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(ACClass & "." & kPROCEDURE_NAME, kCLM_SQL_ACCOUNT_HANDLER_SQL & " failed", gPMConstants.PMELogLevel.PMLogError)
            Else
                If Not Informations.IsArray(vResultArray) Then
                    result = gPMConstants.PMEReturnCode.PMNotFound
                Else
                    r_sAccountHandler = gPMFunctions.ToSafeString(vResultArray(kCOL_ACCOUNT_HANDLER, 0))
                    r_sAccountExecutive = gPMFunctions.ToSafeString(vResultArray(kCOL_ACCOUNT_EXECUTIVE, 0))
                End If
            End If

            Return result

        Catch ex As Exception



            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kPROCEDURE_NAME, r_lFunctionReturn:=result, excep:=ex)

            Return result
        End Try
    End Function


    Public Function ValidateClaimNumber(ByVal sEnteredNumber As String, ByVal v_lBusinessType As Integer, ByVal v_lProductID As Integer, ByRef sFailureReason As String) As Integer
        Dim result As Integer = 0
        Dim sMaskChar, sEnteredChar As String
        Dim iNumericLength As Integer
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim lNumberingScheme As Integer
        Dim m_sMaskCode As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = BeginTrans()

            ' Clear the Database Parameters Collection

            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="product_id", vValue:=v_lProductID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            ' Execute SQL Statement

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetNumberingSchemeIdsFromProductSQL, sSQLName:=ACGetNumberingSchemeIdsFromProductName, bStoredProcedure:=ACGetNumberingSchemeFromProductStored, lNumberRecords:=0, vResultArray:=vResultArray)

            'Fields are retrieved in same order as business type codes.
            'First field (0) is 'code'.
            If Not (Informations.IsArray(vResultArray)) Then
                lNumberingScheme = 0
                'scode = ""
            Else
                'vResultArray(5, 0) to check whether the Claim at Quote is checked or not
                '             If vResultArray(5, 0) = 1 Then
                '                v_lBusinessType = 2
                '             End If
                Dim auxVar As Object = vResultArray(v_lBusinessType, 0)


                If Not (Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar)) Then

                    lNumberingScheme = CInt(vResultArray(v_lBusinessType, 0))
                    'scode = UCase(Trim$(vResultArray(0, 0)))
                End If
            End If

            'If no code found then user can enter anything, but must be checked
            'in ValidateClaimNumber that it doesn't already exist.

            If (lNumberingScheme = 0) Or Convert.IsDBNull(lNumberingScheme) Or Informations.IsNothing(lNumberingScheme) Then
                'm_bGenerate = False
                m_sMaskCode = ""
                m_lReturn = CommitTrans()
                Return result
            End If

            ' Clear the Database Parameters Collection

            m_oDatabase.Parameters.Clear()

            '** Get particular numbering scheme

            m_lReturn = m_oDatabase.Parameters.Add(sName:="language_id", vValue:=m_iLanguageID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="numbering_scheme_id", vValue:=lNumberingScheme, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetNumberingSchemeSQL, sSQLName:="", bStoredProcedure:=True, lNumberRecords:=0, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get Claim Number Rules, setting module level Properties for use in Validate method.

            '        m_bGenerate = vResultArray(iNUM_SCHEME_IS_GEN, 0)

            m_sMaskCode = CStr(vResultArray(iNUM_SCHEME_MASK, 0))
            'sFixedCode = vResultArray(iNUM_SCHEME_FIXED_CODE, 0)
            'sNextNumber = CStr(vResultArray(iNUM_SCHEME_NEXT_NUM, 0))
            'bReuseAbandoned = vResultArray(iNUM_SCHEME_REUSE_ABANDONED, 0)

            'sNumberToAllocate = m_sMaskCode
            m_lReturn = CommitTrans()

            m_sMaskCode = m_sMaskCode.Trim().ToUpper()

            sEnteredNumber = sEnteredNumber.Trim()

            'If there is no mask code, then the only validation required is that
            'the entered number does not already exist.
            If m_sMaskCode <> "" Then
                'Firstly, check entry is same length as mask.
                If sEnteredNumber.Length <> m_sMaskCode.Length Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    sFailureReason = "Enter Claim Number in format """ & m_sMaskCode & """"
                    Return result
                End If

                'Check each individual character of entered number is valid.
                For iPlaceHolder As Integer = 1 To m_sMaskCode.Length
                    sMaskChar = (m_sMaskCode.Substring(iPlaceHolder - 1, 1))
                    sEnteredChar = (sEnteredNumber.Substring(iPlaceHolder - 1, 1))

                    Select Case (sMaskChar)
                        Case "/", "-"
                            If sEnteredChar <> sMaskChar Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                sFailureReason = "Enter Claim Number in format """ & m_sMaskCode & """"
                                Return result
                            End If

                        Case "X"
                            If (Strings.AscW(sEnteredChar(0)) < gPMConstants.Keys.D0) Or (Strings.AscW(sEnteredChar(0)) > gPMConstants.Keys.D9) Then
                                If (Strings.AscW(sEnteredChar(0)) < gPMConstants.Keys.A) Or (Strings.AscW(sEnteredChar(0)) > gPMConstants.Keys.Z) Then
                                    result = gPMConstants.PMEReturnCode.PMFalse
                                    sFailureReason = "Enter Claim Number in format """ & m_sMaskCode & """"
                                    Return result
                                End If
                            End If

                        Case "9"
                            iNumericLength += 1
                            If (Strings.AscW(sEnteredChar(0)) < gPMConstants.Keys.D0) Or (Strings.AscW(sEnteredChar(0)) > gPMConstants.Keys.D9) Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                sFailureReason = "Enter Claim Number in format """ & m_sMaskCode & """"
                                Return result
                            End If

                        Case Else
                            'Error

                    End Select
                Next iPlaceHolder
            End If

            'Now make sure number doesn't already exist.
            sSQL = "SELECT claim_number FROM claim " & "WHERE claim_number = '" & sEnteredNumber & "'"

            ' Execute SQL Statement

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vResultArray) Then
                'Number already exists
                result = gPMConstants.PMEReturnCode.PMFalse
                sFailureReason = "Claim Number already exists."
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            sFailureReason = "Unexpected error: " & excep.Message

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateClaimNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateClaimNumber", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Procedure: GetFSAComplianceValue
    '
    ' Description: Retrieves FSA Product option value.
    ' ***************************************************************** '
    Public Function GetFSAComplianceValue(ByRef r_vValue As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse


            'developer guide no.98
            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=0, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppNAme, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableFSACompliance, v_vBranch:=gPMConstants.SIRBCHHeadOffice, r_vUnderwriting:=r_vValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            'Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFSAComplianceValue failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFSAComplianceValue", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function SelectRenewalStatusType(ByVal v_lInsuranceFolderCnt As Integer, ByRef r_sRenewalStatusTypeCode As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SelectRenewalStatusType"

        Dim vResultArray(,) As Object = Nothing

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=v_lInsuranceFolderCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "sName:=insurance_folder_cnt", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectRenewalControlSQL, sSQLName:=ACSelectRenewalControlName, bStoredProcedure:=ACSelectRenewalControlStored, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "sName:=insurance_folder_cnt", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(vResultArray) Then

                r_sRenewalStatusTypeCode = CStr(vResultArray(9, 0)).Trim()
            Else
                r_sRenewalStatusTypeCode = ""
            End If


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


    Public Function UpdateClaimPolicyDetails() As Integer

        Dim result As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_odOpenClaim.UpdateClaimPolicyDetails

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateClaimPolicyDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '---------------------------------------------------------------------------------------
    ' Procedure   : LogMessageToPMMessageTable
    ' Description : Logs a message to the PMMessage table - based on 1.6 bPMMessage code
    '---------------------------------------------------------------------------------------
    'developer guide no.101
    Public Sub LogMessageToPMMessageTable(ByVal v_iType As Integer, ByVal v_sMsg As String, ByVal v_sCallingAppName As String, Optional ByRef v_vApp As Object = Nothing, Optional ByRef v_vClass As Object = Nothing, Optional ByRef v_vMethod As Object = Nothing, Optional ByRef v_vErrNo As Object = Nothing, Optional ByRef v_vErrDesc As Object = Nothing)




        Dim lError As gPMConstants.PMEReturnCode
        Dim sText, sErrDesc As String
        Dim lMessageID, lRecordsAffected As Integer

        Try

            ' If Error Type is not supplied, set it to Fatal Error
            If v_iType = 0 Then
                v_iType = gPMConstants.PMELogLevel.PMLogFatal
            End If

            ' Set the Optional Parameters to a default value if they are not supplied.

            If Informations.IsNothing(v_vApp) Then
                v_vApp = ""
            End If


            If Informations.IsNothing(v_vClass) Then
                v_vClass = ""
            End If


            If Informations.IsNothing(v_vMethod) Then
                v_vMethod = ""
            End If


            If Informations.IsNothing(v_vErrNo) Then
                v_vErrNo = 0
            End If


            If Informations.IsNothing(v_vErrDesc) Then
                v_vErrDesc = ""
            End If


            With m_oDatabase


                .Parameters.Clear()


                lError = .Parameters.Add(sName:="source_id", vValue:=m_iSourceID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If


                lError = .Parameters.Add(sName:="username", vValue:=m_sUsername, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If


                lError = .Parameters.Add(sName:="log_date", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
                If lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If


                lError = .Parameters.Add(sName:="message_type", vValue:=v_iType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If


                lError = .Parameters.Add(sName:="calling_app_name", vValue:=v_sCallingAppName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If

                sText = v_sMsg.Trim()
                lError = CType(bPMFunc.ValidateSQL(sText), gPMConstants.PMEReturnCode)
                If lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If


                lError = .Parameters.Add(sName:="text", vValue:=sText, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If


                lError = .Parameters.Add(sName:="err_number", vValue:=v_vErrNo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If

                'RDC090800 - check VB error description for apostrophes
                sErrDesc = v_vErrDesc.Trim()
                lError = CType(bPMFunc.ValidateSQL(sErrDesc), gPMConstants.PMEReturnCode)
                If lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If


                lError = .Parameters.Add(sName:="err_description", vValue:=sErrDesc, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If


                lError = .Parameters.Add(sName:="app_name", vValue:=v_vApp, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If


                lError = .Parameters.Add(sName:="class_name", vValue:=v_vClass, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If


                lError = .Parameters.Add(sName:="method_name", vValue:=v_vMethod, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If

                ' Add PMMessageID as an OUTPUT param for an insert

                lError = .Parameters.Add(sName:="message_id", vValue:=lMessageID, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If

                ' Execute SQL Statement

                lError = .SQLAction(sSQL:=kAddPMMessageSQL, sSQLName:=kAddPMMessageName, bStoredProcedure:=kAddPMMessageStored, lRecordsAffected:=lRecordsAffected)
                If lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If

            End With

        Catch ex As Exception


            ' Error Section.

            ' Display the Message we tried to add.
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=v_iType, sMsg:=v_sMsg, vApp:=v_vApp, vClass:=v_vClass, vMethod:=v_vMethod, excep:=ex)

            Exit Sub
        End Try


    End Sub


    ' ***************************************************************** '
    ' Name: GetClaimTypeAndCover
    ' Description:
    ' History:
    ' Created : VB : 02-08-2007
    ' ***************************************************************** '
    Public Function GetClaimTypeAndCover(ByVal v_lRiskTypeID As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimTypeAndCover"

        'Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = AddInputParameter(v_sName:="risk_type_id", v_vValue:=v_lRiskTypeID, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=kGetClaimTypeAndCoverSQL, sSQLName:=kGetClaimTypeAndCoverName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetClaimTypeAndCoverSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function




    ' ***************************************************************** '
    ' Name: GetGISRetroactiveDate
    ' Description:
    ' History:
    ' Created : VB : 02-08-2007
    ' ***************************************************************** '
    Public Function GetGISRetroactiveDate(ByVal v_lInsurancefileID As Integer, ByRef r_vResults(,) As Object, Optional ByVal v_lRiskCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetGISRetroactiveDate"

        'Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsurancefileID, v_iType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = AddInputParameter(v_sName:="risk_cnt", v_vValue:=v_lRiskCnt, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=kGetRetroactiveDateGISPropertySQL, sSQLName:=kGetRetroactiveDateGISPropertyName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetRetroactiveDateGISPropertySQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sTransaction_Type"></param>
    ''' <param name="r_vDataArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetProgressStatus(ByVal sTransaction_Type As String,
                                      ByRef r_vDataArray(,) As Object) As Integer

        Dim nResult As Integer = 0
        Const kMethodName As String = "GetProgressStatus"
        Dim lReturn As gPMConstants.PMEReturnCode


        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "transaction_type", sTransaction_Type, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, True)
            'PN: 73770
            bPMAddParameter.AddParameterLite(m_oDatabase, "mode", m_iTask, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True)
            ' Execute the stored procedure.

            lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetProgressStatusSQL, sSQLName:=ACGetProgressStatusName, bStoredProcedure:=True, vResultArray:=r_vDataArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect()", "Failed to execute spu_CLM_Get_Progress_Status")
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

        Finally

        End Try
        Return nResult
    End Function

    'PN: 73770
    Public Function GetClaimHandler(ByRef r_vDataArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimHandler"
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            ' Execute the stored procedure.
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetClaimHandlerSQL, sSQLName:=ACGetClaimHandlerName, bStoredProcedure:=True, vResultArray:=r_vDataArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect()", "Failed to execute spu_CLM_Get_Claim_Handler")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result
            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    Public Function GetProgressStatusDetails(ByVal iProgressStatusID As Integer, ByRef r_vDataArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetProgressStatusDetails"
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "Progress_Status_Id", iProgressStatusID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True)

            ' Execute the stored procedure.

            lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetProgressStatusDetailsSQL, sSQLName:=ACGetProgressStatusDetailsName, bStoredProcedure:=True, vResultArray:=r_vDataArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect()", "Failed to execute spu_CLM_Get_Progress_Status_Details")
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: GetShowRiskDetails_U
    '
    ' Description: Calls an sp to fetch back Underwriting risk details.
    '
    ' PN - 45245 - 02/08/2008 Created - Added to fetch risk details to show
    '       Event Log correctly...
    '
    ' ***************************************************************** '
    Public Function GetShowRiskDetails_U(ByVal v_lClaimId As Integer, ByRef r_vDataArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase.Parameters.Clear()
            'DC130701 was iClaimId now lClaimId

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimId", vValue:=v_lClaimId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure.

            m_lReturn = m_oDatabase.SQLSelect(ACGetShowRiskDetailsUSQL, ACGetShowRiskDetailsUName, ACGetShowRiskDetailsUStored, , r_vDataArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetShowRiskDetails_U Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetShowRiskDetails_U", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ''Start(Saurabh Agrawal) Tech Spec WR3 User Level RI Display Restriction - (5.5.1)
    'developer guide no.101
    Public Function GetSpecificUserAuthority(ByVal v_vAuthority As Object, ByRef r_vAuthorityValue As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetSpecificUserAuthority"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue



            m_oDatabase.Parameters.Clear()
            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "user_id", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Authority", "display_claim_reinsurance", gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectUserAuthority, sSQLName:=ACSelectUserAuthorityScrName, bStoredProcedure:=ACSelectUserAuthorityStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACSelectUserAuthority & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



            'developer guide no.175
            'start
            If Not (Convert.IsDBNull(m_oDatabase.Records.Fields(0)) Or Informations.IsNothing(m_oDatabase.Records.Fields(0))) Then

                r_vAuthorityValue = m_oDatabase.Records.Fields(0)
                'end
            Else
                r_vAuthorityValue = 0
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function
    ''End(Saurabh Agrawal) Tech Spec WR3 User Level RI Display Restriction - (5.5.1)


    Public Function GetRiskDetailsForClaim(ByRef rvntResultArray(,) As Object, ByVal vvntInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase.Parameters.Clear()
            'DC130701 was iClaimId now lClaimId

            m_lReturn = m_oDatabase.Parameters.Add(sName:="pol_id", vValue:=vvntInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure.

            m_lReturn = m_oDatabase.SQLSelect(ACGetRiskDetailsClaimSQL, ACGetRiskDetailsClaimName, ACGetRiskDetailsClaimStored, , rvntResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskDetailForClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetShowRiskDetails_U", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetUserOtherParty(ByVal iUserID As Integer, ByRef r_vResultArray(,) As Object) As Long
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            bPMAddParameter.AddParameterLite(m_oDatabase, "userid", iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)


            ' Execute the stored procedure.

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetUserotherpartySQL, sSQLName:=ACGetUserotherpartyName, bStoredProcedure:=ACGetUserotherpartyStored, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Get user other party Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Getuserotherparty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
End Class

