Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Public Module GeminiNetFunctions
    ' ***************************************************************** '
    ' Module Name: GeminiNetFunctions
    '
    ' Date:  04/10/2001
    '
    ' Description: Main Module.
    '
    ' Edit History:
    '
    ' 041001 CJB - Created as needed a place to add common functions used
    '              by different components. The functions use early
    '              binding to cGISDatasetControl.dll and so could not be
    '              added in to an existing module since components
    '              without an explicit ref to DatasetControl would not
    '              compile.
    ' ***************************************************************** '


    Private Const ACClass As String = "GeminiNetFunctions"


    ' ***************************************************************** '
    '
    ' Name: GetPrimaryRiskData
    '
    ' Description:  Get main risk data in a string to send in emails etc.
    '
    ' History:      040901 CJB - Created.
    '
    ' ***************************************************************** '
    Public Function GetPrimaryRiskData(ByVal v_sGisDataModelCode As String, ByRef r_sPrimaryRiskData As String, ByRef r_oDataset As Object) As Integer

        Dim result As Integer = 0
        Try

            Dim sPolicyObject, sCoverObject, sProposerObject, sDriverObject, sVehicleObject, sNPPremiumFinanceObject, sNCDObject, sPaymentBankObject, sSex, sTelNoHome, sEmail, sClaims, sConvictions, sSpacer, sCover, sNCDYears, sProtected, sDrivers, sClassOfUse, sAddressLine3, sTime As String
            Const CONST_I4M_PREFIX As String = "I4M"

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set object names 1st
            sPolicyObject = "POLICY"
            sCoverObject = "COVER"
            sDriverObject = "DRIVER"
            sVehicleObject = "VEHICLE"
            sNPPremiumFinanceObject = "NP_PREMIUM_FINANCE"
            sNCDObject = "NCD"
            sPaymentBankObject = "PAYMENT_BANK"

            If v_sGisDataModelCode.ToUpper() = CONST_I4M_PREFIX Then
                sProposerObject = "PROPOSER"
            Else
                sProposerObject = "PROPOSER_POLICYHOLDER"
            End If

            'Add its4me prefix if required
            If v_sGisDataModelCode.ToUpper() = CONST_I4M_PREFIX Then
                sPolicyObject = CONST_I4M_PREFIX & "_" & sPolicyObject
                sCoverObject = CONST_I4M_PREFIX & "_" & sCoverObject
                sDriverObject = CONST_I4M_PREFIX & "_" & sDriverObject
                sVehicleObject = CONST_I4M_PREFIX & "_" & sVehicleObject
                sProposerObject = CONST_I4M_PREFIX & "_" & sProposerObject
                sNPPremiumFinanceObject = CONST_I4M_PREFIX & "_" & sNPPremiumFinanceObject
                sNCDObject = CONST_I4M_PREFIX & "_" & sNCDObject
                sPaymentBankObject = CONST_I4M_PREFIX & "_PAYMENT_AND_BANK"
            End If

            sSex = r_oDataset.Risk.Item(sProposerObject).Item("Sex").Value
            If sSex = "M" Or sSex = "m" Then
                sSex = "Male"
            Else
                If sSex = "F" Or sSex = "f" Then
                    sSex = "Female"
                End If
            End If

            sAddressLine3 = r_oDataset.Risk.Item(sProposerObject).Item("Address_line_3").Value
            sTelNoHome = r_oDataset.Risk.Item(sProposerObject).Item("tel_no_home").Value
            sEmail = r_oDataset.Risk.Item(sProposerObject).Item("email").Value
            sClaims = r_oDataset.Risk.Item(sDriverObject, 1).Item("Claims_Ind").Value
            sConvictions = r_oDataset.Risk.Item(sDriverObject, 1).Item("Convictions_Ind").Value

            r_sPrimaryRiskData = "Proposer/Driver 1" & Strings.Chr(13) & Strings.Chr(10) & "-----------------" & Strings.Chr(13) & Strings.Chr(10)
            r_sPrimaryRiskData = r_sPrimaryRiskData & r_oDataset.Risk.Item(sProposerObject).Item("Forename_initial_1").Value & " "
            r_sPrimaryRiskData = r_sPrimaryRiskData & r_oDataset.Risk.Item(sProposerObject).Item("Surname").Value
            r_sPrimaryRiskData = r_sPrimaryRiskData & " (" & sSex & ", born on "
            r_sPrimaryRiskData = r_sPrimaryRiskData & r_oDataset.Risk.Item(sProposerObject).Item("Date_of_birth").Value & " )" & Strings.Chr(13) & Strings.Chr(10)
            r_sPrimaryRiskData = r_sPrimaryRiskData & r_oDataset.Risk.Item(sProposerObject).Item("Address_line_1").Value & ", "
            r_sPrimaryRiskData = r_sPrimaryRiskData & r_oDataset.Risk.Item(sProposerObject).Item("Address_line_2").Value & ", "

            If sAddressLine3.Trim() <> "" Then
                r_sPrimaryRiskData = r_sPrimaryRiskData & sAddressLine3 & ", "
            End If

            r_sPrimaryRiskData = r_sPrimaryRiskData & r_oDataset.Risk.Item(sProposerObject).Item("Address_post_code").Value & Strings.Chr(13) & Strings.Chr(10)

            If sTelNoHome.Trim() <> "" Then
                r_sPrimaryRiskData = r_sPrimaryRiskData & sTelNoHome
                sSpacer = "   "
            Else
                sSpacer = ""
            End If

            If sEmail.Trim() <> "" Then
                r_sPrimaryRiskData = r_sPrimaryRiskData & sSpacer & sEmail
            End If

            Select Case sClaims
                Case "Y", "y"
                    sClaims = "Claims have been entered, "
                Case "N", "n"
                    sClaims = "No Claims, "
                Case Else
                    sClaims = "Unknown if claims or not, "
            End Select

            Select Case sConvictions
                Case "Y", "y"
                    sConvictions = "Convictions have been entered"
                Case "N", "n"
                    sConvictions = "No Convictions"
                Case Else
                    sConvictions = "Unknown if convictions or not"
            End Select

            sProtected = r_oDataset.Risk.Item(sNCDObject).Item("Claimed_protection_reqd_ind").Value
            If sProtected = "Y" Or sProtected = "y" Then
                sProtected = " (Protected)"
            Else
                sProtected = ""
            End If

            sNCDYears = r_oDataset.Risk.Item(sNCDObject).Item("Claimed_years_earned").Value
            sNCDYears = " with " & sNCDYears & " years NCD"

            sCover = r_oDataset.Risk.Item(sCoverObject).Item("Code").Value
            Select Case sCover
                Case "01"
                    sCover = "Fully Comprehensive" & sNCDYears & sProtected
                Case "02"
                    sCover = "Third Party, Fire and Theft" & sNCDYears & sProtected
                Case Else

            End Select

            sDrivers = r_oDataset.Risk.Item(sCoverObject).Item("Required_Drivers").Value
            Select Case sDrivers
                Case "1"
                    sDrivers = "Cover is for insured only"
                Case "2"
                    sDrivers = "Cover is for insured and spouse"
                Case "5"
                    sDrivers = "Cover is for insured and one named driver"
                Case "B"
                    sDrivers = "Cover is for insured and two named drivers"
                Case "C"
                    sDrivers = "Cover is for insured and three named drivers"
                Case Else
                    sDrivers = "Cover is for " & sDrivers
            End Select

            sClassOfUse = r_oDataset.Risk.Item(sVehicleObject).Item("Class_of_use").Value
            Select Case sClassOfUse
                Case "04"
                    sClassOfUse = "Class of use is Social, Domestic and Pleasure"
                Case "01"
                    sClassOfUse = "Class of use is Social, Domestic and Pleasure + Business use"
                Case "03"
                    sClassOfUse = "Class of use is Social, Domestic and Pleasure + Business use by policyholder & domestic partner"
                Case "16"
                    sClassOfUse = "Class of use is Social, Domestic and Pleasure + Business use by named drivers"
                Case Else
                    sClassOfUse = "Class of use is " & sClassOfUse
            End Select

            sTime = r_oDataset.Risk.Item(sPolicyObject).Item("Effective_start_time").Value
            sTime = sTime.Substring(0, 2) & ":" & Mid(sTime, 3, 2) & ":" & sTime.Substring(sTime.Length - 2)

            r_sPrimaryRiskData = r_sPrimaryRiskData & Strings.Chr(13) & Strings.Chr(10) & sClaims
            r_sPrimaryRiskData = r_sPrimaryRiskData & sConvictions & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)
            r_sPrimaryRiskData = r_sPrimaryRiskData & "Vehicle" & Strings.Chr(13) & Strings.Chr(10) & "-------" & Strings.Chr(13) & Strings.Chr(10)
            r_sPrimaryRiskData = r_sPrimaryRiskData & r_oDataset.Risk.Item(sVehicleObject).Item("Reg_No").Value & "  "
            r_sPrimaryRiskData = r_sPrimaryRiskData & r_oDataset.Risk.Item(sVehicleObject).Item("Model_name").Value
            r_sPrimaryRiskData = r_sPrimaryRiskData & " manufactured on " & r_oDataset.Risk.Item(sVehicleObject).Item("Date_Manufactured").Value
            r_sPrimaryRiskData = r_sPrimaryRiskData & " valued at £" & r_oDataset.Risk.Item(sVehicleObject).Item("value").Value
            r_sPrimaryRiskData = r_sPrimaryRiskData & " covering " & r_oDataset.Risk.Item(sVehicleObject).Item("Annual_Mileage").Value & " miles per year"
            r_sPrimaryRiskData = r_sPrimaryRiskData & " and kept overnight at " & r_oDataset.Risk.Item(sVehicleObject).Item("Post_Code").Value & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)
            r_sPrimaryRiskData = r_sPrimaryRiskData & "Cover" & Strings.Chr(13) & Strings.Chr(10) & "-----" & Strings.Chr(13) & Strings.Chr(10)
            r_sPrimaryRiskData = r_sPrimaryRiskData & sCover & Strings.Chr(13) & Strings.Chr(10)
            r_sPrimaryRiskData = r_sPrimaryRiskData & sDrivers & Strings.Chr(13) & Strings.Chr(10)
            r_sPrimaryRiskData = r_sPrimaryRiskData & sClassOfUse & Strings.Chr(13) & Strings.Chr(10)
            r_sPrimaryRiskData = r_sPrimaryRiskData & "Covered from " & r_oDataset.Risk.Item(sPolicyObject).Item("Effective_start_date").Value & " "
            r_sPrimaryRiskData = r_sPrimaryRiskData & sTime & Strings.Chr(13) & Strings.Chr(10)
            r_sPrimaryRiskData = r_sPrimaryRiskData & "Datacash issuer - " & r_oDataset.Risk.Item(sPolicyObject).Item("NP_Datacash_Issuer").Value & Strings.Chr(13) & Strings.Chr(10)
            r_sPrimaryRiskData = r_sPrimaryRiskData & r_oDataset.Risk.Item(sPolicyObject).Item("NP_Insurer_Name").Value
            r_sPrimaryRiskData = r_sPrimaryRiskData & " (" & r_oDataset.Risk.Item(sPolicyObject).Item("NP_Insurer_No").Value & ")"

            If v_sGisDataModelCode.ToUpper() = CONST_I4M_PREFIX Then
                r_sPrimaryRiskData = r_sPrimaryRiskData & "  -  Scheme id " & r_oDataset.Risk.Item(sPolicyObject).Item("NP_Transacted_Scheme_Id").Value & Strings.Chr(13) & Strings.Chr(10)
            Else
                r_sPrimaryRiskData = r_sPrimaryRiskData & "  -  Scheme id " & r_oDataset.Risk.Item(sPolicyObject).Item("NP_GIS_Scheme_Id_At_Quote").Value & Strings.Chr(13) & Strings.Chr(10)
            End If

            r_sPrimaryRiskData = r_sPrimaryRiskData & "Premium £" & r_oDataset.Risk.Item(sNPPremiumFinanceObject).Item("PF_Total_Premium").Value

            If v_sGisDataModelCode.ToUpper() = CONST_I4M_PREFIX Then
                r_sPrimaryRiskData = r_sPrimaryRiskData & " (paid by " & r_oDataset.Risk.Item(sPolicyObject).Item("NP_Payment_Method").Value & ")"
            Else
                r_sPrimaryRiskData = r_sPrimaryRiskData & " (paid by " & r_oDataset.Risk.Item(sPaymentBankObject).Item("INSR_PMT_METHOD_TYPE").Value & ")"
            End If
            r_sPrimaryRiskData = r_sPrimaryRiskData & " with a voluntary excess of £" & r_oDataset.Risk.Item(sCoverObject).Item("Vol_XS_Amt").Value & Strings.Chr(13) & Strings.Chr(10)

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPrimaryRiskData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPrimaryRiskData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: SendInformationEmail
    '
    ' Description:  Send an error email for use with, initially, attempted
    '               transacts.
    '
    ' History:      040901 CJB - Created.
    '
    '
    ' ***************************************************************** '
    Public Function SendInformationEmail(ByVal v_sEmailMessage As String, ByVal v_sAttatchment As String, ByVal v_sDataModelCode As String, ByVal v_sBusinessTypeCode As String, ByVal v_sQuoteReference As String, ByVal v_sEmailTitle As String, ByVal v_sIncludeRiskDataFlag As String, ByRef r_oDataset As Object) As Integer
        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Dim sInformationEmailAddress, sInformationEmailFrom As String
        Dim oCDONTS As Object
        Dim sRiskDataFile As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode

        ' Get the I4M Information Email Address from the registry
        lReturn = CType(GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sDataModelCode, v_sSettingName:=GISSharedConstants.GISRegInformationEmailAddress, r_sSettingValue:=sInformationEmailAddress, v_sBusinessTypeCode:=v_sBusinessTypeCode, v_sSubKey:=GISSharedConstants.GISRegSubKeyEmails), gPMConstants.PMEReturnCode)
        'If can't find registry entry or it is not set then exit
        If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or sInformationEmailAddress = "" Then
            Return result
        End If

        ' Get the I4M Information Email FROM Address from the registry
        lReturn = CType(GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sDataModelCode, v_sSettingName:=GISSharedConstants.GISRegInformationEmailFrom, r_sSettingValue:=sInformationEmailFrom, v_sBusinessTypeCode:=v_sBusinessTypeCode, v_sSubKey:=GISSharedConstants.GISRegSubKeyEmails), gPMConstants.PMEReturnCode)
        'If can't find registry entry or it is not set then exit
        If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or sInformationEmailFrom = "" Then
            Return result
        End If

        'oCDONTS = New CDONTS.NewMail()
        oCDONTS = New Object()
        If Not (oCDONTS Is Nothing) Then


            oCDONTS.To = sInformationEmailAddress

            oCDONTS.From = sInformationEmailFrom

            oCDONTS.Subject = v_sEmailTitle

            oCDONTS.Body = v_sEmailMessage

            'CJB040901 Save the risk data to a file, attach it, send the email and delete the file
            'Add its4me prefix if required
            'CJB011001 Only do this if flagged to do so
            If v_sIncludeRiskDataFlag = "Y" Or v_sIncludeRiskDataFlag = "y" Then
                sRiskDataFile = "C:\Program Files\PM" & "\TempRiskData" & v_sQuoteReference & ".xml"
                If r_oDataset.SaveXMLToFile(, sRiskDataFile) = gPMConstants.PMEReturnCode.PMTrue Then

                    oCDONTS.AttachFile(sRiskDataFile)
                End If
            End If


            oCDONTS.send()
            oCDONTS = Nothing

            'CJB040901 Delete the temp risk data file
            'CJB011001 Only do this if flagged to do so
            If v_sIncludeRiskDataFlag = "Y" Or v_sIncludeRiskDataFlag = "y" Then
                Try
                    File.Delete(sRiskDataFile)

                Catch
                End Try


            End If

        End If

        Return result

Err_SendInformationEmail:

        ' Log Error Message
        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SendInformationEmail Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SendInformationEmail", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: SendErrorEmail
    '
    ' Description:  Send an error email for use with, initially, Transact
    '               failures.
    '
    ' History:      020801 CJB - Created.
    '               040901 CJB - Send a file attachment of the risk data too
    '               011001 CJB - Pass in email title
    '
    ' ***************************************************************** '
    Public Function SendErrorEmail(ByVal v_sEmailMessage As String, ByVal v_sAttatchment As String, ByVal v_sDataModelCode As String, ByVal v_sBusinessTypeCode As String, ByVal v_sQuoteReference As String, ByVal v_sEmailTitle As String, ByRef r_oDataset As Object) As Integer
        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue

        Dim sErrorsEmailAddress, sErrorsEmailFrom As String
        Dim oCDONTS As Object
        Dim sRiskDataFile As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode

        ' Get the I4M Errors Email Address from the registry
        lReturn = CType(GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sDataModelCode, v_sSettingName:=GISSharedConstants.GISRegErrorsEmailAddress, r_sSettingValue:=sErrorsEmailAddress, v_sBusinessTypeCode:=v_sBusinessTypeCode, v_sSubKey:=GISSharedConstants.GISRegSubKeyEmails), gPMConstants.PMEReturnCode)
        'If can't find registry entry or it is not set then exit
        If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or sErrorsEmailAddress = "" Then
            Return result
        End If

        ' Get the I4M Errors Email FROM Address from the registry
        lReturn = CType(GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sDataModelCode, v_sSettingName:=GISSharedConstants.GISRegErrorsEmailFrom, r_sSettingValue:=sErrorsEmailFrom, v_sBusinessTypeCode:=v_sBusinessTypeCode, v_sSubKey:=GISSharedConstants.GISRegSubKeyEmails), gPMConstants.PMEReturnCode)
        'If can't find registry entry or it is not set then exit
        If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or sErrorsEmailFrom = "" Then
            Return result
        End If

        'oCDONTS = New CDONTS.NewMail()
        oCDONTS = New Object()
        If Not (oCDONTS Is Nothing) Then


            oCDONTS.To = sErrorsEmailAddress

            oCDONTS.From = sErrorsEmailFrom

            oCDONTS.Subject = v_sEmailTitle

            oCDONTS.Body = v_sEmailMessage

            'If a nnnnMSG1.DAT Cobol error file exists then attach it to the email
            If v_sAttatchment <> "" Then

                oCDONTS.AttachFile(v_sAttatchment)
            End If

            'CJB040901 Save the risk data to a file, attach it, send the email and delete the file
            sRiskDataFile = "C:\Program Files\PM" & "\TempRiskData" & v_sQuoteReference & ".xml"
            If r_oDataset.SaveXMLToFile(, sRiskDataFile) = gPMConstants.PMEReturnCode.PMTrue Then

                oCDONTS.AttachFile(sRiskDataFile)
            End If


            oCDONTS.send()
            oCDONTS = Nothing

            'CJB040901 Delete the temp risk data file
            Try
                File.Delete(sRiskDataFile)

            Catch
            End Try



        End If

        Return result

Err_SendErrorEmail:

        ' Log Error Message
        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SendErrorEmail Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SendErrorEmail", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result
    End Function
End Module