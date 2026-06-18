SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXEC DDLDropProcedure 'spu_copy_insurance_cob_section_to_event'
GO


CREATE PROCEDURE spu_copy_insurance_cob_section_to_event
    @event_cnt int,
    @insurance_file_cnt int
AS

BEGIN

    INSERT INTO event_insurance_cob_section
    (
    	insurance_file_cnt,
    	Insurance_section_id,
    	COB_Rating_section_id,
    	Premium_Excluding_Tax,
    	Tax_applied,
    	Premium_Including_Tax,
    	Tax_group_id,
    	Commission_Cnt,
    	Commission_Percentage,
    	Commission_Charge,
    	Commission_Net,
    	Commission_tax_applied,
    	Commission_Payable,
    	Commission_Tax_group_id,
    	Is_minimum_brokerage,
    	Override_rate_table,
    	Base_Premium_Excluding_Tax,
    	Base_Tax_Applied,
    	Base_Premium_Including_Tax,
    	Base_Commission_Charge,
    	Base_Commission_Net,
    	Base_Commission_Tax_Applied,
    	Base_Commission_Payable,
    	is_applied
    )
    SELECT
        @event_cnt,
    	Insurance_section_id,
    	COB_Rating_section_id,
    	Premium_Excluding_Tax,
    	Tax_applied,
    	Premium_Including_Tax,
    	Tax_group_id,
    	Commission_Cnt,
    	Commission_Percentage,
    	Commission_Charge,
    	Commission_Net,
    	Commission_tax_applied,
    	Commission_Payable,
    	Commission_Tax_group_id,
    	Is_minimum_brokerage,
    	Override_rate_table,
    	Base_Premium_Excluding_Tax,
    	Base_Tax_Applied,
    	Base_Premium_Including_Tax,
    	Base_Commission_Charge,
    	Base_Commission_Net,
    	Base_Commission_Tax_Applied,
    	Base_Commission_Payable,
    	is_applied
    FROM
        insurance_cob_section
    WHERE
        insurance_file_cnt = @insurance_file_cnt
    ORDER BY
        Insurance_section_id

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

