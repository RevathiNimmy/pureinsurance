SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_debitinsurer'
GO

CREATE PROCEDURE spu_wp_debitinsurer
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

DECLARE
    @insurer_code VARCHAR(20),
    @insurer_name VARCHAR(60),        
    @gross_amount MONEY,
    @commission_amount MONEY,
    @fee_amount MONEY,
    @ipt MONEY,
    @vat MONEY,
    @tax MONEY,
    @currency_gross_amount MONEY,
    @currency_commission_amount MONEY,
    @currency_fee_amount MONEY,
    @currency_ipt MONEY,
    @currency_tax MONEY,
    @currency_vat MONEY,
    @insurer_fee_type CHAR(1)

/*Remove shared indicator from document_ref if it is there*/
DECLARE @SharedIndicator INT

SELECT @SharedIndicator = CHARINDEX('|',@DocumentRef)

If @SharedIndicator <> 0
BEGIN
    SELECT @DocumentRef = SUBSTRING(@DocumentRef, 1, @SharedIndicator -1)
END

/*Retrieve lead insurer short name*/
 DECLARE @insurer char(20)  
    
 SELECT @insurer=P.shortname FROM  
 insurance_file INF  
 JOIN party P ON INF.lead_insurer_cnt=P.party_cnt  
 WHERE INF.insurance_file_cnt=@InsuranceFileCnt 

/*Retrieve the insurance_file_cnt from event table*/
DECLARE @event_insurance_file_cnt INT

    SELECT  
    	@event_insurance_file_cnt = EIF.insurance_file_cnt  
    FROM  
    transaction_export_folder TEF  
    JOIN event_log EL
    ON EL.event_cnt=TEF.event_log_id  
    JOIN event_insurance_file EIF 
    ON EIF.insurance_folder_cnt=EL.event_cnt  
    WHERE TEF.document_ref = @DocumentRef
    AND TEF.accounts_export_status='c'  
    AND TEF.source_id = (SELECT source_id FROM insurance_file WHERE insurance_file_cnt = @InsuranceFileCnt)

/*Create the scrpt that will return the insurer values*/
DECLARE c_debitinsurer CURSOR SCROLL KEYSET READ_ONLY FOR
    SELECT 
        a.short_code,
        a.account_name,        
        
        /*Base amounts*/
        ABS(td.amount),
        (
            SELECT ABS(ISNULL(MAX(td2.amount),0))
            FROM transdetail td2
            JOIN transdetail_type tt2
                ON tt2.transdetail_type_id = td2.transdetail_type_id
            WHERE td2.document_id = td.document_id
            AND td2.account_id = td.account_id
            AND tt2.code = 'COMM'
        ),
        (
            SELECT ABS(ISNULL(MAX(td2.amount),0))
            FROM transdetail td2
            JOIN transdetail_type tt2
                ON tt2.transdetail_type_id = td2.transdetail_type_id
            WHERE td2.document_id = td.document_id
            AND td2.account_id = td.account_id
            AND tt2.code = 'IFEE'
        ),
        ABS(ROUND(((td.ref_amount * td.amount)/td.currency_amount),2)),
        ABS(ROUND(((td.ref_quantity * td.amount)/td.currency_amount),2)),
        
        /*Currency amounts*/
        ABS(td.currency_amount),
        (
            SELECT ABS(ISNULL(MAX(td2.currency_amount),0))
            FROM transdetail td2
            JOIN transdetail_type tt2
                ON tt2.transdetail_type_id = td2.transdetail_type_id
            WHERE td2.document_id = td.document_id
            AND td2.account_id = td.account_id
            AND tt2.code = 'COMM'
        ),
        (
            SELECT ABS(ISNULL(MAX(td2.currency_amount),0))
            FROM transdetail td2
            JOIN transdetail_type tt2
                ON tt2.transdetail_type_id = td2.transdetail_type_id
            WHERE td2.document_id = td.document_id
            AND td2.account_id = td.account_id
            AND tt2.code = 'IFEE'
        ),
        ABS(td.ref_amount),
        ABS(td.ref_quantity),
	(  
            SELECT insurer_fee_type FROM policy_fee
            WHERE insurance_file_cnt = @InsuranceFileCnt 
        )
    FROM document d
    JOIN transdetail td
        ON td.document_id = d.document_id
    JOIN account a
        ON a.account_id = td.account_id
    JOIN party p
        ON p.party_cnt = a.account_key
    JOIN party_type pt
        ON pt.party_type_id = p.party_type_id
    JOIN transdetail_type tt
        ON tt.transdetail_type_id = td.transdetail_type_id
    WHERE d.document_ref = @DocumentRef
    AND d.insurance_file_cnt = @InsuranceFileCnt
    AND pt.code = 'IN'
    AND tt.code = 'GROSS'


/*Return the values for the single insurer requested*/
OPEN c_debitinsurer

FETCH ABSOLUTE @Instance1 FROM c_debitinsurer INTO
    @insurer_code,
    @insurer_name,        
    @gross_amount,
    @commission_amount,
    @fee_amount,
    @ipt,
    @vat,
    @currency_gross_amount,
    @currency_commission_amount,
    @currency_fee_amount,
    @currency_ipt,
    @currency_vat,
    @insurer_fee_type 

CLOSE c_debitinsurer
DEALLOCATE c_debitinsurer

IF CHARINDEX ('MULTI',@insurer) > 0
BEGIN
    SELECT @ipt = ROUND(((SUM(ISNULL(tc1.value,0)) * @gross_amount) / @currency_gross_amount),2)
    FROM event_policy_coinsurers_section pcs  
    JOIN party pa  
        ON pa.party_cnt = pcs.party_cnt  and pcs.party_cnt =(select account_key from account where short_code = @insurer_code)
    LEFT JOIN event_tax_calculation tc1  
        ON tc1.policy_coinsurers_section_id = pcs.policy_coinsurers_section_id  
        AND tc1.is_commission_tax = 0  
    LEFT JOIN tax_group tg1  
        ON tg1.tax_group_id = pcs.tax_group_id  
    LEFT JOIN event_tax_calculation tc  
        ON tc.policy_coinsurers_section_id = pcs.policy_coinsurers_section_id 
        AND tc.is_commission_tax = 1  
    WHERE pcs.insurance_file_cnt = @event_insurance_file_cnt 
    AND tg1.code = 'IPT'


    SELECT @currency_ipt = ROUND(SUM(ISNULL(tc1.value,0)),2)
    FROM event_policy_coinsurers_section pcs  
    JOIN party pa  
        ON pa.party_cnt = pcs.party_cnt  and pcs.party_cnt =(select account_key from account where short_code = @insurer_code)
    LEFT JOIN event_tax_calculation tc1  
        ON tc1.policy_coinsurers_section_id = pcs.policy_coinsurers_section_id  
        AND tc1.is_commission_tax = 0  
    LEFT JOIN tax_group tg1  
        ON tg1.tax_group_id = pcs.tax_group_id  
    LEFT JOIN event_tax_calculation tc  
        ON tc.policy_coinsurers_section_id = pcs.policy_coinsurers_section_id 
        AND tc.is_commission_tax = 1  
    WHERE pcs.insurance_file_cnt = @event_insurance_file_cnt 
    AND tg1.code = 'IPT'

    SELECT @vat = ROUND(((SUM(ISNULL(tc1.value,0)) * @gross_amount) / @currency_gross_amount),2)
    FROM event_policy_coinsurers_section pcs  
    JOIN party pa  
        ON pa.party_cnt = pcs.party_cnt  and pcs.party_cnt =(select account_key from account where short_code = @insurer_code)
    LEFT JOIN event_tax_calculation tc1  
        ON tc1.policy_coinsurers_section_id = pcs.policy_coinsurers_section_id  
        AND tc1.is_commission_tax = 0  
    LEFT JOIN tax_group tg1  
        ON tg1.tax_group_id = pcs.tax_group_id  
    LEFT JOIN event_tax_calculation tc  
        ON tc.policy_coinsurers_section_id = pcs.policy_coinsurers_section_id 
        AND tc.is_commission_tax = 1  
    WHERE pcs.insurance_file_cnt = @event_insurance_file_cnt 
    AND tg1.code = 'VAT'

    SELECT @currency_vat = ROUND(SUM(ISNULL(tc1.value,0)),2)
    FROM event_policy_coinsurers_section pcs  
    JOIN party pa  
        ON pa.party_cnt = pcs.party_cnt  and pcs.party_cnt =(select account_key from account where short_code = @insurer_code)
    LEFT JOIN event_tax_calculation tc1  
        ON tc1.policy_coinsurers_section_id = pcs.policy_coinsurers_section_id  
        AND tc1.is_commission_tax = 0  
    LEFT JOIN tax_group tg1  
        ON tg1.tax_group_id = pcs.tax_group_id  
    LEFT JOIN event_tax_calculation tc  
        ON tc.policy_coinsurers_section_id = pcs.policy_coinsurers_section_id 
        AND tc.is_commission_tax = 1  
    WHERE pcs.insurance_file_cnt = @event_insurance_file_cnt 
    AND tg1.code = 'VAT'
 
    SELECT @tax = ROUND(((SUM(ISNULL(tc1.value,0)) * @gross_amount) / @currency_gross_amount),2)
    FROM event_policy_coinsurers_section pcs  
    JOIN party pa  
        ON pa.party_cnt = pcs.party_cnt  and pcs.party_cnt =(select account_key from account where short_code = @insurer_code)
    LEFT JOIN event_tax_calculation tc1  
        ON tc1.policy_coinsurers_section_id = pcs.policy_coinsurers_section_id  
        AND tc1.is_commission_tax = 0  
    LEFT JOIN tax_group tg1  
        ON tg1.tax_group_id = pcs.tax_group_id  
    LEFT JOIN event_tax_calculation tc  
        ON tc.policy_coinsurers_section_id = pcs.policy_coinsurers_section_id 
        AND tc.is_commission_tax = 1  
    WHERE pcs.insurance_file_cnt = @event_insurance_file_cnt 

    SELECT @currency_tax = ROUND(SUM(ISNULL(tc1.value,0)),2)
    FROM event_policy_coinsurers_section pcs  
    JOIN party pa  
        ON pa.party_cnt = pcs.party_cnt  and pcs.party_cnt =(select account_key from account where short_code = @insurer_code)
    LEFT JOIN event_tax_calculation tc1  
        ON tc1.policy_coinsurers_section_id = pcs.policy_coinsurers_section_id  
        AND tc1.is_commission_tax = 0  
    LEFT JOIN tax_group tg1  
        ON tg1.tax_group_id = pcs.tax_group_id  
    LEFT JOIN event_tax_calculation tc  
        ON tc.policy_coinsurers_section_id = pcs.policy_coinsurers_section_id 
        AND tc.is_commission_tax = 1  
    WHERE pcs.insurance_file_cnt = @event_insurance_file_cnt 

END
ELSE
BEGIN

    SELECT @ipt = ROUND(((SUM(ISNULL(TC.value,0)) * @gross_amount) / @currency_gross_amount),2) 
    FROM event_insurance_COB_section ICS  
    JOIN COB_rating_section CRS ON CRS.COB_rating_section_id=ICS.COB_rating_section_id  
    JOIN tax_group TG ON TG.tax_group_id=ICS.tax_group_id  
    JOIN event_tax_calculation TC 
    ON TC.insurance_file_cnt = @event_insurance_file_cnt
    AND TC.insurance_section_id=ICS.insurance_section_id 	
    AND TC.is_commission_tax=0  
    WHERE  
    ICS.insurance_file_cnt = @event_insurance_file_cnt
    AND TG.code = 'IPT'

    SELECT @currency_ipt = ROUND(SUM(ISNULL(TC.value,0)),2)
    FROM event_insurance_COB_section ICS  
    JOIN COB_rating_section CRS ON CRS.COB_rating_section_id=ICS.COB_rating_section_id  
    JOIN tax_group TG ON TG.tax_group_id=ICS.tax_group_id  
    JOIN event_tax_calculation TC 
    ON TC.insurance_file_cnt = @event_insurance_file_cnt
    AND TC.insurance_section_id=ICS.insurance_section_id 	
    AND TC.is_commission_tax=0  
    WHERE  
    ICS.insurance_file_cnt = @event_insurance_file_cnt
    AND TG.code = 'IPT'

    SELECT @vat = ROUND(((SUM(ISNULL(TC.value,0)) * @gross_amount) / @currency_gross_amount),2)
    FROM event_insurance_COB_section ICS  
    JOIN COB_rating_section CRS ON CRS.COB_rating_section_id=ICS.COB_rating_section_id  
    JOIN tax_group TG ON TG.tax_group_id=ICS.tax_group_id  
    JOIN event_tax_calculation TC 
    ON TC.insurance_file_cnt = @event_insurance_file_cnt
    AND TC.insurance_section_id=ICS.insurance_section_id 	
    AND TC.is_commission_tax=0  
    WHERE  
    ICS.insurance_file_cnt = @event_insurance_file_cnt
    AND TG.code = 'VAT'

    SELECT @currency_vat = ROUND(SUM(ISNULL(TC.value,0)),2)
    FROM event_insurance_COB_section ICS  
    JOIN COB_rating_section CRS ON CRS.COB_rating_section_id=ICS.COB_rating_section_id  
    JOIN tax_group TG ON TG.tax_group_id=ICS.tax_group_id  
    JOIN event_tax_calculation TC 
    ON TC.insurance_file_cnt = @event_insurance_file_cnt
    AND TC.insurance_section_id=ICS.insurance_section_id 	
    AND TC.is_commission_tax=0  
    WHERE  
    ICS.insurance_file_cnt = @event_insurance_file_cnt
    AND TG.code = 'VAT'

    SELECT @tax = ROUND(((SUM(ISNULL(TC.value,0)) * @gross_amount) / @currency_gross_amount),2)
    FROM event_insurance_COB_section ICS  
    JOIN COB_rating_section CRS ON CRS.COB_rating_section_id=ICS.COB_rating_section_id  
    JOIN tax_group TG ON TG.tax_group_id=ICS.tax_group_id  
    JOIN event_tax_calculation TC 
    ON TC.insurance_file_cnt = @event_insurance_file_cnt
    AND TC.insurance_section_id=ICS.insurance_section_id 	
    AND TC.is_commission_tax=0  
    WHERE  
    ICS.insurance_file_cnt = @event_insurance_file_cnt

    SELECT @currency_tax = ROUND(SUM(ISNULL(TC.value,0)),2)
    FROM event_insurance_COB_section ICS  
    JOIN COB_rating_section CRS ON CRS.COB_rating_section_id=ICS.COB_rating_section_id  
    JOIN tax_group TG ON TG.tax_group_id=ICS.tax_group_id  
    JOIN event_tax_calculation TC 
    ON TC.insurance_file_cnt = @event_insurance_file_cnt
    AND TC.insurance_section_id=ICS.insurance_section_id 	
    AND TC.is_commission_tax=0  
    WHERE  
    ICS.insurance_file_cnt = @event_insurance_file_cnt

END

SELECT
    @insurer_code 'insurer_code',
    @insurer_name 'insurer_name',        
    @gross_amount 'gross_amount',
    @commission_amount 'commission_amount',
    @fee_amount 'fee_amount',
    @ipt 'ipt',
    @vat 'vat',
    @tax 'tax',
    @currency_gross_amount 'currency_gross_amount',
    @currency_commission_amount 'currency_commission_amount',
    @currency_fee_amount 'currency_fee_amount',
    @currency_ipt 'currency_ipt',
    @currency_vat 'currency_vat',
    @currency_tax 'currency_tax',
    CASE @insurer_fee_type  
      WHEN 'C' THEN @fee_amount 
    END 'credit_fee_amount',
    CASE @insurer_fee_type 
      WHEN 'D' THEN @fee_amount 
    END 'debit_fee_amount'

GO

