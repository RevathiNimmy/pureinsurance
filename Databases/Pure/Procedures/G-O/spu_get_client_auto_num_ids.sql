SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_client_auto_num_ids'
GO


CREATE PROCEDURE spu_get_client_auto_num_ids
    @code VARCHAR(10)
AS

    SELECT  NS.numbering_scheme_id,P.description,NST.code
    FROM party_type P WITH(NOLOCK)
    JOIN numbering_scheme NS WITH(NOLOCK)
        ON P.party_type_id= NS.party_type_id
    JOIN numbering_scheme_type NST WITH(NOLOCK)
    ON NS.numbering_scheme_type_id= NST.numbering_scheme_type_id
    WHERE P.code = @code
    AND NS.is_deleted=0
GO
