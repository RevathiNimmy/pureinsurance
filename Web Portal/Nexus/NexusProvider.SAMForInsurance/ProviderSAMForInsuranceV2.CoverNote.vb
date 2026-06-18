Imports NexusProvider.SAMForInsurance.PureService
Imports System.ServiceModel
Imports System.Configuration.Provider
Imports System.Web
Imports System.Web.HttpContext
Imports System.Web.Configuration.WebConfigurationManager
Imports Microsoft.Web.Services3.Security.Tokens
Imports SiriusFS.SAM.Client.Security
Imports System.Xml.Serialization
Imports Microsoft.Practices.EnterpriseLibrary.Logging
Imports System.Diagnostics
Imports System.Text
Imports System.Xml
Partial Public Class ProviderSAMForInsuranceV2
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_oCoverNote"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function FindCoverNoteBooks(ByRef v_oCoverNote As CoverNote,
                                       Optional ByVal v_sBranchCode As String = Nothing) As CoverNoteCollection

        SyncLock oLock
            Dim oSAM As PureServiceClient
            Dim oFindCoverNoteBooksRequest As FindCoverNoteBooksRequestType
            Dim oFindCoverNoteBooksResponse As FindCoverNoteBooksResponseType
            Dim oCoverNoteCollection As CoverNoteCollection
            Dim oCoverNote As CoverNote
            Dim sbLogMessage As StringBuilder
            Try
                oSAM = InitializeServiceMethod()
                oFindCoverNoteBooksRequest = New FindCoverNoteBooksRequestType
                oFindCoverNoteBooksResponse = New FindCoverNoteBooksResponseType
                oCoverNoteCollection = New CoverNoteCollection
                sbLogMessage = New StringBuilder
                With oFindCoverNoteBooksRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    If v_oCoverNote.AgentKey > 0 Then
                        .AgentKey = v_oCoverNote.AgentKey
                        .AgentKeySpecified = True
                    Else
                        .AgentKeySpecified = False
                    End If
                    If v_oCoverNote.AssignedDate <> Date.MinValue Then
                        .AssignedDate = v_oCoverNote.AssignedDate
                        .AssignedDateSpecified = True
                    Else
                        .AssignedDateSpecified = False
                    End If
                    .BookNumber = v_oCoverNote.BookNumber
                    .CoverNoteBranchCode = v_oCoverNote.CoverNoteBranchCode
                    .CoverNoteStatusCode = v_oCoverNote.CoverNoteBookStatusCode
                    If v_oCoverNote.StartNumber <> 0 Then
                        .StartNumber = v_oCoverNote.StartNumber
                        .StartNumberSpecified = True
                    Else
                        .StartNumberSpecified = False
                    End If
                    If v_oCoverNote.EndNumber <> 0 Then
                        .EndNumber = v_oCoverNote.EndNumber
                        .EndNumberSpecified = True
                    Else
                        .EndNumberSpecified = False
                    End If

                    If v_oCoverNote.LastUpdated <> Date.MinValue Then
                        .LastUpdated = v_oCoverNote.LastUpdated
                        .LastUpdatedSpecified = True
                    Else
                        .LastUpdatedSpecified = False
                    End If

                    .PolicyNumber = v_oCoverNote.PolicyNumber

                    If v_oCoverNote.MaxRowsToFetch > 0 Then
                        .MaxRowsToFetch = v_oCoverNote.MaxRowsToFetch
                        .MaxRowsToFetchSpecified = True
                    Else
                        .MaxRowsToFetchSpecified = False
                    End If

                End With
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oFindCoverNoteBooksResponse = oSAM.FindCoverNoteBooks(oFindCoverNoteBooksRequest)
                End Using
                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.
                With oFindCoverNoteBooksResponse

                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else

                        If .FindCoverNoteBooks IsNot Nothing AndAlso .FindCoverNoteBooks.Count > 0 Then
                            For Each oFindCoverNoteBooksRow As BaseFindCoverNoteBooksResponseTypeRow In .FindCoverNoteBooks
                                oCoverNote = New CoverNote()
                                oCoverNote.CoverNoteBookKey = oFindCoverNoteBooksRow.CoverNoteBookKey
                                oCoverNote.BookNumber = oFindCoverNoteBooksRow.BookNumber
                                oCoverNote.StartNumber = oFindCoverNoteBooksRow.StartNumber
                                oCoverNote.EndNumber = oFindCoverNoteBooksRow.EndNumber
                                oCoverNote.AgentKey = oFindCoverNoteBooksRow.AgentKey
                                oCoverNote.AgentName = oFindCoverNoteBooksRow.AgentName
                                oCoverNote.CoverNoteStatusKey = oFindCoverNoteBooksRow.CoverNoteStatusKey
                                oCoverNote.CoverNoteStatusDescription = oFindCoverNoteBooksRow.CoverNoteStatusDescription
                                oCoverNote.CoverNoteBranchKey = oFindCoverNoteBooksRow.CoverNoteBranchKey
                                oCoverNote.CoverNoteBranchDescription = oFindCoverNoteBooksRow.CoverNoteBranchDescription
                                oCoverNote.LastUpdated = oFindCoverNoteBooksRow.LastUpdated
                                oCoverNote.DateCreated = oFindCoverNoteBooksRow.DateCreated
                                oCoverNote.EffectiveDate = oFindCoverNoteBooksRow.EffectiveDate
                                oCoverNoteCollection.Add(oCoverNote)
                            Next
                        End If
                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("FindCoverNoteBooks executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    'sbLogMessage.AppendLine("r_oQuote = " & r_oQuote.Print.Replace("<br />", vbCrLf))

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oFindCoverNoteBooksRequest = Nothing
                oFindCoverNoteBooksResponse = Nothing
            End Try


            Return oCoverNoteCollection

        End SyncLock


    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_oCoverNote"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub AddCoverNoteBook(ByRef r_oCoverNote As CoverNote,
                                               Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock
            Dim oSAM As PureServiceClient
            Dim oAddCoverNoteBookRequest As AddCoverNoteBookRequestType
            Dim oAddCoverNoteBookResponse As AddCoverNoteBookResponseType
            Dim oCoverNoteProduct As BaseCoverNoteBookTypeRow
            Dim iCounter As Integer
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oAddCoverNoteBookRequest = New AddCoverNoteBookRequestType
                oAddCoverNoteBookResponse = New AddCoverNoteBookResponseType
                sbLogMessage = New StringBuilder


                With oAddCoverNoteBookRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If

                    If r_oCoverNote.AgentKey > 0 Then
                        .AgentKey = r_oCoverNote.AgentKey
                        .AgentKeySpecified = True
                    Else
                        .AgentKeySpecified = False
                    End If
                    .BookNumber = r_oCoverNote.BookNumber
                    .CoverNoteBranchCode = r_oCoverNote.CoverNoteBranchCode

                    .CoverNoteProducts = New List(Of BaseCoverNoteBookTypeRow)
                    For iCounter = 0 To r_oCoverNote.CoverNoteBookProducts.Count - 1
                        oCoverNoteProduct = New BaseCoverNoteBookTypeRow
                        oCoverNoteProduct.ProductCode = r_oCoverNote.CoverNoteBookProducts(iCounter).ProductCode
                        .CoverNoteProducts.Add(oCoverNoteProduct)
                    Next

                    .CoverNoteStatusCode = r_oCoverNote.CoverNoteStatusCode
                    .EffectiveDate = r_oCoverNote.EffectiveDate
                    .StartNumber = r_oCoverNote.StartNumber
                    .EndNumber = r_oCoverNote.EndNumber

                End With


                Using trace As New Tracer(Category.Trace)
                    oAddCoverNoteBookResponse = oSAM.AddCoverNoteBook(oAddCoverNoteBookRequest)
                End Using

                With oAddCoverNoteBookResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        r_oCoverNote.CoverNoteBookTimestamp = oAddCoverNoteBookResponse.CoverNoteBookTimestamp
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("AddCoverNoteBook executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine(r_oCoverNote.Print.Replace("<br />", vbCrLf))
                    sbLogMessage.AppendLine("Output: " & vbCrLf)
                    sbLogMessage.AppendLine("CoverNoteBookTimeStamp: " & r_oCoverNote.CoverNoteBookTimestamp.ToString() & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    If Not IsNothing(r_oCoverNote.StartNumber) Then
                        sbLogMessage.AppendLine("r_oCoverNote.StartNumber" & r_oCoverNote.StartNumber.ToString() & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("r_oCoverNote.StartNumber=nothing" & vbCrLf)
                    End If

                    If Not IsNothing(r_oCoverNote.EndNumber) Then
                        sbLogMessage.AppendLine("r_oCoverNote.EndNumber" & r_oCoverNote.EndNumber.ToString() & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("r_oCoverNote.EndNumber=nothing" & vbCrLf)
                    End If

                    If Not IsNothing(r_oCoverNote.EffectiveDate) Then
                        sbLogMessage.AppendLine("r_oCoverNote.EffectiveDate" & r_oCoverNote.EffectiveDate.ToString() & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("r_oCoverNote.EffectiveDate=nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oAddCoverNoteBookRequest = Nothing
                oAddCoverNoteBookResponse = Nothing
            End Try

        End SyncLock
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_oCoverNote"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub GetCoverNoteBook(ByRef r_oCoverNote As CoverNote,
                                            Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock
            Dim oSAM As PureServiceClient
            Dim oGetCoverNoteBookRequest As GetCoverNoteBookRequestType
            Dim oGetCoverNoteBookResponse As GetCoverNoteBookResponseType
            Dim oCoverNoteBookType As CoverNoteBookTypeCollection
            Dim oProduct As Product
            Dim oCoverNoteSheet As CoverNoteSheetType
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oGetCoverNoteBookRequest = New GetCoverNoteBookRequestType
                oGetCoverNoteBookResponse = New GetCoverNoteBookResponseType
                oCoverNoteBookType = New CoverNoteBookTypeCollection
                sbLogMessage = New StringBuilder


                With oGetCoverNoteBookRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If
                    .CoverNoteBookKey = r_oCoverNote.CoverNoteBookKey
                End With


                Using trace As New Tracer(Category.Trace)
                    oGetCoverNoteBookResponse = oSAM.GetCoverNoteBook(oGetCoverNoteBookRequest)
                End Using

                With oGetCoverNoteBookResponse

                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        r_oCoverNote.BookNumber = .BookNumber
                        r_oCoverNote.StartNumber = .StartNumber
                        r_oCoverNote.EndNumber = .EndNumber
                        r_oCoverNote.EffectiveDate = .EffectiveDate
                        r_oCoverNote.AgentKey = .AgentKey
                        r_oCoverNote.AgentName = .AgentName
                        r_oCoverNote.CoverNoteBranchKey = .CoverNoteBranchKey
                        r_oCoverNote.CoverNoteBranchCode = .CoverNoteBranchCode
                        r_oCoverNote.CoverNoteBookStatusKey = .CoverNoteBookStatusKey
                        r_oCoverNote.CoverNoteBookStatusCode = .CoverNoteBookStatusCode
                        r_oCoverNote.DateCreated = .DateCreated
                        r_oCoverNote.CoverNoteBookTimestamp = .CoverNoteBookTimestamp

                        If .CoverNoteBookProducts IsNot Nothing AndAlso .CoverNoteBookProducts.Count > 0 Then
                            For Each oCoverNoteProducts As BaseGetCoverNoteBookResponseTypeRow In .CoverNoteBookProducts
                                oProduct = New Product
                                oProduct.ProductKey = oCoverNoteProducts.ProductKey
                                oProduct.ProductCode = oCoverNoteProducts.ProductCode
                                oProduct.Chosen = oCoverNoteProducts.Chosen
                                oProduct.Description = oCoverNoteProducts.Description

                                r_oCoverNote.CoverNoteBookProducts.Add(oProduct)
                            Next
                        End If
                        If .CoverNoteSheets IsNot Nothing AndAlso .CoverNoteSheets.Count > 0 Then

                            For Each oCoverNoteSheets As BaseGetCoverNoteBookResponseTypeRow1 In .CoverNoteSheets
                                oCoverNoteSheet = New CoverNoteSheetType
                                oCoverNoteSheet.CoverNoteSheetKey = oCoverNoteSheets.CoverNoteSheetKey
                                oCoverNoteSheet.CoverNoteSheetNumber = oCoverNoteSheets.CoverNoteSheetNumber
                                oCoverNoteSheet.CustomerName = oCoverNoteSheets.CustomerName
                                oCoverNoteSheet.CoverNoteSheetStatusKey = oCoverNoteSheets.CoverNoteSheetStatusKey
                                oCoverNoteSheet.CoverNoteSheetStatusCode = oCoverNoteSheets.CoverNoteSheetStatusCode
                                oCoverNoteSheet.CoverNoteSheetStatusDescription = oCoverNoteSheets.CoverNoteSheetStatusDescription
                                oCoverNoteSheet.PolicyNumber = oCoverNoteSheets.PolicyNumber
                                oCoverNoteSheet.BranchName = oCoverNoteSheets.BranchName
                                oCoverNoteSheet.AgentName = oCoverNoteSheets.AgentName
                                oCoverNoteSheet.DateImported = oCoverNoteSheets.DateImported

                                r_oCoverNote.CoverNoteSheets.Add(oCoverNoteSheet)
                            Next
                        End If

                        oCoverNoteBookType.Add(r_oCoverNote)

                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetCoverNoteBook executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("r_oCoverNote.CoverNoteBookKey" & r_oCoverNote.CoverNoteBookKey.ToString() & vbCrLf)
                    sbLogMessage.AppendLine("Output:" & vbCrLf)
                    'to complete
                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetCoverNoteBookRequest = Nothing
                oGetCoverNoteBookResponse = Nothing
            End Try

        End SyncLock

    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_oCoverNote"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub UpdateCoverNoteBook(ByRef r_oCoverNote As CoverNote,
                                               Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock
            Dim oSAM As PureServiceClient
            Dim oUpdateCoverNoteBookRequest As UpdateCoverNoteBookRequestType
            Dim oUpdateCoverNoteBookResponse As UpdateCoverNoteBookResponseType
            Dim oCoverNoteProduct As BaseCoverNoteBookTypeRow
            Dim iCounter As Integer
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oUpdateCoverNoteBookRequest = New UpdateCoverNoteBookRequestType
                oUpdateCoverNoteBookResponse = New UpdateCoverNoteBookResponseType
                sbLogMessage = New StringBuilder


                With oUpdateCoverNoteBookRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode

                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If
                    If r_oCoverNote.AgentKey > 0 Then
                        .AgentKey = r_oCoverNote.AgentKey
                        .AgentKeySpecified = True
                    Else
                        .AgentKeySpecified = False
                    End If
                    If r_oCoverNote.CoverNoteBookKey > 0 Then
                        .CoverNoteBookKey = r_oCoverNote.CoverNoteBookKey
                    Else
                        Throw New ArgumentNullException("CoverNoteBookKey")
                    End If
                    .CoverNoteBookTimestamp = r_oCoverNote.CoverNoteBookTimestamp
                    .CoverNoteBranchCode = r_oCoverNote.CoverNoteBranchCode


                    .CoverNoteProducts = New List(Of BaseCoverNoteBookTypeRow)
                    For iCounter = 0 To r_oCoverNote.CoverNoteBookProducts.Count - 1
                        oCoverNoteProduct = New BaseCoverNoteBookTypeRow
                        oCoverNoteProduct.ProductCode = r_oCoverNote.CoverNoteBookProducts(iCounter).ProductCode
                        .CoverNoteProducts.Add(oCoverNoteProduct)
                    Next

                    .CoverNoteStatusCode = r_oCoverNote.CoverNoteStatusCode
                    .EffectiveDate = r_oCoverNote.EffectiveDate

                    'Checking the CoverNoteBookKey


                End With


                Using trace As New Tracer(Category.Trace)
                    oUpdateCoverNoteBookResponse = oSAM.UpdateCoverNoteBook(oUpdateCoverNoteBookRequest)
                End Using

                With oUpdateCoverNoteBookResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        r_oCoverNote.CoverNoteBookTimestamp = .CoverNoteBookTimestamp
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("UpdateCoverNoteBook executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    '  sbLogMessage.AppendLine("v_iCoverNoteBookKey" & v_iCoverNoteBookKey.ToString() & vbCrLf)
                    ' sbLogMessage.AppendLine("v_bCoverNoteBookTimestamp" & v_bCoverNoteBookTimestamp.ToString() & vbCrLf)
                    sbLogMessage.AppendLine("Output:" & vbCrLf)
                    'to complete
                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If
                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oUpdateCoverNoteBookRequest = Nothing
                oUpdateCoverNoteBookResponse = Nothing
            End Try


        End SyncLock

    End Sub
    Public Overrides Sub AddCoverNoteSheet(ByRef r_oCoverNote As CoverNote,
                                               Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock
            Dim oSAM As PureServiceClient
            Dim oAddCoverNoteSheetRequest As AddCoverNoteSheetRequestType
            Dim oAddCoverNoteSheetResponse As AddCoverNoteSheetResponseType
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oAddCoverNoteSheetRequest = New AddCoverNoteSheetRequestType
                oAddCoverNoteSheetResponse = New AddCoverNoteSheetResponseType
                sbLogMessage = New StringBuilder


                With oAddCoverNoteSheetRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode

                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    'Checking the CoverNoteBookKey
                    If r_oCoverNote.CoverNoteBookKey > 0 Then
                        .CoverNoteBookKey = r_oCoverNote.CoverNoteBookKey
                    Else
                        Throw New ArgumentNullException("CoverNote.CoverNoteBookKey")
                    End If

                    'Checking the CoverNoteSheetNumber
                    If r_oCoverNote.CoverNoteSheets(0).CoverNoteSheetNumber > 0 Then
                        .CoverNoteSheetNumber = r_oCoverNote.CoverNoteSheets(0).CoverNoteSheetNumber
                    Else
                        Throw New ArgumentNullException("CoverNote.CoverNoteSheets.CoverNoteSheetNumber")
                    End If


                    .CoverNoteStatusCode = r_oCoverNote.CoverNoteBookStatusCode
                    .Comments = r_oCoverNote.Comments
                    .CoverNoteBookTimestamp = r_oCoverNote.CoverNoteBookTimestamp
                End With


                Using trace As New Tracer(Category.Trace)
                    oAddCoverNoteSheetResponse = oSAM.AddCoverNoteSheet(oAddCoverNoteSheetRequest)
                End Using

                With oAddCoverNoteSheetResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        r_oCoverNote.CoverNoteBookTimestamp = .CoverNoteBookTimestamp
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("AddCoverNoteSheet executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("r_oCoverNote.CoverNoteBookKey" & r_oCoverNote.CoverNoteBookKey.ToString() & vbCrLf)
                    sbLogMessage.AppendLine("r_oCoverNote.CoverNoteSheets.CoverNoteSheetNumber" & r_oCoverNote.CoverNoteSheets(0).CoverNoteSheetNumber.ToString() & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oAddCoverNoteSheetRequest = Nothing
                oAddCoverNoteSheetResponse = Nothing
            End Try


        End SyncLock

    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_oCoverNote"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub DeleteCoverNoteSheet(ByRef r_oCoverNote As CoverNote,
                                                      Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock
            Dim oSAM As PureServiceClient
            Dim oDeleteCoverNoteSheetRequest As DeleteCoverNoteSheetRequestType
            Dim oDeleteCoverNoteSheetResponse As DeleteCoverNoteSheetResponseType
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oDeleteCoverNoteSheetRequest = New DeleteCoverNoteSheetRequestType
                oDeleteCoverNoteSheetResponse = New DeleteCoverNoteSheetResponseType
                sbLogMessage = New StringBuilder


                With oDeleteCoverNoteSheetRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode

                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    'Checking the CoverNoteBookKey
                    If r_oCoverNote.CoverNoteBookKey > 0 Then
                        .CoverNoteBookKey = r_oCoverNote.CoverNoteBookKey
                    Else
                        Throw New ArgumentNullException("CoverNote.CoverNoteBookKey")
                    End If

                    'Checking the CoverNoteSheetKey
                    If r_oCoverNote.CoverNoteSheets(0).CoverNoteSheetNumber > 0 Then
                        .CoverNoteSheetKey = r_oCoverNote.CoverNoteSheets(0).CoverNoteSheetNumber
                    Else
                        Throw New ArgumentNullException("CoverNote.CoverNoteSheets.CoverNoteSheetKey")
                    End If

                    .CoverNoteBookTimestamp = r_oCoverNote.CoverNoteBookTimestamp
                End With


                Using trace As New Tracer(Category.Trace)
                    oDeleteCoverNoteSheetResponse = oSAM.DeleteCoverNoteSheet(oDeleteCoverNoteSheetRequest)
                End Using

                With oDeleteCoverNoteSheetResponse

                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        r_oCoverNote.CoverNoteBookTimestamp = .CoverNoteBookTimestamp
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("DeleteCoverNoteSheet executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("r_oCoverNote.CoverNoteSheets.CoverNoteSheetKey" & r_oCoverNote.CoverNoteSheets(0).CoverNoteSheetKey.ToString() & vbCrLf)
                    sbLogMessage.AppendLine("r_oCoverNote.CoverNoteBookKey" & r_oCoverNote.CoverNoteBookKey.ToString() & vbCrLf)
                    sbLogMessage.AppendLine(" r_oCoverNote.CoverNoteBookTimestamp" & r_oCoverNote.CoverNoteBookTimestamp.ToString() & vbCrLf)

                    sbLogMessage.AppendLine("Output:" & vbCrLf)
                    sbLogMessage.AppendLine(" r_oCoverNote.CoverNoteBookTimestamp" & r_oCoverNote.CoverNoteBookTimestamp.ToString() & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oDeleteCoverNoteSheetRequest = Nothing
                oDeleteCoverNoteSheetResponse = Nothing
            End Try

        End SyncLock

    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_oCoverNote"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub GetCoverNoteSheet(ByRef r_oCoverNote As CoverNote,
                                                Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock
            Dim oSAM As PureServiceClient
            Dim oGetCoverNoteSheetRequest As GetCoverNoteSheetRequestType
            Dim oGetCovernoteSheetResponse As GetCoverNoteSheetResponseType
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oGetCoverNoteSheetRequest = New GetCoverNoteSheetRequestType
                oGetCovernoteSheetResponse = New GetCoverNoteSheetResponseType
                sbLogMessage = New StringBuilder


                With oGetCoverNoteSheetRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode

                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    'Checking the CoverNoteBookKey
                    If r_oCoverNote.CoverNoteBookKey > 0 Then
                        .CoverNoteBookKey = r_oCoverNote.CoverNoteBookKey
                    Else
                        Throw New ArgumentNullException("CoverNote.CoverNoteBookKey")
                    End If

                    'Checking the CoverNoteSheetNumber

                    .CoverNoteSheetNumber = r_oCoverNote.CoverNoteSheets(0).CoverNoteSheetNumber

                End With


                Using trace As New Tracer(Category.Trace)
                    oGetCovernoteSheetResponse = oSAM.GetCoverNoteSheet(oGetCoverNoteSheetRequest)
                End Using

                With oGetCovernoteSheetResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        r_oCoverNote.CoverNoteSheets(0).CoverNoteSheetKey = .CoverNoteSheetKey
                        r_oCoverNote.CoverNoteSheets(0).CoverNoteSheetNumber = .CoverNoteSheetNumber
                        r_oCoverNote.CoverNoteSheets(0).PolicyNumber = .InsuranceRef
                        r_oCoverNote.InsuranceFileDetails.InsuranceFileCnt = .InsuranceFileCnt
                        r_oCoverNote.AssignedDate = .AssignedDate
                        r_oCoverNote.CoverNoteStausKey = .CoverNoteStatusKey
                        r_oCoverNote.Code = .Code
                        r_oCoverNote.Description = .Description
                        r_oCoverNote.Comments = .Comments
                        r_oCoverNote.CoverNoteBookTimestamp = .CoverNoteBookTimestamp

                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetCoverNoteSheet executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("r_oCoverNote.CoverNoteBookKey" & r_oCoverNote.CoverNoteBookKey.ToString() & vbCrLf)
                    sbLogMessage.AppendLine("r_oCoverNote.CoverNoteSheets.CoverNoteSheetNumber" & r_oCoverNote.CoverNoteSheets(0).CoverNoteSheetNumber.ToString() & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If
                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetCoverNoteSheetRequest = Nothing
                oGetCovernoteSheetResponse = Nothing
            End Try


        End SyncLock

    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_oCoverNote"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub UpdateCoverNoteSheet(ByRef r_oCoverNote As CoverNote,
                                                Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock
            Dim oSAM As PureServiceClient
            Dim oUpdateCoverNoteSheetRequest As UpdateCoverNoteSheetRequestType
            Dim oUpdateCoverNoteSheetResponse As UpdateCoverNoteSheetResponseType
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oUpdateCoverNoteSheetRequest = New UpdateCoverNoteSheetRequestType
                oUpdateCoverNoteSheetResponse = New UpdateCoverNoteSheetResponseType
                sbLogMessage = New StringBuilder


                With oUpdateCoverNoteSheetRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode

                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    'Checking the CoverNoteBookKey
                    If r_oCoverNote.CoverNoteBookKey > 0 Then
                        .CoverNoteBookKey = r_oCoverNote.CoverNoteBookKey
                    Else
                        Throw New ArgumentNullException("CoverNote.CoverNoteBookKey")
                    End If

                    'Checking the OldCoverNoteSheetNumber
                    .OldCoverNoteSheetNumber = r_oCoverNote.CoverNoteSheets(0).OldCoverNoteSheetNumber


                    'Checking the NewCoverNoteSheetNumber
                    .NewCoverNoteSheetNumber = r_oCoverNote.CoverNoteSheets(0).NewCoverNoteSheetNumber

                    .CoverNoteBookKey = r_oCoverNote.CoverNoteBookKey

                    If r_oCoverNote.InsuranceFileDetails.InsuranceFileCnt > 0 Then
                        .InsuranceFileCnt = r_oCoverNote.InsuranceFileDetails.InsuranceFileCnt
                        .InsuranceFileCntSpecified = True
                    Else
                        .InsuranceFileCntSpecified = False
                    End If

                    .AssignedDate = r_oCoverNote.AssignedDate
                    .CoverNoteStatusCode = r_oCoverNote.CoverNoteStatusCode
                    .Comments = r_oCoverNote.Comments
                    .CoverNoteBookTimestamp = r_oCoverNote.CoverNoteBookTimestamp
                End With


                Using trace As New Tracer(Category.Trace)
                    oUpdateCoverNoteSheetResponse = oSAM.UpdateCoverNoteSheet(oUpdateCoverNoteSheetRequest)
                End Using

                With oUpdateCoverNoteSheetResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else

                        r_oCoverNote.CoverNoteBookTimestamp = .CoverNoteBookTimestamp
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("UpdateCoverNoteSheet executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("r_oCoverNote.CoverNoteBookKey" & r_oCoverNote.CoverNoteBookKey.ToString() & vbCrLf)
                    sbLogMessage.AppendLine("r_oCoverNote.CoverNoteSheets.CoverNoteSheetNumber" & r_oCoverNote.CoverNoteSheets(0).CoverNoteSheetNumber.ToString() & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If
                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oUpdateCoverNoteSheetRequest = Nothing
                oUpdateCoverNoteSheetResponse = Nothing
            End Try

        End SyncLock

    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_iEventKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEventNote(ByVal v_iEventKey As Integer,
                                            Optional ByVal v_sBranchCode As String = Nothing) As EventDetailsCollection
        SyncLock oLock
            Dim oSAM As PureServiceClient
            Dim oGetEventNoteRequest As GetEventNoteRequestType
            Dim oGetEventNoteResponse As GetEventNoteResponseType
            Dim oEventDetails As EventDetailsCollection = Nothing
            Dim oNewEventDetails As EventDetails
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oGetEventNoteRequest = New GetEventNoteRequestType
                oGetEventNoteResponse = New GetEventNoteResponseType
                oNewEventDetails = New EventDetails
                sbLogMessage = New StringBuilder


                With oGetEventNoteRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    'Checking the EventKey
                    If v_iEventKey > 0 Then
                        .EventKey = v_iEventKey
                    Else
                        Throw New ArgumentNullException("v_iEventKey")
                    End If

                End With

                Using trace As New Tracer(Category.Trace)
                    oGetEventNoteResponse = oSAM.GetEventNote(oGetEventNoteRequest)
                End Using

                With oGetEventNoteResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        oEventDetails = New EventDetailsCollection


                        If .EventNotes IsNot Nothing AndAlso .EventNotes.Count > 0 Then

                            For Each oEventDetailsRow As BaseGetEventNoteResponseTypeRow In .EventNotes
                                oNewEventDetails = New EventDetails
                                With oNewEventDetails
                                    .EventKey = oEventDetailsRow.EventKey
                                    .EventPublicTextKey = oEventDetailsRow.EventPublicTextKey
                                    .EventText = oEventDetailsRow.EventText
                                End With
                                oEventDetails.Add(oNewEventDetails)
                                oNewEventDetails = Nothing
                            Next

                        End If

                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetEventNoteResponse executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    If Not IsNothing(v_iEventKey) Then
                        sbLogMessage.AppendLine("v_iEventKey = " & v_iEventKey.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_iEventKey = nothing" & vbCrLf)
                    End If
                    sbLogMessage.AppendLine("Returned " & oEventDetails.Print.ToString & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetEventNoteRequest = Nothing
                oGetEventNoteResponse = Nothing
            End Try
            Return oEventDetails

        End SyncLock
    End Function
End Class
