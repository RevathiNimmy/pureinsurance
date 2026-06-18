-------------------------------------------------------------------------------
--  Author:    AMB
--  Date:      24 Sept 2003
--  Desc:      SFU 1.8.6 Deferred Reinsurance development
--             Change the RI_Model of a risk if a new one has come into effect
--  Called by: bSIRDeferredRIAuto
-------------------------------------------------------------------------------

SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Deferred_RI_Change_RI_Model'
GO

CREATE PROCEDURE spu_Deferred_RI_Change_RI_Model
(
	@insurance_file_cnt  int,
	@risk_cnt            int
)
AS 

DECLARE @dtCoverStartDate  datetime
DECLARE @dtRIModelEffDate  datetime
DECLARE @iCurrRIModel      int
DECLARE @iSearchRIModel    int


-- get current risk's ri_model, check 'is_deferred'
SELECT 
    @iCurrRIModel = ri_model_id
FROM
    Risk AS rsk
INNER JOIN
    Risk_Type_RI_Model_Usage AS rsku ON rsku.risk_type_id = rsk.risk_type_id
WHERE
    rsk.risk_cnt = @risk_cnt


IF ISNULL(@iCurrRIModel, 0) = 0 
BEGIN 
    RETURN 1        -- jump out, it's gone wrong already
END ELSE 
BEGIN

    -- get the current policy cover start date
    SELECT
        @dtCoverStartDate = cover_start_date
    FROM 
        Insurance_File 
    WHERE 
        insurance_file_cnt = @insurance_file_cnt
    
    -- search the risk_type's associated ri_models and find the one that
    -- has an effective date *earlier* than the current policy cover start date
    DECLARE curRIModels CURSOR FAST_FORWARD FOR
        SELECT 
            rtmru.ri_model_id, 
            rtmru.effective_date 
        FROM 
            risk_type_ri_model_usage AS rtmru
        INNER JOIN Risk AS rsk 
            ON rsk.risk_type_id = rtmru.risk_type_id
        INNER JOIN Risk_Type AS rskt 
            ON rskt.risk_type_id = rsk.risk_type_id
        WHERE 
            rsk.risk_cnt = @risk_cnt
        AND 
            rtmru.is_deleted = 0

    OPEN curRIModels
    FETCH NEXT FROM curRIModels INTO @iSearchRIModel, @dtRIModelEffDate

    WHILE @@FETCH_STATUS = 0 BEGIN
        IF ISNULL(@iSearchRIModel, 0) > 0 

            -- is it still the same? otherwise, do nothing
            IF @iSearchRIModel <> @iCurrRIModel

                -- check the effective date
                IF @dtRIModelEffDate <= @dtCoverStartDate

                    IF EXISTS 
                        (
                        SELECT risk_ri_arrangement_id FROM Risk_RI_Arrangement WHERE risk_cnt = @risk_cnt
                        )
                    BEGIN
                        -- if it's earlier, update the ri_model in the Risk_RI_Arrangement table
                        UPDATE
                            Risk_RI_Arrangement 
                        SET 
                            RI_Model_id = @iSearchRIModel 
                        WHERE 
                            risk_cnt = @risk_cnt

                    END ELSE
                    BEGIN
                        -- if it's earlier, update the ri_model in the Risk_RI_Arrangement table
                        INSERT INTO 
                            Risk_RI_Arrangement
                            (
                            risk_cnt,
                            ri_band,
                            ri_model_id,
                            original_flag
                            )
                        VALUES
                            (
                            @risk_cnt,
                            0,
                            @iSearchRIModel,
                            0
                            )   

                    END

        FETCH NEXT FROM curRIModels INTO @iSearchRIModel, @dtRIModelEffDate
    END

    CLOSE curRIModels
    DEALLOCATE curRIModels

END