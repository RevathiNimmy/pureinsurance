Option Strict Off
Option Explicit On
'developer guide no 129
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Form_NET.Form")>
Public NotInheritable Class Form

    Implements IDisposable
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


    ' ***************************************************************** '
    ' Class Name: Form
    ' Date: 03/12/1997
    '
    ' Description: Batch allocate methods
    '
    '
    '
    ' Edit History:
    ' RAW 12/3/2003 : ISS2893 : handle write off amounts
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Form"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Error Code (Private)
    Private m_lReturn As Integer
    Private m_oBusiness As Business
    Private m_lAllocationId As Integer


    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

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
    ' Name: GetKeys (Standard Method)
    '
    ' Description: Stores all of the key array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vkeyarray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise the key array with the number of
            ' keys needed to be returned.
            ' Note: Remember arrays are zero based.
            ReDim vkeyarray(1, 0)

            ' Assign the key array with the parameter members.

            vkeyarray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAllocationId

            vkeyarray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lAllocationId

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Public Function SetKeys(ByRef vkeyarray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    m_vCashListId = Null
            '    m_lCashListItemId = Null

            ' Check we have a vaild array.
            If Not Informations.IsArray(vkeyarray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    ' Step through the key array.
            '    For lRow& = LBound(vkeyarray, 2) To UBound(vkeyarray, 2)
            '        ' Assign the parameter member with the
            '        ' correct key array item.
            '
            '        Select Case Trim$(CStr(vkeyarray(PMKeyName, lRow&)))
            '          Case ACTKeyNameCashListId
            '            m_vCashListId = vkeyarray(PMKeyValue, lRow&)
            '          Case ACTKeyNameCashListItemId
            '            m_vCashListItemId = vkeyarray(PMKeyValue, lRow&)
            '        End Select
            '
            '    Next lRow&
            '
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserId As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyId As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUserName
            m_sPassword = sPassword
            m_iUserID = iUserId
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyId
            m_iLogLevel = iLogLevel


            m_oBusiness = New Business()

            m_lReturn = m_oBusiness.Initialise(sUserName:=sUserName, sPassword:=sPassword, iUserId:=iUserId, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyId:=iCurrencyId, iLogLevel:=iLogLevel, sCallingAppName:=sCallingAppName, vDatabase:=vDatabase)


            Return m_lReturn

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
                m_oBusiness = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub

    ' ***************************************************************** '
    ' Name: CalculateValues
    '
    ' Description: Public function to calculate the allocation values
    '              of a single allocation detail. Simply passes on the
    '              call to the private business class
    ' ***************************************************************** '
    Public Function CalculateValues(ByVal v_iOriginalCurrency As Integer, ByVal v_lCompanyID As Integer, ByVal v_iAllocateToBase As Integer, ByVal v_vdOrigXrate As Object, ByVal v_vdEffectiveXrate As Object, ByVal v_cOsBaseAmount As Decimal, ByVal v_cOsCcyAmount As Decimal, ByRef r_cAllocBaseAmount As Decimal, ByRef r_cAllocCcyAmount As Decimal, ByRef r_cNewOsCcyAmount As Decimal, ByRef r_cNewOsBaseAmount As Decimal, ByRef r_cLossGainAmount As Decimal, ByRef r_iFullyMatched As Integer, Optional ByRef r_vdAllocBaseAmountUnrounded As Object = Nothing, Optional ByRef r_cWriteOffBaseAmount As Decimal = 0, Optional ByRef r_cWriteOffCcyAmount As Decimal = 0) As Integer

        Dim result As Integer = 0
        Dim bFullyMatched, bAllocateToBase As Boolean
        Dim vdAllocBaseAmountUnrounded As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bAllocateToBase = v_iAllocateToBase = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.CalculateRecordValues(v_iOriginalCurrency:=v_iOriginalCurrency, v_lCompanyID:=v_lCompanyID, v_bAllocateToBase:=bAllocateToBase, v_vdOrigXrate:=v_vdOrigXrate, v_vdEffectiveXrate:=v_vdEffectiveXrate, v_cOsBaseAmount:=v_cOsBaseAmount, v_cOsCcyAmount:=v_cOsCcyAmount, r_cAllocBaseAmount:=r_cAllocBaseAmount, r_cAllocCcyAmount:=r_cAllocCcyAmount, r_cNewOsCcyAmount:=r_cNewOsCcyAmount, r_cNewOsBaseAmount:=r_cNewOsBaseAmount, r_cLossGainAmount:=r_cLossGainAmount, r_bFullyMatched:=bFullyMatched, r_vdAllocBaseAmountUnrounded:=vdAllocBaseAmountUnrounded, r_cWriteOffBaseAmount:=r_cWriteOffBaseAmount, r_cWriteOffCcyAmount:=r_cWriteOffCcyAmount)

            If bFullyMatched Then
                r_iFullyMatched = gPMConstants.PMEReturnCode.PMTrue
            Else
                r_iFullyMatched = gPMConstants.PMEReturnCode.PMFalse
            End If


            If Not Informations.IsNothing(r_vdAllocBaseAmountUnrounded) Then


                r_vdAllocBaseAmountUnrounded = vdAllocBaseAmountUnrounded
            End If


            Return m_lReturn

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Calculate Values Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CalculateValues", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function
    ' ***************************************************************** '
    ' Name: TotalOfAllocation
    '
    ' Description: Public function to calculate the amount required to
    '              balance an allocation set and a suggestion of what
    '              currency that entry would be in
    ' ***************************************************************** '
    Public Function TotalOfAllocation(ByVal v_lAllocationId As Integer, ByRef r_cTotalCcyAmount As Decimal, ByRef r_cTotalBaseAmount As Decimal, ByRef r_iCurrencyId As Integer, ByRef r_bSameCurrency As Boolean) As Object

        Dim result As Object = Nothing
        Try

            m_oBusiness.AllocationId = v_lAllocationId

            m_lReturn = m_oBusiness.CalculateAllocationSet(r_cTotalBaseAmount:=r_cTotalBaseAmount, r_cTotalCcyAmount:=r_cTotalCcyAmount, r_iCurrencyId:=r_iCurrencyId, r_bSameCurrency:=r_bSameCurrency)


            Return m_lReturn

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TotalOfAllocation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TotalOfAllocation", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetAllocationDetails(ByVal v_lAllocationId As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oBusiness.AllocationId = v_lAllocationId

            m_lReturn = m_oBusiness.GetAllocationDetails()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllocationDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllocationDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function IsTransInAllocation(ByVal v_lTransactionId As Integer) As Integer

        Dim result As Integer = 0
        Try


            Return m_oBusiness.IsTransInAllocation(v_lTransactionId:=v_lTransactionId)

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsTransInAllocation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsTransInAllocation", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class