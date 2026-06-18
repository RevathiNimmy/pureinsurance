Module MainModule

#Region " Constants "
    'Start - (Sankar) - (Tech Spec - PGR024 - Automated Processes.doc)
    Public Const ACApp As String = "CreditControlCLI"
    Private Const m_iDefaultLanguageID As Int32 = 1
    'End - (Sankar) - (Tech Spec - PGR024 - Automated Processes.doc)
#End Region

#Region " Variables "
    'Start - (Sankar) - (Tech Spec - PGR024 - Automated Processes.doc) - (5.1.1.1)
    Private m_oBusiness As bACTFinanceSpoke.Business
    Private m_dtProcessingDate As DateTime
    Private m_sBranch As String
    Private m_bSpoolDocuments As Boolean
    Private m_bArchiveDocuments As Boolean
    'End - (Sankar) - (Tech Spec - PGR024 - Automated Processes.doc) - (5.1.1.1)
#End Region

#Region " Properties "
    'Start - (Sankar) - (Tech Spec - PGR024 - Automated Processes.doc) - (5.1.1.1)
    Public Property ProcessingDate() As DateTime
        Get
            Return m_dtProcessingDate
        End Get
        Set(ByVal value As DateTime)
            m_dtProcessingDate = value
        End Set
    End Property

    Public Property Branch() As String
        Get
            Return m_sBranch
        End Get
        Set(ByVal value As String)
            m_sBranch = value
        End Set
    End Property

    Public Property SpoolDocuments() As Boolean
        Get
            Return m_bSpoolDocuments
        End Get
        Set(ByVal value As Boolean)
            m_bSpoolDocuments = value
        End Set
    End Property

    Public Property ArchiveDocuments() As Boolean
        Get
            Return m_bArchiveDocuments
        End Get
        Set(ByVal value As Boolean)
            m_bArchiveDocuments = value
        End Set
    End Property
    'End - (Sankar) - (Tech Spec - PGR024 - Automated Processes.doc) - (5.1.1.1)
#End Region

#Region " Main "

    'Start - (Sankar) - (Tech Spec - PGR024 - Automated Processes.doc) - (5.1.1.2)
    Sub Main(ByVal args() As String)

        Const kiCurrencyId As Integer = 26
        Const kiUserID As Short = 1

        Const kbHDBranch As Byte = 9
        Const kbHDAsOfDate As Byte = 10
        Const kbHDSpoolDoc As Byte = 11
        Const kbHDArchiveDoc As Byte = 12

        Const ACSpokeStatusCode As Char = CChar("A")
        Const ACSpokeBatch As Char = CChar("")
        Const ACSpokeHeaderXML As String = "<XML>"
        Const ACSpokeDetailData As Char = CChar("")
        Const ACSpokeMessage As Char = CChar("A")
        Const ACSpokeInterfaceCode As String = "CREDITCONTROL"

        Dim bReturn As Boolean
        Dim lBranchId As Short
        Dim lResult As Long
        Dim vHeaderData(1) As Object
        Dim vHeaderDetail(12) As Object

        Try
            If My.Application.CommandLineArgs.Count = 0 Then
                OutputSyntax()
                Exit Sub
            End If

            bReturn = ProcessArguments()
            If Not bReturn Then
                OutputSyntax()
                Exit Sub
            Else
                lBranchId = GetBranchIdFromCode()
                m_oBusiness = New bACTFinanceSpoke.Business

                lResult = m_oBusiness.Initialise(sUsername:="sirius", _
                                sPassword:="", _
                                iUserID:=kiUserID, _
                                iSourceID:=lBranchId, _
                                iLanguageID:=m_iDefaultLanguageID, _
                                iCurrencyID:=kiCurrencyId, _
                                iLogLevel:=Convert.ToInt16(PMELogLevel.PMLogError), _
                                sCallingAppName:=ACApp)
                If lResult <> PMEReturnCode.PMTrue Then
                    Throw New Exception("Unable to initialise bSIROptions.Business")
                End If

                vHeaderDetail(kbHDBranch) = Branch
                vHeaderDetail(kbHDAsOfDate) = ProcessingDate
                vHeaderDetail(kbHDSpoolDoc) = SpoolDocuments
                vHeaderDetail(kbHDArchiveDoc) = ArchiveDocuments
                vHeaderData(0) = "SIRIUS"
                vHeaderData(1) = vHeaderDetail
                SharedFiles.bPMDocFunctions.IsCalledFromBatchProcess = True
                lResult = m_oBusiness.Export(v_sInterfaceCode:=ACSpokeInterfaceCode,
                                                r_sBatchRef:=ACSpokeBatch,
                                                r_sStatusCode:=ACSpokeStatusCode,
                                                r_sMessage:=ACSpokeMessage,
                                                r_sHeaderXML:=ACSpokeHeaderXML,
                                                r_vHeaderData:=CType(vHeaderData, Object),
                                                r_vDetailData:=ACSpokeDetailData,
                                                bCreateBatch:=True)
                SharedFiles.bPMDocFunctions.IsCalledFromBatchProcess = False
                If lResult = PMEReturnCode.PMTrue Then
                    Console.WriteLine("Credit Control Complete - " & Now)
                ElseIf lResult = PMEReturnCode.PMNotFound Then
                    Console.WriteLine("Credit Control Complete - No Records Found - " & Now)
                Else
                    Console.WriteLine("Credit Control Failed - " & Now)
                End If
            End If

            'Comment this code
            'System.Console.ReadLine()
            m_oBusiness.Dispose()
        Catch ex As Exception
            Console.WriteLine("Credit Control Failed - " & ex.Message)
        End Try
    End Sub
    'End - (Sankar) - (Tech Spec - PGR024 - Automated Processes.doc) - (5.1.1.2)
#End Region

#Region " Functions "
    'Start - (Sankar) - (Tech Spec - PGR024 - Automated Processes.doc) - (5.1.1.4)
    Private Function ProcessArguments() As Boolean

        Dim iItem As Integer = 0
        Dim sArg As String
        Dim sArgValues() As String

        Try
            ProcessArguments = True

            For iItem = 0 To My.Application.CommandLineArgs.Count - 1

                sArg = My.Application.CommandLineArgs.Item(iItem)
                sArgValues = sArg.Split(CChar("="))

                Select Case sArgValues(0).ToUpper
                    Case "PROCESSINGDATE"
                        If IsDate(sArgValues(1)) Then
                            ProcessingDate = CType(sArgValues(1), DateTime)
                        ElseIf sArgValues(1).ToUpper = "CURRENTDATE" Then
                            ProcessingDate = DateTime.Now
                        Else
                            ProcessArguments = False
                            Console.WriteLine("Invalid argument for ProcessingDate - " & sArgValues(1))
                            'Throw New ArgumentException("Invalid argument - " & sArgValues(1), "ProcessingDate")
                        End If
                    Case "BRANCH"
                        If String.IsNullOrEmpty(sArgValues(1)) Then
                            ProcessArguments = False
                            Console.WriteLine("Invalid argument for Branch - " & sArgValues(1))
                        Else
                            Branch = sArgValues(1).ToString
                        End If
                    Case "SPOOLDOCUMENTS"
                        If (String.IsNullOrEmpty(sArgValues(1))) Then
                            SpoolDocuments = False
                        ElseIf (sArgValues(1).ToUpper = "TRUE" Or sArgValues(1).ToUpper = "FALSE") Then
                            SpoolDocuments = Convert.ToBoolean(sArgValues(1))
                        Else
                            ProcessArguments = False
                            Console.WriteLine("Invalid argument for SpoolDocuments - " & sArgValues(1))
                        End If
                    Case "ARCHIVEDOCUMENTS"
                        If (String.IsNullOrEmpty(sArgValues(1))) Then
                            ArchiveDocuments = False
                        ElseIf (sArgValues(1).ToUpper = "TRUE" Or sArgValues(1).ToUpper = "FALSE") Then
                            ArchiveDocuments = Convert.ToBoolean(sArgValues(1))
                        Else
                            ProcessArguments = False
                            Console.WriteLine("Invalid argument for ArchiveDocuments - " & sArgValues(1))
                        End If
                    Case Else
                        ProcessArguments = False
                        Console.WriteLine("Invalid argument - " & sArgValues(0))
                End Select
            Next

            If (ProcessingDate <= DateTime.MinValue Or ProcessingDate > DateTime.MaxValue) Then
                Console.WriteLine("ProcessingDate is Mandatory")
                ProcessArguments = False
            End If
            If String.IsNullOrEmpty(Branch) Then
                ProcessArguments = False
                Console.WriteLine("Branch is Mandatory")
            End If
        Catch ex As Exception
            Console.WriteLine("ProcessArguments Failed - " + ex.Message)
            Throw New Exception("ProcessArguments Failed - " + ex.Message)
        End Try
        'End - (Sankar) - (Tech Spec - PGR024 - Automated Processes.doc) - (5.1.1.4)
    End Function

    'Start - (Sankar) - (Tech Spec - PGR024 - Automated Processes.doc) - (5.1.1.5)
    Private Function GetBranchIdFromCode() As Short

        Dim oDatabase As dPMDAO.Database = Nothing
        Dim vResults As Object = Nothing
        Dim oAllBranch(,) As Object
        Dim iReturnCode As Integer
        Dim iCounter As Integer
        Dim bBranchIdFound As Boolean

        Try
            GetBranchIdFromCode = 0

            DBConnect(oDatabase)

            AddParameterLite(oDatabase, "tablename", "source", PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)
            AddParameterLite(oDatabase, "effective_date", DateTime.Now, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
            AddParameterLite(oDatabase, "language_id", m_iDefaultLanguageID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
            AddParameterLite(oDatabase, "id", DBNull.Value, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)

            iReturnCode = oDatabase.SQLSelect("spu_pm_get_lookups", "spu_pm_get_lookups", True, vResultArray:=vResults)

            If Not IsNothing(vResults) Then
                oAllBranch = DirectCast(vResults, Object(,))
                For iCounter = 0 To UBound(oAllBranch, 2)
                    If oAllBranch(2, iCounter).ToString().ToUpper() = Branch.ToString().ToUpper() Then
                        bBranchIdFound = True
                        GetBranchIdFromCode = Convert.ToInt16(oAllBranch(0, iCounter))
                        Exit For
                    End If
                Next
            End If
            DBDisconnect(oDatabase)
            oDatabase = Nothing

            If Not bBranchIdFound Then
                Throw New Exception("LookUp Validation Failed - Branch Not Found: " & Branch)
            End If

        Catch ex As Exception
            System.Console.WriteLine("GetBranchIdFromCode Failed - " + ex.Message)
            Throw ex
        End Try

    End Function
    'End - (Sankar) - (Tech Spec - PGR024 - Automated Processes.doc) - (5.1.1.5)
#End Region

#Region " Procedures "
    'Start - (Sankar) - (Tech Spec - PGR024 - Automated Processes.doc)
    Private Sub OutputSyntax()
        Console.WriteLine("Syntax: CreditControlCLI processingdate spooldocuments archivedocuments")
        Console.WriteLine("processingdate:  The date the credit control processing is to be run for")
        Console.WriteLine("branch:          The branch code for credit control")
        Console.WriteLine("spooldocuments:  (optional) defaults to False. Determines if documents should be spooled")
        Console.WriteLine("archivedocuments:(optional) defaults to False. Determines if documents should be archived")
        Console.WriteLine("Example: CreditControlCLI processingdate=25/12/08 branch=MAIN spooldocuments=true archivedocuments=false")
    End Sub
    'End - (Sankar) - (Tech Spec - PGR024 - Automated Processes.doc)
#End Region

End Module
