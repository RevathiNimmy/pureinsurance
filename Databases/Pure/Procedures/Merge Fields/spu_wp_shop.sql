SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_wp_shop
GO

CREATE PROCEDURE spu_wp_shop
    @PartyCnt INT,  
    @InsuranceFileCnt INT,  
    @ClaimCnt INT,  
    @DocumentRef VARCHAR(25),  
    @Instance1 INT,  
    @Instance2 INT,  
    @Instance3 INT,  
    @address1 VARCHAR(255),  
    @address2 VARCHAR(255),  
    @address3 VARCHAR(255),  
    @address4 VARCHAR(255),  
    @postal_code VARCHAR(255),  
    @is_property_damage TINYINT OUTPUT,  
    @buildings_sum_insured NUMERIC,  
    @improvements_sum_insured NUMERIC,  
    @contents_sum_insured NUMERIC,  
    @s_i_t_sum_insured NUMERIC,  
    @trade_f_a_f_sum_insured NUMERIC,  
    @stock_of_tobacco_sum_insured NUMERIC,  
    @stock_of_alcohol_sum_insured NUMERIC,  
    @is_business_interruption TINYINT OUTPUT,  
    @loss_of_income_sum_insured NUMERIC,  
    @maximum_indemnity_period INT OUTPUT,  
    @is_glass TINYINT OUTPUT,  
    @glass_type INT OUTPUT,  
    @cover_amount NUMERIC,  
    @is_frozen_foods TINYINT OUTPUT,  
    @ff_description VARCHAR(255),  
    @ff_sum_insured NUMERIC,  
    @is_all_risks TINYINT OUTPUT,  
    @ar_description VARCHAR(255),  
    @ar_sum_insured NUMERIC,  
    @is_loss_of_licence TINYINT OUTPUT,  
    @ll_liability_limit NUMERIC 
AS 
BEGIN 
  
    SELECT  
        @address1 = address.address1,  
        @address2 = address.address2,  
        @address3 = address.address3,  
        @address4 = address.address4,  
        @postal_code = address.postal_code,  
        @is_property_damage = shop.is_property_damage,  
        @buildings_sum_insured = shop.buildings_sum_insured,  
        @improvements_sum_insured = shop.improvements_sum_insured,  
        @contents_sum_insured = shop.contents_sum_insured,  
        @s_i_t_sum_insured = shop.s_i_t_sum_insured,  
        @trade_f_a_f_sum_insured = shop.trade_f_a_f_sum_insured,  
        @stock_of_tobacco_sum_insured = shop.stock_of_tobacco_sum_insured,  
        @stock_of_alcohol_sum_insured = shop.stock_of_alcohol_sum_insured,  
        @is_business_interruption = shop.is_business_interruption,  
        @loss_of_income_sum_insured = shop.loss_of_income_sum_insured,  
        @maximum_indemnity_period = shop.maximum_indemnity_period,  
        @is_glass = shop.is_glass,  
        @glass_type = shop.glass_type,  
        @cover_amount = shop.cover_amount,  
        @is_frozen_foods = shop.is_frozen_foods,  
        @ff_description = shop.ff_description,  
        @ff_sum_insured = shop.ff_sum_insured,  
        @is_all_risks = shop.is_all_risks,  
        @ar_description = shop.ar_description,  
        @ar_sum_insured = shop.ar_sum_insured,  
        @is_loss_of_licence = shop.is_loss_of_licence,  
        @ll_liability_limit = shop.ll_liability_limit  
    FROM    
        shop
        LEFT OUTER JOIN address  
            ON shop.address_cnt = address.address_cnt
    WHERE 
        insurance_file_cnt = @insurancefilecnt  

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

