/* Select sub_branch records for a given branch (source_id) */
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_sub_branch_sel'
GO


CREATE PROCEDURE spu_sub_branch_sel

@source_id int

AS

SELECT  sub_branch_id
    source_id,
    caption_id ,
    code char ,
    description ,
    is_deleted,
    effective_date ,
    reg_no_1,
    reg_no_2,
    address1,
    address2,
    address3,
    address4,
    postal_code,
    country_id,
    phone_area_code,
    phone_number,
    phone_extension,
    fax_area_code,
    fax_number,
    fax_extension,
    email,
    vat_no
FROM sub_branch
WHERE source_id = @source_id
-- KN (CMG) 040203 Issue 1941
-- only display sub branches that have not been deleted
and is_deleted <> 1 
ORDER BY description

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO
