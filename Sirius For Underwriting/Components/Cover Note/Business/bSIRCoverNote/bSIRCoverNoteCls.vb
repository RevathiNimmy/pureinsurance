Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
Imports System.Text
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness


    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

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
    Private m_lPartyCnt As Integer

    Private m_sUnderwritingOrAgency As String = ""

    Private m_oLookup As bPMLookup.Business


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


    Public Property PartyCnt() As Integer
        Get

            Return m_lPartyCnt

        End Get
        Set(ByVal Value As Integer)

            m_lPartyCnt = Value

        End Set
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

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserId As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise


        Dim result As Integer = 0
        Const kMethodName As String = "Initialise"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserId
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel



            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to initialize database services.", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Set Username and Password

            ' Set the ProcessMode etc.
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create PM Lookup Business Object
            m_oLookup = New bPMLookup.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserId:=iUserId, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to initialize bPMLookup.Business", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this object.
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
        Const kMethodName As String = "SetProcessModes"
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

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: AddCoverNoteBook
    ' Description:
    ' ***************************************************************** '
    'Developer Guide No. 101
    Public Function AddCoverNoteBook(ByRef r_lCoverNoteBookID As Integer, ByVal sBookNumber As String, ByVal lStart_Number As Integer, ByVal lEnd_Number As Integer, ByVal dtEffective_Date As Date, ByVal lAgent_Cnt As Integer, ByVal lSource_Id As Integer, ByVal lCover_Note_Book_Status_Id As Object) As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "AddCoverNoteBook"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="new_book_id", vValue:=CStr(r_lCoverNoteBookID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="book_number", vValue:=sBookNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="start_number", vValue:=CStr(lStart_Number), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="end_number", vValue:=CStr(lEnd_Number), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=dtEffective_Date, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If lAgent_Cnt > 0 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="agent_cnt", vValue:=CStr(lAgent_Cnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else

                'developer guide no. 85
                m_lReturn = m_oDatabase.Parameters.Add(sName:="agent_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If


            If lSource_Id > 0 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=CStr(lSource_Id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else

                'developer guide no. 85
                m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="cover_note_book_status_id", vValue:=CStr(lCover_Note_Book_Status_Id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddCoverNoteBookSQL, sSQLName:=ACAddCoverNoteBookName, bStoredProcedure:=ACAddCoverNoteBookStored)


            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or Convert.IsDBNull(m_oDatabase.Parameters.Item("new_book_id").Value) Or IsNothing(m_oDatabase.Parameters.Item("new_book_id").Value) Or gPMFunctions.ToSafeLong(m_oDatabase.Parameters.Item("new_book_id").Value) = 0 Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add new Cover Note Book", gPMConstants.PMELogLevel.PMLogError)
            End If

            r_lCoverNoteBookID = m_oDatabase.Parameters.Item("new_book_id").Value

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: EditCoverNoteBook
    ' Description:
    ' Update Cover Note Book
    ' ***************************************************************** '
    'Developer Guide No. 101
    Public Function EditCoverNoteBook(ByRef lCoverNoteBookID As Integer, ByVal dtEffective_Date As Date, ByVal lAgent_Cnt As Integer, ByVal lSource_Id As Integer, ByVal lCover_Note_Book_Status_Id As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "EditCoverNoteBook"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="cover_note_book_id", vValue:=CStr(lCoverNoteBookID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=dtEffective_Date, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If lAgent_Cnt > 0 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="agent_cnt", vValue:=CStr(lAgent_Cnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else

                'developer guide no. 85
                m_lReturn = m_oDatabase.Parameters.Add(sName:="agent_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If lSource_Id > 0 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=CStr(lSource_Id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else

                'developer guide no. 85
                m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="cover_note_book_status_id", vValue:=CStr(lCover_Note_Book_Status_Id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdCoverNoteBookSQL, sSQLName:=ACUpdCoverNoteBookName, bStoredProcedure:=ACUpdCoverNoteBookStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to update Cover Note Book", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: DeleteCoverNoteBook
    ' Description:
    ' Not Functional
    ' ***************************************************************** '
    Public Function DeleteCoverNoteBook() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DeleteCoverNoteBook"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            'm_oDatabase.Parameters.Clear


            'If (m_lReturn <> PMTrue) Then
            '   RaiseError kMethodName, "Failed to delete Cover Note Book", PMLogError
            'End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SelectCoverNoteBook
    ' Description:
    ' ***************************************************************** '
    Public Function SelectCoverNoteBook(ByVal lCoverNoteBookID As Object, ByRef r_sBookNumber As Object, ByRef r_lStart_Number As Object, ByRef r_lEnd_Number As Object, ByRef r_dtEffective_Date As Object, ByRef r_lAgent_Cnt As Integer, ByRef r_lAgent_Name As String, ByRef r_lSource_Id As Object, ByRef r_lCover_Note_Book_Status_Id As Object, ByRef r_dtCreated_Date As Object, ByRef r_dtLastUpdated As Object) As Integer

        Dim result As Integer = 0
        Dim vResult(,) As Object

        Const kMethodName As String = "SelectCoverNoteBook"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="cover_note_book_id", vValue:=CStr(lCoverNoteBookID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelCoverNoteBookSQL, sSQLName:=ACSelCoverNoteBookName, bStoredProcedure:=ACSelCoverNoteBookStored, vResultArray:=vResult)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to select Cover Note Book", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Information.IsArray(vResult) Then


                r_sBookNumber = vResult(ACBookNumber, 0)


                r_lStart_Number = vResult(ACStartNumber, 0)


                r_lEnd_Number = vResult(ACEndNumber, 0)


                r_dtEffective_Date = vResult(ACEffectiveDate, 0)
                r_lAgent_Cnt = gPMFunctions.ToSafeLong(vResult(ACAgentId, 0))
                r_lAgent_Name = gPMFunctions.ToSafeString(vResult(ACAgentName, 0))


                r_lSource_Id = vResult(ACBranch, 0)


                r_lCover_Note_Book_Status_Id = vResult(ACBookStatus, 0)


                r_dtCreated_Date = vResult(ACCreatedDate, 0)


                r_dtLastUpdated = vResult(ACDateUpdated, 0)
            Else
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: FindCoverNoteBook
    ' Description:
    ' ***************************************************************** '
    'Developer Guide No.  17
    Public Function FindCoverNoteBook(ByRef r_vResultArray(,) As Object, ByVal sBookNumber As Object, ByVal lStart_Number As Object, ByVal lEnd_Number As Object, ByVal lAgent_Cnt As Object, ByVal dtLast_Updated As Object, ByVal lSource_Id As Object, ByVal lCover_Note_Book_Status_Id As Object, ByVal sPolicy_ref As Object, ByVal dtAssigned_Date As Object, Optional ByVal iUserId As Object = Nothing) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "FindCoverNoteBook"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            'Reset result array
            r_vResultArray = Nothing

            ' Execute SQL Statement
            With m_oDatabase

                .Parameters.Clear()

                'Developer Guide No. 85 ' NIIT CHANGED IN FOLLOWING LINES - FOR CHANGING THE CSTR IN CASE OF DBNULL IN VARIABLE
                If sBookNumber Is Nothing Then
                    m_lReturn = .Parameters.Add(sName:="book_number", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                Else
                    m_lReturn = .Parameters.Add(sName:="book_number", vValue:=(sBookNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                End If
                m_lReturn = .Parameters.Add(sName:="start_number", vValue:=(lStart_Number), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="end_number", vValue:=(lEnd_Number), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="agent_cnt", vValue:=(lAgent_Cnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'NIIT DONE
                m_lReturn = .Parameters.Add(sName:="last_updated", vValue:=(dtLast_Updated), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                m_lReturn = .Parameters.Add(sName:="source_id", vValue:=(lSource_Id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="cover_note_book_status_id", vValue:=(lCover_Note_Book_Status_Id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'NIIT DONE
                If sPolicy_ref Is Nothing Then
                    m_lReturn = .Parameters.Add(sName:="policy_number", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                Else
                    m_lReturn = .Parameters.Add(sName:="policy_number", vValue:=(sPolicy_ref), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                End If

                m_lReturn = .Parameters.Add(sName:="assigned_date", vValue:=(dtAssigned_Date), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                If Not (Information.IsNothing(iUserId) > 0) Then

                    m_lReturn = .Parameters.Add(sName:="user_id", vValue:=(iUserId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                End If
                m_lReturn = .SQLSelect(sSQL:=ACFindCoverNoteBookSQL, sSQLName:=ACFindCoverNoteBookName, bStoredProcedure:=ACFindCoverNoteBookStored, vResultArray:=r_vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "FindCoverNoteBook failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' If NO records were found return PMFalse
                If Not Information.IsArray(r_vResultArray) Then
                    result = gPMConstants.PMEReturnCode.PMNotFound
                End If

            End With

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: AddCoverNoteSheet
    ' Description:
    ' ***************************************************************** '
    'Developer Guide No. 101
    Public Function AddCoverNoteSheet(ByRef r_lCoverNoteSheetID As Integer, ByVal lBook_Id As Integer, ByVal lSheet_Number As Integer, ByVal sComments As String, Optional ByVal lCover_Note_Book_Status_Id As Object = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddCoverNoteSheet"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="new_sheet_id", vValue:=CStr(r_lCoverNoteSheetID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="book_id", vValue:=CStr(lBook_Id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="sheet_number", vValue:=CStr(lSheet_Number), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="comments", vValue:=sComments, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If Not False Then
                If lCover_Note_Book_Status_Id > 0 Then
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="cover_note_sheet_status_id", vValue:=CStr(lCover_Note_Book_Status_Id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                Else

                    'developer guide no. 85
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="cover_note_sheet_status_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                End If
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddCoverNoteSheetSQL, sSQLName:=ACAddCoverNoteSheetName, bStoredProcedure:=ACAddCoverNoteSheetStored)


            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or Convert.IsDBNull(m_oDatabase.Parameters.Item("new_sheet_id").Value) Or IsNothing(m_oDatabase.Parameters.Item("new_sheet_id").Value) Or gPMFunctions.ToSafeLong(m_oDatabase.Parameters.Item("new_sheet_id").Value) = 0 Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add new sheet", gPMConstants.PMELogLevel.PMLogError)
            End If

            r_lCoverNoteSheetID = m_oDatabase.Parameters.Item("new_sheet_id").Value

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: EditCoverNoteSheet
    ' Description:
    ' ***************************************************************** '
    Public Function EditCoverNoteSheet(ByVal lCoverNoteBookID As Integer, ByVal lNewCoverNoteSheetNumber As Integer, ByVal lOldCoverNoteSheetNumber As Integer, ByVal lCoverNoteSheetStatusID As Integer, ByVal lInsurance_file_cnt As Integer, ByVal dtAssignedDate As Object, ByVal sComments As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "EditCoverNoteSheet"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="cover_note_book_id", vValue:=CStr(lCoverNoteBookID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="old_cover_note_sheet_number", vValue:=CStr(lOldCoverNoteSheetNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="new_cover_note_sheet_number", vValue:=CStr(lNewCoverNoteSheetNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="cover_note_sheet_status_id", vValue:=CStr(lCoverNoteSheetStatusID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If lInsurance_file_cnt = 0 Then

                'developer guide no. 85
                m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(lInsurance_file_cnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="assigned_date", vValue:=CStr(dtAssignedDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="comments", vValue:=sComments, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdCoverNoteSheetSQL, sSQLName:=ACUpdCoverNoteSheetName, bStoredProcedure:=ACUpdCoverNoteSheetStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to update cover note sheet", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: DeleteCoverNoteSheet
    ' Description:
    ' ***************************************************************** '
    Public Function DeleteCoverNoteSheet(ByVal lSheet_Id As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DeleteCoverNoteSheet"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="sheet_id", vValue:=CStr(lSheet_Id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDelCoverNoteSheetSQL, sSQLName:=ACDelCoverNoteSheetName, bStoredProcedure:=ACDelCoverNoteSheetStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to delete sheet", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SelectCoverNoteSheet
    ' Description:
    ' ***************************************************************** '
    Public Function SelectCoverNoteSheet(ByVal lCoverNoteBookID As Integer, ByVal lCoverNoteSheetNumber As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SelectCoverNoteSheet"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Execute SQL Statement
            With m_oDatabase

                .Parameters.Clear()
                m_lReturn = .Parameters.Add(sName:="cover_note_book_id", vValue:=CStr(lCoverNoteBookID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                m_lReturn = .Parameters.Add(sName:="cover_note_sheet_number", vValue:=CStr(lCoverNoteSheetNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLSelect(sSQL:=ACSelCoverNoteSheetsSQL, sSQLName:=ACSelCoverNoteSheetsName, bStoredProcedure:=ACSelCoverNoteSheetsStored, vResultArray:=r_vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "SelectCoverNoteSheet Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' If NO records were found return PMNotFound
                'If (IsArray(r_vResultArray) = False) Then
                '    SelectCoverNoteSheet = PMNotFound
                '    GoTo Finally
                'End If

            End With

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetCoverNoteSheets
    ' Description:
    ' ***************************************************************** '
    Public Function GetCoverNoteSheets(ByVal lCoverNoteBookID As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetCoverNoteSheets"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Execute SQL Statement
            With m_oDatabase

                .Parameters.Clear()
                m_lReturn = .Parameters.Add(sName:="cover_note_book_id", vValue:=CStr(lCoverNoteBookID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLSelect(sSQL:=ACGetCoverNoteSheetsSQL, sSQLName:=ACGetCoverNoteSheetsName, bStoredProcedure:=ACGetCoverNoteSheetsStored, vResultArray:=r_vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetCoverNoteSheets Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' If NO records were found return PMNotFound
                If Not Information.IsArray(r_vResultArray) Then
                    result = gPMConstants.PMEReturnCode.PMNotFound
                End If

            End With

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: AssignCoverNoteSheet
    ' Description:
    ' ***************************************************************** '
    Public Function AssignCoverNoteSheet(ByVal lInsuranceFileCnt As Integer, ByVal sCoverSheetReference As String, ByVal lCoverSheetNumber As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AssignCoverNoteSheet"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Execute SQL Statement
            With m_oDatabase

                .Parameters.Clear()
                m_lReturn = .Parameters.Add(sName:="cover_note_book_number", vValue:=sCoverSheetReference, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                m_lReturn = .Parameters.Add(sName:="cover_note_sheet_number", vValue:=CStr(lCoverSheetNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                m_lReturn = .Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLAction(sSQL:=ACAssignCoverNoteSheetSQL, sSQLName:=ACAssignCoverNoteSheetName, bStoredProcedure:=ACAssignCoverNoteSheetStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "AssignCoverNoteSheet Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End With

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ValidateCoverNoteSheet
    ' Description:
    ' ***************************************************************** '
    Public Function ValidateCoverNoteSheet(ByVal lInsuranceFileCnt As Integer, ByVal sCoverNoteBookNumber As String, ByVal lCoverSheetNumber As Integer, ByVal lBranchId As Integer, ByVal lAgentCnt As Integer, ByVal lProductId As Integer, ByRef r_lResult As Integer, ByRef r_sSheetStatus As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ValidateCoverNoteSheet"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Execute SQL Statement
            With m_oDatabase
                .Parameters.Clear()
                m_lReturn = .Parameters.Add(sName:="sCover_Note_Book_Number", vValue:=sCoverNoteBookNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                m_lReturn = .Parameters.Add(sName:="lCover_Note_Sheet_Number", vValue:=CStr(lCoverSheetNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                m_lReturn = .Parameters.Add(sName:="lInsurance_File_Cnt", vValue:=CStr(lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                m_lReturn = .Parameters.Add(sName:="lBranch_Id", vValue:=CStr(lBranchId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                m_lReturn = .Parameters.Add(sName:="lAgent_Cnt", vValue:=CStr(lAgentCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                m_lReturn = .Parameters.Add(sName:="lProduct_Id", vValue:=CStr(lProductId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                m_lReturn = .Parameters.Add(sName:="lReturn_Status", vValue:=CStr(r_lResult), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
                m_lReturn = .Parameters.Add(sName:="sSheet_Status", vValue:=r_sSheetStatus, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)

                m_lReturn = .SQLAction(sSQL:=ACValidateCoverNoteSheetSQL, sSQLName:=ACValidateCoverNoteSheetName, bStoredProcedure:=ACValidateCoverNoteSheetStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "ValidateCoverNoteSheet Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                r_sSheetStatus = gPMFunctions.ToSafeString(.Parameters.Item("sSheet_Status").Value)
                r_lResult = gPMFunctions.ToSafeLong(.Parameters.Item("lReturn_Status").Value)

            End With

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values
    '
    ' ***************************************************************** '
    'developer guide no. 17
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim dtEffectiveDate As Date

        Dim vTabArray(3, 2) As Object

        Const kMethodName As String = "GetLookupValues"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array
            vResultArray = Nothing
            ' Reset Table Array

            vTableArray = Nothing

            ' {* USER DEFINED CODE (Begin) *}

            ' Setup Lookup Table Names

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = gSIRLibrary.SIRLookupCover_Note_Book_Status

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 1) = gSIRLibrary.SIRLookupCover_Note_Sheet_Status

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 2) = gSIRLibrary.SIRLookupSource

            iLookupType = gPMConstants.PMELookupType.PMLookupAll

            ' Do not supply a key

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""

            ' Default Effective Date to current date/time
            dtEffectiveDate = DateTime.Now

            ' Get the Lookup items
            m_lReturn = m_oLookup.GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTabArray, iLanguageID:=iLanguageID, dtEffectiveDate:=dtEffectiveDate, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetLookupValues Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Return the Table Array

            vTableArray = vTabArray

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ClearParameters (Private)
    '
    ' Description: Clears the Database Parameters Collection if there are any.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (ClearParameters) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub ClearParameters()
    '
    'Const kMethodName As String = "ClearParameters"
    'On Error GoTo Catch_Renamed
    '
    '
    ' Clear the Databases Parameters Collection
    'If m_oDatabase.Parameters Is Nothing Then
    ' Do Nothing
    'Else
    'm_oDatabase.Parameters.Clear()
    'End If
    '
    'GoTo Finally_Renamed
    'Catch_Renamed: '
    '
    ' DO Not Call any functions before here or the error will be lost
    'bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn)
    '
    'Finally_Renamed: '
    '
    'Exit Sub
    '
    'End Sub


    ' ***************************************************************** '
    ' Name: UnderwritingOrAgency
    '
    ' Description:  Finds out if Underwriting or Agency business
    '
    ' ***************************************************************** '
    Private Function getUnderwritingOrAgency() As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Const kMethodName As String = "getUnderwritingOrAgency"


        result = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = CType(bPMFunc.getUnderwritingOrAgency(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, r_vUnderwriting:=m_sUnderwritingOrAgency), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "getUnderwritingOrAgency Failed", gPMConstants.PMELogLevel.PMLogError)
        End If



        Return result

    End Function

    Public Sub New()
        MyBase.New()

        Const kMethodName As String = "Class_Initialize"
        Try


            ' Class Initialise

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally



        End Try
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub



    Public Function PickListLoad(ByVal sPickListType As String, ByVal vFKArray(,) As Object, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0

        Const kMethodName As String = "PickListLoad"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If vFKArray.GetUpperBound(1) > 0 And sPickListType.Trim().ToUpper() = "PRODUCTS" Then
                ReDim Preserve vFKArray(vFKArray.GetUpperBound(0), vFKArray.GetUpperBound(1) - 1)
            End If

            With m_oDatabase
                .Parameters.Clear()

                'Load the parameters
                For iParam As Integer = vFKArray.GetLowerBound(1) To vFKArray.GetUpperBound(1)



                    .Parameters.Add(sName:=CStr(vFKArray(0, iParam)), vValue:=CStr(vFKArray(1, iParam)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=CType(CInt(vFKArray(2, iParam)), gPMConstants.PMEDataType))
                Next iParam

                'Call the SP
                Select Case sPickListType.Trim().ToUpper()
                    Case "PRODUCTS"
                        'NIIT DONE
                        'm_lReturn = .SQLSelect("spu_SIR_SelectAll_CoverNoteProducts " & _
                        '            "(" & PickListParams(vFKArray) & ") ", sPickListType & " PickList Load", True, , vResultArray)

                        m_lReturn = .SQLSelect("spu_SIR_SelectAll_CoverNoteProducts", " PickList Load", True, , vResultArray)

                End Select

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "PickListLoad Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End With

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: PickListParams
    ' Description: Returns a string of question marks for the SP definition
    ' ***************************************************************** '
    Private Function PickListParams(ByRef vParams(,) As Object) As String

        Dim result As String = String.Empty
        Const kMethodName As String = "PickListParams"


        Dim sComma As String = ""
        Dim sParam As New StringBuilder

        sComma = ""
        sParam = New StringBuilder("")
        For iParam As Integer = vParams.GetLowerBound(1) To vParams.GetUpperBound(1)
            sParam.Append(sComma & "?")
            sComma = ","
        Next iParam

        result = sParam.ToString()



        Return result

    End Function

    Public Function PickListSave(ByRef sPickListType As String, ByRef vFKArray(,) As Object, ByRef vKeys As Object) As Integer

        Dim result As Integer = 0

        Const kMethodName As String = "PickListSave"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = BeginTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PickListSave Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If vFKArray.GetUpperBound(1) > 1 And sPickListType.Trim().ToUpper() = "PRODUCTS" Then
                ReDim Preserve vFKArray(vFKArray.GetUpperBound(0), vFKArray.GetUpperBound(1) - 1)
            End If

            With m_oDatabase

                'clear the old data
                .Parameters.Clear()

                'Load the FK parameters
                For iParam As Integer = vFKArray.GetLowerBound(1) To vFKArray.GetUpperBound(1)


                    .Parameters.Add(sName:=CStr(vFKArray(0, iParam)), vValue:=CStr(vFKArray(1, iParam)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                Next iParam



                Select Case sPickListType.Trim().ToUpper()
                    Case "PRODUCTS"

                        m_lReturn = .SQLAction("spu_SIR_Delete_CoverNoteProducts", sPickListType & " PickList Delete", True)

                End Select

                'See if there is anything to save
                If Information.IsArray(vKeys) Then

                    For iKey As Integer = vKeys.GetLowerBound(0) To vKeys.GetUpperBound(0)
                        .Parameters.Clear()


                        .Parameters.Add(sName:=CStr(vFKArray(0, 0)), vValue:=CStr(vFKArray(1, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                        'Call the SP
                        Select Case sPickListType.Trim().ToUpper()
                            Case "PRODUCTS"

                                .Parameters.Add("product_id", CStr(vKeys(iKey)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                                m_lReturn = .SQLAction("spu_SIR_Save_CoverNoteProducts", sPickListType & " PickList Load", True)

                        End Select

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "PickListSave Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    Next iKey
                End If
            End With

            m_lReturn = CommitTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PickListSave Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            RollbackTrans()
        Finally



        End Try
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
        Const kMethodName As String = "BeginTrans"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "BeginTrans Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
        End Try
        Return result

    End Function


    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Private Function CommitTrans() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CommitTrans"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "CommitTrans Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
        End Try

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Private Function RollbackTrans() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "RollbackTrans"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLRollbackTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "RollbackTrans Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
        End Try

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetDetails
    ' Description:
    ' ***************************************************************** '
    Public Function GetDetails(ByVal lInsuranceFileCnt As Integer, ByRef r_vResult(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetDetails"
        Try

            Dim i As Integer
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Execute SQL Statement
            With m_oDatabase

                .Parameters.Clear()
                m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLSelect(sSQL:=ACGetCoverNoteSheetByPolSQL, sSQLName:=ACGetCoverNoteSheetByPolName, bStoredProcedure:=ACGetCoverNoteSheetByPolStored, vResultArray:=r_vResult)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End With

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetBranches
    ' Description:
    ' ***************************************************************** '
    Public Function GetBranches(ByVal iUserId As Integer, ByRef r_vResult(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetBranches"
        Try

            Dim i As Integer
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Execute SQL Statement
            With m_oDatabase

                .Parameters.Clear()
                m_lReturn = .Parameters.Add(sName:="UserId", vValue:=CStr(iUserId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLSelect(sSQL:=ACGetBranchesSQL, sSQLName:=ACGetBranchesName, bStoredProcedure:=ACGetBranchesStored, vResultArray:=r_vResult)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetBranches Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End With

        Catch ex As Exception


            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result

    End Function
End Class
