SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Insurer_Binder_currency'
GO


CREATE PROCEDURE spu_Report_Insurer_Binder_currency (
	@insurer_code	varchar(50),
	@branch_id	int,
	@sort_id	varchar(50),
	@ToDate		datetime
	)

AS

DECLARE	@sInsurerCode		char(20),
	@sSQLSort		varchar(255),
	@iBranchID		int,
	@iSortID		int,
	@iDocumentID 		int,
	@itransdetailid		int,
	@nInsurerPaid 		numeric(19, 4),
	@nClientPaid 		numeric(19, 4),
	@nIPTAmount 		numeric(19, 4),
	@nClientAmount		numeric(19, 4)

SET NOCOUNT ON

IF @insurer_code = 'ALL'
	SELECT	@insurer_code = NULL

SELECT @sInsurerCode = ISNULL(@insurer_code, '')

SELECT @iBranchID = ISNULL(@branch_id, 0)
	
SELECT @sort_id = LEFT(@sort_id, 1)
SELECT @iSortID = CONVERT(int, @sort_id)

IF (@sort_id < 1 OR @sort_id > 4)
	SELECT @iSortID = 1
ELSE
	SELECT @iSortID = @sort_id

IF @Todate IS NULL OR @Todate = ''
	SELECT @ToDate = GETDATE()

-- Construct SQL to delete existing transactions
DELETE FROM Report_Transaction

-- Store transactions on temporary table

INSERT INTO Report_Transaction
	(	account_code,
		account_name,
		document_date,
		extra_datetime1,
		account_code2,
		policy_number,
		extra_char2,
		extra_char3,
		amount,
		transdetail_id,
		extra_datetime2,
		comment,
		amount2,
		--DJM 08/11/2002
		extra_numeric4,
		extra_numeric1,
		branch_id,
		branch_name,
		extra_char4,
		extra_int1,
		extra_char5,			--eck191102
		extra_numeric5
	)
	SELECT DISTINCT	ISNULL(Insurer.shortname, ''),
			ISNULL(Insurer.name, ''),
			Doc.document_date,
			InsFile.renewal_date,
			Client.shortname,
			InsFile.insurance_ref,
			ISNULL(DocType.code, ''),
			ISNULL(DocType.description, ''),
			ISNULL(Premium.currency_amount, 0.0),
			Premium.transdetail_id,
			TransExp.cover_start_date,
			ISNULL(Premium.comment, ''),	
			(	SELECT SUM(round(ISNULL(currency_amount, 0.0),2))
				FROM TransDetail TransDetail
				WHERE Premium.document_id = TransDetail.document_id
				AND Premium.insurance_ref = TransDetail.insurance_ref
				AND Premium.account_id = TransDetail.account_id
				AND Premium.document_sequence < TransDetail.document_sequence
				AND	(
						(
							(
							ISNULL(TransDetail.spare, '') NOT IN ('COMM ADJ', 'AGENT ADJ')
							AND     TransDetail.Document_Sequence NOT IN 	
								(	
								SELECT	Document_Sequence + 1
								FROM	TransDetail 
								WHERE	document_id = Doc.document_id
								AND	spare = 'COMM ADJ'
								)
							)
						)
						OR
						(
						(TransDetail.ref_date <= @ToDate)
						AND	(	
							ISNULL(TransDetail.spare, '') IN ('COMM ADJ', 'AGENT ADJ')
							OR     TransDetail.Document_Sequence IN 	
								(	
								SELECT	Document_Sequence + 1
								FROM	TransDetail 
								WHERE	document_id = Doc.document_id
								AND	spare = 'COMM ADJ'
								)
							)
						)
					)

			),
			--DJM 08/11/2002
			(	SELECT SUM(round(ISNULL(TM.currency_match_amount, 0.0),2))
				FROM TransDetail			TD
				join TransMatch 			TM
				ON td.transdetail_id = TM.transdetail_id
				JOIN MatchGROUP 			MG
				ON TM.match_id = MG.match_id 
				WHERE Premium.document_id = td.document_id
				AND Premium.insurance_ref = td.insurance_ref
				AND Premium.account_id = td.account_id
				AND Premium.document_sequence < td.document_sequence
				AND TM.allocationdetail_id is not NULL
				AND MG.match_date <= @Todate
			),
			0.0,
			Premium.company_id,
			ISNULL(Branch.description, ''),
			'',
			Doc.document_id,
			ISNULL(CoIns.coinsurer_policy_number,''),
			(
			    SELECT ISNULL(SUM(ROUND(tdf.amount,2)),0.0)
			    FROM transdetail tdf
			    JOIN transdetail_type ttf
				ON ttf.transdetail_type_id = tdf.transdetail_type_id
			    WHERE tdf.document_id = doc.document_id
			    AND tdf.account_id =
				(
				    SELECT account_id
				    FROM transdetail
				    WHERE transdetail_id = Premium.transdetail_id
				)
			    AND ttf.code = 'IFEE'
			)
	FROM	Party 										Insurer
	JOIN	Party_Type 									InsPartyType
		ON Insurer.party_type_id = InsPartyType.party_type_id
	JOIN 	Account 							InsurerAcc
		ON InsurerAcc.account_key =   Insurer.party_cnt
	JOIN 	Transdetail 						Premium
		ON 	InsurerAcc.Account_Id = Premium.Account_id
	JOIN 	Document 							Doc
		ON Premium.Document_id = Doc.Document_id
	JOIN 	DocumentType 						DocType
		ON Doc.documenttype_id = DocType.documenttype_id
	LEFT OUTER JOIN	Transaction_Export_Folder 						TransExp
		ON Doc.Document_Ref = TransExp.Document_Ref
		AND TransExp.source_id = Doc.company_id
		AND TransExp.insurance_ref = Premium.insurance_ref
	JOIN 	Insurance_File 									InsFile
		ON TransExp.Insurance_file_cnt = InsFile.insurance_file_cnt
	JOIN 	Insurance_Folder 								InsFold
		ON InsFile.insurance_folder_cnt = InsFold.insurance_folder_cnt
	LEFT OUTER JOIN Policy_Coinsurers							CoIns	 
		ON	InsFile.insurance_file_cnt = CoIns.Insurance_file_cnt				 
		AND	Insurer.party_cnt = CoIns.Party_cnt						 
	JOIN 	Party 										Client
		ON InsFold.insurance_holder_cnt = Client.party_cnt
	JOIN 	Party_Type 									CliPartyType
		ON Client.party_type_id = CliPartyType.party_type_id
	JOIN 		Company 								Branch
		ON Premium.company_id = Branch.company_id
	--DC131201 include Extras
	WHERE InsPartyType.Code IN ('IN', 'EX')
	AND InsurerAcc.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'IN') --PSL16/06/2003 Remove Hard coded ledger_id
	AND TransExp.accounts_export_status = 'c'
	AND CliPartyType.code IN ('PC', 'GC', 'CC')
	AND Premium.document_sequence IN (	SELECT MIN(document_sequence)
						FROM TransDetail
						WHERE Premium.document_id = document_id
						AND InsurerAcc.account_id = account_id
					)
	AND 	(	
			(	@sInsurerCode > ''
				AND 
				Insurer.shortname = @sInsurerCode
			)
			OR
			(	@sInsurerCode = ''
				AND 
				Insurer.shortname <> 'MULTI'
			)
		)
	AND	(
			(	@iBranchID > 0
				AND
				Premium.company_id = @iBranchID
			)
			OR 	@iBranchID = 0
		)
	AND	Doc.document_date <= @ToDate
 
INSERT INTO Report_Transaction
	(	account_code,
		account_name,
		document_date, 
		extra_datetime1, 
		account_code2, 
		policy_number, 
		extra_char2, 
		extra_char3, 
		amount, 
		transdetail_id, 
		extra_datetime2, 
		comment, 
		amount2,
		extra_numeric4,
		transdetail_id2,
		extra_datetime3, 
		comment2, 
		extra_numeric1, 
		branch_id, 
		branch_name, 
		extra_char4, 
		extra_int1,
		extra_char5						 
	)
	SELECT DISTINCT	Insurer.shortname, 
			ISNULL(Insurer.name, ''), 
			Doc.document_date, 
			'', 
			'',
			'',
			ISNULL(DocType.code, ''), 
			ISNULL(DocType.description, ''), 
			ISNULL(Premium.currency_amount, 0.0),
			Premium.transdetail_id, 
			Premium.accounting_date, 
			ISNULL(Premium.comment, ''), 
			0.0, 
			0.0,
			0, 
			'', 
			'', 
			0.0, 
			Premium.company_id,
			ISNULL(Branch.description, ''), 
			'', 
			Doc.document_id,
			'' 							 
	FROM	Party 								Insurer 
	JOIN 	Party_Type 							InsPartyType 
		ON Insurer.party_type_id = InsPartyType.party_type_id 
	JOIN 	Account 							InsurerAcc 
		ON InsurerAcc.account_key =   Insurer.party_cnt
	JOIN 	TransDetail 							Premium 
		ON Premium.account_id = InsurerAcc.account_id
	JOIN 	Document 							Doc 
		ON Doc.document_id = Premium.document_id
	JOIN 	DocumentType 							DocType 
		ON Doc.documenttype_id = DocType.documenttype_id 
	JOIN 	Company 							Branch 
		ON Premium.company_id = Branch.company_id
	WHERE InsPartyType.code IN ( 'IN', 'EX' )
	AND InsurerAcc.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'IN') --PSL16/06/2003 Remove Hard coded ledger_id 
 
	AND 	(	
			(	@sInsurerCode > ''
				AND 
				Insurer.shortname = @sInsurerCode
			)
			OR
			(	@sInsurerCode = ''
				AND 
				Insurer.shortname <> 'MULTI'
			)
		)
	AND	(
			(	@iBranchID > 0
				AND
				Premium.company_id = @iBranchID
			)
			OR 	@iBranchID = 0
		)
	and doc.document_id NOT IN
		(	select	extra_int1
			from	Report_Transaction
		)
	AND	Doc.document_date <= @ToDate

-- Update the transactions
DECLARE cTrans CURSOR FAST_FORWARD FOR
 	SELECT transdetail_id
	FROM Report_Transaction

OPEN cTrans
FETCH NEXT FROM cTrans INTO @itransdetailid

WHILE @@FETCH_STATUS = 0
BEGIN
	
	SELECT @nInsurerPaid = SUM(round(ISNULL(TM.currency_match_amount, 0.0),2))
	FROM 	Report_Transaction 				RT
	JOIN 	TransDetail 					InsTrans
		ON RT.transdetail_id = InsTrans.transdetail_id
	JOIN TransMatch 					TM
		ON InsTrans.transdetail_id = TM.transdetail_id
	JOIN  MatchGROUP 					MG
		ON TM.match_id = MG.match_id 
	WHERE RT.transdetail_id = @itransdetailid
	and TM.allocationdetail_id is not NULL
	and MG.match_date <= @Todate

	SELECT @nClientPaid = SUM(round(ISNULL(TM.currency_match_amount, 0.0),2))
	FROM 	Report_Transaction 				RT
	JOIN 	TransDetail 					CliTrans
		ON RT.extra_int1 = CliTrans.document_id
	JOIN 	Account 					Client
		ON CliTrans.account_id = Client.account_id
	JOIN 	TransMatch 					TM
		ON CliTrans.transdetail_id = TM.transdetail_id
	JOIN 	MatchGROUP 					MG
		ON TM.match_id = MG.match_id 
	WHERE RT.transdetail_id = @itransdetailid
	and TM.allocationdetail_id is not NULL
	and MG.match_date <= @Todate
	AND 	(	
			Client.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name IN ('SA','UB')) --PSL16/06/2003 Remove Hard coded ledger_id
		)

	if (select @nClientPaid) > 
		(	SELECT (-1 * ISNULL(RT.amount, 0.0))
			FROM 	Report_Transaction 	RT
			WHERE RT.transdetail_id = @itransdetailid
		)
	begin
		select @nClientPaid = (-1 * ISNULL(RT.amount, 0.0))
		FROM 	Report_Transaction 	RT
		WHERE RT.transdetail_id = @itransdetailid
	end

	SELECT @nClientAmount = SUM(round(ISNULL(CliTrans.currency_amount, 0.0),2))
	FROM 	Report_Transaction 				RT
	JOIN 	TransDetail 					CliTrans
		ON RT.extra_int1 = CliTrans.document_id
	JOIN 	Account 					Client
		ON CliTrans.account_id = Client.account_id
	JOIN	Ledger						Ledger
		ON Ledger.ledger_id = Client.ledger_id
	WHERE 	RT.transdetail_id = @itransdetailid
	AND 	(	
			Client.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name IN ('SA','UB'))
		)

	select 	@nIPTAmount = ISNULL(td.ref_amount, 0.0)
	FROM	Transdetail					TD
	WHERE   TD.transdetail_id = @itransdetailid
	
 
	IF (SELECT td.currency_amount
	    FROM    Transdetail					TD
	    WHERE   TD.transdetail_id = @itransdetailid) >0
	BEGIN
		SELECT @nIPTAmount = @nIPTAmount * -1
	END

	UPDATE 	Report_Transaction
	SET 	extra_numeric1 = isnull(@nIPTAmount, 0.0),
		extra_numeric2 = isnull(@nInsurerPaid, 0.0),
		extra_numeric3 = isnull(@nClientPaid, 0.0),
		extra_numeric4 = isnull(@nClientAmount, 0.0)
	WHERE transdetail_id = @itransdetailid

	FETCH NEXT FROM cTrans INTO @itransdetailid
END 

UPDATE rt
SET rt.extra_numeric3 = 0
FROM Report_Transaction rt
JOIN party p
    ON p.shortname = rt.account_code
JOIN party_type pt
    ON pt.party_type_id = p.party_type_id
WHERE pt.code = 'EX'
AND rt.extra_numeric3 <> rt.extra_numeric4

CLOSE cTrans
DEALLOCATE cTrans
 
DELETE
FROM Report_Transaction
WHERE amount = extra_numeric2
 
and amount2 = extra_numeric4

SET NOCOUNT OFF

SELECT @sSQLSort = 
	CASE @iSortID
		WHEN 1 THEN ' ORDER BY 2, 5, 20, 11'
		WHEN 2 THEN ' ORDER BY 2, 7, 20, 11'
		WHEN 3 THEN ' ORDER BY 2, 11, 20'
		WHEN 4 THEN ' ORDER BY  2, 3, 20, 11'
		ELSE ' ORDER BY 2, 5, 20'
	END

-- Extract the report data

EXEC 	(	'SELECT	DISTINCT RT.account_code 			as Insurer_Code,
				RT.account_name 			as Insurer_Name,
				RT.document_date 			as Transaction_Date,
				RT.extra_datetime1 			as Policy_Renewal_Date,
				RT.account_code2 			as Client_Code,
				CONVERT(char(20), RT.extra_char2) 	as Transaction_Code,
				RT.policy_number 			as Policy_Number,
				RT.extra_char3 				as Transaction_Description,
				ISNULL(RT.amount, 0.0) 			as Premium,
				RT.transdetail_id 			as Premium_ID,
				RT.extra_datetime2 			as Premium_Effective_Date,
				RT.comment 				as Premium_Comment,
				ISNULL(RT.amount2, 0.0) 		as Commission,
				ISNULL(RT.extra_numeric2, 0.0) 		as Amount_Paid_To_Insurer,
				ISNULL(RT.extra_numeric3, 0.0) 		as Amount_Paid_By_Client,
				RT.branch_id 				as Branch_ID,
				ISNULL(RT.extra_numeric1, 0.0) 		as IPT,
				RT.branch_name 				as Branch_Name,
				RT.extra_char4 				as Insurer_Payment_Group,
				RT.extra_int1 				as Document_id,
				D.document_ref,
				RT.extra_char5				as Coinsurer_Policy,
				ISNULL(RT.extra_numeric5, 0.0)		as fee_amount
		FROM 	Report_Transaction 			RT
		JOIN 	Document 				D
			ON RT.extra_int1 = D.document_id'
		+ @sSQLSort
	)

SET NOCOUNT ON

DELETE FROM Report_Transaction

SET NOCOUNT OFF

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


