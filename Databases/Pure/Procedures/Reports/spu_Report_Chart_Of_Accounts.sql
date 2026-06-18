SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Chart_Of_Accounts'
GO
-- AMJ true Multi branch changes made to get the correct Node_id for the branch
CREATE PROCEDURE spu_Report_Chart_Of_Accounts
    @branch_id int,
    @period_end_date datetime
AS

DECLARE
    @iAccountID int,
    @iBranchID int

    SET NOCOUNT ON

    SELECT @iBranchID = ISNULL(@branch_id, 0)

    IF @iBranchID = 0
    BEGIN
        DECLARE account_cursor CURSOR FAST_FORWARD FOR
            SELECT account_id FROM Report_TreePathNames
    END
    ELSE
    BEGIN
        DECLARE account_cursor CURSOR FAST_FORWARD FOR
            SELECT RTPN.account_id
            FROM Report_TreePathNames AS RTPN
            INNER JOIN Account AS A ON RTPN.account_id = A.account_id
            WHERE A.company_id = @iBranchID
    END
    
    DELETE FROM Report_TreePathNames

	IF EXISTS(SELECT NULL FROM hidden_options WHERE option_number=16)
	BEGIN
		EXEC spu_Report_FullTreePathNames 0
	END
	ELSE
	BEGIN
		EXEC spu_Report_FullTreePathNames 1
	END

    -- Empty the temporary table
    DELETE FROM Report_Transaction

    OPEN account_cursor

    FETCH NEXT FROM account_cursor INTO @iAccountID
    WHILE @@FETCH_STATUS = 0 BEGIN
            INSERT INTO Report_Transaction(
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
    FROM Report_Transaction RT
        JOIN Report_TreePathNames RTPN
        ON RT.account_id = RTPN.account_id
    ORDER BY Report_Map_Id1, Report_Map_Id2, Report_Map_Id3, Report_Map_Id4, Report_Map_Id5, Report_Map_Id6, Report_Map_Id7, Report_Map_Id8, Report_Map_Id9, Report_Map_Id10

GO

