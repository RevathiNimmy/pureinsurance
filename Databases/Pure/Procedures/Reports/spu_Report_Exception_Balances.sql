SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Exception_Balances'
GO
 
CREATE PROCEDURE spu_Report_Exception_Balances
	@branch_id	int,
	@start_date	datetime,
	@end_date	datetime,
	@BA		varchar(11) 

AS

declare @ibranch_id	int,
  	@sBA varchar(11)

select @ibranch_id = isnull(@branch_id,0)
select @sBA =  @BA

--DROP TABLE #TransLines
--DROP TABLE #TransLines2

CREATE TABLE #TransLines
	(	amount		numeric(19,4) NULL,
		ledger_id	smallint NULL,
		ledger_name	varchar(30) NULL,
		code		char(10) NULL,
		adjustment	char(3) NULL,
	)

CREATE TABLE #TransLines2
	(	amount		numeric(19,4) NULL,
		ledger_id	smallint NULL,
		ledger_name	varchar(30) NULL,
		code		char(10) NULL,
		adjustment	char(3) NULL,
	)

INSERT INTO #TransLines
SELECT	T.amount,
		l.ledger_id,
		L.ledger_name,
		DT.code,
        ISNULL(
        (
            SELECT 'YES'
            FROM transdetail T1
            WHERE T1.transdetail_id = T.transdetail_id 
            AND T1.spare IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
        ), 'NO') as                         'Adjustment'
	FROM 	Transdetail				T
-- ECK230902
	LEFT OUTER JOIN	Insurance_File			I
	ON I.insurance_ref = T.insurance_ref
	AND I.policy_version =	
	(	select max(policy_version)
		from insurance_file
		where insurance_ref = T.insurance_ref
		AND lead_insurer_cnt IS NOT NULL	
	)		
	JOIN	Account					A
	ON	A.account_id = T.account_id
	JOIN	Document				D
	ON	D.document_id = T.document_id
	JOIN	DocumentType				DT
	ON	DT.documenttype_id = D.documenttype_id
	JOIN	Ledger					L
	ON	L.ledger_id = A.ledger_id	
	WHERE	(	ISNULL(@branch_id,0) = 0
			or 
			(	ISNULL(@branch_id,0) <> 0
				and 
				D.company_id = @branch_id
			)
		)
		 	
	AND	D.document_date >= @start_date
	AND D.document_date <= @end_date
 	AND	(	(I.cover_start_date <  @start_date		-- eck 
			 AND @BA = 'Before'
			)  
		OR
			(I.cover_start_date >  @end_date
			 AND @BA = 'After'
			)  		
		)
/*eck240902*/
INSERT INTO #TransLines
SELECT	0.00,
	l.ledger_id,
	L.ledger_name,
	'',
	'N0' as Adjustment
 	FROM	Ledger		L
	WHERE l.ledger_id not in (select distinct ledger_id from #translines)
/*eck240902*/ 
insert into #translines2
SELECT sum(amount), ledger_id, ledger_name, code, adjustment
FROM #TransLines
group by ledger_id, ledger_name, code, adjustment

select amount, ledger_name, code,null as document_date,opening_balance = 
		(	SELECT	ISNULL(SUM(amount), 0)
			FROM	Transdetail				T1
-- ECK230902
			LEFT OUTER JOIN	Insurance_File			I
			ON I.insurance_ref = T1.insurance_ref
			AND I.policy_version =	
		(	select max(policy_version)
				from insurance_file
				where insurance_ref = T1.insurance_ref
			AND lead_insurer_cnt IS NOT NULL	
		)		
			JOIN	Document				D1
				ON	D1.document_id = T1.document_id
			JOIN	Account					A1
				ON	A1.account_id = T1.account_id
			WHERE	A1.ledger_id = tl2.ledger_id
			AND 	(	D1.company_id = @ibranch_id
					or
					@ibranch_id = 0
				)
			AND D1.document_date >= @start_date
			AND D1.document_date <= @end_date
 			AND	(	(I.cover_start_date <  @start_date
			 		AND @BA = 'Before'
				)  
			OR
				(I.cover_start_date >  @end_date
			 	AND @BA = 'After'
				)  		
		)				 
 
		),
		 adjustment, 
		null as ref_date
from #translines2 tl2

DROP TABLE #TransLines
DROP TABLE #TransLines2

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

