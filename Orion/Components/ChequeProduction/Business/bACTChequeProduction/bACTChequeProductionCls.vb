Option Strict Off
Option Explicit On
Imports System.Drawing.Printing
Imports System.IO
Imports System.Text
'Developer Guide NO 129
Imports SSP.Shared



<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Automated
    '
    ' Date: 07/04/1998
    '
    ' Description: Creatable Automated class which contains all the
    '              Automated methods which can be called by Navigator.
    '
    ' Edit History:
    ' RKS 27/06/2006    Enchancement to Cheque Production (NEM Insurance)
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
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    Private m_oCurrencyConvert As bACTCurrencyConvert.Form


    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Component Services object

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Process Mode Properties
    ' Task
    Private m_iTask As Integer
    ' Navigate
    Private m_lNavigate As Integer
    ' Current Record Pointer
    Private m_lCurrentRecord As Integer
    ' Process Mode
    Private m_lProcessMode As Integer
    ' Type of Business
    Private m_sTransactionType As String = ""
    ' Effective
    Private m_dtEffectiveDate As Date


    ' Calling Application name.


    'Abort Transaction


    'eck090401
    Private m_lStartChequeNo As Integer

    'DD 09/12/2003: COM+ changes
    Private m_sPath As String = ""
    Private m_sDrive As String = ""

    Private m_vBankWiseStartChequeNumber(,) As Object
    Private m_vChequeArrayForPrinting(,) As Object

    Private Declare Function SetDefaultPrinter Lib "winspool.drv" Alias "SetDefaultPrinterA" (ByVal pszPrinter As String) As Integer


    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            m_sCallingAppName = Value

        End Set
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



    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
        End Get
    End Property
    'eck090401
    Public WriteOnly Property StartChequeNo() As Integer
        Set(ByVal Value As Integer)

            m_lStartChequeNo = Value

        End Set
    End Property

    'Developer Guide No 33
    Public WriteOnly Property BankWiseStartChequeNumber() As Object(,)
        Set(ByVal Value As Object(,))

            m_vBankWiseStartChequeNumber = Value

        End Set
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


            ' Initialisation Code.

            ' Set Username and Password

            ' Set UserID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level


            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Currency Convert

            m_oCurrencyConvert = New bACTCurrencyConvert.Form
            m_lReturn = m_oCurrencyConvert.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Remove instance of Component Services
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bACTCurrencyConvert.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'eck find the cheque Production Program
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="ChequeProduction", r_sSettingValue:=m_sPath), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_sDrive = m_sPath.Substring(0, 1)

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Set the ProcessMode to Generic
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric

            ' Set the Type Of Business to New Business
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric

            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now

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
                If m_oCurrencyConvert IsNot Nothing Then
                    m_oCurrencyConvert.Dispose()
                    m_oCurrencyConvert = Nothing
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
    ' Name: SetKeys (Standard Method)
    '
    ' Description: Stores all of the parameter members with the key
    '              array.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a vaild array.
            If Not Informations.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                ' Assign the parameter member with the
                ' correct key array item.


                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()

                End Select

            Next lRow

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeys (Standard Method)
    '
    ' Description: Stores all of the key array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim vKeyArray(0, 0)


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: Start
    '
    ' Description: Navigator Start function. Entry point into Navigator.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StartFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: Add Cheque
    '
    ' Description: Adds Cheque details to Work File
    '
    '
    ' ***************************************************************** '
    Public Function AddCheque(ByRef lTransdetailId As Integer, ByRef lBankID As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()


                m_lReturn = m_oDatabase.Parameters.Add(sName:="transdetail_id", vValue:=CStr(lTransdetailId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="bankaccount_id", vValue:=CStr(lBankID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="media_ref", vValue:="", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored, lRecordsAffected:=lRecordsAffected)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Cheque Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddCheque", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: Update Cheque
    '
    ' Description: Updates Cheque Number
    '
    '
    ' ***************************************************************** '
    Public Function UpdateCheque(ByRef lChequeId As Integer, ByRef sChequeNumber As String) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()


                m_lReturn = m_oDatabase.Parameters.Add(sName:="cheque_id", vValue:=CStr(lChequeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="media_ref", vValue:=sChequeNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateSQL, sSQLName:=ACUpdateName, bStoredProcedure:=ACUpdateStored, lRecordsAffected:=lRecordsAffected)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Cheque Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AUpdateCheque", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: Get Cheques
    '
    ' Description: Gets Cheques
    '
    ' ***************************************************************** '
    Public Function GetCheques(ByRef r_vResultArray(,) As Object, ByVal r_dtTransactionDate As Date, Optional ByVal v_vBankAccountID As Object = Nothing, Optional ByVal v_iSourceID As Integer = 0) As Integer

        Dim result As Integer = 0

        Dim bMultiCompany As Boolean 'AR20050221 - PN18883 
        Dim vValue As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'AR20050221 - PN18883 Check whether MultiCompany is switched on
            m_lReturn = CType(bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, v_vBranch:=gPMConstants.SIRBCHHeadOffice, r_vUnderwriting:=vValue), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get MultiTreeAccounting option value", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCheques")
                Return result
            End If

            'AR20050221 - PN18883 Check whether MultiCompany is switched on
            bMultiCompany = vValue = "1"

            With m_oDatabase

                .Parameters.Clear()

                'developer guide no. 40
                m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_date", vValue:=r_dtTransactionDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)


                If Not False Then
                    'Add company id
                    'Pass 0 as company id if not multicompany
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(v_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add database parameter for company_id", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCheques")
                        Return result
                    End If
                Else
                    'Add company id
                    'Pass 0 as company id if not multicompany
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=If(bMultiCompany, CStr(m_iSourceID), CStr(0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add database parameter for company_id", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCheques")
                        Return result
                    End If
                End If


                If Informations.IsNothing(v_vBankAccountID) Then

                    ' Execute SQL Statement
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectAllSQL, sSQLName:=ACSelectAllName, bStoredProcedure:=ACSelectAllStored, vResultArray:=r_vResultArray)

                Else

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="bankaccount_id", vValue:=CStr(v_vBankAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    ' Execute SQL Statement
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectBankSQL, sSQLName:=ACSelectBankName, bStoredProcedure:=ACSelectBankStored, vResultArray:=r_vResultArray)
                End If
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Get Cheques Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCheques", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: Update CashListItem
    '
    ' Description: Updates CashListItem Media Ref
    '
    '
    ' ***************************************************************** '
    Public Function UpdateCashListItem(ByRef lTransdetailId As Integer, ByRef sChequeNumber As String) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()


                m_lReturn = m_oDatabase.Parameters.Add(sName:="transdetail_id", vValue:=CStr(lTransdetailId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="media_ref", vValue:=sChequeNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateCashListSQL, sSQLName:=ACUpdateCashListName, bStoredProcedure:=ACUpdateStored, lRecordsAffected:=lRecordsAffected)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update CashListItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateCashListItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetUserPrinter
    '
    ' Description: get default printer attached to this user
    '
    ' History: 12/05/2008.
    '
    ' ***************************************************************** '
    Public Function GetUserPrinter() As String

        Dim result As String = String.Empty
        Dim vResultArray(,) As Object = Nothing

        Try

            result = CStr(gPMConstants.PMEReturnCode.PMTrue)


            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="UserName", vValue:=m_sUsername, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return CStr(gPMConstants.PMEReturnCode.PMFalse)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetUserPrinterSQL, sSQLName:=ACGetUserPrinterName, bStoredProcedure:=ACGetUserPrinterStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return CStr(gPMConstants.PMEReturnCode.PMFalse)
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return CStr(gPMConstants.PMEReturnCode.PMNotFound)
            End If



            Return CStr(vResultArray(0, 0))

        Catch excep As System.Exception



            result = CStr(gPMConstants.PMEReturnCode.PMError)

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserPrinter Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserPrinter", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ChequeMaster Cheques
    '
    ' Description: ChequeMaster Cheque production
    '
    ' ***************************************************************** '
    Public Function ChequeMasterCheques(ByVal v_sSourceDescription As String, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer
        Dim iFileNumber As Integer

        Dim sFileWithPath, sFile, sOutFile, sCommandline, sFileName As String


        Dim sRecord As String, sChequeNo As String = String.Empty, sComment1 As String, sComment2 As String, sComment3 As String, sComment4 As String

        Dim sPrinter_Name, sOriginal_printer As String





        result = gPMConstants.PMEReturnCode.PMTrue

        Dim pd As New PrintDocument()

        sOriginal_printer = pd.PrinterSettings.PrinterName

        sPrinter_Name = GetUserPrinter()
        If sPrinter_Name <> "-1" Then
            'set the default printer
            SetDefaultPrinter(sPrinter_Name)
        End If

        'Verify that CheckPrint is installed
        If Not gPMFunctions.FileExists(m_sPath & "\CheckPrint.exe") Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        'Loop for each of the Bank starting cheque no
        For lBankLoopRow As Integer = m_vBankWiseStartChequeNumber.GetLowerBound(1) To m_vBankWiseStartChequeNumber.GetUpperBound(1)

            'Create File
            sFileWithPath = m_sPath & "\Cheques" & CStr(m_iUserID) & ".csv"
            sFile = "Cheques" & m_iUserID & ".csv"
            sOutFile = m_sPath & "\ZCheques" & CStr(m_iUserID) & ".csv"

            Try
                File.Delete(sFileWithPath)

            Catch
            End Try



            File.Delete(sOutFile)

            iFileNumber = FreeFile()

            File.Open(iFileNumber, sFileWithPath, OpenMode.Output)

            For lRow As Integer = 0 To r_vResultArray.GetUpperBound(1)
                'Process only record which matches the Bank ID for Bank loop

                If m_vBankWiseStartChequeNumber(ACBankID, lBankLoopRow).Equals(r_vResultArray(ACIBankID, lRow)) Then
                    sRecord = ""
                    ' CJB 070203 - Start
                    ' Modify this initial version only currently used by Carribean customers
                    ' to also be used by a UK customer (Lumleys) with a different version of the CheckMaster SW...
                    ' This needs to be changed before the Carribean customers upgrade to 1.8 to work with multiple
                    ' customers. Ideally we would have one CheckMaster install (rather than 2 versions like we have
                    ' now) and we would pass a unique customer identifier to CheckMaster to print the relevant chq.
                    ' Note that also passed in new param - v_sSourceDescription - to this function for Lumleys to
                    ' print on the part above the chq (branch name)
                    sComment1 = "Cheque No "
                    sComment2 = "for"
                    sComment3 = "is attached."
                    sComment4 = "Please see enclosed sheet (s) for details."

                    'Original Caribbean Customers Cheque...
                    '        sRecord = CStr(r_vResultArray(ACITransactionId, lRow)) & "," & _
                    ''            CStr(r_vResultArray(ACIChequeNumber, lRow)) & "," & _
                    ''            CStr(r_vResultArray(ACIBankCode, lRow)) & "," & _
                    ''            CStr(r_vResultArray(ACIAccountId, lRow)) & "," & _
                    ''            CStr(r_vResultArray(ACIAccountName, lRow)) & "," & _
                    ''            CStr(r_vResultArray(ACIAccountAddress1, lRow)) & "," & _
                    ''            CStr(r_vResultArray(ACIAccountAddress2, lRow)) & "," & _
                    ''            CStr(r_vResultArray(ACIAccountAddress3, lRow)) & "," & _
                    ''            CStr(r_vResultArray(ACIAccountAddress4, lRow)) & "," & _
                    ''            CStr(r_vResultArray(ACIAccountPostCode, lRow)) & "," & _
                    ''            ",," & _
                    ''            CStr(r_vResultArray(ACITransactionDate, lRow)) & "," & _
                    ''            CStr(r_vResultArray(ACIAmount, lRow))

                    'Lumleys Cheque


                    sRecord = CStr(r_vResultArray(ACITransactionID, lRow)) & "," &
                              CStr(r_vResultArray(ACIChequeNumber, lRow)) & "," &
                              CStr(r_vResultArray(ACIBankCode, lRow)) & "," &
                              CStr(r_vResultArray(ACIAccountID, lRow)) & "," &
                              CStr(r_vResultArray(ACIAccountName, lRow)) & "," &
                              CStr(r_vResultArray(ACIAddress1, lRow)) & "," &
                              CStr(r_vResultArray(ACIAddress2, lRow)) & "," &
                              CStr(r_vResultArray(ACIAddress3, lRow)) & "," &
                              CStr(r_vResultArray(ACIAddress4, lRow)) & "," &
                              CStr(r_vResultArray(ACIPostalCode, lRow)) & "," &
                              sComment1 & "," &
                              sComment2 & "," &
                              sComment3 & "," &
                              sComment4 & "," &
                              CStr(r_vResultArray(ACITransactionDate, lRow)) & "," &
                              StringsHelper.Format(r_vResultArray(ACIAmount, lRow), "0.00") & "," &
                              v_sSourceDescription
                    ' CJB 070203 - End

                    PrintLine(iFileNumber, sRecord)
                End If
                'Initialise sRecord for next record
                sRecord = ""
            Next lRow

            FileClose(iFileNumber)

            'ACHTUNG  need changes to  Cheque Production from here
            '

            ChDrive(m_sDrive)
            Directory.SetCurrentDirectory(m_sPath)

            sFileName = "CheckPrint.exe "
            sCommandline = m_sPath & "," & sFile & "," &
                           CStr(m_vBankWiseStartChequeNumber(ACStartChequeNumber, lBankLoopRow))

            ShellWait(sCommandline, sFileName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '        'Opened the printed file and get the cheque numbers
            '        iFileNumber = FreeFile
            '        lRow = -1

            '        Open sOutFile For Input As #iFileNumber
            '        Do While Not EOF(iFileNumber)
            '            Line Input #iFileNumber, sRecord
            '            m_lReturn = GetChequeNo(sRecord, sChequeNo)
            '            If m_lReturn <> PMTrue Then
            '                GoTo Err_ChequeMasterCheques
            '            End If
            '            lRow = lRow + 1
            '
            '            r_vResultArray(ACIChequeNumber, lRow) = sChequeNo
            '
            '            m_lReturn = UpdateCheque(lChequeId:=CLng(r_vResultArray(ACIChequeID, lRow)), _
            ''                    sChequeNumber:=sChequeNo)
            '            If m_lReturn <> PMTrue Then
            '                GoTo Err_ChequeMasterCheques
            '            End If
            '
            '        Loop

            'Opened the printed file and get the cheque numbers
            iFileNumber = FreeFile()
            File.Open(iFileNumber, sOutFile, OpenMode.Input)

            For lRow As Integer = 0 To r_vResultArray.GetUpperBound(1)
                'Process only record which matches the Bank ID for Bank loop

                If m_vBankWiseStartChequeNumber(ACBankID, lBankLoopRow).Equals(r_vResultArray(ACIBankID, lRow)) Then
                    sRecord = ""
                    sRecord = LineInput(m_sPath)
                    m_lReturn = CType(GetChequeNo(sRecord, sChequeNo), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        GoTo Err_ChequeMasterCheques
                    End If

                    r_vResultArray(ACIChequeNumber, lRow) = sChequeNo


                    m_lReturn = CType(UpdateCheque(lChequeId:=CInt(r_vResultArray(ACIChequeID, lRow)), sChequeNumber:=sChequeNo), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        GoTo Err_ChequeMasterCheques
                    End If

                End If
            Next lRow


            FileClose(iFileNumber)

        Next lBankLoopRow

        'reset back to OS printer
        SetDefaultPrinter(sOriginal_printer)

        Return result

Err_ChequeMasterCheques:

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Print Cheques Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ChequeMasterCheques", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: Delete Cheques
    '
    ' Description: Clear Cheques
    '
    '
    ' ***************************************************************** '
    Public Function ClearCheques(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer
        Dim lRecordsAffected As Integer
        Dim sFile As String




        result = gPMConstants.PMEReturnCode.PMTrue

        'Update the Cash List Item

        For lRow As Integer = 0 To r_vResultArray.GetUpperBound(1)
            With m_oDatabase

                .Parameters.Clear()


                m_lReturn = m_oDatabase.Parameters.Add(sName:="transdetail_id", vValue:=CStr(CInt(r_vResultArray(ACITransactionID, lRow))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                m_lReturn = m_oDatabase.Parameters.Add(sName:="media_ref", vValue:=CStr(r_vResultArray(ACIChequeNumber, lRow)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACUpdateCashListSQL, sSQLName:=ACUpdateCashListName, bStoredProcedure:=ACUpdateCashListStored, lRecordsAffected:=lRecordsAffected)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With
        Next lRow

        'Delete from the Cheque File

        For lRow As Integer = 0 To r_vResultArray.GetUpperBound(1)
            With m_oDatabase

                .Parameters.Clear()


                m_lReturn = m_oDatabase.Parameters.Add(sName:="cheque_id", vValue:=CStr(CInt(r_vResultArray(ACIChequeID, lRow))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACDeleteSQL, sSQLName:=ACDeleteName, bStoredProcedure:=ACDeleteStored, lRecordsAffected:=lRecordsAffected)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With
        Next lRow


        Try

            sFile = m_sPath & "Cheques" & CStr(m_iUserID) & ".csv"
            File.Delete(sFile)

            sFile = m_sPath & "ZCheques" & CStr(m_iUserID) & ".csv"
            File.Delete(sFile)

            Return result

Err_ClearCheques:

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Clear Cheques Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearCheques", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result

        Catch exc As System.Exception
            'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try
        Return Nothing

    End Function


    ' ***************************************************************** '
    ' Name: GetChequeNo (Private)
    '
    '       Splits out cheque number from csv record
    '
    ' ***************************************************************** '
    Private Function GetChequeNo(ByRef sString As String, ByRef sChequeNo As String) As Integer
        Dim result As Integer = 0
        Dim iComma As Integer



        result = gPMConstants.PMEReturnCode.PMTrue
        iComma = (sString.IndexOf(","c) + 1) + 1
        sString = sString.Substring(sString.Length - (sString.Length - (iComma)))
        iComma = sString.IndexOf(","c)
        sChequeNo = sString.Substring(0, Math.Min(sString.Length, iComma - 1))

        Return result

    End Function


    'DUMMY ROUTINE TO NUMBER THE CHEQUES
    ' ***************************************************************** '
    ' Name: SetChequeNumber (Private)
    '
    '       Dummy routine to allocate cheque numbers
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (SetChequeNumber) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function SetChequeNumber(ByRef sString As String, ByRef sChequeNo As String) As Integer
    'Dim result As Integer = 0
    'Dim iCount, iStart, iNext As Integer
    '
    'Dim iFirstComma, iSecondComma As Integer
    '
    'Dim sNewString As String = ""
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    '
    'iFirstComma = (sString.IndexOf(","c) + 1)
    'iSecondComma = Strings.InStr(iFirstComma + 1, sString, ",")
    '
    'sNewString = sString.Substring(0, iFirstComma) & sChequeNo & sString.Substring(iSecondComma - 1, sString.Length - iSecondComma + 1)
    '
    'sString = sNewString
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMFalse
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Set Cheque Number Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetChequeNumber", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    'Return result
    'End Try
    'End Function
    'END OF DUMMY ROUTINE

    ' ***************************************************************** '
    '
    ' Name: FormatCurrency
    '
    ' Description:  Pass the call through to the currency convert object.
    '
    '
    ' ***************************************************************** '
    Public Function FormatCurrency(ByRef vCurrencyID As Object, ByRef vCurrencyAmount As Object, ByRef vFormattedCurrency As Object, ByRef vConversionDate As Object) As Integer

        Dim result As Integer = 0
        Try



            Return m_oCurrencyConvert.FormatCurrency(vCurrencyID:=vCurrencyID, vCurrencyAmount:=vCurrencyAmount, vFormattedCurrency:=vFormattedCurrency, vConversionDate:=vConversionDate)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FormatCurrency Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FormatCurrency", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function PrintCheques(ByRef r_vResultArray(,) As Object, ByVal lDocumentTemplateID As Integer, ByVal lSpoolMode As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' NAME: PrintCheques
        ' DESCRIPTION: Spool cheques using document production
        ' AUTHOR: Danny Davis
        ' DATE: 23 May 2005, 11:36:06
        ' HISTORY:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Const kMethodName As String = "PrintCheques"

        'Developer Guide No 50
        Dim oDocManagerWrapper As bSIRDocManagerWrapper.Interface_Renamed
        Dim sDocumentTemplateID As String = ""
        Dim lDocumentTypeID As Integer
        Dim sUseDefault As String = ""
        Dim lChequeNumber As Integer = 0
        Dim iCount As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Hook up document management
            'Developer Guide No 50
            oDocManagerWrapper = New bSIRDocManagerWrapper.Interface_Renamed
            m_lReturn = CType(oDocManagerWrapper.InitialiseBusiness(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(m_lReturn.ToString() + ", " + kMethodName + ", Failed to get instance of bSIRDocManagerWrapper")
            End If

            GetTemplateType(lDocumentTemplateID, lDocumentTypeID)

            If lDocumentTemplateID <> 0 Then
                m_lReturn = CType(GenerateDefaultChequeArrayForPrinting(r_vResultArray, m_vChequeArrayForPrinting), gPMConstants.PMEReturnCode)
                'Loop for each of the Bank starting cheque no
                For lBankLoopRow As Integer = m_vBankWiseStartChequeNumber.GetLowerBound(1) To m_vBankWiseStartChequeNumber.GetUpperBound(1)
                    iCount = 0
                    m_lReturn = CType(GenerateChequeNumbersForBank(CInt(m_vBankWiseStartChequeNumber(ACBankID, lBankLoopRow)), m_vChequeArrayForPrinting, CDbl(m_vBankWiseStartChequeNumber(ACStartChequeNumber, lBankLoopRow))), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("GenerateChequeNumbersForBank()", "BankId = " & CStr(m_vBankWiseStartChequeNumber(ACBankID, lBankLoopRow)))
                    End If

                    For lRow As Integer = 0 To r_vResultArray.GetUpperBound(1)
                        'Process only record which matches the Bank ID for Bank loop


                        If m_vBankWiseStartChequeNumber(ACBankID, lBankLoopRow).Equals(r_vResultArray(ACIBankID, lRow)) Then
                            'Set the cheque number first



                            UpdateCheque(gPMFunctions.ToSafeLong(CDbl(r_vResultArray(ACIChequeID, lRow))), gPMFunctions.ToSafeString(CDbl(m_vChequeArrayForPrinting(iCount, 2)), "0"))
                            iCount += 1
                            'Set up the document
                            oDocManagerWrapper.DocumentTemplateId = lDocumentTemplateID
                            oDocManagerWrapper.DocumentTypeId = lDocumentTypeID

                            oDocManagerWrapper.DocumentRef = CStr(r_vResultArray(ACIDocumentRef, lRow))

                            oDocManagerWrapper.PartyCnt = CInt(r_vResultArray(ACIPartyCnt, lRow))
                            oDocManagerWrapper.SpoolDesc = "Cheque Print Letter"
                            oDocManagerWrapper.Mode = lSpoolMode

                            ' Print the document
                            m_lReturn = oDocManagerWrapper.Start()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Throw New System.Exception(m_lReturn.ToString() + ", " + kMethodName + ", Failed to print the document")
                            End If
                        Else
                            iCount += 1
                        End If
                    Next lRow
                Next lBankLoopRow
            End If

            oDocManagerWrapper.Dispose()
            oDocManagerWrapper = Nothing


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

    Public Function ExportCheques(ByRef r_vResultArray(,) As Object, ByVal sExportPath As String, ByRef r_sExportFile As String) As Integer
        ' ---------------------------------------------------------------------------
        ' NAME: ExportCheques
        ' DESCRIPTION: Export the cheque data to a CSV file
        ' AUTHOR: Danny Davis
        ' DATE: 23 May 2005, 15:12:37
        ' HISTORY:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Const kMethodName As String = "ExportCheques"

        Dim oCurrencyConvert As bACTCurrencyConvert.Form
        Dim iFileNumber As Integer
        Dim sFilename As String = ""
        Dim sWords As String = ""
        Dim sRecord As New StringBuilder





        result = gPMConstants.PMEReturnCode.PMTrue

        'Hook up Currency Convert (for amount in words)
        'note: this object does not need a login reference

        oCurrencyConvert = New bACTCurrencyConvert.Form
        m_lReturn = oCurrencyConvert.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(m_lReturn.ToString() + ", GetTemplateType, Failed to get instance of bACTCurrencyConvert.Form")
        End If

        'Generate the new filename, ensuring the directory is present
        If gPMFunctions.BuildDirectory(sExportPath) <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception("0, " + +", Failed to create directory.")
        End If

        If Not sExportPath.EndsWith("\") Then
            sExportPath = sExportPath & "\"
        End If
        r_sExportFile = sExportPath & "Cheques" & DateTime.Now.ToString("yyMMddHHmmss") & ".csv"

        'Clear the file if it already exists
        Try
            File.Delete(sFilename)

        Catch
        End Try



        'Open a new file
        iFileNumber = FreeFile()
        File.Open(iFileNumber, r_sExportFile, OpenMode.Output)

        'Write the header
        sRecord = New StringBuilder(Strings.ChrW(34).ToString() & "Transaction_ID" & Strings.ChrW(34).ToString() & ",")
        sRecord.Append(Strings.ChrW(34).ToString() & "Transaction_Date" & Strings.ChrW(34).ToString() & ",")
        sRecord.Append(Strings.ChrW(34).ToString() & "Payee_Name" & Strings.ChrW(34).ToString() & ",")
        sRecord.Append(Strings.ChrW(34).ToString() & "Payee_Address_1" & Strings.ChrW(34).ToString() & ",")
        sRecord.Append(Strings.ChrW(34).ToString() & "Payee_Address_2" & Strings.ChrW(34).ToString() & ",")
        sRecord.Append(Strings.ChrW(34).ToString() & "Payee_Address_3" & Strings.ChrW(34).ToString() & ",")
        sRecord.Append(Strings.ChrW(34).ToString() & "Payee_Address_4" & Strings.ChrW(34).ToString() & ",")
        sRecord.Append(Strings.ChrW(34).ToString() & "Payee_Address_PostalCode" & Strings.ChrW(34).ToString() & ",")
        sRecord.Append(Strings.ChrW(34).ToString() & "Amount" & Strings.ChrW(34).ToString() & ",")
        sRecord.Append(Strings.ChrW(34).ToString() & "Amount_Words" & Strings.ChrW(34).ToString() & ",")
        sRecord.Append(Strings.ChrW(34).ToString() & "Bank_Name" & Strings.ChrW(34).ToString() & ",")
        sRecord.Append(Strings.ChrW(34).ToString() & "Our_Ref" & Strings.ChrW(34).ToString())

        PrintLine(iFileNumber, sRecord.ToString())

        'Loop through the results putting fields in double quotes
        If Informations.IsArray(r_vResultArray) Then
            For lRow As Integer = 0 To r_vResultArray.GetUpperBound(1)

                sRecord = New StringBuilder(Strings.ChrW(34).ToString() & gPMFunctions.ToSafeString((r_vResultArray(ACITransactionID, lRow))).Trim() & Strings.ChrW(34).ToString() & ",")

                sRecord.Append(Strings.ChrW(34).ToString() & CDate(r_vResultArray(ACITransactionDate, lRow)).ToString("yyyy/MM/dd") & Strings.ChrW(34).ToString() & ",")

                sRecord.Append(Strings.ChrW(34).ToString() & gPMFunctions.ToSafeString((r_vResultArray(ACIAccountName, lRow))).Trim() & Strings.ChrW(34).ToString() & ",")

                sRecord.Append(Strings.ChrW(34).ToString() & gPMFunctions.ToSafeString((r_vResultArray(ACIAddress1, lRow))).Trim() & Strings.ChrW(34).ToString() & ",")

                sRecord.Append(Strings.ChrW(34).ToString() & gPMFunctions.ToSafeString((r_vResultArray(ACIAddress2, lRow))).Trim() & Strings.ChrW(34).ToString() & ",")

                sRecord.Append(Strings.ChrW(34).ToString() & gPMFunctions.ToSafeString((r_vResultArray(ACIAddress3, lRow))).Trim() & Strings.ChrW(34).ToString() & ",")

                sRecord.Append(Strings.ChrW(34).ToString() & gPMFunctions.ToSafeString((r_vResultArray(ACIAddress4, lRow))).Trim() & Strings.ChrW(34).ToString() & ",")

                sRecord.Append(Strings.ChrW(34).ToString() & gPMFunctions.ToSafeString((r_vResultArray(ACIPostalCode, lRow))).Trim() & Strings.ChrW(34).ToString() & ",")

                sRecord.Append(Strings.ChrW(34).ToString() & gPMFunctions.ToSafeString((r_vResultArray(ACIAmount, lRow))).Trim() & Strings.ChrW(34).ToString() & ",")



                oCurrencyConvert.GetAmountInWords(gPMFunctions.ToSafeLong((r_vResultArray(ACICurrencyID, lRow))), gPMFunctions.ToSafeCurrency(r_vResultArray(ACIAmount, lRow)), sWords)

                sRecord.Append(Strings.ChrW(34).ToString() & sWords & Strings.ChrW(34).ToString() & ",")

                sRecord.Append(Strings.ChrW(34).ToString() & gPMFunctions.ToSafeString((r_vResultArray(ACIBankCode, lRow))).Trim() & Strings.ChrW(34).ToString() & ",")

                sRecord.Append(Strings.ChrW(34).ToString() & gPMFunctions.ToSafeString((r_vResultArray(ACIOurRef, lRow))).Trim() & Strings.ChrW(34).ToString())

                PrintLine(iFileNumber, sRecord.ToString())
            Next lRow
        End If

        FileClose(iFileNumber)
        oCurrencyConvert = Nothing

        GoTo Finally_Renamed

Catch_Renamed:

        ' DO Not Call any functions before here or the error will be lost
        bPMFunc.LogError(m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

        ' If you want to rollback a transaction or something, do it here

Finally_Renamed:

        Return result


    End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (BeginTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function BeginTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLBeginTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CommitTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CommitTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLCommitTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (RollbackTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function RollbackTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLRollbackTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
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
    '
    ' Name: GetTemplateType
    '
    ' Description: Get the Document Template Type from the Template
    '
    ' History: 24/03/2005 DD (taken from original frmDocument)
    '
    ' ***************************************************************** '
    Private Function GetTemplateType(ByVal lDocumentTemplateID As Integer, ByRef r_lDocumentTypeID As Integer) As Integer

        Dim result As Integer = 0
        Dim oDocTemplate As bSIRDocTemplate.Business



        result = gPMConstants.PMEReturnCode.PMTrue


        oDocTemplate = New bSIRDocTemplate.Business
        m_lReturn = oDocTemplate.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(m_lReturn.ToString() + ", GetTemplateType, Failed to get instance of bSIRDocManagerWrapper")
        End If


        m_lReturn = oDocTemplate.GetDetails(vDocumentTemplateId:=lDocumentTemplateID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If


        m_lReturn = oDocTemplate.GetNext(vDocumentTypeId:=r_lDocumentTypeID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If


        oDocTemplate.Dispose()

        oDocTemplate = Nothing

        Return result


    End Function


    ' ***************************************************************** '
    '
    ' Name: CheckDuplicateCheque
    '
    ' Description: Checking for Duplicate Check Number
    '
    '
    ' ***************************************************************** '
    Public Function CheckDuplicateCheque(ByVal vBankAccoutID As Object, ByVal sChequeNumber As String, ByRef r_vDuplicateChequeFound(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()


                m_lReturn = m_oDatabase.Parameters.Add(sName:="bankaccount_id", vValue:=CStr(vBankAccoutID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="cheque_number", vValue:=sChequeNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)


                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckDuplicateChequeSQL, sSQLName:=ACCheckDuplicateChequeName, bStoredProcedure:=ACCheckDuplicateChequeStored, vResultArray:=r_vDuplicateChequeFound)

            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckDuplicateCheque Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AUpdateCheque", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: UpdateChequePrinted
    '
    ' Description: Updates Printed Date & Printed By User ID
    '
    '
    ' ***************************************************************** '
    Public Function UpdateChequePrinted(ByRef lChequeId As Integer, ByRef dtPrintedDate As Date, ByRef lPrintedByUserID As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()


                m_lReturn = m_oDatabase.Parameters.Add(sName:="cheque_id", vValue:=CStr(lChequeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                'developer guide no. 40
                m_lReturn = m_oDatabase.Parameters.Add(sName:="printed_date", vValue:=dtPrintedDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="printed_by_user_id", vValue:=CStr(lPrintedByUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateChequePrintedSQL, sSQLName:=ACUpdateChequePrintedName, bStoredProcedure:=ACUpdateChequePrintedStored, lRecordsAffected:=lRecordsAffected)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Cheque Printed Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateChequePrinted", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetBankStartChequeNumber(ByVal v_lBankID As Integer, ByRef r_sStartChequeNumber As String) As Integer
        Dim result As Integer = 0
        Dim lRecordsAffected As Integer
        Const kMethodName As String = "GetBankStartChequeNumber"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="bankaccount_id", vValue:=CStr(v_lBankID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to add parameter bankaccount_id", gPMConstants.PMELogLevel.PMLogError)
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="start_cheque_number", vValue:=r_sStartChequeNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to add parameter start_cheque_number", gPMConstants.PMELogLevel.PMLogError)
                End If
                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetBankStartChequeNumberSQL, sSQLName:=ACGetBankStartChequeNumberName, bStoredProcedure:=ACGetBankStartChequeNumberStored, lRecordsAffected:=lRecordsAffected)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to execute the procedure " & ACGetBankStartChequeNumberSQL, gPMConstants.PMELogLevel.PMLogError)
                End If

                r_sStartChequeNumber = gPMFunctions.ToSafeString(m_oDatabase.Parameters.Item("start_cheque_number").Value)

            End With

        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function
    Public Function GetBankHighestIssuedChequeNumber(ByVal v_lBankID As Integer, ByRef r_sHighestIssuedChequeNumber As String) As Integer
        Dim result As Integer = 0
        Dim lRecordsAffected As Integer
        Const kMethodName As String = "GetBankHighestIssuedChequeNumber"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="bankaccount_id", vValue:=CStr(v_lBankID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to add parameter bankaccount_id", gPMConstants.PMELogLevel.PMLogError)
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="highest_issued_chequenumber", vValue:=r_sHighestIssuedChequeNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to add parameter highest_issued_chequenumber", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetBankHighestIssuedChequeNumberSQL, sSQLName:=ACGetBankHighestIssuedChequeNumberName, bStoredProcedure:=ACGetBankHighestIssuedChequeNumberStored, lRecordsAffected:=lRecordsAffected)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to execute procedure " & ACGetBankHighestIssuedChequeNumberSQL, gPMConstants.PMELogLevel.PMLogError)
                End If

                r_sHighestIssuedChequeNumber = gPMFunctions.ToSafeString(m_oDatabase.Parameters.Item("highest_issued_chequenumber").Value)

            End With


        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally
            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function
    Public Function CanOverrideChequeNumber(ByVal v_lUserId As Integer, ByRef r_bCanOverrideChequeNumber As Boolean) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "CanOverrideChequeNumber"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(v_lUserId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to add parameter user_id", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCanOverrideChequeNumberSQL, sSQLName:=ACCanOverrideChequeNumberName, bStoredProcedure:=ACCanOverrideChequeNumberStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to execute procedure " & ACCanOverrideChequeNumberSQL, gPMConstants.PMELogLevel.PMLogError)
                End If

                'developer guide no. 162
                r_bCanOverrideChequeNumber = Not (gPMFunctions.ToSafeInteger(m_oDatabase.Records.Item(0).Fields("can_override_cheque_Numbers") & "") = 0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With


        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally
            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function
    Private Function GetBankChequeSequence(ByVal v_lBankID As Integer, ByRef r_vChequeSeqArray(,) As Object) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetBankChequeSequence"


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            .Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="bankaccount_id", vValue:=CStr(v_lBankID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter bankaccount_id", gPMConstants.PMELogLevel.PMLogError)
            End If
            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelBankChequeSequenceSQL, sSQLName:=ACSelBankChequeSequenceName, bStoredProcedure:=ACSelBankChequeSequenceStored, vResultArray:=r_vChequeSeqArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to execute procedure " & ACSelBankChequeSequenceSQL, gPMConstants.PMELogLevel.PMLogError)
            End If

        End With

        Return result
    End Function
    Public Function IsOutOfSequenceCheques(ByRef r_bIsOutofSequenceCheques As Boolean, ByVal v_sStartChequeNumber As String, ByVal v_vChequeData(,) As Object, ByVal v_lBankID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "IsOutOfSequenceCheques"
        Dim vResultArray As Object = Nothing, vChequeSequence As Object
        Dim iNumberOfChequesToPrint As Integer, iNumbersAvail As Integer = 0
        Dim lStartChequeNumber As Integer = 0, lLastChequeNumber As Integer

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim vChequeSequence(v_vChequeData.GetUpperBound(1), 2)

            For iCount As Integer = 0 To v_vChequeData.GetUpperBound(1)

                If CDbl(v_vChequeData(21, iCount)) = v_lBankID Then
                    lLastChequeNumber = gPMFunctions.ToSafeLong(CDbl(v_sStartChequeNumber)) + iNumberOfChequesToPrint
                    iNumberOfChequesToPrint += 1
                End If
            Next


            m_lReturn = CType(GetBankChequeSequence(v_lBankID, vResultArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to execute function GetBankChequeSequence")
            End If
            r_bIsOutofSequenceCheques = False

            If Informations.IsArray(vResultArray) Then

                For jCount As Integer = 0 To vResultArray.GetUpperBound(1)

                    If gPMFunctions.ToSafeLong(CDbl(vResultArray(MainModule.ChequeSequenceArrColPosition.ACFirstAvailable, jCount))) <= gPMFunctions.ToSafeLong(CDbl(v_sStartChequeNumber)) And (gPMFunctions.ToSafeLong(CDbl(v_sStartChequeNumber)) <= gPMFunctions.ToSafeLong(CDbl(vResultArray(MainModule.ChequeSequenceArrColPosition.ACLastAvailable, jCount)))) Then

                        If lLastChequeNumber <= gPMFunctions.ToSafeLong(CDbl(vResultArray(MainModule.ChequeSequenceArrColPosition.ACLastAvailable, jCount))) Then
                            r_bIsOutofSequenceCheques = False
                        Else
                            r_bIsOutofSequenceCheques = True
                            Exit For
                        End If
                    End If
                Next
            End If


        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally
            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function
    Public Function GenerateDefaultChequeArrayForPrinting(ByVal v_vChequeData(,) As Object, ByRef r_vChequeArrayForPrinting(,) As Object) As Integer
        Dim result As Integer = 0
        Dim ChequeArrayForPrinting As Object
        Const kMethodName As String = "GenerateDefaultChequeArrayForPrinting"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim ChequeArrayForPrinting(v_vChequeData.GetUpperBound(1), 2)
            For iCount As Integer = 0 To v_vChequeData.GetUpperBound(1)


                ChequeArrayForPrinting(iCount, 0) = v_vChequeData(ACIBankID, iCount) 'Bank ID


                ChequeArrayForPrinting(iCount, 1) = v_vChequeData(ACIChequeID, iCount) 'Cheque Id


                ChequeArrayForPrinting(iCount, 2) = v_vChequeData(ACIChequeNumber, iCount) 'Default Cheque Number
            Next

            r_vChequeArrayForPrinting = ChequeArrayForPrinting


        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally
            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function
    Public Function GenerateChequeNumbersForBank(ByVal v_lBankID As Integer, ByRef v_vChequeArrayForPrinting(,) As Object, ByVal v_lStartChequeNumber As Double) As Integer
        Dim result As Integer = 0
        Dim iNoOfChequesToPrintForBank, iCount, kCount As Integer
        Dim vResultArray(,) As Object = Nothing
        Dim lStartChequeNumber, lLastChequeNumberInSlot As Integer
        Dim iFlag As Integer
        Dim sHighestIssuedChequeNumber As String = ""
        Const kMethodName As String = "GenerateChequeNumbersForBank"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            For iCount = 0 To v_vChequeArrayForPrinting.GetUpperBound(0)

                If CDbl(v_vChequeArrayForPrinting(iCount, 0)) = v_lBankID Then
                    iNoOfChequesToPrintForBank += 1
                End If
            Next

            m_lReturn = CType(GetBankChequeSequence(v_lBankID, vResultArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to execute function GetBankChequeSequence")
            End If

            iCount = 0
            kCount = 0
            iFlag = 0

            While iCount < iNoOfChequesToPrintForBank
                While kCount <= v_vChequeArrayForPrinting.GetUpperBound(0)

                    If CDbl(v_vChequeArrayForPrinting(kCount, 0)) = v_lBankID Then
                        If Informations.IsArray(vResultArray) Then

                            For jCount As Integer = 0 To vResultArray.GetUpperBound(1)

                                If gPMFunctions.ToSafeLong(CDbl(vResultArray(MainModule.ChequeSequenceArrColPosition.ACFirstAvailable, jCount))) <= v_lStartChequeNumber And v_lStartChequeNumber <= gPMFunctions.ToSafeLong(CDbl(vResultArray(MainModule.ChequeSequenceArrColPosition.ACLastAvailable, jCount))) And iCount < iNoOfChequesToPrintForBank Then
                                    lStartChequeNumber = CInt(v_lStartChequeNumber)
                                    'lLastChequeNumberInSlot = ToSafeLong(vResultArray(ChequeSequenceArrColPosition.ACLastAvailable, iCount))

                                    lLastChequeNumberInSlot = gPMFunctions.ToSafeLong(CDbl(vResultArray(MainModule.ChequeSequenceArrColPosition.ACLastAvailable, jCount)))
                                    While lStartChequeNumber <= lLastChequeNumberInSlot And iCount < iNoOfChequesToPrintForBank

                                        v_vChequeArrayForPrinting(kCount, 2) = lStartChequeNumber
                                        lStartChequeNumber += 1
                                        iCount += 1
                                        kCount += 1
                                    End While

                                    If jCount < vResultArray.GetUpperBound(1) Then

                                        lStartChequeNumber = gPMFunctions.ToSafeLong(CDbl(vResultArray(MainModule.ChequeSequenceArrColPosition.ACFirstAvailable, jCount + 1)))
                                    Else
                                        m_lReturn = CType(GetBankHighestIssuedChequeNumber(v_lBankID, sHighestIssuedChequeNumber), gPMConstants.PMEReturnCode)
                                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                            gPMFunctions.RaiseError(kMethodName, "Failed to execute function GetBankHighestIssuedChequeNumber")
                                        End If
                                        lStartChequeNumber = gPMFunctions.ToSafeLong(CDbl(sHighestIssuedChequeNumber)) + 1
                                    End If
                                    v_lStartChequeNumber = lStartChequeNumber
                                End If

                                If jCount = vResultArray.GetUpperBound(1) Then
                                    iFlag = 1
                                End If
                            Next
                        Else
                            If lStartChequeNumber = 0 Then
                                lStartChequeNumber = CInt(v_lStartChequeNumber)
                            End If

                            v_vChequeArrayForPrinting(kCount, 2) = lStartChequeNumber
                            lStartChequeNumber += 1
                            kCount += 1
                            iCount += 1
                        End If
                    Else
                        kCount += 1
                    End If
                    If iFlag = 1 And iCount < iNoOfChequesToPrintForBank Then
                        If lStartChequeNumber = 0 Then
                            lStartChequeNumber = CInt(v_lStartChequeNumber)
                        End If

                        v_vChequeArrayForPrinting(kCount, 2) = lStartChequeNumber
                        lStartChequeNumber += 1
                        kCount += 1
                        iCount += 1
                        iFlag = 0
                    End If
                End While
            End While


        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally
            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
