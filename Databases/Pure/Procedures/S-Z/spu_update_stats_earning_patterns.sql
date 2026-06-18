SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure
   'spu_update_stats_earning_patterns'

GO

CREATE PROCEDURE spu_update_stats_earning_patterns
                @stats_folder_cnt INT
AS
  BEGIN
    SET NOCOUNT ON
    
    UPDATE stats_detail
    SET    stats_detail.earning_pattern_id = rs.earning_pattern_id
    FROM   stats_detail
           JOIN peril p
             ON p.peril_id = stats_detail.peril_id
                AND p.peril_type_id = stats_detail.peril_type_id
                AND p.risk_cnt = stats_detail.risk_id
           JOIN rating_section rs
             ON rs.rating_section_id = p.rating_section_id
                AND rs.risk_cnt = p.risk_cnt
    WHERE  stats_detail.stats_folder_cnt = @stats_folder_cnt
                
  END
  
GO