Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'developer guide no 129. 
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("cTaxParameters_NET.cTaxParameters")> _
Public NotInheritable Class cTaxParameters 

    Private Const ACClass As String = "cScriptingItem"

    Private m_vProcessType As Object
    Private m_vPayee As Object
    Private m_vPaymentToCode As Object
    Private m_vSafeHarbourCode As Object
    Private m_vSafeHarbourPercentage As Object
    Private m_vInsuredDomiciled As Object
    Private m_vInsuredPercentage As Object
    Private m_vInsuredTaxNumber As Object
    Private m_vPayeeDomiciled As Object
    Private m_vPayeePercentage As Object
    Private m_vPayeeTaxNumber As Object
    Private m_vIsTaxExempt As Object
    Private m_vIsWHTExempt As Object
    Private m_vIsSettlement As Object
    Private m_vCurrencyCode As Object
    Private m_vAmount As Object
    Private m_vExcessAmount As Object
    Private m_vPaymentAdjustment As Object
    Private m_vTaxArray As Object

    Public Property ProcessType() As Object
        Get
            Return m_vProcessType
        End Get
        Set(ByVal Value As Object)


            m_vProcessType = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property Payee() As Object
        Get
            Return m_vPayee
        End Get
        Set(ByVal Value As Object)


            m_vPayee = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property PaymentToCode() As Object
        Get
            Return m_vPaymentToCode
        End Get
        Set(ByVal Value As Object)


            m_vPaymentToCode = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property SafeHarbourCode() As Object
        Get
            Return m_vSafeHarbourCode
        End Get
        Set(ByVal Value As Object)


            m_vSafeHarbourCode = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property SafeHarbourPercentage() As Object
        Get
            Return m_vSafeHarbourPercentage
        End Get
        Set(ByVal Value As Object)


            m_vSafeHarbourPercentage = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property InsuredDomiciled() As Object
        Get
            Return m_vInsuredDomiciled
        End Get
        Set(ByVal Value As Object)


            m_vInsuredDomiciled = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property InsuredPercentage() As Object
        Get
            Return m_vInsuredPercentage
        End Get
        Set(ByVal Value As Object)


            m_vInsuredPercentage = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property InsuredTaxNumber() As Object
        Get
            Return m_vInsuredTaxNumber
        End Get
        Set(ByVal Value As Object)


            m_vInsuredTaxNumber = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property PayeeDomiciled() As Object
        Get
            Return m_vPayeeDomiciled
        End Get
        Set(ByVal Value As Object)


            m_vPayeeDomiciled = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property PayeePercentage() As Object
        Get
            Return m_vPayeePercentage
        End Get
        Set(ByVal Value As Object)


            m_vPayeePercentage = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property PayeeTaxNumber() As Object
        Get
            Return m_vPayeeTaxNumber
        End Get
        Set(ByVal Value As Object)


            m_vPayeeTaxNumber = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property IsTaxExempt() As Object
        Get
            Return m_vIsTaxExempt
        End Get
        Set(ByVal Value As Object)


            m_vIsTaxExempt = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property IsWHTExempt() As Object
        Get
            Return m_vIsWHTExempt
        End Get
        Set(ByVal Value As Object)


            m_vIsWHTExempt = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property IsSettlement() As Object
        Get
            Return m_vIsSettlement
        End Get
        Set(ByVal Value As Object)


            m_vIsSettlement = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property CurrencyCode() As Object
        Get
            Return m_vCurrencyCode
        End Get
        Set(ByVal Value As Object)


            m_vCurrencyCode = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property Amount() As Object
        Get
            Return m_vAmount
        End Get
        Set(ByVal Value As Object)


            m_vAmount = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property ExcessAmount() As Object
        Get
            Return m_vExcessAmount
        End Get
        Set(ByVal Value As Object)


            m_vExcessAmount = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property TaxArray() As Object
        Get
            Return m_vTaxArray
        End Get
        Set(ByVal Value As Object)


            m_vTaxArray = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property PaymentAdjustment() As Object
        Get
            Return m_vPaymentAdjustment
        End Get
        Set(ByVal Value As Object)


            m_vPaymentAdjustment = Value
        End Set
    End Property

    ' ***************************************************************** '
    ' Name: ArrayToData
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function ArrayToData(ByVal v_vDataArray() As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ArrayToData"

        Dim lReturn As Integer

        Try



        result = gPMConstants.PMEReturnCode.PMTrue



        m_vProcessType = v_vDataArray(kProcessType)


        m_vPayee = v_vDataArray(kPayee)


        m_vPaymentToCode = v_vDataArray(kPaymentToCode)


        m_vSafeHarbourCode = v_vDataArray(kSafeHarbourCode)


        m_vSafeHarbourPercentage = v_vDataArray(kSafeHarbourPercentage)


        m_vInsuredDomiciled = v_vDataArray(kInsuredDomiciled)


        m_vInsuredPercentage = v_vDataArray(kInsuredPercentage)


        m_vInsuredTaxNumber = v_vDataArray(kInsuredTaxNumber)


        m_vPayeeDomiciled = v_vDataArray(kPayeeDomiciled)


        m_vPayeePercentage = v_vDataArray(kPayeePercentage)


        m_vPayeeTaxNumber = v_vDataArray(kPayeeTaxNumber)


        m_vIsTaxExempt = v_vDataArray(kIsTaxExempt)


        m_vIsWHTExempt = v_vDataArray(kIsWHTExempt)


        m_vIsSettlement = v_vDataArray(kIsSettlement)


        m_vCurrencyCode = v_vDataArray(kCurrencyCode)


        m_vAmount = v_vDataArray(kAmount)


        m_vExcessAmount = v_vDataArray(kExcessAmount)


        m_vPaymentAdjustment = v_vDataArray(kPaymentAdjustment)

        ' array of tax band rates


        m_vTaxArray = v_vDataArray(kTaxArray)



        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally

'        Return result
'        Resume
'        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: DataToArray
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function DataToArray(ByRef v_vDataArray() As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DataToArray"

        Dim lReturn As Integer

        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        ReDim v_vDataArray(kErrorMessage)



        v_vDataArray(kProcessType) = m_vProcessType


        v_vDataArray(kPayee) = m_vPayee


        v_vDataArray(kPaymentToCode) = m_vPaymentToCode


        v_vDataArray(kSafeHarbourCode) = m_vSafeHarbourCode


        v_vDataArray(kSafeHarbourPercentage) = m_vSafeHarbourPercentage


        v_vDataArray(kInsuredDomiciled) = m_vInsuredDomiciled


        v_vDataArray(kInsuredPercentage) = m_vInsuredPercentage


        v_vDataArray(kInsuredTaxNumber) = m_vInsuredTaxNumber


        v_vDataArray(kPayeeDomiciled) = m_vPayeeDomiciled


        v_vDataArray(kPayeePercentage) = m_vPayeePercentage


        v_vDataArray(kPayeeTaxNumber) = m_vPayeeTaxNumber


        v_vDataArray(kIsTaxExempt) = m_vIsTaxExempt


        v_vDataArray(kIsWHTExempt) = m_vIsWHTExempt


        v_vDataArray(kIsSettlement) = m_vIsSettlement


        v_vDataArray(kCurrencyCode) = m_vCurrencyCode


        v_vDataArray(kAmount) = m_vAmount


        v_vDataArray(kExcessAmount) = m_vExcessAmount


        v_vDataArray(kPaymentAdjustment) = m_vPaymentAdjustment

        ' array of tax band rates


        v_vDataArray(kTaxArray) = m_vTaxArray

        v_vDataArray(kErrorMessage) = ""


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally

'        Return result
'        Resume
'        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: CompareTaxArrays
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 30-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function CompareTaxArrays(ByVal v_vOrigTaxArray(,) As Object, ByRef r_vUpdatedTaxArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CompareTaxArrays"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lOrigTaxLBound, lOrigTaxUBound, lUpdatedTaxLBound, lUpdatedTaxUBound As Integer
        Dim bOrigTaxIsArray, bUpdatedTaxIsArray As Boolean
        Dim lProcessType, lCompareTaxUBound As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            bOrigTaxIsArray = Information.IsArray(v_vOrigTaxArray)
            bUpdatedTaxIsArray = Information.IsArray(r_vUpdatedTaxArray)

            ' get the original array boundaries
            If bOrigTaxIsArray Then
                lOrigTaxLBound = v_vOrigTaxArray.GetLowerBound(1)
                lOrigTaxUBound = v_vOrigTaxArray.GetUpperBound(1)
            End If

            ' get the updated array boundaries
            If bUpdatedTaxIsArray Then

                lUpdatedTaxLBound = r_vUpdatedTaxArray.GetLowerBound(1)

                lUpdatedTaxUBound = r_vUpdatedTaxArray.GetUpperBound(1)
            End If

            ' determine what process needs to take place
            If bOrigTaxIsArray And bUpdatedTaxIsArray Then
                ' do compare
                lProcessType = 1
            ElseIf bOrigTaxIsArray And Not bUpdatedTaxIsArray Then
                ' no taxes to compare
                lProcessType = 2
            ElseIf Not bOrigTaxIsArray And bUpdatedTaxIsArray Then
                ' mark all as manually changed
                lProcessType = 3
            End If


            Select Case lProcessType
                Case 1

                    ' Compare Original To Updated
                    If lUpdatedTaxUBound > lOrigTaxUBound Then
                        lCompareTaxUBound = lOrigTaxUBound
                    ElseIf lOrigTaxUBound > lUpdatedTaxUBound Then
                        lCompareTaxUBound = lUpdatedTaxUBound
                    End If

                    ' Compare Matching Array Items
                    lReturn = CType(CompareArrayItems(v_vOrigTaxArray, r_vUpdatedTaxArray, lCompareTaxUBound), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "CompareArrayItems", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ' mark any additional tax items as manually changed
                    If lCompareTaxUBound < lUpdatedTaxUBound Then

                        ' mark all other items as changed by script
                        For lTaxItem As Integer = lCompareTaxUBound + 1 To lUpdatedTaxUBound

                            r_vUpdatedTaxArray(kTaxArrayIsManuallyChanged, lTaxItem) = kIsManuallyChangedScript
                        Next

                    End If

                Case 2
                    ' do nothing

                Case 3
                    ' mark all as changed by script
                    For lTaxItem As Integer = lUpdatedTaxLBound To lUpdatedTaxUBound

                        r_vUpdatedTaxArray(kTaxArrayIsManuallyChanged, lTaxItem) = kIsManuallyChangedScript
                    Next

            End Select


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: CompareArrayItems
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 30-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function CompareArrayItems(ByVal v_vOrigTaxArray(,) As Object, ByRef r_vUpdatedTaxArray(,) As Object, ByVal v_lNoOfItemsToCompare As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CompareArrayItems"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' for each tax item
            For lTaxItem As Integer = 0 To v_lNoOfItemsToCompare

                ' if any items doesnt match the original














                If Not v_vOrigTaxArray(kTaxArrayTaxGroupId, lTaxItem).Equals(r_vUpdatedTaxArray(kTaxArrayTaxGroupId, lTaxItem)) Or Not v_vOrigTaxArray(kTaxArrayTaxBandId, lTaxItem).Equals(r_vUpdatedTaxArray(kTaxArrayTaxBandId, lTaxItem)) Or Not v_vOrigTaxArray(kTaxArrayTaxCurrencyCode, lTaxItem).Equals(r_vUpdatedTaxArray(kTaxArrayTaxCurrencyCode, lTaxItem)) Or Not v_vOrigTaxArray(kTaxArrayPercentage, lTaxItem).Equals(r_vUpdatedTaxArray(kTaxArrayPercentage, lTaxItem)) Or Not v_vOrigTaxArray(kTaxArrayValue, lTaxItem).Equals(r_vUpdatedTaxArray(kTaxArrayValue, lTaxItem)) Or Not v_vOrigTaxArray(kTaxArrayIsValue, lTaxItem).Equals(r_vUpdatedTaxArray(kTaxArrayIsValue, lTaxItem)) Or Not v_vOrigTaxArray(kTaxArrayClassOfBusinessId, lTaxItem).Equals(r_vUpdatedTaxArray(kTaxArrayClassOfBusinessId, lTaxItem)) Then

                    ' mark it as changed by script

                    r_vUpdatedTaxArray(kTaxArrayIsManuallyChanged, lTaxItem) = kIsManuallyChangedScript

                End If

            Next


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function
End Class
