SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

DDLDropProcedure 'spu_set_earning_pattern_usage'

GO

CREATE PROCEDURE spu_set_earning_pattern_usage
                @rating_section_type_id INT,
                @earning_pattern_id     INT,
                @effective_date         DATETIME,
	            @userid INT = NULL,
	            @uniqueid VARCHAR(50) = NULL,
	            @screenhierarchy VARCHAR(100) = NULL
                
AS
  BEGIN
    IF NOT EXISTS (SELECT NULL
                   FROM   Earning_Pattern_Usage
                   WHERE  rating_section_type_id = @rating_section_type_id
                          AND (earning_pattern_id = @earning_pattern_id
                                OR earning_pattern_id = 2))
      BEGIN
        INSERT INTO Earning_Pattern_Usage
                   (rating_section_type_id,
                    earning_pattern_id,
                    effective_date,
                    is_deleted,
					UserId,
					uniqueid ,
					screenhierarchy )
        VALUES     (@rating_section_type_id,
                    @earning_pattern_id,
                    @effective_date,
                    0,
					@userid,
					@uniqueid ,
					@screenhierarchy)
      END
      
  END

GO
