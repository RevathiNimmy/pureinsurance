SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_party_type_sel'
GO


CREATE PROCEDURE spu_party_type_sel
    @party_type_code char(10)
AS

SELECT
party_type_id,
code,
description,
is_deleted,
effective_date,
party_other_posting_type_id,
gis_screen_id,
is_individual_party_type
FROM
party_type
WHERE
code=@party_type_code

GO


