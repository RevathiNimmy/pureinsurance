SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Get_Preferred_Document_Date'
GO


CREATE PROCEDURE spu_ACT_Get_Preferred_Document_Date
    @cashlistitem_id int,
    @document_date datetime OUTPUT
AS

BEGIN

SELECT @document_date = cl.list_date
FROM cashlistitem cli
JOIN cashlist cl ON cli.cashlist_id = cl.cashlist_id
WHERE cashlistitem_id = @cashlistitem_id

END
GO