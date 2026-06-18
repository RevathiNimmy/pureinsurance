SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Audit_Journal_Allocation'
GO

CREATE PROCEDURE spu_Report_Audit_Journal_Allocation

    @branch_id INT,
    @start_date DATETIME,
    @end_date DATETIME,
    @lead_ledger_type varchar(20),
    @secondary_ledger_type varchar(20)

AS

DECLARE @documentref varchar(255),
	@documentdate datetime,
	@shortcode varchar(50),
	@ledger integer,
	@transdetailid integer,
	@amount numeric(19,4),
	@bankaccountcode varchar(50),
	@branchid integer,
	@branchcode varchar(50),
	@branchdesc varchar(255),
	@commission numeric(19,4),
	@allocatedcommission numeric(19,4),
	@allocationdate datetime,
	@allocationid integer,
        @OrigAmount numeric(19,4),
	@OrigMatched numeric(19,4),
        @OrigDocId integer,
	@AllocationDateCheck datetime

IF @lead_ledger_type = 'ALL'
BEGIN
	SELECT @lead_ledger_type = ''
END

IF @secondary_ledger_type = 'ALL'
BEGIN
	SELECT @secondary_ledger_type = ''
END

if @lead_ledger_type = '' and @secondary_ledger_type <> ''
begin
	select @lead_ledger_type = @secondary_ledger_type 
	select @secondary_ledger_type = ''
end

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

CREATE TABLE #journal_alloc
(
	document_ref varchar(50),
	document_date datetime,
	short_code varchar(50),
	ledger integer,
	TransdetailID integer,
	Amount numeric(19,4),
	BankAccountCode varchar(50),
	BranchID integer,
	BranchCode varchar(50),
	BranchDesc varchar(255),
	Commission numeric(19,4),
	AllocatedCommission numeric(19,4),
	AllocationDate datetime,
	AllocationId integer,
        OrigAmount numeric(19,4),
	OrigMatched numeric(19,4)
)

DECLARE	c_cursor CURSOR FAST_FORWARD FOR
SELECT
    D.document_ref,
    D.document_date,
    A.short_code,
    (
        CASE L.ledger_short_name
            WHEN 'NO' THEN 1
            WHEN 'SA' THEN 2
            WHEN 'PU' THEN 3
            WHEN 'IN' THEN 4 
            WHEN 'AG' THEN 5
            WHEN 'RF' THEN 6
            WHEN 'FE' THEN 7 
            WHEN 'DI' THEN 8
            WHEN 'CO' THEN 9
            WHEN 'UB' THEN 10
            WHEN 'TR' THEN 11
            ELSE 0 
        END
    ) 'Ledger',
    T.transdetail_id 'TransdetailID',
    T.amount 'Amount',
    (
        SELECT ISNULL(MAX(short_code),'')
        FROM Account A1
        JOIN BankAccount B
            ON B.account_id = A1.account_id
        WHERE A1.short_code = A.short_code
    ) 'BankAccountCode',
    C.company_id 'BranchID',
    C.code 'BranchCode',
    C.description 'BranchDesc',
    0,
    0,
    NULL,
    NULL,
    0,
    0

FROM TransDetail T
JOIN Document D
    ON T.document_id = D.document_id
JOIN Account A
    ON T.account_id = A.account_id
JOIN Ledger L
    ON L.ledger_id = A.ledger_id
JOIN Company C
    ON C.company_id = D.company_id
WHERE D.documenttype_id IN (1, 8, 10, 11, 12, 20, 21)
AND D.document_date BETWEEN @start_date AND @end_date
AND D.company_id = ISNULL(@branch_id, D.company_id)
AND ((@lead_ledger_type = '' and @secondary_ledger_type = '') OR 
	(( @lead_ledger_type <> '' and @secondary_ledger_type = ''
		AND EXISTS 	(	
				SELECT * 
				FROM transdetail td2 
				JOIN account a2 on td2.account_id = a2.account_id
				JOIN ledger l2 on l2.ledger_id = a2.ledger_id
				WHERE td2.document_id = D.document_id
				AND rtrim(l2.ledger_Name) = rtrim(@lead_ledger_type)
				)
	)
	or
	( @lead_ledger_type <> '' and @secondary_ledger_type <> ''
		AND EXISTS 	(	
				SELECT *
				FROM transdetail td2
				JOIN account a2 on td2.account_id = a2.account_id
				JOIN ledger l2 on l2.ledger_id = a2.ledger_id
				JOIN document d2 on td2.document_id = d2.document_id
				JOIN transdetail td3 on d2.document_id = td3.document_id 
					and td3.transdetail_id <> td2.transdetail_id				
				JOIN account a3 on td3.account_id = a3.account_id
				JOIN ledger l3 on l3.ledger_id = a3.ledger_id	
				WHERE td2.document_id = D.document_id
				and rtrim(l2.ledger_name) = rtrim(@lead_ledger_type)
				AND rtrim(l3.ledger_Name) = rtrim(@secondary_ledger_type)
				)
	))
	)
		
ORDER BY 
    C.company_id,
    D.document_ref, 
    A.short_code

SET NOCOUNT ON

OPEN c_cursor

FETCH NEXT FROM c_cursor INTO	@documentref, @documentdate, @shortcode, @ledger,
				@transdetailid,	@amount, @bankaccountcode, @branchid, @branchcode,
				@branchdesc, @commission, @allocatedcommission, @allocationdate, @allocationid, @OrigAmount, @OrigMatched

WHILE @@FETCH_STATUS = 0
BEGIN

        SELECT @Commission = 
	(SELECT ISNULL(SUM(TD3.amount),0)
	FROM transmatch tm
	join allocationdetail ad on tm.allocationdetail_id = ad.allocationdetail_id
	join allocationdetail ad2 on ad.allocation_id = ad2.allocation_id
		and ad2.transdetail_id <> @transdetailid
	join transdetail td2 on ad2.transdetail_id = td2.transdetail_id
	join document d2 on td2.document_id = d2.document_id
	join transdetail td3 on d2.document_id = td3.document_id
	join account a2 on td3.account_id = a2.account_id
	join ledger l2 on l2.ledger_id = a2.ledger_id and l2.ledger_short_name = 'CO'
	where tm.transdetail_id = @transdetailid
	)

	SELECT @OrigAmount = 0
	SELECT @OrigMatched = 0
	SELECT @AllocationDate = NULL

	DECLARE	c_cursor2 CURSOR FAST_FORWARD FOR
	SELECT ISNULL(TD3.document_id, 0)
	FROM transmatch tm
	join allocationdetail ad on tm.allocationdetail_id = ad.allocationdetail_id
	join allocationdetail ad2 on ad.allocation_id = ad2.allocation_id
		and ad2.transdetail_id <> @transdetailid
	join transdetail td2 on ad2.transdetail_id = td2.transdetail_id
	join document d2 on td2.document_id = d2.document_id
	join transdetail td3 on d2.document_id = td3.document_id
	join account a2 on td3.account_id = a2.account_id
	join ledger l2 on l2.ledger_id = a2.ledger_id and l2.ledger_short_name = 'CO'
	where tm.transdetail_id = @transdetailid

	OPEN c_cursor2

	FETCH NEXT FROM c_cursor2 INTO @OrigDocId

	WHILE @@FETCH_STATUS = 0
	BEGIN

		SELECT @OrigAmount = @OrigAmount + 
		(
			SELECT SUM(TD.amount)
			FROM transdetail td
			JOIN account a ON a.account_id = td.account_id
			JOIN ledger l ON l.ledger_id = a.ledger_id
				AND l.ledger_short_name in ('SA', 'UB')
			WHERE td.document_id = @OrigDocId
		)

		SELECT @OrigMatched = @OrigMatched +
		(
			SELECT ISNULL(SUM(TM.base_match_amount),0)
			FROM transdetail td
			JOIN account a ON a.account_id = td.account_id
			JOIN ledger l ON l.ledger_id = a.ledger_id
				AND l.ledger_short_name in ('SA', 'UB')
			JOIN allocationdetail ad on td.transdetail_id = ad.transdetail_id
			JOIN allocation al on ad.allocation_id = al.allocation_id
				AND al.allocation_date <= @End_Date 
			JOIN transmatch tm on td.transdetail_id = tm.transdetail_id
			JOIN matchgroup mg on mg.match_id = tm.match_id
				AND mg.match_date <= @End_Date
			WHERE td.document_id = @OrigDocId

		)

		SELECT @AllocationDateCheck = 
		(
			SELECT MAX(al.allocation_date)
			FROM transdetail td
			JOIN account a ON a.account_id = td.account_id
			JOIN ledger l ON l.ledger_id = a.ledger_id
				AND l.ledger_short_name in ('SA', 'UB')
			JOIN allocationdetail ad on td.transdetail_id = ad.transdetail_id
			JOIN allocation al on ad.allocation_id = al.allocation_id
				AND al.allocation_date <= @End_Date 
			JOIN transmatch tm on td.transdetail_id = tm.transdetail_id
			WHERE td.document_id = @OrigDocId
		)

		IF @AllocationDate IS NULL or @AllocationDate <= @AllocationDateCheck
		BEGIN
			SELECT @AllocationDate = @AllocationDateCheck
		END

		FETCH NEXT FROM c_cursor2 INTO 	@OrigDocId
	END 
	CLOSE 		c_cursor2
	DEALLOCATE	c_cursor2

	IF @OrigAmount <> 0 AND @Commission <> 0
	BEGIN
		SELECT @AllocatedCommission = round(@Commission * ( @OrigMatched / @OrigAmount ),2)
	END

        SELECT @Allocationid = 
	(SELECT ISNULL(MIN(al.allocation_id),0)
	FROM transmatch tm
	join allocationdetail ad on tm.allocationdetail_id = ad.allocationdetail_id
	join allocation al on al.allocation_id = ad.allocation_id
		and al.allocation_date <= @End_Date
	join allocationdetail ad2 on al.allocation_id = ad2.allocation_id
		and ad2.transdetail_id <> @transdetailid
	join transdetail td2 on ad2.transdetail_id = td2.transdetail_id
	join document d2 on td2.document_id = d2.document_id
	join transdetail td3 on d2.document_id = td3.document_id
	join account a2 on td3.account_id = a2.account_id
	join ledger l2 on l2.ledger_id = a2.ledger_id and l2.ledger_short_name = 'CO'
	where tm.transdetail_id = @transdetailid
	)

	IF EXISTS ( SELECT * FROM #journal_alloc WHERE allocationid = @allocationid )
	BEGIN
		SELECT @commission = 0
		SELECT @allocationdate = NULL
		SELECT @allocationid = 0
		SELECT @allocatedcommission = 0
	END

	INSERT INTO #journal_alloc
	SELECT @documentref, @documentdate, @shortcode, @ledger, @transdetailid, @amount, @bankaccountcode,
				@branchid, @branchcode,	@branchdesc, @commission, @allocatedcommission, @allocationdate, @allocationid,
				@OrigAmount, @OrigMatched

	FETCH NEXT FROM c_cursor INTO	@documentref, @documentdate, @shortcode, @ledger, @transdetailid, 
					@amount, @bankaccountcode, @branchid, @branchcode, @branchdesc, @commission, @allocatedcommission,
					@allocationdate, @allocationid, @OrigAmount, @OrigMatched

END 
CLOSE 		c_cursor
DEALLOCATE	c_cursor

SET NOCOUNT OFF

select  document_ref,
	document_date,
	short_code,
	ledger,
	TransdetailID,
	Amount,
	BankAccountCode,
	BranchID,
	BranchCode,
	BranchDesc,
	Commission,
	AllocatedCommission,
	AllocationDate,
	OrigAmount,
	OrigMatched
from #journal_alloc

DROP TABLE #journal_alloc