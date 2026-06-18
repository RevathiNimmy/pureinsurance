SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_wp_householdbuildings
GO

CREATE PROCEDURE spu_wp_householdbuildings
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
    @sum_insured NUMERIC,  
    @rebuild_cost NUMERIC,  
    @cover_type VARCHAR(255),  
    @number_of_bedrooms INT OUTPUT,  
    @type_of_property VARCHAR(255),  
    @is_subsidence TINYINT OUTPUT,  
    @excess NUMERIC  
AS
BEGIN  
  
    SELECT  
        @address1 = address.address1,  
        @address2 = address.address2,  
        @address3 = address.address3,  
        @address4 = address.address4,  
        @postal_code = address.postal_code,  
        @sum_insured = household_buildings.sum_insured,  
        @rebuild_cost = household_buildings.rebuild_cost,  
        @cover_type = household_buildings.cover_type,  
        @number_of_bedrooms = household_buildings.number_of_bedrooms,  
        @type_of_property = household_buildings.type_of_property,  
        @is_subsidence = household_buildings.is_subsidence,  
        @excess = household_buildings.excess  
    FROM    
        household_buildings
        LEFT OUTER JOIN address  
            ON household_buildings.address_cnt = address.address_cnt 
    WHERE 
        insurance_file_cnt = @insurancefilecnt  

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

