Option Explicit On
Option Strict On

Imports System.Xml
Imports System.Xml.Schema
Imports System.Xml.Xsl
Imports System.Collections.ObjectModel
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions

Public MustInherit Class ExportBase

#Region "Fields"
    ' Construct on first use
    Protected m_sFilename As String = String.Empty
    Private m_sExportPath As String = String.Empty
    Private cloudHostingOptionEnabled As Boolean
    Private transformedFilePath As String = String.Empty

    ' Database connection
    Protected m_oDatabase As dPMDAO.Database = Nothing
    Protected Const kBatchStatusComplete As String = "C"
    Public Const kBatchStatusFailed As String = "F"
#End Region

#Region "Properties"
    ' Builds the export filename for this interface
    Public Overridable ReadOnly Property Filename() As String
        Get
            If (m_sFilename.Length = 0) Then
                m_sFilename = String.Format("{0}_{1}.xml", InterfaceName, Now.ToString("yyyyMMddhhmm"))
            End If
            Return m_sFilename
        End Get
    End Property

    Public ReadOnly Property Path() As String
        Get
            ' If we haven't got the path yet, get it
            If (m_sExportPath.Length = 0) Then
                m_sExportPath = GetSystemOption(ACExportPathOption)
            End If

            ' Check it was configured
            If (m_sExportPath.Length = 0) Then
                OutputLine()
                OutputLine("Warning! Export path not found. File will be exported to current directory.")
                m_sExportPath = My.Application.Info.DirectoryPath
            Else
                ' Check it exists
                If Not Directory.Exists(m_sExportPath) Then
                    ' If we can create it do so, else raise error
                    Directory.CreateDirectory(m_sExportPath)
                End If
            End If

            ' If we made it this far return the path
            Return m_sExportPath
        End Get
    End Property

    Public ReadOnly Property FullPath() As String
        Get
            ' Return the combined path
            Return System.IO.Path.Combine(Path, Filename)
        End Get
    End Property

    ' Specifies the interface name for this class
    Public MustOverride ReadOnly Property InterfaceName() As String
    Public MustOverride Property BatchId() As Integer

    ''' <summary>
    ''' Indicates if cloud hosting is enabled.
    ''' </summary>
    ''' <returns>True if cloud hosting is enabled otherwise False.</returns>
    Public ReadOnly Property CloudHostingEnabled() As Boolean
        Get
            Return cloudHostingOptionEnabled
        End Get
    End Property

    ''' <summary>
    ''' To specify the full path for file created by process transform.
    ''' </summary>
    ''' <returns>The full path of the file created by transform process.</returns>
    Public Property TransformedFile() As String
        Get
            Return transformedFilePath
        End Get

        Set(value As String)
            transformedFilePath = value
        End Set
    End Property
#End Region

#Region "Methods"
    ' Process the command line for the inheriting interface
    Public MustOverride Sub DisplayHelp()

    ' Process the command line for the inheriting interface
    Public MustOverride Sub ProcessCommandLine(ByVal cArgs As Collection(Of String))

    ' Process the export routine
    Public MustOverride Sub ProcessExport()

    ' clean up interops
    Public MustOverride Sub CleanUpInterops()

    ' close database connection
    Public Sub CloseDBConnection()
        DBDisconnect(m_oDatabase)
    End Sub

    Public Sub ProcessTransform(ByVal cArgs As Collection(Of String))
        If InterfaceName.Trim().ToUpper() = "MID2_EXPORT" Or InterfaceName.Trim().ToUpper() = "MID_EXPORT" Then
            Exit Sub
        End If

        Dim xsltFilename As String = String.Empty
        Dim destFileExtn As String = String.Empty
        Dim destFolder As String = String.Empty

        If cArgs.Count >= 0 Then

            Dim NoofCommandLineArgs As Integer
            Dim lItem As Integer = 0
            Dim sArg As String
            Dim sArgValues() As String

            ' get the number of command line arguments passed
            NoofCommandLineArgs = cArgs.Count - 1

            ' for each command line argument passed
            For lItem = 0 To NoofCommandLineArgs
                ' get the argument (should be in the format (argument_name = argument_value)
                sArg = cArgs(lItem).ToString()

                'Ensure that the first item is skipped if this is the batch id
                If Not (IsNumeric(sArg) And lItem = 0) Then
                    ' split the argument into argument name / argument value
                    sArgValues = sArg.Split(CChar("="))

                    ' The key should have a value in key=value i.e. 
                    ' an array of length 2
                    'If sArgValues.Length = 1 Then
                    '    Throw New ArgumentException("Invalid argument - Value missing for " & sArgValues(0), sArgValues(0))
                    'End If

                    ' determine which argument we are looking at
                    Select Case sArgValues(0).ToUpper

                        Case "XSLT_FILE_NAME"
                            ' Try and grab an end date
                            xsltFilename = CStr(sArgValues(1))

                        Case "DEST_FILE_EXTN"
                            ' Try and grab an end date
                            destFileExtn = CStr(sArgValues(1))

                        Case "DEST_FOLDER"
                            ' Try and grab an end date
                            destFolder = CStr(sArgValues(1))
                    End Select
                End If
            Next

        End If

        If xsltFilename.Length <> 0 Then

            Try

                ' Check the XSLT file exists
                If File.Exists(xsltFilename) = False Then
                    Throw New ArgumentException("XSLT file does not exist - " & xsltFilename, "batchid")
                End If

                OutputLine("XSL transform beginning")

                ' Construct the sResultFile path from the source filename, dest folder (if provided) and dest extension.  
                Dim resultFilename As String

                If CloudHostingEnabled Then
                    resultFilename = FullPath
                Else
                    If destFolder.Length > 0 Then
                        destFolder = IIf(destFolder.EndsWith("\"), destFolder, destFolder & "\").ToString
                        If Directory.Exists(destFolder) = False Then
                            Throw New ArgumentException("Destination Folder does not exist - " & destFolder, "batchid")
                        End If
                        resultFilename = destFolder & Filename
                    Else
                        resultFilename = FullPath
                    End If
                End If

                ' Change the Extension
                If destFileExtn.Length > 0 Then
                    resultFilename = resultFilename.Replace(".xml", "." & destFileExtn)
                End If

                ' Check the source and destination files are not the same
                If FullPath = resultFilename Then
                    resultFilename = resultFilename.Replace(".xml", "(XSLT Output).xml")
                End If

                Dim transform As XslCompiledTransform = New XslCompiledTransform()
                Dim xslSettings As New XsltSettings(False, True)
                transform.Load(xsltFilename, xslSettings, Nothing)
                'transform.OutputSettings. XsltSettings.EnableScript 
                transform.Transform(FullPath, resultFilename)

                TransformedFile = resultFilename

                OutputLine("XSL transform complete")

            Catch ex As ArgumentException
                Throw ex
            Catch ex As XmlException
                OutputLine("Invalid XML File Format")
                Throw New ApplicationException("Export file has an invalid XML File Format", ex)
            Catch ex As Exception
                Throw New ApplicationException("Export file could not be transformed using the transform file - " & xsltFilename, ex)
            End Try

        End If
    End Sub

    ''' <summary>
    ''' Update Batch Status
    ''' </summary>
    ''' <param name="sBatchStatusCode"></param>
    ''' <param name="nBatchId"></param>
    ''' <param name="sFileName"></param>
    ''' <param name="nTotal_Transactions"></param>
    ''' <param name="nReject_transactions"></param>
    Public Sub UpdateBatchTask(ByVal sBatchStatusCode As String, ByVal nBatchId As Integer, ByVal sFileName As String, ByVal nTotal_Transactions As Integer, ByVal nReject_transactions As Integer)
        Dim nReturnValue As Integer = PMEReturnCode.PMTrue
        Try
            AddParameterLite(m_oDatabase, "Batch_Id", nBatchId, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, True)
            AddParameterLite(m_oDatabase, "FileName", sFileName, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "BatchStatusCode", sBatchStatusCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "Total_Transactions", nTotal_Transactions, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
            AddParameterLite(m_oDatabase, "Reject_Transactions", nReject_transactions, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
            nReturnValue = m_oDatabase.SQLAction("spu_Update_BatchTask", "Update Batch in Batch", True)
            If nReturnValue <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute 'spu_Update_BatchTask'")
            End If
        Catch ex As Exception
            Throw New Exception("Unable to update entry in Batch", ex)
        End Try
    End Sub

    Public Sub GenerateSchemaHeader(ByRef oXML As XmlDocument)
        'This function can be used to do any generic tidying up on the XML output.
        'At the moment this is not used.
    End Sub


#End Region

#Region "Creator"

    Public Sub New()
        ' Connect to database
        DBConnect(m_oDatabase)

        Dim cloudHostingOptionValue As String = ""
        SharedFiles.bPMFunc.getProductOptionValue(v_sUsername:="", v_sPassword:="", v_iUserID:=0, v_iMainSourceID:=0, v_iLanguageID:=0, v_iCurrencyID:=0, v_iLogLevel:=0, v_sCallingAppName:=ACApp, v_vOptionNumber:=SharedFiles.gPMConstants.SIRHiddenOptions.SIROPTEnableCloudHosting, v_vBranch:=1, r_vUnderwriting:=cloudHostingOptionValue)
        cloudHostingOptionEnabled = (cloudHostingOptionValue = "1")
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

#End Region



End Class
