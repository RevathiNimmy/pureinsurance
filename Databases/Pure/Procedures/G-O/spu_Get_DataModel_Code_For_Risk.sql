EXECUTE DDLDropProcedure 'spu_Get_DataModel_Code_For_Risk'
GO
CREATE PROCEDURE [dbo].[spu_Get_DataModel_Code_For_Risk] @OldInsurance_File_Cnt INT,
@NewRisk_Type_Id INT,
@Return_Value INT OUTPUT
AS
BEGIN
  DECLARE @OldDataModel_Code VARCHAR(30),
          @NewDataModel_Code VARCHAR(30);

  SELECT
    @NewDataModel_Code = gis_data.code
  FROM gis_data_model gis_data
  INNER JOIN gis_screen gis_screen
    ON gis_data.gis_data_model_id = gis_screen.gis_data_model_id
  INNER JOIN risk_type risk_type
    ON gis_screen.gis_screen_id = risk_type.gis_screen_id
  WHERE risk_type.risk_type_id = @NewRisk_Type_Id;

  SELECT
    risk_cnt,
    RowNumber INTO #risk_cnt_table
  FROM (SELECT
    risk_cnt,
    ROW_NUMBER() OVER (ORDER BY risk_cnt) AS RowNumber
  FROM insurance_file_risk_link
  WHERE insurance_file_cnt = @OldInsurance_File_Cnt) AS Risk_Table;

  WHILE (EXISTS (SELECT TOP 1
      RowNumber
    FROM #risk_cnt_table)
    )
  BEGIN
    DECLARE @number int
    SET @number = (SELECT TOP 1
      RowNumber
    FROM #risk_cnt_table);
    SELECT
      @OldDataModel_Code = gis_data.code
    FROM gis_data_model gis_data
    INNER JOIN gis_screen gis_screen
      ON gis_data.gis_data_model_id = gis_screen.gis_data_model_id
    INNER JOIN risk_type risk_type
      ON gis_screen.gis_screen_id = risk_type.gis_screen_id
    INNER JOIN risk risk
      ON risk_type.risk_type_id = risk.risk_type_id
    INNER JOIN #risk_cnt_table risk_cnt
      ON risk.risk_cnt = risk_cnt.risk_cnt
    WHERE risk_cnt.RowNumber = @number;
    IF (@NewDataModel_Code IS NULL
      OR @NewDataModel_Code = '')
    BEGIN
      SET @Return_Value = 0;
      BREAK
    END
    ELSE
    BEGIN
      IF @OldDataModel_Code <> @NewDataModel_Code
      BEGIN
        SET @Return_Value = 0;
        BREAK
      END
      ELSE
      BEGIN
        SET @Return_Value = 1;
      END
    END
    DELETE FROM #risk_cnt_table
    WHERE RowNumber = @number;
  END
  DROP TABLE #risk_cnt_table;
END
GO