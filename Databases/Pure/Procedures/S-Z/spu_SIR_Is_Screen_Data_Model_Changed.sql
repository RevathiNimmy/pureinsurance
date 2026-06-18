SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF
GO
--**********************************************************************************************
-- If exists and different from exisitng 
-- returns previous Data Model Id otherwise ZERO and gis_policy_link_id if there is any
--**********************************************************************************************

EXECUTE DDLDropProcedure 'spu_SIR_Is_Screen_Data_Model_Changed'
GO

CREATE PROCEDURE spu_SIR_Is_Screen_Data_Model_Changed
	@party_cnt INT,
	@previous_data_model_id int OUTPUT,
	@gis_policy_link_id int OUTPUT
AS
BEGIN
    --Fetch previous model code from GIS_POLICY_LINK
    SELECT @previous_data_model_id = GPL.gis_data_model_id,
	@gis_policy_link_id = GPL.gis_policy_link_id
    FROM GIS_Policy_Link GPL
    INNER JOIN GIS_Data_Model GDM ON GPL.gis_data_model_id=GDM.gis_data_model_id
    INNER JOIN gis_data_model_type GDMT on GDMT.gis_data_model_type_id = GDM.gis_data_model_type_id
    WHERE GPL.party_cnt = @party_cnt AND GDMT.code = 'PARTY'

    If (@previous_data_model_id IS NULL)
	SET @previous_data_model_id = 0
    Else
	--Check if existing party type model is different
	IF EXISTS (SELECT GDM.gis_data_model_id	FROM Party P
		INNER JOIN Party_Type PT ON PT.party_type_id = P.party_type_id
		INNER JOIN GIS_Screen GS ON GS.gis_screen_id = PT.gis_screen_id
		LEFT JOIN GIS_Data_Model GDM ON GDM.gis_data_model_id = GS.gis_data_model_id
		WHERE P.party_cnt = @party_cnt And GDM.gis_data_model_id = @previous_data_model_id) 
	    Begin
		SET @previous_data_model_id = 0
	    End

END

GO


