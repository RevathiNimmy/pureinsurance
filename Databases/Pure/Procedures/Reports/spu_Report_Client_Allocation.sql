SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Report_Client_Allocation'
GO

CREATE PROCEDURE spu_Report_Client_Allocation

   @branch_id INT,
   @start_date DATETIME,
   @end_date DATETIME

AS

SET NOCOUNT ON

DECLARE	@rectype AS INTEGER,
	@branchid AS INTEGER,
	@accountid AS INTEGER,
	@shortcode AS VARCHAR(50),
	@documentref AS VARCHAR(50),
	@documentid AS INTEGER,
	@transdetailid AS INTEGER,
	@amount AS NUMERIC(19,4),
	@commission AS NUMERIC(19,4),
	@matchedaccountid AS INTEGER,
	@matchedshortcode AS VARCHAR(50), 
	@matcheddocumentref AS VARCHAR(50), 
	@matcheddocumentid INTEGER, 
	@matchedtransdetailid INTEGER,
	@matchedamount AS NUMERIC(19,4),
	@commissionproportion AS NUMERIC(19,4),
	@allocationdate AS DATETIME,
	@allocatedcommission as NUMERIC(19,4),
	@NoOfInvalidAllocatedDocTypes integer,
	@totalallocatedcommission NUMERIC(19,4),
        @matchedcommission NUMERIC(19,4)

	IF @branch_id = 0
	BEGIN
    		SELECT @branch_id = NULL
	END

	CREATE TABLE #ClientAllocation
	(
	rectype INTEGER,
	branchid INTEGER,
	accountid INTEGER, 
	shortcode VARCHAR(50), 
	documentref VARCHAR(50), 
	documentid INTEGER,
        transdetailid INTEGER,
	amount MONEY, 	
	commission MONEY,
	matchedshortcode VARCHAR(50), 
	matcheddocumentref VARCHAR(50), 
	matcheddocumentid INTEGER,
	matchedamount MONEY, 
        matchedcommission MONEY,
	allocationdate DATETIME
	)

	DECLARE	c_cursor CURSOR FAST_FORWARD FOR
	select DISTINCT d.company_id,
			a.account_id, 
			a.short_code, 
			d.document_ref, 
			d.document_id,
			td.transdetail_id, 
			td.amount, 
			0, 
			NULL, 
			NULL, 
			NULL, 
			NULL, 
			NULL, 
			NULL
	from transdetail td
	join document d on d.document_id = td.document_id
	join account a on td.account_id = a.account_id 
	join ledger l on a.ledger_id = l.ledger_id
	where d.documenttype_id in (2,3,4,5,15,16,17,18,35,36)
	and l.ledger_short_name IN ('SA', 'UB')
	and d.document_date >= @Start_Date
	and d.document_date <= @End_Date
	AND d.company_id = ISNULL(@branch_id, d.company_id)	

	OPEN c_cursor

	FETCH NEXT FROM c_cursor INTO 	@branchid, @accountid, @shortcode, 
					@documentref, @documentid, 	
					@transdetailid, @amount, 
					@commission, @matchedshortcode, 
					@matcheddocumentref, @matcheddocumentid, 
					@matchedamount, @matchedcommission, @allocationdate

	WHILE @@FETCH_STATUS = 0
	BEGIN

		--do not process document_ref if previously processed
		IF NOT EXISTS 	( 	
				SELECT 	NULL 
				FROM 	#ClientAllocation 
				WHERE 	documentid = @documentid 
				OR 	matcheddocumentid = @documentid
				)
		BEGIN
			--check if commission involved ...
				SELECT @Commission =
				(SELECT ISNULL(SUM(td.amount), 0) 
				from transdetail td
				join document d on d.document_id = td.document_id
				join account a on td.account_id = a.account_id 
				join ledger l on a.ledger_id = l.ledger_id
				where d.documenttype_id in (2,3,4,5,15,16,17,18,35,36)
				and l.ledger_short_name = 'CO'
				and d.document_date >= @Start_Date
				and d.document_date <= @End_Date
				and d.document_id = @documentid
				)
	
	
			--check if matched againsts valid documents that wouldnt have commission involved
			SELECT @NoOfInvalidAllocatedDocTypes = 
				(
				SELECT COUNT(d2.document_id)
				FROM transdetail td2
				JOIN allocationdetail ad on ad.transdetail_id = td2.transdetail_id
				JOIN allocationdetail ad2 ON ad2.allocation_id = ad.allocation_id
				AND ad2.transdetail_id <> @transdetailid
				JOIN allocation al ON ad2.allocation_id = al.allocation_id
				AND al.allocation_date <= @End_Date
				JOIN transdetail td3 ON ad2.transdetail_id = td3.transdetail_id
				AND td3.transdetail_id <> @transdetailid
				JOIN document d2 ON td3.document_id = d2.document_id
				JOIN account a ON td3.account_id = a.account_id 
				join ledger l on a.ledger_id = l.ledger_id
				WHERE td2.transdetail_id = @transdetailid
				AND l.ledger_short_name IN ('SA', 'UB')
				AND d2.documenttype_id NOT IN (2,3,4,5,15,16,17,18,35,36)	
				)
	
			IF @Commission <> 0 AND @NoOfInvalidAllocatedDocTypes = 0
			BEGIN
	
				SELECT @TotalAllocatedCommission = 0
	
				--get all the transactions involved in the allocation
				DECLARE	c_cursor2 CURSOR FAST_FORWARD FOR
				SELECT DISTINCT d2.document_id, d2.document_ref, a.short_code, td3.amount, al.allocation_date
				FROM transdetail td2
				JOIN allocationdetail ad on ad.transdetail_id = td2.transdetail_id
				JOIN allocationdetail ad2 ON ad2.allocation_id = ad.allocation_id
				AND ad2.transdetail_id <> @transdetailid
				JOIN allocation al ON ad2.allocation_id = al.allocation_id
				AND al.allocation_date <= @End_Date
				JOIN transdetail td3 ON ad2.transdetail_id = td3.transdetail_id
				AND td3.transdetail_id <> @transdetailid
				JOIN document d2 ON td3.document_id = d2.document_id
				JOIN account a ON td3.account_id = a.account_id 
				join ledger l on a.ledger_id = l.ledger_id
				WHERE td2.transdetail_id = @transdetailid
				AND l.ledger_short_name IN ('SA', 'UB')
				AND d2.documenttype_id in (2,3,4,5,15,16,17,18,35,36)
			
				OPEN c_cursor2
	
				FETCH NEXT FROM c_cursor2 INTO 	@matcheddocumentid, @matcheddocumentref, 
								@matchedshortcode, @matchedamount, 
								@allocationdate
		
				WHILE @@FETCH_STATUS = 0
				BEGIN
	
	
					--see if any commission involved in allocated transactions
					SELECT @AllocatedCommission = 
					(SELECT (ISNULL(SUM(td.amount), 0) * -1)
					from transdetail td
					join document d on d.document_id = td.document_id
					join account a on td.account_id = a.account_id 
					join ledger l on a.ledger_id = l.ledger_id
					join transmatch tm on td.transdetail_id = tm.transdetail_id
					join matchgroup mg on tm.match_id = mg.match_id
						and mg.match_date <= @End_Date
					where d.documenttype_id in (2,3,4,5,15,16,17,18,35,36)
					and l.ledger_short_name = 'CO'
					and d.document_id = @matcheddocumentid
					)
	
					SELECT @TotalAllocatedCommission = @TotalAllocatedCommission + @AllocatedCommission
	
					FETCH NEXT FROM c_cursor2 INTO 	@matcheddocumentid, @matcheddocumentref, 
									@matchedshortcode, @matchedamount, 
									@allocationdate
		
				END 
				CLOSE 		c_cursor2
	
				--only interested in allocated transactions with difference in commission
				IF @totalallocatedcommission <> @Commission
				BEGIN			
					SELECT @rectype = 1
	
					--keep record of all transactions involved in allocation
					OPEN c_cursor2
	
					FETCH NEXT FROM c_cursor2 INTO 	@matcheddocumentid, @matcheddocumentref, 
									@matchedshortcode, @matchedamount, 
									@allocationdate
		
					WHILE @@FETCH_STATUS = 0
					BEGIN
	
						IF @documentid <> @matcheddocumentid 
						BEGIN
							INSERT INTO #ClientAllocation
								(
								rectype,
								branchid,
								accountid, 
								shortcode, 
								documentref, 
								documentid,
							        transdetailid,
								amount, 	
								commission,
								matchedshortcode, 
								matcheddocumentref, 
								matcheddocumentid,
								matchedamount,
		                                                matchedcommission,
								allocationdate
								)
							VALUES  (
								@rectype,
								@branchid, @accountid, @shortcode, 
								@documentref, @documentid, 
								@transdetailid, @amount, 
								@commission, @matchedshortcode, 
								@matcheddocumentref, @matcheddocumentid, 
								@matchedamount, @totalallocatedcommission,
								@allocationdate
								)
		
							SELECT @rectype = @rectype + 1

						END

						FETCH NEXT FROM c_cursor2 INTO 	@matcheddocumentid, @matcheddocumentref, 
										@matchedshortcode, @matchedamount, 
										@allocationdate
					END 
	
					CLOSE 		c_cursor2
	
				END
	
				DEALLOCATE	c_cursor2
			END 

		END

		FETCH NEXT FROM c_cursor INTO 	@branchid, @accountid, @shortcode, 
						@documentref, @documentid, 	
						@transdetailid, @amount, 
						@commission, @matchedshortcode, 
						@matcheddocumentref, @matcheddocumentid, 
						@matchedamount, @matchedcommission, @allocationdate
	END 
	CLOSE 		c_cursor
	DEALLOCATE	c_cursor

	SET NOCOUNT OFF

	SELECT 	CA.rectype as 'Record', 
		CA.branchid as 'Branch Id',
		s.[description] as 'Branch Name',
		CA.shortcode as 'Shortcode', 
		CA.documentref as 'Document Ref', 
		CA.amount as 'Amount', 	
		CA.commission as 'Commission', 
		CA.matchedshortcode as 'Matched Shortcode', 
		CA.matcheddocumentref as 'Matched Document Ref', 
		CA.matchedamount as 'Matched Amount', 
		CA.allocationdate as 'Allocation Date',
		CA.matchedcommission as 'Matched Commission'
	FROM 	#ClientAllocation CA
	JOIN    source s on s.source_id = CA.branchid
	WHERE 	CA.matcheddocumentref is not null

	DROP TABLE #ClientAllocation
GO
