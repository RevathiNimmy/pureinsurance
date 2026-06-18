SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_add_stats_details_finance'
GO

CREATE PROCEDURE spu_add_stats_details_finance

        @insurance_file_cnt     int,
        @finance_charge         numeric(19,4)

AS
BEGIN


/* Declare variable for all columns in stats_details table */
DECLARE @stats_folder_cnt int
DECLARE @stats_detail_id int

SELECT  @stats_folder_cnt = stats_folder_cnt
FROM    stats_folder
WHERE   stats_folder.insurance_file_cnt = @insurance_file_cnt

--Can't use this, as we've removed some erroneous NET postings
--SELECT  @stats_detail_id = (count(*) + 1)
SELECT  @stats_detail_id = max(stats_detail_id) + 1
FROM    stats_detail
WHERE   stats_detail.stats_folder_cnt = @stats_folder_cnt


/* Insert the Stats Detail */
INSERT INTO Stats_Detail (stats_folder_cnt,
          stats_detail_id,
          stats_detail_type,
          lead_commission_value_home)

VALUES   (@stats_folder_cnt,
          @stats_detail_id,
          'SUG',
          @finance_charge)
END


GO


