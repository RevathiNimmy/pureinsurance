DDLDropProcedure 'spu_GetFailedClaimTransaction'
Go

CREATE PROCEDURE spu_GetFailedClaimTransaction
AS
BEGIN
	-- get failed posting for claims or those without transaction export folder
	-- join to payment table via amount and payment date
	-- this is only guess work...user will decide if its legitimate for reposting
	SELECT  (SELECT claim_number FROM Claim WHERE claim_id = sf.loss_id) ClaimNumber,
			sf.document_ref,
            sf.document_date,
            IsNull(p.date_of_payment,'') PaymentDate,
            (sf.premium_total * -1) PremiumTotal,
            IsNull(p.amount,0) PaymentAmount,
            (SELECT shortname FROM Party WHERE party_cnt = Isnull(p.party_cnt,0)) PaymentPartyCode,
			IsNull(tef.accounts_export_status,'No Export') PostingStatus,
			IsNull(tef.transaction_export_folder_cnt,0) ExportFolderCnt,
			sf.insurance_file_cnt,
            sf.loss_id,			
            IsNull(p.party_cnt,0) PayPartyCnt,
            IsNull(p.reserve_id,0) OriginalReserveID,
            IsNull(p.Payment_ID,0) PaymentID
	FROM	Stats_Folder sf
    LEFT JOIN Payment p ON p.claim_id = sf.loss_id AND CONVERT(datetime,CONVERT(Char(10),p.date_of_payment,101)) = CONVERT(datetime,CONVERT(Char(10),sf.document_date,101)) AND p.amount = sf.premium_total * -1
    LEFT JOIN Transaction_Export_Folder tef ON sf.insurance_file_cnt = tef.insurance_file_cnt AND sf.document_ref = tef.document_ref
	WHERE	tef.accounts_export_status <> 'c' 
	AND     (LEFT(tef.document_ref,3) IN ('CLO', 'CLA', 'CLP')
    OR		tef.accounts_export_status IS NULL AND (LEFT(tef.document_ref,3) IN ('CLO', 'CLA', 'CLP')))
    ORDER BY sf.document_ref
END

