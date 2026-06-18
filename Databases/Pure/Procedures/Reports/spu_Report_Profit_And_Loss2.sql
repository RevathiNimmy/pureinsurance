SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Report_Profit_And_Loss2'
GO
CREATE PROCEDURE spu_Report_Profit_And_Loss2
    @branch_id int,
    @period_end_date char(10)
AS

DECLARE @iAccountID int,
    @iBranchID int,
    @dPeriodEndDate datetime,
    @sPeriodName varchar(15),
    @dLastYearPeriodEndDate datetime,
    @sLastYearPeriodName varchar(15)

    SET NOCOUNT ON

    SELECT @iBranchID = ISNULL(@branch_id, 0)

    IF @iBranchID = 0 BEGIN
        DECLARE account_cursor CURSOR FAST_FORWARD FOR
            SELECT account_id FROM Report_TreePathNames
    END ELSE BEGIN
        DECLARE account_cursor CURSOR FAST_FORWARD FOR
            SELECT RTPN.account_id
            FROM Report_TreePathNames RTPN
            INNER JOIN Account AS A ON RTPN.account_id = A.account_id
            WHERE A.company_id = @iBranchID
    END

    -- Empty the temporary table
    DELETE FROM Report_Transaction

    EXECUTE spu_Report_FullTreePathNames 3

    OPEN account_cursor

    FETCH NEXT FROM account_cursor INTO @iAccountID
    WHILE @@FETCH_STATUS = 0
    BEGIN

-- Get the date restrictions
        SELECT @dPeriodEndDate = (SELECT MIN(P.period_end_date)
                      FROM Account A
                            JOIN Period P
                            ON A.company_id = P.company_id
                            AND A.account_id = @iAccountID
                            AND P.period_end_date >= @period_end_date)

        SELECT @sPeriodName = (SELECT P.period_name
                       FROM Account A
                        JOIN Period P
                        ON A.company_id = P.company_id
                        AND A.account_id = @iAccountID
                        AND P.period_end_date = @dPeriodEndDate)

        SELECT @dLastYearPeriodEndDate = (SELECT MAX(P.period_end_date)
                          FROM Period CP
                                JOIN Period P
                                ON CP.company_id = P.company_id
                                AND CP.period_end_date = @dPeriodEndDate
                                AND CP.period_end_date > P.period_end_date
                                AND SUBSTRING(CP.period_name, 1, 2) = SUBSTRING(P.period_name, 1, 2))

        SELECT @sLastYearPeriodName = (SELECT P.period_name
                           FROM Account A
                            JOIN Period P
                            ON A.company_id = P.company_id
                            AND A.account_id = @iAccountID
                            AND P.period_end_date = @dLastYearPeriodEndDate)

-- Get Any details in the current period
        INSERT INTO Report_Transaction(
                            transdetail_id,
                            amount,
                            document_ref,
                            document_date,
                            documenttype_id,
                            account_id,
                            record_type)
        SELECT TD.transdetail_id,
            TD.amount,
            D.document_ref,
            D.document_date,
            D.documenttype_id,
            @iAccountID,
            1
        FROM TransDetail TD
            JOIN Document D
            ON TD.document_id = D.document_id
            JOIN Period P

            ON TD.period_id = P.period_id
            AND P.period_end_date = @dPeriodEndDate
        WHERE TD.account_id = @iAccountID
        UNION
        SELECT TD.transdetail_id,
            TD.amount,
            D.document_ref,
            D.document_date,
            D.documenttype_id,
            @iAccountID,
            2

        FROM TransDetail TD
            JOIN Document D
            ON TD.document_id = D.document_id
            JOIN Period P
            ON TD.period_id = P.period_id
            AND P.period_end_date = @dLastYearPeriodEndDate
        WHERE TD.account_id = @iAccountID

        FETCH NEXT FROM account_cursor INTO @iAccountID

    END

    CLOSE account_cursor
    DEALLOCATE account_cursor

-- Get the data from the temporary tables

    SET NOCOUNT OFF

    SELECT ISNULL(RTPN.element_name1, '') element_name1,
        ISNULL(RTPN.Report_Map_Id1, 0) Report_Map_Id1,
        ISNULL(RTPN.element_name2, '') element_name2,
        ISNULL(RTPN.Report_Map_Id2, 0) Report_Map_Id2,
        ISNULL(RTPN.element_name3, '') element_name3,
        ISNULL(RTPN.Report_Map_Id3, 0) Report_Map_Id3,
        ISNULL(RTPN.element_name4, '') element_name4,
        ISNULL(RTPN.Report_Map_Id4, 0) Report_Map_Id4,
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
        ISNULL(RT.amount, 0.0) this_year_amount,
        0.0 last_year_amount,
        A.account_name,
        A.short_code account_code,
        1 record_type,
        @sPeriodName period_name,
        @dPeriodEndDate period_end_date,
        RT.document_date,
        ISNULL(RT.documenttype_id, 0) documenttype_id,
        ISNULL(RT.document_ref, '') document_ref,
        ISNULL(RT.transdetail_id, 0) transdetail_id,
        ISNULL(LT.description, '') ledger_type,
        ISNULL(ATp.description, '') account_type
    FROM Report_TreePathNames RTPN
        LEFT OUTER JOIN Report_Transaction RT
        ON RTPN.account_id = RT.account_id
        AND RT.record_type = 1
        LEFT OUTER JOIN Account A
        ON RTPN.account_id = A.account_id
        LEFT OUTER JOIN AccountType ATp
        ON A.accounttype_id = ATp.accounttype_id
        LEFT OUTER JOIN Ledger L
        ON A.ledger_id = L.ledger_id
        LEFT OUTER JOIN LedgerType LT
        ON L.ledgertype_id = LT.ledgertype_id
    UNION
    SELECT ISNULL(RTPN.element_name1, '') element_name1,
        ISNULL(RTPN.Report_Map_Id1, 0) Report_Map_Id1,
        ISNULL(RTPN.element_name2, '') element_name2,
        ISNULL(RTPN.Report_Map_Id2, 0) Report_Map_Id2,
        ISNULL(RTPN.element_name3, '') element_name3,
        ISNULL(RTPN.Report_Map_Id3, 0) Report_Map_Id3,
        ISNULL(RTPN.element_name4, '') element_name4,
        ISNULL(RTPN.Report_Map_Id4, 0) Report_Map_Id4,
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
        0.0 this_year_amount,
        ISNULL(RT.amount, 0.0) last_year_amount,
        A.account_name,
        A.short_code account_code,
        2 record_type,
        @sLastYearPeriodName period_name,
        @dLastYearPeriodEndDate period_end_date,
        RT.document_date,
        ISNULL(RT.documenttype_id, 0) documenttype_id,
        ISNULL(RT.document_ref, '') document_ref,
        ISNULL(RT.transdetail_id, 0) transdetail_id,
        ISNULL(LT.description, '') ledger_type,
        ISNULL(ATp.description, '') account_type
    FROM Report_TreePathNames RTPN
        LEFT OUTER JOIN Report_Transaction RT
        ON RTPN.account_id = RT.account_id
        AND RT.record_type = 2
        LEFT OUTER JOIN Account A
        ON RTPN.account_id = A.account_id
        LEFT OUTER JOIN AccountType ATp
        ON A.accounttype_id = ATp.accounttype_id
        LEFT OUTER JOIN Ledger L
        ON A.ledger_id = L.ledger_id
        LEFT OUTER JOIN LedgerType LT
        ON L.ledgertype_id = LT.ledgertype_id
    ORDER BY Report_Map_Id1, Report_Map_Id2, Report_Map_Id3, Report_Map_Id4, Report_Map_Id5, Report_Map_Id6, Report_Map_Id7, Report_Map_Id8, Report_Map_Id9, Report_Map_Id10, account_code, record_type, period_end_date, document_ref

    SET NOCOUNT ON

    DELETE FROM Report_Transaction
    DELETE FROM Report_TreePathNames

    SET NOCOUNT OFF

GO

