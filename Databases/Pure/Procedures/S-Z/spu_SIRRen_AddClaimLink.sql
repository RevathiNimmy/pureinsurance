SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIRRen_AddClaimLink'
GO


CREATE PROCEDURE spu_SIRRen_AddClaimLink
    @Claim_Id Int,
    @Gis_Policy_Id Int,
    @Gis_Claim_Id Int,
    @Gis_Driver_Id Int = NULL,
    @Gis_Incident_Id int = NULL
AS

/* To add Claim Links - For GII specific Claims/Risk data merger */
BEGIN

    INSERT INTO Claim_Gis_Link
        (Claim_Id, Gis_Policy_Id, Gis_Claim_Id, Gis_Driver_Id, Gis_Incident_Id)
    VALUES
        (@Claim_Id, @Gis_Policy_Id, @Gis_Claim_Id, @Gis_Driver_Id, @Gis_Incident_Id)

END
GO


