SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


Execute DDLDropProcedure 'spu_RI_Arrangement_upd'
GO


CREATE PROCEDURE spu_RI_Arrangement_upd
    @ri_arrangement_id int,
    @is_modified tinyint,
	 @ri_override_reason_id int =0    
AS

    -- PBI 35359: Only overwrite ri_override_reason_id when a non-zero value is explicitly
    -- supplied. When @ri_override_reason_id=0 (the default, used by RI2007ArrangementUpdate
    -- which does not pass this parameter), preserve the existing value so that the override
    -- reason saved at NB is not wiped on every MTA save.
    Update  ri_arrangement
    Set     is_modified = @is_modified,
            ri_override_reason_id = CASE
                WHEN ISNULL(@ri_override_reason_id, 0) = 0 THEN ri_override_reason_id
                ELSE @ri_override_reason_id
            END
	Where   ri_arrangement_id = @ri_arrangement_id


Go

