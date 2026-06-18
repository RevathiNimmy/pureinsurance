SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SirRen_Claim_Link_Sel'
GO


CREATE PROCEDURE spu_SirRen_Claim_Link_Sel
    @Insurance_File_Cnt int
AS

/* AK 06082001 - To get Claims Link for Household / Motor */
BEGIN

    SELECT L.Claim_Id, L.GIS_Claim_Id, GIS_Driver_Id, GIS_Incident_Id
    From Claim_GIS_Link L, Gis_Policy_Link P
    Where P.Insurance_File_Cnt = @Insurance_File_Cnt
    And L.GIS_Policy_id = P.GIS_policy_Link_Id
END
GO


