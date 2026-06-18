SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Get_Risk_Group'
GO


CREATE PROCEDURE spu_Get_Risk_Group
    @gis_scheme_id int

AS
    SELECT    risk_group_id 
    FROM      Gis_Qem_Usage
    WHERE     gis_scheme_id = @gis_scheme_id

GO
