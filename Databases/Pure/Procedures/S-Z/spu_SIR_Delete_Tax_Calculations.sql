SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


Execute DDLDropProcedure 'spu_SIR_Delete_Tax_Calculations'
GO


CREATE PROCEDURE spu_SIR_Delete_Tax_Calculations
    @insurance_file_cnt int,
    @risk_cnt int,
    @transtype_premium varchar(20),
    @transtype_commission varchar(20),
    @ri_arrangement_line_id int = Null
AS

    If IsNull(@ri_arrangement_line_id, 0) = 0 
        -- Delete any temporary (no ri_arrangement_line_id) reinsurance tax 
        -- calculation entries for the specified transtype
        Delete From
                tax_calculation  
        Where   insurance_file_cnt = @insurance_file_cnt  
        And     risk_cnt = @risk_cnt  
        And     ri_arrangement_line_id Is Null
        And     transtype In (@transtype_premium, @transtype_commission)
		
    Else
        -- Delete any existing reinsurance taxes for the given transtype
        Delete From
                tax_calculation  
        Where   insurance_file_cnt = @insurance_file_cnt  
        And     risk_cnt = @risk_cnt  
        And     ri_arrangement_line_id = @ri_arrangement_line_id
        And     transtype In (@transtype_premium, @transtype_commission)


GO

