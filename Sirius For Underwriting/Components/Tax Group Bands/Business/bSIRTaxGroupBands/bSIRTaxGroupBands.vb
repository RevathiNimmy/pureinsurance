Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
' developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 09/06/1999
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a SIRSharedPremiums.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 22/12/2003
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
    Private m_lProcessMode As gPMConstants.PMEProcessMode
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private lPMAuthorityLevel As Integer

    ' Primary Keys to work with
    Private m_lTaxGroupId As Integer



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

    Public Property TaxGroupId() As Integer
        Get
            Return m_lTaxGroupId
        End Get
        Set(ByVal Value As Integer)
            m_lTaxGroupId = Value
        End Set
    End Property


    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
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



            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


            If Not Information.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CType(CInt(vProcessMode), gPMConstants.PMEProcessMode)
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
    ' Name:         GetTaxGroupTaxBands
    ' Description:  Loads all tax bands linked to a given tax group
    ' History:      30/04/2003 - Created - Alix Bergeret
    ' ***************************************************************** '
    Public Function GetTaxGroupTaxBands(ByRef r_vTaxGroupTaxBands(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "tax_group_id", m_lTaxGroupId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectTaxGroupTaxBandsSQL, sSQLName:=ACSelectTaxGroupTaxBandsName, bStoredProcedure:=ACSelectTaxGroupTaxBandsStored, vResultArray:=r_vTaxGroupTaxBands)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaxGroupTaxBands")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTaxGroupTaxBands Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaxGroupTaxBands", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' *****************************************************************
    ' Name: Update (Public)
    '
    '
    ' ***************************************************************** '
    Public Function Update(ByVal v_vTaxGroupTaxBands(,) As Object, Optional v_sUniqueId As String = "", Optional v_sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Update"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bTransaction As Boolean
        Dim vAllocSeq As gPMConstants.PMEReturnCode

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin transaction
            lReturn = m_oDatabase.SQLBeginTrans()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLBeginTrans", "Unable to begin transaction")
            End If
            bTransaction = True

            ' First, we delete existing records
            bPMAddParameter.AddParameterLite(m_oDatabase, "tax_group_id", m_lTaxGroupId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "userid", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "UniqueId", v_sUniqueId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "ScreenHierarchy", v_sScreenHierarchy, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteTaxGroupTaxBandsSQL, sSQLName:=ACDeleteTaxGroupTaxBandsName, bStoredProcedure:=ACDeleteTaxGroupTaxBandsStored)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to execute: " & ACDeleteTaxGroupTaxBandsSQL)
            End If

            ' If we have no new tab bands commit the transaction and exit
            If Not Information.IsArray(v_vTaxGroupTaxBands) Then
                Return result
            End If

            ' Then we re-add all the records
            For lCount As Integer = v_vTaxGroupTaxBands.GetLowerBound(1) To v_vTaxGroupTaxBands.GetUpperBound(1)

                If CInt(v_vTaxGroupTaxBands(ACPTaxBandID, lCount)) > 0 Then

                    ' ensure a valid allocation sequence has been provided

                    If CStr(v_vTaxGroupTaxBands(ACPAllocSequence, lCount)) = "" Then

                        vAllocSeq = Nothing
                    Else


                        vAllocSeq = CInt(v_vTaxGroupTaxBands(ACPAllocSequence, lCount))
                    End If

                    ' Add input parameters
                    bPMAddParameter.AddParameterLite(m_oDatabase, "tax_group_id", m_lTaxGroupId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

                    bPMAddParameter.AddParameterLite(m_oDatabase, "tax_band_id", v_vTaxGroupTaxBands(ACPTaxBandID, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                    bPMAddParameter.AddParameterLite(m_oDatabase, "sequence", v_vTaxGroupTaxBands(ACPSequence, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "alloc_seq", vAllocSeq, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                    bPMAddParameter.AddParameterLite(m_oDatabase, "alloc_rule", v_vTaxGroupTaxBands(ACPAllocRule, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                    bPMAddParameter.AddParameterLite(m_oDatabase, "userid", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "UniqueId", v_sUniqueId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "ScreenHierarchy", v_sScreenHierarchy, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                    ' Execute SP
                    lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertTaxGroupTaxBandsSQL, sSQLName:=ACInsertTaxGroupTaxBandsName, bStoredProcedure:=ACInsertTaxGroupTaxBandsStored)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to execute: " & ACInsertTaxGroupTaxBandsSQL)
                    End If
                End If
            Next lCount

            ' If we have an open transaction commit it
            If bTransaction Then
                lReturn = m_oDatabase.SQLCommitTrans()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDatabase.SQLCommitTrans", "Unable to commit transaction")
                End If

                bTransaction = False
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            If bTransaction Then
                lReturn = m_oDatabase.SQLRollbackTrans()
                bTransaction = False
            End If

        Finally
            ' Do any tidy up, e.g. Set x = Nothing here
            '		Return result

            ' This is for debugging only
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function


    Public Sub New()
        MyBase.New()
    End Sub

    Protected Overrides Sub Finalize()
		Dispose(False)
    End Sub

End Class
