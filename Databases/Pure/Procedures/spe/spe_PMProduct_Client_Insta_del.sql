SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_PMProduct_Client_Insta_del'
GO
CREATE PROCEDURE spe_PMProduct_Client_Insta_del
    @pmproduct_id smallint
AS
    DELETE FROM PMProduct_Client_Install
    WHERE pmproduct_id = @pmproduct_id

GO

