Option Strict On
Module MainModule

#Region " Constants "
    Const kiCurrencyId As Integer = 26
    Const kiUserID As Short = 1
    Const kAuthAccountsTransOptionNo As Integer = 81
    Public Const ACApp As String = "PeriodEndCLI"
    Private Const m_iDefaultLanguageID As Int32 = 1
    Private Const MAXLedger As Integer = 3
#End Region

#Region " Variables "
    'Business Objects
    Private m_oBusinessLedger As bACTLedger.Form
    Private m_oBusinessPeriodEnd As bACTPeriodEnd.Form
    Private m_oBusinessPeriod As bACTPeriod.Form

    Private m_iLedgerId As Integer
    Private m_iSequence As Integer
    Private m_iCompanyId As Integer
    Private m_sIncludeYearEnd As String
    Private m_iSourceId As Integer
    Private m_sBranch As String
    Private m_iSubBranchId As Integer

#End Region

#Region " Properties "

    Public Property Branch() As String
        Get
            Return m_sBranch
        End Get
        Set(ByVal value As String)
            m_sBranch = value
        End Set
    End Property

    Public Property IncludeYearEnd() As String
        Get
            Return m_sIncludeYearEnd
        End Get
        Set(ByVal value As String)
            m_sIncludeYearEnd = value
        End Set
    End Property

    Public Property SourceId() As Integer
        Get
            Return m_iSourceId
        End Get
        Set(ByVal value As Integer)
            m_iSourceId = value
        End Set
    End Property

    Public Property SubBranchId() As Integer
        Get
            Return m_iSubBranchId
        End Get
        Set(ByVal value As Integer)
            m_iSubBranchId = value
        End Set
    End Property

#End Region

#Region " Main "

    Sub Main(ByVal args() As String)

        Const kbHDLedgerID As Byte = 0
        Const kbHDLedgerName As Byte = 1
        Const kbHDLedgerShortName As Byte = 2
        Const kbHDMappingID As Byte = 3
        Const kbHDLedgertypeID As Byte = 4
        Const kbHDIsDeletable As Byte = 5
        Const kbHDCurrentPeriodID As Byte = 6
        Const kbHDSequence As Byte = 7
        Const kbHDYearName As Byte = 8
        Const kbHDPeriodName As Byte = 9
        Const kbHDPeriodEndDate As Byte = 10
        Const kbHDPeriodEndComplete As Byte = 11
        Const kbHDAdvanceRetreat As Byte = 12

        Const kClientReorderPosition As Byte = 0
        Const kPurchaseReorderPosition As Byte = 1
        Const kNominalReorderPosition As Byte = 2

        Dim bReturn As Boolean
        Dim lResult As Long
        Dim sLedgerName As String
        Dim sLedgerShortName As String
        Dim iMappingID As Integer
        Dim iLedgerTypeID As Integer
        Dim lCurrentPeriodID As Long
        Dim lBranchId As Integer
        Dim iIsDeletable As Integer
        Dim iSequence As Integer
        Dim bResult As Boolean
        Dim iLoop As Integer
        Dim sYearName As String
        Dim sPeriodName As String
        Dim dPeriodEndDate As Date
        Dim iPeriodEndComplete As Integer
        Dim iNextPeriodId As Integer
        Dim vLedgerID As Object
        Dim vLedgerName As Object
        Dim vLedgerShortName As Object
        Dim vMappingID As Object
        Dim vLedgertypeID As Object
        Dim vIsDeletable As Object
        Dim vCurrentPeriodID As Object
        Dim vSequence As Object
        Dim vYearName As Object
        Dim vPeriodName As Object
        Dim vPeriodEndDate As Object
        Dim vPeriodEndComplete As Object
        Dim vLedgerArray As Object(,)
        Dim vLedgerArrayReOrder As Object(,)
        Dim lRow As Long
        Dim iPreviousPeriodID As Integer
        Dim isPreviousPeriodEndComplete As Integer
        Dim bAllowYearEnd As Boolean
        Dim sAdvanceOrRetreat As String
        Dim iArrayPosition As Integer

        Try

            'Check Whether Bad Parameter has passed
            bReturn = OutputBadParameterPassed()
            If Not bReturn Then
                Console.WriteLine("Unable To Run Period End Process : As Unrecognised Bad Parameter Passed")
                Exit Sub
            End If

            'Set values from Parameters supplied
            bReturn = ProcessArguments()
            If Not bReturn Then
                Exit Sub
            Else
                lBranchId = GetBranchIdFromCode()
                SourceId = lBranchId

                m_oBusinessLedger = New bACTLedger.Form
                m_oBusinessPeriodEnd = New bACTPeriodEnd.Form
                m_oBusinessPeriod = New bACTPeriod.Form

                'Instantiate Business Components
                bResult = InstantiateBusinessComponents()
                If bResult = False Then
                    Console.WriteLine("Unable To Run Period End Process : Business Component Initialise")
                    Exit Sub
                End If

                'Check if Some Unposted Transaction left
                'Checks if system option is ticked
                bResult = PostUnpostedTrans()
                If bResult = False Then
                    OutputSyntax()
                    Exit Sub
                End If

                lResult = m_oBusinessLedger.GetDetails( _
                                vSubBranchID:=CObj(lBranchId))
                If lResult <> PMEReturnCode.PMTrue Then
                    Throw New Exception("Unable to fetch bACTLedger GetDetails")
                End If

                ReDim Preserve vLedgerArray(MAXLedger - 1, kbHDAdvanceRetreat)
                ReDim Preserve vLedgerArrayReOrder(MAXLedger - 1, kbHDAdvanceRetreat)
                For iLoop = 1 To MAXLedger
                    lResult = m_oBusinessLedger.GetNext( _
                                vLedgerID:=vLedgerID, _
                                vCompanyID:=CObj(SourceId), _
                                vLedgerName:=vLedgerName, _
                                vLedgerShortName:=vLedgerShortName, _
                                vMappingID:=vMappingID, _
                                vLedgertypeID:=vLedgertypeID, _
                                vIsDeletable:=vIsDeletable, _
                                vCurrentPeriodID:=vCurrentPeriodID, _
                                vSequence:=vSequence)
                    If lResult <> PMEReturnCode.PMTrue Then
                        Throw New Exception("Unable to fetch bACTLedger GetNext")
                    End If

                    sLedgerName = CStr(vLedgerName)
                    sLedgerShortName = CStr(vLedgerShortName)
                    iMappingID = CInt(vMappingID)
                    iLedgerTypeID = CInt(vLedgertypeID)
                    iIsDeletable = CInt(vIsDeletable)
                    lCurrentPeriodID = CLng(vCurrentPeriodID)
                    iSequence = CInt(vSequence)

                    lResult = m_oBusinessPeriod.GetDetails( _
                                        vPeriodID:=vCurrentPeriodID)
                    If lResult <> PMEReturnCode.PMTrue Then
                        Throw New Exception("Unable to fetch bACTPeriod GetDetails")
                    End If

                    lResult = m_oBusinessPeriod.GetNext( _
                                vPeriodID:=vCurrentPeriodID, _
                                vCompanyID:=CObj(SourceId), _
                                vYearName:=vYearName, _
                                vPeriodName:=vPeriodName, _
                                vPeriodEndDate:=vPeriodEndDate, _
                                vPeriodEndComplete:=vPeriodEndComplete)
                    If lResult <> PMEReturnCode.PMTrue Then
                        Throw New Exception("Unable to fetch bACTPeriod GetNext")
                    End If

                    lCurrentPeriodID = CLng(vCurrentPeriodID)
                    sYearName = CStr(vYearName)
                    sPeriodName = CStr(vPeriodName)
                    dPeriodEndDate = CDate(vPeriodEndDate)
                    iPeriodEndComplete = CInt(vPeriodEndComplete)
                    vLedgerArray(iLoop - 1, kbHDLedgerID) = vLedgerID
                    vLedgerArray(iLoop - 1, kbHDLedgerName) = vLedgerName
                    vLedgerArray(iLoop - 1, kbHDLedgerShortName) = vLedgerShortName
                    vLedgerArray(iLoop - 1, kbHDMappingID) = vMappingID
                    vLedgerArray(iLoop - 1, kbHDLedgertypeID) = vLedgertypeID
                    vLedgerArray(iLoop - 1, kbHDIsDeletable) = vIsDeletable
                    vLedgerArray(iLoop - 1, kbHDCurrentPeriodID) = vCurrentPeriodID
                    vLedgerArray(iLoop - 1, kbHDSequence) = vSequence
                    vLedgerArray(iLoop - 1, kbHDYearName) = vYearName
                    vLedgerArray(iLoop - 1, kbHDPeriodName) = vPeriodName
                    vLedgerArray(iLoop - 1, kbHDPeriodEndDate) = vPeriodEndDate
                    vLedgerArray(iLoop - 1, kbHDPeriodEndComplete) = vPeriodEndComplete

                    'Set Advance and Retreat
                    If (lCurrentPeriodID <> 1) Then
                        iPreviousPeriodID = 0
                        isPreviousPeriodEndComplete = 0

                        lResult = m_oBusinessPeriodEnd.GetPreviousPeriodEndComplete( _
                                v_lCurrentPeriodID:=CInt(lCurrentPeriodID), _
                                r_lPreviousPeriodID:=iPreviousPeriodID, _
                                r_iPreviousPeriodEndComplete:=isPreviousPeriodEndComplete)
                        If lResult <> PMEReturnCode.PMTrue Then
                            Throw New Exception("Unable to fetch bACTPeriodEnd GetPreviousPeriodEndComplete")
                        End If

                        If (CInt(vPeriodEndComplete) = 0) Then
                            If (isPreviousPeriodEndComplete = 1) Then
                                vLedgerArray(iLoop - 1, kbHDAdvanceRetreat) = "ADVANCE"
                            Else
                                vLedgerArray(iLoop - 1, kbHDAdvanceRetreat) = "RETREAT"
                            End If
                        Else
                            vLedgerArray(iLoop - 1, kbHDAdvanceRetreat) = "RETREAT"
                        End If
                    Else
                        vLedgerArray(iLoop - 1, kbHDAdvanceRetreat) = "ADVANCE"
                    End If
                Next iLoop

                'Validation - Legers should be in Advance mode                
                sAdvanceOrRetreat = CStr(vLedgerArray(0, kbHDAdvanceRetreat))
                For iLoop = 2 To MAXLedger
                    If vLedgerArray(iLoop - 1, kbHDAdvanceRetreat) Is sAdvanceOrRetreat And sAdvanceOrRetreat = "ADVANCE" Then
                        'Do Nothing
                    Else
                        Console.WriteLine("Period/Year End Not Completed : As all Ledgers should be in Advance")
                        Console.WriteLine("They Should All be in Advance State")
                        Exit Sub
                    End If
                Next

                'Validation
                bResult = Validation(v_vYearName:=vYearName, _
                                    v_iPeriodEndComplete:=iPeriodEndComplete, _
                                    v_vPeriodEndDate:=vPeriodEndDate, _
                                    v_sPeriodName:=sPeriodName)
                If bResult = False Then
                    Console.WriteLine("Unable To Year End Process : Validation Failed")
                    Exit Sub
                End If

                'Next Year period should be configured                    
                bResult = CheckNextYearPeriodConfigured(vYearName:=vYearName)
                If bResult = False Then
                    Console.WriteLine("Unable To Year End Process : Next Year Periods not configured")
                    Exit Sub
                End If
                'Validation End

                'ReOrdering Array Order should be - Client, Purchase & Nominal
                For iLoop = 1 To MAXLedger
                    If CStr(vLedgerArray(iLoop - 1, kbHDLedgerName)).Trim.ToUpper = "CLIENT" Then
                        iArrayPosition = kClientReorderPosition
                    ElseIf CStr(vLedgerArray(iLoop - 1, kbHDLedgerName)).Trim.ToUpper = "PURCHASE" Then
                        iArrayPosition = kPurchaseReorderPosition
                    ElseIf CStr(vLedgerArray(iLoop - 1, kbHDLedgerName)).Trim.ToUpper = "NOMINAL" Then
                        iArrayPosition = kNominalReorderPosition
                    End If
                    vLedgerArrayReOrder(iArrayPosition, kbHDLedgerID) = vLedgerArray(iLoop - 1, kbHDLedgerID)
                    vLedgerArrayReOrder(iArrayPosition, kbHDLedgerName) = vLedgerArray(iLoop - 1, kbHDLedgerName)
                    vLedgerArrayReOrder(iArrayPosition, kbHDLedgerShortName) = vLedgerArray(iLoop - 1, kbHDLedgerShortName)
                    vLedgerArrayReOrder(iArrayPosition, kbHDMappingID) = vLedgerArray(iLoop - 1, kbHDMappingID)
                    vLedgerArrayReOrder(iArrayPosition, kbHDLedgertypeID) = vLedgerArray(iLoop - 1, kbHDLedgertypeID)
                    vLedgerArrayReOrder(iArrayPosition, kbHDIsDeletable) = vLedgerArray(iLoop - 1, kbHDIsDeletable)
                    vLedgerArrayReOrder(iArrayPosition, kbHDCurrentPeriodID) = vLedgerArray(iLoop - 1, kbHDCurrentPeriodID)
                    vLedgerArrayReOrder(iArrayPosition, kbHDSequence) = vLedgerArray(iLoop - 1, kbHDSequence)
                    vLedgerArrayReOrder(iArrayPosition, kbHDYearName) = vLedgerArray(iLoop - 1, kbHDYearName)
                    vLedgerArrayReOrder(iArrayPosition, kbHDPeriodName) = vLedgerArray(iLoop - 1, kbHDPeriodName)
                    vLedgerArrayReOrder(iArrayPosition, kbHDPeriodEndDate) = vLedgerArray(iLoop - 1, kbHDPeriodEndDate)
                    vLedgerArrayReOrder(iArrayPosition, kbHDPeriodEndComplete) = vLedgerArray(iLoop - 1, kbHDPeriodEndComplete)
                    vLedgerArrayReOrder(iArrayPosition, kbHDAdvanceRetreat) = vLedgerArray(iLoop - 1, kbHDAdvanceRetreat)
                Next
                'Reorder Complete
                vLedgerArray = vLedgerArrayReOrder

                'Advance to Retreat
                For iLoop = 1 To MAXLedger
                    lRow = 0
                    'If Advance then run edit update and no action on Retreat
                    If vLedgerArray(iLoop - 1, 12) Is "ADVANCE" Then
                        lResult = m_oBusinessPeriod.GetNextPeriodID(lPeriodID:=CInt(lCurrentPeriodID), _
                                                                        lNextPeriodID:=iNextPeriodId)
                        If lResult <> PMEReturnCode.PMTrue Then
                            Throw New Exception("Unable to fetch bACTPeriod GetNextPeriodID")
                        End If

                        If iNextPeriodId > lCurrentPeriodID Then

                            vLedgerID = vLedgerArray(iLoop - 1, kbHDLedgerID)
                            vLedgerName = vLedgerArray(iLoop - 1, kbHDLedgerName)
                            vLedgerShortName = vLedgerArray(iLoop - 1, kbHDLedgerShortName)
                            vMappingID = vLedgerArray(iLoop - 1, kbHDMappingID)
                            vLedgertypeID = vLedgerArray(iLoop - 1, kbHDLedgertypeID)
                            vIsDeletable = vLedgerArray(iLoop - 1, kbHDIsDeletable)
                            vCurrentPeriodID = vLedgerArray(iLoop - 1, kbHDCurrentPeriodID)
                            vSequence = vLedgerArray(iLoop - 1, kbHDSequence)

                            lResult = m_oBusinessLedger.GetClosures( _
                                    vLedgerID:=vLedgerID)
                            If lResult <> PMEReturnCode.PMTrue Then
                                Throw New Exception("Unable to fetch bACTLedger GetClosures")
                            End If

                            While lResult = PMEReturnCode.PMTrue                'for all ledgers say (1-12)                                

                                lResult = m_oBusinessLedger.GetNext( _
                                        vLedgerID:=vLedgerID, _
                                        vCompanyID:=CObj(SourceId), _
                                        vSubBranchID:=CObj(SubBranchId), _
                                        vLedgerName:=vLedgerName, _
                                        vLedgerShortName:=vLedgerShortName, _
                                        vMappingID:=vMappingID, _
                                        vLedgertypeID:=vLedgertypeID, _
                                        vIsDeletable:=vIsDeletable, _
                                        vCurrentPeriodID:=vCurrentPeriodID, _
                                        vSequence:=vSequence)
                                'No need to handle error
                                'If lResult <> PMEReturnCode.PMTrue Then
                                'Throw New Exception("Unable to fetch bACTLedger GetNext")                                
                                'End If

                                'Will give next period id                            
                                Select Case lResult
                                    Case PMEReturnCode.PMEOF
                                    Case PMEReturnCode.PMTrue
                                        lRow = lRow + 1
                                        vCurrentPeriodID = iNextPeriodId    'Next Period Id
                                        lResult = m_oBusinessLedger.EditUpdate( _
                                                lRow:=CInt(lRow), _
                                                vLedgerID:=vLedgerID, _
                                                vCompanyID:=CObj(SourceId), _
                                                vSubBranchID:=CObj(SubBranchId), _
                                                vLedgerName:=vLedgerName, _
                                                vLedgerShortName:=vLedgerShortName, _
                                                vMappingID:=vMappingID, _
                                                vLedgertypeID:=vLedgertypeID, _
                                                vIsDeletable:=vIsDeletable, _
                                                vCurrentPeriodID:=vCurrentPeriodID, _
                                                vSequence:=vSequence)
                                        If (lResult <> PMEReturnCode.PMTrue) Then
                                            Exit While
                                        End If

                                        lResult = m_oBusinessLedger.Update()
                                        If (lResult <> PMEReturnCode.PMTrue) Then
                                            Exit While
                                        End If
                                    Case Else
                                        Throw New Exception("Unable to fetch bACTLedger GetNext")
                                End Select
                            End While
                        End If
                    End If
                Next

                'Period/Year End
                If sAdvanceOrRetreat Is "RETREAT" Then
                    iPreviousPeriodID = CInt(vCurrentPeriodID) - 1
                Else
                    iPreviousPeriodID = CInt(lCurrentPeriodID)
                End If

                lResult = m_oBusinessPeriod.GetDetails( _
                                    vPeriodID:=vCurrentPeriodID)
                If lResult <> PMEReturnCode.PMTrue Then
                    Throw New Exception("Unable to fetch bACTPeriod GetDetails")
                End If

                lResult = m_oBusinessPeriod.GetNext( _
                            vPeriodID:=vCurrentPeriodID, _
                            vCompanyID:=CObj(SourceId), _
                            vYearName:=vYearName, _
                            vPeriodName:=vPeriodName, _
                            vPeriodEndDate:=vPeriodEndDate, _
                            vPeriodEndComplete:=vPeriodEndComplete)
                If lResult <> PMEReturnCode.PMTrue Then
                    Throw New Exception("Unable to fetch bACTPeriod GetNext")
                End If

                lResult = m_oBusinessPeriodEnd.ProcessPeriodEnd( _
                            v_lCurrentPeriodID:=CInt(vCurrentPeriodID), _
                            v_lPreviousPeriodId:=iPreviousPeriodID)
                If lResult <> PMEReturnCode.PMTrue Then
                    Throw New Exception("Unable to fetch bACTPeriodEnd ProcessPeriodEnd")
                End If
                Console.WriteLine("Period End - Completed : " & CStr(vPeriodName))

                lResult = m_oBusinessPeriodEnd.AllowYearEnd(iPreviousPeriodID, bAllowYearEnd)
                If lResult <> PMEReturnCode.PMTrue Then
                    Throw New Exception("Unable to fetch bACTPeriodEnd AllowYearEnd")
                End If

                If bAllowYearEnd = True Then
                    IncludeYearEnd = "TRUE"
                End If

                If bAllowYearEnd = True Then
                    If IncludeYearEnd.ToUpper = "TRUE" Then
                        Console.WriteLine("Year End in Progress...")
                        lResult = m_oBusinessPeriodEnd.ProcessRetainedProfitJournal(v_lPeriodID:=iPreviousPeriodID)
                        If lResult <> PMEReturnCode.PMTrue Then
                            Throw New Exception("Unable to fetch bACTPeriodEnd ProcessRetainedProfitJournal")
                        End If
                        Console.WriteLine("Year End - Completed : " & CStr(vYearName))
                    Else 'Not True
                        Console.WriteLine("Year End - Not Completed as IncludeYearEnd Argument not passed with YES value")
                    End If
                End If

            End If

        Catch ex As Exception
            Console.WriteLine("Period End Failed - " & ex.Message)
        Finally
            'Not any use of this code
            'System.Console.ReadLine()
            If Not (m_oBusinessLedger Is Nothing) Then
                m_oBusinessLedger.Dispose()
            End If
            If Not (m_oBusinessPeriod Is Nothing) Then
                m_oBusinessPeriod.Dispose()
            End If
            If Not (m_oBusinessPeriodEnd Is Nothing) Then
                m_oBusinessPeriodEnd.Dispose()
            End If
        End Try

    End Sub

#End Region

#Region " Functions "

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
                    Case "INCLUDEYEAREND"
                        If String.IsNullOrEmpty(sArgValues(1)) Then
                            ProcessArguments = False
                            Console.WriteLine("Invalid argument for IncludeYearEnd - " & sArgValues(1))
                        Else
                            IncludeYearEnd = sArgValues(1).ToString.ToUpper
                        End If
                    Case "BRANCH"
                        If String.IsNullOrEmpty(sArgValues(1)) Then
                            ProcessArguments = False
                            Console.WriteLine("Invalid argument for Branch - " & sArgValues(1))
                        Else
                            Branch = sArgValues(1).ToString
                        End If
                End Select
            Next
            If String.IsNullOrEmpty(Branch) Then
                Branch = "HeadOff" 'Default                
            End If

            If IncludeYearEnd <> "" Then
                Select Case IncludeYearEnd
                    Case "TRUE"
                        'Nothing
                    Case "FALSE"
                        'Nothing
                    Case Else
                        Console.WriteLine("Invalid value passed to Argurment - IncludeYearEnd")
                        Console.WriteLine("This should either be True Or False")
                        ProcessArguments = False
                End Select
            End If

            If String.IsNullOrEmpty(IncludeYearEnd) Then
                IncludeYearEnd = "FALSE" 'As Default                
            End If

        Catch ex As Exception
            Console.WriteLine("ProcessArguments Failed - " + ex.Message)
            Throw New Exception("ProcessArguments Failed - " + ex.Message)
        End Try

    End Function


    Private Function GetBranchIdFromCode() As Integer

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
                        GetBranchIdFromCode = Convert.ToInt32(oAllBranch(0, iCounter))
                        Exit For
                    End If
                Next
            End If

            If GetBranchIdFromCode > 0 Then
                vResults = Nothing
                oDatabase.Parameters.Clear()
                AddParameterLite(oDatabase, "source_id", GetBranchIdFromCode, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
                iReturnCode = oDatabase.SQLSelect("spu_sub_branch_sel", "spu_sub_branch_sel", True, vResultArray:=vResults)

                If Not IsNothing(vResults) Then
                    oAllBranch = DirectCast(vResults, Object(,))
                    'Pick only first one
                    For iCounter = 0 To 0
                        SubBranchId = Convert.ToInt32(oAllBranch(0, iCounter))
                    Next
                End If
            End If

            DBDisconnect(oDatabase)
            oDatabase = Nothing

            If Not bBranchIdFound Then
                Throw New Exception("LookUp Validation Failed - Branch Not Found: " & Branch)
            End If

            If SubBranchId < 1 Then
                GetBranchIdFromCode = 0
                Throw New Exception("LookUp Validation Failed - Sub Branch Not Found: " & Branch)
            End If

        Catch ex As Exception
            System.Console.WriteLine("GetBranchIdFromCode Failed - " + ex.Message)
            Throw ex
        End Try

    End Function


    Private Function PostUnpostedTrans() As Boolean

        Dim iReturnCode As Integer
        Dim sAuthAccountsTransOptionVal As String
        Dim oUnpostedTrans As bSIRUnpostedTransactions.Business
        Dim vUnpostedTrans(,) As Object

        Try
            PostUnpostedTrans = True
            sAuthAccountsTransOptionVal = GetSystemOption(v_iOptionNumber:=kAuthAccountsTransOptionNo)

            If CLng(sAuthAccountsTransOptionVal) = PMEReturnCode.PMTrue Then
                'now check for any o/s transactions
                'create the business object

                ' Get an instance of the business object via
                ' the public object manager.
                oUnpostedTrans = New bSIRUnpostedTransactions.Business

                'call the GetDetails method to return any o/s transactions
                iReturnCode = oUnpostedTrans.GetDetails(r_vResultArray:=vUnpostedTrans)

                If (iReturnCode <> PMEReturnCode.PMTrue) Then
                    PostUnpostedTrans = False
                    Console.WriteLine("GetDetails for Unposted Transaction failed")
                    Exit Function
                End If

                If IsArray(vUnpostedTrans) Then
                    'there are unposted transactions outstanding so period end cannot be completed
                    MsgBox("There are still unposted transactions waiting to be approved." & vbCrLf & vbCrLf & _
                        "These must be actioned before the Accounting Period can be advanced.", vbCritical, "Unposted Transactions Present")

                    Console.WriteLine("There are still unposted transactions waiting to be approved." & vbCrLf & vbCrLf & _
                    "These must be actioned before the Accounting Period can be advanced. Unposted Transactions Present")
                    oUnpostedTrans.Dispose()
                    oUnpostedTrans = Nothing
                    PostUnpostedTrans = False
                    Exit Function
                End If

                oUnpostedTrans.Dispose()
                oUnpostedTrans = Nothing

            End If

        Catch ex As Exception
            System.Console.WriteLine("PostUnpostedTrans Failed - " + ex.Message)
            Throw ex
        End Try

    End Function

    Public Function GetSystemOption(ByVal v_iOptionNumber As Integer) As String
        Dim lResult As Long = 0
        Dim oSystemOptions As bSIROptions.Business = Nothing
        Dim sOptionValue As String = String.Empty

        Try

            ' Create the System Options Object
            oSystemOptions = New bSIROptions.Business
            If (oSystemOptions Is Nothing) Then
                Throw New Exception("Unable to create bSIROptions.Business")
            End If

            ' Initialise
            lResult = oSystemOptions.Initialise(sUsername:="", sPassword:="", iUserID:=0, iSourceID:=SourceId, iLanguageID:=1, iCurrencyID:=26, iLogLevel:=PMELogLevel.PMLogError, sCallingAppName:=ACApp)
            If lResult <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to initialise bSIROptions.Business")
            End If

            ' Get the system option
            lResult = oSystemOptions.GetOption( _
                iOptionNumber:=CShort(v_iOptionNumber), _
                sValue:=sOptionValue, _
                v_iSourceID:=CShort(SourceId))
            If lResult <> PMEReturnCode.PMTrue Then
                Throw New Exception(String.Format("Unable to retrieve system option '{0}'", v_iOptionNumber))
            End If

            ' Return the option value
            Return sOptionValue

        Catch ex As Exception
            Throw New Exception("Unable to retrieve system option", ex)

        Finally
            If Not oSystemOptions Is Nothing Then
                oSystemOptions.Dispose()
            End If
            oSystemOptions = Nothing
        End Try
    End Function

    Private Function OutputBadParameterPassed() As Boolean

        Dim iItem As Integer = 0
        Dim sArg As String
        Dim sArgValues() As String

        Try
            OutputBadParameterPassed = True

            If My.Application.CommandLineArgs.Count > 0 Then
                For iItem = 0 To My.Application.CommandLineArgs.Count - 1
                    sArg = My.Application.CommandLineArgs.Item(iItem)
                    sArgValues = sArg.Split(CChar("="))
                    If sArgValues(0).ToUpper = "INCLUDEYEAREND" Or _
                            sArgValues(0).ToUpper = "BRANCH" Or _
                            sArgValues(0).ToUpper = "LEDGERS" Or _
                            sArgValues(0).ToUpper = "CURRENTPERIODID" Then
                        'Do Nothing
                    Else
                        OutputBadParameterPassed = False
                        Exit Function
                    End If
                Next
            End If

        Catch ex As Exception
            Console.WriteLine("OutputBadParameterPassed Failed - " + ex.Message)
            Throw New Exception("OutputBadParameterPassed Failed - " + ex.Message)
        Finally
        End Try
    End Function

    Private Function InstantiateBusinessComponents() As Boolean

        Dim lResult As Long

        Try
            InstantiateBusinessComponents = True

            lResult = m_oBusinessLedger.Initialise(sUsername:="sirius", _
                sPassword:="sirius", _
                iUserID:=kiUserID, _
                iSourceID:=CShort(SourceId), _
                iLanguageID:=m_iDefaultLanguageID, _
                iCurrencyID:=kiCurrencyId, _
                iLogLevel:=Convert.ToInt16(PMELogLevel.PMLogError), _
                sCallingAppName:=ACApp)
            If lResult <> PMEReturnCode.PMTrue Then
                InstantiateBusinessComponents = False
                Throw New Exception("Unable to initialise bACTLedger.Form")
            End If

            lResult = m_oBusinessPeriodEnd.Initialise(sUsername:="sirius", _
                                sPassword:="sirius", _
                                iUserID:=kiUserID, _
                                iSourceID:=CShort(SourceId), _
                                iLanguageID:=m_iDefaultLanguageID, _
                                iCurrencyID:=kiCurrencyId, _
                                iLogLevel:=Convert.ToInt16(PMELogLevel.PMLogError), _
                                sCallingAppName:=ACApp)
            If lResult <> PMEReturnCode.PMTrue Then
                InstantiateBusinessComponents = False
                Throw New Exception("Unable to initialise bACTPeriodEnd.Form")
            End If

            lResult = m_oBusinessPeriod.Initialise(sUsername:="sirius", _
                            sPassword:="sirius", _
                            iUserID:=kiUserID, _
                            iSourceID:=CShort(SourceId), _
                            iLanguageID:=m_iDefaultLanguageID, _
                            iCurrencyID:=kiCurrencyId, _
                            iLogLevel:=Convert.ToInt16(PMELogLevel.PMLogError), _
                            sCallingAppName:=ACApp)
            If lResult <> PMEReturnCode.PMTrue Then
                InstantiateBusinessComponents = False
                Throw New Exception("Unable to initialise bACTPeriod.Form")
            End If

        Catch ex As Exception
            Console.WriteLine("InstantiateBusinessComponents Failed - " + ex.Message)
            Throw New Exception("InstantiateBusinessComponents Failed - " + ex.Message)
            InstantiateBusinessComponents = False
        End Try
    End Function

    Private Function CheckNextYearPeriodConfigured(ByVal vYearName As Object) As Boolean
        Dim oDatabase As dPMDAO.Database = Nothing
        Dim vResults As Object = Nothing
        Dim oAllYears(,) As Object
        Dim iReturnCode As Integer
        Dim iCounter As Integer

        Try
            CheckNextYearPeriodConfigured = False
            DBConnect(oDatabase)
            AddParameterLite(oDatabase, "company_id", SourceId, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
            AddParameterLite(oDatabase, "sub_branch_id", SubBranchId, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)

            iReturnCode = oDatabase.SQLSelect("spu_ACT_SelAll_PeriodYear", "spu_ACT_SelAll_PeriodYear", True, vResultArray:=vResults)

            If Not IsNothing(vResults) Then
                oAllYears = DirectCast(vResults, Object(,))
                For iCounter = 0 To UBound(oAllYears, 2)
                    If (CLng(oAllYears(0, iCounter)) > CLng(vYearName)) Then
                        CheckNextYearPeriodConfigured = True
                        Exit For
                    End If
                Next
            End If

            DBDisconnect(oDatabase)
            oDatabase = Nothing

        Catch ex As Exception
            Console.WriteLine("CheckNextYearPeriodConfigured Failed - " + ex.Message)
            Throw New Exception("CheckNextYearPeriodConfigured Failed - " + ex.Message)
            CheckNextYearPeriodConfigured = False
        End Try

    End Function

    Private Function Validation(ByVal v_vYearName As Object, _
                                ByVal v_iPeriodEndComplete As Integer, _
                                ByVal v_vPeriodEndDate As Object, _
                                ByVal v_sPeriodName As String) As Boolean

        Try
            Dim iYear As Integer
            Dim dtPeriodEndDate As Date

            Validation = True

            'Validation Start
            dtPeriodEndDate = CDate(v_vPeriodEndDate)
            iYear = CInt(dtPeriodEndDate.Month)

            If (v_iPeriodEndComplete = 1 Or iYear = 12) And UCase(IncludeYearEnd) <> "TRUE" Then
                Console.WriteLine("Please choose TRUE to Include Year End parameter")
                Console.WriteLine("Year End Process can't run untill Include Year End parameter not set to True")
                Validation = False
                Exit Function
            End If

            'If current period is being closed prematurely
            If (CDate(v_vPeriodEndDate) > Now) Then
                Console.WriteLine("Period/Year End : Period does not end for Future date")
                Validation = False
                Exit Function
            End If

        Catch ex As Exception
            Console.WriteLine("Validation Failed - " + ex.Message)
            Throw New Exception("Validation Failed - " + ex.Message)
            Validation = False
        End Try

    End Function

#End Region

#Region " Procedures "

    Private Sub OutputSyntax()
        Console.WriteLine("Unable To Run Period End Process : Please post unposted Transactions First")
    End Sub

#End Region

End Module
