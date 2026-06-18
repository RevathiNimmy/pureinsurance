SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Insurance_File_Tax_upd'
GO

CREATE PROCEDURE spu_Insurance_File_Tax_upd
    @insurance_file_cnt int,
    @tax_band_id int,
    @premium money,
    @percentage float,
    @value money,
    @is_value tinyint,
    @is_manually_changed tinyint,
    @basis_value money,
    @calc_basis int,
    @sum_insured money,
    @sum_insured_rounded tinyint,
    @currency_id smallint,
    @allow_tax_credit tinyint,
    @original_sum_insured money,
    @country_id int,
    @state_id int,
    @class_of_business_id int,
    @tax_group_id int,
    @sequence tinyint,
    @is_deleted tinyint,
	@tax_calculation_cnt int,
	@is_not_applied_to_client tinyint,
	@include_tax_in_instalments tinyint,
    @spread_tax_across_instalments tinyint,
    @apply_tax_by varchar(20) =null
AS
    -- Should we delete or update this record?
    IF @is_deleted > 0
        DELETE  Tax_Calculation
    	WHERE   tax_calculation_cnt = @tax_calculation_cnt
    ELSE
		BEGIN
			UPDATE  Tax_Calculation
			SET     premium = @premium,
					percentage = @percentage,
					value = @value,
					is_value = @is_value,
					is_manually_changed = @is_manually_changed,
					basis_value = @basis_value,
					calc_basis = @calc_basis,
					sum_insured = @sum_insured,
					sum_insured_rounded = @sum_insured_rounded,
					currency_id = @currency_id,
					allow_tax_credit = @allow_tax_credit,
					original_sum_insured = @original_sum_insured,
					country_id = @country_id,
					state_id = @state_id,
					class_of_business_id = @class_of_business_id,
					tax_group_id = @tax_group_id,
					sequence = @sequence,
					is_not_applied_to_client =@is_not_applied_to_client,
					include_tax_in_instalments =@include_tax_in_instalments,
					spread_tax_across_instalments =@spread_tax_across_instalments
    		WHERE   tax_calculation_cnt = @tax_calculation_cnt

			DECLARE @tax_amount MONEY  

			SELECT @tax_amount=SUM(value)  
			FROM Tax_Calculation rt  
			WHERE insurance_file_cnt = @insurance_file_cnt  
					AND rt.transtype IN ('TTR','TTIF')  

			UPDATE insurance_file  
			SET tax_amount = ISNULL(@tax_amount,0)  
			WHERE insurance_file_cnt = @insurance_file_cnt
		END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
