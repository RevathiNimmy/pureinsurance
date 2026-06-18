SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO


EXECUTE DDLDropProcedure 'spu_wp_accountbalance'
GO


CREATE PROCEDURE spu_wp_accountbalance
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @Risk_id INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


SELECT 	SUM(td.amount) account_balance,
		SUM(td.account_amount) account_currency_balance
FROM	party p
JOIN    account a ON a.account_key = p.party_cnt
JOIN    transdetail td ON td.account_id = a.account_id
WHERE	p.party_cnt = @partycnt

GO

