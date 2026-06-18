Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'Developer Guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name:   Business
    ' Description:  Creatable Business class which contains all the
    '               methods, Business rules required to manipulate
    '               a CLMRTInfoChklst.
    ' Author:       SK
    ' Date:         06/07/2000
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
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Collection of CLMRTInfoChklsts (Private)
    Private m_oCLMRiskTypeInfoChecklists As bCLMRTInfoChkLst.CLMRTInfoChklsts

    Private m_oBackOffice As Object

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

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
    Private m_lRisk_type_Exp_ser_id As Integer

    ' PM Lookup Business Component (Private)
    Private m_oLookup As bPMLookup.Business
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

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
                Case Is > m_oCLMRiskTypeInfoChecklists.Count()
                    m_lCurrentRecord = m_oCLMRiskTypeInfoChecklists.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property
    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Number in Collection
            Return m_oCLMRiskTypeInfoChecklists.Count()

        End Get
    End Property
    Public ReadOnly Property Task() As Integer
        Get

            Return m_iTask

        End Get
    End Property

    '##ModelId=39629EDC0058
    Public ReadOnly Property Navigate() As Integer
        Get

            Return m_lNavigate

        End Get
    End Property

    '##ModelId=39629EDC0062
    Public ReadOnly Property ProcessMode() As Integer
        Get

            Return m_lProcessMode

        End Get
    End Property

    '##ModelId=39629EDC0063
    Public ReadOnly Property TransactionType() As String
        Get

            Return m_sTransactionType

        End Get
    End Property

    '##ModelId=39629EDC006C
    Public ReadOnly Property EffectiveDate() As Date
        Get

            Return m_dtEffectiveDate

        End Get
    End Property


    Public Property Risk_type_Exp_ser_id() As Integer
        Get

            Return m_lRisk_type_Exp_ser_id

        End Get
        Set(ByVal Value As Integer)

            m_lRisk_type_Exp_ser_id = Value

        End Set
    End Property


    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    ' Author: SK
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise


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




            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create CLMRTInfoChklsts Collection
            m_oCLMRiskTypeInfoChecklists = New bCLMRTInfoChkLst.CLMRTInfoChklsts()

            ' Create Back Office Link
            '    Set m_oBackOffice = New bBackOfficeLink.bBOLink
            If m_oBackOffice Is Nothing Then

                m_oBackOffice = New bBackOfficeLink.bBOLink()

                '******Changed Here To Make It Comatible For Client Server Model ******
                '******Added By Pandu Date :12-10-2000 ********************************

                m_lReturn = m_oBackOffice.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                '******End of Change Here To Make It Comatible For Client Server Model ******
                '******Added By Pandu Date :12-10-2000 ***

                If m_oBackOffice Is Nothing Then


                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the BackOffice Link Object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If
            End If


            ' Create PM Lookup Business Object 'SK-not sure if we are going to use it
            m_oLookup = New bPMLookup.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUserName:=sUserName, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name:         ClearColl (Public)
    ' Description:  Clears the collection & sets it to zero
    ' Author:       SK
    ' Date:         06/07/2000
    ' ***************************************************************** '
    Public Sub ClearColl()
        m_oCLMRiskTypeInfoChecklists.Clear()
    End Sub
    ' ***************************************************************** '
    ' Name:         CollCount (Public)
    ' Description:  Get the current count of the items in the collection
    ' Author:       SK
    ' Date:         06/07/2000
    ' ***************************************************************** '
    Public Function CollCount() As Integer
        Return m_oCLMRiskTypeInfoChecklists.Count()
    End Function

    ' ***************************************************************** '
    ' Name:         SelectRiskType (Public)
    ' Description:  Selects all the Risk Type records in the
    '               Risk_Type Table
    ' Author:       SK
    ' Date:         06/07/2000
    ' ***************************************************************** '
    Public Function SelectRiskType(ByRef rvResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        'Developer Guide no. 21

        Dim oFields As DataRow
        Dim vResultArray(,) As Object
        Dim lRecordCount As Integer

        Try

            result = True

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskTypeSQL, sSQLName:=ACGetRiskTypeName, bStoredProcedure:=ACGetRiskTypeStored)

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

            'array will have 4 columns, indefinate no. of rows
            ReDim vResultArray(3, 0)

            For nRecCount As Integer = 1 To lRecordCount

                'ReDim the arrasy since we are changing its dimensions
                '& preserve it since we need tosave the values of the array

                ReDim Preserve vResultArray(3, nRecCount - 1)

                'developer Guide no. 111
                'oFields = m_oDatabase.Records.Item(nRecCount).Fields()
                oFields = m_oDatabase.Records.Item(nRecCount - 1).Fields()


                vResultArray(0, nRecCount - 1) = gPMFunctions.NullToLong(oFields("risk_type_id"))

                vResultArray(1, nRecCount - 1) = gPMFunctions.NullToString(oFields("code"))

                vResultArray(2, nRecCount - 1) = gPMFunctions.NullToString(oFields("description"))

                vResultArray(3, nRecCount - 1) = gPMFunctions.NullToInteger(oFields("show_information_checklist"))

            Next


            rvResultArray = vResultArray


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectRiskType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectRiskType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name:         SelRiskTypeExpSer (Public)
    ' Description:  Selects all the Expert Services for the
    '               specified Risk Type
    ' Order of items
    ' in array:     Risk_type_Expert_service.Risk_type_Exp_ser_id,
    '               Expert_Service.Expert_Service_Id,
    '               Expert_Service.Description
    ' Author:       SK
    ' Date:         06/07/2000
    ' ***************************************************************** '
    Public Function SelRiskTypeExpSer(ByRef rvResultArray(,) As Object, ByVal v_lrsk_type_id As Integer) As Integer

        Dim result As Integer = 0
        'Developer Guide no. 21
        Dim oFields As DataRow
        Dim vResultArray(,) As Object
        Dim lRecordCount As Integer

        Try

            result = True

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Insurance Cnt parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="rsk_type_id", vValue:=CStr(v_lrsk_type_id), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelRiskTypeExpSer")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskTypeExpSerSQL, sSQLName:=ACGetRiskTypeExpSerName, bStoredProcedure:=ACGetRiskTypeExpSerStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?
            If lRecordCount < 1 Then
                'No record found, therefore we just exit the function
                'cos we will not need to populate the RskType Exp Ser List box
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            'array will have 3 columns, indefinate no. of rows
            ReDim vResultArray(2, 0)
            For nRecCount As Integer = 1 To lRecordCount

                'ReDim the arrasy since we are changing its dimensions
                '& preserve it since we need tosave the values of the array
                ReDim Preserve vResultArray(2, nRecCount - 1)

                'developer Guide no. 111
                oFields = m_oDatabase.Records.Item(nRecCount - 1).Fields()


                vResultArray(0, nRecCount - 1) = gPMFunctions.NullToLong(oFields("Risk_type_Expert_service_id"))

                vResultArray(1, nRecCount - 1) = gPMFunctions.NullToLong(oFields("Expert_Service_Id"))

                vResultArray(2, nRecCount - 1) = gPMFunctions.NullToString(oFields("Description"))

            Next


            rvResultArray = vResultArray


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelRiskTypeExpSer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelRiskTypeExpSer", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name:         SelExpSer (Public)
    ' Description:  Selects all the Expert Services EXCLUDING the
    '               ones in Risk Type Expert Services
    ' Order of items
    ' in array:     Expert_Service_Id,
    '               Description
    ' Author:       SK
    ' Date:         06/07/2000
    ' ***************************************************************** '
    Public Function SelExpSer(ByRef rvResultArray(,) As Object, ByVal v_lrsk_type_id As Integer) As Integer

        Dim result As Integer = 0
        'Developer Guide no. 21
        Dim oFields As DataRow
        Dim vResultArray(,) As Object
        Dim lRecordCount As Integer

        Try

            result = True

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Insurance Cnt parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="rsk_type_id", vValue:=CStr(v_lrsk_type_id), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelExpSer")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetExpSerSQL, sSQLName:=ACGetExpSerName, bStoredProcedure:=ACGetExpSerStored)

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

            'array will have 3 columns, indefinate no. of rows
            ReDim vResultArray(1, 0)
            For nRecCount As Integer = 1 To lRecordCount

                'ReDim the arrasy since we are changing its dimensions
                '& preserve it since we need tosave the values of the array
                ReDim Preserve vResultArray(1, nRecCount - 1)

                'Developer Guide no. 111
                oFields = m_oDatabase.Records.Item(nRecCount - 1).Fields()


                vResultArray(0, nRecCount - 1) = gPMFunctions.NullToLong(oFields("Expert_Service_Id"))

                vResultArray(1, nRecCount - 1) = gPMFunctions.NullToString(oFields("Description"))

            Next


            rvResultArray = vResultArray


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelExpSer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelExpSer", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    '##ModelId=39629EDC0094
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
                m_oCLMRiskTypeInfoChecklists = Nothing
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
    '##ModelId=39629EDC0095
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Information.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         EditAdd (Public)
    ' Description:  Adds the supplied CLMRTInfoChklst into the Collection.
    '               After the Add, lKey should be equal to the number
    '               of items in the collection.
    ' Arguments:    bCboChange-True:if being called when the
    '                           combo value changes
    ' Author:       SK
    ' Date:         06/07/2000
    ' ***************************************************************** '
    Public Function EditAdd(ByVal lRow As Integer, Optional ByRef bLoad As Boolean = False, Optional ByRef bCboChange As Boolean = False, Optional ByRef vRisk_type_Exp_ser_id As Object = Nothing, Optional ByRef vExp_ser_id As Object = Nothing, Optional ByRef vRisk_type_id As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oCLMRiskTypeInfoChecklist As bCLMRTInfoChkLst.CLMRTInfoChklst

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the
            'same as the Interface Form - 1 (because we havent added this one yet.)

            'The Count(ie. no. of items in the collection) of the collection
            'has always to be 1 less than the Row no. being added
            If m_oCLMRiskTypeInfoChecklists.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new CLMRTInfoChklst (SINGLE)
            oCLMRiskTypeInfoChecklist = New bCLMRTInfoChkLst.CLMRTInfoChklst()
            'Developer Guide no. 9
            m_lReturn = oCLMRiskTypeInfoChecklist.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

            ' Assign Properties to the SINGLE class(CLMRTInfoChklst)


            'developer guide no.98
            m_lReturn = CType(oCLMRiskTypeInfoChecklist.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vRisk_type_Exp_ser_id:=vRisk_type_Exp_ser_id, vExp_ser_id:=vExp_ser_id, vRisk_type_id:=vRisk_type_id), gPMConstants.PMEReturnCode)

            If bLoad Then
                oCLMRiskTypeInfoChecklist.DatabaseStatus = gPMConstants.PMEComponentAction.PMEdit
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oCLMRiskTypeInfoChecklist = Nothing
                Return result
            End If

            ' Add CLMRTInfoChklst(SINGLE) to collection
            m_lReturn = CType(m_oCLMRiskTypeInfoChecklists.Add(oNewCLMRiskTypeInfoChecklist:=oCLMRiskTypeInfoChecklist), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oCLMRiskTypeInfoChecklist = Nothing
                Return result
            End If

            oCLMRiskTypeInfoChecklist = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the CLMRTInfoChklst

    '             specified and updates the CLMRTInfoChklst with the new
    'values.
    '
    ' ***************************************************************** '
    '##ModelId=39629EDC00E4
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vExp_ser_id As Object = Nothing, Optional ByRef vRisk_type_id As Object = Nothing, Optional ByRef vRecoveryTypeID As Object = Nothing, Optional ByRef vIntialReserve As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vPerilID As Object = Nothing) As gPMConstants.PMEReturnCode
        ''
        ''
        ''Dim oCLMRiskTypeInfoChecklist As bCLMRTInfoChkLst.CLMRTInfoChklst
        ''Dim iStatus As Integer
        ''
        ''    On Error GoTo Err_EditUpdate
        ''
        ''    EditUpdate = PMTrue
        ''
        ''    'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
        ''    If (lRow& < 1) Or (lRow& > m_oCLMRiskTypeInfoChecklists.Count) Then
        ''        EditUpdate = PMFalse
        ''        Exit Function
        ''    End If
        ''
        ''    ' Get a reference to the row to Edit
        ''    Set oCLMRiskTypeInfoChecklist = m_oCLMRiskTypeInfoChecklists.Item(lRow)
        ''
        ''    ' Check the Status of the CLMRTInfoChklst
        ''
        ''    'If status is Add (i.e. It is not in the database),then leave status as Add
        ''    'or If status is Delete, report an error
        ''    'Otherwise set to Edit
        ''
        ''    Select Case oCLMRiskTypeInfoChecklist.DatabaseStatus
        ''        Case PMAdd
        ''            ' Leave Status as Add
        ''            iStatus% = PMAdd
        ''        Case PMDelete, PMDummyDelete
        ''            ' Error
        ''            EditUpdate = PMFalse
        ''        Case Else
        ''            ' Set Edit (Update) Status
        ''            iStatus% = PMEdit
        ''    End Select
        ''
        ''    ' Update CLMRTInfoChklst Attributes
        ''    m_lReturn& = oCLMRiskTypeInfoChecklist.SetProperties(iStatus:=iStatus%, vExp_ser_id:=vRecoveryCnt, vRisk_type_id:=vReserveID, vRecoveryTypeID:=vRecoveryTypeID, vIntialReserve:=vIntialReserve, vCurrencyID:=vCurrencyID, vPerilID:=vPerilID)
        ''
        ''    If (m_lReturn& <> PMTrue) Then
        ''        EditUpdate = m_lReturn&
        ''        Set oCLMRiskTypeInfoChecklist = Nothing
        ''        Exit Function
        ''    End If
        ''
        ''    ' Release reference to CLMRTInfoChklst
        ''    Set oCLMRiskTypeInfoChecklist = Nothing

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Return result




        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function
    ' ***************************************************************** '
    ' Name:         EditDelete (Public)
    ' Description:  Validate that the specified CLMRTInfoChklst can be
    '               deleted and mark accordingly.
    ' Author:       SK
    ' Date:         06/07/2000
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oCLMRiskTypeInfoChecklist As bCLMRTInfoChkLst.CLMRTInfoChklst

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid.
            '(i.e. Within the bounds of the Collection)
            'check if the item trying to be deleted from the collection has a
            'count > 0 OR <= Collection count
            If (lRow < 1) Or (lRow > m_oCLMRiskTypeInfoChecklists.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get a reference to the row to be deleted
            'Assign individual item of COLLECTION to SINGLE
            oCLMRiskTypeInfoChecklist = m_oCLMRiskTypeInfoChecklists.Item(lRow)

            ' Check the Status of the CLMRTInfoChklst(SINGLE)

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oCLMRiskTypeInfoChecklist.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oCLMRiskTypeInfoChecklist.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                '(if it is in database)
                oCLMRiskTypeInfoChecklist.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            ' Release reference to CLMRTInfoChklst
            oCLMRiskTypeInfoChecklist = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditDelete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    '##ModelId=39629EDC00F0
    Public Function Cancel() As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Loop round Collection
            For lSub As Integer = 1 To m_oCLMRiskTypeInfoChecklists.Count()
                Select Case m_oCLMRiskTypeInfoChecklists.Item(lSub).DatabaseStatus
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Cancel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         Update (Public)
    ' Description:  Loops round the collection, doing any required
    '               Adds, Deletes or Updates.
    ' Author:       SK
    ' Date:         06/07/2000
    ' ***************************************************************** '
    Public Function Update() As Integer

        Dim result As Integer = 0
        Dim lSub As Integer
        Dim oCLMRiskTypeInfoChecklist As bCLMRTInfoChkLst.CLMRTInfoChklst
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oCLMRiskTypeInfoChecklists.Count()
                'Assign individual item of COLLECTION to SINGLE
                oCLMRiskTypeInfoChecklist = m_oCLMRiskTypeInfoChecklists.Item(lSub)


                Select Case oCLMRiskTypeInfoChecklist.DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete, gPMConstants.PMEComponentAction.PMEdit
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
                        m_lReturn = CType(oCLMRiskTypeInfoChecklist.AddItem(), gPMConstants.PMEReturnCode)
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
                        m_lReturn = CType(oCLMRiskTypeInfoChecklist.DeleteItem(), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Retain the Primary Key of the CLMRTInfoChklst
            With oCLMRiskTypeInfoChecklist
                Risk_type_Exp_ser_id = .Risk_type_Exp_ser_id
            End With

            ' Release last reference
            oCLMRiskTypeInfoChecklist = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Set the database status of all the items in the collection
                    'to PMView
                    ' Refresh the Collection Items D/B Status
                    lSub = 1

                    ' For each item in the collection
                    Do While lSub <= m_oCLMRiskTypeInfoChecklists.Count()

                        ' With the item
                        With m_oCLMRiskTypeInfoChecklists.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove the current SINGLE from Collection
                                    m_oCLMRiskTypeInfoChecklists.Delete(lSub)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    '##ModelId=39629EDC0102
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         CommitTrans (Private)
    ' Description:  Commits a Transaction (Saves changes to DB).
    ' Author:       SK
    ' Date:         06/07/2000
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    '##ModelId=39629EDC010D
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory (Private)
    '
    ' Description: Check Mandatory parameters have been passed.
    '
    ' ***************************************************************** '

    'Private Function CheckMandatory(Optional ByRef vExp_ser_id As Object = Nothing, Optional ByRef vRisk_type_id As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vPerilID As Object = Nothing, Optional ByRef vRecoveryTypeID As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vIntialReserve As Object = Nothing, Optional ByRef vNumber As Object = Nothing, Optional ByRef vExtension As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vModifiedByID As Object = Nothing, Optional ByRef vLastModified As Object = Nothing) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' {* USER DEFINED CODE (Begin) *}
    '


    'If (Information.IsNothing(vRisk_type_id)) Or (Object.Equals(vRisk_type_id, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '


    'If (Information.IsNothing(vSourceID)) Or (Object.Equals(vSourceID, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '


    'If (Information.IsNothing(vPerilID)) Or (Object.Equals(vPerilID, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '


    'If (Information.IsNothing(vRecoveryTypeID)) Or (Object.Equals(vRecoveryTypeID, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '


    'If (Information.IsNothing(vNumber)) Or (Object.Equals(vNumber, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '


    'If (Information.IsNothing(vCreatedByID)) Or (Object.Equals(vCreatedByID, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '


    'If (Information.IsNothing(vDateCreated)) Or (Object.Equals(vDateCreated, Nothing)) Then
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
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckMandatory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    ' PRIVATE Methods (End)


    '##ModelId=39629EDC0128
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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


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
        Dim vTabArray(3, 0) As Object
        Dim dtEffectiveDate As Date
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array
            'vResultArray = ""
            ' Reset Table Array

            vTableArray = Nothing

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = "Risk_Type"



            ''    vTabArray(PMLookupTableName, 1) = "Coinsurance_Treatment"


            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""

            ''    vTabArray(PMLookupKey, 1) = ""


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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name:         SelectRiskCode (Public)
    ' Description:  Selects all the Risk Code records in the
    '               Risk_Code Table
    ' SP:           spu_Rsk_code_sel
    ' Author:       SK
    ' Date:         06/07/2000
    ' ***************************************************************** '
    Public Function SelectRiskCode(ByRef rvResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        'Developer Guide no. 21
        Dim oFields As DataRow
        Dim vResultArray(,) As Object
        Dim lRecordCount As Integer

        Try

            result = True

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskCodeSQL, sSQLName:=ACGetRiskCodeName, bStoredProcedure:=ACGetRiskCodeStored)

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

            'array will have 3 columns, indefinate no. of rows
            ReDim vResultArray(2, 0)
            For nRecCount As Integer = 1 To lRecordCount

                'ReDim the arrasy since we are changing its dimensions
                '& preserve it since we need tosave the values of the array
                ReDim Preserve vResultArray(2, nRecCount - 1)

                'Developer Guide no. 111
                oFields = m_oDatabase.Records.Item(nRecCount - 1).Fields()


                vResultArray(0, nRecCount - 1) = gPMFunctions.NullToLong(oFields("risk_Code_id"))

                vResultArray(1, nRecCount - 1) = gPMFunctions.NullToString(oFields("code"))

                vResultArray(2, nRecCount - 1) = gPMFunctions.NullToString(oFields("description"))

            Next


            rvResultArray = vResultArray


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectRiskCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectRiskCode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         UpdateRiskTypeInfoChkLst (Public)
    ' Description:  To update the Risk_Code Table
    ' SP:           Spu_Info_Checklist_upd
    ' Author:       Puneet Kukreti
    ' Date:         07/08/2006
    ' ***************************************************************** '
    Public Function UpdateRiskTypeInfoChkLst(ByVal v_lrsk_type_id As Integer, ByVal v_IrskTypeInfoChkLst As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Insurance Cnt parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=CStr(v_lrsk_type_id), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskTypeInfoChkLst")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="show_info_checklist", vValue:=CStr(v_IrskTypeInfoChkLst), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskTypeInfoChkLst")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACUpdRskTypeInfoChkLstSQL, sSQLName:="ACRiskTypeChecklist", bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRiskTypeInfoChkLst function Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskTypeInfoCheckList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function
End Class
