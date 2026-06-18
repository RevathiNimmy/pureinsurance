SET QUOTED_IDENTIFIER ON 
GO

SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_Do_CommissionPayments_UW'
GO

CREATE PROCEDURE spu_ACT_Do_CommissionPayments_UW
    @account_id int,
    @date_to datetime = NULL,
    @date_by_trans bit = 0,
    @marked_status int = NULL,
    @month int = NULL
AS
BEGIN
	
	-- create temp table to stored result data
	CREATE TABLE #InsurerTemp
	(
		account_name            char(100),
		insurer_ref             varchar(30) NULL,
		document_ref            varchar(25) NULL,
		gross_transdetail_id    int NULL,
		gross_amount            numeric(19, 4) NULL,
		comm_transdetail_id     int NULL,
		comm_amount             numeric(19, 4) NULL,
		commadj_transdetail_id  varchar(255),
		commadj_amount          numeric(19, 4) NULL,
		amt_settled             numeric(19, 4) NULL,
		document_id             int NULL,
		accounting_date         datetime NULL,
		currency_id             smallint NULL,
		marked_status           tinyint,
		month                   smallint NULL,
		spare                   char(20),
		payment                 numeric(19, 4) NULL,
		source_id               int,
		short_code              char(20),
		client_transdetail_id   int NULL,
		client_amount           numeric(19, 4) NULL,
		client_settled          numeric(19, 4) NULL,
		period                  varchar(15) NULL,
		InstalmentStatus        varchar(3),
		tax_amount              numeric(19,4) NULL,
		tax_transdetail_id      int NULL,
		media_ref				varchar(100) NULL
	)

-- pick up commission agent transactions and store in temp table
	INSERT INTO #InsurerTemp
	SELECT
		a.account_name,
		td.insurance_ref,
		d.document_ref,
		0, --gross_transdetail_id 
		0, --gross_amount 
		td.transdetail_id, --comm_transdetail_id 
		td.currency_amount, --comm_amount 
		0, --commadj_transdetail_id 
		0, --commadj_amount 
		(SELECT IsNull(SUM(ad.alloc_ccy_amount),0)
		 FROM 	AllocationDetail ad 
		 WHERE 	ad.transdetail_id = td.transdetail_id
		), --amt_settled 
		d.document_id, 
		td.accounting_date, 
		td.currency_id, 
        (SELECT ISNULL(sum(0)+1,0) 
         FROM   TransMatch tm
         WHERE  tm.allocationdetail_id IS null
         AND	tm.transdetail_id = td.transdetail_id
        ), --marked_status 
		DatePart(mm, DateAdd(dd, a.settlement_period, td.accounting_date)), --month 
		td.spare, 
		0, --payment 
		td.company_id, --source_id 
		a.short_code, 
		0, --client_transdetail_id 
		0, --client_amount 
		0, --client_settled 
		p.period_name, --period 
		'000', --InstalmentStatus 
		0, --tax_amount 
		0, --tax_transdetail_id
		'' -- media_ref
	FROM	Account a JOIN Ledger l ON a.ledger_id = l.ledger_id
	JOIN	Transdetail td ON a.account_id = td.account_id
	JOIN 	Document d ON td.document_id = d.document_id
	JOIN 	Period p ON td.period_id = p.period_id	
	WHERE	a.account_id = @account_id
	AND		l.ledger_short_name = 'CO'	-- commission
	AND		td.spare = 'COMM'
	AND		td.fully_matched = 0
	AND		(
			(td.accounting_date <= @date_to AND @date_by_trans = 0)
			OR (d.created_date <= @date_to AND @date_by_trans = 1)
			OR @date_to Is Null
			)
	AND   	(
			(@marked_status IS NULL) OR
				(@marked_status = 
					(SELECT  ISNULL(sum(0)+1,0) 
						FROM    transmatch tm
						WHERE   tm.allocationdetail_id IS NULL
						AND     tm.transdetail_id = td.transdetail_id 
					)
				)
			)
      AND   (
                (@month IS NULL) OR 
                (@month = DatePart(mm, DateAdd(dd, a.settlement_period, td.accounting_date)))
            )

	-- get rid of transactions which are fully settled (just in case fully_matched flag is not set)
	DELETE FROM #InsurerTemp WHERE amt_settled = comm_amount

	-- get payment amount (marked but hasn't been allocated)
	UPDATE	#InsurerTemp
	SET		payment = tm.base_match_amount
	FROM	#InsurerTemp it JOIN TransMatch tm ON it.comm_transdetail_id = tm.transdetail_id
	WHERE	tm.allocationdetail_id IS NULL
	
	-- get result set
	SELECT * FROM #InsurerTemp
	
	-- get rid of temp table
	DROP TABLE #InsurerTemp
END
GO

SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS ON 
GO

