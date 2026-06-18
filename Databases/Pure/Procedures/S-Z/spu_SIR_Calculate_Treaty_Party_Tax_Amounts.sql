SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


Execute DDLDropProcedure 'spu_SIR_Calculate_Treaty_Party_Tax_Amounts'
GO


CREATE PROCEDURE spu_SIR_Calculate_Treaty_Party_Tax_Amounts    
    @insurance_file_cnt int, 
    @risk_cnt int,
    @ri_arrangement_line_id int,
    @party_cnt int,
    @premium money, 
    @commission money, 
    @premium_transtype varchar(20), 
    @commission_transtype varchar(20), 
    @premium_tax money output, 
    @commission_tax money output
AS    
    
    Declare @share_percent money,
            @tax_group_id int,
            @policy_company_id int,
            @policy_currency_id int,
            @is_withholding_tax tinyint
     
    -- get policy details    
    SELECT  @policy_company_id = branch_id, 
            @policy_currency_id = currency_id    
    FROM    insurance_file    
    WHERE   insurance_file_cnt = @insurance_file_cnt    

   
    -- get party tax group details    
    SELECT  @tax_group_id = pins.tax_group_id,
            @is_withholding_tax = tg.is_withholding_tax
    FROM    party_insurer pins
    LEFT JOIN 
            tax_group tg ON tg.tax_group_id = pins.tax_group_id
    WHERE   pins.party_cnt = @party_cnt

    
    -- if this treaty party has tax a specified tax group    
    If ISNULL(@tax_group_id,0) <> 0 Begin    
        -- Calculate the Tax and regenerate the tax_calculations for premium    
        Execute spu_SIR_Calculate_Tax_Amounts    
            @company_id = @policy_company_id, 
            @tax_group_id = @tax_group_id, 
            @transtype = @premium_transtype, 
            @currency_id = @policy_currency_id, 
            @amount = @premium, 
            @tax_currency_amount = @premium_tax output, 
            @tax_base_amount = 0, 
            @insurance_file_cnt = @insurance_file_cnt, 
            @risk_cnt = @risk_cnt, 
            @associated_key_id = @party_cnt,
            @associated_key_id2 = @ri_arrangement_line_id
        
        -- Calculate the Tax and regenerate the tax_calculations for commission    
        Execute spu_SIR_Calculate_Tax_Amounts    
            @company_id = @policy_company_id, 
            @tax_group_id = @tax_group_id, 
            @transtype = @commission_transtype, 
            @currency_id = @policy_currency_id, 
            @amount = @commission, 
            @tax_currency_amount = @commission_tax output, 
            @tax_base_amount = 0, 
            @insurance_file_cnt = @insurance_file_cnt, 
            @risk_cnt = @risk_cnt , 
            @associated_key_id = @party_cnt,
            @associated_key_id2 = @ri_arrangement_line_id

        If IsNull(@is_withholding_tax, 0) = 1
            Select  @premium_tax = -@premium_tax,
                    @commission_tax = -@commission_tax
    End Else Begin
        -- this treaty party has no specified tax group so just set the tax
        -- amounts to zero...
        Select  @premium_tax = 0,
                @commission_tax = 0
    End

GO


