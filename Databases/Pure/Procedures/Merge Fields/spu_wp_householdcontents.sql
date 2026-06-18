SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_wp_householdcontents
GO

CREATE PROCEDURE spu_wp_householdcontents
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
    @sum_insured INT OUTPUT,  
    @cover_type VARCHAR(255),  
    @is_all_risks_cover TINYINT OUTPUT,  
    @all_risks_sum_insured NUMERIC,  
    @is_freezer_cover TINYINT OUTPUT,  
    @freezer_sum_insured NUMERIC,  
    @is_credit_card_cover TINYINT OUTPUT,  
    @credit_card_limit NUMERIC,  
    @is_money_cover TINYINT OUTPUT,  
    @money_limit NUMERIC,  
    @is_personal_possessions TINYINT OUTPUT,  
    @personal_possessions_sum NUMERIC,  
    @number_of_bedrooms INT OUTPUT,  
    @type_of_property VARCHAR(255),  
    @excess NUMERIC  
AS  
BEGIN
      
    SELECT  
        @address1 = address.address1,  
        @address2 = address.address2,  
        @address3 = address.address3,  
        @address4 = address.address4,  
        @postal_code = address.postal_code,  
        @sum_insured = household_contents.sum_insured,  
        @cover_type = household_contents.cover_type,  
        @is_all_risks_cover = household_contents.is_all_risks_cover,  
        @all_risks_sum_insured = household_contents.all_risks_sum_insured,  
        @is_freezer_cover = household_contents.is_freezer_cover,  
        @freezer_sum_insured = household_contents.freezer_sum_insured,  
        @is_credit_card_cover = household_contents.is_credit_card_cover,  
        @credit_card_limit = household_contents.credit_card_limit,  
        @is_money_cover = household_contents.is_money_cover,  
        @money_limit = household_contents.money_limit,  
        @is_personal_possessions = household_contents.is_personal_possessions,  
        @personal_possessions_sum = household_contents.personal_possessions_sum,  
        @number_of_bedrooms = household_contents.number_of_bedrooms,  
        @type_of_property = household_contents.type_of_property,  
        @excess = household_contents.excess  
    FROM    
        household_contents
        LEFT OUTER JOIN address  
            ON household_contents.address_cnt = address.address_cnt 
    WHERE 
        insurance_file_cnt = @insurancefilecnt  

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO