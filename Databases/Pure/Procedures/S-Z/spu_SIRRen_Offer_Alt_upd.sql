SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIRRen_Offer_Alt_upd'
GO


CREATE PROCEDURE spu_SIRRen_Offer_Alt_upd
    @insurance_folder_cnt int,
    @Offer_Alt smallint
AS

/* AK 111001 - Procedure to change Offer Alternative Quote flag in Renewal Control */
BEGIN
    UPDATE Renewal_Control
            SET Offer_Alt = @Offer_Alt
            WHERE insurance_folder_cnt = @insurance_folder_cnt
END
GO


