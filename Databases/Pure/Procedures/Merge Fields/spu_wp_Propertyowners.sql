SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_wp_Propertyowners'
GO


CREATE PROCEDURE spu_wp_Propertyowners
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT,
    @is_buildings TINYINT OUTPUT,
    @b_sum_insured NUMERIC,
    @b_contents_sum_insured NUMERIC,
    @b_excess NUMERIC,
    @is_public_liability TINYINT OUTPUT,
    @pl_indemnity_limit NUMERIC,
    @is_employers_liability TINYINT OUTPUT,
    @el_indemnity_limit NUMERIC,
    @is_residential_units TINYINT OUTPUT,
    @ru_contents_sum_insured NUMERIC,
    @is_engineering TINYINT OUTPUT,
    @e_property_insured Varchar(255),
    @e_contingencies Varchar(255),
    @e_indemnity_limit NUMERIC,
    @e_excess NUMERIC
AS


SELECT
    @is_buildings = property_owners.is_buildings ,
    @b_sum_insured = property_owners.b_sum_insured ,
    @b_contents_sum_insured = property_owners.b_contents_sum_insured ,
    @b_excess = property_owners.b_excess ,
    @is_public_liability = property_owners.is_public_liability ,
    @pl_indemnity_limit = property_owners.pl_indemnity_limit ,
    @is_employers_liability = property_owners.is_employers_liability ,
    @el_indemnity_limit = property_owners.el_indemnity_limit ,
    @is_residential_units = property_owners.is_residential_units ,
    @ru_contents_sum_insured = property_owners.ru_contents_sum_insured ,
    @is_engineering = property_owners.is_engineering ,
    @e_property_insured = property_owners.e_property_insured ,
    @e_contingencies = property_owners.e_contingencies ,
    @e_indemnity_limit = property_owners.e_indemnity_limit ,
    @e_excess = property_owners.e_excess
FROM Property_owners
WHERE Property_owners.insurance_file_cnt = @insurancefilecnt
GO


