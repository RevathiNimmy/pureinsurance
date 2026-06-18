SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_ElementExtras'
GO


CREATE PROCEDURE spu_ACT_Select_ElementExtras
    @element_id int = NULL,
    @element_name varchar(30) = NULL
AS


SELECT
    element_id,
    totalling_id,
    description,
    report_map_id,
    account_map_id,
    spare_number,
    spare_text
    FROM ElementExtras

    WHERE element_id = @element_id
GO


