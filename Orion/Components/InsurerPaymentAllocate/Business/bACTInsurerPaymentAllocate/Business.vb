Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'developer guide no.129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness

    ' ************************************************
    ' Added to replace global variables 03/04/2007
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************

    ' ***************************************************************** '
    ' Edit History :
    '
    ' RAW 13/01/2003 : PS187 : replaced hard-coded sql that deleted from
    '                          TransMatch with stored procedure
    '****************************************************************** '

    Private Const ACClass As String = "Business"

    ' Return value
    Private m_lReturn As Integer

    ' Object parameter members.
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Instance of database object

    Private m_oDatabase As dPMDAO.Database
    Private m_bCloseDatabase As Boolean

    Private m_oAllocationCreate As bACTAllocationCreate.Automated
    Private m_oAllocationDetail As bACTAllocationDetail.Form
    Private m_oAllocation As bACTAllocation.Form
    Private m_oMatchPost As bACTMatchPost.Form
    Private m_oDocument As bACTDocument.Form
    Private m_oTransDetail As bACTTransDetail.Form
    Private m_oAutoNumber As bACTAutoNumber.Business
    Private m_oPeriod As bACTPeriod.Form
    Private m_oAccount As bACTAccount.Form
    Public m_oWriteOffReason As Object


    Private m_lBatchId As Integer
    Private m_lAccountId As Integer
    Private m_lAllocationId As Integer
    Private m_dtAllocationDate As Date
    Private m_lAllocationstatusId As Integer

    Private m_lCashListId As Integer
    Private m_lCashListItemId As Integer
    Private m_cCashListSum As Decimal

    ' Stores the List data from the business object.
    Private m_vListData(,) As Object

    Private m_vKeyArray(,) As Object

    ' PW041202
    Private m_sUnderwritingOrAgency As String = ""


    ' Product family
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
        End Get
    End Property

    ' ***************************************************************** '
    '
    ' Name: Initialise
    '
    ' Description:
    '
    ' History: 01/09/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise


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


            ' Get an instance of the database

            m_lReturn = gPMComponentServices.CheckDatabase(v_sUsername:=m_sUsername, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)


            m_oWriteOffReason = New bACTWriteOffReason.Form
            m_lReturn = m_oWriteOffReason.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Remove component services


            ' Initialisation Code.

            ' Set Username and Password

            ' Set User ID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetProcessModes
    '
    ' Description:
    '
    ' History: 16/09/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTypeOfBusiness As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If Not Information.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: Terminate
    '
    ' Description:
    '
    ' History: 01/09/1999 CTAF - Created.
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
                If m_oAllocationCreate IsNot Nothing Then
                    m_oAllocationCreate.Dispose()
                    m_oAllocationCreate = Nothing
                End If
                If m_oAllocationDetail IsNot Nothing Then
                    m_oAllocationDetail.Dispose()
                    m_oAllocationDetail = Nothing
                End If
                If m_oAllocation IsNot Nothing Then
                    m_oAllocation.Dispose()
                    m_oAllocation = Nothing
                End If
                If m_oWriteOffReason IsNot Nothing Then
                    m_oWriteOffReason.Dispose()
                    m_oWriteOffReason = Nothing
                End If
                If m_oMatchPost IsNot Nothing Then
                    m_oMatchPost.Dispose()
                    m_oMatchPost = Nothing
                End If
                If m_oPeriod IsNot Nothing Then
                    m_oPeriod.Dispose()
                    m_oPeriod = Nothing
                End If
                If m_oAutoNumber IsNot Nothing Then
                    m_oAutoNumber.Dispose()
                    m_oAutoNumber = Nothing
                End If
                If m_oDocument IsNot Nothing Then
                    m_oDocument.Dispose()
                    m_oDocument = Nothing
                End If
                If m_oTransDetail IsNot Nothing Then
                    m_oTransDetail.Dispose()
                    m_oTransDetail = Nothing
                End If
                If m_oAccount IsNot Nothing Then
                    m_oAccount.Dispose()
                    m_oAccount = Nothing
                End If
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()

                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub

    '
    ' PW041202 - copied this function in from underwriting 1.6
    '
    ' ***************************************************************** '
    '
    ' Name: UpdateTransferTransDetail
    '
    ' Description: update transdetail with spare = 'allocated' to 'allocatedx'
    '              this is to deal with transfer data
    ' History: 09/09/2002 Thinh Nguyen - Created
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (UpdateTransferTransDetail) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function UpdateTransferTransDetail(ByVal v_lTransDetailID As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '

    'm_oDatabase.SQLBeginTrans()
    '

    'm_oDatabase.Parameters.Clear()
    '

    'If m_oDatabase.Parameters.Add(sName:="TransDetailID", vValue:=v_lTransDetailID, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse

    'm_oDatabase.SQLRollbackTrans()
    'Return result
    'End If
    '

    'If m_oDatabase.SQLAction(sSQL:="{call spu_Act_UpdTransferTransDetail(?)}", sSQLName:="Update TransDetail", bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then
    '

    'm_oDatabase.SQLRollbackTrans()
    '
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '

    'm_oDatabase.SQLCommitTrans()
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '

    'm_oDatabase.SQLRollbackTrans()
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateTransferTransDetail Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateTransferTransDetail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    'End Try
    'End Function



    ' ***************************************************************** '
    '
    ' Name: SetStatus
    '
    ' Description:
    '
    ' History: 02/09/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function SetStatus(ByVal sProcessStatus As String, ByVal sMapStatus As String, ByVal sStepStatus As String) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_vKeyArray = VB6.CopyArray(vKeyArray)

            '    ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)



                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case PMNavKeyConst.PMKeyNameBatchID

                        m_lBatchId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.ACTKeyNameAccountID

                        m_lAccountId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.ACTKeyNameCashListId

                        m_lCashListId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.ACTKeyNameCashListItemId

                        m_lCashListItemId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                End Select

            Next lRow

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim sTmp As String = ""


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Initialise the key array with the number of
        ' keys needed to be returned.
        ' Note: Remember arrays are zero based.
        ReDim vKeyArray(1, 0)

        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAllocationId

        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lAllocationId

        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: Start
    '
    ' ***************************************************************** '
    Public Function Start() As Integer
        ' Objects

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' PW041202 - get underwriting or agency flag
            m_lReturn = bPMFunc.getUnderwritingOrAgency(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, r_vUnderwriting:=m_sUnderwritingOrAgency)

            m_lReturn = ProcessAllocationCreate()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = ProcessAllocation()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Public Function ProcessAllocationCreate() As Integer

        Dim result As Integer = 0
        Dim vKeyArray(,) As Object

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get an instance of DocumentPost if needed
        If m_oAllocationCreate Is Nothing Then



            m_oAllocationCreate = New bACTAllocationCreate.Automated
            m_lReturn = m_oAllocationCreate.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserId:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyId:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


        End If

        m_lReturn = m_oAllocationCreate.SetKeys(vKeyArray:=m_vKeyArray)
        'eck210302 Add a bit of error trapping
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oAllocationCreate.Start()
        'eck210302 Add a bit of error trapping
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If



        m_lReturn = m_oAllocationCreate.GetKeys(vKeyArray)
        'eck210302 Add a bit of error trapping
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Information.IsArray(vKeyArray) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '    ' Step through the key array.

        For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)



            Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                'eck040601 replace Cint with cLng
                Case PMNavKeyConst.ACTKeyNameAllocationId

                    m_lAllocationId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
            End Select
        Next lRow
        Return result



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessAllocationCreate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    Public Function ProcessAllocation() As Integer


        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Dim iCompanyId As Integer = m_iSourceID

        ' Get an instance of AllocationDetail if needed

        If m_oAllocationDetail Is Nothing Then


            m_oAllocationDetail = New bACTAllocationDetail.Form
            m_lReturn = m_oAllocationDetail.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


        End If
        If m_oAllocation Is Nothing Then


            m_oAllocation = New bACTAllocation.Form
            m_lReturn = m_oAllocation.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


        End If

        ' Set the process modes for the busines object.

        m_lReturn = m_oAllocationDetail.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Load")
            Return result
        End If

        ' Get cash list sum
        If m_lCashListId <> 0 Then

            m_lReturn = m_oAllocation.GetCashListSum(v_lCashListID:=m_lCashListId, r_cCashListSum:=m_cCashListSum)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        m_lReturn = m_oAllocation.GetDetails(vAllocationID:=m_lAllocationId)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If


        m_lReturn = m_oAllocation.GetNext(vAllocationID:=m_lAllocationId, vCompanyID:=iCompanyId, vAccountID:=m_lAccountId, vUserID:=m_iUserID, vAllocationDate:=m_dtAllocationDate, vAllocationStatusId:=m_lAllocationstatusId)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        ' Pass the AllocationID to be used in selection
        ' as we only want the Items belonging to a given list


        m_lReturn = m_oAllocationDetail.GetDetails(vAllocationID:=m_lAllocationId)

        ' {* USER DEFINED CODE (End) *}

        ' If no records found return NotFound
        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        ' Check for other errors
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to get details.
            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

            Return result
        End If

        m_lReturn = AllocateToData()
        ' Check for other errors
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to get details.
            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAllocation")

            Return result
        End If

        ' PW041202 - call different version of autoallocate if underwriting
        m_lReturn = AutoAllocateUW()


        ' Check for other errors
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to get details.
            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to auto allocate", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAllocation")

            Return result
        End If

        m_lReturn = ProcessAllocateCommand()
        ' Check for other errors
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to get details.
            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to auto allocate", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAllocation")

            Return result
        End If

        Return result



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessAllocationCreate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: AllocateToData
    '
    ' Description: Updates the data storage from the business object.
    '
    ' ***************************************************************** '
    Private Function AllocateToData() As Integer

        Dim result As Integer = 0
        Dim lAllocationdetailID, lCashlistitemID, lAllocationID As Integer
        Dim iOriginalCurrency As Integer
        Dim lTransdetailID As Integer
        Dim iDocumenttypeID As Integer
        Dim dtAccountingDate, dtOriginalDate As Date
        Dim iAllocateToBase As Integer
        Dim cOrigBaseAmount, cOrigCcyAmount As Decimal
        Dim vdOrigXrate, vdEffectiveXrate As Object
        Dim cOsBaseAmount, cOsCcyAmount, cAllocBaseAmount, cAllocCcyAmount As Decimal
        Dim iFullyMatched As Integer
        Dim cNewOsCcyAmount, cNewOsBaseAmount, cLossGainAmount As Decimal
        Dim iIsPrimary As Integer
        Dim sDocumentRef As String = ""
        Dim vdAllocBaseAmountUnrounded As Object

        Dim lRecord As Integer

        Dim bWriteOff As Boolean
        Dim lWriteOffReasonID As Integer
        Dim sWriteOffReason, sWriteOffCode As String



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the details to the data storage to be
        ' used with the List.

        ' {* USER DEFINED CODE (Begin) *}

        ' Initialise the data array (Allow one extra field
        ' for the unique key to been assigned).
        ' *** This also needs to be edited in DetailsFormReturn ***
        ReDim m_vListData(29, 0)
        Dim cWriteOffAmount As Decimal ' fix this

        ' Retrieve all of the details from the business object.

        While m_oAllocationDetail.GetNext(vAllocationDetailID:=lAllocationdetailID, vCashlistitemID:=lCashlistitemID, vAllocationID:=lAllocationID, vOriginalCurrency:=iOriginalCurrency, vTransdetailID:=lTransdetailID, vDocumenttypeID:=iDocumenttypeID, vAccountingDate:=dtAccountingDate, vOriginalDate:=dtOriginalDate, vAllocateToBase:=iAllocateToBase, vOrigBaseAmount:=cOrigBaseAmount, vOrigCcyAmount:=cOrigCcyAmount, vOrigXrate:=vdOrigXrate, vEffectiveXrate:=vdEffectiveXrate, vOsBaseAmount:=cOsBaseAmount, vOsCcyAmount:=cOsCcyAmount, vAllocBaseAmount:=cAllocBaseAmount, vAllocCcyAmount:=cAllocCcyAmount, vFullyMatched:=iFullyMatched, vWriteOffAmount:=cWriteOffAmount, vNewOsCcyAmount:=cNewOsCcyAmount, vNewOsBaseAmount:=cNewOsBaseAmount, vLossGainAmount:=cLossGainAmount, vIsPrimary:=iIsPrimary, vDocumentRef:=sDocumentRef, vWriteOffReasonID:=lWriteOffReasonID) = gPMConstants.PMEReturnCode.PMTrue

            'vAllocBaseAmountUnrounded:=vdAllocBaseAmountUnrounded, _
            '
            bWriteOff = (cWriteOffAmount <> 0)

            ' Store all of the data.
            lRecord = m_vListData.GetUpperBound(1)
            m_vListData(ACSubAllocationDetailID, lRecord) = lAllocationdetailID
            m_vListData(ACSubCashlistitemID, lRecord) = lCashlistitemID
            m_vListData(ACSubAllocationID, lRecord) = lAllocationID
            m_vListData(ACSubOriginalCurrency, lRecord) = iOriginalCurrency
            m_vListData(ACSubTransdetailID, lRecord) = lTransdetailID
            m_vListData(ACSubDocumenttypeID, lRecord) = iDocumenttypeID
            m_vListData(ACSubAccountingDate, lRecord) = dtAccountingDate
            m_vListData(ACSubOriginalDate, lRecord) = dtOriginalDate
            m_vListData(ACSubAllocateToBase, lRecord) = iAllocateToBase
            m_vListData(ACSubOrigBaseAmount, lRecord) = cOrigBaseAmount
            m_vListData(ACSubOrigCcyAmount, lRecord) = cOrigCcyAmount

            m_vListData(ACSubOrigXrate, lRecord) = vdOrigXrate

            m_vListData(ACSubEffectiveXrate, lRecord) = vdEffectiveXrate
            m_vListData(ACSubOsBaseAmount, lRecord) = cOsBaseAmount
            m_vListData(ACSubOsCcyAmount, lRecord) = cOsCcyAmount
            m_vListData(ACSubAllocBaseAmount, lRecord) = cAllocBaseAmount
            m_vListData(ACSubAllocCcyAmount, lRecord) = cAllocCcyAmount
            m_vListData(ACSubFullyMatched, lRecord) = iFullyMatched

            m_vListData(ACSubWriteOff, lRecord) = bWriteOff
            m_vListData(ACSubWriteOffReasonID, lRecord) = lWriteOffReasonID

            m_vListData(ACSubNewOsCcyAmount, lRecord) = cNewOsCcyAmount
            m_vListData(ACSubNewOsBaseAmount, lRecord) = cNewOsBaseAmount
            m_vListData(ACSubLossGainAmount, lRecord) = cLossGainAmount
            m_vListData(ACSubIsPrimary, lRecord) = iIsPrimary
            m_vListData(ACSubDocumentRef, lRecord) = sDocumentRef

            m_vListData(ACSubAllocBaseAmountUnrounded, lRecord) = vdAllocBaseAmountUnrounded
            m_vListData(ACSubWriteOffAmount, lRecord) = cWriteOffAmount


            If lWriteOffReasonID <> 0 Then


                m_lReturn = m_oWriteOffReason.GetDetails(vWriteOffReasonID:=lWriteOffReasonID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = m_oWriteOffReason.GetNext(vWriteOffReasonID:=lWriteOffReasonID, vDescription:=sWriteOffReason, vCode:=sWriteOffCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_vListData(ACSubWriteOffReason, lRecord) = sWriteOffReason
                m_vListData(ACSubWriteOffCode, lRecord) = sWriteOffCode

            Else

                m_vListData(ACSubWriteOffReason, lRecord) = ""

            End If

            ' {* USER DEFINED CODE (End) *}

            ' Store unique key for this row.
            m_vListData(m_vListData.GetUpperBound(0), m_vListData.GetUpperBound(1)) = m_vListData.GetUpperBound(1) + 1

            ' Increment the data array.
            ReDim Preserve m_vListData(m_vListData.GetUpperBound(0), m_vListData.GetUpperBound(1) + 1)
        End While

        ' Check if we have data in the List array.
        If Information.IsArray(m_vListData) Then
            If m_vListData.GetUpperBound(1) > 0 Then
                ' Decrement the data array.
                ReDim Preserve m_vListData(m_vListData.GetUpperBound(0), m_vListData.GetUpperBound(1) - 1)
            End If
        End If


        Return result

    End Function
    ' ***************************************************************** '
    ' Name: AutoAllocate
    '
    ' Description: Goes through the list and allocates
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (AutoAllocate) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function AutoAllocate() As Integer
    '
    'Dim result As Integer = 0
    'Const kMethodName As String = "AutoAllocate"
    '
    'Dim lTransdetailID As Integer
    'Dim cBaseAmount, cCurrencyAmount, cAllocBaseAmount, cAllocCcyAmount, cNewOsCcyAmount, cNewOsBaseAmount, cLossGainAmount As Decimal
    'Dim iFullyMatched As Integer
    'Dim oAllocationCalculate As Object
    '
    'On Error GoTo Catch_Renamed
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'Create instance of business object for use inside the loop.
    'm_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oAllocationCalculate, v_sClassName:="bACTAllocationCalculate.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'gPMFunctions.RaiseError("gPMComponentServices.CreateBusinessObject", "v_sClassName = bACTAllocationCalculate.Form", gPMConstants.PMELogLevel.PMLogError)
    'End If
    ' For each item in the list, set the allocated amount
    'For 'iLoop1 As Integer = m_vListData.GetLowerBound(1) To m_vListData.GetUpperBound(1)
    '
    'lTransdetailID = CInt(m_vListData(ACSubTransdetailID, iLoop1))
    'm_lReturn = GetMatchPayment(v_lTransDetailID:=lTransdetailID, v_cBaseAmount:=cBaseAmount, v_cCurrencyAmount:=cCurrencyAmount)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'cBaseAmount = 0
    'cCurrencyAmount = 0
    'End If
    '
    'If CDbl(m_vListData(ACSubOsBaseAmount, iLoop1)) <> 0 Then
    'If cBaseAmount = 0 Then
    'cAllocBaseAmount = CDec(m_vListData(ACSubOsBaseAmount, iLoop1))
    'Else
    'If cBaseAmount > 0 Then
    'cAllocBaseAmount = CDbl(m_vListData(ACSubAllocBaseAmount, iLoop1)) + cBaseAmount
    'Else
    'If CDec(m_vListData(ACSubAllocBaseAmount, iLoop1)) = 0 Then
    'cAllocBaseAmount = cBaseAmount
    'Else
    'cAllocBaseAmount = CDbl(m_vListData(ACSubAllocBaseAmount, iLoop1)) + cBaseAmount
    'End If
    'End If
    '
    'End If
    'Else
    'cAllocBaseAmount = 0
    'End If
    '
    'If CDbl(m_vListData(ACSubOsCcyAmount, iLoop1)) <> 0 Then
    'If cCurrencyAmount = 0 Then
    'cAllocCcyAmount = CDec(m_vListData(ACSubOsCcyAmount, iLoop1))
    'Else
    'If cCurrencyAmount > 0 Then
    'cAllocCcyAmount = CDbl(m_vListData(ACSubAllocCcyAmount, iLoop1)) + cCurrencyAmount
    'Else
    'If CDec(m_vListData(ACSubAllocCcyAmount, iLoop1)) = 0 Then
    'cAllocCcyAmount = cCurrencyAmount
    'Else
    'cAllocCcyAmount = CDec(m_vListData(ACSubOsCcyAmount, iLoop1)) - cCurrencyAmount
    'End If
    'End If
    'End If
    'Else
    'cAllocCcyAmount = 0
    'End If
    '

    'm_lReturn = oAllocationCalculate.CalculateValues(v_iOriginalCurrency:=m_vListData(ACSubOriginalCurrency, iLoop1), v_lCompanyID:=m_iSourceID, v_iAllocateToBase:=m_vListData(ACSubAllocateToBase, iLoop1), v_vdOrigXrate:=m_vListData(ACSubOrigXrate, iLoop1), v_vdEffectiveXrate:=m_vListData(ACSubEffectiveXrate, iLoop1), v_cOsBaseAmount:=m_vListData(ACSubOsBaseAmount, iLoop1), v_cOsCcyAmount:=m_vListData(ACSubOsCcyAmount, iLoop1), r_cAllocBaseAmount:=cAllocBaseAmount, r_cAllocCcyAmount:=cAllocCcyAmount, r_cNewOsCcyAmount:=cNewOsCcyAmount, r_cNewOsBaseAmount:=cNewOsBaseAmount, r_cLossGainAmount:=cLossGainAmount, r_iFullyMatched:=iFullyMatched)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'gPMFunctions.RaiseError("oAllocationCalculate.CalculateValues", "Failed", gPMConstants.PMELogLevel.PMLogError)
    'End If
    '
    'm_vListData(ACSubAllocBaseAmount, iLoop1) = cAllocBaseAmount
    'm_vListData(ACSubAllocCcyAmount, iLoop1) = cAllocCcyAmount
    'm_vListData(ACSubNewOsBaseAmount, iLoop1) = cNewOsBaseAmount
    'm_vListData(ACSubNewOsCcyAmount, iLoop1) = cNewOsCcyAmount
    'm_vListData(ACSubLossGainAmount, iLoop1) = cLossGainAmount
    'm_vListData(ACSubFullyMatched, iLoop1) = iFullyMatched
    '
    'm_vListData(ACSubWriteOffAmount, iLoop1) = 0
    'm_vListData(ACSubWriteOffReason, iLoop1) = ""
    'm_vListData(ACSubWriteOffReasonID, iLoop1) = 0
    'm_vListData(ACSubWriteOffCode, iLoop1) = ""
    '
    'm_lReturn = DataToBusiness(lMode:=gPMConstants.PMEComponentAction.PMEdit, lRow:=iLoop1)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'gPMFunctions.RaiseError("gPMComponentServices.CreateBusinessObject", "v_sClassName = bACTAllocationCalculate.Form", gPMConstants.PMELogLevel.PMLogError)
    'End If
    '
    'Next iLoop1
    '

    'm_lReturn = m_oAllocationDetail.Update()
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'gPMFunctions.RaiseError("m_oAllocationDetail.Update", "Failed", gPMConstants.PMELogLevel.PMLogError)
    'End If
    '
    'GoTo Finally_Renamed
    '
    'Catch_Renamed: '
    '
    ' DO Not Call any functions before here or the error will be lost
    'bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
    '
    ' If you want to rollback a transaction or something, do it here
    '
    'Finally_Renamed: '
    '
    ' Do any tidy up, e.g. Set x = Nothing here
    'If Not (oAllocationCalculate Is Nothing) Then

    'm_lReturn = oAllocationCalculate.Terminate()
    'End If
    'oAllocationCalculate = Nothing
    '
    'Return result
    '
    ' This is for debugging only
    'Resume 
    '
    'Return result
    'End Function

    ' ***************************************************************** '
    ' Name: AutoAllocateUW
    '
    ' Description:
    '   Due to new enhancement to SFU insurer payments, much of the old
    '   SFU juggling is no longer required, reverted to base SFB function
    '
    ' History:
    '   17/11/2003 Peter Finney - Created (well copied)
    ' ***************************************************************** '
    Private Function AutoAllocateUW() As Integer

        Dim result As Integer = 0
        Dim lTransdetailID As Integer
        Dim cBaseAmount, cCurrencyAmount As Decimal



        result = gPMConstants.PMEReturnCode.PMTrue

        ' For each item in the list, set the allocated amount
        For lRow As Integer = m_vListData.GetLowerBound(1) To m_vListData.GetUpperBound(1)
            ' Set the current trans detail
            lTransdetailID = CInt(m_vListData(ACSubTransdetailID, lRow))

            ' Get Allocation Amount
            m_lReturn = GetMatchPayment(v_lTransDetailID:=lTransdetailID, v_cBaseAmount:=cBaseAmount, v_cCurrencyAmount:=cCurrencyAmount)

            ' If error set amounts to zero
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                cBaseAmount = 0
                cCurrencyAmount = 0
            End If

            ' Use Amounts to allocate If part payment - Only allocates in BASE CURRENCY
            ' This assumes that Insurer Transactions are all paid in base currency
            If CDbl(m_vListData(ACSubOsBaseAmount, lRow)) <> 0 Then
                If cBaseAmount = 0 Then
                    m_vListData(ACSubAllocBaseAmount, lRow) = m_vListData(ACSubOsBaseAmount, lRow)
                    m_vListData(ACSubOsBaseAmount, lRow) = 0
                    m_vListData(ACSubNewOsBaseAmount, lRow) = 0
                Else
                    m_vListData(ACSubAllocBaseAmount, lRow) = CDec(m_vListData(ACSubAllocBaseAmount, lRow)) + cBaseAmount
                    m_vListData(ACSubOsBaseAmount, lRow) = CDec(m_vListData(ACSubOsBaseAmount, lRow)) - cBaseAmount
                    m_vListData(ACSubNewOsBaseAmount, lRow) = m_vListData(ACSubOsBaseAmount, lRow)
                End If
            End If

            ' Allocate currency amount
            If CDbl(m_vListData(ACSubOsCcyAmount, lRow)) <> 0 Then
                If cCurrencyAmount = 0 Then
                    m_vListData(ACSubAllocCcyAmount, lRow) = m_vListData(ACSubOsCcyAmount, lRow)
                    m_vListData(ACSubOsCcyAmount, lRow) = 0
                    m_vListData(ACSubNewOsCcyAmount, lRow) = 0
                Else
                    m_vListData(ACSubAllocCcyAmount, lRow) = CDec(m_vListData(ACSubAllocCcyAmount, lRow)) + cCurrencyAmount
                    m_vListData(ACSubOsCcyAmount, lRow) = CDec(m_vListData(ACSubOsCcyAmount, lRow)) - cCurrencyAmount
                    m_vListData(ACSubNewOsCcyAmount, lRow) = m_vListData(ACSubOsCcyAmount, lRow)
                End If
            End If

            ' Set other properties
            m_vListData(ACSubWriteOffAmount, lRow) = 0
            m_vListData(ACSubWriteOffReason, lRow) = ""
            m_vListData(ACSubWriteOffReasonID, lRow) = 0
            m_vListData(ACSubWriteOffCode, lRow) = ""

            ' Commit allocation
            m_lReturn = DataToBusiness(lMode:=gPMConstants.PMEComponentAction.PMEdit, lRow:=lRow)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        Next lRow


        m_lReturn = m_oAllocationDetail.Update()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function



    ' ***************************************************************** '
    ' Name:         ProcessAllocateCommand
    '
    ' Description:  Determines which action to take on the details
    '               depending upon the task and interface state.
    '
    ' ***************************************************************** '
    Private Function ProcessAllocateCommand() As Integer

        Dim result As Integer = 0
        Dim lTransdetailID As Integer


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Check the task.
        Select Case (m_iTask)
            Case gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMAdd
                ' Check if form has been cancelled, if so,
                ' check if the details have changed and if
                ' so, prompt if they wish to cancel.


                ' CF190199 - Update the business object
                For lLoop1 As Integer = m_vListData.GetLowerBound(1) To m_vListData.GetUpperBound(1)
                    m_lReturn = DataToBusiness(lMode:=gPMConstants.PMEComponentAction.PMEdit, lRow:=lLoop1)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Next lLoop1
                ' Update the details using the business object                m_lReturn& = m_oAllocationDetail.Update()

                m_lReturn = m_oAllocationDetail.Update()

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to update the details
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAllocateCommand")
                End If

                'DJM 10/12/2003
                m_lReturn = GainLoss()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'EK 090200 Remove the MarkPayment
                For lLoop1 As Integer = m_vListData.GetLowerBound(1) To m_vListData.GetUpperBound(1)
                    lTransdetailID = CInt(m_vListData(ACSubTransdetailID, lLoop1))
                    m_lReturn = DeleteMatchPayment(v_lTransDetailID:=lTransdetailID)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error.
                        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to delete marking match records", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAllocateCommand")
                        Return result
                    End If
                Next lLoop1

        End Select

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: DataToBusiness
    '
    ' Description: Updates all business members from the data storage.
    '
    ' ***************************************************************** '
    Private Function DataToBusiness(ByRef lMode As Integer, ByRef lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim lBusinessDataID As Integer

        ' {* USER DEFINED CODE (Begin) *}
        Dim lAllocationdetailID, lCashlistitemID, lAllocationID As Integer
        Dim iOriginalCurrency As Integer
        Dim lTransdetailID As Integer
        Dim iDocumenttypeID As Integer
        Dim dtAccountingDate, dtOriginalDate As Date
        Dim iAllocateToBase As Integer
        Dim cOrigBaseAmount, cOrigCcyAmount As Decimal
        Dim vdOrigXrate, vdEffectiveXrate As Object
        Dim cOsBaseAmount, cOsCcyAmount, cAllocBaseAmount, cAllocCcyAmount As Decimal
        Dim iFullyMatched As Integer
        Dim bWriteOff As Boolean
        Dim lWriteOffReasonID As Integer
        Dim cNewOsCcyAmount, cNewOsBaseAmount, cLossGainAmount As Decimal
        Dim iIsPrimary As Integer
        Dim sDocumentRef As String = ""
        Dim cOrigBaseAmountUnrounded, cOrigCcyAmountUnrounded, cWriteOffAmount As Decimal

        ' {* USER DEFINED CODE (End) *}



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the details from the data storage to
        ' the business object.

        ' {* USER DEFINED CODE (Begin) *}

        ' Store all of the displayable data.

        ' ************************************************************
        ' Enter your code here to assign all of the details
        ' from the List data array to the data storage.
        '
        ' Example:-
        '
        '    m_DName$ = m_vListData(0, lRow&)
        '
        ' NOTE: Replace this section with your new code.
        ' ************************************************************

        lAllocationdetailID = CInt(m_vListData(ACSubAllocationDetailID, lRow))
        lCashlistitemID = CInt(m_vListData(ACSubCashlistitemID, lRow))
        lAllocationID = CInt(m_vListData(ACSubAllocationID, lRow))
        iOriginalCurrency = CInt(m_vListData(ACSubOriginalCurrency, lRow))
        lTransdetailID = CInt(m_vListData(ACSubTransdetailID, lRow))
        iDocumenttypeID = CInt(m_vListData(ACSubDocumenttypeID, lRow))
        dtAccountingDate = CDate(m_vListData(ACSubAccountingDate, lRow))
        dtOriginalDate = CDate(m_vListData(ACSubOriginalDate, lRow))
        iAllocateToBase = CInt(m_vListData(ACSubAllocateToBase, lRow))
        cOrigBaseAmount = CDec(m_vListData(ACSubOrigBaseAmount, lRow))

        ' TODO - These...
        cOrigBaseAmountUnrounded = cOrigBaseAmount

        cOrigCcyAmount = CDec(m_vListData(ACSubOrigCcyAmount, lRow))

        vdOrigXrate = m_vListData(ACSubOrigXrate, lRow)

        vdEffectiveXrate = m_vListData(ACSubEffectiveXrate, lRow)
        cOsBaseAmount = CDec(m_vListData(ACSubOsBaseAmount, lRow))
        cOsCcyAmount = CDec(m_vListData(ACSubOsCcyAmount, lRow))
        cAllocBaseAmount = CDec(m_vListData(ACSubAllocBaseAmount, lRow))
        cAllocCcyAmount = CDec(m_vListData(ACSubAllocCcyAmount, lRow))
        iFullyMatched = CInt(m_vListData(ACSubFullyMatched, lRow))
        bWriteOff = CBool(m_vListData(ACSubWriteOff, lRow))
        lWriteOffReasonID = CInt(m_vListData(ACSubWriteOffReasonID, lRow))
        cNewOsCcyAmount = CDec(m_vListData(ACSubNewOsCcyAmount, lRow))
        cNewOsBaseAmount = CDec(m_vListData(ACSubNewOsBaseAmount, lRow))
        cLossGainAmount = CDec(m_vListData(ACSubLossGainAmount, lRow))
        iIsPrimary = CInt(m_vListData(ACSubIsPrimary, lRow))
        sDocumentRef = CStr(m_vListData(ACSubDocumentRef, lRow))
        ' {* USER DEFINED CODE (End) *}

        ' This should be in the db as boolean

        If bWriteOff Then
            cWriteOffAmount = cOsBaseAmount - cAllocBaseAmount
        Else
            cWriteOffAmount = 0
            lWriteOffReasonID = VariantType.Null
        End If

        ' Store unique key for this row.
        lBusinessDataID = CInt(m_vListData(m_vListData.GetUpperBound(0), lRow))

        ' Check the task.
        Select Case (lMode)
            Case gPMConstants.PMEComponentAction.PMAdd
                ' Inform the business object with a new data item.
                ' {* USER DEFINED CODE (Begin) *}

                m_lReturn = m_oAllocationDetail.EditAdd(lRow:=lBusinessDataID, vAllocationDetailID:=lAllocationdetailID, vCashlistitemID:=lCashlistitemID, vAllocationID:=lAllocationID, vOriginalCurrency:=iOriginalCurrency, vTransdetailID:=lTransdetailID, vDocumenttypeID:=iDocumenttypeID, vAccountingDate:=dtAccountingDate, vOriginalDate:=dtOriginalDate, vAllocateToBase:=iAllocateToBase, vOrigBaseAmount:=cOrigBaseAmount, vOrigBaseAmountUnrounded:=cOrigBaseAmountUnrounded, vOrigCcyAmount:=cOrigCcyAmount, vOrigXrate:=vdOrigXrate, vEffectiveXrate:=vdEffectiveXrate, vOsBaseAmount:=cOsBaseAmount, vOsCcyAmount:=cOsCcyAmount, vAllocBaseAmount:=cAllocBaseAmount, vAllocCcyAmount:=cAllocCcyAmount, vFullyMatched:=iFullyMatched, vWriteOffAmount:=cWriteOffAmount, vNewOsCcyAmount:=cNewOsCcyAmount, vNewOsBaseAmount:=cNewOsBaseAmount, vLossGainAmount:=cLossGainAmount, vIsPrimary:=iIsPrimary, vDocumentRef:=sDocumentRef, vWriteOffReasonID:=lWriteOffReasonID)

                ' {* USER DEFINED CODE (End) *}

            Case gPMConstants.PMEComponentAction.PMEdit
                ' Inform the business object with an updated data item.

                ' {* USER DEFINED CODE (Begin) *}

                m_lReturn = m_oAllocationDetail.EditUpdate(lRow:=lBusinessDataID, vAllocationDetailID:=lAllocationdetailID, vCashlistitemID:=lCashlistitemID, vAllocationID:=lAllocationID, vOriginalCurrency:=iOriginalCurrency, vTransdetailID:=lTransdetailID, vDocumenttypeID:=iDocumenttypeID, vAccountingDate:=dtAccountingDate, vOriginalDate:=dtOriginalDate, vAllocateToBase:=iAllocateToBase, vOrigBaseAmount:=cOrigBaseAmount, vOrigBaseAmountUnrounded:=cOrigBaseAmountUnrounded, vOrigCcyAmountUnrounded:=cOrigCcyAmountUnrounded, vOrigCcyAmount:=cOrigCcyAmount, vOrigXrate:=vdOrigXrate, vEffectiveXrate:=vdEffectiveXrate, vOsBaseAmount:=cOsBaseAmount, vOsCcyAmount:=cOsCcyAmount, vAllocBaseAmount:=cAllocBaseAmount, vAllocCcyAmount:=cAllocCcyAmount, vFullyMatched:=iFullyMatched, vWriteOffAmount:=cWriteOffAmount, vNewOsCcyAmount:=cNewOsCcyAmount, vNewOsBaseAmount:=cNewOsBaseAmount, vLossGainAmount:=cLossGainAmount, vIsPrimary:=iIsPrimary, vDocumentRef:=sDocumentRef, vWriteOffReasonID:=lWriteOffReasonID)


                ' {* USER DEFINED CODE (End) *}

            Case gPMConstants.PMEComponentAction.PMDelete
                ' Inform the business object with a deleted data item.

                m_lReturn = m_oAllocationDetail.EditDelete(lRow:=lBusinessDataID)

        End Select

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the data details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToBusiness")
        End If

        Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: GetSymbolForCurrency
    '
    ' Description:
    '
    ' History: 01/10/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetSymbolForCurrency(ByVal v_lCurrencyID As Integer, ByRef r_sSymbol As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Construct the SQL
            sSQL = "SELECT symbol FROM Currency WHERE currency_id = " & v_lCurrencyID

            ' Perform the SQL

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetSymbolForCurrency", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the result
            If Information.IsArray(vResultArray) Then

                r_sSymbol = CStr(vResultArray(0, 0)).Trim()
            Else
                ' Default to GBP
                r_sSymbol = "£"
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSymbolForCurrency Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSymbolForCurrency", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetMatchPayment
    '
    ' Description:
    '
    ' History: 090200 - Created.
    '
    ' ***************************************************************** '
    Public Function GetMatchPayment(ByVal v_lTransDetailID As Integer, ByRef v_cBaseAmount As Decimal, ByRef v_cCurrencyAmount As Decimal) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            v_cBaseAmount = 0
            v_cCurrencyAmount = 0

            With m_oDatabase


                .Parameters.Clear()
                ' Add transdetail_id

                m_lReturn = m_oDatabase.Parameters.Add(sName:="transdetail_id", vValue:=v_lTransDetailID, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Construct the SQL
                sSQL = "SELECT allocationdetail_id, base_match_amount, currency_match_amount  FROM transmatch WHERE transdetail_id = {transdetail_id}"
                ' Perform the SQL

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetMatchPayment", bStoredProcedure:=False, vResultArray:=vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With
            ' Get the result
            If Information.IsArray(vResultArray) Then

                For lRow As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                    If (CStr(vResultArray(0, lRow))) = "" Then

                        v_cBaseAmount = CDec(vResultArray(1, lRow))

                        v_cCurrencyAmount = CDec(vResultArray(2, lRow))
                    End If
                Next lRow
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMatchPayment Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMatchPayment", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: DeleteMatchPayment
    '
    ' Description:
    '
    ' History: 090200 - Created.
    '
    ' ***************************************************************** '
    Public Function DeleteMatchPayment(ByVal v_lTransDetailID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            With m_oDatabase


                .Parameters.Clear()
                ' Add transdetail_id

                m_lReturn = m_oDatabase.Parameters.Add(sName:="transdetail_id", vValue:=v_lTransDetailID, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Construct the SQL
                sSQL = "SELECT allocationdetail_id,transmatch_id FROM transmatch WHERE transdetail_id = {transdetail_id}"
                ' Perform the SQL

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetMatchPayment", bStoredProcedure:=False, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                ' Get the result
                If Information.IsArray(vResultArray) Then

                    For lRow As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                        If (CStr(vResultArray(0, lRow))) = "" Then
                            ' Add transmatch_id

                            .Parameters.Clear()



                            m_lReturn = .Parameters.Add(sName:="transmatch_id", vValue:=CInt(vResultArray(1, lRow)), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            ' RAW 13/01/2003 : PS187 : replaced with stored procedure
                            ' sSQL$ = "DELETE FROM transmatch WHERE (transmatch_id = {transmatch_id})"
                            ' Perform the SQL

                            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteMatchPaymentSQL, sSQLName:=ACDeleteMatchPaymentName, bStoredProcedure:=ACDeleteMatchPaymentStored)
                            ' RAW 13/01/2003 : PS187 : end

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                        End If
                    Next lRow
                End If




                ' Construct the SQL
            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteMatchPayment Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteMatchPayment", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ******************************************************************** '
    ' Name: GainLoss
    '
    ' Description: Writes off the current allocation difference
    '
    ' CF: This should really be using AddCompleteDocument in bACTDocument
    '
    ' ******************************************************************** '
    Private Function GainLoss() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GainLoss"





        result = gPMConstants.PMEReturnCode.PMTrue

        'Loop through each transaction in the allocation and if it has a currency exchange difference then create a SCD transaction for it.
        For lLoop As Integer = m_vListData.GetLowerBound(1) To m_vListData.GetUpperBound(1)
            If CDbl(m_vListData(ACSubLossGainAmount, lLoop)) <> 0 Then
                m_lReturn = CreateCurrencyDiffTransactions(v_lTransDetailID:=CInt(m_vListData(ACSubTransdetailID, lLoop)), v_lAllocationDetailID:=CInt(m_vListData(ACSubAllocationDetailID, lLoop)), v_cCurrDiff:=CDec(m_vListData(ACSubLossGainAmount, lLoop)))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("CreateCurrencyDiffTransactions", "v_lTransdetailID:=" & CStr(m_vListData(ACSubTransdetailID, lLoop)), gPMConstants.PMELogLevel.PMLogError)
                End If
            End If
        Next

        ' Do any tidy up, e.g. Set x = Nothing here
        Return result
        ' This is for debugging only
    End Function


    ' ***************************************************************** '
    ' Name: GetLedgerForAccount
    '
    ' Description: Gets the ledger_id for the passed account.
    '
    ' ***************************************************************** '
    Private Function GetLedgerForAccount(ByVal v_lAccountID As Integer, ByRef r_lLedgerID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetLedgerForAccount"

        Dim lLoop As Integer




        result = gPMConstants.PMEReturnCode.PMTrue

        If m_oAccount Is Nothing Then

            m_oAccount = New bACTAccount.Form
            m_lReturn = m_oAccount.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("gPMComponentServices.CreateBusinessObject", "v_sObjectName:=bACTAccount.Form", gPMConstants.PMELogLevel.PMLogError)
            End If
        End If


        m_lReturn = m_oAccount.GetDetails(vAccountID:=v_lAccountID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oAccount.GetDetails", "vAccountID:=" & v_lAccountID, gPMConstants.PMELogLevel.PMLogError)
        End If


        m_lReturn = m_oAccount.GetNext(vLedgerID:=r_lLedgerID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oAccount.GetNext", "vAccountID:=" & v_lAccountID, gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Do any tidy up, e.g. Set x = Nothing here
        Return result
        ' This is for debugging only
    End Function

    ' ***************************************************************** '
    ' Name: GetGLAccount
    '
    ' Description: Gets the gains/loss ledger account_id depending on sales
    '              or purchase ledger
    '
    ' DD 05/08/2002: Alterations for multi-branch accounting
    ' ***************************************************************** '
    Private Function GetGLAccount(ByVal v_cAmount As Decimal, ByRef r_lGLAccountID As Integer) As Integer

        Dim result As Integer = 0
        Dim sShortCode As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        If v_cAmount > 0 Then
            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=ACCurrencyDifferenceDebitAccount, r_sOptionValue:=sShortCode)
        Else
            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=ACCurrencyDifferenceCrebitAccount, r_sOptionValue:=sShortCode)
        End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or sShortCode = "" Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the account_id from the business

        m_lReturn = m_oAllocation.GetAccountID(v_sShortCode:=sShortCode, r_lAccountID:=r_lGLAccountID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    Private Function CreateCurrencyDiffTransactions(ByVal v_lTransDetailID As Integer, ByVal v_lAllocationDetailID As Integer, ByVal v_cCurrDiff As Decimal) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CreateCurrencyDiffTransactions"

        Dim lLoop, lGLAccountID, lDocumentID As Integer
        Dim sDocumentRef As String = ""
        Dim lTransdetailID As Integer
        Dim vCurrencyID As Integer
        Dim vPeriodID, lPeriodID, lLedgerID As Integer
        Dim cAmount As Decimal
        Dim vFullyMatched As Byte
        Dim vCurrencyAmount As Decimal
        Dim vCurrencyBaseXRate As Byte
        Dim vComment As String = ""
        Dim vInsuranceRef, vPurchaseOrderNo, vPurchaseInvoiceNo, vDepartment As Object
        Dim vSpare As String = ""
        Dim vRefDate As Object
        Dim vRefAmount As Byte
        Dim vRefQuantity As Byte
        Dim vRefUnits As Byte
        Dim vAccountingDate As Date
        Dim lGainLossReasonID As Integer
        Dim vBaseAmountUnrounded As Decimal
        Dim vCurrencyAmountUnrounded As Decimal
        Dim lMatchID As Integer
        Dim vAuditSetID As Byte
        Dim vBatchID As Byte
        Dim vDocumentRef As String = ""
        Dim vAuthorisedDate As Date
        Dim vDocumentDate As Date
        Dim vCreatedDate As Date
        Dim lNumberRangeID, lNumber, lCompanyID As Integer

        Dim lReturn As Integer
        '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        Dim sReference As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        'Create all business objects that we need.
        If m_oDocument Is Nothing Then

            m_oDocument = New bACTDocument.Form
            m_lReturn = m_oDocument.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("gPMComponentServices.CreateBusinessObject", "v_sObjectName:=bACTDocument.Form", gPMConstants.PMELogLevel.PMLogError)
            End If
        End If

        If m_oTransDetail Is Nothing Then

            m_oTransDetail = New bACTTransDetail.Form
            m_lReturn = m_oTransDetail.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("gPMComponentServices.CreateBusinessObject", "v_sObjectName:=bACTTransDetail.Form", gPMConstants.PMELogLevel.PMLogError)
            End If
        End If

        If m_oAutoNumber Is Nothing Then

            m_oAutoNumber = New bACTAutoNumber.Business
            m_lReturn = m_oAutoNumber.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("gPMComponentServices.CreateBusinessObject", "v_sObjectName:=bACTAutoNumber.Business", gPMConstants.PMELogLevel.PMLogError)
            End If
        End If

        If m_oPeriod Is Nothing Then

            m_oPeriod = New bACTPeriod.Form
            m_lReturn = m_oPeriod.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("gPMComponentServices.CreateBusinessObject", "v_sObjectName:=bACTPeriod.Form", gPMConstants.PMELogLevel.PMLogError)
            End If
        End If

        If m_oMatchPost Is Nothing Then

            m_oMatchPost = New bACTMatchPost.Form
            m_lReturn = m_oMatchPost.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("gPMComponentServices.CreateBusinessObject", "v_sObjectName:=bACTMatchPost.Form", gPMConstants.PMELogLevel.PMLogError)
            End If
        End If

        'Get the branch id from the transaction that this currency difference is for.

        m_lReturn = m_oTransDetail.GetDetails(vTransdetailID:=v_lTransDetailID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oTransDetail.GetDetails", "vTransdetailID:=" & v_lTransDetailID, gPMConstants.PMELogLevel.PMLogError)
        End If


        m_lReturn = m_oTransDetail.GetNext(vDocumentID:=lDocumentID, vCompanyID:=lCompanyID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oTransDetail.GetDetails", "vTransdetailID:=" & v_lTransDetailID, gPMConstants.PMELogLevel.PMLogError)
        End If



        m_lReturn = m_oDocument.GetDetails(vDocumentID:=lDocumentID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDocument.GetDetails", "vDocumentID:=" & lDocumentID, gPMConstants.PMELogLevel.PMLogError)
        End If


        m_lReturn = m_oDocument.GetNext(vDocumentRef:=sDocumentRef)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("GetNext.GetDetails", "vDocumentID:=" & lDocumentID, gPMConstants.PMELogLevel.PMLogError)
        End If

        'Get the ledger for the account
        m_lReturn = GetLedgerForAccount(v_lAccountID:=m_lAccountId, r_lLedgerID:=lLedgerID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("GetLedgerForAccount", "Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        'Get the period

        m_lReturn = m_oPeriod.GetPostingPeriodForDate(dtDateInPeriod:=DateTime.Now, lPeriodID:=lPeriodID, lLedgerID:=lLedgerID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oPeriod.GetPostingPeriodForDate", "Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Re-assign the period_id
        vPeriodID = lPeriodID

        ' Get the Gains/Loss account for this ledger
        m_lReturn = GetGLAccount(v_cAmount:=v_cCurrDiff, r_lGLAccountID:=lGLAccountID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("GetGLAccount", "Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        'EK 220200 get correct number for Adjustment
        ' Get the number range for documentref


        m_lReturn = m_oAutoNumber.GetNumberRange(v_sGroupCode:=gACTLibrary.ACTAutoNumberGroupCodeDocumentRef49, v_sRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeSCD, r_lNumberRangeID:=lNumberRangeID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oAutoNumber.GenerateNumber", "Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Get the next number
        'Start(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber

        m_lReturn = m_oAutoNumber.GenerateDocumentReferenceNumber(v_lNumberRangeID:=lNumberRangeID, v_iUserID:=m_iUserID, v_iCompanyID:=lCompanyID, r_sDocumentRef:=sReference, v_sRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeSCD)
        'End(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber
            gPMFunctions.RaiseError("m_oAutoNumber.GenerateDocumentReferenceNumber", "Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Generate a document
        lDocumentID = 0
        vAuditSetID = 0
        vAuthorisedDate = DateTime.Now
        vBatchID = 0
        vComment = "Currency Fluctuation (Generated)"
        'Start(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        'vDocumentRef = Format$(lNumber&, "00000000")
        vDocumentRef = gACTLibrary.ACTAutoNumberRangeCodeSCD & sReference
        'End(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        vDocumentDate = DateTime.Now
        vCreatedDate = DateTime.Now
        lGainLossReasonID = 0

        ' Add a match group

        m_lReturn = m_oMatchPost.AddMatchGroup(v_dtMatchDate:=vDocumentDate, r_vMatchId:=lMatchID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("oMatchPost.AddMatchGroup", "Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Add it...

        m_lReturn = m_oDocument.DirectAdd(vDocumentID:=lDocumentID, vCompanyID:=lCompanyID, vPostingStatusID:=gACTLibrary.ACTPostStatusPosted, vDocumenttypeID:=gACTLibrary.ACTDocTypeCurrencyDifferenceCredit, vAuditSetID:=vAuditSetID, vBatchID:=vBatchID, vDocumentRef:=vDocumentRef, vDocumentDate:=vDocumentDate, vCreatedDate:=vCreatedDate, vAuthorisedDate:=vAuthorisedDate, vComment:=vComment)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDocument.DirectAdd", "vDocumentRef:=" & vDocumentRef, gPMConstants.PMELogLevel.PMLogError)
        End If

        vCurrencyID = m_iCurrencyID
        vAccountingDate = vDocumentDate
        cAmount = v_cCurrDiff
        vBaseAmountUnrounded = v_cCurrDiff
        vCurrencyAmountUnrounded = v_cCurrDiff
        vFullyMatched = 1
        vCurrencyAmount = v_cCurrDiff
        vCurrencyBaseXRate = 1
        vComment = "Currency Fluctuation"
        vRefAmount = 0
        vRefQuantity = 0
        vRefUnits = 0
        vSpare = "CURRDIF " & sDocumentRef.Trim()

        ' Generate a transaction for the sales/purchase ledger

        m_lReturn = m_oTransDetail.DirectAdd(vTransdetailID:=lTransdetailID, vAccountID:=m_lAccountId, vPostingStatusID:=gACTLibrary.ACTPostStatusPosted, vCompanyID:=lCompanyID, vCurrencyID:=vCurrencyID, vPeriodID:=vPeriodID, vDocumentID:=lDocumentID, vDocumentSequence:=1, vAccountingDate:=vAccountingDate, vAmount:=cAmount, vBaseAmountUnrounded:=vBaseAmountUnrounded, vFullyMatched:=vFullyMatched, vCurrencyAmount:=vCurrencyAmount, vCurrencyAmountUnrounded:=vCurrencyAmountUnrounded, vCurrencyBaseXRate:=vCurrencyBaseXRate, vComment:=vComment, vInsuranceRef:=vInsuranceRef, vOperatorID:=m_iUserID, vPurchaseOrderNo:=vPurchaseOrderNo, vPurchaseInvoiceNo:=vPurchaseInvoiceNo, vDepartment:=vDepartment, vSpare:=vSpare, vRefQuantity:=vRefQuantity)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("oTransDetail.DirectAdd", "vAccountID:=" & m_lAccountId, gPMConstants.PMELogLevel.PMLogError)
        End If

        'Match the transaction

        m_lReturn = m_oMatchPost.AddMatchTrans(v_lAllocationDetailID:=v_lAllocationDetailID, v_lTransDetailID:=lTransdetailID, v_iCurrencyID:=vCurrencyID, v_cBaseMatchAmount:=v_cCurrDiff, v_cCurrencyMatchAmount:=v_cCurrDiff, v_vdCurrencyMatchXRate:=vCurrencyBaseXRate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oMatchPost.AddMatchTrans", "v_lTransDetailID:=" & lTransdetailID, gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Generate a transaction for the nominal ledger write off account
        cAmount = v_cCurrDiff * -1
        vCurrencyAmount = v_cCurrDiff * -1
        vBaseAmountUnrounded = v_cCurrDiff * -1
        vCurrencyAmountUnrounded = v_cCurrDiff * -1
        vComment = "Matching Write Off Transaction"


        m_lReturn = m_oTransDetail.DirectAdd(vTransdetailID:=lTransdetailID, vAccountID:=lGLAccountID, vPostingStatusID:=gACTLibrary.ACTPostStatusPosted, vCompanyID:=lCompanyID, vCurrencyID:=vCurrencyID, vPeriodID:=vPeriodID, vDocumentID:=lDocumentID, vDocumentSequence:=2, vAccountingDate:=vAccountingDate, vAmount:=cAmount, vBaseAmountUnrounded:=vBaseAmountUnrounded, vFullyMatched:=vFullyMatched, vCurrencyAmount:=vCurrencyAmount, vCurrencyAmountUnrounded:=vCurrencyAmountUnrounded, vCurrencyBaseXRate:=vCurrencyBaseXRate, vComment:=vComment, vInsuranceRef:=vInsuranceRef, vOperatorID:=m_iUserID, vPurchaseOrderNo:=vPurchaseOrderNo, vPurchaseInvoiceNo:=vPurchaseInvoiceNo, vDepartment:=vDepartment, vSpare:=vSpare, vRefQuantity:=vRefQuantity)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("oTransDetail.DirectAdd", "vAccountID:=" & lGLAccountID, gPMConstants.PMELogLevel.PMLogError)
        End If

        'Terminate the objects

        m_lReturn = m_oMatchPost.Commit()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oMatchPost.Commit", "Failed", gPMConstants.PMELogLevel.PMLogError)
        End If



        ' Do any tidy up, e.g. Set x = Nothing here
        Return result
        ' This is for debugging only
    End Function
End Class
