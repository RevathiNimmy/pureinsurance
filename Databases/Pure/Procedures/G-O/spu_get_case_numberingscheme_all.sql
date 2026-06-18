SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF

GO

EXECUTE DDLDropProcedure 'spu_get_all_casenumbering_scheme'

GO

CREATE PROCEDURE spu_get_all_casenumbering_scheme
 
AS
    SELECT NS.numbering_scheme_id,NS.description FROM numbering_scheme NS
    JOIN numbering_scheme_type NST
    ON NS.numbering_scheme_type_id= NST.numbering_scheme_type_id
    WHERE NST.code='CASE' and NS.is_deleted=0
    ORDER BY description

GO