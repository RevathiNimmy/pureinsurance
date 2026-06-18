Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.IO
Imports System.Text
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: PFExport
    '
    ' Date: 13-Aug-2001
    '
    ' Description: Describes the PFExport attributes.
    '
    ' Edit History:
    ' CJB 24/03/2005 PN17702 Changed GetInstalmentsForBatch to fetch all records rather than default 500
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' ************************************************
    ' Added to replace global variables 13/01/2004
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************
    ' Current Record Pointer
    Private m_lCurrentRecord As Integer
    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    ' Database Class
    Private m_oDatabase As dPMDAO.Database
    ' Close Database Flag
    Private m_bCloseDatabase As Boolean
    ' DD 18/06/2003: Added for new Instalments
    Private m_oSpoke As bACTFinanceSpoke.Business
    ' DataBase Attributes
    Private m_lPFPaymentMethod_cnt As Integer
    Private m_sDescription As String = ""
    Private m_sDirectory As String = ""
    Private m_sFilename As String = ""
    Private m_sHeader As String = ""
    Private m_sDetail As String = ""
    Private m_sFooter As String = ""
    Private m_sDelimeter As String = ""
    Private m_bASCIIValue As Boolean
    Private m_bAmountInPence As Boolean
    Private m_sQuoteCharacter As String = ""
    Private m_bQuotedNumerics As Boolean
    Private m_bQuotedStrings As Integer
    Private m_bAccountNumbersOnly As Boolean
    Private m_sDateFormat As String = ""
    Private m_bAllowAutoPost As Boolean
    Private m_bDisableExport As Boolean
    Private m_sBankShortCode As String = ""
    Private m_lNextBatchNumber As Integer
    Private m_bExcludeAudis As Boolean 'MKW160204 PN10300

    ' Function Return Code
    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_bDataChanged As Boolean
    Private m_oLookup As bPMLookup.Business

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property
    Public WriteOnly Property Database() As dPMDAO.Database
        Set(ByVal Value As dPMDAO.Database)
            m_oDatabase = Value
        End Set
    End Property
    Public Property PFPaymentMethod_cnt() As Integer
        Get
            Return m_lPFPaymentMethod_cnt
        End Get
        Set(ByVal Value As Integer)
            m_lPFPaymentMethod_cnt = Value

            'Load up the object properties
            m_lReturn = CType(SelectSingle(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.DivideByZeroException("11, PFPaymentMethod_cnt Let, Payment Method cannot be loaded.")
            End If
        End Set
    End Property

    Public ReadOnly Property Description() As String
        Get
            Return m_sDescription
        End Get
    End Property

    Public Property Directory() As String
        Get
            Return m_sDirectory
        End Get
        Set(ByVal Value As String)
            m_sDirectory = Value

            'append a back slash by default
            If Not Value.EndsWith("\") Then
                m_sDirectory = m_sDirectory & "\"
            End If
        End Set
    End Property

    Public Property Filename() As String
        Get
            Return m_sFilename
        End Get
        Set(ByVal Value As String)
            m_sFilename = Value
        End Set
    End Property

    Public Property Header() As String
        Get
            Return m_sHeader
        End Get
        Set(ByVal Value As String)
            m_sHeader = Value
        End Set
    End Property

    Public Property Detail() As String
        Get
            Return m_sDetail
        End Get
        Set(ByVal Value As String)
            m_sDetail = Value
        End Set
    End Property

    Public Property Footer() As String
        Get
            Return m_sFooter
        End Get
        Set(ByVal Value As String)
            m_sFooter = Value
        End Set
    End Property

    Public Property Delimeter() As String
        Get
            Return m_sDelimeter
        End Get
        Set(ByVal Value As String)
            m_sDelimeter = Value
        End Set
    End Property

    Public Property ASCIIValue() As Boolean
        Get
            Return m_bASCIIValue
        End Get
        Set(ByVal Value As Boolean)
            m_bASCIIValue = Value
        End Set
    End Property

    Public Property AmountInPence() As Boolean
        Get
            Return m_bAmountInPence
        End Get
        Set(ByVal Value As Boolean)
            m_bAmountInPence = Value
        End Set
    End Property

    Public Property QuoteCharacter() As String
        Get
            Return m_sQuoteCharacter
        End Get
        Set(ByVal Value As String)
            m_sQuoteCharacter = Value
        End Set
    End Property

    Public Property QuotedNumerics() As Boolean
        Get
            Return m_bQuotedNumerics
        End Get
        Set(ByVal Value As Boolean)
            m_bQuotedNumerics = Value
        End Set
    End Property

    Public Property QuotedStrings() As Boolean
        Get
            Return m_bQuotedStrings
        End Get
        Set(ByVal Value As Boolean)
            m_bQuotedStrings = Value
        End Set
    End Property

    Public Property AccountNumbersOnly() As Boolean
        Get
            Return m_bAccountNumbersOnly
        End Get
        Set(ByVal Value As Boolean)
            m_bAccountNumbersOnly = Value
        End Set
    End Property

    Public Property DateFormat() As String
        Get
            Return m_sDateFormat
        End Get
        Set(ByVal Value As String)
            m_sDateFormat = Value
        End Set
    End Property

    Public Property AllowAutoPost() As Boolean
        Get
            Return m_bAllowAutoPost
        End Get
        Set(ByVal Value As Boolean)
            m_bAllowAutoPost = Value
        End Set
    End Property

    Public Property DisableExport() As Boolean
        Get
            Return m_bDisableExport
        End Get
        Set(ByVal Value As Boolean)
            m_bDisableExport = Value
        End Set
    End Property

    Public Property BankShortCode() As String
        Get
            Return m_sBankShortCode
        End Get
        Set(ByVal Value As String)
            m_sBankShortCode = Value
        End Set
    End Property

    Public Property NextBatchNumber() As Integer
        Get
            Return m_lNextBatchNumber
        End Get
        Set(ByVal Value As Integer)
            'You cannot set the Batch Number to lower than the existing number
            If Value > m_lNextBatchNumber Then
                m_lNextBatchNumber = Value
            End If
        End Set
    End Property

    'MKW160204 PN10300.  Extra methods for Exclude Audis Option START
    Public Property ExcludeAudis() As Boolean
        Get
            Return m_bExcludeAudis
        End Get
        Set(ByVal Value As Boolean)
            m_bExcludeAudis = Value
        End Set
    End Property

    Public Function GetInstalmentsForBatch(ByRef r_lBatchID As Integer, ByVal lLeadDays As Integer, ByVal sMediaTypeCode As String, ByVal bArrayOnly As Boolean, ByVal bForRecall As Boolean, ByRef r_vResultArray As Object, Optional ByVal v_sDocumentRef As String = "") As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetInstalmentsForBatch
        ' PURPOSE: Gets an array of Instalments due to be exported. If the ArrayOnly
        ' parameter is false then the Spoke generates a new Batch
        ' AUTHOR: Danny Davis
        ' DATE: 18 June 2003, 16:03:06
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim vHeaderArraySub(ehdArrayOnly) As Object
        Dim vHeaderArray(1) As Object
        Dim vResultArray(,) As Object
        Dim sStatusCode As String = String.Empty
        Dim sMessage As String = String.Empty
        Dim sBatchRef As String = String.Empty
        Dim lRow As Integer


        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If r_lBatchID = 0 And v_sDocumentRef = "" Then
                'We are getting the Instalments that are due to go
                'Zero lead days are Instalments that are due today,
                '1 lead days means ones due today and tomorrow, etc.

                vHeaderArraySub(ehdLeadDays) = lLeadDays
                'Source List is not used in the UI

                vHeaderArraySub(ehdSourceList) = ""
                'Pass through the Media Type Code

                vHeaderArraySub(ehdMediaTypeCode) = sMediaTypeCode
                'The Accounts Filter is not used in the UI

                vHeaderArraySub(ehdAccountsFilter) = 0
                'Only return the data, do not build a new batch

                vHeaderArraySub(ehdArrayOnly) = bArrayOnly
                'Auto-Close is not used in the UI

                vHeaderArraySub(ehdAutoClose) = 0
                'The Batch Type is always BAC for now

                vHeaderArraySub(ehdBatchTypeCode) = "BAC"

                'DD 18/06/2003: The Spoke uses an embedded array within an array.
                'The zero entry in the HeaderArray is not used for this call

                vHeaderArray(1) = vHeaderArraySub


                m_lReturn = m_oSpoke.Export(v_sInterfaceCode:=gHUBSpokeConstants.ksICRecurring, r_sBatchRef:=sBatchRef, r_sStatusCode:=sStatusCode, r_sMessage:=sMessage, r_sHeaderXML:="", r_vHeaderData:=vHeaderArray, r_vDetailData:=r_vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the Array from the Spoke", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInstalmentsForBatch", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Convert Array to suitable format
                If Information.IsArray(r_vResultArray) Then

                    r_vResultArray = r_vResultArray(0)

                    'Get the Batch ID if created
                    r_lBatchID = CInt(sBatchRef)
                End If
            Else
                'The batch has been created, get the contents using a direct query
                With m_oDatabase
                    .Parameters.Clear()
                    .Parameters.Add("BatchID", CStr(r_lBatchID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    'Changes done as per VB code
                    '.Parameters.Add("ForRecall", CStr(Math.Abs(bForRecall)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                    '.Parameters.Add("ForReview", CStr(Math.Abs(bArrayOnly)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                    .Parameters.Add("ForRecall", CStr(Math.Abs(CInt(bForRecall))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                    .Parameters.Add("ForReview", CStr(Math.Abs(CInt(bArrayOnly))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                    .Parameters.Add("INCDocumentRef", v_sDocumentRef, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                    'Developer Guide No 39
                    m_lReturn = .SQLSelect("spe_PFInstalments_selectbatch", "Selet Instalments Batch", True, gPMConstants.PMAllRecords, r_vResultArray) 'PN17702

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the result from call to spu_PFInstalments_selectbatch", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInstalmentsForBatch", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End With
            End If
            Return result

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInstalmentsForBatch", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
            End Select

        Finally

        End Try
        Return result
    End Function
    ' ***************************************************************** '
    '
    ' Name: GetPaymentMethods
    '
    ' Description:  Returns an array of Payment Methods available
    '
    ' History: 14/08/2001 DD - Created.
    '
    ' ***************************************************************** '
    Public Function GetPaymentMethods(ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            'Developer Guide No 39
            m_lReturn = m_oDatabase.SQLSelect("spe_PFPaymentMethod_sellist", "Get Payment Methods", True, , vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPaymentMethods Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPaymentMethods", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Export
    '
    ' Description:  Builds a new batch and exports a file using the chosen
    '               format.
    '
    ' History: 13/08/2001 DD - Created.
    '          DD 19/06/2003: Rewritten for 1.9 Instalments
    '
    ' ***************************************************************** '
    Public Function Export(ByVal lLeadDays As Integer, ByVal sMediaTypeCode As String, ByRef r_lBatchID As Integer, ByRef r_lRecordCount As Integer, ByRef r_sFilenameUsed As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim lStart As Integer
        Dim sDir, sFile As String
        Dim iFile As Integer
        Dim sField As String = ""
        Dim sDetail As New StringBuilder
        'DC301105 PN26052 ensure dp set at 2dp when using ?s for amounts
        Dim nLen, nDPpos As Integer
        Dim colTags As Collection
        Dim oTag As TagEntry
        Dim sStringQuote, sNumericQuote, sDelimeter As String
        Dim nDelimterLength As Integer
        Dim sDueDateFormat As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If DisableExport Then
                'Export functions are disabled so quit here
                Return result
            End If

            'Use a transaction here
            m_oDatabase.SQLBeginTrans()

            'Build the batch and get the Batch Array

            m_lReturn = CType(GetInstalmentsForBatch(r_lBatchID:=r_lBatchID, lLeadDays:=lLeadDays, sMediaTypeCode:=sMediaTypeCode, bArrayOnly:=False, bForRecall:=False, r_vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cannot create the Instalments Batch.", vApp:=ACApp, vClass:=ACClass, vMethod:="Export", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                result = gPMConstants.PMEReturnCode.PMFalse

                'Rollback
                m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            If Not Information.IsArray(vResultArray) Then
                'nothing to do
                result = gPMConstants.PMEReturnCode.PMNotFound
                r_lRecordCount = 0
                r_sFilenameUsed = ""

                'Rollback
                m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            'Work out how many records to process

            r_lRecordCount = vResultArray.GetUpperBound(1)

            lStart = vResultArray.GetLowerBound(1)

            'Calculate the filename to be used
            sDir = Directory
            sFile = Filename

            'Build the tag collection
            colTags = New Collection()

            oTag = New TagEntry()
            oTag.Tag = "<D>"
            oTag.Replacement = StringsHelper.Format(DateTime.Now, DateFormat)
            colTags.Add(oTag)

            oTag = New TagEntry()
            oTag.Tag = "<B>"
            oTag.Replacement = CStr(r_lBatchID)
            colTags.Add(oTag)

            oTag = New TagEntry()
            oTag.Tag = "<RC>"
            oTag.Replacement = CStr(r_lRecordCount + 1)
            colTags.Add(oTag)

            'Prepare the filename (strip invalid characters)
            sFile = ReplaceTags(colTags, sFile)
            sFile = Strip(sFile, MainModule.enumStrip.stripQuote, ":\/<>?*|" & Strings.Chr(34).ToString())

            'Prepare the directory (strip invalid characters)
            sDir = ReplaceTags(colTags, sDir)
            sDir = Strip(sDir, MainModule.enumStrip.stripQuote, "/<>?*|" & Strings.Chr(34).ToString())

            'Build the directory on the disk
            m_lReturn = CType(gPMFunctions.BuildDirectory(sDir), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cannot create export directory tree. Check permissions.", vApp:=ACApp, vClass:=ACClass, vMethod:="Export", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                result = gPMConstants.PMEReturnCode.PMFalse

                'Rollback
                m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            'Delete any existing file
            r_sFilenameUsed = sDir & sFile
            Dim fi As System.IO.FileInfo = New FileInfo(r_sFilenameUsed)
            If fi.Exists Then
                fi.Delete()
            End If

            'If FileSystem.Dir(r_sFilenameUsed, FileAttribute.Normal) <> "" Then
            '    File.Delete(r_sFilenameUsed)
            'End If

            'Set formatting elements
            If ASCIIValue Then
                sDelimeter = Strings.Chr(CInt(Delimeter)).ToString()
            Else
                sDelimeter = Delimeter
            End If

            If QuotedStrings Then
                sStringQuote = QuoteCharacter
            Else
                sStringQuote = ""
            End If

            If QuotedNumerics Then
                sNumericQuote = QuoteCharacter
            Else
                sNumericQuote = ""
            End If

            'Now open it for writing
            'iFile = FileSystem.FreeFile()
            'FileSystem.FileOpen(iFile, r_sFilenameUsed, OpenMode.Binary)
            Dim fs As New FileStream(r_sFilenameUsed, FileMode.Create, FileAccess.Write)
            Dim s As New StreamWriter(fs)
            s.BaseStream.Seek(0, SeekOrigin.Begin)
            'Write the Header
            If Header <> "" Then
                sDetail = New StringBuilder(ReplaceTags(colTags, Header) & Strings.Chr(13) & Strings.Chr(10))
            End If


            ' FileSystem.FilePutObject(iFile, sDetail.ToString())
            s.Write(sDetail.ToString())

            For nRecord As Integer = lStart To r_lRecordCount
                If PFPaymentMethod_cnt = 1 Then
                    'DDM BACS EXPORT

                    'DD 05/01/2004: Recoded to format to UK standard
                    'Sort Code

                    sField = CStr(vResultArray(gHUBSpokeConstants.eddBankSortCode, nRecord)).Trim()
                    sField = Strip(sField, MainModule.enumStrip.stripNonNumeric)
                    If Not AccountNumbersOnly Then
                        sField = sField.Substring(0, 2) & "-" & Mid(sField, 3, 2) & "-" & sField.Substring(sField.Length - 2)
                    End If
                    sField = sNumericQuote & sField & sNumericQuote

                    'PSL 30/04/2003 Iss3500 we will put a delimter in front of all values and remove the first
                    'one later, because we don't know which one will be ther first
                    sDetail = New StringBuilder(Detail)
                    sDetail = New StringBuilder(sDetail.ToString().Replace("<BSC>", sDelimeter & sField))

                    'Bank Account

                    sField = CStr(vResultArray(gHUBSpokeConstants.eddBankAccountNumber, nRecord)).Trim()
                    If AccountNumbersOnly Then
                        sField = Strip(sField, MainModule.enumStrip.stripNonNumeric)
                    End If
                    sField = sNumericQuote & sField & sNumericQuote
                    sDetail = New StringBuilder(sDetail.ToString().Replace("<BAC>", sDelimeter & sField))

                    'Bank Account Name

                    sField = sStringQuote & Strip(CStr(vResultArray(gHUBSpokeConstants.eddAccountName, nRecord)).Trim(), MainModule.enumStrip.stripQuote, sStringQuote) & sStringQuote
                    sDetail = New StringBuilder(sDetail.ToString().Replace("<BAN>", sDelimeter & sField))

                    'Reference

                    sField = sStringQuote & Strip(CStr(vResultArray(gHUBSpokeConstants.eddPFAutoGeneratedPlanRef, nRecord)).Trim(), MainModule.enumStrip.stripQuote, sStringQuote) & sStringQuote
                    sDetail = New StringBuilder(sDetail.ToString().Replace("<REF>", sDelimeter & sField))

                    'DC301105 PN26052 make sure pence is shown as 2dp
                    'Amount
                    If AmountInPence Then

                        sField = sNumericQuote & CStr(Conversion.Val(CStr(vResultArray(gHUBSpokeConstants.eddAmount, nRecord))) * 100) & sNumericQuote
                    Else

                        'if auddis show no decimal places

                        If CStr(vResultArray(gHUBSpokeConstants.eddPFInstalmentTransStatusID, nRecord)) <> "0N" Then


                            sField = CStr(Conversion.Val(CStr(vResultArray(gHUBSpokeConstants.eddAmount, nRecord))))
                            nLen = sField.Length
                            nDPpos = (sField.IndexOf("."c) + 1)
                            If nDPpos = 0 Then

                                sField = sNumericQuote & CStr(Conversion.Val(CStr(vResultArray(gHUBSpokeConstants.eddAmount, nRecord)))) & ".00" & sNumericQuote
                            Else
                                If nDPpos = nLen - 1 Then

                                    sField = sNumericQuote & CStr(Conversion.Val(CStr(vResultArray(gHUBSpokeConstants.eddAmount, nRecord)))) & "0" & sNumericQuote
                                End If
                            End If

                        Else

                            sField = "0"

                        End If

                    End If
                    sDetail = New StringBuilder(sDetail.ToString().Replace("<AMT>", sDelimeter & sField))

                    'Transaction Code

                    sField = sStringQuote & CStr(vResultArray(gHUBSpokeConstants.eddPFInstalmentTransStatusID, nRecord)).Trim() & sStringQuote
                    sDetail = New StringBuilder(sDetail.ToString().Replace("<TC>", sDelimeter & sField))

                    'DD 05/01/2004
                    'Due Date
                    sDueDateFormat = Strip(DateFormat, MainModule.enumStrip.stripSpecial, "DMY/\-. ").Trim()

                    sField = StringsHelper.Format(CStr(vResultArray(gHUBSpokeConstants.eddPFInstalmentDueDate, nRecord)).Trim(), sDueDateFormat)
                    sField = sStringQuote & sField & sStringQuote
                    sDetail = New StringBuilder(sDetail.ToString().Replace("<D>", sDelimeter & sField))

                    'Add a carriage return
                    sDetail.Append(Strings.Chr(13) & Strings.Chr(10))

                    'PSL 30/04/2003 Iss3500  remove the first delimter whuich we put in
                    'because we don't know which one will be the first
                    nDelimterLength = sDelimeter.Length
                    sDetail = New StringBuilder(sDetail.ToString().Substring(sDetail.ToString().Length - (sDetail.ToString().Length - nDelimterLength)))
                End If


                'Write the line

                'FileSystem.FilePutObject(iFile, sDetail.ToString())
                s.Write(sDetail.ToString())
            Next nRecord

            'Write the footer
            If Footer <> "" Then
                sDetail = New StringBuilder(ReplaceTags(colTags, Footer))
                'FileSystem.FilePutObject(iFile, sDetail.ToString())
                s.WriteLine(sDetail.ToString())
            End If

            'Close up
            'FileSystem.FileClose(iFile)
            s.Close()
            fs.Close()
            s.Dispose()
            fs.Dispose()
            'And save lot
            m_oDatabase.SQLCommitTrans()

            'Add the extra one (because array is zero based)
            r_lRecordCount += 1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Export Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Export", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            'Rollback
            m_oDatabase.SQLRollbackTrans()

            'Close the file if opened
            If iFile > 0 Then
                FileSystem.FileClose(iFile)
            End If

            Return result

        End Try
    End Function

    '***LookupBegin***
    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values for an Instalment.
    '
    '
    ' ***************************************************************** '
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As String) As Integer

        Dim result As Integer = 0
        Dim oPFInstalment As Object = Nothing
        Dim dtEffectiveDate As Date

        ' {* USER DEFINED CODE (Begin) *}
        Dim vTabArray(3, 1) As Object
        Dim vRepeatTypeID As Object = Nothing
        Dim vEventTypeID As Object = Nothing
        ' {* USER DEFINED CODE (End) *}

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array
            vResultArray = Nothing
            ' Reset Table Array

            vTableArray = Nothing

            ' {* USER DEFINED CODE (Begin) *}

            ' Setup Lookup Table Names
            'vTabArray(PMLookupTableName, 0) = PMLookupEventRepeatType
            'vTabArray(PMLookupTableName, 1) = PMLookupEventType

            ' {* USER DEFINED CODE (End) *}

            ' Do we have any records
            If m_lCurrentRecord < 1 Then
                ' No, we can only lookup all
                iLookupType = gPMConstants.PMELookupType.PMLookupAll
            Else
            End If

            Select Case iLookupType
                Case gPMConstants.PMELookupType.PMLookupAll

                    ' Do not supply a key

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = ""

                Case gPMConstants.PMELookupType.PMLookupAllEffective

                    ' Use keys and effective date from current record
                    ' Note: The keys are not used for the select, but are used by
                    '       the iterface program to set the list index.
                    With oPFInstalment

                        ' {* USER DEFINED CODE (Begin) *}

                        m_lReturn = .GetProperties(iStatus:=gPMConstants.PMEComponentAction.PMView)



                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = vRepeatTypeID


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = vEventTypeID
                        ' {* USER DEFINED CODE (End) *}

                    End With

                Case gPMConstants.PMELookupType.PMLookupSingle

                    ' Set keys from current record
                    With oPFInstalment

                        ' {* USER DEFINED CODE (Begin) *}

                        m_lReturn = .GetProperties(iStatus:=gPMConstants.PMEComponentAction.PMView)



                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = vRepeatTypeID


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = vEventTypeID
                        ' {* USER DEFINED CODE (End) *}

                    End With

            End Select

            ' Default Effective Date to current date/time
            dtEffectiveDate = DateTime.Now

            ' Release Instalment reference
            oPFInstalment = Nothing

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

            ' Get a link to the Finance Spoke

            m_oSpoke = New bACTFinanceSpoke.Business
            m_lReturn = m_oSpoke.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=sCallingAppName, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

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
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: Update (Public)
    '
    ' Description: Updates a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Public Function Update() As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Add the required INPUT parameters
                m_lReturn = CType(AddInputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Add PrimaryKey as INPUT parameters
                m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACUpdateSQL, sSQLName:=ACUpdateName, bStoredProcedure:=ACUpdateStored, lRecordsAffected:=lRecordsAffected)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Check to see that the record was updated OK
                If lRecordsAffected > 0 Then
                    ' Updated No action required
                Else
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Cancel (Public)
    '
    ' Description: Checks to see if Cancel is OK.
    '              i.e. Do we need any Adding, Deleting or Updating.
    '
    '              Returns PMTrue if all items are clean
    '                      (PMView or PMDummyDelete)
    '              Otherwise returns PMDataChanged.
    ' ***************************************************************** '
    Public Function Cancel() As Integer
        Return gPMConstants.PMEReturnCode.PMTrue
    End Function

    ' ***************************************************************** '
    ' Name: SelectSingle (Public)
    '
    ' Description: Selects the required PFExport
    '
    ' ***************************************************************** '
    Public Function SelectSingle(Optional ByRef vLockMode As gPMConstants.PMELockMode = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Default to No Lock if not supplied or not numeric
                Dim dbNumericTemp As Double

                If (Information.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                    vLockMode = gPMConstants.PMELockMode.PMNoLock
                End If

                ' Add PrimaryKey as INPUT parameters
                m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLSelect(sSQL:=ACSelectSingleSQL, sSQLName:=ACSelectSingleName, bStoredProcedure:=ACSelectSingleStored, bKeepNulls:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' How many records were selected
                lRecordCount = .Records.Count()

                ' Do we have any records ?
                If lRecordCount = 1 Then
                    ' Selected, No action required
                Else
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If

                ' Set properties
                'Developer Guide No.: 112
                m_lReturn = CType(SetPropertiesFromDB(oFields:=.Records.Item(0).Fields()), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectSingle Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectSingle", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetPropertiesFromDB (Public)
    '
    ' Description: Sets the supplied PFExport properties from a database
    '              record.
    ' ***************************************************************** '

    'Developer Guide No.: 21
    Public Function SetPropertiesFromDB(ByRef oFields As DataRow) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Populate Base Details

            With oFields
                'SD 31/07/2002 Scalability changes
                m_sDescription = gPMFunctions.NullToString(oFields("Description"))
                Directory = gPMFunctions.NullToString(nz(oFields("Directory")))
                Filename = gPMFunctions.NullToString(nz(oFields("Filename")))
                Header = gPMFunctions.NullToString(nz(oFields("Header")))
                Detail = gPMFunctions.NullToString(oFields("Detail"))
                Footer = gPMFunctions.NullToString(nz(oFields("Footer")))
                Delimeter = gPMFunctions.NullToString(oFields("Delimeter"))
                ASCIIValue = gPMFunctions.NullToBoolean(oFields("ASCIIValue"))
                AmountInPence = gPMFunctions.NullToBoolean(oFields("AmountInPence"))
                QuoteCharacter = gPMFunctions.NullToString(oFields("QuoteCharacter"))
                QuotedNumerics = gPMFunctions.NullToBoolean(oFields("QuotedNumerics"))
                QuotedStrings = gPMFunctions.NullToBoolean(oFields("QuotedStrings"))
                AccountNumbersOnly = gPMFunctions.NullToBoolean(oFields("AccountNumbersOnly"))
                DateFormat = gPMFunctions.NullToString(nz(oFields("DateFormat")))
                AllowAutoPost = gPMFunctions.NullToBoolean(oFields("AllowAutoPost"))
                DisableExport = gPMFunctions.NullToBoolean(oFields("DisableExport"))
                ExcludeAudis = gPMFunctions.NullToBoolean(oFields("ExcludeAudis")) 'MKW160204 PN10300
            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetPropertiesFromDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPropertiesFromDB", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: AddInputParam (Private)
    '
    ' Description: Adds all of the NON-KEY INPUT parameters
    '              required for an Insert or Update.
    '
    ' ***************************************************************** '
    Private Function AddInputParam() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase
            m_lReturn = .Parameters.Add(sName:="Directory", vValue:=Directory, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            m_lReturn = .Parameters.Add(sName:="Filename", vValue:=Filename, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            m_lReturn = .Parameters.Add(sName:="Header", vValue:=Header, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            m_lReturn = .Parameters.Add(sName:="Detail", vValue:=Detail, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            m_lReturn = .Parameters.Add(sName:="Footer", vValue:=Footer, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            m_lReturn = .Parameters.Add(sName:="Delimeter", vValue:=Delimeter, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = .Parameters.Add(sName:="ASCIIValue", vValue:=Math.Abs(CInt(ASCIIValue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            m_lReturn = .Parameters.Add(sName:="AmountInPence", vValue:=Math.Abs(CInt(AmountInPence)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_lReturn = .Parameters.Add(sName:="QuoteCharacter", vValue:=QuoteCharacter, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = .Parameters.Add(sName:="QuotedNumerics", vValue:=Math.Abs(CInt(QuotedNumerics)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            m_lReturn = .Parameters.Add(sName:="QuotedStrings", vValue:=Math.Abs(CInt(QuotedStrings)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            m_lReturn = .Parameters.Add(sName:="AccountNumbersOnly", vValue:=Math.Abs(CInt(AccountNumbersOnly)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_lReturn = .Parameters.Add(sName:="DateFormat", vValue:=DateFormat, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = .Parameters.Add(sName:="AllowAutoPost", vValue:=Math.Abs(CInt(AllowAutoPost)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            m_lReturn = .Parameters.Add(sName:="DisableExport", vValue:=Math.Abs(CInt(DisableExport)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)


            'for future expansion

            m_lReturn = .Parameters.Add(sName:="UseZeroInstalment", vValue:=CInt(1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)


            'MKW160204 PN10300 START.  Added Extra param for Exclude audis

            m_lReturn = .Parameters.Add(sName:="ExcludeAudis", vValue:=Math.Abs(CInt(ExcludeAudis)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            'MKW160204 PN10300 END

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddKeyInputParam (Private)
    '
    ' Description: Adds all of the PRIMARY KEY INPUT parameters
    '              required for a Select, Delete or Update.
    '
    ' ***************************************************************** '
    Private Function AddKeyInputParam() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase
            m_lReturn = .Parameters.Add(sName:="PFPaymentMethod_cnt", vValue:=CInt(PFPaymentMethod_cnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddKeyOutputParam (Private)
    '
    ' Description: Adds all of the PRIMARY KEY OUTPUT parameters
    '              required for an Add.
    '
    '   NOT USED - bSIRPFExport uses a table with a fixed set of records
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (AddKeyOutputParam) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function AddKeyOutputParam() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'With m_oDatabase
    '
    'End With
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
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddKeyOutputParam Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddKeyOutputParam", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: GetNewPrimaryKeyID (Private)
    '
    ' Description: Returns the new PRIMARY KEY values from an Add.
    '
    '   NOT USED - bSIRPFExport uses a table with a fixed set of records
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetNewPrimaryKeyID) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetNewPrimaryKeyID() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'With m_oDatabase
    '
    'End With
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
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNewPrimaryKeyID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNewPrimaryKeyID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        ' Class Initialise

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    'MKW160204 PN10300.  Extra methods for Exclude Audis Option END
End Class

