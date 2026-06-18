Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
Imports System.Text.RegularExpressions
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 08/10/1998
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a SIRAddress.
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' ************************************************
    ' Added to replace global variables 18/12/2003
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************

    ' Collection of SIRAddresss (Private)
    Private m_oSIRAddresss As bSIRAddress.SIRAddresss

    'CT 19/09/00
    ' Database Class (Private)
    Private m_oArchDatabase As dPMDAO.Database

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    'CT 19/09/00
    ' Options object (Private)
    Private m_oOptions As bSIROptions.Business

    'Tomo280900
    ' Close Architecture Database Flag (Private)
    Private m_bCloseArchDatabase As Boolean

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

    ' Primary Keys to work with
    Private m_lAddressCnt As Integer

    Private m_vAccumulationIds() As Object

    'CT 19/09/00
    Private m_sSystem As String = ""

    Private m_bAccumulationGenerated As Boolean

    Private Function AddAccumulationRecord(ByVal v_sPostalCode As String, ByRef r_lAccumulationID As Integer, Optional ByVal v_lParentID As Integer = 0) As Integer

        Dim result As Integer = 0


        Dim sSQL As String = ""
        Dim vAccumulation(,) As Object = Nothing
        Dim lCaptionID As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        'is this accumulation already on the database?
        sSQL = "SELECT accumulation_id "
        sSQL = sSQL & "FROM accumulation "
        sSQL = sSQL & "WHERE code ='" & v_sPostalCode & "'"

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAccumulation", bStoredProcedure:=False, vResultArray:=vAccumulation)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddAccumulationRecord failed to Get Accumulation Records from Database.", vApp:=ACApp, vClass:=ACClass, vMethod:=" AddAccumulationRecord", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        If Informations.IsArray(vAccumulation) Then
            'Accumulation in the database return the ID found

            r_lAccumulationID = ToSafeInteger(vAccumulation(0, 0))
        Else
            'accumulation NOT in the database
            'get the caption id for this postalcode part - will need to access architecture DB
            With m_oArchDatabase
                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="language_id", vValue:=CStr(m_iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddAccumulationRecord failed to add language_id parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:=" AddAccumulationRecord", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                m_lReturn = .Parameters.Add(sName:="caption", vValue:=v_sPostalCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddAccumulationRecord failed to add caption parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:=" AddAccumulationRecord", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If


                m_lReturn = .Parameters.Add(sName:="caption_id", vValue:=CStr(lCaptionID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddAccumulationRecord failed to add caption_id parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:=" AddAccumulationRecord", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                ' Execute SQL Statement
                'developer guide no.39
                m_lReturn = .SQLAction(sSQL:="spu_pm_caption_id_return", sSQLName:="CaptionIdReturnName", bStoredProcedure:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddAccumulationRecord failed to Execute SQL Statement.", vApp:=ACApp, vClass:=ACClass, vMethod:=" AddAccumulationRecord", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                ' Get the caption_id
                lCaptionID = .Parameters.Item("caption_id").Value
            End With

            'Now that we have the caption ID let add the Record
            With m_oDatabase
                .Parameters.Clear()

                'and add the parameters
                m_lReturn = .Parameters.Add(sName:="accumulation_id", vValue:=CStr(r_lAccumulationID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddAccumulationRecord failed to add accumulation_id parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:=" AddAccumulationRecord", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                m_lReturn = .Parameters.Add(sName:="code", vValue:=v_sPostalCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddAccumulationRecord failed to add code parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:=" AddAccumulationRecord", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                m_lReturn = .Parameters.Add(sName:="caption_id", vValue:=CStr(lCaptionID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddAccumulationRecord failed to add caption_id parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:=" AddAccumulationRecord", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                m_lReturn = .Parameters.Add(sName:="description", vValue:=v_sPostalCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddAccumulationRecord failed to add description parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:=" AddAccumulationRecord", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                m_lReturn = .Parameters.Add(sName:="is_deleted", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddAccumulationRecord failed to add is_deleted parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:=" AddAccumulationRecord", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
                'developer guide no. 40
                m_lReturn = .Parameters.Add(sName:="effective_date", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddAccumulationRecord failed to add effective_date parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:=" AddAccumulationRecord", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                m_lReturn = .Parameters.Add(sName:="quick_code", vValue:=v_sPostalCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddAccumulationRecord failed to add quick_code parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:=" AddAccumulationRecord", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                m_lReturn = .Parameters.Add(sName:="caption", vValue:=v_sPostalCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddAccumulationRecord failed to add caption parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:=" AddAccumulationRecord", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                'If we passed in the parent ID set otherwise it is a null
                If v_lParentID > 0 Then
                    m_lReturn = .Parameters.Add(sName:="parent_id", vValue:=CStr(v_lParentID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                Else

                    'developer guide no. 85
                    m_lReturn = .Parameters.Add(sName:="parent_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddAccumulationRecord failed to add parent_id parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:=" AddAccumulationRecord", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If


                'developer guide no. 85(guide)
                m_lReturn = .Parameters.Add(sName:="accumulation_class_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddAccumulationRecord failed to add accumulation_class_id parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:=" AddAccumulationRecord", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACInsertAccumulationSQL, sSQLName:=ACInsertAccumulationName, bStoredProcedure:=ACInsertAccumulationStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddAccumulationRecord failed to Execute SQL Statement.", vApp:=ACApp, vClass:=ACClass, vMethod:=" AddAccumulationRecord", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                r_lAccumulationID = .Parameters.Item("accumulation_id").Value

            End With

        End If

        Return result

    End Function

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public Property CurrentRecord() As Integer
        Get

            Return m_lCurrentRecord

        End Get
        Set(ByVal Value As Integer)

            Select Case Value
                Case Is < 1
                    m_lCurrentRecord = 0
                Case Is > m_oSIRAddresss.Count()
                    m_lCurrentRecord = m_oSIRAddresss.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Number in Collection
            Return m_oSIRAddresss.Count()

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


    Public Property AddressCnt() As Integer
        Get

            Return m_lAddressCnt

        End Get
        Set(ByVal Value As Integer)

            m_lAddressCnt = Value

        End Set
    End Property

    Public Property AccumulationGenerated() As Boolean
        Get

            Return m_bAccumulationGenerated

        End Get
        Set(ByVal Value As Boolean)

            m_bAccumulationGenerated = Value

        End Set
    End Property

    Public ReadOnly Property AccumulationIds() As Object
        Get

            'Return VB6.CopyArray(m_vAccumulationIds)
            Return m_vAccumulationIds

        End Get
    End Property

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


            'CT 19/09/00 start

            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_bNewInstanceCreated:=m_bCloseArchDatabase, r_oCheckedDatabase:=m_oArchDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            m_oOptions = New bSIROptions.Business
            m_lReturn = m_oOptions.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=sCallingAppName, vDatabase:=vDatabase)



            m_sSystem = m_oOptions.UnderwritingOrAgency

            m_oOptions = Nothing

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'CT 19/09/00 end

            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now




            ' Create SIRAddresss Collection
            m_oSIRAddresss = New bSIRAddress.SIRAddresss()


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
                m_oSIRAddresss = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
                If m_bCloseArchDatabase Then
                    ' Close the Database
                    m_oArchDatabase.CloseDatabase()

                End If
                m_oArchDatabase = Nothing
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




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ''' <summary>
    '''Description: Adds a single SIRAddress directly into the database.
    '''Note: The SIRAddress will NOT be added to the collection.
    ''' </summary>
    ''' <param name="vAddressCnt"></param>
    ''' <param name="vAddress1"></param>
    ''' <param name="vAddress2"></param>
    ''' <param name="vAddress3"></param>
    ''' <param name="vAddress4"></param>
    ''' <param name="vPostalCode"></param>
    ''' <param name="vCountryID"></param>
    ''' <param name="vExternalId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DirectAdd(Optional ByRef vAddressCnt As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing,
                              Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing,
                              Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing,
                              Optional ByRef vCountryID As Object = Nothing, Optional ByRef vExternalId As Object = Nothing,
                              Optional ByRef sAddress5 As String = "",
                              Optional ByRef sAddress6 As String = "",
                              Optional ByRef sAddress7 As String = "",
                              Optional ByRef sAddress8 As String = "",
                              Optional ByRef sAddress9 As String = "",
                              Optional ByRef sAddress10 As String = "") As Integer
        Dim nResult As Integer = 0
        Dim oSIRAddress As bSIRAddress.SIRAddress

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new SIRAddress
            oSIRAddress = New bSIRAddress.SIRAddress()
            m_lReturn = CType(oSIRAddress.Initialise(sUsername:=m_sUsername,
                                                     sPassword:=m_sPassword,
                                                     iUserID:=m_iUserID,
                                                     iSourceID:=m_iSourceID,
                                                     iLanguageID:=m_iLanguageID,
                                                     iCurrencyID:=m_iCurrencyID,
                                                     iLogLevel:=m_iLogLevel,
                                                     sCallingAppName:=m_sCallingAppName,
                                                     vDatabase:=m_oDatabase),
                                                     gPMConstants.PMEReturnCode)

            ' Populate SIRAddress Attributes
            'developer guide no. 98
            m_lReturn = CType(oSIRAddress.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vAddressCnt:=vAddressCnt, vAddress1:=vAddress1,
                                                        vAddress2:=vAddress2, vAddress3:=vAddress3, vAddress4:=vAddress4, vPostalCode:=vPostalCode,
                                                        vCountryID:=vCountryID, vExternalId:=vExternalId,
                                                        sAddress5:=sAddress5, sAddress6:=sAddress6,
                                                        sAddress7:=sAddress7, sAddress8:=sAddress8,
                                                        sAddress9:=sAddress9, sAddress10:=sAddress10), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                oSIRAddress = Nothing
                Return nResult
            End If

            ' Add the SIRAddress to the Database
            m_lReturn = CType(oSIRAddress.AddItem(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                oSIRAddress = Nothing
                Return nResult
            End If

            ' Retain the Primary Key of the SIRAddress Added
            With oSIRAddress
                AddressCnt = .AddressCnt
            End With

            oSIRAddress = Nothing

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectDelete (Public)
    '
    ' Description: Deletes a single SIRAddress directly from the database.
    '        Note: The SIRAddress will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    Public Function DirectDelete(Optional ByRef vAddressCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIRAddress As bSIRAddress.SIRAddress

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new SIRAddress
            oSIRAddress = New bSIRAddress.SIRAddress()
            m_lReturn = CType(oSIRAddress.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            ' Set SIRAddress Primary Key

            'developer guide no. 98
            m_lReturn = CType(oSIRAddress.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMDelete, vAddressCnt:=vAddressCnt), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRAddress = Nothing
                Return result
            End If

            ' Delete the SIRAddress from the Database
            m_lReturn = CType(oSIRAddress.DeleteItem(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRAddress = Nothing
                Return result
            End If

            oSIRAddress = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required SIRAddresss and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vLockMode As Integer = 0, Optional ByRef vAddressCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        'developer guide no. 112
        Dim oFields As DataRow
        Dim oSIRAddress As bSIRAddress.SIRAddress

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oSIRAddresss.Clear()

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

            If (Not Informations.IsNothing(vAddressCnt)) And (Not Double.TryParse(CStr(vAddressCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vAddressCnt=" & vAddressCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                Return result
            End If


            If Not Informations.IsNothing(vAddressCnt) Then

                ' Create New SIRAddress
                oSIRAddress = New bSIRAddress.SIRAddress()
                m_lReturn = CType(oSIRAddress.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

                ' Set component primary keys
                With oSIRAddress
                    .AddressCnt = vAddressCnt

                    m_lReturn = CType(.SelectItem(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End With

                ' Add SIRAddress to collection
                If m_oSIRAddresss.Count = 0 Then
                    m_oSIRAddresss.Add(Nothing)
                End If
                m_lReturn = CType(m_oSIRAddresss.Add(oNewSIRAddress:=oSIRAddress), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                oSIRAddress = Nothing

            Else

                ' No Key, Get All Records

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllDetailsSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=0)

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
                    oSIRAddress = New bSIRAddress.SIRAddress()
                    m_lReturn = (oSIRAddress).Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

                    ' Set oFields to refer to one Record
                    'developer guide no. 111
                    oFields = m_oDatabase.Records.Item(lSub - 1).Fields()

                    ' Set component primary keys from current record
                    With oSIRAddress
                        'SD 31/07/2002 Scalability changes
                        .AddressCnt = gPMFunctions.NullToLong(oFields("address_cnt"))

                        m_lReturn = CType(.SelectItem(), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End With

                    ' Add SIRAddress to collection
                    If m_oSIRAddresss.Count = 0 Then
                        m_oSIRAddresss.Add(Nothing)
                    End If
                    m_lReturn = CType(m_oSIRAddresss.Add(oNewSIRAddress:=oSIRAddress), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oSIRAddress = Nothing
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


    ''' <summary>
    ''' Gets the required SIRAddresss and populate the Collection
    ''' </summary>
    ''' <param name="vAddressCnt"></param>
    ''' <param name="vSourceID"></param>
    ''' <param name="vAddressID"></param>
    ''' <param name="vAddress1"></param>
    ''' <param name="vAddress2"></param>
    ''' <param name="vAddress3"></param>
    ''' <param name="vAddress4"></param>
    ''' <param name="vPostalCode"></param>
    ''' <param name="vCountryID"></param>
    ''' <param name="vCreatedByID"></param>
    ''' <param name="vDateCreated"></param>
    ''' <param name="vModifiedByID"></param>
    ''' <param name="vLastModified"></param>
    ''' <param name="vExternalId"></param>
    ''' <param name="sAddress5"></param>
    ''' <param name="sAddress6"></param>
    ''' <param name="sAddress7"></param>
    ''' <param name="sAddress8"></param>
    ''' <param name="sAddress9"></param>
    ''' <param name="sAddress10"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetNext(Optional ByRef vAddressCnt As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vAddressID As Object = Nothing,
                            Optional ByRef vAddress1 As Object = Nothing,
                            Optional ByRef vAddress2 As Object = Nothing,
                            Optional ByRef vAddress3 As Object = Nothing,
                            Optional ByRef vAddress4 As Object = Nothing,
                            Optional ByRef vPostalCode As Object = Nothing,
                            Optional ByRef vCountryID As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vModifiedByID As Object = Nothing, Optional ByRef vLastModified As Object = Nothing,
                            Optional ByRef vExternalId As Object = Nothing,
                            Optional ByRef sAddress5 As String = "",
                            Optional ByRef sAddress6 As String = "",
                            Optional ByRef sAddress7 As String = "",
                            Optional ByRef sAddress8 As String = "",
                            Optional ByRef sAddress9 As String = "",
                            Optional ByRef sAddress10 As String = "") As Integer

        Dim nResult As Integer = 0

        Dim oSIRAddress As bSIRAddress.SIRAddress
        Dim iStatus As Integer

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oSIRAddresss.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                nResult = gPMConstants.PMEReturnCode.PMEOF
            End If

            oSIRAddress = m_oSIRAddresss.Item(m_lCurrentRecord)

            ' Get the SIRAddress Property Values
            m_lReturn = CType(oSIRAddress.GetProperties(iStatus, vAddressCnt:=vAddressCnt,
                                                        vSourceID:=vSourceID,
                                                        vAddressID:=vAddressID,
                                                        vAddress1:=vAddress1,
                                                        vAddress2:=vAddress2,
                                                        vAddress3:=vAddress3,
                                                        vAddress4:=vAddress4,
                                                        vPostalCode:=vPostalCode,
                                                        vCountryID:=vCountryID, vCreatedByID:=vCreatedByID, vDateCreated:=vDateCreated, vModifiedByID:=vModifiedByID, vLastModified:=vLastModified,
                                                        vExternalId:=vExternalId,
                                                        sAddress5:=sAddress5,
                                                        sAddress6:=sAddress6,
                                                        sAddress7:=sAddress7,
                                                        sAddress8:=sAddress8,
                                                        sAddress9:=sAddress9, sAddress10:=sAddress10), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRAddress = Nothing

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", excep:=excep)

            Return nResult
        End Try
    End Function

    ' ***************************************************************** '

    ''' <summary>
    ''' EditAdd (Public)
    ''' Description: Adds the supplied SIRAddress into the Collection.
    '''              After the Add, lKey should be equal to the number
    '''              of items in the collection.
    ''' Changes: RWH(15/09/2000) Added CountryId parameter
    ''' </summary>
    ''' <param name="lRow"></param>
    ''' <param name="vAddressCnt"></param>
    ''' <param name="vAddress1"></param>
    ''' <param name="vAddress2"></param>
    ''' <param name="vAddress3"></param>
    ''' <param name="vAddress4"></param>
    ''' <param name="vPostalCode"></param>
    ''' <param name="vCountryID"></param>
    ''' <param name="vExternalId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vAddressCnt As Object = Nothing,
                            Optional ByRef vAddress1 As Object = Nothing,
                            Optional ByRef vAddress2 As Object = Nothing,
                            Optional ByRef vAddress3 As Object = Nothing,
                            Optional ByRef vAddress4 As Object = Nothing,
                            Optional ByRef vPostalCode As Object = Nothing,
                            Optional ByRef vCountryID As Object = Nothing,
                            Optional ByRef vExternalId As Object = Nothing,
                            Optional ByRef sAddress5 As String = "",
                            Optional ByRef sAddress6 As String = "",
                            Optional ByRef sAddress7 As String = "",
                            Optional ByRef sAddress8 As String = "",
                            Optional ByRef sAddress9 As String = "",
                            Optional ByRef sAddress10 As String = "",
                            Optional ByVal sUniqueId As String = "",
                            Optional ByVal sScreenHeirarchy As String = "") As Integer
        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim oSIRAddress As bSIRAddress.SIRAddress

        Try

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oSIRAddresss.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new SIRAddress
            oSIRAddress = New bSIRAddress.SIRAddress()
            m_lReturn = CType(oSIRAddress.Initialise(sUsername:=m_sUsername,
                                                     sPassword:=m_sPassword,
                                                     iUserID:=m_iUserID,
                                                     iSourceID:=m_iSourceID,
                                                     iLanguageID:=m_iLanguageID,
                                                     iCurrencyID:=m_iCurrencyID,
                                                     iLogLevel:=m_iLogLevel,
                                                     sCallingAppName:=m_sCallingAppName,
                                                     vDatabase:=m_oDatabase),
                                                     gPMConstants.PMEReturnCode)

            'developer guide no.98
            m_lReturn = CType(oSIRAddress.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd,
                                                        vAddressCnt:=vAddressCnt, vAddress1:=vAddress1,
                                                        vAddress2:=vAddress2, vAddress3:=vAddress3,
                                                        vAddress4:=vAddress4, vPostalCode:=vPostalCode,
                                                        vCountryID:=vCountryID, vExternalId:=vExternalId,
                                                        sAddress5:=sAddress5, sAddress6:=sAddress6,
                                                        sAddress7:=sAddress7, sAddress8:=sAddress8,
                                                        sAddress9:=sAddress9, sAddress10:=sAddress10,
                                                        sUniqueId:=sUniqueId, sScreenHeirarchy:=sScreenHeirarchy), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = m_lReturn
                oSIRAddress = Nothing
                Return nResult
            End If

            ' Add SIRAddress to collection
            If m_oSIRAddresss.Count = 0 Then
                m_oSIRAddresss.Add(Nothing)
            End If
            m_lReturn = CType(m_oSIRAddresss.Add(oNewSIRAddress:=oSIRAddress), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                oSIRAddress = Nothing
                Return nResult
            End If
            oSIRAddress = Nothing
            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", excep:=excep)

            Return nResult
        End Try
    End Function

    ''' <summary>
    ''' EditUpdate (Public) : Description: Validates that this action is valid on the SIRAddress
    '''  specified and updates the SIRAddress with the new values.
    ''' Changes: RWH(15/09/2000) Added CountryId parameter
    ''' </summary>
    ''' <param name="lRow"></param>
    ''' <param name="vAddressCnt"></param>
    ''' <param name="vAddress1"></param>
    ''' <param name="vAddress2"></param>
    ''' <param name="vAddress3"></param>
    ''' <param name="vAddress4"></param>
    ''' <param name="vPostalCode"></param>
    ''' <param name="vCountryID"></param>
    ''' <param name="vExternalId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vAddressCnt As Object = Nothing,
                               Optional ByRef vAddress1 As Object = Nothing,
                               Optional ByRef vAddress2 As Object = Nothing,
                               Optional ByRef vAddress3 As Object = Nothing,
                               Optional ByRef vAddress4 As Object = Nothing,
                               Optional ByRef vPostalCode As Object = Nothing,
                               Optional ByRef vCountryID As Object = Nothing,
                               Optional ByRef vExternalId As Object = Nothing,
                               Optional ByRef sAddress5 As String = "",
                               Optional ByRef sAddress6 As String = "",
                               Optional ByRef sAddress7 As String = "",
                               Optional ByRef sAddress8 As String = "",
                               Optional ByRef sAddress9 As String = "",
                               Optional ByRef sAddress10 As String = "",
                               Optional ByVal sUniqueId As String = "",
                               Optional ByVal sScreenHeirarchy As String = "") As Integer


        Dim nResult As Integer = 0
        Dim oSIRAddress As bSIRAddress.SIRAddress
        Dim nStatus As Integer

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oSIRAddresss.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oSIRAddress = m_oSIRAddresss.Item(lRow)

            ' Check the Status of the SIRAddress

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oSIRAddress.DatabaseStatus
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Leave Status as Add
                    nStatus = gPMConstants.PMEComponentAction.PMAdd
                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                    ' Error
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                Case Else
                    ' Set Edit (Update) Status
                    nStatus = gPMConstants.PMEComponentAction.PMEdit
            End Select

            ' Update SIRAddress Attributes
            'developer guide no.98
            m_lReturn = CType(oSIRAddress.SetProperties(iStatus:=nStatus,
                                                        vAddressCnt:=vAddressCnt,
                                                        vAddress1:=vAddress1,
                                                        vAddress2:=vAddress2,
                                                        vAddress3:=vAddress3,
                                                        vAddress4:=vAddress4,
                                                        vPostalCode:=vPostalCode, vCountryID:=vCountryID,
                                                        vExternalId:=vExternalId,
                                                        sAddress5:=sAddress5,
                                                        sAddress6:=sAddress6,
                                                        sAddress7:=sAddress7,
                                                        sAddress8:=sAddress8,
                                                        sAddress9:=sAddress9,
                                                        sAddress10:=sAddress10,
                                                        sUniqueId:=sUniqueId,
                                                        sScreenHeirarchy:=sScreenHeirarchy), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = m_lReturn
                oSIRAddress = Nothing
                Return nResult
            End If

            ' Release reference to SIRAddress
            oSIRAddress = Nothing

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete (Public)
    '
    ' Description: Validate that the specified SIRAddress can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oSIRAddress As bSIRAddress.SIRAddress

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oSIRAddresss.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oSIRAddress = m_oSIRAddresss.Item(lRow)

            ' Check the Status of the SIRAddress

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oSIRAddress.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oSIRAddress.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oSIRAddress.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            ' Release reference to SIRAddress
            oSIRAddress = Nothing

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
            For lSub As Integer = 1 To m_oSIRAddresss.Count()
                Select Case m_oSIRAddresss.Item(lSub).DatabaseStatus
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
        Dim oSIRAddress As bSIRAddress.SIRAddress = Nothing
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oSIRAddresss.Count()
                oSIRAddress = m_oSIRAddresss.Item(lSub)


                Select Case oSIRAddress.DatabaseStatus
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
                        m_lReturn = CType(oSIRAddress.AddItem(), gPMConstants.PMEReturnCode)
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
                        m_lReturn = CType(oSIRAddress.UpdateItem(), gPMConstants.PMEReturnCode)
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
                        m_lReturn = CType(oSIRAddress.DeleteItem(), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Retain the Primary Key of the SIRAddress
            With oSIRAddress
                AddressCnt = .AddressCnt
            End With

            ' Release last reference
            oSIRAddress = Nothing

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
                    Do While lSub <= m_oSIRAddresss.Count()

                        ' With the item
                        With m_oSIRAddresss.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oSIRAddresss.Delete(lSub)

                                    'CT 19/09/00 generate accumulation records
                                Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit
                                    ' If we haven't already started a transaction start one.
                                    m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        Return gPMConstants.PMEReturnCode.PMFalse
                                    End If
                                    bTransStarted = True

                                    m_lReturn = ProcessAccumulationRecords()
                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                                        Return gPMConstants.PMEReturnCode.PMFalse
                                    Else
                                        m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)
                                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                            Return gPMConstants.PMEReturnCode.PMFalse
                                        End If
                                    End If

                                    ' Set Status to view
                                    .DatabaseStatus = gPMConstants.PMEComponentAction.PMView

                                    lSub += 1 ' Anything Else
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
    ' Name: GetContacts
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function GetContacts(ByRef vContacts(,) As Object) As Integer

        Dim result As Integer = 0
        Dim vTabArray(,) As Object
        Dim vResultArray(,) As Object
        Dim m_oLookup As bPMLookup.Business
        Dim sSQL As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Now get the contact details for this address
            'sp todo - make this stored procedure
            sSQL = "SELECT contact.contact_cnt, area_code, number, extension, contact_type_id, " &
                   "contact.description FROM contact, contact_address_usage " &
                   "WHERE contact.contact_cnt = contact_address_usage.contact_cnt " &
                   "AND contact_address_usage.address_cnt = " & CStr(m_lAddressCnt)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GETCONTACTS", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vContacts)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Now convert the contact types to descriptions, if we have contacts
            If Informations.IsArray(vContacts) Then

                ' Create PM Lookup Business Object
                m_oLookup = New bPMLookup.Business()

                ' Initialise PM Lookup Business passing our Database Reference.
                m_lReturn = m_oLookup.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

                ' Get the descriptions and codes for contact type

                'developer guide no. 17
                vResultArray = Nothing
                ReDim vTabArray(3, 0)
                For i As Integer = vContacts.GetLowerBound(1) To vContacts.GetUpperBound(1)

                    ' Setup Lookup Table Names
                    If i > 0 Then
                        ReDim Preserve vTabArray(3, i)
                    End If


                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, i) = "contact_type"


                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, i) = vContacts(4, i)

                Next i

                ' Get the Lookup items

                m_lReturn = m_oLookup.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=vTabArray, iLanguageID:=m_iLanguageID, dtEffectiveDate:=DateTime.Now, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'In the contactarray, replace the contact type id with the
                'looked up description
                For i As Integer = vContacts.GetLowerBound(1) To vContacts.GetUpperBound(1)



                    vContacts(4, i) = CStr(vResultArray(1, i)).Trim()

                Next i

                ' Terminate Lookup Component
                m_oLookup.Dispose()

                ' Release Lookup Reference
                m_oLookup = Nothing

            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetContactsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetContacts", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: UpdateContacts
    '
    ' Description: Update the contact_address usage table with old
    ' and new contacts for the address.
    '
    ' ***************************************************************** '
    Public Function UpdateContacts(ByRef vAddressCnt As Object, Optional ByRef vAddContacts() As Object = Nothing, Optional ByRef vDeleteContacts() As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Delete old contacts for address if supplied

            If Not Informations.IsNothing(vDeleteContacts) Then

                For i As Integer = vDeleteContacts.GetLowerBound(0) To vDeleteContacts.GetUpperBound(0)


                    If ToSafeInteger(vDeleteContacts(i)) <> 0 Then



                        sSQL = "DELETE from contact_address_usage WHERE " &
                               "contact_cnt = " & CStr(vDeleteContacts(i)) & " AND " &
                               "address_cnt = " & CStr(vAddressCnt)

                        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="DELCONADDS", bStoredProcedure:=False)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                Next i
            End If

            'Add new contacts for address if supplied

            If Not Informations.IsNothing(vAddContacts) Then

                For i As Integer = vAddContacts.GetLowerBound(0) To vAddContacts.GetUpperBound(0)


                    If ToSafeInteger(vAddContacts(i)) <> 0 Then



                        sSQL = "INSERT INTO contact_address_usage " &
                               "(contact_cnt, address_cnt) VALUES " &
                               "(" & CStr(vAddContacts(i)) & ", " &
                               CStr(vAddressCnt) & ")"

                        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="ADDCONADDS", bStoredProcedure:=False)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                Next i
            End If


            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateContactsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateContacts", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


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
    'Private Function CheckMandatory(Optional ByRef vAddressCnt As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vAddressID As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vModifiedByID As Object = Nothing, Optional ByRef vLastModified As Object = Nothing) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' {* USER DEFINED CODE (Begin) *}
    '

    '
    'If (Informations.IsNothing(vSourceID)) Or (Object.Equals(vSourceID, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '

    '
    'If (Informations.IsNothing(vAddressID)) Or (Object.Equals(vAddressID, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '

    '
    'If (Informations.IsNothing(vAddress1)) Or (Object.Equals(vAddress1, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '

    '
    'If (Informations.IsNothing(vPostalCode)) Or (Object.Equals(vPostalCode, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '

    '
    'If (Informations.IsNothing(vCountryID)) Or (Object.Equals(vCountryID, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '

    '
    'If (Informations.IsNothing(vCreatedByID)) Or (Object.Equals(vCreatedByID, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '

    '
    'If (Informations.IsNothing(vDateCreated)) Or (Object.Equals(vDateCreated, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '
    '
    ' {* USER DEFINED CODE (End) *}
    '
    'Return result
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
    ' Name: Clear
    '
    ' Description: Clear internal collections etc
    '
    '
    ' ***************************************************************** '
    Public Function Clear() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lCurrentRecord = 0

            ' Create SIRAddresss Collection
            m_oSIRAddresss = Nothing
            m_oSIRAddresss = New bSIRAddress.SIRAddresss()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ClearFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="Clear", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Function ProcessAccumulationRecords() As gPMConstants.PMEReturnCode
        ' ***************************************************************** '
        ' Name: ProcessAccumulationRecords
        '
        ' Description: Add accumulate records for post code blocks
        '              if they do not already exist
        '
        ' Created : Cathy T 19/09/00
        '
        ' *****************************************************************

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse


        Dim sSQL As String = ""
        Dim vPostalCode(,) As Object = Nothing 'Return Results from Database 
        Dim sPostalCode, sPostalCode1, sPostalCode2, sPostalCode3 As String 'The postcodepart to pass to AddAccumulationRecord 
        Dim sPostalCodePart As String = ""
        Dim sChar As String = ""
        Dim i As Integer
        Dim lAccumulationID, lParentId As Integer
        Dim vArray As Object 'Set the results of the m_AccumulationsId
        Dim sQASInstalled As String = ""


        result = gPMConstants.PMEReturnCode.PMTrue


        m_lReturn = CType(bPMFunc.RetrieveSingleSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=13, r_sOptionValue:=sQASInstalled, v_iSourceID:=m_iSourceID), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If


        'Now get the postal code for this address
        ' START CHANGES - Changed By: AAB  - Changed On: 23-Mar-2004 11:56
        ' I am changing this to get the Country Code, we do not need to split the US Zip Code
        sSQL = "SELECT Address.address_id, Address.postal_code, Country.code "
        sSQL = sSQL & "FROM Address INNER JOIN Country ON Address.country_id = Country.country_id "
        sSQL = sSQL & "WHERE address_cnt =" & CStr(m_lAddressCnt)
        ' END CHANGES - Changed By: AAB  - Changed On: 23-Mar-2004 11:56

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetPostalCode", bStoredProcedure:=False, vResultArray:=vPostalCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        'We will have a postcode of valid format as it was validated on input
        'and if this procedure has been called then it will have been commited in ProcessCommand
        'Break the postcode into the 3 postalcode parts/blocks
        If (Informations.IsArray(vPostalCode)) And m_sSystem = "U" Then
            ' RFC15082002 - Do not do the Accumulations Processing if the Postcode is blank.

            If CStr(vPostalCode(1, 0)).Trim() <> "" Then
                ' if this is the usa or another country with postalcode set on
                ' add the accumulation without formatting it first

                If (CStr(vPostalCode(2, 0)).Trim() = "USA" Or CStr(vPostalCode(2, 0)).Trim() <> "GBR") Or sQASInstalled = "0" Then

                    AccumulationGenerated = True

                    ReDim vArray(0)


                    m_lReturn = CType(AddAccumulationRecord(v_sPostalCode:=CStr(vPostalCode(1, 0)), r_lAccumulationID:=lAccumulationID), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Set the values in the array

                    vArray(0) = lAccumulationID

                Else




                    If CStr(vPostalCode(0, 0)) <> CStr(vPostalCode(1, 0)) And CStr(vPostalCode(1, 0)).Trim() <> "" Then

                        AccumulationGenerated = True

                        ReDim vArray(3)


                        sPostalCode = CStr(vPostalCode(1, 0))
                        sPostalCode = sPostalCode.Substring(0, sPostalCode.Length - 2)
                        'insert space if missing
                        If Informations.Mid(sPostalCode, sPostalCode.Length - 1, 1) <> " " Then
                            sChar = sPostalCode.Substring(sPostalCode.Length - 1)
                            sPostalCode = sPostalCode.Substring(0, sPostalCode.Length - 1)
                            sPostalCode = sPostalCode & " " & sChar
                        End If
                        sPostalCode3 = sPostalCode
                        'Trim space now we have stored 3rd segment of postcode
                        sPostalCode = sPostalCode.Substring(0, sPostalCode.Length - 2)
                        sPostalCode2 = sPostalCode

                        i = 1
                        '  Do Until Mid(sPostalCode, i, 1) Like "[0-9]"
                        Do Until Regex.IsMatch(Informations.Mid(sPostalCode, i, 1), "[0-9]")
                            i += 1
                        Loop
                        sPostalCode1 = sPostalCode.Substring(0, i - 1)

                        'We now have our post code blocks so process each of them
                        For i = 1 To 3
                            Select Case i
                                'store our current segment
                                Case 1
                                    sPostalCodePart = sPostalCode1
                                Case 2
                                    sPostalCodePart = sPostalCode2
                                Case 3
                                    sPostalCodePart = sPostalCode3
                            End Select

                            If i = 1 Then
                                m_lReturn = CType(AddAccumulationRecord(v_sPostalCode:=sPostalCodePart, r_lAccumulationID:=lAccumulationID), gPMConstants.PMEReturnCode)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    Return gPMConstants.PMEReturnCode.PMFalse
                                End If
                                'Set the Parent ID to be used later
                                lParentId = lAccumulationID
                            Else
                                m_lReturn = CType(AddAccumulationRecord(v_sPostalCode:=sPostalCodePart, r_lAccumulationID:=lAccumulationID, v_lParentID:=lParentId), gPMConstants.PMEReturnCode)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    Return gPMConstants.PMEReturnCode.PMFalse
                                End If
                                'Set the Parent ID to be used later
                                lParentId = lAccumulationID
                            End If

                            'Set the values in the array

                            vArray(i) = lAccumulationID
                        Next i


                        m_vAccumulationIds = vArray
                    End If
                End If
            End If

        End If

        Return result

    End Function

    Public Function GetCountry(ByRef iCountryId As Integer, ByRef sCountryCode As String, Optional ByRef r_lPostalCodeVisibilityId As Integer = 0) As Integer
        ' ***************************************************************** '
        ' Name: GetCountry
        '
        ' Description: Get Country Code
        '
        ' Created : eck 040101
        '
        ' *****************************************************************


        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oArchDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="country_id", vValue:=CStr(iCountryId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

                'Now get the postal code for this address
                sSQL = "SELECT iso_code, postcode_visibility_id "
                sSQL = sSQL & "FROM country "
                sSQL = sSQL & "WHERE country_id = {country_id}"

                ' Execute SQL Statement
                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetCountry", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                sCountryCode = CStr(vResultArray(0, 0))

                If CStr(vResultArray(1, 0)) = "" Then
                    r_lPostalCodeVisibilityId = 0
                Else
                    r_lPostalCodeVisibilityId = ToSafeInteger(vResultArray(1, 0))
                End If

            End With
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=" GetCountry", vApp:=ACApp, vClass:=ACClass, vMethod:=" GetCountry", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            With m_oArchDatabase

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
    Public Function MultipleUse(ByRef v_lAddressCnt As Integer, ByRef r_bMultipleUse As Boolean) As Integer
        ' ***************************************************************** '
        ' Name: MultipleUse
        '
        ' Description: Check if address is used on more than one party
        '
        ' Created : PSL 24/02/2003
        ' Amended : CJB 20/11/2003 Change datatype from PMInteger to PMLong for Address_cnt
        '
        ' *****************************************************************


        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim lUses As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oArchDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="Address_cnt", vValue:=CStr(v_lAddressCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If


                ' Execute SQL Statement
                m_lReturn = .SQLSelect(sSQL:=ACMultipleUseSQL, sSQLName:=ACMultipleUseName, bStoredProcedure:=ACMultipleUseStored, lNumberRecords:=0, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Informations.IsArray(vResultArray) Then

                    lUses = ToSafeInteger(vResultArray(0, 0))
                    r_bMultipleUse = lUses > 1
                Else
                    result = gPMConstants.PMEReturnCode.PMError

                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check for MultipleUse", vApp:=ACApp, vClass:=ACClass, vMethod:="MultipleUse", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                End If
            End With
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check for MultipleUse", vApp:=ACApp, vClass:=ACClass, vMethod:="MultipleUse", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    Public Function DuplicateAddress(ByVal v_lAddressCnt As Integer, ByVal v_sReference As String, ByVal v_lUserID As Integer, ByVal v_lSourceID As Integer, ByRef r_lNewAddressCnt As Integer) As Integer
        ' ***************************************************************** '
        ' Name: DuplicateAddress
        '
        ' Description: Check if address is used on more than one party
        '
        ' Created : PSL 24/02/2003
        '
        ' *****************************************************************


        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oArchDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="Address_cnt", vValue:=CStr(v_lAddressCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="PartyCode", vValue:=v_sReference, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="User_id", vValue:=CStr(v_lUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="Source_id", vValue:=CStr(v_lSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLSelect(sSQL:=ACDuplicateAddressSQL, sSQLName:=ACDuplicateAddressName, bStoredProcedure:=ACDuplicateAddressStored, lNumberRecords:=0, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Informations.IsArray(vResultArray) Then
                    r_lNewAddressCnt = ToSafeInteger(vResultArray(0, 0))
                Else
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=" Failed to Duplicate Address in database", vApp:=ACApp, vClass:=ACClass, vMethod:="DuplicateAddress", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    result = gPMConstants.PMEReturnCode.PMError
                End If

            End With
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=" DuplicateAddress", vApp:=ACApp, vClass:=ACClass, vMethod:=" DuplicateAddress", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function
End Class