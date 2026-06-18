SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Gis_Scheme_Data_del'
GO

CREATE PROCEDURE spe_Gis_Scheme_Data_del
    @gis_scheme_id int
AS

DELETE FROM Gis_Scheme_Data
WHERE gis_scheme_id = @gis_scheme_id

GO

