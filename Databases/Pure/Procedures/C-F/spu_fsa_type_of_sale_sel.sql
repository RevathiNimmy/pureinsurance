EXECUTE DDLDropProcedure 'spu_fsa_type_of_sale_sel'
GO

CREATE PROCEDURE spu_fsa_type_of_sale_sel

AS

    SELECT 
        fsa_type_of_sale_id,
        description
    FROM fsa_type_of_sale
    WHERE is_deleted = 0
    ORDER BY fsa_type_of_sale_id

GO