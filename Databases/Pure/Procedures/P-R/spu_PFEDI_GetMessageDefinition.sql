DDLDropProcedure 'spu_PFEDI_GetMessageDefinition'
GO

CREATE PROCEDURE spu_PFEDI_GetMessageDefinition
(
    @pfedidefinition_id INT,
    @Section VARCHAR(1)
)
AS

SELECT
    output_order,
    array_index,
    column_name,
    column_size,
    column_type,
    decimal_accuracy,
    signed_field,
    section
FROM
    PFEDIDefinitionFields
WHERE
    pfedidefinition_id=@pfedidefinition_id
AND section=@Section
GO