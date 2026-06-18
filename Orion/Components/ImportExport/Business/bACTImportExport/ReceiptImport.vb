Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Xml
'Developer Guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("ReceiptImport_NET.ReceiptImport")> _
Public NotInheritable Class ReceiptImport
    Implements PreviewBase


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "ReceiptImport"

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

    Private m_oDatabase As dPMDAO.Database
    Private m_oAccount As Object


    ' ***************************************************************** '
    '                 PREVIEWBASE IMPLEMENTED METHODS
    ' ***************************************************************** '
    ' Returns a preview of the supplied xml
    Private Function PreviewBase_GetPreview(ByVal oRoot As XmlNode, ByRef vHeader() As Object, ByRef vDetail(,) As Object) As Integer Implements PreviewBase.GetPreview

        Dim result As Integer = 0
        Dim lRow As Integer

        Dim lReturn As Integer
        Const kMethodName As String = "PreviewBase_GetPreview"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get header info
        ReDim vHeader(8)


        vHeader(0) = TryGetAttribute(oRoot, "batch_reference")


        vHeader(1) = TryGetAttribute(oRoot, "bankaccount_code")


        vHeader(2) = TryGetAttribute(oRoot, "date")


        vHeader(3) = TryGetAttribute(oRoot, "currency_code")

        vHeader(4) = 0 ' Actual total receipt


        vHeader(5) = TryGetAttribute(oRoot, "record_total") ' Expected total

        vHeader(6) = oRoot.ChildNodes.Count ' Actual record count


        vHeader(7) = TryGetAttribute(oRoot, "record_count") ' Expected record count

        vHeader(8) = 0 ' Invalid record count

        ' Prepare record array
        ReDim vDetail(6, oRoot.ChildNodes.Count - 1)
        lRow = 0

        ' Get row info
        For Each oNode As XmlNode In oRoot.ChildNodes
            ' Get details


            vDetail(0, lRow) = TryGetAttribute(oNode, "account_code")


            vDetail(1, lRow) = TryGetAttribute(oNode, "amount")


            vDetail(2, lRow) = TryGetAttribute(oNode, "mediatype_code")


            vDetail(3, lRow) = TryGetAttribute(oNode, "media_reference")


            vDetail(4, lRow) = TryGetAttribute(oNode, "original_account_code")


            vDetail(5, lRow) = TryGetAttribute(oNode, "policy_reference")


            vDetail(6, lRow) = TryGetAttribute(oNode, "error_message")

            ' Keep track of receipt total



            vHeader(4) = CDbl(vHeader(4)) + gPMFunctions.ToSafeCurrency(CStr(vDetail(1, lRow)))

            ' Validate account code

            If ValidateAccount(CStr(vDetail(0, lRow))) <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Account code is invalid retain invalid code (unless we already have one)

                If gPMFunctions.ToSafeString(CStr(vDetail(4, lRow))).Length = 0 Then


                    vDetail(4, lRow) = vDetail(0, lRow)
                End If

                ' Increment invalid counter


                vHeader(8) = CDbl(vHeader(8)) + 1
            End If

            ' Next record
            lRow += 1
        Next oNode

        Return result
    End Function

    Private Function PreviewBase_Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer Implements PreviewBase.Initialise

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

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

        m_oDatabase = vDatabase

        Return result

    End Function

    ' Updates the supplied xml with any changes
    Private Function PreviewBase_Update(ByVal oRoot As XmlNode, ByRef vHeader() As Object, ByRef vDetail(,) As Object) As Integer Implements PreviewBase.Update

        Dim result As Integer = 0
        Dim lRow As Integer

        Dim lReturn As Integer
        Const kMethodName As String = "PreviewBase_Update"


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Fundamental validation: Have we got the same amount of records?
        If oRoot.ChildNodes.Count <> (vDetail.GetUpperBound(1) - vDetail.GetLowerBound(1) + 1) Then
            result = gPMConstants.PMEReturnCode.PMDataChanged
            Return result
        End If

        ' Update header info

        TrySetAttribute(oRoot, "record_total", CStr(gPMFunctions.ToSafeDouble(vHeader(5)))) ' Expected total

        TrySetAttribute(oRoot, "record_count", CStr(gPMFunctions.ToSafeLong(vHeader(7)))) ' Expected record count

        lRow = 0

        ' Get row info
        For Each oNode As XmlNode In oRoot.ChildNodes
            ' Do a little bit of validation here to ensure the file hasn't been updated in the meantime


            If CStr(vDetail(1, lRow)) <> TryGetAttribute(oNode, "amount") Then
                ' Amounts don't match data is incorrect
                result = gPMConstants.PMEReturnCode.PMDataChanged
                Return result
            End If

            ' Amounts are okay, check codes.


            If gPMFunctions.ToSafeString(CStr(vDetail(4, lRow))) <> gPMFunctions.ToSafeString(TryGetAttribute(oNode, "original_account_code")) Then
                ' Original code has changed update


                TrySetAttribute(oNode, "account_code", CStr(vDetail(0, lRow)))


                TrySetAttribute(oNode, "original_account_code", CStr(vDetail(4, lRow)))
            Else
                ' Check that the original code hasn't changed as well


                If gPMFunctions.ToSafeString(TryGetAttribute(oNode, "account_code")) <> gPMFunctions.ToSafeString(CStr(vDetail(0, lRow))) Then
                    ' Account codes don't match data is incorrect


                    TrySetAttribute(oNode, "account_code", CStr(vDetail(0, lRow)))
                    result = gPMConstants.PMEReturnCode.PMDataChanged
                    Return result
                End If
            End If

            ' Next record
            lRow += 1
        Next oNode


        Return result
    End Function


    ' ***************************************************************** '
    '                          PRIVATE METHODS
    ' ***************************************************************** '
    Private Function ValidateAccount(ByVal sAccountCode As String) As Integer

        Dim result As Integer = 0
        Dim lAccountID As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "ValidateAccount"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get business
        If m_oAccount Is Nothing Then
            ' Create account object
            lReturn = CType(gPMComponentServices.CreateBusinessObject(m_oAccount, "bACTAccount.Form", m_sCallingAppName, m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_oDatabase), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("CreateBusinessObject", "Unable to create bACTAccount.Business")
            End If
        End If

        ' Check for valid account

        lReturn = m_oAccount.GetAccountID(v_sShortCode:=sAccountCode, r_lAccountID:=lAccountID)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
        ElseIf lAccountID <= 0 Then
            result = gPMConstants.PMEReturnCode.PMFalse
        End If


        Return result
    End Function
End Class