-- AK03082004: spu_GIS_Policy_Numbers_Sel.sql
-- Selects a policy or covernote number range from the Policy_Numbers table

SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_GIS_Policy_Numbers_Sel'
GO

CREATE PROCEDURE spu_GIS_Policy_Numbers_Sel
    @InsurerID integer,
    @SchemeID integer,
    @BusinessClass char(2),
    @CoverNote integer
AS

    SELECT pn.policy_numbers_id,
        pn.last_used,
        pn.range_start,
        pn.range_end,
        pn.new_range_start,
        pn.new_range_end,
        pn.delete_current,
        pnf.prefix,
        pnf.suffix,
        @BusinessClass as class_of_business,
        pn.is_covernote_no,
        pnf.validation_mask,
        pnf.check_digit,
        pn.min_available_numbers
    FROM Policy_Numbers pn,
         Policy_Numbers_Format pnf,
         gis_scheme_policy_numbers gspn         
    WHERE gspn.gis_scheme_id = @SchemeID
    AND gspn.policy_numbers_id = pn.policy_numbers_id
    AND gspn.policy_numbers_format_id = pnf.policy_numbers_format_id
    AND is_covernote_no = @CoverNote


GO
