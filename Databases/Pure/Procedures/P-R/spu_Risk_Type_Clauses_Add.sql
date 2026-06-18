SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Risk_Type_Clauses_Add'
GO


CREATE PROCEDURE spu_Risk_Type_Clauses_Add
    @risk_type_id int,
    @document_template_id int
AS


INSERT INTO wording_risk_type_link (
            risk_type_id,
            document_template_id)
VALUES  (@risk_type_id,
     @document_template_id)
GO


