SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_pmb_trans_det_coinsurer'
GO
		
	
CREATE PROCEDURE spu_pmb_trans_det_coinsurer
    @transaction_export_folder_cnt INT,
    @transaction_type CHAR(1),
    @total_coinsurers MONEY OUTPUT,
    @total_coinsurers_commission MONEY OUTPUT,
    @total_coinsurers_ipt MONEY OUTPUT,
    @total_coinsurers_fee MONEY OUTPUT
AS

DECLARE 
    @transaction_export_detail_id INT,
    @transaction_ledger_code CHAR(2),
    @account_type_code VARCHAR(10),
    @ceded_ref VARCHAR(10),
    @cover_share_percent FLOAT,
    @sum_insured_total MONEY,
    @charges_total MONEY,
    @taxes_total MONEY,
    @recoveries_total MONEY,
    @commission_excluded MONEY,
    @withholding_tax_excluded MONEY,
    @transaction_amount MONEY,
    @mapping_code VARCHAR(20),
    @transaction_account_key INT,
    @spare VARCHAR(255),
    @coinsurer_insurance_file_cnt INT,
    @coinsurer_party_cnt INT,
    @coinsurer_value MONEY,
    @coinsurer_commission_amount MONEY,
    @coinsurer_ipt_amount MONEY,
    @coinsurer_party_id INT,
    @coinsurer_shortname VARCHAR(255),
    @branch_id INT,
    @suspended TINYINT,
    @release_to_income TINYINT,
    @fee_percentage FLOAT,
    @total_fee_amount MONEY,
    @fee_amount MONEY,
    @tax_calculation_amount MONEY,
    @domiciled_for_tax TINYINT,
    @premium_tax_account_mapping_code VARCHAR(20),
    @premium_tax_account_key INT,
    @premium_tax_type_code VARCHAR(20),
    @premium_tax_amount MONEY,
    @insurer_fee_type CHAR(1),
    @bureau_party_cnt INT,
    @bureau_short_name VARCHAR(255)    
    

SELECT @total_coinsurers = 0
SELECT @total_coinsurers_commission = 0
SELECT @total_coinsurers_ipt = 0
SELECT @total_coinsurers_fee =0

/* Declare the Coinsurer Cursor */
DECLARE c_coinsurers CURSOR FAST_FORWARD FOR
    SELECT 
        MAX(C.party_cnt),
        MAX(P.party_cnt),
        MAX(P.shortname),
        SUM(ROUND(C.coinsurer_value, 2)),
        SUM(ROUND(C.coinsurer_commission_amount, 2)),
        SUM(ROUND(C.coinsurer_ipt_amount, 2)),
        MAX(I.Source_id),
        MAX(P.domiciled_for_tax),
		MAX(C.bureau_party_cnt),
		MAX(P1.shortname) 
    FROM Transaction_Export_Folder T
    JOIN Event_Insurance_File I
        ON I.insurance_file_cnt = T.insurance_file_cnt    
    JOIN Event_Policy_Coinsurers C
        ON C.insurance_file_cnt = I.insurance_file_cnt
    JOIN Party P
        ON P.party_cnt = C.party_cnt
	LEFT JOIN Party P1
		on P1.party_cnt = C.bureau_party_cnt
    WHERE T.transaction_export_folder_cnt = @transaction_export_folder_cnt
    GROUP BY C.party_cnt

/*Open the Coinsurers Cursor*/
OPEN c_coinsurers
FETCH NEXT FROM c_coinsurers INTO
    @coinsurer_party_cnt,
    @coinsurer_party_id,
    @coinsurer_shortname,
    @coinsurer_value,
    @coinsurer_commission_amount,
    @coinsurer_ipt_amount,
    @branch_id,
    @domiciled_for_tax,
    @bureau_party_cnt,
    @bureau_short_name

WHILE @@FETCH_STATUS = 0
BEGIN
    
    IF @bureau_party_cnt = 0
	SET @bureau_party_cnt = NULL

    /*Sort FSA Phase 3 suspension fields*/
    SELECT @suspended = 0
    SELECT @release_to_income = 0  
    
    /*Calculate Totals*/
    SELECT @total_coinsurers = @total_coinsurers + (@coinsurer_value - @coinsurer_commission_amount)
    SELECT @total_coinsurers_commission = @total_coinsurers_commission + @coinsurer_commission_amount
    SELECT @total_coinsurers_ipt = @total_coinsurers_ipt + @coinsurer_ipt_amount

    /*Post Transactions to the Insurer*/
    IF @coinsurer_value + @coinsurer_ipt_amount <> 0
    BEGIN
    
        /*Set GROSS fields*/
        SELECT @transaction_ledger_code = 'IN'
        SELECT @account_type_code = 'INSURERLED'
        SELECT @ceded_ref = NULL
        SELECT @cover_share_percent = 0
        SELECT @sum_insured_total = 0
        SELECT @charges_total = 0
        SELECT @recoveries_total = 0
        SELECT @commission_excluded = 0
        SELECT @withholding_tax_excluded = 0
        SELECT @mapping_code = ISNULL(@bureau_short_name,@coinsurer_shortname)
        SELECT @transaction_account_key = ISNULL(@bureau_party_cnt,@coinsurer_party_id)
        SELECT @spare = 'GROSS'

        /*Datasure calculate tax on insurers premium */
    SELECT @taxes_total = @coinsurer_ipt_amount

        /*Set transaction amount*/
    IF @domiciled_for_tax = 1
        BEGIN
            SELECT @transaction_amount = @coinsurer_value + @coinsurer_ipt_amount
        END
    ELSE
        BEGIN
            SELECT @transaction_amount = @coinsurer_value
        END
        
        IF @transaction_amount > 0 AND @transaction_type = 'D'
        BEGIN
            SELECT @transaction_amount = @transaction_amount * -1
        END
        
        IF @transaction_amount < 0 AND @transaction_type = 'C'
        BEGIN
            SELECT @transaction_amount = @transaction_amount * -1
        END
    
    IF @transaction_amount < 0
        BEGIN
            SELECT @taxes_total = @taxes_total * -1        
        END

        /*Get next export record*/
        SELECT
            @transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
        FROM Transaction_Export_Detail
        WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt
        
        IF @transaction_export_detail_id IS NULL
        BEGIN
            SELECT @transaction_export_detail_id = 1
        END
        
	IF EXISTS(SELECT NULL FROM Transaction_Export_Detail 
	WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt
	AND transaction_account_key = @bureau_party_cnt 
	AND transdetail_type_code = 'GROSS')
	BEGIN
		UPDATE Transaction_Export_Detail
	    SET 
	    transaction_amount = transaction_amount + @transaction_amount,
	    cover_share_percent = cover_share_percent + @cover_share_percent,
	    sum_insured_total = sum_insured_total + @sum_insured_total,
	    charges_total=charges_total+@charges_total,
	    taxes_total=taxes_total+@taxes_total,
	    recoveries_total=recoveries_total+@recoveries_total,
	    commission_excluded=commission_excluded+@commission_excluded,
	    withholding_tax_excluded=withholding_tax_excluded + @withholding_tax_excluded
	    WHERE 
	    transaction_export_folder_cnt = @transaction_export_folder_cnt
	    AND transaction_account_key = @bureau_party_cnt 
	    AND transdetail_type_code = 'GROSS'
	END
   	ELSE
   	BEGIN
		/*Insert GROSS record*/
		INSERT INTO Transaction_Export_Detail 
		(
			transaction_export_folder_cnt,
			transaction_export_detail_id,
			transaction_amount,
			transaction_ledger_code,
			account_type_code,
			ceded_ref,
			cover_share_percent,
			sum_insured_total,
			charges_total,
			taxes_total,
			recoveries_total,
			commission_excluded,
			withholding_tax_excluded,
			mapping_code,
			transaction_account_key,
			spare,
			suspended,
			release_to_income,
			release_account_code,
			transdetail_type_code
		)
		VALUES 
		(
			@transaction_export_folder_cnt,
			@transaction_export_detail_id,
			@transaction_amount,
			@transaction_ledger_code,
			@account_type_code,
			@ceded_ref,
			@cover_share_percent,
			@sum_insured_total,
			@charges_total,
			@taxes_total,
			@recoveries_total,
			@commission_excluded,
			@withholding_tax_excluded,
			@mapping_code,
			@transaction_account_key,
			@spare,
			0,
			0,
			NULL,
			'GROSS'
		)
	END	

    	/*Set COMM fields*/
    	SELECT @transaction_ledger_code = 'IN'
    	SELECT @account_type_code = 'INSURERLED'
    	SELECT @ceded_ref = NULL
    	SELECT @cover_share_percent = 0
    	SELECT @sum_insured_total = 0
    	SELECT @charges_total = 0
    	SELECT @recoveries_total = 0
   	 	SELECT @commission_excluded = 0
   	 	SELECT @withholding_tax_excluded = 0
    	SELECT @mapping_code = ISNULL(@bureau_short_name,@coinsurer_shortname)
    	SELECT @transaction_account_key = ISNULL(@bureau_party_cnt,@coinsurer_party_id)
    	SELECT @spare = 'COMM'
        /*Datasure calculate tax on insurers premium */
    	SELECT @tax_calculation_amount =
    	(SELECT ISNULL(SUM(ETC.value),0)
    	FROM  Event_Tax_Calculation ETC,
        Transaction_Export_Folder   T 
    	WHERE ETC.Insurance_file_cnt = T.insurance_file_cnt
    	AND T.transaction_export_folder_cnt = @transaction_export_folder_cnt
    	AND ETC.TransType = 'TTIC' AND ETC.insurer_party_cnt= @coinsurer_party_cnt)
    	SELECT @Taxes_Total = @tax_calculation_amount
       
    	/* Set transaction amount */
    	SELECT  @transaction_amount = @coinsurer_commission_amount
        
   	 	IF @transaction_amount < 0 AND @transaction_type = 'D'
    	BEGIN
       		SELECT @transaction_amount = @transaction_amount * -1
    	END
        
    	IF @transaction_amount > 0 AND @transaction_type = 'C'
    	BEGIN
       	 	SELECT @transaction_amount = @transaction_amount * -1
    	END
        
    	IF @transaction_amount < 0
    	BEGIN
        	SELECT @taxes_total = @taxes_total * -1
    	END        

        /*Get next export record*/
        SELECT @transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
        FROM Transaction_Export_Detail
        WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt
		
		IF EXISTS(SELECT NULL FROM Transaction_Export_Detail 
		WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt
		AND transaction_account_key = @bureau_party_cnt 
		AND transdetail_type_code = 'COMM' )
		BEGIN
			UPDATE Transaction_Export_Detail
			SET 
			transaction_amount = transaction_amount + @transaction_amount,
			cover_share_percent = cover_share_percent + @cover_share_percent,
			sum_insured_total = sum_insured_total + @sum_insured_total,
			charges_total=charges_total+@charges_total,
			taxes_total=taxes_total+@taxes_total,
			recoveries_total=recoveries_total+@recoveries_total,
			commission_excluded=commission_excluded+@commission_excluded,
			withholding_tax_excluded=withholding_tax_excluded + @withholding_tax_excluded
			WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt
			AND transaction_account_key = @bureau_party_cnt 
			AND transdetail_type_code = 'COMM'
		END
		ELSE
		BEGIN
	        /*Insert COMM record*/
	        INSERT INTO Transaction_Export_Detail
	        (
	            transaction_export_folder_cnt,
	            transaction_export_detail_id,
	            transaction_amount,
	            transaction_ledger_code,
	            account_type_code,
	            ceded_ref,
	            cover_share_percent,
	            sum_insured_total,
	            charges_total,
	            taxes_total,
	            recoveries_total,
	            commission_excluded,
	            withholding_tax_excluded,
	            mapping_code,
	            transaction_account_key,
	            spare,
	            suspended,
	            release_to_income,
	            release_account_code,
	            transdetail_type_code
	        )
	        VALUES 
	        (
	            @transaction_export_folder_cnt,
	            @transaction_export_detail_id,
	            @transaction_amount,
	            @transaction_ledger_code,
	            @account_type_code,
	            @ceded_ref,
	            @cover_share_percent,
	            @sum_insured_total,
	            @charges_total,
	            @taxes_total,
	            @recoveries_total,
	            @commission_excluded,
	            @withholding_tax_excluded,
	            @mapping_code,
	            @transaction_account_key,
	            @spare,
	            @suspended,
	            @release_to_income,
	            NULL,
	            'COMM'
	        )
		END

        IF EXISTS
            (
                SELECT   
                    NULL
                FROM Transaction_Export_Folder T 
                JOIN Event_Policy_Fee PF
                    ON PF.insurance_file_cnt = T.insurance_file_cnt
                WHERE T.transaction_export_folder_cnt = @transaction_export_folder_cnt
                AND PF.party_cnt = @coinsurer_party_cnt
            )
        BEGIN

            /*Get insurer fee details*/
        SET @total_fee_amount=0

        DECLARE IFEE_CURSOR CURSOR FAST_FORWARD FOR
        SELECT   
            epf.fee_percentage,
            ROUND(epf.total_fee, 2),
            ISNULL(epf.insurer_fee_type,'')    
        FROM transaction_export_folder tef 
        JOIN event_policy_fee epf
            ON epf.insurance_file_cnt = tef.insurance_file_cnt
        WHERE tef.transaction_export_folder_cnt = @transaction_export_folder_cnt
        AND epf.party_cnt = @coinsurer_party_cnt

        OPEN IFEE_CURSOR
        FETCH NEXT FROM IFEE_CURSOR INTO @fee_percentage,@fee_amount,@insurer_fee_type

        WHILE @@FETCH_STATUS=0

        BEGIN

            IF @fee_amount = 0
                SET @fee_amount = ROUND(@fee_percentage * (@coinsurer_value + @coinsurer_ipt_amount) / 100, 2)

	    IF @insurer_fee_type='C'
	        SET @fee_amount=-@fee_amount

	    SET @total_fee_amount=@total_fee_amount + @fee_amount

            FETCH NEXT FROM IFEE_CURSOR INTO @fee_percentage,@fee_amount,@insurer_fee_type

        END

        CLOSE IFEE_CURSOR
        DEALLOCATE IFEE_CURSOR

            /*Set IFEE fields*/
            SELECT @suspended = 0
            SELECT @release_to_income = 0 
            SELECT @transaction_ledger_code = 'IN'
            SELECT @account_type_code = 'INSURERLED'
            SELECT @ceded_ref = NULL
            SELECT @cover_share_percent = 0
            SELECT @sum_insured_total = 0
            SELECT @charges_total = 0
            SELECT @taxes_total = 0
            SELECT @recoveries_total = 0
            SELECT @commission_excluded = 0
            SELECT @withholding_tax_excluded = 0
            SELECT @mapping_code = ISNULL(@bureau_short_name,@coinsurer_shortname)
        	SELECT @transaction_account_key = ISNULL(@bureau_party_cnt,@coinsurer_party_id)
            SELECT @spare = 'IFEE'
            SELECT @transaction_amount = @total_fee_amount

            /*Set total fee amount to return*/
            SELECT @total_coinsurers_fee = @total_coinsurers_fee + @transaction_amount
            
            
            SELECT @transaction_amount = @transaction_amount * -1
            
            /*Get next export record*/
            SELECT @transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
            FROM Transaction_Export_Detail
            WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt

            IF @transaction_export_detail_id IS NULL
            BEGIN
                SELECT @transaction_export_detail_id = 1
            END
			
			IF EXISTS(SELECT NULL from Transaction_Export_Detail 
			WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt
			AND transaction_account_key = @bureau_party_cnt 
			AND transdetail_type_code = 'IFEE' )
			BEGIN
				UPDATE Transaction_Export_Detail
				SET 
					transaction_amount = transaction_amount + @transaction_amount,
					cover_share_percent = cover_share_percent + @cover_share_percent,
					sum_insured_total = sum_insured_total + @sum_insured_total,
					charges_total=charges_total+@charges_total,
					taxes_total=taxes_total+@taxes_total,
					recoveries_total=recoveries_total+@recoveries_total,
					commission_excluded=commission_excluded+@commission_excluded,
					withholding_tax_excluded=withholding_tax_excluded + @withholding_tax_excluded
					where transaction_export_folder_cnt = @transaction_export_folder_cnt
					and transaction_account_key = @bureau_party_cnt 
					and transdetail_type_code = 'IFEE'
			END
			ELSE
			BEGIN
				/*Insert IFEE record*/
				INSERT INTO Transaction_Export_Detail 
				(
					transaction_export_folder_cnt,
					transaction_export_detail_id,
					transaction_amount,
					transaction_ledger_code,
					account_type_code,
					ceded_ref,
					cover_share_percent,
					sum_insured_total,
					charges_total,
					taxes_total,
					recoveries_total,
					commission_excluded,
					withholding_tax_excluded,
					mapping_code,
					transaction_account_key,
					spare,
					suspended,
					release_to_income,
					release_account_code,
					transdetail_type_code
				)
				VALUES
				(
					@transaction_export_folder_cnt,
					@transaction_export_detail_id,
					@transaction_amount,
					@transaction_ledger_code,
					@account_type_code,
					@ceded_ref,
					@cover_share_percent,
					@sum_insured_total,
					@charges_total,
					@taxes_total,
					@recoveries_total,
					@commission_excluded,
					@withholding_tax_excluded,
					@mapping_code,
					@transaction_account_key,
					@spare,
					@suspended,
					@release_to_income,
					NULL,
					'IFEE'
				)
        	END
        END

    END

    /* Fetch Next */
    FETCH NEXT FROM c_coinsurers INTO
        @coinsurer_party_cnt ,
        @coinsurer_party_id,
        @coinsurer_shortname,
        @coinsurer_value,
        @coinsurer_commission_amount,
        @coinsurer_ipt_amount,
        @branch_id,
        @domiciled_for_tax,
		@bureau_party_cnt,
		@bureau_short_name



END

CLOSE c_coinsurers
DEALLOCATE c_coinsurers

GO
