SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Check_DirectToInsurer'
GO

CREATE PROCEDURE spu_ACT_Check_DirectToInsurer
    
    @document_id INT
    
AS

DECLARE 
    @document_ref VARCHAR(25),
    @source_id INT,
    @spare VARCHAR(20)

SELECT
    @document_ref = document_ref,
    @source_id = company_id
FROM document 
WHERE document_id = @document_id

SELECT @spare = 'DDREV ' + @document_ref

SELECT
    transdetail_id
FROM transdetail
WHERE spare = @spare
AND company_id = @source_id

GO
