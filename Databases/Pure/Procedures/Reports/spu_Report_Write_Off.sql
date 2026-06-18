SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Write_Off'
GO

CREATE PROCEDURE spu_Report_Write_Off
    @branch_id INT,
    @write_off_account VARCHAR(5),
    @start_date DATETIME,
    @end_date DATETIME
AS


IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

CREATE TABLE #WriteOff
(
branch VARCHAR(100),
write_off VARCHAR(5),
document_id INT,
transaction_date DATETIME,
document_ref VARCHAR(20),
account_code VARCHAR(30),
reason VARCHAR(100),
amount MONEY
)

INSERT INTO #WriteOff

SELECT 
S.[description], 
NULL,
D.document_id,
D.document_date,
D.document_ref,
A.short_code,
WR.[description], 
TD.amount

FROM TransDetail TD 
JOIN Document D
ON TD.document_id = D.document_id
AND 
    (
        document_date >= @start_date 
        AND 
        document_date <= @end_date
    )
LEFT JOIN write_off_reason WR
ON D.write_off_reason_id=WR.write_off_reason_id
JOIN Account A
ON TD.account_id = A.account_id
JOIN source S
ON TD.company_id = S.source_id
WHERE  
    (
        D.documenttype_id in(1,14) --Write Off
        OR
        TD.account_id IN(3102,3104)
    )
    AND 
    TD.company_id = ISNULL(@branch_id,TD.company_id)

IF @write_off_account = 'N4092'
   BEGIN
	UPDATE #WriteOff
	SET write_off='N4092'
	FROM #WriteOff WO
	WHERE document_id in 
	  (SELECT document_id from #WriteOff where 
	    account_code ='N4092')
	AND WO.account_code<>'N4092'
    END

IF @write_off_account = 'N5092'
   BEGIN
	UPDATE #WriteOff
	SET write_off='N5092'
	FROM #WriteOff WO
	WHERE document_id in 
	  (SELECT document_id from #WriteOff where 
	    account_code ='N5092')
	AND WO.account_code<>'N5092'
   END	

IF @write_off_account = 'ALL'
   BEGIN
	UPDATE #WriteOff
	SET write_off='N5092'
	FROM #WriteOff WO
	WHERE document_id in
	  (SELECT document_id from #WriteOff where
	    account_code ='N5092')	
	
	UPDATE #WriteOff
	SET write_off='N4092'
	FROM #WriteOff WO
	WHERE document_id in
	  (SELECT document_id from #WriteOff where
	    account_code ='N4092')
   END


DELETE #WRITEOFF 
WHERE write_off IS NUll

IF  @write_off_account = 'ALL'
BEGIN
	DELETE #WRITEOFF
	WHERE account_code IN('N5092','N4092')
END

SELECT * FROM #WriteOff
ORDER BY branch,transaction_date

DROP TABLE #WriteOff

GO
