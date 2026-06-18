SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Chart_Of_Accounts_SFU'
GO
-- AMJ true Multi branch changes made to get the correct Node_id for the branch
CREATE PROCEDURE spu_Report_Chart_Of_Accounts_SFU
    @branch_id int,
    @period_end_date datetime
AS

DECLARE
    @iAccountID int,
    @iBranchID int,
    @node_id int

    SET NOCOUNT ON

    SELECT @iBranchID = ISNULL(@branch_id, 0)
    
   
    

    delete from Report_TreePathNames
    
     BEGIN TRAN
    
    IF @iBranchID = 0 BEGIN
		select @node_id = 1
		EXECUTE spu_Report_FullTreePathNames_SFU @node_id        
    END ELSE BEGIN
		select @node_id = node_id
		from structuretree
		where parent_node_id = 0
		and company_id = @iBranchID
	EXECUTE spu_Report_FullTreePathNames_SFU @node_id
	    
    END
    
		SELECT * INTO #Report_TreePathNames FROM Report_TreePathNames
		TRUNCATE TABLE Report_TreePathNames
		COMMIT TRAN

    
      IF @iBranchID = 0 BEGIN
			
			DECLARE account_cursor CURSOR FAST_FORWARD FOR
            SELECT account_id FROM #Report_TreePathNames

		END ELSE
		 BEGIN			

	        DECLARE account_cursor CURSOR FAST_FORWARD FOR
            SELECT RTPN.account_id
            FROM #Report_TreePathNames AS RTPN
            INNER JOIN Account AS A ON RTPN.account_id = A.account_id
            WHERE A.company_id = @iBranchID
    END

		

    -- Empty the temporary table
     CREATE TABLE #Report_Transaction(
    	transdetail_id int NULL,
    	amount numeric(19, 4) NULL,
    	document_sequence smallint NULL,
    	policy_number varchar(30) NULL,
    	branch_id int NULL,
    	comment varchar(60) NULL,
    	document_ref varchar(25) NULL,
    	document_date datetime NULL,
    	documenttype_id int NULL,
    	account_id int NULL,
    	account_code char(30) NULL,
    	account_name varchar(100) NULL,
    	account_type varchar(100) NULL,
    	ledger_type varchar(100) NULL,
    	branch_name varchar(100) NULL,
    	period_id int NULL,
    	record_type smallint NULL,
    	transdetail_id2 int NULL,
    	amount2 numeric(19, 4) NULL,
    	document_sequence2 smallint NULL,
    	policy_number2 varchar(30) NULL,
    	branch_id2 int NULL,
    	comment2 varchar(60) NULL,
    	account_id2 int NULL,
    	account_code2 char(20) NULL,
    	account_name2 varchar(100) NULL,
    	account_type2 varchar(100) NULL,
    	ledger_type2 varchar(100) NULL,
    	branch_name2 varchar(100) NULL,
    	period_id2 int NULL,
    	record_type2 smallint NULL,
    	extra_char1 varchar(100) NULL,
    	extra_char2 varchar(255) NULL,
    	extra_char3 varchar(100) NULL,
    	extra_char4 varchar(255) NULL,
    	extra_char5 varchar(100) NULL,
    	extra_char6 varchar(100) NULL,
    	extra_char7 varchar(100) NULL,
    	extra_int1 int NULL,
    	extra_int2 int NULL,
    	extra_int3 int NULL,
    	extra_int4 int NULL,
    	extra_int5 int NULL,
    	extra_int6 int NULL,
    	extar_int7 int NULL,
    	extra_datetime1 datetime NULL,
    	extra_datetime2 datetime NULL,
    	extra_datetime3 datetime NULL,
    	extra_datetime4 datetime NULL,
    	extra_datetime5 datetime NULL,
    	extra_datetime6 datetime NULL,
    	extra_datetime7 datetime NULL,
    	extra_numeric1 numeric(19, 4) NULL,
    	extra_numeric2 numeric(19, 4) NULL,
    	extra_numeric3 numeric(19, 4) NULL,
    	extra_numeric4 numeric(19, 4) NULL,
    	extra_numeric5 numeric(19, 4) NULL,
    	extra_numeric6 numeric(19, 4) NULL,
    	extra_numeric7 numeric(19, 4) NULL
    ) 

    OPEN account_cursor

    FETCH NEXT FROM account_cursor INTO @iAccountID
    WHILE @@FETCH_STATUS = 0 BEGIN
            INSERT INTO #Report_Transaction(
                                transdetail_id,
                                extra_numeric1,
                                amount,
                                extra_numeric2,
                                document_ref,
                                account_id,
                                account_code,
                                account_name,
                                extra_datetime1,
                                ledger_type,
                                account_type,
                                record_type)
            SELECT 0,
                0.0,
                0.0,
                0.0,
                0,
                @iAccountID,
                A.short_code,
                A.account_name,
                0,
                ISNULL(LT.description, ''),
                ISNULL(ATp.description, ''),
                0
            FROM Account A
                LEFT OUTER JOIN Ledger L
                ON A.ledger_id = L.ledger_id
                LEFT OUTER JOIN LedgerType LT
                ON L.ledgertype_id = LT.ledgertype_id
                LEFT OUTER JOIN AccountType ATp
                ON A.accounttype_id = ATp.accounttype_id
            WHERE A.account_id = @iAccountID

        FETCH NEXT FROM account_cursor INTO @iAccountID

    END

    CLOSE account_cursor
    DEALLOCATE account_cursor

-- Get the data from the temporary tables

    SET NOCOUNT OFF

    SELECT RTPN.element_name1,
        RTPN.Report_Map_Id1,
        RTPN.element_name2,
        RTPN.Report_Map_Id2,
        RTPN.element_name3,
        RTPN.Report_Map_Id3,
        RTPN.element_name4,
        RTPN.Report_Map_Id4,
        ISNULL(RTPN.element_name5, '') element_name5,
        ISNULL(RTPN.Report_Map_Id5, 0) Report_Map_Id5,
        ISNULL(RTPN.element_name6, '') element_name6,
        ISNULL(RTPN.Report_Map_Id6, 0) Report_Map_Id6,
        ISNULL(RTPN.element_name7, '') element_name7,
        ISNULL(RTPN.Report_Map_Id7, 0) Report_Map_Id7,
        ISNULL(RTPN.element_name8, '') element_name8,
        ISNULL(RTPN.Report_Map_Id8, 0) Report_Map_Id8,
        ISNULL(RTPN.element_name9, '') element_name9,
        ISNULL(RTPN.Report_Map_Id9, 0) Report_Map_Id9,
        ISNULL(RTPN.element_name10, '') element_name10,
        ISNULL(RTPN.Report_Map_Id10, 0) Report_Map_Id10,
        RT.account_name
    FROM #Report_Transaction RT
        JOIN #Report_TreePathNames RTPN
        ON RT.account_id = RTPN.account_id
    ORDER BY Report_Map_Id1, Report_Map_Id2, Report_Map_Id3, Report_Map_Id4, Report_Map_Id5, Report_Map_Id6, Report_Map_Id7, Report_Map_Id8, Report_Map_Id9, Report_Map_Id10

GO

