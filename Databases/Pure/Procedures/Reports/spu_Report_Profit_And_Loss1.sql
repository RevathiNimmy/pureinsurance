SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Report_Profit_And_Loss1'
GO
CREATE PROCEDURE spu_Report_Profit_And_Loss1
    @branch_id int,
    @period_end_date char(10)
AS

DECLARE @iAccountID int,
    @sBalBF char(25),
    @sBalCF char(25),
    @iBranchID int,
    @dPeriodEndDate datetime,
    @sCurrentYearName varchar(20),
    @dYearPeriod1EndDate datetime,
    @dBFEndDate datetime

    SET NOCOUNT ON

    SELECT @iBranchID = ISNULL(@branch_id, 0)

    IF @iBranchID = 0 BEGIN
        DECLARE account_cursor CURSOR FAST_FORWARD FOR
            SELECT account_id FROM Report_TreePathNames
    END ELSE BEGIN
        DECLARE account_cursor CURSOR FAST_FORWARD FOR
            SELECT RTPN.account_id
            FROM Report_TreePathNames AS RTPN
            INNER JOIN Account AS A ON RTPN.account_id = A.account_id
            WHERE A.company_id = @iBranchID
    END

    SELECT @sBalBF = 'Balance Brought Forward'
    SELECT @sBalCF = 'Balance Carried Forward'
    SELECT @dPeriodEndDate = CONVERT(datetime, @period_end_date)

    -- Empty the temporary table
    DELETE FROM Report_Transaction

    OPEN account_cursor

    FETCH NEXT FROM account_cursor INTO @iAccountID
    WHILE @@FETCH_STATUS = 0
    BEGIN

-- Get the date restrictions
        SELECT @sCurrentYearName = (SELECT P.year_name
                        FROM Account A
                            JOIN Period P
                            ON A.company_id = P.company_id
                        WHERE P.period_end_date = @dPeriodEndDate
                        AND A.account_id = @iAccountID)

        SELECT @dYearPeriod1EndDate = (SELECT MIN(P.period_end_date)
                        FROM Account A
                            JOIN Period P
                            ON A.company_id = P.company_id
                        WHERE P.year_name = @sCurrentYearName
                        AND A.account_id = @iAccountID)

        SELECT @dBFEndDate = (SELECT MAX(P.period_end_date)
                    FROM Account A
                        JOIN Period P
                        ON A.company_id = P.company_id
                    WHERE P.period_end_date < @dPeriodEndDate
                    AND A.account_id = @iAccountID)

-- Brought forward amount

        if @dBFEndDate is null
        BEGIN
            INSERT INTO Report_Transaction(
                                transdetail_id,
                                amount,
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
                @sBalBF,
                @iAccountID,
                A.short_code,
                A.account_name,
                null,
                LT.description,
                ATp.description,
                0
            FROM Account A
                LEFT OUTER JOIN Ledger L
                ON A.ledger_id = L.ledger_id
                LEFT OUTER JOIN LedgerType LT
                ON L.ledgertype_id = LT.ledgertype_id
                LEFT OUTER JOIN AccountType ATp
                ON A.accounttype_id = ATp.accounttype_id
            WHERE A.account_id = @iAccountID
        END
        ELSE IF (SELECT count(TD.transdetail_id)
                FROM Account A
                    JOIN TransDetail TD
                    ON A.account_id = TD.account_id
                    JOIN Period P
                    ON TD.period_id = P.period_id
                WHERE A.account_id = @iAccountID
                AND P.period_end_date >= @dYearPeriod1EndDate
                AND P.period_end_date <= @dBFEndDate) = 0
        BEGIN
            INSERT INTO Report_Transaction(
                                transdetail_id,
                                amount,
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
                @sBalBF,
                @iAccountID,
                A.short_code,
                A.account_name,
                @dBFEndDate,
                LT.description,
                ATp.description,
                0
            FROM Account A
                LEFT OUTER JOIN Ledger L
                ON A.ledger_id = L.ledger_id
                LEFT OUTER JOIN LedgerType LT
                ON L.ledgertype_id = LT.ledgertype_id
                LEFT OUTER JOIN AccountType ATp
                ON A.accounttype_id = ATp.accounttype_id
            WHERE A.account_id = @iAccountID
        END
        ELSE
        BEGIN
            INSERT INTO Report_Transaction(
                                transdetail_id,
                                amount,
                                document_ref,
                                account_id,
                                account_code,
                                account_name,
                                extra_datetime1,
                                ledger_type,
                                account_type,
                                record_type)
            SELECT 0,
                sum(TD.amount),
                @sBalBF,
                @iAccountID,
                A.short_code,
                A.account_name,
                @dBFEndDate,
                LT.description,
                ATp.description,
                0
            FROM Account A
                LEFT OUTER JOIN Ledger L
                ON A.ledger_id = L.ledger_id
                LEFT OUTER JOIN LedgerType LT
                ON L.ledgertype_id = LT.ledgertype_id
                LEFT OUTER JOIN AccountType ATp
                ON A.accounttype_id = ATp.accounttype_id
                JOIN TransDetail TD
                ON A.account_id = TD.account_id
                JOIN Period P
                ON TD.period_id = P.period_id
            WHERE A.account_id = @iAccountID
            AND P.period_end_date >= @dYearPeriod1EndDate
            AND P.period_end_date <= @dBFEndDate
            GROUP BY A.short_code,
                 A.account_name,
                 LT.description,
                 ATp.description
        END

-- Get Any details in the current period
        INSERT INTO Report_Transaction(
                            transdetail_id,
                            amount,
                            document_ref,
                            document_date,
                            documenttype_id,
                            account_id,
                            account_code,
                            account_name,
                            account_type,
                            ledger_type,
                            extra_char1,
                            extra_datetime1,
                            record_type)
        SELECT TD.transdetail_id,
            TD.amount,
            D.document_ref,
            D.document_date,
            D.documenttype_id,
            @iAccountID,
            A.short_code,
            A.account_name,
            ATp.description,
            LT.description,
            P.period_name,
            P.period_end_date,
            1
        FROM Account A
            LEFT OUTER JOIN Ledger L
            ON A.ledger_id = L.ledger_id
            LEFT OUTER JOIN LedgerType LT
            ON L.ledgertype_id = LT.ledgertype_id
            LEFT OUTER JOIN AccountType ATp
            ON A.accounttype_id = ATp.accounttype_id
            JOIN TransDetail TD
            ON A.account_id = TD.account_id
            JOIN Document D
            ON TD.document_id = D.document_id
            JOIN Period P
            ON TD.period_id = P.period_id
        WHERE A.account_id = @iAccountID
        AND P.period_end_date = @dPeriodEndDate

-- Get the carried forward balance
        IF (SELECT count(TD.transdetail_id)
                FROM Account A
                    JOIN TransDetail TD
                    ON A.account_id = TD.account_id
                    JOIN Period P
                    ON TD.period_id = P.period_id
                WHERE A.account_id = @iAccountID
                AND P.period_end_date >= @dYearPeriod1EndDate
                AND P.period_end_date <= @dPeriodEndDate) = 0
        BEGIN
            INSERT INTO Report_Transaction(
                                transdetail_id,
                                amount,
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
                @sBalCF,
                @iAccountID,
                A.short_code,
                A.account_name,
                @dPeriodEndDate,
                LT.description,
                ATp.description,
                9
            FROM Account A
                LEFT OUTER JOIN Ledger L
                ON A.ledger_id = L.ledger_id
                LEFT OUTER JOIN LedgerType LT
                ON L.ledgertype_id = LT.ledgertype_id
                LEFT OUTER JOIN AccountType ATp
                ON A.accounttype_id = ATp.accounttype_id
            WHERE A.account_id = @iAccountID
        END
        ELSE
        BEGIN
            INSERT INTO Report_Transaction(
                                transdetail_id,
                                amount,
                                document_ref,
                                account_id,
                                account_code,
                                account_name,
                                extra_datetime1,
                                ledger_type,
                                account_type,
                                record_type)
            SELECT 0,
                sum(TD.amount),
                @sBalCF,
                @iAccountID,
                A.short_code,
                A.account_name,
                @dBFEndDate,
                LT.description,
                ATp.description,
                9
            FROM Account A
                LEFT OUTER JOIN Ledger L
                ON A.ledger_id = L.ledger_id
                LEFT OUTER JOIN LedgerType LT
                ON L.ledgertype_id = LT.ledgertype_id
                LEFT OUTER JOIN AccountType ATp
                ON A.accounttype_id = ATp.accounttype_id
                JOIN TransDetail TD
                ON A.account_id = TD.account_id
                JOIN Period P
                ON TD.period_id = P.period_id
            WHERE A.account_id = @iAccountID
            AND P.period_end_date >= @dYearPeriod1EndDate
            AND P.period_end_date <= @dPeriodEndDate
            GROUP BY A.short_code,
                 A.account_name,
                 LT.description,
                 ATp.description
        END

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
        RTPN.element_name5,
        RTPN.Report_Map_Id5,
        RTPN.element_name6,
        RTPN.Report_Map_Id6,
        RTPN.element_name7,
        RTPN.Report_Map_Id7,
        RTPN.element_name8,
        RTPN.Report_Map_Id8,
        RTPN.element_name9,
        RTPN.Report_Map_Id9,
        RTPN.element_name10,
        RTPN.Report_Map_Id10,
        RT.amount bf_amount,
        RT.account_name,
        RT.account_code,
        RT.record_type,
        RT.extra_char1 period_name,
        RT.extra_datetime1 period_end_date,
        RT.document_date,
        RT.documenttype_id,
        RT.document_ref,
        RT.transdetail_id,
        RT.ledger_type,
        RT.account_type
    FROM Report_Transaction RT
        JOIN Report_TreePathNames RTPN
        ON RT.account_id = RTPN.account_id
    WHERE RT.record_type = 0
    UNION
    SELECT RTPN.element_name1,
        RTPN.Report_Map_Id1,
        RTPN.element_name2,
        RTPN.Report_Map_Id2,
        RTPN.element_name3,
        RTPN.Report_Map_Id3,
        RTPN.element_name4,
        RTPN.Report_Map_Id4,
        RTPN.element_name5,
        RTPN.Report_Map_Id5,
        RTPN.element_name6,
        RTPN.Report_Map_Id6,
        RTPN.element_name7,
        RTPN.Report_Map_Id7,
        RTPN.element_name8,
        RTPN.Report_Map_Id8,
        RTPN.element_name9,
        RTPN.Report_Map_Id9,
        RTPN.element_name10,
        RTPN.Report_Map_Id10,
        RT.amount bf_amount,
        RT.account_name,
        RT.account_code,
        RT.record_type,
        RT.extra_char1 period_name,
        RT.extra_datetime1 period_end_date,
        RT.document_date,
        RT.documenttype_id,
        RT.document_ref,
        RT.transdetail_id,
        RT.ledger_type,
        RT.account_type
    FROM Report_Transaction RT
        JOIN Report_TreePathNames RTPN
        ON RT.account_id = RTPN.account_id
    WHERE RT.record_type = 0
    ORDER BY Report_Map_Id1, Report_Map_Id2, Report_Map_Id3, Report_Map_Id4, Report_Map_Id5, Report_Map_Id6, Report_Map_Id7, Report_Map_Id8, Report_Map_Id9, Report_Map_Id10, period_name, period_end_date, documenttype_id, document_ref

    SET NOCOUNT ON

    DELETE FROM Report_Transaction
    DELETE FROM Report_TreePathNames

    SET NOCOUNT OFF

GO

