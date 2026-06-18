Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports SharedFiles


<System.Runtime.InteropServices.ProgId("Business_NET.ChaseCycleItem")> _
Public NotInheritable Class ChaseCycleItem
    Implements IDisposable
    ' *****************************************************************
    ' Class Name: Business
    '
    ' Date:01/03/2013
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a Chase Cycle Item.
    '
    ' Edit History:
    ' *****************************************************************


    ' ************************************************
    ' Added to replace global variables 01/03/2013
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
    Private Const ACClass As String = "ChaseCycleItem"

    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

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

    ' PM Lookup Business Component (Private)
    Private m_oLookup As bPMLookup.Business

    'SP to Select a list of Chase Cycle Control Items
    Private Const ACSelAllName As String = "SelAllChaseCycleItem"
    Private Const ACSelAllSQL As String = "spu_SIR_SelAll_Chase_Cycle_Item"

    'SP to Select a single Chase Cycle Item
    Private Const ACSelName As String = "SelectChaseCycleItem"
    Private Const ACSelSQL As String = "spu_SIR_Select_Chase_Cycle_Item"

    'SP to Add a Chase Cycle Item
    Private Const ACDirectAddName As String = "AddChaseCycleItem"
    Private Const ACDirectAddSQL As String = "spu_SIR_Add_Chase_Cycle_Item"

    'SP to Edit a Chase Cycle Item
    Private Const ACDirectEditName As String = "UpdateChaseCycleItem"
    Private Const ACDirectEditSQL As String = "spu_SIR_Update_Chase_Cycle_Item"

    'SP to Delete a Chase Cycle Item
    Private Const ACDirectDeleteName As String = "DeleteChaseCycleItem"
    Private Const ACDirectDeleteSQL As String = "spu_SIR_Delete_Chase_Cycle_Item"


    Private Const kUpdateChaseCycleItemSQL As String = "spu_SIR_Chase_Cycle_Item_Update"
    Private Const kUpdateChaseCycleItemName As String = "spu_SIR_Chase_Cycle_Item_Update"

    'Result Array columns for GetDetails for Chase Cycle Item
    Private Const ACChaseCycleItemID As Integer = 0
    Private Const ACChaseCycleReason As Integer = 1
    Private Const ACInsuranceFolderCnt As Integer = 3
    Private Const ACInsuranceFileCnt As Integer = 2
    Private Const ACCanAutoCancel As Integer = 4
    Private Const ACWillAutoCancel As Integer = 5
    Private Const ACChaseCycleStepID As Integer = 6
    Private Const ACCreatedDate As Integer = 7
    Private Const ACDueDate As Integer = 8
    Private Const ACLetterSent As Integer = 9
    Private Const ACNextStepID As Integer = 10
    Private Const ACPMUserGroupId As Integer = 11
    Private Const ACPMUserId As Integer = 12
    Private Const ACIsDeleted As Integer = 13

    ' Constant for add or edit
    Private Const ACUpdateAdd As Byte = 1
    Private Const ACUpdateEdit As Byte = 2


    ' PRIVATE Data Members (End)


    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
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


    ' PUBLIC Property Procedures (End)

    ' PUBLIC Methods (Begin)

    ' *****************************************************************
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' *****************************************************************
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer

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

            ' Get Reference to Database

            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' *****************************************************************
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' *****************************************************************
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


    ' *****************************************************************
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' *****************************************************************
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

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' *****************************************************************
    ' Name: GetList (Public)
    '
    ' Description: Select multiple Chase Cycle Rule records from
    ' the database.
    '
    ' *****************************************************************
    Public Function GetList(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelAllSQL, sSQLName:=ACSelAllName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception
            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get all Chase Cycle Item records", vApp:=ACApp, vClass:=ACClass, vMethod:="GetList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' *****************************************************************
    ' Name: GetDetails (Public)
    '
    ' Description: Select a single Chase Cycle Item record from the
    ' database.
    '
    ' *****************************************************************
    Public Function GetDetails(ByVal v_lChaseCycleItemId As Integer, Optional ByRef r_vChaseCycleReason As Object = Nothing, Optional ByRef r_vInsuranceFileCnt As Object = Nothing, Optional ByRef r_vInsuranceFolderCnt As Object = Nothing, Optional ByRef r_vCanAutoCancel As Object = Nothing, Optional ByRef r_vWillAutoCancel As Object = Nothing, Optional ByRef r_vChaseCycleStepID As Object = Nothing, Optional ByRef r_vCreatedDate As Object = Nothing, Optional ByRef r_vDueDate As Object = Nothing, Optional ByRef r_vLetterSent As Object = Nothing, Optional ByVal v_vPMUserGroupId As Object = Nothing, Optional ByVal v_vPMUserId As Object = Nothing, Optional ByVal v_vIsDeleted As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object
        Const klFirstRow As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the Chase Cycle Item id INPUT parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Chase_Cycle_item_id", vValue:=v_lChaseCycleItemId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelSQL, sSQLName:=ACSelName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Populate the params

            r_vChaseCycleReason = vResultArray(ACChaseCycleReason, klFirstRow)

            r_vInsuranceFileCnt = vResultArray(ACInsuranceFileCnt, klFirstRow)
            r_vCanAutoCancel = vResultArray(ACCanAutoCancel, klFirstRow)
            r_vWillAutoCancel = vResultArray(ACWillAutoCancel, klFirstRow)
            r_vChaseCycleStepID = vResultArray(ACChaseCycleStepID, klFirstRow)
            r_vCreatedDate = vResultArray(ACCreatedDate, klFirstRow)
            r_vDueDate = vResultArray(ACDueDate, klFirstRow)
            r_vLetterSent = vResultArray(ACLetterSent, klFirstRow)

            v_vPMUserGroupId = vResultArray(ACPMUserGroupId, klFirstRow)

            v_vPMUserId = vResultArray(ACPMUserId, klFirstRow)
            v_vIsDeleted = vResultArray(ACIsDeleted, klFirstRow)


            Return result

        Catch excep As System.Exception

            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get the Chase Cycle Item", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' *****************************************************************
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a Chase Cycle Item record to the database
    '
    ' *****************************************************************
    Public Function DirectAdd(ByRef r_vChaseCycleItemID As Integer, ByVal v_vChaseCycleReason As Object, ByVal v_vInsuranceFolderCnt As Integer, ByVal v_vInsuranceFileCnt As Integer, ByVal v_vCanAutoCancel As Object, ByVal v_vWillAutoCancel As Object, ByVal v_vChaseCycleStepID As Object, ByVal v_vCreatedDate As Object, ByVal v_vDueDate As Object, ByVal v_vLetterSent As Object, ByVal v_vPMUserGroupId As Object, ByVal v_vPMUserId As Object, ByVal v_vIsDeleted As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Call the update method
            m_lReturn = CType(DirectUpdate(r_vChaseCycleItemID:=r_vChaseCycleItemID, v_vChaseCycleReason:=v_vChaseCycleReason, v_vInsuranceFolderCnt:=v_vInsuranceFolderCnt, v_vInsuranceFileCnt:=v_vInsuranceFileCnt, v_vCanAutoCancel:=v_vCanAutoCancel, v_vWillAutoCancel:=v_vWillAutoCancel, v_vChaseCycleStepID:=v_vChaseCycleStepID, v_vCreatedDate:=v_vCreatedDate, v_vDueDate:=v_vDueDate, v_vLetterSent:=v_vLetterSent, v_bAction:=ACUpdateAdd, v_vPMUserGroupId:=v_vPMUserGroupId, v_vPMUserId:=v_vPMUserId, v_vIsDeleted:=v_vIsDeleted), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' *****************************************************************
    ' Name: DirectUpdate (Private)
    '
    ' Description: Adds or edits a Chase Cycle Item record
    '
    ' *****************************************************************
    Private Function DirectUpdate(ByRef r_vChaseCycleItemID As Integer, ByVal v_vChaseCycleReason As Object, ByVal v_vInsuranceFolderCnt As Integer, ByVal v_vInsuranceFileCnt As Integer, ByVal v_vCanAutoCancel As Object, ByVal v_vWillAutoCancel As Object, ByVal v_vChaseCycleStepID As Object, ByVal v_vCreatedDate As Object, ByVal v_vDueDate As Object, ByVal v_vLetterSent As Object, ByVal v_bAction As Byte, ByVal v_vPMUserGroupId As Object, ByVal v_vPMUserId As Object, ByVal v_vIsDeleted As Object) As Integer


        Dim result As Integer = 0
        Dim lRecordsAffected As Integer


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add Chase Cycle Step id

        If v_bAction = ACUpdateAdd Then
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Chase_Cycle_item_id", vValue:=CInt(r_vChaseCycleItemID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
        Else

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Chase_Cycle_item_id", vValue:=CInt(r_vChaseCycleItemID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add Chase Cycle reason as an input param for an insert

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Chase_Cycle_reason", vValue:=v_vChaseCycleReason, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add insurance file count as an input param for an insert

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=v_vInsuranceFolderCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add insurance file count as an input param for an insert

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_vInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add can auto cancel as an input param for an insert

        m_lReturn = m_oDatabase.Parameters.Add(sName:="can_auto_cancel", vValue:=v_vCanAutoCancel, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add will auto cancel as an input param for an insert

        m_lReturn = m_oDatabase.Parameters.Add(sName:="will_auto_cancel", vValue:=v_vWillAutoCancel, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add Chase Cycle step id as an input param for an insert

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Chase_Cycle_step_id", vValue:=v_vChaseCycleStepID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add created date as an input param for an insert

        m_lReturn = m_oDatabase.Parameters.Add(sName:="created_date", vValue:=v_vCreatedDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add Due date as an input param for an insert

        m_lReturn = m_oDatabase.Parameters.Add(sName:="due_date", vValue:=v_vDueDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add letter sent as an input param for an insert

        m_lReturn = m_oDatabase.Parameters.Add(sName:="letter_sent", vValue:=v_vLetterSent, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="pmuser_group_id", vValue:=v_vPMUserGroupId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="pmuser_id", vValue:=v_vPMUserId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="is_deleted", vValue:=v_vIsDeleted, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        ' Execute SQL Statement
        If v_bAction = ACUpdateAdd Then
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDirectAddSQL, sSQLName:=ACDirectAddName, bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)
        Else
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDirectEditSQL, sSQLName:=ACDirectEditName, bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' If record was added, retrieve the ID
        If lRecordsAffected > 0 Then
            If v_bAction = ACUpdateAdd Then

                r_vChaseCycleItemID = m_oDatabase.Parameters.Item("Chase_Cycle_item_id").Value
            End If
        Else
            ' Nothing affected, so set to error
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function
    ' *****************************************************************
    ' Name: DirectEdit (Public)
    '
    ' Description: Edits a Chase Cycle Item record in the database
    '
    ' *****************************************************************
    Public Function DirectEdit(ByVal v_vChaseCycleItemID As Integer, ByVal v_vChaseCycleReason As Object, ByVal v_vInsuranceFolderCnt As Integer, ByVal v_vInsuranceFileCnt As Integer, ByVal v_vCanAutoCancel As Object, ByVal v_vWillAutoCancel As Object, ByVal v_vChaseCycleStepID As Object, ByVal v_vCreatedDate As Object, ByVal v_vDueDate As Object, ByVal v_vLetterSent As Object, ByVal v_vPMUserGroupId As Object, ByVal v_vPMUserId As Object, ByVal v_vIsDeleted As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Call the update method
            m_lReturn = CType(DirectUpdate(r_vChaseCycleItemID:=v_vChaseCycleItemID, v_vChaseCycleReason:=v_vChaseCycleReason, v_vInsuranceFolderCnt:=v_vInsuranceFolderCnt, v_vInsuranceFileCnt:=v_vInsuranceFileCnt, v_vCanAutoCancel:=v_vCanAutoCancel, v_vWillAutoCancel:=v_vWillAutoCancel, v_vChaseCycleStepID:=v_vChaseCycleStepID, v_vCreatedDate:=v_vCreatedDate, v_vDueDate:=v_vDueDate, v_vLetterSent:=v_vLetterSent, v_bAction:=ACUpdateEdit, v_vPMUserGroupId:=v_vPMUserGroupId, v_vPMUserId:=v_vPMUserId, v_vIsDeleted:=v_vIsDeleted), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception
            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectEdit Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectEdit", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' *****************************************************************
    ' Name: DirectDelete (Public)
    '
    ' Description: Deletes a Chase Cycle Item record
    '
    ' *****************************************************************
    Public Function DirectDelete(ByVal v_lChaseCycleItemId As Integer, Optional ByVal v_bDeletePermanent As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add Chase Cycle step id as an input param
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Chase_Cycle_item_id", vValue:=v_lChaseCycleItemId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add Chase Cycle step id as an input param
            m_lReturn = m_oDatabase.Parameters.Add(sName:="bDelete_Permanent", vValue:=v_bDeletePermanent, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDirectDeleteSQL, sSQLName:=ACDirectDeleteName, bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name: AddInputParameter
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' ***************************************************************** '
    Public Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Integer, ByVal v_iType As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddInputParameter"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add Parameter to database object
            lReturn = m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=v_vValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, " Failed to add parameter name:" & v_sName & _
                                        ", values :" & CStr(v_vValue) & ", type:" & CStr(v_iType), gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result

        Catch excep As System.Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=excep)

            ' If you want to rollback a transaction or something, do it here
            Return result
        End Try

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UpdateChaseCycleItem
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created :  : 01/03/2013 
    ' ***************************************************************** '
    Private Function UpdateChaseCycleItem(ByVal v_lChaseCycleItemId As Integer, ByVal v_sChaseCycleReason As String, ByVal v_lChaseCycleStepId As Integer, ByVal v_dtDueDate As Date) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateChaseCycleItem"

        Dim lReturn As gPMConstants.PMEReturnCode



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        m_lReturn = CType(AddInputParameter(v_sName:="Chase_Cycle_item_id", v_vValue:=v_lChaseCycleItemId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
        m_lReturn = CType(AddInputParameter(v_sName:="Chase_Cycle_reason", v_vValue:=CInt(v_sChaseCycleReason), v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
        m_lReturn = CType(AddInputParameter(v_sName:="Chase_Cycle_step_id", v_vValue:=v_lChaseCycleStepId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
        m_lReturn = CType(AddInputParameter(v_sName:="due_date", v_vValue:=CInt(v_dtDueDate.ToOADate), v_iType:=gPMConstants.PMEDataType.PMDate), gPMConstants.PMEReturnCode)

        ' Execute Action Query
        lReturn = m_oDatabase.SQLAction(sSQL:=kUpdateChaseCycleItemSQL, sSQLName:=kUpdateChaseCycleItemName, bStoredProcedure:=True)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, kUpdateChaseCycleItemSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        Return result
    End Function
End Class
