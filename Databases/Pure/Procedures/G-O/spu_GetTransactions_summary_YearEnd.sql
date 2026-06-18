EXEC DDLDropProcedure 'spu_GetTransactions_summary_YearEnd'
GO


CREATE PROCEDURE spu_GetTransactions_summary_YearEnd  
    @nPeriod_id INT
AS
	
Declare @table TABLE
(
	ID INT IDENTITY,
	company_id int,
	sub_branch_id int,
	Records_exists INT
)

INSERT INTO @table
(	
	company_id,
	sub_branch_id
)
	SELECT
     s.source_id,
    (
        SELECT
            ISNULL(MIN(sub_branch_id), 0)
        FROM sub_branch
        WHERE source_id = s.source_id
        AND is_deleted = 0
    )
FROM source s
WHERE s.is_deleted = 0

declare @branch_id int
declare @ID int
Declare @subBranch_id int

declare cur cursor for 
select ID, company_id,sub_branch_id from @table

open cur
fetch next from cur into @ID,@branch_id, @subBranch_id

WHILE @@Fetch_status= 0    -- Cursor are faster here as we can use IF exists with this appraoch
BEGIN
	IF EXISTS( SELECT 1 from Transdetail 
		JOIN Account ON Transdetail.Account_id  = Account.Account_id 
		Join source s ON S.source_id = Transdetail.company_id
		WHERE Account.Accounttype_id in (1,2)
		AND Transdetail.outstanding_amount <> 0  AND Period_id < = @nPeriod_id AND s.source_id = @Branch_id)
	Update @Table set Records_exists = 1 WHERE ID = @ID 

FETCH NEXT FROM cur INTO @ID,@branch_id, @subBranch_id
END


CLOSE cur
DEALLOCATE cur

SELECT Company_id, sub_branch_id  from @Table WHERE Records_Exists =1

GO
